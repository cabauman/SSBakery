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
    public partial class CatalogCategoryListPage : ContentPageBase<ICatalogCategoryListViewModel>
    {
        public CatalogCategoryListPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .OneWayBind(ViewModel, vm => vm.CatalogCategories, v => v.CatalogCategoryListView.ItemsSource)
                        .DisposeWith(disposables);
                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.CatalogCategoryListView.SelectedItem)
                        .DisposeWith(disposables);
                    this
                        .WhenAnyValue(x => x.ViewModel)
                        .Where(x => x != null)
                        .Select(x => Unit.Default)
                        .InvokeCommand(this, x => x.ViewModel.LoadCatalogCategories)
                        .DisposeWith(disposables);
                    CatalogCategoryListView
                        .Events()
                        .ItemAppearing
                        .Select(e => e.Item as ICatalogCategoryCellViewModel)
                        .BindTo(this, x => x.ViewModel.ItemAppearing)
                        .DisposeWith(disposables);
                });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}