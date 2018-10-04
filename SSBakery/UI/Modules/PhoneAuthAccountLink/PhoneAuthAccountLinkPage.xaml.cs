using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhoneAuthenticationPage : ContentPageBase<IPhoneAuthAccountLinkViewModel>
    {
        public PhoneAuthenticationPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .Bind(ViewModel, vm => vm.VerificationCode, v => v.VerificationCodeEntry.Text)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.VerifyCode, v => v.VerifyCodeButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
