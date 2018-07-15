using Square.Connect.Model;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class RepoContainer : IRepoContainer
    {
        public RepoContainer()
        {
            CatalogObjectRepo = new CatalogObjectRepo();
            CustomerRepo = new CustomerRepo();
            RewardDataRepo = new FirebaseRepo<RewardData>();
            UserRepo = new FirebaseRepo<SSBakeryUser>();
        }

        public IRepository<CatalogObject> CatalogObjectRepo { get; }

        public IRepository<Customer> CustomerRepo { get; }

        public IRepository<RewardData> RewardDataRepo { get; }

        public IRepository<SSBakeryUser> UserRepo { get; }
    }
}
