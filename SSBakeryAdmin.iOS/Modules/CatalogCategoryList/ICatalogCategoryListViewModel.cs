using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.CatalogCategoryList
{
    public interface ICatalogCategoryListViewModel
    {
        ReactiveCommand<Unit, IReadOnlyList<ICatalogCategoryCellViewModel>> LoadCategories { get; }

        ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        ReactiveCommand<ICatalogCategoryCellViewModel, Unit> NavigateToCategory { get; }

        IReadOnlyList<ICatalogCategoryCellViewModel> Categories { get; }
    }
}