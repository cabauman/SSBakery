using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Xml;
using ReactiveUI;
using GameCtor.RxNavigation;
using Splat;
using SSBakery.Repositories.Interfaces;
using SSBakeryAdmin.iOS.Modules.Customer;

namespace SSBakeryAdmin.iOS.Modules.CustomerDirectory
{
    public class CustomerDirectoryViewModel : ICustomerDirectoryViewModel, IPageViewModel
    {
        private string _timestampOfLatestSync;

        public CustomerDirectoryViewModel(ICustomerRepo customerRepo = null, IViewStackService viewStackService = null)
        {
            customerRepo = customerRepo ?? Locator.Current.GetService<ICustomerRepo>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            SyncWithPosSystem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return customerRepo
                        .PullFromPosSystemAndStoreInFirebase(_timestampOfLatestSync)
                        .Do(_ => SaveTimestampOfLatestSync());
                });

            NavigateToCustomer = ReactiveCommand.CreateFromObservable<ICustomerCellViewModel, Unit>(
                customerCell =>
                {
                    return viewStackService.PushPage(new CustomerViewModel(null));
                });
        }

        public ReactiveCommand<Unit, IReadOnlyList<ICustomerCellViewModel>> LoadCustomers { get; }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public ReactiveCommand<ICustomerCellViewModel, Unit> NavigateToCustomer { get; }

        public IReadOnlyList<ICustomerCellViewModel> Customers { get; }

        public string Title => "Customer Directory";

        private void SaveTimestampOfLatestSync()
        {
            _timestampOfLatestSync = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
        }
    }
}