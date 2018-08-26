using System;
using System.Drawing;

using CoreFoundation;
using UIKit;
using Foundation;
using ReactiveUI;
using System.Reactive.Disposables;
using Xamarin.Essentials;
using CoreGraphics;

namespace SSBakeryAdmin.iOS.Modules.Home
{
    [Register("HomeController")]
    public class HomeController : ReactiveViewController<IHomeViewModel>
    {
        private UIButton _navigateToCatalogCategoryListPageButton;
        private UIButton _navigateToCustomerDirectoryPageButton;

        public HomeController()
        {
            this.WhenActivated(
                disposables =>
                {
                    this
                        .BindCommand(ViewModel, vm => vm.NavigateToCatalogCategoryListPage, v => v._navigateToCatalogCategoryListPageButton)
                        .DisposeWith(disposables);
                    this
                        .BindCommand(ViewModel, vm => vm.NavigateToCustomerDirectoryPage, v => v._navigateToCustomerDirectoryPageButton)
                        .DisposeWith(disposables);
                });
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

            View.BackgroundColor = UIColor.White;

            _navigateToCatalogCategoryListPageButton = new UIButton(UIButtonType.RoundedRect);
            _navigateToCatalogCategoryListPageButton.SetTitle("Catalog", UIControlState.Normal);
            _navigateToCatalogCategoryListPageButton.BackgroundColor = UIColor.Green;
            _navigateToCatalogCategoryListPageButton.Font = _navigateToCatalogCategoryListPageButton.Font.WithSize(36);

            _navigateToCustomerDirectoryPageButton = new UIButton(UIButtonType.System);
            _navigateToCustomerDirectoryPageButton.SetTitle("Customers", UIControlState.Normal);
            _navigateToCustomerDirectoryPageButton.BackgroundColor = UIColor.Yellow;
            _navigateToCustomerDirectoryPageButton.Font = _navigateToCustomerDirectoryPageButton.Font.WithSize(36);

            View.AddSubview(_navigateToCatalogCategoryListPageButton);
            View.AddSubview(_navigateToCustomerDirectoryPageButton);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if(DeviceDisplay.ScreenMetrics.Orientation == ScreenOrientation.Landscape)
            {
                _navigateToCatalogCategoryListPageButton.Frame = new CGRect(0, 0, View.Bounds.Width * 0.5f, View.Bounds.Height);
                _navigateToCustomerDirectoryPageButton.Frame = new CGRect(View.Bounds.Width * 0.5f, 0, View.Bounds.Width * 0.5f, View.Bounds.Height);
            }
            else
            {
                _navigateToCatalogCategoryListPageButton.Frame = new CGRect(0, 0, View.Bounds.Width, View.Bounds.Height * 0.5f);
                _navigateToCustomerDirectoryPageButton.Frame = new CGRect(0, View.Bounds.Height * 0.5f, View.Bounds.Width, View.Bounds.Height * 0.5f);
            }
        }

        private void SetConstraints()
        {
            if(DeviceDisplay.ScreenMetrics.Orientation == ScreenOrientation.Landscape)
            {
                _navigateToCatalogCategoryListPageButton.TranslatesAutoresizingMaskIntoConstraints = false;
                _navigateToCatalogCategoryListPageButton.TopAnchor.ConstraintEqualTo(View.TopAnchor, 0f).Active = true;
                _navigateToCatalogCategoryListPageButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, 0f).Active = true;
                _navigateToCatalogCategoryListPageButton.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 0f).Active = true;
                _navigateToCatalogCategoryListPageButton.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.5f).Active = true;
                _navigateToCatalogCategoryListPageButton.HeightAnchor.ConstraintEqualTo(View.HeightAnchor).Active = true;

                _navigateToCustomerDirectoryPageButton.TranslatesAutoresizingMaskIntoConstraints = false;
                _navigateToCustomerDirectoryPageButton.TopAnchor.ConstraintEqualTo(View.TopAnchor, 0f).Active = true;
                _navigateToCustomerDirectoryPageButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, 0f).Active = true;
                _navigateToCustomerDirectoryPageButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, 0f).Active = true;
                _navigateToCustomerDirectoryPageButton.WidthAnchor.ConstraintEqualTo(View.WidthAnchor, 0.5f).Active = true;
                _navigateToCustomerDirectoryPageButton.HeightAnchor.ConstraintEqualTo(View.HeightAnchor).Active = true;
            }
            else
            {
                _navigateToCatalogCategoryListPageButton.TranslatesAutoresizingMaskIntoConstraints = false;
                _navigateToCatalogCategoryListPageButton.TopAnchor.ConstraintEqualTo(View.TopAnchor, 0f).Active = true;
                _navigateToCatalogCategoryListPageButton.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 0f).Active = true;
                _navigateToCatalogCategoryListPageButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, 0f).Active = true;
                _navigateToCatalogCategoryListPageButton.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
                _navigateToCatalogCategoryListPageButton.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, 0.5f).Active = true;

                _navigateToCustomerDirectoryPageButton.TranslatesAutoresizingMaskIntoConstraints = false;
                _navigateToCustomerDirectoryPageButton.BottomAnchor.ConstraintEqualTo(View.BottomAnchor, 0f).Active = true;
                _navigateToCustomerDirectoryPageButton.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 0f).Active = true;
                _navigateToCustomerDirectoryPageButton.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor, 0f).Active = true;
                _navigateToCustomerDirectoryPageButton.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
                _navigateToCustomerDirectoryPageButton.HeightAnchor.ConstraintEqualTo(View.HeightAnchor, 0.5f).Active = true;
            }
        }

        public override void UpdateViewConstraints()
        {
            base.UpdateViewConstraints();
            SetConstraints();
        }
    }
}