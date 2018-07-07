using System;
using System.Reactive;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private IFirebaseAuthService _authService;

        public MainViewModel(IFirebaseAuthService authService = null, IScreen hostScreen = null)
            : base(hostScreen)
        {
            _authService = authService ?? Locator.Current.GetService<IFirebaseAuthService>();

            NavigateToCatalogPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return Navigate(new CatalogViewModel());
                });
            NavigateToAlbumPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return Navigate(new AlbumListViewModel());
                });
            NavigateToStoreInfoPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return Navigate(new StoreInfoViewModel());
                });

            if(_authService.IsPhoneNumberLinkedToAccount)
            {
                NavigateToRewardsPage = ReactiveCommand.CreateFromObservable(
                    () =>
                    {
                        return Navigate(new RewardsViewModel());
                    });
            }
            else
            {
                NavigateToRewardsPage = ReactiveCommand.CreateFromObservable(
                    () =>
                    {
                        return Navigate(new RewardsProgramActivationViewModel());
                    });
            }
        }

        public ReactiveCommand NavigateToCatalogPage { get; }

        public ReactiveCommand NavigateToAlbumPage { get; }

        public ReactiveCommand NavigateToRewardsPage { get; }

        public ReactiveCommand NavigateToStoreInfoPage { get; }
    }
}
