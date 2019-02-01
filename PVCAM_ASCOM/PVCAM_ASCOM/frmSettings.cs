using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Photometrics.Pvcam;
using System.IO;



/***********************************************************/
/**** THIS IS A SAMPLE APPLICATION FOR DEMONSTRATION OF ****/
/** GENERAL PVCAM USAGE, BEST PRACTICES, SAFE APPRAOCHES ***/
/****** AND USE OF .NET WRAPPER FOR NATIVE PVCAM DLL *******/
/**************** PHOTOMETRICS (C) 2016 ********************/
/***********************************************************/

//NOTE: C# and .NET platform may not be the right solution for
//aplication development where speed and camera frame rate is
//a critical factor due to overhead in communication between
//unmanaged environment in which PVCAM runs and the managed
//environment of a .NET application such as this one.

//NOTE: Frame number displayed in the frame viewer shows two
//values: "Disp" and "Cam". "Disp" value shows number of frames
//that were received in the application and displayed on the
//screen which involves raw pixels conversion to 32bpp format,
//calculation of image statisics and display of image as BMP.
//Further displayed frame rate decrease will be observed with
//image scaling turned on. If the application only had to re-
//ceive the raw frame pixels without having to do further pro-
//cessing it is likely a .NET application would be able to cap-
//ture all the frames from camera in most cases.

namespace pvcam_helper
{
    //this class handles all UI events and user interactions
    //it receives notifications and messages via events
    //from the camera class

    public partial class FrmSettings : Form
    {
        //camera object we will be working with
        PVCamCamera ActiveCamera;
        //Form to display images
        //FrmFrameView FrameViewer;
        //About form
        //AboutForm FrmAbout;

        volatile Boolean m_exiting;
               
        //Class to tag the contols on post processing TAB to track which control
        //reperesents which feature.
        class ControlTag
        {
            Int32 m_ppFeature;
            Int32 m_ppFunction;
            public Int32 PP_Feature
            {
                get { return m_ppFeature; }
                set { m_ppFeature = value; }
            }

            public Int32 PP_Function
            {
                get { return m_ppFunction; }
                set { m_ppFunction = value; }
            }
        }


        delegate void UpdateControlsCallback(List<ComboBox> cbxList,
                                             List<Button> btnList,
                                             List<TextBox> tbxList,
                                             List<TrackBar> tbrList,
                                             List<CheckBox> chboxList,
                                             List<ToolStripMenuItem> mnuList,
                                             List<GroupBox> gboxList,
                                             List<Panel> panelList,
                                             Boolean state);
        delegate void UpdateControlsButton(List<Button> btnList);

        //delegates needed to update form objects through the thread they
        //were created on
        delegate void SetTextCallback(String text, FrmSettings form);
        delegate void SetListBoxCallback(String text, FrmSettings form);

        public FrmSettings(PVCamCamera p)
        {
            m_exiting = false;

            InitializeComponent();
            ActiveCamera = p;
           
            //FrameViewer = new FrmFrameView();
            SubscribeToReportMessages(ActiveCamera);
            SubscribeToAcquisitionNotifications(ActiveCamera);
            ActiveCamera.ReadCameraParams();
            //FrameViewer.Hide();
        }

        private void FrmTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            //m_exiting = true;
            //ActiveCamera.StopAcquisition();
            //ActiveCamera.WaitForFullAcquisitionStop();
            //ActiveCamera.CloseCamera();
        }

        /*Setup the acquisition for single snap*/
        private void btnSingleCapture_Click(object sender, EventArgs e)
        {
            if (!ActiveCamera.AcqSetup(PVCamCamera.AcqTypes.ACQ_TYPE_SINGLE))
            {
                return;
            }

           

            //SetupFrameViewer(FrameViewer);

            //if acqusition setup succeeded, start the acquisition
            if (!ActiveCamera.StartSeqAcq())
            {
                return;
            }
        }

        /*start acquisition in continuous mode with circular buffer*/
       

        //stop single or continuous acquisition


        //convert the image to BMP and display it
   
        //calculate latest image mean, minimum and maximum
  
        //adjust the form and picture box size for the image display
        //private void SetupFrameViewer(FrmFrameView form)
        //{


        //    form.pbxFrame.Height = 4 + ActiveCamera.ImageSizeY;
        //    form.pbxFrame.Width = 4 + ActiveCamera.ImageSizeX;
           
        //    form.pbxFrame.SizeMode = PictureBoxSizeMode.Normal;
        //    if (form.pbxFrame.Width > 900)
        //    {
        //        form.pbxFrame.Width = 900;
        //        form.pbxFrame.Height = (Int32)(900.0 / ((Double)ActiveCamera.ImageSizeX / (Double)ActiveCamera.ImageSizeY));
        //        form.pbxFrame.SizeMode = PictureBoxSizeMode.StretchImage;
        //    }

        //    if (form.pbxFrame.Height > 700) 
        //    {
        //        form.pbxFrame.Height = 700;
        //        form.pbxFrame.Width = (Int32)(700.0 / ((Double)ActiveCamera.ImageSizeY / (Double)ActiveCamera.ImageSizeX));
        //        form.pbxFrame.SizeMode = PictureBoxSizeMode.StretchImage;
        //    }

        //    form.pbxFrame.Location = new Point(1, 1);
        //    form.Size = new Size(FrameViewer.pbxFrame.Width + 180, FrameViewer.pbxFrame.Height + 100);
        //    if (form.Height < 380)
        //        form.Height = 380;
        //    form.Show();
        //    form.WindowState = FormWindowState.Normal;
        //    form.BringToFront();
        //}

        //set camera exposure time
  

        //set camera readout speed


        //set camera trigger mode


        //set sensor clearing mode
        private void cbxClearMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ActiveCamera.SetClearMode(cbxClearMode.Items[cbxClearMode.SelectedIndex].ToString());
        }

        //set camera binning
        private void cbxBinning_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveCamera.SetBinning(cbxBinning.Items[cbxBinning.SelectedIndex].ToString());
        }

        //set camera clear cycles number
        private void cbxClearCycles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveCamera.SetClearCycles(Convert.ToInt16(cbxClearCycles.Items[cbxClearCycles.SelectedIndex]));
        }

        //set camera clocking mode
        private void cbxClkMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveCamera.SetClockingMode(cbxClkMode.Items[cbxClkMode.SelectedIndex].ToString());
        }

        //set number of frames to get in circular buffer (continuous) mode


        //refresh list of available cameras


        //open the selected camera


        //disable the unavailable buttons and refresh list of cameras
        private void frmSettings_Load(object sender, EventArgs e)
        {
            //UpdateControls(new List<ComboBox> {cbxFramesToGet, cbxExpTime},
            //               new List<Button> { btnSingleCapture, btnRunCont, btnStop, btnInitOpen, btnClose },
            //               new List<TextBox> { },
            //               new List<TrackBar> { },
            //               new List<CheckBox> { },
            //               new List<ToolStripMenuItem> { mnu_advSettings },
            //               new List<GroupBox> {gboxSS,gboxROI,gboxSettings,gboxTrigger,gboxCooling},
            //               new List<Panel> { panelPP},
            //               false);
           // PVCamCamera.RefreshCameras(ActiveCamera);
        }

        //close camera and set button states
        private void btnClose_Click(object sender, EventArgs e)
        {
            ActiveCamera.StopAcquisition();
            ActiveCamera.CloseCamera();

            //UpdateControls(new List<ComboBox> { cbxFramesToGet, cbxExpTime },
            //              new List<Button> { btnSingleCapture, btnRunCont, btnStop, btnInitOpen, btnClose },
            //              new List<TextBox> { },
            //              new List<TrackBar> { },
            //              new List<CheckBox> { },
            //              new List<ToolStripMenuItem> { mnu_advSettings },
            //              new List<GroupBox> { gboxSS, gboxROI, gboxSettings, gboxTrigger, gboxCooling },
            //              new List<Panel> { panelPP },
            //              false);
            //UpdateControls(new List<ComboBox> { },
            //               new List<Button> { btnRefreshCams, btnInitOpen },
            //               new List<TextBox> { },
            //               new List<TrackBar> { },
            //               new List<CheckBox> { },
            //               new List<ToolStripMenuItem> { },
            //               new List<GroupBox> {  },
            //                new List<Panel> {  },
            //               true);
        }

        //while EM gain track bar is being dragged only update the EM gain text box
        private void tbrEMGain_ValueChanged(object sender, EventArgs e)
        {
            tbxEMGain.Text = tbrEMGain.Value.ToString();
        }

        //once EM gain track bar is released, set the EM gain
        private void tbrEMGain_MouseCaptureChanged(object sender, EventArgs e)
        {
            ActiveCamera.SetEMGain((UInt16)tbrEMGain.Value);
        }

        //once user entered new value into EM gain text box set the value to
        //track bar and send the value to the camera
        private void tbxEMGain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Int32 eMGaiValue = Convert.ToInt32(tbxEMGain.Text);
                if (eMGaiValue < 1)
                    tbxEMGain.Text = "1";
                if (eMGaiValue > ActiveCamera.MultGainMax)
                    tbxEMGain.Text = ActiveCamera.MultGainMax.ToString();

                tbrEMGain.Value = Convert.ToInt32(tbxEMGain.Text);
                ActiveCamera.SetEMGain(Convert.ToUInt16(tbxEMGain.Text));
            }
        }

        //set camera gain state (analog gain)
        private void cbxGainStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveCamera.GainStateIndex!=(Int16)cbxGainStates.SelectedIndex)
            ActiveCamera.SetGainState((Int16)cbxGainStates.SelectedIndex);
        }

        //handle manual entry of exposure time (confirmed by Enter)


        //handle manual entry of number of frames to get (confirmed by Enter)
     

     
        

        //private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    FrmAbout = new AboutForm();
        //    FrmAbout.ShowDialog();
        //}

        //save the currently displayed image on disk to a BMP file
   

        //set last error message label on the FrmTester form
        //needs to handle cross-thread access


        //write status and error messages to listbox on the FrmTester form
        //needs to handle cross-thread access
        private void SetListBoxItem(String msgToPrint, FrmSettings form)
        {
            if (form.LbxStatusList.InvokeRequired)
            {
                SetListBoxCallback d = new SetListBoxCallback(SetListBoxItem);
                form.Invoke(d, new object[] { msgToPrint, form });
            }
            else
            {
                form.LbxStatusList.Items.Add(msgToPrint);
                form.LbxStatusList.SelectedIndex = form.LbxStatusList.Items.Count - 1;
            }
        }

        //create error message and write it to the label and listbox on the FrmTester form
        public void ReportErr(Label lblToPrintTo, ListBox lbxToPrintTo, String msgToPrint, FrmSettings form)
        {
            StringBuilder errMsg = new StringBuilder(PvTypes.ERROR_MSG_LEN);
            short erCd = PVCAM.pl_error_code();
            PVCAM.pl_error_message(erCd, errMsg);
            String MsgToPrint = String.Format("{0}, ErrMsg: {1}", msgToPrint, errMsg);
            //SetLblToPrintTo(MsgToPrint, form);
            SetListBoxItem(MsgToPrint, form);
        }

        //report status message to the listbox on the FrmTester form
        public void ReportStat(ListBox lbxToPrintTo, String msgToPrint, FrmSettings form)
        {
            StringBuilder errMsg = new StringBuilder(PvTypes.ERROR_MSG_LEN);
            PVCAM.pl_error_message(PVCAM.pl_error_code(), errMsg);
            String MsgToPrint = String.Format("{0}", msgToPrint);
            SetListBoxItem(MsgToPrint, form);
        }

        //receive messages from the PVCamCamera class
        public void SubscribeToReportMessages(PVCamCamera pvcc)
        {
            pvcc.ReportMsg += new PVCamCamera.ReportHandler(ReportReceived);
        }

        //receive notifications from the PVCamCamera class
        public void SubscribeToAcquisitionNotifications(PVCamCamera pvcc)
        {
            pvcc.CamNotif += new PVCamCamera.CameraNotificationsHandler(CameraNotificationReceived);
        }

        //handle camera notifications - mostly buttons states changes
        private void CameraNotificationReceived(PVCamCamera pvcc, ReportEvent evtType)
        {
            if (m_exiting)
            {
                return;
            }

            switch (evtType.NotifEvent)
            {
                case CameraNotifications.ACQ_SINGLE_STARTED:
                   

                case CameraNotifications.ACQ_SINGLE_CANCELLED:
                case CameraNotifications.ACQ_SINGLE_FAILED:
                   

                case CameraNotifications.ACQ_CONT_STARTED:
  
                    break;

                case CameraNotifications.ACQ_CONT_FINISHED:
                  

                case CameraNotifications.ACQ_CONT_CANCELED:
                case CameraNotifications.ACQ_CONT_FAILED:
                   

                case CameraNotifications.ACQ_NEW_FRAME_RECEIVED:
                 
                case CameraNotifications.NO_CAMERA_FOUND:
                 

                case CameraNotifications.CAMERA_REFRESH_DONE:
      

                case CameraNotifications.CAMERA_OPENED:
                    break;

                case CameraNotifications.SPEED_TABLE_BUILD_DONE:
                    cbxRdOutSpd.Items.Clear();
                    for (int i = 0; i < ActiveCamera.SpeedTable.ReadoutOption.Count; i++)
                    {
                        cbxRdOutSpd.Items.Add(ActiveCamera.SpeedTable.ReadoutOption[i].PortDesc);
                    }
                    break;

                //once camera parameters are read out pre-set user interface
                case CameraNotifications.CAMERA_PARAM_READOUT_COMPLETE:
               
                    //Update Temperature controls
                    tbxSetpoint.Text = String.Format("{0:0.00}", ActiveCamera.CurrentSetpoint / 100.00);

                    lblTempRange.Text = String.Format("{0} .. {1}", ActiveCamera.MinSetpoint / 100.00,
                                                                   ActiveCamera.MaxSetpoint / 100.00);
                    lboxCurTemp.Text = String.Format("{0:0.00}", ActiveCamera.CurrentTemperature / 100.00);

                    //Get Clocking modes on the camera as List of strings and populate the combo box
                    List<String> clkModeList = new List<String>();
                    ActiveCamera.GetCockingModes(clkModeList);

                    cbxClkMode.Items.Clear();
                    for (Int32 i = 0; i < clkModeList.Count; i++)
                    {
                        cbxClkMode.Items.Add(clkModeList[i]);
                    }

                    //Get Trigger modes available on the camera and populate the combo box
                    List<String> trgModes = new List<String>();
                    ActiveCamera.GetTriggerModes(trgModes);

  

                    //Get Expose out mode and update combo box
                    //Check if camera supports Expose Out Modes


                    //Get clearing modes available and update combo box
                    List<String> clrModes = new List<String>();

                    ActiveCamera.GetClearModes(clrModes);

                    cbxClearMode.Items.Clear();
                    for (Int32 i = 0; i < clrModes.Count; i++)
                    {
                        cbxClearMode.Items.Add(clrModes[i]);
                    }
                    //Binning combo box
                    cbxBinning.Items.Clear();
                    //if camera supports Extended binning factors get that information
                    if (ActiveCamera.IsExtBinningSupported)
                    {
                        List<String> extBinnings = new List<String>();
                        ActiveCamera.GetExtBinnings(extBinnings);
                        for (Int32 i = 0; i < extBinnings.Count; i++)
                        {
                            cbxBinning.Items.Add(extBinnings[i]);
                        }
                    }
                    else
                    {
                        //Camera does not support ext binnings just populate 1,2,4,8 common used bin factors
                        cbxBinning.Items.AddRange(new Object[] { "1","2","4","8" });
                    }

                    cbxBinning.SelectedIndex = 0;

                    //Update exposure time range


                    cbxClkMode.SelectedIndex = ActiveCamera.ClockingModeIndex;
                    
                    cbxRdOutSpd.SelectedIndex = ActiveCamera.SpeedTableIndex;

                    //get speed from camera


                    cbxGainStates.Items.Clear();
                    for (int i = 0; i < ActiveCamera.SpeedTable.ReadoutOption[0].GainStates; i++)
                    {
                        cbxGainStates.Items.Add(i + 1);
                    }
                    cbxGainStates.SelectedIndex = ActiveCamera.GainStateIndex;

                    cbxClearMode.SelectedIndex = ActiveCamera.ClearModeIndex;
                 
                    for (int i = 0; i<cbxClearCycles.Items.Count;i++)
                    {
                        if (Convert.ToInt16(cbxClearCycles.Items[i]) == ActiveCamera.ClearCycles)
                            cbxClearCycles.SelectedIndex = i;

                    }

                    if (ActiveCamera.IsMultGain)
                    {

                        tbxEMGain.Text = ActiveCamera.EMGain.ToString();
                        tbrEMGain.Value = ActiveCamera.EMGain;
                        tbrEMGain.Minimum = 1;
                        tbrEMGain.Maximum = ActiveCamera.MultGainMax;
                        tbxEMGain.Enabled = true;
                        tbrEMGain.Enabled = true;

                    }
                    else
                    {
                        tbxEMGain.Enabled = false;
                        tbrEMGain.Enabled = false;

                    }

                   
                    //Populate Post Processing Tab on the form
                  

                    //Get Fan Speed info and update combobox
                    if (ActiveCamera.IsFanControlAvail)
                    {
                        List<String> fanSpeeds = new List<String>();
                        ActiveCamera.GetFanSpeedSetpoints(fanSpeeds);
                        cboxFanSpeed.Items.Clear();
                        for (Int32 i = 0; i < fanSpeeds.Count; i++)
                        {
                            cboxFanSpeed.Items.Add(fanSpeeds[i]);
                        }
                        
                        //set it to index 0
                        cboxFanSpeed.SelectedIndex = 0;
                        cboxFanSpeed.Enabled = true;

                        //optional read current state from the camera and Display current value, this could
                        //be used for any parameter
                        //cboxFanSpeed.SelectedIndex = ActiveCamera.CurrentFanSpeed;
                    
                    }
                    else //Disable the control
                    {
                        cboxFanSpeed.Enabled = false;
                    }

                    //Frame MetaData
                    cboxMetadata.Checked = false;
                    if (ActiveCamera.IsMetadataAvail)
                    {
                        cboxMetadata.Enabled = true;
                       

                    }
                    else
                    {
                        cboxMetadata.Enabled = false;
                       
                    }
                                               
                                    
                    lstViewMultiROI.Items.Clear();
                    //ROI to full frame CCD
                    tbxMxOrigin.Text = "0";
                    tbxMyOrigin.Text = "0";
                    tbxMxSize.Text = Convert.ToString(ActiveCamera.XSize);
                    tbxMySize.Text = Convert.ToString(ActiveCamera.YSize);

                    //Populate Centroid info
                    cboxCentroidEnable.Checked = false;
                    if (ActiveCamera.IsCentroidAvail)
                    {
                        gbxCentroid.Enabled = true;
                        lblCentroidRadiusRange.Text = string.Format("{0} - {1}", ActiveCamera.CentroidInfo.MinRadius, ActiveCamera.CentroidInfo.MaxRadius);
                        lblCentrodCountRange.Text = string.Format("{0} - {1}", ActiveCamera.CentroidInfo.MinCount, ActiveCamera.CentroidInfo.MaxCount);
                        //set some intial value
                        tbxCentroidRadius.Text = (ActiveCamera.CentroidInfo.MinRadius).ToString();
                        tbxCentroidCount.Text = (ActiveCamera.CentroidInfo.MaxCount - 2).ToString();
                    }
                    else
                    {
                        gbxCentroid.Enabled = false;
                        
                    }
                    
                    
                    break;

                case CameraNotifications.READOUT_SPEED_CHANGED:
                    cbxGainStates.Items.Clear();
                    for (int i = 0; i < ActiveCamera.SpeedTable.ReadoutOption[ActiveCamera.SpeedTableIndex].GainStates; i++)
                        cbxGainStates.Items.Add(i + 1);
                    cbxGainStates.SelectedIndex = 0;
                    break;
            }
        }

        //Enable , Disable controls on the form

        private void UpdateControls(List<ComboBox> cbxList, List<Button> btnList,
                                    List<TextBox> tbxList, List<TrackBar> tbrList,
                                    List<CheckBox> chboxList,
                                    List<ToolStripMenuItem> mnuList,
                                    List<GroupBox> gboxList,
                                    List<Panel> panelList,bool enableState)
        {
            if (cbxList != null)
            {
                for (int i = 0; i < cbxList.Count; i++)
                {
                    cbxList[i].Enabled = enableState;
                  
                }
            }

            if (btnList != null)
            {
                for (int i = 0; i < btnList.Count; i++)
                {
                    btnList[i].Enabled = enableState;
                }
            }

            if (tbxList != null)
            {
                for (int i = 0; i < tbxList.Count; i++)
                {
                    tbxList[i].Enabled = enableState;
                }
            }

            if (tbrList != null)
            {
                for (int i = 0; i < tbrList.Count; i++)
                {
                   
                    tbrList[i].Enabled = enableState;
                    
                }
            }

            if (chboxList != null)
            {
                for (int i = 0; i < chboxList.Count; i++)
                {
                    chboxList[i].Enabled = enableState;
                    
                }
            }

            if (mnuList != null)
            {
                for (int i = 0; i < mnuList.Count; i++)
                {
                    mnuList[i].Enabled = enableState;
                }
            }

            //Group Box
            if (gboxList != null)
            {
                for (int i = 0; i < gboxList.Count; i++)
                {
                    gboxList[i].Enabled = enableState;

                    if (gboxList[i].Name == "gboxSS" && !ActiveCamera.IsSmartStreamingSupported)
                    {
                        gboxList[i].Enabled = false;
                    }
                    
                    //for Temperature monitor timer, Stop timer while disabling the box,
                    //While Enablinng check is it was enabled before
                    if (gboxList[i].Name == "gboxCooling")
                    {
                        if (enableState == false)
                        {
                            tmrTempUpdate.Stop();
                        }
                        else
                        {
                            //Check if temperature monitor was enabled before
                            if (chboxTempUpdate.Checked)
                            {
                                tmrTempUpdate.Start();
                            }

                        }
                    }

                   
                }
            }

            //Panel List
            //Group Box
            if (panelList != null)
            {
                for (int i = 0; i < panelList.Count; i++)
                {
                    panelList[i].Enabled = enableState;
                                       
                }
            }

            

        }

        private void ReportReceived(PVCamCamera pvcc, ReportMessage rm)
        {
            if (rm.Type == MsgTypes.MSG_ERROR)
            {
                ReportErr(lblErrMsg, lbxStatusList, rm.Message, this);
            }
            else if (rm.Type == MsgTypes.MSG_STATUS)
            {
                ReportStat(lbxStatusList, rm.Message, this);
            }
        }

        private void chboxTempUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (chboxTempUpdate.Checked == true)
            {
                tmrTempUpdate.Start();
            }
            else
            {
                tmrTempUpdate.Stop();
            }
        }

        //Form timers tick event - Get temperature from the camera and update GUI
        private void tmrTempUpdate_Tick(object sender, EventArgs e)
        {
            if (ActiveCamera.GetCurrentTemprature())
            {
                lboxCurTemp.Text = String.Format("{0:0.00}", ActiveCamera.CurrentTemperature / 100.00);
            }
            else
            {
                lboxCurTemp.Text = "??";
                //stop the timer so we don't keep polling
                tmrTempUpdate.Stop();
                chboxTempUpdate.Checked = false;
            }
        }

        private void btnApplyTemp_Click(object sender, EventArgs e)
        {
            Double sp = Convert.ToDouble(tbxSetpoint.Text);
            //multiply by 100 and convert to Int16
            Int16 newSetpoint = Convert.ToInt16(sp * 100);
            if (newSetpoint > ActiveCamera.MaxSetpoint || newSetpoint < ActiveCamera.MinSetpoint)
            {
                ReportStat(lbxStatusList, "Setpoint is out of range", this);
            }
            else
            {
                ActiveCamera.SetTemperatureSetpoint(newSetpoint);
            }

            //Read Back the setpoint in case it was not set and update text box
            ActiveCamera.ReadCoolingParameters();
            tbxSetpoint.Text = String.Format("{0:0.00}", ActiveCamera.CurrentSetpoint / 100.0);
        }

        //private void cbxExpOutMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ActiveCamera.SetExposeOutMode(cbxExpOutMode.Items[cbxExpOutMode.SelectedIndex].ToString());
        //}

        private void advancedSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

       

        //private void cameraInfoToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    using (CCDInfoForm frmCCDInfo = new CCDInfoForm(ActiveCamera))
        //    {
        //        frmCCDInfo.ShowDialog();
        //    }
        //}

        //private void PopulatePostProcessing()
        //{

        //    List<PP_Feature> ppList = ActiveCamera.PP_FeatureList;
        //    List<GroupBox> gBox = new List<GroupBox>();
        //    List<ComboBox> cBox = new List<ComboBox>();
        //    List<Label> lbl = new List<Label>();
        //    List<TextBox> tbox = new List<TextBox>();

        //    Int32 cBoxIndex = 0;
        //    Int32 lblIndex = 0;
        //    Int32 tBoxIndex = 0;
            
        //    //remove all controls from the panel
        //    panelPP.Controls.Clear();

        //    for (Int32 i = 0; i < ppList.Count; i++)
        //    {
               
        //        //Feature Name Label
        //        lbl.Add(new Label());
        //        panelPP.Controls.Add(lbl[lblIndex]);
        //        if (lblIndex == 0)
        //        {
        //            lbl[lblIndex].Top = 20;
        //        }
        //        else
        //        {
        //            lbl[lblIndex].Top = lbl[lblIndex - 1].Top + lbl[lblIndex - 1].Height + 5;
        //        }
        //        lbl[lblIndex].Left = 20;
        //        lbl[lblIndex].Width = 200;
        //        lbl[lblIndex].Text = ppList[i].Name;
        //        lbl[lblIndex].Font = new Font(lbl[lblIndex].Font, FontStyle.Bold);
        //        lblIndex++;


        //        //For each functions, create a control
        //        for (Int32 j = 0; j < ppList[i].FunctionList.Count; j++)
        //        {

        //            lbl.Add(new Label());
        //            panelPP.Controls.Add(lbl[lblIndex]);
        //            lbl[lblIndex].Top = lbl[lblIndex - 1].Top + lbl[lblIndex - 1].Height + 3;
        //            lbl[lblIndex].Left = 24;
        //            lbl[lblIndex].Text = ppList[i].FunctionList[j].Name;

        //            lbl[lblIndex].Width =150;



        //            //its enabled/disabled , create a combo box
        //            if (ppList[i].FunctionList[j].MaxValue == 1)
        //            {


        //                cBox.Add(new ComboBox());
        //                panelPP.Controls.Add(cBox[cBoxIndex]);
        //                cBox[cBoxIndex].Left = lbl[lblIndex].Left + lbl[lblIndex].Width + 3; 
        //                cBox[cBoxIndex].Top = lbl[lblIndex].Top;
        //                cBox[cBoxIndex].Width =70;

        //                //cBox[cBoxIndex].Name = String.Format("cbox_{0}", cBoxIndex);

        //                cBox[cBoxIndex].Items.Clear();
        //                cBox[cBoxIndex].Items.Add("Disabled");
        //                cBox[cBoxIndex].Items.Add("Enabled");
        //                //Select the box according to the current value
        //                if (ppList[i].FunctionList[j].CurrentVal == 0)
        //                {
        //                    cBox[cBoxIndex].SelectedItem = "Disabled";
        //                }
        //                else if (ppList[i].FunctionList[j].CurrentVal == 1)
        //                {
        //                    cBox[cBoxIndex].SelectedItem = "Enabled";
        //                }
        //                else //Clear combo box to show if values are not 0 or 1
        //                {
        //                    cBox[cBoxIndex].Items.Clear();
        //                }

        //                //Add tags to this control to associate it with
        //                //feature and function index
        //                ControlTag tags = new ControlTag();
        //                tags.PP_Feature = i;
        //                tags.PP_Function = j;
        //                cBox[cBoxIndex].Tag = tags;
        //                cBox[cBoxIndex].SelectedValueChanged += new System.EventHandler(cboBox_Changed);


        //                cBoxIndex++;
        //            }
        //            else //Add Text Box
        //            {

        //                tbox.Add(new TextBox());
        //                panelPP.Controls.Add(tbox[tBoxIndex]);
        //                tbox[tBoxIndex].Left = lbl[lblIndex].Left + lbl[lblIndex].Width + 3; 
        //                tbox[tBoxIndex].Top = lbl[lblIndex].Top;
        //                tbox[tBoxIndex].Width = 60;
        //                //tbox[tBoxIndex].Name = String.Format("tbox{0}", tBoxIndex);
        //                tbox[tBoxIndex].Text = String.Format("{0}",
        //                                                      ppList[i].FunctionList[j].CurrentVal);

        //                ControlTag tags = new ControlTag();
        //                tags.PP_Feature = i;
        //                tags.PP_Function = j;
        //                tbox[tBoxIndex].Tag = tags;
        //                tbox[tBoxIndex].KeyDown += new System.Windows.Forms.KeyEventHandler(tBox_keyDown);

        //                tBoxIndex++;

        //            }
        //            lblIndex++;

        //        }
               

        //    }
        //}

        private void cboBox_Changed(object sender,EventArgs e)
        {
           
            ControlTag tags = new ControlTag();
            ComboBox comboBox = (ComboBox)sender;
            tags =(ControlTag)comboBox.Tag;
            Int32 feat = tags.PP_Feature;
            Int32 funct = tags.PP_Function;
            Int32 value = 0; //function value

            if (comboBox.Items[comboBox.SelectedIndex].ToString() == "Enabled")
            {
                value = 1;
            }
            else if (comboBox.Items[comboBox.SelectedIndex].ToString() == "Disabled")
            {
                value = 0;
            }

            ActiveCamera.WritePostProcessingFeature(feat, funct, value);
        


        }

    private void tBox_keyDown(object sender, KeyEventArgs e)
        {
            TextBox tbox = (TextBox) sender;
            tbox.BackColor = System.Drawing.Color.Yellow;


            if (e.KeyCode == Keys.Enter)
            {
                tbox.BackColor = System.Drawing.Color.White;
                ControlTag tags = new ControlTag();
               
                tags = (ControlTag)tbox.Tag;
                Int32 feat = tags.PP_Feature;
                Int32 funct = tags.PP_Function;
                Int32 value; //function value
                value = Convert.ToInt32(tbox.Text);
                ActiveCamera.WritePostProcessingFeature(feat, funct, value);
            }
        }

        private void cboxFanSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveCamera.SetFanSpeedSetpoint(cboxFanSpeed.Items[cboxFanSpeed.SelectedIndex].ToString());
        }

        private void cboxMultiROI_CheckedChanged(object sender, EventArgs e)
        {
            /*if (cboxMultiROI.Checked == true)
            {
                gboxSingleROI.Enabled = false;
                gboxMultiROI.Enabled = true;
                //as metadata has be enabled for MultiROI to work, 
                cboxMetadata.Checked = true;
                cboxMetadata.Enabled = false;

            }
            else
            {
                gboxSingleROI.Enabled = true;
                gboxMultiROI.Enabled = false;
                //Give option to enable/disable metadata
                cboxMetadata.Enabled = true;

                btnResetROI_Click(this,e);

            }*/
        }

        private void btnAddROI_Click(object sender, EventArgs e)
        {
            string[] roi = new string[4];
           
            roi[0] = tbxMxOrigin.Text;
            roi[1] = tbxMyOrigin.Text;
            roi[2] = tbxMxSize.Text;
            roi[3] = tbxMySize.Text;
                  
            
            ListViewItem itm = new ListViewItem(roi);
            //if Camera supports multiple ROI and Centroiding is off , ADD ROI to existing ROI list
            if ((ActiveCamera.MaxROICount>1) && (!ActiveCamera.IsCentroidEnabled))
            {
                lstViewMultiROI.Items.Add(itm);
                
            }
            else //Camera does not support multiROI or centroiding is on, Change the only roi
            {
                lstViewMultiROI.Items.Clear();
                lstViewMultiROI.Items.Add(itm);
            }

            //Set ROI
            SetROI();

           
        }

        private void btnRemoveROI_Click(object sender, EventArgs e)
        {
            //if these is a selected row, remove it
            if (lstViewMultiROI.SelectedItems != null)
            {
                for (int i = 0;i < lstViewMultiROI.Items.Count; i++)
                {
                    if (lstViewMultiROI.Items[i].Selected)
                    {
                        lstViewMultiROI.Items[i].Remove();
                        //Set ROI again
                        SetROI();
                    }

                    
                }
            }

           

        }

        private void SetROI()
        {
            string[] roi = new string[4];
            int cnt = lstViewMultiROI.Items.Count;
            UInt16[] rgnS1, rgnS2, rgnP1, rgnP2;
           
            if (cnt == 0) //Set to full CCD size
            {
                rgnS1 = new UInt16[1];
                rgnS2 = new UInt16[1];
                rgnP1 = new UInt16[1];
                rgnP2 = new UInt16[1];
                rgnS1[0] = 0;
                rgnS2[0] = (UInt16)(ActiveCamera.XSize - 1);
                rgnP1[0] = 0;
                rgnP2[0] = (UInt16)(ActiveCamera.YSize - 1);
                tbxMxOrigin.Text = Convert.ToString(rgnS1[0]);
                tbxMyOrigin.Text = Convert.ToString(rgnP1[0]);
                tbxMxSize.Text = Convert.ToString(rgnS2[0]+1);
                tbxMySize.Text = Convert.ToString(rgnP2[0]+1);

                ActiveCamera.SetMultiROI(rgnS1, rgnS2, rgnP1, rgnP2);
                ReportStat(lbxStatusList, String.Format("Acquisition region{0} set to: [{1},{2}]:[{3}:{4}]", 0, rgnS1[0], rgnP1[0],
                                                       rgnS2[0], rgnP2[0]), this);

            }
            else
            { 
                rgnS1 = new UInt16[cnt];
                rgnS2 = new UInt16[cnt];
                rgnP1 = new UInt16[cnt];
                rgnP2 = new UInt16[cnt];



                for (int i = 0; i < cnt; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        roi[j] = lstViewMultiROI.Items[i].SubItems[j].Text;

                    }
                    rgnS1[i] = Convert.ToUInt16(roi[0]);
                    rgnS2[i] = (UInt16)(rgnS1[i] + Convert.ToUInt16(roi[2]) - 1);
                    rgnP1[i] = Convert.ToUInt16(roi[1]);
                    rgnP2[i] = (UInt16)(rgnP1[i] + Convert.ToUInt16(roi[3]) - 1);
                    ReportStat(lbxStatusList, String.Format("Acquisition region{0} set to: [{1},{2}]:[{3}:{4}]", i + 1, rgnS1[i], rgnP1[i],
                                                           rgnS2[i], rgnP2[i]), this);
                }

            }

            //if MultiROI, enable Metadata
            if (cnt > 1)
            {
              
                cboxMetadata.Checked = true;
                cboxMetadata.Enabled = false;
            }
            else //enable control if meta deta is available
            {
                cboxMetadata.Enabled = ActiveCamera.IsMetadataAvail;

            }


            ActiveCamera.SetMultiROI(rgnS1, rgnS2, rgnP1, rgnP2);
            //Set Binning Again
            ActiveCamera.SetBinning(cbxBinning.Items[cbxBinning.SelectedIndex].ToString());

        }

        private void cboxMetadata_CheckedChanged(object sender, EventArgs e)
        {

            ActiveCamera.ConfigureMetadata(cboxMetadata.Checked);
           
        }

        private void btnResetMultiROI_Click(object sender, EventArgs e)
        {
            //Clear the list view
            lstViewMultiROI.Items.Clear();

        }

        private void cboxCentroidEnable_CheckedChanged(object sender, EventArgs e)
        {
            UInt16 count = Convert.ToUInt16(tbxCentroidCount.Text);
            UInt16 radius = Convert.ToUInt16(tbxCentroidRadius.Text);

            if (cboxCentroidEnable.Checked)
            {
                //Enable metadata first and disable access to it
                cboxMetadata.Checked = true;
                cboxMetadata.Enabled = false;
                

                if (!ActiveCamera.SetCentroiding(true, count, radius))
                               
                {
                    //if fails to set mark the box unchecked
                    cboxCentroidEnable.Checked = false;
                    tbxCentroidCount.Enabled = true;
                    tbxCentroidRadius.Enabled = true;
                    if (ActiveCamera.IsMetadataAvail)
                    {
                        cboxMetadata.Enabled = true;

                    }

                }

            }
            else
            {
                ActiveCamera.SetCentroiding(false, count, radius);
                if (ActiveCamera.IsMetadataAvail)
                {
                    cboxMetadata.Enabled = true;

                }

            }
            
            //disable/enable access to count and radius
            tbxCentroidCount.Enabled = !cboxCentroidEnable.Checked;
            tbxCentroidRadius.Enabled = !cboxCentroidEnable.Checked;

            //Disable/enable access to binning
            //gboxMultiROI.Enabled = !cboxCentroidEnable.Checked;
            cbxBinning.Enabled = !cboxCentroidEnable.Checked;

        }

        private void cbxCamList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabSettings_Click(object sender, EventArgs e)
        {

        }



        private void tbxSetpoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbxRdOutSpd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveCamera.SpeedTableIndex!= Convert.ToInt16(cbxRdOutSpd.SelectedIndex))
            ActiveCamera.SetReadoutSpeed(Convert.ToInt16(cbxRdOutSpd.SelectedIndex));
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void tbrEMGain_Scroll(object sender, EventArgs e)
        {

        }
    }
}
