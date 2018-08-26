using System;
using UIKit;

namespace SSBakeryAdmin.iOS.Helpers
{
    public class ViewHelper
    {
        public static nfloat ScreenHeightMinusStatusAndNavBar
        {
            get
            {
                var navBarPlusStatusBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height +
                    new UINavigationController().NavigationBar.Frame.Height;

                var result = UIScreen.MainScreen.Bounds.Size.Height - navBarPlusStatusBarHeight;

                return result;
            }
        }

        public static nfloat GetItemWidthViaScreenWidth(int numCols, float interitemSpacing, UIEdgeInsets insets)
        {
            var horizontalSpacing = interitemSpacing * (numCols - 1) + insets.Left + insets.Right;
            var itemWidth = (UIScreen.MainScreen.Bounds.Size.Width - horizontalSpacing) / numCols;

            return itemWidth;
        }

        public static nfloat GetItemHeightViaAvailableScreenHeight(nfloat availHeight, int numRows, float lineSpacing, UIEdgeInsets insets)
        {
            var verticalSpacing = lineSpacing * (numRows - 1) + insets.Bottom + insets.Top;
            var itemHeight = (availHeight - verticalSpacing) / numRows;

            return itemHeight;
        }
    }
}