using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogCategoryPage : ContentPageBase<ICatalogCategoryViewModel>
    {
        public CatalogCategoryPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .OneWayBind(ViewModel, vm => vm.CatalogItems, v => v.CatalogItems.ItemsSource)
                        .DisposeWith(disposables);
                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.CatalogItems.SelectedItem)
                        .DisposeWith(disposables);

                    ViewModel
                        .LoadCatalogItems
                        .Execute()
                        .Subscribe();
                });
        }
    }
}
