using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using SSBakery.Config;
using SSBakery.Core.Common;
using SSBakery.Services.Interfaces;
using Xamarin.Auth;

namespace SSBakery.Services
{
    public class AuthService : IAuthService
    {
        private Subject<Unit> _authFlowTriggered;

        public AuthService()
        {
            _authFlowTriggered = new Subject<Unit>();

            var authCompleted = _authFlowTriggered
                .Select(
                    _ =>
                    {
                        return Observable.FromEventPattern<AuthenticatorCompletedEventArgs>(
                            x => AuthenticationState.Authenticator.Completed += x,
                            x => AuthenticationState.Authenticator.Completed -= x);
                    })
                .Switch();

            SignInCanceled = authCompleted
                .Where(x => !x.EventArgs.IsAuthenticated)
                .Select(_ => Unit.Default);

            SignInSuccessful = authCompleted
                .Where(x => x.EventArgs.IsAuthenticated)
                .Select(x => ExtractAuthToken(x.EventArgs.Account));

            SignInFailed = _authFlowTriggered
                .Select(
                    _ =>
                    {
                        return Observable.FromEventPattern<AuthenticatorErrorEventArgs>(
                            x => AuthenticationState.Authenticator.Error += x,
                            x => AuthenticationState.Authenticator.Error -= x)
                                .Select(x => x.EventArgs.Exception);
                    })
                .Switch();
        }

        public IObservable<string> SignInSuccessful { get; private set; }

        public IObservable<Unit> SignInCanceled { get; private set; }

        public IObservable<Exception> SignInFailed { get; private set; }

        public void TriggerGoogleAuthFlow()
        {
            string clientId = GoogleAuthConfig.CLIENT_ID_ANDROID;
            string redirectUrl = GoogleAuthConfig.REDIRECT_URL_ANDROID;
            //if(CrossDeviceInfo Xamarin.Forms.Device.RuntimePlatform == "iOS")
            //{
            //    clientId = GoogleAuthConfig.CLIENT_ID_IOS;
            //    redirectUrl = GoogleAuthConfig.REDIRECT_URL_IOS;
            //}

            AuthenticationState.Authenticator = new OAuth2Authenticator(
                clientId,
                string.Empty,
                GoogleAuthConfig.SCOPE,
                new Uri(GoogleAuthConfig.AUTHORIZE_URL),
                new Uri(redirectUrl),
                new Uri(GoogleAuthConfig.ACCESS_TOKEN_URL),
                null,
                true);

            _authFlowTriggered.OnNext(Unit.Default);
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(AuthenticationState.Authenticator);
        }

        public void TriggerFacebookAuthFlow()
        {
                AuthenticationState.Authenticator = new OAuth2Authenticator(
                    FacebookAuthConfig.CLIENT_ID,
                    FacebookAuthConfig.SCOPE,
                    new Uri(FacebookAuthConfig.AUTHORIZE_URL),
                    new Uri(FacebookAuthConfig.REDIRECT_URL),
                    null,
                    true);

            _authFlowTriggered.OnNext(Unit.Default);
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(AuthenticationState.Authenticator);
        }

        private string ExtractAuthToken(Xamarin.Auth.Account account)
        {
            string authToken = account.Properties["access_token"];
            if(AuthenticationState.Authenticator.GetType() == typeof(OAuth1Authenticator))
            {
                authToken = account.Properties["oauth_token"];
            }

            return authToken;
        }
    }
}
