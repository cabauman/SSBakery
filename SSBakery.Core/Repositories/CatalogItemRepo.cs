using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using GameCtor.FirebaseDatabase.DotNet;
using Square.Connect.Api;
using Square.Connect.Client;
using Square.Connect.Model;
using SSBakery.Config;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogItemRepo : FirebaseOfflineRepo<SSBakery.Models.CatalogItem>, ICatalogItemRepo
    {
        public CatalogItemRepo(FirebaseClient client, string path, string key = null)
            : base(client, path, key)
        {
        }

        public IObservable<Unit> PullFromPosSystemAndStoreInFirebase(string categoryId, string beginTime = null, int? limit = null)
        {
            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM };
            var query = new CatalogQuery(ExactQuery: new CatalogQueryExact("CategoryId", categoryId));
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes, BeginTime: beginTime, Query: query, Limit: limit);

            IObservable<CatalogObject> resultStream = catalogApi
                .SearchCatalogObjectsAsync(request)
                .ToObservable()
                .SelectMany(x => x.Objects);

            IObservable<Unit> deletedItems = resultStream
                .Where(x => x.IsDeleted.Value)
                .Select(MapDtoToModel)
                .ToList()
                .SelectMany(items => Delete(items));

            IObservable<Unit> addedOrModifiedItems = resultStream
                .Where(x => !x.IsDeleted.Value)
                .Select(MapDtoToModel)
                .ToList()
                .SelectMany(items => Upsert(items));

            return Observable.Merge(deletedItems, addedOrModifiedItems);
        }

        public async System.Threading.Tasks.Task<IList<SSBakery.Models.CatalogItem>> GetAllItems()
        {
            Configuration.Default.AccessToken = ApiKeys.SQUARE_CONNECT;
            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM };
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes);

            return await catalogApi.SearchCatalogObjectsAsync(request)
                .ToObservable()
                .SelectMany(x => x.Objects)
                .Select(MapDtoToModel)
                .ToList();
        }

        private SSBakery.Models.CatalogItem MapDtoToModel(CatalogObject dto)
        {
            return new SSBakery.Models.CatalogItem()
            {
                Id = dto.Id,
                Name = dto.ItemData.Name,
                Description = dto.ItemData.Description,
                //Price = dto.ItemVariationData.PriceMoney.Amount.ToString(),
                CategoryId = dto.ItemData.CategoryId
            };
        }
    }
}
