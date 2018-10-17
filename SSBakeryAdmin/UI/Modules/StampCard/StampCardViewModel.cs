using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.Repository;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakeryAdmin.UI.Common;

namespace SSBakeryAdmin.UI.Modules
{
    public class StampCardViewModel : ViewModelBase, IPageViewModel, IStampCardViewModel
    {
        private const int MAX_STAMPS = 10;

        private readonly RewardsMember _rewardsMember;

        private int _rewardCount;
        private IStampCellViewModel _selectedItem;

        public StampCardViewModel(RewardsMember rewardsMember, IRepository<RewardsMember> rewardsMemberRepo)
        {
            rewardsMemberRepo = rewardsMemberRepo ?? Locator.Current.GetService<IRewardsMemberRepo>();

            _rewardsMember = rewardsMember;
            RewardCount = _rewardsMember.UnclaimedRewardCount;

            var stampCells = new List<IStampCellViewModel>(MAX_STAMPS);
            int numStamps = rewardsMember.TotalVisits % MAX_STAMPS;
            for(int i = 0; i < MAX_STAMPS; ++i)
            {
                bool stamped = i < numStamps;
                stampCells.Add(new StampCellViewModel(stamped));
            }

            StampCells = stampCells.AsReadOnly();

            Save = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    _rewardsMember.UnclaimedRewardCount = RewardCount;

                    return rewardsMemberRepo
                        .Upsert(_rewardsMember);
                });

            IncrementRewardCount = ReactiveCommand.Create(
                () =>
                {
                    ++RewardCount;
                });

            DecrementRewardCount = ReactiveCommand.Create(
                () =>
                {
                    --RewardCount;
                });

            this
                .WhenAnyValue(x => x.SelectedItem)
                .Where(x => x != null)
                .Do(x => x.Stamped = !x.Stamped)
                .Select(x => x.Stamped)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    stamped =>
                    {
                        SelectedItem = null;
                        _rewardsMember.TotalVisits += stamped ? 1 : -1;
                    });
        }

        public int RewardCount
        {
            get => _rewardCount;
            set => this.RaiseAndSetIfChanged(ref _rewardCount, value);
        }

        public IStampCellViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public IReadOnlyList<IStampCellViewModel> StampCells { get; }

        public ReactiveCommand<Unit, Unit> Save { get; }

        public ReactiveCommand<Unit, Unit> IncrementRewardCount { get; }

        public ReactiveCommand<Unit, Unit> DecrementRewardCount { get; }

        public ReactiveCommand<Unit, Unit> StartNewStampCard { get; }

        public string Title => "Stamp Card";
    }
}
