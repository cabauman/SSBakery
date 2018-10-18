using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakeryAdmin.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogItemCell : ReactiveContentView<ICatalogItemCellViewModel>
    {
        public CatalogItemCell()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(PopulateFromViewModel)
                .Subscribe();
        }

        private void PopulateFromViewModel(ICatalogItemCellViewModel viewModel)
        {
            NameLabel.Text = viewModel.Name;
            PriceLabel.Text = viewModel.Price;
            Image.Source = viewModel.ImageUrl;
        }
    }
}
