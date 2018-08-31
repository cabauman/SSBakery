using System;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using CoreFoundation;
using Foundation;
using ReactiveUI;
using UIKit;

namespace SSBakeryAdmin.iOS.Modules.CatalogItemDetails
{
    [Register("CatalogItemDetailsController")]
    public class CatalogItemDetailsController : ReactiveViewController<ICatalogItemDetailsViewModel>
    {
        private static readonly string CellReuseId = "CellReuseId";

        public CatalogItemDetailsController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}