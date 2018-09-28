using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.CustomerDirectory
{
    public interface ICustomerDirectorySearchResultsViewModel
    {
        ReactiveCommand<ICustomerCellViewModel, Unit> NavigateToCustomer { get; }

        IReadOnlyList<ICustomerCellViewModel> Customers { get; }
    }
}