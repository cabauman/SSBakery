using System;
using System.Reactive;
using System.Reactive.Linq;
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
            NavigateToPhoneNumberVerificationPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    IObservable<Unit> completionObservable = NavigateAndReset(new MainViewModel())
                            .SelectMany(_ => Navigate(new RewardsViewModel()));

                    var page = new PhoneNumberVerificationViewModel(
                        PhoneNumberVerificationViewModel.AuthAction.LinkAccount,
                        completionObservable);

                    return Navigate(page);
                });
        }

        public ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }
    }
}
