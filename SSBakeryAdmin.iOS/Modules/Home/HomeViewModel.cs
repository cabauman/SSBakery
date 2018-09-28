using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Xml;
using ReactiveUI;
using GameCtor.RxNavigation;
using Splat;
using SSBakeryAdmin.iOS.Modules.CatalogCategoryList;
using SSBakeryAdmin.iOS.Modules.CustomerDirectory;
using SSBakery.Repositories.Interfaces;

namespace SSBakeryAdmin.iOS.Modules.Home
{
    public class HomeViewModel : IHomeViewModel, IPageViewModel
    {
        public HomeViewModel(IViewStackService viewStackService = null)
        {
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            NavigateToCatalogCategoryListPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return viewStackService.PushPage(new CatalogCategoryListViewModel());
                });

            NavigateToCatalogCategoryListPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return viewStackService.PushPage(new CustomerDirectoryViewModel());
                });
        }

        public ReactiveCommand<Unit, Unit> NavigateToCatalogCategoryListPage { get; }

        public ReactiveCommand<Unit, Unit> NavigateToCustomerDirectoryPage { get; }

        public string Title => "Home";
    }
}