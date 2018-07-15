using System;
using System.Reactive;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakery.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        bool IsAuthenticated { get; }

        bool IsPhoneNumberLinkedToAccount { get; }

        Task<string> GetFreshFirebaseToken();

        IObservable<Unit> SignInWithFacebook(string authToken);

        IObservable<Unit> SignInWithGoogle(string authToken);

        IObservable<IPhoneNumberVerificationResult> SignInWithPhoneNumber(string phoneNumber);

        IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode);

        IObservable<Unit> SignInAnonymously();

        IObservable<IPhoneNumberVerificationResult> LinkPhoneNumberToCurrentUser(string phoneNumber);

        IObservable<Unit> LinkPhoneNumberToCurrentUser(string verificationId, string verificationCode);

        void SignOut();
    }
}