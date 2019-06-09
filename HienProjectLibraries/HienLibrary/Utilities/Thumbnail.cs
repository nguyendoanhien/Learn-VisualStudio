using System;
using System.Drawing;
using System.Web;


namespace Utilities
{
    public class Thumbnail
    {
        public bool GenerateThumbnail(string fileName, string sourcedes, string filedes, int maxwidth)
        {
            double tile = 0;
            var shtWidth = 0;
            var shtHeight = 0;
            try
            {
                //Get The Image From File
                var originalImage = Image.FromFile(HttpContext.Current.Server.MapPath(sourcedes + fileName));
                if (originalImage.Width >= originalImage.Height)
                {
                    tile = originalImage.Width / (double) originalImage.Height;
                    if (originalImage.Width > maxwidth)
                        shtWidth = maxwidth;
                    else
                        shtWidth = originalImage.Width;
                    shtHeight = System.Convert.ToInt32(shtWidth / tile);
                }
                else
                {
                    tile = originalImage.Height / (double) originalImage.Width;
                    if (originalImage.Width > maxwidth)
                        shtWidth = maxwidth;
                    else
                        shtWidth = originalImage.Width;
                    shtHeight = System.Convert.ToInt32(shtWidth * tile);
                }

                //OriginalImage.Save(HttpContext.Current.Server.MapPath(sourcedes + filedes));
                //Create ITs Thumbnail
                var thumbImage = originalImage.GetThumbnailImage(shtWidth, shtHeight, null, new IntPtr());
                //Store It in Thumbnail Folder
                thumbImage.Save(HttpContext.Current.Server.MapPath(sourcedes + filedes));
                thumbImage.Dispose();
                //ThumbImage.Save(HttpContext.Current.Server.MapPath(sourcedes + filedes), ImageFormat.Gif);
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public bool GetGenerateThumbnail(string fileName, string sourcedes, int maxwidth)
        {
            double tile = 0;
            var shtWidth = 0;
            var shtHeight = 0;
            try
            {
                //Get The Image From File
                var originalImage = Image.FromFile(HttpContext.Current.Server.MapPath(sourcedes + fileName));
                if (originalImage.Width >= originalImage.Height)
                {
                    tile = originalImage.Width / (double) originalImage.Height;
                    if (originalImage.Width > maxwidth)
                        shtWidth = maxwidth;
                    else
                        shtWidth = originalImage.Width;
                    shtHeight = System.Convert.ToInt32(shtWidth / tile);
                }
                else
                {
                    tile = originalImage.Height / (double) originalImage.Width;
                    if (originalImage.Width > maxwidth)
                        shtWidth = maxwidth;
                    else
                        shtWidth = originalImage.Width;
                    shtHeight = System.Convert.ToInt32(shtWidth * tile);
                }

                var bitmap = new Bitmap(shtWidth, shtHeight);
                Graphics.FromImage(bitmap).DrawImage(originalImage, 0, 0, shtWidth, shtHeight);
                originalImage.Dispose();
                bitmap.Save(HttpContext.Current.Server.MapPath(sourcedes + fileName));
                bitmap.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }

        public bool ResizeImage(string strResourceFile, int intHeight, int intWidth)
        {
            var image = Image.FromFile(strResourceFile);
            if (image.Height > intHeight || image.Width > intWidth)
            {
                var num = image.Height / (float) intHeight;
                var num2 = image.Width / (float) intWidth;
                var height = Math.Min(intHeight, image.Height);
                var width = image.Width * height / image.Height;
                if (num < num2)
                {
                    width = Math.Min(intWidth, image.Width);
                    height = image.Height * width / image.Width;
                }

                var bitmap = new Bitmap(width, height);
                Graphics.FromImage(bitmap).DrawImage(image, 0, 0, width, height);
                image.Dispose();
                bitmap.Save(strResourceFile);
                bitmap.Dispose();
            }

            return true;
        }

        private bool SaveImageAs(string fileName, string sourcedes, string filedes)
        {
            try
            {
                //Get The Image From File
                var originalImage = Image.FromFile(HttpContext.Current.Server.MapPath(sourcedes + fileName));
                originalImage.Save(HttpContext.Current.Server.MapPath(sourcedes + filedes));
                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
    }
}