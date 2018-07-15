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
    public class CatalogObjectRepo : IRepository<CatalogObject>
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

        public IObservable<IEnumerable<CatalogObject>> GetAll(bool forceRefresh = false)
        {
            var catalogApi = new CatalogApi();

            var catalog = catalogApi
                .ListCatalogAsync()
                .ToObservable()
                .Select(x => x.Objects);

            return catalog;
        }

        public IObservable<Unit> Update(CatalogObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
