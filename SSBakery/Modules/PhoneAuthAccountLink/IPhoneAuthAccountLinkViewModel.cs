using System;
using System.Reactive;
using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IPhoneAuthAccountLinkViewModel
    {
        ReactiveCommand<Unit, Unit> VerifyCode { get; }

        string VerificationCode { get; set; }
    }
}
