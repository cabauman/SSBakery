using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using GameCtor.RxNavigation;
using ReactiveUI;
using Splat;
using SSBakery.Core.Common;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class RewardsProgramActivationViewModel : PageViewModel, IRewardsProgramActivationViewModel
    {
        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _verificationId;

        public RewardsProgramActivationViewModel(IFirebaseAuthService firebaseAuthService = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            NavigateToPhoneNumberVerificationPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return ViewStackService
                        .PushPage(new PhoneAuthPhoneNumberEntryViewModel(AuthAction.LinkAccount, WhenLinked()));
                });
        }

        public ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }

        private IObservable<Unit> WhenLinked()
        {
            return Observable
                .Defer(
                    () =>
                    {
                        return Observable
                            .Start(() => ViewStackService.InsertPage(1, new RewardsViewModel()))
                            .Concat(ViewStackService.PopToPage(1));
                    });
        }
    }
}
