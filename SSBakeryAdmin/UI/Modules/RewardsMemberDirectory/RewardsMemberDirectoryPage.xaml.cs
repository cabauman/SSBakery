using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using SSBakeryAdmin.UI.Common;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakeryAdmin.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RewardsMemberDirectoryPage : ContentPageBase<IRewardsMemberDirectoryViewModel>
    {
        public RewardsMemberDirectoryPage()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(_ => Unit.Default)
                .Take(1)
                .InvokeCommand(this, x => x.ViewModel.LoadRewardsMembers);

            this
                .WhenAnyValue(x => x.ViewModel.MemberCells)
                .Where(x => x != null)
                .Do(members => RewardsMemberListView.ItemsSource = members)
                .Take(1)
                .Subscribe();

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.RewardsMemberListView.SelectedItem)
                        .DisposeWith(disposables);
                });
        }
    }
}
