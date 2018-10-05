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
    public partial class CustomerCell : ReactiveViewCell<ICustomerCellViewModel>
    {
        public CustomerCell()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this.WhenAnyValue(x => x.ViewModel)
                        .Where(x => x != null)
                        .Do(PopulateFromViewModel)
                        .Subscribe()
                        .DisposeWith(disposables);
                });
        }

        private void PopulateFromViewModel(ICustomerCellViewModel catalogItem)
        {
            //NameLabel.Text = catalogItem.Name;
        }
    }
}
