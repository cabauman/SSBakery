using CoreGraphics;
using System;
using UIKit;

namespace SSBakeryAdmin.iOS.Helpers
{
    // Reference: http://blog.adamkemp.com/2014/12/ios-layout-gotchas-redux.html
    public static class UIViewExtensions
    {
        public static void CenterInParent(this UIView view)
        {
            var parent = view.Superview;
            if(parent == null)
            {
                throw new InvalidOperationException("Cannot center a view in its parent unless it has a parent");
            }
            var parentSize = parent.Bounds.Size;
            view.SafeSetCenter(new CGPoint(parentSize.Width / 2, parentSize.Height / 2));
        }

        // Reference: http://blog.adamkemp.com/2014/12/ios-layout-gotchas-redux.html
        public static void SafeSetCenter(this UIView view, CGPoint center)
        {
            var size = view.Bounds.Size;
            center = center.Floor();
            if((int)size.Width % 2 != 0)
            {
                center.X += 0.5f;
            }
            if((int)size.Height % 2 != 0)
            {
                center.Y += 0.5f;
            }

            view.Center = center;
        }
    }
}