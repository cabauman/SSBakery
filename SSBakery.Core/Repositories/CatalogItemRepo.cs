using Firebase.Database;
using GameCtor.FirebaseDatabase.DotNet;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogItemRepo : FirebaseOfflineRepo<SSBakery.Models.CatalogItem>, ICatalogItemRepo
    {
        public CatalogItemRepo(FirebaseClient client, string path, string key = null)
            : base(client, path, key)
        {
        }
    }
}
