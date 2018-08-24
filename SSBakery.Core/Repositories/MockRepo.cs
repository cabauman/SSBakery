using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

// [assembly: Xamarin.Forms.Dependency(typeof(SSBakery.Services.MockDataStore))]
namespace SSBakery.Repositories
{
    public class MockRepo //: IRepository<Item>
    {
        private List<Item> _items;

        public MockRepo()
        {
            _items = new List<Item>();
            var mockItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description = "This is an item description." },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description = "This is an item description." },
            };

            foreach (var item in mockItems)
            {
                _items.Add(item);
            }
        }

        public IObservable<Unit> Add(Item item)
        {
            _items.Add(item);

            return Observable.Return(Unit.Default);
        }

        public IObservable<Unit> Update(Item item)
        {
            var itemToUpdate = _items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            _items.Remove(itemToUpdate);
            _items.Add(item);

            return Observable.Return(Unit.Default);
        }

        public IObservable<Unit> Delete(string id)
        {
            var itemToDelete = _items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            _items.Remove(itemToDelete);

            return Observable.Return(Unit.Default);
        }

        public IObservable<Item> GetItem(string id)
        {
            return _items
                .ToObservable()
                .FirstAsync(s => s.Id == id);
        }

        public IObservable<IEnumerable<Item>> GetItems(int? cursor, int? count, bool forceRefresh = false)
        {
            return Observable
                .Return(_items);
        }
    }
}
