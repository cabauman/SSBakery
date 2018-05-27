using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSBakery.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace SSBakery.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoursAndLocationPage : ContentPageBase<HoursAndLocationViewModel>
    {
        public HoursAndLocationPage()
        {
            InitializeComponent();

            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Position(29.402421, -98.387913), Distance.FromMiles(2)));

            var position = new Position(29.402421, -98.387913);
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "SS Bakery",
                Address = "2000 S East Loop 410 Suite #110"
            };

            MyMap.Pins.Add(pin);
        }
    }
}
