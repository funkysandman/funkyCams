using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Drawing.Imaging;

using GigEApi;

namespace SVCamApi
{


    public delegate void FrameReceivedHandler(object sender, FrameEventArgs args);

    public class FrameEventArgs : EventArgs
    {
        /// <summary>
        /// The Image (data)
        /// </summary>
        private Image m_Image = null;

        /// <summary>
        /// The Exception (data)
        /// </summary>
        private Exception m_Exception = null;

        /// <summary>
        /// Initializes a new instance of the FrameEventArgs class. 
        /// </summary>
        /// <param name="image">The Image to transfer</param>
        public FrameEventArgs(Image image)
        {
            if (null == image)
            {
                throw new ArgumentNullException("image");
            }

            m_Image = image;
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
        public Image Image
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

    public class SVCamGrabber
    {
        public static bool m_saveLocal;
        public static bool useDarks;
        public static float _darkmultiplier;
        private int m_duration;
        private float m_aGain;
        private float m_dGain;
        private float m_frameRate;

        public class NativeMethods
        {
            [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
            public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);
        }

        public struct imagebufferStruct
        {
            public byte[] imagebytes;
            public int sizeX;
            public int sizeY;
            public int dataLegth;
        };
        static class DefineConstants
        {
            public const string SVGenSDK_DLL = "SVGenSDK.DLL";    //public string SVGenSDK_DLL = IntPtr.Size == 8 ? "SVGenSDK64.DLL" : "SVGenSDK.DLL";
            public const string SVGenSDK_DLL64 = "SVGenSDK64.DLL";


            public const int SV_STRING_SIZE = 512;
            public const int SV_GVSP_PIX_MONO = 0x01000000;
            public const int SV_GVSP_PIX_RGB = 0x02000000;
            public const int SV_GVSP_PIX_OCCUPY8BIT = 0x00080000;
            public const int SV_GVSP_PIX_OCCUPY12BIT = 0x000C0000;
            public const int SV_GVSP_PIX_OCCUPY16BIT = 0x00100000;
            public const int SV_GVSP_PIX_OCCUPY24BIT = 0x00180000;
            public const uint SV_GVSP_PIX_COLOR_MASK = 0xFF000000;
            public const int SV_GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK = 0x00FF0000;
            public const int SV_GVSP_PIX_ID_MASK = 0x0000FFFF;
            public const uint INFINIT = 0xFFFFFF;
        }
        public enum SV_GVSP_PIXEL_TYPE
        {
            // Unknown pixel format
            SV_GVSP_PIX_UNKNOWN = 0x0000,

            // Mono buffer format defines
            SV_GVSP_PIX_MONO8 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY8BIT | 0x0001),
            SV_GVSP_PIX_MONO10 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x0003),
            SV_GVSP_PIX_MONO10_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x0004),
            SV_GVSP_PIX_MONO12 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x0005),
            SV_GVSP_PIX_MONO12_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x0006),
            SV_GVSP_PIX_MONO16 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x0007),

            // Bayer buffer format defines
            SV_GVSP_PIX_BAYGR8 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY8BIT | 0x0008),
            SV_GVSP_PIX_BAYRG8 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY8BIT | 0x0009),
            SV_GVSP_PIX_BAYGB8 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY8BIT | 0x000A),
            SV_GVSP_PIX_BAYBG8 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY8BIT | 0x000B),
            SV_GVSP_PIX_BAYGR10 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x000C),
            SV_GVSP_PIX_BAYRG10 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x000D),
            SV_GVSP_PIX_BAYGB10 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x000E),
            SV_GVSP_PIX_BAYBG10 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x000F),
            SV_GVSP_PIX_BAYGR12 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x0010),
            SV_GVSP_PIX_BAYRG12 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x0011),
            SV_GVSP_PIX_BAYGB12 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x0012),
            SV_GVSP_PIX_BAYBG12 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY16BIT | 0x0013),

            SV_GVSP_PIX_BAYGR10_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x0026),
            SV_GVSP_PIX_BAYRG10_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x0027),
            SV_GVSP_PIX_BAYGB10_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x0028),
            SV_GVSP_PIX_BAYBG10_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x0029),
            SV_GVSP_PIX_BAYGR12_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x002A),
            SV_GVSP_PIX_BAYRG12_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x002B),
            SV_GVSP_PIX_BAYGB12_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x002C),
            SV_GVSP_PIX_BAYBG12_PACKED = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY12BIT | 0x002D),

            // Color buffer format defines
            SV_GVSP_PIX_RGB24 = (DefineConstants.SV_GVSP_PIX_RGB | DefineConstants.SV_GVSP_PIX_OCCUPY24BIT),

            // Define for a gray image that was converted from a bayer coded image
            SV_GVSP_PIX_GRAY8 = (DefineConstants.SV_GVSP_PIX_MONO | DefineConstants.SV_GVSP_PIX_OCCUPY8BIT),

        }
        public void setParams(int duration, float aGain)
        {

            m_duration = duration;
            m_aGain = aGain;
            
         
            IntPtr phFeature = IntPtr.Zero;
            Camera cam = current_selected_cam;
            cam.duration = duration;
            SVCamApi.SVcamApi.SVSCamApiReturn ret;
            phFeature = IntPtr.Zero;
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "Gain", ref phFeature);
            ret = SVSCam.myApi.SVS_FeatureSetValueFloat(cam.hRemoteDevice, phFeature, (float)aGain);

            SVcamApi._SVCamFeaturInf info = new SVcamApi._SVCamFeaturInf();
            //SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "GainRaw", ref phFeature);
            //cam.getFeatureValue(phFeature, ref info);

            //phFeature = IntPtr.Zero;
            //SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "ExposureTime", ref phFeature);
            //SVSCam.myApi.SVS_FeatureSetValueFloat(cam.hRemoteDevice, phFeature, duration);

            //phFeature = IntPtr.Zero;
            //SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "AcquisitionFrameRate", ref phFeature);
            //SVSCam.myApi.SVS_FeatureSetValueFloat(cam.hRemoteDevice, phFeature, Math.Min((float)999220 / duration, (float)2));

            //phFeature = IntPtr.Zero;
            //SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "PixelFormat", ref phFeature);
            //cam.getFeatureValue(phFeature, ref info);

            //SVSCam.myApi.SVS_FeatureSetValueInt64(cam.hRemoteDevice, phFeature, (long)SV_GVSP_PIXEL_TYPE.SV_GVSP_PIX_BAYRG12_PACKED);

 
            phFeature = IntPtr.Zero;
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "ExposureAuto", ref phFeature);
            info = new SVcamApi._SVCamFeaturInf();
            SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "ExposureAuto", ref phFeature);
            cam.getFeatureValue(phFeature, ref info);

            //if (duration > 100000)//only bother with autoexposure during the day with short shutter times
            //{
            //    ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature,(int) 0);
            //   // ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref fi);
            //  //  ret = SVSCam.myApi.SVS_FeatureSetValueInt64(cam.hRemoteDevice, "ExposureAuto", ref phFeature);
            //}
            //else
            //{
            //    //ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature, (int) 1);
               
            //}


        }

        public class Camera
        {
            // Image 
            public imagebufferStruct[] imagebufferMono = new imagebufferStruct[4];
            public imagebufferStruct[] imagebufferRGB = new imagebufferStruct[4];
            public imagebufferStruct[] imagebufferDark = new imagebufferStruct[10];
            public byte[] masterDark;
            public int imageSizeX = 0;
            public int imageSizeY = 0;
            public bool isrgb = false;
            public int destdataIndex = 0;
            public string camTemp = "";
            public IntPtr Imagptr = IntPtr.Zero;
            public int duration;

         //   private ObjectDetection.TFDetector md = new ObjectDetection.TFDetector();
            private FrameReceivedHandler m_FrameReceivedHandler = null;
            private Bitmap b;
            // private string m_savePath;

            public int camWidth, camHeight;
            // Device
            SVcamApi myApi = null;
            public IntPtr hRemoteDevice = IntPtr.Zero;
            public IntPtr hDevice = IntPtr.Zero;
            public SVcamApi._SV_DEVICE_INFO devInfo;
            public bool is_opened = false;
            public SVcamApi._SVCamFeaturInf info;
            // Streaming
            public IntPtr hStream = IntPtr.Zero;
            uint dsBufcount = 0;
            bool isStreaming = false;
            // public byte[] buff = null;


            // public Thread thread = null;
            public bool threadIsRuning = false;

            // Camera Feature 
            public Queue<SVcamApi._SVCamFeaturInf> featureInfolist;
            public SVcamApi._SV_BUFFER_INFO bufferInfoDest = new SVcamApi._SV_BUFFER_INFO();
            public SVcamApi._SV_BUFFER_INFO bufferInfosrc = new SVcamApi._SV_BUFFER_INFO();


            public Camera(SVcamApi._SV_DEVICE_INFO _devinfo, SVcamApi _myApi)

            {
                
                devInfo = _devinfo;
                featureInfolist = new Queue<SVcamApi._SVCamFeaturInf>();
                SVcamApi._SVCamFeaturInf info = new SVcamApi._SVCamFeaturInf();
                hRemoteDevice = new IntPtr();
                hDevice = new IntPtr();
                myApi = _myApi;
                bufferInfoDest.pImagePtr = IntPtr.Zero;
                bufferInfosrc.pImagePtr = IntPtr.Zero;
                // bufferInfosrc.pImagePtr = Marshal.AllocHGlobal(1920 * 1200 * 3);
                loadMasterDark();
            }

            public void loadMasterDark()
            {
                try
                { 
                masterDark = File.ReadAllBytes("masterSVSdark.raw");
                }
                catch
                {

                }

            }
            ~Camera()
            {
                if (isStreaming)
                {
                    acquisitionStop();
                    StreamingChannelClose();
                }
                closeConnection();
            }


            // Camera: Connection
            public SVcamApi.SVSCamApiReturn openConnection()
            {
                if (is_opened)
                    return 0;

                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                //Open the device with device id (devInfo.uid) connected to the interface (devInfo.hParentIF)
                ret = myApi.SVS_InterfaceDeviceOpen(devInfo.hParentIF, devInfo.uid, SVcamApi.SV_DEVICE_ACCESS_FLAGS_LIST.SV_DEVICE_ACCESS_CONTROL, ref hDevice, ref hRemoteDevice);


                if (ret == SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                {
                    is_opened = true;
                    Console.WriteLine("openned connection");

                }
                else
                {
                    is_opened = false;
                    Console.WriteLine("openned connection {0}", ret);
                }

                return ret;
            }
            public SVcamApi.SVSCamApiReturn CheckCamera()
            {
                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                try { 
                ret = myApi.SVS_InterfaceDeviceOpen(devInfo.hParentIF, devInfo.uid, SVcamApi.SV_DEVICE_ACCESS_FLAGS_LIST.SV_DEVICE_ACCESS_CONTROL, ref hDevice, ref hRemoteDevice);
                }
                //  Console.WriteLine("open connection");
                catch
                {

                }
                if (ret == SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                { 
                    is_opened = true;
                
                }
                else
                {
                    is_opened = false;
                    
                }
                return ret;
            }


            public SVcamApi.SVSCamApiReturn closeConnection()
            {
                Console.WriteLine("closing connection - optimistic");
                is_opened = false;
                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                ret = myApi.SVS_DeviceClose(hDevice);
                Console.WriteLine("closing connection {0}", ret);
                return ret;
            }



            // Camera: feature list
            public void getFeatureValue(IntPtr hFeature, ref SVcamApi._SVCamFeaturInf SvCamfeatureInfo)
            {
                myApi.SVS_FeatureGetInfo(hRemoteDevice, hFeature, ref SvCamfeatureInfo.SVFeaturInf);
                if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIInteger == SvCamfeatureInfo.SVFeaturInf.type)
                {
                    Int64 value = 0;
                    myApi.SVS_FeatureGetValueInt64(hRemoteDevice, hFeature, ref value);
                    SvCamfeatureInfo.intValue = (ulong)value;
                    string st = value.ToString();
                    SvCamfeatureInfo.strValue = string.Copy(st);
                }
                else if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIFloat == SvCamfeatureInfo.SVFeaturInf.type)
                {
                    double value = 0.0f;
                    myApi.SVS_FeatureGetValueFloat(hRemoteDevice, hFeature, ref value);
                    string st = value.ToString();
                    SvCamfeatureInfo.strValue = string.Copy(st);
                }
                else if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIBoolean == SvCamfeatureInfo.SVFeaturInf.type)
                {
                    bool value = false;
                    myApi.SVS_FeatureGetValueBool(hRemoteDevice, hFeature, ref value);
                    SvCamfeatureInfo.booValue = value;
                    if (value)
                        SvCamfeatureInfo.strValue = "True";
                    else
                        SvCamfeatureInfo.strValue = "False";
                }
                else if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfICommand == SvCamfeatureInfo.SVFeaturInf.type)
                {
                    SvCamfeatureInfo.strValue = " = > Execute Command";
                }
                else if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIString == SvCamfeatureInfo.SVFeaturInf.type)
                {
                    myApi.SVS_FeatureGetValueString(hRemoteDevice, hFeature, ref SvCamfeatureInfo.strValue, SVcamApi.DefineConstants.SV_STRING_SIZE);
                }
                else if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIEnumeration == SvCamfeatureInfo.SVFeaturInf.type)
                {
                    int pInt64Value = 0;
                    uint buffSize = SVcamApi.DefineConstants.SV_STRING_SIZE;
                    SVcamApi.SVSCamApiReturn ret = myApi.SVS_FeatureEnumSubFeatures(hRemoteDevice, hFeature, (int)SvCamfeatureInfo.SVFeaturInf.enumSelectedIndex, ref SvCamfeatureInfo.subFeatureName, buffSize, ref pInt64Value);
                    SvCamfeatureInfo.intValue = (UInt64)pInt64Value;
                    SvCamfeatureInfo.strValue = SvCamfeatureInfo.subFeatureName;
                }
            }

            public void getDeviceFeatureList(SVcamApi.SV_FEATURE_VISIBILITY visibility)
            {
                //DSDeleteContainer(featureInfolist);
                uint iIndex = 0;
                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                while (true)
                {
                    if (iIndex == 500)
                        break;
                    IntPtr hFeature = IntPtr.Zero;
                    ret = myApi.SVS_FeatureGetByIndex(hRemoteDevice, iIndex++, ref hFeature);

                    if (SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS != ret)
                        break;
                    //Create a new Feature structure and
                    SVcamApi._SVCamFeaturInf camFeatureInfo = new SVcamApi._SVCamFeaturInf();
                    ret = myApi.SVS_FeatureGetInfo(hRemoteDevice, hFeature, ref camFeatureInfo.SVFeaturInf);

                    if (SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS != ret)
                    {
                        //  Console.WriteLine(" SVFeatureGetInfo Failed!:%d\n", ret);
                        continue;
                    }

                    //	retrive only a specific features 
                    if (camFeatureInfo.SVFeaturInf.visibility > (uint)visibility || (int)SVcamApi.SV_FEATURE_TYPE.SV_intfIPort == camFeatureInfo.SVFeaturInf.type)
                    {
                        continue;
                    }

                    // get the current value and feature info 
                    getFeatureValue(hFeature, ref camFeatureInfo);
                    //add the feature handle and remote device handle 
                    camFeatureInfo.hFeature = hFeature;
                    camFeatureInfo.hRemoteDevice = hRemoteDevice;
                    featureInfolist.Enqueue(camFeatureInfo);
                }
            }

            //  Stream: Channel creation and control
            public SVcamApi.SVSCamApiReturn StreamingChannelOpen()
            {
                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                string streamId0 = null;
                uint streamId0Size = 512;

                // retriev the stream ID 
                ret = myApi.SVS_DeviceGetStreamId(hDevice, 0, ref streamId0, ref streamId0Size);
                Thread.Sleep(100);
                Console.WriteLine("get device stream {0}", ret);
                if (SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS != ret)
                    return ret;

                //open the Streaming channel with the retrieved stream ID
                ret = myApi.SVS_DeviceStreamOpen(hDevice, streamId0, ref hStream);
                Thread.Sleep(100);
                Console.WriteLine("device stream open {0}", ret);
                return ret;
            }

            public SVcamApi.SVSCamApiReturn StreamingChannelClose()
            {
                return myApi.SVS_SVStreamClose(hStream);
            }

            public bool grab()
            {
                unsafe
                {
                    IntPtr hFeature = IntPtr.Zero;
                    SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                    IntPtr hBuffer = IntPtr.Zero;
                    IntPtr Imagptr2 = IntPtr.Zero;
                    //good time to check temperature

                    //temp = 0;
                    // myApi.Gige_Camera_getSensorTemperature(hCamera, ref temp);
                    IntPtr phFeature = IntPtr.Zero;
                    phFeature = IntPtr.Zero;
                                       
                    camTemp = "na";

                    Console.WriteLine("about to streamWaitForNewBuffer");
                    //
                    ret = SVcamApi.SVSCamApiReturn.SV_ERROR_NOT_INITIALIZED;
                    uint timeout = (uint)this.duration + 1000;
                    
                    while (ret!= SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    { 
                        ret = myApi.SVS_StreamWaitForNewBuffer(hStream, ref Imagptr2, ref hBuffer, 10000);
                        Console.WriteLine("finished streamWaitForNewBuffer");
                        if (ret == SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                        {
                            ret = myApi.SVS_StreamBufferGetInfo(hStream, hBuffer, ref bufferInfosrc);
                            if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                            {
                                Console.Write("ERROR TIMEOUT 1 !!");
                                myApi.SVS_StreamQueueBuffer(hStream, hBuffer);
                                //return false;
                            }
                        }
                        else

                        {

                            Console.WriteLine("ERROR:{0}", ret);

                            //assuming a timeout happened...
                            //send another buffer
                            //myApi.SVS_StreamQueueBuffer(hStream, hBuffer);
                            myApi.SVS_FeatureGetByName(hRemoteDevice, SVcamApi.CameraFeature.AcquisitionStart, ref hFeature);
                            myApi.SVS_StreamQueueBuffer(hStream, hBuffer);
                            ret = myApi.SVS_FeatureCommandExecute(hRemoteDevice, hFeature, 1);
                            Console.WriteLine("call acquisition start:{0}", ret);
                            //return false;

                        }
                    }
                    if (bufferInfosrc.pImagePtr == IntPtr.Zero)
                        return false;

                    if (bufferInfoDest.pImagePtr == IntPtr.Zero)

                        bufferInfoDest.pImagePtr = Marshal.AllocHGlobal(bufferInfosrc.iImageSize);

                    NativeMethods.CopyMemory(bufferInfoDest.pImagePtr, bufferInfosrc.pImagePtr, (uint)bufferInfosrc.iImageSize);

                    bufferInfoDest.iImageSize = bufferInfosrc.iImageSize;
                    bufferInfoDest.iSizeX = bufferInfosrc.iSizeX;
                    bufferInfoDest.iSizeY = bufferInfosrc.iSizeY;
                    bufferInfoDest.iPixelType = bufferInfosrc.iPixelType;
                    bufferInfoDest.iImageId = bufferInfosrc.iImageId;
                    bufferInfoDest.iTimeStamp = bufferInfosrc.iTimeStamp;
                    //Queues a particular buffer for acquisition.
                    myApi.SVS_StreamQueueBuffer(hStream, hBuffer);
                    return true;
                }
            }

            public SVcamApi.SVSCamApiReturn acquisitionStart(uint bufcount,FrameReceivedHandler frh)
            {
                m_FrameReceivedHandler = frh;
                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                IntPtr hFeature = IntPtr.Zero;
                Int64 payloadSize = 0;

                //retrieve the payload size to allocate the buffers
                myApi.SVS_FeatureGetByName(hRemoteDevice, SVcamApi.CameraFeature.PayloadSize, ref hFeature);
                myApi.SVS_FeatureGetValueInt64(hRemoteDevice, hFeature, ref payloadSize);

                // allocat buffers with the retrieved payload size. 
                for (uint i = 0; i < bufcount; i++)
                {

                    IntPtr hBuffer = IntPtr.Zero;
                    myApi.SVS_StreamAllocAndAnnounceBuffer(hStream, (uint)payloadSize, Imagptr, ref hBuffer);
                    if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    {
                        continue;
                    }

                    ret = myApi.SVS_StreamQueueBuffer(hStream, hBuffer);
                    if (SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS != ret)
                        continue;

                }

                myApi.SVS_StreamFlushQueue(hStream, SVcamApi.SV_ACQ_QUEUE_TYPE_LIST.SV_ACQ_QUEUE_ALL_TO_INPUT);
                ret = myApi.SVS_StreamAcquisitionStart(hStream, SVcamApi.SV_ACQ_START_FLAGS_LIST.SV_ACQ_START_FLAGS_DEFAULT, SVcamApi.DefineConstants.INFINIT);

                if (SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS != ret)
                {
                    for (UInt32 i = 0; i < bufcount; i++)
                    {
                        IntPtr hBuffer = IntPtr.Zero;
                        ret = myApi.SVS_StreamGetBufferId(hStream, 0, ref hBuffer);

                        IntPtr pBuffer = IntPtr.Zero;
                        IntPtr imaptr = IntPtr.Zero;

                        if (IntPtr.Zero != hBuffer)
                        {
                            myApi.SVS_StreamRevokeBuffer(hStream, hBuffer, ref pBuffer, ref (imaptr));
                        }
                    }
                }

                //  set acquisitionstart 
                uint ExecuteTimeout = 10000;
                hFeature = IntPtr.Zero;


                myApi.SVS_FeatureGetByName(hRemoteDevice, SVcamApi.CameraFeature.AcquisitionStart, ref hFeature);
                myApi.SVS_FeatureCommandExecute(hRemoteDevice, hFeature, ExecuteTimeout);
                hFeature = IntPtr.Zero;
                ret = myApi.SVS_FeatureGetByName(hRemoteDevice, SVcamApi.CameraFeature.TLParamsLocked, ref hFeature);
                Int64 paramsLocked = 1;
                ret = myApi.SVS_FeatureSetValueInt64(hRemoteDevice, hFeature, paramsLocked);

                if (SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS == ret)
                    dsBufcount = bufcount;
                return ret;
            }

            public SVcamApi.SVSCamApiReturn acquisitionStop()
            {
                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                //  set acquisitionstart 
                uint ExecuteTimeout = 1000;
                IntPtr hFeature = IntPtr.Zero;
                ret = myApi.SVS_FeatureGetByName(hRemoteDevice, SVcamApi.CameraFeature.AcquisitionStop, ref hFeature);

                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return ret;
                ret = myApi.SVS_FeatureCommandExecute(hRemoteDevice, hFeature, ExecuteTimeout);

                // 
                hFeature = IntPtr.Zero;
                ret = myApi.SVS_FeatureGetByName(hRemoteDevice, SVcamApi.CameraFeature.TLParamsLocked, ref hFeature);
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return ret;
                ret = myApi.SVS_FeatureSetValueInt64(hRemoteDevice, hFeature, 0);
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return ret;

                ret = myApi.SVS_StreamAcquisitionStop(hStream, SVcamApi.SV_ACQ_STOP_FLAGS_LIST.SV_ACQ_STOP_FLAGS_DEFAULT);
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return ret;

                ret = myApi.SVS_StreamFlushQueue(hStream, SVcamApi.SV_ACQ_QUEUE_TYPE_LIST.SV_ACQ_QUEUE_INPUT_TO_OUTPUT);
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return ret;
                ret = myApi.SVS_StreamFlushQueue(hStream, SVcamApi.SV_ACQ_QUEUE_TYPE_LIST.SV_ACQ_QUEUE_OUTPUT_DISCARD);
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return ret;


                IntPtr hBuffer = IntPtr.Zero;
                IntPtr pBuffer = IntPtr.Zero;
                IntPtr pBuffer2 = IntPtr.Zero;

                for (UInt32 i = 0; i < dsBufcount; i++)
                {

                    ret = myApi.SVS_StreamGetBufferId(hStream, 0, ref hBuffer);


                    if (hBuffer != IntPtr.Zero)
                    {
                        myApi.SVS_StreamRevokeBuffer(hStream, hBuffer, ref pBuffer, ref pBuffer2);
                    }
                    pBuffer = IntPtr.Zero;
                    pBuffer2 = IntPtr.Zero;
                    hBuffer = IntPtr.Zero;
                }
                return ret;
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

            //public byte[] AddnewDarkImageData(SVcamApi._SV_BUFFER_INFO ImageInfo, bool isImgRGB)
            //{
            //    // Obtain image information structure
            //    Console.WriteLine("addnewDarkImageData");
            //    if (ImageInfo.pImagePtr == IntPtr.Zero)
            //        return null;

            //    //return 
            //    //int currentIdex = destdataIndex;

            //    //// 8 bit Format
            //    //if (((int)ImageInfo.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK) == SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_OCCUPY8BIT)
            //    //{
            //    //    if (isImgRGB)
            //    //    {
            //    //        myApi.SVS_UtilBufferBayerToRGB(ImageInfo, ref imagebufferRGB[currentIdex].imagebytes[0], imagebufferRGB[currentIdex].dataLegth);
            //    //    }
            //    //    else
            //    //    {
            //    //        System.Runtime.InteropServices.Marshal.Copy(ImageInfo.pImagePtr, imagebufferMono[currentIdex].imagebytes, 0, imagebufferMono[currentIdex].dataLegth);
            //    //    }
            //    // }

            //    //---12 bit Format-------------------------------------------------------------------------------------------------------------------

            //    if (((int)ImageInfo.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK) == SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_OCCUPY12BIT)
            //    {
            //        if (isImgRGB)
            //        {
            //           // myApi.SVS_UtilBufferBayerToRGB(ImageInfo, ref imagebufferRGB[currentIdex].imagebytes[0], imagebufferRGB[currentIdex].dataLegth);

            //            //do image stuff here

            //            //ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            //            //Bitmap b = new Bitmap(this.imageSizeX, this.imageSizeY, PixelFormat.Format24bppRgb);
            //            //BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, b.PixelFormat);

            //            //Console.WriteLine("about to copy buffer into bitmapdata");
            //            //Marshal.Copy(imagebufferRGB[currentIdex].imagebytes, 0, bmpData.Scan0, imagebufferRGB[currentIdex].imagebytes.Length);
            //            //Console.WriteLine("copied buffer into bitmapdata");


            //            //b.UnlockBits(bmpData);





            //           // return b;

            //        }
            //        else
            //            return null;

            //        //
            //    }
            //    else
            //        return null;

            //    //else
            //    //{

            //    //    if (ImageInfo.pImagePtr != null)
            //    //    {
            //    //        // Convert to 8 bit 
            //    //        myApi.SVS_UtilBuffer12BitTo8Bit(ImageInfo, ref imagebufferMono[currentIdex].imagebytes[0], imagebufferMono[currentIdex].dataLegth);
            //    //    }
            //    //}


            //}

            //public void addnewImageData(SVcamApi._SV_BUFFER_INFO ImageInfo, bool isImgRGB)
            //{
            //    // Obtain image information structure
            //    Console.WriteLine("addnewImageData");
            //    if (ImageInfo.pImagePtr == IntPtr.Zero)
            //        return;

            //    int currentIdex = destdataIndex;
            //    {
            //        // 8 bit Format
            //        if (((int)ImageInfo.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK) == SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_OCCUPY8BIT)
            //        {
            //            if (isImgRGB)
            //            {

            //                myApi.SVS_UtilBufferBayerToRGB32(ImageInfo, ref imagebufferRGB[currentIdex].imagebytes[0], imagebufferRGB[currentIdex].dataLegth);
            //            }
            //            else
            //            {
            //                System.Runtime.InteropServices.Marshal.Copy(ImageInfo.pImagePtr, imagebufferMono[currentIdex].imagebytes, 0, imagebufferMono[currentIdex].dataLegth);
            //            }
            //        }

            //        //---12 bit Format-------------------------------------------------------------------------------------------------------------------

            //        else if (((int)ImageInfo.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK) == SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_OCCUPY12BIT)
            //        {
            //            if (isImgRGB)
            //            {
            //                //subtract raw darks before debayer
            //                imagebufferStruct rawImage = new imagebufferStruct();
            //                rawImage.imagebytes = new byte[imagebufferRGB[currentIdex].dataLegth / 2];
            //                //rawImage.imagebytes = ImageInfo.pImagePtr;
            //                Marshal.Copy(ImageInfo.pImagePtr, rawImage.imagebytes, 0, imagebufferRGB[currentIdex].dataLegth / 2);
            //                //subtract darks
            //                //load dark array from file
            //                //
            //                Random r = new Random();
            //                int x;
            //                if (useDarks)
            //                {
            //                    for (int k = 0; k < imagebufferRGB[currentIdex].dataLegth / 2; k++)
            //                    {
            //                        if ((masterDark[k]) > 250)
            //                            rawImage.imagebytes[k] = (byte)Math.Max(0, rawImage.imagebytes[k] - _darkmultiplier * (masterDark[k]));

            //                    }
            //                }
            //                //subtract dark

            //                //copy back to imageInfo
            //                Marshal.Copy(rawImage.imagebytes, 0, ImageInfo.pImagePtr, imagebufferRGB[currentIdex].dataLegth / 2);

            //                //debayer buffer into RGB
            //                myApi.SVS_UtilBufferBayerToRGB(ImageInfo, ref imagebufferRGB[currentIdex].imagebytes[0], imagebufferRGB[currentIdex].dataLegth);


            //                //do image stuff here
            //                if (m_saveLocal)
            //                {
            //                    string filename = string.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "image", DateTime.Now);
            //                    try
            //                    {
            //                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            //                        Bitmap b = new Bitmap(this.imageSizeX, this.imageSizeY, PixelFormat.Format24bppRgb);
            //                        BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, b.PixelFormat);

            //                        Console.WriteLine("about to copy buffer into bitmapdata");
            //                        Marshal.Copy(imagebufferRGB[currentIdex].imagebytes, 0, bmpData.Scan0, imagebufferRGB[currentIdex].imagebytes.Length);
            //                        Console.WriteLine("copied buffer into bitmapdata");


            //                        b.UnlockBits(bmpData);

            //                        if (false)
            //                        {




            //                            Bitmap dark = new Bitmap("masterDarksvs.png");

            //                            // Lock the bitmap's bits.  
            //                            Rectangle rect = new Rectangle(0, 0, dark.Width, dark.Height);
            //                            System.Drawing.Imaging.BitmapData bData =
            //                                dark.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
            //                                dark.PixelFormat);

            //                            // Get the address of the first line.
            //                            IntPtr ptr = bData.Scan0;

            //                            // Declare an array to hold the bytes of the bitmap.
            //                            int bytes = Math.Abs(bData.Stride) * dark.Height;
            //                            byte[] darkRGBValues = new byte[bytes];

            //                            // Copy the dark RGB values into the array.
            //                            System.Runtime.InteropServices.Marshal.Copy(ptr, darkRGBValues, 0, bytes);

            //                            float multiplier;
            //                            multiplier = _darkmultiplier;
            //                            System.Drawing.Imaging.BitmapData bmpData2 = b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b.PixelFormat);
            //                            int bytes2 = Math.Abs(bmpData2.Stride) * dark.Height;
            //                            byte[] camRGBValues = new byte[bytes2];
            //                            IntPtr ptr2 = bmpData2.Scan0;
            //                            // Copy the camera RGB values into the array.
            //                            System.Runtime.InteropServices.Marshal.Copy(ptr2, camRGBValues, 0, bytes2);


            //                            int darkcounter = 0;

            //                            for (int counter = 0; counter < camRGBValues.Length - 1; counter = counter + 3)
            //                            {
            //                                //Console.WriteLine(Math.Max(0, (camRGBValues[counter] - darkRGBValues[darkcounter ])));
            //                                if (darkRGBValues[darkcounter] < 25) darkRGBValues[darkcounter] = 0;
            //                                if (darkRGBValues[darkcounter + 1] < 25) darkRGBValues[darkcounter + 1] = 0;
            //                                if (darkRGBValues[darkcounter + 2] < 25) darkRGBValues[darkcounter + 2] = 0;

            //                                camRGBValues[counter] = (byte)Math.Max(0, (camRGBValues[counter] - darkRGBValues[darkcounter] * multiplier));
            //                                camRGBValues[counter + 1] = (byte)Math.Max(0, (camRGBValues[counter + 1] - darkRGBValues[darkcounter + 1] * multiplier));
            //                                camRGBValues[counter + 2] = (byte)Math.Max(0, (camRGBValues[counter + 2] - darkRGBValues[darkcounter + 2] * multiplier));
            //                                //  camRGBValues[counter+1 ] =  Math.Max(0,(camRGBValues[counter +1] - darkRGBValues[darkcounter+2]));
            //                                //  camRGBValues[counter +2] =  Math.Max((byte)0, (byte)(camRGBValues[counter+2 ] - darkRGBValues[darkcounter]));
            //                                // Console.WriteLine(camRGBValues[counter + offset]);
            //                                darkcounter = darkcounter + 4;//bits are stored ARGB

            //                            }


            //                            // Copy the RGB values back to the bitmap
            //                            System.Runtime.InteropServices.Marshal.Copy(camRGBValues, 0, ptr2, bytes2);

            //                            // Unlock the bits.
            //                            b.UnlockBits(bmpData2);
            //                            dark.UnlockBits(bmpData);



            //                        }





            //                        /*too slow
            //                        Color myRgbColor = new Color();
            //                        if (m_useDarks)
            //                        {
            //                            //subtract dark
            //                            for (int x = 0; x < bimageRGB[0].Width; x++)
            //                            {
            //                                for (int y = 0; y < bimageRGB[0].Height; y++)
            //                                {
            //                                    //get pixel from bimagergb
            //                                    Color c = bimageRGB[0].GetPixel(x, y);
            //                                    Color c2 = dark.GetPixel(x, y);
            //                                    int r = Math.Max(0,c.R - c2.R);
            //                                    int g = Math.Max(0, c.G - c2.G);
            //                                    int b = Math.Max(0, c.B - c2.B);

            //                                    myRgbColor = Color.FromArgb(r, g, b);
            //                                    bimageRGB[0].SetPixel(x, y,myRgbColor);

            //                                }
            //                            }


            //                        }
            //                        */


            //                        // Create an Encoder object based on the GUID
            //                        // for the Quality parameter category.
            //                        System.Drawing.Imaging.Encoder myEncoder =
            //                            System.Drawing.Imaging.Encoder.Quality;

            //                        // Create an EncoderParameters object.
            //                        // An EncoderParameters object has an array of EncoderParameter
            //                        // objects. In this case, there is only one
            //                        // EncoderParameter object in the array.
            //                        EncoderParameters myEncoderParameters = new EncoderParameters(1);

            //                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            //                        myEncoderParameters.Param[0] = myEncoderParameter;
            //                        //
            //                        string firstText = string.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now);
            //                        uint temp;
            //                        temp = 0;
            //                        //myApi.Gige_Camera_getSensorTemperature(hCamera, ref temp);
            //                        IntPtr phFeature = IntPtr.Zero;
            //                        phFeature = IntPtr.Zero;
            //                        SVcamApi._SVCamFeaturInf info = new SVcamApi._SVCamFeaturInf();
            //                        Console.WriteLine("get temperature");
            //                        myApi.SVS_FeatureGetByName(hRemoteDevice, "DeviceTemperature", ref phFeature);
            //                        getFeatureValue(phFeature, ref info);

            //                        Console.WriteLine("got temperature");
            //                        string secondText = string.Format("Sensor temp:{0}C", info.strValue);

            //                        PointF firstLocation = new PointF(10f, 10f);
            //                        PointF secondLocation = new PointF(10f, 50f);



            //                        Console.WriteLine("get graphics context");

            //                        using (Graphics graphics = Graphics.FromImage(b))
            //                        {
            //                            using (Font arialFont = new Font("Arial", 20))
            //                            {
            //                                graphics.DrawString(firstText, arialFont, Brushes.GreenYellow, firstLocation);
            //                                graphics.DrawString(secondText, arialFont, Brushes.GreenYellow, secondLocation);
            //                            }
            //                        }

            //                        Console.WriteLine("wrote temperature on image");
            //                        string folderName = string.Format("{0:yyyy-MMM-dd}", DateTime.Now);
            //                        System.IO.Directory.CreateDirectory(Path.Combine("c:\\image", folderName));
            //                        Console.WriteLine("SVS Vistek: saving image");
            //                        b.Save(Path.Combine("c:\\image", folderName, filename), jpgEncoder, myEncoderParameters);
            //                        Console.WriteLine("checking for meteors");
            //                       // md.examine(b, Path.Combine("c:\\image", folderName, filename));
            //                        Console.WriteLine("done");
            //                        //++;
            //                        //fifoCount--;

            //                    }
            //                    catch (Exception e)
            //                    {
            //                        Console.WriteLine("SVS Vistek: " + e.Message);
            //                    }
            //                }


            //                //
            //            }

            //            else
            //            {

            //                if (ImageInfo.pImagePtr != null)
            //                {
            //                    // Convert to 8 bit 
            //                    myApi.SVS_UtilBuffer12BitTo8Bit(ImageInfo, ref imagebufferMono[currentIdex].imagebytes[0], imagebufferMono[currentIdex].dataLegth);
            //                }
            //            }
            //        }
            //        else
            //            return;
            //    }
            //}
            public void addnewImageData2(SVcamApi._SV_BUFFER_INFO ImageInfo, bool isImgRGB)
            {
                // Obtain image information structure
                Console.WriteLine("addnewImageData");
                if (ImageInfo.pImagePtr == IntPtr.Zero)
                    return;

                int currentIdex = destdataIndex;
                {
                    // 8 bit Format
                    if (((int)ImageInfo.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK) == SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_OCCUPY8BIT)
                    {
                        if (isImgRGB)
                        {
                            
                            imagebufferStruct rawImage = new imagebufferStruct();
                            rawImage.imagebytes = new byte[imagebufferRGB[currentIdex].dataLegth / 3];
                            //rawImage.imagebytes = ImageInfo.pImagePtr;
                            Marshal.Copy(ImageInfo.pImagePtr, rawImage.imagebytes, 0, imagebufferRGB[currentIdex].dataLegth  / 3);
                            //subtract darks
                            //load dark array from file
                            //
                            Random r = new Random();
                            int x;
                            if (useDarks)
                            {

                                loadMasterDark();
                                for (int k = 0; k < imagebufferRGB[currentIdex].dataLegth  / 3; k++)
                                {
                                    //if ((masterDark[k]) > 250)
                                        rawImage.imagebytes[k] = (byte)Math.Max(0, rawImage.imagebytes[k] - _darkmultiplier * (masterDark[k]));

                                }
                            }
                            //subtract dark

                            //copy back to imageInfo
                            Marshal.Copy(rawImage.imagebytes, 0, ImageInfo.pImagePtr, imagebufferRGB[currentIdex].dataLegth  / 3);

                            //debayer buffer into RGB
                            myApi.SVS_UtilBufferBayerToRGB(ImageInfo, ref imagebufferRGB[currentIdex].imagebytes[0], imagebufferRGB[currentIdex].dataLegth);


                            //do image stuff here
                            if (m_saveLocal)
                            {
                                string filename = string.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "image", DateTime.Now);
                                try
                                {
                                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                                    b = new Bitmap(this.imageSizeX, this.imageSizeY, PixelFormat.Format24bppRgb);
                                    BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, b.PixelFormat);

                                    Console.WriteLine("about to copy buffer into bitmapdata");
                                    Marshal.Copy(imagebufferRGB[currentIdex].imagebytes, 0, bmpData.Scan0, imagebufferRGB[currentIdex].imagebytes.Length);
                                    Console.WriteLine("copied buffer into bitmapdata");


                                    b.UnlockBits(bmpData);

                                    //raise event to host that we have a bitmap

                                    FrameReceivedHandler frameReceivedHandler = this.m_FrameReceivedHandler;
                                    Console.WriteLine("setup frameReceiveHandler");
                                    if (null != frameReceivedHandler && null != b)
                                    {
                                        // Report image to user
                                        frameReceivedHandler(this, new FrameEventArgs(b));


                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("SVS Vistek: " + e.Message);
                                }
                            }


                            //
                        }

                        else
                        {

                            if (ImageInfo.pImagePtr != null)
                            {
                                // Convert to 8 bit 
                                myApi.SVS_UtilBuffer12BitTo8Bit(ImageInfo, ref imagebufferMono[currentIdex].imagebytes[0], imagebufferMono[currentIdex].dataLegth);
                            }
                        }
                    }


                    //---16 bit Format-------------------------------------------------------------------------------------------------------------------

                    else if (((int)ImageInfo.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK) == SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_OCCUPY16BIT)
                    {
                        if (isImgRGB)
                        {
                            //subtract raw darks before debayer
                            imagebufferStruct rawImage = new imagebufferStruct();
                            rawImage.imagebytes = new byte[imagebufferRGB[currentIdex].dataLegth*2/3 ];
                            //rawImage.imagebytes = ImageInfo.pImagePtr;
                            Marshal.Copy(ImageInfo.pImagePtr, rawImage.imagebytes, 0, imagebufferRGB[currentIdex].dataLegth * 2 / 3);
                            //subtract darks
                            //load dark array from file
                            //
                            Random r = new Random();
                            int x;
                            if (useDarks)
                            {
                                for (int k = 0; k < imagebufferRGB[currentIdex].dataLegth * 2 / 3; k++)
                                {
                                    if ((masterDark[k]) > 250)
                                        rawImage.imagebytes[k] = (byte)Math.Max(0, rawImage.imagebytes[k] - _darkmultiplier * (masterDark[k]));

                                }
                            }
                            //subtract dark

                            //copy back to imageInfo
                            Marshal.Copy(rawImage.imagebytes, 0, ImageInfo.pImagePtr, imagebufferRGB[currentIdex].dataLegth * 2 / 3);

                            //debayer buffer into RGB
                            myApi.SVS_UtilBufferBayerToRGB(ImageInfo, ref imagebufferRGB[currentIdex].imagebytes[0], imagebufferRGB[currentIdex].dataLegth);
                           

                            //do image stuff here
                            if (m_saveLocal)
                            {
                                string filename = string.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "image", DateTime.Now);
                                try
                                {
                                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                                    b = new Bitmap(this.imageSizeX, this.imageSizeY, PixelFormat.Format24bppRgb);
                                    BitmapData bmpData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.WriteOnly, b.PixelFormat);

                                    Console.WriteLine("about to copy buffer into bitmapdata");
                                    Marshal.Copy(imagebufferRGB[currentIdex].imagebytes, 0, bmpData.Scan0, imagebufferRGB[currentIdex].imagebytes.Length);
                                    Console.WriteLine("copied buffer into bitmapdata");


                                    b.UnlockBits(bmpData);

                                    //raise event to host that we have a bitmap

                                    FrameReceivedHandler frameReceivedHandler = this.m_FrameReceivedHandler;
                                    Console.WriteLine("setup frameReceiveHandler");
                                    if (null != frameReceivedHandler && null != b)
                                    {
                                        // Report image to user
                                        frameReceivedHandler(this, new FrameEventArgs(b));


                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("SVS Vistek: " + e.Message);
                                }
                            }


                            //
                        }

                        else
                        {

                            if (ImageInfo.pImagePtr != null)
                            {
                                // Convert to 8 bit 
                                myApi.SVS_UtilBuffer12BitTo8Bit(ImageInfo, ref imagebufferMono[currentIdex].imagebytes[0], imagebufferMono[currentIdex].dataLegth);
                            }
                        }
                    }
                    else
                        return;
                }
            }
        }

        class Cameracontainer
        {
            public SVcamApi myApi;
            public List<SVcamApi._SV_TL_INFO> sv_tl_info_list;
            public List<SVcamApi._SV_INTERFACE_INFO> sv_interface_info_list;
            public List<SVcamApi._SV_DEVICE_INFO> sv_Dev_info_list;
            public List<IntPtr> sv_cam_sys_hdl_list;
            public List<IntPtr> sv_interface_hdl_list;
            public List<Camera> Camlist;

            public Cameracontainer()
            {
                myApi = new SVcamApi();
                sv_tl_info_list = new List<SVcamApi._SV_TL_INFO>();
                sv_interface_info_list = new List<SVcamApi._SV_INTERFACE_INFO>();
                sv_cam_sys_hdl_list = new List<IntPtr>();
                sv_Dev_info_list = new List<SVcamApi._SV_DEVICE_INFO>();
                sv_interface_hdl_list = new List<IntPtr>();
                Camlist = new List<Camera>();
            }

            ~Cameracontainer()
            {
                closeCameracontainer();
            }

            public bool InitSDK()
            {
                string SVGenicamGentl = null;
                string SVGenicamRoot = null;
                string SVGenicamCache = null;
                string SVCLProtocol = null;
                bool is64Env = IntPtr.Size == 8;

                // Check whether the environment variable exists.
                SVGenicamRoot = Environment.GetEnvironmentVariable("SVS_GENICAM_ROOT");
                if (SVGenicamRoot == null)
                {
                    Console.WriteLine("GetEnvironmentVariableA SVS_GENICAM_ROOT failed! ");
                    return false;
                }
                if (is64Env)
                {
                    SVGenicamGentl = Environment.GetEnvironmentVariable("GENICAM_GENTL64_PATH");
                    if (SVGenicamGentl == null)
                    {
                        Console.WriteLine("GetEnvironmentVariableA GENICAM_GENTL64_PATH failed! ");
                        return false;
                    }
                }
                else
                {
                    SVGenicamGentl = Environment.GetEnvironmentVariable("GENICAM_GENTL32_PATH");
                    if (SVGenicamGentl == null)
                    {
                        Console.WriteLine("GetEnvironmentVariableA GENICAM_GENTL32_PATH failed! ");
                        return false;
                    }
                }

                SVCLProtocol = Environment.GetEnvironmentVariable("SVS_GENICAM_CLPROTOCOL");
                if (SVCLProtocol == null)
                {
                    Console.WriteLine("GetEnvironmentVariableA SVS_GENICAM_CLPROTOCOL failed! ");
                    return false;
                }

                SVGenicamCache = Environment.GetEnvironmentVariable("SVS_GENICAM_CACHE");
                if (SVGenicamCache == null)
                {
                    Console.WriteLine("GetEnvironmentVariableA SVS_GENICAM_CACHE failed! ");
                    return false;
                }

                SVcamApi.SVSCamApiReturn ret = myApi.SVS_LibInit(SVGenicamGentl, SVGenicamRoot, SVGenicamCache, SVCLProtocol);

                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                {
                    Console.WriteLine("SVS_LibInit  failed! ");
                    return false;
                }

                return true;
            }

            public void deviceDiscovery()
            {
                InitSDK();
                SVcamApi.SVSCamApiReturn ret = SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS;
                //Open the System module
                UInt32 tlCount = 0;
                ret = myApi.SVS_LibSystemGetCount(ref tlCount);
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                {
                    Console.WriteLine("GetEnvironmentVariableA SVS_GENICAM_CACHE failed! ");
                    return;
                }

                uint timeout = 3000;
                bool bChanged = false;
                UInt32 numInterface = 0;


                // initialize device and get transport layer info

                for (UInt32 i = 0; i < tlCount; i++)
                {
                    SVcamApi._SV_TL_INFO pInfoOut = new SVcamApi._SV_TL_INFO();

                    ret = myApi.SVS_LibSystemGetInfo(i, ref pInfoOut);

                    if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                        continue;

                    string str = pInfoOut.tlType;
                    if (0 != string.Compare("CL", str))
                    {
                        sv_tl_info_list.Add(pInfoOut);

                        IntPtr sv_cam_sys_hdl = new IntPtr();
                        ret = myApi.SVS_LibSystemOpen(i, ref sv_cam_sys_hdl);

                        sv_cam_sys_hdl_list.Add(sv_cam_sys_hdl);
                        myApi.SVS_SystemUpdateInterfaceList(sv_cam_sys_hdl, ref bChanged, timeout);

                        ret = myApi.SVS_SystemGetNumInterfaces(sv_cam_sys_hdl, ref numInterface);
                        for (uint j = 0; j < numInterface; j++)
                        {

                            uint interfaceIdSize = 0;

                            string interfaceId = null;
                            interfaceIdSize = 512;
                            //Queries the ID of the interface at iIndex in the internal interface list .
                            ret = myApi.SVS_SystemGetInterfaceId(sv_cam_sys_hdl, j, ref interfaceId, ref interfaceIdSize);

                            if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                                continue;


                            SVcamApi._SV_INTERFACE_INFO interfaceInfo = new SVcamApi._SV_INTERFACE_INFO();
                            ret = myApi.SVS_SystemInterfaceGetInfo(sv_cam_sys_hdl, interfaceId, ref interfaceInfo);


                            if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                                continue;
                            sv_interface_info_list.Add(interfaceInfo);


                            // Queries the information about the interface on this System module
                            IntPtr hInterface = IntPtr.Zero;
                            ret = myApi.SVS_SystemInterfaceOpen(sv_cam_sys_hdl, interfaceId, ref hInterface);
                            if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                                continue;

                            sv_interface_hdl_list.Add(hInterface);


                            //Updates the internal list of available devices on this interface.
                            ret = myApi.SVS_InterfaceUpdateDeviceList(hInterface, ref bChanged, timeout);
                            if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                                continue;
                            //Queries the number of available devices on this interface
                            UInt32 numDevices = 0;
                            ret = myApi.SVS_InterfaceGetNumDevices(hInterface, ref numDevices);
                            if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                                continue;


                            // Get device info for all available devices and add new device to the camera list.
                            for (UInt32 k = 0; k < numDevices; k++)
                            {
                                string deviceId = null;
                                uint deviceIdSize = 512;
                                ret = myApi.SVS_InterfaceGetDeviceId(hInterface, k, ref deviceId, ref deviceIdSize);
                                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                                    continue;

                                SVcamApi._SV_DEVICE_INFO devInfo = new SVcamApi._SV_DEVICE_INFO();
                                ret = myApi.SVS_InterfaceDeviceGetInfo(hInterface, deviceId, ref devInfo);

                                Camera cam = new Camera(devInfo, myApi);
                                Camlist.Add(cam);
                                sv_interface_hdl_list.Add(hInterface);

                            }
                        }
                    }
                }
            }

            public void closeCameracontainer()
            {
                if (Camlist.Count == 0)
                    return;


                for (int j = 0; j < Camlist.Count; j++)
                {
                    Camera cam = Camlist.ElementAt(j);
                    cam.acquisitionStop();
                    cam.StreamingChannelClose();
                    cam.closeConnection();
                    cam.featureInfolist.Clear();
                }


                for (int j = 0; j < sv_interface_hdl_list.Count; j++)
                    myApi.SVS_InterfaceClose(sv_interface_hdl_list.ElementAt(j));

                for (int j = 0; j < sv_cam_sys_hdl_list.Count; j++)
                    myApi.SVS_SystemClose(sv_cam_sys_hdl_list.ElementAt(j));
                myApi.SVS_LibClose();

            }

        }
        //----------------------------------------------------------------------------------------------------

        public Thread acqThread;
        bool acqThreadIsRuning = false;
        public string CurrentID = "";
        private Graphics gpanel;
        delegate void SetStatusCallBack();
        delegate void SetdisplayCallBack();
        delegate void treeUpdateCallBack();


        treeUpdateCallBack treeUpdate = null;
        // SetdisplayCallBack set_to_display = null;


        string feature_info = null;
        public int curentCamIndex = 0;
        private Rectangle outRectangle;
        Cameracontainer SVSCam = new Cameracontainer();
        public Camera current_selected_cam = null;
        public Bitmap[] display_img_rgb = new Bitmap[4];
        public Bitmap[] display_img_mono = new Bitmap[4];
        private bool newsize = false;
        private FrameReceivedHandler m_frh;

        //---------------------------intialisation and discovery----------------------------------------------------------

        public SVCamGrabber()
        {
            // InitializeComponent();
            /// SVSCam.InitSDK();

            //outRectangle = new Rectangle(0, 0, this.display.Width, display.Height);
            //gpanel = display.CreateGraphics();
        }

        //private void buttonDiscover_Click(object sender, EventArgs e)
        //{

        //    this.buttonDiscover.Cursor = Cursors.WaitCursor;
        //    this.textBox_Result.Text = "Searching for cameras...";
        //    this.textBox_Result.Refresh();

        //    SVSCam.deviceDiscovery();

        //    int number = SVSCam.Camlist.Count;
        //    for (int j = 0; j < SVSCam.Camlist.Count; j++)
        //    {
        //        string camInf = SVSCam.Camlist.ElementAt(j).devInfo.model + "  SN: " + SVSCam.Camlist.ElementAt(j).devInfo.serialNumber;
        //        this.CamSelectComboBox.Items.Add(camInf);
        //    }

        //    if (number > 0)
        //    {
        //        buttonDiscover.Enabled = false;

        //        StringBuilder text = new StringBuilder();
        //        text.AppendFormat("Found {0} cameras !", number);
        //        this.textBox_Result.Text = text.ToString();
        //        this.textBox_Result.ForeColor = Color.Green;
        //    }
        //    else
        //    {

        //        this.textBox_Result.Text = "No cameras found!";
        //        this.textBox_Result.ForeColor = Color.Red;
        //    }

        //    this.buttonDiscover.Cursor = Cursors.Default;
        //}

        //private void CamComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    clearControl();


        //    buttonStop_Click(this, null);
        //public void openCamera()


        //{

            
        //    //find svs cam
        //    for (int i = 0; i < SVSCam.Camlist.Count - 1; i++)
        //    {
        //        //SVSCam.Camlist.ElementAt(i).
        //    }
        //    current_selected_cam = SVSCam.Camlist.ElementAt(0);
        //    current_selected_cam.openConnection();
        //}
        public void openCamera(int i)


        {


            //find svs cam

            current_selected_cam = SVSCam.Camlist.ElementAt(i);
            current_selected_cam.openConnection();
            

        }

        public void closeCamera()
        {
            current_selected_cam.closeConnection();
        }
        public  List<Camera> getCameraList()
        {
            SVSCam.deviceDiscovery();
            return SVSCam.Camlist;
                
        
        } 

        //    if (current_selected_cam.is_opened)
        //    {
        //        start_UpdateViewTree(current_selected_cam);
        //        featureTreeView.Enabled = true;
        //        curentCamIndex = CamSelectComboBox.SelectedIndex;
        //        return;
        //    }

        //    current_selected_cam.openConnection();
        //    if (!current_selected_cam.is_opened)
        //    {
        //        CamSelectComboBox.SelectedIndex = curentCamIndex;
        //        return;

        //    }
        //    curentCamIndex = CamSelectComboBox.SelectedIndex;
        //    current_selected_cam.is_opened = true;
        //    current_selected_cam.StreamingChannelOpen();
        //    featureTreeView.Enabled = true;

        //    start_UpdateViewTree(current_selected_cam);

        //}


        //----------------------     Camera Acquisition control------------------------------------------------------------

        //private void buttonStart_Click(object sender, EventArgs e)
        //{
        //    if (SVSCam.Camlist.Count != 0)
        //    {
        //        if (current_selected_cam == null)
        //            return;
        //        buttonStart.Cursor = Cursors.WaitCursor;


        //        clearControl();
        //        gpanel.Clear(Color.Black);

        //        if (current_selected_cam.bufferInfoDest.pImagePtr != IntPtr.Zero)
        //        {
        //            Marshal.FreeHGlobal(current_selected_cam.bufferInfoDest.pImagePtr);
        //            current_selected_cam.bufferInfoDest.pImagePtr = IntPtr.Zero;
        //        }
        //        if (current_selected_cam.bufferInfoDest.pImagePtr != IntPtr.Zero)
        //        {
        //            Marshal.FreeHGlobal(current_selected_cam.bufferInfoDest.pImagePtr);
        //            current_selected_cam.bufferInfoDest.pImagePtr = IntPtr.Zero;
        //        }


        //        current_selected_cam.acquisitionStrart(4);




        //        if (!acqThreadIsRuning)
        //            startAcquisitionThread();

        //        buttonStop.Visible = true;
        //        buttonStart.Visible = false;
        //        buttonStart.Cursor = Cursors.Default;

        //        updateViewTree();
        //        buttonStart.Cursor = Cursors.Default;
        //    }

        //}
        //private void buttonStop_Click(object sender, EventArgs e)
        //{
        //    if (current_selected_cam == null)
        //        return;

        //    buttonStop.Cursor = Cursors.WaitCursor;

        //    clearControl();

        //    current_selected_cam.threadIsRuning = false;
        //    acqThreadIsRuning = false;

        //    acqThread.Join();

        //    current_selected_cam.acquisitionStop();


        //    buttonStop.Visible = false;
        //    buttonStart.Visible = true;
        //    updateViewTree();
        //    buttonStop.Cursor = Cursors.Default;

        //}

        public void startAcquisitionThread(FrameReceivedHandler frh)
        {

            try
            {
                SVcamApi.SVSCamApiReturn ret;
                ret = current_selected_cam.openConnection();
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return;
                m_frh = frh;
                ret = current_selected_cam.StreamingChannelOpen();
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)

                {
                    Console.WriteLine("could not open channel {0}", ret);
                    current_selected_cam.closeConnection();
                    return;
                }

                current_selected_cam.acquisitionStart(1, frh);

                acqThreadIsRuning = true;
                acqThread = new Thread(new ThreadStart(acqTHreadTriggerWidth));
                acqThread.Start();
            }
            catch
            {
                acqThreadIsRuning = false;
                Console.WriteLine("problem starting acquisition Thread");
            }
        }
        public void startAcquisitionThreadForDarks(FrameReceivedHandler frh)
        {

            try
            {
                SVcamApi.SVSCamApiReturn ret;
                ret = current_selected_cam.openConnection();
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                    return;
                m_frh = frh;
                ret = current_selected_cam.StreamingChannelOpen();
                if (ret != SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)

                {
                    Console.WriteLine("could not open channel {0}", ret);
                    current_selected_cam.closeConnection();
                    return;
                }

                current_selected_cam.acquisitionStart(1, frh);

                acqThreadIsRuning = true;
                acqThread = new Thread(new ThreadStart(acqTHread_darks));
                acqThread.Start();
            }
            catch
            {
                acqThreadIsRuning = false;
                Console.WriteLine("problem starting acquisition Thread");
            }
        }
        public void killCapture()
        {
            //lost control so kill process
            Console.WriteLine("KILLING CAPTURE!!!");
            try {
                acqThread.Abort();
                Console.WriteLine("killed thread");
                //current_selected_cam.closeConnection();
                Console.WriteLine("closed connection");
            }
            catch { }
        }
        public void stopAcquisitionThread()
        {

            acqThreadIsRuning = false;

            //acqThread.Abort();

            //if (current_selected_cam.bufferInfoDest.pImagePtr != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(current_selected_cam.bufferInfoDest.pImagePtr);
            //    current_selected_cam.bufferInfoDest.pImagePtr = IntPtr.Zero;
            //}
            //if (current_selected_cam.bufferInfoDest.pImagePtr != IntPtr.Zero)
            //{
            //    Marshal.FreeHGlobal(current_selected_cam.bufferInfoDest.pImagePtr);
            //    current_selected_cam.bufferInfoDest.pImagePtr = IntPtr.Zero;
            //}










        }
        public void startAcquisitionThread_darks()
        {
            current_selected_cam.is_opened = true;
            current_selected_cam.StreamingChannelOpen();
            if (current_selected_cam.bufferInfoDest.pImagePtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(current_selected_cam.bufferInfoDest.pImagePtr);
                current_selected_cam.bufferInfoDest.pImagePtr = IntPtr.Zero;
            }
            if (current_selected_cam.bufferInfoDest.pImagePtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(current_selected_cam.bufferInfoDest.pImagePtr);
                current_selected_cam.bufferInfoDest.pImagePtr = IntPtr.Zero;
            }


           // current_selected_cam.acquisitionStart(1);





            acqThreadIsRuning = true;
            acqThread = new Thread(new ThreadStart(acqTHread_darks));
            acqThread.Start();
        }
        public void prepareCameraForTriggerWidth(Camera cam)
        {
            IntPtr phFeature = IntPtr.Zero;
            
            SVCamApi.SVcamApi.SVSCamApiReturn ret;

            phFeature = IntPtr.Zero;
            int pValue = 0;
            string subFeatureName = null;

            cam.featureInfolist = new Queue<SVcamApi._SVCamFeaturInf>();

            SVcamApi._SVCamFeaturInf info = new SVcamApi._SVCamFeaturInf();
            //    ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "PayloadSize", ref phFeature);
            //cam.getFeatureValue(info.hFeature, ref info);
            //ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            ////setup pixel depth
            //ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "PixelFormat", ref phFeature);
            //ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            //cam.getFeatureValue(info.hFeature, ref info);

            cam.featureInfolist.Clear();

            cam.featureInfolist = new Queue<SVcamApi._SVCamFeaturInf>();


            // ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            // ret = SVSCam.myApi.SVS_FeatureListRefresh(cam.hRemoteDevice);
            cam.getDeviceFeatureList(SVcamApi.SV_FEATURE_VISIBILITY.SV_Guru);


            for (int j = 1; j < cam.featureInfolist.Count; j++)
            {
                if (cam.featureInfolist.ElementAt(j).SVFeaturInf.displayName == "Pixel Format")
                {
                    ret = SVSCam.myApi.SVS_FeatureEnumSubFeatures(cam.hRemoteDevice, cam.featureInfolist.ElementAt(j).hFeature, 3, ref subFeatureName, 512, ref pValue);
                    ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, cam.featureInfolist.ElementAt(j).hFeature, pValue);
                    Console.WriteLine("set pixel format");
                    
                }
                if (cam.featureInfolist.ElementAt(j).SVFeaturInf.displayName == "PayloadSize")
                {
                    Console.WriteLine("payloadsize");
                }
            }

            cam.featureInfolist = new Queue<SVcamApi._SVCamFeaturInf>();
            ret = SVSCam.myApi.SVS_FeatureListRefresh(cam.hRemoteDevice);
            Application.DoEvents();
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "PayloadSize", ref phFeature);
            cam.getFeatureValue(phFeature, ref info);

            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "TriggerActivation", ref phFeature);
            Console.WriteLine(ret);
            ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);

            //ret =SVSCam.myApi.SVS_FeatureSetValueString(cam.hRemoteDevice,phFeature, "Rising Edge");
            ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature, 0);//rising edge
            Console.WriteLine(ret);
            //set packet delay
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "GevSCPD", ref phFeature);
            ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            ret = SVSCam.myApi.SVS_FeatureSetValueInt64(cam.hRemoteDevice, phFeature, 1000);//packet delay
            //turn on triggerwidth
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "ExposureMode", ref phFeature);
            ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature, 1);//triggerWidth
            phFeature = IntPtr.Zero;
            //turn trigger mode on
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "TriggerMode", ref phFeature);
            ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature, 1);//On
            phFeature = IntPtr.Zero;
            //turn trigger source
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "TriggerSource", ref phFeature);
            ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature, 1);//Line1
            phFeature = IntPtr.Zero;

            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "PayloadSize", ref phFeature);
            ret = SVSCam.myApi.SVS_FeatureGetInfo(cam.hRemoteDevice, phFeature, ref info.SVFeaturInf);
            cam.featureInfolist.Clear();

            cam.featureInfolist = new Queue<SVcamApi._SVCamFeaturInf>();

        }

        public void acqTHread_darks()
        {

            //Bitmap[] darks = new Bitmap[10];
            Camera cam = this.current_selected_cam;
            int numDarks = 10;

            int pDestLength = 0;
            prepareCameraForTriggerWidth(cam);
            for (int d = 0; d < numDarks; d++)
            {
                //take pic
                expose(cam);
                //
                if (!cam.grab())
                    continue;
                Console.WriteLine("inside while loop");

                // Check if a RGB image( Bayer buffer format) arrived
                bool isImgRGB = false;
                pDestLength = (int)(cam.bufferInfoDest.iImageSize);
                int sizeX = (int)cam.bufferInfoDest.iSizeX;
                int sizeY = (int)cam.bufferInfoDest.iSizeY;

                CurrentID = Convert.ToString(cam.bufferInfoDest.iImageId);

                if (((int)cam.bufferInfoDest.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_ID_MASK) >= 8)
                {
                    isImgRGB = true;
                    // pDestLength = 3 * pDestLength / 2;
                }
                if (!isImgRGB)
                    isImgRGB = false;

                //this.initializeBuffer(isImgRGB, sizeX, sizeY);
                cam.imageSizeX = sizeX;
                cam.imageSizeY = sizeY;
                cam.imagebufferDark[d].imagebytes = new byte[pDestLength];
                Marshal.Copy(cam.bufferInfoDest.pImagePtr, cam.imagebufferDark[d].imagebytes, 0, pDestLength);


                cam.isrgb = isImgRGB;
                //

            } //collected all dark frames into byte arrays
            cam.masterDark = new byte[pDestLength];
            for (int pxl = 0; pxl < pDestLength; pxl++)
            {
                int totalPixel = 0;
                for (int d = 0; d < numDarks; d++)
                {
                    totalPixel = totalPixel + cam.imagebufferDark[d].imagebytes[pxl];
                  
                }
                cam.masterDark[pxl] = (byte)(totalPixel / numDarks);
            }




            //write byte array to file






            //string folderName = string.Format("{0:yyyy-MMM-dd}", DateTime.Now);
            //System.IO.Directory.CreateDirectory(Path.Combine("e:\\image", folderName));
            //Console.WriteLine("SVS Vistek: saving master darks");
            //File.WriteAllBytes(Path.Combine("e:\\image", folderName, "masterDark.raw"), cam.masterDark);
            File.WriteAllBytes( "masterSVSDark.raw", cam.masterDark);
            Console.WriteLine("done with darks");

        }
        public void acqTHread()
        {
            Console.WriteLine("starting acqThread");
            Camera cam = this.current_selected_cam;
            while (acqThreadIsRuning)
            {
                if (!cam.grab())
                {
                    stopAcquisitionThread();
                    Console.WriteLine("stopped acquisition");
                   
                    startAcquisitionThread(m_frh);
                    Console.WriteLine("called start acquisition");
                    return;

                } 
                else
                { 
                Console.WriteLine("inside while loop");

                // Check if a RGB image( Bayer buffer format) arrived
                bool isImgRGB = false;
                int pDestLength = (int)(cam.bufferInfoDest.iImageSize);
                int sizeX = (int)cam.bufferInfoDest.iSizeX;
                int sizeY = (int)cam.bufferInfoDest.iSizeY;

                CurrentID = Convert.ToString(cam.bufferInfoDest.iImageId);

                if (((int)cam.bufferInfoDest.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_ID_MASK) >= 8)
                {
                    isImgRGB = true;
                    pDestLength = 3 * pDestLength;
                }
                if (!isImgRGB)
                    isImgRGB = false;

                this.initializeBuffer(isImgRGB, sizeX, sizeY);
                cam.imageSizeX = sizeX;
                cam.imageSizeY = sizeY;
                cam.addnewImageData2(cam.bufferInfoDest, isImgRGB);
                cam.isrgb = isImgRGB;
                    //
                }


            }
            Console.WriteLine("acqThreadIsRuning no more");
            //try to restart camera
            //current_selected_cam.is_opened = false;
            //Console.WriteLine("closing channel");
            //current_selected_cam.StreamingChannelClose();
            ////close camera
            //int i = curentCamIndex;
            //Console.WriteLine("closing connection");
            //current_selected_cam.closeConnection();
            current_selected_cam.acquisitionStop();
            current_selected_cam.is_opened = false;
            current_selected_cam.StreamingChannelClose();


            //openCamera(curentCamIndex);
            //
            //current_selected_cam.StreamingChannelClose();
            //current_selected_cam.StreamingChannelOpen();



        }
        public enum Trigger_mode
        {
            sv_rising = 0, 
            sv_falling = 2
        }
        public void expose(Camera cam)
        {
            SVCamApi.SVcamApi.SVSCamApiReturn ret;
            IntPtr phFeature = IntPtr.Zero;
            //cam.getFeatureValue(hFeature, hInfo);
            ret = SVSCam.myApi.SVS_FeatureGetByName(cam.hRemoteDevice, "TriggerActivation", ref phFeature);
            ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature, 1);//falling edge
            if (ret == SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
            {
                Console.WriteLine("falling edge");
            }
            else
            {
                Console.WriteLine(ret);
            }

            Thread.Sleep(cam.duration / 1000);
            //trigger off
            ret = SVSCam.myApi.SVS_FeatureSetValueInt64Enum(cam.hRemoteDevice, phFeature, 0);//rising edge
            if (ret == SVcamApi.SVSCamApiReturn.SV_ERROR_SUCCESS)
                Console.WriteLine("rising edge");
        }
        public void acqTHreadTriggerWidth()
        {
            Console.WriteLine("starting acqThread trigger width");


            IntPtr phFeature = IntPtr.Zero;
            Camera cam = current_selected_cam;
            SVCamApi.SVcamApi.SVSCamApiReturn ret;
            
            while (acqThreadIsRuning)
            {
                //trigger on
                expose(cam);
                if (!cam.grab())
                {
                    stopAcquisitionThread();
                    Console.WriteLine("stopped acquisition");

                    startAcquisitionThread(m_frh);
                    Console.WriteLine("called start acquisition");
                    return;

                }
                else
                {
                    Console.WriteLine("inside while loop");

                    // Check if a RGB image( Bayer buffer format) arrived
                    bool isImgRGB = false;
                    int pDestLength = (int)(cam.bufferInfoDest.iImageSize);
                    int sizeX = (int)cam.bufferInfoDest.iSizeX;
                    int sizeY = (int)cam.bufferInfoDest.iSizeY;

                    CurrentID = Convert.ToString(cam.bufferInfoDest.iImageId);

                    if (((int)cam.bufferInfoDest.iPixelType & SVCamApi.SVcamApi.DefineConstants.SV_GVSP_PIX_ID_MASK) >= 8)
                    {
                        isImgRGB = true;
                        pDestLength = 3 * pDestLength;
                    }
                    if (!isImgRGB)
                        isImgRGB = false;

                    this.initializeBuffer(isImgRGB, sizeX, sizeY);
                    cam.imageSizeX = sizeX;
                    cam.imageSizeY = sizeY;
                    cam.addnewImageData2(cam.bufferInfoDest, isImgRGB);
                    cam.isrgb = isImgRGB;
                    //
                }


            }
            Console.WriteLine("acqThreadIsRuning no more");
            //try to restart camera
            //current_selected_cam.is_opened = false;
            //Console.WriteLine("closing channel");
            //current_selected_cam.StreamingChannelClose();
            ////close camera
            //int i = curentCamIndex;
            //Console.WriteLine("closing connection");
            //current_selected_cam.closeConnection();
            current_selected_cam.acquisitionStop();
            current_selected_cam.is_opened = false;
            current_selected_cam.StreamingChannelClose();
            current_selected_cam.closeConnection();


            //openCamera(curentCamIndex);
            //
            //current_selected_cam.StreamingChannelClose();
            //current_selected_cam.StreamingChannelOpen();



        }

        private void initializeBuffer(bool rgb, int camWidth, int camHeight)
        {
            newsize = false;
            int k;
            if (current_selected_cam == null)
                return;
            if (rgb)
            {


                if (current_selected_cam.imagebufferRGB[0].dataLegth != 3 * camWidth * camHeight)
                    newsize = true;

                for (k = 0; k < 4; k++)
                {
                    unsafe
                    {
                        if (newsize)
                            current_selected_cam.imagebufferRGB[k].imagebytes = new byte[3 * camWidth * camHeight];

                        fixed (byte* ColorPtr = current_selected_cam.imagebufferRGB[k].imagebytes)
                        {
                            if (newsize)
                                display_img_rgb[k] = new Bitmap(camWidth, camHeight, (3 * camWidth), System.Drawing.Imaging.PixelFormat.Format24bppRgb, (IntPtr)ColorPtr);
                            current_selected_cam.imagebufferRGB[k].sizeX = camWidth;
                            current_selected_cam.imagebufferRGB[k].sizeY = camHeight;
                            current_selected_cam.imagebufferRGB[k].dataLegth = 3 * camWidth * camHeight;
                        }
                    }
                }
            }

            else
            {

                if (current_selected_cam.imagebufferMono[0].dataLegth != camWidth * camHeight)
                    newsize = true;

                for (k = 0; k < 4; k++)
                {
                    unsafe
                    {
                        if (newsize)
                            current_selected_cam.imagebufferMono[k].imagebytes = new byte[camWidth * camHeight];

                        fixed (byte* MonoPtr = current_selected_cam.imagebufferMono[k].imagebytes)
                        {
                            if (newsize)
                                display_img_mono[k] = new Bitmap(camWidth, camHeight, camWidth, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, (IntPtr)MonoPtr);

                            current_selected_cam.imagebufferMono[k].sizeX = camWidth;
                            current_selected_cam.imagebufferMono[k].sizeY = camHeight;
                            current_selected_cam.imagebufferMono[k].dataLegth = camWidth * camHeight;
                        }
                    }
                }
            }
        }

        //private void setTodisplay()
        //{

        //    if (!acqThreadIsRuning)
        //        return;


        //    {

        //        int currentIndex = this.current_selected_cam.destdataIndex;


        //        if (current_selected_cam.isrgb)
        //        {
        //            if (display_img_rgb[currentIndex] != null)
        //            {
        //                // Bitmap resized = null;
        //                double dbl = display_img_rgb[currentIndex].Width / (double)display_img_rgb[currentIndex].Height;
        //                if (dbl < display.Width)
        //                {
        //                    if ((int)((double)display.Height * dbl) <= display.Width)
        //                    {
        //                        outRectangle.Width = (int)((double)display.Height * dbl);
        //                        outRectangle.Height = display.Height;

        //                        // resized = new Bitmap(display_img_rgb[currentIndex], (int)((double)display.Height * dbl), display.Height);
        //                    }
        //                    else
        //                    {
        //                        outRectangle.Width = display.Width;
        //                        outRectangle.Height = (int)((double)display.Width / dbl);
        //                        // resized = new Bitmap(display_img_rgb[currentIndex], display.Width, (int)((double)display.Width / dbl));
        //                    }
        //                }

        //                gpanel.DrawImage(display_img_rgb[currentIndex], outRectangle);
        //            }
        //        }

        //        else
        //        {
        //            if (display_img_mono[currentIndex] != null)
        //            {
        //                //Bitmap resized = null;
        //                double dbl = display_img_mono[currentIndex].Width / (double)display_img_mono[currentIndex].Height;
        //                System.Drawing.Imaging.ColorPalette imgpal = display_img_mono[currentIndex].Palette;

        //                // Build bitmap palette Y8
        //                for (uint i = 0; i < 256; i++)
        //                {
        //                    imgpal.Entries[i] = Color.FromArgb(
        //                    (byte)0xFF,
        //                    (byte)i,
        //                    (byte)i,
        //                    (byte)i);
        //                }
        //                display_img_mono[currentIndex].Palette = imgpal;
        //                imgpal = display_img_mono[currentIndex].Palette;
        //                if (dbl < display.Width)
        //                {
        //                    if ((int)((double)display.Height * dbl) <= display.Width)
        //                    {
        //                        outRectangle.Width = (int)((double)display.Height * dbl);
        //                        outRectangle.Height = display.Height;
        //                    }
        //                    else
        //                    {
        //                        outRectangle.Width = (int)((double)display.Height * dbl);
        //                        outRectangle.Height = display.Height;

        //                    }

        //                }

        //                gpanel.DrawImage(display_img_mono[currentIndex], outRectangle);
        //            }
        //        }

        //        this.current_selected_cam.destdataIndex++;
        //        if (this.current_selected_cam.destdataIndex == 4)
        //            this.current_selected_cam.destdataIndex = 0;
        //    }

        //}


        //------------------------- Camera Feature Tree control ------------------------------------------------------

        //private void start_UpdateViewTree(Camera cam)
        //{

        //    cam.featureInfolist.Clear();

        //    cam.featureInfolist = new Queue<SVcamApi._SVCamFeaturInf>();
        //    featureTreeView.Nodes.Clear();
        //    buttonICommand.Visible = false;

        //    hScrollbarIInteger.Visible = false;
        //    textBoxIInteger.Visible = false; ;
        //    textBoxIInteger.Clear();

        //    comboBoxIEnumeration.Visible = false;
        //    hScrollbarIFloat.Visible = false;
        //    textBoxIFloat.Visible = false;
        //    textBoxIString.Visible = false;
        //    textBoxIString.Clear();

        //    // get Feature list
        //    cam.getDeviceFeatureList(SVcamApi.SV_FEATURE_VISIBILITY.SV_Beginner);
        //    TreeNode node;
        //    string camInf = cam.devInfo.model + "   SN:  " + cam.devInfo.serialNumber;
        //    node = featureTreeView.Nodes.Add(camInf);
        //    node.ForeColor = (Color.ForestGreen);


        //    for (int j = 1; j < cam.featureInfolist.Count; j++)
        //    {

        //        string feature_info = (cam.featureInfolist.ElementAt(j).SVFeaturInf.displayName) + ": " + cam.featureInfolist.ElementAt(j).strValue;
        //        node = AddFeatureToTree(cam.featureInfolist.ElementAt(j), node, feature_info);
        //        object data = cam.featureInfolist.ElementAt(j);
        //        node.Tag = data;
        //        if ((cam.featureInfolist.ElementAt(j).SVFeaturInf.isLocked != 0 && cam.featureInfolist.ElementAt(j).SVFeaturInf.type != (int)SVcamApi.SV_FEATURE_TYPE.SV_intfICategory))
        //        {
        //            node.ForeColor = Color.Gray;
        //        }
        //        else
        //        {
        //            node.ForeColor = Color.Black;
        //        }
        //        node.ExpandAll();
        //    }

        //}


        //private void updateNodeItem(TreeNode itemNode)
        //{
        //    SVcamApi._SVCamFeaturInf featureInfo = (SVcamApi._SVCamFeaturInf)itemNode.Tag;
        //    Camera cam = current_selected_cam;
        //    cam.getFeatureValue(featureInfo.hFeature, ref featureInfo);
        //    itemNode.Tag = featureInfo;
        //    if (featureInfo.SVFeaturInf.isLocked != 0)
        //        itemNode.ForeColor = Color.Gray;
        //    else
        //        itemNode.ForeColor = Color.Black;
        //    feature_info = (featureInfo.SVFeaturInf.displayName) + ": " + featureInfo.strValue;
        //    itemNode.Text = feature_info;
        //}


        //private void clearControl()
        //{
        //    textBoxIInteger.Clear();
        //    textBoxIString.Clear();
        //    textBoxIFloat.Clear();
        //    FeatureTooltip.Clear();

        //    buttonICommand.Text = "";

        //    hScrollbarIInteger.Minimum = 0;
        //    hScrollbarIInteger.Maximum = 0;

        //    hScrollbarIFloat.Minimum = 0;
        //    hScrollbarIFloat.Maximum = 0;

        //    label5.Text = "";

        //    comboBoxIEnumeration.Items.Clear();
        //    textBoxGenApiFeature.Clear();

        //    textBoxIInteger.Visible = false;
        //    textBoxIString.Visible = false;
        //    textBoxIFloat.Visible = false;
        //    buttonICommand.Visible = false;
        //    hScrollbarIInteger.Visible = false;
        //    hScrollbarIFloat.Visible = false;
        //    label5.Visible = false;
        //    comboBoxIEnumeration.Visible = false;
        //}


        //private void updateViewTree()
        //{
        //    if (this.featureTreeView.InvokeRequired)
        //    {
        //        try
        //        {
        //            if (treeUpdate == null)
        //                treeUpdate = new treeUpdateCallBack(updateViewTree);
        //            featureTreeView.Invoke(treeUpdate);
        //        }
        //        catch
        //        {
        //            // Invoke Failed!!  
        //        }
        //    }
        //    else
        //    {
        //        start_UpdateViewTree(current_selected_cam);
        //    }
        //}


        //TreeNode AddFeatureToTree(SVcamApi._SVCamFeaturInf curentfeature, TreeNode tree, string featur_info)
        //{
        //    TreeNode par_tree = tree;
        //    if (curentfeature.SVFeaturInf.level > tree.Level)
        //    {
        //        tree.Nodes.Add(featur_info);
        //        return tree.LastNode;
        //    }
        //    while (curentfeature.SVFeaturInf.level < par_tree.Level)
        //    {
        //        par_tree = par_tree.Parent;
        //    }

        //    par_tree.Parent.Nodes.Add(featur_info);
        //    return par_tree.Parent.LastNode;
        //}


        //private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        //{
        //    if (featureTreeView.SelectedNode == null)
        //        return;

        //    object str = featureTreeView.SelectedNode.Tag;

        //    if (str == null)
        //        return;
        //    SVcamApi._SVCamFeaturInf selected_feature_inf = (SVcamApi._SVCamFeaturInf)str;


        //    clearControl();

        //    FeatureTooltip.Text = ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).SVFeaturInf.toolTip;
        //    textBoxGenApiFeature.Text = "Feature Name: " + selected_feature_inf.SVFeaturInf.name + Environment.NewLine;


        //    FeatureTooltip.Visible = true;

        //    if (selected_feature_inf.SVFeaturInf.isLocked != 0)
        //        return;

        //    if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIString == selected_feature_inf.SVFeaturInf.type)
        //    {
        //        textBoxIString.Text = selected_feature_inf.strValue;
        //        label5.Text = selected_feature_inf.SVFeaturInf.unit;
        //        label5.Visible = true;

        //        textBoxIString.Visible = true;
        //        textBoxGenApiFeature.AppendText("Feature Type :   intfIString");


        //    }
        //    else
        //        if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfICommand == selected_feature_inf.SVFeaturInf.type)
        //    {
        //        buttonICommand.Visible = true;
        //        buttonICommand.Text = selected_feature_inf.SVFeaturInf.displayName;
        //        buttonICommand.Tag = (selected_feature_inf.hFeature);

        //        string codesample = " string feature_name = " + "\"" + selected_feature_inf.SVFeaturInf.name + "\";";
        //        codesample = codesample.Trim();
        //        string codesample2 = " float value";
        //        codesample2.Trim();
        //        textBoxIString.Text = codesample + codesample2;

        //        label5.Text = selected_feature_inf.SVFeaturInf.unit;
        //        label5.Visible = true;

        //        buttonICommand.Enabled = true;
        //        textBoxGenApiFeature.AppendText("Feature Type :   intfICommand");
        //    }

        //    else if ((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIEnumeration == selected_feature_inf.SVFeaturInf.type)
        //    {
        //        for (int j = 0; j < selected_feature_inf.SVFeaturInf.enumCount; j++)
        //        {
        //            uint bufferSize = SVCamApi.SVcamApi.DefineConstants.SV_STRING_SIZE;
        //            char[] schstr = new char[bufferSize];
        //            string subFeatureName = new string(schstr);
        //            int pValue = 0;
        //            SVSCam.myApi.SVS_FeatureEnumSubFeatures(selected_feature_inf.hRemoteDevice, selected_feature_inf.hFeature, j, ref subFeatureName, bufferSize, ref pValue);
        //            comboBoxIEnumeration.Items.Add(subFeatureName);

        //            if (j == selected_feature_inf.SVFeaturInf.enumSelectedIndex)
        //                comboBoxIEnumeration.SelectedIndex = j;
        //        }

        //        label5.Text = selected_feature_inf.SVFeaturInf.unit;
        //        label5.Visible = true;
        //        comboBoxIEnumeration.Visible = true;

        //        textBoxGenApiFeature.AppendText("Feature Type :   intfIEnumeration");
        //    }

        //    else if (((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIInteger == selected_feature_inf.SVFeaturInf.type))
        //    {


        //        hScrollbarIInteger.Minimum = (int)selected_feature_inf.SVFeaturInf.intMin;
        //        hScrollbarIInteger.SmallChange = (int)selected_feature_inf.SVFeaturInf.intInc;

        //        if (selected_feature_inf.SVFeaturInf.intMax > 1000000)
        //            hScrollbarIInteger.Maximum = 1000000;
        //        else
        //            hScrollbarIInteger.Maximum = (int)selected_feature_inf.SVFeaturInf.intMax;


        //        if (selected_feature_inf.SVFeaturInf.intMax > 0)
        //        {
        //            textBoxIInteger.Text = selected_feature_inf.strValue;
        //            textBoxIInteger_Leave(this, null);
        //            label5.Text = selected_feature_inf.SVFeaturInf.unit;
        //            if (hScrollbarIInteger.Maximum > 0)
        //                hScrollbarIInteger.Visible = true;
        //            textBoxIInteger.Visible = true;
        //            hScrollbarIInteger.Visible = true;
        //            textBoxIInteger.Visible = true;
        //            label5.Visible = true;
        //        }

        //        textBoxGenApiFeature.AppendText("Feature Type :   intfIInteger");

        //    }

        //    else if ((((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIFloat == selected_feature_inf.SVFeaturInf.type)))
        //    {

        //        hScrollbarIFloat.Minimum = (int)selected_feature_inf.SVFeaturInf.floatMin * 100;

        //        double scrollrange = selected_feature_inf.SVFeaturInf.floatMax * 100;

        //        if (scrollrange > 10000000)
        //            scrollrange = 10000000;


        //        hScrollbarIFloat.Maximum = (int)scrollrange;
        //        textBoxIFloat.Text = selected_feature_inf.strValue;
        //        textBoxIFloat_Leave(this, null);

        //        if (hScrollbarIFloat.Maximum > 0)
        //            hScrollbarIFloat.Visible = true;
        //        textBoxIFloat.Visible = true;
        //        hScrollbarIFloat.Visible = true;
        //        textBoxIFloat.Visible = true;

        //        textBoxGenApiFeature.AppendText("Feature Type :   intfIFloat");
        //    }
        //}


        //private void update_tree(object sender, EventArgs e)
        //{
        //    clearControl();
        //    updateViewTree();
        //}


        //private void hScrollbarIInteger_Scroll(object sender, ScrollEventArgs e)
        //{
        //    SVSCam.myApi.SVS_FeatureSetValueInt64(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, hScrollbarIInteger.Value);
        //    textBoxIInteger.Text = Convert.ToString(hScrollbarIInteger.Value);
        //    updateNodeItem(featureTreeView.SelectedNode);
        //}


        //private void buttonICommand_Click(object sender, EventArgs e)
        //{
        //    SVSCam.myApi.SVS_FeatureCommandExecute(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, 1000);
        //    updateNodeItem(featureTreeView.SelectedNode);
        //}


        //private void textBoxIFloat_Leave(object sender, EventArgs e)
        //{
        //    double value = 0;
        //    bool success = Double.TryParse(textBoxIFloat.Text, out value);
        //    double scrollpos = value * 100;
        //    if (success)
        //    {
        //        if (hScrollbarIFloat.Maximum > 0)
        //        {
        //            if (scrollpos > hScrollbarIFloat.Maximum)
        //                scrollpos = hScrollbarIFloat.Maximum;

        //            if (scrollpos < hScrollbarIFloat.Minimum)
        //                scrollpos = hScrollbarIFloat.Minimum;

        //            hScrollbarIFloat.Value = (int)scrollpos;
        //            hScrollbarIFloat_MouseLeave(this, null);
        //        }
        //        else
        //            SVSCam.myApi.SVS_FeatureSetValueFloat(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, value);

        //        updateNodeItem(featureTreeView.SelectedNode);
        //    }

        //    return;
        //}


        //private void textBoxIFloat_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        textBoxIFloat_Leave(sender, e);
        //    }
        //}

        //private void textBoxIInteger_Leave(object sender, EventArgs e)
        //{
        //    int value = 0;
        //    bool success = Int32.TryParse(textBoxIInteger.Text, out value);
        //    int scrollpos = value;
        //    if (success)
        //    {
        //        if (hScrollbarIInteger.Maximum > 0)
        //        {
        //            if (scrollpos > hScrollbarIInteger.Maximum)
        //                scrollpos = hScrollbarIInteger.Maximum;

        //            if (scrollpos < hScrollbarIInteger.Minimum)
        //                scrollpos = hScrollbarIInteger.Minimum;

        //            hScrollbarIInteger.Value = scrollpos;
        //            hScrollbarIInteger_Scroll(this, null);
        //        }
        //        else
        //            SVSCam.myApi.SVS_FeatureSetValueInt64(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, value);
        //        updateNodeItem(featureTreeView.SelectedNode);
        //    }
        //    return;
        //}


        //private void textBoxIInteger_KeyDown(object sender, KeyEventArgs e)
        //{

        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        textBoxIInteger_Leave(sender, e);
        //    }

        //}


        //private void textBoxIString_Leave(object sender, EventArgs e)
        //{
        //    string str = textBoxIString.Text;
        //    SVSCam.myApi.SVS_FeatureSetValueString(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, str);
        //    updateNodeItem(featureTreeView.SelectedNode);
        //}


        //private void textBoxIString_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        textBoxIString_Leave(sender, e);
        //    }
        //}


        //private void comboBoxIEnumeration_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int enumvalue = comboBoxIEnumeration.SelectedIndex;
        //    if (((int)SVcamApi.SV_FEATURE_TYPE.SV_intfIBoolean == ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).SVFeaturInf.type))
        //    {
        //        bool bvalue = (enumvalue == 0 ? false : true);
        //        SVSCam.myApi.SVS_FeatureSetValueBool(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, bvalue);
        //    }
        //    else
        //    {
        //        string subFeatureName = null;
        //        int pValue = 0;
        //        SVSCam.myApi.SVS_FeatureEnumSubFeatures(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, enumvalue, ref subFeatureName, 512, ref pValue);
        //        SVSCam.myApi.SVS_FeatureSetValueInt64Enum(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, pValue);
        //    }
        //    updateNodeItem(featureTreeView.SelectedNode);
        //}


        //´------free resources and close---------------------------------------------------------------
        //private void buttonQuit_Click(object sender, EventArgs e)
        //{
        //    buttonQuit.Cursor = Cursors.WaitCursor;

        //    if (current_selected_cam != null)
        //    {
        //        buttonStop_Click(null, null);
        //        SVSCam.closeCameracontainer();
        //        current_selected_cam = null;
        //    }
        //    buttonQuit.Cursor = Cursors.Default;
        //    this.Close();
        //}


        //private void shutdown(Object sender, FormClosingEventArgs e)
        //{
        //    //In case windows is trying to shut down, don't hold the process up
        //    if (e.CloseReason == CloseReason.WindowsShutDown) return;
        //    acqThreadIsRuning = false;
        //    SVSCam.closeCameracontainer();
        //}


        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    this.label6.Text = "imageID: " + CurrentID;
        //}

        //private void hScrollbarIFloat_MouseLeave(object sender, EventArgs e)
        //{
        //    double val = (double)hScrollbarIFloat.Value / 100;
        //    SVSCam.myApi.SVS_FeatureSetValueFloat(((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hRemoteDevice, ((SVcamApi._SVCamFeaturInf)featureTreeView.SelectedNode.Tag).hFeature, val);
        //    textBoxIFloat.Text = Convert.ToString(val);

        //    updateNodeItem(featureTreeView.SelectedNode);

        //}
    }
}


