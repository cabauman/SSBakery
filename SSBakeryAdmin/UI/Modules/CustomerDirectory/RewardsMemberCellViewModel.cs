using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class RewardsMemberCellViewModel : IRewardsMemberCellViewModel
    {
        public string Name { get; }

        public string PhoneNumber { get; }

        public int Stamps { get; }

        public RewardsMember RewardData { get; set; }

        public ReactiveCommand<Unit, Unit> UseReward { get; }
    }
}
