using System;
using System.Drawing;
using CoreGraphics;
using SSBakeryAdmin.Services.Interfaces;
using UIKit;

namespace SSBakeryAdmin.iOS.Services
{
    public class MediaService : IMediaService
    {
        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {

            UIImage originalImage = ImageFromByteArray(imageData);

            var originalHeight = originalImage.Size.Height;
            var originalWidth = originalImage.Size.Width;

            nfloat newHeight = 0;
            nfloat newWidth = 0;

            if(originalHeight > originalWidth)
            {
                newHeight = height;
                nfloat ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                nfloat ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            width = (float)newWidth;
            height = (float)newHeight;

            UIGraphics.BeginImageContext(new SizeF(width, height));
            originalImage.Draw(new RectangleF(0, 0, width, height));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            var bytesImagen = resizedImage.AsJPEG().ToArray();
            resizedImage.Dispose();
            return bytesImagen;
        }

        public UIImage CropImage(UIImage image, CGSize size)
        {
            double newCropWidth, newCropHeight;

            if(image.Size.Width < image.Size.Height)
            {
                if (image.Size.Width < size.Width)
                {
                    newCropWidth = size.Width;
                }
                else
                {
                    newCropWidth = image.Size.Width;
                }
                  newCropHeight = (newCropWidth * size.Height)/size.Width;
            }
            else
            {
                if(image.Size.Height < size.Height)
                {
                    newCropHeight = size.Height;
                }
                else
                {
                    newCropHeight = image.Size.Height;
                }
                newCropWidth = (newCropHeight * size.Width)/size.Height;
            }

            double x = image.Size.Width/2.0 - newCropWidth/2.0;
            double y = image.Size.Height/2.0 - newCropHeight/2.0;

            CGRect cropRect = new CGRect(x, y, newCropWidth, newCropHeight);
            var imageRef = image.CGImage.WithImageInRect(cropRect);

            UIImage cropped = new UIImage(imageRef);
            imageRef.Dispose();

            return cropped;
        }

        public static UIImage ImageFromByteArray(byte[] data)
        {
            if(data == null)
            {
                return null;
            }

            UIImage image;
            try
            {
                image = new UIImage(Foundation.NSData.FromArray(data));
            }
            catch(Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}
