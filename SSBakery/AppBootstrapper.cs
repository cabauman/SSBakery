using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using SSBakery.Config;
using SSBakery.Repositories;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Modules;
using Xamarin.Forms;

namespace SSBakery
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper()
        {
            Router = new RoutingState();

            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
            Locator.CurrentMutable.Register(() => new SignInPage(), typeof(IViewFor<ISignInViewModel>));
            Locator.CurrentMutable.Register(() => new PhoneNumberVerificationPage(), typeof(IViewFor<IPhoneNumberVerificationViewModel>));
            Locator.CurrentMutable.Register(() => new PhoneAuthenticationPage(), typeof(IViewFor<IPhoneAuthenticationViewModel>));
            Locator.CurrentMutable.Register(() => new MainPage(), typeof(IViewFor<MainViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogPage(), typeof(IViewFor<CatalogViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemDetailsPage(), typeof(IViewFor<CatalogItemDetailsViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemCell(), typeof(IViewFor<CatalogItemCellViewModel>));
            Locator.CurrentMutable.Register(() => new StoreInfoPage(), typeof(IViewFor<StoreInfoViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumListPage(), typeof(IViewFor<IAlbumListViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumCell(), typeof(IViewFor<AlbumCellViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumPage(), typeof(IViewFor<AlbumViewModel>));
            Locator.CurrentMutable.Register(() => new RewardsPage(), typeof(IViewFor<RewardsViewModel>));
            Locator.CurrentMutable.Register(() => new RewardsProgramActivationPage(), typeof(IViewFor<IRewardsProgramActivationViewModel>));

            Locator.CurrentMutable.RegisterConstant(new XamarinAuthCredentialsService(), typeof(ICredentialsService));
            var firebaseAuthService = new FirebaseAuthService();
            Locator.CurrentMutable.RegisterConstant(firebaseAuthService, typeof(IFirebaseAuthService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new RepoContainer(), typeof(IRepoContainer));
            Locator.CurrentMutable.RegisterLazySingleton(() => new FacebookPhotoService(), typeof(IFacebookPhotoService));

            Square.Connect.Client.Configuration.Default.AccessToken = ApiKeys.SQUARE_CONNECT;

            GoToPage(new MainViewModel());
            return;

            if(firebaseAuthService.IsAuthenticated)
            {
                GoToPage(new MainViewModel());
            }
            else
            {
                GoToPage(new SignInViewModel());
            }
        }

        public RoutingState Router { get; }

        public Page CreateMainPage()
        {
            return new RoutedViewHost();
        }

        private void GoToPage(IRoutableViewModel routableViewModel)
        {
            Router
                .NavigateAndReset
                .Execute(routableViewModel)
                .Subscribe();
        }
    }
}
