using System;
using System.Runtime.InteropServices;
using Photometrics.Pvcam;

namespace Photometrics
{
    namespace Pvcam
    {
        public static partial class PvTypes
        {
            #region #defines
            /* Constant & Type Definitions */

            /* Name/ID sizes */
            public const Int32 CAM_NAME_LEN = 32;          /* Maximum length of a cam name (includes null term) */
            public const Int32 PARAM_NAME_LEN = 32;        /* Maximum length of a pp param */
            public const Int32 ERROR_MSG_LEN = 255;        /* Maximum length of an error message */
            public const Int32 CCD_NAME_LEN = 17;          /* Maximum length of a sensor chip name */
            public const Int32 MAX_ALPHA_SER_NUM_LEN = 32; /* Maximum length of a camera serial number string */
            public const Int32 MAX_PP_NAME_LEN = 32;       /* Maximum length of a post-processing parameter/feature name */
            public const Int32 MAX_SYSTEM_NAME_LEN = 32;   /* Maximum length of a system name */
            public const Int32 MAX_VENDOR_NAME_LEN = 32;   /* Maximum length of a vendor name */
            public const Int32 MAX_PRODUCT_NAME_LEN = 32;  /* Maximum length of a product name */
            public const Int32 MAX_CAM_PART_NUM_LEN = 32;  /* Maximum length of a Part number */
            public const Int32 MAX_GAIN_NAME_LEN = 32;     /* Maximum length of a gain name */
            public const Int32 MAX_CAM = 16;               /* Maximum number of cameras on this system */

            /* Class 2: Data types */

            /* Data type used by pl_get_param with attribute type (ATTR_TYPE). */
            private const Int32 TYPE_CHAR_PTR = 13;
            private const Int32 TYPE_INT8 = 12;
            private const Int32 TYPE_UNS8 = 5;
            private const Int32 TYPE_INT16 = 1;
            private const Int32 TYPE_UNS16 = 6;
            private const Int32 TYPE_INT32 = 2;
            private const Int32 TYPE_UNS32 = 7;
            private const Int32 TYPE_UNS64 = 8;
            private const Int32 TYPE_FLT64 = 4;
            private const Int32 TYPE_ENUM = 9;
            private const Int32 TYPE_BOOLEAN = 11;
            private const Int32 TYPE_VOID_PTR = 14;
            private const Int32 TYPE_VOID_PTR_PTR = 15;
            private const Int32 TYPE_INT64 = 16;
            private const Int32 TYPE_SMART_STREAM_TYPE = 17;
            private const Int32 TYPE_SMART_STREAM_TYPE_PTR = 18;
            private const Int32 TYPE_FLT32 = 19;

            /* Defines for classes */
            private const Int32 CLASS0 = 0;          /* Camera Communications */
            private const Int32 CLASS1 = 1;          /* Error Reporting */
            private const Int32 CLASS2 = 2;          /* Configuration/Setup */
            private const Int32 CLASS3 = 3;          /* Data Acuisition */

            /* Parameter IDs */
            /* Format: TTCCxxxx, where TT = Data type, CC = Class, xxxx = ID number */

            public const Int32 PARAM_DD_INFO_LENGTH = ((CLASS0 << 16) + (TYPE_INT16 << 24) + 1);
            public const Int32 PARAM_DD_VERSION = ((CLASS0 << 16) + (TYPE_UNS16 << 24) + 2);
            public const Int32 PARAM_DD_RETRIES = ((CLASS0 << 16) + (TYPE_UNS16 << 24) + 3);
            public const Int32 PARAM_DD_TIMEOUT = ((CLASS0 << 16) + (TYPE_UNS16 << 24) + 4);
            public const Int32 PARAM_DD_INFO = ((CLASS0 << 16) + (TYPE_CHAR_PTR << 24) + 5);

            /* CONFIGURATION AND SETUP PARAMETERS */

            /* Sensor skip parameters */

            /* ADC offset setting. */
            public const Int32 PARAM_ADC_OFFSET = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 195);
            /* Sensor chip name.    */
            public const Int32 PARAM_CHIP_NAME = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 129);
            /* Camera system name. */
            public const Int32 PARAM_SYSTEM_NAME = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 130);
            /** Camera vendor name. */
            public const Int32 PARAM_VENDOR_NAME = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 131);
            /** Camera product name. */
            public const Int32 PARAM_PRODUCT_NAME = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 132);
            /* Camera part number. */
            public const Int32 PARAM_CAMERA_PART_NUMBER = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 133);

            public const Int32 PARAM_COOLING_MODE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 214);
            public const Int32 PARAM_PREAMP_DELAY = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 502);
            public const Int32 PARAM_COLOR_MODE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 504);
            public const Int32 PARAM_MPP_CAPABLE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 224);
            public const Int32 PARAM_PREAMP_OFF_CONTROL = ((CLASS2 << 16) + (TYPE_UNS32 << 24) + 507);

            /* Sensor dimensions and physical characteristics */

            /* Pre and post dummies of sensor. */
            public const Int32 PARAM_PREMASK = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 53);
            public const Int32 PARAM_PRESCAN = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 55);
            public const Int32 PARAM_POSTMASK = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 54);
            public const Int32 PARAM_POSTSCAN = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 56);
            public const Int32 PARAM_PIX_PAR_DIST = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 500);
            public const Int32 PARAM_PIX_PAR_SIZE = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 63);
            public const Int32 PARAM_PIX_SER_DIST = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 501);
            public const Int32 PARAM_PIX_SER_SIZE = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 62);
            public const Int32 PARAM_SUMMING_WELL = ((CLASS2 << 16) + (TYPE_BOOLEAN << 24) + 505);
            public const Int32 PARAM_FWELL_CAPACITY = ((CLASS2 << 16) + (TYPE_UNS32 << 24) + 506);
            /* Y dimension of active area of sensor chip. */
            public const Int32 PARAM_PAR_SIZE = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 57);
            /* X dimension of active area of sensor chip. */
            public const Int32 PARAM_SER_SIZE = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 58);
            public const Int32 PARAM_ACCUM_CAPABLE = ((CLASS2 << 16) + (TYPE_BOOLEAN << 24) + 538);
            public const Int32 PARAM_FLASH_DWNLD_CAPABLE = ((CLASS2 << 16) + (TYPE_BOOLEAN << 24) + 539);

            /* General parameters */

            /* Readout time of current ROI in microseconds, valid after pl_exp_setup, if available. */
            public const Int32 PARAM_READOUT_TIME = ((CLASS2 << 16) + (TYPE_FLT64 << 24) + 179); /* The real type is TYPE_UNS32 */
            /* Clearing time, in nano-seconds, valid after pl_exp_setup, if available. */
            public const Int32 PARAM_CLEARING_TIME = ((CLASS2 << 16) + (TYPE_INT64 << 24) + 180);
            /* Post trigger delay, in nano-seconds, valid after pl_exp_setup, if available. */
            public const Int32 PARAM_POST_TRIGGER_DELAY = ((CLASS2 << 16) + (TYPE_INT64 << 24) + 181);
            /* Pre trigger delay, in nano-seconds, valid after pl_exp_setup, if available. */
            public const Int32 PARAM_PRE_TRIGGER_DELAY = ((CLASS2 << 16) + (TYPE_INT64 << 24) + 182);

            /* CAMERA PARAMETERS */
            public const Int32 PARAM_CLEAR_CYCLES = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 97);
            public const Int32 PARAM_CLEAR_MODE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 523);
            public const Int32 PARAM_FRAME_CAPABLE = ((CLASS2 << 16) + (TYPE_BOOLEAN << 24) + 509);
            public const Int32 PARAM_PMODE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 524);

            /* These are the temperature parameters for the detector. */
            public const Int32 PARAM_TEMP = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 525); 
            public const Int32 PARAM_TEMP_SETPOINT = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 526);

            /* These are the parameters used for firmware version retrieval. */
            public const Int32 PARAM_CAM_FW_VERSION = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 532);
            public const Int32 PARAM_HEAD_SER_NUM_ALPHA = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 533);
            public const Int32 PARAM_PCI_FW_VERSION = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 534);

            /* This is to control the fan speed available on certain cameras*/
            public const Int32 PARAM_FAN_SPEED_SETPOINT = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 710);

            /* Exsposure mode, timed strobed etc, etc */
            public const Int32 PARAM_EXPOSURE_MODE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 535);
            public const Int32 PARAM_EXPOSE_OUT_MODE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 560);

            /* SPEED TABLE PARAMETERS */
            public const Int32 PARAM_BIT_DEPTH = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 511);
            public const Int32 PARAM_GAIN_INDEX = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 512);
            public const Int32 PARAM_SPDTAB_INDEX = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 513);
            public const Int32 PARAM_GAIN_NAME = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 514);
            public const Int32 PARAM_READOUT_PORT = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 247);
            public const Int32 PARAM_PIX_TIME = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 516);

            /* SHUTTER PARAMETERS */
            public const Int32 PARAM_SHTR_CLOSE_DELAY = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 519);
            public const Int32 PARAM_SHTR_OPEN_DELAY = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 520);
            public const Int32 PARAM_SHTR_OPEN_MODE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 521);
            public const Int32 PARAM_SHTR_STATUS = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 522);

            /* I/O PARAMETERS */
            public const Int32 PARAM_IO_ADDR = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 527);
            public const Int32 PARAM_IO_TYPE = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 528);
            public const Int32 PARAM_IO_DIRECTION = ((CLASS2 << 16) + (TYPE_ENUM << 24) + 529);
            public const Int32 PARAM_IO_STATE = ((CLASS2 << 16) + (TYPE_FLT64 << 24) + 530);
            public const Int32 PARAM_IO_BITDEPTH = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 531);

            /* GAIN MULTIPLIER PARAMETERS */
            public const Int32 PARAM_GAIN_MULT_FACTOR = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 537);
            public const Int32 PARAM_GAIN_MULT_ENABLE = ((CLASS2 << 16) + (TYPE_BOOLEAN << 24) + 541);

            /*  POST PROCESSING PARAMETERS */
            public const Int32 PARAM_PP_FEAT_NAME = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 542);
            public const Int32 PARAM_PP_INDEX = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 543);
            public const Int32 PARAM_ACTUAL_GAIN = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 544);
            public const Int32 PARAM_PP_PARAM_INDEX = ((CLASS2 << 16) + (TYPE_INT16 << 24) + 545);
            public const Int32 PARAM_PP_PARAM_NAME = ((CLASS2 << 16) + (TYPE_CHAR_PTR << 24) + 546);
            public const Int32 PARAM_PP_PARAM = ((CLASS2 << 16) + (TYPE_UNS32 << 24) + 547);
            public const Int32 PARAM_READ_NOISE = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 548);
            public const Int32 PARAM_PP_FEAT_ID = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 549);
            public const Int32 PARAM_PP_PARAM_ID = ((CLASS2 << 16) + (TYPE_UNS16 << 24) + 550);

            /*  S.M.A.R.T. STREAMING PARAMETERS */
            public const Int32 PARAM_SMART_STREAM_MODE_ENABLED = ((CLASS2<<16) + (TYPE_BOOLEAN << 24) + 700);
            public const Int32 PARAM_SMART_STREAM_MODE = ((CLASS2<<16) + (TYPE_UNS16 << 24) + 701);
            public const Int32 PARAM_SMART_STREAM_EXP_PARAMS = ((CLASS2<<16) + (TYPE_VOID_PTR << 24) + 702);
            public const Int32 PARAM_SMART_STREAM_DLY_PARAMS = ((CLASS2<<16) + (TYPE_VOID_PTR << 24) + 703);

            /* DATA AQUISITION PARAMETERS */

            /* ACQUISITION PARAMETERS */
            public const Int32 PARAM_EXP_TIME = ((CLASS3 << 16) + (TYPE_UNS16 << 24) + 1);
            public const Int32 PARAM_EXP_RES = ((CLASS3 << 16) + (TYPE_ENUM << 24) + 2);
            public const Int32 PARAM_EXP_RES_INDEX = ((CLASS3 << 16) + (TYPE_UNS16 << 24) + 4);
            public const Int32 PARAM_EXPOSURE_TIME = ((CLASS3 << 16) + (TYPE_UNS64 << 24) + 8);

            /* PARAMETERS FOR  BEGIN and END of FRAME Interrupts */
            public const Int32 PARAM_BOF_EOF_ENABLE = ((CLASS3 << 16) + (TYPE_ENUM << 24) + 5);
            public const Int32 PARAM_BOF_EOF_COUNT = ((CLASS3 << 16) + (TYPE_UNS32 << 24) + 6);
            public const Int32 PARAM_BOF_EOF_CLR = ((CLASS3 << 16) + (TYPE_BOOLEAN << 24) + 7);

            /* Test to see if hardware/software can perform circular buffer */
            public const Int32 PARAM_CIRC_BUFFER = ((CLASS3 << 16) + (TYPE_BOOLEAN << 24) + 299);
            public const Int32 PARAM_FRAME_BUFFER_SIZE = ((CLASS3 << 16) + (TYPE_UNS64 << 24) + 300);

            /* Supported binning reported by camera */
            public const Int32 PARAM_BINNING_SER = ((CLASS3 << 16) + (TYPE_ENUM << 24) + 165);
            public const Int32 PARAM_BINNING_PAR = ((CLASS3 << 16) + (TYPE_ENUM << 24) + 166);

            /* Parameters related to multiple ROIs and Centroids */
            public const Int32 PARAM_METADATA_ENABLED = ((CLASS3 << 16) + (TYPE_BOOLEAN << 24) + 168);
            public const Int32 PARAM_ROI_COUNT = ((CLASS3 << 16) + (TYPE_UNS16 << 24) + 169);
            public const Int32 PARAM_CENTROIDS_ENABLED = ((CLASS3 << 16) + (TYPE_BOOLEAN << 24) + 170);
            public const Int32 PARAM_CENTROIDS_RADIUS = ((CLASS3 << 16) + (TYPE_UNS16 << 24) + 171);
            public const Int32 PARAM_CENTROIDS_COUNT = ((CLASS3 << 16) + (TYPE_UNS16 << 24) + 172);

            /* Parameters related to triggering table */
            public const Int32 PARAM_TRIGTAB_SIGNAL = ((CLASS3 << 16) + (TYPE_ENUM << 24) + 180);
            public const Int32 PARAM_LAST_MUXED_SIGNAL = ((CLASS3 << 16) + (TYPE_UNS8 << 24) + 181);

            /* Frame Meta data defines */

            /* The signature is located in the first 4 bytes of the frame header. The signature
            is checked before any metadata-related operations are executed on the buffer. */
            public const Int32 PL_MD_FRAME_SIGNATURE = 5328208;

            /* Maximum number of extended metadata tags supported. */
            public const Int32 PL_MD_EXT_TAGS_MAX_SUPPORTED = 255;

            #endregion

            #region DataTypes

            /*
            The modes under which the camera can be open.
            Used with the function pl_cam_open().
            Treated as int16 type.
            */
            public enum CameraOpenMode
            {
                OPEN_EXCLUSIVE
            }

            /*
            Used with the #PARAM_COOLING_MODE parameter ID.
            Treated as int32 type.
            */
            public enum CoolingTypes
            {
                NORMAL_COOL,
                CRYO_COOL
            }

            /*
            Used with the #PARAM_MPP_CAPABLE parameter ID.
            Treated as int32 type.
            */
            public enum MPPCapabilities
            {
                MPP_UNKNOWN,
                MPP_ALWAYS_OFF,
                MPP_ALWAYS_ON,
                MPP_SELECTABLE
            }

            /*
            Used with the #PARAM_SHTR_STATUS parameter ID.
            Treated as int32 type.
            */
            public enum ShutterFlags
            {
                SHTR_FAULT,
                SHTR_OPENING,
                SHTR_OPEN,
                SHTR_CLOSING,
                SHTR_CLOSED,
                SHTR_UNKNOWN
            }

            /*
            Used with the #PARAM_PMODE parameter ID.
            Treated as int32 type.
            */
            public enum PModes
            {
                PMODE_NORMAL,
                PMODE_FT,
                PMODE_MPP,
                PMODE_FT_MPP,
                PMODE_ALT_NORMAL,
                PMODE_ALT_FT,
                PMODE_ALT_MPP,
                PMODE_ALT_FT_MPP,
                PMODE_INTERLINE
            }

            /*
            Used with the #PARAM_COLOR_MODE parameter ID.
            Treated as int32 type (but should not exceed a value of 255 due to md_frame_header.colorMask)
            */
            public enum ColorSupport
            {
                COLOR_NONE = 0, /* No color mask */
                COLOR_RESERVED = 1, /* Reserved, do not use */
                COLOR_RGGB = 2,
                COLOR_GRBG,
                COLOR_GBRG,
                COLOR_BGGR
            }

            /*
            Used with the function pl_get_param().
            Treated as int16 type.
            */
            public enum AttributeIDs
            {
                ATTR_CURRENT,
                ATTR_COUNT,
                ATTR_TYPE,
                ATTR_MIN,
                ATTR_MAX,
                ATTR_DEFAULT,
                ATTR_INCREMENT,
                ATTR_ACCESS,
                ATTR_AVAIL
            }

            /*
            Used with the function pl_get_param() and #ATTR_ACCESS.
            Treated as uns16 type.
            */
            public enum AccessTypes
            {
                ACC_ERROR,
                ACC_READ_ONLY,
                ACC_READ_WRITE,
                ACC_EXIST_CHECK_ONLY,
                ACC_WRITE_ONLY
            }

            /*
            Used with the #PARAM_IO_TYPE parameter ID.
            Treated as int32 type.
            */
            public enum IOTypes
            {
                IO_TYPE_TTL,
                IO_TYPE_DAC
            }

            /*
            Used with the #PARAM_IO_DIRECTION parameter ID.
            Treated as int32 type.
            */
            public enum IODirections
            {
                IO_DIR_INPUT,
                IO_DIR_OUTPUT,
                IO_DIR_INPUT_OUTPUT
            }

            /*
            Used with the #PARAM_READOUT_PORT parameter ID.
            Treated as int32 type.
            */
            public enum ReadouPort
            {
                READOUT_PORT_0 = 0,
                READOUT_PORT_1
            }

            /*
            Used with the #PARAM_CLEAR_MODE parameter ID.
            Treated as int32 type.
            */
            public enum ClearingModes
            {
                CLEAR_NEVER,
                CLEAR_PRE_EXPOSURE,
                CLEAR_PRE_SEQUENCE,
                CLEAR_POST_SEQUENCE,
                CLEAR_PRE_POST_SEQUENCE,
                CLEAR_PRE_EXPOSURE_POST_SEQ,
                MAX_CLEAR_MODE
            }

            /*
            Used with the #PARAM_SHTR_OPEN_MODE parameter ID.
            Treated as int32 type.
            */
            public enum ShutterModes
            {
                OPEN_NEVER,
                OPEN_PRE_EXPOSURE,
                OPEN_PRE_SEQUENCE,
                OPEN_PRE_TRIGGER,
                OPEN_NO_CHANGE
            }

            /*
            Used with the #PARAM_EXPOSURE_MODE parameter ID.
            Treated as int32 type.
            Used with the functions pl_exp_setup_cont() and pl_exp_setup_seq().
            Treated as int16 type.
            */
            public enum ExposureModes
            {
                TIMED_MODE,
                STROBED_MODE,
                BULB_MODE,
                TRIGGER_FIRST_MODE,
                FLASH_MODE,
                VARIABLE_TIMED_MODE,
                INT_STROBE_MODE,
                MAX_EXPOSE_MODE = 7,

                /*
                Extended EXPOSURE modes used with PARAM_EXPOSURE_MODE when
                camera dynamically reports it's capabilities.
                The "7" in each of these calculations comes from previous
                definition of MAX_EXPOSE_MODE when this file was defined.
                */
                EXT_TRIG_INTERNAL = (7 + 0) << 8,
                EXT_TRIG_TRIG_FIRST = (7 + 1) << 8,
                EXT_TRIG_EDGE_RISING = (7 + 2) << 8
            }

            /*
            Used with the PARAM_EXPOSE_OUT_MODE parameter ID.
            Build the values for the expose out modes that are "ORed" with the trigger
            modes(ExposureModes) when setting up the script.
            Treated as int32 type.
            */
            public enum ExposeOutModes
            {
                /*
                Expose out high when first row is exposed (from first row begin to first row end)
                */
                EXPOSE_OUT_FIRST_ROW = 0,
                /*
                Expose out high when all rows are exposed at once (from last row begin to first row end).
                The duration of the signal equals to exposure value entered which means the actual exposure
                time is longer - use this mode with triggered light source only.
                */
                EXPOSE_OUT_ALL_ROWS,
                /*
                Expose out high when any row is exposed (from first row begin to last row end)
                */
                EXPOSE_OUT_ANY_ROW,
                /**
                Like FIRST_ROW but the expose out signal is high when all rows are being exposed at once.
                If the exposure time entered is shorter than readout time the expose out signal will
                not become high at all.
                */
                EXPOSE_OUT_ROLLING_SHUTTER,
                MAX_EXPOSE_OUT_MODE
            }

            /*
            Used with the #PARAM_FAN_SPEED_SETPOINT parameter ID.
            Treated as int32 type.
            */
            public enum FanSpeeds
            {
                FAN_SPEED_HIGH, /* Maximum speed, the default state. */
                FAN_SPEED_MEDIUM,
                FAN_SPEED_LOW,
                FAN_SPEED_OFF /* Fan is turned off. */
            }

            /*
            Used with the #PARAM_TRIGTAB_SIGNAL parameter ID.
            Treated as int32 type.
            */
            public enum TriggerTabSignals
            {
                PL_TRIGTAB_SIGNAL_EXPOSE_OUT
            }

            /*
            Used with the #PARAM_CAM_INTERFACE_TYPE parameter ID.
            32-bit enum
            Upper 24 bits are interface classes, flags, 1bit = one class, 24 possible classes.
            Lower  8 bits are interface revisions, there are 254 possible revisions for each interface class
            Usage:
                if (attrCurrent & PL_CAM_IFC_TYPE_USB)
                    // The camera is running on USB, any USB
                if (attrCurrent & PL_CAM_IFC_TYPE_USB && type >= PL_CAM_IFC_TYPE_USB_3_0)
                    // The camera is running on USB, the camera is running on at least USB 3.0
                if (attrCurrent == PL_CAM_IFC_TYPE_USB_3_1)
                    // The camera is running exactly on USB 3.1
            */
            public enum PL_CAM_INTERFACE_TYPES
            {
                PL_CAM_IFC_TYPE_UNKNOWN     = 0,

                PL_CAM_IFC_TYPE_1394        = 0x100,    /* A generic 1394 in case we cannot identify the sub type */
                PL_CAM_IFC_TYPE_1394_A,                 /* FireWire 400 */
                PL_CAM_IFC_TYPE_1394_B,                 /* FireWire 800 */

                PL_CAM_IFC_TYPE_USB         = 0x200,    /* A generic USB in case we cannot identify the sub type */
                PL_CAM_IFC_TYPE_USB_1_1,                /* SlowSpeed */
                PL_CAM_IFC_TYPE_USB_2_0,                /* HighSpeed */
                PL_CAM_IFC_TYPE_USB_3_0,                /* SuperSpeed */
                PL_CAM_IFC_TYPE_USB_3_1,                /* SuperSpeedPlus */

                PL_CAM_IFC_TYPE_PCI         = 0x400,    /* A generic PCI interface */
                PL_CAM_IFC_TYPE_PCI_LVDS,               /* LVDS PCI interface */

                PL_CAM_IFC_TYPE_PCIE        = 0x800,    /* A generic PCIe interface */
                PL_CAM_IFC_TYPE_PCIE_LVDS,              /* LVDS PCIe interface */
                PL_CAM_IFC_TYPE_PCIE_X1,                /* Single channel PCIe interface */
                PL_CAM_IFC_TYPE_PCIE_X4,                /* 4 channel PCIe interface */

                PL_CAM_IFC_TYPE_VIRTUAL     = 0x1000,   /* Base for all Virtual camera interfaces */

                PL_CAM_IFC_TYPE_ETHERNET    = 0x2000    /* Base for all Ethernet based cameras */

                /*
                PL_CAM_IFC_TYPE_RESERVED = 0x4000,
                PL_CAM_IFC_TYPE_RESERVED = 0x8000,
                PL_CAM_IFC_TYPE_RESERVED = 0x10000,
                PL_CAM_IFC_TYPE_RESERVED = 0x20000
                */
            }

            /*
            Used with the #PARAM_CAM_INTERFACE_MODE parameter ID.
            */
            public enum PL_CAM_INTERFACE_MODES
            {
                PL_CAM_IFC_MODE_UNSUPPORTED = 0, /* Interface is not supported */
                PL_CAM_IFC_MODE_CONTROL_ONLY,    /* Control commands */
                PL_CAM_IFC_MODE_IMAGING          /* Imaging */
            }

            /*
            Used with the #PARAM_PP_FEAT_ID parameter ID.
            Treated as uns16 type.
            */
            public enum PP_FEATURE_IDS
            {
                PP_FEATURE_RING_FUNCTION,
                PP_FEATURE_BIAS,
                PP_FEATURE_BERT,
                PP_FEATURE_QUANT_VIEW,
                PP_FEATURE_BLACK_LOCK,
                PP_FEATURE_TOP_LOCK,
                PP_FEATURE_VARI_BIT,
                PP_FEATURE_RESERVED, /* Should not be used at any time moving forward. */
                PP_FEATURE_DESPECKLE_BRIGHT_HIGH,
                PP_FEATURE_DESPECKLE_DARK_LOW,
                PP_FEATURE_DEFECTIVE_PIXEL_CORRECTION,
                PP_FEATURE_DYNAMIC_DARK_FRAME_CORRECTION,
                PP_FEATURE_HIGH_DYNAMIC_RANGE,
                PP_FEATURE_DESPECKLE_BRIGHT_LOW,
                PP_FEATURE_DENOISING,
                PP_FEATURE_DESPECKLE_DARK_HIGH,
                PP_FEATURE_ENHANCED_DYNAMIC_RANGE,
                PP_FEATURE_MAX
            }

            /*
            Used with the #PARAM_PP_PARAM_ID parameter ID.
            */
            public const Int32 PP_MAX_PARAMETERS_PER_FEATURE = 10;

            /*
            Used with the #PARAM_PP_PARAM_ID parameter ID.
            Treated as uns16 type.
            */
            public enum PP_PARAMETER_IDS
            {
                PP_PARAMETER_RF_FUNCTION = (PP_FEATURE_IDS.PP_FEATURE_RING_FUNCTION * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_BIAS_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_BIAS * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_BIAS_LEVEL,
                PP_FEATURE_BERT_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_BERT * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_BERT_THRESHOLD,
                PP_FEATURE_QUANT_VIEW_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_QUANT_VIEW * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_QUANT_VIEW_E,
                PP_FEATURE_BLACK_LOCK_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_BLACK_LOCK * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_BLACK_LOCK_BLACK_CLIP,
                PP_FEATURE_TOP_LOCK_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_TOP_LOCK * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_TOP_LOCK_WHITE_CLIP,
                PP_FEATURE_VARI_BIT_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_VARI_BIT * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_VARI_BIT_BIT_DEPTH,
                PP_FEATURE_DESPECKLE_BRIGHT_HIGH_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_DESPECKLE_BRIGHT_HIGH * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_DESPECKLE_BRIGHT_HIGH_THRESHOLD,
                PP_FEATURE_DESPECKLE_BRIGHT_HIGH_MIN_ADU_AFFECTED,
                PP_FEATURE_DESPECKLE_DARK_LOW_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_DESPECKLE_DARK_LOW * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_DESPECKLE_DARK_LOW_THRESHOLD,
                PP_FEATURE_DESPECKLE_DARK_LOW_MAX_ADU_AFFECTED,
                PP_FEATURE_DEFECTIVE_PIXEL_CORRECTION_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_DEFECTIVE_PIXEL_CORRECTION * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_DYNAMIC_DARK_FRAME_CORRECTION_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_DYNAMIC_DARK_FRAME_CORRECTION * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_HIGH_DYNAMIC_RANGE_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_HIGH_DYNAMIC_RANGE * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_DESPECKLE_BRIGHT_LOW_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_DESPECKLE_BRIGHT_LOW * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_DESPECKLE_BRIGHT_LOW_THRESHOLD,
                PP_FEATURE_DESPECKLE_BRIGHT_LOW_MAX_ADU_AFFECTED,
                PP_FEATURE_DENOISING_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_DENOISING * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_DENOISING_NO_OF_ITERATIONS,
                PP_FEATURE_DENOISING_GAIN,
                PP_FEATURE_DENOISING_OFFSET,
                PP_FEATURE_DENOISING_LAMBDA,
                PP_FEATURE_DESPECKLE_DARK_HIGH_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_DESPECKLE_DARK_HIGH * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_FEATURE_DESPECKLE_DARK_HIGH_THRESHOLD,
                PP_FEATURE_DESPECKLE_DARK_HIGH_MIN_ADU_AFFECTED,
                PP_FEATURE_ENHANCED_DYNAMIC_RANGE_ENABLED = (PP_FEATURE_IDS.PP_FEATURE_ENHANCED_DYNAMIC_RANGE * PP_MAX_PARAMETERS_PER_FEATURE),
                PP_PARAMETER_ID_MAX
            }

            /**
            Used with the #PARAM_SMART_STREAM_MODE parameter ID.
            Treated as uns16 type.
            */
            public enum SmtModes
            {
                SMTMODE_ARBITRARY_ALL = 0,
                SMTMODE_MAX
            }

            /*
            Used with the functions pl_exp_check_status(), and pl_exp_check_cont_status()
            and pl_exp_check_cont_status_ex().
            Treated as int16 type.

            if NEWDATARDY or NEWDATAFIXED     READOUT_COMPLETE
            else if RUNNING                   ACQUISITION_IN_PROGRESS
            else if INITIALIZED or DONEDCOK   READOUT_NOT_ACTIVE
            else                              READOUT_FAILED
            */
            public enum ReadoutStatuses
            {
                READOUT_NOT_ACTIVE,
                EXPOSURE_IN_PROGRESS,
                READOUT_IN_PROGRESS,
                READOUT_COMPLETE,                   /* Means frame available for a circular buffer acq */
                FRAME_AVAILABLE = READOUT_COMPLETE, /* New camera status indicating at least one frame is available */
                READOUT_FAILED,
                ACQUISITION_IN_PROGRESS,
                MAX_CAMERA_STATUS
            }

            /*
            Used with the function pl_exp_abort().
            Treated as int16 type.
            */
            public enum AbortExposureFlags
            {
                CCS_NO_CHANGE,
                CCS_HALT,
                CCS_HALT_CLOSE_SHTR,
                CCS_CLEAR,
                CCS_CLEAR_CLOSE_SHTR,
                CCS_OPEN_SHTR,
                CCS_CLEAR_OPEN_SHTR
            }

            /*
            Used with the #PARAM_BOF_EOF_ENABLE parameter ID.
            Treated as int32 type.
            */
            public enum EOFBOFConstants
            {
                NO_FRAME_IRQS,
                BEGIN_FRAME_IRQS,
                END_FRAME_IRQS,
                BEGIN_END_FRAME_IRQS
            }

            /*
            Used with the function pl_exp_setup_cont().
            Treated as int16 type.
            */
            public enum ContinuousModes : short
            {
                CIRC_NONE,
                CIRC_OVERWRITE,
                CIRC_NO_OVERWRITE
            }

            /*
            Used with the #PARAM_EXP_RES parameter ID.
            Treated as int32 type.
            */
            public enum FastExposureResolutions
            {
                EXP_RES_ONE_MILLISEC,
                EXP_RES_ONE_MICROSEC,
                EXP_RES_ONE_SEC
            }

            /*
            Used with the function pl_io_script_control().
            Treated as uns32 type.
            */
            public enum IOScriptLocations
            {
                SCR_PRE_OPEN_SHTR,
                SCR_POST_OPEN_SHTR,
                SCR_PRE_FLASH,
                SCR_POST_FLASH,
                SCR_PRE_INTEGRATE,
                SCR_POST_INTEGRATE,
                SCR_PRE_READOUT,
                SCR_POST_READOUT,
                SCR_PRE_CLOSE_SHTR,
                SCR_POST_CLOSE_SHTR
            }

            /*
            Used with the functions pl_cam_register_callback*() and pl_cam_deregister_callback().
            Used directly as an enum type without casting to any integral type.
            */
            public enum PL_CALLBACK_EVENT
            {
                PL_CALLBACK_BOF,
                PL_CALLBACK_EOF,
                PL_CALLBACK_CHECK_CAMS,
                PL_CALLBACK_CAM_REMOVED,
                PL_CALLBACK_CAM_RESUMED,
                PL_CALLBACK_MAX
            }

            /* Frame Meta data types */

            /*
            Used in #md_frame_header structure.
            Treated as uns8 type.
            */
            enum PL_MD_FRAME_FLAGS
            {
                PL_MD_FRAME_FLAG_ROI_TS_SUPPORTED = 0x01, /* Check this bit before using the timestampBOR and timestampEOR */
                PL_MD_FRAME_FLAG_UNUSED_2 = 0x02,
                PL_MD_FRAME_FLAG_UNUSED_3 = 0x04,
                PL_MD_FRAME_FLAG_UNUSED_4 = 0x10,
                PL_MD_FRAME_FLAG_UNUSED_5 = 0x20,
                PL_MD_FRAME_FLAG_UNUSED_6 = 0x40,
                PL_MD_FRAME_FLAG_UNUSED_7 = 0x80
            }

            /*
            Used in #md_frame_roi_header structure.
            Treated as uns8 type.
            */
            enum PL_MD_ROI_FLAGS
            {
                PL_MD_ROI_FLAG_INVALID  = 0x01, /* ROI is invalid (centroid unavailable). */
                PL_MD_ROI_FLAG_UNUSED_2 = 0x02,
                PL_MD_ROI_FLAG_UNUSED_3 = 0x04,
                PL_MD_ROI_FLAG_UNUSED_4 = 0x10,
                PL_MD_ROI_FLAG_UNUSED_5 = 0x20,
                PL_MD_ROI_FLAG_UNUSED_6 = 0x40,
                PL_MD_ROI_FLAG_UNUSED_7 = 0x80
            }

            /*
            Available extended metadata tags.
            Currently there are no extended metadata available.
            Used in #md_ext_item_info structure.
            Used directly as an enum type without casting to any integral type.
            */
            enum PL_MD_EXT_TAGS
            {
                PL_MD_EXT_TAG_MAX = 0
            }

            #endregion

            #region Structures

            /* Region Definition */
            [StructLayout(LayoutKind.Sequential, Pack = 2)]
            public struct RegionType
            {
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 s1;               /* First pixel in the serial register */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 s2;               /* Last pixel in the serial register */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 sbin;             /* Serial binning for this region */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 p1;               /* First row in the parallel register */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 p2;               /* Last row in the parallel register */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 pbin;             /* Parallel binning for this region */
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct PVCAM_FRAME_INFO_GUID
            {
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 f1;
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 f2;
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 f3;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
                public Byte[] f4;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct FRAME_INFO
            {
                PVCAM_FRAME_INFO_GUID FrameInfoGUID;
                [MarshalAs(UnmanagedType.I2)]
                public Int16 hCam;
                [MarshalAs(UnmanagedType.I4)]
                public Int32 FrameNr;
                [MarshalAs(UnmanagedType.I8)]
                public Int64 TimeStamp;
                [MarshalAs(UnmanagedType.I4)]
                public Int32 ReadoutTime;
                [MarshalAs(UnmanagedType.I8)]
                public Int64 TimeStampBOF;
            }

            /*
            Used with the #PARAM_SMART_STREAM_EXP_PARAMS and #PARAM_SMART_STREAM_DLY_PARAMS
            parameter IDs and pl_create_smart_stream_struct() and
            pl_release_smart_stream_struct() functions.
            */
            [StructLayout(LayoutKind.Sequential)]
            public struct smart_stream_type
            {
                [MarshalAs(UnmanagedType.U2)]
                public Int16 entries;
                public IntPtr parameters;
            }

            /* Frame Metadata structures */
            /* These structures are shared beween platforms, thus we must ensure that no
            compiler will apply different struct alignment. */

            /*
            This is a frame header that is located before each frame. The size of this
            structure must remain constant. The structure is generated by the camera
            and should be 16-byte aligned.
            */
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct MD_Frame_Header
            {
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 signature;/* See signature definition */
                [MarshalAs(UnmanagedType.U1)]
                public Byte version;    /* Must be 1 in first release */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 frameNr;  /* 1-based, reset with each aquisition */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 roiCount; /* Number of ROIs in frame, at least 1 */

                /* The final timestamp = timestampBOF * timestampResNs (in nano-seconds) */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 timeStampBOF;  /*Depends upon resolution */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 timeStampEOF;  /*Depends upon resolution */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 timeStampResNs;  /* Time stamp resolution (in ns) */

                /* The final exposure time = exposureTime * exposureTimeResNs (nano-seconds) */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 exposureTime;  /* Depends upon resolution */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 exposureTimeResNs;  /* Expose time resolution (in ns) */

                /* ROI timestamp resolution is stored here, no need to transfer with each ROI */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 roiTimestampResN; /* ROI time stamp resolution */

                [MarshalAs(UnmanagedType.U1)]
                public Byte bitDepth;  /* 10,12,14,16 etc. */
                [MarshalAs(UnmanagedType.U1)]
                public Byte colorMask;  /* Corresponds to PL_COLOR_MODES */
                [MarshalAs(UnmanagedType.U1)]
                public Byte flags;      /* Frame flags */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 extendedMdSize;  /* Must be 0 or actual ext md data size */
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
                public Byte[] reserved;
            }

            /*
            This is a ROI header that is located before every ROI data. The size of this
            structure must remain constant. The structure is genereated by the camera.
            */
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct MD_Frame_ROI_Header
            {
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 roiNr;  /* 1-based, reset with each frame */

                /* The final timestamp = timestampBOR * roiTimestampResNs */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 timestampBOR; /* Depends upon RoiTimestampResNs resolution */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 timestampEOR; /* Depends upon RoiTimestampResNs resolution */

                public RegionType roi;             /* ROI coordinated and binning */

                [MarshalAs(UnmanagedType.U1)]
                public Byte flags;          /* ROI flags */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 extendedMdSize;/* Must be 0 or actual ext md data size in bytes */
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
                public Byte[] reserved;
            }

            /*
            This is a helper structure that is used to decode the md_frame_roi_header. Since
            the header cannot contain any pointers PVCAM will calculate all information
            using offsets from frame & ROI headers.
            The structure must be created using the pl_md_create_frame_struct() function.
            Please note the structure keeps only pointers to data residing in the image
            buffer. Once the buffer is deleted the contents of the structure become invalid.
            */
            [StructLayout(LayoutKind.Sequential)]
            public struct Md_Frame_Roi
            {
                public IntPtr header;         /* Points directly to the header within the buffer. */
                public IntPtr data;           /* Points to the ROI image data. */
                [MarshalAs(UnmanagedType.U4)]
                public UInt32 dataSize;       /* Size of the ROI image data in bytes. */
                public IntPtr extMdData;      /* Points directly to ext/ MD data within the buffer. */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 extMdDataSize;  /* Size of the ext. MD buffer. */
            }

            /*
            This is a helper structure that is used to decode the md_frame_header. Since
            the header cannot contain any pointers we need to calculate all information
            using only offsets.
            Please note the structure keeps only pointers to data residing in the image
            buffer. Once the buffer is deleted the contents of the structure become invalid.
            */
            [StructLayout(LayoutKind.Sequential)]
            public struct MD_Frame
            {
                public IntPtr header;       /* Points directly to the header withing the buffer. */
                public IntPtr extMdData;    /* Points directly to ext/ MD data within the buffer. */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 extMdDataSize;/* Size of the ext. MD buffer in bytes. */
                public RegionType impliedRoi;/* Implied ROI calculated during decoding. */
                public IntPtr roiArray;     /* An array of ROI descriptors. */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 roiCapacity;  /* Number of ROIs the structure can hold. */
                [MarshalAs(UnmanagedType.U2)]
                public UInt16 roiCount;     /* Number of ROIs found during decoding. */
            }

            #endregion
        }
    }
}
