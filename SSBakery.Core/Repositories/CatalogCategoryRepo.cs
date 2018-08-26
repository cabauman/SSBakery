using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using GameCtor.FirebaseDatabase.DotNet;
using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogCategoryRepo : FirebaseOfflineRepo<SSBakery.Models.CatalogCategory>, ICatalogCategoryRepo
    {
        public CatalogCategoryRepo(FirebaseClient client, string path, string key = null)
            : base(client, path, key)
        {
        }

        public IObservable<Unit> PullFromPosSystemAndStoreInFirebase(string beginTime = null, int? limit = null)
        {
            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.CATEGORY };
            //var beginTime = "2018-08-02T15:00:00Z";
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes, IncludeDeletedObjects: true, BeginTime: beginTime, Limit: limit);

            IObservable<CatalogObject> resultStream = catalogApi
                .SearchCatalogObjectsAsync(request)
                .ToObservable()
                .SelectMany(x => x.Objects);

            IObservable<Unit> deletedCategories = resultStream
                .Where(x => x.IsDeleted.Value)
                .Select(MapDtoToModel)
                .ToList()
                .SelectMany(categories => Delete(categories));

            IObservable<Unit> addedOrModifiedCategories = resultStream
                .Where(x => !x.IsDeleted.Value)
                .Select(MapDtoToModel)
                .ToList()
                .SelectMany(categories => Upsert(categories));

            return Observable.Merge(deletedCategories, addedOrModifiedCategories);
        }

        public async System.Threading.Tasks.Task<IList<Models.CatalogCategory>> GetCategories()
        {
            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.CATEGORY };
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes);

            var result = await catalogApi.SearchCatalogObjectsAsync(request);

            return result.Objects
                .Select(MapDtoToModel)
                .ToList();
        }

        private Models.CatalogCategory MapDtoToModel(CatalogObject dto)
        {
            return new Models.CatalogCategory()
            {
                Id = dto.Id,
                Name = dto.CategoryData.Name
            };
        }
    }
}
