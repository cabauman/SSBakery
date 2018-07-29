using Square.Connect.Model;
using SSBakery.Models;

namespace SSBakery.Repositories.Interfaces
{
    public interface IRepoContainer
    {
        IRepository<Customer> CustomerRepo { get; }

        IRepository<RewardData> RewardDataRepo { get; }

        IRepository<SSBakeryUser> UserRepo { get; }
    }
}
