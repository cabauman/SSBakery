using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using SSBakery.Config;
using SSBakery.Core.Common;
using SSBakery.Services.Interfaces;
using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;
using Xamarin.Auth;

namespace SSBakery.UI.Modules
{
    public class SignInViewModel : PageViewModel, ISignInViewModel
    {
        private const string PhoneNum = "+1 653-555-4117";
        private const string VerificationCode = "897604";

        private readonly IFirebaseAuthService _firebaseAuthService;
        private IObservable<Unit> _signInSuccessful;
        private IObservable<Unit> _signInCanceled;
        private IObservable<AuthenticatorErrorEventArgs> _signInFailed;
        private string _provider;

        public SignInViewModel(IFirebaseAuthService firebaseAuthService = null, IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();

            ContinueAsGuest = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return SignInAnonymously()
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
                    if(_provider != "Google")
                    {
                        _provider = "Google";

                        string clientId = GoogleAuthConfig.CLIENT_ID_ANDROID;
                        string redirectUrl = GoogleAuthConfig.REDIRECT_URL_ANDROID;
                        if(Xamarin.Forms.Device.RuntimePlatform == "iOS")
                        {
                            clientId = GoogleAuthConfig.CLIENT_ID_IOS;
                            redirectUrl = GoogleAuthConfig.REDIRECT_URL_IOS;
                        }

                        Authenticator = new OAuth2Authenticator(
                            clientId,
                            string.Empty,
                            GoogleAuthConfig.SCOPE,
                            new Uri(GoogleAuthConfig.AUTHORIZE_URL),
                            new Uri(redirectUrl),
                            new Uri(GoogleAuthConfig.ACCESS_TOKEN_URL),
                            null,
                            true);

                        Observe(Authenticator);
                    }

                    var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(Authenticator);
                });

            TriggerGoogleAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    Console.WriteLine(ex);
                });

            TriggerFacebookAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    if(_provider != "Facebook")
                    {
                        _provider = "Facebook";

                        Authenticator = new OAuth2Authenticator(
                            FacebookAuthConfig.CLIENT_ID,
                            FacebookAuthConfig.SCOPE,
                            new Uri(FacebookAuthConfig.AUTHORIZE_URL),
                            new Uri(FacebookAuthConfig.REDIRECT_URL),
                            null,
                            true);

                        Observe(Authenticator);
                    }

                    var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(Authenticator);
                });

            TriggerFacebookAuthFlow.ThrownExceptions.Subscribe(
                ex =>
                {
                    Console.WriteLine(ex);
                });

            this.WhenAnyObservable(x => x.SignInSuccessful)
                .SelectMany(x => ViewStackService.PushPage(new MainViewModel(), null, true))
                .Subscribe();
        }

        public ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }

        public ReactiveCommand TriggerGoogleAuthFlow { get; }

        public ReactiveCommand TriggerFacebookAuthFlow { get; }

        public ReactiveCommand ContinueAsGuest { get; }

        public IObservable<Unit> SignInSuccessful
        {
            get { return _signInSuccessful; }
            private set { this.RaiseAndSetIfChanged(ref _signInSuccessful, value); }
        }

        public IObservable<Unit> SignInCanceled
        {
            get { return _signInCanceled; }
            private set { this.RaiseAndSetIfChanged(ref _signInCanceled, value); }
        }

        public IObservable<AuthenticatorErrorEventArgs> SignInFailed
        {
            get { return _signInFailed; }
            private set { this.RaiseAndSetIfChanged(ref _signInFailed, value); }
        }

        public WebRedirectAuthenticator Authenticator { get; set; }

        private void Observe(WebRedirectAuthenticator authenticator)
        {
            AuthenticationState.Authenticator = authenticator;

            var authCompleted = Observable.FromEventPattern<AuthenticatorCompletedEventArgs>(
                x => authenticator.Completed += x,
                x => authenticator.Completed -= x);

            SignInSuccessful = authCompleted
                .Where(x => x.EventArgs.IsAuthenticated)
                .Select(x => ExtractAuthToken(x.EventArgs.Account))
                .SelectMany(authToken => AuthenticateWithFirebase(authToken));

            SignInCanceled = authCompleted
                .Where(x => !x.EventArgs.IsAuthenticated)
                .Select(_ => Unit.Default);

            SignInFailed = Observable.FromEventPattern<AuthenticatorErrorEventArgs>(
                x => authenticator.Error += x,
                x => authenticator.Error -= x)
                    .Select(x => x.EventArgs);
        }

        private string ExtractAuthToken(Xamarin.Auth.Account account)
        {
            string authToken = account.Properties["access_token"];
            if(Authenticator.GetType() == typeof(OAuth1Authenticator))
            {
                authToken = account.Properties["oauth_token"];
            }

            return authToken;
        }

        private IObservable<Unit> AuthenticateWithFirebase(string authToken)
        {
            IObservable<Unit> result = null;
            if(_provider == "Google")
            {
                result = _firebaseAuthService
                    .SignInWithGoogle(authToken);
            }
            else if(_provider == "Facebook")
            {
                result = _firebaseAuthService
                    .SignInWithFacebook(authToken);
            }

            return result;
        }

        private IObservable<Unit> SignInAnonymously()
        {
            return _firebaseAuthService
                .SignInAnonymously();
        }
    }
}
