namespace GigEApi
{
    partial class CameraControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          this.Gainlabel = new System.Windows.Forms.Label();
          this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
          this.labelExposuretimeLabel = new System.Windows.Forms.Label();
          this.numericUpDownExposure = new System.Windows.Forms.NumericUpDown();
          this.labelFramerate = new System.Windows.Forms.Label();
          this.trackBarFramerate = new System.Windows.Forms.TrackBar();
          this.textBoxFramerate = new System.Windows.Forms.TextBox();
          ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExposure)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.trackBarFramerate)).BeginInit();
          this.SuspendLayout();
          // 
          // Gainlabel
          // 
          this.Gainlabel.AutoSize = true;
          this.Gainlabel.BackColor = System.Drawing.Color.AliceBlue;
          this.Gainlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.Gainlabel.ForeColor = System.Drawing.SystemColors.InfoText;
          this.Gainlabel.Location = new System.Drawing.Point(12, 102);
          this.Gainlabel.Name = "Gainlabel";
          this.Gainlabel.Size = new System.Drawing.Size(70, 17);
          this.Gainlabel.TabIndex = 0;
          this.Gainlabel.Text = "Gain [db]:";
          // 
          // numericUpDown1
          // 
          this.numericUpDown1.DecimalPlaces = 1;
          this.numericUpDown1.Location = new System.Drawing.Point(187, 99);
          this.numericUpDown1.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
          this.numericUpDown1.Name = "numericUpDown1";
          this.numericUpDown1.Size = new System.Drawing.Size(62, 20);
          this.numericUpDown1.TabIndex = 1;
          this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
          // 
          // labelExposuretimeLabel
          // 
          this.labelExposuretimeLabel.AutoSize = true;
          this.labelExposuretimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.labelExposuretimeLabel.ForeColor = System.Drawing.SystemColors.InfoText;
          this.labelExposuretimeLabel.Location = new System.Drawing.Point(12, 150);
          this.labelExposuretimeLabel.Name = "labelExposuretimeLabel";
          this.labelExposuretimeLabel.Size = new System.Drawing.Size(139, 16);
          this.labelExposuretimeLabel.TabIndex = 2;
          this.labelExposuretimeLabel.Text = "ExposureTime [usec]:";
          // 
          // numericUpDownExposure
          // 
          this.numericUpDownExposure.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
          this.numericUpDownExposure.Location = new System.Drawing.Point(187, 150);
          this.numericUpDownExposure.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
          this.numericUpDownExposure.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
          this.numericUpDownExposure.Name = "numericUpDownExposure";
          this.numericUpDownExposure.Size = new System.Drawing.Size(120, 20);
          this.numericUpDownExposure.TabIndex = 3;
          this.numericUpDownExposure.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
          this.numericUpDownExposure.ValueChanged += new System.EventHandler(this.numericUpDownExposure_ValueChanged);
          // 
          // labelFramerate
          // 
          this.labelFramerate.AutoSize = true;
          this.labelFramerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.labelFramerate.Location = new System.Drawing.Point(12, 33);
          this.labelFramerate.Name = "labelFramerate";
          this.labelFramerate.Size = new System.Drawing.Size(108, 17);
          this.labelFramerate.TabIndex = 4;
          this.labelFramerate.Text = "Framerate [fps]:";
          // 
          // trackBarFramerate
          // 
          this.trackBarFramerate.BackColor = System.Drawing.Color.LightGoldenrodYellow;
          this.trackBarFramerate.Location = new System.Drawing.Point(187, 12);
          this.trackBarFramerate.Minimum = 1;
          this.trackBarFramerate.Name = "trackBarFramerate";
          this.trackBarFramerate.Size = new System.Drawing.Size(168, 45);
          this.trackBarFramerate.TabIndex = 5;
          this.trackBarFramerate.TickStyle = System.Windows.Forms.TickStyle.Both;
          this.trackBarFramerate.Value = 1;
          this.trackBarFramerate.Scroll += new System.EventHandler(this.trackBarFramerate_Scroll);
          // 
          // textBoxFramerate
          // 
          this.textBoxFramerate.Location = new System.Drawing.Point(374, 30);
          this.textBoxFramerate.Name = "textBoxFramerate";
          this.textBoxFramerate.ReadOnly = true;
          this.textBoxFramerate.Size = new System.Drawing.Size(83, 20);
          this.textBoxFramerate.TabIndex = 6;
          // 
          // CameraControl
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.Color.AliceBlue;
          this.ClientSize = new System.Drawing.Size(469, 208);
          this.Controls.Add(this.textBoxFramerate);
          this.Controls.Add(this.trackBarFramerate);
          this.Controls.Add(this.labelFramerate);
          this.Controls.Add(this.numericUpDownExposure);
          this.Controls.Add(this.labelExposuretimeLabel);
          this.Controls.Add(this.numericUpDown1);
          this.Controls.Add(this.Gainlabel);
          this.MaximizeBox = false;
          this.MaximumSize = new System.Drawing.Size(600, 600);
          this.MinimizeBox = false;
          this.Name = "CameraControl";
          this.Text = "CameraControl";
          this.Load += new System.EventHandler(this.CameraControl_Load);
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CameraControl_FormClosing);
          ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExposure)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.trackBarFramerate)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Gainlabel;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
      
        private System.Windows.Forms.Label labelExposuretimeLabel;
        private System.Windows.Forms.NumericUpDown numericUpDownExposure;
        private System.Windows.Forms.Label labelFramerate;
        private System.Windows.Forms.TrackBar trackBarFramerate;
        private System.Windows.Forms.TextBox textBoxFramerate;
    }
}