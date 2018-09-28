using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseAuth.Mobile;
using ReactiveUI;
using GameCtor.RxNavigation;
using Splat;
using SSBakery.Core.Common;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class PhoneAuthPhoneNumberEntryViewModel : PageViewModel, IPhoneAuthPhoneNumberEntryViewModel
    {
        private const string PhoneNumberTest = "+1 653-555-4117";
        private const string VerificationCodeTest = "897604";

        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _phoneNumber;

        public PhoneAuthPhoneNumberEntryViewModel(
            AuthAction authAction,
            IObservable<Unit> whenVerified,
            IFirebaseAuthService firebaseAuthService = null,
            IViewStackService viewStackService = null)
                : base(viewStackService)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            var canExecute = this.WhenAnyValue(
                vm => vm.PhoneNumber,
                phoneNumber =>
                {
                    return !string.IsNullOrEmpty(phoneNumber);
                });

            VerifyPhoneNumber = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    if(authAction == AuthAction.SignIn)
                    {
                        return _firebaseAuthService
                            .SignInWithPhoneNumber(_phoneNumber)
                            .SelectMany(result => HandleResult(authAction, result, whenVerified));
                    }
                    else
                    {
                        return _firebaseAuthService
                            .LinkPhoneNumberToCurrentUser(_phoneNumber)
                            .SelectMany(result => HandleResult(authAction, result, whenVerified));
                    }
                },
                canExecute);

            VerifyPhoneNumber.ThrownExceptions
                .Subscribe(
                    ex =>
                    {
                        if(ex is FirebaseAuthException firebaseEx)
                        {
                            switch(firebaseEx.FirebaseAuthExceptionType)
                            {
                                case FirebaseAuthExceptionType.FirebaseAuth:
                                    Console.WriteLine(firebaseEx.Message);
                                    break;
                                case FirebaseAuthExceptionType.FirebaseAuthInvalidCredentials:
                                    Console.WriteLine(firebaseEx.Message);
                                    break;
                            }
                            Console.WriteLine(firebaseEx.Message);
                        }
                        else
                        {
                            Console.WriteLine(ex.Message);
                        }
                    });
        }

        public ReactiveCommand VerifyPhoneNumber { get; }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { this.RaiseAndSetIfChanged(ref _phoneNumber, value); }
        }

        private IObservable<Unit> HandleResult(AuthAction authAction, PhoneNumberVerificationResult result, IObservable<Unit> completionObservable)
        {
            if(result.Authenticated)
            {
                return completionObservable;
            }
            else
            {
                return ViewStackService.PushPage(new PhoneAuthVerificationCodeEntryViewModel(authAction, result.VerificationId, completionObservable));
            }
        }
    }
}
