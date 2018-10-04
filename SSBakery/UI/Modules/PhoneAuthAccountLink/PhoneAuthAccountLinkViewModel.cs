using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class PhoneAuthAccountLinkViewModel : PageViewModel, IPhoneAuthAccountLinkViewModel
    {
        private const string PhoneNumberTest = "+1 653-555-4117";
        private const string VerificationCodeTest = "897604";
        private const int REQUIRED_CODE_LENGTH = 6;

        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _verificationCode;

        public PhoneAuthAccountLinkViewModel(
            PhoneNumberVerificationForAccountLinkViewModel.AuthAction authAction,
            string verificationId,
            IObservable<Unit> completionObservable,
            IFirebaseAuthService firebaseAuthService = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            var canExecute = this.WhenAnyValue(vm => vm.VerificationCode, code => code != null && code.Length == REQUIRED_CODE_LENGTH);
            VerifyCode = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    if(authAction == PhoneNumberVerificationForAccountLinkViewModel.AuthAction.SignIn)
                    {
                        return _firebaseAuthService.SignInWithPhoneNumber(verificationId, VerificationCode)
                            .SelectMany(_ => completionObservable);
                    }
                    else
                    {
                        return _firebaseAuthService.LinkPhoneNumberToCurrentUser(verificationId, VerificationCode)
                            .SelectMany(_ => completionObservable);
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
