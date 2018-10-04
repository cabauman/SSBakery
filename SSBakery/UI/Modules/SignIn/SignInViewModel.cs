using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using GameCtor.RxNavigation;
using GameCtor.XamarinAuth;
using ReactiveUI;
using Splat;
using SSBakery.Core.Common;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class SignInViewModel : PageViewModel, ISignInViewModel
    {
        private readonly IFirebaseAuthService _firebaseAuthService;
        private string _provider;

        public SignInViewModel(IAuthService authService = null, IFirebaseAuthService firebaseAuthService = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();
            authService = authService ?? Locator.Current.GetService<IAuthService>();

            ContinueAsGuest = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return _firebaseAuthService
                        .SignInAnonymously()
                        .SelectMany(_ => ViewStackService.PushPage(new MainViewModel()));
                });

            ContinueAsGuest.ThrownExceptions.Subscribe(
                ex =>
                {
                    Console.WriteLine(ex);
                });

            NavigateToPhoneNumberVerificationPage = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    IObservable<Unit> whenSignedIn = Observable
                        .Defer(
                            () =>
                            {
                                return ViewStackService
                                    .PushPage(new MainViewModel(), null, true);
                            });

                    return ViewStackService
                        .PushPage(new PhoneAuthPhoneNumberEntryViewModel(AuthAction.SignIn, whenSignedIn));
                });

            TriggerGoogleAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    _provider = "google";
                    authService.TriggerGoogleAuthFlow(
                        Config.GoogleAuthConfig.CLIENT_ID_ANDROID,
                        null,
                        Config.GoogleAuthConfig.SCOPE,
                        Config.GoogleAuthConfig.AUTHORIZE_URL,
                        Config.GoogleAuthConfig.REDIRECT_URL_ANDROID,
                        Config.GoogleAuthConfig.ACCESS_TOKEN_URL);
                });

            TriggerGoogleAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    this.Log().Debug(ex);
                });

            TriggerFacebookAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    _provider = "facebook";
                    authService.TriggerFacebookAuthFlow(
                        Config.FacebookAuthConfig.CLIENT_ID,
                        null,
                        Config.FacebookAuthConfig.SCOPE,
                        Config.FacebookAuthConfig.AUTHORIZE_URL,
                        Config.FacebookAuthConfig.REDIRECT_URL,
                        string.Empty);
                });

            TriggerFacebookAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    this.Log().Debug(ex);
                });

            authService.SignInSuccessful
                .SelectMany(authToken => AuthenticateWithFirebase(authToken))
                .SelectMany(_ => ViewStackService.PushPage(new MainViewModel(), null, true))
                .Subscribe();

            authService.SignInCanceled
                .Subscribe(
                    x =>
                    {
                        this.Log().Debug("");
                    },
                    ex => this.Log().Debug(""),
                    () =>
                    {
                        this.Log().Debug("");
                    });

            authService.SignInFailed
                .Subscribe(
                    x =>
                    {
                        this.Log().Debug("");
                    });
        }

        public ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }

        public ReactiveCommand TriggerGoogleAuthFlow { get; }

        public ReactiveCommand TriggerFacebookAuthFlow { get; }

        public ReactiveCommand ContinueAsGuest { get; }

        private IObservable<Unit> AuthenticateWithFirebase(string authToken)
        {
            IObservable<Unit> result = null;
            if(_provider == "google")
            {
                //result = _firebaseAuthService.CurrentUser
                //    .LinkWithGoogle(null, authToken).Select(_ => Unit.Default);
                result = _firebaseAuthService
                    .SignInWithGoogle(null, authToken);
            }
            else if(_provider == "facebook")
            {
                result = _firebaseAuthService
                    .SignInWithFacebook(authToken);
            }

            return result;
        }
    }
}
