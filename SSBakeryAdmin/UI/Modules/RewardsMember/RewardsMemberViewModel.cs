using System.Reactive;
using GameCtor.RxNavigation;
using ReactiveUI;

namespace SSBakeryAdmin.UI.Modules
{
    public class RewardsMemberViewModel : IRewardsMemberViewModel, IPageViewModel
    {
        private SSBakery.Models.RewardsMember _model;

        public string Title => "Rewards Member";

        public string Name => _model.Name;

        public string PhoneNumber => _model.PhoneNumber;

        public int TotalVisits => _model.TotalVisits;

        public int UnclaimedRewardCount => _model.UnclaimedRewardCount;
    }
}
