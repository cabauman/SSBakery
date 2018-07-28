using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace SSBakery.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IObservable<Unit> Add(T item);

        IObservable<Unit> Update(T item);

        IObservable<Unit> Delete(string id);

        IObservable<T> Get(string id);

        IObservable<IEnumerable<T>> GetAll(bool forceRefresh = false);

        //IObservable<T> GetAll(bool forceRefresh = false);
    }
}
