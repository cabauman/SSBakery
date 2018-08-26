using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.CustomerDirectory
{
    public interface ICustomerDirectoryViewModel
    {
        ReactiveCommand<Unit, IReadOnlyList<ICustomerCellViewModel>> LoadCustomers { get; }

        ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        ReactiveCommand<ICustomerCellViewModel, Unit> NavigateToCustomer { get; }

        IReadOnlyList<ICustomerCellViewModel> Customers { get; }
    }
}