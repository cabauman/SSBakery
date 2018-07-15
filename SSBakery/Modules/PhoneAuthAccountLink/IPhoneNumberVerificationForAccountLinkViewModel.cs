using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IPhoneNumberVerificationForAccountLinkViewModel
    {
        ReactiveCommand VerifyPhoneNumber { get; }

        string PhoneNumber { get; set; }
    }
}
