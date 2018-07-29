using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class CatalogCategoryListViewModel : PageViewModel, ICatalogCategoryListViewModel
    {
        private ObservableAsPropertyHelper<IEnumerable<ICatalogCategoryCellViewModel>> _catalogCategories;
        private ObservableAsPropertyHelper<bool> _isRefreshing;
        private ICatalogCategoryCellViewModel _selectedItem;
        private ICatalogCategoryCellViewModel _itemAppearing;
        private string _cursor = null;

        public CatalogCategoryListViewModel(IRepository<CatalogCategory> catalogCategoryRepo = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            catalogCategoryRepo = catalogCategoryRepo ?? Locator.Current.GetService<IRepository<CatalogCategory>>();

            LoadCatalogCategories = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return catalogCategoryRepo.GetAll()
                        .SelectMany(x => x)
                        .Select(x => new CatalogCategoryCellViewModel(x) as ICatalogCategoryCellViewModel)
                        .ToList()
                        .Select(x => x.AsEnumerable());
                });

            this
                .WhenAnyValue(vm => vm.SelectedItem)
                .Where(x => x != null)
                .SelectMany(x => viewStackService.PushPage(new CatalogCategoryViewModel(x.Id)))
                .Subscribe();

            _isRefreshing = LoadCatalogCategories
                .IsExecuting
                .ToProperty(this, vm => vm.IsRefreshing, true);
        }

        public ReactiveCommand<Unit, IEnumerable<ICatalogCategoryCellViewModel>> LoadCatalogCategories { get; }

        public IEnumerable<ICatalogCategoryCellViewModel> CatalogCategories => _catalogCategories.Value;

        public bool IsRefreshing => _isRefreshing.Value;

        public ICatalogCategoryCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }

        public ICatalogCategoryCellViewModel ItemAppearing
        {
            get { return _itemAppearing; }
            set { this.RaiseAndSetIfChanged(ref _itemAppearing, value); }
        }
    }
}
