using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Offline;
using Firebase.Database.Query;
using Splat;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services.Interfaces;

namespace SSBakery.Repositories
{
    public class FirebaseOfflineRepo<T> : IRepository<T>, IEnableLogger
        where T : class, IModel
    {
        private const string BaseUrl = "https://<YOUR PROJECT ID>.firebaseio.com";

        private readonly RealtimeDatabase<T> _realtimeDb;
        private readonly ChildQuery _baseQuery;

        public FirebaseOfflineRepo(IFirebaseAuthService authService, string path, string key = "")
        {
            FirebaseOptions options = new FirebaseOptions()
            {
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
                AuthTokenAsyncFactory = async () => await authService.GetIdTokenAsync()
            };

            // The offline database filename is named after type T.
            // So, if you have more than one list of type T objects, you need to differentiate it
            // by adding a filename modifier; which is what we're using the "key" parameter for.
            var client = new FirebaseClient(BaseUrl, options);
            _baseQuery = client.Child(path);
            _realtimeDb = _baseQuery
                .AsRealtimeDatabase<T>(key, "", StreamingOptions.Everything, InitialPullStrategy.MissingOnly, true);
        }

        public IObservable<Unit> Add(T item)
        {
            return Observable
                .Start(() => _realtimeDb.Post(item))
                .Do(itemKey => item.Id = itemKey)
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> Delete(string id)
        {
            return Observable
                .Start(() => _realtimeDb.Delete(id));
        }

        public IObservable<T> Get(string id)
        {
            return Observable
                .Start(() => _realtimeDb.Database[id].Deserialize<T>());
        }

        public IObservable<T> GetAll(bool forceRefresh = false)
        {
            return Observable
                .Return(forceRefresh ? Pull() : Observable.Return(Unit.Default))
                .SelectMany(x => x)
                .SelectMany(_ => _realtimeDb.Once())
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        public IObservable<Unit> Update(T item)
        {
            return Observable
                .Start(() => _realtimeDb.Put(item.Id, item));
        }

        public IObservable<T> Observe()
        {
            _realtimeDb.SyncExceptionThrown += (s, ex) => Console.WriteLine(ex.Exception);

            return _realtimeDb
                .AsObservable()
                .Do(MapKeyToId)
                .Select(x => x.Object);
        }

        public IObservable<Unit> Populate(IEnumerable<T> items)
        {
            _realtimeDb.Database.Clear();

            return _baseQuery
                .PutAsync(items.ToDictionary(x => x.Id))
                .ToObservable()
                .Concat(_realtimeDb.PullAsync().ToObservable());
        }

        private IObservable<Unit> Pull()
        {
            return _realtimeDb
                .PullAsync()
                .ToObservable();
        }

        private void MapKeyToId(FirebaseObject<T> firebaseObj)
        {
            firebaseObj.Object.Id = firebaseObj.Key;
        }
    }
}
