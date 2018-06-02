﻿using System;
using System.Collections.Generic;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using SSBakery.Config;
using SSBakery.Core.Services;
using SSBakery.Repositories;
using SSBakery.Repositories.Interfaces;
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
            Locator.CurrentMutable.Register(() => new MainPage(), typeof(IViewFor<MainViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogPage(), typeof(IViewFor<CatalogViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemDetailsPage(), typeof(IViewFor<CatalogItemDetailsViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogItemCell(), typeof(IViewFor<CatalogItemCellViewModel>));
            Locator.CurrentMutable.Register(() => new StoreInfoPage(), typeof(IViewFor<StoreInfoViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumPage(), typeof(IViewFor<IAlbumViewModel>));
            Locator.CurrentMutable.Register(() => new AlbumCell(), typeof(IViewFor<AlbumCellViewModel>));
            Locator.CurrentMutable.Register(() => new GalleryPage(), typeof(IViewFor<GalleryViewModel>));
            //Locator.CurrentMutable.Register(() => new PhotoCell(), typeof(IViewFor<PhotoCellViewModel>));

            Locator.CurrentMutable.RegisterLazySingleton(() => new DataStore(), typeof(IDataStore));
            Locator.CurrentMutable.RegisterLazySingleton(() => new FacebookPhotoService(), typeof(IFacebookPhotoService));

            this
                .Router
                .NavigateAndReset
                .Execute(new AlbumViewModel())
                .Subscribe();

            Square.Connect.Client.Configuration.Default.AccessToken = ApiKeys.SQUARE_CONNECT;
        }

        public RoutingState Router { get; protected set; }

        public Page CreateMainPage()
        {
            return new RoutedViewHost();
        }
    }
}
