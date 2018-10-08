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
                .InvokeCommand(ViewModel, x => x.SyncWithPosSystem);

            this.WhenActivated(
                disposables =>
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.CustomersCells, v => v.RewardsMemberListView.ItemsSource)
                        .DisposeWith(disposables);

                    Observable.FromEventPattern<ItemDisappearingEventHandler, ItemDisappearingEventArgs>(
                        h => RewardsMemberListView.ItemDisappearing += h,
                        h => RewardsMemberListView.ItemDisappearing += h)
                            .Select(x => x.EventArgs.ItemData as IRewardsMemberCellViewModel)
                            .BindTo(ViewModel, vm => vm.CellDisappearing)
                            .DisposeWith(disposables);
                });
        }
    }
}
