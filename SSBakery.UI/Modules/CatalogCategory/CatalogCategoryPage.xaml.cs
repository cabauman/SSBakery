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

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.LoadCatalogItems);

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .OneWayBind(ViewModel, vm => vm.CatalogItems, v => v.CatalogItemListView.ItemsSource)
                        .DisposeWith(disposables);
                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.CatalogItemListView.SelectedItem)
                        .DisposeWith(disposables);
                    //this
                    //    .CatalogItemListView
                    //    .Events()
                    //    .ItemAppearing
                    //    .Select(e => e.Item as ICatalogItemCellViewModel)
                    //    .BindTo(this, x => x.ViewModel.ItemAppearing)
                    //    .DisposeWith(disposables);
                });
        }
    }
}
