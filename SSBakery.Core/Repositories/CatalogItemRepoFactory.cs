using System.IO;
using Firebase.Database;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogItemRepoFactory
    {
        private FirebaseClient _client;
        private string _basePath;

        public CatalogItemRepoFactory(FirebaseClient client, string basePath)
        {
            _client = client;
            _basePath = basePath;
        }

        public ICatalogItemRepo Create(string categoryId)
        {
            string path = Path.Combine(_basePath, categoryId);
            return new CatalogItemRepo(_client, path, categoryId);
        }
    }
}
