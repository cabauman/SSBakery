using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IPhoneNumberVerificationViewModel
    {
        ReactiveCommand VerifyPhoneNumber { get; }

        string PhoneNumber { get; }
    }
}
