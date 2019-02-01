namespace ASCOM.Foculus
{
    partial  class Camera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Camera));
            this.axFGControlCtrl1 = new AxFGControlLib.AxFGControlCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.axFGControlCtrl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axFGControlCtrl1
            // 
            this.axFGControlCtrl1.Enabled = true;
            this.axFGControlCtrl1.Location = new System.Drawing.Point(55, 61);
            this.axFGControlCtrl1.Name = "axFGControlCtrl1";
            this.axFGControlCtrl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axFGControlCtrl1.OcxState")));
            this.axFGControlCtrl1.Size = new System.Drawing.Size(511, 342);
            this.axFGControlCtrl1.TabIndex = 0;
            // 
            // Camera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 444);
            this.Controls.Add(this.axFGControlCtrl1);
            //this.Name = "Camera";
            this.RightToLeftLayout = true;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmAxFG_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axFGControlCtrl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxFGControlLib.AxFGControlCtrl axFGControlCtrl1;
    }
}