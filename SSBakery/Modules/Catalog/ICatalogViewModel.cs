using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.UI.Modules
{
    public interface ICatalogViewModel
    {
        IEnumerable<ICatalogItemCellViewModel> CatalogItems { get; }

        ReactiveCommand<Unit, IEnumerable<ICatalogItemCellViewModel>> LoadCatalogItems { get; }

        IRepoContainer RepoContainer { get; }

        ICatalogItemCellViewModel SelectedItem { get; set; }
    }
}
