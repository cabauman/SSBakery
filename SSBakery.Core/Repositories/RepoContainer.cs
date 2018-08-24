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
        private const string NODE_USERS = "users";
        private const string NODE_USER_OWNED = "userOwned";
        private const string NODE_USER_READABLE = "userReadable";
        private const string NODE_USER_WRITABLE = "userWritable";
        private const string NODE_AUTH_READABLE = "authReadable";

        private const string NODE_CATALOG_CATAGORIES = "catalogCategories";
        private const string NODE_CATALOG_ITEMS = "catalogItems";
        private const string NODE_REWARD_DATA = "customerRewardData";

        private static readonly string PATHFMT_USER = Path.Combine(NODE_USERS, "{0}");
        private static readonly string PATH_CATALOG_CATEGORIES = Path.Combine(NODE_AUTH_READABLE, NODE_CATALOG_CATAGORIES);
        private static readonly string PATHFMT_CATALOG_ITEMS_FOR_CATEGORY = Path.Combine(PATH_CATALOG_CATEGORIES, "{0}");
        private static readonly string PATH_REWARD_DATA = Path.Combine(NODE_USER_READABLE, NODE_REWARD_DATA);
        private static readonly string PATHFMT_REWARD_DATA_FOR_CUSTOMER = Path.Combine(PATH_REWARD_DATA, "{0}");

        private readonly FirebaseClient _firebaseClient;

        public RepoContainer()
        {
            _firebaseClient = InitFirebaseClient();
        }

        public IRepository<CatalogCategory> CatalogCategoryRepo
        {
            get { return new FirebaseOfflineRepo<CatalogCategory>(_firebaseClient, PATH_CATALOG_CATEGORIES); }
        }

        public IRepository<CustomerRewardData> RewardDataRepo
        {
            get { return new FirebaseOfflineRepo<CustomerRewardData>(_firebaseClient, PATH_REWARD_DATA); }
        }

        public IRepository<SSBakeryUser> UserRepo
        {
            get { return new FirebaseOfflineRepo<SSBakeryUser>(_firebaseClient, NODE_USERS); }
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
