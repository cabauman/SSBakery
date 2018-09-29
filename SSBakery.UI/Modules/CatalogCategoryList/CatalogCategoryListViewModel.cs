using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Repositories.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class CatalogCategoryListViewModel : PageViewModel, ICatalogCategoryListViewModel
    {
        private ObservableAsPropertyHelper<IReadOnlyList<ICatalogCategoryCellViewModel>> _catalogCategories;
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
                        .Select(x => x as IReadOnlyList<ICatalogCategoryCellViewModel>)
                        .SubscribeOn(RxApp.TaskpoolScheduler);
                });

            _catalogCategories = LoadCatalogCategories
                .ToProperty(this, x => x.CatalogCategories, scheduler: RxApp.MainThreadScheduler);

            this
                .WhenAnyValue(vm => vm.SelectedItem)
                .Where(x => x != null)
                .SelectMany(x => ViewStackService.PushPage(new CatalogCategoryViewModel(x.Id)))
                .Subscribe();

            _isRefreshing = LoadCatalogCategories
                .IsExecuting
                .ToProperty(this, vm => vm.IsRefreshing, true);
        }

        public ReactiveCommand<Unit, IReadOnlyList<ICatalogCategoryCellViewModel>> LoadCatalogCategories { get; }

        public IReadOnlyList<ICatalogCategoryCellViewModel> CatalogCategories => _catalogCategories.Value;

        public bool IsRefreshing => _isRefreshing.Value;

        public ICatalogCategoryCellViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.RaiseAndSetIfChanged(ref _selectedItem, value); }
        }
    }
}
