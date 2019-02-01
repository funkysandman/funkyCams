using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GigEApi
{
    public partial class frmTest : Form
    {

        public GigeApi myApi;
        SVS_Wrapper.SVS_Vistek_Grabber cam;

        public frmTest()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {


            if (disposing)
            {

                //if (components != null)
                //{
                //    components.Dispose();
                //}
            }
            base.Dispose(disposing);
        }
        private void frmTest_Load(object sender, EventArgs e)
        {

        }
        [STAThread]
        static void Main()
        {
            Application.Run(new frmTest());
        }

                private void button1_Click(object sender, EventArgs e)
                {
                    cam = new SVS_Wrapper.SVS_Vistek_Grabber();


                    cam.OpenCamera();
                }

                private void button2_Click(object sender, EventArgs e)
                {
                   // myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.A);
                    cam.setGain((float)0.5,1,1,0);
                    cam.startCapture();
                }

                private void button3_Click(object sender, EventArgs e)
                {
                    cam.stopCapture();
                    while (cam.isWorking)
                    {
                        Thread.Sleep(10);
                    }
                   Bitmap b =cam.Image();
                    pictureBox1.Image = b;

                    //myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
 
                }
     

    }
}
