using System;
using System.Reactive;
using GameCtor.Repository;
using SSBakery.Models;

namespace SSBakery.Repositories.Interfaces
{
    public interface ICatalogItemRepo : IRepository<CatalogItem>
    {
        IObservable<Unit> PullFromPosSystemAndStoreInFirebase(string categoryId, string beginTime = null, int? limit = null);
    }
}
