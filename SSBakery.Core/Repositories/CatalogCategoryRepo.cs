using Firebase.Database;
using GameCtor.FirebaseDatabase.DotNet;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogCategoryRepo : FirebaseOfflineRepo<SSBakery.Models.CatalogCategory>, ICatalogCategoryRepo
    {
        public CatalogCategoryRepo(FirebaseClient client, string path, string key = null)
            : base(client, path, key)
        {
        }
    }
}
