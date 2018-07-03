using ReactiveUI;
using Splat;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class RewardsProgramActivationViewModel : ViewModelBase, IRewardsProgramActivationViewModel
    {
        public RewardsProgramActivationViewModel(IScreen hostScreen = null)
            : base(hostScreen)
        {
        }

        public ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }
    }
}
