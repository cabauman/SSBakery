using System;
using Square.Connect.Model;

namespace SSBakery.Repositories.Interfaces
{
    public interface ICatalogObjectRepo : IRepository<CatalogObject>
    {
        IObservable<ListCatalogResponse> GetAll(string cursor = null, string types = null, bool forceRefresh = false);
    }
}
