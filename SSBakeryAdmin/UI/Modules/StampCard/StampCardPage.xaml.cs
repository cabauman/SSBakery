using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakeryAdmin.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StampCardPage : ReactiveContentPage<IStampCardViewModel>
    {
        public StampCardPage()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(x => StampListView.ItemsSource = x.StampCells)
                .Take(1)
                .Subscribe();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.StampListView.SelectedItem)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.Save, v => v.SaveToolbarItem)
                        .DisposeWith(disposables);
                    this
                        .OneWayBind(ViewModel, vm => vm.RewardCount, v => v.RewardCountLabel.Text)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.IncrementRewardCount, v => v.IncrementRewardCountButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.DecrementRewardCount, v => v.DecrementRewardCountButton)
                        .DisposeWith(disposables);
                    //this
                    //    .WhenAnyValue(x => ViewModel.SelectedItem)
                    //    .Subscribe(_ => Device.BeginInvokeOnMainThread(() => StampListView.SelectedItem = null))
                    //    .DisposeWith(disposables);
                });
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            StampListView.ItemSize = width / 4;
        }
    }
}
