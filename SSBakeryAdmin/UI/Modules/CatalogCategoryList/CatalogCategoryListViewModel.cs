using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Helpers;
using SSBakery.Repositories.Interfaces;

namespace SSBakeryAdmin.UI.Modules
{
    public class CatalogCategoryListViewModel : ReactiveObject, ICatalogCategoryListViewModel, IPageViewModel
    {
        private readonly ObservableAsPropertyHelper<IReadOnlyList<ICatalogCategoryCellViewModel>> _categories;

        public CatalogCategoryListViewModel(
            ICatalogCategoryRepo categoryRepo = null,
            CatalogSynchronizer catalogSynchronizer = null,
            IViewStackService viewStackService = null)
        {
            categoryRepo = categoryRepo ?? Locator.Current.GetService<ICatalogCategoryRepo>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            SyncWithPosSystem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return catalogSynchronizer
                        .PullFromPosSystemAndStoreInFirebase();
                });

            LoadCategories = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return categoryRepo
                        .GetItems()
                        .SelectMany(x => x)
                        .Select(x => new CatalogCategoryCellViewModel(x))
                        .ToList()
                        .Select(x => x as IReadOnlyList<ICatalogCategoryCellViewModel>);
                });

            _categories = LoadCategories.ToProperty(this, x => x.Categories);

            SyncWithPosSystem
                .InvokeCommand(LoadCategories);

            NavigateToCategory = ReactiveCommand.CreateFromObservable<ICatalogCategoryCellViewModel, Unit>(
                catalogCategoryCell =>
                {
                    return viewStackService.PushPage(new CatalogItemListViewModel(catalogCategoryCell.CateogryId));
                });
        }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public ReactiveCommand<Unit, IReadOnlyList<ICatalogCategoryCellViewModel>> LoadCategories { get; }

        public ReactiveCommand<ICatalogCategoryCellViewModel, Unit> NavigateToCategory { get; }

        public IReadOnlyList<ICatalogCategoryCellViewModel> Categories => _categories.Value;

        public string Title => "Catalog Categories";
    }
}
