using System;
using System.Reactive;
using System.Threading.Tasks;
using SSBakery.Models;

namespace SSBakery.Services.Interfaces
{
    public interface IAuthService
    {
        IObservable<Unit> TriggerGoogleAuthFlow();

        IObservable<Unit> SignInWithFacebook();
    }
}