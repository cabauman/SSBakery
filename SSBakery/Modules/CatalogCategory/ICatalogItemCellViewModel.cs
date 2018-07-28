using ReactiveUI;
using Square.Connect.Model;

namespace SSBakery.UI.Modules
{
    public interface ICatalogItemCellViewModel
    {
        string Id { get; }

        string Name { get; }

        string Description { get; }

        string Price { get; }

        CatalogObject CatalogObject { get; }
    }
}
