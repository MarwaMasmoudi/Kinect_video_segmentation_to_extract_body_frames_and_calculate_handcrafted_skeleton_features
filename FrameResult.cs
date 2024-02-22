using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Kinect;
using System.IO;
using System.Threading;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public static class FrameResultParams
    {
        public static string TempDirectory = Environment.CurrentDirectory + @"\tmp";
        public static List<FrameResult> Resultat = new List<FrameResult>();
        public static void ResetTemp()
        {
            if (System.IO.Directory.Exists(TempDirectory))
                System.IO.Directory.Delete(TempDirectory, true);

            System.IO.Directory.CreateDirectory(TempDirectory);
        }
    }
    [Serializable]
  public   class FrameResult
    {
        public FrameResult()
        {

        }
        public FrameResult(FrameResult2 f)
        {
            fromFrameResult(f);
        }
        public Dictionary<JointType, Point> source2 ;
        public Dictionary<JointType, Joint> source;
        public TimeSpan RelativeTime;
        public Double TimeOfFrame
        {
            get { return RelativeTime.TotalMilliseconds; }
        }
        public int BodyIndex;
        RecordedImage recordedImage;
        bool ab_activ = true;
        bool confirmed = true;
        string message = "";

        public System.Windows.Size size1 ;
        public System.Windows.Size size2;

        System.Drawing.Image frameImage;
        System.Drawing.Image bodyImage;

        public RecordedImage RecordedImage { get => recordedImage; set => recordedImage = value; }

        public System.Drawing.Image FrameImage { get => frameImage; set => frameImage = value; }
        public System.Drawing.Image BodyImage { get => bodyImage; set => bodyImage = value; }

        public string FrameImageFileName
        {
            get
            {
                return string.Format("{0}.jpeg", RelativeTime.ToString().Replace(':', ' ').Replace('.', ' '), BodyIndex); //_{1}
            }
        }
        public string BodyImageFileName
        {
            get
            {
                return string.Format("{0}_{1}_F.jpeg", RelativeTime.ToString().Replace(':', ' ').Replace('.', ' '), BodyIndex);
            }
        }

        public System.Windows.Rect Rectangle
        {
            get
            {
                if (source == null)
                    return new Rect();
                else
                {
                    Point min = new Point();
                    Point max = new Point();
                    int i = 0;

                    if(source2 == null )
                    {
                        source2 = new Dictionary<JointType, Point>();
                        foreach (var v in source)
                            source2.Add(v.Key, new Point(v.Value.Position.X, v.Value.Position.Y));
                    }

                    foreach (var v in source2)
                    {
                        Point p = new Point(v.Value.X, v.Value.Y);
                        if (i == 0)
                        {
                            min = new Point(p.X, p.Y)  ;
                            max = new Point(p.X, p.Y);
                        }
                        else
                        {
                            min.X = Math.Min(min.X, p.X);
                            min.Y = Math.Min(min.Y, p.Y);
                            max.X = Math.Max(max.X, p.X);
                            max.Y = Math.Max(max.Y, p.Y);
                        }

                        i++;
                    }

                    System.Windows.Rect res = new Rect(min, max);


                    return res;
                }
            }
        }

        public System.Drawing.RectangleF rectangle2
        {
            get
            {
                System.Drawing.RectangleF res = new System.Drawing.RectangleF();
                if (size2 == new Size())
                    res = new System.Drawing.RectangleF() { X = (float)Rectangle.X, Y = (float)Rectangle.Y, Width = (float)Rectangle.Width, Height = (float)Rectangle.Height };
                else
                    res = new System.Drawing.RectangleF() { X = (float)(Rectangle.X * (size2.Width / size1.Width)), Y = (float)(Rectangle.Y * (size2.Height / size1.Height)), Width = (float)(Rectangle.Width * (size2.Width / size1.Width)), Height = (float)(Rectangle.Height * (size2.Height / size1.Height)) };


                if (FrameImage != null)
                {
                    res.Width = Math.Min(res.Width, FrameImage.Width - res.X);
                    res.Height = Math.Min(res.Height, FrameImage.Height - res.Y);
                }
                return res;
            }
        }

        public bool Ab_activ { get => ab_activ; set => ab_activ = value; }
        public bool Confirmed { get => confirmed; set => confirmed = value; }
        public string Message { get => message; set => message = value; }

        public void SaveImage(string floderPath)
        {
            if (!System.IO.Directory.Exists(floderPath))
            {
                System.IO.Directory.CreateDirectory(floderPath);
            }
            try
            {
                FrameImage.Save(string.Format(@"{0}\{1}", floderPath, FrameImageFileName));
            }
            catch (Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]", ex.Message, FrameImageFileName, BodyIndex), "FrameImage.Save");
            }
            try
            {
                BodyImage.Save(string.Format(@"{0}\{1}", floderPath, BodyImageFileName));
            }
            catch (Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]", ex.Message, BodyImageFileName, BodyIndex), "BodyImage.Save");
            }
        }
        public void SaveImage()
        {
            string floderPath = Environment.CurrentDirectory + @"\img";
            SaveImage(floderPath);
        }

        public void ReadImage()
        {
            lireImage();
        }

        public void ReadBodyImage()
        {
            message = "";
            
                string filename = FrameResultParams.TempDirectory + @"\" + RelativeTimeString + ".jpeg";
                
                string m = "";
                if (savedTemp)
                    try
                    {
                        if (FrameImage == null)
                        {
                            m = "F";
                            ImageConverterResult r = Parametres.ImageConverter.ExtractImage(filename);
                            if (r.Success)
                            {
                                FrameImage = r.Image;
                                size2 = new Size(FrameImage.Size.Width, FrameImage.Size.Height);
                            }
                            else
                                Message = r.Message;
                        }

                        if (FrameImage != null)
                        {
                            m = "B";
                            ImageConverterResult r = new ImageConverterResult();

                            try
                            {
                                r = Parametres.ImageConverter.ExtractImage(FrameImage, rectangle2);
                            }
                            catch (Exception ex)
                            {
                            Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]", ex.Message , RelativeTimeString , BodyIndex), "Temp.Save");

                            MessageBox.Show(ex.Message);
                            }

                            if (r.Success)
                            {
                                BodyImage = r.Image;
                            }
                            else
                            {
                                r = Parametres.ImageConverter.ExtractImage(filename, rectangle2);

                                if (r.Success)
                                {
                                    BodyImage = r.Image;
                                }
                                else
                                    Message = r.Message;
                            }
                        }
                }
                catch (Exception ex)
                {
                    Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]", m+" : "+ex.Message , RelativeTimeString , BodyIndex), "Temp.Save");
                    Message = m + ":" + ex.Message;  //MessageBox.Show(filename, ex.Message);
                    }
        }
        public bool savedTemp = false;

        public string RelativeTimeString {  get { return RelativeTime.ToString().Replace(':', ' ').Replace('.', ' ') + "_" + BodyIndex; } }
        private void lireImage ()
        {
            message = "";
            if (BodyImage == null || FrameImage == null)
            {
                string filename = FrameResultParams.TempDirectory + @"\" + FrameImageFileName;
                

                if (!savedTemp && recordedImage !=null )
                {
                    ImageConverterResult r = Parametres.ImageConverter.SaveRecordedImage(RecordedImage, filename);
                    if (r.Success)
                        savedTemp = true;
                    else
                        Message = r.Message;
                }

                        string m ="";
                //savedTemp = true;
                if(savedTemp)
                try
                {
                    if (FrameImage == null)
                    {
                            m = "F";
                            ImageConverterResult r = Parametres.ImageConverter.ExtractImage(filename);
                            if (r.Success)
                            {
                                FrameImage = r.Image;
                                size2 = new Size(FrameImage.Size.Width, FrameImage.Size.Height);
                            }
                            else
                                Message = r.Message;
                        }

                        if (FrameImage != null)
                        {
                            m = "B";
                            ImageConverterResult r = new ImageConverterResult();

                            try
                            {
                                r = Parametres.ImageConverter.ExtractImage(FrameImage, rectangle2);
                            }
                            catch (Exception ex)
                            {
                                Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]", m + " : " + ex.Message, RelativeTimeString, BodyIndex), "Read Body Image");

                                MessageBox.Show(ex.Message);
                            }

                            if (r.Success)
                            {
                                BodyImage = r.Image;
                            }
                            else
                            {
                                r = Parametres.ImageConverter.ExtractImage(filename, rectangle2);

                                if (r.Success)
                                {
                                    BodyImage = r.Image;
                                }
                                else
                                    Message = r.Message;
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]", m + " : " + ex.Message, RelativeTimeString, BodyIndex), "Read Image");

                        Message = m+":"+ ex.Message;  //MessageBox.Show(filename, ex.Message);
                }


            }
        }

        public Dictionary<string, double> CalculedValues = new Dictionary<string, double>();

        void AddAngel( JointType j1 , JointType j2 , JointType j3  , Calcule C)
        {
             CalculedValues.Add(string.Format("Angel : {0}-{1}-{2}", j1 , j2 , j3 ) , C.calculateAngle(source[j2], source[j1], source[j2], source[j3]));
        }

        void AddDeffernceAngel(JointType j1, JointType j2, JointType j3, Calcule C , int p_f_index)
        {
            if (p_f_index == -1)
                CalculedValues.Add(string.Format("Distance {0}-{1}-{2} P.Frame ", j1, j2, j3), 0);// CalculedValues[string.Format("Angel : {0}-{1}-{2}", j1, j2, j3)] - Resultat[p_f_index].CalculedValues[string.Format("Angel : {0}-{1}-{2}", j1, j2, j3)]);

            else 
            CalculedValues.Add(string.Format("Distance {0}-{1}-{2} P.Frame ", j1, j2, j3), CalculedValues[string.Format("Angel : {0}-{1}-{2}", j1, j2, j3)] - FrameResultParams.Resultat[p_f_index].CalculedValues[string.Format("Angel : {0}-{1}-{2}", j1, j2, j3)]);
        }

        void AddAngel(JointType j1, JointType j2, JointType j3, JointType j4, Calcule C)
        {
            CalculedValues.Add(string.Format("Angel : {0} {1}-{2} {3}", j1, j2, j3 , j4), C.calculateAngle(source[j1], source[j2], source[j3], source[j4]));
        }

        void AddDeffernceAngel(JointType j1, JointType j2, JointType j3, JointType j4, Calcule C, int p_f_index)
        {
            if (p_f_index == -1)
                CalculedValues.Add(string.Format("Distance {0} {1}-{2} {3} P.Frame ", j1, j2, j3, j4), 0);// CalculedValues[string.Format("Angel : {0}-{1}-{2}", j1, j2, j3)] - Resultat[p_f_index].CalculedValues[string.Format("Angel : {0}-{1}-{2}", j1, j2, j3)]);

            else
                CalculedValues.Add(string.Format("Distance {0} {1}-{2} {3} P.Frame ", j1, j2, j3 , j4), CalculedValues[string.Format("Angel : {0} {1}-{2} {3}", j1, j2, j3 , j4)] - FrameResultParams.Resultat[p_f_index].CalculedValues[string.Format("Angel : {0} {1}-{2} {3}", j1, j2, j3 , j4)]);
        }

        public void Calculer(int p_f_index)
        {
            Calcule C = new Calcule();
            CalculedValues.Clear();
            CalculedValues.Add(string.Format("frame"), TimeOfFrame);
            CalculedValues.Add(string.Format("B.I"), BodyIndex);

            // X  , Y  , Z
            foreach (JointType t in C.JointTypes)
            {
                CalculedValues.Add(string.Format("{0} X", t), source[t].Position.X);
                CalculedValues.Add(string.Format("{0} Y", t), source[t].Position.Y);
                CalculedValues.Add(string.Format("{0} Z", t), source[t].Position.Z);
            }
            // Distance From Kinect
            foreach (JointType t in C.JointTypes)
                CalculedValues.Add(string.Format("Distance {0} kinect", t), C.distanceFromKinect(source[t]));
            // Distance from preview frame 
            if (p_f_index == -1)
                foreach (JointType t in C.JointTypes)
                    CalculedValues.Add(string.Format("Difference Distance {0} P.Frame ", t), 0);
            else
                foreach (JointType t in C.JointTypes)
                    CalculedValues.Add(string.Format("Difference Distance {0} P.Frame ", t), CalculedValues[string.Format("Distance {0} kinect", t)] - FrameResultParams.Resultat[p_f_index].CalculedValues[string.Format("Distance {0} kinect", t)]);
            // Angel
            #region Calculate Angels
            AddAngel(JointType.Head, JointType.SpineShoulder, JointType.SpineMid, C);
            AddAngel(JointType.SpineShoulder, JointType.SpineMid, JointType.SpineBase, C);
            AddAngel(JointType.SpineBase, JointType.HipRight, JointType.KneeRight, C);
            AddAngel(JointType.HipRight, JointType.KneeRight, JointType.AnkleRight, C);
            AddAngel(JointType.HipLeft, JointType.SpineBase, JointType.HipRight, C);

            AddAngel(JointType.HipLeft, JointType.KneeLeft, JointType.AnkleLeft, C);
            CalculedValues.Add(string.Format("Angel : {0}-{1}-{2}", JointType.SpineMid, JointType.SpineShoulder, "Yaxis"), C.calculateAngleBetweenVectorAndYaxis(source[JointType.SpineMid], source[JointType.SpineShoulder]));
            AddAngel(JointType.HipRight, JointType.KneeRight, JointType.SpineMid, JointType.SpineShoulder, C);
            AddAngel(JointType.HipLeft, JointType.KneeLeft, JointType.SpineMid, JointType.SpineShoulder, C);
            AddAngel(JointType.SpineMid, JointType.SpineShoulder, JointType.ShoulderRight, C);

            AddAngel(JointType.SpineShoulder, JointType.ShoulderRight, JointType.ElbowRight, C);
            AddAngel(JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, C);
            AddAngel(JointType.SpineMid, JointType.SpineShoulder, JointType.ShoulderLeft, C);
            AddAngel(JointType.SpineShoulder, JointType.ShoulderLeft, JointType.ElbowLeft, C);
            AddAngel(JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, C);
            #endregion

            #region Calculate difference Angels
            AddDeffernceAngel(JointType.Head, JointType.SpineShoulder, JointType.SpineMid, C, p_f_index);
            AddDeffernceAngel(JointType.SpineShoulder, JointType.SpineMid, JointType.SpineBase, C, p_f_index);
            AddDeffernceAngel(JointType.SpineBase, JointType.HipRight, JointType.KneeRight, C, p_f_index);
            AddDeffernceAngel(JointType.HipRight, JointType.KneeRight, JointType.AnkleRight, C, p_f_index);
            AddDeffernceAngel(JointType.HipLeft, JointType.SpineBase, JointType.HipRight, C, p_f_index);

            AddDeffernceAngel(JointType.HipLeft, JointType.KneeLeft, JointType.AnkleLeft, C, p_f_index);

            //CalculedValues.Add(string.Format("Angel : {0}-{1}-{2}", JointType.SpineMid, JointType.SpineShoulder, "Yaxis"), C.calculateAngleBetweenVectorAndYaxis(source[JointType.SpineMid], source[JointType.SpineShoulder]));
            if (p_f_index == -1)
                CalculedValues.Add(string.Format("Distance {0}-{1}-{2} P.Frame ", JointType.SpineMid, JointType.SpineShoulder, "Yaxis"), 0);// CalculedValues[string.Format("Angel : {0}-{1}-{2}", JointType.SpineMid, JointType.SpineShoulder, "Yaxis")] - Resultat[p_f_index].CalculedValues[string.Format("Angel : {0}-{1}-{2}", JointType.SpineMid, JointType.SpineShoulder, "Yaxis")]);
            else
                CalculedValues.Add(string.Format("Distance {0}-{1}-{2} P.Frame ", JointType.SpineMid, JointType.SpineShoulder, "Yaxis"), CalculedValues[string.Format("Angel : {0}-{1}-{2}", JointType.SpineMid, JointType.SpineShoulder, "Yaxis")] - FrameResultParams.Resultat[p_f_index].CalculedValues[string.Format("Angel : {0}-{1}-{2}", JointType.SpineMid, JointType.SpineShoulder, "Yaxis")]);

            AddDeffernceAngel(JointType.HipRight, JointType.KneeRight, JointType.SpineMid, JointType.SpineShoulder, C, p_f_index);
            AddDeffernceAngel(JointType.HipLeft, JointType.KneeLeft, JointType.SpineMid, JointType.SpineShoulder, C, p_f_index);
            AddDeffernceAngel(JointType.SpineMid, JointType.SpineShoulder, JointType.ShoulderRight, C, p_f_index);

            AddDeffernceAngel(JointType.SpineShoulder, JointType.ShoulderRight, JointType.ElbowRight, C, p_f_index);
            AddDeffernceAngel(JointType.ShoulderRight, JointType.ElbowRight, JointType.WristRight, C, p_f_index);
            AddDeffernceAngel(JointType.SpineMid, JointType.SpineShoulder, JointType.ShoulderLeft, C, p_f_index);
            AddDeffernceAngel(JointType.SpineShoulder, JointType.ShoulderLeft, JointType.ElbowLeft, C, p_f_index);
            AddDeffernceAngel(JointType.ShoulderLeft, JointType.ElbowLeft, JointType.WristLeft, C, p_f_index);
            #endregion

            // from floor 
            #region Floor
            foreach (JointType t in C.JointTypes)
                CalculedValues.Add(string.Format("Distance of {0} from Floor", t), C.jointDistanceFromTheFloor(source[t]));
            foreach (JointType t in C.JointTypes)
                if (p_f_index == -1)
                    CalculedValues.Add(string.Format("Difference Distance of {0}  from Floor P.Frame", t), 0); // CalculedValues[string.Format("Distance of {0} from Floor", t)] - Resultat[p_f_index].CalculedValues[string.Format("Distance of {0} from Floor", t)]);
                else CalculedValues.Add(string.Format("Difference Distance of {0}  from Floor P.Frame", t), CalculedValues[string.Format("Distance of {0} from Floor", t)] - FrameResultParams.Resultat[p_f_index].CalculedValues[string.Format("Distance of {0} from Floor", t)]);


            CalculedValues.Add(string.Format("Distance of Center from Floor"), C.CenterToFloor(source[JointType.SpineMid], source[JointType.SpineBase]));
            #endregion

            //Velocity 
            #region Velocity
            double TimeDifference = p_f_index == -1 ? TimeOfFrame : TimeOfFrame - FrameResultParams.Resultat[p_f_index].TimeOfFrame;

            double Velocitys = 0;
            foreach (JointType t in C.JointTypes)
            {
                double v = 1000 * (CalculedValues[string.Format("Difference Distance {0} P.Frame ", t)] / TimeDifference);
                CalculedValues.Add(string.Format("Velocity of {0}", t), v);
                Velocitys += v;
            }
            CalculedValues.Add(string.Format("Velocity Avrage"), Velocitys / 25);
            #endregion

            #region Euclidiens Distances
            JointType t1;
            JointType t2;

            t1 = JointType.ShoulderRight;
            t2 = JointType.HandRight;
            CalculedValues.Add(string.Format("Distance {0} - {1} kinect", t1, t2), C.jointDistance2D(source[t1], source[t2]));
            
            t1 = JointType.ShoulderLeft;
            t2 = JointType.HandLeft;
            CalculedValues.Add(string.Format("Distance {0} - {1} kinect", t1, t2), C.jointDistance2D(source[t1], source[t2]));
            
            t1 = JointType.SpineBase;
            t2 = JointType.SpineShoulder;
            CalculedValues.Add(string.Format("Distance {0} - {1} kinect", t1, t2), C.jointDistance2D(source[t1], source[t2]));
            
            t1 = JointType.HipRight;
            t2 = JointType.FootRight;
            CalculedValues.Add(string.Format("Distance {0} - {1} kinect", t1, t2), C.jointDistance2D(source[t1], source[t2]));
            
            t1 = JointType.HipLeft;
            t2 = JointType.FootLeft;
            CalculedValues.Add(string.Format("Distance {0} - {1} kinect", t1, t2), C.jointDistance2D(source[t1], source[t2]));

            t1 = JointType.SpineShoulder;
            t2 = JointType.Head;
            CalculedValues.Add(string.Format("Distance {0} - {1} kinect", t1, t2), C.jointDistance2D(source[t1], source[t2]));

            #endregion

            CalculedValues.Add(string.Format("Ab. Activite"), Convert.ToInt32( Ab_activ) );
        }

        public void fromFrameResult(FrameResult2 fr)
        {
            //source2 = fr.source2;
            source = new Dictionary<JointType, Joint>();
            foreach (var v in fr.source)
                source.Add(v.Type, v.Joint);
            source2 = new Dictionary<JointType, Point>();
            foreach (var v in fr.source2)
                source2.Add(v.Type, v.Joint);
            BodyIndex = fr.BodyIndex;
            RelativeTime = fr.RelativeTime;
            Ab_activ = fr.Ab_activ;
            Confirmed = fr.Confirmed;
            Message = fr.Message;
            size1 = fr.size1;
            size2 = fr.size2;
            try
            {
                FrameImage = System.Drawing.Image.FromFile(string.Format(@"{0}\{1}", FrameResultParams.TempDirectory, FrameImageFileName));
                savedTemp = true;
            }
            catch
            {

            }
        }
        }
    

    [Serializable]
    public class FrameResult2
    {
        public FrameResult2() { }
        public FrameResult2(FrameResult f)
        {
            fromFrameResult(f);
        }
        //public Dictionary<JointType, Point> source2;
        public List<JointT> source;
        public List<JointT2> source2;
        int h=0;
        int m=0;
        int s=0;
        long ms=0;
        public TimeSpan RelativeTime { get { return new TimeSpan(ms); } }

        public Double TimeOfFrame
        {
            get { return RelativeTime.TotalMilliseconds; }
        }
        public int BodyIndex;
        bool ab_activ = true;
        bool confirmed = true;
        string message = "";
        public System.Windows.Size size1;
        public System.Windows.Size size2;
        public string FrameImageFileName
        {
            get
            {
                return string.Format("{0}.jpeg", RelativeTime.ToString().Replace(':', ' ').Replace('.', ' '), BodyIndex); //_{1}
            }
        }
        public string BodyImageFileName
        {
            get
            {
                return string.Format("{0}_{1}_F.jpeg", RelativeTime.ToString().Replace(':', ' ').Replace('.', ' '), BodyIndex);
            }
        }
        public System.Windows.Rect Rectangle
        {
            get
            {
                if (source == null)
                    return new Rect();
                else
                {
                    Point min = new Point();
                    Point max = new Point();
                    int i = 0;

                    foreach (var v in source)
                    {
                        Point p = new Point(v.Joint.Position.X, v.Joint.Position.Y);
                        if (i == 0)
                        {
                            min = new Point(p.X, p.Y);
                            max = new Point(p.X, p.Y);
                        }
                        else
                        {
                            min.X = Math.Min(min.X, p.X);
                            min.Y = Math.Min(min.Y, p.Y);
                            max.X = Math.Max(max.X, p.X);
                            max.Y = Math.Max(max.Y, p.Y);
                        }

                        i++;
                    }

                    System.Windows.Rect res = new Rect(min, max);


                    return res;
                }
            }
        }
        public bool Ab_activ { get => ab_activ; set => ab_activ = value; }
        public bool Confirmed { get => confirmed; set => confirmed = value; }
        public string Message { get => message; set => message = value; }
        public string RelativeTimeString { get { return RelativeTime.ToString().Replace(':', ' ').Replace('.', ' ') + "_" + BodyIndex; } }

        public int H { get => h; set => h = value; }
        public int M { get => m; set => m = value; }
        public int S { get => s; set => s = value; }
        public long Ms { get => ms; set => ms = value; }

        public void fromFrameResult(FrameResult fr)
        {
            //source2 = fr.source2;
            source = new List<JointT>();
            foreach (var v in fr.source)
                source.Add(new JointT( v.Key, v.Value));
            source2 = new List<JointT2>();
            foreach (var v in fr.source2)
                source2.Add(new JointT2(v.Key, v.Value));
            BodyIndex = fr.BodyIndex;
            h = fr.RelativeTime.Hours;
            m = fr.RelativeTime.Minutes;
            s = fr.RelativeTime.Seconds;
            ms = fr.RelativeTime.Ticks;
            Ab_activ = fr.Ab_activ;
            Confirmed = fr.Confirmed;
            Message = fr.Message;
            size1 = fr.size1;
            size2 = fr.size2;
        }

    }
    [Serializable]
    public class JointT
    {
        JointType type;
        Joint joint;
        public JointT() { }
        public JointT(JointType t , Joint j)
        {
            type = t;
            joint = j;
        }
        public JointType Type { get => type; set => type = value; }
        public Joint Joint { get => joint; set => joint = value; }
    }
    public class JointT2
    {
        JointType type;
        Point joint;
        public JointT2() { }
        public JointT2(JointType t, Point j)
        {
            type = t;
            joint = j;
        }
        public JointType Type { get => type; set => type = value; }
        public Point Joint { get => joint; set => joint = value; }
    }
}
