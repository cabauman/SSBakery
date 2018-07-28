using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Square.Connect.Api;
using Square.Connect.Model;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogItemRepo
    {
        public CatalogItemRepo()
        {
        }

        public IObservable<IEnumerable<SSBakery.Models.CatalogItem>> GetAll(string categoryId, bool syncWithPos)
        {
            return Observable
                .Return(syncWithPos ? PullFromPosAndStoreInFirebase(categoryId) : PullFromFirebase(categoryId))
                .SelectMany(x => x);
        }

        private IObservable<IEnumerable<SSBakery.Models.CatalogItem>> PullFromPosAndStoreInFirebase(string categoryId)
        {
            var firebaseRepo = new FirebaseOfflineRepo<SSBakery.Models.CatalogItem>(null, null);

            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM };
            var query = new CatalogQuery(ExactQuery: new CatalogQueryExact("CategoryId", categoryId));
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes, Query: query);

            return catalogApi
                .SearchCatalogObjectsAsync(request)
                .ToObservable()
                .SelectMany(x => x.Objects)
                .Select(MapDtoToModel)
                .ToList()
                .SelectMany(items => firebaseRepo.Populate(items).Select(_ => items));
        }

        private IObservable<IEnumerable<SSBakery.Models.CatalogItem>> PullFromFirebase(string categoryId)
        {
            var firebaseRepo = new FirebaseOfflineRepo<SSBakery.Models.CatalogItem>(null, null);

            return firebaseRepo
                .GetAll()
                .ToList();
        }

        private SSBakery.Models.CatalogItem MapDtoToModel(CatalogObject dto)
        {
            return new SSBakery.Models.CatalogItem()
            {
                Id = dto.Id,
                Name = dto.CategoryData.Name
            };
        }
    }
}
