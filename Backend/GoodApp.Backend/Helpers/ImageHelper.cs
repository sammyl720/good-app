using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace GoodApp.Backend.Helpers
{
    public class ImageHelper
    {
        public static Image Shrink(Image originalImage, int maxWidth)
        {
            int originalWidth = originalImage.Width;
            int originalHeight = originalImage.Height;
            if (originalWidth > maxWidth)
            {
                int targetHeight = originalHeight*maxWidth/originalWidth;
                var newImage = new Bitmap(originalImage, maxWidth, targetHeight);
                return newImage;
            }

            return originalImage;
        }

        public static void Resize(Stream input, Stream output, int maxWidth, ImageFormat imageFormat)
        {
            using (Image image = Image.FromStream(input))
            {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float whRatio = originalWidth/(float) originalHeight;
                int width = originalWidth;
                int height = originalHeight;

                if (maxWidth < width)
                {
                    width = maxWidth;
                    height = (int) (width/whRatio);
                }

                using (var bmp = new Bitmap(width, height))
                {
                    using (Graphics gr = Graphics.FromImage(bmp))
                    {
                        gr.CompositingQuality = CompositingQuality.HighQuality;
                        gr.SmoothingMode = SmoothingMode.HighQuality;
                        gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        gr.DrawImage(image, new Rectangle(0, 0, width, height));
                        bmp.Save(output, imageFormat);
                    }
                }
            }
        }
    }
}