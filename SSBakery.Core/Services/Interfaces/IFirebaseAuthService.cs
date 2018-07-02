using System;
using System.Reactive;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakery.Services.Interfaces
{
    public interface IFirebaseAuthService
    {
        bool IsAuthenticated { get; }

        Task<string> GetFreshFirebaseToken();

        IObservable<Unit> SignInWithFacebook(string authToken);

        IObservable<Unit> SignInWithGoogle(string authToken);

        IObservable<IPhoneNumberSignInResult> SignInWithPhoneNumber(string phoneNumber);

        IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode);

        IObservable<Unit> SignInAnonymously();

        void SignOut();
    }
}