using System;
using System.Reactive;

namespace SSBakery.Helpers
{
    public interface ICatalogSynchronizer
    {
        IObservable<Unit> PullFromPosSystemAndStoreInFirebase();
    }
}
