using System;
using System.Reactive.Disposables;
using ReactiveUI;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPageBase<MainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    this
                        .BindCommand(ViewModel, vm => vm.GoToCatalogPage, v => v.CatalogButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.GoToAlbumPage, v => v.AlbumButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.GoToRewardsPage, v => v.RewardsButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.GoToStoreInfoPage, v => v.StoreInfoButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
