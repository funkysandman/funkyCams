using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

using QCamManagedDriver;

namespace CSharpPreviewManDrv
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    ///
    public class Form1 : System.Windows.Forms.Form
    {
        //my global variables
        private QCamM_CamListItem[] mCamList;
        private IntPtr              mhCamera;
        private myPanel             mDisplayPanel;
        private bool                mIsMono;
        private Bitmap              mDisplayBitmap;
        private QCamM_AsyncCallback mFrameCallback;
        private QCamM_Frame         mFrame1;
        private QCamM_Frame         mFrame2;
        private QCamM_Frame         mRgbFrame;
        private QCamM_SettingsEx    mSettings;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private Button button1;
        private CheckBox checkBox1;
        private TrackBar tbExposure;
        private Label lblExposureVal;
        private Panel panel1;
        private GroupBox gbAcquisition;
        private GroupBox gbExposure;
        private GroupBox gbGain;
        private Label lblGainVal;
        private TrackBar tbGain;
        private GroupBox gbInfo;
        private Label lblCameraModel;
        private PictureBox pictureBox1;
        private Label lblSerNum;



        public Form1()
        {
            InitializeComponent();

            // check if opening a camera was successful
            if ( QCamM_Err.qerrSuccess != QCam.QCamM_LoadDriver() )
            {
                System.Windows.Forms.MessageBox.Show( "The application was unable to load the QCam driver." );
                System.Environment.Exit( 0 );
            }

            if ( !OpenCamera() )
            {
                System.Windows.Forms.MessageBox.Show( "The application was unable to connect to a QImaging camera.  Please ensure one is connected and turned on before running this application." );
                System.Environment.Exit( 0 );
            }

            if ( !InitCamera() )
            {
                System.Windows.Forms.MessageBox.Show( "Failed to initialize the camera" );
                System.Environment.Exit( 0 );
            }
            GrabFrame();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if ( disposing )
            {
                QCam.QCamM_Abort( mhCamera );

                if ( mFrame1 != null )
                    QCam.QCamM_Free( mFrame1.pBuffer );
                if ( mFrame2 != null )
                    QCam.QCamM_Free( mFrame2.pBuffer );
                if ( mRgbFrame != null )
                    QCam.QCamM_Free( mRgbFrame.pBuffer );

                QCam.QCamM_ReleaseCameraSettingsStruct( mSettings );

                QCam.QCamM_CloseCamera( mhCamera );

                QCam.QCamM_ReleaseDriver();

                if ( components != null )
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tbExposure = new System.Windows.Forms.TrackBar();
            this.lblExposureVal = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbInfo = new System.Windows.Forms.GroupBox();
            this.lblSerNum = new System.Windows.Forms.Label();
            this.lblCameraModel = new System.Windows.Forms.Label();
            this.gbGain = new System.Windows.Forms.GroupBox();
            this.lblGainVal = new System.Windows.Forms.Label();
            this.tbGain = new System.Windows.Forms.TrackBar();
            this.gbExposure = new System.Windows.Forms.GroupBox();
            this.gbAcquisition = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbExposure)).BeginInit();
            this.panel1.SuspendLayout();
            this.gbInfo.SuspendLayout();
            this.gbGain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbGain)).BeginInit();
            this.gbExposure.SuspendLayout();
            this.gbAcquisition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 37);
            this.button1.TabIndex = 1;
            this.button1.Text = "GrabFrame";
            this.button1.Click += new System.EventHandler(this.OnGrabFrame);
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(7, 61);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 23);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Streaming";
            this.checkBox1.Click += new System.EventHandler(this.OnStream);
            // 
            // tbExposure
            // 
            this.tbExposure.Location = new System.Drawing.Point(4, 22);
            this.tbExposure.Maximum = 1000;
            this.tbExposure.Minimum = 10;
            this.tbExposure.Name = "tbExposure";
            this.tbExposure.Size = new System.Drawing.Size(124, 56);
            this.tbExposure.TabIndex = 4;
            this.tbExposure.TickFrequency = 100;
            this.tbExposure.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbExposure.Value = 10;
            this.tbExposure.ValueChanged += new System.EventHandler(this.tbExposure_ValueChanged);
            // 
            // lblExposureVal
            // 
            this.lblExposureVal.AutoSize = true;
            this.lblExposureVal.Location = new System.Drawing.Point(139, 37);
            this.lblExposureVal.Name = "lblExposureVal";
            this.lblExposureVal.Size = new System.Drawing.Size(46, 17);
            this.lblExposureVal.TabIndex = 6;
            this.lblExposureVal.Text = "10 ms";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbInfo);
            this.panel1.Controls.Add(this.gbGain);
            this.panel1.Controls.Add(this.gbExposure);
            this.panel1.Controls.Add(this.gbAcquisition);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 547);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(712, 86);
            this.panel1.TabIndex = 7;
            // 
            // gbInfo
            // 
            this.gbInfo.Controls.Add(this.lblSerNum);
            this.gbInfo.Controls.Add(this.lblCameraModel);
            this.gbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbInfo.Location = new System.Drawing.Point(510, 0);
            this.gbInfo.Name = "gbInfo";
            this.gbInfo.Size = new System.Drawing.Size(202, 86);
            this.gbInfo.TabIndex = 10;
            this.gbInfo.TabStop = false;
            this.gbInfo.Text = "Info";
            // 
            // lblSerNum
            // 
            this.lblSerNum.AutoSize = true;
            this.lblSerNum.Location = new System.Drawing.Point(8, 38);
            this.lblSerNum.Name = "lblSerNum";
            this.lblSerNum.Size = new System.Drawing.Size(48, 17);
            this.lblSerNum.TabIndex = 2;
            this.lblSerNum.Text = "Serial:";
            // 
            // lblCameraModel
            // 
            this.lblCameraModel.AutoSize = true;
            this.lblCameraModel.Location = new System.Drawing.Point(7, 18);
            this.lblCameraModel.Name = "lblCameraModel";
            this.lblCameraModel.Size = new System.Drawing.Size(50, 17);
            this.lblCameraModel.TabIndex = 1;
            this.lblCameraModel.Text = "Model:";
            // 
            // gbGain
            // 
            this.gbGain.Controls.Add(this.lblGainVal);
            this.gbGain.Controls.Add(this.tbGain);
            this.gbGain.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbGain.Location = new System.Drawing.Point(324, 0);
            this.gbGain.Name = "gbGain";
            this.gbGain.Size = new System.Drawing.Size(186, 86);
            this.gbGain.TabIndex = 9;
            this.gbGain.TabStop = false;
            this.gbGain.Text = "Gain";
            // 
            // lblGainVal
            // 
            this.lblGainVal.AutoSize = true;
            this.lblGainVal.Location = new System.Drawing.Point(133, 38);
            this.lblGainVal.Name = "lblGainVal";
            this.lblGainVal.Size = new System.Drawing.Size(16, 17);
            this.lblGainVal.TabIndex = 1;
            this.lblGainVal.Text = "0";
            // 
            // tbGain
            // 
            this.tbGain.Location = new System.Drawing.Point(2, 22);
            this.tbGain.Name = "tbGain";
            this.tbGain.Size = new System.Drawing.Size(125, 56);
            this.tbGain.TabIndex = 0;
            this.tbGain.TickFrequency = 200;
            this.tbGain.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.tbGain.ValueChanged += new System.EventHandler(this.tbGain_ValueChanged);
            // 
            // gbExposure
            // 
            this.gbExposure.Controls.Add(this.tbExposure);
            this.gbExposure.Controls.Add(this.lblExposureVal);
            this.gbExposure.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbExposure.Location = new System.Drawing.Point(113, 0);
            this.gbExposure.Name = "gbExposure";
            this.gbExposure.Size = new System.Drawing.Size(211, 86);
            this.gbExposure.TabIndex = 8;
            this.gbExposure.TabStop = false;
            this.gbExposure.Text = "Exposure";
            // 
            // gbAcquisition
            // 
            this.gbAcquisition.Controls.Add(this.button1);
            this.gbAcquisition.Controls.Add(this.checkBox1);
            this.gbAcquisition.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbAcquisition.Location = new System.Drawing.Point(0, 0);
            this.gbAcquisition.Name = "gbAcquisition";
            this.gbAcquisition.Size = new System.Drawing.Size(113, 86);
            this.gbAcquisition.TabIndex = 7;
            this.gbAcquisition.TabStop = false;
            this.gbAcquisition.Text = "Acquisition";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(44, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(608, 470);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(712, 633);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "QCamManagedDriver Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.tbExposure)).EndInit();
            this.panel1.ResumeLayout(false);
            this.gbInfo.ResumeLayout(false);
            this.gbInfo.PerformLayout();
            this.gbGain.ResumeLayout(false);
            this.gbGain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbGain)).EndInit();
            this.gbExposure.ResumeLayout(false);
            this.gbExposure.PerformLayout();
            this.gbAcquisition.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run( new Form1() );
        }

        /// <summary>
        /// Initializes the camera and UI based on camera capabilities
        /// </summary>
        /// <returns>True if succeeded</returns>
        private bool OpenCamera()
        {
            mCamList = new QCamM_CamListItem[10];
            uint listLen = 10;

            // listLen is the length of your QCam_CamListItem array
            QCam.QCamM_ListCameras( mCamList, ref listLen );
            // listLen is now the number of cameras available. It may be
            // larger than your QCam_CamListItem array length!
            if ( ( listLen > 0 ) && ( mCamList[0].isOpen == 0 ) )
            {
                // Open the first camera in the list.
                if ( QCam.QCamM_OpenCamera( mCamList[0].cameraId, ref mhCamera ) != QCamM_Err.qerrSuccess )
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Prepares the camera and resources for single image grab
        /// </summary>
        /// <returns>True if succeded</returns>
        private bool InitCamera()
        {
            uint ccdType = 0;
            QCamM_Err err = QCamM_Err.qerrSuccess;

            // Prepare the camera settings structure
            mSettings = new QCamM_SettingsEx();
            QCam.QCamM_CreateCameraSettingsStruct( mSettings );
            QCam.QCamM_InitializeCameraSettings( mhCamera, mSettings );
            QCam.QCamM_ReadDefaultSettings( mhCamera, mSettings );

            //check what type of CCD this camera has
            err = QCam.QCamM_GetInfo( mhCamera, QCamM_Info.qinfCcdType, ref ccdType );

            // Set the default frame format based on CCD type
            if ( ccdType == Convert.ToUInt32( QCamM_qcCcdType.qcCcdColorBayer ) )
            {
                err = QCam.QCamM_SetParam( mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32( QCamM_ImageFormat.qfmtBayer8 ) );
                mIsMono = false;
            }
            else if ( ccdType == Convert.ToUInt32( QCamM_qcCcdType.qcCcdMonochrome ) )
            {
                err = QCam.QCamM_SetParam( mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32( QCamM_ImageFormat.qfmtMono8 ) );
                mIsMono = true;
            }
            else
                return false;

            // Send the settings to the camera
            err = QCam.QCamM_SendSettingsToCam( mhCamera, mSettings );

            // Prepare all our buffers
            uint frameSize = 0;
            QCam.QCamM_GetInfo( mhCamera, QCamM_Info.qinfImageSize, ref frameSize );

            mFrame1 = new QCamM_Frame();
            mFrame1.bufferSize = frameSize;
            mFrame1.pBuffer = QCam.QCamM_Malloc( mFrame1.bufferSize );
            mFrame2 = new QCamM_Frame();
            mFrame2.bufferSize = frameSize;
            mFrame2.pBuffer = QCam.QCamM_Malloc( mFrame2.bufferSize );
            if ( !mIsMono )
            {
                // Prepare a temporary buffer for color cameras (used with QCam_BayerToRgb)
                mRgbFrame = new QCamM_Frame();
                mRgbFrame.bufferSize = frameSize * 3;
                mRgbFrame.pBuffer = QCam.QCamM_Malloc( mRgbFrame.bufferSize );
                // Use BGR format as this is actually the Windows RGB
                mRgbFrame.format = (uint)QCamM_ImageFormat.qfmtBgr24;
            }


            // Prepare the UI control based on camera capabilities
            if ( QCam.QCamM_IsParamSupported( mhCamera, QCamM_Param.qprmGain ) == QCamM_Err.qerrSuccess )
            {
                gbGain.Enabled = true;
                uint val = 0;
                QCam.QCamM_GetParamMin( mSettings, QCamM_Param.qprmGain, ref val );
                tbGain.Minimum = (int)val;
                QCam.QCamM_GetParamMax( mSettings, QCamM_Param.qprmGain, ref val );
                tbGain.Maximum = (int)val;
                QCam.QCamM_GetParam( mSettings, QCamM_Param.qprmGain, ref val );
                tbGain.Value = (int)val;
            }
            else
            {
                gbGain.Enabled = false;
            }
            // Get the current exposure value set in the camera
            if ( QCam.QCamM_IsParamSupported( mhCamera, QCamM_Param.qprmExposure ) == QCamM_Err.qerrSuccess )
            {
                gbExposure.Enabled = true;
                uint val = 0;
                QCam.QCamM_GetParam( mSettings, QCamM_Param.qprmExposure, ref val );
                tbExposure.Value = (int)( val / 1000 ); // Convert from us to ms
            }
            else
            {
                gbExposure.Enabled = false;
            }

            lblCameraModel.Text = "Model: " + ( (QCamM_qcCameraType)( mCamList[0].cameraType ) ).ToString().Remove( 0, 8 ); // remove the "qcCamera" enum prefix
            string serNum = "";
            QCam.QCamM_GetSerialString( mhCamera, ref serNum );
            lblSerNum.Text = "Serial: " + serNum;

            // Create the callback delegate for streaming
            mFrameCallback = new QCamM_AsyncCallback( frameCallback );

            return true;
        }

        /// <summary>
        /// Stops the streaming
        /// </summary>
        private void StopStream()
        {
            QCam.QCamM_Abort( mhCamera );
            QCam.QCamM_SetStreaming( mhCamera, 0 );
        }

        /// <summary>
        /// Prepares the camera and resources for live image streaming
        /// </summary>
        /// <returns></returns>
        private void StartStream()
        {
            QCam.QCamM_Abort( mhCamera );
            QCam.QCamM_SetStreaming( mhCamera, 1 );
            QueueFrame( 1 );
            QueueFrame( 2 );
        }

        /// <summary>
        /// Grabs a single frame
        /// </summary>
        public void GrabFrame()
        {
            uint width, height;

            QCamM_Err err = QCamM_Err.qerrSuccess;

            uint sizeInBytes = 0;
            // Image size depends on the current region & image format.
            QCam.QCamM_GetInfo( mhCamera, QCamM_Info.qinfImageSize, ref sizeInBytes );

            err = QCam.QCamM_GrabFrame( mhCamera, mFrame1 );


            width = mFrame1.width;
            height = mFrame1.height;

            Bitmap bmp = null;
            if ( mIsMono )
            {
                // .NET does not have 8bit grayscale pixel format, so we use 8bit indexed...
                bmp = new Bitmap( (int)width, (int)height, (int)width, PixelFormat.Format8bppIndexed, mFrame1.pBuffer );
                // And set the color palette to 256 shades of gray
                ColorPalette pt = bmp.Palette;
                for ( int i = 0; i < pt.Entries.Length; i++ )
                    pt.Entries[i] = Color.FromArgb( i, i, i );
                bmp.Palette = pt;
            }
            else
            {
                // Convert the input buffer to standart RGB pixel format so we can easily construct a Bitmap from it
                // Use the QCam provided conversion method which is HW accelerated
                QCamImgfnc.QCamM_BayerToRgb( QCamM_qcBayerInterp.qcBayerInterpFast, mFrame1, mRgbFrame );
                bmp = new Bitmap( (int)width, (int)height, (int)width * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer );
            }
            mDisplayBitmap = bmp;
            pictureBox1.Image = bmp;

            try
            {
                using ( FileStream fs = new FileStream( "image.raw", FileMode.Create ) )
                {
                    BinaryWriter bw = new System.IO.BinaryWriter( fs );
                    for ( int i = 0; i < mFrame1.size; i++ )
                    {
                        bw.Write( Marshal.ReadByte( mFrame1.pBuffer, i ) );
                    }
                    bw.Close();
                }
            }
            catch ( Exception e )
            {
                MessageBox.Show( "Unable to save the image data: " + e.Message );
            }

        }

        /// <summary>
        /// Frame callback. Called when the frame is exposed and/or fully read out.
        /// </summary>
        /// <param name="userPtr">User defined</param>
        /// <param name="userData">User defined</param>
        /// <param name="errcode">Error code</param>
        /// <param name="flags">Combination of QCam_qcCallbackFlags</param>
        private void frameCallback
            (
            IntPtr userPtr,
            uint userData,
            QCamM_Err errcode,
            uint flags
            )
        {
            QCamM_Frame myFrame;

            // Find out for which frame the callback was sent
            if ( userData == 1 )
                myFrame = mFrame1;
            else if ( userData == 2 )
                myFrame = mFrame2;
            else
                return;

            uint width = myFrame.width;
            uint height = myFrame.height;

            Bitmap bmp;

            if ( mIsMono )
            {
                bmp = new Bitmap( (int)width, (int)height, (int)width, PixelFormat.Format8bppIndexed, myFrame.pBuffer );

                ColorPalette pt = bmp.Palette;
                for ( int i = 0; i < pt.Entries.Length; i++ )
                    pt.Entries[i] = Color.FromArgb( i, i, i );
                bmp.Palette = pt;
            }
            else
            {
                // Convert the RAW Bayer data to RGB color space. Note the RGB image will be 3 times larger.
                QCamImgfnc.QCamM_BayerToRgb( QCamM_qcBayerInterp.qcBayerInterpFast, myFrame, mRgbFrame );
                bmp = new Bitmap( (int)width, (int)height, (int)width * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer );
            }
            mDisplayBitmap = bmp;

            QueueFrame( userData );  // Image processing done, queue the same frame again

            mDisplayPanel.Invalidate();
        }
        
        private void panel1_Paint( object sender, System.Windows.Forms.PaintEventArgs e )
        {
            if ( mDisplayBitmap != null )
            {
                Graphics g = e.Graphics;
                g.DrawImage( mDisplayBitmap, 0, 0, mDisplayPanel.Width, mDisplayPanel.Height );
            }
        }
        

        private void OnGrabFrame( object sender, System.EventArgs e )
        {
            GrabFrame();
        }

        private bool QueueFrame( uint frameNum )
        {
            QCamM_Err err;

            if ( frameNum == 1 )
                err = QCam.QCamM_QueueFrame( this.mhCamera, mFrame1, mFrameCallback, (uint)QCamM_qcCallbackFlags.qcCallbackDone, IntPtr.Zero, frameNum );
            else if ( frameNum == 2 )
                err = QCam.QCamM_QueueFrame( this.mhCamera, mFrame2, mFrameCallback, (uint)QCamM_qcCallbackFlags.qcCallbackDone, IntPtr.Zero, frameNum );
            else
                return false;

            if ( err == QCamM_Err.qerrSuccess )
                return true;
            else
                return false;
        }

        bool streaming = false;
        private void OnStream( object sender, System.EventArgs e )
        {
            streaming = !streaming;
            button1.Enabled = !streaming;

            if ( streaming )
                StartStream();
            else
                StopStream();
        }



        private void Form1_SizeChanged( object sender, EventArgs e )
        {
            //mDisplayPanel.Invalidate();
        }

        private void tbGain_ValueChanged( object sender, EventArgs e )
        {
            lblGainVal.Text = tbGain.Value.ToString();
            QCam.QCamM_SetParam( mSettings, QCamM_Param.qprmGain, (uint)( tbGain.Value ) );
            QCam.QCamM_SendSettingsToCam( mhCamera, mSettings );
        }

        private void tbExposure_ValueChanged( object sender, EventArgs e )
        {
            lblExposureVal.Text = tbExposure.Value.ToString() + " ms";
            QCam.QCamM_SetParam( mSettings, QCamM_Param.qprmExposure, (uint)( tbExposure.Value * 1000 ) );
            QCam.QCamM_SendSettingsToCam( mhCamera, mSettings );
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    // A custom panel class with disabled background paint - helps to resolve the flickering
    public class myPanel : System.Windows.Forms.Panel
    {
        protected override void OnPaintBackground( PaintEventArgs pevent )
        {
        }
    }


}
