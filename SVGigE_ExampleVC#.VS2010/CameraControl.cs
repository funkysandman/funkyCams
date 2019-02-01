using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace GigEApi
{
  public partial class CameraControl : Form
  {
    float gain;
    float expotime;
    private GigeApi myApi;
    private Form1 camDisplay;
    private IntPtr hCamera;
    private int framerate_scaling = 10;
    private float framerate = 1.0f;
    private float maxFrames = 40.0f;        
    private bool setRate = false;

    //-----------------------------------------------------------------

    public CameraControl(Form1 cd)
    {
        InitializeComponent();

        camDisplay = cd;
        myApi = camDisplay.myApi;
        numericUpDown1.Enabled = false;

        hCamera = camDisplay.getCamHandle();

        myApi.Gige_Camera_getGain(hCamera, ref gain);
        myApi.Gige_Camera_getExposureTime(hCamera, ref expotime);

        // Adjust settings: frame rate

        myApi.Gige_Camera_getFrameRate(hCamera, ref framerate);
        myApi.Gige_Camera_getFrameRateMax(hCamera, ref maxFrames);

        textBoxFramerate.Text = "" + (int)framerate;

        trackBarFramerate.TickFrequency = (int)(maxFrames * framerate_scaling / 10);
        trackBarFramerate.Maximum = (int)(maxFrames * framerate_scaling);
        trackBarFramerate.Minimum = (int)(1 * framerate_scaling);

        trackBarFramerate.Value = (int)framerate * framerate_scaling;

        numericUpDown1.Value = (decimal)gain;
        numericUpDown1.Enabled = true;

        numericUpDownExposure.Value = (decimal)expotime;
        numericUpDownExposure.Enabled = true;
    }

    //-----------------------------------------------------------------

    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
      gain = (float)numericUpDown1.Value;

      myApi.Gige_Camera_setGain(hCamera, gain);
    }

    //-----------------------------------------------------------------------------

    private void CameraControl_FormClosing(object sender, FormClosingEventArgs e)
    {
      camDisplay.CameraControlClosed();
    }
    //-----------------------------------------------------------------------------

    private void numericUpDownExposure_ValueChanged(object sender, EventArgs e)
    {
      expotime = (float) numericUpDownExposure.Value;

      myApi.Gige_Camera_setExposureTime(hCamera, expotime);
    }
    //-----------------------------------------------------------------------------

    private void trackBarFramerate_Scroll(object sender, EventArgs e)
    {
        if (setRate == false)
        {
            setRate = true;
            framerate = ((float)trackBarFramerate.Value / framerate_scaling);

            myApi.Gige_Camera_setFrameRate(hCamera, framerate);


            myApi.Gige_Camera_getExposureTime(hCamera, ref expotime);
            numericUpDownExposure.Value = (decimal)expotime;
            myApi.Gige_Camera_getFrameRate(hCamera, ref framerate);

            textBoxFramerate.Text = "" + framerate;
        }

        setRate = false;
    }

    private void CameraControl_Load(object sender, EventArgs e)
    {

    }
    //-----------------------------------------------------------------------------
  }
}