using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.Foculus;

namespace ASCOM.Foculus
{
    [ComVisible(false)]					// Form not registered for COM!
    public  partial class SetupDialogForm : Form
    {
        
        public SetupDialogForm( )
        {
         
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            //axFG.Camera=cbCamera.SelectedIndex;
            if (comboBoxComPort.SelectedIndex != -1) { 
            Camera.comPort = (string)comboBoxComPort.SelectedItem;
            }
            else {
                Camera.comPort = "";
            }
          
            Camera.tl.Enabled = chkTrace.Checked;
            Hide();

        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }
        private void SetupDialogForm_Load(object sender, EventArgs e)
        {

        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }
        public AxFGControlLib.AxFGControlCtrl myCameraControl
        {
            get
            {
                return axFG;
            }

        }
        private void InitUI()
        {
            chkTrace.Checked = Camera.tl.Enabled;
           // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            //if (comboBoxComPort.Items.Contains(Camera.comPort))
            //{
            //    comboBoxComPort.SelectedItem = Camera.comPort;
            //}
            System.Array CameraList;
            try
            {
                CameraList = (System.Array)axFG.GetCameraList();
                for (int i = 0; i < CameraList.Length; i++)
                    this.cbCamera.Items.Add(CameraList.GetValue(i));

                if (CameraList.Length <= 0)
                {
                    cbCamera.SelectedIndex = -1;
                }
                //
                foreach (string sp in System.IO.Ports.SerialPort.GetPortNames())
                    comboBoxComPort.Items.Add(sp);
            }
            catch
            { }


        }

        private void axFG_ImageReceived(object sender, AxFGControlLib._IFGControlEvents_ImageReceivedEvent e)
        {

        }
    }
}