using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Database.Offline;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services.Interfaces;

namespace SSBakery.Repositories
{
    //using Cat = FirebaseOfflineRepo<CatalogCategory>;

    public class RepoContainer // : IRepoContainer
    {
        private FirebaseClient _firebaseClient;
        private Lazy<FirebaseOfflineRepo<CatalogCategory>> _catalogCategoryRepo;

        public RepoContainer()
        {
        }

        public IRepository<Models.CatalogCategory> CatalogCategoryRepo => _catalogCategoryRepo.Value;

        public IRepository<RewardData> RewardDataRepo
        {
            get { return new FirebaseOfflineRepo<RewardData>(_firebaseClient, "rewardData"); }
        }

        public IRepository<SSBakeryUser> UserRepo
        {
            get { return new FirebaseOfflineRepo<SSBakeryUser>(_firebaseClient, "users"); }
        }

        public FirebaseOfflineRepo<Models.CatalogItem> GetCatalogItemRepo(string catalogCategoryId)
        {
            string path = string.Format("catalogItems/{0}", catalogCategoryId);
            var repo = new FirebaseOfflineRepo<Models.CatalogItem>(_firebaseClient, path, catalogCategoryId);

            return repo;
        }

        private void InitFirebaseClient()
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

            _firebaseClient = new FirebaseClient(BaseUrl, options);
        }
    }
}
