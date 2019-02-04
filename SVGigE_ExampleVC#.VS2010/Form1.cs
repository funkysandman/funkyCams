using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;



namespace GigEApi
{
   

    public class Form1 : System.Windows.Forms.Form
    {
        private bool bayerPattern;
        private const int bufferCount = 1;
        //private bool bayerPattern = false;
        private bool drawing = false;
        private bool selectBayer = false;
        private GigeApi.BAYER_METHOD bayerMethod = GigeApi.BAYER_METHOD.BAYER_METHOD_HQLINEAR ;

        private System.ComponentModel.IContainer components;

        private int camWidth, camHeight;

        private int PacketSize;

        private Rectangle outRectangle;
        private Graphics gpanel;

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItemCameraMode;
        private System.Windows.Forms.MenuItem MenuItemFreerunning;
        private System.Windows.Forms.MenuItem MenuItemFreeze;

        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;

        private bool isZoomX2;
        private int imageCount, readimageCount, lostCount;

        private bool openChannel;
        private int selectedIndex;
        private int gigeBitcount;

        public GigeApi myApi;

        private CameraControl camCtrl;
        private FormIOConfig ioConfiguration;

        private int hCameraContainer;
        private IntPtr hCamera;
        private System.IntPtr hStreamingChannel;

        //private int _hStreamingChannel;
        private string modelname;
        private GigeApi.SVSGigeApiReturn errorflag;
        private GigeApi.StreamCallback GigeCallBack;
        private int bitCount;
        private bool GigeIsInit;
        private int running = 0;
        private bool notFirstImage = false;
        private int cameraSelection = -99;
        private bool cameraConnected = false;

        //---------------------------------------
#if DEBUG
        float timeout = 3.0f;//1500.0f;                    // extended timeout for maintaining connection during a debug session
#else
        float timeout = 3.0f;                     // timeout: the time without traffic or heartbeat
                                                 //          after which a camera drops a connection
#endif
        //---------------------------------------


        private int bufferWriteind;
        private int bufferReadind;
        private int overflow;
        private int fifoCount;

        private Bitmap[] bimage;
        private Bitmap[] bimageRGB;
        // The object that will contain the palette information for the bitmap
        private ColorPalette imgpal = null;

        private bool grab, initgrab;
        private System.Windows.Forms.Timer timer1;  // for display-interval of camera-image = 20 fps
        private MenuItem menuItemCamera;

        private Thread imgproc;
        private bool threadStop = false;
        delegate void SetStatusBarCallBack();

        private int[] threeBythree;
        private int[] itemThree;
        private int[] diffThree;



        //---------------------------------------------------------------------------

        private struct imagebufferStruct
        {
            public byte[] imagebytes;

        };
        private imagebufferStruct[] imagebuffer;
        private imagebufferStruct[] imagebufferRGB;

        private imagebufferStruct[] outputbuffer;
        private imagebufferStruct[] outputbufferRGB;

        private MenuItem menuItemSave;
        private SaveFileDialog saveFileDialog1;
        private Panel panelInfo;
        private Label label1;
        private Label label2;
        private ComboBox GigeComboBox;
        private Button buttonExit;
        private Button buttonDiscover;
        private GroupBox groupBox1;
        private Label labelDeviceVersion1;
        private Label labelManufacturer1;
        private Label labelModelName1;
        private Label labelUserDefinedName1;
        private Label labelSerial1;
        private Label labelHeight1;
        private Label labelWidth1;
        private TextBox textBox_Result;
        private Label labelHeight2;
        private Label labelWidth2;
        private Label labelUserDefinedName2;
        private Label labelSerial2;
        private Label labelDeviceVersion2;
        private Label labelManufacturer2;
        private Label labelModelName2;
        private ComboBox comboBoxBayerConversion;
        private Label label3;
        private Label labelPixelDepth1;
        private Label labelPixelDepth2;
        private Label labelColor2;
        private Button buttonSnap;
        private ComboBox comboBoxAcquisition;
        private Label labelAcqMode;
        private MenuItem menuItemCameraControl;
        private MenuItem menuItemIOConfiguration;
        private MenuItem menuItem1;
        private MenuItem Load_Sequencer;
        private MenuItem start_Sequencer;
        private OpenFileDialog openFileDialog1;
        private Button button2;
        private Button button1;
        private PictureBox pictureBox1;
        private Label labelColor1;


        //---------------------------------------------------------------------------

        public Form1()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------

        protected override void Dispose(bool disposing)
        {


            if (disposing)
            {

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        //---------------------------------------------------------------------------
        #region Windows Form Designer generated code
        /// <summary>
        /// Generated by designer. 
        /// Do not change following code by editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemCameraMode = new System.Windows.Forms.MenuItem();
            this.MenuItemFreerunning = new System.Windows.Forms.MenuItem();
            this.MenuItemFreeze = new System.Windows.Forms.MenuItem();
            this.menuItemCamera = new System.Windows.Forms.MenuItem();
            this.menuItemCameraControl = new System.Windows.Forms.MenuItem();
            this.menuItemIOConfiguration = new System.Windows.Forms.MenuItem();
            this.menuItemSave = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.Load_Sequencer = new System.Windows.Forms.MenuItem();
            this.start_Sequencer = new System.Windows.Forms.MenuItem();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonSnap = new System.Windows.Forms.Button();
            this.comboBoxAcquisition = new System.Windows.Forms.ComboBox();
            this.labelAcqMode = new System.Windows.Forms.Label();
            this.comboBoxBayerConversion = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Result = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelColor2 = new System.Windows.Forms.Label();
            this.labelColor1 = new System.Windows.Forms.Label();
            this.labelPixelDepth2 = new System.Windows.Forms.Label();
            this.labelPixelDepth1 = new System.Windows.Forms.Label();
            this.labelUserDefinedName2 = new System.Windows.Forms.Label();
            this.labelSerial2 = new System.Windows.Forms.Label();
            this.labelDeviceVersion2 = new System.Windows.Forms.Label();
            this.labelManufacturer2 = new System.Windows.Forms.Label();
            this.labelModelName2 = new System.Windows.Forms.Label();
            this.labelHeight2 = new System.Windows.Forms.Label();
            this.labelWidth2 = new System.Windows.Forms.Label();
            this.labelHeight1 = new System.Windows.Forms.Label();
            this.labelWidth1 = new System.Windows.Forms.Label();
            this.labelUserDefinedName1 = new System.Windows.Forms.Label();
            this.labelSerial1 = new System.Windows.Forms.Label();
            this.labelDeviceVersion1 = new System.Windows.Forms.Label();
            this.labelManufacturer1 = new System.Windows.Forms.Label();
            this.labelModelName1 = new System.Windows.Forms.Label();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonDiscover = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.GigeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCameraMode,
            this.menuItemCamera,
            this.menuItemSave,
            this.menuItem1});
            // 
            // menuItemCameraMode
            // 
            this.menuItemCameraMode.Index = 0;
            this.menuItemCameraMode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuItemFreerunning,
            this.MenuItemFreeze});
            this.menuItemCameraMode.Text = "Camera Mode";
            this.menuItemCameraMode.Click += new System.EventHandler(this.menuItemCameraMode_Click);
            // 
            // MenuItemFreerunning
            // 
            this.MenuItemFreerunning.Index = 0;
            this.MenuItemFreerunning.Text = "Freerunning";
            this.MenuItemFreerunning.Click += new System.EventHandler(this.menuItemFreerunning_Click);
            // 
            // MenuItemFreeze
            // 
            this.MenuItemFreeze.Index = 1;
            this.MenuItemFreeze.Text = "Freeze";
            this.MenuItemFreeze.Click += new System.EventHandler(this.menuItemFreeze_Click);
            // 
            // menuItemCamera
            // 
            this.menuItemCamera.Index = 1;
            this.menuItemCamera.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCameraControl,
            this.menuItemIOConfiguration});
            this.menuItemCamera.Text = "Camera";
            // 
            // menuItemCameraControl
            // 
            this.menuItemCameraControl.Index = 0;
            this.menuItemCameraControl.Text = "Camera Control";
            this.menuItemCameraControl.Click += new System.EventHandler(this.menuItemCamControl_Click);
            // 
            // menuItemIOConfiguration
            // 
            this.menuItemIOConfiguration.Index = 1;
            this.menuItemIOConfiguration.Text = "Camera IO Configuration";
            this.menuItemIOConfiguration.Click += new System.EventHandler(this.menuItemIOConfiguration_Click);
            // 
            // menuItemSave
            // 
            this.menuItemSave.Index = 2;
            this.menuItemSave.Text = "Save";
            this.menuItemSave.Click += new System.EventHandler(this.menuItemSave_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.Load_Sequencer,
            this.start_Sequencer});
            this.menuItem1.Text = "Sequencer";
            // 
            // Load_Sequencer
            // 
            this.Load_Sequencer.Index = 0;
            this.Load_Sequencer.Text = "Load Sequencer";
            this.Load_Sequencer.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // start_Sequencer
            // 
            this.start_Sequencer.Index = 1;
            this.start_Sequencer.Text = "Start Sequencer";
            this.start_Sequencer.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 688);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(1179, 65);
            this.statusBar1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Desktop;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 733);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Desktop;
            this.panel2.Location = new System.Drawing.Point(14, 14);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(623, 784);
            this.panel2.TabIndex = 0;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Bitmap|*.bmp|All files|*.*";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panelInfo.Controls.Add(this.button2);
            this.panelInfo.Controls.Add(this.button1);
            this.panelInfo.Controls.Add(this.buttonSnap);
            this.panelInfo.Controls.Add(this.comboBoxAcquisition);
            this.panelInfo.Controls.Add(this.labelAcqMode);
            this.panelInfo.Controls.Add(this.comboBoxBayerConversion);
            this.panelInfo.Controls.Add(this.label3);
            this.panelInfo.Controls.Add(this.textBox_Result);
            this.panelInfo.Controls.Add(this.groupBox1);
            this.panelInfo.Controls.Add(this.buttonExit);
            this.panelInfo.Controls.Add(this.buttonDiscover);
            this.panelInfo.Controls.Add(this.label2);
            this.panelInfo.Controls.Add(this.GigeComboBox);
            this.panelInfo.Controls.Add(this.label1);
            this.panelInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelInfo.Location = new System.Drawing.Point(923, 29);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(477, 782);
            this.panelInfo.TabIndex = 3;
            this.panelInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelInfo_Paint);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(295, 704);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 38);
            this.button2.TabIndex = 13;
            this.button2.Text = "stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(180, 704);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 38);
            this.button1.TabIndex = 12;
            this.button1.Text = "start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSnap
            // 
            this.buttonSnap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonSnap.Enabled = false;
            this.buttonSnap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSnap.Location = new System.Drawing.Point(25, 698);
            this.buttonSnap.Name = "buttonSnap";
            this.buttonSnap.Size = new System.Drawing.Size(141, 45);
            this.buttonSnap.TabIndex = 11;
            this.buttonSnap.Text = "Snap Image";
            this.buttonSnap.UseVisualStyleBackColor = false;
            this.buttonSnap.Click += new System.EventHandler(this.buttonSnap_Click);
            // 
            // comboBoxAcquisition
            // 
            this.comboBoxAcquisition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAcquisition.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxAcquisition.FormattingEnabled = true;
            this.comboBoxAcquisition.Items.AddRange(new object[] {
            "Freeze",
            "Freerunning",
            "Software trigger",
            "ext. Trigger / int. Exposure",
            "ext. Trigger / ext. Exposure"});
            this.comboBoxAcquisition.Location = new System.Drawing.Point(25, 639);
            this.comboBoxAcquisition.Name = "comboBoxAcquisition";
            this.comboBoxAcquisition.Size = new System.Drawing.Size(432, 28);
            this.comboBoxAcquisition.TabIndex = 10;
            this.comboBoxAcquisition.SelectedIndexChanged += new System.EventHandler(this.comboBoxAcquisition_SelectedIndexChanged);
            // 
            // labelAcqMode
            // 
            this.labelAcqMode.AutoSize = true;
            this.labelAcqMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAcqMode.Location = new System.Drawing.Point(26, 613);
            this.labelAcqMode.Name = "labelAcqMode";
            this.labelAcqMode.Size = new System.Drawing.Size(178, 25);
            this.labelAcqMode.TabIndex = 9;
            this.labelAcqMode.Text = "Acquisition Mode";
            // 
            // comboBoxBayerConversion
            // 
            this.comboBoxBayerConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBayerConversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxBayerConversion.FormattingEnabled = true;
            this.comboBoxBayerConversion.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.comboBoxBayerConversion.Items.AddRange(new object[] {
            "High Performance (simple)",
            "High Quality (linear)"});
            this.comboBoxBayerConversion.Location = new System.Drawing.Point(29, 567);
            this.comboBoxBayerConversion.Name = "comboBoxBayerConversion";
            this.comboBoxBayerConversion.Size = new System.Drawing.Size(432, 28);
            this.comboBoxBayerConversion.TabIndex = 8;
            this.comboBoxBayerConversion.SelectedIndexChanged += new System.EventHandler(this.comboBoxBayerConversion_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 540);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Bayer Conversion";
            // 
            // textBox_Result
            // 
            this.textBox_Result.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox_Result.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Result.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.textBox_Result.Location = new System.Drawing.Point(25, 436);
            this.textBox_Result.Name = "textBox_Result";
            this.textBox_Result.ReadOnly = true;
            this.textBox_Result.Size = new System.Drawing.Size(420, 30);
            this.textBox_Result.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.labelColor2);
            this.groupBox1.Controls.Add(this.labelColor1);
            this.groupBox1.Controls.Add(this.labelPixelDepth2);
            this.groupBox1.Controls.Add(this.labelPixelDepth1);
            this.groupBox1.Controls.Add(this.labelUserDefinedName2);
            this.groupBox1.Controls.Add(this.labelSerial2);
            this.groupBox1.Controls.Add(this.labelDeviceVersion2);
            this.groupBox1.Controls.Add(this.labelManufacturer2);
            this.groupBox1.Controls.Add(this.labelModelName2);
            this.groupBox1.Controls.Add(this.labelHeight2);
            this.groupBox1.Controls.Add(this.labelWidth2);
            this.groupBox1.Controls.Add(this.labelHeight1);
            this.groupBox1.Controls.Add(this.labelWidth1);
            this.groupBox1.Controls.Add(this.labelUserDefinedName1);
            this.groupBox1.Controls.Add(this.labelSerial1);
            this.groupBox1.Controls.Add(this.labelDeviceVersion1);
            this.groupBox1.Controls.Add(this.labelManufacturer1);
            this.groupBox1.Controls.Add(this.labelModelName1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.groupBox1.Location = new System.Drawing.Point(14, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(448, 294);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Camera Information";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(-91, -32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(309, 134);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // labelColor2
            // 
            this.labelColor2.AutoSize = true;
            this.labelColor2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelColor2.Location = new System.Drawing.Point(190, 255);
            this.labelColor2.Name = "labelColor2";
            this.labelColor2.Size = new System.Drawing.Size(28, 20);
            this.labelColor2.TabIndex = 17;
            this.labelColor2.Text = " - ";
            // 
            // labelColor1
            // 
            this.labelColor1.AutoSize = true;
            this.labelColor1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelColor1.Location = new System.Drawing.Point(7, 255);
            this.labelColor1.Name = "labelColor1";
            this.labelColor1.Size = new System.Drawing.Size(60, 20);
            this.labelColor1.TabIndex = 16;
            this.labelColor1.Text = "Color:";
            // 
            // labelPixelDepth2
            // 
            this.labelPixelDepth2.AutoSize = true;
            this.labelPixelDepth2.Location = new System.Drawing.Point(190, 220);
            this.labelPixelDepth2.Name = "labelPixelDepth2";
            this.labelPixelDepth2.Size = new System.Drawing.Size(32, 25);
            this.labelPixelDepth2.TabIndex = 15;
            this.labelPixelDepth2.Text = " - ";
            // 
            // labelPixelDepth1
            // 
            this.labelPixelDepth1.AutoSize = true;
            this.labelPixelDepth1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPixelDepth1.Location = new System.Drawing.Point(7, 224);
            this.labelPixelDepth1.Name = "labelPixelDepth1";
            this.labelPixelDepth1.Size = new System.Drawing.Size(112, 20);
            this.labelPixelDepth1.TabIndex = 14;
            this.labelPixelDepth1.Text = "Pixel Depth:";
            // 
            // labelUserDefinedName2
            // 
            this.labelUserDefinedName2.AutoSize = true;
            this.labelUserDefinedName2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserDefinedName2.Location = new System.Drawing.Point(191, 140);
            this.labelUserDefinedName2.Name = "labelUserDefinedName2";
            this.labelUserDefinedName2.Size = new System.Drawing.Size(28, 20);
            this.labelUserDefinedName2.TabIndex = 13;
            this.labelUserDefinedName2.Text = " - ";
            // 
            // labelSerial2
            // 
            this.labelSerial2.AutoSize = true;
            this.labelSerial2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSerial2.Location = new System.Drawing.Point(191, 112);
            this.labelSerial2.Name = "labelSerial2";
            this.labelSerial2.Size = new System.Drawing.Size(28, 20);
            this.labelSerial2.TabIndex = 12;
            this.labelSerial2.Text = " - ";
            // 
            // labelDeviceVersion2
            // 
            this.labelDeviceVersion2.AutoSize = true;
            this.labelDeviceVersion2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeviceVersion2.Location = new System.Drawing.Point(191, 82);
            this.labelDeviceVersion2.Name = "labelDeviceVersion2";
            this.labelDeviceVersion2.Size = new System.Drawing.Size(28, 20);
            this.labelDeviceVersion2.TabIndex = 11;
            this.labelDeviceVersion2.Text = " - ";
            // 
            // labelManufacturer2
            // 
            this.labelManufacturer2.AutoSize = true;
            this.labelManufacturer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelManufacturer2.Location = new System.Drawing.Point(191, 55);
            this.labelManufacturer2.Name = "labelManufacturer2";
            this.labelManufacturer2.Size = new System.Drawing.Size(28, 20);
            this.labelManufacturer2.TabIndex = 10;
            this.labelManufacturer2.Text = " - ";
            // 
            // labelModelName2
            // 
            this.labelModelName2.AutoSize = true;
            this.labelModelName2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModelName2.Location = new System.Drawing.Point(191, 32);
            this.labelModelName2.Name = "labelModelName2";
            this.labelModelName2.Size = new System.Drawing.Size(28, 20);
            this.labelModelName2.TabIndex = 9;
            this.labelModelName2.Text = " - ";
            // 
            // labelHeight2
            // 
            this.labelHeight2.AutoSize = true;
            this.labelHeight2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeight2.Location = new System.Drawing.Point(191, 193);
            this.labelHeight2.Name = "labelHeight2";
            this.labelHeight2.Size = new System.Drawing.Size(28, 20);
            this.labelHeight2.TabIndex = 8;
            this.labelHeight2.Text = " - ";
            // 
            // labelWidth2
            // 
            this.labelWidth2.AutoSize = true;
            this.labelWidth2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWidth2.Location = new System.Drawing.Point(191, 166);
            this.labelWidth2.Name = "labelWidth2";
            this.labelWidth2.Size = new System.Drawing.Size(28, 20);
            this.labelWidth2.TabIndex = 7;
            this.labelWidth2.Text = " - ";
            // 
            // labelHeight1
            // 
            this.labelHeight1.AutoSize = true;
            this.labelHeight1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeight1.Location = new System.Drawing.Point(7, 193);
            this.labelHeight1.Name = "labelHeight1";
            this.labelHeight1.Size = new System.Drawing.Size(122, 20);
            this.labelHeight1.TabIndex = 6;
            this.labelHeight1.Text = "Image height:";
            // 
            // labelWidth1
            // 
            this.labelWidth1.AutoSize = true;
            this.labelWidth1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWidth1.Location = new System.Drawing.Point(7, 166);
            this.labelWidth1.Name = "labelWidth1";
            this.labelWidth1.Size = new System.Drawing.Size(115, 20);
            this.labelWidth1.TabIndex = 5;
            this.labelWidth1.Text = "Image width:";
            // 
            // labelUserDefinedName1
            // 
            this.labelUserDefinedName1.AutoSize = true;
            this.labelUserDefinedName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserDefinedName1.Location = new System.Drawing.Point(7, 140);
            this.labelUserDefinedName1.Name = "labelUserDefinedName1";
            this.labelUserDefinedName1.Size = new System.Drawing.Size(173, 20);
            this.labelUserDefinedName1.TabIndex = 4;
            this.labelUserDefinedName1.Text = "User defined name:";
            // 
            // labelSerial1
            // 
            this.labelSerial1.AutoSize = true;
            this.labelSerial1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSerial1.Location = new System.Drawing.Point(7, 112);
            this.labelSerial1.Name = "labelSerial1";
            this.labelSerial1.Size = new System.Drawing.Size(132, 20);
            this.labelSerial1.TabIndex = 3;
            this.labelSerial1.Text = "Serial number:";
            // 
            // labelDeviceVersion1
            // 
            this.labelDeviceVersion1.AutoSize = true;
            this.labelDeviceVersion1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeviceVersion1.Location = new System.Drawing.Point(7, 85);
            this.labelDeviceVersion1.Name = "labelDeviceVersion1";
            this.labelDeviceVersion1.Size = new System.Drawing.Size(140, 20);
            this.labelDeviceVersion1.TabIndex = 2;
            this.labelDeviceVersion1.Text = "Device version:";
            // 
            // labelManufacturer1
            // 
            this.labelManufacturer1.AutoSize = true;
            this.labelManufacturer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelManufacturer1.Location = new System.Drawing.Point(7, 59);
            this.labelManufacturer1.Name = "labelManufacturer1";
            this.labelManufacturer1.Size = new System.Drawing.Size(126, 20);
            this.labelManufacturer1.TabIndex = 1;
            this.labelManufacturer1.Text = "Manufacturer:";
            // 
            // labelModelName1
            // 
            this.labelModelName1.AutoSize = true;
            this.labelModelName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModelName1.Location = new System.Drawing.Point(7, 32);
            this.labelModelName1.Name = "labelModelName1";
            this.labelModelName1.Size = new System.Drawing.Size(116, 20);
            this.labelModelName1.TabIndex = 0;
            this.labelModelName1.Text = "Model name:";
            // 
            // buttonExit
            // 
            this.buttonExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExit.Location = new System.Drawing.Point(406, 737);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(56, 33);
            this.buttonExit.TabIndex = 4;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonDiscover
            // 
            this.buttonDiscover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.buttonDiscover.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDiscover.Location = new System.Drawing.Point(25, 388);
            this.buttonDiscover.Name = "buttonDiscover";
            this.buttonDiscover.Size = new System.Drawing.Size(420, 41);
            this.buttonDiscover.TabIndex = 3;
            this.buttonDiscover.Text = "Discover GigE-Cameras at local network";
            this.buttonDiscover.UseVisualStyleBackColor = false;
            this.buttonDiscover.Click += new System.EventHandler(this.buttonDiscover_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 482);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Select a camera:";
            // 
            // GigeComboBox
            // 
            this.GigeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GigeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GigeComboBox.FormattingEnabled = true;
            this.GigeComboBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.GigeComboBox.Location = new System.Drawing.Point(29, 509);
            this.GigeComboBox.Name = "GigeComboBox";
            this.GigeComboBox.Size = new System.Drawing.Size(432, 28);
            this.GigeComboBox.TabIndex = 1;
            this.GigeComboBox.SelectedIndexChanged += new System.EventHandler(this.GigeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "SVS GigE Example";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1179, 753);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "SVS GigE  -   .NET Sample";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Closed += new System.EventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


        //---------------------------------------------------------------------------


        /// <summary>
        /// Main entry point to application.
        /// </summary>
        //[STAThread]
        //static void Main()
        //{
        //    Application.Run(new Form1());
        //}
        private void initApp()
        {
            isZoomX2 = false;
            openChannel = false;

            selectedIndex = 0;
            camWidth = 0;
            camHeight = 0;
            gigeBitcount = 0;
            PacketSize = 0;

            GigeIsInit = false;

            grab = false;
            initgrab = false;
            notFirstImage = false;

            hCamera = new IntPtr(-1);
            hCameraContainer = -1;
            camWidth = 640;
            camHeight = 480;

            threeBythree = new int[9];
            itemThree = new int[9];
            diffThree = new int[9];

            this.panel1.Left = 0;
            this.panel1.Top = 0;
            this.panel1.Width = 760;
            this.panel1.Height = 700;

            this.panel2.Left = 10;
            this.panel2.Top = 10;
            this.panel2.Width = 720;
            this.panel2.Height = 680;

            menuItemCameraMode.Enabled = false;
            menuItemCamera.Enabled = false;

            bimage = new Bitmap[bufferCount];
            bimageRGB = new Bitmap[bufferCount];

            threadStop = true;
            imgproc = new Thread(new ThreadStart(this.imageProcessing));
            imgproc.IsBackground = true;

            this.menuItemCameraMode.Visible = false;
            this.menuItemCameraMode.Enabled = false;

            this.comboBoxBayerConversion.SelectedIndex = 0;
            this.comboBoxBayerConversion.Enabled = false;
            this.comboBoxAcquisition.SelectedIndex = 0;
            this.comboBoxAcquisition.Enabled = false;

            enableControls(false);
            this.comboBoxBayerConversion.Enabled = false;

            gpanel = panel2.CreateGraphics();

        }


        //---------------------------------------------------------------------------

        private void Form1_Load(object sender, System.EventArgs e)
        {
            this.initApp();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void setGigeHandles(GigeApi mapi, IntPtr hCam, int hCamCont, int xsize, int ysize, string name, int gigebcnt)
        {

            myApi = mapi;
            hCamera = hCam;
            hCameraContainer = hCamCont;
            modelname = name;
            camWidth = xsize;
            camHeight = ysize;
            bitCount = gigebcnt;
            String pixelString = "";

            GigeApi.SVGIGE_PIXEL_DEPTH PixelDepth;
            GigeApi.GVSP_PIXEL_TYPE PixelType;

            // set Camera parameters
            this.labelWidth2.Text = camWidth.ToString();
            this.labelHeight2.Text = camHeight.ToString();

            string strReturn;
            GigeApi.SVSGigeApiReturn apiReturn;

            strReturn = myApi.Gige_Camera_getModelName(hCamera);
            this.labelModelName2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getManufacturerName(hCamera);
            this.labelManufacturer2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getDeviceVersion(hCamera);
            this.labelDeviceVersion2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getSerialNumber(hCamera);
            this.labelSerial2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getUserDefinedName(hCamera);
            this.labelUserDefinedName2.Text = strReturn;

            {
                //--------------------------------------------------------------------------------
                // get pixel depth

                PixelDepth = GigeApi.SVGIGE_PIXEL_DEPTH.SVGIGE_PIXEL_DEPTH_16;

                strReturn = myApi.Gige_Camera_getUserDefinedName(hCamera);

                apiReturn = myApi.Gige_Camera_getPixelDepth(hCamera, ref(PixelDepth));

                if (apiReturn == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    switch (PixelDepth)
                    {
                        case (GigeApi.SVGIGE_PIXEL_DEPTH.SVGIGE_PIXEL_DEPTH_8): { pixelString = "8 Bit"; bitCount = 8; break; }
                        case (GigeApi.SVGIGE_PIXEL_DEPTH.SVGIGE_PIXEL_DEPTH_12): { pixelString = "12 Bit"; bitCount = 12; break; }
                        case (GigeApi.SVGIGE_PIXEL_DEPTH.SVGIGE_PIXEL_DEPTH_16): { pixelString = "16 Bit"; bitCount = 16; break; }

                    }
                }
                else
                {
                    pixelString = "--";
                }

                this.labelPixelDepth2.Text = pixelString;

                //--------------------------------------------------------------------------------
                // get pixel type

                bayerPattern = false;
                PixelType = GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_UNKNOWN;

                apiReturn = myApi.Gige_Camera_getPixelType(hCamera, ref(PixelType));

                if (apiReturn == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {

                    switch (PixelType)
                    {
                        // Mono buffer formats
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_MONO8:    // 8 bit
                            {
                                bayerPattern = false;
                                break;
                            }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_MONO12:     // 16 bit
                            {
                                bayerPattern = false;
                                break;
                            }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_MONO12_PACKED:   // 12 bit
                            {
                                bayerPattern = false;
                                break;
                            }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_MONO16:        // 16 bit
                            {
                                bayerPattern = false;
                                break;
                            }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYGR8: { bayerPattern = true; break; }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYRG8: { bayerPattern = true; break; }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYGB8: { bayerPattern = true; break; }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYBG8: { bayerPattern = true; break; }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYGR12: { bayerPattern = true; break; }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYRG12: { bayerPattern = true; break; }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYGB12: { bayerPattern = true; break; }
                        case GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYBG12: { bayerPattern = true; break; }
                        default:
                            bayerPattern = false;
                            break;
                    }


                    if (bayerPattern)
                    {
                        this.labelColor2.Text = "with Bayer Pattern (color)";
                        comboBoxBayerConversion.Enabled = true;
                    }
                    else
                    {
                        this.labelColor2.Text = "Monochrome (bw) ";
                        comboBoxBayerConversion.Enabled = false;
                    }
                }
            }

            //--------------------------------------------------------------------------
            // adjust Panel-Size

            this.panel1.Width = 760;
            this.panel1.Height = 700;
            this.panel2.Width = 720;
            this.panel2.Height = 680;

            if ((camWidth > 1) && (camHeight > 1))
            {

                float zoomW = camWidth / panel2.Width;
                float zoomH = camHeight / panel2.Height;

                float ratio = 0.0f;
                ratio = (float)((float)camWidth / (float)camHeight);

                if (zoomW < zoomH)
                {

                    float height = (float)this.panel2.Height;
                    this.panel2.Width = (int)(height * ratio);

                }
                else
                {
                    ratio = (float)1.0f / ratio;

                    float width = (float)this.panel2.Width;
                    this.panel2.Height = (int)(width * ratio);
                }
            }
            else
            {

                this.panel1.Width = 760;
                this.panel1.Height = 700;
                this.panel2.Width = 720;
                this.panel2.Height = 680;
            }

            outRectangle = new Rectangle(0, 0, panel2.Width, panel2.Height);

            menuItemCameraMode.Enabled = true;
            GigeIsInit = true;

        }

        //---------------------------------------------------------------------------

        public IntPtr getCamHandle()
        {
            return hCamera;
        }


        //---------------------------------------------------------------------------
        // is called at Discover - Cameras
        //    return = number of current found cameras
        //---------------------------------------------------------------------------

        private int ChooseMyCamera()
        {
            int number = 0;

            if (grab == false)
            {
                if (this.GigeIsInit == true)
                {

                    this.stopGrabbing();
                    this.initApp();
                }

            }
            else
            {
                this.freeze();
                this.stopGrabbing();
                this.initApp();

            }

            number = this.getCameras();

            return number;

        }

        //---------------------------------------------------------------------------
        // initialize and allocate memory buffers
        //---------------------------------------------------------------------------
        private void initializeBuffer()
        {
            int k;

            imagebuffer = new imagebufferStruct[bufferCount];
            outputbuffer = new imagebufferStruct[bufferCount];

            for (k = 0; k < bufferCount; k++)
            {
                imagebuffer[k].imagebytes = new byte[camHeight * camWidth];
                outputbuffer[k].imagebytes = new byte[camHeight * camWidth];
                unsafe
                {
                    fixed (byte* MonoPtr = outputbuffer[k].imagebytes)
                    {
                        bufferReadind = 0;
                        bufferWriteind = 0;
                        bimage[k] = new Bitmap(camWidth, camHeight, camWidth, PixelFormat.Format8bppIndexed, (IntPtr)MonoPtr);

                        imgpal = bimage[k].Palette;

                        // Build bitmap palette Y8
                        for (uint i = 0; i < 256; i++)
                        {
                            imgpal.Entries[i] = Color.FromArgb(
                            (byte)0xFF,
                            (byte)i,
                            (byte)i,
                            (byte)i);
                        }
                        bimage[k].Palette = imgpal;
                        imgpal = bimage[k].Palette;
                    }
                }

            } // for

            // allocate memory for color-image

            imagebufferRGB = new imagebufferStruct[bufferCount];
            outputbufferRGB = new imagebufferStruct[bufferCount];

            for (k = 0; k < bufferCount; k++)
            {

                imagebufferRGB[k].imagebytes = new byte[3 * camHeight * camWidth];
                outputbufferRGB[k].imagebytes = new byte[3 * camHeight * camWidth];

                unsafe
                {

                    fixed (byte* ColorPtr = outputbufferRGB[k].imagebytes)
                    {
                        bimageRGB[k] = new Bitmap(camWidth, camHeight, (3 * camWidth), PixelFormat.Format24bppRgb, (IntPtr)ColorPtr);
                    }
                }

            }  // for

        }

        //---------------------------------------------------------------------------
        // TCP/IP connection has already been established to a selected camera
        //---------------------------------------------------------------------------
        private void startGrabbing()
        {
            this.imageCount = 0;
            this.lostCount = 0;
            this.readimageCount = 0;
            this.overflow = 0;

            fifoCount = 0;

            if (initgrab == false)
            {

                initgrab = true;

                if (GigeIsInit == true)  // = true, if connection to the camera is established and the panel and Buffer 
                //           are initialized
                {

                    notFirstImage = false;

                    // check the current Acquisition-Mode

                    
                    {

                        // set freerunning - mode

                        errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_FIXED_FREQUENCY);

                        if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                        {
                            MessageBox.Show("Errorflag: " + errorflag.ToString());

                        }

                    }

                    this.comboBoxAcquisition.SelectedIndex = 1;
                    this.imageCount = 0;
                    this.lostCount = 0;
                    timer1.Enabled = true;
                    menuItemCamera.Enabled = true;
                    grab = true;

                }  // if GigeIsInit

                initgrab = false;

            }  //if initgrab

        }

        //---------------------------------------------------------------------------
        // set to freerunning mode
        //---------------------------------------------------------------------------
        private void menuItemFreerunning_Click(object sender, System.EventArgs e)
        {

            freerunning();

        }
        //---------------------------------------------------------------------------
        // set acquisition mode at camera
        //---------------------------------------------------------------------------
        private void setAcquisitionMode(GigeApi.ACQUISITION_MODE newAcqmode)
        {

            if (GigeIsInit == true)
            {

                errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, newAcqmode);

                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    MessageBox.Show("Errorflag: " + errorflag.ToString());
                }


            }  //if (GigeIsInit == true)

        }


        //---------------------------------------------------------------------------
        // set to freerunning mode
        //---------------------------------------------------------------------------
        private void freerunning()
        {

            if (GigeIsInit == true)
            {

                    GigeApi.ACQUISITION_MODE acquisitionMode;
                    acquisitionMode = GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER;


                    // check the current acquisition-mode
                    errorflag = myApi.Gige_Camera_getAcquisitionMode(hCamera, ref acquisitionMode);

                    // if camera is freerunning, then do nothing 
                    if (acquisitionMode != GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_FIXED_FREQUENCY)
                    {

                        errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_FIXED_FREQUENCY);

                        if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                        {
                            MessageBox.Show("Errorflag: " + errorflag.ToString());
                        }

                        grab = true;
                    }


            }  //if (GigeIsInit == true)

        }


        //---------------------------------------------------------------------------
        // set to mode : "no acquisition"
        //---------------------------------------------------------------------------
        private void freeze()
        {
            if (GigeIsInit == true)
            {

                errorflag = myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    MessageBox.Show("Errorflag: " + errorflag.ToString());
                }

                grab = false;


            }  //if (GigeIsInit == true)
        }

        //---------------------------------------------------------------------------
        // freeze
        //---------------------------------------------------------------------------
        private void menuItemFreeze_Click(object sender, System.EventArgs e)
        {
            this.freeze();
        }

        //---------------------------------------------------------------------------
        private void Form1_Closed(object sender, System.EventArgs e)
        {
        }


        byte[] globaldata = new byte[1];

        //---------------------------------------------------------------------------
        // Callback - Function
        //---------------------------------------------------------------------------

        [return: MarshalAs(UnmanagedType.Error)]
        public GigeApi.SVSGigeApiReturn myStreamCallback(int Image, IntPtr Context)
        {
            GigeApi.SVSGigeApiReturn apiReturn;
            int xSize, ySize, size;

            IntPtr imgPtr;

            if (running == 1){
                return GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS;
            }
            if (GigeIsInit == false){
                return (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS);
            }
            running = 1;

            imgPtr = myApi.Gige_Image_getDataPointer(Image);
            //GigeApi.SVSGigeApiReturn r = myApi.Gige_StreamingChannel_getBufferData(hStreamingChannel, 2000, 0, ref globaldata);

            if ((int)(imgPtr) == 0)
            {
                running = 0;
                lostCount++;
                return (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS);
            }

            xSize = myApi.Gige_Image_getSizeX(Image);
            ySize = myApi.Gige_Image_getSizeY(Image);
            size = xSize * ySize;    // = Image-Size = Gesamt-Anz. der Pixel
            int bufferWriteind_current = bufferWriteind;
            lock (bimage[bufferWriteind])
            {
                lock (bimageRGB[bufferWriteind])
                {
                    if (fifoCount < bufferCount)
                    {
                        fifoCount++;

                        if (bitCount == 8)
                        {
                            unsafe
                            {
                                if (!bayerPattern)
                                {
                                    System.Runtime.InteropServices.Marshal.Copy(imgPtr, imagebuffer[bufferWriteind].imagebytes, 0, size);
                                }
                                else
                                {
                                    size = camWidth * camHeight * 3;

                                    fixed (byte* dest = imagebufferRGB[bufferReadind].imagebytes)
                                    {
                                        // convert to RGB-Image
                                        apiReturn = myApi.Gige_Image_getImageRGB(Image, dest, size, bayerMethod);
                                    }

                                    if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                                    {
                                        return (apiReturn);
                                    }
                                }
                            }
                        }  // if                                                                                                                 
                        else
                            if (bitCount == 16)
                            {
                              //  MessageBox.Show(" Bits: " + bitCount + "bits", "MyApplication ", MessageBoxButtons.OKCancel);
                            }

                            else if (bitCount == 12)
                            {
                                unsafe
                                {
                                    size = camWidth * camHeight;

                                    fixed (byte* dest = imagebuffer[bufferReadind].imagebytes)

                                        // convert to 8-bit Image
                                        apiReturn = myApi.Gige_Image_getImage12bitAs8bit(Image, dest, size);

                                    if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                                    {
                                        return (apiReturn);
                                    }
                                }
                            }
                            else
                            {
                            //    MessageBox.Show(" Bits: " + bitCount + "bits", "MyApplication ", MessageBoxButtons.OKCancel);
                            }

                        bufferWriteind = (bufferWriteind + 1) % bufferCount;
                    }
                    else
                    {
                        overflow++;
                    }
                }
            }
            if (notFirstImage == false)
            {
                notFirstImage = true;

                if (threadStop == true)
                {
                    threadStop = false;

                    imgproc.Start();
                }
            }
            else
            {
                imgproc.Interrupt();
            }

            imageCount++;
            //string fn = @"C:\temp\test.bmp";
            //GigeApi.SVSGigeApiReturn r1 = myApi.GigE_writeImageToBitmapFile(fn, ref imgPtr, xSize, ySize, GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_MONO8);

            running = 0;



            return (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS);
        }

        //---------------------------------------------------------------------------
        // draw camera image
        //---------------------------------------------------------------------------

        void Redraw(bool color, bool clear)
        {
            if (clear)
            {
                gpanel.Clear(panel2.BackColor);
                return;
            }
            else
            {
                if (drawing == false)
                {
                    drawing = true;
                    int index = bufferReadind;

                    try
                    {
                        if (bimage[index] != null)
                        {
                            if (isZoomX2 == false)
                            {
                                // The image is scaled to fit the output - rectangle
                                if (!color)
                                {
                                    gpanel.DrawImage(bimage[index], outRectangle);
                                }
                                else
                                {
                                   // gpanel.DrawImage(bimageRGB[index], outRectangle);
                                    pictureBox1.Image = bimageRGB[index];
                                }
                            }
                            else
                            {
                                //
                            }
                        }
                    }
                    catch
                    {
                        //
                    }
                }
                drawing = false;
            }
        }

        //---------------------------------------------------------------------------

        private void imageProcessing()
        {
            while (threadStop == false)
            {
                try
                {
                    if (fifoCount > 0)
                    {
                        if (!bayerPattern)
                        {
                            lock (bimage[bufferReadind])
                            {
                                copyToOutput();
                                readimageCount++;
                                fifoCount--;
                            }
                        }
                        else
                        {
                            lock (bimageRGB[bufferReadind])
                            {
                                copyToOutput();
                                readimageCount++;
                                fifoCount--;
                            }
                        }

                        // refresh the items at status bar
                        this.SetStatusBar();
                        bool clear = false;
                        Redraw(bayerPattern, clear);    // display image

                        bufferReadind = (bufferReadind + 1) % bufferCount;

                    }
                    if (fifoCount <= 0)
                    {
                        try
                        {
                            Thread.Sleep(2000);
                        }
                        catch
                        {
                            // go on                    
                        }
                    }
                }

                catch (ThreadInterruptedException)
                {
                    //
                }
            }
        }

        //---------------------------------------------------------------------------
        // copy image to outputbuffer
        //---------------------------------------------------------------------------

        private void copyToOutput()
        {
            int size;

            if (!bayerPattern)
            {
                size = camWidth * camHeight;

                // Copy BW8 image
                System.Buffer.BlockCopy(imagebuffer[bufferReadind].imagebytes, 0, outputbuffer[bufferReadind].imagebytes, 0, size);
            }
            else
            {
                // Copy RGB image

                size = camWidth * camHeight * 3;

                System.Buffer.BlockCopy(imagebufferRGB[bufferReadind].imagebytes, 0, outputbufferRGB[bufferReadind].imagebytes, 0, size);
            }
        }

        //---------------------------------------------------------------------------
        private void SetStatusBar()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.statusBar1.InvokeRequired)
            {
                SetStatusBarCallBack d = new SetStatusBarCallBack(SetStatusBar);
                try
                {
                    this.Invoke(d, new object[] { });
                }
                catch
                {
                    //no problem?? 
                }
            }
            else
            {
                this.statusBar1.Text = "imagecounter: " + imageCount + ";   lost images: " + lostCount + ";   overflow counter: " + overflow + ";   fifo counter: " + fifoCount;
            }
        }

        //---------------------------------------------------------------------------

        private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
        }

        //---------------------------------------------------------------------------

        private void panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            bool clear = false;
            Redraw(bayerPattern, clear);
        }


        //---------------------------------------------------------------------------
        private void Form1_Resize(object sender, System.EventArgs e)
        {
            panel1.Width = this.Width - 10;
            panel1.Height = this.Height - this.statusBar1.Height - 40;
        }

        //---------------------------------------------------------------------------
        // stopGrabbing()
        //     set the Acquisition-Mode to 'no-Acquisition',
        //     deletes the Streaming-Channel and disconnects the camera 
        //---------------------------------------------------------------------------

        private void stopGrabbing()
        {
            threadStop = true;
            if (GigeIsInit == true)
            {
                myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
                grab = false;

                imgproc.Interrupt();

                Thread.Sleep(500);

                errorflag = myApi.Gige_StreamingChannel_delete(hStreamingChannel);

                errorflag = myApi.Gige_Camera_closeConnection(hCamera);
            }
        }

        //---------------------------------------------------------------------------

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            stopGrabbing();
        }

        //---------------------------------------------------------------------------
        // zoom X2
        //---------------------------------------------------------------------------
        private void menuItemZoomX2_Click(object sender, System.EventArgs e)
        {
        }

        //---------------------------------------------------------------------------

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        //---------------------------------------------------------------------------
        public void CameraControlClosed()
        {
            camCtrl = null;
        }

        //---------------------------------------------------------------------------
        // save Image
        //---------------------------------------------------------------------------
        private void menuItemSave_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.ShowDialog();
        }

        //---------------------------------------------------------------------------
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string SaveFilePath;
            SaveFilePath = this.saveFileDialog1.FileName;
            if (bayerPattern)
            {
                bimageRGB[bufferReadind].Save(SaveFilePath, ImageFormat.Bmp);
            }
            else
            {
                bimage[bufferReadind].Save(SaveFilePath, ImageFormat.Bmp);
            }
        }

        //---------------------------------------------------------------------------
        private void buttonDiscover_Click(object sender, EventArgs e)
        {
            this.buttonDiscover.Cursor = Cursors.WaitCursor;
            this.panelInfo.Cursor = Cursors.WaitCursor;
            this.Cursor = Cursors.WaitCursor;

            this.textBox_Result.Text = "Searching for cameras...";
            this.textBox_Result.Refresh();

            int number = 0;
            number = this.ChooseMyCamera();

            if (number > 0)
            {
                buttonDiscover.Enabled = false;

                StringBuilder text = new StringBuilder();
                text.AppendFormat("Found {0} cameras !", number);
                this.textBox_Result.Text = text.ToString();
                this.textBox_Result.ForeColor = Color.Green;
            }
            else
            {
                this.textBox_Result.Text = "No cameras found!";
                this.textBox_Result.ForeColor = Color.Red;
            }

            this.textBox_Result.Refresh();
            this.buttonDiscover.Cursor = Cursors.Default;
            this.panelInfo.Cursor = Cursors.Default;
            this.Cursor = Cursors.Default;

            openChannel = false;
            GigeComboBox.SelectedIndex = 0;

            Thread.Sleep(100);
            openChannel = true;
            GigeComboBox.Focus();
        }

        //---------------------------------------------------------------------------
        // Search for all cameras available in network and add them to a list
        //---------------------------------------------------------------------------

        private int getCameras()
        {
            int camAnz = 0;
            int n = 0;
            string SN = "";

            myApi = new GigeApi();

            if (hCameraContainer != GigeApi.SVGigE_NO_CLIENT)
            {
                myApi.Gige_CameraContainer_delete(hCameraContainer);
            }
            hCameraContainer = myApi.Gige_CameraContainer_create(GigeApi.SVGigETL_Type.SVGigETL_TypeFilter);

            if (hCameraContainer >= 0)
            {
                errorflag = myApi.Gige_CameraContainer_discovery(hCameraContainer);

                if (errorflag == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    camAnz = myApi.Gige_CameraContainer_getNumberOfCameras(hCameraContainer);

                    GigeComboBox.Items.Add("Select Camera!");

                    if (camAnz > 0)
                    {
                        for (n = 0; n < camAnz; n++)
                        {
                            hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, n);
                            modelname = myApi.Gige_Camera_getModelName(hCamera);
                            SN = myApi.Gige_Camera_getSerialNumber(hCamera);
                            GigeComboBox.Items.Add(modelname + " SN: " + SN);
                        }
                    }

                    GigeComboBox.Focus();
                }
                else
                {
                    MessageBox.Show("Error Containerdiscovery:");
                    return 0;
                }
            }
            else
            {
                MessageBox.Show("Error ContainerCreate:");
                return 0;
            }

            return camAnz;
        }

        //---------------------------------------------------------------------------
        // select camera from comboBox
        //---------------------------------------------------------------------------
        private void GigeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GigeComboBox.Enabled = false;
            this.panelInfo.Cursor = Cursors.WaitCursor;

            int n;

            selectedIndex = GigeComboBox.SelectedIndex - 1;

            if ((selectedIndex == cameraSelection) || (selectedIndex == -1))
            {
                this.GigeComboBox.Enabled = true;
                this.panelInfo.Cursor = Cursors.Default;
                return;
            }

            GigeApi.SVSGigeApiReturn apiReturn;

            if (openChannel)  // will be set to 'true' after a Discover
            {
                n = selectedIndex;
                cameraSelection = n;

                //------------------------------------------------------------------
                // if camera is already connected then it needs to be stopped
                //------------------------------------------------------------------

                if (cameraConnected)
                {
                    apiReturn = myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
                    if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    {
                        MessageBox.Show("Errorflag: " + apiReturn.ToString());
                        enableControls(false);
                        Redraw(false, true);
                    }

                    // Remove streaming channel
                    apiReturn = myApi.Gige_StreamingChannel_delete(hStreamingChannel);

                    hStreamingChannel = new IntPtr(-1);

                    if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    {
                        MessageBox.Show("Errorflag: " + apiReturn.ToString());
                    }

                    // Close camera
                    apiReturn = myApi.Gige_Camera_closeConnection(hCamera);

                    if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    {
                        MessageBox.Show("Errorflag: " + apiReturn.ToString());
                    }

                    hCamera = new IntPtr(-1);

                    cameraConnected = false;
                }  // if cameraConnected

                hCamera = new IntPtr(GigeApi.SVGigE_NO_CAMERA);

                hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, n);

                modelname = myApi.Gige_Camera_getModelName(hCamera);

                uint IPAddress = 0;
                uint SubnetMask = 0;

                //Open connection to camera. A control channel will be established.
                errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);

                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_CAMERA_OCCUPIED)
                    {
                        
                        {
                            // Try assigning a valid network address
                            errorflag = myApi.Gige_Camera_forceValidNetworkSettings(hCamera, ref IPAddress, ref SubnetMask);
                        }

                        if (errorflag == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                        {
                            // Try opening the camera again
                            errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);
                        }

                    }

                    if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    {
                        MessageBox.Show("Errorflag: " + errorflag.ToString());
                        cameraSelection = -99;
                        cameraConnected = false;
                        this.GigeComboBox.Enabled = true;
                        this.panelInfo.Cursor = Cursors.Default;
                        enableControls(false);
                        Redraw(false, true);

                        return;
                    }
                }

                // Create a StreamCallback
                GigeCallBack = new GigeApi.StreamCallback(this.myStreamCallback);

                // Adjust camera to maximal possible packet size
                myApi.Gige_Camera_evaluateMaximalPacketSize(hCamera, ref(PacketSize));

                // Adjust desired binning mode (default = OFF, others: 2x2, 3x3, 4x4)
                myApi.Gige_Camera_setBinningMode(hCamera, GigeApi.BINNING_MODE.BINNING_MODE_OFF);
                // Create a matching streaming channel
                errorflag = myApi.Gige_StreamingChannel_create(ref hStreamingChannel, hCameraContainer, hCamera, 10, GigeCallBack, new IntPtr(0));
                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    MessageBox.Show("Errorflag: " + errorflag.ToString());
                    cameraSelection = -99;
                    cameraConnected = false;
                    this.GigeComboBox.Enabled = true;
                    this.panelInfo.Cursor = Cursors.Default;
                    enableControls(false);
                    Redraw(false, true);

                    return;
                }

                cameraConnected = true;

                camWidth = 0;

                errorflag = myApi.Gige_Camera_getSizeX(hCamera, ref(camWidth));

                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    MessageBox.Show("Errorflag: " + errorflag.ToString());
                }
                camHeight = 0;
                errorflag = myApi.Gige_Camera_getSizeY(hCamera, ref(camHeight));

                this.setGigeHandles(myApi, hCamera, hCameraContainer, camWidth, camHeight, modelname, gigeBitcount);

                // allocate memory for buffers
                this.initializeBuffer();

                // set camera-mode to freerunning
                //startGrabbing();

            } // if openChannel

            this.GigeComboBox.Enabled = true;

            enableControls(true);

            this.panelInfo.Cursor = Cursors.Default;
        }

        //---------------------------------------------------------------------------
        private void enableControls(bool enable)
        {
            if (enable)
            {

                this.comboBoxAcquisition.Enabled = true;
                this.menuItemCamera.Enabled = true;
                this.comboBoxBayerConversion.Enabled = bayerPattern;
                this.buttonSnap.Enabled = true;


            }
            else
            {
                this.comboBoxAcquisition.Enabled = false;
                this.menuItemCamera.Enabled = false;
                this.comboBoxBayerConversion.Enabled = false;
                this.buttonSnap.Enabled = false;
            }
        }

        //---------------------------------------------------------------------------
        private void buttonExit_Click(object sender, EventArgs e)
        {
            stopGrabbing();

            this.Close();
        }

        private void menuItemCameraMode_Click(object sender, EventArgs e)
        {
        }

        //---------------------------------------------------------------------------
        private void comboBoxAcquisition_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (GigeIsInit != true)
                return;

            this.enableControls(false);
            this.comboBoxAcquisition.Enabled = false;
            this.panelInfo.Cursor = Cursors.WaitCursor;
            this.Cursor = Cursors.WaitCursor;

            int index = comboBoxAcquisition.SelectedIndex;

            switch (index)
            {
                case 0:   // no acquisition
                    {
                        myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
                        break;
                    }

                case 1:   // freerunning
                    {
                        setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_FIXED_FREQUENCY);
                        break;
                    }
                case 2:   // Software trigger
                    {
                        setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER);
                        break;
                    }
                case 3:   // ext. Trigger / int. Exposure
                    {
                        setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_INT_EXPOSURE);
                        break;
                    }
                case 4:   // ext. Trigger / ext. Exposure
                    {
                        setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE);
                        break;
                    }
            }

            this.comboBoxAcquisition.Enabled = true;
            this.panelInfo.Cursor = Cursors.Default;
            this.Cursor = Cursors.Default;
            this.enableControls(true);
        }

        //---------------------------------------------------------------------------
        private void buttonSnap_Click(object sender, EventArgs e)
        {

            if (GigeIsInit != true)
                return;

            this.enableControls(false);

            setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER);

            // Snap single image

            errorflag = myApi.Gige_Camera_startAcquisitionCycle(hCamera);

            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                MessageBox.Show("Errorflag: " + errorflag.ToString());
            }

            comboBoxAcquisition.SelectedIndex = 2;

            this.enableControls(true);

        }

        //---------------------------------------------------------------------------
        // Select Bayer-Conversation-Method
        //---------------------------------------------------------------------------
        private void comboBoxBayerConversion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GigeIsInit != true)
                return;

            if (selectBayer == false)
            {
                selectBayer = true;

                int index = comboBoxBayerConversion.SelectedIndex;

                switch (index)
                {
                    case 0: bayerMethod = GigeApi.BAYER_METHOD.BAYER_METHOD_SIMPLE; break;
                    case 1: bayerMethod = GigeApi.BAYER_METHOD.BAYER_METHOD_HQLINEAR; break;
                }

                Thread.Sleep(100);
            }
            selectBayer = false;
        }

        //---------------------------------------------------------------------------
        private void menuItemCamControl_Click(object sender, EventArgs e)
        {
            if (GigeIsInit == true)
            {
                if (camCtrl == null)
                {
                    camCtrl = new CameraControl(this);
                }
                camCtrl.ShowDialog();
            }
        }

        //---------------------------------------------------------------------------
        void menuItemIOConfiguration_Click(object sender, EventArgs e)
        {
            if (GigeIsInit == true)
            {

                if (ioConfiguration == null)
                {
                    ioConfiguration = new FormIOConfig(this);
                }
                ioConfiguration.ShowDialog();
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            string Pfad = string.Empty;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Pfad = openFileDialog1.FileName;


                myApi.Gige_Camera_setIOAssignment(hCamera, GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT1,
                                GigeApi.SVGigE_IOMux_IN_PWMA);

                myApi.Gige_Camera_setIOAssignment(hCamera, GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT2,
                               GigeApi.SVGigE_IOMux_IN_PWMB);

                GigeApi.SVSGigeApiReturn apiReturn;

                apiReturn = myApi.Gige_Camera_loadSequenceParameters(hCamera, Pfad);


                if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    MessageBox.Show(" load sequencer failed!!!");
                else
                    MessageBox.Show(" load sequencer succeseful!!!");
            }

        }

        private void menuItem3_Click(object sender, EventArgs e)
        {

            if (hCamera != new IntPtr(GigeApi.SVGigE_NO_CAMERA))
            {
                GigeApi.SVSGigeApiReturn status;
                status = myApi.Gige_Camera_startSequencer(hCamera);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GigeIsInit != true)
                return;

          //  this.enableControls(false);
            setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER);
            setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE);

  
            // Snap single image

          

            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                MessageBox.Show("Errorflag: " + errorflag.ToString());
            }

           // comboBoxAcquisition.SelectedIndex = 2;

            this.enableControls(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (GigeIsInit != true)
                return;

            //  this.enableControls(false);

          //  setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE);

            // Snap single image

            setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER);

            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                MessageBox.Show("Errorflag: " + errorflag.ToString());
            }

           // comboBoxAcquisition.SelectedIndex = 2;

            this.enableControls(true);
        }

        private void panelInfo_Paint(object sender, PaintEventArgs e)
        {

        }



    }   // public class Form1
}   // namespace GigEApi
	

