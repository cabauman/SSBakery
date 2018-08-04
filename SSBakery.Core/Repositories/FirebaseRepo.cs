using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Splat;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class FirebaseRepo<T> : IRepository<T>, IEnableLogger
        where T : class, IModel
    {
        private ChildQuery _childQuery;

        public IObservable<Unit> Add(T item)
        {
            return _childQuery
                .PostAsync(item)
                .ToObservable()
                .Do(MapKeyToId)
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> Delete(string id)
        {
            return _childQuery
                .Child(id)
                .DeleteAsync()
                .ToObservable();
        }

        public IObservable<T> GetItem(string id)
        {
            return _childQuery
                .OnceSingleAsync<T>()
                .ToObservable()
                .Do(item => item.Id = id);
        }

        public IObservable<IEnumerable<T>> GetItems(int? cursor, int? count, bool forceRefresh = false)
        {
            return _childQuery
                .OnceAsync<T>()
                .ToObservable()
                .SelectMany(x => x)
                .Do(MapKeyToId)
                .Select(x => x.Object)
                .ToList();
        }

        public IObservable<Unit> Update(T item)
        {
            return _childQuery
                .Child(item.Id)
                .PutAsync(item)
                .ToObservable();
        }

        public IObservable<T> Observe()
        {
            return _childQuery
                .AsObservable<T>()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        public IObservable<Unit> Populate(IDictionary<string, T> idToItemDict)
        {
            return _childQuery
                .PutAsync(idToItemDict)
                .ToObservable();
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            firebaseObj.Object.Id = firebaseObj.Key;
        }
    }
}
