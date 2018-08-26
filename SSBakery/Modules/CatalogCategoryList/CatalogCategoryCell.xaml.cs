﻿using System.Reactive.Disposables;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogCategoryCell : ReactiveViewCell<ICatalogCategoryCellViewModel>
    {
        public CatalogCategoryCell()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this.OneWayBind(ViewModel, vm => vm.Name, v => v.Name.Text)
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

            Image.Source = ImageSource.FromFile("Icon.png"); // item.ImageUrl;
        }
    }
}
