using System;
using System.Reactive;

namespace SSBakery.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IObservable<Unit> Add(T item);

        IObservable<Unit> Update(T item);

        IObservable<Unit> Delete(string id);

        IObservable<T> GetItem(string id);

        IObservable<RepoItemCollection<T>> GetItems(int? cursor = null, int? count = null, bool fetchOnline = false);

        //IObservable<T> GetAll(bool forceRefresh = false);
    }
}
