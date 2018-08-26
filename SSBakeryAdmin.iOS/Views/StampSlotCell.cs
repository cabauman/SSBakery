using CoreGraphics;
using Foundation;
using SSBakeryAdmin.iOS.Helpers;
using System;
using UIKit;

namespace SSBakeryAdmin.iOS.Views
{
    public class StampSlotCell : UICollectionViewCell
    {
        private UIImageView _stampImageView;
        private bool _stamped;
        private nfloat _inactiveAlpha = 0.25f;
        private double _alphaAnimDuration = 0.1d;

        [Export("initWithFrame:")]
        public StampSlotCell(CGRect frame)
            : base(frame)
        {
            ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
            ContentView.Layer.BorderWidth = 2.0f;
            ContentView.BackgroundColor = UIColor.White;

            _stampImageView = new UIImageView(UIImage.FromBundle("Stamp"));

            ContentView.AddSubview(_stampImageView);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _stampImageView.Frame = Bounds;
            _stampImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            _stampImageView.Transform = CGAffineTransform.MakeScale(0.9f, 0.9f);
            _stampImageView.CenterInParent();
        }

        public bool Stamped
        {
            get { return _stamped; }
            set
            {
                _stamped = value;
                nfloat alpha = _stamped ? 1f : _inactiveAlpha;
                Animate(_alphaAnimDuration, () => _stampImageView.Alpha = alpha);
            }
        }
    }
}