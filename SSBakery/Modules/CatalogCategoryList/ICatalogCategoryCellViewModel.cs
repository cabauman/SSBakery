
using SSBakery.Models;

namespace SSBakery.UI.Modules
{
    public interface ICatalogCategoryCellViewModel
    {
        string Id { get; }

        string Name { get; }

        CatalogCategory Model { get; }
    }
}
