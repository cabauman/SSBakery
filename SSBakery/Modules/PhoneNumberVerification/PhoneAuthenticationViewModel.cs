using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class PhoneAuthenticationViewModel : ViewModelBase, IPhoneAuthenticationViewModel
    {
        private const string PhoneNum = "+1 653-555-4117";
        //private const string VerificationCode = "897604";
        private const int REQUIRED_CODE_LENGTH = 6;

        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _verificationCode;

        public PhoneAuthenticationViewModel(
            PhoneNumberVerificationViewModel.AuthAction authAction,
            string verificationId,
            IObservable<Unit> completionObservable,
            IFirebaseAuthService firebaseAuthService = null,
            IScreen hostScreen = null)
                : base(hostScreen)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            var canExecute = this.WhenAnyValue(vm => vm.VerificationCode, code => code != null && code.Length == REQUIRED_CODE_LENGTH);
            VerifyCode = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    if(authAction == PhoneNumberVerificationViewModel.AuthAction.SignIn)
                    {
                        return _firebaseAuthService.SignInWithPhoneNumber(verificationId, VerificationCode)
                            .SelectMany(_ => completionObservable);
                    }
                    else
                    {
                        return _firebaseAuthService.LinkPhoneNumberWithCurrentUser(verificationId, VerificationCode)
                            .SelectMany(_ => completionObservable);
                    }
                },
                canExecute);
        }

        public ReactiveCommand VerifyCode { get; }

        public string VerificationCode
        {
            get { return _verificationCode; }
            set { this.RaiseAndSetIfChanged(ref _verificationCode, value); }
        }
    }
}
