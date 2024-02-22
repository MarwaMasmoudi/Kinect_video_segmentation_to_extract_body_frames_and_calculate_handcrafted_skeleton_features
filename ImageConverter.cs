using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Microsoft.Samples.Kinect.BodyBasics
{
   public class ImageConverter
    {
        FileStream fs;
        //MemoryStream ms;
        //System.Drawing.Image i2;
        //System.Drawing.Bitmap bitmap1;
        //System.Drawing.Bitmap bitmap2;

        public ImageConverterResult SaveRecordedImage( RecordedImage image  , string fileName )
        {
            if(!File.Exists(fileName))
            try
            {
                if(image == null )
                        return new ImageConverterResult(string.Format("{0}", "Wrong Frame", fileName));
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                image.JpegImage.Save(fs);
                fs.Dispose();
                fs = null;
                return new ImageConverterResult();
            }
            catch (Exception ex)
                {
                    Log.WriteErrorLogToFile(string.Format("{1} [{0}]", fileName, ex.Message), "ImageConverterResult.ExtractImage");

                    return new ImageConverterResult(string.Format("{0} : {1}", ex.Message, fileName));
            }
            else
                return new ImageConverterResult();
        }

        public ImageConverterResult ExtractImage (string fileName )
        {
            try
            {
                return new ImageConverterResult( Image.FromFile(fileName));
            }
            catch (Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{1} [{0}]", fileName , ex.Message), "ImageConverterResult.ExtractImage");

                return new ImageConverterResult(ex.Message);
            }
        }
        public ImageConverterResult ExtractImage(string fileName, System.Drawing.RectangleF rectangle)
        {
            try
            {
                MemoryStream ms;
                System.Drawing.Image i2;
                System.Drawing.Bitmap bitmap1;
                System.Drawing.Bitmap bitmap2;

                ms = null;
                i2 = System.Drawing.Image.FromFile(fileName);
                bitmap1 = (System.Drawing.Bitmap)i2;
                bitmap2 = bitmap1.Clone(rectangle, bitmap1.PixelFormat);
                ms = new MemoryStream();
                bitmap2.Save(ms, i2.RawFormat);
                
                return new ImageConverterResult(System.Drawing.Image.FromStream(ms));
            }
            catch (Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{1} [{0}][{2}]", fileName, ex.Message , rectangle), "ImageConverterResult.ExtractImageRectangle");

                return new ImageConverterResult(ex.Message);
            }
        }
        public ImageConverterResult ExtractImage(Image im, System.Drawing.RectangleF rectangle)
        {

            try
            {
                if (rectangle.X < 0 || rectangle.Y < 0 || rectangle.Width <= 0 || rectangle.Height <= 0)
                    return new ImageConverterResult("prob. de Cadrage");
                else
                {
                    MemoryStream ms;
                    System.Drawing.Image i2;
                    System.Drawing.Bitmap bitmap1;
                    System.Drawing.Bitmap bitmap2;

                    ms = null;

                    bitmap1 = (System.Drawing.Bitmap)im;

                    ms = new MemoryStream();
                    bitmap2 = bitmap1.Clone(rectangle, bitmap1.PixelFormat);
                    bitmap2.Save(ms, im.RawFormat);
                    
                    return new ImageConverterResult(System.Drawing.Image.FromStream(ms));
                }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{1} [{0}][{2}]", "From file", ex.Message, rectangle), "ImageConverterResult.ExtractImageRectangle");

                return new ImageConverterResult(ex.Message);
            }
        }

        public bool SaveImage(Image image , string fileName)
        {
            try
            {
                image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                return true;
            }
            catch(Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{1} [{0}]", fileName, ex.Message), "ImageConverterResult.SaveImage");

                return false;
            }
        }
    }
}

public class ImageConverterResult
{
    bool success = true;
    string message = "";
    Image image;

    public ImageConverterResult()
    {
    }
    public ImageConverterResult (bool ok , string m)
    {
        Success = ok;
        message = m;
    }
    public ImageConverterResult(string m)
    {
        Success = false;
        message = m;
    }
    public ImageConverterResult(Image  im)
    {
        Image  = im;
    }

    public bool Success { get => success; set => success = value; }
    public string Message { get => message; set => message = value; }
    public Image Image {
        get => image;
        set
        {
            if(value == null )
            {
                Success = false;
                message = "Error of Read";
            }
            image = value;
        }
    }

    public override string ToString()
    {
        if (Success) return "Ok";
        else return Message;
    }
}
