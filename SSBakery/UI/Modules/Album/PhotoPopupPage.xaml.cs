using System;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoPopupPage : PopupPage
    {
        public PhotoPopupPage(string imageUrl)
        {
            InitializeComponent();

            Image.Source = imageUrl;
        }
    }
}