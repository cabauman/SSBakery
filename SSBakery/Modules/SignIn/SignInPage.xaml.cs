using System;
using System.Reactive.Disposables;
using ReactiveUI;
using SSBakery.UI.Common;
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
                        .BindCommand(ViewModel, vm => vm.GoToMainPage, v => v.ContinueAnonymouslyButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
