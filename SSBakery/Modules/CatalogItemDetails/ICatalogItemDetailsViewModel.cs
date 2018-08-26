using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface ICatalogItemDetailsViewModel
    {
        string Id { get; }

        string Name { get; }

        string Description { get; }

        string Price { get; }

        string ImageUrl { get; }
    }
}
