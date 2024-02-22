using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace Microsoft.Samples.Kinect.BodyBasics
{
    public partial class FormResult : Form
    {
        public FormResult(List<FrameResult> source)
        {
            InitializeComponent();
            Source = source;
            floderPathtext.Text =FrameResultParams.TempDirectory + @"\" + DateTime.Now.ToString().Replace("/", "_").Replace(":", "_").Replace(".", "_");
        }
        int wrongSource = 0;
        public string FloderPath { get { return floderPathtext.Text; } }

        public List<FrameResult> Source
        {
            get => source;
            set
            {
                source = value;
            }
        }

        DateTime startT;
        DateTime endT;

        List<FrameResult> source;
        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (FloderPath != string.Empty)
            {
                this.Cursor = Cursors.WaitCursor;
                startT = DateTime.Now;
                if (Directory.Exists(FloderPath))
                    Directory.Delete(FloderPath, true);

                    Directory.CreateDirectory(FloderPath);
                SaveImages();
                SaveExcel();

                this.Cursor = Cursors.Default;
                MessageBox.Show("Successfull Save." , string.Format( "{0}" , (endT-startT) ) , MessageBoxButtons.OK , MessageBoxIcon.Information);
                //DialogResult = DialogResult.OK;
            }
        }
        List<FrameResultControl> list = new List<FrameResultControl>();

        private void FormResult_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            comboBox1.Items.Add("");
            comboBox1.Items.Add("Wrong");
            Log.WriteInfoLogToFile("Start of crating results", "FormResult_Shown");
            List<int> indexes = new List<int>();
            foreach (FrameResult r in Source)
            {
                Log.WriteInfoLogToFile(string.Format("[{1}][{0}]", r.RelativeTimeString, r.BodyIndex), "FormResult_Shown");
                FrameResultControl c = new FrameResultControl(r);
                if (r.savedTemp)
                {
                    list.Add(c);
                    Log.WriteSucessLogToFile(string.Format("[{1}][{0}]", r.RelativeTimeString, r.BodyIndex), "FormResult_Shown");
                    if (!indexes.Exists(x => x == r.BodyIndex))
                    {
                        indexes.Add(r.BodyIndex);
                        Log.WriteInfoLogToFile(string.Format("[{1}][{0}]", "", r.BodyIndex), "BodyIndex.Added");

                    }
                }
                else
                    Log.WriteErrorLogToFile(string.Format("[{1}][{0}] : {2}", r.RelativeTimeString, r.BodyIndex , r.Message ), "FormResult_Shown");
            }
            foreach (int i in indexes)
                comboBox1.Items.Add(i.ToString());

            Log.WriteSucessLogToFile(string.Format("[{0} Controls]", list.Count), "Form.Load");

            Afficher();

            Cursor = Cursors.Default;
        }

        private void Afficher()
        {
            flowLayoutPanel1.Controls.Clear();
            foreach (FrameResultControl c in list)
            {
                Log.WriteInfoLogToFile(string.Format("[{1}][{0}]", c.Source.RelativeTimeString, c.Source.BodyIndex), "Control.Show");

                switch (comboBox1.SelectedIndex)
                {
                    case -1:
                        flowLayoutPanel1.Controls.Add(c);
                        break;
                    case 0:
                        flowLayoutPanel1.Controls.Add(c);
                        break;
                    case 1:
                        if (c.Source.Message.Contains("Wrong"))
                            flowLayoutPanel1.Controls.Add(c);
                        break;
                    default:
                        int i = Convert.ToInt32(comboBox1.Text);
                        if (c.Source.BodyIndex == i)
                            flowLayoutPanel1.Controls.Add(c);
                        break;
                }
                Log.WriteSucessLogToFile(string.Format("[{1}][{0}]", c.Source.RelativeTimeString, c.Source.BodyIndex), "Control.Show");

            }
            label2.Text = flowLayoutPanel1.Controls.Count.ToString();
            Log.WriteSucessLogToFile(string.Format("[{0} Controls]", list.Count), "Form.Show");

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if (o.ShowDialog() == DialogResult.OK)
                floderPathtext.Text = o.FileName;
        }

        string count
        {
            get
            {
                int i = 0;
                int errorC = 0;
                foreach (FrameResult f in Source)
                {
                    if (f.Confirmed) i++;
                    if (f.Message != "") errorC++;
                }
                return string.Format("{1}/{0} Frames{2}{3}", Source.Count, i, errorC == 0 ? "" : string.Format(" {0} erreurs.", errorC), wrongSource == 0 ? "" : string.Format("    {0} Wrong Frames.", wrongSource));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        void Calculer()
        {
            #region calculate data 
            DataTable dt = new DataTable();

            Dictionary<int, int> lastIndex = new Dictionary<int, int>();

            int index = -1;
            foreach (FrameResult r in Source)
            {
                index++;
                if (r.Confirmed)
                {
                    int i = -1;
                    if (lastIndex.ContainsKey(r.BodyIndex))
                    {
                        i = lastIndex[r.BodyIndex];
                        lastIndex[r.BodyIndex] = index;
                    }
                    else
                        lastIndex.Add(r.BodyIndex, index);

                    r.Calculer(i);
                }
            }

            #endregion
        }
        void SaveExcel()
        {
            if (FloderPath != string.Empty)
            {
                Calculer();

                #region generete the excel file 
                string excelFile = string.Format(@"{0}\{1}.{2}", FloderPath, "ExcelResult", "xlsx");


                // CREATE EXCEL
                var excelApp = new Excel.Application();
                //excelApp.Visible = true;

                var excelWorkbook = excelApp.Workbooks.Add("");

                // EXCEL HEADER
                var rowIndex = 1;
                var columnIndex = 1;

                if (Source.Count > 0)
                {
                    int ind = Source.FindIndex(x => x.CalculedValues.Count > 0);

                    if(ind>=0)
                    foreach (var s in Source[ind].CalculedValues)
                    {
                            excelApp.Cells[rowIndex, columnIndex].Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red);
                            excelApp.Cells[rowIndex, columnIndex++] = s.Key;
                        }

                }

                // MAIN EXCEL BODY

                for (int i = 0; i < Source.Count; i++)
                    if (Source[i].Confirmed)
                    {
                        rowIndex++;
                        columnIndex = 1;
                        foreach (var s in Source[i].CalculedValues)
                        {
                            excelApp.Cells[rowIndex, columnIndex++] = s.Value.ToString().Replace(",", ".");
                        }
                    }

                // CLOSE EXCEL FILE
                if (excelWorkbook != null)
                {
                    excelWorkbook.SaveAs(excelFile);
                    //excelWorkbook.Close();
                    excelApp.Quit();
                }
                endT = DateTime.Now;
                if (MessageBox.Show("Do you want to open the Excel File?" , "Excel File Result" , MessageBoxButtons.YesNo , MessageBoxIcon.Question ) == DialogResult.Yes)
                    System.Diagnostics.Process.Start(excelFile);
                #endregion
            }
        }

        private void SaveImages()
        {
            if (FloderPath != string.Empty)
                for (int i = 0; i < Source.Count; i++)
                    if (Source[i].Confirmed)
                    {
                        if (Source[i].BodyImage == null)
                            ;
                        Parametres.ImageConverter.SaveImage(Source[i].BodyImage, FloderPath + @"\" + Source[i].BodyImageFileName);
                        //if (!Parametres.ImageConverter.SaveImage(Source[i].BodyImage, FloderPath + @"\" + Source[i].BodyImageFileName))
                        //    MessageBox.Show(string.Format("Error to Save Body Iamge {0}.", Source[i].BodyImageFileName), "Error");
                    }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Afficher();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control f in flowLayoutPanel1.Controls)
            {
                ((FrameResultControl)f).checkConfirm.Checked = checkBox1.Checked;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    }
