using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Xml;
using ReactiveUI;
using GameCtor.RxNavigation;
using Splat;
using SSBakeryAdmin.iOS.Modules.CatalogCategory;
using SSBakery.Repositories.Interfaces;

namespace SSBakeryAdmin.iOS.Modules.CatalogCategoryList
{
    public class CatalogCategoryListViewModel : ReactiveObject, ICatalogCategoryListViewModel, IPageViewModel
    {
        private readonly ObservableAsPropertyHelper<IReadOnlyList<ICatalogCategoryCellViewModel>> _categories;

        private string _timestampOfLatestSync;

        public CatalogCategoryListViewModel(
            ICatalogCategoryRepo categoryRepo = null,
            ICatalogItemRepo itemRepo = null,
            IViewStackService viewStackService = null)
        {
            categoryRepo = categoryRepo ?? Locator.Current.GetService<ICatalogCategoryRepo>();
            itemRepo = itemRepo ?? Locator.Current.GetService<ICatalogItemRepo>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            SyncWithPosSystem = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return GetTimestampOfLatestSync()
                        .SelectMany(
                            timestamp =>
                            {
                                return categoryRepo
                                    .PullFromPosSystemAndStoreInFirebase(timestamp)
                                    .Select(_ => categoryRepo.GetItems())
                                    .Switch()
                                    .SelectMany(x => x)
                                    .SelectMany(category => itemRepo.PullFromPosSystemAndStoreInFirebase(category.Id, timestamp))
                                    .SelectMany(_ => SaveTimestampOfLatestSync());
                            });
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

            NavigateToCategory = ReactiveCommand.CreateFromObservable<ICatalogCategoryCellViewModel, Unit>(
                catalogCategoryCell =>
                {
                    return viewStackService.PushPage(new CatalogCategoryViewModel(catalogCategoryCell.CateogryId));
                });
        }

        public ReactiveCommand<Unit, IReadOnlyList<ICatalogCategoryCellViewModel>> LoadCategories { get; }

        public ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        public ReactiveCommand<ICatalogCategoryCellViewModel, Unit> NavigateToCategory { get; }

        public IReadOnlyList<ICatalogCategoryCellViewModel> Categories => _categories.Value;

        public string Title => "Catalog Categories";

        private IObservable<string> GetTimestampOfLatestSync()
        {
            return Xamarin.Essentials.SecureStorage
                .GetAsync("timestampOfLatestPosSystemSync")
                .ToObservable();
        }

        private IObservable<Unit> SaveTimestampOfLatestSync()
        {
            var timestamp = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
            return Xamarin.Essentials.SecureStorage
                .SetAsync("timestampOfLatestPosSystemSync", timestamp)
                .ToObservable();
        }
    }
}