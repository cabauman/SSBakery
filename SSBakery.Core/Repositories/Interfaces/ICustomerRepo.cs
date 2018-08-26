using System;
using System.Reactive;

namespace SSBakery.Repositories.Interfaces
{
    public interface ICustomerRepo
    {
        IObservable<Unit> PullFromPosSystemAndStoreInFirebase(string beginTime = null, int? limit = null);
    }
}
