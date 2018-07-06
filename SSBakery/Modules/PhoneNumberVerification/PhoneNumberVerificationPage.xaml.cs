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
    public partial class PhoneNumberVerificationPage : ContentPageBase<IPhoneNumberVerificationViewModel>
    {
        public PhoneNumberVerificationPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .Bind(ViewModel, vm => vm.PhoneNumber, v => v.PhoneNumberEntry.Text)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.VerifyPhoneNumber, v => v.PhoneNumberVerificationButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
