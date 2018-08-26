using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.iOS.Modules.SignIn
{
    public interface ISignInViewModel
    {
        ReactiveCommand<Unit, Unit> TriggerGoogleAuthFlow { get; }
    }
}