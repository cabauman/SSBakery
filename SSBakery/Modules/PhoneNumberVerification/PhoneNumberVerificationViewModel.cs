using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.Firebase.AuthWrapper;
using ReactiveUI;
using Splat;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class PhoneNumberVerificationViewModel : ViewModelBase, IPhoneNumberVerificationViewModel
    {
        //private const string PhoneNum = "+1 653-555-4117";
        //private const string VerificationCode = "897604";

        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _phoneNumber;

        public PhoneNumberVerificationViewModel(
            AuthAction authAction,
            IObservable<Unit> completionObservable,
            IFirebaseAuthService firebaseAuthService = null,
            IScreen hostScreen = null)
                : base(hostScreen)
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
                        return _firebaseAuthService.SignInWithPhoneNumber(_phoneNumber)
                            .SelectMany(result => HandleResult(authAction, result, completionObservable));
                    }
                    else
                    {
                        return _firebaseAuthService.LinkPhoneNumberWithCurrentUser(_phoneNumber)
                            .SelectMany(result => HandleResult(authAction, result, completionObservable));
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

        public enum AuthAction
        {
            SignIn,
            LinkAccount,
        }

        public ReactiveCommand VerifyPhoneNumber { get; }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { this.RaiseAndSetIfChanged(ref _phoneNumber, value); }
        }

        private IObservable<Unit> HandleResult(AuthAction authAction, IPhoneNumberVerificationResult result, IObservable<Unit> completionObservable)
        {
            if(result.Authenticated)
            {
                return completionObservable;
            }
            else
            {
                return Navigate(new PhoneAuthenticationViewModel(authAction, result.VerificationId, completionObservable));
            }
        }
    }
}
