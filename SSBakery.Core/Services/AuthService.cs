//using System;
//using System.Collections.Generic;
//using System.Reactive;
//using System.Reactive.Linq;
//using System.Reactive.Threading.Tasks;
//using SSBakery.Models;
//using SSBakery.Services.Interfaces;
//using TTKoreanSchool.Config;
//using Xamarin.Auth;

//namespace SSBakery.Services
//{
//    public class AuthService : IAuthService
//    {
//        private string _provider;

//        public WebRedirectAuthenticator Authenticator { get; private set; }

//        public IObservable<Unit> TriggerGoogleAuthFlow()
//        {
//            if(_provider == "Google")
//            {
//                return Authenticator;
//            }

//            _provider = "Google";

//            string clientId = GoogleAuthConfig.CLIENT_ID_ANDROID;
//            string redirectUrl = GoogleAuthConfig.REDIRECT_URL_ANDROID;
//            if(Xamarin.Forms.Device.RuntimePlatform == "iOS")
//            {
//                clientId = GoogleAuthConfig.CLIENT_ID_IOS;
//                redirectUrl = GoogleAuthConfig.REDIRECT_URL_IOS;
//            }

//            var authenticator = new OAuth2Authenticator(
//                clientId,
//                string.Empty,
//                GoogleAuthConfig.SCOPE,
//                new Uri(GoogleAuthConfig.AUTHORIZE_URL),
//                new Uri(redirectUrl),
//                new Uri(GoogleAuthConfig.ACCESS_TOKEN_URL),
//                null,
//                true);

//            Observe(authenticator);

//            return authenticator;
//        }

//        public IObservable<Unit> SignInWithFacebook()
//        {
//            throw new NotImplementedException();
//        }

//        private void Observe(WebRedirectAuthenticator authenticator)
//        {
//            Authenticator = authenticator;

//            var authCompleted = Observable.FromEventPattern<AuthenticatorCompletedEventArgs>(
//                x => authenticator.Completed += x,
//                x => authenticator.Completed -= x);

//            SignInSuccessful = authCompleted
//                .Where(x => x.EventArgs.IsAuthenticated)
//                .Select(x => x.EventArgs.Account)
//                .Select(authAccount => ConvertToBakeryUser(authAccount))
//                .SelectMany(bakeryUser => AuthenticateWithFirebase(bakeryUser));

//            SignInCanceled = authCompleted
//                .Where(x => !x.EventArgs.IsAuthenticated)
//                .Select(_ => Unit.Default);

//            SignInFailed = Observable.FromEventPattern<AuthenticatorErrorEventArgs>(
//                x => authenticator.Error += x,
//                x => authenticator.Error -= x)
//                    .Select(x => x.EventArgs);

//            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
//            presenter.Login(Authenticator);
//        }
//    }
//}