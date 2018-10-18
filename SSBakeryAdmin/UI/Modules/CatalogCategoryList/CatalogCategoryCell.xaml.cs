using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakeryAdmin.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogCategoryCell : ReactiveContentView<ICatalogCategoryCellViewModel>
    {
        public CatalogCategoryCell()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(PopulateFromViewModel)
                .Subscribe();
        }

        private void PopulateFromViewModel(ICatalogCategoryCellViewModel viewModel)
        {
            NameLabel.Text = viewModel.Name;
            Image.Source = viewModel.ImageUrl;
        }
    }
}
