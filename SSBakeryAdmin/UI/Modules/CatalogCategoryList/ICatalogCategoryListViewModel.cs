using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.UI.Modules
{
    public interface ICatalogCategoryListViewModel
    {
        ReactiveCommand<Unit, Unit> LoadCategories { get; }

        ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        ReactiveCommand<ICatalogCategoryCellViewModel, Unit> NavigateToCategory { get; }

        ReadOnlyObservableCollection<ICatalogCategoryCellViewModel> CategoryCells { get; }
    }
}
