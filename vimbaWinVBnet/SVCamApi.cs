

/*******************************************************************************
* SVS Gen API   Declaration of  camera access functions
*******************************************************************************
*
* Version:     2.5.0  / Januar 2018
*
* Copyright:   SVS VISTEK GmbH
 * */


using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SVCamApi
{
    public class SVcamApi
    {
        // this feature defines can be extended.
        internal static class CameraFeature
        {
            // SV_intfIString
            public const string DeviceUserID = "DeviceUserID";
            //....
            //SV_intfIFloat
            public const string AcquisitionFrameRate = "AcquisitionFrameRate";
            //...
            //SV_intfIInteger:
            public const string SeqCount = "SeqCount";
            public const string PayloadSize = "PayloadSize";
            public const string TLParamsLocked = "TLParamsLocked";
            //...
            //SV_intfICommand:
            public const string TriggerMode = "TriggerMode";
            public const string AcquisitionStart = "AcquisitionStart";
            public const string AcquisitionStop = "AcquisitionStop";
            public const string TriggerSoftware = "TriggerSoftware";
            //...
            // SV_intfIEnumeration
            public const string AcquisitionMode = "AcquisitionMode";
            //...
            // SV_intfIBoolean
            public const string SeqEnablem = "SeqEnable";
            //...
        }

        internal class CameraFeatureInOutVaue
        {
            public bool pBoolValue = false;
            public Int64 pInt64Value = 0;
            public double pFloatValue = 0;
            public string strValue = null;
            public uint Timeout = 0;
        }

        internal static class DefineConstants
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

        public int SVCam_NO_EVENT = -1;
		
		
		public struct SV_LIB_VERSION
		{
			uint   MajorVersion;
			uint   MinorVersion;
			uint   Revision;
			uint   BuildVersion;
		} 
		

        public enum SVSCamApiReturn
        {
            SV_ERROR_SUCCESS = 0, ///< OK
                                  ///
            SV_ERROR_UNKNOWN = -1001, ///< Generic errorcode
            SV_ERROR_NOT_INITIALIZED = -1002,
            SV_ERROR_NOT_IMPLEMENTED = -1003,
            SV_ERROR_RESOURCE_IN_USE = -1004,
            SV_ERROR_ACCESS_DENIED = -1005,
            SV_ERROR_INVALID_HANDLE = -1006,
            SV_ERROR_INVALID_ID = -1007,
            SV_ERROR_NO_MORE_DATA = -1008,
            SV_ERROR_INVALID_PARAMETER = -1009,
            SV_ERROR_FILE_IO = -1010,
            SV_ERROR_TIMEOUT = -1011,
            SV_ERROR_ABORT = -1012,

            SV_ERROR_NOT_OPENED = -2001,
            SV_ERROR_NOT_AVAILABLE = -2002,
            SV_ERROR_NOT_FOUND = -2003,
            SV_ERROR_BUFFER_TOO_SMALL = -2004,
            SV_ERROR_INVALID_FEATURE_TYPE = -2005,
            SV_ERROR_GENICAM_EXCEPTION = -2006,
            SV_ERROR_OUT_OF_MEMORY = -2007,
            SV_ERROR_GENICAM_DLL_NOT_FOUND = -2008,
            SV_ERROR_INVALID_GENICAM_CACHE_DIR = -2009,
            SV_ERROR_GENICAM_DLL_LOAD_FAILED = -2010,
            SV_ERROR_INVALID_CONFIG_FILE = -2011,
            SV_ERROR_LOG_DLL_NOT_LOADED = -2012,

            //SDK 2.5.0
            SV_ERROR_PIXEL_FORMAT_NOT_SUPPORTED = -2013,
            SV_ERROR_LIBPNG_DLL_NOT_LOADED      = -2014,
            SV_ERROR_DLL_VERSION_MISMATCH       = -2015
        }

        public enum SV_FEATURE_VISIBILITY
        {
            SV_Beginner = 0, //!< Always visible
            SV_Expert = 1, //!< Visible for experts or Gurus
            SV_Guru = 2, //!< Visible for Gurus
            SV_Invisible = 3, //!< Not Visible
            SV_UndefinedVisibility = 99 //!< Object is not yetinitialized
        }

        public enum SV_FEATURE_TYPE
        {
            SV_intfIValue = 0, //!> IValue interface
            SV_intfIBase = 1, //!> IBase interface
            SV_intfIInteger = 2, //!> IInteger interface
            SV_intfIBoolean = 3, //!> IBoolean interface
            SV_intfICommand = 4, //!> ICommand interface
            SV_intfIFloat = 5, //!> IFloat interface
            SV_intfIString = 6, //!> IString interface
            SV_intfIRegister = 7, //!> IRegister interface
            SV_intfICategory = 8, //!> ICategory interface
            SV_intfIEnumeration = 9, //!> IEnumeration interface
            SV_intfIEnumEntry = 10, //!> IEnumEntry interface
            SV_intfIPort = 11 //!> IPort interface
        }

        public enum SV_FEATURE_CACHINGMODE
        {
            SV_NoCache, //!< Do not use cache
            SV_WriteThrough, //!< Write to cache and register
            SV_WriteAround, //!< Write to register, write to cache on read
            SV_UndefinedCachingMode //!< Not yet initialized
        }

        public enum SV_FEATURE_REPRESENTATION
        {
            SV_Linear, //!< Slider with linear behavior
            SV_Logarithmic, //!< Slider with logarithmic behavior
            SV_Boolean, //!< Check box
            SV_PureNumber, //!< Decimal number in an edit control
            SV_HexNumber, //!< Hex number in an edit control
            SV_IPV4Address, //!< IP-Address
            SV_MACAddress, //!< MAC-Address
            SV_UndefinedRepresentation,
        }

        public enum SV_FEATURE_DISPLAY_NOTATION
        {
            SV_fnAutomatic, //!> the notation if either scientific or fixed depending on what is shorter
            SV_fnFixed, //!> the notation is fixed, e.g. 123.4
            SV_fnScientific, //!> the notation is scientific, e.g. 1.234e2
            SV_UndefinedEDisplayNotation //!< Object is not yetinitialized
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _SV_DEVICE_INFO
        {

            public IntPtr hParentIF;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string uid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string vendor;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string model;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string displayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string tlType;
            public int accessStatus;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string userDefinedName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string serialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string version;
            public ulong timeStampFreq;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]

            //extra info gev specific
            public string ipAddress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string subnetMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string macAddress;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _SV_INTERFACE_INFO
        {
            public IntPtr hParentIF;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string uid;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string displayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string tlType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            //extra info gev specific
            public string ipAddress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string subnetMask;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string macAddress;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _SV_TL_INFO
        {

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string id;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string vendor;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string model;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string version;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string tlType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string pathName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string displayName;

            public uint gentlVersionMajor;

            public uint gentlVersionMinor;

            public int encoding;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct _SV_FEATURE_INFO
        {

            public sbyte type;
            // [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            // public char[] name;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string name;


            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string node;


            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string displayName;


            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string toolTip;

            public byte level;
            public byte visibility;
            public byte isImplemented;
            public byte isAvailable;
            public byte isLocked;

            public long intMin;
            public long intMax;
            public long intInc;


            public double floatMin;
            public double floatMax;
            public double floatInc;

            //feature specific

            public byte representation;
            public byte displayNotation;
            public long displayPrecision;

            public long strMaxLength;

            public int enumSelectedIndex;
            public long enumCount;

            public long pollingTime;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = DefineConstants.SV_STRING_SIZE)]
            public string unit;

        }

        public struct _SVCamFeaturInf
        {
            public UInt64 intValue;
            public double doubleValue;
            public bool booValue;
            public string strValue;
            public string subFeatureName;
            public _SV_FEATURE_INFO SVFeaturInf;
            public IntPtr hFeature;
            public IntPtr hRemoteDevice;

        }

        public enum SV_LOG_LEVEL
        {
            SV_LOG_LVL_CRITICAL = 0x00000001,
            SV_LOG_LVL_ERROR = 0x00000002,
            SV_LOG_LVL_WARNING = 0x00000004,
            SV_LOG_LVL_INFO = 0x00000008,
            SV_LOG_LVL_CAMERA_IO = 0x00000010,
            SV_LOG_LVL_DEBUG = 0x00000020,
            SV_LOG_LVL_RESERVED = 0x10000000
        }

        // Indicate that pixel is monochrome

        // Indicate effective number of bits occupied by the pixel (including padding).
        // This can be used to compute amount of memory required to store an image.

        // Bit masks
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

        public enum SV_DEVICE_ACCESS_STATUS_LIST
        {
            SV_DEVICE_ACCESS_STATUS_UNKNOWN = 0, // The device accessibility is not known.
            SV_DEVICE_ACCESS_STATUS_READWRITE = 1, // The device is available for read/write access.
            SV_DEVICE_ACCESS_STATUS_READONLY = 2, // The device is available for read only access.
            SV_DEVICE_ACCESS_STATUS_NOACCESS = 3, // The device is not accessible.
            SV_DEVICE_ACCESS_STATUS_BUSY = 4, // The device has already been opened by another process/host. GenTL v1.5
            SV_DEVICE_ACCESS_STATUS_OPEN_READWRITE = 5, // The device has already been opened by this process. GenTL v1.5
            SV_DEVICE_ACCESS_STATUS_OPEN_READ = 6, // The device has already been opened by this process. GenTL v1.5

            SV_DEVICE_ACCESS_STATUS_CUSTOM_ID = 1000 // Starting value for custom IDs.
        }

        public enum SV_DEVICE_ACCESS_FLAGS_LIST
        {
            SV_DEVICE_ACCESS_UNKNOWN = 0, // Not used in a command. Can be used to initialize a variable to query that information.
            SV_DEVICE_ACCESS_NONE = 1, // This either means that the device is not open because it was not opened before or the access to it was denied.
            SV_DEVICE_ACCESS_READONLY = 2, // Open the device read only. All Port functions can only read from the device.
            SV_DEVICE_ACCESS_CONTROL = 3, // Open the device in a way that other hosts/processes can have read only access to the device. Device access level is read/write for this process.
            SV_DEVICE_ACCESS_EXCLUSIVE = 4, // Open the device in a way that only this host/process can have access to the device. Device access level is read/write for this process.

            SV_DEVICE_ACCESS_CUSTOM_ID = 1000 //  Starting value for GenTL Producer custom IDs.
        }

        public enum SV_ACQ_START_FLAGS_LIST
        {
            SV_ACQ_START_FLAGS_DEFAULT = 0, // Default behavior.
            SV_ACQ_START_FLAGS_CUSTOM_ID = 1000 // Starting value for GenTL Producer custom IDs.
        }

        public enum SV_ACQ_STOP_FLAGS_LIST
        {
            SV_ACQ_STOP_FLAGS_DEFAULT = 0, // Stop the acquisition engine when the currently running tasks like filling a buffer are completed (default behavior).
            SV_ACQ_STOP_FLAGS_KILL = 1, // Stop the acquisition engine immediately and leave buffers currently being filled in the Input Buffer Pool.

            SV_ACQ_STOP_FLAGS_CUSTOM_ID = 1000 // Starting value for GenTL Producer custom IDs.
        }

        public enum SV_ACQ_QUEUE_TYPE_LIST
        {
            SV_ACQ_QUEUE_INPUT_TO_OUTPUT = 0, // Flushes the input pool to the output queue and if necessary adds entries in the New Buffer event data queue.
            SV_ACQ_QUEUE_OUTPUT_DISCARD = 1, // Discards all buffers in the output queue and if necessary remove the entries from the event data queue.
            SV_ACQ_QUEUE_ALL_TO_INPUT = 2, // Puts all buffers in the input pool. Even those in the output queue and discard entries in the event data queue.
            SV_ACQ_QUEUE_UNQUEUED_TO_INPUT = 3, // Puts all buffers that are not in the input pool or the output queue in the input pool.
            SV_ACQ_QUEUE_ALL_DISCARD = 4, // Discards all buffers in the input pool and output queue.

            SV_ACQ_QUEUE_CUSTOM_ID = 1000 // Starting value for GenTL Producer custom IDs.
        }



        [StructLayout(LayoutKind.Explicit)]
        public struct _SV_BUFFER_FLAG
        {
            [FieldOffset(0)]
            public uint value;
            [FieldOffset(0)]
            public byte newData;
            [FieldOffset(0)]
            public byte acquiring;
            [FieldOffset(0)]
            public byte queued;
            [FieldOffset(0)]
            public byte incomplete;
        }


        [StructLayout(LayoutKind.Sequential)]

        public struct _SV_BUFFER_INFO
        {
            public IntPtr pImagePtr;
            public IntPtr pUserPtr;
            public IntPtr iSizeX;
            public IntPtr iSizeY;
            public IntPtr iImageSize;
            public _SV_BUFFER_FLAG flags;
            public ulong iPixelType;
            public ulong iImageId;
            public ulong iTimeStamp;
            public uint iReserved2;
            public uint iReserved3;
            public uint iReserved4;
            public uint iReserved5;
            public uint iReserved6;
          
        }

        public enum SV_TL_TYPE
        {
            TL_GEV = 0,
            TL_U3V = 1,
            TL_CL = 2,
        }

        public delegate void SV_CB_FEATURE_INVALIDATED_PFN2(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string value);

        //-----------------------------------------------------------------------------------------------------------------------



        internal static class NativeMethods
        {


            /** SVLibInit. 
            *  This function must be called prior to any other function call to allow global initialization of SVGenSDK.
            *  Multiple calls to SVLibInit without accompanied calls to SVLibClose will return the error
            *  SV_ERROR_RESOURCE_IN_USE.
            *
            *  @param [in] TLIPath (optional) Path containing the SVS CTI's. If this parameter is NULL,
            *  SVGenSDK will use “{Application Dir}/cti”. 
            *  @param [in] genicamRootDir (optional)Path containing the Genicam Dll's needed by SVGenSDK. If this
            *  parameter is NULL, SVGenSDK will use “{Application Dir}/genicam”. 
            *  @param [in] genicamCacheDir (optional) Path containing the Genicam cache dir. If this
            *  parameter is NULL, SVGenSDK will use “{Application Dir}/cache”.
            *  NOTE:Application need to have a read/write access on specified folder.
            *  @param [in] clProtocolDriverDir (optional)   Path containing the SVCLProtocolDriver dll's. If this
            *  parameter is NULL, SVGenSDK will use “{Application Dir}/CLProtocol”.
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLibInit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVLibInit_64(byte* TLIPath = null,
                                                 byte* genicamRootDir = null,
                                                 byte* genicamCacheDir = null,
                                                 byte* clProtocolDriverDir = null);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLibInit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVLibInit_32(byte* TLIPath = null,
                                                 byte* genicamRootDir = null,
                                                 byte* genicamCacheDir = null,
                                                 byte* clProtocolDriverDir = null);
            internal static  unsafe
            SVSCamApiReturn SVLibInit(byte* TLIPath = null,
                                              byte* genicamRootDir = null,
                                              byte* genicamCacheDir = null,
                                              byte* clProtocolDriverDir = null)
            {

                return IntPtr.Size == 8 /* 64bit */ ? SVLibInit_64(TLIPath,  genicamRootDir,  genicamCacheDir , clProtocolDriverDir ) :

                                                        SVLibInit_32(TLIPath, genicamRootDir, genicamCacheDir, clProtocolDriverDir);
            }


            /** SVLibSystemGetCount
            *  Queries the number of available SVS GenTL Producers on TLIPath directory specified on SVLibInit.
            *  @param [out] tlCount Number of available System module/SVS GenTL Producers.
            *  @return success or error code
            *  
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLibSystemGetCount", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn
            SVLibSystemGetCount_64(UInt32* tlCount);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLibSystemGetCount", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn
            SVLibSystemGetCount_32(UInt32* tlCount);
            internal static unsafe
            SVSCamApiReturn
            SVLibSystemGetCount(UInt32* tlCount)
            {       
             return IntPtr.Size == 8 /* 64bit */ ? SVLibSystemGetCount_64(tlCount) : SVLibSystemGetCount_32(tlCount);
            }


            /** SVLibSystemGetInfo
            *  Queries the information of SVS GenTL Producers.
            *  @param [in] uiIndex Zero-based index of loaded GenTLProducers.
            *  @param[out] pInfoOut Information about the current GenTL Producers
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLibSystemGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVLibSystemGetInfo_64(uint uiIndex, ref _SV_TL_INFO pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLibSystemGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVLibSystemGetInfo_32(uint uiIndex, ref _SV_TL_INFO pInfoOut);
            internal static unsafe
            SVSCamApiReturn SVLibSystemGetInfo(uint uiIndex, ref _SV_TL_INFO pInfoOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVLibSystemGetInfo_64(uiIndex, ref pInfoOut) : SVLibSystemGetInfo_32(uiIndex, ref pInfoOut);
            }


            /** SVLibSystemOpen
          *  Opens the System module and puts the instance in the phSystem handle. This allocates all system wide
          *  resources. A System module can only be opened once.
          *  @param [in] uiIndex  Zero-based index of loaded GenTLProducers.
          *  @param [out] phSystemOut System module handle. 
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLibSystemOpen", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVLibSystemOpen_64(uint uiIndex, void* phSystemOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLibSystemOpen", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVLibSystemOpen_32(uint uiIndex, void* phSystemOut);
            internal static  unsafe
            SVSCamApiReturn SVLibSystemOpen(uint uiIndex, void* phSystemOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVLibSystemOpen_64(uiIndex, phSystemOut) : SVLibSystemOpen_32(uiIndex, phSystemOut);
            }


          /** SVSystemGetInfo
           *  Queries the information on System Module.
           *  @param [in] hSystem System module handle from SVLibSystemOpen.   
           *  @param [out] pInfoOut Information about the System Module. 
           *  @return success or error code
          */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVSystemGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemGetInfo_64(UInt32 index, void* pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVSystemGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemGetInfo_32(UInt32 index, void* pInfoOut);
            internal static  unsafe
            SVSCamApiReturn SVSystemGetInfo(UInt32 index, void* pInfoOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVSystemGetInfo_64(index, pInfoOut) : SVSystemGetInfo_32(index, pInfoOut);
            }


            /** SVLibClose
              *  Close the library and frees all the allocated resources.
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLibClose", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVLibClose_64();
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLibClose", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVLibClose_32();
            internal static  unsafe
            SVSCamApiReturn SVLibClose()
            { 
                return IntPtr.Size == 8 /* 64bit */ ? SVLibClose_64() : SVLibClose_32();
            }


            /** SVSystemUpdateInterfaceList
            *  Updates the internal list of available interfaces.
            *  @param [in] hSystem System module handle from SVLibSystemOpen.
            *  @param [out] pbChanged (optional) returns true if the internal list was changed and false
            *  otherwise. If set to NULL nothing is written to this parameter.
            *  @param [in] timeOut timeout in ms.
            *  @return success or error code
           */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVSystemUpdateInterfaceList", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemUpdateInterfaceList_64(void* hSystem, bool* pbChanged, uint timeOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVSystemUpdateInterfaceList", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemUpdateInterfaceList_32(void* hSystem, bool* pbChanged, uint timeOut);
            internal static unsafe
            SVSCamApiReturn SVSystemUpdateInterfaceList(void* hSystem, bool* pbChanged, uint timeOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVSystemUpdateInterfaceList_64(hSystem, pbChanged, timeOut) : SVSystemUpdateInterfaceList_32(hSystem, pbChanged, timeOut);
            }



            /** SVSystemGetNumInterfaces
          *  Queries the number of available interfaces on this System module.
          *  @param [in] hSystem System module handle from SVLibSystemOpen.
          *  @param [out] piNumIfaces Number of interfaces on this System module.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVSystemGetNumInterfaces", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemGetNumInterfaces_64(void* hSystem, uint* piNumIfaces);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVSystemGetNumInterfaces", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemGetNumInterfaces_32(void* hSystem, uint* piNumIfaces);
            internal static unsafe
            SVSCamApiReturn SVSystemGetNumInterfaces(void* hSystem, uint* piNumIfaces)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVSystemGetNumInterfaces_64(hSystem, piNumIfaces) : SVSystemGetNumInterfaces_32(hSystem, piNumIfaces);
            }


            /** SVSystemGetInterfaceId
          *  Queries the ID of the interface at iIndex in the internal interface list .
          *  @param [in] hSystem System module handle from SVLibSystemOpen.  
          *  @param [in] Index Number of interfaces on this System module. 
          *  @param [out]  pInterfaceId (optional) Pointer to a user allocated C string buffer to receive the Interface
          *  module ID at the given iIndex. If this parameter is NULL, pSize will contain the needed size of sIfaceID in bytes. The size. includes the *  terminating 0. 
          *  @param [in,out] pSize size of the Interface id.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVSystemGetInterfaceId", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemGetInterfaceId_64(void* hSystem, uint Index, sbyte* pInterfaceId, uint* pSize);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVSystemGetInterfaceId", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemGetInterfaceId_32(void* hSystem, uint Index, sbyte* pInterfaceId, uint* pSize);
            internal static unsafe
            SVSCamApiReturn SVSystemGetInterfaceId(void* hSystem, uint Index, sbyte* pInterfaceId, uint* pSize)
            { 
             return IntPtr.Size == 8 /* 64bit */ ? SVSystemGetInterfaceId_64( hSystem, Index, pInterfaceId, pSize) : SVSystemGetInterfaceId_32( hSystem, Index, pInterfaceId, pSize);
            }



            /** SVSystemInterfaceGetInfo
          *  Queries the information about the interface on this System module without opening the interface.
          *  @param [in] hSystem System module handle from SVLibSystemOpen.
          *  @param [in] pInterfaceId Id of interface acquired on SVSystemGetInterfaceId.
          *  @param [out] pInfoOut Information about the current Interface.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVSystemInterfaceGetInfo", CallingConvention = CallingConvention.Cdecl),]
            private static extern unsafe
            SVSCamApiReturn
            SVSystemInterfaceGetInfo_64(void* hSystem, byte* pInterfaceId, ref _SV_INTERFACE_INFO pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVSystemInterfaceGetInfo", CallingConvention = CallingConvention.Cdecl),]
            private static extern unsafe
            SVSCamApiReturn
            SVSystemInterfaceGetInfo_32(void* hSystem, byte* pInterfaceId, ref _SV_INTERFACE_INFO pInfoOut);
            internal static unsafe
            SVSCamApiReturn
            SVSystemInterfaceGetInfo(void* hSystem, byte* pInterfaceId, ref _SV_INTERFACE_INFO pInfoOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVSystemInterfaceGetInfo_64(hSystem, pInterfaceId, ref  pInfoOut) : SVSystemInterfaceGetInfo_32(hSystem, pInterfaceId, ref  pInfoOut);
            }


            /** SVSystemInterfaceOpen
              *  Opens the given pInterfaceId on the given hSystem.
              *  @param [in] hSystem System module handle from SVLibSystemOpen.
              *  @param [in] pInterfaceId Id of interface acquired on SVSystemGetInterfaceId.
              *  @param [out] phInterfaceOut Interface module handle.
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVSystemInterfaceOpen", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemInterfaceOpen_64(void* hSystem, byte* pInterfaceId, void* phInterfaceOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVSystemInterfaceOpen", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemInterfaceOpen_32(void* hSystem, byte* pInterfaceId, void* phInterfaceOut);
            internal static  unsafe
            SVSCamApiReturn SVSystemInterfaceOpen(void* hSystem, byte* pInterfaceId, void* phInterfaceOut)
            { 
                 return IntPtr.Size == 8 /* 64bit */ ? SVSystemInterfaceOpen_64(hSystem, pInterfaceId, phInterfaceOut) : SVSystemInterfaceOpen_32(hSystem, pInterfaceId, phInterfaceOut);
            }


            /** SVSystemClose
             *  Close system module and frees all the resources.
             *  @param [in] hSystem System module handle from SVLibSystemOpen.  
             *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVSystemClose", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemClose_64(void* hSystem);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVSystemClose", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVSystemClose_32(void* hSystem);
            internal static  unsafe
            SVSCamApiReturn SVSystemClose(void* hSystem)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVSystemClose_64(hSystem) : SVSystemClose_32(hSystem);
            }


            /** SVInterfaceGetInfo
          *  Queries the information about the interface after opening the interface.
          *  @param [in] hInterface Interface module handle from SVSystemInterfaceOpen. 
          *  @param [out] pInfoOut Information about the current Interface.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVInterfaceGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceGetInfo_64(void* hInterface, ref _SV_INTERFACE_INFO pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVInterfaceGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceGetInfo_32(void* hInterface, ref _SV_INTERFACE_INFO pInfoOut);
            internal static unsafe
            SVSCamApiReturn SVInterfaceGetInfo(void* hInterface, ref _SV_INTERFACE_INFO pInfoOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVInterfaceGetInfo_64( hInterface, ref pInfoOut) : SVInterfaceGetInfo_32(hInterface, ref pInfoOut);
            }


            /** SVInterfaceUpdateDeviceList
            *  Updates the internal list of available devices on this interface.
            *  @param [in] hInterface Interface module handle from SVSystemInterfaceOpen.
            *  @param [out] pbChanged (optional) returns true if the internal list was changed and false 
            *  otherwise. If set to NULL nothing is written to this parameter.
            *  @param [in] timeOut timeout in ms. 
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVInterfaceUpdateDeviceList", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceUpdateDeviceList_64(void* hInterface, bool* pbChanged, uint timeOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVInterfaceUpdateDeviceList", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceUpdateDeviceList_32(void* hInterface, bool* pbChanged, uint timeOut);
            internal static  unsafe
            SVSCamApiReturn SVInterfaceUpdateDeviceList(void* hInterface, bool* pbChanged, uint timeOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVInterfaceUpdateDeviceList_64(hInterface, pbChanged, timeOut) : SVInterfaceUpdateDeviceList_32(hInterface, pbChanged, timeOut);
            }



            /** SVInterfaceGetNumDevices
            *  Queries the number of available devices on this interface.
            *  @param [in] hInterface Interface module handle from SVSystemInterfaceOpen.
            *  @param [out] piDevices Number of Devices on this interface.    
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVInterfaceGetNumDevices", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceGetNumDevices_64(void* hInterface, uint* piDevices);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVInterfaceGetNumDevices", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceGetNumDevices_32(void* hInterface, uint* piDevices);
            internal static unsafe
            SVSCamApiReturn SVInterfaceGetNumDevices(void* hInterface, uint* piDevices)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVInterfaceGetNumDevices_64(hInterface, piDevices) : SVInterfaceGetNumDevices_32(hInterface, piDevices);
            }



            /** SVInterfaceGetDeviceId
            *  Queries the ID of the Device at Index in the internal device list.
            *  @param [in] hInterface System module handle from SVLibSystemOpen. 
            *  @param [in] Index Number of interfaces on this System module. 
            *  @param [in,out] pDeviceId Pointer to a user allocated C string buffer to receive the Device module ID at the given iIndex. If this parameter is NULL.
            *  @param [in,out] pSize size of the Device id.
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVInterfaceGetDeviceId", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceGetDeviceId_64(void* hInterface, uint Index, sbyte* pDeviceId, uint* pSize);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVInterfaceGetDeviceId", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceGetDeviceId_32(void* hInterface, uint Index, sbyte* pDeviceId, uint* pSize);
            internal static unsafe
            SVSCamApiReturn SVInterfaceGetDeviceId(void* hInterface, uint Index, sbyte* pDeviceId, uint* pSize)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVInterfaceGetDeviceId_64(hInterface, Index, pDeviceId, pSize) : SVInterfaceGetDeviceId_32(hInterface, Index, pDeviceId, pSize);
            }



            /** SVInterfaceDeviceGetInfo
            *  Queries the information about the Device on this Interface module without opening the Device.
            *  @param [in] hInterface Interface module handle from SVSystemInterfaceOpen. 
            *  @param [in] pDeviceId Id of Device acquired on SVInterfaceGetDeviceId. 
            *  @param [out] pInfoOut Information about the current Device. 
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVInterfaceDeviceGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceDeviceGetInfo_64(void* hInterface, byte* pDeviceId, ref _SV_DEVICE_INFO pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVInterfaceDeviceGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceDeviceGetInfo_32(void* hInterface, byte* pDeviceId, ref _SV_DEVICE_INFO pInfoOut);
            internal static unsafe
            SVSCamApiReturn SVInterfaceDeviceGetInfo(void* hInterface, byte* pDeviceId, ref _SV_DEVICE_INFO pInfoOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVInterfaceDeviceGetInfo_64(hInterface, pDeviceId, ref pInfoOut) : SVInterfaceDeviceGetInfo_32(hInterface, pDeviceId, ref pInfoOut);
            }



            /** SVInterfaceClose
            *  Close the interface and frees all the resources allocated by this module.
            *  @param  [in] hInterface Interface module handle from SVSystemInterfaceOpen. 
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVInterfaceClose", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceClose_64(void* hInterface);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVInterfaceClose", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceClose_32(void* hInterface);
            internal static  unsafe
            SVSCamApiReturn SVInterfaceClose(void* hInterface)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVInterfaceClose_64(hInterface) : SVInterfaceClose_32(hInterface);
            }



            /** SVInterfaceDeviceOpen
            *  Opens the device with pDeviceId connected on this interface.
            *  @param [in] hInterface Interface module handle from SVSystemInterfaceOpen. 
            *  @param [in] pDeviceId Id of Device acquired on SVInterfaceGetDeviceId. 
            *  @param [in] accessFlags Configures the open process as defined in the SV_DEVICE_ACCESS_FLAGS. 
            *  @param [out] phDeviceOut handle for local Device Module.
            *  @param [out] phRemoteDeviceOut handle for Remote Device.   
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVInterfaceDeviceOpen", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceDeviceOpen_64(void* hInterface,
                                                    sbyte* pDeviceId,
                                                    SV_DEVICE_ACCESS_FLAGS_LIST accessFlags,
                                                    void* phDeviceOut,
                                                    void* phRemoteDeviceOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVInterfaceDeviceOpen", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVInterfaceDeviceOpen_32(void* hInterface,
                                                    sbyte* pDeviceId,
                                                    SV_DEVICE_ACCESS_FLAGS_LIST accessFlags,
                                                    void* phDeviceOut,
                                                    void* phRemoteDeviceOut);
            internal static unsafe
            SVSCamApiReturn SVInterfaceDeviceOpen(void* hInterface,
                                                    sbyte* pDeviceId,
                                                    SV_DEVICE_ACCESS_FLAGS_LIST accessFlags,
                                                    void* phDeviceOut,
                                                    void* phRemoteDeviceOut)
            { 
            return IntPtr.Size == 8 /* 64bit */ ? SVInterfaceDeviceOpen_64( hInterface, pDeviceId, accessFlags, phDeviceOut, phRemoteDeviceOut): 
                                                  SVInterfaceDeviceOpen_32( hInterface, pDeviceId, accessFlags, phDeviceOut, phRemoteDeviceOut);
            }


            /** SVDeviceGetInfo
            *  Queries the information for the Device after opening.
            *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen.
            *  @param [out] pInfoOut Information about the Device module.   
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVDeviceGetInfo_64(void* hDevice, ref  _SV_DEVICE_INFO pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVDeviceGetInfo_32(void* hDevice, ref  _SV_DEVICE_INFO pInfoOut);
            internal static  unsafe
            SVSCamApiReturn SVDeviceGetInfo(void* hDevice, ref  _SV_DEVICE_INFO pInfoOut)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVDeviceGetInfo_64(hDevice, ref pInfoOut) : SVDeviceGetInfo_32(hDevice, ref pInfoOut);
            }



            /** SVDeviceGetNumStreams
            *  Queries the number of streams supported by device module.
            *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen. 
            *  @param [out] piStreams Number of streams supported on this device module.  
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceGetNumStreams", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceGetNumStreams_64(void* hDevice, uint* piStreams);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceGetNumStreams", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceGetNumStreams_32(void* hDevice, uint* piStreams);
            internal static  unsafe
            SVSCamApiReturn SVDeviceGetNumStreams(void* hDevice, uint* piStreams)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVDeviceGetNumStreams_64(hDevice, piStreams) : SVDeviceGetNumStreams_32(hDevice, piStreams);
            }





            /** SVDeviceGetStreamId----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            *  Queries the stream id at index.
            *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen.
            *  @param [in] Index Zero-based index of the Stream on this device.
            *  @param [in,out] pStreamId Pointer to a user allocated C string buffer to receive the Stream module ID at the given iIndex. 
            *  If this parameter is NULL, piSize will contain the needed size of pDeviceId in bytes. The size includes the terminating 0.
            *  @param [in,out] pSize size of the Stream id.
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceGetStreamId", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceGetStreamId_64(void* hDevice, uint Index, sbyte* pStreamId, uint* pSize);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceGetStreamId", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceGetStreamId_32(void* hDevice, uint Index, sbyte* pStreamId, uint* pSize);
            internal static  unsafe
            SVSCamApiReturn SVDeviceGetStreamId(void* hDevice, uint Index, sbyte* pStreamId, uint* pSize)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVDeviceGetStreamId_64(hDevice,  Index,  pStreamId,  pSize) :  SVDeviceGetStreamId_32(hDevice,  Index,  pStreamId,  pSize);
            }


            /** SVDeviceSaveSettings
          * The current streamable settings will be stored in the given file. 
          *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen.
          *  @param [in] fileName a complete path and filename where to save the streamable settings.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceSaveSettings", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceSaveSettings_64(void* hDevice, sbyte* fileName);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceSaveSettings", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceSaveSettings_32(void* hDevice, sbyte* fileName);
            internal static  unsafe
            SVSCamApiReturn SVDeviceSaveSettings(void* hDevice, sbyte* fileName)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?SVDeviceSaveSettings_64( hDevice, fileName) :  SVDeviceSaveSettings_32(hDevice, fileName);
            }



            /** SVDeviceLoadSettings
              *  load the streamable camera settings.
              *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen.
              *  @param [in] fileName  a complete path and filename where to load the settings from.
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceLoadSettings", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceLoadSettings_64(void* hDevice, sbyte* fileName);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceLoadSettings", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceLoadSettings_32(void* hDevice, sbyte* fileName);
            internal static  unsafe
            SVSCamApiReturn SVDeviceLoadSettings(void* hDevice, sbyte* fileName)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVDeviceLoadSettings_64( hDevice, fileName): SVDeviceLoadSettings_32( hDevice, fileName);
            }


            /** SVDeviceRead
            *  Reads a number of bytes from a given iAddress from Remote Device.
            *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen. 
            *  @param [in] nAddress Byte address to read from.  
            *  @param [out] pData Pointer to a user allocated byte buffer to receive data; this must not be NULL. 
            *  @param [in,out] pSize size of the read data in bytes. 
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceRead", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceRead_64(void* hDevice, uint nAddress, void* pData, uint* pSize);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceRead", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceRead_32(void* hDevice, uint nAddress, void* pData, uint* pSize);
            internal static  unsafe
            SVSCamApiReturn SVDeviceRead(void* hDevice, uint nAddress, void* pData, uint* pSize)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVDeviceRead_64( hDevice,  nAddress,  pData, pSize)  :  SVDeviceRead_32( hDevice,  nAddress,  pData, pSize);
            }


            /** SVDeviceWrite
            *  Write a number of bytes from a given iAddress from Remote Device. 
            *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen.
            *  @param [in] nAddress Byte address to write to.
            *  @param [in] pData Pointer to a user allocated byte buffer containing the data to write; this must not be NULL.
            *  @param [in,out] pSize size of the written data in bytes.   
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceWrite", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceWrite_64(void* hDevice, uint nAddress, sbyte* pData, uint* pSize);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceWrite", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceWrite_32(void* hDevice, uint nAddress, sbyte* pData, uint* pSize);
            internal static  unsafe
            SVSCamApiReturn SVDeviceWrite(void* hDevice, uint nAddress, sbyte* pData, uint* pSize)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVDeviceWrite_64( hDevice,nAddress,  pData, pSize) : SVDeviceWrite_32( hDevice,nAddress,  pData, pSize);
            }


            /** SVDeviceStreamOpen
            * 
            *  Opens the given sDataStreamID on the given hDevice.
            *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen.  
            *  @param [in] sDataStreamID Id of Stream module acquired on SVDeviceGetStreamId. 
            *  @param [out] phStream handle for the opened stream module. 
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceStreamOpen", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceStreamOpen_64(void* hDevice, sbyte* sDataStreamID, void* phStream);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceStreamOpen", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceStreamOpen_32(void* hDevice, sbyte* sDataStreamID, void* phStream);
            internal static  unsafe
            SVSCamApiReturn SVDeviceStreamOpen(void* hDevice, sbyte* sDataStreamID, void* phStream)
            {
            
                 return IntPtr.Size == 8 /* 64bit */ ? SVDeviceStreamOpen_64( hDevice,  sDataStreamID,  phStream): SVDeviceStreamOpen_32( hDevice,  sDataStreamID,  phStream);
            }


            /** SVDeviceClose
            *  Close the device module and frees all the resources allocated by this module.
            *  @param [in] hDevice Device module handle from SVInterfaceDeviceOpen. 
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceClose", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceClose_64(void* hDevice);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceClose", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVDeviceClose_32(void* hDevice);
            internal static  unsafe
            SVSCamApiReturn SVDeviceClose(void* hDevice)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVDeviceClose_64( hDevice): SVDeviceClose_32( hDevice);
            }



            /** SVStreamAcquisitionStart
           * Starts the acquisition engine on the host. Each call to SVStreamAcquisitionStart must be accompanied by a call to SVStreamAcquisitionStop.
           *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
           *  @param [in] flags As defined in SV_ACQ_START_FLAGS . 
           *  @param [in] iNumToAcquire Number of buffer to be delivered.  
           *  @return success or error code
          */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamAcquisitionStart", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAcquisitionStart_64(void* hStream, SV_ACQ_START_FLAGS_LIST flags, UInt64 iNumToAcquire);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamAcquisitionStart", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAcquisitionStart_32(void* hStream, SV_ACQ_START_FLAGS_LIST flags, UInt64 iNumToAcquire);
            internal static  unsafe
            SVSCamApiReturn SVStreamAcquisitionStart(void* hStream, SV_ACQ_START_FLAGS_LIST flags, UInt64 iNumToAcquire)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVStreamAcquisitionStart_64( hStream,  flags,  iNumToAcquire): SVStreamAcquisitionStart_32( hStream,  flags,  iNumToAcquire);
            }



            /** SVStreamAcquisitionStop
              *  Stops the acquisition engine on the host.
              *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
              *  @param [in] flags As defined in SV_ACQ_STOP_FLAGS.
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamAcquisitionStop", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAcquisitionStop_64(void* hStream, SV_ACQ_STOP_FLAGS_LIST flags);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamAcquisitionStop", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAcquisitionStop_32(void* hStream, SV_ACQ_STOP_FLAGS_LIST flags);
            internal static  unsafe
            SVSCamApiReturn SVStreamAcquisitionStop(void* hStream, SV_ACQ_STOP_FLAGS_LIST flags)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVStreamAcquisitionStop_64( hStream,  flags):SVStreamAcquisitionStop_32( hStream,  flags);
            }



            /** SVStreamAnnounceBuffer
          *  This announces a user allocated memory to the Data Stream associated with the hStream handle and
          *  returns a hBuffer handle which references that single buffer until the buffer is revoked. This will allocate
          *  internal resources which will be freed upon a call to SVStreamRevokeBuffer.
          *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen.
          *  @param [in] pBuffer Pointer to buffer memory to announce.
          *  @param [in] uiSize Size of the pBuffer in bytes.
          *  @param [in] pPrivate Pointer to private data which user can pass and retrieved on SVStreamWaitForNewBuffer. This parameter may be NULL.
          *  @param [out] phBuffer Buffer module handle for the announce Buffer. 
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamAnnounceBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAnnounceBuffer_64(void* hStream, sbyte* pBuffer, uint uiSize, void* pPrivate, void* phBuffer);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamAnnounceBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAnnounceBuffer_32(void* hStream, sbyte* pBuffer, uint uiSize, void* pPrivate, void* phBuffer);
            internal static  unsafe
            SVSCamApiReturn SVStreamAnnounceBuffer(void* hStream, sbyte* pBuffer, uint uiSize, void* pPrivate, void* phBuffer)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVStreamAnnounceBuffer_64( hStream,  pBuffer,  uiSize,  pPrivate,  phBuffer): SVStreamAnnounceBuffer_32( hStream,  pBuffer,  uiSize,  pPrivate,  phBuffer);
            }



            /** SVStreamAllocAndAnnounceBuffer
              * Allocates the memory and announce to the Data Stream module associated with the hStream handle.
              * This will allocate internal resources which will be freed upon a call to SVStreamRevokeBuffer.
              *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
              *  @param [in] uiSize Size of the buffer in bytes. 
              *  @param [in] pPrivate Pointer to private data which user can pass and retrieved on SVStreamWaitForNewBuffer. This parameter may be NULL. 
              *  @param [out] phBuffer Buffer module handle for the announce Buffer.  
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamAllocAndAnnounceBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAllocAndAnnounceBuffer_64(void* hStream, uint uiSize, void* pPrivate, void* phBuffer);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamAllocAndAnnounceBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamAllocAndAnnounceBuffer_32(void* hStream, uint uiSize, void* pPrivate, void* phBuffer);
            internal static  unsafe
            SVSCamApiReturn SVStreamAllocAndAnnounceBuffer(void* hStream, uint uiSize, void* pPrivate, void* phBuffer)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVStreamAllocAndAnnounceBuffer_64( hStream,  uiSize,  pPrivate,  phBuffer):SVStreamAllocAndAnnounceBuffer_32( hStream,  uiSize,  pPrivate,  phBuffer);
            }


            /** SVStreamRevokeBuffer
          *  Removes an announced buffer from the acquisition engine. This function will free all internally allocated resources associated with this buffer.
          *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
          *  @param [in] hBuffer Buffer handle to revoke. 
          *  @param [out] pBuffer Pointer to the buffer memory if buffer is announce usingSVStreamAnnounceBuffer. 
          *  This value is NULL if buffer was announced using SVStreamAllocAndAnnounceBuffer. 
          *  @param [out] pPrivate Pointer to private data which is pass upon SVStreamAnnounceBuffer or SVStreamAllocAndAnnounceBuffer. 
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamRevokeBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamRevokeBuffer_64(void* hStream, void* hBuffer, void* pBuffer, void* pPrivate);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamRevokeBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamRevokeBuffer_32(void* hStream, void* hBuffer, void* pBuffer, void* pPrivate);
            internal static  unsafe
            SVSCamApiReturn SVStreamRevokeBuffer(void* hStream, void* hBuffer, void* pBuffer, void* pPrivate)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVStreamRevokeBuffer_64( hStream,  hBuffer,  pBuffer,  pPrivate) : SVStreamRevokeBuffer_32( hStream,  hBuffer,  pBuffer,  pPrivate);
            }


            /** SVStreamQueueBuffer
              * Queues a particular buffer for acquisition. Unqueued buffer will not be used by the SDK and thus effectively locking it.
              *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
              *  @param [in] hBuffer Buffer handle to queue. 
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamQueueBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamQueueBuffer_64(void* hStream, void* hBuffer);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamQueueBuffer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamQueueBuffer_32(void* hStream, void* hBuffer);
            internal static  unsafe
            SVSCamApiReturn SVStreamQueueBuffer(void* hStream, void* hBuffer)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVStreamQueueBuffer_64(hStream, hBuffer):SVStreamQueueBuffer_32(hStream, hBuffer);
            }



            /** SVStreamGetBufferId
           *  Queries the buffer handle for the give iIndex.
           *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen 
           *  @param [in] iIndex Zero-based index of the buffer on this data stream. 
           *  @param [out] phBuffer Buffer handle of the given index.
           *  @return success or error code
          */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamGetBufferId", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamGetBufferId_64(void* hStream, uint iIndex, void* phBuffer);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamGetBufferId", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamGetBufferId_32(void* hStream, uint iIndex, void* phBuffer);
            internal static  unsafe
            SVSCamApiReturn SVStreamGetBufferId(void* hStream, uint iIndex, void* phBuffer)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVStreamGetBufferId_64(hStream,  iIndex, phBuffer):  SVStreamGetBufferId_32(hStream,  iIndex, phBuffer);
            }




            /** SVStreamFlushQueue
            *  Flushes the Buffer queue by iOperation.
            *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
            *  @param [in] iOperation Flush operation to perform as defined in SV_ACQ_QUEUE_TYPE. 
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamFlushQueue", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamFlushQueue_64(void* hStream, SV_ACQ_QUEUE_TYPE_LIST iOperation);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamFlushQueue", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamFlushQueue_32(void* hStream, SV_ACQ_QUEUE_TYPE_LIST iOperation);
            internal static  unsafe
            SVSCamApiReturn SVStreamFlushQueue(void* hStream, SV_ACQ_QUEUE_TYPE_LIST iOperation)
            {

                 return IntPtr.Size == 8 /* 64bit */ ?  SVStreamFlushQueue_64(hStream,  iOperation):SVStreamFlushQueue_32(hStream,  iOperation);
            }



            /** SVStreamWaitForNewBuffer
              *  Waits for the new buffer event.  
              *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
              *  @param [out] ppUserData Pointer of the pPrivateData parameter passed on SVStreamAnnounceBuffer or SVStreamAllocAndAnnounceBuffer.
              *  @param [out] phBufferOut Buffer handle.
              *  @param [in] timeOut Timeout in ms.   
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamWaitForNewBuffer", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVStreamWaitForNewBuffer_64(void* hStream, void* ppUserData, void* phBufferOut, uint timeOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamWaitForNewBuffer", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVStreamWaitForNewBuffer_32(void* hStream, void* ppUserData, void* phBufferOut, uint timeOut);
            internal static  unsafe
            SVSCamApiReturn SVStreamWaitForNewBuffer(void* hStream, void* ppUserData, void* phBufferOut, uint timeOut)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVStreamWaitForNewBuffer_64(hStream,  ppUserData,  phBufferOut,  timeOut):SVStreamWaitForNewBuffer_32(hStream,  ppUserData,  phBufferOut,  timeOut);
            }



            /** SVStreamBufferGetInfo
            *  Queries the information of hBuffer on hStream.
            *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen. 
            *  @param [in] hBuffer Buffer handle.
            *  @param [out] pInfoOut Information about the current Buffer.
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamBufferGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVStreamBufferGetInfo_64(void* hStream, void* hBuffer, ref  _SV_BUFFER_INFO pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamBufferGetInfo", CallingConvention = CallingConvention.Cdecl)]
            private static extern unsafe
            SVSCamApiReturn SVStreamBufferGetInfo_32(void* hStream, void* hBuffer, ref  _SV_BUFFER_INFO pInfoOut);
            internal static  unsafe
            SVSCamApiReturn SVStreamBufferGetInfo(void* hStream, void* hBuffer, ref  _SV_BUFFER_INFO pInfoOut)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVStreamBufferGetInfo_64( hStream, hBuffer, ref pInfoOut):SVStreamBufferGetInfo_32( hStream, hBuffer, ref pInfoOut);
            }


            /** SVStreamClose
            *  Close the stream module and frees all the resources it allocated including the Buffers.
            *  @param [in] hStream Data Stream module handle from SVDeviceStreamOpen.
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVStreamClose", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamClose_64(void* hStream);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVStreamClose", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVStreamClose_32(void* hStream);
            internal static  unsafe
            SVSCamApiReturn SVStreamClose(void* hStream)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVStreamClose_64( hStream): SVStreamClose_32( hStream);
            }



            /** SVFeatureGetByName
            *  Get the handle for the features/node on hModule by name. 
            *  @param [in] hModule Module handle. 
            *  @param [in] featureName Feature/Node name to get.
            *  @param [out] phFeature feature handle.
            *  @return success or error code
            */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetByName", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetByName_64(void* hModule, sbyte* featureName, void* phFeature);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetByName", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetByName_32(void* hModule, sbyte* featureName, void* phFeature);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetByName(void* hModule, sbyte* featureName, void* phFeature)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureGetByName_64( hModule,  featureName, phFeature): SVFeatureGetByName_32( hModule, featureName, phFeature);
            }



            /** SVFeatureGetByIndex
              *  Get the handle for the features/node on hModule by Index.
              *  @param [in] hModule Module handle. 
              *  @param [in] iIndex Zero base index of features on current module. 
              *  @param [out] phFeature feature handle. 
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetByIndex", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetByIndex_64(void* hModule, uint iIndex, void* phFeature);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetByIndex", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetByIndex_32(void* hModule, uint iIndex, void* phFeature);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetByIndex(void* hModule, uint iIndex, void* phFeature)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVFeatureGetByIndex_64(hModule,  iIndex, phFeature) : SVFeatureGetByIndex_32(hModule,  iIndex, phFeature);
            }


            /** SVFeatureGetInfo
          *  Get the Feature Info on  hModule for a given Feature handle.
          *  @param [in] hModule Module handle.
          *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName. 
          *  @param [out] pInfoOut Information about the current features/node.  
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetInfo", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetInfo_64(void* hModule, void* hFeature, ref _SV_FEATURE_INFO pInfoOut);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetInfo", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetInfo_32(void* hModule, void* hFeature, ref _SV_FEATURE_INFO pInfoOut);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetInfo(void* hModule, void* hFeature, ref _SV_FEATURE_INFO pInfoOut)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureGetInfo_64( hModule,  hFeature, ref pInfoOut): SVFeatureGetInfo_32( hModule,  hFeature, ref pInfoOut);
            }



            /** SVFeatureGetValueBool
               *  Get the value of Feature with type SV_intfIBoolean.
               *  @param [in] hModule Module handle.
               *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
               *  @param [out] pBoolValue Boolean Value of hFeature.
               *  @param [in] verify (optional) Enable range verification.
               *  @param [in] ignoreCache (optional) Ignore cache and read directly from the module when set to true.
               *  @return success or error code
              */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetValueBool", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueBool_64(void* hModule, void* hFeature, bool* pBoolValue, bool verify = false, bool ignoreCache = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetValueBool", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueBool_32(void* hModule, void* hFeature, bool* pBoolValue, bool verify = false, bool ignoreCache = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetValueBool(void* hModule, void* hFeature, bool* pBoolValue, bool verify = false, bool ignoreCache = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureGetValueBool_64(hModule, hFeature,  pBoolValue,  verify ,  ignoreCache ):SVFeatureGetValueBool_32(hModule, hFeature,  pBoolValue,  verify ,  ignoreCache );
            }



            /** SVFeatureGetValueInt64
           *  Get the value of Feature with type SV_intfIInteger.
           *  @param [in] hModule Module handle.
           *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
           *  @param [out] pInt64Value INT64 Value of hFeature.
           *  @param [in] verify (optional) Enable range verification.
           *  @param [in] ignoreCache (optional) Ignore cache and read directly from the module when set to true.
           *  @return success or error code
          */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetValueInt64", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueInt64_64(void* hModule, void* hFeature, Int64* pInt64Value, bool verify = false, bool ignoreCache = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetValueInt64", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueInt64_32(void* hModule, void* hFeature, Int64* pInt64Value, bool verify = false, bool ignoreCache = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetValueInt64(void* hModule, void* hFeature, Int64* pInt64Value, bool verify = false, bool ignoreCache = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVFeatureGetValueInt64_64( hModule,  hFeature, pInt64Value,  verify ,  ignoreCache): SVFeatureGetValueInt64_32( hModule,  hFeature, pInt64Value,  verify ,  ignoreCache);
            }


            /** SVFeatureGetValueFloat
               *  Get the value of Feature with type SV_intfIFloat.
               *  @param [in] hModule Module handle.
               *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName. 
               *  @param [out] pFloatValue double Value of hFeature.
               *  @param [in] verify (optional) Enable range verification.
               *  @param [in] ignoreCache (optional) Ignore cache and read directly from the module when set to true.  
               *  @return success or error code
              */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetValueFloat", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueFloat_64(void* hModule, void* hFeature, double* pFloatValue, bool verify = false, bool ignoreCache = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetValueFloat", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueFloat_32(void* hModule, void* hFeature, double* pFloatValue, bool verify = false, bool ignoreCache = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetValueFloat(void* hModule, void* hFeature, double* pFloatValue, bool verify = false, bool ignoreCache = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVFeatureGetValueFloat_64(hModule,  hFeature,  pFloatValue,  verify ,  ignoreCache ): SVFeatureGetValueFloat_32(hModule,  hFeature,  pFloatValue,  verify ,  ignoreCache );
            }


            /** SVFeatureGetValueString
              *  Get the value of Feature with type SV_intfIString.
              *  @param [in] hModule Module handle.
              *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
              *  @param [out] strValue String Value of hFeature.
              *  @param [in] bufferSize Size in bytes of strValue.
              *  @param [in] verify (optional) Enable range verification.
              *  @param [in] ignoreCache (optional) Ignore cache and read directly from the module when set to true.   
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetValueString", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueString_64(void* hModule, void* hFeature, sbyte* strValue, uint bufferSize, bool verify = false, bool ignoreCache = false);

            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetValueString", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueString_32(void* hModule, void* hFeature, sbyte* strValue, uint bufferSize, bool verify = false, bool ignoreCache = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetValueString(void* hModule, void* hFeature, sbyte* strValue, uint bufferSize, bool verify = false, bool ignoreCache = false)
            {
                 return IntPtr.Size == 8 /* 64bit */  ? SVFeatureGetValueString_64( hModule, hFeature,  strValue,  bufferSize,  verify ,  ignoreCache ) : SVFeatureGetValueString_32( hModule, hFeature,  strValue,  bufferSize,  verify ,  ignoreCache );
            }


            /** SVFeatureGetValueInt64Enum
          *  Get the value of Feature with type SV_intfIEnumeration.
          *  @param [in] hModule Module handle.
          *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
          *  @param [out] pInt64Value INT64 Value of hFeature.
          *  @param [in] verify (optional) Enable range verification.
          *  @param [in] ignoreCache (optional) Ignore cache and read directly from the module when set to true.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetValueInt64Enum", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueInt64Enum_64(void* hModule, void* hFeature, Int64* pInt64Value, bool verify = false, bool ignoreCache = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetValueInt64Enum", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureGetValueInt64Enum_32(void* hModule, void* hFeature, Int64* pInt64Value, bool verify = false, bool ignoreCache = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureGetValueInt64Enum(void* hModule, void* hFeature, Int64* pInt64Value, bool verify = false, bool ignoreCache = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureGetValueInt64Enum_64( hModule, hFeature, pInt64Value, verify , ignoreCache): SVFeatureGetValueInt64Enum_32( hModule, hFeature, pInt64Value, verify , ignoreCache);
            }



            /** SVFeatureSetValueBool
          *  Set the value of Feature with type SV_intfIBoolean.
          *  @param [in] hModule Module handle.
          *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
          *  @param [in] boolValue bool8_t Value to set.
          *  @param [in] verify (optional) Enable access mode and range verification.
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureSetValueBool", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueBool_64(void* hModule, void* hFeature, bool boolValue, bool verify = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureSetValueBool", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueBool_32(void* hModule, void* hFeature, bool boolValue, bool verify = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureSetValueBool(void* hModule, void* hFeature, bool boolValue, bool verify = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureSetValueBool_64( hModule, hFeature, boolValue, verify): SVFeatureSetValueBool_32( hModule, hFeature, boolValue, verify);
            }


            /** SVFeatureSetValueInt64
               *  Set the value of Feature with type SV_intfIInteger.
               *  @param [in] hModule Module handle.
               *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
               *  @param [in] int64Value INT64 Value to set.
               *  @param [in] verify (optional) Enable access mode and range verification.
               *  @return success or error code
              */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureSetValueInt64", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueInt64_64(void* hModule, void* hFeature, Int64 int64Value, bool verify = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureSetValueInt64", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueInt64_32(void* hModule, void* hFeature, Int64 int64Value, bool verify = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureSetValueInt64(void* hModule, void* hFeature, Int64 int64Value, bool verify = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureSetValueInt64_64( hModule,  hFeature,  int64Value, verify): SVFeatureSetValueInt64_32( hModule,  hFeature,  int64Value, verify);
            }




            /** SVFeatureSetValueFloat
           *  Set the value of Feature with type SV_intfIFloat.
           *  @param [in] hModule Module handle.
           *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
           *  @param [in] floatValue double Value to set.
           *  @param [in] verify (optional) Enable access mode and range verification.
           *  @return success or error code
          */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureSetValueFloat", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueFloat_64(void* hModule, void* hFeature, double floatValue, bool verify = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureSetValueFloat", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueFloat_32(void* hModule, void* hFeature, double floatValue, bool verify = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureSetValueFloat(void* hModule, void* hFeature, double floatValue, bool verify = false)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVFeatureSetValueFloat_64(hModule, hFeature, floatValue, verify) : SVFeatureSetValueFloat_32(hModule, hFeature, floatValue, verify);
            }



            /** SVFeatureSetValueString
          *  Set the value of Feature with type SV_intfIString.
          *  @param [in] hModule Module handle.
          *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName. 
          *  @param [in] strValue String Value to set.
          *  @param [in] verify (optional) Enable access mode and range verification. 
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureSetValueString", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueString_64(void* hModule, void* hFeature, sbyte* strValue, bool verify = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureSetValueString", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueString_32(void* hModule, void* hFeature, sbyte* strValue, bool verify = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureSetValueString(void* hModule, void* hFeature, sbyte* strValue, bool verify = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureSetValueString_64( hModule, hFeature, strValue, verify):SVFeatureSetValueString_32( hModule, hFeature, strValue, verify);
            }



            /** SVFeatureSetValueInt64Enum
          *  Set the value of Feature with type SV_intfIEnumeration.
          *  @param [in] hModule Module handle.
          *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
          *  @param [in] int64Value INT64 Value to set.
          *  @param [in] verify (optional) Enable access mode and range verification.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureSetValueInt64Enum", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueInt64Enum_64(void* hModule, void* hFeature, Int64 int64Value, bool verify = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureSetValueInt64Enum", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureSetValueInt64Enum_32(void* hModule, void* hFeature, Int64 int64Value, bool verify = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureSetValueInt64Enum(void* hModule, void* hFeature, Int64 int64Value, bool verify = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureSetValueInt64Enum_64(hModule, hFeature,  int64Value,  verify):SVFeatureSetValueInt64Enum_32(hModule, hFeature,  int64Value,  verify);
            }


            /** SVFeatureCommandExecute
          *  Execute the Feature with type SV_intfICommand.
          *  @param [in] hModule Module handle.
          *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
          *  @param [in] Timeout Timeout in MS.
          *  @param [in] bWait Ignores timeout and returns immediately when set to false. Wait until command is executed or Timeout expires when set to true 
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureCommandExecute", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureCommandExecute_64(void* hModule, void* hFeature, uint Timeout, bool bWait = false);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureCommandExecute", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureCommandExecute_32(void* hModule, void* hFeature, uint Timeout, bool bWait = false);
            internal static  unsafe
            SVSCamApiReturn SVFeatureCommandExecute(void* hModule, void* hFeature, uint Timeout, bool bWait = false)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureCommandExecute_64( hModule,  hFeature, Timeout, bWait):SVFeatureCommandExecute_32( hModule,  hFeature, Timeout, bWait);
            }


            /** SVFeatureEnumSubFeatures
          *  Enumerate all the Subfeatures of Feature with type SV_intfIEnumeration.
          *  @param [in]  hModule Module handle.
          *  @param [in]  hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
          *  @param [in]  iIndex Zero based index of the child/sub features of hFeature.
          *  @param [out] subFeatureName Name of the subfeature on iIndex.
          *  @param [in] bufferSize Buffer size in bytes of subFeatureName.
          *  @param [out] pValue (optional) Value of the subfeature. 
          *  This will be the value to set on SVFeatureSetValueInt64Enum when updating the hFeature. 
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureEnumSubFeatures", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureEnumSubFeatures_64(void* hModule, void* hFeature, int iIndex, sbyte* subFeatureName, uint bufferSize, int* pValue);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureEnumSubFeatures", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureEnumSubFeatures_32(void* hModule, void* hFeature, int iIndex, sbyte* subFeatureName, uint bufferSize, int* pValue);
            internal static  unsafe
            SVSCamApiReturn SVFeatureEnumSubFeatures(void* hModule, void* hFeature, int iIndex, sbyte* subFeatureName, uint bufferSize, int* pValue)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureEnumSubFeatures_64(hModule,  hFeature, iIndex,  subFeatureName, bufferSize, pValue): SVFeatureEnumSubFeatures_32(hModule,  hFeature, iIndex,  subFeatureName, bufferSize, pValue);
            }



            /** SVFeatureRegisterInvalidateCB2
              *  Register callback to be called when hFeature is invalidated.
              *  @param [in] hModule Module handle.
              *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.
              *  @param [in] objCb Callback object.This can't be NULL.
              *  @param [in] pfnFeatureInvalidateCb Callback function.This can't be NULL. 
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureRegisterInvalidateCB", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureRegisterInvalidateCB2_64(void* hModule, void* hFeature, Object objCb, [MarshalAs(UnmanagedType.FunctionPtr)] SV_CB_FEATURE_INVALIDATED_PFN2 pfnFeatureInvalidateCb2);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureRegisterInvalidateCB", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureRegisterInvalidateCB2_32(void* hModule, void* hFeature, Object objCb, [MarshalAs(UnmanagedType.FunctionPtr)] SV_CB_FEATURE_INVALIDATED_PFN2 pfnFeatureInvalidateCb2);
            internal static  unsafe
            SVSCamApiReturn SVFeatureRegisterInvalidateCB2(void* hModule, void* hFeature, Object objCb, [MarshalAs(UnmanagedType.FunctionPtr)] SV_CB_FEATURE_INVALIDATED_PFN2 pfnFeatureInvalidateCb2)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureRegisterInvalidateCB2_64( hModule, hFeature,  objCb,   pfnFeatureInvalidateCb2):SVFeatureRegisterInvalidateCB2_32( hModule, hFeature,  objCb,   pfnFeatureInvalidateCb2);
            }
         

            /** SVFeatureUnRegisterInvalidateCB
              *  Unregister callback function registered on SVFeatureRegisterInvalidateCB
              *  @param [in] hModule Module handle.
              *  @param [in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.  
              *  @return success or error code
             */

            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureUnRegisterInvalidateCB", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureUnRegisterInvalidateCB_64(void* hModule, void* hFeature);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureUnRegisterInvalidateCB", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVFeatureUnRegisterInvalidateCB_32(void* hModule, void* hFeature);
            internal static  unsafe
            SVSCamApiReturn SVFeatureUnRegisterInvalidateCB(void* hModule, void* hFeature)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVFeatureUnRegisterInvalidateCB_64( hModule,hFeature):SVFeatureUnRegisterInvalidateCB_32( hModule,hFeature);
            }



            /** SVUtilBuffer12BitTo8Bit
          *  Utility function to convert 12 bit Image format to 8 Bit Mono
          *  @param [in] srcInfo Information of the source Image to convert.
          *  @param [out] pDest Pointer to user allocated buffer to receive the converted buffer.
          *  @param [in] pDestLength Size in bytes of the buffer passed on pDest. 
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVUtilBuffer12BitTo8Bit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBuffer12BitTo8Bit_64(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVUtilBuffer12BitTo8Bit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBuffer12BitTo8Bit_32(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);
            internal static  unsafe
            SVSCamApiReturn SVUtilBuffer12BitTo8Bit(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVUtilBuffer12BitTo8Bit_64( srcInfo,pDest,pDestLength):  SVUtilBuffer12BitTo8Bit_32( srcInfo,pDest,pDestLength);
            }

            /** SVUtilBuffer12BitTo16Bit
              *  Utility function to convert 12 bit Image format to 16 Bit Mono.
              *  @param [in] srcInfo Information of the source Image to convert.
              *  @param [out] pDest Pointer to user allocated buffer to receive the converted buffer.
              *  @param [in] pDestLength Size in bytes of the buffer passed on pDest. 
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVUtilBuffer12BitTo16Bit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBuffer12BitTo16Bit_64(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVUtilBuffer12BitTo16Bit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBuffer12BitTo16Bit_32(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);
            internal static  unsafe
            SVSCamApiReturn SVUtilBuffer12BitTo16Bit(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVUtilBuffer12BitTo16Bit_64( srcInfo, pDest, pDestLength): SVUtilBuffer12BitTo16Bit_32( srcInfo, pDest, pDestLength); 
            }


            /** SVUtilBuffer16BitTo8Bit
               *  Utility function to convert 16 bit Image format to 8 Bit Mono.
               *  @param [in] srcInfo Information of the source Image to convert.
               *  @param [out] pDest Pointer to user allocated buffer to receive the converted buffer.
               *  @param [in] pDestLength Size in bytes of the buffer passed on pDest.  
               *  @return success or error code
              */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVUtilBuffer16BitTo8Bit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBuffer16BitTo8Bit_64(_SV_BUFFER_INFO srcInfo, sbyte* pDest, int pDestLength);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVUtilBuffer16BitTo8Bit", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBuffer16BitTo8Bit_32(_SV_BUFFER_INFO srcInfo, sbyte* pDest, int pDestLength);
            internal static  unsafe
            SVSCamApiReturn SVUtilBuffer16BitTo8Bit(_SV_BUFFER_INFO srcInfo, sbyte* pDest, int pDestLength)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVUtilBuffer16BitTo8Bit_64( srcInfo, pDest, pDestLength) : SVUtilBuffer16BitTo8Bit_32( srcInfo, pDest, pDestLength); 
            }



            /** SVUtilBufferBayerToRGB
              * Utility function to convert BAYXX8 or BAYXX12 Image format to 24 bit RGB
              *  @param [in] srcInfo Information of the source Image to convert.
              *  @param [out] pDest Pointer to user allocated buffer to receive the converted buffer.
              *  @param [in] pDestLength Size in bytes of the buffer passed on pDest. 
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVUtilBufferBayerToRGB", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBufferBayerToRGB_64(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVUtilBufferBayerToRGB", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVUtilBufferBayerToRGB_32(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);
            internal static  unsafe
            SVSCamApiReturn SVUtilBufferBayerToRGB(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVUtilBufferBayerToRGB_64( srcInfo,  pDest,  pDestLength) : SVUtilBufferBayerToRGB_32( srcInfo,  pDest,  pDestLength);
            }


            /** SVLogRegister
              *  Log messages can be requested for various log levels:
              *   SV_LOG_LVL_CRITICAL 
              *   SV_LOG_LVL_ERROR 
              *   SV_LOG_LVL_WARNING 
              *   SV_LOG_LVL_INFO 
              *   SV_LOG_LVL_CAMERA_IO 
              *   SV_LOG_LVL_DEBUG
              *  
              *  @param [in] moduleName module name that will show on left most side of log traces.
              *  @param [in] debugLevel debug level to set on specific module. 
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLogRegister", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVLogRegister_64(byte* moduleName, uint debugLevel);
                        [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLogRegister", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVLogRegister_32(byte* moduleName, uint debugLevel);
            internal static  unsafe
            SVSCamApiReturn SVLogRegister(byte* moduleName, uint debugLevel)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVLogRegister_64( moduleName,  debugLevel) : SVLogRegister_32( moduleName,  debugLevel);
            }



         /** SVLogEnableWindbg
          *  allows enabling or disabling system logging.
          *  @param [in] bEnable flag to enable or disable system logging.
          *  @return success or error code
         */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLogEnableWindbg", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVLogEnableWindbg_64(bool bEnable);
            [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLogEnableWindbg", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
            SVSCamApiReturn SVLogEnableWindbg_32(bool bEnable);
  
            internal static  unsafe
            SVSCamApiReturn SVLogEnableWindbg(bool bEnable)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVLogEnableWindbg_64( bEnable): SVLogEnableWindbg_32( bEnable);
            }




            /** SVLogEnableFileLogging
              *  Resulting log messages can be written into a log file if enabled.
              *  @param [in] bEnable flag to enable or disable file writing
              *  @param [in] logFileName Log file name to be generated. Must have read/write access. 
              *  it can be set to NULL when disabling File Logging. 
              *  @return success or error code
             */
            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLogEnableFileLogging", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVLogEnableFileLogging_64(bool bEnable, byte* logFileName);
                      [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLogEnableFileLogging", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVLogEnableFileLogging_32(bool bEnable, byte* logFileName);
 
            internal static  unsafe
             SVSCamApiReturn SVLogEnableFileLogging(bool bEnable, byte* logFileName)
            {
                 return IntPtr.Size == 8 /* 64bit */ ?  SVLogEnableFileLogging_64( bEnable,logFileName):SVLogEnableFileLogging_32( bEnable,logFileName);
            }


			/** SVLogSetGlobalDebugLevel
			  *  Set Global debug Level. This will overwrite module specific debug level set from SVLogRegister. 
			  *  @param [in] debugLevel debug level to be set.
			  *  @return success or error code
			*/

            [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLogSetGlobalDebugLevel", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVLogSetGlobalDebugLevel_64(uint debugLevel);
                       [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLogSetGlobalDebugLevel", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVLogSetGlobalDebugLevel_32(uint debugLevel);

            internal static  unsafe
             SVSCamApiReturn SVLogSetGlobalDebugLevel(uint debugLevel)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVLogSetGlobalDebugLevel_64(debugLevel): SVLogSetGlobalDebugLevel_32(debugLevel);
            }

			
			/** SVUtilBufferBayerToRGB Utility function to convert BAYXX8 or BAYXX12 Image format to 32 bit RGB 
			*  @param[in] srcInfo Information of the source Image to convert.  
			*  @param[out] pDest Pointer to user allocated buffer to receive the converted buffer.  
			*  @param[in] pDestLength Size in bytes of the buffer passed on pDest.  
			*  @return success or error code 
			*/
			[DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVUtilBufferBayerToRGB32", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVUtilBufferBayerToRGB32_64(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);
                       [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVUtilBufferBayerToRGB32", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVUtilBufferBayerToRGB32_32(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength);

            internal static  unsafe
             SVSCamApiReturn SVUtilBufferBayerToRGB32(_SV_BUFFER_INFO srcInfo, byte* pDest, int pDestLength)
            {
                 return IntPtr.Size == 8 /* 64bit */ ? SVUtilBufferBayerToRGB32_64(srcInfo,pDest,pDestLength): SVUtilBufferBayerToRGB32_32(srcInfo,pDest,pDestLength);
            }



			/** isVersionCompliant. The DLL's version at compile time will be checked against an expected version at runtime. 
			  *  @param[in] expectedVersion a pointer to a version structure with the expected DLL version  
			  *  @param[out] pCurrentVersion a pointer to a version structure for the current DLL version  
			  *  @return success or error code 
			*/
             [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVLibIsVersionCompliant", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVLibIsVersionCompliant_64( SV_LIB_VERSION expectedVersion, ref SV_LIB_VERSION pCurrentVersion);
                       [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVLibIsVersionCompliant", CallingConvention = CallingConvention.Cdecl)]
            internal static extern unsafe
             SVSCamApiReturn SVLibIsVersionCompliant_32( SV_LIB_VERSION expectedVersion, ref SV_LIB_VERSION pCurrentVersion);

            internal static  unsafe
             SVSCamApiReturn SVLibIsVersionCompliant( SV_LIB_VERSION expectedVersion, ref SV_LIB_VERSION pCurrentVersion)
            {
                return IntPtr.Size == 8 /* 64bit */ ? SVLibIsVersionCompliant_64(expectedVersion, ref pCurrentVersion) : SVLibIsVersionCompliant_32(expectedVersion, ref pCurrentVersion);
            }
 
			/** SVFeatureGetValueEnum Get feature enumeration value as string. 
			  *  @param[in] hModule Module handle.  
			  *  @param[in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.  
			  *  @param[in,out] buffer Pointer to a user allocated string buffer for the enumeration value.  
			  *  @param[in] bufferSize size of StringBuffer for the enumeration value.  
			  *  @param[in] verify (optional) Enable range verification.  
			  *  @param[in] ignoreCache (optional) Ignore cache and read directly from the module when set to true.  
			  *  @return success or error code 
			  */
			  [DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetValueEnum", CallingConvention = CallingConvention.Cdecl)]
			  internal static extern unsafe
			 SVSCamApiReturn SVFeatureGetValueEnum_64(
													  void *hModule, void * hFeature,
													  sbyte* buffer, uint bufferSize,
													  bool verify = false, bool ignoreCache = false);
													  
			 [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetValueEnum", CallingConvention = CallingConvention.Cdecl)]
			internal static extern unsafe
			SVSCamApiReturn SVFeatureGetValueEnum_32(void *hModule, void * hFeature,
													sbyte* buffer, uint bufferSize, 
													bool verify = false, bool ignoreCache = false);
			internal static  unsafe
			SVSCamApiReturn SVFeatureGetValueEnum(void *hModule, void * hFeature,
												 sbyte* buffer, uint bufferSize,
												bool verify = false, bool ignoreCache = false)
						{
							 return IntPtr.Size == 8 /* 64bit */ ? SVFeatureGetValueEnum_64( hModule,  hFeature,
																							 buffer, bufferSize,
																							 verify ,  ignoreCache ):

																	SVFeatureGetValueEnum_32(hModule,  hFeature,
																							 buffer,   bufferSize,
																							 verify,  ignoreCache);	
						}


			/** SVFeatureSetValueEnum Set feature enumeration value as string. 
			*  @param[in] hModule Module handle.  
			*  @param[in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.  
			*  @param[out] buffer enumeration value to set.  
			*  @param[in] verify (optional) Enable range verification.  
			*  @return success or error code 
			*/
			[DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureSetValueEnum", CallingConvention = CallingConvention.Cdecl)]
			internal static extern unsafe
			 SVSCamApiReturn SVFeatureSetValueEnum_64(void *hModule, void * hFeature, sbyte* buffer, bool verify = false);
					   [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureSetValueEnum", CallingConvention = CallingConvention.Cdecl)]
			internal static extern unsafe
			 SVSCamApiReturn SVFeatureSetValueEnum_32(void *hModule, void * hFeature, sbyte* buffer, bool verify = false);

			internal static  unsafe
			 SVSCamApiReturn SVFeatureSetValueEnum(void *hModule, void * hFeature, sbyte* buffer, bool verify = false)
			{
			  return IntPtr.Size == 8 /* 64bit */ ? SVFeatureSetValueEnum_64(hModule,hFeature,buffer, verify): SVFeatureSetValueEnum_32( hModule,hFeature,buffer, verify);
			}



			/** SVDeviceSaveSettingsToString The current streamable settings will be stored in StringBuffer. 
			*  @param[out] hDevice Device module handle from SVInterfaceDeviceOpen.  
			*  @param[in,out] buffer pointer to the user allocated StringBuffer where to receive the streamable settings.  
			*  @param[in,out] pBufferSize pointer to the size of StringBuffer.  
			*  @return success or error code 
			*/ 
			[DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceSaveSettingsToString", CallingConvention = CallingConvention.Cdecl)]
			internal static extern unsafe
			SVSCamApiReturn SVDeviceSaveSettingsToString_64(void* hDevice, sbyte* buffer, uint *pBufferSize);
			[DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceSaveSettingsToString", CallingConvention = CallingConvention.Cdecl)]
			internal static extern unsafe
            SVSCamApiReturn SVDeviceSaveSettingsToString_32(void* hDevice, sbyte* buffer, uint* pBufferSize);

			internal static  unsafe
            SVSCamApiReturn SVDeviceSaveSettingsToString(void* hDevice, sbyte* buffer, uint* pBufferSize)
			{
				return IntPtr.Size == 8 /* 64bit */ ? SVDeviceSaveSettingsToString_64(hDevice,buffer,pBufferSize): SVDeviceSaveSettingsToString_32(hDevice,buffer,pBufferSize);
			}

		
	
		
			/** SVDeviceLoadSettingsFromString load the streamable camera settings from the given StringBuffer. 
			*  @param[in] hDevice Device module handle from SVInterfaceDeviceOpen.  
			*  @param[in] buffer a StringBuffer where to load the settings from.  
			*  @return success or error code 
			*/
			[DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVDeviceLoadSettingsFromString", CallingConvention = CallingConvention.Cdecl)]
			internal static extern unsafe
            SVSCamApiReturn SVDeviceLoadSettingsFromString_64(void* hDevice, sbyte* buffer);
			[DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVDeviceLoadSettingsFromString", CallingConvention = CallingConvention.Cdecl)]
			internal static extern unsafe
            SVSCamApiReturn SVDeviceLoadSettingsFromString_32(void* hDevice, sbyte* buffer);

			internal static  unsafe
            SVSCamApiReturn SVDeviceLoadSettingsFromString(void* hDevice, sbyte* buffer)
			{
				return IntPtr.Size == 8 /* 64bit */ ? SVDeviceLoadSettingsFromString_64(hDevice,buffer): SVDeviceLoadSettingsFromString_32(hDevice,buffer);
			}
		
		
		
		/** SVFeatureListRefresh refresh the feature list. 
		  *  @param[in] hModule Module handle  
		  *  @return success or error code 
		 */

		[DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureListRefresh", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe
		SVSCamApiReturn SVFeatureListRefresh_64(void *hModule);
		[DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureListRefresh", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe
		SVSCamApiReturn SVFeatureListRefresh_32(void *hModule);

		internal static  unsafe
		SVSCamApiReturn SVFeatureListRefresh(void *hModule)
		{
			return IntPtr.Size == 8 /* 64bit */ ? SVFeatureListRefresh_64(hModule): SVFeatureListRefresh_32(hModule);
		}


		/** SVUtilSaveImageToPNGFile Write image as a PNG file to disk. 
		  *  @param[out] info Information about the current Buffer.  
		  *  @param[in] fileName a path for the PNG file  
		  *  @return success or error code 
		  */

		[DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVUtilSaveImageToPNGFile", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe
		SVSCamApiReturn SVUtilSaveImageToPNGFile_64( _SV_BUFFER_INFO info, byte*  fileName);
		[DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVUtilSaveImageToPNGFile", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe
		SVSCamApiReturn SVUtilSaveImageToPNGFile_32( _SV_BUFFER_INFO info, byte*  fileName);

		internal static  unsafe
		SVSCamApiReturn SVUtilSaveImageToPNGFile( _SV_BUFFER_INFO info, byte*  fileName)
		{
			return IntPtr.Size == 8 /* 64bit */ ? SVUtilSaveImageToPNGFile_64(info,fileName): SVUtilSaveImageToPNGFile_32(info,fileName);
		}


		/** SVFeatureGetDescription Get the description of Feature with the a specific Feature handle. 
		  *  @param[in] hModule Module handle.  
		  *  @param[in] hFeature Feature handle from SVFeatureGetByIndex or SVFeatureGetByName.  
		  *  @param[in,out] pBuffer buffer pointer to the user allocated StringBuffer where to receive the feature description.  
		  *  @param[in] bufferSize size of StringBuffer.  
		  *  @return success or error code 
		 */
			
		[DllImport(DefineConstants.SVGenSDK_DLL64, EntryPoint = "SVFeatureGetDescription", CallingConvention = CallingConvention.Cdecl)]
		internal static extern unsafe
		SVSCamApiReturn SVFeatureGetDescription_64( void *hModule, void * hFeature,  sbyte* pBuffer, uint bufferSize);

        [DllImport(DefineConstants.SVGenSDK_DLL, EntryPoint = "SVFeatureGetDescription", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe
        SVSCamApiReturn SVFeatureGetDescription_32(void* hModule, void* hFeature, sbyte* pBuffer, uint bufferSize);
													 

		internal static  unsafe
		SVSCamApiReturn SVFeatureGetDescription( void *hModule, void * hFeature, sbyte* pBuffer, uint bufferSize)
		{
			return IntPtr.Size == 8 /* 64bit */ ? SVFeatureGetDescription_64(hModule,hFeature, pBuffer,bufferSize ): SVFeatureGetDescription_32(hModule,hFeature, pBuffer,bufferSize);
		}
		
		
	}


	
        public unsafe
        SVSCamApiReturn
        SVS_LibInit(string TLIPath = null, string genicamRootDir = null, string genicamCacheDir = null, string clProtocolDriverDir = null)
        {
            byte[] _TLIPath = Encoding.ASCII.GetBytes(TLIPath);
            byte[] _genicamRootDir = Encoding.ASCII.GetBytes(genicamRootDir);
            byte[] _genicamCacheDir = Encoding.ASCII.GetBytes(genicamCacheDir);
            byte[] _clProtocolDriverDir = Encoding.ASCII.GetBytes(clProtocolDriverDir);

            fixed (byte* tlP = _TLIPath)
            fixed (byte* genR = _genicamRootDir)
            fixed (byte* geCa = _genicamCacheDir)
            fixed (byte* clP = _clProtocolDriverDir)
            return NativeMethods.SVLibInit(tlP, genR, geCa, clP);

        }

        public unsafe
        SVSCamApiReturn
        SVS_LibSystemGetCount(ref UInt32 tlCount)
        {
            fixed (UInt32* p = &tlCount)
            {
                return NativeMethods.SVLibSystemGetCount(p);
            }
        }

        public unsafe
        SVSCamApiReturn
        SVS_LibSystemGetInfo(UInt32 index, ref   _SV_TL_INFO pInfoOut)
        {

            return NativeMethods.SVLibSystemGetInfo(index, ref pInfoOut);
        }

        public unsafe
        SVSCamApiReturn
        SVS_LibSystemOpen(uint uiIndex, ref  IntPtr phSystemOut)
        {
            fixed (void* p = &phSystemOut)
            {
                return NativeMethods.SVLibSystemOpen(uiIndex, p);
            }
        }

        public unsafe
        SVSCamApiReturn
        SVS_LibClose()
        {
            return NativeMethods.SVLibClose();
        }

        public unsafe
        SVSCamApiReturn
        SVS_SystemUpdateInterfaceList(IntPtr hSystem, ref bool pbChanged, uint timeOut)
        {
            void* _hSystem = hSystem.ToPointer();
            fixed (bool* p = &pbChanged)
            {
                return NativeMethods.SVSystemUpdateInterfaceList(_hSystem, p, timeOut);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_SystemGetNumInterfaces(IntPtr hSystem, ref uint piNumIfaces)
        {
            void* _hSystem = hSystem.ToPointer();
            fixed (uint* p = &piNumIfaces)
            {
                return NativeMethods.SVSystemGetNumInterfaces(_hSystem, p);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_SystemGetInterfaceId(IntPtr hSystem, uint Index, ref string pInterfaceId, ref  uint pSize)
        {
            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            void* _hSystem = hSystem.ToPointer();
            sbyte[] str = new sbyte[512];
            {

                fixed (uint* psize = &pSize)
                {
                    fixed (sbyte* _pInterfaceId = str)
                    {
                        ret = NativeMethods.SVSystemGetInterfaceId(_hSystem, Index, _pInterfaceId, psize);
                        pInterfaceId = new string(_pInterfaceId);
                    }
                }
            }
            return ret;
        }


        public unsafe
        SVSCamApiReturn
        SVS_SystemInterfaceGetInfo(IntPtr hSystem, string pInterfaceId, ref  _SV_INTERFACE_INFO pInfoOut)
        {
            void* _hSystem = hSystem.ToPointer();
            byte[] _pInterfaceId = Encoding.ASCII.GetBytes(pInterfaceId);
            fixed (byte* piD = _pInterfaceId)
                return NativeMethods.SVSystemInterfaceGetInfo(_hSystem, piD, ref (pInfoOut));
        }


        public unsafe
        SVSCamApiReturn
         SVS_SystemInterfaceOpen(IntPtr hSystem, string pInterfaceId, ref IntPtr phInterfaceOut)
        {
            void* _hSystem = hSystem.ToPointer();
            byte[] _pInterfaceId = Encoding.ASCII.GetBytes(pInterfaceId);

            fixed (void* p = &phInterfaceOut)
            {
                fixed (byte* pifId = _pInterfaceId)
                    return NativeMethods.SVSystemInterfaceOpen(_hSystem, pifId, p);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_SystemClose(IntPtr hSystem)
        {
            void* _hSystem = hSystem.ToPointer();
            return NativeMethods.SVSystemClose(_hSystem);
        }


        public unsafe
        SVSCamApiReturn
        SVS_InterfaceGetInfo(IntPtr hInterface, ref   _SV_INTERFACE_INFO pInfoOut)
        {
            void* _hInterface = hInterface.ToPointer();
            return NativeMethods.SVInterfaceGetInfo(_hInterface, ref  (pInfoOut));
        }


        public unsafe
        SVSCamApiReturn
         SVS_InterfaceUpdateDeviceList(IntPtr hInterface, ref bool pbChanged, uint timeOut)
        {
            void* _hInterface = hInterface.ToPointer();
            fixed (bool* p = &pbChanged)
            {
                return NativeMethods.SVInterfaceUpdateDeviceList(_hInterface, p, timeOut);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_InterfaceGetNumDevices(IntPtr hInterface, ref uint piDevices)
        {
            void* _hInterface = hInterface.ToPointer();
            fixed (uint* p = &piDevices)
            {
                return NativeMethods.SVInterfaceGetNumDevices(_hInterface, p);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_InterfaceGetDeviceId(IntPtr hInterface, uint Index, ref string pDeviceId, ref uint pSize)
        {
            void* _hInterface = hInterface.ToPointer();
            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            sbyte[] str = new sbyte[512];
            fixed (uint* psize = &pSize)
            {
                fixed (sbyte* _pDeviceId = str)
                {
                    ret = NativeMethods.SVInterfaceGetDeviceId(_hInterface, Index, _pDeviceId, psize);
                    pDeviceId = new string(_pDeviceId);
                }
            }
            return ret;
        }


        public unsafe
        SVSCamApiReturn
        SVS_InterfaceDeviceGetInfo(IntPtr hInterface, string pDeviceId, ref _SV_DEVICE_INFO pInfoOut)
        {
            void* _hInterface = hInterface.ToPointer();
            byte[] _pDeviceId = Encoding.ASCII.GetBytes(pDeviceId);
            fixed (byte* pDevId = _pDeviceId)
                return NativeMethods.SVInterfaceDeviceGetInfo(_hInterface, pDevId, ref  pInfoOut);
        }


        public unsafe
        SVSCamApiReturn
        SVS_InterfaceClose(IntPtr hInterface)
        {
            void* _hInterface = hInterface.ToPointer();

            return NativeMethods.SVInterfaceClose(_hInterface);
        }


        public unsafe
        SVSCamApiReturn
        SVS_InterfaceDeviceOpen(IntPtr hInterface,
                               string pDeviceId,
                                SV_DEVICE_ACCESS_FLAGS_LIST accessFlags,
                                ref IntPtr phDeviceOut,
                                ref IntPtr phRemoteDeviceOut)
        {
            void* _hInterface = hInterface.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(pDeviceId);
            fixed (void* phdev = &phDeviceOut)
            {
                fixed (void* phremdev = &phRemoteDeviceOut)
                {
                    fixed (byte* _pDeviceId = bytes)
                    {
                        return NativeMethods.SVInterfaceDeviceOpen(_hInterface, (sbyte*)_pDeviceId, accessFlags, phdev, phremdev);
                    }
                }
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_DeviceGetInfo(IntPtr hDevice, ref _SV_DEVICE_INFO pInfoOut)
        {
            void* _hDevice = hDevice.ToPointer();
            return NativeMethods.SVDeviceGetInfo(_hDevice, ref pInfoOut);
        }


        public unsafe
        SVSCamApiReturn
         SVS_DeviceGetNumStreams(IntPtr hDevice, ref uint piStreams)
        {
            void* _hDevice = hDevice.ToPointer();
            fixed (uint* p = &piStreams)
            {
                return NativeMethods.SVDeviceGetNumStreams(_hDevice, p);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_DeviceGetStreamId(IntPtr hDevice, uint Index, ref string pStreamId, ref uint pSize)
        {
            void* _hDevice = hDevice.ToPointer();
            sbyte[] bytes = new sbyte[SVcamApi.DefineConstants.SV_STRING_SIZE];
            fixed (sbyte* _pStreamId = bytes)
            {
                fixed (uint* psize = &pSize)
                {
                    SVSCamApiReturn ret = NativeMethods.SVDeviceGetStreamId(_hDevice, Index, _pStreamId, psize);
                    pStreamId = new string(_pStreamId);
                    return ret;
                }
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_DeviceSaveSettings(IntPtr hDevice, sbyte* fileName)
        {
            void* _hDevice = hDevice.ToPointer();
            return NativeMethods.SVDeviceSaveSettings(_hDevice, fileName);
        }


        public unsafe
        SVSCamApiReturn
        SVDeviceLoadSettings(IntPtr hDevice, sbyte* fileName)
        {
            void* _hDevice = hDevice.ToPointer();
            return NativeMethods.SVDeviceLoadSettings(_hDevice, fileName);
        }


        public unsafe
        SVSCamApiReturn
         SVS_DeviceRead(IntPtr hDevice, uint nAddress, ref IntPtr pData, ref  uint pSize)
        {
            void* _hDevice = hDevice.ToPointer();
            fixed (void* pdata = &pData)
            {
                fixed (uint* psize = &pSize)
                {
                    return NativeMethods.SVDeviceRead(_hDevice, nAddress, pdata, psize);
                }
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_DeviceWrite(IntPtr hDevice, uint nAddress, sbyte* pData, ref  uint pSize)
        {
            void* _hDevice = hDevice.ToPointer();
            fixed (uint* p = &pSize)
            {
                return NativeMethods.SVDeviceWrite(_hDevice, nAddress, pData, p);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_DeviceStreamOpen(IntPtr hDevice, string sDataStreamID, ref IntPtr phStream)
        {
            void* _hDevice = hDevice.ToPointer();
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(sDataStreamID);
            fixed (void* phs = &phStream)
            {
                fixed (byte* _sDataStreamID = bytes)
                    return NativeMethods.SVDeviceStreamOpen(_hDevice, (sbyte*)_sDataStreamID, phs);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_DeviceClose(IntPtr hDevice)
        {
            void* _hDevice = hDevice.ToPointer();
            return NativeMethods.SVDeviceClose(_hDevice);
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamAcquisitionStart(IntPtr hStream, SV_ACQ_START_FLAGS_LIST flags, UInt64 iNumToAcquire)
        {
            void* _hStream = hStream.ToPointer();
            return NativeMethods.SVStreamAcquisitionStart(_hStream, flags, iNumToAcquire);
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamAcquisitionStop(IntPtr hStream, SV_ACQ_STOP_FLAGS_LIST flags)
        {
            void* _hStream = hStream.ToPointer();
            return NativeMethods.SVStreamAcquisitionStop(_hStream, flags);
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamAnnounceBuffer(IntPtr hStream, sbyte* pBuffer, uint uiSize, IntPtr pPrivate, ref IntPtr phBuffer)
        {
            void* _hStream = hStream.ToPointer();
            void* _pPrivate = pPrivate.ToPointer();
            fixed (void* phs = &phBuffer)
            {
                return NativeMethods.SVStreamAnnounceBuffer(_hStream, pBuffer, uiSize, _pPrivate, phs);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamAllocAndAnnounceBuffer(IntPtr hStream, uint uiSize, IntPtr pPrivate, ref IntPtr phBuffer)
        {
            void* _hStream = hStream.ToPointer();
            void* _pPrivate = pPrivate.ToPointer();
            fixed (void* phs = &phBuffer)
            {
                return NativeMethods.SVStreamAllocAndAnnounceBuffer(_hStream, uiSize, _pPrivate, phs);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamRevokeBuffer(IntPtr hStream, IntPtr hBuffer, ref IntPtr pBuffer, ref  IntPtr pPrivate)
        {
            void* _hStream = hStream.ToPointer();
            void* _hBuffer = hBuffer.ToPointer();
            fixed (void* pbuf = &pBuffer)
            {
                fixed (void* ppri = &pPrivate)
                {
                    return NativeMethods.SVStreamRevokeBuffer(_hStream, _hBuffer, pbuf, ppri);
                }
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamQueueBuffer(IntPtr hStream, IntPtr hBuffer)
        {
            void* _hStream = hStream.ToPointer();
            void* _hBuffer = hBuffer.ToPointer();
            return NativeMethods.SVStreamQueueBuffer(_hStream, _hBuffer);

        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamGetBufferId(IntPtr hStream, uint iIndex, ref  IntPtr phBuffer)
        {
            void* _hStream = hStream.ToPointer();
            fixed (void* phbuf = &phBuffer)
            {
                return NativeMethods.SVStreamGetBufferId(_hStream, iIndex, phbuf);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamFlushQueue(IntPtr hStream, SV_ACQ_QUEUE_TYPE_LIST iOperation)
        {
            void* _hStream = hStream.ToPointer();
            return NativeMethods.SVStreamFlushQueue(_hStream, iOperation);
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamWaitForNewBuffer(IntPtr hStream, ref IntPtr ppUserData, ref IntPtr phBufferOut, uint timeOut)
        {
            void* _hStream = hStream.ToPointer();
            fixed (void* pusdata = &ppUserData)
            {
                fixed (void* phbuf = &phBufferOut)
                {
                    return NativeMethods.SVStreamWaitForNewBuffer(_hStream, pusdata, phbuf, timeOut);
                }
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_StreamBufferGetInfo(IntPtr hStream, IntPtr hBuffer, ref _SV_BUFFER_INFO pInfoOut)
        {
            void* _hStream = hStream.ToPointer();
            void* _hBuffer = hBuffer.ToPointer();
            return NativeMethods.SVStreamBufferGetInfo(_hStream, _hBuffer, ref pInfoOut);
        }


        public unsafe
        SVSCamApiReturn
        SVS_SVStreamClose(IntPtr hStream)
        {
            void* _hStream = hStream.ToPointer();
            return NativeMethods.SVStreamClose(_hStream);
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetByName(IntPtr hModule, string featureName, ref IntPtr phFeature)
        {
            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            void* _hModule = hModule.ToPointer();
            byte[] fname = Encoding.ASCII.GetBytes(featureName);

            fixed (void* phfeatur = &phFeature)
            {
                fixed (byte* _featureName = fname)
                {
                    ret = NativeMethods.SVFeatureGetByName(_hModule, (sbyte*)_featureName, phfeatur);
                    return ret;
                }
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetByIndex(IntPtr hModule, uint iIndex, ref IntPtr phFeature)
        {
            void* _hModule = hModule.ToPointer();

            fixed (void* phfeatur = &phFeature)
            {
                return NativeMethods.SVFeatureGetByIndex(_hModule, iIndex, phfeatur);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetInfo(IntPtr hModule, IntPtr hFeature, ref  _SV_FEATURE_INFO pInfoOut)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureGetInfo(_hModule, _hFeature, ref pInfoOut);
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetValueBool(IntPtr hModule, IntPtr hFeature, ref bool pBoolValue, bool verify = false, bool ignoreCache = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();

            fixed (bool* p = &pBoolValue)
            {
                return NativeMethods.SVFeatureGetValueBool(_hModule, _hFeature, p, verify, ignoreCache);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetValueInt64(IntPtr hModule, IntPtr hFeature, ref Int64 pInt64Value, bool verify = false, bool ignoreCache = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            fixed (Int64* p = &pInt64Value)
            {
                return NativeMethods.SVFeatureGetValueInt64(_hModule, _hFeature, p, verify, ignoreCache);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetValueFloat(IntPtr hModule, IntPtr hFeature, ref double pFloatValue, bool verify = false, bool ignoreCache = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            fixed (double* p = &pFloatValue)
            {
                return NativeMethods.SVFeatureGetValueFloat(_hModule, _hFeature, p, verify, ignoreCache);
            }
        }



        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetValueString(IntPtr hModule, IntPtr hFeature, ref string strValue, uint bufferSize, bool verify = false, bool ignoreCache = false)
        {

            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            byte[] st = new byte[DefineConstants.SV_STRING_SIZE];
            fixed (byte* p = st)
            {
                sbyte* sp = (sbyte*)p;
                ret = NativeMethods.SVFeatureGetValueString(_hModule, _hFeature, sp, bufferSize, verify, ignoreCache);
                strValue = new string(sp);
            }
            return ret;
        }



        public unsafe
        SVSCamApiReturn
        SVS_FeatureGetValueInt64Enum(IntPtr hModule, IntPtr hFeature, ref  Int64 pInt64Value, bool verify = false, bool ignoreCache = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            fixed (Int64* p = &pInt64Value)
            {
                return NativeMethods.SVFeatureGetValueInt64Enum(_hModule, _hFeature, p, verify, ignoreCache);
            }
        }


        public unsafe
        SVSCamApiReturn
         SVS_FeatureSetValueBool(IntPtr hModule, IntPtr hFeature, bool boolValue, bool verify = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureSetValueBool(_hModule, _hFeature, boolValue, verify);
        }


        public unsafe
        SVSCamApiReturn
         SVS_FeatureSetValueInt64(IntPtr hModule, IntPtr hFeature, Int64 int64Value, bool verify = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureSetValueInt64(_hModule, _hFeature, int64Value, verify);
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureSetValueFloat(IntPtr hModule, IntPtr hFeature, double floatValue, bool verify = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureSetValueFloat(_hModule, _hFeature, floatValue, verify);
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureSetValueString(IntPtr hModule, IntPtr hFeature, string strValue, bool verify = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            byte[] toBytes = Encoding.ASCII.GetBytes(strValue);
            fixed (byte* str_value = toBytes)
            {
                return NativeMethods.SVFeatureSetValueString(_hModule, _hFeature, (sbyte*)str_value, verify);
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureSetValueInt64Enum(IntPtr hModule, IntPtr hFeature, Int64 int64Value, bool verify = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureSetValueInt64Enum(_hModule, _hFeature, int64Value, verify);
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureCommandExecute(IntPtr hModule, IntPtr hFeature, uint Timeout, bool bWait = true)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureCommandExecute(_hModule, _hFeature, Timeout, false);
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureEnumSubFeatures(IntPtr hModule, IntPtr hFeature, int iIndex, ref string subFeatureName, uint bufferSize, ref int pValue)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            byte[] bytes = new byte[DefineConstants.SV_STRING_SIZE];
            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            fixed (byte* pFname = bytes)
            {
                fixed (int* pvalue = &pValue)
                {
                    sbyte* sp = (sbyte*)pFname;
                    ret = NativeMethods.SVFeatureEnumSubFeatures(_hModule, _hFeature, iIndex, sp, bufferSize, pvalue);
                    subFeatureName = new string(sp);
                    return ret;
                }
            }
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureRegisterInvalidateCB(IntPtr hModule, IntPtr hFeature, Object objCb, SV_CB_FEATURE_INVALIDATED_PFN2 pfnFeatureInvalidateCb2)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureRegisterInvalidateCB2(_hModule, _hFeature, objCb, pfnFeatureInvalidateCb2);
        }


        public unsafe
        SVSCamApiReturn
        SVS_FeatureUnRegisterInvalidateCB(IntPtr hModule, IntPtr hFeature)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            return NativeMethods.SVFeatureUnRegisterInvalidateCB(_hModule, _hFeature);
        }


        public unsafe
        SVSCamApiReturn
        SVS_UtilBuffer12BitTo8Bit(_SV_BUFFER_INFO srcInfo, ref byte pDest, int pDestLength)
        {
            fixed (byte* pdest = &pDest)
            {
                return NativeMethods.SVUtilBuffer12BitTo8Bit(srcInfo, pdest, pDestLength);
            }
        }


        public unsafe
       SVSCamApiReturn
       SVS_UtilBuffer12BitTo16Bit(_SV_BUFFER_INFO srcInfo, ref byte pDest, int pDestLength)
        {
            fixed (byte* pdest = &pDest)
            {
                return NativeMethods.SVUtilBuffer12BitTo16Bit(srcInfo, pdest, pDestLength);
            }
        }



        public unsafe
        SVSCamApiReturn
        SVS_UtilBuffer16BitTo8Bit(_SV_BUFFER_INFO srcInfo, ref sbyte pDest, int pDestLength)
        {
            fixed (sbyte* pdest = &pDest)
            {
                return NativeMethods.SVUtilBuffer16BitTo8Bit(srcInfo, pdest, pDestLength);
            }
        }



        public unsafe
        SVSCamApiReturn
        SVS_UtilBufferBayerToRGB(_SV_BUFFER_INFO srcInfo, ref byte pDest, int pDestLength)
        {

            fixed (byte* pdest = &pDest)
            {
                return NativeMethods.SVUtilBufferBayerToRGB(srcInfo, pdest, pDestLength);
            }
        }

        public unsafe
        SVSCamApiReturn
        SVS_LogRegister(string moduleName, uint debugLevel)
        {
		
		
			byte[] bytevalue = Encoding.ASCII.GetBytes(moduleName);

            fixed (byte* _moduleName   = bytevalue)
			{
            return NativeMethods.SVLogRegister(_moduleName, debugLevel);
			}
        }

        public unsafe
        SVSCamApiReturn
        SVS_LogEnableWindbg(bool bEnable)
        {
            return NativeMethods.SVLogEnableWindbg(bEnable);
        }


        public unsafe
        SVSCamApiReturn
        SVS_LogEnableFileLogging(bool bEnable, string logFileName)
        {
		 	byte[] bytevalue = Encoding.ASCII.GetBytes(logFileName);

            fixed (byte* _logFileName   = bytevalue)
			{
             return NativeMethods.SVLogEnableFileLogging(bEnable, _logFileName);
			}
		
    
        }


        public unsafe
        SVSCamApiReturn
        SVS_LogSetGlobalDebugLevel(uint debugLevel)
        {
            return NativeMethods.SVLogSetGlobalDebugLevel(debugLevel);
        }
		
		public unsafe
        SVSCamApiReturn
		SVS_UtilBufferBayerToRGB32(_SV_BUFFER_INFO srcInfo, ref byte pDest, int pDestLength)
        {

            fixed (byte* pdest = &pDest)
            {
                return NativeMethods.SVUtilBufferBayerToRGB32(srcInfo, pdest, pDestLength);
            }
        }
		
		public unsafe
        SVSCamApiReturn
		SVS_LibIsVersionCompliant(SV_LIB_VERSION expectedVersion, ref SV_LIB_VERSION  pCurrentVersion)
        {
            return NativeMethods.SVLibIsVersionCompliant(expectedVersion, ref pCurrentVersion);   
        }
		
		
		
		   public unsafe
        SVSCamApiReturn
        SVS_FeatureGetValueEnum(IntPtr hModule, IntPtr hFeature, ref string strValue, uint bufferSize, bool verify = false, bool ignoreCache = false)
        {

            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            byte[] st = new byte[DefineConstants.SV_STRING_SIZE];
            fixed (byte* p = st)
            {
                sbyte* sp = (sbyte*)p;
                ret = NativeMethods.SVFeatureGetValueEnum(_hModule, _hFeature, sp, bufferSize, verify, ignoreCache);
                strValue = new string(sp);
            }
            return ret;
        
		
		}
		
		
		public unsafe
        SVSCamApiReturn
        SVS_FeatureSetValueEnum(IntPtr hModule, IntPtr hFeature, string strValue, bool verify = false)
        {
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            byte[] toBytes = Encoding.ASCII.GetBytes(strValue);
            fixed (byte* str_value = toBytes)
            {
                return NativeMethods.SVFeatureSetValueEnum(_hModule, _hFeature, (sbyte*)str_value, verify);
            }
        }



    public unsafe
    SVSCamApiReturn
    SVS_DeviceSaveSettingsToString(IntPtr hDevice, ref string buffer, ref  uint pBufferSize)
        {
            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            void* _hDevice = hDevice.ToPointer();
          
            sbyte[] str =null;
            {
                fixed (uint* psize = &pBufferSize)
                {
                        ret = NativeMethods.SVDeviceSaveSettingsToString(_hDevice, null, psize);

                        if (ret == SVSCamApiReturn.SV_ERROR_SUCCESS)
                        {
                            str = new sbyte[*psize];
                            fixed (sbyte* pbuffer = str)
                            {
                                ret = NativeMethods.SVDeviceSaveSettingsToString(_hDevice, pbuffer, psize);

                                if (ret == SVSCamApiReturn.SV_ERROR_SUCCESS)
                                    buffer = new string(pbuffer);
                            }
                        }
                    }
                }
            
            return ret;
        }



		public unsafe
        SVSCamApiReturn
        SVS_DeviceLoadSettingsFromString(IntPtr hDevice, string strValue)
        {
            void* _hDevice = hDevice.ToPointer();
            byte[] toBytes = Encoding.ASCII.GetBytes(strValue);
            fixed (byte* str_value = toBytes)
            {
                return NativeMethods.SVDeviceLoadSettingsFromString(_hDevice, (sbyte*)str_value);
            }
        }
		
			public unsafe
        SVSCamApiReturn
        SVS_FeatureListRefresh(IntPtr hModule)
        {
            void* _hModule = hModule.ToPointer();
			return NativeMethods.SVFeatureListRefresh(_hModule);
            
        }
		
		
		 public unsafe
        SVSCamApiReturn
        SVS_UtilSaveImageToPNGFile(_SV_BUFFER_INFO srcInfo, string fileName)
        {
			byte[] toBytes = Encoding.ASCII.GetBytes(fileName);
			 fixed (byte* _fileName = toBytes)
            {
				return NativeMethods.SVUtilSaveImageToPNGFile(srcInfo, _fileName);
			}
        }
		
		



		
		
		public unsafe
        SVSCamApiReturn
        SVS_FeatureGetDescription(IntPtr hModule, IntPtr hFeature, ref string strValue, uint bufferSize)
        {

            SVSCamApiReturn ret = SVSCamApiReturn.SV_ERROR_SUCCESS;
            void* _hModule = hModule.ToPointer();
            void* _hFeature = hFeature.ToPointer();
            byte[] st = new byte[DefineConstants.SV_STRING_SIZE];
            fixed (byte* p = st)
            {
                sbyte* sp = (sbyte*)p;
                ret = NativeMethods.SVFeatureGetDescription(_hModule, _hFeature, sp, bufferSize);
                strValue = new string(sp);
            }
            return ret;
        
		
		}
		
		


    }// end class
}// end namespace