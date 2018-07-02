using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using GameCtor.Firebase.AuthWrapper;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.Config;
using SSBakery.Core.Common;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;
using Xamarin.Auth;

namespace SSBakery.UI.Modules
{
    public class PhoneNumberVerificationViewModel : ViewModelBase, IPhoneNumberVerificationViewModel
    {
        //private const string PhoneNum = "+1 653-555-4117";
        //private const string VerificationCode = "897604";

        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _phoneNumber;

        public PhoneNumberVerificationViewModel(IFirebaseAuthService firebaseAuthService = null, IScreen hostScreen = null)
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
                    return _firebaseAuthService.SignInWithPhoneNumber(_phoneNumber)
                        .Select(
                            x =>
                            {
                                return x.Verified ?
                                    HostScreen.Router.NavigateAndReset.Execute(new MainViewModel()) :
                                    HostScreen.Router.Navigate.Execute(new PhoneNumberVerificationCodeEntryViewModel(x.VerificationId));
                            })
                        .Switch()
                        .Select(x => Unit.Default);
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
    }
}
