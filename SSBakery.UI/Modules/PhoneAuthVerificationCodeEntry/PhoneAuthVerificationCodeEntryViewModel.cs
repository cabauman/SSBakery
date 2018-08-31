using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using ReactiveUI;
using RxNavigation;
using Splat;
using SSBakery.Core.Common;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class PhoneAuthVerificationCodeEntryViewModel : PageViewModel, IPhoneAuthVerificationCodeEntryViewModel
    {
        private const string PhoneNumberTest = "+1 653-555-4117";
        private const string VerificationCodeTest = "897604";
        private const int REQUIRED_CODE_LENGTH = 6;

        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _verificationCode;

        public PhoneAuthVerificationCodeEntryViewModel(
            AuthAction authAction,
            string verificationId,
            IObservable<Unit> whenVerified,
            IFirebaseAuthService firebaseAuthService = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            var canExecute = this.WhenAnyValue(vm => vm.VerificationCode, code => code != null && code.Length == REQUIRED_CODE_LENGTH);
            VerifyCode = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    if(authAction == AuthAction.SignIn)
                    {
                        return _firebaseAuthService
                            .SignInWithPhoneNumber(verificationId, VerificationCode)
                            .SelectMany(_ => whenVerified);
                    }
                    else
                    {
                        return _firebaseAuthService
                            .LinkPhoneNumberToCurrentUser(verificationId, VerificationCode)
                            .ObserveOn(RxApp.MainThreadScheduler)
                            .SelectMany(_ => whenVerified);
                    }
                },
                canExecute);

            VerifyCode.ThrownExceptions
                .Subscribe(
                    ex =>
                    {
                        Console.WriteLine(ex.Message);
                    });
        }

        public ReactiveCommand<Unit, Unit> VerifyCode { get; }

        public string VerificationCode
        {
            get { return _verificationCode; }
            set { this.RaiseAndSetIfChanged(ref _verificationCode, value); }
        }
    }
}
