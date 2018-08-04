using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface ICatalogCategoryListViewModel
    {
        ReactiveCommand<Unit, IEnumerable<ICatalogCategoryCellViewModel>> LoadCatalogCategories { get; }

        IEnumerable<ICatalogCategoryCellViewModel> CatalogCategories { get; }

        ICatalogCategoryCellViewModel SelectedItem { get; set; }
    }
}