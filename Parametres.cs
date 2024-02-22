using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Microsoft.Samples.Kinect.BodyBasics
{
   public static  class Parametres
    {
        public static ImageConverter ImageConverter = new ImageConverter();

    }


    public static class XmlSerial
    {
       // static string TempFile = FrameResultParams.TempDirectory;// string.Format(@"{0}\{1}", Environment.CurrentDirectory, "tmp");
        static List<FrameResult2> frameResults = new List<FrameResult2>();
        public static List<FrameResult2> FrameResults
        {
            get
            {
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(List<FrameResult2>));
                    using (var sr = new StreamReader(FrameResultParams.TempDirectory + @"\Frames.xml"))
                    {
                        List<FrameResult2> ls = (List<FrameResult2>)xs.Deserialize(sr);
                        frameResults.Clear();
                        frameResults.AddRange(ls);
                    }
                }
                catch(Exception ex)
                {
                    Log.WriteErrorLogToFile(ex.Message, "Deserializer");
                }
                return frameResults;
            }
            set
            {
                if (value != null)
                {
                    frameResults = value;
                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(List<FrameResult2>));
                        TextWriter tw = new StreamWriter(FrameResultParams.TempDirectory + @"\Frames.xml");
                        xs.Serialize(tw, frameResults);
                        tw.Dispose();

                    }
                    catch (Exception ex)
                    {
                        Log.WriteErrorLogToFile(ex.Message, "Serializer");
                    }
                }
            }
        }
    }
}
