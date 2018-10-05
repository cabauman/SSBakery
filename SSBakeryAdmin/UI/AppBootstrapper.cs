using System;
using GameCtor.RxNavigation;
using GameCtor.RxNavigation.XamForms;
using ReactiveUI;
using Splat;
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
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new MasterCell(), typeof(IViewFor<MasterCellViewModel>));

            // Detail pages
            Locator.CurrentMutable.Register(() => new CustomerDirectoryPage(), typeof(IViewFor<ICustomerDirectoryViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryListPage(), typeof(IViewFor<ICatalogCategoryListViewModel>));
        }

        private void RegisterViewModels()
        {
            // Here, we use contracts to distinguish which IPageViewModel we want to instantiate.
            Locator.CurrentMutable.Register(() => new CustomerDirectoryViewModel(), typeof(IPageViewModel), typeof(CustomerDirectoryViewModel).FullName);
            Locator.CurrentMutable.Register(() => new CatalogCategoryListViewModel(), typeof(IPageViewModel), typeof(CatalogCategoryListViewModel).FullName);
        }
    }
}
