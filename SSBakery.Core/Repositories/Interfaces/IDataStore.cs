using Square.Connect.Model;
using SSBakery.Models;

namespace SSBakery.Repositories.Interfaces
{
    public interface IDataStore
    {
        IRepository<CatalogObject> CatalogObjectRepo { get; }

        IRepository<Customer> CustomerRepo { get; }

        IRepository<RewardData> RewardDataRepo { get; }
    }
}
