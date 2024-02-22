using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public partial class FrameResultControl : UserControl
    {
        public FrameResultControl()
        {
            InitializeComponent();
        }

        public FrameResultControl(FrameResult source)
        {
            InitializeComponent();
            this.source  = source;
            if (Source != null)
            {
                AfficherSource();
            }

            Log.WriteSucessLogToFile(string.Format("[{1}][{0}]", Source.RelativeTimeString, Source.BodyIndex), "FrameControl.Load");

        }

        void AfficherSource()
        {
            Log.WriteInfoLogToFile(string.Format("[{1}][{0}]", Source.RelativeTimeString, Source.BodyIndex), "ReadingImages.Show");

            Source.ReadImage();
            try
            {
                Log.WriteInfoLogToFile(string.Format("[{1}][{0}]", Source.RelativeTimeString, Source.BodyIndex), "ReadingFrameImage.Show");

                pictureFrame.Image = Source.FrameImage;
            }

            catch (Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]", ex.Message, Source.RelativeTimeString, Source.BodyIndex), "FrameImage.Show");
            }
            try
            {
                Log.WriteInfoLogToFile(string.Format("[{1}][{0}]", Source.RelativeTimeString, Source.BodyIndex), "ReadingBodyImages.Show");

                pictureFace.Image = Source.BodyImage;
            }

            catch (Exception ex)
            {
                Log.WriteErrorLogToFile(string.Format("{0} [{2}][{1}]",  ex.Message, Source.RelativeTimeString, Source.BodyIndex), "BodyImage.show");
            }

            //label3.Text = string.Format("{0}/{1}", Source.rectangle2.X, Source.Rectangle.X);
            //label4.Text = string.Format("{0}/{1}", Source.rectangle2.Y, Source.Rectangle.Y);


            if (Source.FrameImage == null)
                Source.Confirmed = false;


            checkConfirm.Checked = Source.Confirmed;
            checkConfirm_CheckedChanged(new object(), new EventArgs());

            checkEmotion.Checked = Source.Ab_activ;
            label1.Text = Source.Message;
            label2.Text = Source.RelativeTimeString;
            if (Source.Message != "")
            {
                toolTip1.ToolTipTitle = Source.Message;
                toolTip1.Active = true;
            }
            else toolTip1.Active = false;
        }

        FrameResult source;

        public FrameResult Source
        {
            get { return source; }
            set
            {
                source = value;
            }
        }

        private void checkConfirm_CheckedChanged(object sender, EventArgs e)
        {
            checkEmotion.Visible = checkConfirm.Checked;
            pictureFace.Visible  = pictureFrame.Visible = checkConfirm.Checked;

            Source.Confirmed = checkConfirm.Checked;

        }

        private void checkEmotion_CheckedChanged(object sender, EventArgs e)
        {
            Source.Ab_activ = checkEmotion.Checked;
        }

        private void FrameResultControl_DoubleClick(object sender, EventArgs e)
        {
            AfficherSource();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            //if(Source.Message != "")
            //toolTip1.Show(source.Message , label1 );
        }

        private void pictureFace_DoubleClick(object sender, EventArgs e)
        {

            Source.ReadBodyImage();
            try
            {
                pictureFace.Image = Source.BodyImage;
            }
            catch(Exception ex)
            {

                Log.WriteErrorLogToFile(string.Format("{2} [{1}][{0}]", Source.RelativeTimeString, Source.BodyIndex , ex.Message ), "ReadingBodyImage.DoubleClick");
            }

            //label3.Text = string.Format("{0}/{1}", Source.rectangle2.X, Source.Rectangle.X);
            //label4.Text = string.Format("{0}/{1}", Source.rectangle2.Y, Source.Rectangle.Y);

            label1.Text = Source.Message;
            if (Source.Message != "")
            {
                toolTip1.ToolTipTitle = Source.Message;
                toolTip1.Active = true;
            }
            else toolTip1.Active = false;
        }
    }
}
