using System;
using System.Reactive.Threading.Tasks;
using GameCtor.RxNavigation;
using GameCtor.RxNavigation.XamForms;
using ReactiveUI;
using Splat;
using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Config;
using SSBakery.Helpers;
using SSBakery.Repositories;
using SSBakeryAdmin.UI.Modules;

namespace SSBakeryAdmin
{
    public class AppBootstrapper
    {
        public AppBootstrapper()
        {
            RegisterServices();
            RegisterViews();
            RegisterViewModels();
        }

        public IView NavigationShell { get; private set; }

        public MainViewModel CreateMainViewModel()
        {
            var viewModel = new MainViewModel();

            return viewModel;
        }

        private void RegisterServices()
        {
            NavigationShell = new MainView(RxApp.TaskpoolScheduler, RxApp.MainThreadScheduler, ViewLocator.Current);
            var viewStackService = new ViewStackService(NavigationShell);
            Locator.CurrentMutable.RegisterConstant(viewStackService, typeof(IViewStackService));
            Locator.CurrentMutable.Register(() => new RewardsMemberSynchronizer(), typeof(IRewardsMemberSynchronizer));
            Locator.CurrentMutable.Register(() => new CatalogSynchronizer(), typeof(ICatalogSynchronizer));
            var repositoryRegistrar = new RepositoryRegistrar(null, Locator.CurrentMutable);
            Square.Connect.Client.Configuration.Default.AccessToken = ApiKeys.SQUARE_CONNECT;

            //var customersApi = new CustomersApi();
            //var filter = new CustomerFilter();
            //var query = new CustomerQuery(filter);
            //var request = new SearchCustomersRequest(Cursor: null, Query: query);

            //customersApi
            //    .SearchCustomersAsync(request)
            //    .ToObservable()
            //    .Subscribe(
            //        x =>
            //        {
            //            Console.WriteLine(x);
            //        },
            //        ex =>
            //        {
            //            Console.WriteLine(ex.Message);
            //        });
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new MasterCell(), typeof(IViewFor<MasterCellViewModel>));

            // Detail pages
            Locator.CurrentMutable.Register(() => new RewardsMemberDirectoryPage(), typeof(IViewFor<IRewardsMemberDirectoryViewModel>));
            Locator.CurrentMutable.Register(() => new RewardsMemberCell(), typeof(IViewFor<IRewardsMemberCellViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryListPage(), typeof(IViewFor<ICatalogCategoryListViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemListPage(), typeof(IViewFor<ICatalogItemListViewModel>));
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which IPageViewModel we want to instantiate.
            Locator.CurrentMutable.Register(() => new RewardsMemberDirectoryViewModel(), typeof(IPageViewModel), typeof(RewardsMemberDirectoryViewModel).FullName);
            Locator.CurrentMutable.Register(() => new CatalogCategoryListViewModel(), typeof(IPageViewModel), typeof(CatalogCategoryListViewModel).FullName);
        }
    }
}
