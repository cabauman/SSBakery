using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public interface IRewardsMemberCellViewModel
    {
        string Name { get; }

        string PhoneNumber { get; }

        int Stamps { get; }

        RewardsMember RewardData { get; set; }

        ReactiveCommand<Unit, Unit> UseReward { get; }
    }
}
