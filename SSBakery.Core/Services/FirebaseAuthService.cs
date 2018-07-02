using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using GameCtor.Firebase.AuthWrapper;
using Splat;
using SSBakery.Services.Interfaces;

namespace SSBakery.Services
{
    public class FirebaseAuthService : IFirebaseAuthService
    {
        public FirebaseAuthService()
        {
        }

        public bool IsAuthenticated
        {
            get { return CrossFirebaseAuth.Current.CurrentUser != null; }
        }

        public async Task<string> GetFreshFirebaseToken()
        {
            return await CrossFirebaseAuth.Current.CurrentUser.GetIdTokenAsync(false);
        }

        public IObservable<Unit> SignInWithFacebook(string authToken)
        {
            return CrossFirebaseAuth.Current.SignInWithFacebookAsync(authToken)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInWithGoogle(string authToken)
        {
            return CrossFirebaseAuth.Current.SignInWithGoogleAsync(null, authToken)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<IPhoneNumberSignInResult> SignInWithPhoneNumber(string phoneNumber)
        {
            return CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(phoneNumber)
                .ToObservable()
                .Select(
                    x =>
                    {
                        return new PhoneNumberSignInResult(x.AuthResult != null, x.VerificationId);
                    });

                //.SelectMany(x => CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(x.VerificationId, VerificationCode))
                //.SelectMany(x => GoToPage(new MainViewModel()));
        }

        public IObservable<Unit> SignInWithPhoneNumber(string verificationId, string verificationCode)
        {
            return CrossFirebaseAuth.Current.SignInWithPhoneNumberAsync(verificationId, verificationCode)
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public IObservable<Unit> SignInAnonymously()
        {
            return CrossFirebaseAuth.Current.SignInAnonymouslyAsync()
                .ToObservable()
                .Select(_ => Unit.Default);
        }

        public void SignOut()
        {
            CrossFirebaseAuth.Current.SignOut();
        }
    }
}