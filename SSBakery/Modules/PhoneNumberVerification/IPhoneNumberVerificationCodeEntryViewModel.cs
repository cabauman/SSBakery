using System;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IPhoneNumberVerificationCodeEntryViewModel
    {
        ReactiveCommand VerifyCode { get; }

        string VerificationCode { get; }
    }
}
