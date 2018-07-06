using System;
using System.Reactive;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class MainViewModel : ViewModelBase
    {
        private IFirebaseAuthService _authService;

        public MainViewModel(IFirebaseAuthService authService = null, IScreen hostScreen = null)
            : base(hostScreen)
        {
            _authService = authService ?? Locator.Current.GetService<IFirebaseAuthService>();

            GoToCatalogPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return GoToPage(new CatalogViewModel());
                });
            GoToAlbumPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return GoToPage(new AlbumListViewModel());
                });

            if(_authService.IsPhoneNumberLinkedToAccount)
            {
                GoToRewardsPage = ReactiveCommand.CreateFromObservable(
                    () =>
                    {
                        return GoToPage(new RewardsViewModel());
                    });
            }
            else
            {
                GoToRewardsPage = ReactiveCommand.CreateFromObservable(
                    () =>
                    {
                        return GoToPage(new RewardsProgramActivationViewModel());
                    });
            }

            GoToStoreInfoPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return GoToPage(new StoreInfoViewModel());
                });

            //IUserWrapper user = CrossFirebaseAuth.Current.CurrentUser;
            //if(user != null)
            //{
            //    user.GetIdTokenAsync(false)
            //        .ToObservable()
            //        .Subscribe(
            //            token =>
            //            {
            //                Console.WriteLine(token);
            //            },
            //            ex =>
            //            {
            //                Console.WriteLine(ex.Message);
            //            });
            //}
        }

        public ReactiveCommand<Unit, IRoutableViewModel> GoToCatalogPage { get; }

        public ReactiveCommand GoToAlbumPage { get; }

        public ReactiveCommand GoToRewardsPage { get; }

        public ReactiveCommand GoToStoreInfoPage { get; }

        public IObservable<IRoutableViewModel> GoToPage(IRoutableViewModel routableViewModel)
        {
            return HostScreen
                .Router
                .Navigate
                .Execute(routableViewModel);
        }
    }
}
