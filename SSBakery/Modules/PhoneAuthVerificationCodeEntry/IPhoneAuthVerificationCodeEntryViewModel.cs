using System;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IPhoneAuthVerificationCodeEntryViewModel
    {
        ReactiveCommand<Unit, Unit> VerifyCode { get; }

        string VerificationCode { get; set; }
    }
}
