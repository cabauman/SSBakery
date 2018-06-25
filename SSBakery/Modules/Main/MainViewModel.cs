﻿using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Threading.Tasks;
using GameCtor.Firebase.AuthWrapper;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IScreen hostScreen = null)
            : base(hostScreen)
        {
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
            GoToRewardsPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return GoToPage(new RewardsViewModel());
                });
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
