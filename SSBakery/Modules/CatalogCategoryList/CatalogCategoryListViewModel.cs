using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using GameCtor.Repository;
using ReactiveUI;
using RxNavigation;
using Splat;
using SSBakery;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogCategoryListViewModel : PageViewModel, ICatalogCategoryListViewModel
    {
        private ObservableAsPropertyHelper<IEnumerable<ICatalogCategoryCellViewModel>> _catalogCategories;
        private ObservableAsPropertyHelper<bool> _isRefreshing;
        private ICatalogCategoryCellViewModel _selectedItem;

        public CatalogCategoryListViewModel(ICatalogCategoryRepo catalogCategoryRepo = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            catalogCategoryRepo = catalogCategoryRepo ?? Locator.Current.GetService<ICatalogCategoryRepo>();

            LoadCatalogCategories = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return catalogCategoryRepo.GetItems()
                        .SelectMany(x => x)
                        .Select(x => new CatalogCategoryCellViewModel(x) as ICatalogCategoryCellViewModel)
                        .ToList()
                        .Select(x => x.AsEnumerable());
                });

            _catalogCategories = LoadCatalogCategories
                .ToProperty(this, x => x.CatalogCategories);

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
    }
}
