using ReactiveUI;

namespace SSBakery.UI.Modules
{
    public interface IPhoneAuthPhoneNumberEntryViewModel
    {
        ReactiveCommand VerifyPhoneNumber { get; }

        string PhoneNumber { get; set; }
    }
}
