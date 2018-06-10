using System;
using System.Reactive;
using ReactiveUI;
using Xamarin.Auth;

namespace SSBakery.UI.Modules.SignIn
{
    public interface ISignInViewModel
    {
        ReactiveCommand<Unit, WebRedirectAuthenticator> TriggerGoogleAuthFlow { get; }

        ReactiveCommand<Unit, WebRedirectAuthenticator> TriggerFacebookAuthFlow { get; }

        ReactiveCommand<Unit, Unit> ContinueAsGuest { get; }

        IObservable<Unit> SignInSuccessful { get; }

        IObservable<Unit> SignInCanceled { get; }

        IObservable<AuthenticatorErrorEventArgs> SignInFailed { get; }

        WebRedirectAuthenticator Authenticator { get; }
    }
}
