using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public interface IRewardsMemberViewModel
    {
        string Name { get; }

        string PhoneNumber { get; }

        int TotalVisits { get; }

        int UnclaimedRewardCount { get; }
    }
}
