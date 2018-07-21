using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;
using Square.Connect.Model;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.UI.Modules
{
    public interface ICatalogViewModel
    {
        ReactiveList<CatalogItemCellViewModel> CatalogItems { get; }

        ReactiveCommand<Unit, IEnumerable<CatalogObject>> LoadCatalogObjects { get; }

        IRepoContainer RepoContainer { get; }

        ICatalogItemCellViewModel SelectedItem { get; set; }
    }
}
