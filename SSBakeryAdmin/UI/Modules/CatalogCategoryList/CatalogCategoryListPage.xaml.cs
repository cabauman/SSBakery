using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakeryAdmin.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogCategoryListPage : ReactiveContentPage<ICatalogCategoryListViewModel>
    {
        public CatalogCategoryListPage()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(_ => Unit.Default)
                .Take(1)
                .InvokeCommand(ViewModel, x => x.SyncWithPosSystem);

            this
                .WhenAnyValue(x => x.ViewModel.Categories)
                .Where(x => x != null)
                .Do(categories => CategoryListView.ItemsSource = categories)
                .Take(1)
                .Subscribe();
        }
    }
}
