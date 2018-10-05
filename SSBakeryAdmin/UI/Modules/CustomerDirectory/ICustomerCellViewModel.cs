using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public interface ICustomerCellViewModel
    {
        string Name { get; }

        string PhoneNumber { get; }

        int Stamps { get; }

        CustomerRewardData RewardData { get; }

        ReactiveCommand<Unit, Unit> UseReward { get; }
    }
}