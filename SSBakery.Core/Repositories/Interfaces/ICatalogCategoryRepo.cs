using System;
using System.Reactive;
using GameCtor.Repository;
using SSBakery.Models;

namespace SSBakery.Repositories.Interfaces
{
    public interface ICatalogCategoryRepo : IRepository<CatalogCategory>
    {
        IObservable<Unit> PullFromPosSystemAndStoreInFirebase(string beginTime = null, int? limit = null);
    }
}
