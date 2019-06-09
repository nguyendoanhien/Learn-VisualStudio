using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using sd = System.Drawing;

namespace Utilities
{
    public class FileHelper
    {
        public bool checkUploadImg(string fileName, float size)
        {
            bool bl;
            var fileExt = Path.GetExtension(fileName).ToLower();
            if (fileExt == ".gif" || fileExt == ".jpg" || fileExt == ".png" || fileExt == ".jpeg" ||
                fileExt == ".bmp" && size <= 204800)
            {
                bl = true;
                return bl;
            }

            bl = false;
            return bl;
        }

        public string DeleteFile(string filePath)
        {
            var msg = "";
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                else
                    msg = "File không tồn tại.";
                return msg;
            }
            catch (Exception ex)
            {
                return msg + ex;
            }
        }

        public void DrawImageToWithHeight(string pathAvatars, string fileName, Stream fileStream, int width, int height)
        {
            try
            {
                var thumbWidth = width;
                var thumbHeight = height;
                var oImg = sd.Image.FromStream(fileStream);
                sd.Image oThumbNail = new sd.Bitmap(thumbWidth, thumbHeight, oImg.PixelFormat);
                var oGraphic = sd.Graphics.FromImage(oThumbNail);
                oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var oRectangle = new sd.Rectangle(0, 0, thumbWidth, thumbHeight);
                oGraphic.DrawImage(oImg, oRectangle);
                oThumbNail.Save(pathAvatars + fileName, ImageFormat.Png);
                oImg.Dispose();
            }
            catch (Exception ex)
            {
                
            }
        }

        public void Drawing(sd.Graphics gr, string s, sd.PointF p, Random rnd)
        {
            var x = p.X;
            var y = p.Y;
            var c = s.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                var ch = c[i].ToString();
                var brush = GetBrush(rnd);
                var font = GetFont(rnd);
                var sf = gr.MeasureString(ch, font);
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.DrawString(ch, font, brush, new sd.PointF(x, y + (rnd.Next(7) - 3)));
                x = x + sf.Width / 2;
            }
        }

//EXPORT EXCEL FILE
        public void ExportGridView(string fileName, string filePath, DataGrid gv)
        {
            gv.EnableViewState = false;
            var tw = new StringWriter();
            var hw = new HtmlTextWriter(tw);
            gv.RenderControl(hw);
            var htmlInfo = tw.ToString().Trim();
            var xlsFileName = fileName + ".xls";
            var filePathName = filePath + xlsFileName;
            if (File.Exists(filePathName))
                File.Delete(filePathName);
            var Fs = new FileStream(filePathName, FileMode.Create);
            var BWriter = new BinaryWriter(Fs, Encoding.GetEncoding("UTF-8"));
            BWriter.Write(htmlInfo);
            BWriter.Close();
            Fs.Close();
        }

        public sd.Brush GetBrush(Random rnd)
        {
            var brush = sd.Brushes.Black;
            var ir = rnd.Next(9);
            switch (ir)
            {
                case 0:
                    brush = sd.Brushes.DarkCyan;
                    break;
                case 1:
                    brush = sd.Brushes.DarkGreen;
                    break;
                case 2:
                    brush = sd.Brushes.Crimson;
                    break;
                case 3:
                    brush = sd.Brushes.DarkViolet;
                    break;
                case 4:
                    brush = sd.Brushes.DarkOliveGreen;
                    break;
                case 5:
                    brush = sd.Brushes.DarkOrchid;
                    break;
                case 6:
                    brush = sd.Brushes.DarkTurquoise;
                    break;
            }

            return brush;
        }

//get Current Page Name
        /// <summary>
        ///     Request.Url.AbsolutePath
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public string GetCurrentPageName(string sPath)
        {
            var oInfo = new FileInfo(sPath);
            var sRet = oInfo.Name;
            return sRet;
        }

        public sd.Font GetFont(Random rnd)
        {
            float sz = 15 + rnd.Next(4);
            var f = new sd.Font("Arial", sz);
            var i = rnd.Next(4);
            switch (i)
            {
                case 0:
                    f = new sd.Font("Tahoma", sz, sd.FontStyle.Bold);
                    break;
                case 1:
                    f = new sd.Font("Verdana", sz, sd.FontStyle.Bold);
                    break;
                case 2:
                    f = new sd.Font("Times New Roman", sz, sd.FontStyle.Bold);
                    break;
            }

            return f;
        }

        public sd.Pen GetPen(Random rnd)
        {
            var p = sd.Pens.Black;
            var i = rnd.Next(4);
            switch (i)
            {
                case 0:
                    p = sd.Pens.Crimson;
                    break;
                case 1:
                    p = sd.Pens.DarkCyan;
                    break;
                case 2:
                    p = sd.Pens.DarkOliveGreen;
                    break;
            }

            return p;
        }

//Resize the Height image
        public void
            ResizeImageHeight(string imgPath, string saveTo, int resizeHeight) //, int Width, int Height, int X, int Y)
        {
            using (var originalImage = sd.Image.FromFile(imgPath))
            {
                var originalWidth = int.Parse(originalImage.Width.ToString());
                var originalHeight = int.Parse(originalImage.Height.ToString());
/*
* chia theo tỉ lể tìm ra chiều cao mới
* int originalImage_H -----------> resizeHeight
* int originalImage_W ----------->???NewWidth
*/
                var newWidth = originalWidth * resizeHeight / originalHeight;
                using (var bmp = new sd.Bitmap(newWidth, resizeHeight)) //(Width, Height))
                {
                    bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                    using (var graphic = sd.Graphics.FromImage(bmp))
                    {
                        graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.CompositingQuality = CompositingQuality.HighQuality;
                        graphic.DrawImage(originalImage,
                            new sd.Rectangle(0, 0, newWidth,
                                resizeHeight)); //, X, Y, Width, Height, sd.GraphicsUnit.Pixel);
                        var msBuffer = new MemoryStream();
                        bmp.Save(msBuffer, originalImage.RawFormat);
                        var bufferImage = msBuffer.GetBuffer();
                        msBuffer.Dispose();
                        using (var msWrite = new MemoryStream(bufferImage, 0, bufferImage.Length))
                        {
                            msWrite.Write(bufferImage, 0, bufferImage.Length);
                            using (var imageResized = sd.Image.FromStream(msWrite, true))
                            {
//string saveTo = Server.MapPath("~/Images/imgCrop/") + "small" + ImageName;
                                imageResized.Save(saveTo, imageResized.RawFormat);
                            }

                            msWrite.Dispose();
                        }

                        graphic.Dispose();
                    }

                    bmp.Dispose();
                }
            }
        }

//Resize the Width image
        public void
            ResizeImageWidth(string imgPath, string saveTo, int resizeWidth) //, int Width, int Height, int X, int Y)
        {
            using (var originalImage = sd.Image.FromFile(imgPath))
            {
                var originalWidth = int.Parse(originalImage.Width.ToString());
                var originalHeight = int.Parse(originalImage.Height.ToString());
/*
* chia theo tỉ lể tìm ra chiều cao mới
* int originalImage_W ----------->resizeWidth
* int originalImage_H -----------> ???New_Height
*/
                var newHeight = originalHeight * resizeWidth / originalWidth;
                using (var bmp = new sd.Bitmap(resizeWidth, newHeight)) //(Width, Height))
                {
                    bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                    using (var graphic = sd.Graphics.FromImage(bmp))
                    {
                        graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.CompositingQuality = CompositingQuality.HighQuality;
                        graphic.DrawImage(originalImage,
                            new sd.Rectangle(0, 0, resizeWidth,
                                newHeight)); //, X, Y, Width, Height, sd.GraphicsUnit.Pixel);
                        var msBuffer = new MemoryStream();
                        bmp.Save(msBuffer, originalImage.RawFormat);
                        var bufferImage = msBuffer.GetBuffer();
                        msBuffer.Dispose();
                        using (var msWrite = new MemoryStream(bufferImage, 0, bufferImage.Length))
                        {
                            msWrite.Write(bufferImage, 0, bufferImage.Length);
                            using (var imageResized = sd.Image.FromStream(msWrite, true))
                            {
//string saveTo = Server.MapPath("~/Images/imgCrop/") + "small" + ImageName;
                                imageResized.Save(saveTo, imageResized.RawFormat);
                            }

                            msWrite.Dispose();
                        }

                        graphic.Dispose();
                    }

                    bmp.Dispose();
                }
            }
        }

//Resize the Height and Width image
        public byte[]
            ResizeImageWidthHeight(string imgPath, int resizeWidth,
                int resizeHeight) //, int Width, int Height, int X, int Y)
        {
            using (var originalImage = sd.Image.FromFile(imgPath))
            {
//int h = int.Parse(originalImage.Height.ToString()) / resize;
//int w = int.Parse(originalImage.Width.ToString()) / resize;
                using (var bmp = new sd.Bitmap(resizeWidth, resizeHeight)) //(Width, Height))
                {
                    bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                    using (var graphic = sd.Graphics.FromImage(bmp))
                    {
                        graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.DrawImage(originalImage,
                            new sd.Rectangle(0, 0, resizeWidth,
                                resizeHeight)); //, X, Y, Width, Height, sd.GraphicsUnit.Pixel);
                        var ms = new MemoryStream();
                        bmp.Save(ms, originalImage.RawFormat);
                        return ms.GetBuffer();
                    }
                }
            }
        }

//CAPCHA Drawing
        public sd.Bitmap TransformImage(sd.Bitmap bmp, Random rnd)
        {
            var width = bmp.Width;
            var height = bmp.Height;
            double distort = rnd.Next(5, 15) * (rnd.Next(10) == 1 ? 1 : -1);
// Copy the image so that we're always using the original for source color
            using (var copy = (sd.Bitmap) bmp.Clone())
            {
                for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                {
// Adds a simple wave
                    var newX = (int) (x + distort * Math.Sin(Math.PI * y / 84.0));
                    var newY = (int) (y + distort * Math.Cos(Math.PI * x / 44.0));
                    if (newX < 0 || newX >= width) newX = 0;
                    if (newY < 0 || newY >= height) newY = 0;
                    bmp.SetPixel(x, y, copy.GetPixel(newX, newY));
                }
            }

            return bmp;
        }
    }
}