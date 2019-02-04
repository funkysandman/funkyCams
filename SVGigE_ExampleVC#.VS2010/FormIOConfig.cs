using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GigEApi
{
  public partial class FormIOConfig : Form
  {
    private Form1 camDisplay = null;
    private IntPtr hCamera;
    private GigeApi myApi;
    private GigeApi.SVSGigeApiReturn errorflag;

    private System.Windows.Forms.Label[] IOLabel = new Label[16];
    private System.Windows.Forms.ComboBox[] IOSelect = new ComboBox[16];

    private System.Windows.Forms.Label[] IOSignal = new Label[24];
    private System.Windows.Forms.Button[] IOState = new Button[24];

    private int SelectCount;

    private int PHYSICAL_COUNT     = 5;
    private int PHYSICAL_COUNT_MAX = 8;
    private string[] LabelPhysical = { "OUT0", "OUT1", "OUT2", "OUT3", "OUT_TXD"};

    private int LOGICAL_COUNT     = 6;
    private int LOGICAL_COUNT_MAX = 8;
    private string[] LabelLogical = { "UART_IN", "Trigger","Debounce","Prescal","LogicA","LogicB" };

    private int SELECT_COUNT = 7;
      private string[] SelectionItems = { "n.a.", "IN0", "IN1", "IN2", "IN_RS422", "IN_RS232", "UART_OUT" };

    GigeApi.SVGigE_IOMux_IN[] SelectMask =
    {
      (GigeApi.SVGigE_IOMux_IN) 0,
      GigeApi.SVGigE_IOMux_IN.SVGigE_IOMUX_IN0,
      GigeApi.SVGigE_IOMux_IN.SVGigE_IOMUX_IN1,
      GigeApi.SVGigE_IOMux_IN.SVGigE_IOMUX_IN2,
      GigeApi.SVGigE_IOMux_IN.SVGigE_IOMUX_IN3,
      (GigeApi.SVGigE_IOMux_IN) GigeApi.SVGigE_IOMux_IN_IO_RXD,
      (GigeApi.SVGigE_IOMux_IN) GigeApi.SVGigE_IOMux_IN_UART_OUT,
      (GigeApi.SVGigE_IOMux_IN)0,
      (GigeApi.SVGigE_IOMux_IN)0,
      (GigeApi.SVGigE_IOMux_IN)0,
       (GigeApi.SVGigE_IOMux_IN)0,
      (GigeApi.SVGigE_IOMux_IN) 0,
       (GigeApi.SVGigE_IOMux_IN)0,
       (GigeApi.SVGigE_IOMux_IN)0,
        (GigeApi.SVGigE_IOMux_IN)0,
       (GigeApi.SVGigE_IOMux_IN)0,
       (GigeApi.SVGigE_IOMux_IN)0,
      (GigeApi.SVGigE_IOMux_IN) 0,
       (GigeApi.SVGigE_IOMux_IN)0,
       (GigeApi.SVGigE_IOMux_IN)0,
       (GigeApi.SVGigE_IOMux_IN)0,
      (GigeApi.SVGigE_IOMux_IN) 0,
       (GigeApi.SVGigE_IOMux_IN)0,
       (GigeApi.SVGigE_IOMux_IN)0,
      // ATTENTION: there must be as many free entries as dynamical items can be added
    };

    //private int SOURCE_SIGNAL_COUNT = 24;
    private string[] SourceSignal = {"IN0","IN1","IN2","IN_RS422","IN_RS232","UART_OUT","","","","","","","","","","","","","","","","","",""};
    // ATTENTION: there must be as many free entries as dynamical items can be added

    private int IOOUT_COUNT = 16;
    private int MAX_SIGNAL_COUNT = 24;

    GigeApi.SVGigE_IOMux_OUT[] SelectOuput =
    {    
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT0,
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT1,
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT2,
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT3,
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT4,  //IO_TXD 
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT31,  // not defined.
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT31, 
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT31,
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT5,  // UART_IN 
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT6,  // TRIGGER
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT8,  // Debounce
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT9,  // TRIGGER
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT10, // LogicA
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT11, // LogicB
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT31,
        GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT31,
    };
  
    //-----------------------------------------------------------------
    public FormIOConfig(Form1 mainForm)
    {
      InitializeComponent();

      initIOConfig(mainForm);

      // Update IO settings from camera
      updateIOSettings();
    }

    //-----------------------------------------------------------------
    private void initIOConfig(Form1 mainForm)
    {
      this.camDisplay = mainForm;
      int index = 0;
      
      myApi = camDisplay.myApi;
      hCamera = camDisplay.getCamHandle();

      // Initialize counts
      SelectCount = SELECT_COUNT;

      // Generate arrays of GUI elements

      // Output - Labels
      IOLabel[0] = label1;
      IOLabel[1] = label2;
      IOLabel[2] = label3;
      IOLabel[3] = label4;
      IOLabel[4] = label5;
      IOLabel[5] = label6;
      IOLabel[6] = label7;
      IOLabel[7] = label8;
      IOLabel[8] = label9;
      IOLabel[9] = label10;
      IOLabel[10] = label11;
      IOLabel[11] = label12;
      IOLabel[12] = label13;
      IOLabel[13] = label14;
      IOLabel[14] = label15;
      IOLabel[15] = label16;

      // Output - ComboBox
      IOSelect[0] = comboBox1;
      IOSelect[1] = comboBox2;
      IOSelect[2] = comboBox3;
      IOSelect[3] = comboBox4;
      IOSelect[4] = comboBox5;
      IOSelect[5] = comboBox6;
      IOSelect[6] = comboBox7;
      IOSelect[7] = comboBox8;
      IOSelect[8] = comboBox9;
      IOSelect[9] = comboBox10;
      IOSelect[10] = comboBox11;
      IOSelect[11] = comboBox12;
      IOSelect[12] = comboBox13;
      IOSelect[13] = comboBox14;
      IOSelect[14] = comboBox15;
      IOSelect[15] = comboBox16;

      // Input - Label
      IOSignal[0] = label_IN1;
      IOSignal[1] = label_IN2;
      IOSignal[2] = label_IN3;
      IOSignal[3] = label_IN4;
      IOSignal[4] = label_IN5;
      IOSignal[5] = label_IN6;
      IOSignal[6] = label_IN7;
      IOSignal[7] = label_IN8;
      IOSignal[8] = label_IN9;
      IOSignal[9] = label_IN10;
      IOSignal[10] = label_IN11;
      IOSignal[11] = label_IN12;
      IOSignal[12] = label_IN13;
      IOSignal[13] = label_IN14;
      IOSignal[14] = label_IN15;
      IOSignal[15] = label_IN16;
      IOSignal[16] = label_IN17;
      IOSignal[17] = label_IN18;
      IOSignal[18] = label_IN19;
      IOSignal[19] = label_IN20;
      IOSignal[20] = label_IN21;
      IOSignal[21] = label_IN22;
      IOSignal[22] = label_IN23;
      IOSignal[23] = label_IN24;
    
   

      // Input - Button
      IOState[0] = button1;
      IOState[1] = button2;
      IOState[2] = button3;
      IOState[3] = button4;
      IOState[4] = button5;
      IOState[5] = button6;
      IOState[6] = button7;
      IOState[7] = button8;
      IOState[8] = button9;
      IOState[9] = button10;
      IOState[10] = button11;
      IOState[11] = button12;
      IOState[12] = button13;
      IOState[13] = button14;
      IOState[14] = button15;
      IOState[15] = button16;
      IOState[16] = button17;
      IOState[17] = button18;
      IOState[18] = button19;
      IOState[19] = button20;
      IOState[20] = button21;
      IOState[21] = button22;
      IOState[22] = button23;
      IOState[23] = button24;
   
      //----------------------------------------------------------------
      // Set/hide physical labels
      for (index = 0; index < PHYSICAL_COUNT_MAX; index++)
      {
        if (index < PHYSICAL_COUNT)
          IOLabel[index].Text = LabelPhysical[index];
        else
        {
          IOLabel[index].Visible = false;
          IOSelect[index].Visible = false;
        }
      }

      // Set/hide logical labels
      for (index = 0; index < LOGICAL_COUNT_MAX; index++)
      {
        if (index < LOGICAL_COUNT)
          IOLabel[index+8].Text = LabelLogical[index];
        else
        {
          IOLabel[index + 8].Visible = false;
          IOSelect[index + 8].Visible = false;
        }
      }

      // Set/hide signal labels and states
      for (index = 0; index < MAX_SIGNAL_COUNT; index++)
      {
          
      if (index < SELECT_COUNT - 1)
         IOSignal[index].Text = SourceSignal[index];
      
        else
        {
          IOSignal[index].Visible = false;
          IOState[index].Visible = false;
        }
      }

      // Update IO settings from camera
      updateIOSettings();
    }

    //-----------------------------------------------------------------
    int getItemIndex(int IOOut)
    {  
     if( (IOOut < 0) || (IOOut >= IOOUT_COUNT) )
        return 0; // invalid IOOut
        
        GigeApi.SVGigE_IOMux_OUT  output  = SelectOuput[IOOut];

        // Determine selection mask
        uint Mask = 0;

        if (myApi == null)
        return 0;

        errorflag = myApi.Gige_Camera_getIOAssignment(hCamera, output, ref(Mask));
        
        if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        return 0; // IOOut not supported by camera

        // Determine selection
        int selctInd = 0;

        // Get index from current mask list
        bool found = false;
               
        for (int j = 0 ; j < SelectCount; j++)
        {
            GigeApi.SVGigE_IOMux_IN d = (GigeApi.SVGigE_IOMux_IN)Mask; 
            if (SelectMask[j] == d)
            {
                found = true;
                return j;
            }
        }
        return selctInd;
         
    }


    //---------------------------------------------------------------------------   
    GigeApi.SVSGigeApiReturn applyIOConfig()
    {
    if (myApi == null)
    return GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_IO_CONFIG_NOT_SUPPORTED;

    // Process all IO selections

    for (int Index = 0; Index < IOOUT_COUNT; Index++)
    {
        // Determine the IOAssignment Parameters
        GigeApi.SVGigE_IOMux_OUT output = SelectOuput[Index];
        // get only the predefined ouputs
        if (output == GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT31)   
        continue;
    
        uint  currentIndex = (uint)IOSelect[Index].SelectedIndex;
        GigeApi.SVSGigeApiReturn err =     myApi.Gige_Camera_setIOAssignment(hCamera, output, SelectMask[currentIndex]);
        if (err != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        return GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_IO_CONFIG_NOT_SUPPORTED;
    }
  
    return GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS;

    }

    //---------------------------------------------------------------------------
    private void buttonEEPROM_Click(object sender, EventArgs e)
    {
      if (myApi != null)
      {
        errorflag = GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_ERROR;

        errorflag = myApi.Gige_Camera_writeEEPROM(hCamera);

        if (errorflag == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        {
          MessageBox.Show("Settings have been successfully saved to EEPROM");
        }
        else
        {
          MessageBox.Show("Error while saving settings to EEPROM");
        }
      }
    }

    //-----------------------------------------------------------------
    private void updateIOSettings()
    {
      // Update all IO settings on GUI
      for (int Index = 0; Index < 16; Index++)
      {
        IOSelect[Index].Items.Clear();

        // Add items that are always present
        SelectCount = SELECT_COUNT;
        for (int item = 0; item < SELECT_COUNT; item++)
        {
          IOSelect[Index].Items.Add("  " + SelectionItems[item]);
        }

        // Add STROBE0 and STROBE1, if available
        // ATTENTION: there must be as many free entries in SelectMask and SourceSignal
        //            as dynamical items are added

        if (myApi.Gige_Camera_isCameraFeature(hCamera, GigeApi.CAMERA_FEATURE.CAMERA_FEATURES2_IOMUX_STROBE4))
        {
            IOSelect[Index].Items.Add("  STROBE0");
            SourceSignal[SelectCount - 1] = "STROBE0";
            SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_STROBE0;
            SelectCount++;
            IOSelect[Index].Items.Add("  STROBE1");
            SourceSignal[SelectCount - 1] = "STROBE1";
            SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_STROBE1;
            SelectCount++;
            IOSelect[Index].Items.Add("  STROBE2");
            SourceSignal[SelectCount - 1] = "STROBE2";
            SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_STROBE2;
            SelectCount++;
            IOSelect[Index].Items.Add("  STROBE3");
            SourceSignal[SelectCount - 1] = "STROBE3";
            SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_STROBE3;
            SelectCount++;

        }

        else if (myApi.Gige_Camera_isCameraFeature(hCamera,GigeApi.CAMERA_FEATURE.CAMERA_FEATURES2_IOMUX_STROBE2))
        {
          IOSelect[Index].Items.Add("  STROBE0");
          SourceSignal[SelectCount - 1] = "STROBE0";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_STROBE0;
          SelectCount++;
          IOSelect[Index].Items.Add("  STROBE1");
          SourceSignal[SelectCount - 1] = "STROBE1";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_STROBE1;
          SelectCount++;
     
        }
        else
        {
          IOSelect[Index].Items.Add("  STROBE");
          SourceSignal[SelectCount - 1] = "STROBE";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_STROBE;
          SelectCount++;
        }

        // Add PWMA and PWMB, if available
        // ATTENTION: there must be as many free entries in SelectMask and SourceSignal
        //            as dynamical items are added
        if (myApi.Gige_Camera_isCameraFeature(hCamera, GigeApi.CAMERA_FEATURE.CAMERA_FEATURES2_IOMUX_PWM))
        {
          IOSelect[Index].Items.Add("  PWMA");
          SourceSignal[SelectCount - 1] = "PWMA";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_PWMA;
          SelectCount++;
          IOSelect[Index].Items.Add("  PWMB");
          SourceSignal[SelectCount - 1] = "PWMB";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_PWMB;
          SelectCount++;
          IOSelect[Index].Items.Add("  PWMC");
          SourceSignal[SelectCount - 1] = "PWMC";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_PWMC;
          SelectCount++;
          IOSelect[Index].Items.Add("  PWMD");
          SourceSignal[SelectCount - 1] = "PWMD";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_PWMD;
          SelectCount++;
          IOSelect[Index].Items.Add(" Pulse");
          SourceSignal[SelectCount - 1] = "Pulse";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMUX_IN_MASK_PULSE;
          SelectCount++;

        }
        if (myApi.Gige_Camera_isCameraFeature(hCamera, GigeApi.CAMERA_FEATURE.CAMERA_FEATURES2_IOMUX_LOGIC))        
        {
            IOSelect[Index].Items.Add(" LOGIC");
            SourceSignal[SelectCount - 1] = "LOGIC";
            SelectMask[SelectCount] = GigeApi.SVGigE_IOMUX_IN_MASK_LOGIC;
            SelectCount++;
        }


        // Add EXPOSE, if available
        // ATTENTION: there must be as many free entries in SelectMask and SourceSignal
        //            as dynamical items are added
        if (myApi.Gige_Camera_isCameraFeature(hCamera, GigeApi.CAMERA_FEATURE.CAMERA_FEATURES2_IOMUX_EXPOSE))
        {
          IOSelect[Index].Items.Add("  EXPOSE");
          SourceSignal[SelectCount - 1] = "EXPOSE";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_EXPOSE;
          SelectCount++;
        }

        // Add READOUT, if available
        // ATTENTION: there must be as many free entries in SelectMask and SourceSignal
        //            as dynamical items are added
        if (myApi.Gige_Camera_isCameraFeature(hCamera, GigeApi.CAMERA_FEATURE.CAMERA_FEATURES2_IOMUX_READOUT))
        {
          IOSelect[Index].Items.Add("  READOUT");
          SourceSignal[SelectCount - 1] = "READOUT";
          SelectMask[SelectCount] = GigeApi.SVGigE_IOMux_IN_READOUT;
          SelectCount++;
        }

        // Add static low signal level
        if (Index < 8)
        {
          IOSelect[Index].Items.Add("  " + "OFF/LOW");
          IOSelect[Index].Items.Add("  " + "ON/HIGH");
        }

        // Select entry from camera settings
      IOSelect[Index].SelectedIndex = getItemIndex(Index);

      }
 
          // Update signal labels and states
      for (int Index = 0; Index < MAX_SIGNAL_COUNT; Index++)
      {
          if (Index < SelectCount - 1)
          {
              IOSignal[Index].Text= SourceSignal[Index];
              IOSignal[Index].Visible = true;
              IOState[Index].Visible = true;
          }
          else
          {

              IOSignal[Index].Visible = false;
              IOState[Index].Visible = false;
          }
      }

    }

    //-----------------------------------------------------------------
    private void buttonFactory_Click(object sender, EventArgs e)
    {
      // Adjust factory settings
      if ((myApi == null) || (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS == myApi.Gige_Camera_restoreFactoryDefaults(hCamera)))
      {
        // Update IO settings from camera
        updateIOSettings();

        myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_START);

        MessageBox.Show("Factory defaults have been successfully restored.");
      }
      else
        MessageBox.Show("Error while restoring factory defaults");
    }

    //-----------------------------------------------------------------
    private void buttonApply_Click(object sender, EventArgs e)
    {
      // Send IO configuration to camera
      if (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS != applyIOConfig())
          MessageBox.Show("Camera does not support IO configuration");
    }

    //-----------------------------------------------------------------
    private void buttonOK_Click(object sender, EventArgs e)
    {
      // Send IO configuration to camera and close dialog
      if (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS != applyIOConfig())
          MessageBox.Show("Camera does not support IO configuration");

      Close();
    }

    //-----------------------------------------------------------------
    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    //-----------------------------------------------------------------
    // set the current Signal State
    //-----------------------------------------------------------------
    void timer1_Tick(object sender, System.EventArgs e)
    {
      uint[] signalVector = new uint[31];
     GigeApi.SVGigE_IOMux_IN  Mask =0;
      GigeApi.SVGigE_IO_Signal SignalValue =   GigeApi.SVGigE_IO_Signal.IO_SIGNAL_OFF;
      if (myApi != null)
      {
        // Switch signal state indicators on/off
        for (int Index = 0; Index < SelectCount - 1; Index++)
        {
            Mask = SelectMask[Index + 1];
            myApi.Gige_Camera_getIO(hCamera, Mask, ref (SignalValue));
            this.IOState[Index].BackColor = (SignalValue == GigeApi.SVGigE_IO_Signal.IO_SIGNAL_OFF ? Color.Red : Color.Green);
        }
    }
  }
 }
}
