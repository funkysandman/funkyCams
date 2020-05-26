/*

This sample illustrates how to use the PylonDeviceGrabSingleFrame() convenience
method for grabbing images in a loop. PylonDeviceGrabSingleFrame() grabs one
single frame in single frame mode.

Grabbing in single frame mode is the easiest way to grab images. Note: in single frame
mode the maximum frame rate of the camera can't be achieved. The full frame
rate can be achieved by setting the camera to the continuous frame
mode and by grabbing in overlapped mode, i.e., image acquisition is done in parallel
with image processing. This is illustrated in the OverlappedGrab sample program.

*/

using System;
using System.Collections.Generic;
using PylonC.NET;
using PylonC.NETSupportLibrary;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
namespace BaslerWrapper
{

    public delegate void FrameReceivedHandler(object sender, FrameEventArgs args);
    public class FrameEventArgs : EventArgs
    {
        /// <summary>
        /// The Image (data)
        /// </summary>
        private ImageProvider.Image m_Image = null;
        private  Bitmap m_bitmap = null;

        /// <summary>
        /// The Exception (data)
        /// </summary>
        private Exception m_Exception = null;

        /// <summary>
        /// Initializes a new instance of the FrameEventArgs class. 
        /// </summary>
        /// <param name="image">The Image to transfer</param>
        public FrameEventArgs(ImageProvider.Image img)
        {
            if (null == img)
            {
                throw new ArgumentNullException("image");
            }

            m_Image = img;
        }
        public FrameEventArgs(Bitmap img)
        {
            if (null == img)
            {
                throw new ArgumentNullException("image");
            }

            m_bitmap = img;
        }
        /// <summary>
        /// Initializes a new instance of the FrameEventArgs class. 
        /// </summary>
        /// <param name="exception">The Exception to transfer</param>
        public FrameEventArgs(Exception exception)
        {
            if (null == exception)
            {
                throw new ArgumentNullException("exception");
            }

            m_Exception = exception;
        }

        /// <summary>
        /// Gets the image 
        /// </summary>
        public ImageProvider.Image image
        {
            get
            {
                return m_Image;
            }
        }

        /// <summary>
        /// Gets the Exception
        /// </summary>
        public Exception Exception
        {
            get
            {
                return m_Exception;
            }
        }
    }
    public class Grabber
    {
        private NODEMAP_HANDLE hNodeMap;
        private NODE_HANDLE m_hNode = new NODE_HANDLE();
        private PylonC.NETSupportLibrary.ImageProvider m_imageProvider = new PylonC.NETSupportLibrary.ImageProvider(); /* Create one image provider. */
        private Bitmap m_bitmap = null; /* The bitmap is used for displaying the image. */
        private Boolean waitingForImage = true;
        protected Thread m_grabThread;
        private FrameReceivedHandler m_frh;
        private long sizeX;
        private long sizeY;
        public void Open()
        {
            //try
            //{
                uint numDevices;    /* Number of available devices. */
                Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "300000" /*ms*/);
                //PYLON_DEVICE_HANDLE hDev = new PYLON_DEVICE_HANDLE();
                /* Before using any pylon methods, the pylon runtime must be initialized. */
                Pylon.Initialize();

                /* Enumerate all camera devices. You must call
                PylonEnumerateDevices() before creating a device. */
                numDevices = Pylon.EnumerateDevices();

                if (0 == numDevices)
                {
                    throw new Exception("No devices found.");
                }
                m_imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
                m_imageProvider.Open(0);
                m_hNode = m_imageProvider.GetNodeFromDevice("Width");
                sizeX = GenApi.IntegerGetValue(m_hNode);
                m_hNode = m_imageProvider.GetNodeFromDevice("Height");
                sizeY = GenApi.IntegerGetValue(m_hNode);
                m_hNode = m_imageProvider.GetNodeFromDevice("PixelFormat");
                GenApi.NodeFromString(m_hNode, "Mono16");
                m_hNode = m_imageProvider.GetNodeFromDevice("TriggerMode");
                GenApi.NodeFromString(m_hNode, "Off");
                m_hNode = m_imageProvider.GetNodeFromDevice("AcquisitionFrameRateEnable");
                GenApi.BooleanSetValue(m_hNode, true);
                m_hNode = m_imageProvider.GetNodeFromDevice("AcquisitionFrameRateAbs");
                GenApi.FloatSetValue(m_hNode, 0.2f);


                //m_hNode = m_imageProvider.GetNodeFromDevice("TriggerSource");
                //GenApi.NodeFromString(m_hNode, "Software");
                //m_hNode = m_imageProvider.GetNodeFromDevice("ExposureMode");
                //GenApi.NodeFromString(m_hNode, "TriggerWidth");

            }
            //catch (Exception e)
            //{
            //    // ShowException(e, m_imageProvider.GetLastErrorMessage());
            //}
               

       
        public void close()
        {
            m_imageProvider.Close();
            Pylon.Terminate();
        }

        public void setParams(int duration, long aGain)
        {



            setGain(aGain);
            setExposure(duration*1000); 


           

        }


        public void startAcquisition(FrameReceivedHandler frameEventHandler)
        {
            m_frh = frameEventHandler;
            m_imageProvider.ContinuousShot();


        }
        public void stopAcquisition()
        {

            m_imageProvider.Stop();
        }
        public void setGain(Int64 gain)
        {
            m_hNode = m_imageProvider.GetNodeFromDevice("GainRaw");
            GenApi.IntegerSetValue(m_hNode, gain);

        }

        public void setExposure(Int64 timeValue)



        {
            m_hNode = m_imageProvider.GetNodeFromDevice("ExposureTimeBaseAbs");
            GenApi.FloatSetValue(m_hNode, 2000f);
            m_hNode = m_imageProvider.GetNodeFromDevice("ExposureTimeAbs");
            GenApi.FloatSetValue(m_hNode, timeValue);

        }
        //public Bitmap getImage(string timeValue, Int64 gain)
        //{

            //try
            //{

            //    m_hNode = m_imageProvider.GetNodeFromDevice("GainRaw");
            //    GenApi.IntegerSetValue(m_hNode, gain);
            //   // double v;
            //   // m_hNode = m_imageProvider.GetNodeFromDevice("ExposureTimeBaseAbsEnable");
            //   //GenApi.BooleanSetValue(m_hNode, true);



            //  //  GenApi.NodeFromString(m_hNode, "true");
            //    m_imageProvider.OneShot();
            //    System.Threading.Thread.Sleep(50);
            //    Debug.WriteLine("taking picture...");

            //    m_hNode = m_imageProvider.GetNodeFromDevice("TriggerActivation");
            //    GenApi.NodeFromString(m_hNode, "FallingEdge");
            //    var stopwatch = new Stopwatch();
            //    long frequency = Stopwatch.Frequency;
            //    long numValue;
            //    //determine ticks
            //    if (timeValue.Contains("us"))
            //    {
            //        numValue = Convert.ToInt32(timeValue.Replace("us", ""));
            //    }
            //    else
            //        if (timeValue.Contains("ms"))
            //        {

            //            numValue = Convert.ToInt32(timeValue.Replace("ms", ""))*frequency/1000L;
            //        }
            //        else
            //        {
            //            numValue = Convert.ToInt32(timeValue.Replace("s","")) * frequency ;
            //        }
            //    stopwatch.Start();
            //    while (stopwatch.ElapsedTicks < numValue)
            //    {


            //    }

            //    m_hNode = m_imageProvider.GetNodeFromDevice("TriggerActivation");
            //    GenApi.NodeFromString(m_hNode, "RisingEdge");
            //    waitingForImage = true;
            //    stopwatch.Stop();


            //    stopwatch.Start();
            //    while (waitingForImage && stopwatch.ElapsedMilliseconds<1000)
            //    {

            //        Debug.WriteLine("sleeping...");
            //        System.Threading.Thread.Sleep(100);
            //    }
            //    stopwatch.Stop();
            //    return m_bitmap;
            //    }
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //    return null;
            //}
            //}
        //}
        public long getSizeX
        {
              
               
          get { return sizeX;}


        }
        public long getSizeY
        {

        
             get { return sizeY;}

           
           
        }
        private void imageWork()
        {

         
        }

       private void OnImageReadyEventCallback()
        {
            //if (InvokeRequired)
            //{
            //    /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
            //    BeginInvoke(new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback));
            //    return;
            //}
            Debug.WriteLine("picture ready");
            try
            {
                /* Acquire the image from the image provider. Only show the latest image. The camera may acquire images faster than images can be displayed*/
                ImageProvider.Image image = m_imageProvider.GetLatestImage();

                /* Check if the image has been removed in the meantime. */
                if (image != null)
                {
                    /* Check if the image is compatible with the currently used bitmap. */
                    //if (BitmapFactory.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                    //{
                    //    /* Update the bitmap with the image data. */
                    //    BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                    //    /* To show the new image, request the display control to update itself. */
                    //    //  pictureBox.Refresh();
                    //}
                    //else /* A new bitmap is required. */
                    //{
                    //    BitmapFactory.CreateBitmap(out m_bitmap, image.Width, image.Height,image.Color);
                    //    BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                    //    /* We have to dispose the bitmap after assigning the new one to the display control. */
                    //    //  Bitmap bitmap = pictureBox.Image as Bitmap;
                    //    //  /* Provide the display control with the new bitmap. This action automatically updates the display. */
                    //    ////  pictureBox.Image = m_bitmap;
                    //    //  if (bitmap != null)
                    //    //  {
                    //    //      /* Dispose the bitmap. */
                    //    //      bitmap.Dispose();
                    //    //  }
                    //}

                    //send the image to the host
                    FrameReceivedHandler frameReceivedHandler = this.m_frh;
                    Console.WriteLine("setup frameReceiveHandler");
                    if (null != frameReceivedHandler)
                    {
                        // Report image to user
                        
                        frameReceivedHandler(this, new FrameEventArgs(image));


                    }

                    /* The processing of the image is done. Release the image buffer. */
                    m_imageProvider.ReleaseImage();
                    waitingForImage = false;
                    /* The buffer can be used for the next image grabs. */
                }
            }
            catch (Exception e)
            {
                waitingForImage = false;
               // ShowException(e, m_imageProvider.GetLastErrorMessage());
            }
        }
    }
   //class BaslerImageGrabber
   // {


   //    static void Main(string[] args)
   //    {

   //        try
   //        {
              

   //            int i;
   //            Grabber g = new Grabber();
   //            g.Open();
   //           // g.getImage("4000ms",192);
   //            g.close();
   //            Pylon.Terminate();
   //        }
   //        catch (Exception e)
   //        {
   //            /* Retrieve the error message. */
   //            string msg = GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail();
   //            Console.Error.WriteLine("Exception caught:");
   //            Console.Error.WriteLine(e.Message);
   //            if (msg != "\n")
   //            {
   //                Console.Error.WriteLine("Last error message:");
   //                Console.Error.WriteLine(msg);
   //            }



   //            Pylon.Terminate();  /* Releases all pylon resources. */

   //            Console.Error.WriteLine("\nPress enter to exit.");
   //            Console.ReadLine();

   //            Environment.Exit(1);
   //        }
   //    }

   //     /* Simple "image processing" function returning the minimum and maximum gray
   //     value of an 8 bit gray value image. */
   //     static void getMinMax(Byte[] imageBuffer, long width, long height, out Byte min, out Byte max)
   //     {
   //         min = 255; max = 0;
   //         long imageDataSize = width * height;

   //         for (long i = 0; i < imageDataSize; ++i)
   //         {
   //             Byte val = imageBuffer[i];
   //             if (val > max)
   //                 max = val;
   //             if (val < min)
   //                 min = val;
   //         }
   //     }
        
   // }
}
