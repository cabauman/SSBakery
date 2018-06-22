using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using SSBakery.UI.Common;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPageBase<SignInViewModel>
    {
        public SignInPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .BindCommand(ViewModel, vm => vm.ContinueAsGuest, v => v.ContinueAsGuestButton)
                        .DisposeWith(disposables);
                    //this
                    //    .BindCommand(ViewModel, vm => vm.TriggerGoogleAuthFlow, v => v.GoogleSignInButton)
                    //    .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.TriggerFacebookAuthFlow, v => v.FacebookSignInButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.SignInWithPhoneNumber, v => v.GoogleSignInButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
