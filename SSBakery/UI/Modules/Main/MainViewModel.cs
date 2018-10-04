using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class MainViewModel : PageViewModel, IMainViewModel
    {
        private IFirebaseAuthService _authService;

        public MainViewModel(IFirebaseAuthService authService = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _authService = authService ?? Locator.Current.GetService<IFirebaseAuthService>();

            NavigateToCatalogPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ViewStackService.PushPage(new CatalogCategoryListViewModel());
                });
            NavigateToAlbumPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ViewStackService.PushPage(new AlbumListViewModel());
                });
            NavigateToStoreInfoPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ViewStackService.PushPage(new StoreInfoViewModel());
                });

            if(_authService.CurrentUser.Providers.Contains("phone"))
            {
                NavigateToRewardsPage = ReactiveCommand.CreateFromObservable(
                    () =>
                    {
                        return ViewStackService.PushPage(new RewardsViewModel());
                    });
            }
            else
            {
                NavigateToRewardsPage = ReactiveCommand.CreateFromObservable(
                    () =>
                    {
                        return ViewStackService.PushPage(new RewardsProgramActivationViewModel());
                    });
            }

            this.WhenActivated(
                d =>
                {
                    Disposable.Empty.DisposeWith(d);
                });

            NavigateToCatalogPage.ThrownExceptions.Subscribe(
                ex =>
                {
                    System.Console.WriteLine(ex.Message);
                });
        }

        public ReactiveCommand NavigateToCatalogPage { get; }

        public ReactiveCommand NavigateToAlbumPage { get; }

        public ReactiveCommand NavigateToRewardsPage { get; }

        public ReactiveCommand NavigateToStoreInfoPage { get; }
    }
}
