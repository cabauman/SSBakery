﻿using System;
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
using SSBakery.UI.Navigation;
using SSBakery.UI.Navigation.Interfaces;
using Xamarin.Forms;

namespace SSBakery
{
    public class AppBootstrapper : ReactiveObject
    {
        private readonly IView _mainView;

        public AppBootstrapper()
        {
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            _mainView = new MainView(RxApp.TaskpoolScheduler, RxApp.MainThreadScheduler, ViewLocator.Current);
            var viewStackService = new ViewStackService(_mainView);
            Locator.CurrentMutable.RegisterConstant(viewStackService, typeof(IViewStackService));
            Locator.CurrentMutable.Register(() => new SignInPage(), typeof(IViewFor<ISignInViewModel>));
            Locator.CurrentMutable.Register(() => new PhoneAuthPhoneNumberEntryPage(), typeof(IViewFor<IPhoneAuthPhoneNumberEntryViewModel>));
            Locator.CurrentMutable.Register(() => new PhoneAuthVerificationCodeEntryPage(), typeof(IViewFor<IPhoneAuthVerificationCodeEntryViewModel>));
            Locator.CurrentMutable.Register(() => new MainPage(), typeof(IViewFor<IMainViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryListPage(), typeof(IViewFor<ICatalogCategoryListViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryCell(), typeof(IViewFor<ICatalogCategoryCellViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryPage(), typeof(IViewFor<ICatalogCategoryViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemCell(), typeof(IViewFor<ICatalogItemCellViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemDetailsPage(), typeof(IViewFor<ICatalogItemDetailsViewModel>));
            Locator.CurrentMutable.Register(() => new StoreInfoPage(), typeof(IViewFor<IStoreInfoViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumListPage(), typeof(IViewFor<IAlbumListViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumCell(), typeof(IViewFor<IAlbumCellViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumPage(), typeof(IViewFor<IAlbumViewModel>));
            Locator.CurrentMutable.Register(() => new RewardsPage(), typeof(IViewFor<IRewardsViewModel>));
            Locator.CurrentMutable.Register(() => new RewardsProgramActivationPage(), typeof(IViewFor<IRewardsProgramActivationViewModel>));

            Locator.CurrentMutable.Register(() => new AuthService(), typeof(IAuthService));
            var firebaseAuthService = new FirebaseAuthService();
            Locator.CurrentMutable.RegisterConstant(firebaseAuthService, typeof(IFirebaseAuthService));
            Locator.CurrentMutable.RegisterLazySingleton(() => new RepoContainer(), typeof(IRepoContainer));
            Locator.CurrentMutable.RegisterLazySingleton(() => new FacebookPhotoService(), typeof(IFacebookPhotoService));

            Square.Connect.Client.Configuration.Default.AccessToken = ApiKeys.SQUARE_CONNECT;

            viewStackService.PushPage(new MainViewModel())
                .Subscribe();

            return;

            if(firebaseAuthService.IsAuthenticated)
            {
                viewStackService.PushPage(new MainViewModel());
            }
            else
            {
                viewStackService.PushPage(new SignInViewModel());
            }
        }

        public RoutingState Router { get; }

        public Page CreateMainPage()
        {
            return _mainView as NavigationPage;
        }
    }
}
