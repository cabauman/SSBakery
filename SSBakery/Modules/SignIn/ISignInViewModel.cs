using System;
using System.Reactive;
using ReactiveUI;
using Xamarin.Auth;

namespace SSBakery.UI.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand TriggerGoogleAuthFlow { get; }

        ReactiveCommand TriggerFacebookAuthFlow { get; }

        ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }

        ReactiveCommand ContinueAsGuest { get; }

        IObservable<Unit> SignInSuccessful { get; }

        IObservable<Unit> SignInCanceled { get; }

        IObservable<AuthenticatorErrorEventArgs> SignInFailed { get; }

        WebRedirectAuthenticator Authenticator { get; }
    }
}
