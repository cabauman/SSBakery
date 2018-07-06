using System;
using SSBakery.Services.Interfaces;

namespace SSBakery.Services
{
    public class PhoneNumberVerificationResult : IPhoneNumberVerificationResult
    {
        public PhoneNumberVerificationResult(bool authenticated, string verificationId)
        {
            Authenticated = authenticated;
            VerificationId = verificationId;
        }

        public bool Authenticated { get; }

        public string VerificationId { get; }
    }
}