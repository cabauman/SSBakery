using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.UI.Modules
{
    public interface ISignInViewModel
    {
        ReactiveCommand TriggerGoogleAuthFlow { get; }

        ReactiveCommand TriggerFacebookAuthFlow { get; }
    }
}
