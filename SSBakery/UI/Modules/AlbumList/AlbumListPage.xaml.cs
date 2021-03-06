﻿using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FFImageLoading;
using ReactiveUI;
using SSBakery.Services;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumListPage : ContentPageBase<IAlbumListViewModel>
    {
        public AlbumListPage()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.LoadAlbums);

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .OneWayBind(ViewModel, vm => vm.Albums, v => v.AlbumListView.ItemsSource)
                        .DisposeWith(disposables);
                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.AlbumListView.SelectedItem)
                        .DisposeWith(disposables);
                });
        }
    }
}
