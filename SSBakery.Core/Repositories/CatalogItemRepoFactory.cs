using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class CatalogItemRepoFactory
    {
        private const string PathFmt = "authReadable/catalogItems/{0}";

        public IRepository<CatalogItem> Create(string catalogCategoryId)
        {
            return new FirebaseOfflineRepo<CatalogItem>(null, string.Format(PathFmt, catalogCategoryId));
        }
    }
}
