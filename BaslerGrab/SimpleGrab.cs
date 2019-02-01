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
namespace SimpleGrab
{

    public class Grabber
    {
        private NODE_HANDLE m_hNode = new NODE_HANDLE();
        private PylonC.NETSupportLibrary.ImageProvider m_imageProvider = new PylonC.NETSupportLibrary.ImageProvider(); /* Create one image provider. */
        private Bitmap m_bitmap = null; /* The bitmap is used for displaying the image. */
        private Boolean waitingForImage = true;
        protected Thread m_grabThread;  

        public void Open()
        {
            try
            {
                uint numDevices;    /* Number of available devices. */
                const int numGrabs = 1; /* Number of images to grab. */
                PylonBuffer<Byte> imgBuf = null;  /* Buffer used for grabbing. */
                bool isAvail;
                int i;

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
                m_hNode = m_imageProvider.GetNodeFromDevice("PacketSize");
                GenApi.IntegerSetValue(m_hNode, 1200);
                m_hNode = m_imageProvider.GetNodeFromDevice("PixelFormat");
                GenApi.NodeFromString(m_hNode, "Mono16");
                m_hNode = m_imageProvider.GetNodeFromDevice("TriggerMode");
                GenApi.NodeFromString(m_hNode, "On");
                m_hNode = m_imageProvider.GetNodeFromDevice("TriggerSource");
                GenApi.NodeFromString(m_hNode, "Software");
                m_hNode = m_imageProvider.GetNodeFromDevice("ExposureMode");
                GenApi.NodeFromString(m_hNode, "TriggerWidth");
                //s.m_hNode = s.m_imageProvider.GetNodeFromDevice("AcquisitionMode");
                //GenApi.NodeFromString(s.m_hNode, "SingleFrame");
            }
            catch (Exception e)
            {
                // ShowException(e, m_imageProvider.GetLastErrorMessage());
            }
               

        }
        public void close()
        {
            m_imageProvider.Close();
            Pylon.Terminate();
        }
        public Bitmap getImage(int timeValue)
        {
                m_imageProvider.OneShot();
                System.Threading.Thread.Sleep(10);
                Debug.WriteLine("taking picture...");

                m_hNode = m_imageProvider.GetNodeFromDevice("TriggerActivation");
                GenApi.NodeFromString(m_hNode, "RisingEdge");
                m_hNode = m_imageProvider.GetNodeFromDevice("TriggerActivation");
                GenApi.NodeFromString(m_hNode, "FallingEdge");
                System.Threading.Thread.Sleep(timeValue);
                m_hNode = m_imageProvider.GetNodeFromDevice("TriggerActivation");
                GenApi.NodeFromString(m_hNode, "RisingEdge");
                waitingForImage = true;
                while (waitingForImage)
                {
                    
                    System.Threading.Thread.Sleep(10);
                }
                return m_bitmap;
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
                    if (BitmapFactory.IsCompatible(m_bitmap, image.Width, image.Height, image.Color))
                    {
                        /* Update the bitmap with the image data. */
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* To show the new image, request the display control to update itself. */
                      //  pictureBox.Refresh();
                    }
                    else /* A new bitmap is required. */
                    {
                        BitmapFactory.CreateBitmap(out m_bitmap, image.Width, image.Height, image.Color);
                        BitmapFactory.UpdateBitmap(m_bitmap, image.Buffer, image.Width, image.Height, image.Color);
                        /* We have to dispose the bitmap after assigning the new one to the display control. */
                      //  Bitmap bitmap = pictureBox.Image as Bitmap;
                      //  /* Provide the display control with the new bitmap. This action automatically updates the display. */
                      ////  pictureBox.Image = m_bitmap;
                      //  if (bitmap != null)
                      //  {
                      //      /* Dispose the bitmap. */
                      //      bitmap.Dispose();
                      //  }
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
   class SimpleGrab
    {


       static void Main(string[] args)
       {

           try
           {
              

               int i;
               Grabber g = new Grabber();
               g.Open();
               g.getImage(4000);
               g.close();
               Pylon.Terminate();
           }
           catch (Exception e)
           {
               /* Retrieve the error message. */
               string msg = GenApi.GetLastErrorMessage() + "\n" + GenApi.GetLastErrorDetail();
               Console.Error.WriteLine("Exception caught:");
               Console.Error.WriteLine(e.Message);
               if (msg != "\n")
               {
                   Console.Error.WriteLine("Last error message:");
                   Console.Error.WriteLine(msg);
               }



               Pylon.Terminate();  /* Releases all pylon resources. */

               Console.Error.WriteLine("\nPress enter to exit.");
               Console.ReadLine();

               Environment.Exit(1);
           }
       }

        /* Simple "image processing" function returning the minimum and maximum gray
        value of an 8 bit gray value image. */
        static void getMinMax(Byte[] imageBuffer, long width, long height, out Byte min, out Byte max)
        {
            min = 255; max = 0;
            long imageDataSize = width * height;

            for (long i = 0; i < imageDataSize; ++i)
            {
                Byte val = imageBuffer[i];
                if (val > max)
                    max = val;
                if (val < min)
                    min = val;
            }
        }
        
    }
}
