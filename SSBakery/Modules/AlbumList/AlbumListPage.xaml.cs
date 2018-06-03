using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using FFImageLoading;
using ReactiveUI;
using SSBakery.Core.Services;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumListPage : ContentPageBase<IAlbumListViewModel>
    {
        public AlbumListPage()
        {
            InitializeComponent();

            return;

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .OneWayBind(ViewModel, vm => vm.Albums, v => v.Albums.ItemsSource)
                        .DisposeWith(disposables);
                    this
                        .Bind(ViewModel, vm => vm.SelectedItem, v => v.Albums.SelectedItem)
                        .DisposeWith(disposables);
                    this
                        .WhenAnyValue(x => x.ViewModel)
                        .Where(x => x != null)
                        .Select(x => Unit.Default)
                        .InvokeCommand(this, x => x.ViewModel.LoadAlbums)
                        .DisposeWith(disposables);
                });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
