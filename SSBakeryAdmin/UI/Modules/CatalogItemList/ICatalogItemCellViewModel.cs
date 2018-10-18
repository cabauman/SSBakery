using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public interface ICatalogItemCellViewModel
    {
        CatalogItem Model { get; }

        string Id { get; }

        string Name { get; }

        string Description { get; }

        string Price { get; }

        string ImageUrl { get; set; }
    }
}
