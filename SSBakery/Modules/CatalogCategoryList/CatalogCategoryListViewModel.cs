using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class CatalogCategoryListViewModel : PageViewModel, ICatalogCategoryListViewModel
    {
        private ObservableAsPropertyHelper<IEnumerable<ICatalogCategoryCellViewModel>> _catalogCategories;
        private ICatalogCategoryCellViewModel _selectedItem;

        public CatalogCategoryListViewModel(ICatalogObjectRepo catalogObjectRepo = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            catalogObjectRepo = catalogObjectRepo ?? Locator.Current.GetService<ICatalogObjectRepo>();
        }

        public ReactiveCommand<Unit, IEnumerable<ICatalogCategoryCellViewModel>> LoadCatalogCategories { get; }

        public IEnumerable<ICatalogCategoryCellViewModel> CatalogCategories
        {
            get { return _catalogCategories.Value; }
        }

        public ICatalogCategoryCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }
    }
}
