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
        private ISourceCache<CatalogCategory, string> _categoryCache;
        private ReadOnlyObservableCollection<ICatalogCategoryCellViewModel> _categoryCells;
        private ICatalogCategoryCellViewModel _selectedItem;

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
                categoryCell =>
                {
                    return viewStackService.PushPage(new CatalogItemListViewModel(categoryCell.Id));
                });

            this
                .WhenAnyValue(x => x.SelectedItem)
                .Where(x => x != null)
                .SelectMany(categoryCell => viewStackService.PushPage(new CatalogItemListViewModel(categoryCell.Id)))
                .Subscribe();
        }

        public ICatalogCategoryCellViewModel SelectedItem
        {
            get => _selectedItem;
            set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
        }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public ReactiveCommand<Unit, Unit> LoadCategories { get; }

        public ReactiveCommand<ICatalogCategoryCellViewModel, Unit> NavigateToCategory { get; }

        public ReadOnlyObservableCollection<ICatalogCategoryCellViewModel> CategoryCells => _categoryCells;

        public string Title => "Catalog Categories";
    }
}
