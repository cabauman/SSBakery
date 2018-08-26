using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.Home
{
    public interface IHomeViewModel
    {
        ReactiveCommand<Unit, Unit> NavigateToCatalogCategoryListPage { get; }

        ReactiveCommand<Unit, Unit> NavigateToCustomerDirectoryPage { get; }
    }
}