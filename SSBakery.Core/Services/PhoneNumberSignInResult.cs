using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using SSBakery.Services.Interfaces;
using Xamarin.Auth;

namespace SSBakery.Services
{
    public class PhoneNumberSignInResult : IPhoneNumberSignInResult
    {
        public PhoneNumberSignInResult(bool verified, string verificationId)
        {
            Verified = verified;
            VerificationId = verificationId;
        }

        public bool Verified { get; }

        public string VerificationId { get; }
    }
}