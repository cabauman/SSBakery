
using SSBakery.Models;

namespace SSBakery.UI.Modules
{
    public interface ICatalogCategoryCellViewModel
    {
        string Id { get; }

        string Name { get; }

        string Description { get; }

        CatalogCategory Model { get; }
    }
}
