using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IMainViewModel
    {
        ReactiveCommand NavigateToCatalogPage { get; }

        ReactiveCommand NavigateToAlbumPage { get; }

        ReactiveCommand NavigateToRewardsPage { get; }

        ReactiveCommand NavigateToStoreInfoPage { get; }
    }
}
