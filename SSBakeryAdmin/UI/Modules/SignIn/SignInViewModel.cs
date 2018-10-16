using System;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using GameCtor.XamarinAuth;
using ReactiveUI;
using Splat;
using SSBakeryAdmin.UI.Common;

namespace SSBakeryAdmin.UI.Modules
{
    public class SignInViewModel : PageViewModel, ISignInViewModel
    {
        private readonly IFirebaseAuthService _firebaseAuthService;

        private string _provider;

        public SignInViewModel(
            AppBootstrapper appBootstrapper,
            IAuthService authService = null,
            IFirebaseAuthService firebaseAuthService = null)
                : base(null)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();
            authService = authService ?? Locator.Current.GetService<IAuthService>();

            TriggerGoogleAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    _provider = "google";
                    authService.TriggerGoogleAuthFlow(
                        Config.GoogleAuthConfig.CLIENT_ID,
                        null,
                        Config.GoogleAuthConfig.SCOPE,
                        Config.GoogleAuthConfig.AUTHORIZE_URL,
                        Config.GoogleAuthConfig.REDIRECT_URL,
                        Config.GoogleAuthConfig.ACCESS_TOKEN_URL);
                });

            TriggerGoogleAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    this.Log().Debug(ex);
                });

            authService.SignInSuccessful
                .SelectMany(authToken => AuthenticateWithFirebase(authToken))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => appBootstrapper.MainView = new MainViewModel(appBootstrapper));
        }

        public ReactiveCommand TriggerGoogleAuthFlow { get; }

        public ReactiveCommand TriggerFacebookAuthFlow { get; }

        private IObservable<Unit> AuthenticateWithFirebase(string authToken)
        {
            IObservable<Unit> result = null;
            if (_provider == "google")
            {
                result = _firebaseAuthService
                    .SignInWithGoogle(null, authToken);
            }
            else if (_provider == "facebook")
            {
                result = _firebaseAuthService
                    .SignInWithFacebook(authToken);
            }

            return result;
        }
    }
}
