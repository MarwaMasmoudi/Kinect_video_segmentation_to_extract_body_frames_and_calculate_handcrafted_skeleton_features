namespace Microsoft.Samples.Kinect.BodyBasics
{
    partial class FrameResultControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureFrame = new System.Windows.Forms.PictureBox();
            this.pictureFace = new System.Windows.Forms.PictureBox();
            this.checkConfirm = new System.Windows.Forms.CheckBox();
            this.checkEmotion = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFace)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureFrame
            // 
            this.pictureFrame.Location = new System.Drawing.Point(3, 3);
            this.pictureFrame.Name = "pictureFrame";
            this.pictureFrame.Size = new System.Drawing.Size(183, 143);
            this.pictureFrame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureFrame.TabIndex = 0;
            this.pictureFrame.TabStop = false;
            // 
            // pictureFace
            // 
            this.pictureFace.Location = new System.Drawing.Point(192, 3);
            this.pictureFace.Name = "pictureFace";
            this.pictureFace.Size = new System.Drawing.Size(121, 100);
            this.pictureFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureFace.TabIndex = 1;
            this.pictureFace.TabStop = false;
            this.pictureFace.DoubleClick += new System.EventHandler(this.pictureFace_DoubleClick);
            // 
            // checkConfirm
            // 
            this.checkConfirm.AutoSize = true;
            this.checkConfirm.Location = new System.Drawing.Point(193, 109);
            this.checkConfirm.Name = "checkConfirm";
            this.checkConfirm.Size = new System.Drawing.Size(73, 17);
            this.checkConfirm.TabIndex = 2;
            this.checkConfirm.Text = "Confirmed";
            this.checkConfirm.UseVisualStyleBackColor = true;
            this.checkConfirm.CheckedChanged += new System.EventHandler(this.checkConfirm_CheckedChanged);
            // 
            // checkEmotion
            // 
            this.checkEmotion.AutoSize = true;
            this.checkEmotion.Location = new System.Drawing.Point(193, 129);
            this.checkEmotion.Name = "checkEmotion";
            this.checkEmotion.Size = new System.Drawing.Size(78, 17);
            this.checkEmotion.TabIndex = 3;
            this.checkEmotion.Text = "Ab. Normal";
            this.checkEmotion.UseVisualStyleBackColor = true;
            this.checkEmotion.CheckedChanged += new System.EventHandler(this.checkEmotion_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(3, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // toolTip1
            // 
            this.toolTip1.OwnerDraw = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(262, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(262, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 7;
            // 
            // FrameResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkEmotion);
            this.Controls.Add(this.checkConfirm);
            this.Controls.Add(this.pictureFace);
            this.Controls.Add(this.pictureFrame);
            this.Name = "FrameResultControl";
            this.Size = new System.Drawing.Size(317, 159);
            this.DoubleClick += new System.EventHandler(this.FrameResultControl_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.pictureFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureFrame;
        private System.Windows.Forms.PictureBox pictureFace;
        private System.Windows.Forms.CheckBox checkEmotion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.CheckBox checkConfirm;
    }
}
