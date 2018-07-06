using System;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IPhoneAuthenticationViewModel
    {
        ReactiveCommand VerifyCode { get; }

        string VerificationCode { get; }
    }
}
