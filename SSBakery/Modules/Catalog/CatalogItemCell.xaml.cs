﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogItemCell : ReactiveViewCell<CatalogItemCellViewModel>
    {
        public CatalogItemCell()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this.OneWayBind(ViewModel, vm => vm.Name, v => v.Name.Text)
                        .DisposeWith(disposables);

                    this.OneWayBind(ViewModel, vm => vm.Description, v => v.Description.Text)
                        .DisposeWith(disposables);

                    this.OneWayBind(ViewModel, vm => vm.Price, v => v.Price.Text)
                        .DisposeWith(disposables);
                });
        }

        /// <summary>
        /// Using the binding context changed instead of binding accordingly to this:
        /// https://github.com/luberda-molinet/FFImageLoading/wiki/Xamarin.Forms-Advanced
        /// https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/performance/#RecycleElement
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            Image.Source = null;

            var item = BindingContext as CatalogItemCellViewModel;
            if(item == null)
            {
                return;
            }

            Image.Source = item.ImageUrl;
        }
    }
}