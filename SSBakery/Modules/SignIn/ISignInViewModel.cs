using System;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand TriggerGoogleAuthFlow { get; }

        ReactiveCommand TriggerFacebookAuthFlow { get; }

        ReactiveCommand NavigateToPhoneNumberVerificationPage { get; }

        ReactiveCommand ContinueAsGuest { get; }
    }
}
