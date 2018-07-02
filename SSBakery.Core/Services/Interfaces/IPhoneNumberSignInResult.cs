using System;
using System.Reactive;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakery.Services.Interfaces
{
    public interface IPhoneNumberSignInResult
    {
        bool Verified { get; }

        string VerificationId { get; }
    }
}