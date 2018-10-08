using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Xml;
using DynamicData;
using DynamicData.Binding;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Helpers;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakeryAdmin.UI.Modules
{
    public class RewardsMemberDirectoryViewModel : ReactiveObject, IPageViewModel, IRewardsMemberDirectoryViewModel
    {
        private const int MIN_SEARCH_LENGTH = 4;
        private readonly ObjectPool<IRewardsMemberCellViewModel> _customerCellPool;

        private IRewardsMemberCellViewModel _cellDisappearing;
        private ISourceList<RewardsMember> _customerSource;
        private ReadOnlyObservableCollection<IRewardsMemberCellViewModel> _customerCells;
        private string _timestampOfLatestSync;
        private string _searchText;

        public RewardsMemberDirectoryViewModel(ICustomerRepo customerRepo = null, IViewStackService viewStackService = null)
        {
            customerRepo = customerRepo ?? Locator.Current.GetService<ICustomerRepo>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            _customerCellPool = new ObjectPool<IRewardsMemberCellViewModel>(() => new RewardsMemberCellViewModel());

            SyncWithPosSystem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return customerRepo
                        .PullFromPosSystemAndStoreInFirebase(_timestampOfLatestSync)
                        .Do(_ => SaveTimestampOfLatestSync());
                });

            NavigateToCustomer = ReactiveCommand.CreateFromObservable<IRewardsMemberCellViewModel, Unit>(
                customerCell =>
                {
                    return viewStackService.PushPage(new RewardsMemberViewModel());
                });

            this
                .WhenAnyValue(x => x.CellDisappearing)
                .Do(x => _customerCellPool.PutObject(x))
                .Subscribe();

            var dynamicFilter = this.WhenValueChanged(@this => @this._searchText)
                .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                .Select(CreatePredicate);

            _customerSource
                .Connect()
                .Filter(dynamicFilter)
                .Transform(
                    x =>
                    {
                        var cell = _customerCellPool.GetObject();
                        cell.RewardData = x;
                        return cell;
                    })
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _customerCells)
                .Subscribe();
        }

        public ReactiveCommand<Unit, IReadOnlyList<RewardsMember>> LoadCustomers { get; }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public ReactiveCommand<IRewardsMemberCellViewModel, Unit> NavigateToCustomer { get; }

        public ReadOnlyObservableCollection<IRewardsMemberCellViewModel> CustomersCells => _customerCells;

        public IRewardsMemberCellViewModel CellDisappearing
        {
            get { return _cellDisappearing; }
            set { this.RaiseAndSetIfChanged(ref _cellDisappearing, value); }
        }

        public string Title => "Customer Directory";

        private void SaveTimestampOfLatestSync()
        {
            _timestampOfLatestSync = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
        }

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
