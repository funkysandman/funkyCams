using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.Threading;
using System.IO;
using GigEApi;
using System.Diagnostics;




namespace SVS_Wrapper
{
    public class SVS_Vistek_Grabber
    {
        private bool bayerPattern;
        private const int bufferCount = 1;
        //private bool bayerPattern = false;
        //  private bool drawing = false;
        // private bool selectBayer = false;
        private GigeApi.BAYER_METHOD bayerMethod = GigeApi.BAYER_METHOD.BAYER_METHOD_HQLINEAR;

        private ObjectDetection.TFDetector md = new ObjectDetection.TFDetector();
        


        private bool m_saveLocal;
        // private string m_savePath;

        private int camWidth, camHeight;
        
        private int PacketSize;

        //  private Rectangle outRectangle;

        private bool m_isStreaming = false;
        private int m_duration;
        private float m_aGain;
        private float m_dGain;
        private float m_offset;
        private float m_gamma;
        private bool m_useDarks = false;
        private bool isZoomX2;
        private int imageCount, readimageCount, lostCount;

        private bool openChannel;
        private int selectedIndex;
        private int gigeBitcount;

        public GigeApi myApi;

        //private CameraControl camCtrl;
        //private FormIOConfig ioConfiguration;

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
        float timeout = 10.0f;//1500.0f;                    // extended timeout for maintaining connection during a debug session
#else
        float timeout = 10.0f;                     // timeout: the time without traffic or heartbeat
                                                 //          after which a camera drops a connection
#endif
        //---------------------------------------


        private int bufferWriteind;
        private int bufferReadind;
        private int overflow;
        private int fifoCount;

        private Bitmap[] bimage;
        public Bitmap[] bimageRGB;
        // The object that will contain the palette information for the bitmap
        private ColorPalette imgpal = null;

        private bool grab, initgrab;
        private float _darkmultiplier;

        private Thread imgproc;
        private bool threadStop = false;
        delegate void SetStatusBarCallBack();

        private int[] threeBythree;
        private int[] itemThree;
        private int[] diffThree;

        private bool m_isWorking = false;

        //---------------------------------------------------------------------------
        //private struct imageStreamParams
        //{
        //    public int duration;
        //    public float aGain;
        //    public float dGain;
        //    public float gamma;
        //    public float offset;

        //};

        public SVS_Vistek_Grabber()
        {
            md.LoadModel("c:\\tmp\\frozen_inference_graph.pb", "c:\\tmp\\object-detection.pbtxt");
        }


        private struct imagebufferStruct
        {
            public byte[] imagebytes;

        };
        private imagebufferStruct[] imagebuffer;
        private imagebufferStruct[] imagebufferRGB;

        private imagebufferStruct[] outputbuffer;
        private imagebufferStruct[] outputbufferRGB;


        public GigeApi.SVSGigeApiReturn setGain(float aGain, float dGain, float gamma, float offset) //0-18db
        {
            //set the gain
            return myApi.Gige_Camera_setGain(hCamera, aGain);
            // myApi.Gige_Camera_setGammaCorrectionExt(hCamera, gamma,dGain,offset);

        }

        public void checkCamera()
        {
            hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, 0);
            errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);
            if (errorflag!= GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_ALREADY_CONNECTED)
            {
                reconnect();
            }

        }
        public void OpenCamera()
        {
            //discover first camera and open it
            initApp();
            getCameraAndOpen();
            setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE);

        }
        public void reconnect()
        {
            //Open connection to camera. A control channel will be established.


            errorflag = myApi.Gige_StreamingChannel_delete(hStreamingChannel);

            hStreamingChannel = new IntPtr(-1);

            //if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            //{
            //    //    //MessageBox.Show("Errorflag: " + apiReturn.ToString());
            //}

            // Close camera
            //apiReturn = myApi.Gige_Camera_closeConnection(hCamera);

            //if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            //{
            //    //    //MessageBox.Show("Errorflag: " + apiReturn.ToString());
            //}

            hCamera = new IntPtr(-1);

            cameraConnected = false;
            // if cameraConnected

            hCamera = new IntPtr(GigeApi.SVGigE_NO_CAMERA);
            //if (hCameraContainer != GigeApi.SVGigE_NO_CLIENT)
            //{
            //    myApi.Gige_CameraContainer_delete(hCameraContainer);
            //}
            //hCameraContainer = myApi.Gige_CameraContainer_create(GigeApi.SVGigETL_Type.SVGigETL_TypeFilter);
            hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, 0);//assume camera zero
            errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);

            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS && errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_ALREADY_CONNECTED)
            {
                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_CAMERA_OCCUPIED)
                {

                    //{
                    //    // Try assigning a valid network address
                    //    errorflag = myApi.Gige_Camera_forceValidNetworkSettings(hCamera, ref IPAddress, ref SubnetMask);
                    //}

                    //if (errorflag == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    //{
                    // Try opening the camera again
                    errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);
                    //}

                }

                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    //   //MessageBox.Show("Errorflag: " + errorflag.ToString());
                    cameraSelection = -99;
                    cameraConnected = false;
                    //     this.GigeComboBox.Enabled = true;
                    //     this.panelInfo.Cursor = Cursors.Default;
                    //    enableControls(false);
                    //    Redraw(false, true);
                    Console.WriteLine("SVS Vistek: Errorflag while trying to reopen camera: " + errorflag.ToString());
                    return;
                }
            }
            else
            {
                Thread.Sleep((int)10000);
                errorflag = myApi.Gige_Camera_closeConnection(hCamera);
                //sleep for 5 seconds
                Thread.Sleep((int)10000);
                errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);

                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    //   //MessageBox.Show("Errorflag: " + errorflag.ToString());
                    cameraSelection = -99;
                    cameraConnected = false;
                    //     this.GigeComboBox.Enabled = true;
                    //     this.panelInfo.Cursor = Cursors.Default;
                    //    enableControls(false);
                    //    Redraw(false, true);
                    Console.WriteLine("SVS Vistek: Errorflag while trying to reopen camera (after close connection too): " + errorflag.ToString());
                    GigeCallBack = new GigeApi.StreamCallback(this.myStreamCallback);

                    return;
                }
            }
            //setup call back again
            // Create a StreamCallback
            GigeCallBack = new GigeApi.StreamCallback(this.myStreamCallback);
            //errorflag = myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);

            // Create a matching streaming channel
            errorflag = myApi.Gige_StreamingChannel_create(ref hStreamingChannel, hCameraContainer, hCamera, 10, GigeCallBack, new IntPtr(0));
            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                //   //MessageBox.Show("Errorflag: " + errorflag.ToString());
                cameraSelection = -99;
                cameraConnected = false;
                //    this.GigeComboBox.Enabled = true;
                //     this.panelInfo.Cursor = Cursors.Default;
                //     enableControls(false);
                //     Redraw(false, true);
                Console.WriteLine("SVS Vistek: problem creating channel in reconnect");
                return;
            }

            errorflag = myApi.Gige_Camera_setAcquisitionModeAndStart(hCamera, GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_FIXED_FREQUENCY, true);
            Console.WriteLine("SVS Vistek: camera reconnected");
            cameraConnected = true;

        }

        public IntPtr gethStreamingChannel()
        {
            return hStreamingChannel;



        }
        public Bitmap Image()
        {

            //   var ram = new MemoryStream();

            //   EncoderParameter quality = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
            //   EncoderParameters Params = new EncoderParameters(1);
            //   Params.Param[0] = quality;
            //   ImageCodecInfo[] myCodecs = ImageCodecInfo.GetImageEncoders();
            //   ImageCodecInfo jpegCodec = myCodecs[1];


            //   bimageRGB[0].Save(ram, System.Drawing.Imaging.ImageFormat.Jpeg);//, jpegCodec,  Params);
            //        ram.Seek(0, SeekOrigin.Begin); // reset stream to start so it can be read again

            //return new Bitmap(ram);

            return bimageRGB[0];





        }
        public bool useDarks
        {
            get { return m_useDarks; }
            set { m_useDarks = value; }
        }
        public float darkMultiplier
        {
            get { return _darkmultiplier; }
            set { _darkmultiplier = value; }
        }
        public bool saveFiles
        {
            get { return m_saveLocal; }
            set { m_saveLocal = value; }
        }

        public bool isWorking
        {
            get
            {
                return m_isWorking;
            }
        }
        public bool isStreaming
        {
            get
            {
                return m_isStreaming;
            }
        }

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
        private void freeze()
        {
            if (GigeIsInit == true)
            {

                errorflag = myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    //MessageBox.Show("Errorflag: " + errorflag.ToString());
                }

                grab = false;


            }  //if (GigeIsInit == true)
        }
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

            //this.panel1.Left = 0;
            //this.panel1.Top = 0;
            //this.panel1.Width = 760;
            //this.panel1.Height = 700;

            //this.panel2.Left = 10;
            //this.panel2.Top = 10;
            //this.panel2.Width = 720;
            //this.panel2.Height = 680;

            //menuItemCameraMode.Enabled = false;
            //menuItemCamera.Enabled = false;

            bimage = new Bitmap[bufferCount];
            bimageRGB = new Bitmap[bufferCount];

            threadStop = true;
          //  imgproc = new Thread(new ThreadStart(this.imageProcessing));
          //  imgproc.IsBackground = true;


            //this.menuItemCameraMode.Visible = false;
            //this.menuItemCameraMode.Enabled = false;

            //this.comboBoxBayerConversion.SelectedIndex = 0;
            //this.comboBoxBayerConversion.Enabled = false;
            //this.comboBoxAcquisition.SelectedIndex = 0;
            //this.comboBoxAcquisition.Enabled = false;

            //enableControls(false);
            //this.comboBoxBayerConversion.Enabled = false;

            //   gpanel = panel2.CreateGraphics();

        }
        private void waiting(object x)
        {
            //DateTime startTime = DateTime.Now;

            //Int64 elapsedTicks = DateTime.Now.Ticks - startTime.Ticks;
            //while (elapsedTicks < (int)x)
            //{
            //    elapsedTicks = DateTime.Now.Ticks - startTime.Ticks;
            //}
            Thread.Sleep((int)x);
            m_isWorking = false;
            Console.WriteLine("SVS Vistek: waited too long");

        }
        private void imageProcessing()
        {


            Console.WriteLine("SVS Vistek: starting imageProcessing Thread");
           // while (threadStop == false)
           // {
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
                        // this.SetStatusBar();
                        //   bool clear = false;
                        //   Redraw(bayerPattern, clear);    // display image

                        bufferReadind = (bufferReadind + 1) % bufferCount;

                    }
                    if (fifoCount <= 0)
                    {
                        try
                        {
                            Thread.Sleep(1);
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
           // }
        }
        private void copyToOutput()
        {
            unsafe
            { 
            int size;

            if (!bayerPattern)
            {
                size = camWidth * camHeight * 2;

                // Copy BW8 image
                System.Buffer.BlockCopy(imagebuffer[0].imagebytes, 0, outputbuffer[0].imagebytes, 0, size);
                if (m_saveLocal)
                {
                    string filename = string.Format("{0}-{1:ddMMMyyyy-HHmmss}.png", @"c:\image\image", DateTime.Now);
                    try
                    {
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Png);

                        // Create an Encoder object based on the GUID
                        // for the Quality parameter category.
                        System.Drawing.Imaging.Encoder myEncoder =
                            System.Drawing.Imaging.Encoder.Quality;

                        // Create an EncoderParameters object.
                        // An EncoderParameters object has an array of EncoderParameter
                        // objects. In this case, there is only one
                        // EncoderParameter object in the array.
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);

                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 95L);
                        myEncoderParameters.Param[0] = myEncoderParameter;


                        bimageRGB[0].Save(filename);//, jpgEncoder, myEncoderParameters);
                        Console.WriteLine("SVS Vistek: saving image");
                        // check if it is a meteor
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("SVS Vistek: " + e.Message);
                    }
                }
            }
            else
            {
                // Copy RGB image

                size = camWidth * camHeight * 3;

                //System.Buffer.BlockCopy(imagebufferRGB[0].imagebytes, 0, outputbufferRGB[0].imagebytes, 0, size);
                if (m_saveLocal)
                {
                    string filename = string.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "image", DateTime.Now);
                    try
                    {
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);


                        if (useDarks)
                        {




                            Bitmap dark = new Bitmap("masterDarksvs.png");

                            // Lock the bitmap's bits.  
                            Rectangle rect = new Rectangle(0, 0, dark.Width, dark.Height);
                            System.Drawing.Imaging.BitmapData bmpData =
                                dark.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                dark.PixelFormat);

                            // Get the address of the first line.
                            IntPtr ptr = bmpData.Scan0;

                            // Declare an array to hold the bytes of the bitmap.
                            int bytes = Math.Abs(bmpData.Stride) * dark.Height;
                            byte[] darkRGBValues = new byte[bytes];

                            // Copy the dark RGB values into the array.
                            System.Runtime.InteropServices.Marshal.Copy(ptr, darkRGBValues, 0, bytes);

                            float multiplier;
                            multiplier = _darkmultiplier;
                            System.Drawing.Imaging.BitmapData bmpData2 = bimageRGB[0].LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bimageRGB[0].PixelFormat);
                            int bytes2 = Math.Abs(bmpData2.Stride) * dark.Height;
                            byte[] camRGBValues = new byte[bytes2];
                            IntPtr ptr2 = bmpData2.Scan0;
                            // Copy the camera RGB values into the array.
                            System.Runtime.InteropServices.Marshal.Copy(ptr2, camRGBValues, 0, bytes2);


                            int darkcounter = 0;

                            for (int counter = 0; counter < camRGBValues.Length - 1; counter = counter + 3)
                            {
                                //Console.WriteLine(Math.Max(0, (camRGBValues[counter] - darkRGBValues[darkcounter ])));
                                if (darkRGBValues[darkcounter] < 25) darkRGBValues[darkcounter] = 0;
                                if (darkRGBValues[darkcounter+1] < 25) darkRGBValues[darkcounter+1] = 0;
                                if (darkRGBValues[darkcounter+2] < 25) darkRGBValues[darkcounter+2] = 0;

                                camRGBValues[counter] = (byte)Math.Max(0, (camRGBValues[counter] - darkRGBValues[darkcounter]*multiplier));
                                camRGBValues[counter + 1] = (byte)Math.Max(0, (camRGBValues[counter + 1] - darkRGBValues[darkcounter+1] * multiplier));
                                camRGBValues[counter + 2] = (byte)Math.Max(0, (camRGBValues[counter + 2] - darkRGBValues[darkcounter+2] * multiplier));
                                //  camRGBValues[counter+1 ] =  Math.Max(0,(camRGBValues[counter +1] - darkRGBValues[darkcounter+2]));
                                //  camRGBValues[counter +2] =  Math.Max((byte)0, (byte)(camRGBValues[counter+2 ] - darkRGBValues[darkcounter]));
                                // Console.WriteLine(camRGBValues[counter + offset]);
                                darkcounter = darkcounter + 4;//bits are stored ARGB

                            }


                            // Copy the RGB values back to the bitmap
                            System.Runtime.InteropServices.Marshal.Copy(camRGBValues, 0, ptr2, bytes2);

                            // Unlock the bits.
                            bimageRGB[0].UnlockBits(bmpData2);
                            dark.UnlockBits(bmpData);



                        }





                        /*too slow
                        Color myRgbColor = new Color();
                        if (m_useDarks)
                        {
                            //subtract dark
                            for (int x = 0; x < bimageRGB[0].Width; x++)
                            {
                                for (int y = 0; y < bimageRGB[0].Height; y++)
                                {
                                    //get pixel from bimagergb
                                    Color c = bimageRGB[0].GetPixel(x, y);
                                    Color c2 = dark.GetPixel(x, y);
                                    int r = Math.Max(0,c.R - c2.R);
                                    int g = Math.Max(0, c.G - c2.G);
                                    int b = Math.Max(0, c.B - c2.B);
                                    
                                    myRgbColor = Color.FromArgb(r, g, b);
                                    bimageRGB[0].SetPixel(x, y,myRgbColor);

                                }
                            }


                        }
                        */


                        // Create an Encoder object based on the GUID
                        // for the Quality parameter category.
                        System.Drawing.Imaging.Encoder myEncoder =
                            System.Drawing.Imaging.Encoder.Quality;

                        // Create an EncoderParameters object.
                        // An EncoderParameters object has an array of EncoderParameter
                        // objects. In this case, there is only one
                        // EncoderParameter object in the array.
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);

                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 95L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        //
                        string firstText = string.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now);
                        uint temp;
                        temp = 0;
                        myApi.Gige_Camera_getSensorTemperature(hCamera, ref temp);
                        string secondText = string.Format("Sensor temp:{0}C", temp);

                        PointF firstLocation = new PointF(10f, 10f);
                        PointF secondLocation = new PointF(10f, 50f);
                            //  fixed (byte* ColorPtr = outputbufferRGB[0].imagebytes) {

                            Bitmap b = new Bitmap(camWidth, camHeight, 3*camWidth,
          PixelFormat.Format24bppRgb,
          System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(outputbufferRGB[0].imagebytes, 0));

                            using (Graphics graphics = Graphics.FromImage(b))
                        {
                            using (Font arialFont = new Font("Arial", 20))
                            {
                                graphics.DrawString(firstText, arialFont, Brushes.GreenYellow, firstLocation);
                                graphics.DrawString(secondText, arialFont, Brushes.GreenYellow, secondLocation);
                            }
                        }


                        string folderName = string.Format("{0:yyyy-MMM-dd}", DateTime.Now);
                        System.IO.Directory.CreateDirectory(Path.Combine("c:\\image",folderName));
                     
                        bimageRGB[0].Save(Path.Combine("c:\\image",folderName, filename), jpgEncoder, myEncoderParameters);
                        md.examine(Path.Combine("c:\\image", folderName, filename));
                        Console.WriteLine("SVS Vistek: saving image");
                        readimageCount++;
                        fifoCount--;
                            
                        }
                    catch (Exception e)
                    {
                        Console.WriteLine("SVS Vistek: " + e.Message);
                    }
                }
            }
            }

        }
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

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

                    // GigeComboBox.Items.Add("Select Camera!");

                    if (camAnz > 0)
                    {
                        for (n = 0; n < camAnz; n++)
                        {
                            hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, n);
                            modelname = myApi.Gige_Camera_getModelName(hCamera);
                            SN = myApi.Gige_Camera_getSerialNumber(hCamera);
                            //GigeComboBox.Items.Add(modelname + " SN: " + SN);
                        }
                    }

                    // GigeComboBox.Focus();
                }
                else
                {
                    //MessageBox.Show("Error Containerdiscovery:");
                    return 0;
                }
            }
            else
            {
                //MessageBox.Show("Error ContainerCreate:");
                return 0;
            }

            return camAnz;
        }

        private void getCameraAndOpen()
        {
            // this.GigeComboBox.Enabled = false;
            // this.panelInfo.Cursor = Cursors.WaitCursor;
            int n;
            n = ChooseMyCamera();

            if (n < 1)
            {
                Console.WriteLine("SVS Vistek: NO CAMERA FOUND!");
                return;
            }


            Thread.Sleep(100);



            selectedIndex = 0; //only one camera



            GigeApi.SVSGigeApiReturn apiReturn;
            openChannel = true;

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
                        //   //MessageBox.Show("Errorflag: " + apiReturn.ToString());
                        //   enableControls(false);
                        //   Redraw(false, true);
                    }

                    // Remove streaming channel
                    apiReturn = myApi.Gige_StreamingChannel_delete(hStreamingChannel);

                    hStreamingChannel = new IntPtr(-1);

                    if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    {
                        //    //MessageBox.Show("Errorflag: " + apiReturn.ToString());
                    }

                    // Close camera
                    apiReturn = myApi.Gige_Camera_closeConnection(hCamera);

                    if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                    {
                        //    //MessageBox.Show("Errorflag: " + apiReturn.ToString());
                    }

                    hCamera = new IntPtr(-1);

                    cameraConnected = false;
                }  // if cameraConnected

                hCamera = new IntPtr(GigeApi.SVGigE_NO_CAMERA);

                hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, n);

                myApi.Gige_Camera_setInterPacketDelay(hCamera, 50000);
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
                        //   //MessageBox.Show("Errorflag: " + errorflag.ToString());
                        cameraSelection = -99;
                        cameraConnected = false;
                        //     this.GigeComboBox.Enabled = true;
                        //     this.panelInfo.Cursor = Cursors.Default;
                        //    enableControls(false);
                        //    Redraw(false, true);

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
                    //   //MessageBox.Show("Errorflag: " + errorflag.ToString());
                    cameraSelection = -99;
                    cameraConnected = false;
                    //    this.GigeComboBox.Enabled = true;
                    //     this.panelInfo.Cursor = Cursors.Default;
                    //     enableControls(false);
                    //     Redraw(false, true);

                    return;
                }
                // myApi.Gige_Camera_setLookupTableMode(hCamera, GigeApi.LUT_MODE.LUT_MODE_DISABLED);

                cameraConnected = true;

                camWidth = 0;

                errorflag = myApi.Gige_Camera_getSizeX(hCamera, ref(camWidth));

                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    //      //MessageBox.Show("Errorflag: " + errorflag.ToString());
                }
                camHeight = 0;
                errorflag = myApi.Gige_Camera_getSizeY(hCamera, ref(camHeight));
                //set bit depth
                //  myApi.Gige_Camera_setPixelDepth(hCamera, GigeApi.SVGIGE_PIXEL_DEPTH.SVGIGE_PIXEL_DEPTH_12);
                this.setGigeHandles(myApi, hCamera, hCameraContainer, camWidth, camHeight, modelname, gigeBitcount);

                // allocate memory for buffers
                this.initializeBuffer();

                // set camera-mode to freerunning
                //startGrabbing();

            } // if openChannel

            // this.GigeComboBox.Enabled = true;

            //  enableControls(true);

            //  this.panelInfo.Cursor = Cursors.Default;

        }
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

            //// set Camera parameters
            //this.labelWidth2.Text = camWidth.ToString();
            //this.labelHeight2.Text = camHeight.ToString();

            string strReturn;
            GigeApi.SVSGigeApiReturn apiReturn;

            strReturn = myApi.Gige_Camera_getModelName(hCamera);
            //this.labelModelName2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getManufacturerName(hCamera);
            //  this.labelManufacturer2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getDeviceVersion(hCamera);
            // this.labelDeviceVersion2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getSerialNumber(hCamera);
            // this.labelSerial2.Text = strReturn;

            strReturn = myApi.Gige_Camera_getUserDefinedName(hCamera);
            // this.labelUserDefinedName2.Text = strReturn;

            {
                //--------------------------------------------------------------------------------
                // get pixel depth

                PixelDepth = GigeApi.SVGIGE_PIXEL_DEPTH.SVGIGE_PIXEL_DEPTH_8;

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

                // this.labelPixelDepth2.Text = pixelString;

                //--------------------------------------------------------------------------------
                // get pixel type

                bayerPattern = false;
                PixelType = GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_UNKNOWN;
                //how do you set pixel type?

                //apiReturn = myApi.Gige_Camera_setPixelType(hCamera, GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_BAYGR12);

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
                            bayerPattern = true;
                            break;
                    }


                    //if (bayerPattern)
                    //{
                    //    this.labelColor2.Text = "with Bayer Pattern (color)";
                    //    comboBoxBayerConversion.Enabled = true;
                    //}
                    //else
                    //{
                    //    this.labelColor2.Text = "Monochrome (bw) ";
                    //    comboBoxBayerConversion.Enabled = false;
                    //}
                }
            }

            //--------------------------------------------------------------------------
            // adjust Panel-Size

            //this.panel1.Width = 760;
            //this.panel1.Height = 700;
            //this.panel2.Width = 720;
            //this.panel2.Height = 680;

            //if ((camWidth > 1) && (camHeight > 1))
            //{

            //    float zoomW = camWidth / panel2.Width;
            //    float zoomH = camHeight / panel2.Height;

            //    float ratio = 0.0f;
            //    ratio = (float)((float)camWidth / (float)camHeight);

            //    if (zoomW < zoomH)
            //    {

            //        float height = (float)this.panel2.Height;
            //        this.panel2.Width = (int)(height * ratio);

            //    }
            //    else
            //    {
            //        ratio = (float)1.0f / ratio;

            //        float width = (float)this.panel2.Width;
            //        this.panel2.Height = (int)(width * ratio);
            //    }
            //}
            //else
            //{

            //    this.panel1.Width = 760;
            //    this.panel1.Height = 700;
            //    this.panel2.Width = 720;
            //    this.panel2.Height = 680;
            //}

            //outRectangle = new Rectangle(0, 0, panel2.Width, panel2.Height);

            //menuItemCameraMode.Enabled = true;
            GigeIsInit = true;

        }

        //---------------------------------------------------------------------------

        public IntPtr getCamHandle()
        {
            return hCamera;
        }

        //[return: MarshalAs(UnmanagedType.Error)]
        public GigeApi.SVSGigeApiReturn myStreamCallback(int Image, IntPtr Context)
        {
            // errorflag = myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
            Console.WriteLine("SVS Vistek: image callback received {0}", DateTime.Now);
            GigeApi.SVSGigeApiReturn apiReturn;
            int xSize, ySize, size;
            int answ;
            IntPtr imgPtr;

            if (running == 1)
            {
                Console.WriteLine("SVS Vistek: callback bailout {0}", DateTime.Now);
                return GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS;
            }
            if (GigeIsInit == false)
            {
                return (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS);
            }
            running = 1;

            imgPtr = myApi.Gige_Image_getDataPointer(Image);
            //GigeApi.SVSGigeApiReturn r = myApi.Gige_StreamingChannel_getBufferData(hStreamingChannel, 2000, 0, ref globaldata);
            answ = myApi.Gige_Image_getImageSize(Image);
            if ((imgPtr.ToInt64()) == 0)
            {
                Console.WriteLine("SVS Vistek: callback lost image {0}", DateTime.Now);
                running = 0;
                lostCount++;
                return (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS);
            }

            xSize = myApi.Gige_Image_getSizeX(Image);
            ySize = myApi.Gige_Image_getSizeY(Image);
            size = xSize * ySize*12/8;    // = Image-Size = Gesamt-Anz. der Pixel
            int bufferWriteind_current = bufferWriteind;
            IntPtr ip;
            byte[] temp;
            byte[] tempunpacked;
            byte bitshift;
            temp =  new byte[size];
            tempunpacked = new byte[xSize * ySize * 3];
            int byteCounter = 0;
            lock (bimage[0])
            {
                lock (bimageRGB[0])
                {
                   


                        if (bitCount == 12)
                        {
                            unsafe
                            {
                                    //Emgu.CV.CvInvoke.CvtColor(SRC, DEST, COLOR_CONV, CHANN)


                                    size = camWidth * camHeight * 3;

                            //byte* dest = outputbufferRGB[0].imagebytes[0];
                                    
                                // convert to RGB-Image
                                //apiReturn = myApi.Gige_Image_getImage12bitAs8bit(Image, dest, size);
                            //    SVCamApi.SVcamApi._SV_BUFFER_INFO ImageInfo;
                            //    ImageInfo = new SVCamApi.SVcamApi._SV_BUFFER_INFO();
                            //    ImageInfo.pImagePtr = imgPtr;
                            //    ImageInfo.iPixelType =17563691;
                            //SVCamApi.SVcamApi._SV_BUFFER_FLAG flags;
                            //flags = new SVCamApi.SVcamApi._SV_BUFFER_FLAG();
                            //flags.acquiring = 0;
                            //flags.incomplete = 0;
                            //flags.queued = 0;
                            //flags.newData = 0;
                            //flags.value = 0;

                            //ImageInfo.iImageId = 1;
                            //ImageInfo.flags = flags;
                            //    var handle = System.Runtime.InteropServices.GCHandle.Alloc(myApi.Gige_Image_getSizeX(Image), System.Runtime.InteropServices.GCHandleType.Pinned);
                            //    var ptr = handle.AddrOfPinnedObject();
                            //    var handle2 = System.Runtime.InteropServices.GCHandle.Alloc(myApi.Gige_Image_getSizeY(Image), System.Runtime.InteropServices.GCHandleType.Pinned);
                            //    var ptr2 = handle2.AddrOfPinnedObject();
                            //    var handle3 = System.Runtime.InteropServices.GCHandle.Alloc(myApi.Gige_Image_getImageSize(Image), System.Runtime.InteropServices.GCHandleType.Pinned);
                            //    var ptr3 = handle3.AddrOfPinnedObject();
                            //    ImageInfo.iSizeX = ptr;
                            //    ImageInfo.iSizeY = ptr2;
                            //    ImageInfo.iImageSize = ptr3;
                            //    SVCamApi.SVcamApi svc;

                            //    svc = new SVCamApi.SVcamApi();
                            //            svc.SVS_UtilBufferBayerToRGB(ImageInfo, ref outputbufferRGB[0].imagebytes[0], size);
                                    }
                                    fifoCount++;
                                    copyToOutput();

                                    //if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                                    //{
                                    //    return (apiReturn);
                                    //}
                                
                           
                        }  // if                                                                                                                 

                            else if (bitCount == 8)
                            {
                                unsafe
                                {
                                    size = camWidth * camHeight * 3;

                                    //fixed (byte* dest = imagebufferRGB[0].imagebytes)
                                        //apiReturn = myApi.Gige_Image_getImageRGB
                                        // convert to 8-bit Image
                                        //  apiReturn = myApi.Gige_Image_getImage12bitAs16bit(Image, dest, size);
                                        // apiReturn = myApi.Gige_Image_getImage12bitAs8bit(Image, dest, size);
                                       // apiReturn = myApi.Gige_Image_getImageRGB(Image, dest, size, GigeApi.BAYER_METHOD.BAYER_METHOD_HQLINEAR);
                                       
                                     
                            System.Runtime.InteropServices.Marshal.Copy(imgPtr, temp, 0, answ);
                            //parse the image into color array
                            int stride = camWidth * 3;
                            for (int y = 0; y < camHeight; y++)
                            {
                                for (int x = 0; x < camWidth * 3; x = x + 6)
                                {
                                    //first 4 bits
                                    bitshift = 4;
                                    byte red = (byte)(temp[byteCounter]>>bitshift);
                                    byte green = (byte)(temp[byteCounter]  & (byte) 0xf);
                                    byte blue = (byte)(temp[byteCounter + 1] >> bitshift);

                                    outputbufferRGB[0].imagebytes[x + y * stride] = (byte)(red<<bitshift);
                                    outputbufferRGB[0].imagebytes[x + 1 + y * stride] = (byte)(green<<bitshift);
                                    outputbufferRGB[0].imagebytes[x + 2 + y * stride] = (byte)(blue << bitshift);
                            // 
                                     red = (byte)(temp[byteCounter+1] & (byte)0xf);
                                     green = (byte)(temp[byteCounter+2] >> bitshift);
                                     blue = (byte)(temp[byteCounter + 2] & (byte)0xf );

                                    outputbufferRGB[0].imagebytes[x + 3 + y * stride] = (byte)(red<<bitshift);
                                    outputbufferRGB[0].imagebytes[x + 4 + y * stride] = (byte)(green << bitshift);
                                    outputbufferRGB[0].imagebytes[x + 5 + y * stride] = (byte)(blue << bitshift);
                                    byteCounter = byteCounter + 3;
                                }

                            }

                                    fifoCount++;
                            fixed (byte* dest = imagebufferRGB[0].imagebytes)
                            {
                                // convert to RGB-Image
                                apiReturn = myApi.Gige_Image_getImageRGB( Image, dest, size, GigeApi.BAYER_METHOD.BAYER_METHOD_NEAREST);
                              
                            }
                            
                            copyToOutput();
           
                                }
                            }
                            else
                            {
                                //    //MessageBox.Show(" Bits: " + bitCount + "bits", "MyApplication ", MessageBoxButtons.OKCancel);
                            }

                        bufferWriteind = (bufferWriteind + 1) % bufferCount;
                    }
                
            }
            if (notFirstImage == false)
            {
                notFirstImage = true;

                if (threadStop == true)
                {
                    threadStop = false;

                    //imgproc.Start();
                }
            }
            else
            {
                // imgproc.Interrupt();
            }

            imageCount++;
            //string fn = @"C:\temp\test.bmp";
            //GigeApi.SVSGigeApiReturn r1 = myApi.GigE_writeImageToBitmapFile(fn, ref imgPtr, xSize, ySize, GigeApi.GVSP_PIXEL_TYPE.GVSP_PIX_MONO8);

            running = 0;
            m_isWorking = false;


            return (GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS);
        }
        private void initializeBuffer()
        {
            int k;

            bimage = new Bitmap[bufferCount];
            bimageRGB = new Bitmap[bufferCount];

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
        private void setAcquisitionMode(GigeApi.ACQUISITION_MODE newAcqmode)
        {

            if (GigeIsInit == true)
            {

                errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, newAcqmode);

                if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                {
                    // //MessageBox.Show("Errorflag: " + errorflag.ToString());
                }


            }  //if (GigeIsInit == true)

        }

        public Bitmap getLastImage()
        {

            // wait for an image to be ready
            // no longer than 20 seconds
            Stopwatch s;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (running==1 && stopWatch.ElapsedMilliseconds<20000)
            { }
            stopWatch.Stop();
            return bimageRGB[0];

        }

        public void setParams(int duration, float aGain, float dGain, float gamma, float offset)
        {

            m_duration = duration;
            m_aGain = aGain;
            m_dGain = dGain;
            m_gamma = gamma;
            m_offset = offset;
            errorflag = this.setGain(m_aGain, m_dGain, m_gamma, m_offset);
            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                Console.WriteLine("setGain:Errorflag: " + errorflag.ToString());
            }
            errorflag = myApi.Gige_Camera_setExposureTime(hCamera, (float)(duration));
            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                Console.WriteLine("Gige_Camera_setExposureTime:Errorflag: " + errorflag.ToString());
            }
            errorflag = myApi.Gige_Camera_setFrameRate(hCamera, Math.Min((float)999220 / duration, (float)2));
            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                Console.WriteLine("Gige_Camera_setFrameRate:Errorflag: " + errorflag.ToString());
            }
            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
            {
                Console.WriteLine("attempting to reconnect");
                reconnect();
            }
        }
        public void startStreamingFF()
        {
            this.setGain(m_aGain, m_dGain, m_gamma, m_offset);
            myApi.Gige_Camera_setAcquisitionModeAndStart(hCamera, GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_FIXED_FREQUENCY, true);

        }
        public void stopStreamingFF()
        {

            myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);

        }
        public void startStreaming()
        {

            m_isStreaming = true;
            Thread imgGetter = new Thread(imageStreaming);
            imgGetter.Start();
            imgGetter.IsBackground = true;

        }
        public void stopStreaming()
        {

            m_isStreaming = false;
        }
        public void imageStreaming()
        {


            //  myApi.Gige_Camera_setLookupTableMode(hCamera, GigeApi.LUT_MODE.LUT_MODE_DISABLED);


            while (m_isStreaming)
            {
                this.setGain(m_aGain, m_dGain, m_gamma, m_offset);
                this.startCapture();
                //DateTime startTime = DateTime.Now;

                //Int64 elapsedTicks = DateTime.Now.Ticks - startTime.Ticks;
                //while (elapsedTicks < m_duration)
                //{
                //    elapsedTicks = DateTime.Now.Ticks - startTime.Ticks;
                //}
                Thread.Sleep((int)m_duration);
                // Console.WriteLine(elapsedTicks);
                this.stopCapture();


                Thread waitproc = new Thread(waiting);
                waitproc.Start(m_duration + 30000);//timeout after duration + 30 second
                waitproc.IsBackground = true;

                while (this.isWorking)
                {
                    Thread.Sleep(10);
                }
                waitproc.Abort();
            }




        }


        public void startCapture()
        {
            setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE);
            //myApi.Gige_Camera_setIOAssignment(hCamera, GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT6, GigeApi.SVGigE_IOMux_IN_FIXED_HIGH);
            myApi.Gige_Camera_setIOAssignment(hCamera, GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT6, GigeApi.SVGigE_IOMux_IN_FIXED_LOW);
            //myApi.Gige_Camera_setIOAssignment(hCamera,)
            //  GigeApi.Camera_setIOAssignment(hCamera, SVGigE_IOMux_OUT_TRIGGER, SVGigE_IOMux_IN_FIXED_LOW);
            // GigeApi.Camera_setIOAssignment(hCamera, SVGigE_IOMux_OUT_TRIGGER, GigeApi.SVGigE_IOMux_IN.SVGigE_IOMux_IN_FIXED_High);
            //setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER);
            //setAcquisitionMode(GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE);
            // Camera_setIOAssignment(Camera, SVGigE_IOMux_OUT_TRIGGER, SVGigE_IOMux_IN_FIXED_LOW);
            // Camera_setIOAssignment(Camera, SVGigE_IOMux_OUT_TRIGGER, SVGigE_IOMux_IN_FIXED_High);
            //         // Fixed High signal 
            //public const SVGigE_IOMux_IN SVGigE_IOMux_IN_FIXED_HIGH = SVGigE_IOMux_IN.SVGigE_IOMUX_IN31;
            //// Fixed Low signal 
            //public const SVGigE_IOMux_IN SVGigE_IOMux_IN_FIXED_LOW = SVGigE_IOMux_IN.SVGigE_IOMUX_IN30;


        }
        public void close()
        {

            this.close();



        }
        public int getSizeX()
        {
            errorflag = myApi.Gige_Camera_getSizeX(hCamera, ref(camWidth));
            return this.camWidth;

        }
        public int getSizeY()
        {
            errorflag = myApi.Gige_Camera_getSizeY(hCamera, ref(camHeight));
            return this.camHeight;

        }
        public Bitmap getImage()
        {


            this.startCapture();
            //DateTime startTime = DateTime.Now;



            //Int64 elapsedTicks = DateTime.Now.Ticks - startTime.Ticks; 
            //while (elapsedTicks < m_duration)
            //{
            //    elapsedTicks = DateTime.Now.Ticks - startTime.Ticks;
            //}
            Thread.Sleep(m_duration / 1000);
            this.stopCapture();
            Thread waitproc = new Thread(waiting);
            waitproc.Start(m_duration / 1000 + 10000);//timeout after 10 seconds
            waitproc.IsBackground = true;

            while (this.isWorking)
            {
                Thread.Sleep(10);
            }
            waitproc.Abort();

            Bitmap b = this.Image();

            return b;
        }
        public Bitmap getImage(int duration, float aGain, float dGain, float gamma, float offset)
        {

            // myApi.Gige_Camera_setLookupTableMode(hCamera, GigeApi.LUT_MODE.LUT_MODE_DISABLED);

            this.setGain(aGain, dGain, gamma, offset);
            this.startCapture();
            //DateTime startTime = DateTime.Now;



            //Int64 elapsedTicks = DateTime.Now.Ticks - startTime.Ticks; 
            //while (elapsedTicks < m_duration)
            //{
            //    elapsedTicks = DateTime.Now.Ticks - startTime.Ticks;
            //}
            Thread.Sleep(m_duration);
            this.stopCapture();
            Thread waitproc = new Thread(waiting);
            waitproc.Start(duration + 10000);//timeout after 10 seconds
            waitproc.IsBackground = true;

            while (this.isWorking)
            {
                Thread.Sleep(10);
            }
            waitproc.Abort();

            Bitmap b = this.Image();

            return b;
        }

        //    GigeApi.SVSGigeApiReturn apiReturn;

        //    if (openChannel)  // will be set to 'true' after a Discover
        //    {

        //        if (cameraConnected)
        //        {
        //            apiReturn = myApi.Gige_Camera_setAcquisitionControl(hCamera, GigeApi.ACQUISITION_CONTROL.ACQUISITION_CONTROL_STOP);
        //            if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //            {
        //                ////MessageBox.Show("Errorflag: " + apiReturn.ToString());
        //                //enableControls(false);
        //                //Redraw(false, true);
        //            }

        //            // Remove streaming channel
        //            apiReturn = myApi.Gige_StreamingChannel_delete(hStreamingChannel);

        //            hStreamingChannel = new IntPtr(-1);

        //            if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //            {
        //               // //MessageBox.Show("Errorflag: " + apiReturn.ToString());
        //            }

        //            // Close camera
        //            apiReturn = myApi.Gige_Camera_closeConnection(hCamera);

        //            if (apiReturn != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //            {
        //               // //MessageBox.Show("Errorflag: " + apiReturn.ToString());
        //            }

        //            hCamera = new IntPtr(-1);

        //            cameraConnected = false;
        //        }  // if cameraConnected

        //        hCamera = new IntPtr(GigeApi.SVGigE_NO_CAMERA);

        //        hCamera = myApi.Gige_CameraContainer_getCamera(hCameraContainer, 0);

        //        modelname = myApi.Gige_Camera_getModelName(hCamera);

        //        uint IPAddress = 0;
        //        uint SubnetMask = 0;

        //        //Open connection to camera. A control channel will be established.
        //        errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);

        //        if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //        {
        //            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SVCAM_STATUS_CAMERA_OCCUPIED)
        //            {

        //                {
        //                    // Try assigning a valid network address
        //                    errorflag = myApi.Gige_Camera_forceValidNetworkSettings(hCamera, ref IPAddress, ref SubnetMask);
        //                }

        //                if (errorflag == GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //                {
        //                    // Try opening the camera again
        //                    errorflag = myApi.Gige_Camera_openConnection(hCamera, timeout);
        //                }

        //            }

        //            if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //            {
        //                ////MessageBox.Show("Errorflag: " + errorflag.ToString());
        //                //cameraSelection = -99;
        //                cameraConnected = false;
        //                //this.GigeComboBox.Enabled = true;
        //                //this.panelInfo.Cursor = Cursors.Default;
        //                //enableControls(false);
        //                //Redraw(false, true);

        //                return;
        //            }
        //        }

        //        // Create a StreamCallback
        //        GigeCallBack = new GigeApi.StreamCallback(this.myStreamCallback);

        //        // Adjust camera to maximal possible packet size
        //        myApi.Gige_Camera_evaluateMaximalPacketSize(hCamera, ref(PacketSize));

        //        // Adjust desired binning mode (default = OFF, others: 2x2, 3x3, 4x4)
        //        myApi.Gige_Camera_setBinningMode(hCamera, GigeApi.BINNING_MODE.BINNING_MODE_OFF);
        //        // Create a matching streaming channel
        //        errorflag = myApi.Gige_StreamingChannel_create(ref hStreamingChannel, hCameraContainer, hCamera, 10, GigeCallBack, new IntPtr(0));
        //        if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //        {
        //            ////MessageBox.Show("Errorflag: " + errorflag.ToString());
        //            //cameraSelection = -99;
        //            cameraConnected = false;
        //            //this.GigeComboBox.Enabled = true;
        //            //this.panelInfo.Cursor = Cursors.Default;
        //            //enableControls(false);
        //            //Redraw(false, true);

        //            return;
        //        }

        //        cameraConnected = true;

        //        camWidth = 0;

        //        errorflag = myApi.Gige_Camera_getSizeX(hCamera, ref(camWidth));

        //        if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
        //        {
        //           // //MessageBox.Show("Errorflag: " + errorflag.ToString());
        //        }
        //        camHeight = 0;
        //        errorflag = myApi.Gige_Camera_getSizeY(hCamera, ref(camHeight));

        //        this.setGigeHandles(myApi, hCamera, hCameraContainer, camWidth, camHeight, modelname, gigeBitcount);

        //        // allocate memory for buffers
        //        this.initializeBuffer();

        //        // set camera-mode to freerunning
        //        startGrabbing();

        //    } // if openChannel



        //}
        public void stopCapture()
        {
            m_isWorking = true;
            // myApi.Gige_Camera_setIOAssignment(hCamera, GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT6, GigeApi.SVGigE_IOMux_IN_FIXED_HIGH);
            myApi.Gige_Camera_setIOAssignment(hCamera, GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT6, GigeApi.SVGigE_IOMux_IN_FIXED_HIGH);
            // myApi.Gige_Camera_setIOAssignment(hCamera, GigeApi.SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT6, GigeApi.SVGigE_IOMux_IN_FIXED_HIGH);
            // errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER);
            //errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, 0);

        }
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


                        errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_SOFTWARE_TRIGGER);

                        errorflag = myApi.Gige_Camera_setAcquisitionMode(hCamera, GigeApi.ACQUISITION_MODE.ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE);

                        if (errorflag != GigeApi.SVSGigeApiReturn.SVGigE_SUCCESS)
                        {
                            ////MessageBox.Show("Errorflag: " + errorflag.ToString());

                        }

                    }

                    //  this.comboBoxAcquisition.SelectedIndex = 1;
                    this.imageCount = 0;
                    this.lostCount = 0;
                    //timer1.Enabled = true;
                    //menuItemCamera.Enabled = true;
                    grab = true;

                }  // if GigeIsInit

                initgrab = false;

            }  //if initgrab

        }
    }
}
