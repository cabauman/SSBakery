using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public interface IRewardsMemberCellViewModel
    {
        RewardsMember Model { get; }

        string Name { get; }

        string PhoneNumber { get; }

        int Stamps { get; }

        int TotalVisits { get; set; }

        int UnclaimedRewardCount { get; set; }
    }
}
