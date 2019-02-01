/*******************************************************************************
* SVS GigE API   Declaration of GigE camera access functions
*******************************************************************************
*
* Version:     1.5.3.265 / December 2016
*
* Copyright:   SVS VISTEK GmbH
*
*******************************************************************************
 *
* Function categories:
* ---------------------------------------------------------
*    1 - Camera: Discovery and bookkeeping
*    2 - Camera: Connection
*    3 - Camera: Information
*    4 - Stream: Channel creation and control
*    5 - Stream: Channel statistics
*    6 - Stream: Channel info
*    7 - Stream: Transfer parameters
*    8 - Stream: Image access
*    9 - Stream: Image conversion
*   10 - Stream: Image characteristics
*   11 - Stream: Image statistics
*   12 - Stream: Messaging channel
*   13 - Controlling camera: Frame rate
*   14 - Controlling camera: Exposure
*   15 - Controlling camera: Gain and offset
*   16 - Controlling camera: Auto gain/exposure
*   17 - Controlling camera: Acquisition trigger
*   18 - Controlling camera: Strobe
*   19 - Controlling camera: Tap balance
*   20 - Controlling camera: Image parameter
*   21 - Controlling camera: Image appearance
*   22 - Special control: IOMux configuration
*   23 - Special control: IO control
*   24 - Special control: Serial communication
*   25 - Special control: Direct register and memory access
*   26 - Special control: Persistent settings and recovery
*   27 - General functions
*   28 - Diagnostics
* ---------------------------------------------------------
*   99 - Deprecated functions
*
*******************************************************************************
*
*
* Revision history:
 *
 * ** Version 1.5.3
 *   
 *	New Camera events are added. 
 *  Functions added:
 *    + Camera_LensReset()
 *    + Camera_LensUpdate()
 *    + Camera_getLensState()
 *    + Camera_setLensState()
 *   
 * ** Version 1.5.2
 * --------------------
 *	The maximum number of interfaces that can be detected is no more limited to 20.
 *
 ** Version 1.5.1
 * --------------------
 *	A negative value for PixelsCorrectionMapOffset is allowed. 
 *	Improvements in the c# interface. 
 *	Bug fixing when using an Adapter with multiple IP addresses.
 *
 * Version 1.5.0
 * --------------------
 * The SVS build system has been re-structured for enabling
 * “nightly builds” as well as for making InstallShield merge
 * modules available for the main “SVCam GigE Software” components
* --------------------
*
** Version 1.4.26.62
* --------------------
*   	+ Camera_setLensFocusUnit()
*		+ Camera_getLensFocusUnit() 
*		+ firmware with support for 'flipping mode'
*
** Version 1.4.26.61-2
* --------------------
*       + Camera_setTapConfigurationEx(()
*	    + Camera_getTapConfigurationEx()
*  	    + Camera_setFlippingMode()	
*	    + Camera_getFlippingMode()
*	    + Camera_setShutterMode()
*	    + Camera_getShutterMode()
*
** Version 1.4.26.61-1
* --------------------
*    - bugfix for communication timeout.
*		ACQUISITION_MODE_NO_ACQUISITION is no longer available
*
* Version 1.4.26.61
* --------------------
*    - bugfix for Multicast (thus new driver version 1.4.26)
*
*
* Version 1.4.25.60-2
* --------------------
*    - functions added:
*      + Camera_setPixelsCorrectionMap()
*	   + Camera_getPixelsCorrectionMap()
*  	   + Camera_setPixelsCorrectionControlEnabel()	
*	   + Camera_getPixelsCorrectionControlEnabel()
*	   + Camera_setPixelsCorrectionControlMark()
*	   + Camera_getPixelsCorrectionControlMark()
*      + Camera_setPixelsCorrectionMapOffset()
*      + Camera_getPixelsCorrectionMapOffset()
*      + Camera_getPixelsCorrectionMapSize()
*      + Camera_getMaximalPixelsCorrectionMapSize()
*      + Camera_getMapIndexCoordinate()
*      + Camera_deletePixelCoordinateFromMap()
*
* Version 1.4.25.60-1
* -------------------
*    - functions added:
*      + Camera_isLensAvailable()
*	   + Camera_getLensName()
*  	   + Camera_setLensFocalLenght()	
*	   + Camera_getLensFocalLenght()
*	   + Camera_getLensFocalLenghtMin()
*	   + Camera_getLensFocalLenghtMax()
*      + Camera_setLensFocus()
*      + Camera_getLensFocus()
*      + Camera_getLensFocusMin()
*      + Camera_getLensFocusMax()
*      + Camera_setLensAperture()
*      + Camera_getLensAperture()
*      + Camera_getLensApertureMin()
*      + Camera_getLensApertureMax()
*      
* Version 1.4.25.60
* ------------------
*    - functions added:
*     + SVGigE_estimateWhiteBalanceExtended()
*	  + Camera_openConnectionEx()
*	  + StreamingChannel_createEx()

* Revision history:
* Version 1.4.24.59
* -----------------
*   - functions added:
*     + Camera_getSensorTemperature()
*     + Camera_setPivMode()
*     + Camera_getPivMode()
*     + Camera_setPrescalerDevisor()
*     + Camera_getPrescalerDevisor()
*     + Camera_setDebouncerDuration()
*     + Camera_getDebouncerDuration()
*     + Camera_loadSequenceParameters()
*     + Camera_startSequencer()
*     + Camera_setStrobePolarityExtended()
*     + Camera_getStrobePolarityExtended()
*     + Camera_setStrobePositionExtended()
*     + Camera_getStrobePositionExtended()
*     + Camera_getStrobePositionIncrement()
*     + Camera_setStrobeDurationExtended()
*     + Camera_getStrobeDurationExtended()
*     + Camera_getTapGain()
*     + Camera_setTapGain()
*     + Camera_loadSettingsFromXml()  
*     + Camera_SaveSettingsToXml()  
*     + Camera_loadSequenceParameters()
*     + Camera_startSequencer()
*   - removed
*     - Camera_setUserTapGain()
*     - Camera_getUserTapGain()
*
* Version 1.4.24.58
* -----------------
*   - firmware for 'eco' camera series updated to build 1759 with support for 'trigger violation' 
*
* Version 1.4.24.57
* -----------------
*   - messages from camera added, like e.g. "trigger violation"
*   - signals 9/10 exchanged for TL consistency ("camera found"/"multicast message")
*   - GEV(GigE Vision) 	return codes properly translated into GigE_GEV_ codes (-301..-314)
*   - custom GEV codes translated to GigE_SVCAM_STATUS_ codes (-130..-132,-140..-145)
*   - functions added:
*	  + Camera_startImageCorrection()
*     + Camera_isIdleImageCorrection()
*     + Camera_setImageCorrection()
*     + Camera_getImageCorrection()
*     + Camera_setTapUserSettings()
*     + Camera_getTapUserSettings()
*   - functions deprecated
*     - Camera_setTapBalance()
*     - Camera_getTapBalance()
*   - "MultiStream" (removed)->deprecated
*
* Version 1.4.23.56
* -----------------
*   - functions deprecated
*		 - Image_getImageGray()
*     - StreamingChannel_createMultiStream()
*     - Camera_forceOpenConnection()
*
* Version 1.4.23.55
* -----------------
*   - functions added:
*     + isDriverAvailable()
*
* Version 1.4.23.54
* -----------------
*  - frame rate (color) increased by removing performance bottlenecks
*  - naming consistency improved: SVGige_ to SVGigE_
* 
* 
*******************************************************************************
* Detailed function listing
*******************************************************************************
*
* 0 - GigE DLL (implicitly called)
* ---------------------------------
*  isVersionCompliantDLL()
*  isLoadedGigEDLL()
*  getCamera()
*
* 1 - Camera: Discovery and bookkeeping
* -------------------------------------
*  CameraContainer_create()
*  CameraContainer_delete()
*  CameraContainer_discovery()
*  CameraContainer_getNumberOfCameras()
*  CameraContainer_getCamera()
*  CameraContainer_findCamera()
*	
* 2 - Camera: Connection
* ----------------------
*  Camera_openConnection()
*  Camera_openConnectionEx()
*  Camera_closeConnection()
*  Camera_setIPAddress()
*  Camera_forceValidNetworkSettings() 
*  Camera_restartIPConfiguration() 
*
* 3 - Camera: Information
* ----------------------
*  Camera_getManufacturerName()
*  Camera_getModelName()
*  Camera_getDeviceVersion()
*  Camera_getManufacturerSpecificInformation()
*  Camera_getSerialNumber()
*  Camera_setUserDefinedName()
*  Camera_getUserDefinedName()
*  Camera_getMacAddress()
*  Camera_getIPAddress()
*  Camera_getSubnetMask()
*  Camera_getPixelClock()
*  Camera_isCameraFeature()
*  Camera_readXMLFile()
*  Camera_getSensorTemperature() 
*
* 4 - Stream: Channel creation and control
* ----------------------------------------
*  StreamingChannel_create()
*  StreamingChannel_createEx()
*  StreamingChannel_createMultiStream()
*  StreamingChannel_delete()
*  StreamingChannel_setChannelTimeout()
*  StreamingChannel_getChannelTimeout()
*  StreamingChannel_setReadoutTransfer()
*  StreamingChannel_getReadoutTransfer()
*
* 5 - Stream: Channel statistics
* ------------------------------------------
*  StreamingChannel_getFrameLoss()
*  StreamingChannel_getActualFrameRate()
*  StreamingChannel_getActualDataRate()
*  StreamingChannel_getPeakDataRate()
* 
* 6 - Stream: Channel info
* ------------------------
*  StreamingChannel_getPixelType()
*  StreamingChannel_getBufferData()
*  StreamingChannel_getBufferSize()
*  StreamingChannel_getImagePitch()
*  StreamingChannel_getImageSizeX()
*  StreamingChannel_getImageSizeY()
*
* 7 - Stream: Transfer parameters
* -------------------------------
*  Camera_evaluateMaximalPacketSize()
*  Camera_setStreamingPacketSize()
*  Camera_setInterPacketDelay()
*  Camera_getInterPacketDelay()
*  Camera_setMulticastMode()
*  Camera_getMulticastMode()
*  Camera_getMulticastGroup()
*
* 8 - Stream: Image access
* ------------------------
*  Image_getDataPointer()
*  Image_getBufferIndex()
*  Image_getSignalType()
*  Image_getCamera()
*  Image_release()
*
* 9 - Stream: Image conversion
* ----------------------------
*  Image_getImageRGB()
*  Image_getImageGray()
*  Image_getImage12bitAs8bit()
*  Image_getImage12bitAs16bit()
*  Image_getImage16bitAs8bit()
*		
* 10 - Stream: Image characteristics
* ----------------------------------
*  Image_getPixelType()
*  Image_getImageSize()
*  Image_getPitch()
*  Image_getSizeX()
*  Image_getSizeY()
*
* 11 - Stream: Image statistics
* ----------------------------
*  Image_getImageID()
*  Image_getTimestamp()
*  Image_getTransferTime()
*  Image_getPacketCount()
*  Image_getPacketResend()
*
* 12 - Stream: Messaging channel
* ------------------------------
*  Stream_createEvent()
*  Stream_addMessageType()
*  Stream_removeMessageType()
*  Stream_isMessagePending()
*  Stream_registerEventCallback()
*  Stream_unregisterEventCallback()
*  Stream_getMessage()
*  Stream_getMessageData()
*  Stream_getMessageTimestamp()
*  Stream_releaseMessage()
*  Stream_flushMessages()
*  Stream_closeEvent()
*  
* 13 - Controlling camera: Frame rate
* -----------------------------------
*  Camera_setFrameRate()
*  Camera_getFrameRate()
*  Camera_getFrameRateMin()
*  Camera_getFrameRateMax()
*  Camera_getFrameRateRange()
*  Camera_getFrameRateIncrement()
*
* 14 - Controlling camera: Exposure
* ---------------------------------
*  Camera_setExposureTime()
*  Camera_getExposureTime()
*  Camera_getExposureTimeMin()
*  Camera_getExposureTimeMax()
*  Camera_getExposureTimeRange()
*  Camera_getExposureTimeIncrement()
*  Camera_setExposureDelay()
*  Camera_getExposureDelay()
*  Camera_getExposureDelayMax()
*  Camera_getExposureDelayIncrement()
*
* 15 - Controlling camera: Gain and offset
* ----------------------------------------
*  Camera_setGain()
*  Camera_getGain()
*  Camera_getGainMax()
*  Camera_getGainMaxExtended()
*  Camera_getGainIncrement()
*  Camera_setOffset()
*  Camera_getOffset()
*  Camera_getOffsetMax()
*
* 16 - Controlling camera: Auto gain/exposure
* -------------------------------------------
*  Camera_setAutoGainEnabled()
*  Camera_getAutoGainEnabled()
*  Camera_setAutoGainBrightness()
*  Camera_getAutoGainBrightness()
*  Camera_setAutoGainDynamics()
*  Camera_getAutoGainDynamics()
*  Camera_setAutoGainLimits()
*  Camera_getAutoGainLimits()
*  Camera_setAutoExposureLimits()
*  Camera_getAutoExposureLimits()
*
* 17 - Controlling camera: Acquisition trigger
* --------------------------------------------
*  Camera_setAcquisitionControl()
*  Camera_getAcquisitionControl()
*  Camera_setAcquisitionMode()
*  Camera_setAcquisitionModeAndStart()
*  Camera_getAcquisitionMode()
*  Camera_softwareTrigger()
*  Camera_softwareTriggerID()
*  Camera_softwareTriggerIDEnable()
*  Camera_setTriggerPolarity()
*  Camera_getTriggerPolarity()
*  Camera_setPivMode()
*  Camera_getPivMode()
*  Camera_setDebouncerDuration()
*  Camera_getDebouncerDuration()
*  Camera_setPrescalerDevisor()
*  Camera_getPrescalerDevisor()
*  Camera_loadSequenceParameters()
*  Camera_startSequencer()
*
* 18 - Controlling camera: Strobe
* -------------------------------
*  Camera_setStrobePolarity()
*  Camera_setStrobePolarityExtended()
*  Camera_getStrobePolarity()
*  Camera_getStrobePolarityExtended()
*  Camera_setStrobePosition()
*  Camera_setStrobePositionExtended()
*  Camera_getStrobePosition()
*  Camera_getStrobePositionExtended()
*  Camera_getStrobePositionMax()
*  Camera_getStrobePositionIncrement()
*  Camera_setStrobeDuration()
*  Camera_setStrobeDurationExtended()
*  Camera_getStrobeDuration()
*  Camera_getStrobeDurationExtended()
*  Camera_getStrobeDurationMax()
*  Camera_getStrobeDurationIncrement()
*
* 19 - Controlling camera: Tap balance
* ------------------------------------
*  Camera_saveTapBalanceSettings()
*  Camera_loadTapBalanceSettings()
*  Camera_setTapConfiguration()
*  Camera_getTapConfiguration()
*  Camera_setTapConfigurationEx()
*  Camera_getTapConfigurationEx()
*  Camera_setAutoTapBalanceMode()
*  Camera_getAutoTapBalanceMode()
*  Camera_setTapGain()
*  Camera_getTapGain()
*
* 20 - Controlling camera: Image parameter
* ---------------------------------------
*  Camera_getImagerWidth()
*  Camera_getImagerHeight()
*  Camera_getImageSize()
*  Camera_getPitch()
*  Camera_getSizeX()
*  Camera_getSizeY()
*  Camera_setBinningMode()
*  Camera_getBinningMode()
*  Camera_setAreaOfInterest()
*  Camera_getAreaOfInterest()
*  Camera_getAreaOfInterestRange()
*  Camera_getAreaOfInterestIncrement()
*  Camera_resetTimestampCounter()
*  Camera_getTimestampCounter()
*  Camera_getTimestampTickFrequency()
*  Camera_setFlippingMode()	
*	Camera_getFlippingMode()
*	Camera_setShutterMode()
*	Camera_getShutterMode()
*
* 21 - Controlling camera: Image appearance
* -----------------------------------------
*  Camera_getPixelType()
*  Camera_setPixelDepth()
*  Camera_getPixelDepth()
*  Camera_setWhiteBalance()
*  Camera_getWhiteBalance()
*  Camera_getWhiteBalanceMax()
*  Camera_setGammaCorrection()
*  Camera_setLowpassFilter()
*  Camera_getLowpassFilter()
*  Camera_setLookupTableMode()
*  Camera_getLookupTableMode()
*  Camera_setLookupTable()
*  Camera_getLookupTable()
*  Camera_startImageCorrection()
*  Camera_isIdleImageCorrection()
*  Camera_setImageCorrection()
*  Camera_getImageCorrection()
*  Camera_setPixelsCorrectionMap()
*  Camera_getPixelsCorrectionMap()
*  Camera_setPixelsCorrectionControlEnabel()	
*	Camera_getPixelsCorrectionControlEnabel()
*	Camera_setPixelsCorrectionControlMark()
*	Camera_getPixelsCorrectionControlMark()
*  Camera_setPixelsCorrectionMapOffset()
*  Camera_getPixelsCorrectionMapOffset()
*  Camera_getPixelsCorrectionMapSize()
*  Camera_getMaximalPixelsCorrectionMapSize()
*  Camera_getMapIndexCoordinate()
*  Camera_deletePixelCoordinateFromMap()
*
*
* 22 - Special control: IOMux configuration
* -------------------------------------------------------
*  Camera_getMaxIOMuxIN()
*  Camera_getMaxIOMuxOUT()
*  Camera_setIOAssignment()
*  Camera_getIOAssignment()
*
* 23 - Special control: IO control
* -------------------------------------------------------
*  Camera_setIOMuxIN()
*  Camera_getIOMuxIN()
*  Camera_setIO()
*  Camera_getIO()
*  Camera_setAcqLEDOverride()
*  Camera_getAcqLEDOverride()
*  Camera_setLEDIntensity()
*  Camera_getLEDIntensity()
*
* 24 - Special control: Serial communication
* -------------------------------------------------------
*  Camera_setUARTBuffer()
*  Camera_getUARTBuffer()
*  Camera_setUARTBaud()
*  Camera_getUARTBaud()
*
* 25 - Special control: Direct register and memory access
* -------------------------------------------------------
*  Camera_setGigECameraRegister()
*  Camera_getGigECameraRegister()
*  Camera_writeGigECameraMemory()
*  Camera_readGigECameraMemory()
*  Camera_forceOpenConnection()
*
* 26 - Special control: Persistent settings and recovery
* ------------------------------------------------------
*  Camera_writeEEPROM()
*  Camera_readEEPROM()
*  Camera_restoreFactoryDefaults()
*  Camera_loadSettingsFromXml()  
*  Camera_SaveSettingsToXml()  
*
* 27 - General functions
* ----------------------
*  SVGigE_estimateWhiteBalance()
*  SVGigE_estimateWhiteBalanceExtended()
*  SVGigE_writeImageToBitmapFile()
*  SVGigE_installFilterDriver()
*  SVGigE_uninstallFilterDriver()
*
* 28 - Diagnostics
* ----------------
*  getErrorMessage()
*  Camera_registerForLogMessages()
*
* 29 - Special control: Lens control
* ------------------------------------------------------
*  Camera_isLensAvailable()
*  Camera_getLensName()
*  Camera_setLensFocalLength()
*  Camera_getLensFocalLength()
*  Camera_getLensFocalLengthMin()
*  Camera_getLensFocalLengthMax()
* Camera_setLensFocusUnit
* Camera_getLensFocusUnit
*  Camera_setLensFocus()
*  Camera_getLensFocus() 
*  Camera_getLensFocusMin()
*  Camera_getLensFocusMax() 
*  Camera_setLensAperture()
*  Camera_getLensAperture()
*  Camera_getLensApertureMin()
*  Camera_getLensApertureMax()
*
* ---------------------------------------------------------
* 99 - Deprecated functions
* ---------------------------------------------------------
 *  Camera_startAcquisitionCycle()
 *  Camera_setTapCalibration()
 *  Camera_getTapCalibration()
 *  Camera_setLUTMode()
 *  Camera_getLUTMode()
 *  Camera_createLUTwhiteBalance()
 *  Camera_stampTimestamp()
 *  Camera_getTimestamp()
 *  Image_getImageGray()
 *  Camera_forceOpenConnection()
 *  StreamingChannel_createMultiStream()
 *  Camera_setTapBalance()
 *  Camera_getTapBalance()
 *	StreamingChannel_setChannelTimeout()
 *  StreamingChannel_getChannelTimeout()
 *	Camera_setTapUserSettings()
 *	Camera_getTapUserSettings()
 *
*
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GigEApi
{
    public class GigeApi
    {
        /** SVGgiE DLL
        *  Dependent on system platform a 32-bit or 64-bit version of SVGigE.dll will be loaded
        */

        const string SVGigE_DLL64 = "SVGigE.x64.dll";

        const string SVGigE_DLL = "SVGigE.dll";

        /** Camera Container client handle.
        *  A camera container client handle serves as a reference for managing multiple
        *  clients that are connected to a single camera container (which has no notion
        *  about clients). A value of SVGigE_NO_CLIENT serves as an indicator for
        *  an invalid camera container client before obtaining a valid handle.
        */
        public const int SVGigE_NO_CLIENT = -1;

        /** Camera handle.
        *  A camera handle serves as a reference for access functions to camera
        *  functionality. A value of SVGigE_NO_CAMERA serves as an indicator for an
        *  invalid camera handle before obtaining a camera from a camera container.
        */
        public const int SVGigE_NO_CAMERA = -1;

        /** Streaming channel handle.
        *  A streaming channel handle serves as a reference for an image stream
        */
        public const int SVGigE_NO_STREAMING_CHANNEL = -1;

        /** Event handle.
        *  An event handle serves as a reference for a messaging channel
        */
        public const int SVGigE_NO_EVENT = -1;

        /** Message handle.
        *  A message handle serves as a reference for a single message
        */
        public const int SVGigE_NO_MESSAGE = -1;


        // Ver. 1.4.16.40                   
        // Indicate if pixel is monochrom or RGB
        public const uint GVSP_PIX_MONO = 0x01000000;
        public const uint GVSP_PIX_RGB = 0x02000000;

        // Indicate effective number of bits occupied by the pixel (including padding).
        // This can be used to compute amount of memory required to store an image.
        public const uint GVSP_PIX_OCCUPY8BIT = 0x00080000;
        public const uint GVSP_PIX_OCCUPY12BIT = 0x000C0000;
        public const uint GVSP_PIX_OCCUPY16BIT = 0x00100000;
        public const uint GVSP_PIX_OCCUPY24BIT = 0x00180000;

        // Bit masks 
        public const uint GVSP_PIX_COLOR_MASK = 0xFF000000;
        public const uint GVSP_PIX_EFFECTIVE_PIXELSIZE_MASK = 0x00FF0000;
        public const uint GVSP_PIX_ID_MASK = 0x0000FFFF;

        // Bit shift value
        public const uint GVSP_PIX_EFFECTIVE_PIXELSIZE_SHIFT = 16;



        public const uint GVPC_USER_SPACE_SENSORDATA = 0xA800;
        public const uint GVPC_USER_SPACE_SENSORDATA_STORE = GVPC_USER_SPACE_SENSORDATA + 8;
        public const uint GVPC_USER_SPACE_SENSORDATA_GAIN_0DB_LEFT = GVPC_USER_SPACE_SENSORDATA + 12;
        public const uint GVPC_USER_SPACE_SENSORDATA_OFFSET_LEFT = GVPC_USER_SPACE_SENSORDATA + 16;
        public const uint GVPC_USER_SPACE_SENSORDATA_GAIN_0DB_RIGHT = GVPC_USER_SPACE_SENSORDATA + 20;
        public const uint GVPC_USER_SPACE_SENSORDATA_OFFSET_RIGHT = GVPC_USER_SPACE_SENSORDATA + 24;

        public const uint GVPC_USER_SPACE_CAMMODE = 0xA300;

        public const uint GVPC_USER_SPACE_CAMMMODE_ADCGAIN = GVPC_USER_SPACE_CAMMODE + 72;
        public const uint GVPC_USER_SPACE_CAMMMODE_ADCOFFSET = GVPC_USER_SPACE_CAMMODE + 76;

        public const uint GIGE_CAMERA_ACCESS_KEY = 42;



        /** Function 	returns.
        *  API Functions may 	return success or error codes by this data type unless
        *  they 	return specific values
        */
        public enum SVSGigeApiReturn : int
        {
            SVGigE_SUCCESS = 0,
            SVGigE_ERROR = -1,
            SVGigE_DLL_NOT_LOADED = -2,
            SVGigE_DLL_VERSION_MISMATCH = -3,
            SVGigE_DRIVER_VERSION_MISMATCH = -4,
            SVGigE_WINSOCK_INITIALIZATION_FAILED = -5,
            SVGigE_CAMERA_CONTAINER_NOT_AVAILABLE = -6,
            SVGigE_NO_CAMERAS_DETECTED = -7,
            SVGigE_CAMERA_NOT_FOUND = -8,
            SVGigE_CAMERA_OPEN_FAILED = -9,
            SVGigE_CAMERA_COMMUNICATION_FAILED = -10,
            SVGigE_CAMERA_REGISTER_ACCESS_DENIED = -11,
            SVGigE_UNKNOWN_LUT_MODE = -12,
            SVGigE_STREAMING_FUNCTION_ALREADY_REGISTERED = -13,
            SVGigE_STREAMING_NOT_INITIALIZED = -14,
            SVGigE_OUT_OF_MEMORY = -15,
            SVGigE_INVALID_CALLBACK_INITIALIZATION = -16,
            SVGigE_INVALID_CALLBACK_FUNCTION_POINTER = -17,
            SVGigE_IMAGE_NOT_AVAILABLE = -18,
            SVGigE_INSUFFICIENT_RGB_BUFFER_PROVIDED = -19,
            SVGigE_STREAMING_CHANNEL_NOT_AVAILABLE = -20,
            SVGigE_INVALID_PARAMETERS = -21,
            SVGigE_PIXEL_TYPE_NOT_SUPPORTED = -22,
            SVGigE_FILE_COULD_NOT_BE_OPENED = -23,
            SVGigE_TRANSPORT_LAYER_NOT_AVAILABLE = -24,
            SVGigE_XML_FILE_NOT_AVAILABLE = -25,
            SVGigE_WHITE_BALANCE_FAILED = -26,
            SVGigE_DEPRECATED_FUNCTION = -27,
            SVGigE_WRONG_DESTINATION_BUFFER_SIZE = -28,
            SVGigE_INSUFFICIENT_DESTINATION_BUFFER_SIZE = -29,
            SVGigE_CAMERA_NOT_IN_CURRENT_SUBNET = -30,
            SVGigE_CAMERA_MOVED_TO_FOREIGN_SUBNET = -31,
            SVGigE_IMAGE_SKIPPED_IN_CALLBACK = -32,
            SVGigE_MESSAGE_CHANNEL_NOT_SUPPORTED = -33,
            SVGigE_MESSAGE_CHANNEL_NOT_OPENED    = -34,
            SVGigE_MESSAGE_TYPE_NOT_SUPPORTED    = -35,

            // Mapped camera 	return codes
            SVGigE_SVCAM_STATUS_ERROR = -101,
            SVGigE_SVCAM_STATUS_SOCKET_ERROR = -102,
            SVGigE_SVCAM_STATUS_ALREADY_CONNECTED = -103,
            SVGigE_SVCAM_STATUS_INVALID_MAC = -104,
            SVGigE_SVCAM_STATUS_UNREACHABLE = -105,
            SVGigE_SVCAM_STATUS_ACCESS_DENIED = -106,
            SVGigE_SVCAM_STATUS_BUSY = -107,
            SVGigE_SVCAM_STATUS_LOCAL_PROBLEM = -108,
            SVGigE_SVCAM_STATUS_MSG_MISMATCH = -109,
            SVGigE_SVCAM_STATUS_PROTOCOL_ID_MISMATCH = -110,
            SVGigE_SVCAM_STATUS_NOT_ACKNOWLEDGED = -111,
            SVGigE_SVCAM_STATUS_RECEIVE_LENGTH_MISMATCH = -112,
            SVGigE_SVCAM_STATUS_ACKNOWLEDGED_LENGTH_MISMATCH = -113,
            SVGigE_SVCAM_STATUS_NO_ACK_BUFFER_PROVIDED = -114,
            SVGigE_SVCAM_STATUS_CONNECTION_LOST = -115,
            SVGigE_SVCAM_STATUS_TL_NOT_AVAILABLE = -116,
            SVGigE_SVCAM_STATUS_DRIVER_VERSION_MISMATCH = -117,
            SVGigE_SVCAM_STATUS_FEATURE_NOT_SUPPORTED = -118,
            SVGigE_SVCAM_STATUS_PENDING_CHANNEL_DETECTED = -119,
            SVGigE_SVCAM_STATUS_LUT_NOT_AVAILABLE = -120,
            SVGigE_SVCAM_STATUS_LUT_SIZE_MISMATCH = -121,
            SVGigE_SVCAM_STATUS_NO_MATCHING_IP_ADDRESS = -122,
            SVGigE_SVCAM_STATUS_DISCOVERY_INFO_CHANGED = -123,
            SVGigE_SVCAM_STATUS_FIRMWARE_UPGRADE_REQUIRED = -124,
            SVGigE_SVCAM_STATUS_MULTICAST_NOT_SUPPORTED = -125,
            SVGigE_SVCAM_STATUS_PIXELDEPTH_NOT_SUPPORTED = -126,
            SVGigE_SVCAM_STATUS_IO_CONFIG_NOT_SUPPORTED = -127,
            SVGigE_SVCAM_STATUS_USER_DEFINED_NAME_TOO_LONG = -128,
            SVGigE_SVCAM_STATUS_INVALID_RESULT_POINTER = -129,
            SVGigE_SVCAM_STATUS_ARP_REQUEST_FAILED = -130,
            SVGigE_SVCAM_STATUS_ALREADY_STREAMING = -131,
            SVGigE_SVCAM_STATUS_NO_STREAMING_CHANNEL = -132,
            SVGigE_SVCAM_STATUS_CAMERA_OCCUPIED = -133,
            SVGigE_SVCAM_STATUS_SECOND_LINK_MISSING = -134,
            SVGigE_SVCAM_STATUS_CAMERA_NOT_CONNECTED = -135,
            SVGigE_SVCAM_STATUS_FILTER_DRIVER_NOT_AVAILABLE = -136,
            SVGigE_SVCAM_STATUS_FILTER_INF_FILE_NOT_FOUND = -137,
            SVGigE_SVCAM_STATUS_FILTER_INF_FILE_COPY_FAILED = -138,
            SVGigE_SVCAM_STATUS_BINNING_MODE_NOT_AVAILABLE = -139,
            SVGigE_SVCAM_STATUS_CREATE_BUFFERS_FAILED       	= -140,
            SVGigE_SVCAM_STATUS_FFC_INVALID_PARAMETER      	    = -141,
            SVGigE_SVCAM_STATUS_FFC_ACQUISITION_RUNNING    	    = -142,
            SVGigE_SVCAM_STATUS_FFC_COLDEPTH_NOT_12BPP        	= -143,
            SVGigE_SVCAM_STATUS_FFC_ACTIVE                      = -144,
            SVGigE_SVCAM_STATUS_ONLY_OFFSET_NOT_ACTIVE         	= -145,
            SVGigE_SVCAM_STATUS_CAMERA_COMMUNICATION_ERROR = -199,

            // Mapped transport layer 	return codes
            SVGigE_TL_SUCCESS = 0,
            SVGigE_TL_DLL_NOT_LOADED = -201,
            SVGigE_TL_DLL_VERSION_MISMATCH = -202,
            SVGigE_TL_DLL_ALIGNMENT_PROBLEM = -203,
            SVGigE_TL_OPERATING_SYSTEM_NOT_SUPPORTED = -204,
            SVGigE_TL_WINSOCK_INITIALIZATION_FAILED = -205,
            SVGigE_TL_CAMERA_NOT_FOUND = -206,
            SVGigE_TL_CAMERA_OPEN_FAILED = -207,
            SVGigE_TL_CAMERA_NOT_OPEN = -208,
            SVGigE_TL_CAMERA_UNKNWON_COMMAND = -209,
            SVGigE_TL_CAMERA_PAYLOAD_LENGTH_EXCEEDED = -210,
            SVGigE_TL_CAMERA_PAYLOAD_LENGTH_INVALID = -211,
            SVGigE_TL_CAMERA_COMMUNICATION_TIMEOUT = -212,
            SVGigE_TL_CAMERA_ACCESS_DENIED = -213,
            SVGigE_TL_CAMERA_CONNECTION_LOST = -214,
            SVGigE_TL_STREAMING_FUNCTION_ALREADY_REGISTERED = -215,
            SVGigE_TL_NO_STREAMING_PORT_FOUND = -216,
            SVGigE_TL_OUT_OF_MEMORY = -217,
            SVGigE_TL_INVALID_CALLBACK_FUNCTION_POINTER = -218,
            SVGigE_TL_STREAMING_CHANNEL_NOT_AVAILABLE = -219,
            SVGigE_TL_STREAMING_CHANNEL_VERSION_MISMATCH = -220,
            SVGigE_TL_CALLBACK_INVALID_PARAMETER = -221,
            SVGigE_TL_CALLBACK_IMAGE_DATA_MISSING = -222,
            SVGigE_TL_OPENING_STREAMING_CHANNEL_FAILED = -223,
            SVGigE_TL_CHANNEL_CREATION_FAILED = -224,
            SVGigE_TL_DRIVER_NOT_INSTALLED = -225,
            SVGigE_TL_PENDING_CHANNEL_DETECTED = -226,
            SVGigE_TL_UNSUPPORTED_PACKET_FORMAT = -227,
            SVGigE_TL_INCORRECT_BLOCK_ID = -228,
            SVGigE_TL_INVALID_PARAMETER = -229,
            SVGigE_TL_STREAMING_CHANNEL_LOOSING_FRAMES = -230,
            SVGigE_TL_FRAME_BUFFER_ALLOCATION_FAILED = -231,
            SVGigE_TL_FRAME_BUFFER_INFO_NOT_AVAILABLE = -232,
            SVGigE_TL_MULTICAST_MANAGEMENT_FAILED = -233,
            SVGigE_TL_CAMERA_SIGNAL_IGNORED = -234,
            SVGigE_TL_FILTER_DRIVER_INSTALLATION_NOT_SUPPORTED = -235,
            SVGigE_TL_FILTER_DRIVER_NOT_AVAILABLE = -236,
            SVGigE_TL_FILTER_INF_FILE_NOT_FOUND = -237,
            SVGigE_TL_FILTER_INF_FILE_COPY_FAILED = -238,
            SVGigE_TL_FILTER_INITIALIZING_COM_FAILED = -239,
            SVGigE_TL_FILTER_DRIVER_INSTALLATION_FAILED = -240,
            SVGigE_TL_FILTER_DRIVER_DEINSTALLATION_FAILED = -241,
            SVGigE_TL_DRIVER_INSTALL_REQUIRES_ADMIN_PRIVILEGES = -242,
            SVGigE_TL_DRIVER_INSTALL_REQUIRES_REBOOT			= -243,
            SVGigE_TL_DRIVER_REINSTALL_AFTER_REBOOT_REQUIRED	= -244,
            SVGigE_TL_DRIVER_X86_ON_X64_NOT_SUPPORTED			= -245,

            // Mapped GEV 	return codes
            SVGigE_GEV_STATUS_NOT_IMPLEMENTED = -301,
            SVGigE_GEV_STATUS_INVALID_PARAMETER = -302,
            SVGigE_GEV_STATUS_INVALID_ADDRESS = -303,
            SVGigE_GEV_STATUS_WRITE_PROTECT = -304,
            SVGigE_GEV_STATUS_BAD_ALIGNMENT = -305,
            SVGigE_GEV_STATUS_ACCESS_DENIED = -306,
            SVGigE_GEV_STATUS_BUSY = -307,
            SVGigE_GEV_STATUS_LOCAL_PROBLEM = -308,
            SVGigE_GEV_STATUS_MSG_MISMATCH = -309,
            SVGigE_GEV_STATUS_INVALID_PROTOCOL = -310,
            SVGigE_GEV_STATUS_NO_MSG = -311,
            SVGigE_GEV_STATUS_PACKET_UNAVAILABLE = -312,
            SVGigE_GEV_STATUS_DATA_OVERRUN = -313,
            SVGigE_GEV_STATUS_INVALID_HEADER = -314,
            SVGigE_GEV_STATUS_ERROR = -399,

            // Mapped MessagingChannel 	return codes
            SVGigE_MC_MESSAGING_CHANNEL_NOT_AVAILABLE = -400,
            SVGigE_MC_BUFFER_OVERRUN = -401,
            SVGigE_MC_MEMORY_PROBLEM = -402,
            SVGigE_MC_EVENT_NOT_FOUND = -403,
            SVGigE_MC_MESSAGE_TYPE_EVENT_NOT_FOUND = -404,
            SVGigE_MC_MESSAGE_TYPE_EVENT_EXISTS_ALREADY = -405,
            SVGigE_MC_MESSAGE_PENDING = -406,
            SVGigE_MC_MESSAGE_TIMEOUT = -407,
            SVGigE_MC_MESSAGE_QUEU_EMPTY = -408,
            SVGigE_MC_MESSAGE_EVENT_ERROR = -409,
            SVGigE_MC_MESSAGE_ID_MISMATCH = -410,
            SVGigE_MC_CALLBACK_INVALID = -411,
            SVGigE_MC_CALLBACK_NOT_REGISTERED = -412,
            SVGigE_MC_CALLBACK_REGISTERED_ALREADY = -413,
            SVGigE_MC_CALLBACK_THREAD_NOT_RUNNING = -414,
            SVGigE_MC_CALLBACK_THREAD_STILL_RUNNING = -415,

        }

        public enum SVGigETL_Type : int
        {
            SVGigETL_TypeNone = 0,
            SVGigETL_TypeFilter = 1,
            SVGigETL_TypeWinSock = 2,
            //SVGigETL_TypeLinuxKmod = 3,    // loadable kernel module on Linux platforms
            //SVGigETL_TypeLinuxSocket = 4,    // sockets availabel on Linux platforms
        }

        /** Camera feature information.         
        *  A camera can support several items from the following set of camera features.
        */
        public enum CAMERA_FEATURE : uint
        {
            CAMERA_FEATURE_SOFTWARE_TRIGGER = 0,  // camera can be triggered by software
            CAMERA_FEATURE_HARDWARE_TRIGGER = 1,  // hardware trigger supported as well as trigger polarity
            CAMERA_FEATURE_HARDWARE_TRIGGER_EXTERNAL_EXPOSURE = 2,  // hardware trigger with internal exposure supported as well as trigger polarity
            CAMERA_FEATURE_FRAMERATE_IN_FREERUNNING_MODE = 3,  // framerate can be adjusted (in free-running mode)
            CAMERA_FEATURE_EXPOSURE_TIME = 4,  // exposure time can be adjusted
            CAMERA_FEATURE_EXPOSURE_DELAY = 5,  // exposure delay can be adjusted
            CAMERA_FEATURE_STROBE = 6,  // strobe is supported (polarity, duration and delay)
            CAMERA_FEATURE_AUTOGAIN = 7,  // autogain is supported
            CAMERA_FEATURE_ADCGAIN = 8,  // 2009-05-15/deprecated
            CAMERA_FEATURE_GAIN = 8,  // the ADC's gain can be adjusted
            CAMERA_FEATURE_AOI = 9,  // image acquisition can be done for an AOI (area of interest)
            CAMERA_FEATURE_BINNING = 10, // binning is supported
            CAMERA_FEATURE_UPDATE_REGISTER = 11, // streaming channel related registers can be pre-set and then updated at once (e.g. for changing an AOI)
            CAMERA_FEATURE_NOT_USED = 12, // not in use
            CAMERA_FEATURE_COLORDEPTH_8BPP = 13, // a pixel depth of 8bit is supported
            CAMERA_FEATURE_COLORDEPTH_10BPP = 14, // a pixel depth of 10bit is supported
            CAMERA_FEATURE_COLORDEPTH_12BPP = 15, // a pixel depth of 12bit is supported
            CAMERA_FEATURE_COLORDEPTH_16BPP = 16, // a pixel depth of 16bit is supported
            CAMERA_FEATURE_ADCOFFSET = 17, // the ADC's offset can be adjusted
            CAMERA_FEATURE_SENSORDATA = 18, // the camera's sensor/ADC settings can be adjusted (the factory settings)
            CAMERA_FEATURE_WHITEBALANCE = 19, // a LUT for whitebalancing is available
            CAMERA_FEATURE_LUT_10TO8 = 20, // a LUT from 10 bit to 8 bit is available
            CAMERA_FEATURE_LUT_12TO8 = 21, // a LUT from 12 bit to 8 bit is available
            CAMERA_FEATURES_FLAGS = 22, // streaming state and image availability can be queried from camera
            CAMERA_FEATURES_READOUT_CONTROL = 23, // time of image read out from camera can be controlled by application
            CAMERA_FEATURES_TAP_CONFIG = 24, // the tap configuration can be changed (switching between one and two taps)
            CAMERA_FEATURES_ACQUISITION = 25, // acquisition can be controlled by start/stop
            CAMERA_FEATURES_TAPBALANCE = 26, // camera is capable of running auto tap balance
            CAMERA_FEATURES_BINNING_V2 = 27, // camera offers extended binning modes
            CAMERA_FEATURES_ROTATE_180 = 28, // image is rotated by 180°
            CAMERA_FEATURES_CAMMODE_NG = 29, // camera has a next-generation register mapping
            CAMERA_FEATURES_CAMMODE_CLASSIC = 30, // camera has a classic register mapping
            CAMERA_FEATURES_NEXT_FEATUREVECTOR = 31, // a subsequent feature vector is available
            // Extended feature vector
            CAMERA_FEATURES2_START = 32, // first extended camera feature
            CAMERA_FEATURES2_1_TAP = 32, // camera supports a single tap sensor
            CAMERA_FEATURES2_2_TAP = 33, // camera supports a dual tap sensor
            CAMERA_FEATURES2_3_TAP = 34, // camera supports a triple tap sensor
            CAMERA_FEATURES2_4_TAP = 35, // camera supports a quadruple tap sensor
            CAMERA_FEATURES2_REBOOT = 36, // a remote reboot command is supported
            CAMERA_FEATURES2_IOMUX = 37, // IO multiplexer functionality is available
            CAMERA_FEATURES2_SOFTWARE_TRIGGER_ID = 38, // writing a software trigger ID into images is supported
            CAMERA_FEATURES2_39 = 39, // reserved
            CAMERA_FEATURES2_40 = 40, // reserved
            CAMERA_FEATURES2_41 = 41, // reserved
            CAMERA_FEATURES2_42 = 42, // reserved
            CAMERA_FEATURES2_IOMUX_PWM = 43, // PWM A and B signals are available in IO multiplexer
            CAMERA_FEATURES2_IOMUX_STROBE2 = 44, // STROBE0 and STROBE1 signals are available in IO multiplexer
            CAMERA_FEATURES2_45 = 45, // reserved
            CAMERA_FEATURES2_IOMUX_EXPOSE = 46, // EXPOSE signal is available in IO multiplexer
            CAMERA_FEATURES2_IOMUX_READOUT = 47, // READOUT signal is available in IO multiplexer
            CAMERA_FEATURES2_FLATFIELDCORRECTION			   = 48, // FLATFIELDCORRECTION is available
            CAMERA_FEATURES2_SHADINGCORRECTION				   = 49, // SHADINGCORRECTION is available
            CAMERA_FEATURES2_DEFECTPIXEL					   = 50, // DEFECTPIXEL treatment is available
            CAMERA_FEATURES2_TRIGGERBOTHEDGES                  = 51, // TRIGGERBOTHEDGES is available
            CAMERA_FEATURES2_USERGAIN                          = 52, // USERGAIN is available
            CAMERA_FEATURES2_USEROFFSET                        = 53, // USEROFFSET is available
            CAMERA_FEATURES2_BINNING_X2                        = 54, // BINNING_X2 is availble
            CAMERA_FEATURES2_BINNING_X3                        = 55, // BINNING_X3 is available
            CAMERA_FEATURES2_BINNING_X4                        = 56, // BINNING_X4 is available
            CAMERA_FEATURES2_IOMUX_LOGIC					   = 57, // IOMUX_LOGIC is available
            CAMERA_FEATURES2_IOMUX_STROBE4                     = 58, // IOMUX_STROBE4 is available
            CAMERA_FEATURES2_LENSCONTROL       				   = 59, // LENSCONTROL is supported
            CAMERA_FEATURES2_1_TAP_1X_1Y			  		   = 60, // camera supports a single tap sensor
            CAMERA_FEATURES2_2_TAP_2XE_1Y			  		   = 61, // camera supports a dual tap left/right sensor
            CAMERA_FEATURES2_2_TAP_1X_2YE			  		   = 62, // camera supports a dual tap top/bottom sensor
            CAMERA_FEATURES2_4_TAP_2XE_2YE					   = 63, // camera supports a quad tap sensor
            // Extended feature vector
            CAMERA_FEATURES3_START               			   = 64, // second extended camera feature 
            CAMERA_FEATURES3_REVERSE_X	              		   = 64, // camera supports horizontal flipping
            CAMERA_FEATURES3_REVERSE_Y	                	   = 65, // camera supports vertical flipping
            CAMERA_FEATURES3_GLOBAL_SHUTTER                    = 66, // camera supports GLOBAL SHUTTER  Mode
            CAMERA_FEATURES3_ROLLING_SHUTTER                   = 67, // camera supports ROLLING SHUTTER Mode
            CAMERA_FEATURES3_MFT_FOCUS_UNIT                    = 68, // MFT focus unit can be changed,  
        }

        /** Look-up table mode.     // Ver. 1.4.16.40
        *  A camera can be instructed to apply a look-up table. Usually this will
        *  be used for running a gamma correction. However, other goals can also
        *  be implemented by a look-up table, like converting a gray-scale picture 
        *  into a binary black/white picture.
        */
        public enum LUT_MODE : int
        {
            LUT_MODE_DISABLED = 0,
            LUT_MODE_WHITE_BALANCE = 1,    // 2006-12-20: deactivated, use  // Camera_setWhiteBalance() instead
            LUT_MODE_ENABLED = 2,
        }

        /** Binning mode.         // Ver. 1.4.16.40
        *  A camera can be set to one of the following pre-defined binning modes
        */
        public enum BINNING_MODE : int
        {
            BINNING_MODE_OFF = 0,
            BINNING_MODE_HORIZONTAL = 1,
            BINNING_MODE_VERTICAL = 2,
            BINNING_MODE_2x2 = 3,
            //	BINNING_MODE_3x3=4,// not supported 
            //	BINNING_MODE_4x4=5,// not supported 
        }

        /** Lowpass filter.
        *  A lowpass filter can be activated/deactivated according to the
        *  following options.
        */
        public enum LOWPASS_FILTER : int
        {
            LOWPASS_FILTER_NONE = 0,
            LOWPASS_FILTER_3X3 = 1,
        }


        /** Multicast mode      // Ver. 1.4.16.40
        *  An application can receive images from a camera as a multicast controller,
        *  multicast listener or by non-multicast (default, unicast).
        */
        public enum MULTICAST_MODE : int
        {
            MULTICAST_MODE_NONE = 0,
            MULTICAST_MODE_LISTENER = 1,
            MULTICAST_MODE_CONTROLLER = 2
        }

        /** Acquisition Mode */
        // Ver. 1.4.16.40
        public enum ACQUISITION_MODE : int
        {
            ACQUISITION_MODE_STOP = 0, // deprecated
            ACQUISITION_MODE_FIXED_FREQUENCY = 1,
            ACQUISITION_MODE_SOFTWARE_TRIGGER = 2,
            ACQUISITION_MODE_EXT_TRIGGER_INT_EXPOSURE = 3,
            ACQUISITION_MODE_EXT_TRIGGER_EXT_EXPOSURE = 4,
        }

        /** Acquisition control        // Ver. 1.4.16.40
        *  A camera can be set to the following acquisition control modes
        */
        public enum ACQUISITION_CONTROL : uint
        {
            ACQUISITION_CONTROL_STOP = 0,
            ACQUISITION_CONTROL_START = 1,
        }

        /** Trigger polarity.       // Ver. 1.4.16.40
        *  A camera can be set to positive or negative trigger polarity
        */
        public enum TRIGGER_POLARITY : uint
        {
            TRIGGER_POLARITY_POSITIVE = 0,
            TRIGGER_POLARITY_NEGATIVE = 1,
        }

        /** Strobe polarity.      // Ver. 1.4.16.40
        *  A camera can be set to positive or negative strobe polarity
        */
        public enum STROBE_POLARITY : uint
        {
            STROBE_POLARITY_POSITIVE = 0,
            STROBE_POLARITY_NEGATIVE = 1,
        }

        /** Pive mode // Ver. 1.4.23.56
        *  A  camera can be set to enabled or disabled Piv mode 
        */
        public enum PIV_MODE : uint
        {
            PIV_MODE_ENABLED = 1,
            PIV_MODE_DISABLED = 0,
        }

        /** Tap selection defines.
        *  Each tap of a 2-tap or 4-tap camera can be selected
        *  by using one of the following defines.
        */

        public enum SVGIGE_TAP_SELECT : uint
        {
            SVGIGE_TAP_SELECT_TAP0 = 0,
            SVGIGE_TAP_SELECT_TAP1 = 1,
            SVGIGE_TAP_SELECT_TAP2 = 2,
            SVGIGE_TAP_SELECT_TAP3 = 3,
        }


        /** Tap configuration	selection defines.
        *  Each configuration  of a 1-tap, 2-tap (horizontal and vertical) or 4-tap can be selected
        *  by using one of the following defines.
        */
        public enum SVGIGE_TAP_CONFIGURATION_SELECT : uint
        {
            SVGIGE_SELECT_SINGLE_TAP = 0, //  version 1.4.26.62
            SVGIGE_SELECT_DUAL_TAP_H = 1,
            SVGIGE_SELECT_DUAL_TAP_V = 2,
            SVGIGE_SELECT_QUAD = 3,
        }


        /** flipping mode selection defines.
        *  the following modes of flipping are available 
        */
        public enum SVGIGE_FLIPPING_MODE : uint //  version 1.4.26.62
        {
            SVGIGE_REVERSE_OFF = 0,
            SVGIGE_REVERSE_X = 1,
            SVGIGE_REVERSE_Y = 2,
            SVGIGE_REVERSE_X_Y = 3,
        }

        /** Shutter mode selection defines.
        *  the following modes of Shutter are available
        */
        public enum SVGIGE_SHUTTER_MODE : uint // 1.4.26.62
        {
            SVGIGE_GLOBAL_SHUTTER = 0,
            SVGIGE_ROLLING_SHUTTER = 1,
        }

        /** Bayer conversion method   */
        // Ver. 1.4.16.40       
        public enum BAYER_METHOD : int
        {
            BAYER_METHOD_NONE = -1,
            BAYER_METHOD_NEAREST = 0,  // 2009-03-30: deprecated, mapped to BAYER_METHOD_SIMPLE
            BAYER_METHOD_SIMPLE = 1,
            BAYER_METHOD_BILINEAR = 2,  // 2009-03-30: deprecated, mapped to BAYER_METHOD_HQLINEAR
            BAYER_METHOD_HQLINEAR = 3,
            BAYER_METHOD_EDGESENSE = 4,  // 2009-03-30: deprecated, mapped to BAYER_METHOD_HQLINEAR
            BAYER_METHOD_GRAY = 5,
        };


        /** Pixel depth defines.
        *  The following pixel depths can be supported by a camera
        */
        public enum SVGIGE_PIXEL_DEPTH : uint   // Ver. 1.4.16.40       
        {
            SVGIGE_PIXEL_DEPTH_8 = 0,
            //SVGIGE_PIXEL_DEPTH_10    =1, // not supported 
            SVGIGE_PIXEL_DEPTH_12 = 2,
            SVGIGE_PIXEL_DEPTH_16 = 3,
        }

        /** Correction step.
        *  While performing image correction, a sequence of
        *  particular steps is needed as they are defined below .
        */
        public enum IMAGE_CORRECTION_STEP : uint   // Ver. 1.4.24.59       
        {
            IMAGE_CORRECTION_IDLE = 0,
            IMAGE_CORRECTION_ACQUIRE_BLACK_IMAGE = 1,
            IMAGE_CORRECTION_ACQUIRE_WHITE_IMAGE = 2,
            IMAGE_CORRECTION_SAVE_TO_EEPROM = 3,

        }

        /** Correction mode.
        *  After a successful image correction run, one of
        *  the following modes can be activated in order to
        *  control what image correction method is used.
        */
        public enum IMAGE_CORRECTION_MODE : uint   // Ver. 1.4.24.59       
        {
            IMAGE_CORRECTION_NONE = 0,
            IMAGE_CORRECTION_OFFSET_ONLY = 1,
            IMAGE_CORRECTION_ENABLED = 2,
        }

        /** Auto tap balance modes.
        *  The following modes of auto tap balancing are available
        */
        public enum SVGIGE_AUTO_TAP_BALANCE_MODE : uint  // Ver. 1.4.16.40
        {
            SVGIGE_AUTO_TAP_BALANCE_MODE_OFF = 0,
            SVGIGE_AUTO_TAP_BALANCE_MODE_ONCE = 1,
            SVGIGE_AUTO_TAP_BALANCE_MODE_CONTINUOUS = 2,
        }

        /** The following maps for pixels correction are available
        */
        public enum PIXELS_CORRECTION_MAP_SELECT : uint 	//  1.4.25.60-2
        {
            Factory_Map = 0,
            SVS_Map = 1,
            Custom_Map = 2,
        }

        /** Whitebalance Arts.
        *  The following Art of Whitebalance are available
        */
        public enum SVGIGE_Whitebalance_SELECT : uint // Ver 1.4.25.60
        {
            Histogramm_Based_WB = 0,
            Gray_Based_WB = 1,
        }

        public enum GVSP_PIXEL_TYPE : uint   // Ver. 1.4.16.40
        {
            // Unknown pixel format
            GVSP_PIX_UNKNOWN = 0x0000,

            // Mono buffer format defines
            GVSP_PIX_MONO8 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY8BIT | 0x0001),
            GVSP_PIX_MONO10 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x0003),
            GVSP_PIX_MONO10_PACKED = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY12BIT | 0x0004),
            GVSP_PIX_MONO12 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x0005),
            GVSP_PIX_MONO12_PACKED = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY12BIT | 0x0006),
            GVSP_PIX_MONO16 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x0007),

            // Bayer buffer format defines
            GVSP_PIX_BAYGR8 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY8BIT | 0x0008),
            GVSP_PIX_BAYRG8 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY8BIT | 0x0009),
            GVSP_PIX_BAYGB8 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY8BIT | 0x000A),
            GVSP_PIX_BAYBG8 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY8BIT | 0x000B),
            GVSP_PIX_BAYGR10 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x000C),
            GVSP_PIX_BAYRG10 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x000D),
            GVSP_PIX_BAYGB10 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x000E),
            GVSP_PIX_BAYBG10 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x000F),
            GVSP_PIX_BAYGR12 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x0010),
            GVSP_PIX_BAYRG12 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x0011),
            GVSP_PIX_BAYGB12 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x0012),
            GVSP_PIX_BAYBG12 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY16BIT | 0x0013),

            // Color buffer format defines
            GVSP_PIX_RGB24 = (GVSP_PIX_RGB | GVSP_PIX_OCCUPY24BIT),

            // Define for a gray image that was converted from a bayer coded image
            GVSP_PIX_GRAY8 = (GVSP_PIX_MONO | GVSP_PIX_OCCUPY8BIT),
        }

        /** following focus units   are available
        */
        public enum FOCUS_UNIT : uint   // Ver.  1.4.25.60-2
        {
            One_mm_Unit = 0,		//  focus  unit: 1 mm 
            Dec_mm_Unit = 1,	   //   focus  unit:  1/10 mm
        }

		
		/** following Lense states are available
		*/
		public enum LENS_STATE : uint
		{
		Lens_inactive = 0,	
		Lens_active =1,	  
		}
	
		
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------


        /** Signal types             // Ver. 1.4.16.40
        *  Each image that is delivered to an application by a callback will have a related signal
        *  which informs about the circumstances why a callback was triggered.
        *
        *  Usually a complete image will be delivered with the SVGigE_SIGNAL_FRAME_COMPLETED signal.
        */
        public enum SVGigE_SIGNAL_TYPE : uint
        {
            SVGigE_SIGNAL_NONE = 0,
            SVGigE_SIGNAL_FRAME_COMPLETED = 1,              // new image available, transfer was successful
            SVGigE_SIGNAL_FRAME_ABANDONED = 2,               // an image could not be completed in time and was therefore abandoned
            SVGigE_SIGNAL_START_OF_TRANSFER = 3,            // transfer of a new image has started
            SVGigE_SIGNAL_BANDWIDTH_EXCEEDED = 4,           // available network bandwidth has been exceeded
            SVGigE_SIGNAL_OLD_STYLE_DATA_PACKETS = 5,         // driver problem due to old-style driver behavior (prior to 2003, not WDM driver)
            SVGigE_SIGNAL_TEST_PACKET = 6,                   // a test packet arrived
            SVGigE_SIGNAL_CAMERA_IMAGE_TRANSFER_DONE = 7,   // the camera has finished an image transfer
            SVGigE_SIGNAL_CAMERA_CONNECTION_LOST = 8,         // connection to camera got lost
            SVGigE_SIGNAL_MULTICAST_MESSAGE = 9,             // an exceptional situation occurred during a 
            SVGigE_SIGNAL_FRAME_INCOMPLETE = 10,            // a frame could not be properly completed
            SVGigE_SIGNAL_MESSAGE_FIFO_OVERRUN = 11,         // a next entry was put into the message FIFO before the old one was released
            SVGigE_SIGNAL_CAMERA_SEQ_DONE = 12,             // the camera has finished a shutter sequence
            SVGigE_SIGNAL_CAMERA_TRIGGER_VIOLATION	= 13,  // the camera detected a trigger violation
			
			SVGigE_SIGNAL_CAMERA_FRAMETRANSFER_START		=	14,
			SVGigE_SIGNAL_CAMERA_FRAMETRANSFER_END			=	15,
			SVGigE_SIGNAL_CAMERA_FRAME_TRIGGER				=	16,
			SVGigE_SIGNAL_CAMERA_FRAME_START				=	17,
			SVGigE_SIGNAL_CAMERA_FRAME_END					=	18,
			SVGigE_SIGNAL_CAMERA_EXPOSURE_START				=	19,
			SVGigE_SIGNAL_CAMERA_EXPOSURE_END				=	20,
			
		
        }

        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------

        /* ***********************************************************************************
        ---------------------------        IOMUX      --------------------------------------
        *********************************************************************************** */
   
        public const uint IOMUX_IN_MASK_STROBE = (1 << 6);
        public const uint IOMUX_IN_MASK_UART_OUT = (1 << 5);
        public const uint IOMUX_IN_MASK_IO_RXD = (1 << 4);

        /** IO multiplexer IN signals (signal sources).
        *  A camera provides for a flexible IO signal handling where one or
        *  multiple IN signals (signal sources) can be assigned to an OUT
        *  signal (signal sink). The following IN signals are defined.
        */
        public enum SVGigE_IOMux_IN : int
        {
            SVGigE_IOMUX_IN0 = (1 << 0),
            SVGigE_IOMUX_IN1 = (1 << 1),
            SVGigE_IOMUX_IN2 = (1 << 2),
            SVGigE_IOMUX_IN3 = (1 << 3),
            SVGigE_IOMUX_IN4 = (1 << 4),
            SVGigE_IOMUX_IN5 = (1 << 5),
            SVGigE_IOMUX_IN6 = (1 << 6),
            SVGigE_IOMUX_IN7 = (1 << 7),
            SVGigE_IOMUX_IN8 = (1 << 8),
            SVGigE_IOMUX_IN9 = (1 << 9),
            SVGigE_IOMUX_IN10 = (1 << 10),
            SVGigE_IOMUX_IN11 = (1 << 11),
            SVGigE_IOMUX_IN12 = (1 << 12),
            SVGigE_IOMUX_IN13 = (1 << 13),
            SVGigE_IOMUX_IN14 = (1 << 14),
            SVGigE_IOMUX_IN15 = (1 << 15),
            SVGigE_IOMUX_IN16 = (1 << 16),
            SVGigE_IOMUX_IN17 = (1 << 17),
            SVGigE_IOMUX_IN18 = (1 << 18),
            SVGigE_IOMUX_IN19 = (1 << 19),
            SVGigE_IOMUX_IN20 = (1 << 20),
            SVGigE_IOMUX_IN21 = (1 << 21),
            SVGigE_IOMUX_IN22 = (1 << 22),
            SVGigE_IOMUX_IN23 = (1 << 23),
            SVGigE_IOMUX_IN24 = (1 << 24),
            SVGigE_IOMUX_IN25 = (1 << 25),
            SVGigE_IOMUX_IN26 = (1 << 26),
            SVGigE_IOMUX_IN27 = (1 << 27),
            SVGigE_IOMUX_IN28 = (1 << 28),
            SVGigE_IOMUX_IN29 = (1 << 29),
            SVGigE_IOMUX_IN30 = (1 << 30),
            SVGigE_IOMUX_IN31 = (1 << 31)
        }


        /** Some of the multiplexer's IN signals (signal sources) have a
        *  pre-defined usage:
        */



        // STROBE3 signal from the camera
        public const SVGigE_IOMux_IN  SVGigE_IOMux_IN_STROBE3 = SVGigE_IOMux_IN.SVGigE_IOMUX_IN21;
          
        // STROBE2 signal from the camera
	    public const  SVGigE_IOMux_IN SVGigE_IOMux_IN_STROBE2 = SVGigE_IOMux_IN.SVGigE_IOMUX_IN20;
        
        // for logic trigger
	    public const SVGigE_IOMux_IN SVGigE_IOMUX_IN_MASK_LOGIC = SVGigE_IOMux_IN.SVGigE_IOMUX_IN19;
       
        // pre-scaler for trigger purposes
	    public const SVGigE_IOMux_IN  SVGigE_IOMUX_IN_MASK_PRESCALE = SVGigE_IOMux_IN.SVGigE_IOMUX_IN17;
        
        // Reject short pulses (debounce)
	    public const SVGigE_IOMux_IN SVGigE_IOMUX_IN_MASK_DEBOUNCE = SVGigE_IOMux_IN.SVGigE_IOMUX_IN16;
        
        // PWMD signal (pulse width modulator)
	    public const SVGigE_IOMux_IN SVGigE_IOMux_IN_PWMD = SVGigE_IOMux_IN.SVGigE_IOMUX_IN14;
        
        // PWMC signal (pulse width modulator)
	    public const SVGigE_IOMux_IN SVGigE_IOMux_IN_PWMC = SVGigE_IOMux_IN.SVGigE_IOMUX_IN13;
        
        // PULS signal from camera
        public const SVGigE_IOMux_IN SVGigE_IOMUX_IN_MASK_PULSE = SVGigE_IOMux_IN.SVGigE_IOMUX_IN12;

        // READOUT signal from camera
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_READOUT = SVGigE_IOMux_IN.SVGigE_IOMUX_IN11;

        // EXPOSE signal from camera
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_EXPOSE = SVGigE_IOMux_IN.SVGigE_IOMUX_IN10;

        // PWMB signal (pulse width modulator)
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_PWMB = SVGigE_IOMux_IN.SVGigE_IOMUX_IN9;

        // PWMA signal (pulse width modulator)
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_PWMA = SVGigE_IOMux_IN.SVGigE_IOMUX_IN8;

        // STROBE1 and STROBE2 signal from the camera
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_STROBE_0and1 = (SVGigE_IOMux_IN)((int)SVGigE_IOMux_IN.SVGigE_IOMUX_IN6 | (int)SVGigE_IOMux_IN.SVGigE_IOMUX_IN7);

        // STROBE1 signal from the camera
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_STROBE1 = SVGigE_IOMux_IN.SVGigE_IOMUX_IN7;

        // STROBE0 signal from the camera (same as STROBE)
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_STROBE0 = SVGigE_IOMux_IN.SVGigE_IOMUX_IN6;

        // Strobe signal from the camera
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_STROBE = SVGigE_IOMux_IN.SVGigE_IOMUX_IN6;

        // Transmitter output from UART
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_UART_OUT = SVGigE_IOMux_IN.SVGigE_IOMUX_IN5;

        // Receiver IO line from camera connector
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_IO_RXD = SVGigE_IOMux_IN.SVGigE_IOMUX_IN4;


        // Fixed High signal 
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_FIXED_HIGH = SVGigE_IOMux_IN.SVGigE_IOMUX_IN31;
        // Fixed Low signal 
        public const SVGigE_IOMux_IN SVGigE_IOMux_IN_FIXED_LOW = SVGigE_IOMux_IN.SVGigE_IOMUX_IN30;

        /** Signal values for a particular signal
        */
        public enum SVGigE_IO_Signal : uint
        {
            IO_SIGNAL_OFF = 0,
            IO_SIGNAL_ON = 1
        }


        /** IO multiplexer OUT signals (signal sinks).
        *  A camera provides for a flexible IO signal handling where one or
        *  multiple IN signals (signal sources) can be assigned to an OUT
        *  signal (signal sink). The following OUT signals are defined.
        */

        public enum SVGigE_IOMux_OUT : int
        {
            SVGigE_IOMUX_OUT0 = 0,
            SVGigE_IOMUX_OUT1 = 1,
            SVGigE_IOMUX_OUT2 = 2,
            SVGigE_IOMUX_OUT3 = 3,
            SVGigE_IOMUX_OUT4 = 4,
            SVGigE_IOMUX_OUT5 = 5,
            SVGigE_IOMUX_OUT6 = 6,
            SVGigE_IOMUX_OUT7 = 7,
            SVGigE_IOMUX_OUT8 = 8,
            SVGigE_IOMUX_OUT9 = 9,
            SVGigE_IOMUX_OUT10 = 10,
            SVGigE_IOMUX_OUT11 = 11,
            SVGigE_IOMUX_OUT12 = 12,
            SVGigE_IOMUX_OUT13 = 13,
            SVGigE_IOMUX_OUT14 = 14,
            SVGigE_IOMUX_OUT15 = 15,
            SVGigE_IOMUX_OUT16 = 16,
            SVGigE_IOMUX_OUT17 = 17,
            SVGigE_IOMUX_OUT18 = 18,
            SVGigE_IOMUX_OUT19 = 19,
            SVGigE_IOMUX_OUT20 = 20,
            SVGigE_IOMUX_OUT21 = 21,
            SVGigE_IOMUX_OUT22 = 22,
            SVGigE_IOMUX_OUT23 = 23,
            SVGigE_IOMUX_OUT24 = 24,
            SVGigE_IOMUX_OUT25 = 25,
            SVGigE_IOMUX_OUT26 = 26,
            SVGigE_IOMUX_OUT27 = 27,
            SVGigE_IOMUX_OUT28 = 28,
            SVGigE_IOMUX_OUT29 = 29,
            SVGigE_IOMUX_OUT30 = 30,
            SVGigE_IOMUX_OUT31 = 31,
        }

        /** Some of the multiplexer's OUT signals (signal sinks) have a
        *  pre-defined usage:
        */

        // LogicB signal to camera
        public const uint SVGigE_IOMux_OUT_LOGICB = (int)(SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT11);

        // LogicA signal to camera
        public const uint SVGigE_IOMux_OUT_LOGICA = (int)(SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT10);

        // Prescaller signal to camera
        public const uint SVGigE_IOMux_OUT_PRESCALE = (int)(SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT9);

        // Debouncer signal to camera
        public const uint SVGigE_IOMux_OUT_DEBOUNCER = (int)(SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT8);

        // Trigger signal to camera
        public const uint SVGigE_IOMux_OUT_TRIGGER = (int)(SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT6);

        // Receiver input to UART
        public const uint SVGigE_IOMux_OUT_UART_IN = (int)(SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT5);

        // Transmitter IO line from camera connector
        public const uint SVGigE_IOMux_OUT_IO_TXD = (int)(SVGigE_IOMux_OUT.SVGigE_IOMUX_OUT4);

      
        //------------------------------------------------------------------------------
        //------------------------------------------------------------------------------

        /** Baud rate for a camera's UART
        *  A camera's UART can be set to the following pre-defined baud rates.
        */
        public enum SVGigE_BaudRate : uint
        {
            SVGigE_BaudRate_110 = 110,
            SVGigE_BaudRate_300 = 300,
            SVGigE_BaudRate_1200 = 1200,
            SVGigE_BaudRate_2400 = 2400,
            SVGigE_BaudRate_4800 = 4800,
            SVGigE_BaudRate_9600 = 9600,
            SVGigE_BaudRate_19200 = 19200,
            SVGigE_BaudRate_38400 = 38400,
            SVGigE_BaudRate_57600 = 57600,
            SVGigE_BaudRate_115200 = 115200,
        }

        public static int callbackCount = 0;
       

        public GigeApi()
        {
			//
			// TODO: Fügen Sie hier die Konstruktorlogik hinzu
			//
		} 


        

        //------------------------------------------------------------------------------
        // 0 - GigE DLL and driver
        //------------------------------------------------------------------------------

        /** isVersionCompliant.
        *  The DLL's version at compile time will be checked against an expected
        *  version at runtime. The calling program knows the runtime version that
        *  it needs for proper functioning and can handle a version mismatch, e.g.
        *  by displaying the expected and the found DLL version to the user.
        *
        *  NOTE: If the DLL version matches expected version, then subsequently a
        *        check for driver availability will be performed. The result may be:
        *          - "not installed" (if no SVGigE driver could be found)
        *          - "not available" (if a driver is installed but communication failed, e.g. x86 on x64)
        *
        *  @param DllVersion a pointer to a version structure for the current DLL version
        *  @param ExpectedVersion a pointer to a version structure with the expected DLL version
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        /** isDriverAvailable.
        *  It will be checked whether a FilterDriver is available. The following 	return codes apply:
        *    - "success" - driver is installed and available for work
        *    - "TL not loaded" - a FilterDriver transport layer is not available
        *    - "not installed" - no FilterDriver installed
        *    - "not available" - FilterDriver installed but not available (e.g. x86 on x64)
        *
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE 	return code
        */
        [DllImport(SVGigE_DLL64, EntryPoint = "isDriverAvailable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            isDriverAvailable64();

        [DllImport(SVGigE_DLL, EntryPoint = "isDriverAvailable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            isDriverAvailable32();
        public static unsafe
            SVSGigeApiReturn
            isDriverAvailable()
        {
            return IntPtr.Size == 8 /* 64bit */ ? isDriverAvailable64() : isDriverAvailable32();
        }


        //------------------------------------------------------------------------------
        // 1 - Camera: Discovery and bookkeeping
        //------------------------------------------------------------------------------	

        [DllImport(SVGigE_DLL64, EntryPoint = "CameraContainer_create", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        CameraContainer_create64(SVGigETL_Type TransPortLayerType);

        [DllImport(SVGigE_DLL, EntryPoint = "CameraContainer_create", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        CameraContainer_create32(SVGigETL_Type TransPortLayerType);

        private static unsafe
        int
        CameraContainer_create(SVGigETL_Type TransPortLayerType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? CameraContainer_create64(TransPortLayerType) : CameraContainer_create32(TransPortLayerType);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "CameraContainer_delete", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        CameraContainer_delete64(int hCameraContainer);

        [DllImport(SVGigE_DLL, EntryPoint = "CameraContainer_delete", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        CameraContainer_delete32(int hCameraContainer);

        public static unsafe
        SVSGigeApiReturn 
        CameraContainer_delete(int hCameraContainer)
        {
           return IntPtr.Size == 8 /* 64bit */ ? CameraContainer_delete64(hCameraContainer) : CameraContainer_delete32(hCameraContainer);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "CameraContainer_discovery", CallingConvention = CallingConvention.Cdecl)]  
        private static extern unsafe
        SVSGigeApiReturn
        CameraContainer_discovery64(int hCameraContainer);

        [DllImport(SVGigE_DLL,EntryPoint = "CameraContainer_discovery", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        CameraContainer_discovery32(int hCameraContainer);

        public static unsafe 
        SVSGigeApiReturn CameraContainer_discovery(int hCameraContainer)
        {
            return IntPtr.Size == 8 /* 64bit */ ? CameraContainer_discovery64(hCameraContainer) : CameraContainer_discovery32(hCameraContainer);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "CameraContainer_getNumberOfCameras", CallingConvention = CallingConvention.Cdecl)]  
        private static extern unsafe
        int
        CameraContainer_getNumberOfCameras64(int hCameraContainer);

        [DllImport(SVGigE_DLL, EntryPoint = "CameraContainer_getNumberOfCameras", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        CameraContainer_getNumberOfCameras32(int hCameraContainer);

        public static unsafe int CameraContainer_getNumberOfCameras(int hCameraContainer)
        {
            return IntPtr.Size == 8 /* 64bit */ ? CameraContainer_getNumberOfCameras64(hCameraContainer) : CameraContainer_getNumberOfCameras32(hCameraContainer);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "CameraContainer_getCamera", CallingConvention = CallingConvention.Cdecl)]  
        private static extern unsafe
        void*
        CameraContainer_getCamera64(int hCameraContainer, int CameraIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "CameraContainer_getCamera", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        void*
        CameraContainer_getCamera32(int hCameraContainer, int CameraIndex);

        public static unsafe 
        void*
        CameraContainer_getCamera(int hCameraContainer, int CameraIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? CameraContainer_getCamera64(hCameraContainer, CameraIndex) : CameraContainer_getCamera32(hCameraContainer, CameraIndex);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "CameraContainer_findCamera", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        void* 
        CameraContainer_findCamera64(int hCameraContainer, sbyte* CameraItem);

        [DllImport(SVGigE_DLL, EntryPoint = "CameraContainer_findCamera", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        void* 
        CameraContainer_findCamera32(int hCameraContainer, sbyte* CameraItem);

        public static unsafe 
        void* 
        CameraContainer_findCamera(int hCameraContainer, sbyte* CameraItem)
        {
            return IntPtr.Size == 8 /* 64bit */ ? CameraContainer_findCamera64(hCameraContainer, CameraItem) : CameraContainer_findCamera32(hCameraContainer, CameraItem);
        }

        //------------------------------------------------------------------------------
        // 2 - Camera: Connection
        //------------------------------------------------------------------------------	
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_openConnection", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        SVSGigeApiReturn
        Camera_openConnection64(void* hCamera, float Timeout);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_openConnection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_openConnection32(void* hCamera, float Timeout);

        public static unsafe
        SVSGigeApiReturn
        Camera_openConnection(void* hCamera, float Timeout)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_openConnection64(hCamera, Timeout) : Camera_openConnection32(hCamera,Timeout);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_openConnectionEx", CallingConvention = CallingConvention.Cdecl)]  
        private static extern unsafe
        SVSGigeApiReturn
        Camera_openConnectionEx64(void* hCamera, float HeartbeatTimeout, int GVCPRetryCount, int GVCPTimeout);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_openConnectionEx", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_openConnectionEx32(void* hCamera, float HeartbeatTimeout, int GVCPRetryCount, int GVCPTimeout);

        public static unsafe
        SVSGigeApiReturn
        Camera_openConnectionEx(void* hCamera, float HeartbeatTimeout, int GVCPRetryCount, int GVCPTimeout)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_openConnectionEx64(hCamera, HeartbeatTimeout, GVCPRetryCount, GVCPTimeout) : Camera_openConnectionEx32(hCamera,HeartbeatTimeout, GVCPRetryCount, GVCPTimeout);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_closeConnection", CallingConvention = CallingConvention.Cdecl)]  
        private static extern unsafe
        SVSGigeApiReturn
        Camera_closeConnection64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_closeConnection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_closeConnection32(void* hCamera);

        public static unsafe
        SVSGigeApiReturn
        Camera_closeConnection(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_closeConnection64(hCamera) : Camera_closeConnection32(hCamera);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setIPAddress", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIPAddress64(void* hCamera, uint IPAddress, uint NetMask);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setIPAddress", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIPAddress32(void* hCamera, uint IPAddress, uint NetMask);

        public static unsafe
        SVSGigeApiReturn
        Camera_setIPAddress(void* hCamera, uint IPAddress, uint NetMask)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setIPAddress64(hCamera, IPAddress, NetMask) : Camera_setIPAddress32(hCamera, IPAddress, NetMask);
        }
        
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_forceValidNetworkSettings", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_forceValidNetworkSettings64(void* hCamera, uint* IPAddress, uint* SubnetMask);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_forceValidNetworkSettings", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_forceValidNetworkSettings32(void* hCamera, uint* IPAddress, uint* SubnetMask);

        public static unsafe
        SVSGigeApiReturn
        Camera_forceValidNetworkSettings(void* hCamera, uint* IPAddress, uint* SubnetMask)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_forceValidNetworkSettings64(hCamera,  IPAddress,  SubnetMask) : Camera_forceValidNetworkSettings32(hCamera,  IPAddress,  SubnetMask);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_restartIPConfiguration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_restartIPConfiguration64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_restartIPConfiguration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_restartIPConfiguration32(void * hCamera);

        public static unsafe
        SVSGigeApiReturn
        Camera_restartIPConfiguration(void * hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_restartIPConfiguration64(hCamera): Camera_restartIPConfiguration32(hCamera);
        }

        
        //------------------------------------------------------------------------------
        // 3 - Camera: Information
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getManufacturerName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getManufacturerName64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getManufacturerName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getManufacturerName32(void* hCamera);

        public static unsafe
        sbyte*
        Camera_getManufacturerName(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getManufacturerName64(hCamera) : Camera_getManufacturerName32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getModelName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getModelName64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getModelName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getModelName32(void* hCamera);

        public static unsafe
        sbyte*
        Camera_getModelName(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ?Camera_getModelName64(hCamera) : Camera_getModelName32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getDeviceVersion", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getDeviceVersion64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getDeviceVersion", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getDeviceVersion32(void* hCamera);

        public static unsafe
        sbyte*
        Camera_getDeviceVersion(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getDeviceVersion64(hCamera): Camera_getDeviceVersion32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getManufacturerSpecificInformation", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getManufacturerSpecificInformation64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getManufacturerSpecificInformation", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getManufacturerSpecificInformation32(void* hCamera);

        public static unsafe
        sbyte*
        Camera_getManufacturerSpecificInformation(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getManufacturerSpecificInformation64(hCamera) : Camera_getManufacturerSpecificInformation32(hCamera);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getSerialNumber", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getSerialNumber64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getSerialNumber", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getSerialNumber32(void* hCamera);
        public static unsafe
        sbyte*
        Camera_getSerialNumber(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getSerialNumber64(hCamera) :Camera_getSerialNumber32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setUserDefinedName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setUserDefinedName64(void* hCamera, sbyte* UserDefinedName);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setUserDefinedName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setUserDefinedName32(void* hCamera, sbyte* UserDefinedName);

        public static unsafe
        SVSGigeApiReturn
        Camera_setUserDefinedName(void* hCamera, sbyte* UserDefinedName)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setUserDefinedName64(hCamera, UserDefinedName) : Camera_setUserDefinedName32(hCamera,UserDefinedName);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getUserDefinedName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getUserDefinedName64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getUserDefinedName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getUserDefinedName32(void* hCamera);

        public static unsafe
        sbyte*
        Camera_getUserDefinedName(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getUserDefinedName64(hCamera) :Camera_getUserDefinedName32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getMacAddress", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getMacAddress64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getMacAddress", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getMacAddress32(void* hCamera);
        public static unsafe
        sbyte*
        Camera_getMacAddress(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getMacAddress64(hCamera) : Camera_getMacAddress32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getIPAddress", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getIPAddress64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getIPAddress", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getIPAddress32(void* hCamera);

        public static unsafe
        sbyte*
        Camera_getIPAddress(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getIPAddress64(hCamera) : Camera_getIPAddress32(hCamera);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getSubnetMask", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getSubnetMask64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getSubnetMask", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getSubnetMask32(void* hCamera);

        public static unsafe
        sbyte*
        Camera_getSubnetMask(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getSubnetMask64(hCamera) : Camera_getSubnetMask32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelClock", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelClock64(void* hCamera, int* PixelClock);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelClock", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelClock32(void* hCamera, int* PixelClock);
        
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelClock(void* hCamera, int* PixelClock)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelClock64(hCamera, PixelClock) : Camera_getPixelClock32(hCamera, PixelClock);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_isCameraFeature", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        bool
        Camera_isCameraFeature64(void* hCamera, CAMERA_FEATURE Feature);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_isCameraFeature", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe 
        bool
        Camera_isCameraFeature32(void* hCamera, CAMERA_FEATURE Feature);

        public static unsafe
        bool
        Camera_isCameraFeature(void* hCamera, CAMERA_FEATURE Feature)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_isCameraFeature64(hCamera, Feature) : Camera_isCameraFeature32(hCamera,Feature);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_readXMLFile", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_readXMLFile64(void* hCamera, sbyte** XML);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_readXMLFile", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_readXMLFile32(void* hCamera, sbyte** XML);
        public static unsafe
        SVSGigeApiReturn
        Camera_readXMLFile(void* hCamera, sbyte** XML)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_readXMLFile64( hCamera, XML) : Camera_readXMLFile32( hCamera, XML);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getSensorTemperature", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getSensorTemperature64(void* hCamera, uint* SensorTemperature);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getSensorTemperature", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getSensorTemperature32(void* hCamera, uint* SensorTemperature);

        public static unsafe
        SVSGigeApiReturn
        Camera_getSensorTemperature(void* hCamera, uint* SensorTemperature)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getSensorTemperature64( hCamera, SensorTemperature) : Camera_getSensorTemperature32(hCamera,  SensorTemperature);
        }


        //------------------------------------------------------------------------------
        // 4 - Stream: Channel creation and control
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_create", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_create64(void* hStreamingChannel, int hCameraContainer, void* hCamera, int BufferCount, StreamCallback CallbackFunction, void* Context);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_create", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_create32(void* hStreamingChannel, int hCameraContainer, void* hCamera, int BufferCount, StreamCallback CallbackFunction, void* Context);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_create(void* hStreamingChannel, int hCameraContainer, void* hCamera, int BufferCount, StreamCallback CallbackFunction, void* Context)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_create64(hStreamingChannel, hCameraContainer, hCamera, BufferCount, CallbackFunction, Context)
                                                : StreamingChannel_create32(hStreamingChannel, hCameraContainer, hCamera, BufferCount, CallbackFunction, Context);
        }




        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_createEx", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_createEx64(void* hStreamingChannel, int hCameraContainer, void* hCamera, int BufferCount, int PacketResendTimeout, StreamCallback CallbackFunction, void* Context);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_createEx", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_createEx32(void* hStreamingChannel, int hCameraContainer, void* hCamera, int BufferCount, int PacketResendTimeout, StreamCallback CallbackFunction, void* Context);
        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_createEx(void* hStreamingChannel, int hCameraContainer, void* hCamera, int BufferCount, int PacketResendTimeout, StreamCallback CallbackFunction, void* Context)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_createEx64(hStreamingChannel, hCameraContainer, hCamera, BufferCount, PacketResendTimeout, CallbackFunction, Context)
                                                : StreamingChannel_createEx32(hStreamingChannel, hCameraContainer, hCamera, BufferCount, PacketResendTimeout, CallbackFunction, Context);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_delete", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_delete64(void* hStreamingChannel);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_delete", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_delete32(void* hStreamingChannel);
        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_delete(void* hStreamingChannel)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_delete64(hStreamingChannel) : StreamingChannel_delete32(hStreamingChannel);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_setReadoutTransfer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_setReadoutTransfer64(void* hStreamingChannel, bool isReadoutTransfer);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_setReadoutTransfer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_setReadoutTransfer32(void* hStreamingChannel, bool isReadoutTransfer);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_setReadoutTransfer(void* hStreamingChannel, bool isReadoutTransfer)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_setReadoutTransfer64(hStreamingChannel, isReadoutTransfer) : StreamingChannel_setReadoutTransfer32(hStreamingChannel, isReadoutTransfer);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getReadoutTransfer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getReadoutTransfer64(void* hStreamingChannel, bool* isReadoutTransfer);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getReadoutTransfer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getReadoutTransfer32(void* hStreamingChannel, bool* isReadoutTransfer);
        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getReadoutTransfer(void* hStreamingChannel, bool* isReadoutTransfer)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getReadoutTransfer64( hStreamingChannel,  isReadoutTransfer) : StreamingChannel_getReadoutTransfer32( hStreamingChannel, isReadoutTransfer);
        }

        //------------------------------------------------------------------------------
        // 5 - Stream: Channel statistics
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getFrameLoss", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getFrameLoss64(void* hStreamingChannel, int* FrameLoss);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getFrameLoss", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getFrameLoss32(void* hStreamingChannel, int* FrameLoss);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getFrameLoss(void* hStreamingChannel, int* FrameLoss)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getFrameLoss64(hStreamingChannel, FrameLoss) : StreamingChannel_getFrameLoss32(hStreamingChannel, FrameLoss);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getActualFrameRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getActualFrameRate64(void* hStreamingChannel, float* ActualFrameRate);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getActualFrameRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getActualFrameRate32(void* hStreamingChannel, float* ActualFrameRate);
        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getActualFrameRate(void* hStreamingChannel, float* ActualFrameRate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getActualFrameRate64(hStreamingChannel, ActualFrameRate) : StreamingChannel_getActualFrameRate32(hStreamingChannel, ActualFrameRate);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getActualDataRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getActualDataRate64(void* hStreamingChannel, float* ActualDataRate);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getActualDataRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getActualDataRate32(void* hStreamingChannel, float* ActualDataRate);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getActualDataRate(void* hStreamingChannel, float* ActualDataRate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getActualDataRate64(hStreamingChannel, ActualDataRate) : StreamingChannel_getActualDataRate32(hStreamingChannel, ActualDataRate);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getPeakDataRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getPeakDataRate64(void* hStreamingChannel, float* PeakDataRate);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getPeakDataRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getPeakDataRate32(void* hStreamingChannel, float* PeakDataRate);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getPeakDataRate(void* hStreamingChannel, float* PeakDataRate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getPeakDataRate64(hStreamingChannel, PeakDataRate) : StreamingChannel_getPeakDataRate32(hStreamingChannel, PeakDataRate);
        }
        //------------------------------------------------------------------------------
        // 6 - Stream: Channel info 
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getPixelType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getPixelType64(void* hStreamingChannel, GVSP_PIXEL_TYPE* PixelType);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getPixelType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getPixelType32(void* hStreamingChannel, GVSP_PIXEL_TYPE* PixelType);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getPixelType(void* hStreamingChannel, GVSP_PIXEL_TYPE* PixelType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getPixelType64(hStreamingChannel,  PixelType) :StreamingChannel_getPixelType32(hStreamingChannel, PixelType);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getBufferData", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getBufferData64(void* hStreamingChannel, uint BufferIndex, sbyte** BufferData);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getBufferData", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getBufferData32(void* hStreamingChannel, uint BufferIndex, sbyte** BufferData);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getBufferData(void* hStreamingChannel, uint BufferIndex, sbyte** BufferData)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getBufferData64(hStreamingChannel, BufferIndex, BufferData) : StreamingChannel_getBufferData32(hStreamingChannel, BufferIndex, BufferData);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getBufferSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getBufferSize64(void* hStreamingChannel, int* BufferSize);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getBufferSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getBufferSize32(void* hStreamingChannel, int* BufferSize);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getBufferSize(void* hStreamingChannel, int* BufferSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getBufferSize64(hStreamingChannel, BufferSize) : StreamingChannel_getBufferSize32(hStreamingChannel, BufferSize);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getImagePitch", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getImagePitch64(void* hStreamingChannel, int* ImagePitch);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getImagePitch", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getImagePitch32(void* hStreamingChannel, int* ImagePitch);
        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getImagePitch(void* hStreamingChannel, int* ImagePitch)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getImagePitch64( hStreamingChannel,  ImagePitch) : StreamingChannel_getImagePitch32( hStreamingChannel, ImagePitch);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getImageSizeX", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getImageSizeX64(void* hStreamingChannel, int* ImageSizeX);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getImageSizeX", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getImageSizeX32(void* hStreamingChannel, int* ImageSizeX);

        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getImageSizeX(void* hStreamingChannel, int* ImageSizeX)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getImageSizeX64( hStreamingChannel, ImageSizeX) : StreamingChannel_getImageSizeX32(hStreamingChannel, ImageSizeX);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "StreamingChannel_getImageSizeY", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getImageSizeY64(void* hStreamingChannel, int* ImageSizeY);

        [DllImport(SVGigE_DLL, EntryPoint = "StreamingChannel_getImageSizeY", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        StreamingChannel_getImageSizeY32(void* hStreamingChannel, int* ImageSizeY);
        public static unsafe
        SVSGigeApiReturn
        StreamingChannel_getImageSizeY(void* hStreamingChannel, int* ImageSizeY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? StreamingChannel_getImageSizeY64( hStreamingChannel,  ImageSizeY) : StreamingChannel_getImageSizeY32(hStreamingChannel,  ImageSizeY);
        }


        //------------------------------------------------------------------------------
        // 7 - Stream: Transfer Parameters
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_evaluateMaximalPacketSize", CallingConvention = CallingConvention.Cdecl)]  
        private static extern unsafe
        SVSGigeApiReturn
        Camera_evaluateMaximalPacketSize64(void* hCamera, int* MaximalPacketSize);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_evaluateMaximalPacketSize", CallingConvention = CallingConvention.Cdecl)]  
        private static extern unsafe
        SVSGigeApiReturn
        Camera_evaluateMaximalPacketSize32(void* hCamera, int* MaximalPacketSize);
        public static unsafe
        SVSGigeApiReturn
        Camera_evaluateMaximalPacketSize(void* hCamera, int* MaximalPacketSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_evaluateMaximalPacketSize64( hCamera, MaximalPacketSize) : Camera_evaluateMaximalPacketSize32(hCamera, MaximalPacketSize);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setStreamingPacketSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStreamingPacketSize64(void* hCamera, int StreamingPacketSize);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setStreamingPacketSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStreamingPacketSize32(void* hCamera, int StreamingPacketSize);
        public static unsafe
        SVSGigeApiReturn
        Camera_setStreamingPacketSize(void* hCamera, int StreamingPacketSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setStreamingPacketSize64(hCamera, StreamingPacketSize) : Camera_setStreamingPacketSize32(hCamera, StreamingPacketSize);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setInterPacketDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setInterPacketDelay64(void* hCamera, float InterPacketDelay);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setInterPacketDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setInterPacketDelay32(void* hCamera, float InterPacketDelay);
        public static unsafe
        SVSGigeApiReturn
        Camera_setInterPacketDelay(void* hCamera, float InterPacketDelay)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setInterPacketDelay64(hCamera,InterPacketDelay): Camera_setInterPacketDelay32(hCamera, InterPacketDelay);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getInterPacketDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getInterPacketDelay64(void* hCamera, float* InterPacketDelay);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getInterPacketDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getInterPacketDelay32(void* hCamera, float* InterPacketDelay);
        public static unsafe
        SVSGigeApiReturn
        Camera_getInterPacketDelay(void* hCamera, float* InterPacketDelay)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getInterPacketDelay64( hCamera,  InterPacketDelay) : Camera_getInterPacketDelay32(hCamera,InterPacketDelay);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setMulticastMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setMulticastMode64(void* hCamera, MULTICAST_MODE MulticastMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setMulticastMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setMulticastMode32(void* hCamera, MULTICAST_MODE MulticastMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_setMulticastMode(void* hCamera, MULTICAST_MODE MulticastMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setMulticastMode64(hCamera, MulticastMode) : Camera_setMulticastMode32(hCamera, MulticastMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getMulticastMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMulticastMode64(void* hCamera, MULTICAST_MODE* MulticastMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getMulticastMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMulticastMode32(void* hCamera, MULTICAST_MODE* MulticastMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_getMulticastMode(void* hCamera, MULTICAST_MODE* MulticastMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getMulticastMode64( hCamera,  MulticastMode) : Camera_getMulticastMode32( hCamera, MulticastMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getMulticastGroup", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMulticastGroup64(void* hCamera, uint* MulticastGroup, uint* MulticastPort);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getMulticastGroup", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMulticastGroup32(void* hCamera, uint* MulticastGroup, uint* MulticastPort);
        public static unsafe
        SVSGigeApiReturn
        Camera_getMulticastGroup(void* hCamera, uint* MulticastGroup, uint* MulticastPort)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getMulticastGroup64(hCamera, MulticastGroup, MulticastPort) : Camera_getMulticastGroup32(hCamera, MulticastGroup, MulticastPort);
        }

        //------------------------------------------------------------------------------
        // 8 - Stream: Image access
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getDataPointer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            sbyte*
            Image_getDataPointer64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getDataPointer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            sbyte*
            Image_getDataPointer32(int hImage);
        public static unsafe
            sbyte*
            Image_getDataPointer(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getDataPointer64(hImage) : Image_getDataPointer32(hImage);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getBufferIndex", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            int Image_getBufferIndex64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getBufferIndex", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            int Image_getBufferIndex32(int hImage);
        public static unsafe
        int Image_getBufferIndex(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getBufferIndex64(hImage) : Image_getBufferIndex32(hImage);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getSignalType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVGigE_SIGNAL_TYPE Image_getSignalType64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getSignalType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVGigE_SIGNAL_TYPE Image_getSignalType32(int hImage);
        public static unsafe
        SVGigE_SIGNAL_TYPE Image_getSignalType(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getSignalType64(hImage) : Image_getSignalType32(hImage);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getCamera", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            int Image_getCamera64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getCamera", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            int Image_getCamera32(int hImage);
        public static unsafe
            int Image_getCamera(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getCamera64(hImage) : Image_getCamera32( hImage);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_release", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn  Image_release64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_release", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn  Image_release32(int hImage);
        public static unsafe
            SVSGigeApiReturn  Image_release(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_release64(hImage) : Image_release32(hImage);
        }


        //------------------------------------------------------------------------------
        // 9 - Stream: Image conversion
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getImageRGB", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Image_getImageRGB64(int hImage, byte* BufferRGB, int BufferLength, BAYER_METHOD BayerMethod);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getImageRGB", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Image_getImageRGB32(int hImage, byte* BufferRGB, int BufferLength, BAYER_METHOD BayerMethod);
        public static unsafe
        SVSGigeApiReturn Image_getImageRGB (int hImage, byte*BufferRGB, int BufferLength, BAYER_METHOD BayerMethod)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getImageRGB64(hImage, BufferRGB, BufferLength, BayerMethod) : Image_getImageRGB32(hImage, BufferRGB, BufferLength, BayerMethod);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getImage12bitAs8bit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImage12bitAs8bit64(int hImage, byte* Buffer8bit, int BufferLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getImage12bitAs8bit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImage12bitAs8bit32(int hImage, byte* Buffer8bit, int BufferLength);

        public static unsafe
        SVSGigeApiReturn
        Image_getImage12bitAs8bit(int hImage, byte* Buffer8bit, int BufferLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getImage12bitAs8bit64( hImage,  Buffer8bit,  BufferLength) : Image_getImage12bitAs8bit32(hImage, Buffer8bit, BufferLength);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getImage12bitAs16bit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImage12bitAs16bit64(int hImage, byte* Buffer16bit, int BufferLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getImage12bitAs16bit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImage12bitAs16bit32(int hImage, byte* Buffer16bit, int BufferLength);
        public static unsafe
        SVSGigeApiReturn
        Image_getImage12bitAs16bit(int hImage, byte* Buffer16bit, int BufferLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getImage12bitAs16bit64( hImage, Buffer16bit, BufferLength) : Image_getImage12bitAs16bit32( hImage, Buffer16bit, BufferLength);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getImage16bitAs8bit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImage16bitAs8bit64(int hImage, byte* Buffer8bit, int BufferLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getImage16bitAs8bit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImage16bitAs8bit32(int hImage, byte* Buffer8bit, int BufferLength);

        public static unsafe
        SVSGigeApiReturn
        Image_getImage16bitAs8bit(int hImage, byte* Buffer8bit, int BufferLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getImage16bitAs8bit64( hImage,  Buffer8bit,  BufferLength) : Image_getImage16bitAs8bit32( hImage,  Buffer8bit,  BufferLength);
        }


        //------------------------------------------------------------------------------
        // 10 - Stream: Image characteristics
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getPixelType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        GVSP_PIXEL_TYPE Image_getPixelType64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getPixelType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        GVSP_PIXEL_TYPE Image_getPixelType32(int hImage);

        public static unsafe
        GVSP_PIXEL_TYPE Image_getPixelType(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getPixelType64(hImage) : Image_getPixelType32(hImage);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getImageSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getImageSize64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getImageSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getImageSize32(int hImage);

        public static unsafe
        int
        Image_getImageSize(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getImageSize64(hImage) : Image_getImageSize32(hImage);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getPitch", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getPitch64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getPitch", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getPitch32(int hImage);

        public static unsafe
        int
        Image_getPitch(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ?Image_getPitch64( hImage) : Image_getPitch32(hImage);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getSizeX", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getSizeX64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getSizeX", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getSizeX32(int hImage);
        public static unsafe
        int
        Image_getSizeX(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getSizeX64(hImage) : Image_getSizeX32(hImage);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getSizeY", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getSizeY64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getSizeY", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getSizeY32(int hImage);
        public static unsafe
        int
        Image_getSizeY(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getSizeY64(hImage) : Image_getSizeY32(hImage);
        }

        //------------------------------------------------------------------------------
        // 11 - Stream: Image statistics
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getImageID", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getImageID64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getImageID", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int
        Image_getImageID32(int hImage);
        public static unsafe
        int
        Image_getImageID(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getImageID64( hImage) : Image_getImageID32(hImage);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        double
        Image_getTimestamp64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        double
        Image_getTimestamp32(int hImage);

        public static unsafe
        double
        Image_getTimestamp(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getTimestamp64(hImage): Image_getTimestamp32(hImage);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getTransferTime", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        double Image_getTransferTime64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getTransferTime", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        double Image_getTransferTime32(int hImage);

        public static unsafe
        double Image_getTransferTime(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getTransferTime64(hImage) : Image_getTransferTime32(hImage);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getPacketCount", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int Image_getPacketCount64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getPacketCount ", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        int Image_getPacketCount32(int hImage);

        public static unsafe
        int Image_getPacketCount(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getPacketCount64( hImage) : Image_getPacketCount32( hImage);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getPacketResend", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            int Image_getPacketResend64(int hImage);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getPacketResend", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            int Image_getPacketResend32(int hImage);

        public static unsafe
            int Image_getPacketResend(int hImage)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getPacketResend64(hImage) : Image_getPacketResend32(hImage);
        }


        //------------------------------------------------------------------------------
        // 12 - Stream: Messaging channel
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_createEvent", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_createEvent64(void* hStreamingChannel, void** EventID, int SizeFIFO);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_createEvent", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_createEvent32(void* hStreamingChannel, void** EventID, int SizeFIFO);
        public static unsafe
            SVSGigeApiReturn
            Stream_createEvent(void* hStreamingChannel, void** EventID, int SizeFIFO)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_createEvent64(hStreamingChannel, EventID, SizeFIFO) : Stream_createEvent32(hStreamingChannel, EventID, SizeFIFO);
        }




        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_addMessageType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_addMessageType64(void* hStreamingChannel, void* EventID, SVGigE_SIGNAL_TYPE MessageType);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_addMessageType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_addMessageType32(void* hStreamingChannel, void* EventID, SVGigE_SIGNAL_TYPE MessageType);

        public static unsafe
            SVSGigeApiReturn
            Stream_addMessageType(void* hStreamingChannel, void* EventID, SVGigE_SIGNAL_TYPE MessageType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_addMessageType64(hStreamingChannel, EventID, MessageType) : Stream_addMessageType32(hStreamingChannel, EventID, MessageType);
        }




        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_removeMessageType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_removeMessageType64(void* hStreamingChannel, void* EventID, SVGigE_SIGNAL_TYPE MessageType);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_removeMessageType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_removeMessageType32(void* hStreamingChannel, void* EventID, SVGigE_SIGNAL_TYPE MessageType);

        public static unsafe
                SVSGigeApiReturn
            Stream_removeMessageType(void* hStreamingChannel, void* EventID, SVGigE_SIGNAL_TYPE MessageType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_removeMessageType64( hStreamingChannel,  EventID,  MessageType) : Stream_removeMessageType32( hStreamingChannel, EventID, MessageType);
        }




        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_isMessagePending", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_isMessagePending64(void* hStreamingChannel, void* EventID, int Timeout_ms);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_isMessagePending", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_isMessagePending32(void* hStreamingChannel, void* EventID, int Timeout_ms);
        public static unsafe
            SVSGigeApiReturn
            Stream_isMessagePending(void* hStreamingChannel, void* EventID, int Timeout_ms)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_isMessagePending64(hStreamingChannel,  EventID,  Timeout_ms): Stream_isMessagePending32( hStreamingChannel,  EventID,  Timeout_ms);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_registerEventCallback", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_registerEventCallback64(void* hStreamingChannel, void* EventID, EventCallback Callback, void* Context);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_registerEventCallback", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_registerEventCallback32(void* hStreamingChannel, void* EventID, EventCallback Callback, void* Context);

        public static unsafe
            SVSGigeApiReturn
            Stream_registerEventCallback(void* hStreamingChannel, void* EventID, EventCallback Callback, void* Context)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_registerEventCallback64(hStreamingChannel, EventID, Callback, Context) : Stream_registerEventCallback32(hStreamingChannel, EventID, Callback, Context);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_unregisterEventCallback", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_unregisterEventCallback64(void* hStreamingChannel, void* EventID, EventCallback Callback);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_unregisterEventCallback", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_unregisterEventCallback32(void* hStreamingChannel, void* EventID, EventCallback Callback);
        public static unsafe
            SVSGigeApiReturn
            Stream_unregisterEventCallback(void* hStreamingChannel, void* EventID, EventCallback Callback)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_unregisterEventCallback64(hStreamingChannel, EventID, Callback) : Stream_unregisterEventCallback32(hStreamingChannel, EventID, Callback);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_getMessage", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_getMessage64(void* hStreamingChannel, void* EventID, int* MessageID, SVGigE_SIGNAL_TYPE* MessageType);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_getMessage", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_getMessage32(void* hStreamingChannel, void* EventID, int* MessageID, SVGigE_SIGNAL_TYPE* MessageType);
        public static unsafe
            SVSGigeApiReturn
            Stream_getMessage(void* hStreamingChannel, void* EventID, int* MessageID, SVGigE_SIGNAL_TYPE* MessageType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_getMessage64(hStreamingChannel, EventID, MessageID, MessageType): Stream_getMessage32( hStreamingChannel, EventID, MessageID, MessageType);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_getMessageData", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_getMessageData64(void* hStreamingChannel, void* EventID, int MessageID, void** MessageData, int* MessageLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_getMessageData", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_getMessageData32(void* hStreamingChannel, void* EventID, int MessageID, void** MessageData, int* MessageLength);
        public static unsafe
            SVSGigeApiReturn
            Stream_getMessageData(void* hStreamingChannel, void* EventID, int MessageID, void** MessageData, int* MessageLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_getMessageData64(hStreamingChannel, EventID, MessageID, MessageData, MessageLength) : Stream_getMessageData32(hStreamingChannel, EventID, MessageID, MessageData, MessageLength);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_getMessageTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_getMessageTimestamp64(void* hStreamingChannel, void* EventID, int MessageID, double* MessageTimestamp);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_getMessageTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_getMessageTimestamp32(void* hStreamingChannel, void* EventID, int MessageID, double* MessageTimestamp);
        public static unsafe
            SVSGigeApiReturn
            Stream_getMessageTimestamp(void* hStreamingChannel, void* EventID, int MessageID, double* MessageTimestamp)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_getMessageTimestamp64( hStreamingChannel,  EventID,  MessageID,  MessageTimestamp): Stream_getMessageTimestamp32( hStreamingChannel,  EventID,  MessageID,  MessageTimestamp);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_releaseMessage", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_releaseMessage64(void* hStreamingChannel, void* EventID, int MessageID);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_releaseMessage", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_releaseMessage32(void* hStreamingChannel, void* EventID, int MessageID);

        public static unsafe
            SVSGigeApiReturn
            Stream_releaseMessage(void* hStreamingChannel, void* EventID, int MessageID)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_releaseMessage( hStreamingChannel,  EventID,  MessageID) : Stream_releaseMessage( hStreamingChannel,  EventID, MessageID);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_flushMessages", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_flushMessages64(void* hStreamingChannel, void* EventID);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_flushMessages", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_flushMessages32(void* hStreamingChannel, void* EventID);

        public static unsafe
            SVSGigeApiReturn
            Stream_flushMessages(void* hStreamingChannel, void* EventID)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_flushMessages64( hStreamingChannel,  EventID) : Stream_flushMessages32( hStreamingChannel, EventID);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Stream_closeEvent", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_closeEvent64(void* hStreamingChannel, void* EventID);

        [DllImport(SVGigE_DLL, EntryPoint = "Stream_closeEvent", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Stream_closeEvent32(void* hStreamingChannel, void* EventID);
        public static unsafe
            SVSGigeApiReturn
            Stream_closeEvent(void* hStreamingChannel, void* EventID)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Stream_closeEvent64( hStreamingChannel,  EventID) : Stream_closeEvent32( hStreamingChannel, EventID);
        }


        //------------------------------------------------------------------------------
        // 13 - Controlling camera : Frame rate
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setFrameRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setFrameRate64(void* hCamera, float Framerate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setFrameRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setFrameRate32(void* hCamera, float Framerate);
        public static unsafe
            SVSGigeApiReturn
            Camera_setFrameRate(void* hCamera, float Framerate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setFrameRate64( hCamera, Framerate) : Camera_setFrameRate32( hCamera, Framerate);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getFrameRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getFrameRate64(void* hCamera, float* Framerate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getFrameRate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getFrameRate32(void* hCamera, float* Framerate);
        public static unsafe
            SVSGigeApiReturn
            Camera_getFrameRate(void* hCamera, float* Framerate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getFrameRate64( hCamera, Framerate) : Camera_getFrameRate32(hCamera, Framerate);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getFrameRateMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_getFrameRateMin64(void* hCamera, float* MinFramerate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getFrameRateMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_getFrameRateMin32(void* hCamera, float* MinFramerate);
        public static unsafe
            SVSGigeApiReturn Camera_getFrameRateMin(void* hCamera, float* MinFramerate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getFrameRateMin64( hCamera,  MinFramerate) :Camera_getFrameRateMin32( hCamera,  MinFramerate);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getFrameRateMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_getFrameRateMax64(void* hCamera, float* MaxFramerate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getFrameRateMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_getFrameRateMax32(void* hCamera, float* MaxFramerate);
        public static unsafe
            SVSGigeApiReturn Camera_getFrameRateMax(void* hCamera, float* MaxFramerate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getFrameRateMax64(hCamera, MaxFramerate) : Camera_getFrameRateMax32(hCamera, MaxFramerate);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getFrameRateRange", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getFrameRateRange64(void* hCamera, float* MinFramerate, float* MaxFramerate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getFrameRateRange", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getFrameRateRange32(void* hCamera, float* MinFramerate, float* MaxFramerate);
        public static unsafe
            SVSGigeApiReturn
            Camera_getFrameRateRange(void* hCamera, float* MinFramerate, float* MaxFramerate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getFrameRateRange64( hCamera,  MinFramerate, MaxFramerate) : Camera_getFrameRateRange32( hCamera,  MinFramerate, MaxFramerate);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getFrameRateIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getFrameRateIncrement64(void* hCamera, float* FramerateIncrement);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getFrameRateIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getFrameRateIncrement32(void* hCamera, float* FramerateIncrement);
        public static unsafe
            SVSGigeApiReturn
            Camera_getFrameRateIncrement(void* hCamera, float* FramerateIncrement)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getFrameRateIncrement64( hCamera, FramerateIncrement) : Camera_getFrameRateIncrement32( hCamera,  FramerateIncrement);
        }


        //------------------------------------------------------------------------------
        // 14 - Controlling camera : Exposure
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setExposureTime", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setExposureTime64(void* hCamera, float ExposureTime);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setExposureTime", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setExposureTime32(void* hCamera, float ExposureTime);
        public static unsafe
            SVSGigeApiReturn
            Camera_setExposureTime(void* hCamera, float ExposureTime)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setExposureTime64( hCamera,  ExposureTime) : Camera_setExposureTime32( hCamera,  ExposureTime);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureTime", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTime64(void* hCamera, float* ExposureTime);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureTime", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTime32(void* hCamera, float* ExposureTime);
        public static unsafe
            SVSGigeApiReturn
            Camera_getExposureTime(void* hCamera, float* ExposureTime)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureTime64( hCamera,  ExposureTime) : Camera_getExposureTime32( hCamera, ExposureTime);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureTimeMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeMin64(void* hCamera, float* MinExposureTime);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureTimeMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeMin32(void* hCamera, float* MinExposureTime);
        public static unsafe
              SVSGigeApiReturn
            Camera_getExposureTimeMin(void* hCamera, float* MinExposureTime)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureTimeMin64( hCamera, MinExposureTime) : Camera_getExposureTimeMin32( hCamera, MinExposureTime);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureTimeMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeMax64(void* hCamera, float* MaxExposureTime);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureTimeMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeMax32(void* hCamera, float* MaxExposureTime);
        public static unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeMax(void* hCamera, float* MaxExposureTime)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureTimeMax64( hCamera, MaxExposureTime) : Camera_getExposureTimeMax32( hCamera, MaxExposureTime);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureTimeRange", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeRange64(void* hCamera, float* MinExposureTime, float* MaxExposureTime);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureTimeRange", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeRange32(void* hCamera, float* MinExposureTime, float* MaxExposureTime);
        public static unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeRange(void* hCamera, float* MinExposureTime, float* MaxExposureTime)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureTimeRange64( hCamera,  MinExposureTime, MaxExposureTime) : Camera_getExposureTimeRange32( hCamera, MinExposureTime, MaxExposureTime);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureTimeIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeIncrement64(void* hCamera, float* ExposureTimeIncrement);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureTimeIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeIncrement32(void* hCamera, float* ExposureTimeIncrement);
        public static unsafe
            SVSGigeApiReturn
            Camera_getExposureTimeIncrement(void* hCamera, float* ExposureTimeIncrement)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureTimeIncrement64( hCamera,  ExposureTimeIncrement) : Camera_getExposureTimeIncrement32( hCamera, ExposureTimeIncrement);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setExposureDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setExposureDelay64(void* hCamera, float ExposureDelay);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setExposureDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setExposureDelay32(void* hCamera, float ExposureDelay);
        public static unsafe
             SVSGigeApiReturn
            Camera_setExposureDelay(void* hCamera, float ExposureDelay)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setExposureDelay64( hCamera, ExposureDelay): Camera_setExposureDelay32( hCamera, ExposureDelay);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureDelay64(void* hCamera, float* ExposureDelay);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureDelay", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureDelay32(void* hCamera, float* ExposureDelay);
        public static unsafe
            SVSGigeApiReturn
            Camera_getExposureDelay(void* hCamera, float* ExposureDelay)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureDelay64(hCamera, ExposureDelay) : Camera_getExposureDelay32(hCamera, ExposureDelay);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureDelayMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureDelayMax64(void* hCamera, float* MaxExposureDelay);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureDelayMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureDelayMax32(void* hCamera, float* MaxExposureDelay);
        public static unsafe
            SVSGigeApiReturn
            Camera_getExposureDelayMax(void* hCamera, float* MaxExposureDelay)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureDelayMax64(hCamera, MaxExposureDelay) : Camera_getExposureDelayMax32(hCamera, MaxExposureDelay);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getExposureDelayIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureDelayIncrement64(void* hCamera, float* ExposureDelayIncrement);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getExposureDelayIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getExposureDelayIncrement32(void* hCamera, float* ExposureDelayIncrement);
        public static unsafe
            SVSGigeApiReturn
            Camera_getExposureDelayIncrement(void* hCamera, float* ExposureDelayIncrement)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getExposureDelayIncrement64( hCamera, ExposureDelayIncrement) : Camera_getExposureDelayIncrement32( hCamera,  ExposureDelayIncrement);
        }

        //------------------------------------------------------------------------------
        // 15 - Controlling camera : Gain and offset
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setGain", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setGain64(void* hCamera, float Gain);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setGain", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setGain32(void* hCamera, float Gain);
        public static unsafe
            SVSGigeApiReturn
            Camera_setGain(void* hCamera, float Gain)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setGain64( hCamera, Gain) : Camera_setGain32( hCamera, Gain);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getGain", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGain64(void* hCamera, float* Gain);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getGain", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGain32(void* hCamera, float* Gain);
        public static unsafe
            SVSGigeApiReturn
            Camera_getGain(void* hCamera, float* Gain)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getGain64( hCamera, Gain) :Camera_getGain32( hCamera, Gain);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getGainMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGainMax64(void* hCamera, float* MaxGain);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getGainMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGainMax32(void* hCamera, float* MaxGain);
        public static unsafe
            SVSGigeApiReturn
            Camera_getGainMax(void* hCamera, float* MaxGain)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getGainMax64( hCamera, MaxGain) : Camera_getGainMax32( hCamera, MaxGain);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getGainMaxExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGainMaxExtended64(void* hCamera, float* MaxGainExtended);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getGainMaxExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGainMaxExtended32(void* hCamera, float* MaxGainExtended);
        public static unsafe
            SVSGigeApiReturn
            Camera_getGainMaxExtended(void* hCamera, float* MaxGainExtended)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getGainMaxExtended64( hCamera,  MaxGainExtended) : Camera_getGainMaxExtended32( hCamera, MaxGainExtended);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getGainIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGainIncrement64(void* hCamera, float* GainIncrement);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getGainIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getGainIncrement32(void* hCamera, float* GainIncrement);
        public static unsafe
            SVSGigeApiReturn
            Camera_getGainIncrement(void* hCamera, float* GainIncrement)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getGainIncrement64( hCamera,  GainIncrement) : Camera_getGainIncrement32( hCamera, GainIncrement);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setOffset64(void* hCamera, float Offset);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setOffset32(void* hCamera, float Offset);
        public static unsafe
            SVSGigeApiReturn
            Camera_setOffset(void* hCamera, float Offset)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setOffset64( hCamera,  Offset) : Camera_setOffset32( hCamera, Offset);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getOffset64(void* hCamera, float* Offset);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getOffset32(void* hCamera, float* Offset);
        public static unsafe
            SVSGigeApiReturn
            Camera_getOffset(void* hCamera, float* Offset)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getOffset64( hCamera,  Offset) : Camera_getOffset32( hCamera, Offset);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getOffsetMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getOffsetMax64(void* hCamera, float* MaxOffset);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getOffsetMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getOffsetMax32(void* hCamera, float* MaxOffset);
        public static unsafe
            SVSGigeApiReturn
            Camera_getOffsetMax(void* hCamera, float* MaxOffset)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getOffsetMax64( hCamera,  MaxOffset) : Camera_getOffsetMax32( hCamera, MaxOffset);
        }

        //------------------------------------------------------------------------------
        // 16 - Controlling camera: Auto gain / exposure
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAutoGainEnabled", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAutoGainEnabled64(void* hCamera, bool isAutoGainEnabled);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAutoGainEnabled", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAutoGainEnabled32(void* hCamera, bool isAutoGainEnabled);
        public static unsafe
        SVSGigeApiReturn
        Camera_setAutoGainEnabled(void* hCamera, bool isAutoGainEnabled)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAutoGainEnabled64(hCamera, isAutoGainEnabled) : Camera_setAutoGainEnabled32( hCamera, isAutoGainEnabled);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAutoGainEnabled", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getAutoGainEnabled64(void* hCamera, bool* isAutoGainEnabled);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAutoGainEnabled", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getAutoGainEnabled32(void* hCamera, bool* isAutoGainEnabled);
        public static unsafe
             SVSGigeApiReturn
            Camera_getAutoGainEnabled(void* hCamera, bool* isAutoGainEnabled)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAutoGainEnabled64( hCamera, isAutoGainEnabled) : Camera_getAutoGainEnabled32( hCamera, isAutoGainEnabled);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAutoGainBrightness", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setAutoGainBrightness64(void* hCamera, float Brightness);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAutoGainBrightness", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setAutoGainBrightness32(void* hCamera, float Brightness);
        public static unsafe
            SVSGigeApiReturn
            Camera_setAutoGainBrightness(void* hCamera, float Brightness)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAutoGainBrightness64( hCamera,  Brightness) : Camera_setAutoGainBrightness32( hCamera,  Brightness);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAutoGainBrightness", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getAutoGainBrightness64(void* hCamera, float* Brightness);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAutoGainBrightness", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getAutoGainBrightness32(void* hCamera, float* Brightness);

        public static unsafe
             SVSGigeApiReturn
            Camera_getAutoGainBrightness(void* hCamera, float* Brightness)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAutoGainBrightness64( hCamera,  Brightness) : Camera_getAutoGainBrightness32( hCamera, Brightness);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAutoGainDynamics", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setAutoGainDynamics64(void* hCamera, float AutoGainParameterI, float AutoGainParameterD);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAutoGainDynamics", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setAutoGainDynamics32(void* hCamera, float AutoGainParameterI, float AutoGainParameterD);

        public static unsafe
            SVSGigeApiReturn
            Camera_setAutoGainDynamics(void* hCamera, float AutoGainParameterI, float AutoGainParameterD)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAutoGainDynamics64( hCamera, AutoGainParameterI, AutoGainParameterD) : Camera_setAutoGainDynamics32( hCamera, AutoGainParameterI, AutoGainParameterD);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAutoGainDynamics", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getAutoGainDynamics64(void* hCamera, float* AutoGainParameterI, float* AutoGainParameterD);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAutoGainDynamics", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getAutoGainDynamics32(void* hCamera, float* AutoGainParameterI, float* AutoGainParameterD);
        public static unsafe
            SVSGigeApiReturn
            Camera_getAutoGainDynamics(void* hCamera, float* AutoGainParameterI, float* AutoGainParameterD)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAutoGainDynamics64( hCamera, AutoGainParameterI, AutoGainParameterD) : Camera_getAutoGainDynamics32( hCamera,  AutoGainParameterI, AutoGainParameterD);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAutoGainLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setAutoGainLimits64(void* hCamera, float MinGain, float MaxGain);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAutoGainLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setAutoGainLimits32(void* hCamera, float MinGain, float MaxGain);
        public static unsafe
            SVSGigeApiReturn
            Camera_setAutoGainLimits(void* hCamera, float MinGain, float MaxGain)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAutoGainLimits64( hCamera, MinGain, MaxGain) : Camera_setAutoGainLimits32( hCamera,  MinGain, MaxGain);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAutoGainLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAutoGainLimits64(void* hCamera, float* MinGain, float* MaxGain);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAutoGainLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAutoGainLimits32(void* hCamera, float* MinGain, float* MaxGain);
        public static unsafe
        SVSGigeApiReturn
        Camera_getAutoGainLimits(void* hCamera, float* MinGain, float* MaxGain)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAutoGainLimits64( hCamera,  MinGain,  MaxGain) : Camera_getAutoGainLimits32( hCamera,  MinGain,  MaxGain);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAutoExposureLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAutoExposureLimits64(void* hCamera, float MinExposure, float MaxExposure);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAutoExposureLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAutoExposureLimits32(void* hCamera, float MinExposure, float MaxExposure);

        public static unsafe
        SVSGigeApiReturn
        Camera_setAutoExposureLimits(void* hCamera, float MinExposure, float MaxExposure)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAutoExposureLimits64( hCamera, MinExposure, MaxExposure) : Camera_setAutoExposureLimits32( hCamera, MinExposure, MaxExposure);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAutoExposureLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAutoExposureLimits64(void* hCamera, float* MinExposure, float* MaxExposure);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAutoExposureLimits", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAutoExposureLimits32(void* hCamera, float* MinExposure, float* MaxExposure);
        public static unsafe
            SVSGigeApiReturn
            Camera_getAutoExposureLimits(void* hCamera, float* MinExposure, float* MaxExposure)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAutoExposureLimits64( hCamera,  MinExposure,  MaxExposure) : Camera_getAutoExposureLimits32( hCamera,  MinExposure, MaxExposure);
        }


        //------------------------------------------------------------------------------
        // 17 - Controlling camera: Acquisition trigger
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAcquisitionControl", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAcquisitionControl64(void* hCamera, ACQUISITION_CONTROL AcquisitionControl);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAcquisitionControl", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAcquisitionControl32(void* hCamera, ACQUISITION_CONTROL AcquisitionControl);
        public static unsafe
        SVSGigeApiReturn
        Camera_setAcquisitionControl(void* hCamera, ACQUISITION_CONTROL AcquisitionControl)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAcquisitionControl64(hCamera, AcquisitionControl)
                                                : Camera_setAcquisitionControl32(hCamera, AcquisitionControl);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAcquisitionControl", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAcquisitionControl64(void* hCamera, ACQUISITION_CONTROL* AcquisitionControl);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAcquisitionControl", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAcquisitionControl32(void* hCamera, ACQUISITION_CONTROL* AcquisitionControl);
        public static unsafe
        SVSGigeApiReturn
        Camera_getAcquisitionControl(void* hCamera, ACQUISITION_CONTROL* AcquisitionControl)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAcquisitionControl64( hCamera, AcquisitionControl)
                                                : Camera_getAcquisitionControl32( hCamera, AcquisitionControl);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAcquisitionMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setAcquisitionMode64(void* hCamera, ACQUISITION_MODE AcquisitionMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAcquisitionMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setAcquisitionMode32(void* hCamera, ACQUISITION_MODE AcquisitionMode);
        public static unsafe
        SVSGigeApiReturn Camera_setAcquisitionMode(void* hCamera, ACQUISITION_MODE AcquisitionMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAcquisitionMode64(hCamera, AcquisitionMode)
                                                : Camera_setAcquisitionMode32(hCamera, AcquisitionMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAcquisitionModeAndStart" , CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAcquisitionModeAndStart64(void* hCamera, ACQUISITION_MODE AcquisitionMode, bool AcquisitionStart);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAcquisitionModeAndStart", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAcquisitionModeAndStart32(void* hCamera, ACQUISITION_MODE AcquisitionMode, bool AcquisitionStart);
        public static unsafe
        SVSGigeApiReturn
        Camera_setAcquisitionModeAndStart(void* hCamera, ACQUISITION_MODE AcquisitionMode, bool AcquisitionStart)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAcquisitionModeAndStart64(hCamera, AcquisitionMode, AcquisitionStart) 
                                                : Camera_setAcquisitionModeAndStart32(hCamera, AcquisitionMode, AcquisitionStart);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAcquisitionMode" ,CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getAcquisitionMode64(void* hCamera, ACQUISITION_MODE* AcquisitionMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAcquisitionMode" ,CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getAcquisitionMode32(void* hCamera, ACQUISITION_MODE* AcquisitionMode);
        public static unsafe 
        SVSGigeApiReturn Camera_getAcquisitionMode(void* hCamera, ACQUISITION_MODE* AcquisitionMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAcquisitionMode64( hCamera, AcquisitionMode) : Camera_getAcquisitionMode32( hCamera, AcquisitionMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_softwareTrigger", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_softwareTrigger64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_softwareTrigger", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_softwareTrigger32(void* hCamera);
        public static unsafe
        SVSGigeApiReturn
        Camera_softwareTrigger(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_softwareTrigger64(hCamera) : Camera_softwareTrigger32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_softwareTriggerID", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_softwareTriggerID64(void* hCamera, int TriggerID);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_softwareTriggerID", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_softwareTriggerID32(void* hCamera, int TriggerID);
        public static unsafe
        SVSGigeApiReturn
        Camera_softwareTriggerID(void* hCamera, int TriggerID)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_softwareTriggerID64(hCamera, TriggerID) : Camera_softwareTriggerID32(hCamera,TriggerID);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_softwareTriggerIDEnable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_softwareTriggerIDEnable64(void* hCamera, bool TriggerIDEnable);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_softwareTriggerIDEnable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_softwareTriggerIDEnable32(void* hCamera, bool TriggerIDEnable);
        public static unsafe
        SVSGigeApiReturn
        Camera_softwareTriggerIDEnable(void* hCamera, bool TriggerIDEnable)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_softwareTriggerIDEnable64(hCamera,  TriggerIDEnable) : Camera_softwareTriggerIDEnable32(hCamera, TriggerIDEnable);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setTriggerPolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setTriggerPolarity64(void* hCamera, TRIGGER_POLARITY TriggerPolarity);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setTriggerPolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setTriggerPolarity32(void* hCamera, TRIGGER_POLARITY TriggerPolarity);
        public static unsafe
        SVSGigeApiReturn Camera_setTriggerPolarity(void* hCamera, TRIGGER_POLARITY TriggerPolarity)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setTriggerPolarity64(hCamera, TriggerPolarity)
                                                : Camera_setTriggerPolarity32(hCamera, TriggerPolarity);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTriggerPolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getTriggerPolarity64(void* hCamera, TRIGGER_POLARITY* TriggerPolarity);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTriggerPolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getTriggerPolarity32(void* hCamera, TRIGGER_POLARITY* TriggerPolarity);
        public static unsafe
        SVSGigeApiReturn Camera_getTriggerPolarity(void* hCamera, TRIGGER_POLARITY* TriggerPolarity)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTriggerPolarity64(hCamera, TriggerPolarity)
                                                : Camera_getTriggerPolarity32(hCamera, TriggerPolarity);
        }
                

        //-----------------------Piv Mode --------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPivMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setPivMode64(void* hCamera, PIV_MODE PivMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPivMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setPivMode32(void* hCamera, PIV_MODE PivMode);
        public static unsafe
        SVSGigeApiReturn Camera_setPivMode(void* hCamera, PIV_MODE PivMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPivMode64(hCamera, PivMode) : Camera_setPivMode32(hCamera, PivMode);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPivMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getPivMode64(void* hCamera, PIV_MODE* PivMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPivMode" ,CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getPivMode32(void* hCamera, PIV_MODE* PivMode);
        public static unsafe
        SVSGigeApiReturn Camera_getPivMode(void* hCamera, PIV_MODE* PivMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPivMode64(hCamera, PivMode) : Camera_getPivMode32(hCamera, PivMode);
        }

        //------------------------ Debouncer ------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setDebouncerDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setDebouncerDuration64(void* hCamera, float DebouncerDuration);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setDebouncerDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setDebouncerDuration32(void* hCamera, float DebouncerDuration);
        public static unsafe
        SVSGigeApiReturn Camera_setDebouncerDuration(void* hCamera, float DebouncerDuration)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setDebouncerDuration64(hCamera, DebouncerDuration)
                                                : Camera_setDebouncerDuration32(hCamera, DebouncerDuration);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getDebouncerDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getDebouncerDuration64(void* hCamera, float* Duration);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getDebouncerDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getDebouncerDuration32(void* hCamera, float* Duration);
        public static unsafe
        SVSGigeApiReturn Camera_getDebouncerDuration(void* hCamera, float* Duration)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getDebouncerDuration64(hCamera, Duration) : Camera_getDebouncerDuration32(hCamera, Duration);
        }

        //------------------------- Prescaler -------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPrescalerDevisor", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setPrescalerDevisor64(void* hCamera, uint PrescalerDevisor);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPrescalerDevisor", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setPrescalerDevisor32(void* hCamera, uint PrescalerDevisor);
        public static unsafe
        SVSGigeApiReturn Camera_setPrescalerDevisor(void* hCamera, uint PrescalerDevisor)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPrescalerDevisor64(hCamera, PrescalerDevisor) : Camera_setPrescalerDevisor32(hCamera, PrescalerDevisor);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPrescalerDevisor", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getPrescalerDevisor64(void* hCamera, uint* PrescalerDevisor);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPrescalerDevisor", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getPrescalerDevisor32(void* hCamera, uint* PrescalerDevisor);
        public static unsafe
        SVSGigeApiReturn Camera_getPrescalerDevisor(void* hCamera, uint* PrescalerDevisor)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPrescalerDevisor64( hCamera, PrescalerDevisor) : Camera_getPrescalerDevisor32( hCamera,  PrescalerDevisor);
        }


        //------------------------  Sequencer  --------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_loadSequenceParameters", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_loadSequenceParameters64(void* hCamera, sbyte* Filename);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_loadSequenceParameters", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_loadSequenceParameters32(void* hCamera, sbyte* Filename);
        public static unsafe
        SVSGigeApiReturn Camera_loadSequenceParameters(void* hCamera, sbyte* Filename)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_loadSequenceParameters64(hCamera, Filename) : Camera_loadSequenceParameters32(hCamera, Filename);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_startSequencer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_startSequencer64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_startSequencer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_startSequencer32(void* hCamera);
        public static unsafe
        SVSGigeApiReturn Camera_startSequencer(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_startSequencer64(hCamera) : Camera_startSequencer32(hCamera);
        }


        //-------------------------------------------------------------------------
        // 18 - Controlling camera: Strobe
        //-------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setStrobePolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePolarity64(void* hCamera, STROBE_POLARITY StrobePolarity);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setStrobePolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePolarity32(void* hCamera, STROBE_POLARITY StrobePolarity);
        public static unsafe
        SVSGigeApiReturn
        Camera_setStrobePolarity(void* hCamera, STROBE_POLARITY StrobePolarity)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setStrobePolarity64(hCamera, StrobePolarity)
                                                : Camera_setStrobePolarity32(hCamera, StrobePolarity);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setStrobePolarityExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePolarityExtended64(void* hCamera, STROBE_POLARITY StrobePolarity, int StrobeIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setStrobePolarityExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePolarityExtended32(void* hCamera, STROBE_POLARITY StrobePolarity, int StrobeIndex);
        public static unsafe
        SVSGigeApiReturn
        Camera_setStrobePolarityExtended(void* hCamera, STROBE_POLARITY StrobePolarity, int StrobeIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setStrobePolarityExtended64(hCamera, StrobePolarity, StrobeIndex)
                                                : Camera_setStrobePolarityExtended32(hCamera, StrobePolarity, StrobeIndex);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobePolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePolarity64(void* hCamera, STROBE_POLARITY* StrobePolarity);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobePolarity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePolarity32(void* hCamera, STROBE_POLARITY* StrobePolarity);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobePolarity(void* hCamera, STROBE_POLARITY* StrobePolarity)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobePolarity64(hCamera, StrobePolarity) : Camera_getStrobePolarity32(hCamera, StrobePolarity);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobePolarityExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePolarityExtended64(void* hCamera, STROBE_POLARITY* StrobePolarity, int StrobeIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobePolarityExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePolarityExtended32(void* hCamera, STROBE_POLARITY* StrobePolarity, int StrobeIndex);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobePolarityExtended(void* hCamera, STROBE_POLARITY* StrobePolarity, int StrobeIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobePolarityExtended64(hCamera, StrobePolarity, StrobeIndex)
                                                : Camera_getStrobePolarityExtended32(hCamera, StrobePolarity, StrobeIndex);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setStrobePosition", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePosition64(void* hCamera, float StrobePosition);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setStrobePosition", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePosition32(void* hCamera, float StrobePosition);
        public static unsafe
        SVSGigeApiReturn
        Camera_setStrobePosition(void* hCamera, float StrobePosition)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setStrobePosition64(hCamera, StrobePosition)
                                                : Camera_setStrobePosition32(hCamera, StrobePosition);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setStrobePositionExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePositionExtended64(void* hCamera, float StrobePosition, int StrobeIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setStrobePositionExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobePositionExtended32(void* hCamera, float StrobePosition, int StrobeIndex);
        public static unsafe
        SVSGigeApiReturn
        Camera_setStrobePositionExtended(void* hCamera, float StrobePosition, int StrobeIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setStrobePositionExtended64( hCamera, StrobePosition, StrobeIndex)
                                                : Camera_setStrobePositionExtended32( hCamera, StrobePosition, StrobeIndex);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobePosition", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePosition64(void* hCamera, float* StrobePosition);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobePosition", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePosition32(void* hCamera, float* StrobePosition);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobePosition(void* hCamera, float* StrobePosition)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobePosition64(hCamera,  StrobePosition)
                                                : Camera_getStrobePosition32(hCamera,  StrobePosition);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobePositionExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionExtended64(void* hCamera, float* StrobePosition, int StrobeIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobePositionExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionExtended32(void* hCamera, float* StrobePosition, int StrobeIndex);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionExtended(void* hCamera, float* StrobePosition, int StrobeIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobePositionExtended64( hCamera,  StrobePosition, StrobeIndex)
                                                : Camera_getStrobePositionExtended32( hCamera,  StrobePosition, StrobeIndex);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobePositionMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionMax64(void* hCamera, float* MaxStrobePosition);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobePositionMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionMax32(void* hCamera, float* MaxStrobePosition);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionMax(void* hCamera, float* MaxStrobePosition)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobePositionMax64(hCamera, MaxStrobePosition)
                                                : Camera_getStrobePositionMax32(hCamera, MaxStrobePosition);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobePositionIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionIncrement64(void* hCamera, float* StrobePositionIncrement);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobePositionIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionIncrement32(void* hCamera, float* StrobePositionIncrement);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobePositionIncrement(void* hCamera, float* StrobePositionIncrement)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobePositionIncrement64(hCamera, StrobePositionIncrement) : Camera_getStrobePositionIncrement32(hCamera, StrobePositionIncrement);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setStrobeDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobeDuration64(void* hCamera, float StrobeDuration);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setStrobeDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobeDuration32(void* hCamera, float StrobeDuration);
        public static unsafe
        SVSGigeApiReturn
        Camera_setStrobeDuration(void* hCamera, float StrobeDuration)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setStrobeDuration64( hCamera, StrobeDuration) : Camera_setStrobeDuration32( hCamera, StrobeDuration);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setStrobeDurationExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobeDurationExtended64(void* hCamera, float StrobeDuration, int StrobeIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setStrobeDurationExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setStrobeDurationExtended32(void* hCamera, float StrobeDuration, int StrobeIndex);
        public static unsafe
        SVSGigeApiReturn
        Camera_setStrobeDurationExtended(void* hCamera, float StrobeDuration, int StrobeIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setStrobeDurationExtended64( hCamera, StrobeDuration, StrobeIndex)
                                                : Camera_setStrobeDurationExtended32( hCamera, StrobeDuration, StrobeIndex);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobeDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDuration64(void* hCamera, float* StrobeDuration);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobeDuration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDuration32(void* hCamera, float* StrobeDuration);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobeDuration(void* hCamera, float* StrobeDuration)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobeDuration64(hCamera, StrobeDuration)
                                                : Camera_getStrobeDuration32(hCamera, StrobeDuration);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobeDurationExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationExtended64(void* hCamera, float* StrobeDuration, int StrobeIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobeDurationExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationExtended32(void* hCamera, float* StrobeDuration, int StrobeIndex);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationExtended(void* hCamera, float* StrobeDuration, int StrobeIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobeDurationExtended64(hCamera, StrobeDuration, StrobeIndex)
                                                : Camera_getStrobeDurationExtended32(hCamera, StrobeDuration, StrobeIndex);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobeDurationMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationMax64(void* hCamera, float* MaxStrobeDuration);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobeDurationMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationMax32(void* hCamera, float* MaxStrobeDuration);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationMax(void* hCamera, float* MaxStrobeDuration)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobeDurationMax64(hCamera, MaxStrobeDuration)
                                                : Camera_getStrobeDurationMax32(hCamera, MaxStrobeDuration);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getStrobeDurationIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationIncrement64(void* hCamera, float* StrobeDurationIncrement);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getStrobeDurationIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationIncrement32(void* hCamera, float* StrobeDurationIncrement);
        public static unsafe
        SVSGigeApiReturn
        Camera_getStrobeDurationIncrement(void* hCamera, float* StrobeDurationIncrement)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getStrobeDurationIncrement64(hCamera, StrobeDurationIncrement)
                                                : Camera_getStrobeDurationIncrement32(hCamera, StrobeDurationIncrement);
        }


        //------------------------------------------------------------------------------
        // 19 - Controlling camera: Tap balance
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_saveTapBalanceSettings", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_saveTapBalanceSettings64(void* hCamera, sbyte* Filename);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_saveTapBalanceSettings", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_saveTapBalanceSettings32(void* hCamera, sbyte* Filename);
        public static unsafe
        SVSGigeApiReturn
        Camera_saveTapBalanceSettings(void* hCamera, sbyte* Filename)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_saveTapBalanceSettings64(hCamera, Filename) : Camera_saveTapBalanceSettings32(hCamera, Filename);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_loadTapBalanceSettings", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_loadTapBalanceSettings64(void* hCamera, sbyte* Filename);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_loadTapBalanceSettings", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_loadTapBalanceSettings32(void* hCamera, sbyte* Filename);
        public static unsafe
        SVSGigeApiReturn
        Camera_loadTapBalanceSettings(void* hCamera, sbyte* Filename)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_loadTapBalanceSettings64(hCamera, Filename) :Camera_loadTapBalanceSettings32(hCamera, Filename);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setTapConfiguration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapConfiguration64(void* hCamera, int TapCount);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setTapConfiguration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapConfiguration32(void* hCamera, int TapCount);
        public static unsafe
        SVSGigeApiReturn
        Camera_setTapConfiguration(void* hCamera, int TapCount)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setTapConfiguration64(hCamera, TapCount) : Camera_setTapConfiguration32(hCamera, TapCount);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTapConfiguration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapConfiguration64(void* hCamera, int* TapCount);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTapConfiguration", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapConfiguration32(void* hCamera, int* TapCount);
        public static unsafe
        SVSGigeApiReturn
        Camera_getTapConfiguration(void* hCamera, int* TapCount)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTapConfiguration64(hCamera, TapCount) : Camera_getTapConfiguration32(hCamera, TapCount);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setTapConfigurationEx", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapConfigurationEx64(void* hCamera, SVGIGE_TAP_CONFIGURATION_SELECT SelectedTapConfig);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setTapConfigurationEx", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapConfigurationEx32(void* hCamera, SVGIGE_TAP_CONFIGURATION_SELECT SelectedTapConfig);
        public static unsafe
        SVSGigeApiReturn
        Camera_setTapConfigurationEx(void* hCamera, SVGIGE_TAP_CONFIGURATION_SELECT SelectedTapConfig)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setTapConfigurationEx64(hCamera, SelectedTapConfig) : Camera_setTapConfigurationEx32(hCamera, SelectedTapConfig);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTapConfigurationEx", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapConfigurationEx64(void* hCamera, SVGIGE_TAP_CONFIGURATION_SELECT* TapConfig);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTapConfigurationEx", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapConfigurationEx32(void* hCamera, SVGIGE_TAP_CONFIGURATION_SELECT* TapConfig);
        public static unsafe
        SVSGigeApiReturn
        Camera_getTapConfigurationEx(void* hCamera, SVGIGE_TAP_CONFIGURATION_SELECT* TapConfig)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTapConfigurationEx64(hCamera, TapConfig) : Camera_getTapConfigurationEx32(hCamera, TapConfig);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAutoTapBalanceMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAutoTapBalanceMode64(void* hCamera, SVGIGE_AUTO_TAP_BALANCE_MODE AutoTapBalanceMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAutoTapBalanceMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAutoTapBalanceMode32(void* hCamera, SVGIGE_AUTO_TAP_BALANCE_MODE AutoTapBalanceMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_setAutoTapBalanceMode(void* hCamera, SVGIGE_AUTO_TAP_BALANCE_MODE AutoTapBalanceMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAutoTapBalanceMode64(hCamera, AutoTapBalanceMode) : Camera_setAutoTapBalanceMode32(hCamera, AutoTapBalanceMode);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAutoTapBalanceMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAutoTapBalanceMode64(void* hCamera, SVGIGE_AUTO_TAP_BALANCE_MODE* AutoTapBalanceMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAutoTapBalanceMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAutoTapBalanceMode32(void* hCamera, SVGIGE_AUTO_TAP_BALANCE_MODE* AutoTapBalanceMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_getAutoTapBalanceMode(void* hCamera, SVGIGE_AUTO_TAP_BALANCE_MODE* AutoTapBalanceMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAutoTapBalanceMode64(hCamera, AutoTapBalanceMode) : Camera_getAutoTapBalanceMode32(hCamera, AutoTapBalanceMode);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setTapGain",  CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapGain64(void* hCamera, float TapGain, SVGIGE_TAP_SELECT TapSelect);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setTapGain",  CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapGain32(void* hCamera, float TapGain, SVGIGE_TAP_SELECT TapSelect);
        public static unsafe
        SVSGigeApiReturn
        Camera_setTapGain(void* hCamera, float TapGain, SVGIGE_TAP_SELECT TapSelect)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setTapGain64(hCamera, TapGain, TapSelect) : Camera_setTapGain32(hCamera, TapGain, TapSelect);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTapGain", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapGain64(void* hCamera, float* TapBalance, SVGIGE_TAP_SELECT TapSelect);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTapGain", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapGain32(void* hCamera, float* TapBalance, SVGIGE_TAP_SELECT TapSelect);
        public static unsafe
        SVSGigeApiReturn
        Camera_getTapGain(void* hCamera, float* TapBalance, SVGIGE_TAP_SELECT TapSelect)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTapGain64(hCamera, TapBalance, TapSelect) : Camera_getTapGain32(hCamera, TapBalance, TapSelect);
        }


        //------------------------------------------------------------------------------
        // 20 - Controlling camera: Image parameter
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getImagerWidth", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImagerWidth64(void* hCamera, int* ImagerWidth);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getImagerWidth", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImagerWidth32(void* hCamera, int* ImagerWidth);
        public static unsafe
        SVSGigeApiReturn
        Camera_getImagerWidth(void* hCamera, int* ImagerWidth)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getImagerWidth64( hCamera, ImagerWidth) : Camera_getImagerWidth32(hCamera, ImagerWidth);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getImagerHeight", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImagerHeight64(void* hCamera, int* ImagerHeight);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getImagerHeight", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImagerHeight32(void* hCamera, int* ImagerHeight);
        public static unsafe
        SVSGigeApiReturn
        Camera_getImagerHeight(void* hCamera, int* ImagerHeight)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getImagerHeight64( hCamera,  ImagerHeight) : Camera_getImagerHeight32( hCamera, ImagerHeight);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getImageSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImageSize64(void* hCamera, int* ImageSize);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getImageSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImageSize32(void* hCamera, int* ImageSize);
        public static unsafe
        SVSGigeApiReturn
        Camera_getImageSize(void* hCamera, int* ImageSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getImageSize64( hCamera, ImageSize) : Camera_getImageSize32( hCamera, ImageSize);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPitch", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPitch64(void* hCamera, int* Pitch);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPitch", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPitch32(void* hCamera, int* Pitch);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPitch(void* hCamera, int* Pitch)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPitch64(hCamera, Pitch) : Camera_getPitch32(hCamera, Pitch);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getSizeX", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getSizeX64(void* hCamera, int* SizeX);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getSizeX", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getSizeX32(void* hCamera, int* SizeX);
        public static unsafe
        SVSGigeApiReturn
        Camera_getSizeX(void* hCamera, int* SizeX)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getSizeX64( hCamera, SizeX) : Camera_getSizeX32( hCamera, SizeX);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getSizeY", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getSizeY64(void* hCamera, int* SizeY);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getSizeY", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getSizeY32(void* hCamera, int* SizeY);
        public static unsafe
        SVSGigeApiReturn
        Camera_getSizeY(void* hCamera, int* SizeY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getSizeY64( hCamera, SizeY) : Camera_getSizeY32( hCamera, SizeY);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setBinningMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setBinningMode64(void* hCamera, BINNING_MODE BinningMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setBinningMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_setBinningMode32(void* hCamera, BINNING_MODE BinningMode);

        public static unsafe
        SVSGigeApiReturn Camera_setBinningMode(void* hCamera, BINNING_MODE BinningMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setBinningMode64( hCamera, BinningMode) : Camera_setBinningMode32( hCamera, BinningMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getBinningMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getBinningMode64(void* hCamera, BINNING_MODE* BinningMode);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getBinningMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn Camera_getBinningMode32(void* hCamera, BINNING_MODE* BinningMode);
        public static unsafe
        SVSGigeApiReturn Camera_getBinningMode(void* hCamera, BINNING_MODE* BinningMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ?  Camera_getBinningMode64( hCamera,  BinningMode) :  Camera_getBinningMode32( hCamera, BinningMode);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAreaOfInterest", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAreaOfInterest64(void* hCamera, int SizeX, int SizeY, int OffsetX, int OffsetY);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAreaOfInterest", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAreaOfInterest32(void* hCamera, int SizeX, int SizeY, int OffsetX, int OffsetY);
        public static unsafe
        SVSGigeApiReturn
        Camera_setAreaOfInterest(void* hCamera, int SizeX, int SizeY, int OffsetX, int OffsetY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAreaOfInterest64( hCamera, SizeX, SizeY, OffsetX, OffsetY) 
                                                : Camera_setAreaOfInterest32( hCamera, SizeX, SizeY, OffsetX, OffsetY);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAreaOfInterest", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterest64(void* hCamera, int* SizeX, int* SizeY, int* OffsetX, int* OffsetY);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAreaOfInterest", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterest32(void* hCamera, int* SizeX, int* SizeY, int* OffsetX, int* OffsetY);
        public static unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterest(void* hCamera, int* SizeX, int* SizeY, int* OffsetX, int* OffsetY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAreaOfInterest64( hCamera,  SizeX,  SizeY,  OffsetX, OffsetY)
                                                : Camera_getAreaOfInterest32( hCamera,  SizeX,  SizeY,  OffsetX, OffsetY);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAreaOfInterestRange", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterestRange64(void* hCamera, int* MinSizeX, int* MinSizeY, int* MaxSizeX, int* MaxSizeY);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAreaOfInterestRange", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterestRange32(void* hCamera, int* MinSizeX, int* MinSizeY, int* MaxSizeX, int* MaxSizeY);
        public static unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterestRange(void* hCamera, int* MinSizeX, int* MinSizeY, int* MaxSizeX, int* MaxSizeY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAreaOfInterestRange64( hCamera, MinSizeX,  MinSizeY, MaxSizeX, MaxSizeY) 
                                                : Camera_getAreaOfInterestRange32( hCamera,  MinSizeX,  MinSizeY,  MaxSizeX, MaxSizeY);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAreaOfInterestIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterestIncrement64(void* hCamera, int* SizeXIncrement, int* SizeYIncrement, int* OffsetXIncrement, int* OffsetYIncrement);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAreaOfInterestIncrement", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterestIncrement32(void* hCamera, int* SizeXIncrement, int* SizeYIncrement, int* OffsetXIncrement, int* OffsetYIncrement);
        public static unsafe
        SVSGigeApiReturn
        Camera_getAreaOfInterestIncrement(void* hCamera, int* SizeXIncrement, int* SizeYIncrement, int* OffsetXIncrement, int* OffsetYIncrement)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAreaOfInterestIncrement64( hCamera, SizeXIncrement, SizeYIncrement, OffsetXIncrement, OffsetYIncrement)
                                                : Camera_getAreaOfInterestIncrement32( hCamera, SizeXIncrement, SizeYIncrement, OffsetXIncrement, OffsetYIncrement);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_resetTimestampCounter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_resetTimestampCounter64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_resetTimestampCounter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_resetTimestampCounter32(void* hCamera);
        public static unsafe
        SVSGigeApiReturn
        Camera_resetTimestampCounter(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_resetTimestampCounter64(hCamera) : Camera_resetTimestampCounter32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTimestampCounter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTimestampCounter64(void* hCamera, double* TimestampCounter);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTimestampCounter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTimestampCounter32(void* hCamera, double* TimestampCounter);
        public static unsafe
        SVSGigeApiReturn
        Camera_getTimestampCounter(void* hCamera, double* TimestampCounter)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTimestampCounter64( hCamera, TimestampCounter) : Camera_getTimestampCounter32( hCamera, TimestampCounter);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTimestampTickFrequency", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTimestampTickFrequency64(void* hCamera, double* TimestampCounter);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTimestampTickFrequency", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTimestampTickFrequency32(void* hCamera, double* TimestampCounter);
        public static unsafe
        SVSGigeApiReturn
        Camera_getTimestampTickFrequency(void* hCamera, double* TimestampCounter)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTimestampTickFrequency64(hCamera, TimestampCounter) :Camera_getTimestampTickFrequency32(hCamera, TimestampCounter);
        }




        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setFlippingMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setFlippingMode64(void* hCamera, SVGIGE_FLIPPING_MODE FlippingMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setFlippingMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setFlippingMode32(void* hCamera, SVGIGE_FLIPPING_MODE FlippingMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_setFlippingMode(void* hCamera, SVGIGE_FLIPPING_MODE FlippingMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setFlippingMode64(hCamera, FlippingMode) : Camera_setFlippingMode32(hCamera, FlippingMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getFlippingMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getFlippingMode64(void* hCamera, SVGIGE_FLIPPING_MODE* FlippingMode);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getFlippingMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getFlippingMode32(void* hCamera, SVGIGE_FLIPPING_MODE* FlippingMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_getFlippingMode(void* hCamera, SVGIGE_FLIPPING_MODE* FlippingMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getFlippingMode64(hCamera, FlippingMode) : Camera_getFlippingMode32( hCamera, FlippingMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setShutterMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setShutterMode64(void* hCamera, SVGIGE_SHUTTER_MODE ShutterMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setShutterMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setShutterMode32(void* hCamera, SVGIGE_SHUTTER_MODE ShutterMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_setShutterMode(void* hCamera, SVGIGE_SHUTTER_MODE ShutterMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setShutterMode64(hCamera, ShutterMode) : Camera_setShutterMode32(hCamera, ShutterMode);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getShutterMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getShutterMode64(void* hCamera, SVGIGE_SHUTTER_MODE* ShutterMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getShutterMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getShutterMode32(void* hCamera, SVGIGE_SHUTTER_MODE* ShutterMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_getShutterMode(void* hCamera, SVGIGE_SHUTTER_MODE* ShutterMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getShutterMode64(hCamera, ShutterMode) : Camera_getShutterMode32(hCamera, ShutterMode);
        }


        //------------------------------------------------------------------------------
        // 21 - Controlling camera: Image appearance
        //------------------------------------------------------------------------------



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelType64(void* hCamera, GVSP_PIXEL_TYPE* PixelType);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelType", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelType32(void* hCamera, GVSP_PIXEL_TYPE* PixelType);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelType(void* hCamera, GVSP_PIXEL_TYPE* PixelType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelType64( hCamera, PixelType) : Camera_getPixelType32( hCamera,  PixelType);
        }

        //
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPixelType", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelType64(void* hCamera, GVSP_PIXEL_TYPE PixelType);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPixelType", CallingConvention = CallingConvention.Cdecl)]
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelType32(void* hCamera, GVSP_PIXEL_TYPE PixelType);
        public static unsafe
        SVSGigeApiReturn
        Camera_setPixelType(void* hCamera, GVSP_PIXEL_TYPE PixelType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPixelType64(hCamera, PixelType) : Camera_setPixelType32(hCamera, PixelType);
        }
        //

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPixelDepth", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelDepth64(void* hCamera, SVGIGE_PIXEL_DEPTH PixelDepth);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPixelDepth", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelDepth32(void* hCamera, SVGIGE_PIXEL_DEPTH PixelDepth);
        public static unsafe
        SVSGigeApiReturn
        Camera_setPixelDepth(void* hCamera, SVGIGE_PIXEL_DEPTH PixelDepth)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPixelDepth64(hCamera, PixelDepth) : Camera_setPixelDepth32( hCamera, PixelDepth);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelDepth", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelDepth64(void* hCamera, SVGIGE_PIXEL_DEPTH* PixelDepth);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelDepth", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelDepth32(void* hCamera, SVGIGE_PIXEL_DEPTH* PixelDepth);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelDepth(void* hCamera, SVGIGE_PIXEL_DEPTH* PixelDepth)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelDepth64(hCamera, PixelDepth) : Camera_getPixelDepth32( hCamera, PixelDepth);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setWhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setWhiteBalance64(void* hCamera, float Red, float Green, float Blue);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setWhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setWhiteBalance32(void* hCamera, float Red, float Green, float Blue);
        public static unsafe
        SVSGigeApiReturn
        Camera_setWhiteBalance(void* hCamera, float Red, float Green, float Blue)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setWhiteBalance64( hCamera, Red, Green, Blue)
                                                : Camera_setWhiteBalance32(hCamera, Red, Green, Blue);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getWhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getWhiteBalance64(void* hCamera, float* Red, float* Green, float* Blue);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getWhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getWhiteBalance32(void* hCamera, float* Red, float* Green, float* Blue);
        public static unsafe
        SVSGigeApiReturn
        Camera_getWhiteBalance(void* hCamera, float* Red, float* Green, float* Blue)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getWhiteBalance64(hCamera, Red, Green, Blue)
                                                : Camera_getWhiteBalance32(hCamera, Red, Green, Blue);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getWhiteBalanceMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getWhiteBalanceMax64(void* hCamera, float* WhiteBalanceMax);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getWhiteBalanceMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getWhiteBalanceMax32(void* hCamera, float* WhiteBalanceMax);
        public static unsafe
        SVSGigeApiReturn
        Camera_getWhiteBalanceMax(void* hCamera, float* WhiteBalanceMax)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getWhiteBalanceMax64(hCamera, WhiteBalanceMax)
                                                : Camera_getWhiteBalanceMax32(hCamera, WhiteBalanceMax);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setGammaCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setGammaCorrection64(void* hCamera, float GammaCorrection);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setGammaCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setGammaCorrection32(void* hCamera, float GammaCorrection);
        public static unsafe
        SVSGigeApiReturn
        Camera_setGammaCorrection(void* hCamera, float GammaCorrection)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setGammaCorrection64( hCamera,  GammaCorrection) : Camera_setGammaCorrection32( hCamera,  GammaCorrection);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setGammaCorrectionExt", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setGammaCorrectionExt64(void* hCamera, float GammaCorrection, float DigitalGain, float DigitalOffset);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setGammaCorrectionExt", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setGammaCorrectionExt32(void* hCamera, float GammaCorrection, float DigitalGain, float DigitalOffset);
        public static unsafe
        SVSGigeApiReturn
        Camera_setGammaCorrectionExt(void* hCamera, float GammaCorrection, float DigitalGain, float DigitalOffset)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setGammaCorrectionExt64( hCamera,  GammaCorrection, DigitalGain, DigitalOffset)
                                                : Camera_setGammaCorrectionExt32( hCamera,  GammaCorrection, DigitalGain, DigitalOffset);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLowpassFilter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLowpassFilter64(void* hCamera, LOWPASS_FILTER LowpassFilter);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLowpassFilter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLowpassFilter32(void* hCamera, LOWPASS_FILTER LowpassFilter);
        public static unsafe
        SVSGigeApiReturn
        Camera_setLowpassFilter(void* hCamera, LOWPASS_FILTER LowpassFilter)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLowpassFilter64( hCamera,  LowpassFilter) : Camera_setLowpassFilter32( hCamera, LowpassFilter);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLowpassFilter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLowpassFilter64(void* hCamera, LOWPASS_FILTER* LowpassFilter);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLowpassFilter", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLowpassFilter32(void* hCamera, LOWPASS_FILTER* LowpassFilter);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLowpassFilter(void* hCamera, LOWPASS_FILTER* LowpassFilter)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLowpassFilter64( hCamera, LowpassFilter) : Camera_getLowpassFilter32( hCamera,  LowpassFilter);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLookupTableMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLookupTableMode64(void* hCamera, LUT_MODE LUTMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLookupTableMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLookupTableMode32(void* hCamera, LUT_MODE LUTMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_setLookupTableMode(void* hCamera, LUT_MODE LUTMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLookupTableMode64(hCamera, LUTMode) : Camera_setLookupTableMode32(hCamera, LUTMode);
        }

        
        
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLookupTableMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLookupTableMode64(void* hCamera, LUT_MODE* LUTMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLookupTableMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLookupTableMode32(void* hCamera, LUT_MODE* LUTMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLookupTableMode(void* hCamera, LUT_MODE* LUTMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLookupTableMode64( hCamera, LUTMode) : Camera_getLookupTableMode32( hCamera, LUTMode);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLookupTable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLookupTable64(void* hCamera, sbyte* LookupTable, int LookupTableSize);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLookupTable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLookupTable32(void* hCamera, sbyte* LookupTable, int LookupTableSize);
        public static unsafe
        SVSGigeApiReturn
        Camera_setLookupTable(void* hCamera, sbyte* LookupTable, int LookupTableSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLookupTable64(hCamera, LookupTable, LookupTableSize)
                                                : Camera_setLookupTable32(hCamera, LookupTable, LookupTableSize);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLookupTable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLookupTable64(void* hCamera, sbyte* LookupTable, int LookupTableSize);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLookupTable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLookupTable32(void* hCamera, sbyte* LookupTable, int LookupTableSize);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLookupTable(void* hCamera, sbyte* LookupTable, int LookupTableSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLookupTable64(hCamera, LookupTable, LookupTableSize)
                                                : Camera_getLookupTable32(hCamera, LookupTable, LookupTableSize);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_startImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_startImageCorrection64(void* hCamera, IMAGE_CORRECTION_STEP ImageCorrectionStep);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_startImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_startImageCorrection32(void* hCamera, IMAGE_CORRECTION_STEP ImageCorrectionStep);
        public static unsafe
        SVSGigeApiReturn
        Camera_startImageCorrection(void* hCamera, IMAGE_CORRECTION_STEP ImageCorrectionStep)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_startImageCorrection64(hCamera, ImageCorrectionStep)
                                                : Camera_startImageCorrection32(hCamera, ImageCorrectionStep);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_isIdleImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_isIdleImageCorrection64(void* hCamera, IMAGE_CORRECTION_STEP* ImageCorrectionStep, bool* isIdle);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_isIdleImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_isIdleImageCorrection32(void* hCamera, IMAGE_CORRECTION_STEP* ImageCorrectionStep, bool* isIdle);
        public static unsafe
        SVSGigeApiReturn
        Camera_isIdleImageCorrection(void* hCamera, IMAGE_CORRECTION_STEP* ImageCorrectionStep, bool* isIdle)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_isIdleImageCorrection64(hCamera, ImageCorrectionStep, isIdle)
                                                : Camera_isIdleImageCorrection32(hCamera, ImageCorrectionStep, isIdle);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setImageCorrection64(void* hCamera, IMAGE_CORRECTION_MODE ImageCorrectionMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setImageCorrection32(void* hCamera, IMAGE_CORRECTION_MODE ImageCorrectionMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_setImageCorrection(void* hCamera, IMAGE_CORRECTION_MODE ImageCorrectionMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setImageCorrection64(hCamera, ImageCorrectionMode) 
                                                : Camera_setImageCorrection32(hCamera, ImageCorrectionMode);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImageCorrection64(void* hCamera, IMAGE_CORRECTION_MODE* ImageCorrectionMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getImageCorrection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getImageCorrection32(void* hCamera, IMAGE_CORRECTION_MODE* ImageCorrectionMode);
        public static unsafe
        SVSGigeApiReturn
        Camera_getImageCorrection(void* hCamera, IMAGE_CORRECTION_MODE* ImageCorrectionMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getImageCorrection64(hCamera, ImageCorrectionMode)
                                                : Camera_getImageCorrection32(hCamera, ImageCorrectionMode);
        }


        //--------------------------------------Pixel correction-----------------------------------------																
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPixelsCorrectionMap", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionMap64(void* hCamera, PIXELS_CORRECTION_MAP_SELECT PixelsCorrectionMap);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPixelsCorrectionMap", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionMap32(void* hCamera, PIXELS_CORRECTION_MAP_SELECT PixelsCorrectionMap);
        public static unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionMap(void* hCamera, PIXELS_CORRECTION_MAP_SELECT PixelsCorrectionMap)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPixelsCorrectionMap64(hCamera, PixelsCorrectionMap) : Camera_setPixelsCorrectionMap32(hCamera, PixelsCorrectionMap);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelsCorrectionMap", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMap64(void* hCamera, PIXELS_CORRECTION_MAP_SELECT* PixelsCorrectionMap);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelsCorrectionMap", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMap32(void* hCamera, PIXELS_CORRECTION_MAP_SELECT* PixelsCorrectionMap);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMap(void* hCamera, PIXELS_CORRECTION_MAP_SELECT* PixelsCorrectionMap)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelsCorrectionMap64(hCamera, PixelsCorrectionMap) : Camera_getPixelsCorrectionMap32(hCamera, PixelsCorrectionMap);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPixelsCorrectionControlEnabel", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionControlEnabel64(void* hCamera, bool isPixelsCorrectionEnabled);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPixelsCorrectionControlEnabel", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionControlEnabel32(void* hCamera, bool isPixelsCorrectionEnabled);
        public static unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionControlEnabel(void* hCamera, bool isPixelsCorrectionEnabled)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPixelsCorrectionControlEnabel64(hCamera, isPixelsCorrectionEnabled) : Camera_setPixelsCorrectionControlEnabel32(hCamera, isPixelsCorrectionEnabled);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelsCorrectionControlEnabel", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionControlEnabel64(void* hCamera, bool* isPixelsCorrectionEnabled);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelsCorrectionControlEnabel", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionControlEnabel32(void* hCamera, bool* isPixelsCorrectionEnabled);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionControlEnabel(void* hCamera, bool* isPixelsCorrectionEnabled)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelsCorrectionControlEnabel64(hCamera, isPixelsCorrectionEnabled) : Camera_getPixelsCorrectionControlEnabel32(hCamera, isPixelsCorrectionEnabled);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPixelsCorrectionControlMark", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionControlMark64(void* hCamera, bool isPixelsCorrectionMarked);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPixelsCorrectionControlMark", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionControlMark32(void* hCamera, bool isPixelsCorrectionMarked);
        public static unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionControlMark(void* hCamera, bool isPixelsCorrectionMarked)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPixelsCorrectionControlMark64(hCamera, isPixelsCorrectionMarked) : Camera_setPixelsCorrectionControlMark32(hCamera, isPixelsCorrectionMarked);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelsCorrectionControlMark", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionControlMark64(void* hCamera, bool* isPixelsCorrectionMarked);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelsCorrectionControlMark", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionControlMark32(void* hCamera, bool* isPixelsCorrectionMarked);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionControlMark(void* hCamera, bool* isPixelsCorrectionMarked)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelsCorrectionControlMark64(hCamera, isPixelsCorrectionMarked) : Camera_getPixelsCorrectionControlMark32(hCamera, isPixelsCorrectionMarked);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setPixelsCorrectionMapOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionMapOffset64(void* hCamera, int OffsetX, int OffsetY);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setPixelsCorrectionMapOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionMapOffset32(void* hCamera, int OffsetX, int OffsetY);
        public static unsafe
        SVSGigeApiReturn
        Camera_setPixelsCorrectionMapOffset(void* hCamera, int OffsetX, int OffsetY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setPixelsCorrectionMapOffset64(hCamera, OffsetX, OffsetY) : Camera_setPixelsCorrectionMapOffset32(hCamera, OffsetX, OffsetY);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelsCorrectionMapOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMapOffset64(void* hCamera, int* OffsetX, int* OffsetY);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelsCorrectionMapOffset", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMapOffset32(void* hCamera, int* OffsetX, int* OffsetY);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMapOffset(void* hCamera, int* OffsetX, int* OffsetY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelsCorrectionMapOffset64(hCamera, OffsetX, OffsetY) : Camera_getPixelsCorrectionMapOffset32(hCamera, OffsetX, OffsetY);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getPixelsCorrectionMapSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMapSize64(void* hCamera, uint* MapSize);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getPixelsCorrectionMapSize" , CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMapSize32(void* hCamera, uint* MapSize);
        public static unsafe
        SVSGigeApiReturn
        Camera_getPixelsCorrectionMapSize(void* hCamera, uint* MapSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getPixelsCorrectionMapSize64(hCamera, MapSize) : Camera_getPixelsCorrectionMapSize32(hCamera, MapSize);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getMaximalPixelsCorrectionMapSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMaximalPixelsCorrectionMapSize64(void* hCamera, uint* MaximalprogrammedMapSize);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getMaximalPixelsCorrectionMapSize", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMaximalPixelsCorrectionMapSize32(void* hCamera, uint* MaximalprogrammedMapSize);
        public static unsafe
        SVSGigeApiReturn
        Camera_getMaximalPixelsCorrectionMapSize(void* hCamera, uint* MaximalprogrammedMapSize)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getMaximalPixelsCorrectionMapSize64(hCamera, MaximalprogrammedMapSize) : Camera_getMaximalPixelsCorrectionMapSize32(hCamera, MaximalprogrammedMapSize);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setMapIndexCoordinate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setMapIndexCoordinate64(void* hCamera, uint MapIndex, uint X_Coordinate, uint Y_Coordinate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setMapIndexCoordinate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setMapIndexCoordinate32(void* hCamera, uint MapIndex, uint X_Coordinate, uint Y_Coordinate);
        public static unsafe
        SVSGigeApiReturn
        Camera_setMapIndexCoordinate(void* hCamera, uint MapIndex, uint X_Coordinate, uint Y_Coordinate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setMapIndexCoordinate64(hCamera, MapIndex, X_Coordinate, Y_Coordinate) : Camera_setMapIndexCoordinate32(hCamera, MapIndex, X_Coordinate, Y_Coordinate);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getMapIndexCoordinate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMapIndexCoordinate64(void* hCamera, uint MapIndex, uint* CoordinateX, uint* CoordinateY);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getMapIndexCoordinate", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMapIndexCoordinate32(void* hCamera, uint MapIndex, uint* CoordinateX, uint* CoordinateY);
        public static unsafe
        SVSGigeApiReturn
        Camera_getMapIndexCoordinate(void* hCamera, uint MapIndex, uint* CoordinateX, uint* CoordinateY)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getMapIndexCoordinate64(hCamera, MapIndex, CoordinateX, CoordinateY) : Camera_getMapIndexCoordinate32(hCamera, MapIndex, CoordinateX, CoordinateY);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_deletePixelCoordinateFromMap", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_deletePixelCoordinateFromMap64(void* hCamera, uint MapIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_deletePixelCoordinateFromMap", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_deletePixelCoordinateFromMap32(void* hCamera, uint MapIndex);
        public static unsafe
        SVSGigeApiReturn
        Camera_deletePixelCoordinateFromMap(void* hCamera, uint MapIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_deletePixelCoordinateFromMap64(hCamera, MapIndex) : Camera_deletePixelCoordinateFromMap32(hCamera, MapIndex);
        }

        //------------------------------------------------------------------------------
        // 22 -  Special Control: IOMux configuration
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getMaxIOMuxIN", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMaxIOMuxIN64(void* hCamera, int* MaxIOMuxINSignals);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getMaxIOMuxIN", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMaxIOMuxIN32(void* hCamera, int* MaxIOMuxINSignals);
        public static unsafe
        SVSGigeApiReturn
        Camera_getMaxIOMuxIN(void* hCamera, int* MaxIOMuxINSignals)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getMaxIOMuxIN64(hCamera, MaxIOMuxINSignals) : Camera_getMaxIOMuxIN32(hCamera, MaxIOMuxINSignals);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getMaxIOMuxOUT", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMaxIOMuxOUT64(void* hCamera, int* MaxIOMuxOUTSignals);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getMaxIOMuxOUT", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getMaxIOMuxOUT32(void* hCamera, int* MaxIOMuxOUTSignals);
        public static unsafe
        SVSGigeApiReturn
        Camera_getMaxIOMuxOUT(void* hCamera, int* MaxIOMuxOUTSignals)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getMaxIOMuxOUT64( hCamera, MaxIOMuxOUTSignals) : Camera_getMaxIOMuxOUT32(hCamera, MaxIOMuxOUTSignals);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setIOAssignment", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIOAssignment64(void* hCamera, SVGigE_IOMux_OUT IOMuxOUT, SVGigE_IOMux_IN SignalIOMuxIN);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setIOAssignment", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIOAssignment32(void* hCamera, SVGigE_IOMux_OUT IOMuxOUT, SVGigE_IOMux_IN SignalIOMuxIN);
        public static unsafe
        SVSGigeApiReturn
        Camera_setIOAssignment(void* hCamera, SVGigE_IOMux_OUT IOMuxOUT, SVGigE_IOMux_IN SignalIOMuxIN)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setIOAssignment64(hCamera, IOMuxOUT, SignalIOMuxIN) : Camera_setIOAssignment32(hCamera, IOMuxOUT, SignalIOMuxIN);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getIOAssignment", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getIOAssignment64(void* hCamera, SVGigE_IOMux_OUT IOMuxOUT, uint* IOMuxIN);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getIOAssignment", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getIOAssignment32(void* hCamera, SVGigE_IOMux_OUT IOMuxOUT, uint* IOMuxIN);
        public static unsafe
        SVSGigeApiReturn
        Camera_getIOAssignment(void* hCamera, SVGigE_IOMux_OUT IOMuxOUT, uint* IOMuxIN)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getIOAssignment64(hCamera, IOMuxOUT, IOMuxIN) : Camera_getIOAssignment32( hCamera, IOMuxOUT, IOMuxIN);
        }

        //------------------------------------------------------------------------------
        // 23 - Special Control: IO control
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setIOMuxIN", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIOMuxIN64(void* hCamera, uint VectorIOMuxIN);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setIOMuxIN", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIOMuxIN32(void* hCamera, uint VectorIOMuxIN);
        public static unsafe
        SVSGigeApiReturn
        Camera_setIOMuxIN(void* hCamera, uint VectorIOMuxIN)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setIOMuxIN64( hCamera, VectorIOMuxIN) : Camera_setIOMuxIN32(hCamera, VectorIOMuxIN);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getIOMuxIN", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getIOMuxIN64(void* hCamera, uint* VectorIOMuxIN);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getIOMuxIN", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getIOMuxIN32(void* hCamera, uint* VectorIOMuxIN);
        public static unsafe
        SVSGigeApiReturn
        Camera_getIOMuxIN(void* hCamera, uint* VectorIOMuxIN)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getIOMuxIN64(hCamera, VectorIOMuxIN) : Camera_getIOMuxIN32(hCamera, VectorIOMuxIN);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setIO", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIO64(void* hCamera, SVGigE_IOMux_IN SignalIOMuxIN, SVGigE_IO_Signal SignalValue);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setIO", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setIO32(void* hCamera, SVGigE_IOMux_IN SignalIOMuxIN, SVGigE_IO_Signal SignalValue);
        public static unsafe
        SVSGigeApiReturn
        Camera_setIO(void* hCamera, SVGigE_IOMux_IN SignalIOMuxIN, SVGigE_IO_Signal SignalValue)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setIO64(hCamera, SignalIOMuxIN, SignalValue) : Camera_setIO32(hCamera, SignalIOMuxIN, SignalValue);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getIO", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getIO64(void* hCamera, SVGigE_IOMux_IN SignalIOMuxIN, SVGigE_IO_Signal* SignalValue);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getIO", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getIO32(void* hCamera, SVGigE_IOMux_IN SignalIOMuxIN, SVGigE_IO_Signal* SignalValue);
        public static unsafe
        SVSGigeApiReturn
        Camera_getIO(void* hCamera, SVGigE_IOMux_IN SignalIOMuxIN, SVGigE_IO_Signal* SignalValue)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getIO64(hCamera, SignalIOMuxIN, SignalValue) : Camera_getIO32( hCamera, SignalIOMuxIN,  SignalValue);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setAcqLEDOverride", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAcqLEDOverride64(void* hCamera, bool isOverrideActive);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setAcqLEDOverride", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setAcqLEDOverride32(void* hCamera, bool isOverrideActive);

        public static unsafe
        SVSGigeApiReturn
        Camera_setAcqLEDOverride(void* hCamera, bool isOverrideActive)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setAcqLEDOverride64(hCamera, isOverrideActive) : Camera_setAcqLEDOverride32(hCamera, isOverrideActive);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getAcqLEDOverride", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAcqLEDOverride64(void* hCamera, bool* isOverrideActive);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getAcqLEDOverride", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getAcqLEDOverride32(void* hCamera, bool* isOverrideActive);
        public static unsafe
        SVSGigeApiReturn
        Camera_getAcqLEDOverride(void* hCamera, bool* isOverrideActive)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getAcqLEDOverride64( hCamera, isOverrideActive) : Camera_getAcqLEDOverride32( hCamera, isOverrideActive);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLEDIntensity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLEDIntensity64(void* hCamera, int LEDIntensity);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLEDIntensity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLEDIntensity32(void* hCamera, int LEDIntensity);

        public static unsafe
        SVSGigeApiReturn
        Camera_setLEDIntensity(void* hCamera, int LEDIntensity)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLEDIntensity64(hCamera, LEDIntensity) : Camera_setLEDIntensity32( hCamera, LEDIntensity);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLEDIntensity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLEDIntensity64(void* hCamera, int* LEDIntensity);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLEDIntensity", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLEDIntensity32(void* hCamera, int* LEDIntensity);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLEDIntensity(void* hCamera, int* LEDIntensity)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLEDIntensity64(hCamera, LEDIntensity) : Camera_getLEDIntensity32(hCamera, LEDIntensity);
        }
        //------------------------------------------------------------------------------
        // 24 - Special control: Serial communication
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setUARTBuffer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setUARTBuffer64(void* hCamera, byte* Data, int DataLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setUARTBuffer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setUARTBuffer32(void* hCamera, byte* Data, int DataLength);
        public static unsafe
        SVSGigeApiReturn
        Camera_setUARTBuffer(void* hCamera, byte* Data, int DataLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setUARTBuffer64(hCamera, Data, DataLength) : Camera_setUARTBuffer32(hCamera, Data, DataLength);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getUARTBuffer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getUARTBuffer64(void* hCamera, sbyte* Data, int* DataLengthReceived, int DataLengthMax, float Timeout);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getUARTBuffer", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getUARTBuffer32(void* hCamera, sbyte* Data, int* DataLengthReceived, int DataLengthMax, float Timeout);
        public static unsafe
        SVSGigeApiReturn
        Camera_getUARTBuffer(void* hCamera, sbyte* Data, int* DataLengthReceived, int DataLengthMax, float Timeout)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getUARTBuffer64(hCamera, Data, DataLengthReceived, DataLengthMax, Timeout) : Camera_getUARTBuffer32(hCamera, Data, DataLengthReceived, DataLengthMax, Timeout);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setUARTBaud", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setUARTBaud64(void* hCamera, SVGigE_BaudRate BaudRate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setUARTBaud", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setUARTBaud32(void* hCamera, SVGigE_BaudRate BaudRate);
        public static unsafe
        SVSGigeApiReturn
        Camera_setUARTBaud(void* hCamera, SVGigE_BaudRate BaudRate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setUARTBaud64(hCamera, BaudRate) : Camera_setUARTBaud32(hCamera, BaudRate);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getUARTBaud", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getUARTBaud64(void* hCamera, SVGigE_BaudRate* BaudRate);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getUARTBaud", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getUARTBaud32(void* hCamera, SVGigE_BaudRate* BaudRate);
        public static unsafe
        SVSGigeApiReturn
        Camera_getUARTBaud(void* hCamera, SVGigE_BaudRate* BaudRate)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getUARTBaud64( hCamera, BaudRate) : Camera_getUARTBaud32(hCamera, BaudRate);
        }


        //------------------------------------------------------------------------------
        // 25 - Special control: Direct register and memory access
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setGigECameraRegister", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setGigECameraRegister64(void* hCamera, uint RegisterAddress, uint RegisterValue, uint GigECameraAccessKey);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setGigECameraRegister", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setGigECameraRegister32(void* hCamera, uint RegisterAddress, uint RegisterValue, uint GigECameraAccessKey);
        public static unsafe
        SVSGigeApiReturn
        Camera_setGigECameraRegister(void* hCamera, uint RegisterAddress, uint RegisterValue, uint GigECameraAccessKey)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setGigECameraRegister64(hCamera, RegisterAddress, RegisterValue, GigECameraAccessKey) : Camera_setGigECameraRegister32(hCamera, RegisterAddress, RegisterValue, GigECameraAccessKey);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getGigECameraRegister", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getGigECameraRegister64(void* hCamera, uint RegisterAddress, uint* ProgammedRegisterValue, uint GigECameraAccessKey);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getGigECameraRegister", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getGigECameraRegister32(void* hCamera, uint RegisterAddress, uint* ProgammedRegisterValue, uint GigECameraAccessKey);
        public static unsafe
        SVSGigeApiReturn
        Camera_getGigECameraRegister(void* hCamera, uint RegisterAddress, uint* ProgammedRegisterValue, uint GigECameraAccessKey)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getGigECameraRegister64(hCamera, RegisterAddress, ProgammedRegisterValue, GigECameraAccessKey) : Camera_getGigECameraRegister32(hCamera, RegisterAddress, ProgammedRegisterValue, GigECameraAccessKey);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_writeGigECameraMemory", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_writeGigECameraMemory64(void* hCamera, uint MemoryAddress, sbyte* DataBlock, uint DataLength, uint GigECameraAccessKey);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_writeGigECameraMemory", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_writeGigECameraMemory32(void* hCamera, uint MemoryAddress, sbyte* DataBlock, uint DataLength, uint GigECameraAccessKey);
        public static unsafe
        SVSGigeApiReturn
        Camera_writeGigECameraMemory(void* hCamera, uint MemoryAddress, sbyte* DataBlock, uint DataLength, uint GigECameraAccessKey)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_writeGigECameraMemory64(hCamera, MemoryAddress, DataBlock, DataLength, GigECameraAccessKey) : Camera_writeGigECameraMemory32( hCamera, MemoryAddress,  DataBlock, DataLength, GigECameraAccessKey);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_readGigECameraMemory", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_readGigECameraMemory64(void* hCamera, uint MemoryAddress, sbyte* DataBlock, uint DataLength, uint GigECameraAccessKey);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_readGigECameraMemory", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_readGigECameraMemory32(void* hCamera, uint MemoryAddress, sbyte* DataBlock, uint DataLength, uint GigECameraAccessKey);
        public static unsafe
        SVSGigeApiReturn
        Camera_readGigECameraMemory(void* hCamera, uint MemoryAddress, sbyte* DataBlock, uint DataLength, uint GigECameraAccessKey)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_readGigECameraMemory64(hCamera, MemoryAddress, DataBlock, DataLength, GigECameraAccessKey) : Camera_readGigECameraMemory32(hCamera, MemoryAddress,  DataBlock, DataLength, GigECameraAccessKey);
        }

        //------------------------------------------------------------------------------
        // 26 - Special control: Persistent settings and recovery
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_writeEEPROM", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn 
        Camera_writeEEPROM64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_writeEEPROM", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_writeEEPROM32(void* hCamera);
        public static unsafe
        SVSGigeApiReturn
        Camera_writeEEPROM(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_writeEEPROM64(hCamera) : Camera_writeEEPROM32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_readEEPROM", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_readEEPROM64(void* hCamera);
        
        [DllImport(SVGigE_DLL, EntryPoint = "Camera_readEEPROM", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_readEEPROM32(void* hCamera);
        public static unsafe
        SVSGigeApiReturn
        Camera_readEEPROM(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_readEEPROM64( hCamera) : Camera_readEEPROM32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_restoreFactoryDefaults", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_restoreFactoryDefaults64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_restoreFactoryDefaults", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_restoreFactoryDefaults32(void* hCamera);
        public static unsafe
        SVSGigeApiReturn
        Camera_restoreFactoryDefaults(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_restoreFactoryDefaults64(hCamera) : Camera_restoreFactoryDefaults32(hCamera);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_loadSettingsFromXml", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_loadSettingsFromXml64(void* hCamera, sbyte* Filename);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_loadSettingsFromXml", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_loadSettingsFromXml32(void* hCamera, sbyte* Filename);
        public static unsafe
        SVSGigeApiReturn
        Camera_loadSettingsFromXml(void* hCamera, sbyte* Filename)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_loadSettingsFromXml64( hCamera,  Filename) : Camera_loadSettingsFromXml32( hCamera,  Filename);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_SaveSettingsToXml", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_SaveSettingsToXml64(void* hCamera, sbyte* Filename);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_SaveSettingsToXml", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_SaveSettingsToXml32(void* hCamera, sbyte* Filename);
        public static unsafe
        SVSGigeApiReturn
        Camera_SaveSettingsToXml(void* hCamera, sbyte* Filename)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_SaveSettingsToXml64( hCamera,  Filename) : Camera_SaveSettingsToXml32(hCamera, Filename);
        }

        //------------------------------------------------------------------------------
        // 27 - General functions
        //------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "SVGigE_estimateWhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_estimateWhiteBalance64(byte* BufferRGB, int BufferLength, float* Red, float* Green, float* Blue);

        [DllImport(SVGigE_DLL, EntryPoint = "SVGigE_estimateWhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_estimateWhiteBalance32(byte* BufferRGB, int BufferLength, float* Red, float* Green, float* Blue);
        public static unsafe
        SVSGigeApiReturn
        SVGigE_estimateWhiteBalance(byte* BufferRGB, int BufferLength, float* Red, float* Green, float* Blue)
        {
            return IntPtr.Size == 8 /* 64bit */ ? SVGigE_estimateWhiteBalance64(BufferRGB, BufferLength, Red, Green, Blue) : SVGigE_estimateWhiteBalance32(BufferRGB, BufferLength, Red, Green, Blue);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "SVGigE_estimateWhiteBalanceExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            SVGigE_estimateWhiteBalanceExtended64(byte* BufferRGB, int PixelNumber, int* Red, int* Green, int* Blue, SVGIGE_Whitebalance_SELECT Whitebalance_Art);
        
        [DllImport(SVGigE_DLL, EntryPoint = "SVGigE_estimateWhiteBalanceExtended", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            SVGigE_estimateWhiteBalanceExtended32(byte* BufferRGB, int PixelNumber, int* Red, int* Green, int* Blue, SVGIGE_Whitebalance_SELECT Whitebalance_Art);
        public static unsafe
            SVSGigeApiReturn
            SVGigE_estimateWhiteBalanceExtended(byte* BufferRGB, int PixelNumber, int* Red, int* Green, int* Blue, SVGIGE_Whitebalance_SELECT Whitebalance_Art)
        {
            return IntPtr.Size == 8 /* 64bit */ ? SVGigE_estimateWhiteBalanceExtended64(BufferRGB, PixelNumber, Red, Green, Blue, Whitebalance_Art)
                                                : SVGigE_estimateWhiteBalanceExtended32(BufferRGB, PixelNumber, Red, Green, Blue, Whitebalance_Art) ;
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "SVGigE_writeImageToBitmapFile", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_writeImageToBitmapFile64(sbyte* Filename, byte* Data, int SizeX, int SizeY, GVSP_PIXEL_TYPE PixelType);

        [DllImport(SVGigE_DLL, EntryPoint = "SVGigE_writeImageToBitmapFile", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_writeImageToBitmapFile32(sbyte* Filename, byte* Data, int SizeX, int SizeY, GVSP_PIXEL_TYPE PixelType);
        public static unsafe
        SVSGigeApiReturn
        SVGigE_writeImageToBitmapFile(sbyte* Filename, byte* Data, int SizeX, int SizeY, GVSP_PIXEL_TYPE PixelType)
        {
            return IntPtr.Size == 8 /* 64bit */ ? SVGigE_writeImageToBitmapFile64(Filename, Data, SizeX, SizeY, PixelType)
                                                : SVGigE_writeImageToBitmapFile32(Filename, Data, SizeX, SizeY, PixelType);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "SVGigE_installFilterDriver", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_installFilterDriver64(sbyte* PathToDriverPackage, sbyte* FilenameINF, sbyte* FilenameINF_m);

        [DllImport(SVGigE_DLL, EntryPoint = "SVGigE_installFilterDriver", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_installFilterDriver32(sbyte* PathToDriverPackage, sbyte* FilenameINF, sbyte* FilenameINF_m);
        public static unsafe
        SVSGigeApiReturn
        SVGigE_installFilterDriver(sbyte* PathToDriverPackage, sbyte* FilenameINF, sbyte* FilenameINF_m)
        {
            return IntPtr.Size == 8 /* 64bit */ ? SVGigE_installFilterDriver64(PathToDriverPackage, FilenameINF, FilenameINF_m) : SVGigE_installFilterDriver32(PathToDriverPackage, FilenameINF, FilenameINF_m);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "SVGigE_uninstallFilterDriver", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_uninstallFilterDriver64();

        [DllImport(SVGigE_DLL, EntryPoint = "SVGigE_uninstallFilterDriver", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        SVGigE_uninstallFilterDriver32();
        public static unsafe
        SVSGigeApiReturn
        SVGigE_uninstallFilterDriver()
        {
            return IntPtr.Size == 8 /* 64bit */ ? SVGigE_uninstallFilterDriver64() : SVGigE_uninstallFilterDriver32();
        }



        //------------------------------------------------------------------------------
        // 28 - Diagnostics
        //------------------------------------------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "_Error_getMessage", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            sbyte*
            _Error_getMessage64(SVSGigeApiReturn returnCode);

        [DllImport(SVGigE_DLL, EntryPoint = "_Error_getMessage", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            sbyte*
            _Error_getMessage32(SVSGigeApiReturn returnCode);
        public static unsafe
            sbyte*
            _Error_getMessage(SVSGigeApiReturn returnCode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? _Error_getMessage64( returnCode) : _Error_getMessage32( returnCode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_registerForLogMessages", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_registerForLogMessages64(void* hCamera, int LogLevel, sbyte* LogFilename, LogMessageCallback LogCallback, void* MessageContext);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_registerForLogMessages", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_registerForLogMessages32(void* hCamera, int LogLevel, sbyte* LogFilename, LogMessageCallback LogCallback, void* MessageContext);
        public static unsafe
            SVSGigeApiReturn
            Camera_registerForLogMessages(void* hCamera, int LogLevel, sbyte* LogFilename, LogMessageCallback LogCallback, void* MessageContext)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_registerForLogMessages64(hCamera, LogLevel, LogFilename, LogCallback, MessageContext) : Camera_registerForLogMessages32(hCamera, LogLevel, LogFilename, LogCallback, MessageContext);
        }

        //-----------------------------------------------------------------------------------------------
        // 28 - Special control: Lens control
        //-----------------------------------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_isLensAvailable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_isLensAvailable64(void* hCamera, bool* isAvailable);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_isLensAvailable", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_isLensAvailable32(void* hCamera, bool* isAvailable);
        public static unsafe
        SVSGigeApiReturn
        Camera_isLensAvailable(void* hCamera, bool* isAvailable)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_isLensAvailable64(hCamera, isAvailable) : Camera_isLensAvailable32(hCamera, isAvailable);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getLensName64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensName", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        sbyte*
        Camera_getLensName32(void* hCamera);
        public static unsafe
        sbyte*
        Camera_getLensName(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensName64(hCamera) : Camera_getLensName32(hCamera);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLensFocalLength", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensFocalLength64(void* hCamera, uint FocalLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLensFocalLength", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensFocalLength32(void* hCamera, uint FocalLength);
        public static unsafe
        SVSGigeApiReturn
        Camera_setLensFocalLength(void* hCamera, uint FocalLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLensFocalLength64(hCamera, FocalLength) : Camera_setLensFocalLength32(hCamera, FocalLength);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensFocalLength", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLength64(void* hCamera, uint* FocalLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensFocalLength", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLength32(void* hCamera, uint* FocalLength);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLength(void* hCamera, uint* FocalLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensFocalLength64(hCamera, FocalLength) : Camera_getLensFocalLength32(hCamera, FocalLength);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensFocalLengthMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLengthMin64(void* hCamera, uint* FocalLengthMin);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensFocalLengthMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLengthMin32(void* hCamera, uint* FocalLengthMin);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLengthMin(void* hCamera, uint* FocalLengthMin)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensFocalLengthMin64(hCamera, FocalLengthMin) : Camera_getLensFocalLengthMin32(hCamera, FocalLengthMin);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensFocalLengthMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLengthMax64(void* hCamera, uint* FocalLengthMax);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensFocalLengthMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLengthMax32(void* hCamera, uint* FocalLengthMax);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensFocalLengthMax(void* hCamera, uint* FocalLengthMax)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensFocalLengthMax64(hCamera, FocalLengthMax) : Camera_getLensFocalLengthMax32(hCamera, FocalLengthMax);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLensFocusUnit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensFocusUnit64(void* hCamera, FOCUS_UNIT Selected_unit);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLensFocusUnit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensFocusUnit32(void* hCamera, FOCUS_UNIT Selected_unit);
        public static unsafe
        SVSGigeApiReturn
        Camera_setLensFocusUnit(void* hCamera, FOCUS_UNIT Selected_unit)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLensFocusUnit64(hCamera, Selected_unit) : Camera_setLensFocusUnit32(hCamera, Selected_unit);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensFocusUnit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocusUnit64(void* hCamera, FOCUS_UNIT* Selected_unit);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensFocusUnit", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocusUnit32(void* hCamera, FOCUS_UNIT* Selected_unit);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensFocusUnit(void* hCamera, FOCUS_UNIT* Selected_unit)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensFocusUnit64(hCamera, Selected_unit) : Camera_getLensFocusUnit32( hCamera, Selected_unit);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLensFocus", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensFocus64(void* hCamera, uint Focus);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLensFocus", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensFocus32(void* hCamera, uint Focus);
        public static unsafe
        SVSGigeApiReturn
        Camera_setLensFocus(void* hCamera, uint Focus)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLensFocus64(hCamera, Focus) : Camera_setLensFocus32(hCamera, Focus);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensFocus", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocus64(void* hCamera, uint* Focus);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensFocus", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocus32(void* hCamera, uint* Focus);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensFocus(void* hCamera, uint* Focus)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensFocus64(hCamera, Focus) : Camera_getLensFocus32(hCamera, Focus);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensFocusMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocusMin64(void* hCamera, uint* FocusMin);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensFocusMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensFocusMin32(void* hCamera, uint* FocusMin);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensFocusMin(void* hCamera, uint* FocusMin)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensFocusMin64(hCamera, FocusMin) : Camera_getLensFocusMin32(hCamera, FocusMin);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensFocusMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getLensFocusMax64(void* hCamera, uint* FocusMax);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensFocusMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getLensFocusMax32(void* hCamera, uint* FocusMax);
        public static unsafe
            SVSGigeApiReturn
            Camera_getLensFocusMax(void* hCamera, uint* FocusMax)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensFocusMax64(hCamera, FocusMax) : Camera_getLensFocusMax32(hCamera, FocusMax);
        }


        //------------------------------------- Aperture   ----------------------------------------------
        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLensAperture", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensAperture64(void* hCamera, uint Aperture);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLensAperture", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setLensAperture32(void* hCamera, uint Aperture);
        public static unsafe
        SVSGigeApiReturn
        Camera_setLensAperture(void* hCamera, uint Aperture)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLensAperture64(hCamera, Aperture) : Camera_setLensAperture32(hCamera, Aperture);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensAperture", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensAperture64(void* hCamera, uint* Aperture);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensAperture", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensAperture32(void* hCamera, uint* Aperture);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensAperture(void* hCamera, uint* Aperture)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensAperture64(hCamera, Aperture) : Camera_getLensAperture32(hCamera, Aperture);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensApertureMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensApertureMin64(void* hCamera, uint* ApertureMin);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensApertureMin", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensApertureMin32(void* hCamera, uint* ApertureMin);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensApertureMin(void* hCamera, uint* ApertureMin)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensApertureMin64(hCamera, ApertureMin) : Camera_getLensApertureMin32(hCamera, ApertureMin);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensApertureMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensApertureMax64(void* hCamera, uint* ApertureMax);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensApertureMax", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getLensApertureMax32(void* hCamera, uint* ApertureMax);
        public static unsafe
        SVSGigeApiReturn
        Camera_getLensApertureMax(void* hCamera, uint* ApertureMax)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensApertureMax64(hCamera, ApertureMax) : Camera_getLensApertureMax32(hCamera, ApertureMax);
        }

		
		 //------------------------------------- LensReset   ----------------------------------------------
		[DllImport(SVGigE_DLL64, EntryPoint = "Camera_LensReset", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_LensReset64(void* hCamera);
		[DllImport(SVGigE_DLL, EntryPoint = "Camera_LensReset", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_LensReset32(void* hCamera);
		public static unsafe
		SVSGigeApiReturn
		Camera_LensReset(void* hCamera)
		{
			return IntPtr.Size == 8 /* 64bit */ ? Camera_LensReset64(hCamera) : Camera_LensReset32(hCamera);
		}

		 //------------------------------------- LensUpdate   ----------------------------------------------
		[DllImport(SVGigE_DLL64, EntryPoint = "Camera_LensUpdate", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_LensUpdate64(void* hCamera);
		[DllImport(SVGigE_DLL, EntryPoint = "Camera_LensUpdate", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_LensUpdate32(void* hCamera);
		public static unsafe
		SVSGigeApiReturn
		Camera_LensUpdate(void* hCamera)
		{
			return IntPtr.Size == 8 /* 64bit */ ? Camera_LensUpdate64(hCamera) : Camera_LensUpdate32(hCamera);
		}

		 //------------------------------------- LensState   ----------------------------------------------
		[DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLensState", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_setLensState64(void* hCamera, LENS_STATE LensState);
		[DllImport(SVGigE_DLL, EntryPoint = "Camera_setLensState", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_setLensState32(void* hCamera, LENS_STATE LensState);
		public static unsafe
		SVSGigeApiReturn
		Camera_setLensState(void* hCamera, LENS_STATE LensState)
		{
			return IntPtr.Size == 8 /* 64bit */ ? Camera_setLensState64(hCamera, LensState) : Camera_setLensState32(hCamera, LensState);
		}

		[DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLensState", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_getLensState64(void* hCamera, LENS_STATE *LensState);
		[DllImport(SVGigE_DLL, EntryPoint = "Camera_getLensState", CallingConvention = CallingConvention.Cdecl)] 
		private static extern unsafe
		SVSGigeApiReturn
		Camera_getLensState32(void* hCamera, LENS_STATE *LensState);
		public static unsafe
		SVSGigeApiReturn
		Camera_getLensState(void* hCamera, LENS_STATE *LensState)
		{
				return IntPtr.Size == 8 /* 64bit */ ? Camera_getLensState64(hCamera, LensState) : Camera_getLensState32(hCamera, LensState);
		}
		
		

         /**********************************************************************/

        //------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------------------------		  

        /** Streaming channel callback function.
        *  The user-defined streaming channel callback function will be called each
        *  time when an image acquisition from camera has been finished and the image
        *  is ready for processing
        *
        *  NOTE: The callback function will 	return a NULL pointer in case an image
        *        could not be completely received over the network due to a timeout,
        *        e.g. in the result of insufficient bandwidth
        */

        public delegate SVSGigeApiReturn StreamCallback([MarshalAs(UnmanagedType.I4)] int Image, IntPtr Context);


        /** Messaging channel callback function.
        *  The user-defined messaging channel callback function will be called each time
        *  when an event was signaled which arrived from a camera or from intermediate software
        *  layers.
        *
        *  An application should retrieve, process appropriately and finally release any message
        *  that has arrived when the event callback function gets control. There might be one or
        *  multiple messages waiting for processing in a FIFO on entry to the callback function.
        *
        *  HINT: If the size of the FIFO was not sufficient for handling all messages that
        *  arrived from one callback to the next callback then an exception will be raised.
        *  An application must catch this exception and should react with an appropriate
        *  user message, log entry or the like which informs an operator about this
        *  exceptional situation.
        */

        public delegate SVSGigeApiReturn EventCallback(IntPtr EventID, IntPtr Context);


        /** Register for log messages.
        *  Log messages can be requested for various log levels:
        *  0 - logging off
        *  1 - CRITICAL errors that prevent from further operation
        *  2 - ERRORs that prevent from proper functioning
        *  3 - WARNINGs which usually do not affect proper work
        *  4 - INFO for listing camera communication (default)
        *  5 - DIAGNOSTICS for investigating image callbacks
        *  6 - DEBUG for receiving multiple parameters for image callbacks
        *  7 - DETAIL for receiving multiple signals for each image callback
        *
        *  Resulting log messages can be either written into a log file
        *  respectively they can be received by a callback and further
        *  processed by an application.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LogLevel one of the above log levels
        *  @param LogFilename a filename where all log messages will be written to or NULL
        *  @param LogCallback a callback function that will receive all log messages or NULL
        *  @param MessageContext a context that will be 	returned to application with each callback or NULL
        *  @	return success or error code
        */

        public delegate SVSGigeApiReturn LogMessageCallback([MarshalAs(UnmanagedType.I4)] sbyte LogMessage, IntPtr MessageContext);



        //-----------------------------------------------------------------------------------------------
        // 1 - Camera: Discovery and bookkeeping
        //-----------------------------------------------------------------------------------------------
        /**
        * Create camera container.
        * A handle is obtained that references a management object for discovering
        * and selecting GigE cameras in the network segments that a computer is
        * connected to.
        *
        * @	return on success a handle for subsequent camera container function calls
        *         or -1 if creating a camera container failed
        */
        public unsafe
        int
        Gige_CameraContainer_create(SVGigETL_Type TransportLayerType)
        {
            return CameraContainer_create(TransportLayerType);
        }

        /**
        * Delete Camera container.
        * A previously created camera container reference will be released. After
        * deleting a camera container all camera specific functions are no longer
        * available
        *
        * @param hCameraContainer a handle received from CameraContainer_create()
        */
        public unsafe
        void
        Gige_CameraContainer_delete(int hCameraContainer)
        {
            CameraContainer_delete(hCameraContainer);
        }

        /**
        * Device discovery.
        * All network segments that a computer is connected to will be serached for
        * GigE cameras by sending out a network broadcast and subsequently analyzing
        * camera responses.
        *
        * @param hCameraContainer a handle received from CameraContainer_create()
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_CameraContainer_discovery(int hCameraContainer)
        {
            return CameraContainer_discovery(hCameraContainer);
        }

        /**
        * Get number of connected cameras.
        * The maximal index is 	returned that cameras can be accessed with in the
        * camera container
        *
        * @param hCameraContainer a handle received from CameraContainer_create()
        * @	return number of available cameras
        */
        public unsafe
        int
        Gige_CameraContainer_getNumberOfCameras(int hCameraContainer)
        {
            return CameraContainer_getNumberOfCameras(hCameraContainer);
        }

        /**
        * Get camera.
        * A camera handle is obtained accordingly to an index that was specified
        * as an input parameter
        *
        * @param hCameraContainer a handle received from CameraContainer_create()
        * @param CameraIndex camera index from zero to one less the number of available cameras
        * @	return a handle for subsequent camera function calls or NULL in case the
        *         index was specified out of range
        */
        public unsafe
        IntPtr
        Gige_CameraContainer_getCamera(int hCameraContainer, int CameraIndex)
        {

            void* ptr = CameraContainer_getCamera(hCameraContainer, CameraIndex);
            return new IntPtr(ptr);
        }


        /** Find camera.
         *  A camera handle is obtained accordingly to a search string that will be
         *  matched against the following items:
         *   - MAC address
         *   - IP address
         *   - serial number
         *   - user defined name
         *   If one of those items can be matched a valid handle is returned.
         *   Otherwise a SVGigE_NO_CAMERA return value will be generated
         *
         *  @param hCameraContainer a handle received from CameraContainer_create()
         *  @param CameraItem a string for matching it against the above items
         *  @return a handle for subsequent camera function calls or NULL in case the
         *          index was specified out of range
         */
        public unsafe
        IntPtr
        Gige_CameraContainer_findCamera(int hCameraContainer, string CameraItem)
        {

            byte[] CameraItem_bytes = Encoding.ASCII.GetBytes(CameraItem);
            fixed (byte* CameraItem_p = CameraItem_bytes)
            {
                sbyte* CameraItem_ps = (sbyte*)CameraItem_p;
                void* vp = CameraContainer_findCamera(hCameraContainer, CameraItem_ps);
                IntPtr ip = new IntPtr(vp);
                return ip;
            }
        }
        //------------------------------------------------------------------------------
        // 2 - Camera: Connection
        //------------------------------------------------------------------------------

        /**
        * Open connection to camera.
        * A TCP/IP control channel will be established.
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_openConnection(IntPtr hCamera, float timeout)
        {
            void* c = hCamera.ToPointer();
            SVSGigeApiReturn ret = Camera_openConnection(c, (float)timeout);
            return ret;
        }

        /** Open connection to camera.
        *  A TCP/IP control channel will be established.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param HeartbeatTimeout the time without traffic or heartbeat after which a camera drops a connection (default: 3.0 sec.)
        *  NOTE: Values between 0.0 to 0.5 sec. will be mapped to the default value (3.0 sec.)
        *  HINT: It is recommended to use an extended timeout for debug sessions (e.g. 30 sec.). 
        *  This prevents from loosing connection to a camera due to missing hartbeat when 
        *  stepping through a program in debug mode.
        *  @param GVCPRetryCount Retrys before giving up, valid values between 1 and n. Default is 3
        *  @param GVCPTimeout Timeout before next retry in msec. Default is 200 msec
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_openConnectionEx(IntPtr hCamera, float HeartbeatTimeout, int GVCPRetryCount, int GVCPTimeout)
        {
            void* cam = hCamera.ToPointer();
            return Camera_openConnectionEx(cam, HeartbeatTimeout, GVCPRetryCount, GVCPTimeout);
        }

        /**
        * Disconnect camera.
        * The TCP/IP control channel will be closed
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return success or error code
        ****/
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_closeConnection(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_closeConnection(cam);
        }


        /** Set IP address.
        *  The camera will get a new persistent IP address assigned. If the camera
        *  is currently unavailable in the subnet where it is attached to, then a
        *  temporary IP address will be forced into the camera first. In any case
        *  the camera will have the new IP address assigned as a persistent IP 
        *  which will apply after camera's next reboot.
        *
        *  HINT:
        *  If an IP address is set that is not inside the subnet where the camera
        *  is currently connected to, then the camera becomes unavailable after next
        *  reboot. This can be avoided by having a valid IP address assigned automatically 
        *  by setting both values to zero, IPAddress and NetMask
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param IPAddress a valid IP address or zero (for automatically assigning a valid IP/netmask)
        *  @param NetMask a matching net mask or zero (for automatically assigning a valid IP/netmask)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setIPAddress(IntPtr hCamera, uint IPAddress, uint NetMask)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setIPAddress(cam, IPAddress, NetMask);
        }


        /** Force valid network settings
        *  A camera's availability will be evaluated. If it is outside current subnet 
        *  then it will be forced to valid network settings inside current subnet. 
        *  Valid network settings will be reported back to caller.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param IPAddress the new IP address that has been selected and forced
        *  @param SubnetMask the new subnet mask that has been selected and forced
        *  @	return success or error code
        *
        *  HINT: If the 	return code is SVGigE_SVCAM_STATUS_CAMERA_OCCUPIED then the 
        *        IPAdress will show the IP of the host that occupies the camera.
        */


        public unsafe
        SVSGigeApiReturn
        Gige_Camera_forceValidNetworkSettings(IntPtr hCamera, ref uint IPAddress, ref uint SubnetMask)
        {

            void* cam = hCamera.ToPointer();
            fixed (uint* iPAddress = &IPAddress)
            {
                fixed (uint* subnetMask = &SubnetMask)
                {
                    return Camera_forceValidNetworkSettings(cam, iPAddress, subnetMask);
                }
            }
        }

        //------------------------------------------------------------------------------
        // 3- Camera: Information
        //------------------------------------------------------------------------------


        /**
        * Get manufacturer name.
        * The manufacturer name that is obtained from the camera firmware will be
        * 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getManufacturerName(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            sbyte* strPtr = Camera_getManufacturerName(cam);
            string rstring = new string(strPtr);
            return rstring;
        }

        /**
        * Get model name.
        * The model name that is obtained from the camera firmware will be
        * 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getModelName(IntPtr hCamera)
        {
            sbyte* ascii;
            string rstring;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getModelName(cam);
            rstring = new string(ascii);
            return rstring;

        }
        public unsafe
        void
        Gige_Camera_getModelNameBytes(IntPtr hCamera, byte[] buffer)
        {
            int k = 0;
            int s = buffer.GetLength(0);
            void* cam = hCamera.ToPointer();
            sbyte* ascii = Camera_getModelName(cam);
            int count = 0;
            while (*ascii != 0 && count < s)
            {
                buffer[k] = (byte)(*ascii);
                ascii++;
                count++;
                k++;
            }

        }
        /**
        * Get device version.
        * The device version that is obtained from the camera firmware will be
        * 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getDeviceVersion(IntPtr hCamera)
        {
            sbyte* ascii;
            string rstring;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getDeviceVersion(cam);
            rstring = new string(ascii);
            return rstring;

        }

        /**
        * Get manufacturer specific information.
        * The manufacturer specific information that is obtained from the camera
        * firmware will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getManufacturerSpecificInformation(IntPtr hCamera)
        {
            sbyte* ascii;
            string rstring;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getManufacturerSpecificInformation(cam);
            rstring = new string(ascii);
            return rstring;


        }

        /**
        *  Get serial number.
        *  The (manufacturer-assigned) serial number will be obtained from the camera
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getSerialNumber(IntPtr hCamera)
        {
            sbyte* ascii;
            string rstring;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getSerialNumber(cam);
            rstring = new string(ascii);
            return rstring;

        }

        /**
        *  Set user-defined name
        *  A user-defined name will be assigned permanently to a camera
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param UserDefinedName the name to be transferred to the camera
        * @	return success or error code
         * http://stackoverflow.com/questions/5666073/c-converting-string-to-sbyte
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setUserDefinedName(IntPtr hCamera, string UserDefinedName)
        {

            void* cam = hCamera.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(UserDefinedName);
            fixed (byte* p = bytes)
            {
                sbyte* sp = (sbyte*)p;
                return Camera_setUserDefinedName(cam, sp);
            }

        }

        /**
        *  Get user-defined name
        *  A name that has been assigned to a camera by the user will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getUserDefinedName(IntPtr hCamera)
        {
            sbyte* ascii;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getUserDefinedName(cam);
            string rstring = new string(ascii);
            return rstring;
        }

        /**
        * Get MAC address.
        * The MAC address that is obtained from the camera firmware will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getMacAddress(IntPtr hCamera)
        {
            sbyte* ascii;
            string rstring;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getMacAddress(cam);
            rstring = new string(ascii);
            return rstring;

        }

        /**
        * Get IP address.
        * The IP address that the camera is currently working on will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return a string containing requested information
        */
        public unsafe
        string
        Gige_Camera_getIPAddress(IntPtr hCamera)
        {
            sbyte* ascii;
            string rstring;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getIPAddress(cam);
            rstring = new string(ascii);
            return rstring;
        }

        /** Get subnet mask.
        *  The subnet mask that the camera is currently working with will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @	return success or error code
        */

        public unsafe
        string
        Gige_Camera_getSubnetMask(IntPtr hCamera)
        {
            sbyte* ascii;
            string rstring;
            void* cam = hCamera.ToPointer();
            ascii = Camera_getSubnetMask(cam);
            rstring = new string(ascii);
            return rstring;
        }

        /** Get pixel clock.
        *  The camera's pixel clock will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param PixelClock a reference to the pixel clock value
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPixelClock(IntPtr hCamera, ref int PixelClock)
        {
            SVSGigeApiReturn rval;
            fixed (int* clockPtr = &PixelClock)
            {
                void* cam = hCamera.ToPointer();
                rval = Camera_getPixelClock(cam, clockPtr);
                //PixelClock = *clockPtr;
            }
            return rval;
        }

        /** Read XML file.
        *  The camera's XML file will be retrieved and made available for further
        *  processing by an application. The 	returned pointer to the content of the
        *  XML file will be valid until the function is called again or until
        *  the camera is closed.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param bufXML a pointer to a zero-terminated string containing the complete XML file
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_readXMLFile(IntPtr hCamera, ref string bufXML) 
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            sbyte* xmlptr1 = null;
            sbyte** xmlptr2 = &xmlptr1;
            rval = Camera_readXMLFile(cam, xmlptr2);
            bufXML = new string(*xmlptr2);
            return rval;
        }

        /**
        * Is camera feature.
        * The camera will be queried whether a feature is available or not.
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Feature a feature which availability will be determined
        * @	return a boolean value indicating whether the queried feature is available or not
        */
        public unsafe 
        bool
        Gige_Camera_isCameraFeature(IntPtr hCamera, CAMERA_FEATURE Feature)
        {
            void* cam = hCamera.ToPointer();
            return Camera_isCameraFeature(cam, Feature);
        }

        /**  Get Sensor temperature.
        * The current camera's Sensor temperature  will be returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param SensorTemperature  the actual sensor temperature of the camera [°C]
        *  @return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getSensorTemperature(IntPtr hCamera, ref uint SensorTemperature)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* st = &SensorTemperature)
            {
                return Camera_getSensorTemperature(cam, st);
            }
        }

        //---Look-up-table-/-Binning----------------------------------------------------

        /**
		* 2009-02-19: Re-implemented for backward compatibility.
        * Set look-up table mode.
        * The look-up table mode will be switched on or off
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param LUTMode new setting for look-up table mode
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLUTMode(IntPtr hCamera, LUT_MODE LUTMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLUTMode(cam, LUTMode);
        }

        /**
		* 2009-02-19: Re-implemented for backward compatibility 
        * Get look-up table mode.
        * The currently programmed look-up table mode will be retrieved
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param LUTMode currently programmed look-up table mode
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLUTMode(IntPtr hCamera,
        LUT_MODE* LUTMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_getLUTMode(cam, LUTMode);
        }

        /**
		*  DEACTIVATED !
		*  2006-12-20: White balance has been re-implemented by the Camera_setWhiteBalance() 
		*  function and a separate lookup table can be uploaded by setLookupTable().
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_createLUTwhiteBalance(IntPtr hCamera, float Red, float Green, float Blue)
        {
            void* cam = hCamera.ToPointer();
            return Camera_createLUTwhiteBalance(cam, Red, Green, Blue);
        }

        //---Acquisition-modes-(trigger)------------------------------------------------

        /**
		*  2009-05-05: DEPRECATED, please use Camera_softwareTrigger()
        * Start acquisition cycle.
        * The camera will be triggerred for starting a new acquisition cycle.
        * A mandatory pre-requisition for successfully starting an acquisition
        * cycle by software is to have the camera set to SVSGige_CameraCodes_INT_TIGGER
        * before
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_startAcquisitionCycle(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_startAcquisitionCycle(cam);
        }

        //---Timestamps-----------------------------------------------------------------

        /**
		* (2008: functionality removed)
        * Stamp timestamp.
        * A hardware timestamp will be written into the selected camera register.
        * The timestamp's actual value can be read out by Camera_getTimestamp()
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param TimestampIndex the index of the timestamp to be set by hardware
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_stampTimestamp(IntPtr hCamera,
        int TimestampIndex)
        {
            void* cam = hCamera.ToPointer();
            return Camera_stampTimestamp(cam, TimestampIndex);
        }

        /**
		* (2008: functionality removed)
        * Get timestamp.
        * The value of a selected hardware timestamp will be 	returned. The index
        * provided to the function is valid in the range between 0 and 8
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param TimestampIndex the index of the timestamp to be 	returned
        * @param Timestamp the timestamp's value in seconds and part of a second
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTimestamp(IntPtr hCamera, int TimestampIndex, ref double Timestamp)
        {
            void* cam = hCamera.ToPointer();


            fixed (double* ts = &Timestamp)
            {
                SVSGigeApiReturn ret = Camera_getTimestamp(cam, TimestampIndex, ts);
                return ret;
            }
        }

        //------------------------------------------------------------------------------
        // 4 - Stream: Channel creation and control
        //------------------------------------------------------------------------------
        /**
        * Create streaming channel.
        * A UDP streaming channel will be established for image data transfer.
        * A connection to the camera has to be successfully opened first using
        * Camera_openConnection() before a streaming channel can be established
        *
        * @see Camera_openConnection()
        * @param hStreamingChannel a handle for the streaming channel will be 	returned
        * @param hCameraContainer a camera container handle received from CameraContainer_create()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param DriverEnabled specifies whether a filter driver will be used or not
        * @param CallbackFunction user-defined callback function for image processing
        * @param Context user-defined data that will be 	returned on each callback
        * @	return a streaming channel handle for subsequent streaming channel function calls
        */
        // void ** ?
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_create(ref IntPtr hStreamingChannel,
                                    int hCameraContainer,
                                    IntPtr hCamera,
                                    int BufferCount,
                                    StreamCallback CallbackFunction,
                                    IntPtr Context)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            void* stream = hStreamingChannel.ToPointer();
            void** st1 = &stream;
            rval = StreamingChannel_create(st1,
                                                hCameraContainer,
                                                cam,
                                                BufferCount,
                                                CallbackFunction,
                                                Context.ToPointer());

            hStreamingChannel = new IntPtr(*st1);

            return rval;
        }

        /** Create streaming channel.
        *  A UDP streaming channel will be established for image data transfer.
        *  A connection to the camera has to be successfully opened first using
        *  Camera_openConnection() before a streaming channel can be established
        *
        *  @see Camera_openConnection()
        *  @param hStreamingChannel a handle for the streaming channel will be 	returned
        *  @param hCameraContainer a camera container handle received from CameraContainer_create()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param BufferCount specifies the number of image buffers (default: 3 buffers)
        *                    NOTE: A value of 0 will be mapped to the default value (3 buffers)
        *  @param PacketResendTimeout Timeout for packet resend in msec.
        *  @param CallbackFunction user-defined callback function for image processing
        *  @param Context user-defined data that will be 	returned on each callback
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_createEx(ref IntPtr hStreamingChannel,
                                int hCameraContainer,
                                IntPtr hCamera,
                                int BufferCount,
                                int PacketResendTimeout,
                                StreamCallback CallbackFunction,
                                IntPtr Context)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            void* stream = hStreamingChannel.ToPointer();
            void** st1 = &stream;
            rval = StreamingChannel_createEx(st1,
                                            hCameraContainer,
                                            cam,
                                            BufferCount,
                                            PacketResendTimeout,
                                            CallbackFunction,
                                            Context.ToPointer());

            hStreamingChannel = new IntPtr(*st1);

            return rval;
        }


        /**
        * Delete streaming channel.
        * A streaming channel will be closed and all resources will be released that
        * have been occupied by the streaming channel
        *
        * @param hStreamingChannel a streaming channel handle received from
        * StreamingChannel_create()
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_delete(IntPtr hStreamingChannel)
        {
            SVSGigeApiReturn rval;
            void* streamingChannel = hStreamingChannel.ToPointer();
            rval = StreamingChannel_delete(streamingChannel);
            return rval;
        }


        /** Set readout transfer.
        *  The readout transfer of a streaming channel will be enabled or disabled
        *  dependent on an isReadoutTransfer parameter. If false, then the camera
        *  would capture an image but it would not transfer the image to the host.
        *  The application can request the streaming channel to send the already
        *  captured data by toggling the isRedoutTransfer parameter to true at
        *  any time after image exposure has finished.
        *  If the isReadoutTransfer parameter is toggled to true before image
        *  exposure has finished then the default behavior will take place where
        *  the camera starts image data transfer immediately after image exposure
        *  has finished.
        *  Controlling readout transfer might be usefull when operating multiple
        *  cameras that are triggered all at the same time. The application will
        *  be able to request data in a pre-defined way on a one-by-one basis and 
        *  to avoid bandwidth bottlenecks this way which otherwise might occur.
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param isReadoutTransfer whether an image will be transferred to the host after readout
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_setReadoutTransfer(IntPtr hStreamingChannel, bool isReadoutTransfer)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            rval = StreamingChannel_setReadoutTransfer(stream, isReadoutTransfer);
            return rval;
        }


        /** Get readout transfer.
        *  The readout transfer of a streaming channel will be retrieved. If false, 
        *  then the camera would capture an image but it would not transfer the image 
        *  to the host. The application can request the streaming channel to send the 
        *  already captured data by toggling the isRedoutTransfer parameter to true at
        *  any time after image exposure has finished.
        *  If the isReadoutTransfer parameter is toggled to true before image
        *  exposure has finished then the default behavior will take place where
        *  the camera starts image data transfer immediately after image exposure
        *  has finished.
        *  Controlling readout transfer might be usefull when operating multiple
        *  cameras that are triggered all at the same time. The application will
        *  be able to request data in a pre-defined way on a one-by-one basis and 
        *  to avoid bandwidth bottlenecks this way which otherwise might occur.
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param isReadoutTransfer whether an image will be transferred to the host after readout
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getReadoutTransfer(IntPtr hStreamingChannel, ref bool isReadoutTransfer)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (bool* b = &isReadoutTransfer)
            {
                rval = StreamingChannel_getReadoutTransfer(stream, b);
            }
            return rval;
        }
        //------------------------------------------------------------------------------
        // 5 - Stream: Channel statistics
        //------------------------------------------------------------------------------
        /** Get frame loss.
        *  The number of lost frames in a streaming channel will be 	returned
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @ FrameLoss the number of frames that have been lost since the streaming channel was opened
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getFrameLoss(IntPtr hStreamingChannel, ref int FrameLoss)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (int* b = &FrameLoss)
            {
                rval = StreamingChannel_getFrameLoss(stream, b);
            }
            return rval;
        }

        /** Get actual frame rate.
        *  The actual frame rate calculated from received images will be 	returned
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param ActualFrameRate the actual frame rate measured based on received images
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getActualFrameRate(IntPtr hStreamingChannel, ref float ActualFrameRate)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (float* b = &ActualFrameRate)
            {
                rval = StreamingChannel_getActualFrameRate(stream, b);
            }
            return rval;
        }

        /** Get actual data rate.
        *  The actual data rate calculated from received image data will be 	returned
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param ActualDataRate the actual frame rate measured based on received image data
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getActualDataRate(IntPtr hStreamingChannel, ref float ActualDataRate)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (float* b = &ActualDataRate)
            {
                rval = StreamingChannel_getActualDataRate(stream, b);
            }
            return rval;
        }


        /** Get peak data rate.
        *  The peak data rate will be 	returned. It is determined for a single image
        *  transfer by measuring the transfer time from first to last network packet
        *  which belong to a single image. The peak data rate is received by dividing
        *  the amount of data of one image by that transfer time. It can be used for 
        *  evaluating the bandwidth situation when operating multiple GigE cameras 
        *  over a single GigE line.
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param PeakDataRate the actual frame rate measured based on received image data
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getPeakDataRate(IntPtr hStreamingChannel, ref float PeakDataRate)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (float* b = &PeakDataRate)
            {
                rval = StreamingChannel_getPeakDataRate(stream, b);
            }
            return rval;
        }
        //------------------------------------------------------------------------------
        // 6 - Stream: Channel info 
        //------------------------------------------------------------------------------
        /** Get pixel type.
        *  The pixel type will be 	returned that applies to the output image (or output
        *  view in case of a MultiStream channel).
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param PixelType the pixel type that has been set for the output image/view
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getPixelType(IntPtr hStreamingChannel, ref GVSP_PIXEL_TYPE PixelType)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (GVSP_PIXEL_TYPE* b = &PixelType)
            {
                rval = StreamingChannel_getPixelType(stream, b);
            }
            return rval;
        }

        /** Get buffer data.
        *  A streaming channel will be queried for information about one of its image buffers.
        *  On success, a pointer to image data will be 	returned.
        *  Since the buffer's data pointer is queried asynchronously with regard to image 
        *  acquisition, no assumption can be made for the content that is in the buffer at 
        *  time of running this function. Image access functions have to be used for obtaining 
        *  actual images that were captured by the camera.
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param BufferIndex an index of the buffer where information is requested for
        *  @param BufferData a pointer to a data pointer that will 	return the buffer address
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getBufferData(IntPtr hStreamingChannel, int ImageSize, uint BufferIndex, ref byte[] BufferData) 
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            sbyte* p = null;
            sbyte** p0 = &p;

            sbyte* dataptr = null;
            sbyte** dataptr2 = &dataptr;

            rval = StreamingChannel_getBufferData(stream, BufferIndex, dataptr2);
            sbyte* ret = *dataptr2;
            byte[] list = new byte[ImageSize];
            int count = 0;
            sbyte* b = null;
            for( b = ret  ; count < ImageSize; b++  ){
                list[count] = (byte)(*b);
                count++;
   
            }

            BufferData = list;
            return rval;
        }


        /** Get buffer size.
        *  The buffer size will be 	returned that applies to the output image (or output
        *  view in case of a MultiStream channel).
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param BufferSize the buffer size for the output image/view
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getBufferSize(IntPtr hStreamingChannel, ref int BufferSize)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (int* b = &BufferSize)
            {
                rval = StreamingChannel_getBufferSize(stream, b);
            }
            return rval;
        }

        /** Get image pitch.
        *  The image pitch will be 	returned that applies to the output image (or output
        *  view in case of a MultiStream channel).
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param ImagePitch the image pitch for the output image/view
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getImagePitch(IntPtr hStreamingChannel, ref int ImagePitch)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (int* b = &ImagePitch)
            {
                rval = StreamingChannel_getImagePitch(stream, b);
            }
            return rval;
        }

        /** Get image size X.
        *  The image size X will be 	returned that applies to the output image (or output
        *  view in case of a MultiStream channel).
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param ImageSizeX the image size X for the output image/view
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getImageSizeX(IntPtr hStreamingChannel, ref int ImageSizeX)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (int* b = &ImageSizeX)
            {
                rval = StreamingChannel_getImageSizeX(stream, b);
            }
            return rval;
        }

        /** Get image size Y.
        *  The image size Y will be 	returned that applies to the output image (or output
        *  view in case of a MultiStream channel).
        *
        *  @param hStreamingChannel a streaming channel handle received from StreamingChannel_create()
        *  @param ImageSizeY the image size Y for the output image/view
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_StreamingChannel_getImageSizeY(IntPtr hStreamingChannel, ref int ImageSizeY)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            fixed (int* y = &ImageSizeY)
            {
                rval = StreamingChannel_getImageSizeY(stream, y);
            }
            return rval;
        }

        //------------------------------------------------------------------------------
        // 7 - Stream: Transfer Parameters
        //------------------------------------------------------------------------------

        /** Evaluate maximal packet size.
        *  A test will be performed which determines the maximal usable packet size
        *  based on given network hardware. This value will be used when opening a
        *  streaming channel.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaximalPacketSize the maximal possible packet size without fragmentation
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_evaluateMaximalPacketSize(IntPtr hCamera, ref int MaximalPacketSize)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (int* maxPS = &MaximalPacketSize)
            {
                rval = Camera_evaluateMaximalPacketSize(cam, maxPS);
            }
            return rval;
        }

        /** Set streaming packet size.
        *  The packet size is set which will be generated by the camera for streaming
        *  image data as payload packets to the host
        *
        *  WARNING! Explicitly setting network packet size to values above 1500 bytes
        *           may provide to unpredictable results and also to a system crash.
        *           Please use "Camera_evaluateMaximalPacketSize" for a save adjustment
        *           to jumbo packets.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StreamingPacketSize the packet size used by the camera for image packets
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setStreamingPacketSize(IntPtr hCamera, int StreamingPacketSize)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            rval = Camera_setStreamingPacketSize(cam, StreamingPacketSize);
            return rval;
        }


        /** Set inter-packet delay
        *  A delay between network packets will be introduced and set to a specified
        *  number of ticks.
        *
        *  NOTE: The resulting total image transfer time must not exceed 250 ms.
        *  Otherwise a timeout condition will be reached in the SVGigE driver.
        *  The dependency between inter-packet delay and total image transfer time
        *  depends on pixel clock, image size and has to be determine case by case.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param InterPacketDelay the new value for inter-packet delay (0..1000, relative number)
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setInterPacketDelay(IntPtr hCamera, float InterPacketDelay)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            rval = Camera_setInterPacketDelay(cam, InterPacketDelay);
            return rval;
        }

        /** Get inter-packet delay
        *  The delay between network packets will be 	returned as a relative number
        *  of ticks.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param InterPacketDelay the currently programmed value for inter-packet delay (0..1000, relative number)
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getInterPacketDelay(IntPtr hCamera, ref float InterPacketDelay)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (float* f = &InterPacketDelay)
            {
                rval = Camera_getInterPacketDelay(cam, f);
            }
            return rval;
        }


        /** Set multicast mode
        *  The multicast mode will be set to none (default), listener or controller.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MulticastMode the intended new multicast mode
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setMulticastMode(IntPtr hCamera, MULTICAST_MODE MulticastMode)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            rval = Camera_setMulticastMode(cam, MulticastMode);
            return rval;
        }


        /** Get multicast mode
        *  Current multicast mode will be retrieved from the camera.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MulticastMode current multicast mode will be 	returned
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getMulticastMode(IntPtr hCamera, ref MULTICAST_MODE MulticastMode)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (MULTICAST_MODE* p = &MulticastMode)
            {
                rval = Camera_getMulticastMode(cam, p);
            }
            return rval;
        }


        /** Get multicast group
        *  Current multicast group (IP) and port will be retrieved from the camera.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MulticastGroup current multicast group (IP) will be 	returned
        *  @param MulticastPort current multicast port will be 	returned
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getMulticastGroup(IntPtr hCamera, ref uint MulticastGroup, ref uint MulticastPort)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (uint* g = &MulticastGroup)
            {
                fixed (uint* p = &MulticastPort)
                {
                    rval = Camera_getMulticastGroup(cam, g, p);
                }
            }
            return rval;
        }


        //------------------------------------------------------------------------------
        // 8 - Stream: Image access
        //------------------------------------------------------------------------------

        /**
        * Release image.
        * An image which availability was indicated by a StreamCallback function needs
        * to be released after processing it by a user application in order to free
        * the occupied buffer space for a subsequent image acquisition.
        *
        * After releasing a picture the following access functions must not be
        * called anymore using the released image handle
        * @param hImage an image handle that was received from the callback function
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Image_release(int hImage)
        {
            return Image_release(hImage);
        }

        /**
        * Get image pointer.
        * A pointer to the image data will be 	returned
        * @param hImage an image handle that was received from the callback function
        * @	return a pointer to the image data
        */
        public unsafe
        IntPtr
        Gige_Image_getDataPointer(int hImage)
        {
            IntPtr iptr;
            sbyte* image;
            image = Image_getDataPointer(hImage);
            iptr = (IntPtr)image;
            return iptr;
        }

        /** Get buffer index.
        *  The index of the current image buffer will be 	returned
        *
        *  @param hImage an image handle that was received from the callback function
        *  @	return a pointer to the image data
        */
        public unsafe
        int Gige_Image_getBufferIndex(int hImage)
        {
            return Image_getBufferIndex(hImage);
        }

        /** Get signal type
        *  The signal type that is related to a callback will be 	returned
        *
        *  @param hImage an image handle that was received from the callback function
        *  @	return the signal type which informs why a callback was triggered
        */

        public unsafe
        SVGigE_SIGNAL_TYPE Gige_Image_getSignalType(int hImage)
        {
            return Image_getSignalType(hImage);
        }

        /** Get camera handle
        *  A handle of the camera that captured the image will be 	returned
        *
        *  @param hImage an image handle that was received from the callback function
        *  @	return a camera handle
        */

        public unsafe
        int Gige_Image_getCamera(int hImage)
        {
            return Image_getCamera(hImage);
        }
        //------------------------------------------------------------------------------
        // 9 - Stream: Image conversion
        //------------------------------------------------------------------------------

        // Convert image to RGB-image
        /** Get image RGB
        *
        *  The image will be converted by a selectable Bayer conversion algorithm into
        *  a RGB image. The caller needs to provide a sufficient buffer.
        *
        *  @param hImage an image handle that was received from the callback function
        *  @param BufferRGB a buffer for RGB image data
        *  @param BufferLength the length of the image buffer
        *  @param BayerMethod the a demosaicking method (Bayer method)
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Image_getImageRGB(int hImage, byte* BufferRGB,               // IntPtr bufferRGB,
                               int BufferLength, BAYER_METHOD BayerMethod)
        {
            SVSGigeApiReturn rval = SVSGigeApiReturn.SVGigE_ERROR;
            rval = Image_getImageRGB(hImage, BufferRGB, BufferLength, BayerMethod);
            return rval;
        }

        /** Get image gray
		 /** Get image gray
		*
		*  OBSOLETE. Please obtain a raw image and use a suitable conversion algorithm 
		*            in order to get from a raw image of a bayer coded sensor to a 
		*            gray image of intended quality.
		*			 Please refer to: http://en.wikipedia.org/wiki/Grayscale
        *
        *  A bayer coded image will be converted into a BW image. The caller needs to
        *  provide a sufficient 8-bit buffer.
        *
        *  @param hImage an image handle that was received from the callback function
        *  @param Buffer8bit a buffer for 8-bit image data
        *  @param BufferLength the length of the image buffer
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Image_getImageGray(int hImage, byte* Buffer8bit, int BufferLength)
        {
            SVSGigeApiReturn rval = SVSGigeApiReturn.SVGigE_ERROR;
            rval = Image_getImageGray(hImage, Buffer8bit, BufferLength);
            return rval;
        }

        /** Get image 12-bit as 8-bit
        *  A 12-bit image will be converted into a 8-bit image. The caller needs to
        *  provide for a sufficient buffer for the 8-bit image.
        *
        *  @param hImage an image handle that was received from the callback function
        *  @param Buffer8bit a buffer for 8-bit image data
        *  @param BufferLength the length of the image buffer
        *  @	return success or error code
        */


        public unsafe
        SVSGigeApiReturn
        Gige_Image_getImage12bitAs8bit(int hImage, byte* Buffer8bit, int BufferLength)
        {
            SVSGigeApiReturn rval = SVSGigeApiReturn.SVGigE_ERROR;
            rval = Image_getImage12bitAs8bit(hImage, Buffer8bit, BufferLength);
            return rval;
        }

        /** Get image 12-bit as 16-bit
        *  A 12-bit image will be converted into a 16-bit image. The caller needs to
        *  provide for a sufficiently large buffer (2 x width x height bytes) for the
        *  16-bit image.
        *
        *  @param hImage an image handle that was received from the callback function
        *  @param Buffer16bit a buffer for 16-bit image data
        *  @param BufferLength the length of the image buffer
        *  @	return success or error code
        */


        public unsafe
        SVSGigeApiReturn
        Gige_Image_getImage12bitAs16bit(int hImage, byte* Buffer16bit, int BufferLength)
        {
            SVSGigeApiReturn rval = SVSGigeApiReturn.SVGigE_ERROR;
            rval = Image_getImage12bitAs16bit(hImage, Buffer16bit, BufferLength);
            return rval;
        }

        /** Get image 16-bit as 8-bit
        *  A 16-bit image will be converted into a 8-bit image. The caller needs to
        *  provide for a sufficient buffer for the 8-bit image.
        *
        *  @param hImage an image handle that was received from the callback function
        *  @param Buffer8bit a buffer for 8-bit image data
        *  @param BufferLength the length of the image buffer
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Image_getImage16bitAs8bit(int hImage, byte* Buffer8bit, int BufferLength)
        {
            SVSGigeApiReturn rval = SVSGigeApiReturn.SVGigE_ERROR;
            rval = Image_getImage16bitAs8bit(hImage, Buffer8bit, BufferLength);
            return rval;
        }

        //------------------------------------------------------------------------------
        // 10 - Stream: Image characteristics
        //------------------------------------------------------------------------------

        /**
        * Get image size X
        * The horizontal pixel number will be 	returned as received from the camera
        * @param hImage an image handle that was received from the callback function
        * @	return the image's size X as received from the camera
        */
        public unsafe
        int
        Gige_Image_getSizeX(int hImage)
        {
            return Image_getSizeX(hImage);
        }

        /**
        * Get image size Y
        * The vertical pixel number will be 	returned as received from the camera
        * @param hImage an image handle that was received from the callback function
        * @	return the image's size Y as received from the camera
        */
        public unsafe
        int
        Gige_Image_getSizeY(int hImage)
        {
            return Image_getSizeY(hImage);

        }

        /**
        * Get image pitch
        * The number of bytes in a row (pitch) will be 	returned as received from the camera
        * @param hImage an image handle that was received from the callback function
        * @	return the image's pitch as received from the camera
        */
        public unsafe
        int
        Gige_Image_getPitch(int hImage)
        {
            return Image_getPitch(hImage);
        }

        /**
        * Get image size.
        * The number of bytes in the image data field will be 	returned.
        * @param hImage an image handle that was received from the callback function
        * @	return the number of bytes in the image data field
        */
        public unsafe
        int
        Gige_Image_getImageSize(int hImage)
        {
            return Image_getImageSize(hImage);
        }


        /**
        * Get pixel type
        * The pixel type as indicated by the camera will be 	returned
        * @param hImage an image handle that was received from the callback function
        * @	return the pixel type as indicated by the camera
        */
        public unsafe
        GVSP_PIXEL_TYPE
        Gige_Image_getPixelType(int hImage)
        {
            GVSP_PIXEL_TYPE type = GVSP_PIXEL_TYPE.GVSP_PIX_UNKNOWN;
            type = Image_getPixelType(hImage);
            return type;
        }

        //------------------------------------------------------------------------------
        // 11 - Stream: Image statistics
        //------------------------------------------------------------------------------


        /**
        * Get image identifier.
        *
        * An image identifier will be 	returned as it was assigned by the camera.
        * Usually the camera will assign an increasing sequence of integer numbers
        * to subsequent images which will wrap at some point and jump back to 1.
        * The 0 identifier will not be used as it is defined in the GigE Vision
        * specification

        * @param hImage an image handle that was received from the callback function
        * @	return an integer number that is unique inside a certain sequence of numbers
        */
        public unsafe
        int
        Gige_Image_getImageID(int hImage)
        {
            return Image_getImageID(hImage);
        }

        /**
        * Get image timestamp
        * The timestamp that was assigned to an image by the camera on image
        * acquisition will be 	returned
        * @param hImage an image handle that was received from the callback function
        * @	return a timestamp as it was received from the camera in seconds after
        *   January, 1st 1970 where the fraction represents parts of a second
        */
        public unsafe
        double
        Gige_Image_getTimestamp(int hImage)
        {
            return Image_getTimestamp(hImage);
        }


        /** Get image transfer time
        *  The time that elapsed from image's first network packet arriving on PC side
        *  until image completion will be determined, including possible packet resends.
        *
        *  @param hImage an image handle that was received from the callback function
        *  @	return image's transfer time as it was explained above
        */

        public unsafe
        double Gige_Image_getTransferTime(int hImage)
        {
            return Image_getTransferTime(hImage);
        }


        /** Get packet count
        *  The number of packets that belong to a frame will be 	returned
        *
        *  @param hImage an image handle that was received from the callback function
        *  @	return the pixel type as indicated by the camera
        */

        public unsafe
        int Gige_Image_getPacketCount(int hImage)
        {
            return Image_getPacketCount(hImage);
        }


        /** Get packet resend
        *  The number of packets that have been resent will be reported
        *
        *  @param hImage an image handle that was received from the callback function
        *  @	return the pixel type as indicated by the camera
        */

        public unsafe
        int Gige_Image_getPacketResend(int hImage)
        {
            return Image_getPacketResend(hImage);
        }

        //------------------------------------------------------------------------------
        // 12 - Stream: Messaging channel
        //------------------------------------------------------------------------------
        /** Create event.
        *  An event will be created inside GigE API which is capable of waiting for
        *  messages that are issued inside a streaming channel. One or more message
        *  types have to be added to the event before messages will be actually
        *  delivered to the application.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID a pointer to the identifier of a messaging channel
        *  @param SizeFIFO the number of entries in a message FIFO (0 = no FIFO)
        *  @	return success or error code
        */

        // void ** ?
        public unsafe
        SVSGigeApiReturn
        Gige_Stream_createEvent(IntPtr hStreamingChannel, ref IntPtr EventID, int SizeFIFO)
        {
            SVSGigeApiReturn rval;
            void* evt = EventID.ToPointer();
            void** p = &evt;
            void* stream = hStreamingChannel.ToPointer();

            rval = Stream_createEvent(stream, p, SizeFIFO);
            EventID = new IntPtr(evt);
            return rval;

        }

        /** Add message type.
        *  A message type will be added to a previously created messaging channel.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param MessageType one of pre-defined message types
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Stream_addMessageType(IntPtr hStreamingChannel, IntPtr EventID, SVGigE_SIGNAL_TYPE MessageType)
        {
            SVSGigeApiReturn rval;
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* eventID = EventID.ToPointer();
            rval = Stream_addMessageType(streamingChannel, eventID, MessageType);
            return rval;
        }

        /** Remove message type.
        *  A message type will be removed from a previously created messaging channel.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param MessageType one of pre-defined message types
        *  @	return success or error code
        */

        public unsafe
       SVSGigeApiReturn
       Gige_Stream_removeMessageType(IntPtr hStreamingChannel,
       IntPtr EventID,
       SVGigE_SIGNAL_TYPE MessageType)
        {
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* eventID = EventID.ToPointer();
            SVSGigeApiReturn rval;
            rval = Stream_removeMessageType(streamingChannel, eventID, MessageType);
            return rval;
        }

        /** Is message pending.
        *  A messaging channel with a given EventID will be checked whether pending
        *  messages are available. The function will 	return a result immediately if
        *  the timeout is set to zero. Otherwise it will wait for a message atmost
        *  till the timeout elapses.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param Timeout_ms a timeout value in milliseconds
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Stream_isMessagePending(IntPtr hStreamingChannel, IntPtr EventID, int Timeout_ms)
        {
            SVSGigeApiReturn rval;
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* eventID = EventID.ToPointer();
            rval = Stream_isMessagePending(streamingChannel, eventID, Timeout_ms);
            return rval;
        }


        /** Register event callback.
        *  A callback function will be registered which will be called whenever an event is
        *  signalled in the messaging channel. One parameter of the callback is a Context
        *  which was registered along with the callback.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param Callback the pointer of an EventCallback function
        *  @param Context an arbitrary value that will be 	returned with each call to EventCallback
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Stream_registerEventCallback(IntPtr hStreamingChannel,
                                        IntPtr EventID,
                                        EventCallback Callback,
                                        IntPtr Context)
        {
            SVSGigeApiReturn rval;
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* eventID = EventID.ToPointer();
            void* context = Context.ToPointer();
            rval = Stream_registerEventCallback(streamingChannel, eventID, Callback, context);
            return rval;
        }


        /** Unregister event callback.
        *  A previously registered callback function will be unregistered from message channel.
        *  This will effectively stop any further calls to that function.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param Callback the pointer of an EventCallback function
        *  @	return success or error code
        */
        public SVSGigeApiReturn
        Gige_Stream_unregisterEventCallback(IntPtr hStreamingChannel, IntPtr EventID, EventCallback Callback)
        {
            SVSGigeApiReturn rval;
            rval = Gige_Stream_unregisterEventCallback_unsafe(hStreamingChannel, EventID, Callback);
            return rval;
        }

        private unsafe
        SVSGigeApiReturn
        Gige_Stream_unregisterEventCallback_unsafe(IntPtr hStreamingChannel, IntPtr EventID,
                                            EventCallback Callback)
        {
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* eventID = EventID.ToPointer();

            SVSGigeApiReturn rval;
            rval = Stream_unregisterEventCallback(streamingChannel, eventID, Callback);
            return rval;
        }

        /** Get message.
        *  A subsequent MessageID will be retrieved for a previously received EventID.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID an ID of a messaging channel from EventCallback() or from isMessagePending()
        *  @param MessageID a pointer to a MessageID variable
        *  @param MessageType a pointer to a MessageType variable
        *  @	return success or error code
        */
        public
        SVSGigeApiReturn
        Gige_Stream_getMessage(IntPtr hStreamingChannel, IntPtr EventID, ref int MessageID,
                                ref SVGigE_SIGNAL_TYPE MessageType)
        {
            SVSGigeApiReturn rval;
            rval = Gige_Stream_getMessage_unsafe(hStreamingChannel, EventID, ref MessageID, ref MessageType);
            return rval;
        }

        private unsafe
        SVSGigeApiReturn
        Gige_Stream_getMessage_unsafe(IntPtr hStreamingChannel, IntPtr EventID, ref int MessageID,
                                ref SVGigE_SIGNAL_TYPE MessageType)
        {
            SVSGigeApiReturn rval;
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* evtID = EventID.ToPointer();
            fixed (int* abc = &MessageID)
            {
                fixed (SVGigE_SIGNAL_TYPE* abc1 = &MessageType)
                {
                    rval = Stream_getMessage(streamingChannel, evtID, abc, abc1);
                }
            }
            return rval;
        }


        /** Get message data.
        *  The data pointer and length will be retrieved for a previously received MessageID.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param MessageID a message identifier received from getMessage()
        *  @param MessageData a pointer to a void* variable
        *  @param MessageLength a pointer to a data length variable
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Stream_getMessageData(IntPtr hStreamingChannel, IntPtr EventID, int MessageID,
                                    ref IntPtr MessageData, ref int MessageLength)
        {
            SVSGigeApiReturn rval;
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* eventID = EventID.ToPointer();

            fixed (void* msg = &MessageData)
            {
                void* p0 = msg;
                void** pp = &p0;
                fixed (int* abc1 = &MessageLength)
                {
                    rval = Stream_getMessageData(streamingChannel, eventID, MessageID, pp, abc1);
                    MessageData = (IntPtr)(pp);
                }
            }
            return rval;
        }

        /** Get message Timestamp.
        *  A message's timestamp will be retrieved for a previously received MessageID.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param MessageID a message identifier received from getMessage()
        *  @param MessageTimestamp a pointer to a Timestamp variable
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Stream_getMessageTimestamp(IntPtr hStreamingChannel,
                                        IntPtr EventID,
                                        int MessageID,
                                        ref double MessageTimestamp)
        {
            SVSGigeApiReturn rval;
            void* streamingChannel = hStreamingChannel.ToPointer();
            void* eventID = EventID.ToPointer();
             fixed ( double *d = &MessageTimestamp)
             {
            rval = Stream_getMessageTimestamp(streamingChannel, eventID, MessageID, d);
             }

            return rval;
        }

        /** Release message.
        *  A previously received MessageID will be released. No further access must happen
        *  for the released MessageID since it will be removed from memory.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @param MessageID a message identifier received from getMessage()
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Stream_releaseMessage(IntPtr hStreamingChannel,
                                   IntPtr EventID,
                                    int MessageID)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            void* ev = EventID.ToPointer();
            rval = Stream_releaseMessage(stream, ev, MessageID);
            return rval;
        }

        /** Flush messages.
        *  All messages in the message FIFO will be removed.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Stream_flushMessages(IntPtr hStreamingChannel, IntPtr EventID)
        {
            SVSGigeApiReturn rval;
            void* stream = hStreamingChannel.ToPointer();
            void* ev = EventID.ToPointer();
            rval = Stream_flushMessages(stream, ev);
            return rval;
        }

        /** Close event.
        *  The messaging channel with given EventID will be closed and all resources
        *  will be freed.
        *
        *  @see CameraContainer_getCamera()
        *  @param hStreamingChannel a handle to a valid streaming channel
        *  @param EventID the identifier of a messaging channel
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Stream_closeEvent(IntPtr hStreamingChannel, IntPtr EventID)
        {
            SVSGigeApiReturn rval;

            void* stream = hStreamingChannel.ToPointer();
            void* ev = EventID.ToPointer();
            rval = Stream_closeEvent(stream, ev);
            return rval;
        }

        //------------------------------------------------------------------------------
        // 13 - Controlling camera: Frame rate
        //------------------------------------------------------------------------------

        /**
        * Set framerate.
        * The camera will be adjusted to a new framerate
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Framerate new framerate in 1/s
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setFrameRate(IntPtr hCamera,
        float Framerate)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setFrameRate(cam, Framerate);
        }

        /**
        * Get framerate.
        * The currently programmed framerate will be retrieved
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Framerate currently programmed framerate in 1/s
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getFrameRate(IntPtr hCamera, ref float Framerate)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &Framerate)
            {
                return Camera_getFrameRate(cam, p);
            }
        }

        /**
        * Get framerate min.
        *  The minimal available framerate will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MinFramerate the minimal framerate in 1/s
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getFrameRateMin(IntPtr hCamera, ref float MinFramerate)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MinFramerate)
            {
                return Camera_getFrameRateMin(cam, p);
            }
        }



        /** Get framerate max.
        *  The maximal available framerate will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxFramerate the maximal framerate in 1/s
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getFrameRateMax(IntPtr hCamera, ref float MaxFramerate)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MaxFramerate)
            {
                return Camera_getFrameRateMax(cam, p);
            }
        }


        /** Get framerate range.
        *  The currently available framerate range will be retrieved
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MinFramerate currently available minimal framerate in 1/s
        *  @param MaxFramerate currently available maximal framerate in 1/s
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getFrameRateRange(IntPtr hCamera, ref float MinFramerate, ref float MaxFramerate)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* pmin = &MinFramerate)
            {
                fixed (float* pmax = &MaxFramerate)
                {
                    return Camera_getFrameRateRange(cam, pmin, pmax);
                }
            }
        }


        /** Get framerate increment.
        *  The framerate increment will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FramerateIncrement the framerate increment in 1/s
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getFrameRateIncrement(IntPtr hCamera, ref float FramerateIncrement)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &FramerateIncrement)
            {
                return Camera_getFrameRateIncrement(cam, p);
            }
        }

        //------------------------------------------------------------------------------
        // 14 - Controlling camera: Exposure
        //------------------------------------------------------------------------------

        /**
        * Set exposure time.
        * The camera will be adjusted to a new exposure time
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param ExposureTime new exposure time in s
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setExposureTime(IntPtr hCamera, float ExposureTime)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setExposureTime(cam, ExposureTime);
        }

        /**
        * Get exposure time.
        * The currently programmed exposure time will be retrieved
        *         
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param ExposureTime currently programmed exposure time in s
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureTime(IntPtr hCamera, ref float ExposureTime)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &ExposureTime)
            {
                return Camera_getExposureTime(cam, p);
            }
        }


        /** Get exposure time min.
        *  The minimal setting for exposure time will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MinExposureTime the minimal exposure time setting in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureTimeMin(IntPtr hCamera, ref float MinExposureTime)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MinExposureTime)
            {
                return Camera_getExposureTimeMin(cam, p);
            }
        }



        /** Get exposure time max.
        *  The maximal setting for exposure will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxExposureTime the maximal exposure time setting in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureTimeMax(IntPtr hCamera, ref float MaxExposureTime)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MaxExposureTime)
            {
                return Camera_getExposureTimeMax(cam, p);
            }
        }

        /** Get exposure time range.
        *  The currently available exposure time range will be retrieved.
        *  NOTE: The received values will apply to free-running mode. In triggered mode the
        *        usual exposure time is limited to slightly more than 1 second. Exposure
        *        times above 1 second require changes in internal camera settings. Please
        *        contact SVS VISTEK if the camera needs to run in that exposure time range.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MinExposureTime currently available minimal exposure time in microseconds
        *  @param MaxExposureTime currently available maximal exposure time in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureTimeRange(IntPtr hCamera,
                                        ref float MinExposureTime,
                                        ref float MaxExposureTime)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* pmin = &MinExposureTime)
            {
                fixed (float* pmax = &MaxExposureTime)
                {
                    return Camera_getExposureTimeRange(cam, pmin, pmax);
                }
            }





        }


        /** Get exposure time increment.
        *  The increment for exposure time will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ExposureTimeIncrement the exposure time increment in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureTimeIncrement(IntPtr hCamera, ref float ExposureTimeIncrement)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &ExposureTimeIncrement)
            {
                return Camera_getExposureTimeIncrement(cam, p);
            }
        }

        /**
        * Set exposure delay
        * The camera's exposure delay in micro seconds relative to the trigger
        * pulse will be set to the provided value. The delay will become active
        * each time an active edge of an internal or external trigger pulse arrives
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param ExposureDelay the new value for exposure delay
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setExposureDelay(IntPtr hCamera, float ExposureDelay)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setExposureDelay(cam, ExposureDelay);
        }

        /**
        * Get exposure delay
        * The camera's current exposure delay will be 	returned in micro seconds
        * relative to the trigger pulse
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param ExposureDelay the currently programmed value for exposure delay
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureDelay(IntPtr hCamera, ref float ExposureDelay)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &ExposureDelay)
            {
                return Camera_getExposureDelay(cam, p);
            }
        }

        /** Get maximal exposure delay.
        *  The camera's maximal exposure delay will be 	returned in micro seconds
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxExposureDelay the maximal value for exposure delay in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureDelayMax(IntPtr hCamera, ref float MaxExposureDelay)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MaxExposureDelay)
            {
                return Camera_getExposureDelayMax(cam, p);
            }
        }

        /** Get exposure delay increment.
        *  The camera's exposure delay increment will be 	returned in micro seconds
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ExposureDelayIncrement the exposure delay increment in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getExposureDelayIncrement(IntPtr hCamera, ref float ExposureDelayIncrement)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &ExposureDelayIncrement)
            {
                return Camera_getExposureDelayIncrement(cam, p);
            }
        }


        //------------------------------------------------------------------------------
        // 15 - Controlling camera: Gain and offset
        //------------------------------------------------------------------------------

        /**
        * Set gain.
        * The camera will be adjusted to a new analog gain
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Gain currently programmed analog gain
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setGain(IntPtr hCamera, float Gain)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setGain(cam, Gain);
        }

        /**
        * Get gain.
        * The currently programmed analog gain will be retrieved
        * Note: A gain of 1.0 is represented as integer 128 in the appropriate camera
        * register. Thus the gain can be adjusted only in discrete steps.
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Gain currently programmed analog gain
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getGain(IntPtr hCamera, ref float Gain)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &Gain)
            {
                return Camera_getGain(cam, p);
            }
        }

        /** Get maximal gain.
        *  The maximal analog gain will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxGain the maximal analog gain
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getGainMax(IntPtr hCamera, ref float MaxGain)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MaxGain)
            {
                return Camera_getGainMax(cam, p);
            }
        }

        /** Get maximal extended gain.
        *  The maximal extended analog gain will be 	returned. An extended gain
        *  allows for operating a camera outside specified range. Noise and
        *  distortions will increase above those values that are met inside
        *  specified range.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxGainExtended the maximal analog gain
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getGainMaxExtended(IntPtr hCamera, ref float MaxGainExtended)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MaxGainExtended)
            {
                return Camera_getGainMaxExtended(cam, p);
            }
        }


        /** Get gain increment.
        *  The analog gain increment will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param GainIncrement the analog gain increment
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getGainIncrement(IntPtr hCamera, ref float GainIncrement)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &GainIncrement)
            {
                return Camera_getGainIncrement(cam, p);
            }
        }


        /**
        * Set offset
        * The ofset value will be set to the provided value
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Offset the new value for offset
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setOffset(IntPtr hCamera, float Offset)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setOffset(cam, Offset);
        }

        /**
        * Get offset
        * The current offset value will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Offset the currently programmed value for offset
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getOffset(IntPtr hCamera, ref float Offset)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &Offset)
            {
                return Camera_getOffset(cam, p);
            }
        }

        /** Get maximal offset
        *  The maximal offset value will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxOffset the maximal value for pixel offset
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getOffsetMax(IntPtr hCamera, ref float MaxOffset)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MaxOffset)
            {
                return Camera_getOffsetMax(cam, p);
            }
        }

        //------------------------------------------------------------------------------
        // 16 - Controlling camera: Auto gain / exposure
        //------------------------------------------------------------------------------

        /** Set auto gain enabled
        *  The auto gain status will be switched on or off.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param isAutoGainEnabled whether auto gain has to be enabled or disabled
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAutoGainEnabled(IntPtr hCamera, bool isAutoGainEnabled)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAutoGainEnabled(cam, isAutoGainEnabled);
        }

        /** Get auto gain enabled
        *  Current auto gain status will be 	returned.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param isAutoGainEnabled whether auto gain is enabled or disabled
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAutoGainEnabled(IntPtr hCamera, ref bool isAutoGainEnabled)
        {
            void* cam = hCamera.ToPointer();
            fixed (bool* p = &isAutoGainEnabled)
            {
                return Camera_getAutoGainEnabled(cam, p);
            }
        }

        /** Set auto gain brightness
        *  The target brightness (0..255) will be set which the camera tries to
        *  reach automatically when auto gain/exposure is enabled. The range
        *  0..255 always applies independently from pixel depth.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param Brightness the target brightness for auto gain enabled
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAutoGainBrightness(IntPtr hCamera, float Brightness)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAutoGainBrightness(cam, Brightness);
        }

        /** Get auto gain brightness
        *  The target brightness (0..255) will be 	returned that the camera tries
        *  to reach automatically when auto gain/exposure is enabled.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param Brightness a pointer to a target brightness value
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAutoGainBrightness(IntPtr hCamera, ref float Brightness)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &Brightness)
            {
                return Camera_getAutoGainBrightness(cam, p);
            }
        }

        /** Set auto gain dynamics
        *  AutoGain PID regulator's time constants for the I (integration) and
        *  D (differentiation) components will be set to new values.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param AutoGainParameterI the I parameter in a PID regulation loop
        *  @param AutoGainParameterD the D parameter in a PID regulation loop
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAutoGainDynamics(IntPtr hCamera,
                                        float AutoGainParameterI,
                                        float AutoGainParameterD)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAutoGainDynamics(cam, AutoGainParameterI, AutoGainParameterD);
        }

        /** Get auto gain dynamics
        *  AutoGain PID regulator's time constants for the I (integration) and
        *  D (differentiation) components will be retrieved from the camera.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param AutoGainParameterI the I parameter in a PID regulation loop
        *  @param AutoGainParameterD the D parameter in a PID regulation loop
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAutoGainDynamics(IntPtr hCamera,
                                        ref float AutoGainParameterI,
                                        ref float AutoGainParameterD)
        {
            void* cam = hCamera.ToPointer();

            fixed (float* pI = &AutoGainParameterI)
            {
                fixed (float* pD = &AutoGainParameterD)
                {
                    return Camera_getAutoGainDynamics(cam, pI, pD);
                }
            }
        }


        /** Set auto gain limits
        *  The minimal and maximal gain will be determined that the camera
        *  must not exceed in auto gain/exposure mode.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param MinGain the minimal allowed gain value
        *  @param MaxGain the maximal allowed gain value
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAutoGainLimits(IntPtr hCamera,
                                    float MinGain,
                                    float MaxGain)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAutoGainLimits(cam, MinGain, MaxGain);
        }

        /** Get auto gain limits
        *  The minimal and maximal gain will be 	returned that the camera
        *  must not exceed in auto gain/exposure mode.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param MinGain a pointer to the minimal allowed gain value
        *  @param MaxGain a pointer to the maximal allowed gain value
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAutoGainLimits(IntPtr hCamera,
                                    ref float MinGain,
                                    ref float MaxGain)
        {
            void* cam = hCamera.ToPointer();

            fixed (float* pmin = &MinGain)
            {
                fixed (float* pmax = &MaxGain)
                {
                    return Camera_getAutoGainLimits(cam, pmin, pmax);
                }
            }
        }

        /** Set auto exposure limits
        *  The minimal and maximal exposure will be determined that the camera
        *  must not exceed in auto gain/exposure mode.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param MinExposure the minimal allowed exposure value
        *  @param MaxExposure the maximal allowed exposure value
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAutoExposureLimits(IntPtr hCamera,
                                        float MinExposure,
                                        float MaxExposure)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAutoExposureLimits(cam, MinExposure, MaxExposure);
        }

        /** Set auto exposure limits
        *  The minimal and maximal exposure will be determined that the camera
        *  must not exceed in auto gain/exposure mode.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param MinExposure the minimal allowed exposure value
        *  @param MaxExposure the maximal allowed exposure value
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAutoExposureLimits(IntPtr hCamera,
                                        ref float MinExposure,
                                        ref float MaxExposure)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* pmin = &MinExposure)
            {
                fixed (float* pmax = &MaxExposure)
                {
                    return Camera_getAutoExposureLimits(cam, pmin, pmax);
                }
            }
        }


        //------------------------------------------------------------------------------
        // 17 - Controlling camera: Acquisition trigger
        //------------------------------------------------------------------------------

        /** Set acquisition control.
        *  The camera's acquisition will be controlled (start/stop).
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param AcquisitionControl the new setting for acquisition control
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAcquisitionControl(IntPtr hCamera, ACQUISITION_CONTROL AcquisitionControl)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAcquisitionControl(cam, AcquisitionControl);
        }


        /** Get acquisition control.
        *  The camera's current acquisition control (start/stop) will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param AcquisitionControl the currently programmed setting for acquisition control
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAcquisitionControl(IntPtr hCamera, ref ACQUISITION_CONTROL AcquisitionControl)
        {
            void* cam = hCamera.ToPointer();
            fixed (ACQUISITION_CONTROL* p = &AcquisitionControl)
            {
                return Camera_getAcquisitionControl(cam, p);
            }
        }

        /**
        * Set acquisition mode.
        * The camera's acquisition mode will be set to the selected value
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param AcquisitionMode the new setting for acquisition mode
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAcquisitionMode(IntPtr hCamera, ACQUISITION_MODE AcquisitionMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAcquisitionMode(cam, AcquisitionMode);
        }


        /** Set Acquisition mode and start
        *  In addition to setting the acquisition mode it can be determined whether
        *  acquisition control will go to enabled or stay on disabled after the new
        *  acquisition mode has been adjusted
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param AcquisitionMode the new setting for acquisition mode
        *  @param AcquisitionStart whether camera control switches to START immediately
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAcquisitionModeAndStart(IntPtr hCamera, ACQUISITION_MODE AcquisitionMode,
                                                bool AcquisitionStart)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAcquisitionModeAndStart(cam, AcquisitionMode, AcquisitionStart);
        }

        /**
        * Get acquisition mode.
        * The camera's current acquisition mode will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param AcquisitionMode the currently programmed setting for acquisition mode
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAcquisitionMode(IntPtr hCamera, ref ACQUISITION_MODE AcquisitionMode)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (ACQUISITION_MODE* p = &AcquisitionMode)
            {
                rval = Camera_getAcquisitionMode(cam, p);
            }
            return rval;
        }

        /** Software trigger.
        *  The camera will be triggerred for starting a new acquisition cycle.
        *  A mandatory pre-requisition for a successfull software trigger is to have
        *  the camera set to ACQUISITION_MODE_SOFTWARE_TIGGER before.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_softwareTrigger(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_softwareTrigger(cam);
        }

        /** Software trigger ID. (defined but not yet available)
        *  The camera will be triggerred for starting a new acquisition cycle.
        *  A mandatory pre-requisition for a successfull software trigger is to have
        *  the camera set to ACQUISITION_MODE_SOFTWARE_TIGGER before.
        *  In addition to a usual software trigger, an ID will be accepted that
        *  can be written into the image on demand, e.g. for maintaining a unique
        *  trigger/image reference
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TriggerID an ID to be transferred into first bytes of resulting image data
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_softwareTriggerID(IntPtr hCamera, int TriggerID)
        {
            void* cam = hCamera.ToPointer();
            return Camera_softwareTriggerID(cam, TriggerID);
        }


        /** Software trigger ID enable. (defined but not yet available)
        *  The "software trigger ID" mode will be enabled respectively disabled
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TriggerIDEnable whether "trigger ID" will be enabled or disabled
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_softwareTriggerIDEnable(IntPtr hCamera, bool TriggerIDEnable)
        {
            void* cam = hCamera.ToPointer();
            return Camera_softwareTriggerIDEnable(cam, TriggerIDEnable);
        }


        /**
        * Set trigger polarity
        * The camera's trigger polarity will be set to the selected value
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param TriggerPolarity the new setting for trigger polarity
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setTriggerPolarity(IntPtr hCamera, TRIGGER_POLARITY TriggerPolarity)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setTriggerPolarity(cam, TriggerPolarity);
        }

        /**
        * Get trigger polarity
        * The camera's current trigger polarity will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param TriggerPolarity the currently programmed setting for trigger polarity
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTriggerPolarity(IntPtr hCamera, ref TRIGGER_POLARITY TriggerPolarity)
        {
            void* cam = hCamera.ToPointer();
            fixed (TRIGGER_POLARITY* p = &TriggerPolarity)
            {
                return Camera_getTriggerPolarity(cam, p);
            }
        }

        //-----------------------   PIV mode  ----------------------------------------

        /** Set a new PIV mode
        *  The camera's PIV mode will be enabled or disabled.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param PivMode the new setting for PivMode
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPivMode(IntPtr hCamera, PIV_MODE PivMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPivMode(cam, PivMode);
        }

        /**
        * Get PIV Mode 
        * Check if camera's PIV mode is enabled or disabled.
        *  The state of camera's current Piv mode will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param PivMode the currently programmed setting for PivMode
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPivMode(IntPtr hCamera, ref PIV_MODE PivMode)
        {
            void* cam = hCamera.ToPointer();
            fixed (PIV_MODE* p = &PivMode)
            {
                return Camera_getPivMode(cam, p);
            }
        }

        //----------------------------------  Debouncer  ---------------------------------------

        /** Set Debouncer  duration
        *   The camera's Debouncer duration will be set to the selected value
        *
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param DebouncerDuration the new setting for DebouncerDuration in ticks.
        *  The systemclock is 66666666 HZ.  (66666666 ticks per second)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setDebouncerDuration(IntPtr hCamera, float DebouncerDuration)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setDebouncerDuration(cam, DebouncerDuration);
        }

        /** Get Debouncer  duration
        *  The camera's Debouncer  duration will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Duration the currently programmed setting for DebouncerDuration in ticks.
        *  The systemclock is 66666666 HZ.  (66666666 ticks per second) 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getDebouncerDuration(IntPtr hCamera, ref float Duration)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &Duration)
            {
                return Camera_getDebouncerDuration(cam, p);
            }
        }

        //--------------------------  Prescaler  ------------------------------------------------

        /** Set prescaler devisor
        *   The camera's prescaler Devisor will be set to the selected value
        *
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param PrescalerDevisor the new setting for PrescalerDevisor
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPrescalerDevisor(IntPtr hCamera, uint PrescalerDevisor)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPrescalerDevisor(cam, PrescalerDevisor);
        }

        /** Get prescaler devisor
        *  The camera's prescaler devisor will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param PrescalerDevisor the currently programmed setting for PrescalerDevisor. 
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPrescalerDevisor(IntPtr hCamera, ref uint PrescalerDevisor)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &PrescalerDevisor)
            {
                return Camera_getPrescalerDevisor(cam, p);
            }
        }

        //-----------------------------  Sequencer  -----------------------------------------------
        /** load Sequence parameters 
        *  New sequence parameters will be loaded from a XML file into the camera  
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Filename a complete path and file name where to load the settings from
        *  @return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_loadSequenceParameters(IntPtr hCamera, string Filename)
        {
            void* cam = hCamera.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(Filename);
            fixed (byte* p = bytes)
            {
                sbyte* ps = (sbyte*)p;
                SVSGigeApiReturn ret = Camera_loadSequenceParameters(cam, ps);
                Filename = new string(ps);
                return ret;
            }
        }

        /** Start Sequencer
        * Start acquisition using sequencer.
        * This will occur after loading the appropriate sequence parameters. 
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_startSequencer(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_startSequencer(cam);
        }

        //------------------------------------------------------------------------------
        // 18 - Controlling camera: Strobe
        //------------------------------------------------------------------------------

        /**
        * Set strobe polarity
        * The camera's strobe polarity will be set to the selected value
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param StrobePolarity the new setting for strobe polarity
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setStrobePolarity(IntPtr hCamera, STROBE_POLARITY StrobePolarity)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setStrobePolarity(cam, StrobePolarity);
        }

        /** Set strobe polarity extended
        *  The camera's strobe polarity will be set to the selected value
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobePolarity the new setting for strobe polarity
        *  @param StrobIndex the index of the current strobe channel 
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setStrobePolarityExtended(IntPtr hCamera, STROBE_POLARITY StrobePolarity, int StrobeIndex)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setStrobePolarityExtended(cam, StrobePolarity, StrobeIndex);
        }

        /**
        * Get strobe polarity
        * The camera's current strobe polarity will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param StrobePolarity the currently programmed setting for strobe polarity
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobePolarity(IntPtr hCamera,
        STROBE_POLARITY* StrobePolarity)
        {
            void* cam = hCamera.ToPointer();
            return Camera_getStrobePolarity(cam, StrobePolarity);
        }

        /** Get strobe polarity extended
        *  The camera's current strobe polarity will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobePolarity the currently programmed setting for strobe polarity
        *  @param StrobIndex the index of the current strobe channel 
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobePolarityExtended(IntPtr hCamera, ref STROBE_POLARITY StrobePolarity, int StrobeIndex)
        {
            void* cam = hCamera.ToPointer();
            fixed (STROBE_POLARITY* p = &StrobePolarity)
            {
                return Camera_getStrobePolarityExtended(cam, p, StrobeIndex);
            }
        }

        /**
        * Set strobe position
        * The camera's strobe position in micro seconds relative to the trigger
        * pulse will be set to the selected value
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param StrobePosition the new value for strobe position in microseconds
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setStrobePosition(IntPtr hCamera, float StrobePosition)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setStrobePosition(cam, StrobePosition);
        }

        /** Set strobe position extended
        *  The camera's strobe position in micro seconds relative to the trigger
        *  pulse will be set to the selected value
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobePosition the new value for strobe position in microseconds
        *  @param StrobIndex the index of the current strobe channel 
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setStrobePositionExtended(IntPtr hCamera, float StrobePosition, int StrobeIndex)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setStrobePositionExtended(cam, StrobePosition, StrobeIndex);
        }

        /**
        * Get strobe position
        * The camera's current strobe position will be 	returned in micro seconds
        * relative to the trigger pulse
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param StrobePosition the currently programmed value for strobe position in microseconds
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobePosition(IntPtr hCamera, ref float StrobePosition)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &StrobePosition)
            {
                return Camera_getStrobePosition(cam, p);
            }
        }

        /** Get strobe position extended
        *  The camera's current strobe position will be 	returned in micro seconds
        *  relative to the trigger pulse
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobePosition the currently programmed value for strobe position in microseconds
        *  @param StrobIndex the index of the current strobe channel 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobePositionExtended(IntPtr hCamera, ref float StrobePosition, int StrobeIndex)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &StrobePosition)
            {
                return Camera_getStrobePositionExtended(cam, p, StrobeIndex);
            }
        }

        /**
        * Set strobe duration
        * The camera's strobe duration in micro seconds will be set to the selected
        * value
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param StrobeDuration the new value for strobe duration in microseconds
        * @	return success or error code
        */
        public unsafe 
        SVSGigeApiReturn
        Gige_Camera_setStrobeDuration(IntPtr hCamera, float StrobeDuration)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setStrobeDuration(cam, StrobeDuration);
        }

        /** Set strobe duration extended
        *  The camera's strobe duration in micro seconds will be set to the selected
        *  value
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobeDuration the new value for strobe duration in microseconds
        *  @param StrobIndex the index of the current strobe channel 
        *  @	return success or error code
        */
        public unsafe 
        SVSGigeApiReturn
        Gige_Camera_setStrobeDurationExtended(IntPtr hCamera, float StrobeDuration, int StrobeIndex)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setStrobeDurationExtended(cam, StrobeDuration, StrobeIndex);
        }

        /**
        * Get strobe duration
        * The camera's current strobe duration in micro seconds will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param StrobeDuration the currently programmed value for strobe duration in microseconds
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobeDuration(IntPtr hCamera,
        float* StrobeDuration)
        {
            void* cam = hCamera.ToPointer();
            return Camera_getStrobeDuration(cam, StrobeDuration);
        }

        /** Get strobe duration extended
        *  The camera's current strobe duration in micro seconds will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobeDuration the currently programmed value for strobe duration in microseconds
        *  @param StrobIndex the index of the current strobe channel 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobeDurationExtended(IntPtr hCamera, ref float StrobeDuration, int StrobeIndex)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &StrobeDuration)
            {
                return Camera_getStrobeDurationExtended(cam, p, StrobeIndex);
            }
        }

        /** Get maximal strobe position
        *  The camera's maximal strobe position (delay) will be 	returned in micro seconds
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobePositionMax the maximal value for strobe position in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobePositionMax(IntPtr hCamera, ref float MaxStrobePosition)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &MaxStrobePosition)
            {
                return Camera_getStrobePositionMax(cam, p);
            }
        }

        /** Get strobe position increment
        *  The camera's strobe position increment will be 	returned in micro seconds
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param StrobePositionIncrement the strobe position increment in microseconds
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getStrobePositionIncrement(IntPtr hCamera, ref float StrobePositionIncrement)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &StrobePositionIncrement)
            {
                return Camera_getStrobePositionIncrement(cam, p);
            }
        }

        //------------------------------------------------------------------------------
        // 19 - Controlling camera: Tap balance
        //------------------------------------------------------------------------------

        /** Save tap balance settings.
        *  Current settings for tap balance will be saved into a XML file. Usually the
        *  tap balance is adjusted during camera production. Whenever a need exists for
        *  changing those factory settings, e.g. for balancing image sensor characteristics
        *  dependent on gain, particular settings can be determined, saved to files and
        *  later loaded on demand.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Filename a complete path and filename where to save the settings
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_saveTapBalanceSettings(IntPtr hCamera, string Filename)
        {
            void* cam = hCamera.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(Filename);
            fixed (byte* p = bytes)
            {
                sbyte* ps = (sbyte*)p;
                return Camera_saveTapBalanceSettings(cam, ps);
            }
        }

        /** Load tap balance settings.
        *  New settings for tap balance will be loaded from a XML file.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Filename a complete path and filename where to load the settings from
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_loadTapBalanceSettings(IntPtr hCamera, string Filename)
        {
            void* cam = hCamera.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(Filename);
            fixed (byte* p = bytes)
            {
                sbyte* ps = (sbyte*)p;
                return Camera_loadTapBalanceSettings(cam, ps);
            }
        }

        /** Set tap configuration
        *  The camera will be controlled for working with one or two taps
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TapCount the number of taps (1, 2) to be used by the camera
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setTapConfiguration(IntPtr hCamera, int TapCount)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setTapConfiguration(cam, TapCount);
        }

        /** Get tap configuration
        *  The camera will be queried whether it is working with one or two taps
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TapCount the number of taps (1, 2) currently used by the camera is 	returned
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTapConfiguration(IntPtr hCamera, ref int TapCount)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* p = &TapCount)
            {
                return Camera_getTapConfiguration(cam, p);
            }
        }

        /**
        *	Set tap configuration extended
        *  The camera will be controlled for working with one of the following tap configurations: 
        *  SVGIGE_SELECT_SINGLE_TAP 
        *	SVGIGE_SELECT_DUAL_TAP_H	
        *	SVGIGE_SELECT_DUAL_TAP_V	
        *	SVGIGE_SELECT_QUAD		 	
        *  
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param SelectedTapConfig the selected tap configuration to be used by the camera 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setTapConfigurationEx(IntPtr hCamera, SVGIGE_TAP_CONFIGURATION_SELECT SelectedTapConfig)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setTapConfigurationEx(cam, SelectedTapConfig);
        }

        /**
        *  Get tap configuration extended
        *  The camera will be queried whether it is working with one of the following configurations:
        *  SVGIGE_SELECT_SINGLE_TAP  
        *	SVGIGE_SELECT_DUAL_TAP_H	
        *	SVGIGE_SELECT_DUAL_TAP_V	
        *	SVGIGE_SELECT_QUAD	
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TapConfig the tap configuration currently used by the camera is 	returned 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTapConfigurationEx(IntPtr hCamera, ref SVGIGE_TAP_CONFIGURATION_SELECT TapConfig)
        {
            void* cam = hCamera.ToPointer();
            fixed (SVGIGE_TAP_CONFIGURATION_SELECT* p = &TapConfig)
            {
                return Camera_getTapConfigurationEx(cam, p);
            }
        }

        /** Set auto tap balance mode
        *  One of the modes (OFF,ONCE,CONTINUOUS,RESET) will be applied for an auto
        *  tap balance procedure.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param AutoTapBalanceMode the mode for auto tap balancing to be activated
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAutoTapBalanceMode(IntPtr hCamera, SVGIGE_AUTO_TAP_BALANCE_MODE AutoTapBalanceMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAutoTapBalanceMode(cam, AutoTapBalanceMode);
        }

        /** Get auto tap balance mode
        *  Currently adjusted auto tap balance mode will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param AutoTapBalanceMode a pointer to a value for auto tap balancing
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAutoTapBalanceMode(IntPtr hCamera, ref SVGIGE_AUTO_TAP_BALANCE_MODE AutoTapBalanceMode)
        {
            void* cam = hCamera.ToPointer();
            fixed (SVGIGE_AUTO_TAP_BALANCE_MODE* p = &AutoTapBalanceMode)
            {
                return Camera_getAutoTapBalanceMode(cam, p);
            }
        }


        /** Set tap balance
		*  2011-08-18 - DEPRECATED. Please use setTapGain() instead.  
        *  A provided tap balance in [dB] will be transferred to camera.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TapBalance the new value for tap balance to be activated
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setTapBalance(IntPtr hCamera, float TapBalance)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setTapBalance(cam, TapBalance);
        }

        /** Get tap balance
		* 2011-08-18 - DEPRECATED. Please use getTapGain() instead 
        *  Currently adjusted tap balance in [dB] will be retrieved from camera.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TapBalance a pointer to a tap balance value
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTapBalance(IntPtr hCamera, ref float TapBalance)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &TapBalance)
            {
                return Camera_getTapBalance(cam, p);
            }
        }

        /** Set tap gain
        *  A provided tap gain in [dB] will be transferred to camera.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TapGain  one of the defined tap selectors 
        *  @param TapSelect the new value for tap gain 
        *  @return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setTapGain(IntPtr hCamera, float TapGain, SVGIGE_TAP_SELECT TapSelect)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setTapGain(cam, TapGain, TapSelect);

        }

        /** Get tap gain
         *  Currently adjusted tap gain in [dB] will be queried from camera.
         *
         *  @see CameraContainer_getCamera()
         *  @param hCamera a camera handle received from CameraContainer_getCamera()
         *  @param TapSelect one of the defined tap selectors 
         *  @param TapGain the new value for tap gain 
         *  @return success or error code
         */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTapGain(IntPtr hCamera, ref float TapBalance, SVGIGE_TAP_SELECT TapSelect)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* p = &TapBalance)
            {
                return Camera_getTapGain(cam, p, TapSelect);
            }

        }

        //------------------------------------------------------------------------------
        // 20 - Controlling camera: Image parameter
        //------------------------------------------------------------------------------

        /** Get imager width.
        *  The imager width will be retrieved from the camera
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ImagerWidth a reference to the imager width value
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getImagerWidth(IntPtr hCamera, ref int ImagerWidth)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* p = &ImagerWidth)
            {
                return Camera_getImagerWidth(cam, p);
            }
        }

        /** Get imager height.
        *  The imager height will be retrieved from the camera
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ImagerHeight a reference to the imager height value
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getImagerHeight(IntPtr hCamera, ref int ImagerHeight)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* p = &ImagerHeight)
            {
                return Camera_getImagerHeight(cam, p);
            }
        }

        /**
        * Get size X.
        * The horizontal picture size X will be retrieved from the camera
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param SizeX a reference to the size X value
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getSizeX(IntPtr hCamera, ref int SizeX)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (int* sx = &SizeX)
            {
                rval = Camera_getSizeX(cam, sx);
            }
            return rval;
        }

        /**
        * Get size Y.
        * The vertical picture size Y will be retrieved from the camera
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param SizeY a reference to the size Y value
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getSizeY(IntPtr hCamera, ref int SizeY)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (int* sy = &SizeY)
            {
                rval = Camera_getSizeY(cam, sy);
            }
            return rval;
        }

        /**
        * Get pitch.
        * The number of bytes in a row will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param Pitch a reference to the pitch value
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPitch(IntPtr hCamera, ref int Pitch)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* p = &Pitch)
            {
                return Camera_getPitch(cam, p);
            }
        }

        /**
        * Get image size.
        * The number of bytes in an image will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param ImageSize a reference to the image size value
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getImageSize(IntPtr hCamera, ref int ImageSize)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* p = &ImageSize)
            {
                return Camera_getImageSize(cam, p);
            }
        }

        /**
        * Set binning mode.
        * The camera's binning mode will be set to the selected value
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param BinningMode the new setting for binning mode
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setBinningMode(IntPtr hCamera, BINNING_MODE BinningMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setBinningMode(cam, BinningMode);
        }

        /**
        * Get binning mode.
        * The camera's current binning mode will be 	returned
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param BinningMode the currently programmed setting for binning mode
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getBinningMode(IntPtr hCamera, ref BINNING_MODE BinningMode)
        {
            void* cam = hCamera.ToPointer();
            fixed (BINNING_MODE* p = &BinningMode)
            {
                return Camera_getBinningMode(cam, p);
            }
        }

        /** Set area of interest (AOI)
        *  The camera will be switched to partial scan mode and an AOI will be set
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param SizeX the number of pixels in one row
        *  @param SizeY the number of scan lines
        *  @param OffsetX a left side offset of the scanned area
        *  @param OffsetY an upper side offset of the scanned area
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAreaOfInterest(IntPtr hCamera, int SizeX, int SizeY, int OffsetX, int OffsetY)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAreaOfInterest(cam, SizeX, SizeY, OffsetX, OffsetY);
        }

        /** Get area of interest(AOI)
        *  The currently set parameters for partial scan will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param SizeX the number of pixels in one row
        *  @param SizeY the number of scan lines
        *  @param OffsetX a left side offset of the scanned area
        *  @param OffsetY an upper side offset of the scanned area
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAreaOfInterest(IntPtr hCamera,
                                    ref int SizeX,
                                    ref int SizeY,
                                    ref int OffsetX,
                                    ref int OffsetY)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* x = &SizeX)
            {
                fixed (int* y = &SizeY)
                {
                    fixed (int* ox = &OffsetX)
                    {
                        fixed (int* oy = &OffsetY)
                        {
                            return Camera_getAreaOfInterest(cam,
                                                                    x,
                                                                    y,
                                                                    ox,
                                                                    oy);
                        }
                    }
                }
            }
        }

        /** Get minimal/maximal area of interest(AOI).
        *  The boundaries for partial scan will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MinSizeX the minimal AOI width
        *  @param MinSizeY the minimal AOI height
        *  @param MaxSizeX the maximal AOI width
        *  @param MaxSizeY the maximal AOI height
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAreaOfInterestRange(IntPtr hCamera, ref int MinSizeX, ref int MinSizeY, ref int MaxSizeX, ref int MaxSizeY)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* minx = &MinSizeX)
            {
                fixed (int* miny = &MinSizeY)
                {
                    fixed (int* maxx = &MaxSizeX)
                    {
                        fixed (int* maxy = &MaxSizeY)
                        {

                            return Camera_getAreaOfInterestRange(cam, minx, miny, maxx, maxy);
                        }
                    }
                }
            }
        }

        /** Get area of interest(AOI) increment
        *  The increment for partial scan parameters will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param SizeXIncrement the increment for AOI width
        *  @param SizeYIncrement the increment for AOI height
        *  @param OffsetXIncrement the increment for AOI width offset
        *  @param OffsetYIncrement the increment for AOI height offset
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAreaOfInterestIncrement(IntPtr hCamera,
                                                ref int SizeXIncrement,
                                                ref int SizeYIncrement,
                                                ref int OffsetXIncrement,
                                                ref int OffsetYIncrement)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* x = &SizeXIncrement)
            {
                fixed (int* y = &SizeYIncrement)
                {
                    fixed (int* ox = &OffsetXIncrement)
                    {
                        fixed (int* oy = &OffsetYIncrement)
                        {
                            return Camera_getAreaOfInterestIncrement(cam, x, y, ox, oy);
                        }
                    }
                }
            }
        }

        /** Reset timestamp counter
        *  The camera's timestamp counter will be set to zero.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_resetTimestampCounter(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_resetTimestampCounter(cam);
        }

        /** Get timestamp counter
        *  Current value of the camera's timestamp counter will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TimestampCounter the current value of the timestamp counter
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTimestampCounter(IntPtr hCamera, ref double TimestampCounter)
        {
            void* cam = hCamera.ToPointer();
            fixed (double* d = &TimestampCounter)
            {
                return Camera_getTimestampCounter(cam, d);
            }
        }

        /** Get timestamp tick frequency
        *  A camera's timestamp tick frequency will be 	returned.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param TimestampTickFrequency the camera's timestamp tick frequency
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getTimestampTickFrequency(IntPtr hCamera, ref double TimestampTickFrequency)
        {
            void* cam = hCamera.ToPointer();
            fixed (double* d = &TimestampTickFrequency)
            {
                return Camera_getTimestampTickFrequency(cam, d);
            }
        }

        //-----------------------------------------------------------------------------------------------------

        /** Set Flipping mode.
        *   The camera will be controlled for working with the following flipping mode if selected:
        *   REVERSE_OFF (without flipping)
        *   REVERSE_X (vertical flipping)
        *   REVERSE_Y (horizontal flipping)
        *   REVERSE_X_Y( horizontal and vertical flipping) 
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FlippingMode the new setting for flipping mode
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setFlippingMode(IntPtr hCamera, SVGIGE_FLIPPING_MODE FlippingMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setFlippingMode(cam, FlippingMode);
        }

        /** Get Flipping mode.
        *  The camera will be queried whether it is working with one of the following flipping mode:
        *   REVERSE_OFF (without flipping)
        *   REVERSE_X (vertical flipping)
        *   REVERSE_Y (horizontal flipping)
        *   REVERSE_X_Y( horizontal and vertical flipping)
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FlippingMode  the currently programmed setting for flipping mode
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getFlippingMode(IntPtr hCamera, ref SVGIGE_FLIPPING_MODE FlippingMode)
        {
            void* cam = hCamera.ToPointer();
            fixed (SVGIGE_FLIPPING_MODE* p = &FlippingMode)
            {
                return Camera_getFlippingMode(cam, p);
            }
        }

        //-----------------------------------------------------------------------------------------------------  
        /** Set Shutter mode.
        *  The camera will be controlled for working with the following shutter mode if selected:
        *  GLOBAL_SHUTTER 
        *  ROLLING_SHUTTER
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ShutterMode the new setting for shutter mode
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setShutterMode(IntPtr hCamera, SVGIGE_SHUTTER_MODE ShutterMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setShutterMode(cam, ShutterMode);
        }

        /** Get Shutter mode.
        *  The camera will be queried whether it is working with one of the following shutter mode:
        *  GLOBAL_SHUTTER 
        *  ROLLING_SHUTTER
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ShutterMode  the currently programmed setting for shutter mode
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getShutterMode(IntPtr hCamera, ref SVGIGE_SHUTTER_MODE ShutterMode)
        {
            void* cam = hCamera.ToPointer();
            fixed (SVGIGE_SHUTTER_MODE* p = &ShutterMode)
            {
                return Camera_getShutterMode(cam, p);
            }
        }

        //------------------------------------------------------------------------------
        // 21 - Controlling camera: Image appearance
        //------------------------------------------------------------------------------

        /**
        * Get pixel type.
        * The pixel type will be retrieved from the camera
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param PixelType a reference to the pixel type value
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn Gige_Camera_getPixelType(IntPtr hCamera, ref GVSP_PIXEL_TYPE PixelType)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (GVSP_PIXEL_TYPE* type = &(PixelType))
            {
                apiReturn = Camera_getPixelType(cam, type);
            }
            return apiReturn;
        }

        //[DllImport("SVGenSDK.dll", EntryPoint = "DoSomethingInC")]

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPixelType(IntPtr hCamera, GVSP_PIXEL_TYPE PixelType)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPixelType(cam, PixelType);
        }




        /**
        * Set pixel depth.
        * The number of bits for a pixel will be set to 8, 12 or 16 bits. Before this function 
        * is called the camera's feature vector should be queried whether the desired pixel depth
        * is supported
        *
        * @see Camera_isCameraFeature()
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param PixelDepth the intended value for pixel depth
        * @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPixelDepth(IntPtr hCamera, SVGIGE_PIXEL_DEPTH PixelDepth)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPixelDepth(cam, PixelDepth);
        }

        /**
        * Get pixel depth.
        * The camera's current setting for pixel depth will be queried.
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param PixelDepth an enum for the number of bits in a pixel will be 	returned
        * @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPixelDepth(IntPtr hCamera, ref SVGIGE_PIXEL_DEPTH PixelDepth)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (SVGIGE_PIXEL_DEPTH* depth = &(PixelDepth))
            {
                apiReturn = Camera_getPixelDepth(cam, depth);
            }
            return apiReturn;
        }


        /** setWhiteBalance.
        *  The provided values will be applied for white balance.
        *
        *  NOTE: The color component strength for Red, Green and Blue can either be
        *        provided by user or they can conveniently be calculated inside an image
        *        callback using the Image_estimateWhiteBalance() function.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Red balanced value for red color
        *  @param Green balanced value for green color
        *  @param Blue balanced value for blue color
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setWhiteBalance(IntPtr hCamera, float Red, float Green, float Blue)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setWhiteBalance(cam, Red, Green, Blue);
        }

        /** getWhiteBalance.
        *  Currently set values for white balance will be 	returned.
        *  Previously adjusted values will be 	returned either unchanged or adjusted
        *  if necessary. The 	returned values will be 100 and above where the color
        *  which got 100 assigned will be transferred unchanged, however two
        *  other color components might be enhanced above 100 for each image.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Red balanced value for red color
        *  @param Green balanced value for green color
        *  @param Blue balanced value for blue color
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getWhiteBalance(IntPtr hCamera, ref float Red, ref float Green, ref float Blue)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* red = &Red)
            {
                fixed (float* green = &Green)
                {
                    fixed (float* blue = &Blue)
                    {
                        return Camera_getWhiteBalance(cam, red, green, blue);
                    }
                }
            }
        }

        /** getWhiteBalanceMax.
        *  The maximal white-balance value for amplifying colors will be 	returned.
        *  A value of 1.0 is the reference for a balanced situation.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param WhiteBalanceMax the maximal white-balance (e.g. 4.0 or 2.0)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getWhiteBalanceMax(IntPtr hCamera, ref float WhiteBalanceMax)
        {
            void* cam = hCamera.ToPointer();
            fixed (float* f = &WhiteBalanceMax)
            {
                return Camera_getWhiteBalanceMax(cam, f);
            }
        }

        /** setGammaCorrection.
        *  A lookup table will be generated based on given gamma correction.
        *  Subsequently the lookup table will be uploaded to the camera.
        *  A gamma correction is supported in a range 0.4 - 2.5
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param GammaCorrection a gamma correction factor
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setGammaCorrection(IntPtr hCamera, float GammaCorrection)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setGammaCorrection(cam, GammaCorrection);
        }

        /** setGammaCorrectionExt.
        *  A lookup table will be generated based on given gamma correction.
        *  Additionally, a digital gain and offset will be taken into account.
        *  Subsequently the lookup table will be uploaded to the camera.
        *  A gamma correction is supported in a range 0.4 - 2.5
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param GammaCorrection a gamma correction factor
        *  @param DigitalGain a digital gain factor
        *  @param DigitalOffset a digital Offset factor
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setGammaCorrectionExt(IntPtr hCamera, float GammaCorrection, float DigitalGain, float DigitalOffset)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setGammaCorrectionExt(cam, GammaCorrection, DigitalGain, DigitalOffset);
        }

        /** setLowpassFilter.
        *  A filter will be enabled/disabled which smoothes an image inside
        *  a camera accordingly to a given algorithm, e.g. 3x3.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param LowpassFilter a control value for activating/deactivating smoothing
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLowpassFilter(IntPtr hCamera, LOWPASS_FILTER LowpassFilter)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLowpassFilter(cam, LowpassFilter);
        }

        /** getLowpassFilter.
        *  Current mode of a lowpass filter will be retrieved from camera.
        *
        *  @param hCamera a handle from a camera that has been opened before
        *  @param LowpassFilter the currently programmed lowpass filter will be 	returned
        *  @	return SVGigE_SUCCESS or an appropriate SVGigE error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLowpassFilter(IntPtr hCamera, ref LOWPASS_FILTER LowpassFilter)
        {
            void* cam = hCamera.ToPointer();
            fixed (LOWPASS_FILTER* lpf = &LowpassFilter)
            {
                return Camera_getLowpassFilter(cam, lpf);
            }
        }


        /** setLookupTableMode.
        *  The look-up table mode will be switched on or off
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LUTMode new setting for look-up table mode
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLookupTableMode(IntPtr hCamera, LUT_MODE LUTMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLookupTableMode(cam, LUTMode);
        }

        /** Get look-up table mode.
        *  The currently programmed look-up table mode will be retrieved
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LUTMode currently programmed look-up table mode
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLookupTableMode(IntPtr hCamera, ref LUT_MODE LUTMode)
        {
            void* cam = hCamera.ToPointer();
            fixed (LUT_MODE* lm = &LUTMode)
            {
                return Camera_getLookupTableMode(cam, lm);
            }
        }


        /** setLookupTable.
        *  A user-defined lookup table will be uploaded to the camera. The size has to match
        *  the lookup table size that is supported by the camera (1024 for 10to8 or 4096 for 12to8).
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LookupTable an array of user-defined lookup table values (bytes)
        *  @param LookupTableSize the size of the user-defined lookup table
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLookupTable(IntPtr hCamera, ref sbyte LookupTable, int LookupTableSize)
        {
            void* cam = hCamera.ToPointer();
            fixed (sbyte* sb = &LookupTable)
            {
                return Camera_setLookupTable(cam, sb, LookupTableSize);
            }

        }

        /** getLookupTable.
        *  The currently installed lookup table will be downloaded from the camera. The size of the
        *  reserved download space has to match the lookup table size (1024 for 10to8 or 4096 for 12to8).
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LookupTable an array for downloading the lookup table from camera
        *  @param LookupTableSize the size of the provided lookup table space
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLookupTable(IntPtr hCamera, ref sbyte LookupTable, int LookupTableSize)
        {
            void* cam = hCamera.ToPointer();
            fixed (sbyte* sb = &LookupTable)
            {
                return Camera_getLookupTable(cam, sb, LookupTableSize);
            }
        }

        /** startImageCorrection.
        *  A particular step inside of acquiring a correction image for either,
        *  flat field correction (FFC) or shading correction will be started:
        *    - acquire a black image
        *    - acquire a white image
        *    - save a correction image to camera's persistent memory
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ImageCorrectionStep a particular step from running an image correction
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_startImageCorrection(IntPtr hCamera, IMAGE_CORRECTION_STEP ImageCorrectionStep)
        {
            void* cam = hCamera.ToPointer();
            return Camera_startImageCorrection(cam, ImageCorrectionStep);
        }

        /** isIdleImageCorrection.
        *  A launched image correction processs will be checked whether a recently
        *  initiated image correction step has be finished:
        *    - acquire a black image
        *    - acquire a white image
        *    - save a correction image to camera's persistent memory
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ImageCorrectionStep a particular step from running an image correction
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_isIdleImageCorrection(IntPtr hCamera, ref IMAGE_CORRECTION_STEP ImageCorrectionStep, ref bool isIdle)
        {
            void* cam = hCamera.ToPointer();
            fixed (IMAGE_CORRECTION_STEP* p0 = &ImageCorrectionStep)
            {
                fixed (bool* b = &isIdle)
                {
                    return Camera_isIdleImageCorrection(cam, p0, b);
                }
            }
        }

        /** setImageCorrection.
        *  A camera will be switched to one of the following image correction modes
        *    - None (image correction is off)
        *    - Offset only (available for Flat Field Correction, FFC)
        *    - Enabled (image correction is on)
        *  If image correction is enabled, then it depends on the camera whether 
        *  Flat Field Correction (FFC) is enabled (gain and offset for each pixel) 
        *  or Shading Correction (gain interpolation for a group of adjacent pixels)
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ImageCorrectionMode one of above image correction modes
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setImageCorrection(IntPtr hCamera, IMAGE_CORRECTION_MODE ImageCorrectionMode)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setImageCorrection(cam, ImageCorrectionMode);
        }
        /** getImageCorrection.
        *  A camera will be queried for current image correction mode, either of:
        *    - None (image correction is off)
        *    - Offset only (available for Flat Field Correction, FFC)
        *    - Enabled (image correction is on)
        *  If image correction is enabled, then it depends on the camera whether 
        *  Flat Field Correction (FFC) is enabled (gain and offset for each pixel) 
        *  or Shading Correction (gain interpolation for a group of adjacent pixels)
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ImageCorrectionMode one of above image correction modes
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getImageCorrection(IntPtr hCamera, ref IMAGE_CORRECTION_MODE ImageCorrectionMode)
        {
            void* cam = hCamera.ToPointer();
            fixed (IMAGE_CORRECTION_MODE* p = &ImageCorrectionMode)
            {
                return Camera_getImageCorrection(cam, p);
            }
        }
        /** Set pixels correction Map
        * A camera will be switched to one of the following pixels correction maps if slected:
        *    - factory map
        *    - SVS map
        *    - custom map
        * the pixels correction control status of the currently slected map is enabled per default.
        * 
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param PixelsCorrectionMap one of above pixels correction maps
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPixelsCorrectionMap(IntPtr hCamera, PIXELS_CORRECTION_MAP_SELECT PixelsCorrectionMap)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPixelsCorrectionMap(cam, PixelsCorrectionMap);
        }

        /** Get pixels correction map 
        *  A camera will be queried for current selected pixels correction map
        *
        *  @see Camera_isCameraFeature()
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param PixelsCorrectionMap a currently selected map 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPixelsCorrectionMap(IntPtr hCamera, ref PIXELS_CORRECTION_MAP_SELECT PixelsCorrectionMap)
        {
            void* cam = hCamera.ToPointer();
            fixed (PIXELS_CORRECTION_MAP_SELECT* p = &PixelsCorrectionMap)
            {
                return Camera_getPixelsCorrectionMap(cam, p);
            }
        }
        /** Set Pixels Correction Control enabel.
        *  The Pixels Correction Control status will be switched on or off.
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param isPixelsCorrectionEnabled whether Pixels Correction control has to be enabled or disabled    
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPixelsCorrectionControlEnabel(IntPtr hCamera, bool isPixelsCorrectionEnabled)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPixelsCorrectionControlEnabel(cam, isPixelsCorrectionEnabled);
        }

        /** Get pixels correction control enabel.
        *  A camera will be queried for current pixels correction control status.
        *  the pixels correction can be enabled or disabled.
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param isPixelsCorrectionEnabled the programmed status of the pixels correction control.
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPixelsCorrectionControlEnabel(IntPtr hCamera, ref bool isPixelsCorrectionEnabled)
        {
            void* cam = hCamera.ToPointer();
            fixed (bool* b = &isPixelsCorrectionEnabled)
            {
                return Camera_getPixelsCorrectionControlEnabel(cam, b);
            }
        }

        /** Set Pixels Correction Control mark.
        *  The defect pixels will be marked or not.
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param isPixelsCorrectionMarked whether defect pixels have to be marked or not  
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPixelsCorrectionControlMark(IntPtr hCamera, bool isPixelsCorrectionMarked)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPixelsCorrectionControlMark(cam, isPixelsCorrectionMarked);
        }

        /** Get pixels correction control mark.
        *  A camera will be queried for current pixels correction control status. 
        *  the defect pixels can be marked or not.
        *  
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param isPixelsCorrectionMarked the programmed mark status of the pixels correction control
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPixelsCorrectionControlMark(IntPtr hCamera, ref bool isPixelsCorrectionMarked)
        {
            void* cam = hCamera.ToPointer();
            fixed (bool* b = &isPixelsCorrectionMarked)
            {
                return Camera_getPixelsCorrectionControlMark(cam, b);
            }
        }


        /** set pixels correction map offset 
        *  The offset x and y of the currently slected map will be set to the provided values
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param OffsetX the new value for pixel offset in x-axis 
        *  @param OffsetY the new value for pixel offset in y-axis 
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setPixelsCorrectionMapOffset(IntPtr hCamera, int OffsetX, int OffsetY)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setPixelsCorrectionMapOffset(cam, OffsetX, OffsetY);
        }

        /** Get pixels correction map offset 
        *  The offset x and y values of the currently slected map will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param OffsetX the currently programmed value for pixel offset in x-axis 
        *  @param OffsetY the currently programmed value for pixel offset in Y-axis 
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPixelsCorrectionMapOffset(IntPtr hCamera,
                                                ref int OffsetX,
                                                ref int OffsetY)
        {
            void* cam = hCamera.ToPointer();
            fixed (int* x = &OffsetX)
            {
                fixed (int* y = &OffsetY)
                {

                    return Camera_getPixelsCorrectionMapOffset(cam,
                                                            x,
                                                            y);
                }
            }
        }

        /** Get pixels correction map size
        *  The currently coordinates number of defect pixels in selected map will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MapSize the currently programmed number of pixels coordinates in selected map 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getPixelsCorrectionMapSize(IntPtr hCamera, ref uint MapSize)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &MapSize)
            {
                return Camera_getPixelsCorrectionMapSize(cam, p);
            }
        }

        /** Get maximal pixels correction map size
        *  The Maximal pixels coordinates number per map will be 	returned
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaximalprogrammedMapSize the Maximal programmed number of deffect pixels coordinates per map  
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getMaximalPixelsCorrectionMapSize(IntPtr hCamera, ref uint MaximalprogrammedMapSize)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &MaximalprogrammedMapSize)
            {
                return Camera_getMaximalPixelsCorrectionMapSize(cam, p);
            }
        }

        /** Set map index coordinate
        *  write a new X and Y coordinate accordingly to a map index that was specified
        *  as an input parameter
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Mapindex the map index from zero to one less the Mapsize 
        *  @param X_Coordinate the new X coordinate to be written
        *  @param Y_Coordinate the new Y coordinate to be written
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setMapIndexCoordinate(IntPtr hCamera,
                                        uint MapIndex,
                                        uint X_Coordinate,
                                        uint Y_Coordinate)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setMapIndexCoordinate(cam,
                                            MapIndex,
                                            X_Coordinate,
                                            Y_Coordinate);
        }

        /** Get map index coordinate 
        *  get X and Y coordinate accordingly to a map index that was specified
        *  as an input parameter
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MapIndex the map index from zero to one less the Mapsize 
        *  @param CoordinateX the programed X coordinate accordingly to a map index 
        *  @param CoordinateY the programed Y coordinate accordingly to a map index 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getMapIndexCoordinate(IntPtr hCamera,
                                        uint MapIndex,
                                        ref uint CoordinateX,
                                        ref uint CoordinateY)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* x = &CoordinateX)
            {
                fixed (uint* y = &CoordinateY)
                {
                    return Camera_getMapIndexCoordinate(cam,
                                                MapIndex,
                                                x,
                                                y);
                }
            }
        }

        /**  delete Pixel Coordinate accordingly to a map index that was specified
        *  as an input parameter
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Mapindex the map index from zero to one less the Mapsize
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_deletePixelCoordinateFromMap(IntPtr hCamera, uint MapIndex)
        {
            void* cam = hCamera.ToPointer();
            return Camera_deletePixelCoordinateFromMap(cam, MapIndex);
        }



        //------------------------------------------------------------------------------
        // 22 -  Special Control: IOMux configuration
        //------------------------------------------------------------------------------

        /** getMaxIOMuxIN.
        *  The maximal number of IN signals (signal sources) to the multiplexer will
        *  be 	returned that are currently available in the camera for connecting them
        *  to the multiplexer's OUT signals.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxIOMuxINSignals the currently supported number of IN signals (signal sources)
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getMaxIOMuxIN(IntPtr hCamera, ref int MaxIOMuxINSignals)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (int* p = &MaxIOMuxINSignals)
            {
                apiReturn = Camera_getMaxIOMuxIN(cam, p);
            }
            return apiReturn;
        }


        /** getMaxIOMuxOut.
        *  The maximal number of OUT signals (signal sinks) will be 	returned that
        *  are currently activated in the camera's IO multiplexer.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MaxIOMuxOUTSignals the currently supported number of OUT signals (signal sinks)
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getMaxIOMuxOUT(IntPtr hCamera, ref int MaxIOMuxOUTSignals)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (int* p = &MaxIOMuxOUTSignals)
            {
                apiReturn = Camera_getMaxIOMuxOUT(cam, p);
            }
            return apiReturn;
        }

        /** setIOAssignment.
        *  An OUT signal (signal sink) will get one or multiple IN signals (signal
        *  sources) assigned in a camera's multiplexer. In case of multiple signal
        *  sources (IN signals) those signals will be or'd for combining them to
        *  one 32-bit value that will subsequently be assigned to an OUT signal.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param IOMuxOUT the multiplexer's OUT signal (signal sink) to be configured
        *  @param SignalIOMuxIN the IN signal vector (signal sources) to be activated
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setIOAssignment(IntPtr hCamera, SVGigE_IOMux_OUT IOMuxOUT,

        SVGigE_IOMux_IN SignalIOMuxIN)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            apiReturn = Camera_setIOAssignment(cam, IOMuxOUT, SignalIOMuxIN);
            return apiReturn;
        }

        /** getIOAssignment.
        *  Current assignment of IN signals (signal sources) to an OUT signal
        *  (signal sink) will be retrieved from a camera's multiplexer.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param IOMuxOUT the multiplexer's OUT signal (signal sink) to be queried
        *  @param IOMuxIN the IN signal vector (signal sources) connected to the OUT signal
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getIOAssignment(IntPtr hCamera, SVGigE_IOMux_OUT IOMuxOUT, ref uint IOMuxIN)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &IOMuxIN)
            {
                apiReturn = Camera_getIOAssignment(cam, IOMuxOUT, p);
            }
            return apiReturn;

        }

        //------------------------------------------------------------------------------
        // 23 - Special Control: IO control
        //------------------------------------------------------------------------------

        /** setIOMuxIN.
        *  The complete vector of IN signals (source signals, max 32 bits) will be
        *  set in a camera's multiplexer in one go.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param VectorIOMuxIN the IN signal vector's new state to be assigned
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setIOMuxIN(IntPtr hCamera, uint VectorIOMuxIN)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            apiReturn = Camera_setIOMuxIN(cam, VectorIOMuxIN);
            return apiReturn;
        }

        /** getIOMuxIN.
        *  The complete vector of IN signals (source signals, max 32 bits) will be
        *  read oout from a camera's multiplexer in one go.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ProgrammedVectorIOMuxIN the IN signal vector's current state
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getIOMuxIN(IntPtr hCamera, ref uint signalVector)
        {
            SVSGigeApiReturn apiReturn = SVSGigeApiReturn.SVGigE_ERROR;
            void* cam = hCamera.ToPointer();
            fixed (uint* ptr = (&signalVector))
            {
                apiReturn = Camera_getIOMuxIN(cam, ptr);
            }
            return apiReturn;
        }

        /** setIO.
        *  A single IN signal (source signal, one out of max 32 bits) will be set.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param SignalIOMuxIN a particular signal from the IN signal vector
        *  @param SignalValue the signal value to be assigned to the IN signal
        *  @	return success or error code
        */


        public unsafe
        SVSGigeApiReturn
        Gige_Camera_Camera_setIO(IntPtr hCamera, SVGigE_IOMux_IN SignalIOMuxIN,
        SVGigE_IO_Signal SignalValue)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            apiReturn = Camera_setIO(cam, SignalIOMuxIN, SignalValue);
            return apiReturn;
        }


        /** getIO.
        *  A single IN signal (source signal, one out of max 32 bits) will be read.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param SignalIOMuxIN a particular signal from the IN signal vector
        *  @param the current value of the selected IN signal
        *  @	return success or error code
        */


        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getIO(IntPtr hCamera, SVGigE_IOMux_IN SignalIOMuxIN, ref SVGigE_IO_Signal SignalValue)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (GigeApi.SVGigE_IO_Signal* signalValuePtr = &(SignalValue))
            {
                apiReturn = Camera_getIO(cam, SignalIOMuxIN, signalValuePtr);
            }
            return apiReturn;
        }

        /** setAcqLEDOverride.
        *  Override default LED mode by an alternative behavior:
        *  - blue:    waiting for trigger
        *  - cyan:    exposure
        *  - magenta: read-out
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param isOverrideActive whether LED override will be activated or deactivated
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setAcqLEDOverride(IntPtr hCamera, bool isOverrideActive)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setAcqLEDOverride(cam, isOverrideActive);
        }

        /** getAcqLEDOverride.
        *  Check whether default LED mode was overridden by an alternative behavior:
        *  - blue:    waiting for trigger
        *  - cyan:    exposure
        *  - magenta: read-out
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param isOverrideActive a flag indicating whether LED override is currently activated
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getAcqLEDOverride(IntPtr hCamera, ref bool isOverrideActive)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (bool* overrideActive = &isOverrideActive)
            {
                apiReturn = Camera_getAcqLEDOverride(cam, overrideActive);
            }
            return apiReturn;
        }


        /** setLEDIntensity.
        *  The LED intensity will be controlled in the range 0..255 as follows:
        *  0   - dark
        *  255 - light
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LEDIntensity the new intensity (0..255=max) to be assigned to the LED
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLEDIntensity(IntPtr hCamera, int LEDIntensity)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLEDIntensity(cam, LEDIntensity);
        }

        /** getLEDIntensity.
        *  The LED intensity will be retrieved from camera with the following meaning:
        *  0   - dark
        *  255 - light
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LEDIntensity currently assigned LED intensity
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLEDIntensity(IntPtr hCamera, ref int LEDIntensity)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (int* p = &LEDIntensity)
            {
                apiReturn = Camera_getLEDIntensity(cam, p);
            }
            return apiReturn;
        }

        //------------------------------------------------------------------------------
        // 24 - Special control: Serial communication
        //------------------------------------------------------------------------------

        /** setUARTBuffer.
        *  A block of data (max 512 bytes) will be sent to the camera's UART for
        *  transmitting it over the serial line to a receiver.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Data a pointer to a block of data to be sent over the camera's UART
        *  @param DataLength the length of the data block (1..512)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setUARTBuffer(IntPtr hCamera, ref byte[] Data, int DataLength)
        {
            void* cam = hCamera.ToPointer();

            fixed (byte* p = Data)
            {
                return Camera_setUARTBuffer(cam, p, DataLength);
            }
        }

        /** getUARTBuffer.
        *  A block of data will be retrieved which has arrived in the camera's UART
        *  receiver buffer. If this function 	returns the maximal possible byte
        *  count then there might be more data available which should be retrieved
        *  by a subsequent call to this function.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Data a pointer to a data buffer
        *  @param DataLengthReceived a pointer to a value for 	returning actual data read
        *  @param DataLengthMax the maximal data length to be read (1..512)
        *  @param Timeout a time period [s] after which the function 	returns if no data was received
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getUARTBuffer(IntPtr hCamera,
                                sbyte* Data,
                                int* DataLengthReceived,
                                int DataLengthMax, float Timeout)
        {
            void* cam = hCamera.ToPointer();
            return Camera_getUARTBuffer(cam, Data, DataLengthReceived, DataLengthMax, Timeout);
        }

        /** setUARTBaud.
        *  The baud rate of the camera's UART will be set to one out of a set of
        *  pre-defined baud rates. Alternatively, any baud rate can be provided
        *  as integer which would not have to comply with any pre-defined value.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param BaudRate the baud rate to be assigned to the UART
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setUARTBaud(IntPtr hCamera, SVGigE_BaudRate BaudRate)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setUARTBaud(cam, BaudRate);
        }

        /** getUARTBaud.
        *  The currently set baud rate in the camera's UART will be 	returned.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param BaudRate the UART's currently assigned baud rate
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getUARTBaud(IntPtr hCamera, ref SVGigE_BaudRate BaudRate)
        {
            SVSGigeApiReturn apiReturn;
            void* cam = hCamera.ToPointer();
            fixed (SVGigE_BaudRate* p = &BaudRate)
            {
                apiReturn = Camera_getUARTBaud(cam, p);
            }
            return apiReturn;
        }

        //------------------------------------------------------------------------------
        // 25 - Special control: Direct register and memory access
        //------------------------------------------------------------------------------


        /**
        * Set GigE camera register.
        * A register of a GigE camera will be directly written to
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param RegisterAddress a valid address of a GigE camera register
        * @param RegisterValue a value that has to be written to selected register
        * @param GigECameraAccessKey a valid key for directly accessing a GigE camera
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setGigECameraRegister(IntPtr hCamera,
                                        uint RegisterAddress,
                                        uint RegisterValue,
                                        uint GigECameraAccessKey)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setGigECameraRegister(cam,
                                            RegisterAddress,
                                            RegisterValue,
                                            GigECameraAccessKey);
        }

        /**
        * Get GigE camera register.
        * A value from a GigE camera register will be directly read out
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @param RegisterAddress a valid address of a GigE camera register
        * @param RegisterValue the current programmed value will be 	returned
        * @param GigECameraAccessKey a valid key for directly accessing a GigE camera
        * @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getGigECameraRegister(IntPtr hCamera,
                                        uint RegisterAddress,
                                        ref uint RegisterValue,
                                        uint GigECameraAccessKey)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            fixed (uint* maskPtr = &RegisterValue)
            {
                rval = Camera_getGigECameraRegister(cam,
                                                    RegisterAddress,
                                                    maskPtr,
                                                    GigECameraAccessKey);
            }
            return rval;
        }

        /** Write GigE camera memory.
        *  A block of data will be written to the memory of a SVS GigE camera
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MemoryAddress a valid memory address in a SVS GigE camera
        *  @param DataBlock a block of data that has to be written to selected memory
        *  @param DataLength the length of the specified DataBlock
        *  @param GigECameraAccessKey a valid key for directly accessing a GigE camera
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_writeGigECameraMemory(IntPtr hCamera,
                                        uint MemoryAddress,
                                        ref sbyte[] DataBlock,
                                        uint DataLength,
                                        uint GigECameraAccessKey)
        {
            void* cam = hCamera.ToPointer();
            fixed (sbyte * p = DataBlock)
            {
                return Camera_writeGigECameraMemory(cam,
                                            MemoryAddress,
                                            p,
                                            DataLength,
                                            GigECameraAccessKey);
            }
           
        }

        /** Read GigE camera memory.
        *  A block of data will be read from the memory of a SVS GigE camera
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param MemoryAddress a valid memory address in a SVS GigE camera
        *  @param DataBlock an address where the data from selected memory will be written to
        *  @param DataLength the data length to be read from the camera's memory
        *  @param GigECameraAccessKey a valid key for directly accessing a GigE camera
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_readGigECameraMemory(IntPtr hCamera,
                                        uint MemoryAddress,
                                        ref sbyte[] DataBlock,
                                        uint DataLength,
                                        uint GigECameraAccessKey)
        {
            void* cam = hCamera.ToPointer();
            fixed (sbyte* p = DataBlock)
            {
                return Camera_readGigECameraMemory(cam,
                                                    MemoryAddress,
                                                    p,
                                                    DataLength,
                                                    GigECameraAccessKey);
            }
        }

        /** Force open connection to camera.
        *  A TCP/IP control channel will be established.
        *  The connection will be established independently whether the camera firmware
        *  matches a minimal build number or not. In case of a low build number the
        *  application may run into error conditions with SDK functions. Therefore
        *  this function can be used only by those applications that need to access
        *  cameras with an older firmware build. The application has to deal with all
        *  problem situations in this case. Usually an application needs to do direct
        *  register access in order to operate a camera with an older build number
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Timeout the time without traffic or heartbeat after which a camera drops a connection (default: 3.0 sec.)
        *                NOTE: Values between 0.0 to 0.5 sec. will be mapped to the default value (3.0 sec.)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_forceOpenConnection(IntPtr hCamera, float Timeout)
        {
            void* cam = hCamera.ToPointer();
            return Camera_forceOpenConnection(cam, Timeout);
        }


        //------------------------------------------------------------------------------
        // 26 - Special control: Persistent settings and recovery
        //------------------------------------------------------------------------------

        /**
        * Write EEPROM defaults.
        * The current settings will be made the EEPROM defaults that will be
        * restored on each camera start-up
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_writeEEPROM(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_writeEEPROM(cam);
        }

        /**
        * Read EEPROM defaults.
        * The EEPROM content will be moved to the appropriate camera registers.
        * This operation will restore the camera settings that were active when
        * the EEPROM write function was performed
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_readEEPROM(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_readEEPROM(cam);
        }

        /**
        * Restore factory defaults.
        * The camera's registers will be restored to the factory defaults and at
        * the same time those settings will be written as default to EEPROM
        *
        * @see CameraContainer_getCamera()
        * @param hCamera a camera handle received from CameraContainer_getCamera()
        * @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_restoreFactoryDefaults(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
            return Camera_restoreFactoryDefaults(cam);
        }


        /** Load settings from XML
        *  New camera settings will be loaded from a XML file.
        *  The  XML file content will be moved to the appropriate camera registers.
        *  In this operation the XML file will be used instead of the EEPROM. 
        *  
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Filename a complete path and filename where to load the settings from
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_loadSettingsFromXml(IntPtr hCamera, string Filename)
        {
            void* cam = hCamera.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(Filename);
            fixed (byte* p = bytes)
            {
                sbyte* ps = (sbyte*)p;
                return Camera_loadSettingsFromXml(cam, ps);
            }
        }

        /** Save settings to XML
        *  The current settings will be stored in a XML file 
        *  In this operation the XML file will be used instead of the EEPROM. 
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Filename a complete path and filename where to write the new settings.
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_SaveSettingsToXml(IntPtr hCamera, string Filename)
        {
            void* cam = hCamera.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(Filename);
            fixed (byte* p = bytes)
            {
                sbyte* ps = (sbyte*)p;
                SVSGigeApiReturn ret =  Camera_SaveSettingsToXml(cam, ps);
                return ret;
            }
        }

        //------------------------------------------------------------------------------
        // 27 - General functions
        //------------------------------------------------------------------------------

        /**
        *  Estimate white balance.
        *  Current image will be investigated for a suitable white balance setting
        *
        *  @param BufferRGB a buffer with current RGB image
        *  @param BufferLength the length of the RGB buffer
        *  @param Red new value for red color
        *  @param Green new value for green color
        *  @param Blue new value for blue color
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        GigE_estimateWhiteBalance(ref byte BufferRGB, int BufferLength, ref float Red, ref float Green, ref float Blue)
        {
            fixed (byte* buf = &BufferRGB)
            {
                fixed (float* pred = &Red)
                {
                    fixed (float* pgreen = &Green)
                    {
                        fixed (float* pblue = &Blue)
                        {
                            return SVGigE_estimateWhiteBalance(buf,
                                                                        BufferLength,
                                                                        pred,
                                                                        pgreen,
                                                                        pblue
                                                                        );
                        }
                    }
                }
            }


        }

        /** Estimate white balance with and without using a gray card.
        *  Current image will be investigated for a suitable white balance
        *
        *  @param BufferRGB a buffer with current RGB image
        *  @param PixelNumber of the Current image
        *  @param Red new value for red color
        *  @param Green new value for green color
        *  @param Blue new value for blue color
        *  @param Whitebalance_Art white balance estimation methode used
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        GigE_estimateWhiteBalanceExtended(ref byte BufferRGB,
                                        int PixelNumber,
                                        ref int Red,
                                        ref int Green,
                                        ref int Blue,
                                        SVGIGE_Whitebalance_SELECT Whitebalance_Art)
        {
            fixed (byte* buf = &BufferRGB)
            {
                fixed (int* pred = &Red)
                {
                    fixed (int* pgreen = &Green)
                    {
                        fixed (int* pblue = &Blue)
                        {
                            return SVGigE_estimateWhiteBalanceExtended(buf,
                                                                        PixelNumber,
                                                                        pred,
                                                                        pgreen,
                                                                        pblue,
                                                                        Whitebalance_Art);
                        }
                    }
                }
            }
        }

        /**
        *  Write image as a bitmap file to disk
        *  An image given by image data, geometry and type will be written to a
        *  specified location on disk.
        *
        *  @param Filename a path and filename for the bitmap file
        *  @param Data a pointer to image data
        *  @param SizeX the width of the image
        *  @param SizeY the height of the image
        *  @param PixelType either GVSP_PIX_MONO8 or GVSP_PIX_RGB24
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        GigE_writeImageToBitmapFile(string Filename, ref IntPtr Data, int SizeX, int SizeY, GVSP_PIXEL_TYPE PixelType)
        {
            SVSGigeApiReturn ret;
            byte[] fn_bytes = Encoding.ASCII.GetBytes(Filename);
            void* p = Data.ToPointer();

            byte* bp = (byte*)p;
            fixed (byte* fn = fn_bytes)
            {
                sbyte* ps = (sbyte*)fn;
                ret = SVGigE_writeImageToBitmapFile(ps, bp, SizeX, SizeY, PixelType);
            }


            return ret;
        }

        //------------------------------------------------------------------------------
        // 28 - Diagnostics
        //------------------------------------------------------------------------------

        /**
        *  Error get message.
        *  If the provided function 	return code respresents an error condition then
        *  a message will be mapped to the 	return code which will explain it.
        *
        *  @param 	returnCode a valid function 	return code
        *  @	return a string which will explain the 	return code
        */
        public unsafe
        sbyte*
        Gige_Error_getMessage(SVSGigeApiReturn returnCode)
        {
            return _Error_getMessage(returnCode);
        }

        /** Register for log messages.
        *  Log messages can be requested for various log levels:
        *  0 - logging off
        *  1 - CRITICAL errors that prevent from further operation
        *  2 - ERRORs that prevent from proper functioning
        *  3 - WARNINGs which usually do not affect proper work
        *  4 - INFO for listing camera communication (default)
        *  5 - DIAGNOSTICS for investigating image callbacks
        *  6 - DEBUG for receiving multiple parameters for image callbacks
        *  7 - DETAIL for receiving multiple signals for each image callback
        *
        *  Resulting log messages can be either written into a log file
        *  respectively they can be received by a callback and further
        *  processed by an application.
        *
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param LogLevel one of the above log levels
        *  @param LogFilename a filename where all log messages will be written to or NULL
        *  @param LogCallback a callback function that will receive all log messages or NULL
        *  @param MessageContext a context that will be 	returned to application with each callback or NULL
        *  @	return success or error code
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_registerForLogMessages(IntPtr hCamera, int LogLevel, string LogFilename,
                                            LogMessageCallback LogCallback,
                                            IntPtr MessageContext)
        {
            SVSGigeApiReturn rval;
            void* cam = hCamera.ToPointer();
            void* msgc = MessageContext.ToPointer();
            byte[] bytes = Encoding.ASCII.GetBytes(LogFilename);
            fixed (byte* p = bytes)
            {
                sbyte* ps = (sbyte*)p;
                rval = Camera_registerForLogMessages(cam, LogLevel, ps, LogCallback, msgc);
            }
            return rval;
        }

        //------------------------------------------------------------------------------
        // 29 - Special control: Lens control
        //------------------------------------------------------------------------------

        /** Is Lens available.
        *  The camera will be queried whether a MFT Lens is active or not.
        *
        *  @see Camera_isCameraFeature()
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param isAvailable a flag indicating whether a MFT Lens is active or not
        *  @	return success or error code	
        */

        public unsafe
        SVSGigeApiReturn
        Gige_Camera_isLensAvailable(IntPtr hCamera, ref bool isAvailable)
        {
            void* cam = hCamera.ToPointer();
            fixed (bool* p = &isAvailable)
            {
                return Camera_isLensAvailable(cam, p);
            }
        }

        /** Get lens name.
        *  The lens name that is obtained from the lens firmware will be
        *  	returned
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @	return a string indicating the requested information. 
        */
        public unsafe
        string
        Gige_Camera_getLensName(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();

            return new string(Camera_getLensName(cam));
        }

        /** setLensFocalLength.
        *
        *  A provided focal Length  will be transferred to lens.
        *  @see Camera_isCameraFeature()  
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FocalLength the focal Length to be assigned to the Lens (granularity: 1/10 mm, ex. 350 corresponds to 35 mm). 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLensFocalLength(IntPtr hCamera, uint FocalLength)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLensFocalLength(cam, FocalLength);
        }

        /** getLensFocalLength.
        *  The currently set focal Length  of the lens will be 	returned.
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FocalLength the currently assigned focal Length (granularity: 1/10 mm, ex. 350 corresponds to 35 mm).
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensFocalLength(IntPtr hCamera, ref uint FocalLength)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &FocalLength)
            {
                return Camera_getLensFocalLength(cam, p);
            }
        }

        /** getLensFocalLengthMin.

        *  Get the minimal focal length that can be used.
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FocalLengthMin the minimal focal length setting(granularity: 1/10 mm, ex. 140 corresponds to 14 mm). 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensFocalLengthMin(IntPtr hCamera, ref uint FocalLengthMin)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &FocalLengthMin)
            {
                return Camera_getLensFocalLengthMin(cam, p);
            }
        }

        /** getLensFocalLengthMax.

        *  Get the maximal focal length that can be used.
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FocalLengthMax the maximal focal length setting (granularity: 1/10 mm, ex. 420 corresponds to 42 mm). 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensFocalLengthMax(IntPtr hCamera, ref uint FocalLengthMax)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &FocalLengthMax)
            {
                return Camera_getLensFocalLengthMax(cam, p);
            }
        }

        /** set focus unit
       *  A selected focus unit will be transferred to lens.
       *
       *  @see Camera_isCameraFeature() 
       *  @see CameraContainer_getCamera()  
       *  @param hCamera a camera handle received from CameraContainer_getCamera()
       *  @param Selected_unit the focus unit( mm or 1/10 mm) to be assigned to Lens
       *  @return success or error code
       */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLensFocusUnit(IntPtr hCamera, FOCUS_UNIT Selected_unit)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLensFocusUnit(cam, Selected_unit);
        }

        /** get focus unit
         *  The currently focus unit will be returned.
         *  @see Camera_isCameraFeature() 
         *  @see CameraContainer_getCamera()  
         *  @param hCamera a camera handle received from CameraContainer_getCamera()
         *  @param Selected_unit the currently used focus unit.
         *  @return success or error code
         */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensFocusUnit(IntPtr hCamera, ref FOCUS_UNIT Selected_unit)
        {
            void* cam = hCamera.ToPointer();
            fixed (FOCUS_UNIT* p = &Selected_unit)
            {
                return Camera_getLensFocusUnit(cam, p);
            }
        }
        /** setLensFocus.
        *  A provided focus will be transferred to lens.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Focus the focus (in mm) to be assigned to Lens
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLensFocus(IntPtr hCamera, uint Focus)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLensFocus(cam, Focus);
        }


        /** getLensFocus.
        *  The currently set focus of the lens will be 	returned.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Focus the currently assigned focus in mm .
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensFocus(IntPtr hCamera, ref uint Focus)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &Focus)
            {
                return Camera_getLensFocus(cam, p);
            }
        }


        /** getLensFocusMin.
        *  Get the minimal focus that can be used.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FocusMin the minimal focus setting in mm. 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensFocusMin(IntPtr hCamera, ref uint FocusMin)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &FocusMin)
            {
                return Camera_getLensFocusMin(cam, p);
            }
        }


        /** getLensFocusMax.
        *  Get the maximal focus that can be used.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param FocusMax the maximal focus setting in mm. 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensFocusMax(IntPtr hCamera, ref uint FocusMax)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &FocusMax)
            {
                return Camera_getLensFocusMax(cam, p);
            }
        }


        /** setLensAperture.
        *  A provided aperture will be transferred to lens.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera() 
        *  @param Aperture the aperture to be assigned to the Lens (granularity: 1/10 , ex. 90 corresponds to 9)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_setLensAperture(IntPtr hCamera, uint Aperture)
        {
            void* cam = hCamera.ToPointer();
            return Camera_setLensAperture(cam, Aperture);
        }


        /** getLensAperture.
        *  The currently set aperture of the lens will be 	returned.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param Aperture the currently assigned aperture (granularity: 1/10 , ex. 90 corresponds to 9)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensAperture(IntPtr hCamera, ref uint Aperture)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &Aperture)
            {
                return Camera_getLensAperture(cam, p);
            }
        }

        /** getLensApertureMin.
        *  Get the minimal aperture that can be used.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ApertureMin the minimal aperture setting (granularity: 1/10 , ex. 35 corresponds to 3.5) 
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensApertureMin(IntPtr hCamera, ref uint ApertureMin)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &ApertureMin)
            {
                return Camera_getLensApertureMin(cam, p);
            }
        }

        /** getLensApertureMax.
        *  Get the maximal aperture that can be used.
        *
        *  @see Camera_isCameraFeature() 
        *  @see CameraContainer_getCamera()  
        *  @param hCamera a camera handle received from CameraContainer_getCamera()
        *  @param ApertureMax the maximal aperture setting (granularity: 1/10 , ex. 90 corresponds to 9)
        *  @	return success or error code
        */
        public unsafe
        SVSGigeApiReturn
        Gige_Camera_getLensApertureMax(IntPtr hCamera, ref uint ApertureMax)
        {
            void* cam = hCamera.ToPointer();
            fixed (uint* p = &ApertureMax)
            {
                return Camera_getLensApertureMax(cam, p);
            }
        }

		/** LensReset.
		*  Reset the lens
		*
		*  @see Camera_isCameraFeature() 
		*  @see CameraContainer_getCamera()  
		*  @param hCamera a camera handle received from CameraContainer_getCamera()
		*  @return success or error code
		*/
        public  unsafe
            SVSGigeApiReturn
            Gige_Camera_LensReset(IntPtr hCamera)
        {
              void* cam = hCamera.ToPointer();
              return Camera_LensReset(cam);
           
        }
	
		/** LensReset.
		*  Update of current lens parameters
		*
		*  @see Camera_isCameraFeature() 
		*  @see CameraContainer_getCamera()  
		*  @param hCamera a camera handle received from CameraContainer_getCamera()
		*  @return success or error code
		*/
        public  unsafe
            SVSGigeApiReturn
            Gige_Camera_LensUpdate(IntPtr hCamera)
        {
            void* cam = hCamera.ToPointer();
			return Camera_LensUpdate(cam);
            
        }
	
		/** setLensState.
		*   The currently lens state will be will be transferred to lens.
		*   Only active and sleep state can be transferred to the lens.
		*
		*  @see Camera_isCameraFeature() 
		*  @see CameraContainer_getCamera()  
		*  @param hCamera a camera handle received from CameraContainer_getCamera()
		*  @param LensState the current lens state 
		*  @return success or error code
		*/
        public  unsafe
            SVSGigeApiReturn
            Gige_Camera_setLensState(IntPtr hCamera, LENS_STATE LensState)
        {
		    void* cam = hCamera.ToPointer();
            return Camera_setLensState(cam, LensState);
        }
		
	  /** getLensState.
	   *  The currently  lens state will be returned.
	   *
	   *  @see Camera_isCameraFeature() 
	   *  @see CameraContainer_getCamera()  
	   *  @param hCamera a camera handle received from CameraContainer_getCamera()
	   *  @param LensState the current lens state
	   *  @return success or error code
	   */
        public  unsafe
            SVSGigeApiReturn
            Gige_Camera_getLensState(IntPtr hCamera, ref LENS_STATE LensState)
        {
             void* cam = hCamera.ToPointer();


             fixed (LENS_STATE* p = &LensState)
             {
                 return Camera_getLensState(cam, p);
             }
        }

            
		
		
		


        // -------------------------------------------------------------------------
        // 99 - Deprecated functions
        // ---------------------------------------------------------------------

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setLUTMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setLUTMode64(void* hCamera, LUT_MODE LUTMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setLUTMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_setLUTMode32(void* hCamera, LUT_MODE LUTMode);
        public static unsafe
            SVSGigeApiReturn
            Camera_setLUTMode(void* hCamera, LUT_MODE LUTMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setLUTMode64(hCamera, LUTMode) : Camera_setLUTMode32(hCamera, LUTMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getLUTMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getLUTMode64(void* hCamera, LUT_MODE* LUTMode);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getLUTMode", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getLUTMode32(void* hCamera, LUT_MODE* LUTMode);
        public static unsafe
            SVSGigeApiReturn
            Camera_getLUTMode(void* hCamera, LUT_MODE* LUTMode)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getLUTMode64(hCamera, LUTMode) : Camera_getLUTMode32(hCamera, LUTMode);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_createLUTwhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_createLUTwhiteBalance64(void* hCamera, float Red, float Green, float Blue);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_createLUTwhiteBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_createLUTwhiteBalance32(void* hCamera, float Red, float Green, float Blue);
        public static unsafe
            SVSGigeApiReturn Camera_createLUTwhiteBalance(void* hCamera, float Red, float Green, float Blue)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_createLUTwhiteBalance64(hCamera, Red, Green, Blue) : Camera_createLUTwhiteBalance32(hCamera, Red, Green, Blue);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_startAcquisitionCycle", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_startAcquisitionCycle64(void* hCamera);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_startAcquisitionCycle", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn Camera_startAcquisitionCycle32(void* hCamera);
        public static unsafe
        SVSGigeApiReturn Camera_startAcquisitionCycle(void* hCamera)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_startAcquisitionCycle64(hCamera) : Camera_startAcquisitionCycle32(hCamera);
        }




        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_stampTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_stampTimestamp64(void* hCamera, int TimestampIndex);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_stampTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_stampTimestamp32(void* hCamera, int TimestampIndex);
        public static unsafe
            SVSGigeApiReturn
            Camera_stampTimestamp(void* hCamera, int TimestampIndex)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_stampTimestamp64(hCamera, TimestampIndex) : Camera_stampTimestamp32(hCamera, TimestampIndex);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getTimestamp64(void* hCamera, int TimestampIndex, double* Timestamp);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTimestamp", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
            SVSGigeApiReturn
            Camera_getTimestamp32(void* hCamera, int TimestampIndex, double* Timestamp);
        public static unsafe
            SVSGigeApiReturn
            Camera_getTimestamp(void* hCamera, int TimestampIndex, double* Timestamp)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTimestamp64(hCamera, TimestampIndex, Timestamp) : Camera_getTimestamp32(hCamera, TimestampIndex, Timestamp);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_forceOpenConnection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_forceOpenConnection64(void* hCamera, float Timeout);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_forceOpenConnection", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_forceOpenConnection32(void* hCamera, float Timeout);
        public static unsafe
        SVSGigeApiReturn
        Camera_forceOpenConnection(void* hCamera, float Timeout)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_forceOpenConnection64(hCamera, Timeout) : Camera_forceOpenConnection32(hCamera, Timeout);
        }



        [DllImport(SVGigE_DLL64, EntryPoint = "Image_getImageGray", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImageGray64(int hImage, byte* Buffer8bit, int BufferLength);

        [DllImport(SVGigE_DLL, EntryPoint = "Image_getImageGray", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Image_getImageGray32(int hImage, byte* Buffer8bit, int BufferLength);
        public static unsafe
        SVSGigeApiReturn
        Image_getImageGray(int hImage, byte* Buffer8bit, int BufferLength)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Image_getImageGray64(hImage, Buffer8bit, BufferLength) : Image_getImageGray32(hImage, Buffer8bit, BufferLength);
        }


        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_setTapBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapBalance64(void* hCamera, float TapBalance);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_setTapBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_setTapBalance32(void* hCamera, float TapBalance);
        public static unsafe
        SVSGigeApiReturn
        Camera_setTapBalance(void* hCamera, float TapBalance)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_setTapBalance64(hCamera, TapBalance) : Camera_setTapBalance32(hCamera, TapBalance);
        }

        [DllImport(SVGigE_DLL64, EntryPoint = "Camera_getTapBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapBalance64(void* hCamera, float* TapBalance);

        [DllImport(SVGigE_DLL, EntryPoint = "Camera_getTapBalance", CallingConvention = CallingConvention.Cdecl)] 
        private static extern unsafe
        SVSGigeApiReturn
        Camera_getTapBalance32(void* hCamera, float* TapBalance);
        public static unsafe
        SVSGigeApiReturn
        Camera_getTapBalance(void* hCamera, float* TapBalance)
        {
            return IntPtr.Size == 8 /* 64bit */ ? Camera_getTapBalance64(hCamera, TapBalance) : Camera_getTapBalance32(hCamera, TapBalance);
        }


    }// end class
}// end namespace
