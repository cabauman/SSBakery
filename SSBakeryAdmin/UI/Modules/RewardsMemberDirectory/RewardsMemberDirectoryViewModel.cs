using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Binding;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Helpers;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakeryAdmin.UI.Common;

namespace SSBakeryAdmin.UI.Modules
{
    public class RewardsMemberDirectoryViewModel : ViewModelBase, IPageViewModel, IRewardsMemberDirectoryViewModel
    {
        private const int MIN_SEARCH_LENGTH = 4;

        private ISourceCache<RewardsMember, string> _rewardsMemberCache;
        private ReadOnlyObservableCollection<IRewardsMemberCellViewModel> _rewardsMemberCells;
        private string _searchText;

        public RewardsMemberDirectoryViewModel(
            IRepository<RewardsMember> rewardsMemberRepo = null,
            IRewardsMemberSynchronizer rewardsMemberSynchronizer = null,
            IViewStackService viewStackService = null)
        {
            rewardsMemberRepo = rewardsMemberRepo ?? Locator.Current.GetService<IRepository<RewardsMember>>();
            rewardsMemberSynchronizer = rewardsMemberSynchronizer ?? Locator.Current.GetService<IRewardsMemberSynchronizer>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            _rewardsMemberCache = new SourceCache<RewardsMember, string>(x => x.Id);

            LoadRewardsMembers = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return Observable.Return(Unit.Default);

                    //return rewardsMemberRepo
                    //    .GetItems()
                    //    .Do(x => _rewardsMemberCache.AddOrUpdate(x))
                    //    .Select(_ => Unit.Default);
                });

            SyncWithPosSystem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return rewardsMemberSynchronizer
                        .PullFromPosSystemAndStoreInFirebase(_rewardsMemberCache);
                });

            SyncWithPosSystem.ThrownExceptions
                .Subscribe(
                    x =>
                    {
                        Console.WriteLine(x.Message);
                    });

            LoadRewardsMembers.InvokeCommand(this, x => x.SyncWithPosSystem);

            var dynamicFilter = this.WhenValueChanged(@this => @this.SearchText)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                .Select(CreatePredicate);

            _rewardsMemberCache
                .Connect()
                //.Filter(dynamicFilter)
                .Transform(x => new RewardsMemberCellViewModel(x) as IRewardsMemberCellViewModel)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _rewardsMemberCells)
                .DisposeMany()
                .Subscribe();

            NavigateToRewardsMember = ReactiveCommand.CreateFromObservable<IRewardsMemberCellViewModel, Unit>(
                customerCell =>
                {
                    return viewStackService.PushPage(new RewardsMemberViewModel());
                });

            //WhenRewardsMembersModified = rewardsMemberRepo
            //    .Observe()
            //    .Publish();

            //this.WhenActivated(
            //    disposables =>
            //    {
            //        WhenRewardsMembersModified
            //            .Where(x => x.EventSource == FirebaseEventSource.Online)
            //            .Subscribe(
            //                x =>
            //                {
            //                    if(x.EventType == FirebaseEventType.AddOrUpdate)
            //                    {
            //                        _rewardsMemberCache.AddOrUpdate(x.Object);
            //                    }
            //                    else
            //                    {
            //                        _rewardsMemberCache.RemoveKey(x.Key);
            //                    }
            //                })
            //            .DisposeWith(disposables);

            //        WhenRewardsMembersModified.Connect();
            //    });
        }

        public IConnectableObservable<FirebaseEvent<RewardsMember>> WhenRewardsMembersModified { get; }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public ReactiveCommand<Unit, Unit> LoadRewardsMembers { get; }

        public ReactiveCommand<IRewardsMemberCellViewModel, Unit> NavigateToRewardsMember { get; }

        public ReadOnlyObservableCollection<IRewardsMemberCellViewModel> MemberCells => _rewardsMemberCells;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        public string Title => "Rewards Members";

        private Func<RewardsMember, bool> CreatePredicate(string text)
        {
            if(text == null || text.Length < MIN_SEARCH_LENGTH)
            {
                return customer => false;
            }

            return customer => string.Concat(customer.PhoneNumber.Where(char.IsDigit))?.IndexOf(text) >= 0;
        }
    }
}
