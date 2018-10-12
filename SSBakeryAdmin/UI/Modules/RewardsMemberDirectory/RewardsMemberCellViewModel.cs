using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public class RewardsMemberCellViewModel : IRewardsMemberCellViewModel
    {
        private RewardsMember _model;

        public RewardsMemberCellViewModel(RewardsMember model)
        {
            _model = model;
        }

        public RewardsMember Model => _model;

        public string Name => _model.Name;

        public string PhoneNumber => _model.PhoneNumber;

        public int Stamps { get; }

        public int TotalVisits
        {
            get => _model.TotalVisits;
            set => _model.TotalVisits = value;
        }

        public int UnclaimedRewardCount
        {
            get => _model.UnclaimedRewardCount;
            set => _model.UnclaimedRewardCount = value;
        }
    }
}
