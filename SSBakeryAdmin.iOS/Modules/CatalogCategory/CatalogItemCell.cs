using System;
using System.Drawing;

using CoreGraphics;
using Foundation;
using UIKit;

namespace SSBakeryAdmin.iOS.Modules.CatalogCategory
{
    public class CatalogItemCell : UICollectionViewCell
    {
        public static NSString CellId = new NSString("CellId");

        [Export("initWithFrame:")]
        public CatalogItemCell(RectangleF frame)
            : base(frame)
        {
            ImageView = new UIImageView();
            ImageView.Layer.BorderColor = UIColor.DarkGray.CGColor;
            ImageView.Layer.BorderWidth = 1f;
            ImageView.Layer.CornerRadius = 3f;
            ImageView.Layer.MasksToBounds = true;
            ImageView.ContentMode = UIViewContentMode.ScaleToFill;

            ContentView.AddSubview(ImageView);

            TitleLabel = new UILabel();
            TitleLabel.BackgroundColor = UIColor.Clear;
            TitleLabel.TextColor = UIColor.DarkGray;
            TitleLabel.TextAlignment = UITextAlignment.Center;

            ContentView.AddSubview(TitleLabel);
        }

        public UIImageView ImageView { get; private set; }

        public UILabel TitleLabel { get; private set; }

        //public void UpdateRow(UserElement element, Single fontSize, SizeF imageViewSize)
        //{
        //    TitleLabel.Text = element.Caption;
        //    ImageView.Image = element.Image;

        //    TitleLabel.Font = UIFont.FromName("HelveticaNeue-Bold", fontSize);

        //    ImageView.Frame = new RectangleF(0, 0, imageViewSize.Width, imageViewSize.Height);
        //    TitleLabel.Frame = new RectangleF(0, (float)ImageView.Frame.Bottom, imageViewSize.Width,
        //                                     (float)ContentView.Frame.Height - (float)ImageView.Frame.Bottom);
        //}
    }
}