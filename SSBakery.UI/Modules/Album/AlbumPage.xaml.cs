using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Rg.Plugins.Popup.Services;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumPage : ContentPageBase<IAlbumViewModel>
    {
        public AlbumPage()
        {
            InitializeComponent();

            this
                .WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Select(x => Unit.Default)
                .InvokeCommand(this, x => x.ViewModel.LoadPhotos);
        }

        private async void FlowListView_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var photoCell = e.Item as PhotoCellViewModel;
            await PopupNavigation.Instance.PushAsync(new PhotoPopupPage(photoCell.ImageUrlMaxSize));
        }
    }
}
