using System;
using System.Reactive;
using ReactiveUI;
using Xamarin.Auth;

namespace SSBakery.UI.Modules
{
    public interface IPhoneNumberVerificationViewModel
    {
        ReactiveCommand<Unit, Unit> VerifyPhoneNumber { get; }

        ReactiveCommand<Unit, Unit> ConfirmVerificationCode { get; }
    }
}
