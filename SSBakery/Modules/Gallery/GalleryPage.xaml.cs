using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using SSBakery.Core.Services;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPageBase<GalleryViewModel>
    {
        public GalleryPage()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    ViewModel.SelectedItem = null;

                    this
                        .WhenAnyValue(x => x.ViewModel)
                        .Where(x => x != null)
                        .Select(x => Unit.Default)
                        .InvokeCommand(this, x => x.ViewModel.LoadPhotos)
                        .DisposeWith(disposables);
                    this
                        .WhenAnyValue(x => x.ViewModel.Photos)
                        .Where(x => x != null)
                        .Subscribe(LayoutGrid);
                });
        }

        private void LayoutGrid(IList<PhotoCellViewModel> photos)
        {
            foreach(var photo in photos)
            {
                var image = new FFImageLoading.Forms.CachedImage
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    WidthRequest = 110,
                    HeightRequest = 110,
                    Aspect = Aspect.AspectFill,
                    DownsampleToViewSize = true,
                    Source = photo.ImageUrl
                };

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += TapGesture_Tapped;
                image.GestureRecognizers.Add(tapGesture);

                wrapLayout.Children.Add(image);
            }
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "This is an image button", "OK");
        }
    }
}
