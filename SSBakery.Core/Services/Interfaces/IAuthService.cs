using System;
using System.Reactive;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakery.Services.Interfaces
{
    public interface IAuthService
    {
        IObservable<string> SignInSuccessful { get; }

        IObservable<Unit> SignInCanceled { get; }

        IObservable<Exception> SignInFailed { get; }

        void TriggerGoogleAuthFlow();

        void TriggerFacebookAuthFlow();
    }
}