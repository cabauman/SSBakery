using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;

namespace SSBakery.Repositories
{
    public class RepoItemCollection<T>
    {
        public RepoItemCollection(int cursor, IEnumerable<T> items)
        {
            Cursor = cursor;
            Items = items;
        }

        public int Cursor { get; }

        public IEnumerable<T> Items { get; }
    }
}
