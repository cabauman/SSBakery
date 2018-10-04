using ReactiveUI;
using SSBakery.Models;

namespace SSBakery.UI.Modules
{
    public interface ICatalogItemCellViewModel
    {
        string Id { get; }

        string Name { get; }

        string Description { get; }

        string Price { get; }

        string ImageUrl { get; }

        CatalogItem CatalogItem { get; }
    }
}
