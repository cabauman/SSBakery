using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
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
                .InvokeCommand(this, x => x.ViewModel.LoadCategories);

            this
                .WhenAnyValue(x => x.ViewModel.CategoryCells)
                .Where(x => x != null)
                .Do(categories => CategoryListView.ItemsSource = categories)
                .Take(1)
                .Subscribe();

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.CategoryListView.SelectedItem)
                        .DisposeWith(disposables);
                });
        }
    }
}
