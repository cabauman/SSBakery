using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Xml;
using DynamicData;
using Splat;
using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Helpers
{
    public class CatalogSynchronizer : ICatalogSynchronizer
    {
        private ICatalogCategoryRepo _categoryRepo;
        private ICatalogItemRepoFactory _itemRepoFactory;
        private IAdminVarRepo _adminVarRepo;

        public CatalogSynchronizer(
            ICatalogCategoryRepo categoryRepo = null,
            ICatalogItemRepoFactory itemRepoFactory = null,
            IAdminVarRepo adminVarRepo = null)
        {
            _categoryRepo = categoryRepo ?? Locator.Current.GetService<ICatalogCategoryRepo>();
            _itemRepoFactory = itemRepoFactory ?? Locator.Current.GetService<ICatalogItemRepoFactory>();
            _adminVarRepo = adminVarRepo ?? Locator.Current.GetService<IAdminVarRepo>();
        }

        public IObservable<Unit> PullFromPosSystemAndStoreInFirebase(ISourceCache<SSBakery.Models.CatalogCategory, string> categoryCache)
        {
            return GetTimestampOfLatestSync()
                .SelectMany(
                    beginTime =>
                    {
                        return SyncCategories(beginTime, categoryCache);
                        //return Observable
                        //    .Merge(SyncCategories(beginTime, categoryCache), SyncItems(beginTime));
                    })
                .Concat(SaveTimestampOfLatestSync());
        }

        private IObservable<Unit> SyncCategories(string beginTime, ISourceCache<SSBakery.Models.CatalogCategory, string> categoryCache)
        {
            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.CATEGORY };
            //var beginTime = "2018-08-02T15:00:00Z";
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes, IncludeDeletedObjects: true, BeginTime: beginTime);

            IObservable<CatalogObject> resultStream = catalogApi
                .SearchCatalogObjectsAsync(request)
                .ToObservable()
                .Where(x => x.Objects != null)
                .SelectMany(x => x.Objects);

            IObservable<Unit> deletedCategories = resultStream
                .Where(x => x.IsDeleted.Value && categoryCache.Lookup(x.Id).HasValue)
                .SelectMany(x => _categoryRepo.Delete(x.Id));

            IObservable<Unit> addedOrModifiedCategories = resultStream
                .Where(x => !x.IsDeleted.Value)
                .Select(x => UpdateCategoryCache(x, categoryCache))
                .SelectMany(category => _categoryRepo.Upsert(category));
                //.ToList()
                //.SelectMany(categories => _categoryRepo.Upsert(categories));

            return Observable.Merge(deletedCategories, addedOrModifiedCategories);
        }

        private IObservable<Unit> SyncItems(string beginTime)
        {
            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM };
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes, BeginTime: beginTime);

            IObservable<CatalogObject> resultStream = catalogApi
                .SearchCatalogObjectsAsync(request)
                .ToObservable()
                .Where(x => x.Objects != null)
                .SelectMany(x => x.Objects);

            IObservable<Unit> deletedItems = resultStream
                .Where(x => x.IsDeleted.Value)
                .GroupBy(x => x.ItemData.CategoryId)
                .SelectMany(
                    x =>
                    {
                        return x
                            .Select(catalogObject => catalogObject.Id)
                            .SelectMany(itemId => _itemRepoFactory.Create(x.Key).Delete(itemId));
                            //.ToList()
                            //.SelectMany(items => _itemRepoFactory.Create(x.Key).Delete(items));
                    });

            IObservable<Unit> addedOrModifiedItems = resultStream
                .Where(x => !x.IsDeleted.Value)
                .Select(MapDtoToItem)
                .GroupBy(x => x.CategoryId)
                .SelectMany(
                    x =>
                    {
                        return x
                            .SelectMany(item => _itemRepoFactory.Create(x.Key).Upsert(item));
                            //.ToList()
                            //.SelectMany(items => _itemRepoFactory.Create(x.Key).Upsert(items));
                    });

            return Observable.Merge(deletedItems, addedOrModifiedItems);
        }

        private SSBakery.Models.CatalogCategory UpdateCategoryCache(CatalogObject catalogObject, ISourceCache<SSBakery.Models.CatalogCategory, string> categoryCache)
        {
            var lookupResult = categoryCache.Lookup(catalogObject.Id);
            var isNew = !lookupResult.HasValue;
            var category = isNew ? null : lookupResult.Value;
            if(isNew)
            {
                category = MapDtoToCategory(catalogObject);
                categoryCache.AddOrUpdate(category);
            }
            else
            {
                category.Name = catalogObject.CategoryData.Name;
            }

            return category;
        }

        private SSBakery.Models.CatalogCategory MapDtoToCategory(CatalogObject dto)
        {
            return new Models.CatalogCategory()
            {
                Id = dto.Id,
                Name = dto.CategoryData.Name
            };
        }

        private SSBakery.Models.CatalogItem MapDtoToItem(CatalogObject dto)
        {
            return new SSBakery.Models.CatalogItem()
            {
                Id = dto.Id,
                Name = dto.ItemData.Name,
                Description = dto.ItemData.Description,
                Price = Convert.ToDecimal(dto.ItemVariationData.PriceMoney.Amount / 100).ToString("C2"),
                CategoryId = dto.ItemData.CategoryId
            };
        }

        private IObservable<string> GetTimestampOfLatestSync()
        {
            return _adminVarRepo
                .GetInstance()
                .Select(x => x.CatalogSyncTimestamp);
        }

        private IObservable<Unit> SaveTimestampOfLatestSync()
        {
            return _adminVarRepo
                .GetInstance()
                .Do(
                    x =>
                    {
                        var timestamp = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
                        x.CatalogSyncTimestamp = timestamp;
                    })
                .SelectMany(x => _adminVarRepo.Upsert(x));
        }
    }
}
