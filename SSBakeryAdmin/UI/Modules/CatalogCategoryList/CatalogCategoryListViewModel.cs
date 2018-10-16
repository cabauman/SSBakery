using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Helpers;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogCategoryListViewModel : ReactiveObject, ICatalogCategoryListViewModel, IPageViewModel
    {
        private readonly ObservableAsPropertyHelper<IReadOnlyList<ICatalogCategoryCellViewModel>> _categories;

        private ISourceCache<CatalogCategory, string> _categoryCache;
        private ReadOnlyObservableCollection<ICatalogCategoryCellViewModel> _categoryCells;

        public CatalogCategoryListViewModel(
            ICatalogCategoryRepo categoryRepo = null,
            ICatalogSynchronizer catalogSynchronizer = null,
            IViewStackService viewStackService = null)
        {
            categoryRepo = categoryRepo ?? Locator.Current.GetService<ICatalogCategoryRepo>();
            catalogSynchronizer = catalogSynchronizer ?? Locator.Current.GetService<ICatalogSynchronizer>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            _categoryCache = new SourceCache<CatalogCategory, string>(x => x.Id);

            SyncWithPosSystem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return catalogSynchronizer
                        .PullFromPosSystemAndStoreInFirebase(_categoryCache);
                });

            LoadCategories = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return categoryRepo
                        .GetItems()
                        .Do(x => _categoryCache.AddOrUpdate(x))
                        .Select(_ => Unit.Default);
                });

            //LoadCategories.InvokeCommand(this, x => x.SyncWithPosSystem);

            _categoryCache
                .Connect()
                //.Filter(dynamicFilter)
                .Transform(x => new CatalogCategoryCellViewModel(x) as ICatalogCategoryCellViewModel)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _categoryCells)
                .DisposeMany()
                .Subscribe();

            NavigateToCategory = ReactiveCommand.CreateFromObservable<ICatalogCategoryCellViewModel, Unit>(
                catalogCategoryCell =>
                {
                    return viewStackService.PushPage(new CatalogItemListViewModel(catalogCategoryCell.CateogryId));
                });
        }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public ReactiveCommand<Unit, Unit> LoadCategories { get; }

        public ReactiveCommand<ICatalogCategoryCellViewModel, Unit> NavigateToCategory { get; }

        public ReadOnlyObservableCollection<ICatalogCategoryCellViewModel> CategoryCells => _categoryCells;

        public IReadOnlyList<ICatalogCategoryCellViewModel> Categories => _categories.Value;

        public string Title => "Catalog Categories";
    }
}
