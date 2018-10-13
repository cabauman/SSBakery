using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
                .InvokeCommand(ViewModel, x => x.LoadItems);

            this
                .WhenAnyValue(x => x.ViewModel.Items)
                .Where(x => x != null)
                .Do(items => CatalogItemListView.ItemsSource = items)
                .Take(1)
                .Subscribe();
        }
    }
}
