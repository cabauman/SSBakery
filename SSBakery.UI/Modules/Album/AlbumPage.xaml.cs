using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using SSBakery.Services;
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
                });
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("Alert", "This is an image button", "OK");
        }
    }
}
