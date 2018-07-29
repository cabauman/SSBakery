using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Offline;
using Splat;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogCategoryRepo : FirebaseOfflineRepo<CatalogCategory>
    {
        private const string PathFmt = "catalogItems/{0}";

        public CatalogCategoryRepo(FirebaseClient client, string pathFmt)
            : base(client, "")
        {
        }

        //public string CatalogCategoryId
        //{
        //    get
        //    {
        //        return "";
        //    }

        //    set
        //    {
        //        _realtimeDb?.Dispose();
        //        string path = string.Format(PathFmt, value);
        //        _baseQuery = _client.Child(path);
        //        _realtimeDb = _baseQuery
        //            .AsRealtimeDatabase<CatalogCategory>(value, string.Empty, StreamingOptions.Everything, InitialPullStrategy.MissingOnly, true);
        //    }
        //}
    }
}
