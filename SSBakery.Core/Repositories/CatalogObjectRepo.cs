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
    public class CatalogObjectRepo : ICatalogObjectRepo
    {
        public CatalogObjectRepo()
        {
        }

        public IObservable<Unit> Add(CatalogObject obj)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<CatalogObject> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<IEnumerable<CatalogObject>> GetAll(string categoryId)
        {
            var catalogApi = new CatalogApi();
            var objectTypes = new List<SearchCatalogObjectsRequest.ObjectTypesEnum> { SearchCatalogObjectsRequest.ObjectTypesEnum.ITEM };
            var query = new CatalogQuery(ExactQuery: new CatalogQueryExact("CategoryId", categoryId));
            var request = new SearchCatalogObjectsRequest(ObjectTypes: objectTypes, Query: query);

            var catalog = catalogApi
                .SearchCatalogObjectsAsync(request)
                .ToObservable()
                .Select(x => x.Objects);

            return catalog;
        }

        public IObservable<Unit> Update(CatalogObject obj)
        {
            throw new NotImplementedException();
        }

        public IObservable<ListCatalogResponse> GetAll(string cursor = null, string types = null, bool forceRefresh = false)
        {
            var catalogApi = new CatalogApi();

            var catalog = catalogApi
                .ListCatalogAsync(cursor, types)
                .ToObservable();

            return catalog;
        }
    }
}
