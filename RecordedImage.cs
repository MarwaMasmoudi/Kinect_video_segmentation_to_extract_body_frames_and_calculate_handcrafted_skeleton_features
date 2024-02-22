using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public class RecordedImage
    {
        JpegBitmapEncoder jpegImage = new JpegBitmapEncoder();
        TimeSpan time;

        public JpegBitmapEncoder JpegImage { get => jpegImage; set => jpegImage = value; }
        public TimeSpan Time { get => time; set => time = value; }
    }

}
