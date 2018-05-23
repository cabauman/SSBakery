using System;
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
        }
    }
}
