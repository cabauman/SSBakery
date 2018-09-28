using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using GameCtor.FirebaseAuth;
using GameCtor.XamarinAuth;
using ReactiveUI;
using GameCtor.RxNavigation;
using Splat;
using SSBakeryAdmin.iOS.Modules.Home;
using SSBakeryAdmin.Services.Interfaces;

namespace SSBakeryAdmin.iOS.Modules.SignIn
{
    public class SignInViewModel : ISignInViewModel, IPageViewModel, IEnableLogger
    {
        private IFirebaseAuthService _firebaseAuthService;

        public SignInViewModel(IAuthService authService = null, IFirebaseAuthService firebaseAuthService = null, IViewStackService viewStackService = null)
        {
            authService = authService ?? Locator.Current.GetService<IAuthService>();
            _firebaseAuthService = firebaseAuthService ?? Locator.Current.GetService<IFirebaseAuthService>();
            viewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();

            TriggerGoogleAuthFlow = ReactiveCommand.Create(
                () =>
                {
                    authService.TriggerGoogleAuthFlow();
                });

            authService.SignInSuccessful
                .SelectMany(authProperties => AuthenticateWithFirebase(authProperties))
                .SelectMany(_ => viewStackService.PushPage(new HomeViewModel(), null, true))
                .Subscribe();

            authService.SignInCanceled
                .Subscribe(
                    x =>
                    {
                        this.Log().Debug("Sign in canceled");
                    });

            authService.SignInFailed
                .Subscribe(
                    x =>
                    {
                        this.Log().Debug("Sign in failed");
                    });
        }

        public ReactiveCommand<Unit, Unit> TriggerGoogleAuthFlow { get; }

        public string Title => "Sign In";

        private IObservable<Unit> AuthenticateWithFirebase(string accessToken/*IDictionary<string, string> authProperties*/)
        {
            //string accessToken = authProperties["access_token"];
            //string idToken = authProperties["id_token"];

            return _firebaseAuthService
                .SignInWithGoogle(/*idToken, */accessToken);
        }
    }
}