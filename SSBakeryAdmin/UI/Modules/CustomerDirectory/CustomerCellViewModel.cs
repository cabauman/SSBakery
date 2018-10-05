using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class CustomerCellViewModel : ICustomerCellViewModel
    {
        public string Name { get; }

        public string PhoneNumber { get; }

        public int Stamps { get; }

        public CustomerRewardData RewardData { get; }

        public ReactiveCommand<Unit, Unit> UseReward { get; }
    }
}