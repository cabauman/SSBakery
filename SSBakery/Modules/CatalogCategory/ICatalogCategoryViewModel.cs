using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.UI.Modules
{
    public interface ICatalogCategoryViewModel
    {
        IList<ICatalogItemCellViewModel> CatalogItems { get; }

        ReactiveCommand<Unit, ICatalogItemCellViewModel> LoadCatalogItems { get; }

        IRepoContainer RepoContainer { get; }

        ICatalogItemCellViewModel SelectedItem { get; set; }

        //ICatalogItemCellViewModel ItemAppearing { get; set; }
    }
}
