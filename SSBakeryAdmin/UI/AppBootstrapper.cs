using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseAuth.DotNet;
using GameCtor.FirebaseStorage.DotNet;
using GameCtor.RxNavigation;
using GameCtor.RxNavigation.XamForms;
using GameCtor.XamarinAuth;
using LocalStorage.XamarinEssentials;
using ReactiveUI;
using Splat;
using SSBakery.Config;
using SSBakery.Helpers;
using SSBakery.Repositories;
using SSBakeryAdmin.UI.Modules;

namespace SSBakeryAdmin
{
    public class AppBootstrapper : ReactiveObject
    {
        private object _mainView;

        public AppBootstrapper()
        {
            RegisterServices();
            RegisterViews();
            RegisterViewModels();
        }

        public IView NavigationShell { get; private set; }

        public IFirebaseAuthService FirebaseAuthService { get; private set; }

        public object MainView
        {
            get => _mainView;
            set => this.RaiseAndSetIfChanged(ref _mainView, value);
        }

        public async Task NavigateToFirstPage()
        {
            bool isAuthenticated = await FirebaseAuthService.IsAuthenticated;
            if(isAuthenticated)
            {
                MainView = new MainViewModel(this);
            }
            else
            {
                MainView = new SignInViewModel(this);
            }
        }

        private void RegisterServices()
        {
            NavigationShell = new MainView(RxApp.TaskpoolScheduler, RxApp.MainThreadScheduler, ViewLocator.Current);
            var viewStackService = new ViewStackService(NavigationShell);
            Locator.CurrentMutable.RegisterConstant(viewStackService, typeof(IViewStackService));
            Locator.CurrentMutable.Register(() => new RewardsMemberSynchronizer(), typeof(IRewardsMemberSynchronizer));
            Locator.CurrentMutable.Register(() => new CatalogSynchronizer(), typeof(ICatalogSynchronizer));

            Square.Connect.Client.Configuration.Default.AccessToken = ApiKeys.SQUARE_CONNECT;

            Locator.CurrentMutable.Register(() => new AuthService(), typeof(IAuthService));
            FirebaseAuthService = new FirebaseAuthService(ApiKeys.FIREBASE, new LocalStorageService());
            Locator.CurrentMutable.RegisterConstant(FirebaseAuthService, typeof(IFirebaseAuthService));
            var repositoryRegistrar = new RepositoryRegistrar(FirebaseAuthService, Locator.CurrentMutable);

            var firebaseStorageOptions = new Firebase.Storage.FirebaseStorageOptions()
            {
                AuthTokenAsyncFactory = () => FirebaseAuthService.GetIdTokenAsync()
            };
            var firebaseStorage = new Firebase.Storage.FirebaseStorage("ss-bakery.appspot.com", firebaseStorageOptions);
            Locator.CurrentMutable.Register(() => new FirebaseStorageService(firebaseStorage), typeof(IFirebaseStorageService));
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new MasterCell(), typeof(IViewFor<MasterCellViewModel>));

            Locator.CurrentMutable.Register(() => new RewardsMemberDirectoryPage(), typeof(IViewFor<IRewardsMemberDirectoryViewModel>));
            Locator.CurrentMutable.Register(() => new RewardsMemberCell(), typeof(IViewFor<IRewardsMemberCellViewModel>));
            Locator.CurrentMutable.Register(() => new StampCardPage(), typeof(IViewFor<IStampCardViewModel>));
            Locator.CurrentMutable.Register(() => new StampCell(), typeof(IViewFor<IStampCellViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryListPage(), typeof(IViewFor<ICatalogCategoryListViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryCell(), typeof(IViewFor<ICatalogCategoryCellViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemListPage(), typeof(IViewFor<ICatalogItemListViewModel>));
            Locator.CurrentMutable.Register(() => new MainPage(NavigationShell), typeof(IViewFor<IMainViewModel>));
            Locator.CurrentMutable.Register(() => new SignInPage(), typeof(IViewFor<ISignInViewModel>));
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which IPageViewModel we want to instantiate.
            Locator.CurrentMutable.Register(() => new RewardsMemberDirectoryViewModel(), typeof(IPageViewModel), typeof(RewardsMemberDirectoryViewModel).FullName);
            Locator.CurrentMutable.Register(() => new CatalogCategoryListViewModel(), typeof(IPageViewModel), typeof(CatalogCategoryListViewModel).FullName);
        }
    }
}
