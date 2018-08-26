using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogItemCell : ReactiveViewCell<ICatalogItemCellViewModel>
    {
        public CatalogItemCell()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Do(PopulateFromViewModel)
                .Subscribe();
        }

        /// <summary>
        /// Using the binding context changed instead of binding accordingly to this:
        /// https://github.com/luberda-molinet/FFImageLoading/wiki/Xamarin.Forms-Advanced
        /// https://developer.xamarin.com/guides/xamarin-forms/user-interface/listview/performance/#RecycleElement
        /// </summary>
        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();

        //    Image.Source = null;

        //    var item = BindingContext as CatalogItemCellViewModel;
        //    if(item == null)
        //    {
        //        return;
        //    }

        //    Image.Source = ImageSource.FromFile("Icon.png"); // item.ImageUrl;
        //}

        private void PopulateFromViewModel(ICatalogItemCellViewModel catalogItem)
        {
            NameLabel.Text = catalogItem.Name;
            DescriptionLabel.Text = catalogItem.Description;
            PriceLabel.Text = catalogItem.Price;
            Image.Source = ImageSource.FromFile("Icon.png"); // catalogItem.ImageUrl;
        }
    }
}
