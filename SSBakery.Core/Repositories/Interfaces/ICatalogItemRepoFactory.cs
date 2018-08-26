using GameCtor.Repository;
using SSBakery.Models;

namespace SSBakery.Repositories.Interfaces
{
    public interface ICatalogItemRepoFactory
    {
        IRepository<CatalogItem> Create(string catalogCategoryId);
    }
}
