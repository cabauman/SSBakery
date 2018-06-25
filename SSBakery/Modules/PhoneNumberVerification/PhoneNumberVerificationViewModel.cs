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
        private const string PhoneNum = "+1 653-555-4117";
        private const string VerificationCode = "897604";

        private readonly IFirebaseAuthService _firebaseAuthService;

        public PhoneNumberVerificationViewModel(IFirebaseAuthService firebaseAuthService = null, IScreen hostScreen = null)
            : base(hostScreen)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            //VerifyPhoneNumber = ReactiveCommand.CreateFromObservable(
            //    () =>
            //    {
            //        return CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(PhoneNum)
            //            .ToObservable()
            //            .SelectMany(x => CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(x.VerificationId, VerificationCode));
            //    });
        }

        public ReactiveCommand<Unit, Unit> VerifyPhoneNumber { get; }

        public ReactiveCommand<Unit, Unit> ConfirmVerificationCode { get; }
    }
}
