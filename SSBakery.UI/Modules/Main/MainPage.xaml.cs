using System;
using System.Reactive.Disposables;
using ReactiveUI;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPageBase<IMainViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    //CatalogButton.Clicked += (s, e) => throw new ArgumentNullException();
                    //return;

                    this
                        .BindCommand(ViewModel, vm => vm.NavigateToCatalogPage, v => v.CatalogButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.NavigateToAlbumPage, v => v.AlbumButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.NavigateToRewardsPage, v => v.RewardsButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.NavigateToStoreInfoPage, v => v.StoreInfoButton)
                        .DisposeWith(disposables);
                });
        }
    }
}
