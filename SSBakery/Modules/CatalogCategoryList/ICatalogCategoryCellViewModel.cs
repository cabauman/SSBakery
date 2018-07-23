using ReactiveUI;
using Square.Connect.Model;

namespace SSBakery.UI.Modules
{
    public interface ICatalogCategoryCellViewModel
    {
        string Name { get; }

        string Description { get; }

        CatalogObject CatalogObject { get; }
    }
}
