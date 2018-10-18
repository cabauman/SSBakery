using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using SSBakeryAdmin.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakeryAdmin.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogItemListPage : ContentPageBase<ICatalogItemListViewModel>
    {
        public CatalogItemListPage()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(_ => Unit.Default)
                .Take(1)
                .InvokeCommand(this, x => x.ViewModel.LoadItems);

            this
                .WhenAnyValue(x => x.ViewModel.Items)
                .Where(x => x != null)
                .Do(items => ItemListView.ItemsSource = items)
                .Take(1)
                .Subscribe();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .BindCommand(ViewModel, vm => vm.DownloadImages, v => v.DownloadImagesToolbarItem)
                        .DisposeWith(disposables);
                });
        }
    }
}
