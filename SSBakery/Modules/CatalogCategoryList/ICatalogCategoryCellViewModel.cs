using ReactiveUI;
using Square.Connect.Model;

namespace SSBakery.UI.Modules
{
    public interface ICatalogCategoryCellViewModel
    {
        string Id { get; }

        string Name { get; }

        CatalogObject CatalogObject { get; }
    }
}
