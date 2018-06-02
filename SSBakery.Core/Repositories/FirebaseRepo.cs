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
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class FirebaseRepo<T> : IRepository<T>, IEnableLogger
        where T : class
    {
        protected RealtimeDatabase<T> RealtimeDb { get; set; }

        public IObservable<Unit> Add(T item)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<T> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IObservable<IEnumerable<T>> GetAll(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> Update(T item)
        {
            throw new NotImplementedException();
        }

        protected IObservable<T> ReadAll(ChildQuery childQuery, string filenameModifier = "")
        {
            RealtimeDb = childQuery
                .AsRealtimeDatabase<T>(filenameModifier, string.Empty, StreamingOptions.LatestOnly, InitialPullStrategy.Everything, true);

            RealtimeDb.SyncExceptionThrown +=
                (s, ex) =>
                {
                    this.Log().Error(ex.Exception);
                };

            return RealtimeDb
                .PullAsync()
                .ToObservable()
                .SelectMany(_ => ReadAll(RealtimeDb));
        }

        protected IObservable<T> Read(ChildQuery childQuery, string key)
        {
            return childQuery
                .OnceSingleAsync<T>()
                .ToObservable()
                .Do(obj => MapKeyToId(obj, key));
        }

        protected IObservable<Unit> Add(ChildQuery childQuery, T obj)
        {
            return childQuery
                .PostAsync(obj)
                .ToObservable()
                .Do(MapKeyToId)
                .Select(x => Unit.Default);
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

        protected IObservable<T> ReadAll(RealtimeDatabase<T> realtimeDb)
        {
            return realtimeDb
                .Once()
                .ToObservable()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            MapKeyToId(firebaseObj.Object, firebaseObj.Key);
        }

        private void MapKeyToId(T obj, string key)
        {
            //if(obj is BaseEntity model)
            //{
            //    model.Id = key;
            //}
        }
    }
}
