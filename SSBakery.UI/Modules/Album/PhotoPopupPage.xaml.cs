using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules.Album
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoPopupPage : PopupPage
    {
        public PhotoPopupPage(ImageSource imageSource)
        {
            InitializeComponent();
            Image.Source = imageSource;
        }
    }
}