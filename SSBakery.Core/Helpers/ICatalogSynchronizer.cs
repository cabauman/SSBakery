using System;
using System.Reactive;
using DynamicData;
using SSBakery.Models;

namespace SSBakery.Helpers
{
    public interface ICatalogSynchronizer
    {
        IObservable<Unit> PullFromPosSystemAndStoreInFirebase(ISourceCache<CatalogCategory, string> categoryCache);
    }
}
