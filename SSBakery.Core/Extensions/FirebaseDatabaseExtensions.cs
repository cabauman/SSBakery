using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Threading.Tasks;

namespace Firebase.Database.Query
{
    public static class FirebaseDatabaseExtensions
    {
        public static IObservable<Unit> FanOut<T>(this FirebaseQuery query, T item, params string[] relativePaths)
        {
            if(relativePaths == null)
            {
                throw new NullReferenceException("realativePaths can't be null.");
            }

            var fanoutObject = new Dictionary<string, T>(relativePaths.Length);
            foreach(var path in relativePaths)
            {
                fanoutObject.Add(path, item);
            }

            return query
                .PatchAsync(fanoutObject)
                .ToObservable();
        }
    }
}
