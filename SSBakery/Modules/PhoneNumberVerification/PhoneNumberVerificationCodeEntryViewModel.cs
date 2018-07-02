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
    public class PhoneNumberVerificationCodeEntryViewModel : ViewModelBase, IPhoneNumberVerificationCodeEntryViewModel
    {
        private const string PhoneNum = "+1 653-555-4117";
        //private const string VerificationCode = "897604";

        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _verificationCode;

        public PhoneNumberVerificationCodeEntryViewModel(
            string verificationId,
            IFirebaseAuthService firebaseAuthService = null,
            IScreen hostScreen = null)
                : base(hostScreen)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            var canExecute = this.WhenAnyValue(vm => vm.VerificationCode, code => !string.IsNullOrEmpty(code));
            VerifyCode = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return _firebaseAuthService.SignInWithPhoneNumber(verificationId, VerificationCode)
                        .SelectMany(_ => HostScreen.Router.NavigateAndReset.Execute(new MainViewModel()))
                        .Select(_ => Unit.Default);
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
