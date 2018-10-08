using System;
using System.Collections.Concurrent;

namespace SSBakery.Helpers
{
    public class ObjectPool<T>
    {
        private ConcurrentBag<T> _objects;
        private Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator)
        {
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException("objectGenerator");
            _objects = new ConcurrentBag<T>();
        }

        public T GetObject()
        {
            if(_objects.TryTake(out var item))
            {
                return item;
            }

            return _objectGenerator();
        }

        public void PutObject(T item)
        {
            _objects.Add(item);
        }
    }
}
