
namespace SSBakery.Services.Interfaces
{
    public interface IPhoneNumberVerificationResult
    {
        bool Authenticated { get; }

        string VerificationId { get; }
    }
}