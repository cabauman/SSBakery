using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using ReactiveUI;
using Splat;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class FirebaseRepo<T> : IRepository<T>, IEnableLogger
        where T : class, IModel
    {
        private ChildQuery _childQuery { get; set; }

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

        public IObservable<T> Get(string id)
        {
            return _childQuery
                .OnceSingleAsync<T>()
                .ToObservable()
                .Do(item => item.Id = id);
        }

        public IObservable<IEnumerable<T>> GetAll(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> Update(T item)
        {
            return _childQuery
                .Child(item.Id)
                .PutAsync(item)
                .ToObservable();
        }

        public IObservable<Unit> ReplaceAll(IDictionary<string, T> idToItemDict)
        {
            return _childQuery
                .PutAsync(idToItemDict)
                .ToObservable();
        }

        protected IObservable<Unit> Add(ChildQuery childQuery, T obj)
        {
            return childQuery
                .PostAsync(obj)
                .ToObservable()
                .Do(MapKeyToId)
                .Select(_ => Unit.Default);
        }

        protected IObservable<Unit> Update(ChildQuery childQuery, T obj)
        {
            return childQuery
                .PutAsync(obj)
                .ToObservable();
        }

        protected IObservable<Unit> Delete(ChildQuery childQuery)
        {
            return childQuery
                .DeleteAsync()
                .ToObservable();
        }

        protected IObservable<T> Observe(ChildQuery childQuery)
        {
            var realtimeDb = childQuery
                .AsRealtimeDatabase<T>(string.Empty, string.Empty, StreamingOptions.LatestOnly, InitialPullStrategy.Everything, true);

            realtimeDb.SyncExceptionThrown += (s, ex) => Console.WriteLine(ex.Exception);

            return realtimeDb
                .AsObservable()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            firebaseObj.Object.Id = firebaseObj.Key;
        }
    }
}
