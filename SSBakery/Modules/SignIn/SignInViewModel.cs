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
    public class SignInViewModel : ViewModelBase
    {
        private const string PhoneNum = "+1 653-555-4117";
        private const string VerificationCode = "897604";

        private readonly IFirebaseAuthService _firebaseAuthService;
        private readonly IRepoContainer _repositoryService;
        private IObservable<Unit> _signInSuccessful;
        private IObservable<Unit> _signInCanceled;
        private IObservable<AuthenticatorErrorEventArgs> _signInFailed;
        private string _provider;

        public SignInViewModel(IFirebaseAuthService firebaseAuthService = null, IRepoContainer repositoryService = null, IScreen hostScreen = null)
            : base(hostScreen)
        {
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();
            _repositoryService = repositoryService ?? Locator.Current.GetService<IRepoContainer>();

            ContinueAsGuest = ReactiveCommand.CreateFromObservable(() => SignInAnonymously());
            ContinueAsGuest
                .SelectMany(_ => GoToPage(new MainViewModel()))
                .Subscribe();

            ContinueAsGuest.ThrownExceptions.Subscribe(
                ex =>
                {
                    Console.WriteLine(ex);
                });

            SignInWithPhoneNumber = ReactiveCommand.CreateFromObservable(
                () =>
                {
                    return CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(PhoneNum)
                        .ToObservable()
                        .SelectMany(x => CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(x.VerificationId, VerificationCode));
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
                .SelectMany(x => GoToPage(new MainViewModel()))
                .Subscribe();

            SignInWithPhoneNumber
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(
                    x =>
                    {
                        Console.WriteLine(x.User.ToString());
                    },
                    ex =>
                    {
                        Console.WriteLine(ex.Message);
                    });
        }

        public ReactiveCommand<Unit, IAuthResultWrapper> SignInWithPhoneNumber { get; }

        public ReactiveCommand TriggerGoogleAuthFlow { get; }

        public ReactiveCommand TriggerFacebookAuthFlow { get; }

        public ReactiveCommand<Unit, Unit> ContinueAsGuest { get; }

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
                //.Do(_ => SetUser());

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

        private IObservable<Unit> SetUser()
        {
            var user = _repositoryService.UserRepo.Get("");
            return user
                .Where(x => x == null)
                .SelectMany(
                    x =>
                    {
                        var newUser = new SSBakeryUser();
                        return _repositoryService.UserRepo.Add(newUser)
                            .Select(_ => newUser);
                    })
                .Select(_ => Unit.Default);
        }

        private IObservable<Unit> SignInAnonymously()
        {
            return _firebaseAuthService
                .SignInAnonymously();
        }

        private IObservable<Unit> GoToPage(IRoutableViewModel routableViewModel)
        {
            return HostScreen
                .Router
                .Navigate
                .Execute(routableViewModel)
                .Select(_ => Unit.Default);
        }
    }
}
