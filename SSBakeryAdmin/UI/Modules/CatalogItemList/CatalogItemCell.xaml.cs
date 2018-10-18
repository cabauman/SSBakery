using System;
using System.Reactive.Disposables;
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

            this.WhenActivated(
                disposables =>
                {
                    this
                        .Bind(ViewModel, vm => vm.VisibleToUsers, v => v.VisibleToUsersSwitch.IsToggled)
                        .DisposeWith(disposables);
                });
        }

        private void PopulateFromViewModel(ICatalogItemCellViewModel viewModel)
        {
            NameLabel.Text = viewModel.Name;
            PriceLabel.Text = viewModel.Price;
            Image.Source = viewModel.ImageUrl;
        }
    }
}
