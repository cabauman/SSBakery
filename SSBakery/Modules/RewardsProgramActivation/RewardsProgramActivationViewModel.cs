using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using SSBakery.Core.Common;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

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
                    IObservable<Unit> whenLinked = Observable
                        .Defer(
                            () =>
                            {
                                return ViewStackService
                                    .PopToPageAndPush<MainViewModel>(new RewardsViewModel());
                                    //.PopToPage<MainViewModel>(false)
                                    //.SelectMany(_ => ViewStackService.PushPage(new RewardsViewModel()));
                            });

                    return ViewStackService
                        .PushPage(new PhoneAuthPhoneNumberEntryViewModel(AuthAction.LinkAccount, whenLinked));
                });
        }

        public ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }
    }
}
