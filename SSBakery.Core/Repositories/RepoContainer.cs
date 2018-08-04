using System;
using System.Collections.Generic;
using System.IO;
using Firebase.Database;
using Firebase.Database.Offline;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services.Interfaces;

namespace SSBakery.Repositories
{
    public class RepoContainer // : IRepoContainer
    {
        private const string NODE_USER_READABLE = "userReadable";
        private const string NODE_AUTH_READABLE = "authReadable";
        private const string NODE_USER_OWNED = "userOwned";

        private const string USERS = "users";
        private const string NODE_CATALOG_CATAGORIES = "catalogCategories";
        private const string NODE_CATALOG_ITEMS = "catalogItems";

        private static readonly string PATH_CATALOG_CATEGORIES = Path.Combine(NODE_AUTH_READABLE, NODE_CATALOG_CATAGORIES);
        private static readonly string PATH_CATALOG_ITEMS = Path.Combine(NODE_AUTH_READABLE, NODE_CATALOG_ITEMS);
        private static readonly string PATHFMT_CATALOG_ITEMS_FOR_CATEGORY =
            Path.Combine(PATH_CATALOG_ITEMS, "{0}");

        private readonly FirebaseClient _firebaseClient;

        public RepoContainer()
        {
            _firebaseClient = InitFirebaseClient();
        }

        public IRepository<CatalogCategory> CatalogCategoryRepo
        {
            get { return new FirebaseOfflineRepo<CatalogCategory>(_firebaseClient, PATH_CATALOG_CATEGORIES); }
        }

        public IRepository<RewardData> RewardDataRepo
        {
            get { return new FirebaseOfflineRepo<RewardData>(_firebaseClient, "rewardData"); }
        }

        public IRepository<SSBakeryUser> UserRepo
        {
            get { return new FirebaseOfflineRepo<SSBakeryUser>(_firebaseClient, USERS); }
        }

        public FirebaseOfflineRepo<CatalogItem> GetCatalogItemRepo(string catalogCategoryId)
        {
            string path = string.Format(PATHFMT_CATALOG_ITEMS_FOR_CATEGORY, catalogCategoryId);
            var repo = new FirebaseOfflineRepo<CatalogItem>(_firebaseClient, path, catalogCategoryId);

            return repo;
        }

        private FirebaseClient InitFirebaseClient()
        {
            const string BaseUrl = "https://<YOUR PROJECT ID>.firebaseio.com";
            IFirebaseAuthService authService = null;

            FirebaseOptions options = new FirebaseOptions()
            {
                OfflineDatabaseFactory = (t, s) => new OfflineDatabase(t, s),
                AuthTokenAsyncFactory = async () => await authService.GetIdTokenAsync(),
                JsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore
                }
            };

            return new FirebaseClient(BaseUrl, options);
        }
    }
}
