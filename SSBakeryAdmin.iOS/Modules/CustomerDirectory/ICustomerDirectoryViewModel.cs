using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.CustomerDirectory
{
    public interface ICustomerDirectoryViewModel
    {
        IReadOnlyList<ICustomerCellViewModel> Customers { get; }

        ReactiveCommand<Unit, IReadOnlyList<ICustomerCellViewModel>> LoadCustomers { get; }

        ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        ReactiveCommand<ICustomerCellViewModel, Unit> NavigateToCustomer { get; }

        void AddUnclaimedReward(int index);

        void RemoveUnclaimedReward(int index);
    }
}