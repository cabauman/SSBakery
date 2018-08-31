using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RewardsProgramActivationPage : ContentPageBase<IRewardsProgramActivationViewModel>
    {
        public RewardsProgramActivationPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .BindCommand(ViewModel, vm => vm.NavigateToPhoneNumberVerificationPage, v => v.NavigateToPhoneNumberVerificationPageButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
