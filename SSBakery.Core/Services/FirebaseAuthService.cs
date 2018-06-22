using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Firebase.Auth;
using GameCtor.Firebase.AuthWrapper;
using Newtonsoft.Json;
using Splat;
using SSBakery.Models;
using SSBakery.Services.Interfaces;
using Xamarin.Auth;

namespace SSBakery.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        private readonly FirebaseAuthProvider _authProvider;
        private readonly ICredentialsService _credentialsService;

        private FirebaseAuthLink _authLink;

        public FirebaseAuthService(ICredentialsService credentialsService = null)
        {
            _credentialsService = credentialsService ?? Locator.Current.GetService<ICredentialsService>();
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig(Config.ApiKeys.FIREBASE));

            if(_credentialsService.CredentialsExist)
            {
                var auth = JsonConvert.DeserializeObject<FirebaseAuth>(_credentialsService.FirebaseAuthJson);
                _authLink = new FirebaseAuthLink(_authProvider, auth);
            }

            FirebaseAuthRefreshed
                .Select(firebaseAuth => JsonConvert.SerializeObject(firebaseAuth))
                .Subscribe(json => _credentialsService.FirebaseAuthJson = json);
        }

        public IObservable<FirebaseAuth> FirebaseAuthRefreshed
        {
            get
            {
                return Observable
                    .FromEventPattern<FirebaseAuthEventArgs>(
                        x => _authLink.FirebaseAuthRefreshed += x,
                        x => _authLink.FirebaseAuthRefreshed -= x)
                    .Select(x => x.EventArgs.FirebaseAuth);
            }
        }

        public bool IsAuthenticated
        {
            get { return _authLink != null; }
        }

        public async Task<string> GetFreshFirebaseToken()
        {
            return (await _authLink.GetFreshAuthAsync()).FirebaseToken;
        }

        public IObservable<Unit> SignInWithFacebook(string authToken)
        {
            return SignInWithOAuth(FirebaseAuthType.Facebook, authToken);
        }

        //public IObservable<Unit> SignInWithGoogle(string authToken)
        //{
        //    return SignInWithOAuth(FirebaseAuthType.Google, authToken);
        //}

        public IObservable<Unit> SignInWithGoogle(string authToken)
        {
            return CrossFirebaseAuth.Current.SignInWithGoogleAsync(null, authToken)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInAnonymously()
        {
            return _authProvider
                .SignInAnonymouslyAsync()
                .ToObservable()
                .SelectMany(authLink => SetCredentials("Anonymous", string.Empty, authLink));
        }

        public void SignOut()
        {
            _authLink = null;
            _credentialsService.Delete();
        }

        private IObservable<Unit> SignInWithOAuth(FirebaseAuthType authType, string authToken)
        {
            return _authProvider
                .SignInWithOAuthAsync(authType, authToken)
                .ToObservable()
                .SelectMany(authLink => SetCredentials(authType.ToString(), authToken, authLink));
        }

        private IObservable<Unit> SetCredentials(string provider, string authToken, FirebaseAuthLink firebaseAuthLink)
        {
            _authLink = firebaseAuthLink;
            string firebaseAuthJson = JsonConvert.SerializeObject(firebaseAuthLink);

            return _credentialsService
                .Set(provider, authToken, firebaseAuthJson);
        }
    }
}