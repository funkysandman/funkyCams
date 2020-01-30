using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Photometrics.Pvcam;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.IO;

namespace pvcam_helper
{


    public class CameraNotifications
    {
        public const int ACQ_SINGLE_STARTED = 0;
        public const int ACQ_SINGLE_READY = 1;
        public const int ACQ_SINGLE_FINISHED = 2;
        public const int ACQ_SINGLE_FAILED = 3;
        public const int ACQ_SINGLE_CANCELLED = 4;

        public const int ACQ_CONT_STARTED = 5;
        public const int ACQ_CONT_READY = 6;
        public const int ACQ_CONT_FINISHED = 7;
        public const int ACQ_CONT_FAILED = 8;
        public const int ACQ_CONT_CANCELED = 9;

        public const int ACQ_NEW_FRAME_RECEIVED = 10;

        public const int NO_CAMERA_FOUND = 11;
        public const int CAMERA_REFRESH_DONE = 12;
        public const int CAMERA_OPENED = 13;
        public const int CAMERA_PARAM_READOUT_COMPLETE = 14;

        public const int SPEED_TABLE_BUILD_DONE = 15;
        public const int READOUT_SPEED_CHANGED = 16;
    }

    //codes of message types for communication between camera and UI class
    public class MsgTypes
    {
        public const int MSG_STATUS = 0;
        public const int MSG_ERROR = 1;
    }

    //Example of structure pointer to which can be passed to a callback registration
    //function, the pointer to the same structure will then be passed to callback
    //once the callback event arrives.
    //Here we are letting PVCAM push the structure with binning and exposure time
    //to the callback function.
    struct AcqParamsContext
    {
        UInt32 exposureTime;
        UInt32 binning;

        public UInt32 Binning
        {
            get { return binning; }
            set { binning = value; }
        }
        public UInt32 ExposureTime
        {
            get { return exposureTime; }
            set { exposureTime = value; }
        }
    }

    //Speed table holds the numbers of available readout ports
    //and readout speeds and another structure with readout options
    class SpeedTable
    {
        UInt32 readoutPorts;
        UInt32 readoutSpeeds;
        List<ReadoutOption> readoutOption;

        public UInt32 ReadoutPorts
        {
            get { return readoutPorts; }
            set { readoutPorts = value; }
        }
        public UInt32 ReadoutSpeeds
        {
            get { return readoutSpeeds; }
            set { readoutSpeeds = value; }
        }
        internal List<ReadoutOption> ReadoutOption
        {
            get { return readoutOption; }
            set { readoutOption = value; }
        }

        public SpeedTable()
        {
            readoutOption = new List<ReadoutOption>();
        }
    }

    //each readout option (a combination of readout port and readout speed
    //is further characterized by its bit depth, number of available gain states
    //and a description
    //NOTE: most Interline/sCMOS cameras report descriptions of their ports as
    //"Multiplication gain" which is a known issue. Interline/sCMOS cameras don't have
    //multiplication gain, only the Frame Transfer cameras do have it.
    struct ReadoutOption
    {
        Int16 m_port;
        Int16 m_speed;
        Int16 m_bitDepth;
        UInt32 m_gainStates;
        String m_portDesc;

        public Int16 BitDepth
        {
            get { return m_bitDepth; }
            set { m_bitDepth = value; }
        }

        public UInt32 GainStates
        {
            get { return m_gainStates; }
            set { m_gainStates = value; }
        }

        public Int16 Port
        {
            get { return m_port; }
            set { m_port = value; }
        }

        public Int16 Speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        public string PortDesc
        {
            get { return m_portDesc; }
            set { m_portDesc = value; }
        }

        public ReadoutOption(Int16 portNr, Int16 speedNr, Int16 bD, UInt32 gS, string portDescription)
        {
            m_port = portNr;
            m_speed = speedNr;
            m_bitDepth = bD;
            m_gainStates = gS;
            m_portDesc = portDescription;
        }
    }

    //Name - value pair for Enumeration type parameters
    public struct NVP
    {
        Int32 m_value;
        String m_name;

        public Int32 Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
    }

    //Post processing fuctions structure
    public struct PP_Function
    {
        UInt32 m_id;
        String m_name;
        UInt32 m_minVal;
        UInt32 m_maxVal;
        UInt32 m_defVal;
        UInt32 m_currentVal;

        public UInt32 ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public UInt32 MinValue
        {
            get { return m_minVal; }
            set { m_minVal = value; }
        }

        public UInt32 MaxValue
        {
            get { return m_maxVal; }
            set { m_maxVal = value; }
        }

        public UInt32 DefValue
        {
            get { return m_defVal; }
            set { m_defVal = value; }
        }

        public UInt32 CurrentVal
        {
            get { return m_currentVal; }
            set { m_currentVal = value; }
        }
    }

    //Post processing feature structure, each feature could have numerous
    //functions
    public struct PP_Feature
    {
        UInt32 m_id;
        String m_name;
        List<PP_Function> m_functionList;

        public UInt32 ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public List<PP_Function> FunctionList
        {
            get { return m_functionList; }
            set { m_functionList = value; }
        }
    }

    //class holding the image statistics (mean, min, max)
    public class ImageStats
    {
        Int64 m_mean;
        Int32 m_min;
        Int32 m_max;

        public Int64 Mean
        {
            get { return m_mean; }
            set { m_mean = value; }
        }

        public Int32 Min
        {
            get { return m_min; }
            set { m_min = value; }
        }

        public Int32 Max
        {
            get { return m_max; }
            set { m_max = value; }
        }
    }
    //TO hold important Frame Metadata, add others as required
    public class FrameMetadata
    {
        UInt32 m_frameNr;
        UInt16 m_roiCount;
        UInt32 m_timeStampBOF;
        UInt32 m_timeStampEOF;
        UInt32 m_expTime;

        public UInt32 FrameNr
        {
            get { return m_frameNr; }
            set { m_frameNr = value; }
        }

        public UInt16 RoiCount
        {
            get { return m_roiCount; }
            set { m_roiCount = value; }
        }

        public UInt32 TimeStampBOF
        {
            get { return m_timeStampBOF; }
            set { m_timeStampBOF = value; }
        }

        public UInt32 TimeStampEOF
        {
            get { return m_timeStampEOF; }
            set { m_timeStampEOF = value; }
        }

        public UInt32 ExpTime
        {
            get { return m_expTime; }
            set { m_expTime = value; }
        }

    }

    //TO hold ROI metadata
    //TO hold important Frame Metadata, add others as required
    public class ROIMetadata
    {
        UInt32 m_roiNr;
        UInt16 m_s1;  //ROI Region
        UInt16 m_s2;
        UInt16 m_p1;
        UInt16 m_p2;

        UInt16 m_roiCount;
        UInt32 m_timeStampBOR;
        UInt32 m_timeStampEOR;

        public UInt32 ROINr
        {
            get { return m_roiNr; }
            set { m_roiNr = value; }
        }

        public UInt16 S1
        {
            get { return m_s1; }
            set { m_s1 = value; }
        }
        public UInt16 S2
        {
            get { return m_s2; }
            set { m_s2 = value; }
        }
        public UInt16 P1
        {
            get { return m_p1; }
            set { m_p1 = value; }
        }
        public UInt16 P2
        {
            get { return m_p2; }
            set { m_p2 = value; }
        }

        public UInt32 TimeStampBOR
        {
            get { return m_timeStampBOR; }
            set { m_timeStampBOR = value; }
        }

        public UInt32 TimeStampEOR
        {
            get { return m_timeStampEOR; }
            set { m_timeStampEOR = value; }
        }

    }

    //Structure to hold centroiding info, if available on the camera
    public struct Centroid
    {
        UInt16 m_count_min;
        UInt16 m_count_max;
        UInt16 m_count_current;
        UInt16 m_radius_min;
        UInt16 m_radius_max;
        UInt16 m_radius_current;


        public UInt16 MaxCount
        {
            get { return m_count_max; }
            set { m_count_max = value; }
        }

        public UInt16 MinCount
        {
            get { return m_count_min; }
            set { m_count_min = value; }
        }

        public UInt16 CurrentCount
        {
            get { return m_count_current; }
            set { m_count_current = value; }
        }

        public UInt16 MaxRadius
        {
            get { return m_radius_max; }
            set { m_radius_max = value; }
        }

        public UInt16 MinRadius
        {
            get { return m_radius_min; }
            set { m_radius_min = value; }
        }

        public UInt16 CurrentRadius
        {
            get { return m_radius_current; }
            set { m_radius_current = value; }
        }
    }

    //class holding description and parameters of currently opened camera
    //and a method to convert raw pixel data to a BMP image
    //this class also does all camera related operations - single and continuous
    //acquisitions and all parameter settings
    public class PVCamCamera
    {
        //External function memset 
        [DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr MemSet(IntPtr dest, int c, int byteCount);

        //codes of camera event notifications for the user interface
        public delegate void ReportHandler(PVCamCamera pvcc, ReportMessage rm);
        public event ReportHandler ReportMsg;

        public delegate void CameraNotificationsHandler(PVCamCamera pvcc, ReportEvent e);
        public event CameraNotificationsHandler CamNotif;
        public const Int32 RUN_UNTIL_STOPPED = 100000000;

        //Acquisition types
        public enum AcqTypes
        {
            ACQ_TYPE_SINGLE = 0,
            ACQ_TYPE_CONTINUOUS = 1
        }

        AcqTypes m_acqType;

        public AcqTypes AcqType
        {
            get { return m_acqType; }
            //read only - so no set method
        }

        //Pointers to data buffers
        UInt32 m_stream_size;       //Pixel data size 
        UInt32 m_full_frame_size;   //full frame size including metadata
        IntPtr m_pixel_stream;      //pointer to Pixel data buffer
        IntPtr m_full_frame;      //pointer to full frame buffer
        Int32 m_circBuffSize;
        IntPtr m_circBuffer;
        Int16 m_imageSizeX;       //Final Image size depends on multiROI/Centoiding etc., Used for display window and 
        Int16 m_imageSizeY;       //recomposing frame



        IntPtr m_latestFrameAddress;
        Int16 m_hCam;

        UInt16[] m_frameDataShorts;
        static Byte[] DataRGB;

        Int16 m_xSize;
        Int16 m_ySize;
        Int16 m_pixelSize;
        public static Int16 NrOfCameras;

        //ROI 
        PvTypes.RegionType[] m_region;       //Array of region tyoes
        Int16 m_maxROICount;                //Max ROI camera supports
        Int16 m_currentROICount;           //Current ROI count

        //Centroids
        Boolean m_isCentroidAvail;        //is centroid feature available on the camera
        Boolean m_isCentroid;             //is centroid feature currently enabled
        Centroid m_centroid;              //Structure to hold centroid info

        String m_openedCamName;
        Boolean m_fTCapable;
        Boolean m_acqRunning;
        UInt32 m_exposureTime;
        Int16 m_binning;
        Int16 m_speedTableIndex;
        Int16 m_ClearCycles;
        Int16 m_ClearModeIndex=0;
        Int16 m_ClockingModeIndex = 0;
        Int16 m_GainStateIndex = 0;
        Int16 m_EMGain = 1;
        public Int16 Binning
        {
            get { return m_binning; }

            set
            {
                m_binning = value;
            }
        }
        public Int16 EMGain
        {
            get { return m_EMGain; }

            set
            {
                m_EMGain = value;
            }
        }
        public Int16 GainStateIndex
        {
            get { return m_GainStateIndex; }

            set
            {
                m_GainStateIndex = value;
            }
        }
        public Int16 ClockingModeIndex
        {
            get { return m_ClockingModeIndex; }

            set
            {
                m_ClockingModeIndex = value;
            }
        }
        public Int16 ClearCycles
        {
            get { return m_ClearCycles; }
            
            set
            {
                m_ClearCycles = value;
            }
        }

        public Int16 ClearModeIndex
        {
            get { return m_ClearModeIndex; }

            set
            {
                m_ClearModeIndex = value;
            }
        }

        Int32 m_framesToGet;

        
        StringBuilder m_chipName;
        Int32 m_frameNumber;
        Bitmap m_lastBMP;
        Int16 m_bitDepth;
        Int32 m_triggerMode;
        Int32 m_exposeOutMode;
        List<Double> m_readoutSpeeds;
        volatile Boolean m_abortAcquisition;
        Object m_bmpLock;
        GCHandle m_bmpHandle;
        Thread m_SingleAcqThread;
        PvTypes.PMCallBack_Ex3 m_EOFHandlerDelegate;
        Thread m_FrameDispThread;
        SpeedTable m_spdTable;
        PvTypes.FRAME_INFO m_frameInfoLatest;
        AcqParamsContext m_acqParamsContext;
        ImageStats m_imgStats;

        Boolean m_isMultGain;
        UInt16 m_emGainMax;
        UInt16 m_ADCoffset;
        UInt16 m_read_noise;

        Boolean m_IsSmartStreamingSupported;
        Boolean m_IsExposeOutModeSupported;
        Boolean m_IsSmartStreamingOn;
        Boolean m_IsExtBinningSupported;
        Boolean m_isPostProcessingAvail;
        Boolean m_isFanControlAvail;

        Boolean m_isMetadataAvail;
        Boolean m_metadataEnabled;  //is metadata currently available
        PvTypes.MD_Frame m_mdFrame;
        PvTypes.MD_Frame_Header m_md_frameHeader;
        FrameMetadata m_frameMetadata;  //Local class to hold frameMetadata
        ROIMetadata[] m_roiMetadata;      //Local class to hold roi metadata

        Int16[] m_frameDataSigned;
        //Trigger Modes & expose out available on the camera
        List<NVP> m_triggerModeList;
        List<NVP> m_expOutModeList;
        //list of Clocking Modes available on the camera Name-Value pair
        List<NVP> m_clockModeList;
        //List of Clearing Modes available on the camera
        List<NVP> m_clearModeList;

        //List of binning factors available on camera if supported
        List<NVP> m_binningSerList; //Serial binning list
        List<NVP> m_binningParList; //Parallel binning list

        //List of Fan speeds available on the camera
        List<NVP> m_fanSpeedList;
        Int32 m_currentFanSpeed; //PvTypes.FanSpeeds

        //Cooling parameter
        Int16 m_currentTemperaure; //Current CCD temperature
        Int16 m_tempSetpoint;      //Current Setpoint
        Int16 m_tempSetpointMin;
        Int16 m_tempSetpointMax;

        //Exposure Time Range
        UInt64 m_expTimeMin;
        UInt64 m_expTimeMax;

        //Estimated read out time
        UInt32 m_readoutTime;

        //List of post processing features
        List<PP_Feature> m_ppFeatureList;

        static String PVCamVersion;
        static AutoResetEvent m_EofEvent = new AutoResetEvent(false);
        public static List<StringBuilder> CameraList = new List<StringBuilder>();

        public PVCamCamera()
        {
            m_acqType = AcqTypes.ACQ_TYPE_SINGLE;
            m_acqRunning = false;
            //m_region = new PvTypes.RegionType[1];
            m_bmpLock = new Object();
            m_fTCapable = false;
            m_EOFHandlerDelegate = new PvTypes.PMCallBack_Ex3(EOFHandler);
            m_spdTable = new SpeedTable();
            m_acqParamsContext = new AcqParamsContext();
            m_imgStats = new ImageStats();
            m_frameMetadata = new FrameMetadata();
            m_framesToGet = 1;
            m_frameNumber = 0;
            m_chipName = new StringBuilder(PvTypes.CCD_NAME_LEN);
            m_triggerMode = (Int32)PvTypes.ExposureModes.TIMED_MODE;
            m_exposeOutMode = 0;
            m_readoutSpeeds = new List<Double>();
            m_IsSmartStreamingSupported = false;
            m_IsSmartStreamingOn = false;
            m_emGainMax = 1000;
            m_clockModeList = new List<NVP>();
            m_triggerModeList = new List<NVP>();
            m_expOutModeList = new List<NVP>();
            m_clearModeList = new List<NVP>();
            m_binningSerList = new List<NVP>();
            m_binningParList = new List<NVP>();
            m_ppFeatureList = new List<PP_Feature>();
            m_fanSpeedList = new List<NVP>();
            m_readoutTime = 0;
            m_pixel_stream = IntPtr.Zero;
            m_circBuffer = IntPtr.Zero;
        }

        //Set multiROI, save structure, actual data is sent to camera while setting up Exposure
        public void SetMultiROI(UInt16[] s1, UInt16[] s2, UInt16[] p1, UInt16[] p2)
        {

            m_currentROICount = (Int16)s1.Length;
            for (int i = 0; i < s1.Length; i++)
            {
                m_region[i].s1 = s1[i];
                m_region[i].s2 = s2[i];
                m_region[i].p1 = p1[i];
                m_region[i].p2 = p2[i];

            }

        }

        //Allocate umnamaged and managed buffers so store image + Metadata (if enabled)
        void AllocateBuffers(AcqTypes acqType)
        {

            //release the BMP handle from previous acquisition
            if (m_bmpHandle.IsAllocated)
                m_bmpHandle.Free();
            //release pixel data buffer
            if (m_pixel_stream != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(m_pixel_stream);
                m_pixel_stream = IntPtr.Zero;
            }
            //Determine final image size and allocate memory accordingly
            //if metadata is enabled full frame size returned by exposure setup includes metadata + pixel data
            if (m_metadataEnabled)
            {
                //Image size 
                //if doing multiroi set full sensor size as full frame will be recomposed else just the ROI 
                if (CurrentROICount > 1)
                {
                    //Determing image size, binning for all regions is same 
                    m_imageSizeX = (Int16)(m_xSize / m_region[0].sbin);
                    m_imageSizeY = (Int16)(m_ySize / m_region[0].pbin);

                }
                else //only one region, set it to the region size
                {
                    //Determing image size 
                    m_imageSizeX = (Int16)((m_region[0].s2 - m_region[0].s1 + 1) / m_region[0].sbin);
                    m_imageSizeY = (Int16)((m_region[0].p2 - m_region[0].p1 + 1) / m_region[0].pbin);

                }

                //2 bytes for each pixel
                m_stream_size = (UInt32)(m_imageSizeX * m_imageSizeY * sizeof(UInt16));

                //Allocate the pixel data buffer only if metadata is enabled, without Metadata full frame is pixel data
                m_pixel_stream = Marshal.AllocHGlobal((Int32)m_stream_size);
            }
            else //no metadata so size returned by pl_exp_setup_seq is pixel data size, image size is same as roi
            {

                m_stream_size = m_full_frame_size;

                m_imageSizeX = (Int16)((m_region[0].s2 - m_region[0].s1 + 1) / m_region[0].sbin);
                m_imageSizeY = (Int16)((m_region[0].p2 - m_region[0].p1 + 1) / m_region[0].pbin);

            }

            //if single seq acquisition allocate single buffer 
            if (acqType == AcqTypes.ACQ_TYPE_SINGLE)
            {

                //release the pre-allocated memory
                if (m_full_frame != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(m_full_frame);
                    m_full_frame = IntPtr.Zero;
                }

                //allocate the m_full_Frame_size amount of bytes in non-managed environment 
                m_full_frame = Marshal.AllocHGlobal((Int32)m_full_frame_size);
            }
            else if (acqType == AcqTypes.ACQ_TYPE_CONTINUOUS)
            {
                //free previous circular buffer
                if (m_circBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(m_circBuffer);
                    m_circBuffer = IntPtr.Zero;
                }
                //allocate circular buffer 5 times the frame size
                //allocate circular buffer of 5 frames
                m_circBuffSize = (Int32)(5 * m_full_frame_size);
                //allocate the circular buffer in the unmanaged environment
                m_circBuffer = Marshal.AllocHGlobal(m_circBuffSize);
            }

            //allocate the pixel buffer in the managed environment, since all cameras are more
            //than 8 bit we will be using UInt16 therefore the size is m_stream_size / 2
            m_frameDataShorts = new UInt16[m_stream_size / 2];

            //alocate a buffer to copy frame data from unmanaged to managed environment
            //Int16 array needs to be used due to available Marshal class methods
            //see conversion to UInt16 once the frame is read out
            m_frameDataSigned = new Int16[m_stream_size / 2];

            //if multiROI or Centroiding zero out the values of pixel buffer for recomposing the frame
            if ((CurrentROICount > 1) || (IsCentroidEnabled))
            {
                //Array.Clear(m_frameDataShorts, 0, m_frameDataSigned.Length - 1);
                //now zero out the pixel stream as well
                //Marshal.Copy(m_frameDataSigned, 0, m_pixel_stream, m_frameDataSigned.Length);
                MemSet(m_pixel_stream, 0, (int)m_stream_size);

            }
            //for the RGB data needed to create the grayscale BMP file in 32bpp format 4x the
            //size of pixels received from the camera is needed (will be explained later)
            PVCamCamera.DataRGB = new Byte[m_stream_size / 2 * 4];

            //allocate a pinned handle to a future BMP file
            m_bmpHandle = GCHandle.Alloc(PVCamCamera.DataRGB, GCHandleType.Pinned);

        }

        //Creates Metadata structure 
        private bool CreateMetadataStructure()
        {

            IntPtr ptr_md_Frame = Marshal.AllocHGlobal(Marshal.SizeOf(m_mdFrame));  //pointer to md structure
            UInt16 roiCount; //number of roi
            Boolean returnVal = false;
            if (IsCentroidEnabled)
            {
                roiCount = CentroidInfo.CurrentCount;

            }
            else
            {
                roiCount = (UInt16)CurrentROICount;
            }

            //Create new structure
            if (!PVCAM.pl_md_create_frame_struct_cont(ref ptr_md_Frame, (UInt16)roiCount))
            {
                ReportMsg(this, new ReportMessage("Failed to create metadata structure", MsgTypes.MSG_ERROR));
            }
            else
            {
                //Create structure from the pointer
                m_mdFrame = (PvTypes.MD_Frame)Marshal.PtrToStructure(ptr_md_Frame, typeof(PvTypes.MD_Frame));
                returnVal = true;

            }

            //free unmanaged memory
            Marshal.FreeHGlobal(ptr_md_Frame);
            ptr_md_Frame = IntPtr.Zero;

            return returnVal;
        }

        //Extract frame data from the MD structure and 
        private void ExtractFrameHeader()
        {

            m_md_frameHeader = (PvTypes.MD_Frame_Header)Marshal.PtrToStructure(m_mdFrame.header, typeof(PvTypes.MD_Frame_Header));

            //populate important meta data
            FrmMetadata.FrameNr = m_md_frameHeader.frameNr;
            FrmMetadata.RoiCount = m_md_frameHeader.roiCount;
            FrmMetadata.TimeStampBOF = (m_md_frameHeader.timeStampBOF) * (m_md_frameHeader.timeStampResNs);
            FrmMetadata.TimeStampEOF = (m_md_frameHeader.timeStampEOF) * (m_md_frameHeader.timeStampResNs);
            FrmMetadata.ExpTime = (m_md_frameHeader.exposureTime) * (m_md_frameHeader.exposureTimeResNs);

        }

        //Extract frame data from the MD structure and 
        private void ExtractROIHeader(IntPtr ptr_roi_header, Int16 roiNr)
        {

            PvTypes.MD_Frame_ROI_Header roi_header = (PvTypes.MD_Frame_ROI_Header)Marshal.PtrToStructure(ptr_roi_header, typeof(PvTypes.MD_Frame_ROI_Header));

            //Initialize each object of array before assigning values
            m_roiMetadata[roiNr] = new ROIMetadata();
            //populate important meta data
            m_roiMetadata[roiNr].ROINr = roi_header.roiNr;
            m_roiMetadata[roiNr].S1 = roi_header.roi.s1;
            m_roiMetadata[roiNr].S2 = roi_header.roi.s2;
            m_roiMetadata[roiNr].P1 = roi_header.roi.p1;
            m_roiMetadata[roiNr].P2 = roi_header.roi.p2;
            m_roiMetadata[roiNr].TimeStampBOR = (roi_header.timestampBOR) * (m_md_frameHeader.roiTimestampResN);
            m_roiMetadata[roiNr].TimeStampEOR = (roi_header.timestampEOR) * (m_md_frameHeader.roiTimestampResN);

        }

        //setup accquisition
        public bool AcqSetup(AcqTypes acqType)
        {


            //if SMART streaming mode is On, then exposure time passed to
            //setup function must be non-zero value and all exposures are defined
            //by SMART streaming parameters (currently only supported
            //on Evolve-512 and Evolve-512 Delta
            if (m_IsSmartStreamingOn)
                m_exposureTime = 10;

            //exposure mode is bitWsie of trigger in and expose out mode. Cameras that don't support
            //Expose out mode as we have set initial value of m_exposeOutMode=0, bitwise OR will have no effect
            //and expMode will be same as Trigger mode
            //trigger in mode
            Int16 expMode = (Int16)(m_triggerMode | m_exposeOutMode);

            //Metadata should be enabled for MultiROI or centroiding. Enable it if it is not already done
            if (((CurrentROICount > 1) || (IsCentroidEnabled)) && (m_metadataEnabled == false))
            {
                //Enable metadata
                ConfigureMetadata(true);
            }

            if (acqType == AcqTypes.ACQ_TYPE_SINGLE)
            {

                ////if a known number of frames (in this case 1 frame) is to be acquired, call
                //pl_exp_setup_seq(), the second parameter ("1") can be changed to e.g. "1000"
                //if 1000 frames need to be acquired, in that case the function will also return
                //larger m_full_frame_size value since more memory will be needed to acquire 1000 frames

                if (!PVCAM.pl_exp_setup_seq(m_hCam, 1, (UInt16)CurrentROICount, ref Region[0], expMode, m_exposureTime, out m_full_frame_size))
                {
                    ReportMsg(this, new ReportMessage("Single acquisition setup failed", MsgTypes.MSG_ERROR));
                    return false;
                }
                else
                {
                    ReportMsg(this, new ReportMessage("Single acquisition setup OK", MsgTypes.MSG_STATUS));
                }

                m_frameNumber = 0;

            }
            else if (acqType == AcqTypes.ACQ_TYPE_CONTINUOUS)
            {

                //use pl_exp_setup_cont() when the acquisition has to collect unknown number of frames ahead of the
                //acquisition start
                if (!PVCAM.pl_exp_setup_cont(m_hCam, (UInt16)CurrentROICount, ref Region[0], expMode, m_exposureTime, out m_full_frame_size,
                                             (Int16)PvTypes.ContinuousModes.CIRC_OVERWRITE))
                {
                    ReportMsg(this, new ReportMessage("Continuous acquisition setup failed", MsgTypes.MSG_ERROR));
                    return false;
                }
                else
                {
                    ReportMsg(this, new ReportMessage("Continuous acquisition setup OK", MsgTypes.MSG_STATUS));
                }

                //populate fields of our example/test structure pointer to which
                //is being passed to callback registration function
                m_acqParamsContext.ExposureTime = m_exposureTime;
                m_acqParamsContext.Binning = m_region[0].sbin;

                //pointer to structure passed to unmanaged environment should not change so tell
                //garbage collector to pin it
                GCHandle paramsHandle = GCHandle.Alloc(m_acqParamsContext, GCHandleType.Pinned);

                //register a callback function (pointed to by the m_EOFHandlerDelegate). The PVCAM will be calling this function
                //each time EOF event occurs (EOF = end of frame readout) and pass the address of the structure that should be
                //pushed back to callback handler function once the EOF event occurs
                //callback functions are the preferred notification methods to polling thanks to its lower impact on the CPU and acquisition
                if (!PVCAM.pl_cam_register_callback_ex3(m_hCam, PvTypes.PL_CALLBACK_EVENT.PL_CALLBACK_EOF, m_EOFHandlerDelegate, paramsHandle.AddrOfPinnedObject()))
                {
                    ReportMsg(this, new ReportMessage("Callback registration failed", MsgTypes.MSG_ERROR));
                    return false;
                }
                //reset the event that notifies the camera thread that the new frame has arrived in the callback handler
                m_EofEvent.Reset();
                m_frameNumber = 1;
            }

            //Allocate buffers based on frame size return by exposure setup
            AllocateBuffers(acqType);

            //Get Estimated read out time/ not all cameras support it, called after setting up the exposure
            GetEstReadoutTime();

            m_acqType = acqType;

            return true;
        }

        //start sequential acquisition
        public bool StartSeqAcq()
        {
            m_abortAcquisition = false;

            //Create metadata structure only if metadata is enabled
            if (m_metadataEnabled)
            {
                if (!CreateMetadataStructure())
                {
                    return false;
                }
            }

            //start the acquisition, pass the pointer (IntPtr) m_pixel_stream
            //into which camera will deliver the pixel data
            if (!PVCAM.pl_exp_start_seq(m_hCam, m_full_frame))
            {
                ReportMsg(this, new ReportMessage("Acquisition start failed", MsgTypes.MSG_ERROR));
                return false;
            }

            m_acqRunning = true;

            //notify UI thread the acquisition has started
            ReportMsg(this, new ReportMessage("Acquisition started", MsgTypes.MSG_STATUS));
            CamNotif(this, new ReportEvent(CameraNotifications.ACQ_SINGLE_STARTED));

            //start a new thread waiting for the frame readout
            m_SingleAcqThread = new Thread(new ThreadStart(SingleAcquisition));
            m_SingleAcqThread.Start();

            return true;
        }

        //single frame readout thread
        private void SingleAcquisition()
        {
            Int16 status;
            UInt32 byte_cnt;
            Boolean isMultiROI = (CurrentROICount > 1) || (IsCentroidEnabled);
            UInt16 roiCount = IsCentroidEnabled ? (CentroidInfo.CurrentCount) : ((UInt16)CurrentROICount);

            status = (Int16)PvTypes.ReadoutStatuses.READOUT_FAILED;

            //wait for image acquisition to be completed with polling, every 10ms check
            //whether the frame has arrived
            while (!m_abortAcquisition
                    && PVCAM.pl_exp_check_status(m_hCam, out status, out byte_cnt)
                    && status != (Int16)PvTypes.ReadoutStatuses.READOUT_COMPLETE
                    && status != (Int16)PvTypes.ReadoutStatuses.READOUT_FAILED)
            {
                Thread.Sleep(10);

            }

            if (m_abortAcquisition)
            {
                m_acqRunning = false;
                return;
            }

            if (status == (Int16)PvTypes.ReadoutStatuses.READOUT_FAILED)
            {
                ReportMsg(this, new ReportMessage("Single acquisition readout failed", MsgTypes.MSG_ERROR));
            }
            else if (status == (Int16)PvTypes.ReadoutStatuses.READOUT_COMPLETE)
            {
                ReportMsg(this, new ReportMessage("Readout completed", MsgTypes.MSG_STATUS));

                if (m_metadataEnabled)
                {
                    IntPtr ptr_md_Frame = Marshal.AllocHGlobal(Marshal.SizeOf(m_mdFrame));
                    //get pointer to the structure
                    Marshal.StructureToPtr(m_mdFrame, ptr_md_Frame, false);

                    if (PVCAM.pl_md_frame_decode(ptr_md_Frame, m_full_frame, m_full_frame_size))
                    {

                        m_mdFrame = (PvTypes.MD_Frame)Marshal.PtrToStructure(ptr_md_Frame, typeof(PvTypes.MD_Frame));
                        ExtractFrameHeader();

                        //Define ROI arrays based on number of regions
                        PvTypes.Md_Frame_Roi[] m_frame_roi = new PvTypes.Md_Frame_Roi[roiCount];

                        //Initialize array 
                        m_roiMetadata = new ROIMetadata[roiCount];


                        IntPtr ptr;
                        int roi_md_size = Marshal.SizeOf(m_frame_roi[0]);
                        for (Int16 i = 0; i < roiCount; i++)
                        {
                            //First roi metadata 
                            if (i == 0)
                            {
                                m_frame_roi[0] = (PvTypes.Md_Frame_Roi)Marshal.PtrToStructure(m_mdFrame.roiArray, typeof(PvTypes.Md_Frame_Roi));
                                // roi_md_size = Marshal.SizeOf(m_frame_roi[0]);
                            }
                            else
                            {

                                //subsequent ROI metadata at offset from the first roi pointer
                                if (IntPtr.Size == sizeof(Int64))
                                {
                                    ptr = new IntPtr((m_mdFrame.roiArray).ToInt64() + roi_md_size * i);
                                }
                                else
                                {
                                    ptr = new IntPtr((m_mdFrame.roiArray).ToInt32() + roi_md_size * i);

                                }
                                m_frame_roi[i] = (PvTypes.Md_Frame_Roi)Marshal.PtrToStructure(ptr, typeof(PvTypes.Md_Frame_Roi));
                            }
                            //Frame Roi Header can be extracted from above if required.
                            ExtractROIHeader(m_frame_roi[i].header, i);

                        }

                        //free umnamaged pointers
                        Marshal.FreeHGlobal(ptr_md_Frame);
                        ptr_md_Frame = IntPtr.Zero;
                        ptr = IntPtr.Zero;

                        if (!isMultiROI) //if single ROI (with metadate) copy ROI data to managed array
                        {

                            Marshal.Copy(m_frame_roi[0].data, m_frameDataSigned, 0, (int)m_frame_roi[0].dataSize / 2);
                        }
                        else //recompose the frame 
                        {
                            //if centroiding Zero out the frame before recomposing as ROI are dynamically changing
                            if (IsCentroidEnabled)
                                MemSet(m_pixel_stream, 0, (int)m_stream_size);
                            if (!PVCAM.pl_md_frame_recompose(m_pixel_stream, m_mdFrame.impliedRoi.s1, m_mdFrame.impliedRoi.p1, (UInt16)m_imageSizeX, (UInt16)m_imageSizeY, ref m_mdFrame))
                            {
                                ReportMsg(this, new ReportMessage("Failed to recompose frame", MsgTypes.MSG_ERROR));
                                m_acqRunning = false;
                                return;
                            }
                            else
                            {
                                //Copy recomposed frame to managed array
                                Marshal.Copy(m_pixel_stream, m_frameDataSigned, 0, m_frameDataSigned.Length);
                            }

                        }

                    }
                    else
                    {
                        ReportMsg(this, new ReportMessage("Failed to decode metadata structure", MsgTypes.MSG_ERROR));
                        m_acqRunning = false;
                        Marshal.FreeHGlobal(ptr_md_Frame);
                        ptr_md_Frame = IntPtr.Zero;
                        CamNotif(this, new ReportEvent(CameraNotifications.ACQ_SINGLE_FAILED));
                        return;
                    }

                }
                else //no metadata simply copy the full frame buffer to managed array
                {

                    //copy the data from unmanaged to managed memory, Marshal.Copy has no overload that 
                    //works with UInt16, therefore we use signed Int16 array
                    Marshal.Copy(m_full_frame, m_frameDataSigned, 0, m_frameDataSigned.Length);

                }

                //convert the data in managed memory from signed to unsigned shorts
                System.Buffer.BlockCopy(m_frameDataSigned, 0, m_frameDataShorts, 0, (Int32)(m_stream_size));

                m_frameNumber++;

                m_frameInfoLatest.FrameNr = m_frameNumber;

                CamNotif(this, new ReportEvent(CameraNotifications.ACQ_NEW_FRAME_RECEIVED));

                CamNotif(this, new ReportEvent(CameraNotifications.ACQ_SINGLE_FINISHED));

                //pass image along
                

            }
            m_acqRunning = false;

            //Show ROI metadata of some ROIs
            if (roiCount > 1)
            {
                int j = (roiCount >= 5) ? 5 : roiCount;
                string s;
                ReportMsg(this, new ReportMessage("ROI metadata :- ", MsgTypes.MSG_STATUS));
                for (int i = 0; i < j; i++)
                {

                    s = string.Format("ROI #{0} - region = [{1}..{2}],[{3}..{4}], Time Stamp EOR = {5}",
                                       m_roiMetadata[i].ROINr, m_roiMetadata[i].S1, m_roiMetadata[i].S2, m_roiMetadata[i].P1,
                                       m_roiMetadata[i].P2, m_roiMetadata[i].TimeStampEOR);
                    ReportMsg(this, new ReportMessage(s, MsgTypes.MSG_STATUS));

                }
            }


        }

        //start continuous acquisition
        public bool StartContinuousAcquisition()
        {
            m_abortAcquisition = false;

            UInt16 roiCount = IsCentroidEnabled ? (CentroidInfo.CurrentCount) : ((UInt16)CurrentROICount);

            //Create metadata structure only if metadata is enabled
            if (m_metadataEnabled)
            {
                if (!CreateMetadataStructure())
                {
                    return false;
                }
            }

            //start the acquisition, pass the pointer (IntPtr) m_circBuffer
            //into which camera will deliver the pixel data and tell the camera
            //the size of circular buffer
            if (!PVCAM.pl_exp_start_cont(m_hCam, m_circBuffer, (UInt32)m_circBuffSize))
            {
                ReportMsg(this, new ReportMessage("Acquisition start failed", MsgTypes.MSG_ERROR));
                return false;
            }

            m_acqRunning = true;

            ReportMsg(this, new ReportMessage("Acquisition started", MsgTypes.MSG_STATUS));

            CamNotif(this, new ReportEvent(CameraNotifications.ACQ_CONT_STARTED));

            //start a new thread retrieveing the frames from the circular buffer
            m_FrameDispThread = new Thread(new ThreadStart(GetNewFrame));
            m_FrameDispThread.Start();

            return true;
        }

        //thread waiting for frames in circular buffer (continuous) acquisition mode
        private void GetNewFrame()
        {
            Boolean isMultiROI = (CurrentROICount > 1) || (IsCentroidEnabled);
            UInt16 roiCount = IsCentroidEnabled ? (CentroidInfo.CurrentCount) : ((UInt16)CurrentROICount);
            while (!m_abortAcquisition)
            {
                //wait for the callback notification
                m_EofEvent.WaitOne();

                if (m_abortAcquisition)
                {
                    m_acqRunning = false;
                    return;
                }

                //use the pl_exp_get_latest_frame_ex() function to get the address of the latest frame in the
                //circular buffer and additional information pertaining to this image in m_frameInfoLatest, see
                //PvTypes.FRAME_INFO type for further details
                //FrameNr field of m_frameInfoLatest is used in the UI as part of Image Statistics to compare
                //the number of frames produced by the camera and number of frames displayed by the application
                if (!PVCAM.pl_exp_get_latest_frame_ex(m_hCam, out m_latestFrameAddress, out m_frameInfoLatest))
                {
                    ReportMsg(this, new ReportMessage("Getting latest frame address failed", MsgTypes.MSG_ERROR));
                    m_acqRunning = false;
                }
                else
                {

                    if (m_metadataEnabled)
                    {

                        IntPtr ptr_md_Frame = Marshal.AllocHGlobal(Marshal.SizeOf(m_mdFrame));
                        Marshal.StructureToPtr(m_mdFrame, ptr_md_Frame, false);
                        if (PVCAM.pl_md_frame_decode(ptr_md_Frame, m_latestFrameAddress, m_full_frame_size))
                        {

                            m_mdFrame = (PvTypes.MD_Frame)Marshal.PtrToStructure(ptr_md_Frame, typeof(PvTypes.MD_Frame));

                            //Extract Frame metadata
                            ExtractFrameHeader();

                            //Define ROI arrays based on number of regions
                            PvTypes.Md_Frame_Roi[] m_frame_roi = new PvTypes.Md_Frame_Roi[roiCount];

                            //define ROI headers based on number of rois
                            m_roiMetadata = new ROIMetadata[roiCount];

                            IntPtr ptr;
                            int roi_md_size = Marshal.SizeOf(m_frame_roi[0]);
                            for (Int16 i = 0; i < roiCount; i++)
                            {
                                //First roi metadata 
                                if (i == 0)
                                {
                                    m_frame_roi[0] = (PvTypes.Md_Frame_Roi)Marshal.PtrToStructure(m_mdFrame.roiArray, typeof(PvTypes.Md_Frame_Roi));
                                    // roi_md_size = Marshal.SizeOf(m_frame_roi[0]);
                                }
                                else
                                {

                                    //subsequent ROI metadata at offset from the first roi pointer
                                    if (IntPtr.Size == sizeof(Int64))
                                    {
                                        ptr = new IntPtr((m_mdFrame.roiArray).ToInt64() + roi_md_size * i);
                                    }
                                    else
                                    {
                                        ptr = new IntPtr((m_mdFrame.roiArray).ToInt32() + roi_md_size * i);

                                    }
                                    m_frame_roi[i] = (PvTypes.Md_Frame_Roi)Marshal.PtrToStructure(ptr, typeof(PvTypes.Md_Frame_Roi));
                                }
                                //Frame Roi Header can be extracted from above if required.
                                ExtractROIHeader(m_frame_roi[i].header, i);

                            }

                            Marshal.FreeHGlobal(ptr_md_Frame);
                            ptr_md_Frame = IntPtr.Zero;

                            //free pointer
                            ptr = IntPtr.Zero;

                            //for single ROI copy ROI data to managed array
                            if (!isMultiROI)
                            {
                                Marshal.Copy(m_frame_roi[0].data, m_frameDataSigned, 0, (int)m_frame_roi[0].dataSize / 2);
                            }
                            else //recompose the frame if multiroi
                            {
                                //if centroiding Zero out the frame before as ROI are dynamically changing
                                if (IsCentroidEnabled)
                                    MemSet(m_pixel_stream, 0, (int)m_stream_size);
                                if (!PVCAM.pl_md_frame_recompose(m_pixel_stream, m_mdFrame.impliedRoi.s1, m_mdFrame.impliedRoi.p1, (UInt16)m_imageSizeX, (UInt16)m_imageSizeY, ref m_mdFrame))
                                {
                                    ReportMsg(this, new ReportMessage("Failed to recompose frame", MsgTypes.MSG_ERROR));
                                    m_acqRunning = false;
                                    return;
                                }
                                else //Copy recomposed frame to managed atrray
                                {
                                    Marshal.Copy(m_pixel_stream, m_frameDataSigned, 0, m_frameDataSigned.Length);
                                }

                            }

                        }

                        else
                        {
                            ReportMsg(this, new ReportMessage("Failed to decode metadata structure", MsgTypes.MSG_ERROR));
                            Marshal.FreeHGlobal(ptr_md_Frame);
                            ptr_md_Frame = IntPtr.Zero;
                            m_acqRunning = false;
                            CamNotif(this, new ReportEvent(CameraNotifications.ACQ_CONT_FAILED));
                            return;
                        }

                    }
                    else //no metadata
                    {

                        //copy the data from unmanaged to managed memory, Marshal.Copy has no overload that
                        //works with UInt16, therefore we copy data to signed Int16 array first
                        Marshal.Copy(m_latestFrameAddress, m_frameDataSigned, 0, m_frameDataSigned.Length);
                    }

                    //convert the data in managed memory from signed to unsigned shorts
                    System.Buffer.BlockCopy(m_frameDataSigned, 0, m_frameDataShorts, 0, (Int32)(m_stream_size));

                    m_frameNumber++;

                    CamNotif(this, new ReportEvent(CameraNotifications.ACQ_NEW_FRAME_RECEIVED));

                    //if the application has acquired all the images specified by the user in the UI,
                    //stop the acquisition
                    if (m_frameNumber > m_framesToGet)
                    {
                        StopAcquisition();

                        CamNotif(this, new ReportEvent(CameraNotifications.ACQ_CONT_FINISHED));

                        return;
                    }
                }
            }
            m_acqRunning = false;

            //Show ROI metadata of some ROIs
            if (roiCount > 1)
            {
                int j = (roiCount >= 5) ? 5 : roiCount;
                string s;
                ReportMsg(this, new ReportMessage("ROI metadata :- ", MsgTypes.MSG_STATUS));
                for (int i = 0; i < j; i++)
                {

                    s = string.Format("ROI #{0} - region = [{1}..{2}],[{3}..{4}], Time Stamp EOR = {5}",
                                       m_roiMetadata[i].ROINr, m_roiMetadata[i].S1, m_roiMetadata[i].S2, m_roiMetadata[i].P1,
                                       m_roiMetadata[i].P2, m_roiMetadata[i].TimeStampEOR);
                    ReportMsg(this, new ReportMessage(s, MsgTypes.MSG_STATUS));

                }
            }

        }

        //Check if a parameter is available
        public bool isParamAvailable(UInt32 ParamID)
        {
            IntPtr unmngIsAvaiable = Marshal.AllocHGlobal(sizeof(UInt16));
            Boolean retValue;
            if (!PVCAM.pl_get_param(m_hCam, ParamID, (Int16)PvTypes.AttributeIDs.ATTR_AVAIL, unmngIsAvaiable))
            {
                ReportMsg(this, new ReportMessage("Couldn't read PARAM_FRAME_CAPABLE attribute AVAILABLE", MsgTypes.MSG_ERROR));
                retValue = false;
            }
            else
            {
                retValue = Convert.ToBoolean(Marshal.ReadInt16(unmngIsAvaiable));

            }

            //free unmanaged memory
            Marshal.FreeHGlobal(unmngIsAvaiable);
            unmngIsAvaiable = IntPtr.Zero;
            return retValue;
        }

        //To read enumeration type parameters/ Name and value pair
        public bool ReadEnumeration(List<NVP> nvpList, UInt32 ParamID)
        {
            bool retValue = true;
            nvpList.Clear();
            UInt32 count;
            IntPtr unmngIsAvaiable = Marshal.AllocHGlobal(sizeof(UInt16));
            IntPtr unmngCount = Marshal.AllocHGlobal(sizeof(UInt32));
            UInt32 strLength;
            StringBuilder paramName = new StringBuilder();
            Int32 paramValue;

            if (!PVCAM.pl_get_param(m_hCam, ParamID, (Int16)PvTypes.AttributeIDs.ATTR_AVAIL, unmngIsAvaiable))
            {
                ReportMsg(this, new ReportMessage("Couldn't read PARAM_FRAME_CAPABLE attribute AVAILABLE", MsgTypes.MSG_ERROR));
                retValue = false;
            }
            else
            {
                if (Marshal.ReadInt16(unmngIsAvaiable) == 1)  //Atrribute is available
                {
                    if (!PVCAM.pl_get_param(m_hCam, ParamID, (Int16)PvTypes.AttributeIDs.ATTR_COUNT, unmngCount))
                    {
                        ReportMsg(this, new ReportMessage("Couldn't read COUNT Value of parameter", MsgTypes.MSG_ERROR));
                        retValue = false;
                    }
                    else
                    {
                        count = Convert.ToUInt32(Marshal.ReadInt32(unmngCount));

                        //Now get the value and name associated
                        for (UInt32 i = 0; i < count; i++)
                        {
                            if (!PVCAM.pl_enum_str_length(m_hCam, ParamID, i, out strLength))
                            {
                                ReportMsg(this, new ReportMessage("Couldn't read string length of the parameter", MsgTypes.MSG_ERROR));
                                retValue = false;
                            }
                            else
                            {
                                if (!PVCAM.pl_get_enum_param(m_hCam, ParamID, i, out paramValue, paramName, strLength))
                                {
                                    ReportMsg(this, new ReportMessage("Couldn't read Name value of the parameter", MsgTypes.MSG_ERROR));
                                    retValue = false;
                                }
                                NVP nvp = new NVP();
                                nvp.Value = paramValue;
                                nvp.Name = paramName.ToString();
                                nvpList.Add(nvp);
                            }
                        }
                    }
                }
            }

            //free all unmanaged pointers
            Marshal.FreeHGlobal(unmngIsAvaiable);
            unmngIsAvaiable = IntPtr.Zero;

            Marshal.FreeHGlobal(unmngCount);
            unmngCount = IntPtr.Zero;

            return retValue;
        }

        //called when user pressed Stop Acquisition button
        public void StopAcquisition()
        {
           
            if (!m_acqRunning)
            {
                return;
            }

            m_abortAcquisition = true;

            if (m_acqType == AcqTypes.ACQ_TYPE_CONTINUOUS)
            {
                //stop the acquisition
                if (!PVCAM.pl_exp_stop_cont(m_hCam, (Int16)PvTypes.AbortExposureFlags.CCS_CLEAR))
                {
                    ReportMsg(this, new ReportMessage("Failed to stop continuous acquisition", MsgTypes.MSG_ERROR));
                }

                //deregister the callback
                PVCAM.pl_cam_deregister_callback(m_hCam, PvTypes.PL_CALLBACK_EVENT.PL_CALLBACK_EOF);

                //send one more EOF event as if the last frame arrived just in case acquisition thread keeps
                //waiting for it
                m_EofEvent.Set();

                ReportMsg(this, new ReportMessage("Live mode stopped", MsgTypes.MSG_STATUS));

                CamNotif(this, new ReportEvent(CameraNotifications.ACQ_CONT_CANCELED));
            }
            else if (m_acqType == AcqTypes.ACQ_TYPE_SINGLE)
            {
                //abort the exposure
                if (!PVCAM.pl_exp_abort(m_hCam, (Int16)PvTypes.AbortExposureFlags.CCS_CLEAR))
                {
                    ReportMsg(this, new ReportMessage("Failed to stop single acquisition", MsgTypes.MSG_ERROR));
                }

                ReportMsg(this, new ReportMessage("Single acquisition cancelled", MsgTypes.MSG_STATUS));

                CamNotif(this, new ReportEvent(CameraNotifications.ACQ_SINGLE_CANCELLED));
            }
            m_acqRunning = false;

        }

        public bool isRunning()
        {
            return m_acqRunning;
        }

        //called when user exits the application
        public void WaitForFullAcquisitionStop()
        {
            if (!m_acqRunning)
            {
                return;
            }

            if (m_acqType == AcqTypes.ACQ_TYPE_SINGLE)
            {
                if (m_SingleAcqThread != null)
                {
                    m_SingleAcqThread.Join();
                }
            }
            else if (m_acqType == AcqTypes.ACQ_TYPE_CONTINUOUS)
            {
                if (m_FrameDispThread != null)
                {
                    m_FrameDispThread.Join();
                }
            }
        }

        //open camera
        public static Boolean OpenCamera(StringBuilder cameraToOpen, PVCamCamera activeCamera)
        {
            //open camera in EXCLUSIVE mode, camera to be opened is specified by the string cameraToOpen
            if (!PVCAM.pl_cam_open(cameraToOpen, out activeCamera.m_hCam, PvTypes.CameraOpenMode.OPEN_EXCLUSIVE))
            {
                activeCamera.ReportMsg(activeCamera, new ReportMessage("Camera opening failed", MsgTypes.MSG_ERROR));
                return false;
            }

            activeCamera.OpenedCamName = cameraToOpen.ToString();
            activeCamera.ReportMsg(activeCamera, new ReportMessage((""), MsgTypes.MSG_STATUS));
            activeCamera.ReportMsg(activeCamera, new ReportMessage(String.Format("Camera {0} opened successfully", cameraToOpen.ToString()), MsgTypes.MSG_STATUS));
            activeCamera.CamNotif(activeCamera, new ReportEvent(CameraNotifications.CAMERA_OPENED));

            return true;
        }

        //Read Extended binning information- Supported cameras reports serial and parallel binnings available and
        //also the description associated with it
        private void ReadExtBinningParameters()
        {
            //Check if Expose Out Parameter is available on the camera
            m_IsExtBinningSupported = false;  //start with false
            if (isParamAvailable(PvTypes.PARAM_BINNING_SER))
            {
                m_IsExtBinningSupported = true;

                if (!ReadEnumeration(m_binningSerList, PvTypes.PARAM_BINNING_SER))
                {
                    ReportMsg(this, new ReportMessage("Failed to get Extended serial binning information", MsgTypes.MSG_ERROR));
                }
                else
                {
                    ReportMsg(this, new ReportMessage("Extended serial binning information read successfully", MsgTypes.MSG_STATUS));
                    if (!ReadEnumeration(m_binningParList, PvTypes.PARAM_BINNING_PAR))
                    {
                        ReportMsg(this, new ReportMessage("Failed to get Extended parallel binning information", MsgTypes.MSG_ERROR));
                    }
                    else
                    {
                        ReportMsg(this, new ReportMessage("Extended parallel binning information read successfully", MsgTypes.MSG_STATUS));
                    }
                }
            }
        }
        //Enable or Disable Metadata
        public Boolean ConfigureMetadata(Boolean enableState)
        {
            if (m_isMetadataAvail)
            {
                IntPtr unmgEnableMD;
                unmgEnableMD = Marshal.AllocHGlobal(sizeof(Int16));
                if (enableState == true)
                {
                    Marshal.WriteInt16(unmgEnableMD, 1); //write true
                }
                else
                {
                    Marshal.WriteInt16(unmgEnableMD, 0); //write false
                }

                //send value to camera Frame metadata
                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_METADATA_ENABLED, unmgEnableMD))
                {
                    ReportMsg(this, new ReportMessage("Enabling Frame MetaData Failed", MsgTypes.MSG_ERROR));
                    Marshal.FreeHGlobal(unmgEnableMD);
                    unmgEnableMD = IntPtr.Zero;
                    m_metadataEnabled = false;
                    return false;
                }
                else
                {
                    ReportMsg(this, new ReportMessage(string.Format("Successfully set meta data state to {0}", enableState), MsgTypes.MSG_STATUS));
                    m_metadataEnabled = enableState;
                    return true;
                }

            }
            else
            {
                ReportMsg(this, new ReportMessage("Metadata not available for this camera", MsgTypes.MSG_ERROR));
                m_metadataEnabled = false;
                return false;
            }
        }


        //Read Fan speed setpoints available on the camera and also the current value
        private void ReadFanSpeedParameters()
        {
            m_isFanControlAvail = false;
            if (isParamAvailable(PvTypes.PARAM_FAN_SPEED_SETPOINT))
            {
                m_isFanControlAvail = true;

                if (!ReadEnumeration(m_fanSpeedList, PvTypes.PARAM_FAN_SPEED_SETPOINT))
                {
                    ReportMsg(this, new ReportMessage("Failed to get fan speed setpoint information", MsgTypes.MSG_ERROR));
                }
                else
                {
                    //now read the current fan speed, will be used for display on GUI
                    IntPtr unmgCurrentFanSpeed = Marshal.AllocHGlobal(sizeof(Int32));

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_FAN_SPEED_SETPOINT, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmgCurrentFanSpeed))
                    {
                        ReportMsg(this, new ReportMessage("Reading Current fan speed failed", MsgTypes.MSG_ERROR));
                    }
                    else
                    {
                        m_currentFanSpeed = Marshal.ReadInt32(unmgCurrentFanSpeed);
                        ReportMsg(this, new ReportMessage("Fan speed setpoint information read successfully", MsgTypes.MSG_STATUS));
                    }
                    Marshal.FreeHGlobal(unmgCurrentFanSpeed);
                    unmgCurrentFanSpeed = IntPtr.Zero;

                }
            }
        }


        //Reads cooling parameters from the camera like current cooling setpoint/range and current CCD temperature
        public void ReadCoolingParameters()
        {
            //Get Temperature setpoint - range and current CCD temperature
            IntPtr unmgTempSetpoint = Marshal.AllocHGlobal(sizeof(Int16));
            IntPtr unmgTempSetpointMin = Marshal.AllocHGlobal(sizeof(Int16));
            IntPtr unmgTempSetpointMax = Marshal.AllocHGlobal(sizeof(Int16));

            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_TEMP_SETPOINT, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmgTempSetpoint))
            {
                ReportMsg(this, new ReportMessage("Reading Current temperature Setpoint failed", MsgTypes.MSG_ERROR));
            }
            else
            {
                m_tempSetpoint = Marshal.ReadInt16(unmgTempSetpoint);

                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_TEMP_SETPOINT, (Int16)PvTypes.AttributeIDs.ATTR_MIN, unmgTempSetpointMin))
                {
                    ReportMsg(this, new ReportMessage("Reading temperature Setpoint Min Value failed", MsgTypes.MSG_ERROR));
                }
                else
                {
                    m_tempSetpointMin = Marshal.ReadInt16(unmgTempSetpointMin);
                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_TEMP_SETPOINT, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmgTempSetpointMax))
                    {
                        ReportMsg(this, new ReportMessage("Reading temperature Setpoint Max Value failed", MsgTypes.MSG_ERROR));
                    }
                    else
                    {
                        m_tempSetpointMax = Marshal.ReadInt16(unmgTempSetpointMax);
                        ReportMsg(this, new ReportMessage("Reading Cooling paramters complete", MsgTypes.MSG_STATUS));
                    }
                }
            }

            Marshal.FreeHGlobal(unmgTempSetpoint);
            unmgTempSetpoint = IntPtr.Zero;

            Marshal.FreeHGlobal(unmgTempSetpointMin);
            unmgTempSetpointMin = IntPtr.Zero;

            Marshal.FreeHGlobal(unmgTempSetpointMax);
            unmgTempSetpointMax = IntPtr.Zero;
        }

        //Read Region parameters, checks is camera supports multiroi and set the initial ROI
        private void ReadRegionParameters()
        {
            //Check Frame Metadata available
            if (isParamAvailable(PvTypes.PARAM_METADATA_ENABLED))
            {
                m_isMetadataAvail = true;
                //Disable metadata at start up
                ConfigureMetadata(false);
            }
            else
            {
                m_isMetadataAvail = false;

            }

            //Set Max Roi count to 1, if multiroi is not available 
            m_maxROICount = 1;
            if (isParamAvailable(PvTypes.PARAM_ROI_COUNT))
            {

                //read Max ROI available
                IntPtr unmgROICount = Marshal.AllocHGlobal(sizeof(UInt16));
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_ROI_COUNT, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmgROICount))
                {
                    ReportMsg(this, new ReportMessage("Failed to get Max ROI count info", MsgTypes.MSG_ERROR));

                }
                else
                {
                    m_maxROICount = Marshal.ReadInt16(unmgROICount);
                }
                Marshal.FreeHGlobal(unmgROICount);
                unmgROICount = IntPtr.Zero;
            }

            //define Region type array 
            m_region = new PvTypes.RegionType[m_maxROICount];

            //set current roi count to 1 and set default CCD region to full size 
            m_currentROICount = 1;
            m_region[0].s1 = 0;
            m_region[0].s2 = (UInt16)(m_xSize - 1);
            m_region[0].p1 = 0;
            m_region[0].p2 = (UInt16)(m_ySize - 1);
            m_region[0].sbin = 1;
            m_region[0].pbin = 1;


        }
        //Read information regarinding centroiding feature available on certain cameras
        void ReadCentroidParameters()
        {
            IsCentroidAvail = false;
            if (isParamAvailable(PvTypes.PARAM_CENTROIDS_ENABLED))
            {
                IsCentroidAvail = true;
                m_centroid = new Centroid();
                IntPtr unmgCentroidCountMin = Marshal.AllocHGlobal(sizeof(Int16));
                IntPtr unmgCentroidCountMax = Marshal.AllocHGlobal(sizeof(Int16));
                IntPtr unmgCentroidRadiusMin = Marshal.AllocHGlobal(sizeof(Int16));
                IntPtr unmgCentroidRadiusMax = Marshal.AllocHGlobal(sizeof(Int16));

                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CENTROIDS_COUNT, (Int16)PvTypes.AttributeIDs.ATTR_MIN, unmgCentroidCountMin))
                {
                    ReportMsg(this, new ReportMessage("Reading Min Centroid count failed", MsgTypes.MSG_ERROR));
                }
                else
                {

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CENTROIDS_COUNT, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmgCentroidCountMax))
                    {
                        ReportMsg(this, new ReportMessage("Reading Max. Centroid count failed", MsgTypes.MSG_ERROR));
                    }
                    else
                    {
                        if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CENTROIDS_RADIUS, (Int16)PvTypes.AttributeIDs.ATTR_MIN, unmgCentroidRadiusMin))
                        {
                            ReportMsg(this, new ReportMessage("Reading Min. Centroid Radius failed", MsgTypes.MSG_ERROR));
                        }
                        else
                        {
                            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CENTROIDS_RADIUS, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmgCentroidRadiusMax))
                            {
                                ReportMsg(this, new ReportMessage("Reading Max. Centroid Radius failed", MsgTypes.MSG_ERROR));
                            }
                            else //All is good
                            {
                                m_centroid.MinCount = (UInt16)Marshal.ReadInt16(unmgCentroidCountMin);
                                m_centroid.MaxCount = (UInt16)Marshal.ReadInt16(unmgCentroidCountMax);
                                m_centroid.MinRadius = (UInt16)Marshal.ReadInt16(unmgCentroidRadiusMin);
                                m_centroid.MaxRadius = (UInt16)Marshal.ReadInt16(unmgCentroidRadiusMax);
                            }
                        }
                    }
                }

                Marshal.FreeHGlobal(unmgCentroidCountMin);
                unmgCentroidCountMin = IntPtr.Zero;

                Marshal.FreeHGlobal(unmgCentroidCountMax);
                unmgCentroidCountMax = IntPtr.Zero;

                Marshal.FreeHGlobal(unmgCentroidRadiusMin);
                unmgCentroidRadiusMin = IntPtr.Zero;

                Marshal.FreeHGlobal(unmgCentroidRadiusMax);
                unmgCentroidRadiusMax = IntPtr.Zero;

            }
        }

        //Set Centroid Feature on/off 
        public Boolean SetCentroiding(Boolean enableState, UInt16 centroidCnt, UInt16 CentroidRadius)
        {
            IsCentroidEnabled = false;
            if (!IsCentroidAvail)
            {
                ReportMsg(this, new ReportMessage("Centroid Feature not available for this camera", MsgTypes.MSG_ERROR));
                return false;

            }

            IntPtr unmgCentroidState = Marshal.AllocHGlobal(sizeof(Int16));
            IntPtr unmgCentroidCount = Marshal.AllocHGlobal(sizeof(Int16));
            IntPtr unmgCentroidRadius = Marshal.AllocHGlobal(sizeof(Int16));
            Boolean returnVal = false;
            if (enableState == true)
            {
                Marshal.WriteInt16(unmgCentroidState, 1);
                Marshal.WriteInt16(unmgCentroidCount, (Int16)centroidCnt);
                Marshal.WriteInt16(unmgCentroidRadius, (Int16)CentroidRadius);

                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_CENTROIDS_ENABLED, unmgCentroidState))
                {
                    ReportMsg(this, new ReportMessage("Failed to Set Centroid feature", MsgTypes.MSG_ERROR));
                }
                else
                {

                    if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_CENTROIDS_RADIUS, unmgCentroidRadius))
                    {
                        ReportMsg(this, new ReportMessage("Failed to Set centroid radius", MsgTypes.MSG_ERROR));
                    }
                    else
                    {
                        if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_CENTROIDS_COUNT, unmgCentroidCount))
                        {
                            ReportMsg(this, new ReportMessage("Failed to Set centroid count", MsgTypes.MSG_ERROR));
                        }
                        else //all is well
                        {
                            ReportMsg(this, new ReportMessage(string.Format("Successfully set centroid state to {0}, Count = {1}, Radius = {2}",
                                                                             enableState, centroidCnt, CentroidRadius), MsgTypes.MSG_STATUS));
                            IsCentroidEnabled = true;
                            m_centroid.CurrentCount = centroidCnt;
                            returnVal = true;


                        }

                    }

                }
            }
            else
            {
                Marshal.WriteInt16(unmgCentroidState, 0);

                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_CENTROIDS_ENABLED, unmgCentroidState))
                {
                    ReportMsg(this, new ReportMessage("Failed to disable Centroid feature", MsgTypes.MSG_ERROR));
                }
                IsCentroidEnabled = false;

            }

            Marshal.FreeHGlobal(unmgCentroidState);
            unmgCentroidState = IntPtr.Zero;

            Marshal.FreeHGlobal(unmgCentroidCount);
            unmgCentroidCount = IntPtr.Zero;

            Marshal.FreeHGlobal(unmgCentroidRadius);
            unmgCentroidRadius = IntPtr.Zero;

            return returnVal;

        }


        //Reads Exposure time range min -Max exposure time, This parameter is always available
        //But some older camera don't return the correct value - min vlaue is set to 0 and max to
        //32 bit value for backward compatibilty.
        public void ReadExpTimeRange()
        {
            IntPtr unmgMaxExpTime = Marshal.AllocHGlobal(sizeof(Int64));
            IntPtr unmgMinExpTime = Marshal.AllocHGlobal(sizeof(Int64));
            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_EXPOSURE_TIME, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmgMaxExpTime))
            {
                ReportMsg(this, new ReportMessage("Reading Max. Exposure time Value failed", MsgTypes.MSG_ERROR));
            }
            else
            {
                m_expTimeMax = Convert.ToUInt64(Marshal.ReadInt64(unmgMaxExpTime));
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_EXPOSURE_TIME, (Int16)PvTypes.AttributeIDs.ATTR_MIN, unmgMinExpTime))
                {
                    ReportMsg(this, new ReportMessage("Reading Min. Exposure time Value failed", MsgTypes.MSG_ERROR));
                }
                else
                {
                    m_expTimeMin = Convert.ToUInt64(Marshal.ReadInt64(unmgMinExpTime));
                    ReportMsg(this, new ReportMessage("Reading Exposure time limits complete", MsgTypes.MSG_STATUS));
                }
            }

            //free unmanaged memory
            Marshal.FreeHGlobal(unmgMinExpTime);
            unmgMinExpTime = IntPtr.Zero;

            Marshal.FreeHGlobal(unmgMaxExpTime);
            unmgMaxExpTime = IntPtr.Zero;
        }

        //Gets Estimated readout time, this should be called after setting up the
        //pl_exp_setup_seq or pl_exp_setup_cont before the camera will calculate the
        //readout time for the new settings.Not all cameras support it so check before reading
        public void GetEstReadoutTime()
        {
            if (isParamAvailable(PvTypes.PARAM_READOUT_TIME))
            {
                IntPtr unmgReadoutTime = Marshal.AllocHGlobal(sizeof(Int32));
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_READOUT_TIME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmgReadoutTime))
                {
                    ReportMsg(this, new ReportMessage("Reading Readout time failed", MsgTypes.MSG_ERROR));
                }
                else
                {
                    m_readoutTime = (UInt32)Marshal.ReadInt32(unmgReadoutTime);
                }
                Marshal.FreeHGlobal(unmgReadoutTime);
                unmgReadoutTime = IntPtr.Zero;
            }
            else //Parameter not available set radout time to 0
            {
                m_readoutTime = 0;
            }
        }

        //Reads post processing features on the camera and stores in structure.
        private void ReadPostProcessingFeatures()
        {
            Int32 i, j; //Loop variables
            string featureName;
            string functionName;
            UInt32 featureID;
            UInt32 functionID;
            UInt32 featCount;
            UInt32 functionCount;
            UInt32 minValue, maxValue, defValue, currentValue;

            if (!isParamAvailable(PvTypes.PARAM_PP_INDEX))
            {
                m_isPostProcessingAvail = false;
                return;
            }
            m_isPostProcessingAvail = true;
            //Clear the current feature list
            m_ppFeatureList.Clear();

            //Get post processing count
            IntPtr unmgPPCount = Marshal.AllocHGlobal(sizeof(Int32));

            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_INDEX, (Int16)PvTypes.AttributeIDs.ATTR_COUNT, unmgPPCount))
            {
                ReportMsg(this, new ReportMessage("Error while reading post processing count", MsgTypes.MSG_ERROR));
                //unallocate the memory and return
                Marshal.FreeHGlobal(unmgPPCount);
                unmgPPCount = IntPtr.Zero;
                return;
            }

            featCount = (UInt32)Marshal.ReadInt32(unmgPPCount);

            //free unmanaged memory
            Marshal.FreeHGlobal(unmgPPCount);
            unmgPPCount = IntPtr.Zero;

            PP_Feature ppFeature = new PP_Feature();

            for (i = 0; i < featCount; i++)
            {
                IntPtr unmgFeatureIndex = Marshal.AllocHGlobal(sizeof(Int32));
                Marshal.WriteInt32(unmgFeatureIndex, i);

                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_INDEX, unmgFeatureIndex))
                {
                    ReportMsg(this, new ReportMessage("Error setting PP_INDEX", MsgTypes.MSG_ERROR));
                    //unallocate the memory and return
                    Marshal.FreeHGlobal(unmgFeatureIndex);
                    unmgFeatureIndex = IntPtr.Zero;
                    return;
                }

                Marshal.FreeHGlobal(unmgFeatureIndex);
                unmgFeatureIndex = IntPtr.Zero;

                //Get Feature name
                IntPtr unmngFeatureName = Marshal.AllocHGlobal(PvTypes.PARAM_NAME_LEN);

                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_FEAT_NAME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngFeatureName))
                {
                    ReportMsg(this, new ReportMessage("Error getting PP feature name", MsgTypes.MSG_ERROR));
                    //unallocate the memory and return
                    Marshal.FreeHGlobal(unmngFeatureName);
                    unmngFeatureName = IntPtr.Zero;
                    return;
                }

                featureName = Marshal.PtrToStringAnsi(unmngFeatureName, PvTypes.PARAM_NAME_LEN);

                //free unmanaged pointers
                Marshal.FreeHGlobal(unmngFeatureName);
                unmngFeatureName = IntPtr.Zero;

                IntPtr unmngFeaturID = Marshal.AllocHGlobal(sizeof(Int32));

                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_FEAT_ID, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngFeaturID))
                {
                    ReportMsg(this, new ReportMessage("Error getting PP feature ID", MsgTypes.MSG_ERROR));
                    //unallocate the memory and return
                    Marshal.FreeHGlobal(unmngFeaturID);
                    unmngFeaturID = IntPtr.Zero;
                    return;
                }

                featureID = (UInt32)Marshal.ReadInt32(unmngFeaturID);

                //free unmanaged pointers
                Marshal.FreeHGlobal(unmngFeaturID);
                unmngFeaturID = IntPtr.Zero;

                //determine # of functions in this feature
                IntPtr unmgFuctCount = Marshal.AllocHGlobal(sizeof(Int32));

                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_PARAM_INDEX, (Int16)PvTypes.AttributeIDs.ATTR_COUNT, unmgFuctCount))
                {
                    ReportMsg(this, new ReportMessage("Error getting function count", MsgTypes.MSG_ERROR));
                    //unallocate the memory and return
                    Marshal.FreeHGlobal(unmgFuctCount);
                    unmgFuctCount = IntPtr.Zero;
                    return;
                }

                functionCount = (UInt32)Marshal.ReadInt32(unmgFuctCount);

                //free unmanaged memory
                Marshal.FreeHGlobal(unmgFuctCount);
                unmgFuctCount = IntPtr.Zero;

                //List pp_Function structures
                List<PP_Function> ppFunctionList = new List<PP_Function>();

                //Loop through functions
                for (j = 0; j < functionCount; j++)
                {
                    IntPtr unmgFunctionIndex = Marshal.AllocHGlobal(sizeof(Int32));
                    Marshal.WriteInt32(unmgFunctionIndex, j);

                    if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_PARAM_INDEX, unmgFunctionIndex))
                    {
                        ReportMsg(this, new ReportMessage("Error setting PP_INDEX- " + featureName, MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmgFunctionIndex);
                        unmgFunctionIndex = IntPtr.Zero;
                        return;
                    }

                    Marshal.FreeHGlobal(unmgFunctionIndex);
                    unmgFunctionIndex = IntPtr.Zero;

                    //Get name and id of the current function
                    //Get Feature name
                    IntPtr unmngFunctionName = Marshal.AllocHGlobal(PvTypes.PARAM_NAME_LEN);

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_PARAM_NAME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngFunctionName))
                    {
                        ReportMsg(this, new ReportMessage("Error getting PP PARAM name", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmngFunctionName);
                        unmngFunctionName = IntPtr.Zero;
                        return;
                    }

                    functionName = Marshal.PtrToStringAnsi(unmngFunctionName, PvTypes.PARAM_NAME_LEN);

                    //free unmanaged pointers
                    Marshal.FreeHGlobal(unmngFunctionName);
                    unmngFunctionName = IntPtr.Zero;

                    IntPtr unmngFunctionID = Marshal.AllocHGlobal(sizeof(Int32));

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_PARAM_ID, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngFunctionID))
                    {
                        ReportMsg(this, new ReportMessage("Error getting PP feature ID", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmngFunctionID);
                        unmngFunctionID = IntPtr.Zero;
                        return;
                    }

                    functionID = (UInt32)Marshal.ReadInt32(unmngFunctionID);

                    //free unmanaged pointers
                    Marshal.FreeHGlobal(unmngFunctionID);
                    unmngFunctionID = IntPtr.Zero;

                    //Get min/max/def/current values of function
                    IntPtr unmngValue = Marshal.AllocHGlobal(sizeof(Int32));

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_PARAM, (Int16)PvTypes.AttributeIDs.ATTR_MIN, unmngValue))
                    {
                        ReportMsg(this, new ReportMessage("Error getting Min value of function", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmngValue);
                        unmngValue = IntPtr.Zero;
                        return;
                    }
                    minValue = (UInt32)Marshal.ReadInt32(unmngValue);

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_PARAM, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmngValue))
                    {
                        ReportMsg(this, new ReportMessage("Error getting Min value of function", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmngValue);
                        unmngValue = IntPtr.Zero;
                        return;
                    }

                    maxValue = (UInt32)Marshal.ReadInt32(unmngValue);

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_PARAM, (Int16)PvTypes.AttributeIDs.ATTR_DEFAULT, unmngValue))
                    {
                        ReportMsg(this, new ReportMessage("Error getting Min value of function", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmngValue);
                        unmngValue = IntPtr.Zero;
                        return;
                    }

                    defValue = (UInt32)Marshal.ReadInt32(unmngValue);

                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PP_PARAM, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngValue))
                    {
                        ReportMsg(this, new ReportMessage("Error getting Min value of function", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmngValue);
                        unmngValue = IntPtr.Zero;
                        return;
                    }

                    currentValue = (UInt32)Marshal.ReadInt32(unmngValue);

                    //Free unmanaged memory
                    Marshal.FreeHGlobal(unmngValue);
                    unmngValue = IntPtr.Zero;

                    PP_Function ppFunction = new PP_Function();
                    ppFunction.ID = functionID;
                    ppFunction.Name = functionName;
                    ppFunction.MaxValue = maxValue;
                    ppFunction.DefValue = defValue;
                    ppFunction.MinValue = minValue;
                    ppFunction.CurrentVal = currentValue;

                    //Add to function list
                    ppFunctionList.Add(ppFunction);
                } //function loop

                ppFeature.ID = featureID;
                ppFeature.Name = featureName;
                ppFeature.FunctionList = ppFunctionList;
                m_ppFeatureList.Add(ppFeature);
            } //Feature loop
        }

        //Write updated post processing features to the camera
        //Only current values needs to be updated
        //Called from post processing feature form
        public Boolean UpdatePostProceesingFeature(List<PP_Feature> ppList)
        {
            for (Int32 i = 0; i < ppList.Count; i++)
            {
                IntPtr unmgFeatureIndex = Marshal.AllocHGlobal(sizeof(Int32));
                Marshal.WriteInt32(unmgFeatureIndex, i);

                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_INDEX, unmgFeatureIndex))
                {
                    ReportMsg(this, new ReportMessage("Error setting PP_INDEX", MsgTypes.MSG_ERROR));
                    //unallocate the memory and return
                    Marshal.FreeHGlobal(unmgFeatureIndex);
                    unmgFeatureIndex = IntPtr.Zero;
                    return false;
                }

                Marshal.FreeHGlobal(unmgFeatureIndex);
                unmgFeatureIndex = IntPtr.Zero;

                for (Int32 j = 0; j < ppList[i].FunctionList.Count; j++)
                {
                    IntPtr unmgFunctionIndex = Marshal.AllocHGlobal(sizeof(Int32));
                    Marshal.WriteInt32(unmgFunctionIndex, j);

                    if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_PARAM_INDEX, unmgFunctionIndex))
                    {
                        ReportMsg(this, new ReportMessage("Error setting PP_INDEX", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmgFunctionIndex);
                        unmgFunctionIndex = IntPtr.Zero;
                        return false;
                    }

                    Marshal.FreeHGlobal(unmgFunctionIndex);
                    unmgFunctionIndex = IntPtr.Zero;

                    //Now set the value
                    IntPtr unmngValue = Marshal.AllocHGlobal(sizeof(Int32));
                    Marshal.WriteInt32(unmngValue, (Int32)ppList[i].FunctionList[j].CurrentVal);

                    if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_PARAM, unmngValue))
                    {
                        ReportMsg(this, new ReportMessage("Error getting Min value of function", MsgTypes.MSG_ERROR));
                        //unallocate the memory and return
                        Marshal.FreeHGlobal(unmngValue);
                        unmngValue = IntPtr.Zero;
                        return false;
                    }
                    //unallocate the memory and return
                    Marshal.FreeHGlobal(unmngValue);
                    unmngValue = IntPtr.Zero;
                }
            }
            //Update member variable, no need to read back
            m_ppFeatureList = ppList;
            //read things back to update
            //ReadPostProcessingFeatures();
            return true;
        }

        //Write updated post processing features/function to the camera 
        public void WritePostProcessingFeature(Int32 featidx, Int32 funcidx, Int32 value)
        {
            IntPtr unmgFeatureIndex = Marshal.AllocHGlobal(sizeof(Int32));
            Marshal.WriteInt32(unmgFeatureIndex, featidx);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_INDEX, unmgFeatureIndex))
            {
                ReportMsg(this, new ReportMessage("Error setting PP_INDEX", MsgTypes.MSG_ERROR));
                //unallocate the memory and return
                Marshal.FreeHGlobal(unmgFeatureIndex);
                unmgFeatureIndex = IntPtr.Zero;
                return;
            }

            Marshal.FreeHGlobal(unmgFeatureIndex);
            unmgFeatureIndex = IntPtr.Zero;

            IntPtr unmgFunctionIndex = Marshal.AllocHGlobal(sizeof(Int32));
            Marshal.WriteInt32(unmgFunctionIndex, funcidx);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_PARAM_INDEX, unmgFunctionIndex))
            {
                ReportMsg(this, new ReportMessage("Error setting PP_INDEX", MsgTypes.MSG_ERROR));
                //unallocate the memory and return
                Marshal.FreeHGlobal(unmgFunctionIndex);
                unmgFunctionIndex = IntPtr.Zero;
                return;
            }

            Marshal.FreeHGlobal(unmgFunctionIndex);
            unmgFunctionIndex = IntPtr.Zero;

            //Now set the value
            IntPtr unmngValue = Marshal.AllocHGlobal(sizeof(Int32));
            Marshal.WriteInt32(unmngValue, value);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PP_PARAM, unmngValue))
            {
                ReportMsg(this, new ReportMessage("Error getting Min value of function", MsgTypes.MSG_ERROR));
                //unallocate the memory and return
                Marshal.FreeHGlobal(unmngValue);
                unmngValue = IntPtr.Zero;
                return;
            }
            //unallocate the memory and return
            Marshal.FreeHGlobal(unmngValue);
            unmngValue = IntPtr.Zero;

            PP_Feature ppFeature = m_ppFeatureList[featidx];
            PP_Function ppFunction = ppFeature.FunctionList[funcidx];
            ppFunction.CurrentVal = (UInt32)value;

            m_ppFeatureList[featidx] = ppFeature;
            string featName = ppFeature.Name.Substring(0, ppFeature.Name.IndexOf('\0'));
            string funcName = ppFunction.Name.Substring(0, ppFunction.Name.IndexOf('\0'));
            String msg = "Feature " + featName + " parameter " + funcName + "value set to " + value.ToString();

            ReportMsg(this, new ReportMessage(msg, MsgTypes.MSG_STATUS));

            return;
        }


        //Get CCD/Camera information and return in form of string list with parameter name
        //and value ,Called from CCD info Form which displays the strings as it is
        public void GetCameraInformation(List<String> infoList)
        {
            String paramString;
            infoList.Clear();

            //Get Chip Name
            IntPtr unmngChipName = Marshal.AllocHGlobal(PvTypes.CCD_NAME_LEN);
            if (isParamAvailable(PvTypes.PARAM_CHIP_NAME))
            {
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CHIP_NAME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngChipName))
                {
                    ReportMsg(this, new ReportMessage("Error getting Chip Name", MsgTypes.MSG_ERROR));
                    paramString = "Chip Name : Error";
                }
                else
                {
                    paramString = String.Format("Chip Name : {0}", Marshal.PtrToStringAnsi(unmngChipName, PvTypes.CCD_NAME_LEN));
                }
            }
            else
            {
                //Set chip name as NA
                paramString = "Chip Name : NA";
            }

            Marshal.FreeHGlobal(unmngChipName);
            unmngChipName = IntPtr.Zero;

            infoList.Add(paramString);

            //Get System Name
            IntPtr unmngSystemName = Marshal.AllocHGlobal(PvTypes.MAX_SYSTEM_NAME_LEN);

            if (isParamAvailable(PvTypes.PARAM_SYSTEM_NAME))
            {
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_SYSTEM_NAME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngSystemName))
                {
                    ReportMsg(this, new ReportMessage("Error getting System Name", MsgTypes.MSG_ERROR));
                    paramString = "System Name : Error";
                }
                else
                {
                    paramString = String.Format("System Name : {0}", Marshal.PtrToStringAnsi(unmngSystemName, PvTypes.MAX_SYSTEM_NAME_LEN));
                }
            }
            else
            {
                //Set chip name as NA
                paramString = "System Name : NA";
            }

            Marshal.FreeHGlobal(unmngSystemName);
            unmngSystemName = IntPtr.Zero;

            infoList.Add(paramString);

            //Get Vendor Name
            IntPtr unmngVendorName = Marshal.AllocHGlobal(PvTypes.MAX_VENDOR_NAME_LEN);

            if (isParamAvailable(PvTypes.PARAM_VENDOR_NAME))
            {
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_VENDOR_NAME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngVendorName))
                {
                    ReportMsg(this, new ReportMessage("Error getting System Name", MsgTypes.MSG_ERROR));
                    paramString = "Vendor Name : Error";
                }
                else
                {
                    paramString = String.Format("Vendor Name : {0}", Marshal.PtrToStringAnsi(unmngVendorName, PvTypes.MAX_VENDOR_NAME_LEN));
                }
            }
            else
            {
                //Set chip name as NA
                paramString = "Vendor Name : NA";
            }

            Marshal.FreeHGlobal(unmngVendorName);
            unmngVendorName = IntPtr.Zero;

            infoList.Add(paramString);

            //Get Product Name
            IntPtr unmngProductName = Marshal.AllocHGlobal(PvTypes.MAX_PRODUCT_NAME_LEN);
            if (isParamAvailable(PvTypes.PARAM_PRODUCT_NAME))
            {
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PRODUCT_NAME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngProductName))
                {
                    ReportMsg(this, new ReportMessage("Error getting product Name", MsgTypes.MSG_ERROR));
                    paramString = "Product Name : Error";
                }
                else
                {
                    paramString = String.Format("Product Name : {0}", Marshal.PtrToStringAnsi(unmngProductName, PvTypes.MAX_PRODUCT_NAME_LEN));
                }
            }
            else
            {
                //Set chip name as NA
                paramString = "Product Name : NA";
            }

            Marshal.FreeHGlobal(unmngProductName);
            unmngProductName = IntPtr.Zero;

            infoList.Add(paramString);

            //Get Camera part #
            IntPtr unmngPartNum = Marshal.AllocHGlobal(PvTypes.MAX_CAM_PART_NUM_LEN);
            if (isParamAvailable(PvTypes.PARAM_CAMERA_PART_NUMBER))
            {
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CAMERA_PART_NUMBER, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngPartNum))
                {
                    ReportMsg(this, new ReportMessage("Error getting Camera part #", MsgTypes.MSG_ERROR));
                    paramString = "Camera Part # : Error";
                }
                else
                {
                    paramString = String.Format("Camera Part # : {0}", Marshal.PtrToStringAnsi(unmngPartNum, PvTypes.MAX_CAM_PART_NUM_LEN));
                }
            }
            else
            {
                //Set chip name as NA
                paramString = "Camera Part # : NA";
            }

            Marshal.FreeHGlobal(unmngPartNum);
            unmngPartNum = IntPtr.Zero;

            infoList.Add(paramString);

            //Camera Head Serial #
            IntPtr unmngSerial = Marshal.AllocHGlobal(PvTypes.MAX_ALPHA_SER_NUM_LEN);

            if (isParamAvailable(PvTypes.PARAM_HEAD_SER_NUM_ALPHA))
            {
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_HEAD_SER_NUM_ALPHA, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngSerial))
                {
                    ReportMsg(this, new ReportMessage("Error getting Camera Serial #", MsgTypes.MSG_ERROR));
                    paramString = "Camera Serial # : Error";
                }
                else
                {
                    paramString = String.Format("Camera Serial # : {0}", Marshal.PtrToStringAnsi(unmngSerial, PvTypes.MAX_ALPHA_SER_NUM_LEN));
                }
            }
            else
            {
                //Set chip name as NA
                paramString = "Camera Serial # : NA";
            }

            infoList.Add(paramString);

            Marshal.FreeHGlobal(unmngSerial);
            unmngSerial = IntPtr.Zero;

            //PVCAM version is already available
            infoList.Add("PvCam Version: " + PVCamVersion);

            //Camera Firmware version
            IntPtr unmngFwVersion;
            unmngFwVersion = Marshal.AllocHGlobal(sizeof(UInt16));

            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CAM_FW_VERSION, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngFwVersion))
            {
                ReportMsg(this, new ReportMessage("Firmware version retrieval failed", MsgTypes.MSG_ERROR));
                paramString = "Camera firmware: NA";
            }
            else
            {
                UInt16 firmwareVersion = (UInt16)Marshal.ReadInt16(unmngFwVersion);
                UInt16 fwHighByte = (UInt16)((firmwareVersion & 0xFF00) >> 8);
                UInt16 fwLowByte = (UInt16)(firmwareVersion & 0xFF);
                paramString = String.Format("Camera firmware: {0}.{1}", fwHighByte, fwLowByte);
            }

            infoList.Add(paramString);

            Marshal.FreeHGlobal(unmngFwVersion);
            unmngFwVersion = IntPtr.Zero;

            //Color mode
            IntPtr unmngColorMode;
            unmngColorMode = Marshal.AllocHGlobal(sizeof(Int32));
            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_COLOR_MODE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngColorMode))
            {
                ReportMsg(this, new ReportMessage("Camera Color Mode retrieval failed", MsgTypes.MSG_ERROR));
                paramString = "Color Mode: Error";
            }
            else
            {
                Int32 colorMode = Marshal.ReadInt32(unmngColorMode);
                //as this is enum paramter. Change enum to string for display
                //this matches the enum definition in PvTypes
                String[] colorModeList = {"COLOR_NONE",
                                    "COLOR_RESERVED",
                                    "COLOR_RGGB",
                                    "COLOR_GRBG",
                                    "COLOR_GBRG",
                                    "COLOR_BGGR"};
                paramString = String.Format("Color Mode: {0}", colorModeList[colorMode]);
            }

            infoList.Add(paramString);

            Marshal.FreeHGlobal(unmngColorMode);
            unmngColorMode = IntPtr.Zero;

            //CCD Size is already available
            infoList.Add(String.Format("CCD Size: {0} x {1}", m_xSize, m_ySize));

            //Pixel Size
            IntPtr unmgPixelSize = Marshal.AllocHGlobal(sizeof(Int16));
            Int16 serPixelSize;
            Int16 parPixelSize;

            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PIX_SER_SIZE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmgPixelSize))
            {
                ReportMsg(this, new ReportMessage("Serial Pixel Size retrieval failed", MsgTypes.MSG_ERROR));
                paramString = "Pixel Size : Error";
            }
            else
            {
                serPixelSize = Marshal.ReadInt16(unmgPixelSize);
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PIX_PAR_SIZE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmgPixelSize))
                {
                    ReportMsg(this, new ReportMessage("Parallel Pixel Size retrieval failed", MsgTypes.MSG_ERROR));
                    paramString = "Pixel Size : Error";
                }
                else
                {
                    parPixelSize = Marshal.ReadInt16(unmgPixelSize);
                    paramString = String.Format("Pixel Size : {0} x {1} um", serPixelSize / 1000.0, parPixelSize / 1000.0);
                }
            }

            infoList.Add(paramString);

            Marshal.FreeHGlobal(unmgPixelSize);
            unmgPixelSize = IntPtr.Zero;
        }

        //retrieve camera parameters after it is opened
        public void ReadCameraParams()
        {
            m_readoutSpeeds.Clear();

            //find out if the sensor is the Frame Transfer type
            //follow the approach below to be 100% sure you detected the type correctly
            IntPtr unmngFrameCapable = Marshal.AllocHGlobal(sizeof(UInt16));
            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_FRAME_CAPABLE, (Int16)PvTypes.AttributeIDs.ATTR_AVAIL, unmngFrameCapable))
            {
                ReportMsg(this, new ReportMessage("Couldn't read PARAM_FRAME_CAPABLE attribute AVAILABLE", MsgTypes.MSG_ERROR));
                m_fTCapable = false;
            }
            else
            {
                if (Marshal.ReadInt16(unmngFrameCapable) == 1)
                {
                    if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_FRAME_CAPABLE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngFrameCapable))
                    {
                        ReportMsg(this, new ReportMessage("Couldn't read PARAM_FRAME_CAPABLE attribute CURRENT", MsgTypes.MSG_ERROR));
                        m_fTCapable = false;
                    }
                    else
                    {
                        m_fTCapable = Convert.ToBoolean(Marshal.ReadInt16(unmngFrameCapable));
                        if (m_fTCapable == true)
                        {
                            ReportMsg(this, new ReportMessage("Camera with frame transfer capability", MsgTypes.MSG_STATUS));
                        }
                        else
                        {
                            ReportMsg(this, new ReportMessage("Camera without frame transfer capability", MsgTypes.MSG_STATUS));
                        }
                    }
                }
                else
                {
                    ReportMsg(this, new ReportMessage("Camera without frame transfer capability", MsgTypes.MSG_STATUS));
                    m_fTCapable = false;
                }
            }
            Marshal.FreeHGlobal(unmngFrameCapable);
            unmngFrameCapable = IntPtr.Zero;

            //get serial (X) and parallel (Y) size of the CCD
            IntPtr unmngCcdSize = Marshal.AllocHGlobal(sizeof(Int16));
            PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PAR_SIZE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngCcdSize);
            m_ySize = Marshal.ReadInt16(unmngCcdSize);

            PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_SER_SIZE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngCcdSize);
            m_xSize = Marshal.ReadInt16(unmngCcdSize);

            PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_PIX_PAR_SIZE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngCcdSize);
            m_pixelSize = Marshal.ReadInt16(unmngCcdSize);
            Marshal.FreeHGlobal(unmngCcdSize);
            unmngCcdSize = IntPtr.Zero;

            ReportMsg(this, new ReportMessage(String.Format("CCD size: {0}x{1}", m_xSize, m_ySize), MsgTypes.MSG_STATUS));

            //read the camera chip name, currently used for main identification of the camera model
            IntPtr unmngChipName = Marshal.AllocHGlobal(PvTypes.CCD_NAME_LEN);
            PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CHIP_NAME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngChipName);
            m_chipName = new StringBuilder(Marshal.PtrToStringAnsi(unmngChipName, PvTypes.CCD_NAME_LEN));

            ReportMsg(this, new ReportMessage("Chip name: " + m_chipName, MsgTypes.MSG_STATUS));

            Marshal.FreeHGlobal(unmngChipName);
            unmngChipName = IntPtr.Zero;

            //read the camera firmware version
            UInt16 firmwareVersion;
            IntPtr unmngFwVersion;
            unmngFwVersion = Marshal.AllocHGlobal(sizeof(UInt16));

            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_CAM_FW_VERSION, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngFwVersion))
            {
                ReportMsg(this, new ReportMessage("Firmware version retrieval failed", MsgTypes.MSG_ERROR));
            }
            firmwareVersion = (UInt16)Marshal.ReadInt16(unmngFwVersion);

            
            IntPtr unmngADCoffset;
            unmngADCoffset = Marshal.AllocHGlobal(sizeof(UInt16));
            PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_ADC_OFFSET, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngADCoffset);
            m_ADCoffset = (UInt16)Marshal.ReadInt16(unmngADCoffset);

            //IntPtr unmngReadoutNoise;
            //unmngReadoutNoise = Marshal.AllocHGlobal(sizeof(UInt16));
            //PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_READ_NOISE, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngReadoutNoise);
            //m_read_noise = (UInt16)Marshal.ReadInt16(unmngReadoutNoise);

            IntPtr unmngbitDepth;
            unmngbitDepth = Marshal.AllocHGlobal(sizeof(UInt16));
            PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_BIT_DEPTH, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmngbitDepth);
            m_ADCoffset = (UInt16)Marshal.ReadInt16(unmngbitDepth);

            //PVCAM.pl_set_param(m_hCam,PvTypes.PARAM_ADC_OFFSET,)

            UInt16 fwHighByte = (UInt16)((firmwareVersion & 0xFF00) >> 8);
            UInt16 fwLowByte = (UInt16)(firmwareVersion & 0xFF);
            Marshal.FreeHGlobal(unmngFwVersion);

            ReportMsg(this, new ReportMessage(String.Format("Camera firmware: {0}.{1}", fwHighByte, fwLowByte), MsgTypes.MSG_STATUS));

            //find out whether the camera supports multiplication gain - typically all Frame Transfer cameras support EM (Multiplication)
            //gain while none of the Interline/sCMOS cameras have multiplication gain
            //NOTE: Interline/sCMOS camera often return their port description as can be seen in BuildSpeedTable() as "Multiplication gain"
            //which is a bug
            UInt16 multGainAvail = 0;
            IntPtr unmngMultGainAvail = Marshal.AllocHGlobal(sizeof(UInt16));
            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_GAIN_MULT_FACTOR, (Int16)PvTypes.AttributeIDs.ATTR_AVAIL, unmngMultGainAvail))
            {
                ReportMsg(this, new ReportMessage("Multiplication gain availability check failed", MsgTypes.MSG_ERROR));
            }

            multGainAvail = (UInt16)Marshal.ReadInt16(unmngMultGainAvail);

            if (multGainAvail == 0)
            {
                ReportMsg(this, new ReportMessage("Multiplication gain is not available", MsgTypes.MSG_STATUS));
                m_isMultGain = false;
            }
            else
            {
                //if multiplication gain is available find its range (1 to ATTR_MAX)
                m_isMultGain = true;
                IntPtr unmngMultGainMax;
                unmngMultGainMax = Marshal.AllocHGlobal(sizeof(UInt16));
                PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_GAIN_MULT_FACTOR, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmngMultGainMax);
                m_emGainMax = (UInt16)Marshal.ReadInt16(unmngMultGainMax);

                ////reset the EM gain to 1 upon camera opening
                //SetEMGain(1);
                ReportMsg(this, new ReportMessage("Multiplication gain is available", MsgTypes.MSG_STATUS));
            }

            Marshal.FreeHGlobal(unmngMultGainAvail);
            unmngMultGainAvail = IntPtr.Zero;

            IntPtr unmngGainIndex = Marshal.AllocHGlobal(sizeof(Int16));
            Marshal.WriteInt16(unmngGainIndex, 1);

            PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_GAIN_INDEX, unmngGainIndex);

            Marshal.FreeHGlobal(unmngGainIndex);
            unmngGainIndex = IntPtr.Zero;

            //detect whether the camera supports SMART streaming mode or not
            m_IsSmartStreamingSupported = isParamAvailable(PvTypes.PARAM_SMART_STREAM_MODE);

            //if SMART streaming is supported, read maximum number of exposures in the
            //SMART streaming exposure list
            if (m_IsSmartStreamingSupported)
            {
                //to read the maximum number of exposures in the list allocate smart_stream_type structure
                //the maximum possible exposures will be returned in the .entries field of the
                //smart_stream_type variable
                PvTypes.smart_stream_type smartStreamVals = new PvTypes.smart_stream_type();
                IntPtr unmngSsStruct = Marshal.AllocHGlobal(Marshal.SizeOf(smartStreamVals));

                //send the SMART streaming structure to camera so we can receive back the same structure
                //with .entries field populated with the maximum number of possible exposures
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_SMART_STREAM_EXP_PARAMS, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmngSsStruct))
                {
                    ReportMsg(this, new ReportMessage("Reading maximum number of exposures in SMART streaming mode failed", MsgTypes.MSG_ERROR));
                }
                else
                {
                    //read the structure from unmanaged environment
                    smartStreamVals = (PvTypes.smart_stream_type)Marshal.PtrToStructure(unmngSsStruct, typeof(PvTypes.smart_stream_type));
                    ReportMsg(this, new ReportMessage(String.Format("Maximum number of exposures in SMART streaming list: {0}", smartStreamVals.entries), MsgTypes.MSG_STATUS));
                }
                Marshal.FreeHGlobal(unmngSsStruct);
                unmngSsStruct = IntPtr.Zero;
            }
            //Get Clocking modes supported by camera
            if (!ReadEnumeration(m_clockModeList, PvTypes.PARAM_PMODE))
            {
                ReportMsg(this, new ReportMessage("Failed to Get clocking Mode information", MsgTypes.MSG_ERROR));
            }
            else
            {
                ReportMsg(this, new ReportMessage("Clocking mode information done", MsgTypes.MSG_STATUS));
            }


            //build the speed table
            if (!BuildSpeedTable(m_hCam))
            {
                ReportMsg(this, new ReportMessage("Speed table build failed", MsgTypes.MSG_ERROR));
                CloseCamera();
                return;
            }
            else
            {
                ReportMsg(this, new ReportMessage("Speed table build done", MsgTypes.MSG_STATUS));
            }




            //Read Trigger input Modes available on the camera
            if (!ReadEnumeration(m_triggerModeList, PvTypes.PARAM_EXPOSURE_MODE))
            {
                ReportMsg(this, new ReportMessage("Failed to get trigger mode information", MsgTypes.MSG_ERROR));
            }
            else
            {
                ReportMsg(this, new ReportMessage("Reading Trigger modes information done", MsgTypes.MSG_STATUS));
            }

            //Check if Expose Out Parameter is available on the camera
            m_IsExposeOutModeSupported = false;  //start with false
            if (isParamAvailable(PvTypes.PARAM_EXPOSE_OUT_MODE))
            {
                if (!ReadEnumeration(m_expOutModeList, PvTypes.PARAM_EXPOSE_OUT_MODE))
                {
                    ReportMsg(this, new ReportMessage("Failed to get expose out information", MsgTypes.MSG_ERROR));
                }
                else
                {
                    ReportMsg(this, new ReportMessage("Reading Expose Out modes information done", MsgTypes.MSG_STATUS));
                    m_IsExposeOutModeSupported = true;
                }
            }

            //Read Clear Modes available
            if (m_clearModeList.Count == 0)
            {
                if (!ReadEnumeration(m_clearModeList, PvTypes.PARAM_CLEAR_MODE))
                {
                    ReportMsg(this, new ReportMessage("Failed to get Clearing mode information", MsgTypes.MSG_ERROR));
                }
                else
                {
                    ReportMsg(this, new ReportMessage("Reading clearing modes information done", MsgTypes.MSG_STATUS));
                }
            }
            //Read cooling parameter
            ReadCoolingParameters();

            //Check if Fan speed control is available on the camera 
            //ReadFanSpeedParameters();

            //Read if Extended binning factors are available then read them
            ReadExtBinningParameters();

            //Read Exposure time range
            ReadExpTimeRange();

            //Read Post processing features on the camera
           // ReadPostProcessingFeatures();

            //read ROI/MultiRoi/Metadata information
            ReadRegionParameters();

            //read Centroid informatiom
            ReadCentroidParameters();

            //Notify that parameters have been read
            CamNotif(this, new ReportEvent(CameraNotifications.CAMERA_PARAM_READOUT_COMPLETE));
        }

        //Get List of Trigger modes avaialbe on camera in string list for GUI
        public void GetTriggerModes(List<String> triggerModeList)
        {
            for (Int32 i = 0; i < m_triggerModeList.Count; i++)
            {
                triggerModeList.Add(m_triggerModeList[i].Name);
            }
        }

        //trigger mode is only applied when pl_exp_setup_seq() or pl_exp_setup_cont() is called
        //so just remember the value set in the UI for now
        public void SetTriggerMode(string triggerMode)
        {
            Int32 i;

            //we already have Name/value pair list of trigger mode available on the camera
            //Iterate through the list to find the value corresponding to the Name
            for (i = 0; i < m_triggerModeList.Count; i++)
            {
                if (m_triggerModeList[i].Name == triggerMode)
                {
                    m_triggerMode = m_triggerModeList[i].Value;
                    ReportMsg(this, new ReportMessage(String.Format("Trigger mode set to: {0}", triggerMode), MsgTypes.MSG_STATUS));
                    break;
                }
            }
            //if value not found, report error and set mode to timed mode

            if (i >= m_triggerModeList.Count)
            {
                ReportMsg(this, new ReportMessage(String.Format("Trigger mode: {0} not found, set to Timed", triggerMode), MsgTypes.MSG_ERROR));
                m_triggerMode = (Int32)PvTypes.ExposureModes.TIMED_MODE;
            }
        }

        //Get ExtBinning info in form of string list from camera, Called from GUI
        public void GetExtBinnings(List<String> extBinList)
        {
            //How many binnings?, get min of paralle and serial NVP
            for (Int32 i = 0; i < (Math.Min(m_binningSerList.Count, m_binningParList.Count)); i++)
            {
                //There is no need to re - create the string
                // from numeric values. Those string are the same for serial and
                // parallel NVP.
                extBinList.Add(m_binningSerList[i].Name);
            }
        }

        //Get List of Expose modes avaialbe on camera in string list for GUI
        public void GetExpOutModes(List<String> expOutModeList)
        {
            for (Int32 i = 0; i < m_expOutModeList.Count; i++)
            {
                expOutModeList.Add(m_expOutModeList[i].Name);
            }
        }

        //Expose out mode is only set when setting up exposure pl_exp_setup_seq() or pl_exp_setup_cont()
        //Just update the variable now in accordance with GUI
        public void SetExposeOutMode(String exposeMode)
        {
            Int32 i;
            //we already have Name/value pair list of expose out mode available on the camera
            //Iterate through the list to find the value corresponding to the Name
            for (i = 0; i < m_expOutModeList.Count; i++)
            {
                if (m_expOutModeList[i].Name == exposeMode)
                {
                    m_exposeOutMode = m_expOutModeList[i].Value;
                    ReportMsg(this, new ReportMessage(String.Format("Expose Out mode set to: {0}", exposeMode), MsgTypes.MSG_STATUS));
                    break;
                }
            }

            //if match not found - display error and set expose out mode to 0

            if (i >= m_expOutModeList.Count)
            {
                ReportMsg(this, new ReportMessage(String.Format("Expose out mode: {0} not found, set to 0 instead", exposeMode), MsgTypes.MSG_ERROR));
                m_exposeOutMode = 0;
            }
        }

        //Get clearing modes as list of strings for GUI, Read Name/Value pair list
        public void GetClearModes(List<String> clrModeList)
        {
            for (Int32 i = 0; i < m_clearModeList.Count; i++)
            {
                clrModeList.Add(m_clearModeList[i].Name);
            }
        }

        //Find value corresponding to the clear mode name and set clear mode as it is
        //directly written to camera when selected in the UI, so write it
        public void SetClearMode(string clearMode)
        {
            IntPtr unmngClearMode;

            unmngClearMode = Marshal.AllocHGlobal(sizeof(Int32));

            Int32 i;

            for (i = 0; i < m_clearModeList.Count; i++)
            {
                if (m_clearModeList[i].Name == clearMode)
                {
                    Marshal.WriteInt32(unmngClearMode, m_clearModeList[i].Value);
                    ClearModeIndex = (Int16)i;
                    break;
                }
            }

            //if corresponding value not found, report message and don't change anything

            if (i >= m_clearModeList.Count)
            {
                ReportMsg(this, new ReportMessage(String.Format("Clearing mode: {0} not found", clearMode), MsgTypes.MSG_ERROR));
            }
            else
            {
                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_CLEAR_MODE, unmngClearMode))
                {
                    ReportMsg(this, new ReportMessage("Setting Clearing mode failed", MsgTypes.MSG_ERROR));
                }
                else
                    ReportMsg(this, new ReportMessage(String.Format("Clearing mode set to {0}", clearMode), MsgTypes.MSG_STATUS));
            }

            Marshal.FreeHGlobal(unmngClearMode);
            unmngClearMode = IntPtr.Zero;
        }

        // Called from the main form to get fan speeds available to populate the combobox
        public void GetFanSpeedSetpoints(List<string> fanSpeedList)
        {
            for (Int32 i = 0; i < m_fanSpeedList.Count; i++)
            {
                fanSpeedList.Add(m_fanSpeedList[i].Name);
            }
        }

        //Set Fan speed - called from main form to set fan speed, loop through name value pair to find match
        public void SetFanSpeedSetpoint(string fanSpeed)

        {
            IntPtr unmngFanSpeed;
            unmngFanSpeed = Marshal.AllocHGlobal(sizeof(Int32));
            Int32 i;

            for (i = 0; i < m_fanSpeedList.Count; i++)
            {
                if (m_fanSpeedList[i].Name == fanSpeed)
                {
                    Marshal.WriteInt32(unmngFanSpeed, m_fanSpeedList[i].Value);
                    break;
                }
            }

            //if corresponding value not found, report message and don't change anything

            if (i >= m_fanSpeedList.Count)
            {
                ReportMsg(this, new ReportMessage(String.Format("Fan Speed Set point: {0} not found", fanSpeed), MsgTypes.MSG_ERROR));
            }
            else
            {
                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_FAN_SPEED_SETPOINT, unmngFanSpeed))
                {
                    ReportMsg(this, new ReportMessage("Setting Fan Speed setpoint failed", MsgTypes.MSG_ERROR));
                }
                else
                    ReportMsg(this, new ReportMessage(String.Format("Fan speed set to {0}", fanSpeed), MsgTypes.MSG_STATUS));
            }

            Marshal.FreeHGlobal(unmngFanSpeed);
            unmngFanSpeed = IntPtr.Zero;
        }

        //Read current temperature of the CCD from the camera and update variable
        public Boolean GetCurrentTemprature()
        {
            Boolean retValue = false;
            IntPtr unmngCurTemp;
            unmngCurTemp = Marshal.AllocHGlobal(sizeof(Int16));

            if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_TEMP, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngCurTemp))
            {
                ReportMsg(this, new ReportMessage("Getting current temperature failed", MsgTypes.MSG_ERROR));
                retValue = false;
            }
            else
            {
                m_currentTemperaure = Marshal.ReadInt16(unmngCurTemp);
                retValue = true;
            }

            Marshal.FreeHGlobal(unmngCurTemp);
            unmngCurTemp = IntPtr.Zero;

            return retValue;
        }

        //This is called from GUI to set the temperature setpoint of the camera.
        //Value is sent to camera
        public void SetTemperatureSetpoint(Int16 setPoint)
        {
            //Check if value is in range
            if (setPoint < m_tempSetpointMin || setPoint > m_tempSetpointMax)
            {
                ReportMsg(this, new ReportMessage("Setpoint is out of range, Can not set", MsgTypes.MSG_ERROR));
                return;
            }
            IntPtr unmngSetpoint;
            unmngSetpoint = Marshal.AllocHGlobal(sizeof(Int16));
            Marshal.WriteInt16(unmngSetpoint, setPoint);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_TEMP_SETPOINT, unmngSetpoint))
            {
                ReportMsg(this, new ReportMessage("Setting temperature setpoint has failed", MsgTypes.MSG_ERROR));
            }
            else
            {
                ReportMsg(this, new ReportMessage(String.Format("Temperature setpoint Changed to  {0:0.00}", setPoint / 100.0), MsgTypes.MSG_STATUS));
            }
            Marshal.FreeHGlobal(unmngSetpoint);
            unmngSetpoint = IntPtr.Zero;
        }

        //Get Clocking mode as list of strings from Clocking mode Name/Value pair list.
        //Used to populate the GUI

        public void GetCockingModes(List<String> clkModeList)
        {
            for (Int32 i = 0; i < m_clockModeList.Count; i++)
            {
                clkModeList.Add(m_clockModeList[i].Name);
            }
        }

        //clocking mode is written to camera directly, so set it as soon as it is selected in the UI
        // Iterate through Name/Value pair of clocking modes read from the camera to find matching value for the
        //name in GUI and send it to the camera right away.
        public void SetClockingMode(string clockingMode)
        {
            IntPtr unmngClockMode;

            unmngClockMode = Marshal.AllocHGlobal(sizeof(Int32));

            Int32 i;

            for (i = 0; i < m_clockModeList.Count; i++)
            {
                if (m_clockModeList[i].Name == clockingMode)
                {
                    Marshal.WriteInt32(unmngClockMode, m_clockModeList[i].Value);
                    m_ClockingModeIndex = (Int16)i;
                    break;
                }
            }

            //if corresponding value not found, report message and don't change anything
            if (i >= m_clockModeList.Count)
            {
                ReportMsg(this, new ReportMessage(String.Format("Clocking mode: {0} not found", clockingMode), MsgTypes.MSG_ERROR));
            }
            else
            {
                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_PMODE, unmngClockMode))
                {
                    ReportMsg(this, new ReportMessage("Setting clocking mode failed", MsgTypes.MSG_ERROR));
                }
                else
                    ReportMsg(this, new ReportMessage(String.Format("Clocking mode set to {0}", clockingMode), MsgTypes.MSG_STATUS));
            }

            Marshal.FreeHGlobal(unmngClockMode);
            unmngClockMode = IntPtr.Zero;
        }

        //set number of clear cycles, write directly to the camera
        //default value is 2 and this value rarely needs to be changed
        public void SetClearCycles(Int16 clearCycles)
        {
            IntPtr unmngClearCycles;
            ClearCycles = clearCycles;
            unmngClearCycles = Marshal.AllocHGlobal(sizeof(UInt16));
            Marshal.WriteInt16(unmngClearCycles, clearCycles);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_CLEAR_CYCLES, unmngClearCycles))
                ReportMsg(this, new ReportMessage("Setting clear cycles failed", MsgTypes.MSG_ERROR));
            else
                ReportMsg(this, new ReportMessage(String.Format("Number of clear cycles set to {0}", clearCycles), MsgTypes.MSG_STATUS));

            Marshal.FreeHGlobal(unmngClearCycles);
            unmngClearCycles = IntPtr.Zero;
        }

        public void SetADCoffset(Int16 offset)
        {
            IntPtr unmngADCOffset;

            unmngADCOffset = Marshal.AllocHGlobal(sizeof(UInt16));
            Marshal.WriteInt16(unmngADCOffset, offset);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_ADC_OFFSET, unmngADCOffset))
                ReportMsg(this, new ReportMessage("Setting adc offset failed", MsgTypes.MSG_ERROR));
            else
                ReportMsg(this, new ReportMessage(String.Format("ADC Offset set to {0}", offset), MsgTypes.MSG_STATUS));

            Marshal.FreeHGlobal(unmngADCOffset);
            unmngADCOffset = IntPtr.Zero;
        }
        //set the camera readout speed
        public bool SetReadoutSpeed(Int16 spdTblIndex)
        {
            IntPtr unmngValue;
            //select the port and speed index according to the speed table index selected by the user
            Int32 port = m_spdTable.ReadoutOption[spdTblIndex].Port;
            Int16 speedIndex = m_spdTable.ReadoutOption[spdTblIndex].Speed;
            m_speedTableIndex = spdTblIndex;
            unmngValue = Marshal.AllocHGlobal(sizeof(Int32));
            Marshal.WriteInt32(unmngValue, port);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_READOUT_PORT, unmngValue))
            {
                ReportMsg(this, new ReportMessage("Setting readout port failed", MsgTypes.MSG_ERROR));
                Marshal.FreeHGlobal(unmngValue);
                return false;
            }

            Marshal.FreeHGlobal(unmngValue);
            unmngValue = IntPtr.Zero;

            unmngValue = Marshal.AllocHGlobal(sizeof(Int16));
            Marshal.WriteInt32(unmngValue, speedIndex);

            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_SPDTAB_INDEX, unmngValue))
            {
                ReportMsg(this, new ReportMessage("Setting readout speed failed", MsgTypes.MSG_ERROR));
                Marshal.FreeHGlobal(unmngValue);
                return false;
            }

            ReportMsg(this, new ReportMessage(String.Format("Readout set to {0}", m_spdTable.ReadoutOption[spdTblIndex].PortDesc), MsgTypes.MSG_STATUS));
            CamNotif(this, new ReportEvent(CameraNotifications.READOUT_SPEED_CHANGED));
            Marshal.FreeHGlobal(unmngValue);
            unmngValue = IntPtr.Zero;

            return true;
        }

        //set camera gain state (analog gain), write directly to the camera
        public bool SetGainState(Int16 gainState)
        {
            IntPtr unmngGainState;
            unmngGainState = Marshal.AllocHGlobal(sizeof(Int16));
            Marshal.WriteInt16(unmngGainState, (Int16)(gainState + 1));
            m_GainStateIndex = gainState;
            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_GAIN_INDEX, unmngGainState))
            {
                ReportMsg(this, new ReportMessage("Setting gain state failed", MsgTypes.MSG_ERROR));
                Marshal.FreeHGlobal(unmngGainState);
                return false;

            }
            Marshal.FreeHGlobal(unmngGainState);
            return true;
        }

        //set EM (Multiplication) gain, write directly to the camera
        public bool SetEMGain(UInt16 emGain)
        {
            IntPtr unmngEMGain;
            unmngEMGain = Marshal.AllocHGlobal(sizeof(UInt16));
            Marshal.WriteInt16(unmngEMGain, (Int16)emGain);
            EMGain = (Int16)emGain;
            if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_GAIN_MULT_FACTOR, unmngEMGain))
            {
                ReportMsg(this, new ReportMessage("pl_set_param(PARAM_GAIN_MULT_FACTOR) failed", MsgTypes.MSG_ERROR));
                Marshal.FreeHGlobal(unmngEMGain);
                return false;
            }
            ReportMsg(this, new ReportMessage(String.Format("EM Gain set to: {0}", emGain), MsgTypes.MSG_STATUS));
            Marshal.FreeHGlobal(unmngEMGain);
            return true;
        }

        //exposure time is only applied when pl_exp_setup_seq() or pl_exp_setup_cont() is called
        //so just remember the value for now
        public void SetExposureTime(UInt32 expTime)
        {
            m_exposureTime = expTime;
            ReportMsg(this, new ReportMessage(String.Format("Exposure time set to {0} ms", m_exposureTime), MsgTypes.MSG_STATUS));
        }

        //binning is only applied as part of the ROI when pl_exp_setup_seq() or pl_exp_setup_cont() is called
        //so just remember the value for now
        //if camera supports extended binning then string passed is actual binning description and get the correponding
        // ser/par binning factors from the NVP pair read from the camera
        //if camera does not support extended binning factor this number is simple serial/parallel binning factor
        public void SetBinning(String binning)
        {
            Binning = Convert.ToInt16(binning);
            UInt16 sBin = 1;
            UInt16 pBin = 1;
            Boolean matchFound = false;
            if (m_IsExtBinningSupported)
            {
                //loop through either ser or par NVP and find the corresponding values
                for (Int32 i = 0; i < (Math.Min(m_binningSerList.Count, m_binningParList.Count)); i++)
                {
                    //Just compare serial as string descriptions of ser/par are same
                    if (binning == m_binningSerList[i].Name)
                    {
                        sBin = (UInt16)m_binningSerList[i].Value;
                        pBin = (UInt16)m_binningParList[i].Value;
                        matchFound = true;
                        break;
                    }
                }
                if (!matchFound)
                {
                    ReportMsg(this, new ReportMessage("Binning value corresponding to the string not found", MsgTypes.MSG_ERROR));
                }
            }
            else // if camera does not support extending binning just convert the string to UInt16
            {
                sBin = Convert.ToUInt16(binning);
                pBin = Convert.ToUInt16(binning);
            }
            //Set same binning for all regions.
            for (int i = 0; i < CurrentROICount; i++)
            {
                m_region[i].sbin = sBin;
                m_region[i].pbin = pBin;
            }
            ReportMsg(this, new ReportMessage(String.Format("Binning changed to {0}x{1}", m_region[0].sbin, m_region[0].pbin), MsgTypes.MSG_STATUS));
        }

        //Set SMART streaming On or Off and set exposure time list
        public void SetSmartStreaming(String enableState, List<Int32> expTimes)
        {
            if (enableState == "On")
            {
                //SMART streaming uses a special structure that holds the number of exposures
                //that camera should cycle through and the exposure values
                //create and configure the structure
                PvTypes.smart_stream_type smartStreamVals = new PvTypes.smart_stream_type();

                //Check possible max exposure camera supports
                IntPtr unmngSsStruct = Marshal.AllocHGlobal(Marshal.SizeOf(smartStreamVals));

                //send the SMART streaming structure to camera so we can receive back the same structure
                //with .entries field populated with the maximum number of possible exposures
                if (!PVCAM.pl_get_param(m_hCam, PvTypes.PARAM_SMART_STREAM_EXP_PARAMS, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmngSsStruct))
                {
                    ReportMsg(this, new ReportMessage("Reading maximum number of exposures in SMART streaming mode failed", MsgTypes.MSG_ERROR));
                }
                else
                {
                    //read the structure from unmanaged environment
                    smartStreamVals = (PvTypes.smart_stream_type)Marshal.PtrToStructure(unmngSsStruct, typeof(PvTypes.smart_stream_type));
                    ReportMsg(this, new ReportMessage(String.Format("Maximum number of exposures in SMART streaming list: {0}", smartStreamVals.entries), MsgTypes.MSG_STATUS));
                }

                //Limit the exposure time if needed
                smartStreamVals.entries = (Int16)Math.Min(smartStreamVals.entries, expTimes.Count);

                Int32[] ssExposures = new Int32[smartStreamVals.entries];
                //fill exposure list with our exposure times
                for (Int32 i = 0; i < smartStreamVals.entries; i++)
                {
                    ssExposures[i] = expTimes[i];
                }

                IntPtr unmngExposures = Marshal.AllocHGlobal(sizeof(UInt32) * 4);

                Marshal.Copy(ssExposures, 0, unmngExposures, ssExposures.Length);
                smartStreamVals.parameters = unmngExposures;

                Marshal.StructureToPtr(smartStreamVals, unmngSsStruct, true);

                //send the SMART streaming structure with its values to the camera
                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_SMART_STREAM_EXP_PARAMS, unmngSsStruct))
                {
                    ReportMsg(this, new ReportMessage("Setting PARAM_SMART_STREAM_EXP_PARAMS failed", MsgTypes.MSG_ERROR));
                    Marshal.FreeHGlobal(unmngExposures);
                    Marshal.FreeHGlobal(unmngSsStruct);
                    return;
                }

                IntPtr unmngEnableSS;
                unmngEnableSS = Marshal.AllocHGlobal(sizeof(Int16));
                Marshal.WriteInt16(unmngEnableSS, 1);

                //enable SMART streaming on the camera
                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_SMART_STREAM_MODE_ENABLED, unmngEnableSS))
                {
                    ReportMsg(this, new ReportMessage("Enabling SMART streaming mode failed", MsgTypes.MSG_ERROR));
                    Marshal.FreeHGlobal(unmngExposures);
                    Marshal.FreeHGlobal(unmngSsStruct);
                    Marshal.FreeHGlobal(unmngEnableSS);
                    return;
                }
                m_IsSmartStreamingOn = true;
                String exposureList = "";
                for (Int32 i = 0; i < smartStreamVals.entries; i++)
                {
                    exposureList += String.Format("{0} ", expTimes[i]);
                }
                ReportMsg(this, new ReportMessage(String.Format("SMART streaming mode enabled, exposure times set to: {0} ms", exposureList), MsgTypes.MSG_STATUS));

                Marshal.FreeHGlobal(unmngExposures);
                Marshal.FreeHGlobal(unmngSsStruct);
                Marshal.FreeHGlobal(unmngEnableSS);
            }
            else if (enableState == "Off")
            {
                IntPtr unmngEnableSS;
                unmngEnableSS = Marshal.AllocHGlobal(sizeof(Int16));
                Marshal.WriteInt16(unmngEnableSS, 0);
                //disable SMART streaming on the camera
                if (!PVCAM.pl_set_param(m_hCam, PvTypes.PARAM_SMART_STREAM_MODE_ENABLED, unmngEnableSS))
                {
                    ReportMsg(this, new ReportMessage("Disabling SMART streaming mode failed", MsgTypes.MSG_ERROR));
                    Marshal.FreeHGlobal(unmngEnableSS);
                    return;
                }
                m_IsSmartStreamingOn = false;
                ReportMsg(this, new ReportMessage(String.Format("SMART streaming mode disabled", m_exposureTime), MsgTypes.MSG_STATUS));
                Marshal.FreeHGlobal(unmngEnableSS);
            }
        }

        //identify all readout ports and readout speeds
        //typically Frame Transfer cameras have one or two readout ports where port 1 would be
        //the EM (Multiplication) gain port and the port 2 would be Normal port (non EM gain, only analog gain).
        //Each of these ports has one or more readout speeds (e.g. 20MHz, 10MHz etc.)
        //Interline and sCMOS cameras usually only have one port with one or more readout speeds
        private bool BuildSpeedTable(Int16 hCam)
        {
            Int32 enumValue;
            StringBuilder desc = new StringBuilder(100);
            IntPtr unmngRdPortCount = Marshal.AllocHGlobal(sizeof(UInt32));
            IntPtr unmngRdPortSet = Marshal.AllocHGlobal(sizeof(UInt32));
            IntPtr unmngSpdTabIndexCount = Marshal.AllocHGlobal(sizeof(UInt32));
            IntPtr unmngBitDepth = Marshal.AllocHGlobal(sizeof(Int16));
            IntPtr unmngGainCount = Marshal.AllocHGlobal(sizeof(Int32));
            IntPtr unmngSpdTabIdx = Marshal.AllocHGlobal(sizeof(Int16));
            IntPtr unmngPixTime = Marshal.AllocHGlobal(sizeof(UInt16));
            IntPtr unmngGainMax = Marshal.AllocHGlobal(sizeof(UInt16));
            UInt16 gainMax;

            if (m_spdTable.ReadoutPorts==0)
            { 
            m_spdTable.ReadoutOption.Clear();

            //read number of available ports
            if (!PVCAM.pl_get_param(hCam, PvTypes.PARAM_READOUT_PORT, (Int16)PvTypes.AttributeIDs.ATTR_COUNT, unmngRdPortCount))
            {
                ReportMsg(this, new ReportMessage("Getting PARAM_READOUT_PORT attribute count failed", MsgTypes.MSG_ERROR));
                return false;
            }

            m_spdTable.ReadoutPorts = Convert.ToUInt32(Marshal.ReadInt32(unmngRdPortCount));
            Marshal.FreeHGlobal(unmngRdPortCount);
            unmngRdPortCount = IntPtr.Zero;

            //set each port and find number of readout speeds on that port
            for (Int16 i = 0; i < m_spdTable.ReadoutPorts; i++)
            {
                Marshal.WriteInt32(unmngRdPortSet, i);

                //set readout port
                if (!PVCAM.pl_set_param(hCam, PvTypes.PARAM_READOUT_PORT, unmngRdPortSet))
                {
                    ReportMsg(this, new ReportMessage("Setting PARAM_READOUT_PORT failed", MsgTypes.MSG_ERROR));
                    return false;
                }

                    //get port description
                    //NOTE: for Interline and sCMOS cameras this often returns "Mutliplication gain" even though the port does
                    //not have any multiplication gain capability
                  
                PVCAM.pl_get_enum_param(hCam, PvTypes.PARAM_READOUT_PORT, (UInt32)i, out enumValue, desc, 100);

                //get number of readout speeds on each readout port
                PVCAM.pl_get_param(hCam, PvTypes.PARAM_SPDTAB_INDEX, (Int16)PvTypes.AttributeIDs.ATTR_COUNT, unmngSpdTabIndexCount);
                m_spdTable.ReadoutSpeeds = Convert.ToUInt32(Marshal.ReadInt32(unmngSpdTabIndexCount));

                //for all readout speeds
                for (Int16 j = 0; j < m_spdTable.ReadoutSpeeds; j++)
                {
                    Marshal.WriteInt16(unmngSpdTabIdx, j);
                    if (!PVCAM.pl_set_param(hCam, PvTypes.PARAM_SPDTAB_INDEX, unmngSpdTabIdx))
                    {
                        ReportMsg(this, new ReportMessage("Setting PARAM_SPDTAB_INDEX failed", MsgTypes.MSG_ERROR));
                        return false;
                    }
                    //Get Number of gains available on this speed, note that use ATTR_MAX to find max gains available as ATTR_COUNT
                    //has issues in certain version of PVCAM
                    PVCAM.pl_get_param(hCam, PvTypes.PARAM_GAIN_INDEX, (Int16)PvTypes.AttributeIDs.ATTR_COUNT, unmngGainMax);
                    gainMax = (UInt16)Marshal.ReadInt16(unmngGainMax);

                    //get bit depth of the speed
                    PVCAM.pl_get_param(hCam, PvTypes.PARAM_BIT_DEPTH, (Int16)PvTypes.AttributeIDs.ATTR_MAX, unmngBitDepth);
                    m_bitDepth = Marshal.ReadInt16(unmngBitDepth);

                    //get readout frequency (pixel time) of the speed
                    PVCAM.pl_get_param(hCam, PvTypes.PARAM_PIX_TIME, (Int16)PvTypes.AttributeIDs.ATTR_CURRENT, unmngPixTime);
                    double pixTime = Convert.ToDouble(Marshal.ReadInt16(unmngPixTime));
                    ReportMsg(this, new ReportMessage(String.Format("Readout speed detected: {0}MHz, Port {1}, Speed index {2}, Bit Depth {3}-bit, Gain states: {4}, Description: {5}", 1000 / pixTime, i, j, m_bitDepth, gainMax, desc), MsgTypes.MSG_STATUS));

                    //add new readout option to our list of options
                    m_spdTable.ReadoutOption.Add(new ReadoutOption(i, j, m_bitDepth, gainMax, String.Format("{0}MHz, Port {1}, Speed index {2}, Bit Depth {3}-bit, Gain states: {4}, Description: {5}", 1000 / pixTime, i, j, m_bitDepth, gainMax, desc)));
                }
            }
            
                }
            CamNotif(this, new ReportEvent(CameraNotifications.SPEED_TABLE_BUILD_DONE));
            Marshal.FreeHGlobal(unmngRdPortSet);
            unmngRdPortSet = IntPtr.Zero;

            Marshal.FreeHGlobal(unmngSpdTabIndexCount);
            unmngSpdTabIndexCount = IntPtr.Zero;

            Marshal.FreeHGlobal(unmngBitDepth);
            unmngBitDepth = IntPtr.Zero;

            Marshal.FreeHGlobal(unmngGainCount);
            unmngGainCount = IntPtr.Zero;

            Marshal.FreeHGlobal(unmngSpdTabIdx);
            unmngSpdTabIdx = IntPtr.Zero;

            Marshal.FreeHGlobal(unmngPixTime);
            unmngPixTime = IntPtr.Zero;

            return true;
        }

        //close the camera
        public void CloseCamera()
        {
            ReportMsg(this, new ReportMessage("Closing camera...", MsgTypes.MSG_STATUS));

            //close the camera
            if (!PVCAM.pl_cam_close(m_hCam))
            {
                ReportMsg(this, new ReportMessage("Cannot close the camera", MsgTypes.MSG_ERROR));
            }
            else
            {
                ReportMsg(this, new ReportMessage("Camera closed successfully", MsgTypes.MSG_STATUS));
            }
        }

        //refresh available cameras list
        public static void RefreshCameras(PVCamCamera activeCamera)
        {
            //clear list of the cameras and readout speeds
            CameraList.Clear();
            activeCamera.m_readoutSpeeds.Clear();

            //make sure PVCAM is initialized
            PVCAM.pl_pvcam_uninit();
            if (!PVCAM.pl_pvcam_init())
            {
                activeCamera.ReportMsg(activeCamera, new ReportMessage("PVCAM init failed", MsgTypes.MSG_ERROR));
                return;
            }
            else
            {
                activeCamera.ReportMsg(activeCamera, new ReportMessage("PVCAM initialized", MsgTypes.MSG_STATUS));
            }

            //read PVCAM version
            UInt16 pvcamVersion;
            IntPtr unmngPvcamVersion;
            unmngPvcamVersion = Marshal.AllocHGlobal(sizeof(UInt16));

            if (!PVCAM.pl_pvcam_get_ver(out pvcamVersion))
            {
                activeCamera.ReportMsg(activeCamera, new ReportMessage("Couldn't obtain PVCAM version info", MsgTypes.MSG_ERROR));
            }
            else
            {
                PVCamCamera.PVCamVersion = String.Format("{0}.{1}.{2}", (Int32)(pvcamVersion & 0xFF00) >> 8, (Int32)(pvcamVersion & 0xF0) >> 4, (Int32)(pvcamVersion & 0xF));
                activeCamera.ReportMsg(activeCamera, new ReportMessage("PVCAM version: " + PVCamCamera.PVCamVersion, MsgTypes.MSG_STATUS));
            }
            Marshal.FreeHGlobal(unmngPvcamVersion);
            unmngPvcamVersion = IntPtr.Zero;

            //read number of avaialable (connected) cameras
            if (!PVCAM.pl_cam_get_total(out PVCamCamera.NrOfCameras))
            {
                activeCamera.ReportMsg(activeCamera, new ReportMessage("Couldn't obtain number of cameras in the system.", MsgTypes.MSG_ERROR));
                activeCamera.CamNotif(activeCamera, new ReportEvent(CameraNotifications.NO_CAMERA_FOUND));
                return;
            }

            activeCamera.ReportMsg(activeCamera, new ReportMessage(String.Format("Cameras found: {0}", PVCamCamera.NrOfCameras), MsgTypes.MSG_STATUS));

            //read string descriptions of each camera, these strings are later used to to open each camera
            Int16 listCnt = 0;
            while (listCnt < PVCamCamera.NrOfCameras)
            {
                StringBuilder cameraName = new StringBuilder(PvTypes.CAM_NAME_LEN);
                if (!PVCAM.pl_cam_get_name(listCnt, cameraName))
                {
                    activeCamera.ReportMsg(activeCamera, new ReportMessage(String.Format("Couldn't get camera {0} name", listCnt), MsgTypes.MSG_STATUS));
                }
                else
                {
                    activeCamera.ReportMsg(activeCamera, new ReportMessage(String.Format("Camera {0} name: {1}", listCnt, cameraName), MsgTypes.MSG_STATUS));
                    CameraList.Add(cameraName);
                }
                listCnt++;
            }

            activeCamera.CamNotif(activeCamera, new ReportEvent(CameraNotifications.CAMERA_REFRESH_DONE));
        }

        //notify the acquisition thread that the frame readout has completed
        private void EOFHandler(ref PvTypes.FRAME_INFO pFrameInfo, IntPtr context)
        {
            //for demonstration purposes you can inspect the acqParams values
            AcqParamsContext acqParams = (AcqParamsContext)Marshal.PtrToStructure(context, typeof(AcqParamsContext));

            //set the EOF event notifying the acquisition thread that a new frame has arrived
            m_EofEvent.Set();
        }

        //convert RAW image data to 8-bit grayscale BMP
        //since PixelFormat.Format16bppGrayScale is not supported at all by .NET
        //and PixelFormat.Format48bppRgb does not work correctly convert the image
        //to be displayed to 8-bit image.
        //To make a grayscale BMP with 32bpp RGB format each of the 4 bytes describing
        //every pixel has to be populated with the same pixel value (byte 4 is unused)
        //parameters- Array of data to be coverted, image width & Height, if data needs to be scaled for display
        public void FrameToBMP(ushort[] dataToConvert, Int32 width, Int32 height, Boolean isScaling)
        {


            double divisor;
            int value;
            if (isScaling)
            {
                //Image is scaled so that minimum pixel value in the image is converted to 0 and max pixel value is converted to max 8 bit number (255) 
                double rangeOriginal = Math.Pow(2, m_bitDepth); //Original image range depends upon camera bit depth
                double rangeScaled = Math.Pow(2, 8);            //range of 8 bit image 
                int offset;                                     //offset to be subtracted from pixel for scaling
                //if image is saturated or all 0s
                if (m_imgStats.Min == m_imgStats.Max)
                {
                    //Set divisor to simple ratio and we are not subtracting anything from the pixel
                    divisor = rangeOriginal / rangeScaled;
                    offset = 0;
                }
                else
                {
                    //adjust divisor based on min to max of image data and subtract min value from the pixel
                    divisor = (m_imgStats.Max - m_imgStats.Min) / rangeScaled;
                    offset = m_imgStats.Min;
                }
                //Scale each pixel
                for (Int32 i = 0, j = 0; i < dataToConvert.Length; i += 1, j += 4)
                {
                    value = (int)((dataToConvert[i] - offset) / divisor);
                    if (value >= rangeScaled) value = value - 1;
                    DataRGB[j] = DataRGB[j + 1] = DataRGB[j + 2] = DataRGB[j + 3] = Convert.ToByte(value);
                }

            }
            else
            {
                divisor = (UInt16)(Math.Pow(2, m_bitDepth) / Math.Pow(2, 8));

                for (Int32 i = 0, j = 0; i < dataToConvert.Length; i += 1, j += 4)
                {
                    value = (int)(dataToConvert[i] / divisor);
                    DataRGB[j] = DataRGB[j + 1] = DataRGB[j + 2] = DataRGB[j + 3] = Convert.ToByte(value);
                }
            }

            m_lastBMP = new Bitmap(width, height, width * 4, PixelFormat.Format32bppRgb, m_bmpHandle.AddrOfPinnedObject());
        }

        //getters/setters for PVCamCamera properties
        public GCHandle BmpHandle
        {
            get { return m_bmpHandle; }
            set { m_bmpHandle = value; }
        }

        public Object BmpLock
        {
            get { return m_bmpLock; }
            set { m_bmpLock = value; }
        }

        public Int16 BitDepth
        {
            get { return m_bitDepth; }
            set { m_bitDepth = value; }
        }

        public PvTypes.RegionType[] Region
        {
            get { return m_region; }
            set { m_region = value; }
        }

        public Int16 CurrentROICount
        {
            get { return m_currentROICount; }
            set { m_currentROICount = value; }
        }
        public Int16 MaxROICount
        {
            get { return m_maxROICount; }
        }

        public Boolean IsCentroidAvail
        {
            get { return m_isCentroidAvail; }
            set { m_isCentroidAvail = value; }
        }
        public Boolean IsCentroidEnabled
        {
            get { return m_isCentroid; }
            set { m_isCentroid = value; }
        }
        public Centroid CentroidInfo
        {
            get { return m_centroid; }
            set { m_centroid = value; }
        }

        public Bitmap LastBMP
        {
            get { return m_lastBMP; }
            set { m_lastBMP = value; }
        }

        public String OpenedCamName
        {
            get { return m_openedCamName; }
            set { m_openedCamName = value; }
        }

        public Boolean FTCapable
        {
            get { return m_fTCapable; }
            set { m_fTCapable = value; }
        }

        public Int32 FrameNumber
        {
            get { return m_frameNumber; }
            set { m_frameNumber = value; }
        }

        public Int32 FramesToGet
        {
            get { return m_framesToGet; }
            set
            {
                m_framesToGet = value;
                if (m_framesToGet == RUN_UNTIL_STOPPED)
                    ReportMsg(this, new ReportMessage("Frames to get set to run until stopped", MsgTypes.MSG_STATUS));
                else
                    ReportMsg(this, new ReportMessage(String.Format("Frames to get set to {0}", m_framesToGet), MsgTypes.MSG_STATUS));
            }
        }

        public UInt32 ExposureTime
        {
            get { return m_exposureTime; }
            set
            {
                if (value > 0 && value < 100000)
                    m_exposureTime = value;
                else
                {
                    MessageBox.Show("Please, enter value from 0 ms to 100000 ms", "Parameter incorrect");
                    m_exposureTime = 10;
                }
            }
        }

        public Int16 XSize
        {
            get { return m_xSize; }
            set { m_xSize = value; }
        }

        public Int16 YSize
        {
            get { return m_ySize; }
            set { m_ySize = value; }
        }

        public Int16 PixelSize
        {
            get { return m_pixelSize; }
            set { m_pixelSize = value; }
        }

        public Int16 ImageSizeX
        {
            get { return m_imageSizeX; }
            set { m_imageSizeX = value; }
        }

        public Int16 ImageSizeY
        {
            get { return m_imageSizeY; }
            set { m_imageSizeY = value; }
        }

        public Int16 SpeedTableIndex
        {
            get { return m_speedTableIndex; }
            set { m_speedTableIndex = value; }
        }


        public UInt16[] FrameDataShorts
        {
            get { return m_frameDataShorts; }
            set { m_frameDataShorts = value; }
        }

        public UInt16 MultGainMax
        {
            get { return m_emGainMax; }
            set { m_emGainMax = value; }
        }

        public Boolean IsMultGain
        {
            get { return m_isMultGain; }
            set { m_isMultGain = value; }
        }

        internal ImageStats ImgStats
        {
            get { return m_imgStats; }
            set { m_imgStats = value; }
        }

        internal FrameMetadata FrmMetadata
        {
            get { return m_frameMetadata; }
            set { m_frameMetadata = value; }
        }

        internal ROIMetadata[] RgnMetadata
        {
            get { return m_roiMetadata; }
            set { m_roiMetadata = value; }
        }

        public PvTypes.FRAME_INFO FrameInfoLatest
        {
            get { return m_frameInfoLatest; }
            set { m_frameInfoLatest = value; }
        }

        internal SpeedTable SpeedTable
        {
            get { return m_spdTable; }
            set { m_spdTable = value; }
        }

        public Boolean IsSmartStreamingSupported
        {
            get { return m_IsSmartStreamingSupported; }
            set { m_IsSmartStreamingSupported = value; }
        }

        public Boolean IsExposeOutModeSupported
        {
            get { return m_IsExposeOutModeSupported; }
            set { m_IsExposeOutModeSupported = value; }
        }

        public Boolean IsExtBinningSupported
        {
            get { return m_IsExtBinningSupported; }
            //read only - so no set method
        }

        public Int16 CurrentTemperature
        {
            get { return m_currentTemperaure; }
            //read only - no set
        }
        public Int16 CurrentSetpoint
        {
            get { return m_tempSetpoint; }
            set { m_tempSetpoint = value; }
        }
        public Int16 MinSetpoint
        {
            get { return m_tempSetpointMin; }
            //read only no set
        }
        public Int16 MaxSetpoint
        {
            get { return m_tempSetpointMax; }
            //read only no set
        }

        public UInt64 MaxExposureTime
        {
            get { return m_expTimeMax; }
            //no set - read only
        }

        public UInt64 MinExposureTime
        {
            get { return m_expTimeMin; }
            //no set - read only
        }

        public UInt32 ReadoutTime
        {
            get { return m_readoutTime; }
            //not set - read only
        }

        public Int32 CurrentFanSpeed
        {
            get { return m_currentFanSpeed; }
        }

        public Boolean IsPostProcessingAvail
        {
            get { return m_isPostProcessingAvail; }
            //no set - Read only
        }

        public Boolean IsFanControlAvail
        {
            get { return m_isFanControlAvail; }
            //no set read only
        }

        public Boolean IsMetadataAvail
        {
            get { return m_isMetadataAvail; }
        }


        public List<PP_Feature> PP_FeatureList
        {
            get { return m_ppFeatureList; }
            set { m_ppFeatureList = value; }
        }
    }

    //class used to send messages to UI
    public class ReportMessage : EventArgs
    {
        public ReportMessage(string msg, int tp)
        {
            Message = msg;
            TypeToReport = tp;
        }
        private string MessageToReport;
        public string Message
        {
            set
            {
                MessageToReport = value;
            }
            get
            {
                return this.MessageToReport;
            }
        }

        private int TypeToReport;
        public int Type
        {
            set
            {
                TypeToReport = value;
            }
            get
            {
                return this.TypeToReport;
            }
        }
    }

    //class used to send event notifications to UI
    public class ReportEvent : EventArgs
    {
        public ReportEvent(int e)
        {
            NotifEvent = e;
        }
        private int NotifEventReport;
        public int NotifEvent
        {
            set
            {
                NotifEventReport = value;
            }
            get
            {
                return this.NotifEventReport;
            }
        }
    }
}
