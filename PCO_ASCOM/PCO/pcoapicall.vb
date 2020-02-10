
Option Strict Off
Option Explicit On

Imports System.Runtime.InteropServices
Module sc2_cam
	

	'Other DDL's
  'Public Declare Function VarPtrArray Lib "msvbvm60.dll" Alias "VarPtr" (ByRef Ptr() As Long) As Integer
	
	
	
  <StructLayoutAttribute(LayoutKind.Sequential)> Public Structure PCO_Description
    Dim wSize As Short 'Sizeof this struct
    Dim wSensorTypeDESC As Short 'Sensor type
    Dim wSensorSubTypeDESC As Short 'Sensor subtype
    Dim wMaxHorzResStdDESC As Short 'Maxmimum horz. resolution in std.mode
    Dim wMaxVertResStdDESC As Short 'Maxmimum vert. resolution in std.mode
    Dim wMaxHorzResExtDESC As Short 'Maxmimum horz. resolution in ext.mode
    Dim wMaxVertResExtDESC As Short 'Maxmimum vert. resolution in ext.mode
    Dim wDynResDESC As Short 'Dynamic resolution of ADC in bit
    Dim wMaxBinHorzDESC As Short 'Maxmimum horz. binning
    Dim wBinHorzSteppingDESC As Short 'Horz. bin. stepping (0:bin, 1:lin)
    Dim wMaxBinVertDESC As Short 'Maxmimum vert. binning
    Dim wBinVertSteppingDESC As Short 'Vert. bin. stepping (0:bin, 1:lin)
    Dim wRoiHorStepsDESC As Short 'Minimum granularity of ROI in pixels
    Dim wRoiVertStepsDESC As Short 'Minimum granularity of ROI in pixels
    Dim wNumADCsDESC As Short 'Number of ADCs in system
    Dim ZZwAlignDummy1 As Short '
    Dim dwPixelRateDESC1 As Integer
    Dim dwPixelRateDESC2 As Integer
    Dim dwPixelRateDESC3 As Integer
    Dim dwPixelRateDESC4 As Integer
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=20)> Dim ZZdwDummypr() As Integer '
    Dim wConvFactDESC1 As Short
    Dim wConvFactDESC2 As Short
    Dim wConvFactDESC3 As Short
    Dim wConvFactDESC4 As Short ' Possible conversion factor in e/cnt
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=10)> Dim ZZdwDummycv() As Integer '
    Dim wIRDESC As Short 'IR enhancment possibility
    Dim ZZwAlignDummy2 As Short '
    Dim dwMinDelayDESC As Integer 'Minimum delay time in ns
    Dim dwMaxDelayDESC As Integer 'Maximum delay time in ms
    Dim dwMinDelayStepDESC As Integer 'Minimum stepping of delay time in ns
    Dim dwMinExposureDESC As Integer 'Minimum exposure time in ns
    Dim dwMaxExposureDESC As Integer 'Maximum exposure time in ms
    Dim dwMinExposureStepDESC As Integer 'Minimum stepping of exposure time in ns
    Dim dwMinDelayIRDESC As Integer 'Minimum delay time in ns
    Dim dwMaxDelayIRDESC As Integer 'Maximum delay time in ms
    Dim dwMinExposureIRDESC As Integer 'Minimum exposure time in ns
    Dim dwMaxExposureIRDESC As Integer 'Maximum exposure time in ms
    Dim wTimeTableDESC As Short 'Timetable for exp/del possibility
    Dim wDoubleImageDESC As Short 'Double image mode possibility
    Dim sMinCoolSetDESC As Short 'Minimum value for cooling
    Dim sMaxCoolSetDESC As Short 'Maximum value for cooling
    Dim sDefaultCoolSetDESC As Short 'Default value for cooling
    Dim wPowerDownModeDESC As Short 'Power down mode possibility
    Dim wOffsetRegulationDESC As Short 'Offset regulation possibility
    Dim wColorPatternDESC As Short 'Color pattern of color chip
    Dim wPatternTypeDESC As Short 'Pattern type of color chip
    Dim wDummy1 As Short 'DSNU correction mode
    Dim wDummy2 As Short '
    Dim wNumCoolingSetpoints As Short
    Dim dwGeneralDESC1 As Integer
    Dim dwGeneralDESC2 As Integer
    Dim dwExtSyncFrequency1 As Integer
    Dim dwExtSyncFrequency2 As Integer
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=4)> Dim dwReservedDESC() As Integer '32bit dummy
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=40)> Dim ZZdwDummy() As Integer '
  End Structure
	
	
  <StructLayoutAttribute(LayoutKind.Sequential)> Public Structure PCO_Description2
    Dim wSize As Short ' Sizeof this struct
    Dim dwMinPeriodicalTimeDESC2 As Integer ' Minimum periodical time tp in (nsec)
    Dim dwMaxPeriodicalTimeDESC2 As Integer ' Maximum periodical time tp in (msec)        (12)
    Dim dwMinPeriodicalConditionDESC2 As Integer ' System imanent condition in (nsec)
    ' tp - (td + te) must be equal or longer than
    ' dwMinPeriodicalCondition
    Dim dwMaxNumberOfExposuresDESC2 As Integer ' Maximum number of exporures possible        (20)
    Dim lMinMonitorSignalOffsetDESC2 As Integer ' Minimum monitor signal offset tm in (nsec)
    ' if(td + tstd) > dwMinMon.)
    '   tm must not be longer than dwMinMon
    ' else
    '   tm must not be longer than td + tstd
    Dim dwMaxMonitorSignalOffsetDESC2 As Integer ' Maximum -''- in (nsec)
    Dim dwMinPeriodicalStepDESC2 As Integer ' Minimum step for periodical time in (nsec)  (32)
    Dim dwStartTimeDelayDESC2 As Integer ' Minimum monitor signal offset tstd in (nsec)
    ' see condition at dwMinMonitorSignalOffset
    Dim dwMinMonitorStepDESC2 As Integer ' Minimum step for monitor time in (nsec)     (40)
    Dim dwMinDelayModDESC2 As Integer ' Minimum delay time for modulate mode in (nsec)
    Dim dwMaxDelayModDESC2 As Integer ' Maximum delay time for modulate mode in (msec)
    Dim dwMinDelayStepModDESC2 As Integer ' Minimum delay time step for modulate mode in (nsec)(52)
    Dim dwMinExposureModDESC2 As Integer ' Minimum exposure time for modulate mode in (nsec)
    Dim dwMaxExposureModDESC2 As Integer ' Maximum exposure time for modulate mode in (msec)(60)
    Dim dwMinExposureStepModDESC2 As Integer ' Minimum exposure time step for modulate mode in (nsec)
    Dim dwModulateCapsDESC2 As Integer ' Modulate capabilities descriptor
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=16)> Dim dwReservedDESC() As Integer ' 32bit dummy
    Dim ZZwDummy As Short
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=40)> Dim ZZdwDummy() As Integer '
  End Structure
	
  ' Not all arrays are expanded to fit .net code
  ' See upper arrays how to do
  <StructLayoutAttribute(LayoutKind.Sequential)> Public Structure PCO_Sensor
    Dim wSize As Short ' Sizeof this struct
    Dim ZZwAlignDummy1 As Short
    'UPGRADE_WARNING: Arrays in structure strDescription may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
    Dim strDescription As PCO_Description ' previous described structure
    'UPGRADE_WARNING: Arrays in structure strDescription2 may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
    Dim strDescription2 As PCO_Description2 ' second descriptor
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=256)> Dim ZZdwDummy2() As Integer '
    Dim wSensorformat As Short ' Sensor format std/ext
    Dim wRoiX0 As Short ' Roi upper left x
    Dim wRoiY0 As Short ' Roi upper left y
    Dim wRoiX1 As Short ' Roi lower right x
    Dim wRoiY1 As Short ' Roi lower right y
    Dim wBinHorz As Short ' Horizontal binning
    Dim wBinVert As Short ' Vertical binning
    Dim ZZwAlignDummy4 As Short
    Dim dwPixelRate As Integer ' 32bit unsigend, Pixelrate in Hz:
    ' depends on descriptor values
    Dim wConvFact As Short ' Conversion factor:
    ' depends on descriptor values
    Dim wDoubleImage As Short ' Double image mode
    Dim wADCOperation As Short ' Number of ADCs to use
    Dim wIR As Short ' IR sensitivity mode
    Dim sCoolSet As Short ' Cooling setpoint
    Dim wOffsetRegulation As Short ' Offset regulation mode
    Dim wNoiseFilterMode As Short ' Noise filter mode
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=39)> Dim ZZwDummy() As Short
  End Structure
	
	
  <StructLayoutAttribute(LayoutKind.Sequential)> Public Structure PCO_storage
    Dim wSize As Short '                   // Sizeof this struct
    Dim ZZwAlignDummy1 As Short ';
    Dim dwRamSize As Integer '               // Size of camera ram in pages
    Dim wPageSize As Short ';               // Size of one page in pixel
    Dim ZZwAlignDummy4 As Short ';
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=3)> Dim dwRamSegSize() As Integer '// Size of ram segment 1-4 in pages
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=20)> Dim ZZdwDummyrs() As Integer '
    Dim wActSeg As Short '                 // no. (0 .. 3) of active segment
    <MarshalAs(UnmanagedType.ByValArray, SizeConst:=39)> Dim ZZwDummy() As Short '
  End Structure
	
	
	'//-----------------------------------------------------------------//
	'// Name        | SC2_CamExport.h             | Type: ( ) source    //
	'//-------------------------------------------|       (*) header    //
	'// Project     | PCO                         |       ( ) others    //
	'//-----------------------------------------------------------------//
	'// Platform    | PC                                                //
	'//-----------------------------------------------------------------//
	'// Environment | Visual 'C++'                                      //
	'//-----------------------------------------------------------------//
	'// Purpose     | PCO - SC2 Camera DLL Functions                    //
	'//-----------------------------------------------------------------//
	'// Author      | FRE, PCO AG                                       //
	'//-----------------------------------------------------------------//
	'// Revision    |  rev. 1.06 rel. 1.06                              //
	'//-----------------------------------------------------------------//
	'// Notes       | Some functions are illustrated with an example    //
	'//             | source code. If the function you need doesn't     //
	'//             | have some source code sample, please take a look  //
	'//             | on other functions supplied with source code. You //
	'//             | will find some usefull code there and you will be //
	'//             | able to adapt the code to the function you need.  //
	'//             |                                                   //
	'//             | To get informations about the ranges of the       //
	'//             | data values please take a look at the SDK docu.   //
	'//-----------------------------------------------------------------//
	'// (c) 2002 PCO AG * Donaupark 11 *                                //
	'// D-93309      Kelheim / Germany * Phone: +49 (0)9441 / 2005-0 *  //
	'// Fax: +49 (0)9441 / 2005-20 * Email: info@pco.de                 //
	'//-----------------------------------------------------------------//
	'
	'
	'//-----------------------------------------------------------------//
	'// Revision History:                                               //
	'//-----------------------------------------------------------------//
	'// Rev.:     | Date:      | Changed:                               //
	'// --------- | ---------- | ---------------------------------------//
	'//  0.10     | 03.07.2003 |  new file, FRE                         //
	'//-----------------------------------------------------------------//
	'//  0.13     | 08.12.2003 |  Added GetSizes, FRE                   //
	'//-----------------------------------------------------------------//
	'//  0.14     | 14.01.2004 |  Added GetCOCRuntime,                  //
	'//           |            |  Added GetBufferStatus, FRE            //
	'//-----------------------------------------------------------------//
	'//  0.15     | 06.02.2004 |  Added SetImagestruct                  //
	'//           |            |  Added SetStoragestruct                //
	'//           | 18.02.2004 |  Added Self calibration and correction //
	'//-----------------------------------------------------------------//
	'//  0.16     | 23.03.2004 |  Removed single entries for dwDelay    //
	'//           |            |  and dwExposure, now they are part of  //
	'//           |            |  the delay/exposure table, FRE         //
	'//-----------------------------------------------------------------//
	'//  1.0      | 04.05.2004 |  Released to market                    //
	'//           |            |                                        //
	'//-----------------------------------------------------------------//
	'//  1.01     | 04.05.2004 |  Added FPSExposureMode, FRE            //
	'//           |            |  Set-Get-1394Transferparameter         //
	'//-----------------------------------------------------------------//
	'//  1.02     | 29.07.2004 |  Changed to explicit linking           //
	'//           |            |  Added CamLink interface capability    //
	'//           | 23.07.2004 |  Added OpenCameraEx                    //
	'//           | 06.10.2004 |  Added SetTimeouts                     //
	'//           | 10.11.2004 |  Added GetBuffer                       //
	'//-----------------------------------------------------------------//
	'//  1.03     | 22.02.2005 |  Added AddBufferEx and GetImageEx, FRE //
	'//           |            |  Allocate sizes adapted due to possible//
	'//           |            |  crash in case of changing the transfer//
	'//           |            |  parameters.                           //
	'//-----------------------------------------------------------------//
	'//  1.04     | 19.04.2005 |  Added PCO_Get(Set)NoiseFilterMode, FRE//
	'//           |            |  Added try catch blocks where pointer  //
	'//           |            |  are passed in. Changed the init. where//
	'//           |            |  an error occured while retrieving data//
	'//           |            |  Bugfix: GetImage(Ex) is able to trans.//
	'//           |            |  more than one image, now...           //
	'//           | 20.07.2005 |  Added record stop event stuff, FRE    //
	'//-----------------------------------------------------------------//
	'//  1.05     | 27.02.2006 |  Added PCO_GetCameraName, FRE          //
	'//           |            |  Added PCO_xxxHotPixelxxx, FRE         //
	'//-----------------------------------------------------------------//
	'//  1.06     | 02.06.2006 |  Added PCO_GetCameraDescriptionEx, FRE //
	'//           |            |  Added PCO_xxxModulationMode, FRE      //
	'//           |            |  Added PCO_GetInfoString, FRE          //
	'//-----------------------------------------------------------------//
	'
	'#ifdef SC2_CAM_EXPORTS
	'#define SC2_SDK_FUNC __declspec(dllexport)
	'#Else
	'#define SC2_SDK_FUNC __declspec(dllimport)
	'#End If
	'
	'#ifdef __cplusplus
	'extern "C" {            //  Assume C declarations for C++
	'#endif  //C++
	'
	'// VERY IMPORTANT INFORMATION:
	'/*******************************************************************/
	'/* PLEASE: Do not forget to fill in all wSize Parameters while     */
	'/* using the structure funtions. Some structures even have embedded*/
	'/* wSize parameters.                                               */
	'/*******************************************************************/
	'/* All indexes, but segment and image parameters are zero based.   */
	'/* If you access the camera with segment and image parameters the  */
	'/* base is 1! E.g.
	'  PCO_Image strImage;
	'  int err;
	'  strImage.wSize = sizeof(PCO_Image);
	'  err = PCO_GetImageStruct(ph, &strImage);
	'
	'  // Info about segment 1:
	'  dwValidImageCnt = strImage.strSegment[0].dwValidImageCnt;
	'
	'  // Info about segment 2:
	'  dwValidImageCnt = strImage.strSegment[1].dwValidImageCnt;
	'
	'  // Info about segment 3:
	'  dwValidImageCnt = strImage.strSegment[2].dwValidImageCnt;
	'
	'  // Info about segment 4:
	'  dwValidImageCnt = strImage.strSegment[3].dwValidImageCnt;
	'
	'but:----Access-To-Segment-1-is----
	'                                ||
	'                                \/
	'  err = PCO_GetSegmentStruct(ph, 1, &strImage.strSegment[0].wSize);
	'
	'and:
	'  DWORD dw1stImage = 1; // 1 based !!!!! This accesses the first image.
	'  DWORD dwLastImage = 2;
	'
	'again:-Access-To-Segment-1--
	'                          ||
	'                          \/
	'  err = PCO_GetImageEx(ph, 1, dw1stImage, dwLastImage, sBufNr,
	'                       wXRes, wYRes, wBitPerPixel);
	'
	'/*******************************************************************/
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// General commands ////////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'SC2_SDK_FUNC int WINAPI PCO_GetGeneral(HANDLE ph, PCO_General *strGeneral);
	' Public Declare Function PCO_GetGeneral Lib "sc2_cam.dll" (ByVal hdriver As Long, strGeneral As String) As Long
	
	'// Gets all data of the general settings in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_General *strGeneral -> Pointer to a PCO_General structure.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  PCO_General strGeneral;
	'  strGeneral.wSize = sizeof(PCO_General);
	'  int err = PCO_GetGeneral(hCamera, &strGeneral);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraType(HANDLE ph, PCO_CameraType *strCamType);
	'// Gets the camera type in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_CameraType *strCamType -> Pointer to a PCO_CameraType structure.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  PCO_CameraType strCamType;
	'  int err = PCO_GetCameraType(hCamera, &strCamType);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraHealthStatus(HANDLE ph, DWORD* dwWarn, DWORD* dwErr, DWORD* dwStatus);
  Public Declare Function PCO_GetCameraHealthStatus Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef lWarning As Integer, ByRef lError As Integer, ByRef lStatus As Integer) As Integer
	
	'// Gets the last warnings, errors and status of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD *dwWarn -> Pointer to a DWORD variable, to receive the warning value.
	'//     DWORD *dwErr -> Pointer to a DWORD variable, to receive the error value.
	'//     DWORD *dwStatus -> Pointer to a DWORD variable, to receive the status value.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  DWORD dwWarn, DWORD dwErr, DWORD dwStatus
	'  int err = PCO_GetCameraHealthStatus(hCamera, &dwWarn, &dwErr, &dwStatus);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_ResetSettingsToDefault(HANDLE ph);
  Public Declare Function PCO_ResetSettingsToDefault Lib "sc2_cam.dll" (ByVal hdriver As IntPtr) As Integer
	
	'// Resets the camera to a default setting.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'// Out: int -> Error message.
	'/* Example: see PCO_CloseCamera */
	'
	'SC2_SDK_FUNC int WINAPI PCO_InitiateSelftestProcedure(HANDLE ph);
  Public Declare Function PCO_InitiateSelftestProcedure Lib "sc2_cam.dll" (ByVal hdriver As IntPtr) As Integer
	
	'// Starts a self test procedure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'// Out: int -> Error message.
	'/* Example: see PCO_CloseCamera */
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetTemperature(HANDLE ph, SHORT* sCCDTemp, SHORT* sCamTemp, SHORT* sPowTemp);
  Public Declare Function PCO_GetTemperature Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef iCCDTemp As Short, ByRef iCamTemp As Short, ByRef iPowTemp As Short) As Integer
	'// Gets the actual temperatures of the camera and the power device.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     SHORT *sCCDTemp -> Pointer to a SHORT variable, to receive the CCD temp. value.
	'//     SHORT *sCamTemp -> Pointer to a SHORT variable, to receive the camera temp. value.
	'//     SHORT *sPowTemp -> Pointer to a SHORT variable, to receive the power device temp. value.
	'// Out: int -> Error message.
	'/* Example: see PCO_GetCameraHealthStatus.*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraName(HANDLE ph, char* szCameraName, WORD wSZCameraNameLen);
  Public Declare Function PCO_GetCameraName Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef lPtrCamName As Integer, ByRef lCamLen As Short) As Integer
	'// Gets the name of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     char *szCameraName -> Pointer to a string, to receive the camera name.
	'//     WORD wSZCameraNameLen -> WORD variable which holds the maximum length of the string.
	'// Out: int -> Error message.
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: General commands ///////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// Sensor commands /////////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'SC2_SDK_FUNC int WINAPI PCO_GetSensorStruct(HANDLE ph, PCO_Sensor *strSensor);
	'UPGRADE_WARNING: Structure PCO_Sensor may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
  Public Declare Function PCO_GetSensorStruct Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef strSensor As PCO_Sensor) As Integer
	'// Gets all of the sensor data in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Sensor *strSensor -> Pointer to a PCO_Sensor structure.
	'// Out: int -> Error message.
	'/* Example: see PCO_SetSensorStruct */
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetSensorStruct(HANDLE ph, PCO_Sensor *strSensor);
	'// Sets the sensor data structure. Individual values can be set by following functions.
	'// This function can be used, if you have to set more than one parameter (see Example).
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Sensor *strSensor -> Pointer to a PCO_Sensor structure.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  PCO_Sensor strSensor;
	'  strSensor.wSize = sizeof(PCO_Sensor);
	'  int err = PCO_GetSensorStruct(hCamera, &strSensor);
	'  ...
	'  strSensor.wRoiX0 = 20;
	'  strSensor.wRoiX1 = 820;
	'  strSensor.wRoiY0 = 200;
	'  strSensor.wRoiY1 = 400;
	'  strSensor.wBinHorz = 2;                // Change horizontal binning
	'  strSensor.wBinVert = 2;                // Change vertical binning
	'  ...
	'  int err = PCO_SetSensorStruct(hCamera, &strSensor);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraDescription(HANDLE ph, PCO_Description *strDescription);
	'UPGRADE_WARNING: Structure PCO_Description may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
  Public Declare Function PCO_GetCameraDescription Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef strDescription As PCO_Description) As Integer
	
	'// Gets the camera description data structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Description *strDescription -> Pointer to a PCO_Description structure.
	'// Out: int -> Error message.
	'/* Example: see PCO_GetSensorStruct in PCO_SetSensorStruct */
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraDescriptionEx(HANDLE ph, PCO_DescriptionEx *strDescription, WORD wType);
	'// Gets the camera description data structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_DescriptionEx *strDescription -> Pointer to a PCO_Description structure.
	'//     WORD wType -> Type of descriptor: 0 -> standard (must have); 1 -> second (check standard)
	'// Out: int -> Error message.
	'/* Example: see PCO_GetSensorStruct in PCO_SetSensorStruct */
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetSensorFormat(HANDLE ph, WORD* wSensor);
  Public Declare Function PCO_GetSensorFormat Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef SensorFormat As Short) As Integer
	
	'// Gets the sensor format.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wSensor -> Pointer to a WORD variable to receive the sensor format.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  WORD wSensorFormat;
	'  int err = PCO_GetSensorFormat(hCamera, &wSensorFormat);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetSensorFormat(HANDLE ph, WORD wSensor);
  Public Declare Function PCO_SetSensorFormat Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal SensorFormat As Integer) As Integer
	'// Sets the sensor format.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wSensor -> WORD variable which holds the sensor format.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  WORD wSensorFormat;
	'  wSensorFormat = 1;                   // 0: normal, 1: extended
	'  int err = PCO_SetSensorFormat(hCamera, wSensorFormat);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetSizes(HANDLE ph,
	'                            WORD *wXResAct, // Actual X Resolution
	'                            WORD *wYResAct, // Actual Y Resolution
	'                            WORD *wXResMax, // Maximum X Resolution
	'                            WORD *wYResMax); // Maximum Y Resolution
  Public Declare Function PCO_GetSizes Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wXResAct As Short, ByRef wYResAct As Short, ByRef wXResMax As Short, ByRef wYResMax As Short) As Integer
	'// Gets the actual and maximum sizes of the camera. The maximum y value includes the
	'// size of a double shutter image.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wXResAct -> Pointer to a WORD variable to receive the actual X resolution.
	'//     WORD *wYResAct -> Pointer to a WORD variable to receive the actual Y resolution.
	'//     WORD *wXResMax -> Pointer to a WORD variable to receive the maximal X resolution.
	'//     WORD *wXResMax -> Pointer to a WORD variable to receive the maximal Y resolution.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  WORD wXResAct;                       // Actual X Resolution
	'  WORD wYResAct;                       // Actual Y Resolution
	'  WORD wXResMax;                       // Maximum X Resolution
	'  WORD wYResMax;                       // Maximum Y Resolution
	'  int err = PCO_GetSizes(hCamera, &wXResAct, &wYResAct, &wXResMax, &wYResMax);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetROI(HANDLE ph,
	'                            WORD *wRoiX0, // Roi upper left x
	'                            WORD *wRoiY0, // Roi upper left y
	'                            WORD *wRoiX1, // Roi lower right x
	'                            WORD *wRoiY1);// Roi lower right y
  Public Declare Function PCO_GetROI Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wRoiX0 As Short, ByRef wRoiY0 As Short, ByRef wRoiX1 As Short, ByRef wRoiY1 As Short) As Integer
	'// Gets the region of interest of the camera. X0, Y0 start at 1. X1, Y1 end with max. sensor size.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wRoiX0 -> Pointer to a WORD variable to receive the x value for the upper left corner.
	'//     WORD *wRoiY0 -> Pointer to a WORD variable to receive the y value for the upper left corner.
	'//     WORD *wRoiX1 -> Pointer to a WORD variable to receive the x value for the lower right corner.
	'//     WORD *wRoiY0 -> Pointer to a WORD variable to receive the y value for the lower right corner.
	'//      x0,y0----------|
	'//      |     ROI      |
	'//      ---------------x1,y1
	'// Out: int -> Error message.
	'/* Example: see PCO_GetSizes */
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetROI(HANDLE ph,
	'                            WORD wRoiX0, // Roi upper left x
	'                            WORD wRoiY0, // Roi upper left y
	'                            WORD wRoiX1, // Roi lower right x
	'                            WORD wRoiY1);// Roi lower right y
  Public Declare Function PCO_SetROI Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wRoiX0 As Short, ByVal wRoiY0 As Short, ByVal wRoiX1 As Short, ByVal wRoiY1 As Short) As Integer
	'// Gets the region of interest of the camera. X0, Y0 start at 1. X1, Y1 end with max. sensor size.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wRoiX0 -> WORD variable to hold the x value for the upper left corner.
	'//     WORD wRoiY0 -> WORD variable to hold the y value for the upper left corner.
	'//     WORD wRoiX1 -> WORD variable to hold the x value for the lower right corner.
	'//     WORD wRoiY0 -> WORD variable to hold the y value for the lower right corner.
	'//      x0,y0----------|
	'//      |     ROI      |
	'//      ---------------x1,y1
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  WORD wRoiX0;                         // x value for the upper left corner.
	'  WORD wRoiY0;                         // y value for the upper left corner.
	'  WORD wRoiX1;                         // x value for the lower right corner.
	'  WORD wRoiY0;                         // y value for the lower right corner.
	'
	'  wRoiX0 = 20;  wRoiX1 = 820;  wRoiY0 = 200;  wRoiY1 = 400;
	'  int err = PCO_SetROI(hCamera, wRoiX0, wRoiY0, wRoiX1, wRoiY1);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetBinning(HANDLE ph,
	'                                WORD *wBinHorz, // Binning horz. (x)
	'                                WORD *wBinVert);// Binning vert. (y)
  Public Declare Function PCO_GetBinning Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wBinHorz As Short, ByRef wBinVert As Short) As Integer
	'// Gets the binning values of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wBinHorz -> Pointer to a WORD variable to hold the horizontal binning value.
	'//     WORD *wBinVert -> Pointer to a WORD variable to hold the vertikal binning value.
	'// Out: int -> Error message.
	'/* Example: PCO_GetSizes */
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetBinning(HANDLE ph,
	'                                WORD wBinHorz, // Binning horz. (x)
	'                                WORD wBinVert);// Binning vert. (y)
  Public Declare Function PCO_SetBinning Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wBinHorz As Short, ByVal wBinVert As Short) As Integer
	'// Sets the binning values of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wBinHorz -> WORD variable to hold the horizontal binning value.
	'//     WORD wBinVert -> WORD variable to hold the vertikal binning value.
	'// Out: int -> Error message.
	'/* Example: PCO_SetROI */
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetPixelRate(HANDLE ph,
	'                                  DWORD *dwPixelRate); // Pixelrate
  Public Declare Function PCO_GetPixelRate Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dwPixelRate As Integer) As Integer
	'// Gets the pixel rate of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD *dwPixelRate -> Pointer to a DWORD variable to receive the pixelrate.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  DWORD dwPixelRate;                   // PixelRate
	'
	'  int err = PCO_GetPixelRate(hCamera, &dwPixelRate);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetPixelRate(HANDLE ph,
	'                                  DWORD dwPixelRate); // Pixelrate
  Public Declare Function PCO_SetPixelRate Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal dwPixelRate As Integer) As Integer
	'// Sets the pixel rate of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD dwPixelRate -> DWORD variable to hold the pixelrate.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  DWORD dwPixelRate;
	'
	'  dwPixelRate = 20000000;              // PixelRate in Hz
	'  int err = PCO_SetPixelRate(hCamera, dwPixelRate);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetConversionFactor(HANDLE ph,
	'                                 WORD *wConvFact); // Conversion Factor (Gain)
  Public Declare Function PCO_GetConversionFactor Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wConvFact As Short) As Integer
	'// Gets the conversion factor of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wConvFact -> Pointer to a WORD variable to receive the conversin factor.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetConversionFactor(HANDLE ph,
	'                                 WORD wConvFact); // Conversion Factor (Gain)
  Public Declare Function PCO_SetConversionFactor Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wConvFact As Short) As Integer
	'// Sets the conversion factor of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wConvFact -> WORD variable to hold the conversin factor.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetDoubleImageMode(HANDLE ph,
	'                                        WORD *wDoubleImage); // DoubleShutter Mode
  Public Declare Function PCO_GetDoubleImageMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wDoubleImage As Short) As Integer
	'// Gets the double image mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wDoubleImage -> Pointer to a WORD variable to receive the double image mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetDoubleImageMode(HANDLE ph,
	'                                        WORD wDoubleImage); // DoubleShutter Mode
  Public Declare Function PCO_SetDoubleImageMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wDoubleImage As Short) As Integer
	'// Sets the double image mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wDoubleImage -> WORD variable to hold the double image mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetADCOperation(HANDLE ph,
	'                                     WORD *wADCOperation); // ADC Operation
  Public Declare Function PCO_GetADCOperation Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wADCOperation As Short) As Integer
	'// Gets the adc operation mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wADCOperation -> Pointer to a WORD variable to receive the adc operation mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetADCOperation(HANDLE ph,
	'                                     WORD wADCOperation); // ADC Operation
  Public Declare Function PCO_SetADCOperation Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wADCOperation As Short) As Integer
	'// Sets the adc operation mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wADCOperation -> WORD variable to hold the adc operation mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetIRSensitivity(HANDLE ph,
	'                               WORD *wIR); // IR Sensitivity
  Public Declare Function PCO_GetIRSensitivity Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wIR As Short) As Integer
	'// Gets the IR Sensitivity mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wIR -> Pointer to a WORD variable to receive the IR Sensitivity mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetIRSensitivity(HANDLE ph,
	'                               WORD wIR); // IR Sensitivity
  Public Declare Function PCO_SetIRSensitivity Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wIR As Short) As Integer
	'// Sets the IR Sensitivity mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wIR -> WORD variable to hold the IR Sensitivity mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCoolingSetpointTemperature(HANDLE ph,
	'                                SHORT *sCoolSet); // Cooling setpoint
  Public Declare Function PCO_GetCoolingSetpointTemperature Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef sCoolSet As Short) As Integer
	'// Gets the cooling setpoint temperature of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     SHORT *sCoolSet -> Pointer to a SHORT variable to receive the cooling setpoint temperature.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetCoolingSetpointTemperature(HANDLE ph,
	'                                SHORT sCoolSet); // Cooling setpoint
  Public Declare Function PCO_SetCoolingSetpointTemperature Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal sCoolSet As Short) As Integer
	'// Sets the cooling setpoint temperature of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     SHORT sCoolSet -> SHORT variable to hold the cooling setpoint temperature.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetOffsetMode(HANDLE ph,
	'                                   WORD *wOffsetRegulation); // Offset mode
  Public Declare Function PCO_GetOffsetMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wOffsetRegulation As Short) As Integer
	'// Gets the offset mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wOffsetRegulation -> Pointer to a WORD variable to receive the offset mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetOffsetMode(HANDLE ph,
	'                                   WORD wOffsetRegulation); // Offset mode
  Public Declare Function PCO_SetOffsetMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wOffsetRegulation As Short) As Integer
	'// Sets the offset mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wOffsetRegulation -> WORD variable to hold the offset mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetNoiseFilterMode(HANDLE ph,
	'                                   WORD *wNoiseFilterMode); //
  Public Declare Function PCO_GetNoiseFilterMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wNoiseFilterMode As Short) As Integer
	'// Gets the noise filter mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wNoiseFilterMode -> Pointer to a WORD variable to receive the noise filter mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetNoiseFilterMode(HANDLE ph,
	'                                   WORD wNoiseFilterMode); //
  Public Declare Function PCO_SetNoiseFilterMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wNoiseFilterMode As Short) As Integer
	'// Sets the noise filter mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wNoiseFilterMode -> WORD variable to hold the noise filter mode.
	'// Out: int -> Error message.
	'
	'/* Following functions have been removed:
	'SC2_SDK_FUNC int WINAPI PCO_StartSelfCalibration(HANDLE ph, WORD *wCalibrated);
	'// Starts the self calibration of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wCalibrated -> Pointer to a WORD variable to receive the result of the calibration.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetDSNUCorrectionMode(HANDLE ph,
	'                                   WORD *wDSNUCorrectionMode);
	'// Gets the DSNU correction mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wDSNUCorrectionMode -> Pointer to a WORD variable to receive the corr. mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetDSNUCorrectionMode(HANDLE ph,
	'                                   WORD wDSNUCorrectionMode);
	'// Sets the DSNU correction mode of the camera, if available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wDSNUCorrectionMode -> WORD variable to hold the corr. mode.
	'// Out: int -> Error message.*/
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: Sensor commands ////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// Timing commands /////////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'SC2_SDK_FUNC int WINAPI PCO_GetTimingStruct(HANDLE ph, PCO_Timing *strTiming);
	
	'// Gets all of the timing data in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Timing *strTiming -> Pointer to a PCO_Timing structure.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetTimingStruct(HANDLE ph, PCO_Timing *strTiming);
	'// Sets all of the timing data in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Timing strTiming -> PCO_Timing structure.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetDelayExposureTime(HANDLE ph, // Timebase: 0-ns; 1-us; 2-ms
	'                             DWORD* dwDelay,
	'                             DWORD* dwExposure,
	'                             WORD* wTimeBaseDelay,
	'                             WORD* wTimeBaseExposure);
  Public Declare Function PCO_GetDelayExposureTime Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dwDelay As Integer, ByRef dwExposure As Integer, ByRef wTimeBaseDelay As Short, ByRef wTimeBaseExposure As Short) As Integer
    '// Gets the exposure and delay time and the time bases of the camera.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     DWORD* dwDelay -> Pointer to a DWORD variable to receive the exposure time.
    '//     DWORD* dwExposure -> Pointer to a DWORD variable to receive the delay time.
    '//     WORD* wTimeBaseDelay -> Pointer to a WORD variable to receive the exp. timebase.
    '//     WORD* wTimeBaseExposure -> Pointer to a WORD variable to receive the del. timebase.
    '// Timebase: 0 -> value is in ns: exp. time of 100 means 0.0000001s.
    '//           1 -> value is in us: exp. time of 100 means 0.0001s.
    '//           2 -> value is in ms: exp. time of 100 means 0.1s.
    '// Out: int -> Error message.
    '
    'SC2_SDK_FUNC int WINAPI PCO_SetDelayExposureTime(HANDLE ph, // Timebase: 0-ns; 1-us; 2-ms
    '                             DWORD dwDelay,
    '                             DWORD dwExposure,
    '                             WORD wTimeBaseDelay,
    '                             WORD wTimeBaseExposure);


    Public Declare Function PCO_SetFPSExposureMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wFPSExposureMode As Integer, ByVal dwFPSExposureTime As Short) As Integer
    '//set fps mode




    Public Declare Function PCO_SetDelayExposureTime Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal dwDelay As Integer, ByVal dwExposure As Integer, ByVal wTimeBaseDelay As Short, ByVal wTimeBaseExposure As Short) As Integer
	'// Sets the exposure and delay time and the time bases of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD dwDelay -> DWORD variable to hold the exposure time.
	'//     DWORD dwExposure -> DWORD variable to hold the delay time.
	'//     WORD wTimeBaseDelay -> WORD variable to hold the exp. timebase.
	'//     WORD wTimeBaseExposure -> WORD variable to hold the del. timebase.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetDelayExposureTimeTable(HANDLE ph, // Timebase: 0-ns; 1-us; 2-ms
	'                                  DWORD* dwDelay,
	'                                  DWORD* dwExposure,
	'                                  WORD* wTimeBaseDelay,
	'                                  WORD* wTimeBaseExposure,
	'                                  WORD wCount);
	'// Gets the exposure and delay time table and the time bases of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD* dwDelay -> Pointer to a DWORD array to receive the exposure times.
	'//     DWORD* dwExposure -> Pointer to a DWORD array to receive the delay times.
	'//     WORD* wTimeBaseDelay -> Pointer to a WORD variable to receive the exp. timebase.
	'//     WORD* wTimeBaseExposure -> Pointer to a WORD variable to receive the del. timebase.
	'// Out: int -> Error message.
	'/* Example: see PCO_SetDelayExposureTimeTable */
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetDelayExposureTimeTable(HANDLE ph, // Timebase: 0-ns; 1-us; 2-ms
	'                                  DWORD* dwDelay,
	'                                  DWORD* dwExposure,
	'                                  WORD wTimeBaseDelay,
	'                                  WORD wTimeBaseExposure,
	'                                  WORD wCount);
	'// Sets the exposure and delay time table and the time bases of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD* dwDelay -> Pointer to a DWORD array to hold the exposure times.
	'//     DWORD* dwExposure -> Pointer to a DWORD array to hold the delay times.
	'//     WORD wTimeBaseDelay -> WORD variable to hold the exp. timebase.
	'//     WORD wTimeBaseExposure -> WORD variable to hold the del. timebase.
	'// Out: int -> Error message.
	'/* Example:
	'#define MAXTIMEPAIRS 16 // maximum count of delay and exposure pairs
	'  HANDLE hHandleCam;
	'  ...
	'  DWORD dwDelay[MAXTIMEPAIRS], dwExposure[MAXTIMEPAIRS];
	'  WORD wTimeBaseDelay, wTimeBaseExposure;
	'  int err = PCO_GetDelayExposureTimeTable(hHandleCam, &dwDelay[0], &dwExposure[0],
	'                                          &wTimeBaseDelay, &wTimeBaseExposure, MAXTIMEPAIRS);
	'  dwDelay[0] = 100;
	'  dwExposure[0] = 10;
	'  dwDelay[1] += 200;
	'  dwExposure[1] += 10;                 // This changes only the first two pairs.
	'  int err = PCO_SetDelayExposureTimeTable(hHandleCam, &dwDelay[0], &dwExposure[0],
	'                                          wTimeBaseDelay, wTimeBaseExposure, 2);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetTriggerMode(HANDLE ph, WORD* wTriggerMode);
  Public Declare Function PCO_GetTriggerMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wTriggerMode As Short) As Integer
	'// Gets the trigger mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wTriggerMode -> Pointer to a WORD variable to receive the triggermode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetTriggerMode(HANDLE ph, WORD wTriggerMode);
  Public Declare Function PCO_SetTriggerMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wTriggerMode As Short) As Integer
	'// Sets the trigger mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wTriggerMode -> WORD variable to hold the triggermode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_ForceTrigger(HANDLE ph, WORD *wTriggered);
  Public Declare Function PCO_ForceTrigger Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wTriggerMode As Short) As Integer
	'// Forces a trigger to the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wTriggered -> Pointer to a WORD variable to receive whether
	'//                         a trigger occured or not.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraBusyStatus(HANDLE ph, WORD* wCameraBusyState);
  Public Declare Function PCO_GetCameraBusyStatus Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wCameraBusyState As Short) As Integer
	'// Gets the busy state of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wCameraBusyState -> Pointer to a WORD variable to receive the busy state.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetPowerDownMode(HANDLE ph, WORD* wPowerDownMode);
  Public Declare Function PCO_GetPowerDownMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wPowerDownMode As Short) As Integer
	'// Gets the power down mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wPowerDownMode -> Pointer to a WORD variable to receive the power down mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetPowerDownMode(HANDLE ph, WORD wPowerDownMode);
  Public Declare Function PCO_SetPowerDownMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wPowerDownMode As Short) As Integer
    '// Sets the power down mode of the camera, if available.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     WORD wPowerDownMode -> WORD variable to hold the power down mode.
    '// Out: int -> Error message.
    '
    'SC2_SDK_FUNC int WINAPI PCO_GetUserPowerDownTime(HANDLE ph, DWORD* dwPowerDownTime);
    Public Declare Function PCO_GetUserPowerDownTime Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dwTime_us As Integer) As Integer


    '// Gets the power down time of the camera.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     WORD* wPowerDownTime -> Pointer to a WORD variable to receive the power down time.
    '// Out: int -> Error message.
    Public Declare Function PCO_SetUserPowerDownTime Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal dwTime_us As Integer) As Integer

    '
    'SC2_SDK_FUNC int WINAPI PCO_SetUserPowerDownTime(HANDLE ph, DWORD dwPowerDownTime);
    '// Sets the power down time of the camera, if available.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     WORD* wPowerDownTime -> Pointer to a WORD variable to receive the power down time.
    '// Out: int -> Error message.
    '
    'SC2_SDK_FUNC int WINAPI PCO_GetExpTrigSignalStatus(HANDLE ph, WORD* wExpTrgSignal);
    Public Declare Function PCO_GetExpTrigSignalStatus Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wExpTrgSignal As Short) As Integer
	'// Gets the exposure trigger signal state of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wExpTrgSignal -> Pointer to a WORD variable to receive the
	'//                            exposure trigger signal state.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCOCRuntime(HANDLE ph, DWORD* dwTime_s, DWORD* dwTime_us);
  Public Declare Function PCO_GetCOCRuntime Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dwTime_s As Integer, ByRef dwTime_us As Integer) As Integer
	'// Gets the exposure trigger signal state of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD* dwTime_s -> Pointer to a DWORD variable to receive the
	'//                        time part in seconds of the COC.
	'//     DWORD* dwTime_us -> Pointer to a DWORD variable to receive the
	'//                         time part in microseconds of the COC.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetModulationMode(HANDLE ph, WORD *wModulationMode, DWORD *dwPeriodicalTime,
	'                                              WORD *wTimebasePeriodical, DWORD *dwNumberOfExposures,
	'                                              LONG *lMonitorOffset);
	'// Gets the modulation mode and neccessary parameters
	'// In: HANDLE ph -> Handle to a proviously opened camera.
	'//     WORD *wModulationMode -> Pointer to a WORD variable to receive the modulation mode
	'//     DWORD *dwPeriodicalTime -> Pointer to a DWORD variable to receive the periodical time
	'//     WORD *wTimebasePeriodical -> Pointer to a WORD variable to receive the time base of pt
	'//     DWORD *dwNumberOfExposures -> Pointer to a DWORD variable to receive the number of exposures
	'//     LONG *lMonitorOffset -> Pointer to a signed DWORD variable to receive the monitor offset
	'// Out: int -> Error message
	'SC2_SDK_FUNC int WINAPI PCO_SetModulationMode(HANDLE ph, WORD wModulationMode, DWORD dwPeriodicalTime,
	'                                              WORD wTimebasePeriodical, DWORD dwNumberOfExposures,
	'                                              LONG lMonitorOffset);
	'// Sets the modulation mode and neccessary parameters
	'// In: HANDLE ph -> Handle to a proviously opened camera.
	'//     WORD wModulationMode -> WORD variable to hold the modulation mode
	'//     DWORD dwPeriodicalTime -> DWORD variable to hold the periodical time
	'//     WORD wTimebasePeriodical -> WORD variable to hold the time base of pt
	'//     DWORD dwNumberOfExposures -> DWORD variable to hold the number of exposures
	'//     LONG lMonitorOffset -> DWORD variable to hold the monitor offset
	'// Out: int -> Error message
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: Timing commands ////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// Storage commands ////////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'SC2_SDK_FUNC int WINAPI PCO_GetStorageStruct(HANDLE ph, PCO_Storage *strStorage);
	'UPGRADE_WARNING: Structure PCO_storage may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="C429C3A5-5D47-4CD9-8F51-74A1616405DC"'
  Public Declare Function PCO_GetStorageStruct Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef strStorage As PCO_storage) As Integer
	'// Gets all of the storage data in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Recording *strRecording -> Pointer to a PCO_Recording structure.
	'// Out: int -> Error message.
	'
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetStorageStruct(HANDLE ph, PCO_Storage *strStorage);
	'// Sets all of the storage data in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Recording *strRecording -> Pointer to a PCO_Recording structure.
	'// Out: int -> Error message.
	'
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraRamSize(HANDLE ph, DWORD* dwRamSize, WORD* wPageSize);
  Public Declare Function PCO_GetCameraRamSize Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dwRamSize As Integer, ByRef wPageSize As Short) As Integer
	'// Gets the ram and pagesize of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD* dwRamSize -> Pointer to a DWORD variable to receive the ramsize in pages.
	'//     DWORD* dwPageSize -> Pointer to a DWORD variable to receive the pagesize in bytes.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetCameraRamSegmentSize(HANDLE ph, DWORD* dwRamSegSize);
  Public Declare Function PCO_GetCameraRamSegmentSize Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dwRamSegSize As Integer) As Integer
	'// Gets the segmentsizes of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD* dwRamSegSize -> Pointer to a DWORD array to receive the ramsegmentsize in pages.
	'// Out: int -> Error message.
	'/* Example: see PCO_SetCameraRamSegmentSize */
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetCameraRamSegmentSize(HANDLE ph, DWORD* dwRamSegSize);
  Public Declare Function PCO_SetCameraRamSegmentSize Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal dwRamSegSize As Integer) As Integer
	'// Gets the segmentsizes of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD* dwRamSegSize -> Pointer to a DWORD array to receive the ramsegmentsize in pages.
	'// Out: int -> Error message.
	'/* Example:
	'  #define MAXSEGMENTS 4
	'  HANDLE hHandleCam;
	'  ...
	'  DWORD dwRamSegSize[MAXSEGMENTS];
	'  int err = PCO_GetCameraRamSegmentSize(hHandleCam, &dwRamSegSize[0]);
	'  dwRamSegSize[0] = dwRamSegSize[0] + dwRamSegSize[1] + dwRamSegSize[2] + dwRamSegSize[3];
	'  dwRamSegSize[1] = dwRamSegSize[2] = dwRamSegSize[3] = 0;// Set all memory to segment 1.
	'  // Our camera has got 4 segments (up to now). They start with Segment 1, up to 4.
	'  // In programming languages every array starts with index 0! So, segment number 1
	'  // has the index 0, seg. 2 has 1, 3 has 2 and 4 has 3.
	'  err = PCO_SetCameraRamSegmentSize(hHandleCam, &dwRamSegSize[0]);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_ClearRamSegment(HANDLE ph);
  Public Declare Function PCO_ClearRamSegment Lib "sc2_cam.dll" (ByVal hdriver As IntPtr) As Integer
	'// Clears (deletes all images) of the actieve ram segment of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetActiveRamSegment(HANDLE ph, WORD* wActSeg);
  Public Declare Function PCO_GetActiveRamSegment Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wActSeg As Short) As Integer
	'// Gets the active ram segment of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wActSeg -> Pointer to a WORD variable to receive the actual segment.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetActiveRamSegment(HANDLE ph, WORD wActSeg);
  Public Declare Function PCO_SetActiveRamSegment Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wActSeg As Short) As Integer
	'// Sets the active ram segment of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wActSeg -> WORD variable to hold the actual segment.
	'// Out: int -> Error message.
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: Storage commands ///////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// Recording commands //////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'SC2_SDK_FUNC int WINAPI PCO_GetRecordingStruct(HANDLE ph, PCO_Recording *strRecording);
	'// Gets all of the recording data in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Recording *strRecording -> Pointer to a PCO_Recording structure.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetRecordingStruct(HANDLE ph, PCO_Recording *strRecording);
	'// Sets all of the recording data in one structure.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Recording *strRecording -> Pointer to a PCO_Recording structure.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetStorageMode(HANDLE ph, WORD* wStorageMode);
  Public Declare Function PCO_GetStorageMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wStorageMode As Short) As Integer
	'// Gets the storage mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wStorageMode -> Pointer to a WORD variable to receive the storage mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetStorageMode(HANDLE ph, WORD wStorageMode);
  Public Declare Function PCO_SetStorageMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wStorageMode As Short) As Integer
	'// Sets the storage mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wStorageMode -> WORD variable to hold the storage mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetRecorderSubmode(HANDLE ph, WORD* wRecSubmode);
  Public Declare Function PCO_GetRecorderSubmode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wRecSubmode As Short) As Integer
	'// Gets the recorder sub mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wRecSubmode -> Pointer to a WORD variable to receive the recorder sub mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetRecorderSubmode(HANDLE ph, WORD wRecSubmode);
  Public Declare Function PCO_SetRecorderSubmode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wRecSubmode As Short) As Integer
	'// Sets the recorder sub mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wRecSubmode -> WORD variable to hold the recorder sub mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetRecordingState(HANDLE ph, WORD* wRecState);
  Public Declare Function PCO_GetRecordingState Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wRecState As Short) As Integer
	'// Gets the recording state of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wRecState -> Pointer to a WORD variable to receive the recording state.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetRecordingState(HANDLE ph, WORD wRecState);
  Public Declare Function PCO_SetRecordingState Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wRecState As Short) As Integer
	'// Sets the recording state of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wRecState -> WORD variable to hold the recording state.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_ArmCamera(HANDLE ph);
  Public Declare Function PCO_ArmCamera Lib "sc2_cam.dll" (ByVal hdriver As IntPtr) As Integer
	'// Sets all previously set data to the camera operation code. This command prepares the
	'// camera for recording images. This is the last command before setting the recording
	'// state. If you change any settings after this command, you have to send this command again.
	'// If you don't arm your camera after changing settings, the camera will run with the last
	'// 'armed' settings and in this case you do not know in what way your camera is acquiring images.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetAcquireMode(HANDLE ph, WORD* wAcquMode);
  Public Declare Function PCO_GetAcquireMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wAcquMode As Short) As Integer
	'// Gets the acquire mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wAcquMode -> Pointer to a WORD variable to receive the acquire mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetAcquireMode(HANDLE ph, WORD wAcquMode);
  Public Declare Function PCO_SetAcquireMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wAcquMode As Short) As Integer
	'// Sets the acquire mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wAcquMode -> WORD variable to hold the acquire mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetAcqEnblSignalStatus(HANDLE ph, WORD* wAcquEnableState);
  Public Declare Function PCO_GetAcqEnblSignalStatus Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wAcquEnableState As Short) As Integer
	'// Gets the acquire enable signal status.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wAcquEnableState -> Pointer to a WORD variable to receive the acquire
	'//                               enable signal status.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetDateTime(HANDLE ph,
	'                                        BYTE ucDay,
	'                                        BYTE ucMonth,
	'                                        WORD wYear,
	'                                        WORD wHour,
	'                                        BYTE ucMin,
	'                                        BYTE ucSec);
  Public Declare Function PCO_SetDateTime Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef ucDay As Byte, ByRef ucMonth As Integer, ByRef wYear As Short, ByRef wHour As Short, ByRef ucMin As Byte, ByRef ucsec As Byte) As Integer
	'// Sets the time and date to the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     BYTE ucDay -> Day of month (1-31).
	'//     BYTE ucMonth -> Month of the year (1-12).
	'//     WORD wYear -> Year with four digits: 2003
	'//     WORD wHour -> Hour of day in 24hour mode
	'//     BYTE ucMin -> Minute
	'//     BYTE ucSec -> Second
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetTimestampMode(HANDLE ph, WORD* wTimeStampMode);
  Public Declare Function PCO_GetTimestampMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wTimeStampMode As Short) As Integer
	'// Gets the time stamp mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wTimeStampMode -> Pointer to a WORD variable to receive the time stamp mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetTimestampMode(HANDLE ph, WORD wTimeStampMode);
  Public Declare Function PCO_setTimestampMode Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wTimeStampMode As Short) As Integer
	'// Sets the time stamp mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wTimeStampMode -> WORD variable to hold the time stamp mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetRecordStopEvent(HANDLE ph, WORD* wRecordStopEventMode, DWORD *dwRecordStopDelayImages);
  Public Declare Function PCO_GetRecordStopEvent Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wRecordStopEventMode As Short, ByRef dwRecordStopDelayImages As Integer) As Integer
	'// Sets the time stamp mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wRecordStopEventMode -> Pointer to a WORD variable to receive the record stop event mode.
	'//     DWORD* dwRecordStopDelayImages -> Pointer to a DWORD variable to receive the number of images to pass till stop.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetRecordStopEvent(HANDLE ph, WORD wRecordStopEventMode, DWORD dwRecordStopDelayImages);
  Public Declare Function PCO_SetRecordStopEvent Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wRecordStopEventMode As Short, ByVal dwRecordStopDelayImages As Integer) As Integer
	'// Sets the time stamp mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wRecordStopEventMode -> WORD variable to hold the record stop event mode.
	'//     DWORD dwRecordStopDelayImages -> DWORD variable to hold the number of images to pass till stop.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_StopRecord(HANDLE ph, WORD* wReserved0, DWORD *dwReserved1);
  Public Declare Function PCO_StopRecord Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wReserved0 As Short, ByRef dwReserved1 As Integer) As Integer
	'// Sets the time stamp mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wReserved0 -> Pointer to a WORD variable (Set content to zero: wReserved0 = 0!).
	'//     DWORD* dwReserved1 -> Pointer to a DWORD variable (Set to zero!).
	'// Out: int -> Error message.
	'
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: Recording commands /////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// Image commands //////////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetImageStruct(HANDLE ph, PCO_Image *strImage);
	'// Gets the image data (= segment data of  all segments).
	'// see also GetSegmentStruct
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Segment *strImage -> Pointer to a PCO_Image structure to receive the image data.
	'// Out: int -> Error message.
	'
	'
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetSegmentStruct(HANDLE ph, WORD wSegment, PCO_Segment *strSegment);
	'// Gets the segment data of the addressed segment. The segment data contains the resolution,
	'// binning and ROI of the images plus the valid and the maximum image count.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     PCO_Segment *strSegment -> Pointer to a PCO_Segment structure to receive the segment data.
	'// Out: int -> Error message.
	'
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetSegmentImageSettings(HANDLE ph, WORD wSegment,
	'                                                    WORD* wXRes,
	'                                                    WORD* wYRes,
	'                                                    WORD* wBinHorz,
	'                                                    WORD* wBinVert,
	'                                                    WORD* wRoiX0,
	'                                                    WORD* wRoiY0,
	'                                                    WORD* wRoiX1,
	'                                                    WORD* wRoiY1);
  Public Declare Function PCO_GetSegmentImageSettings Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wXRes As Short, ByRef wYRes As Short, ByRef wBinHorz As Short, ByRef wBinVert As Short, ByRef wRoiX0 As Short, ByRef wRoiY0 As Short, ByRef wRoiX1 As Short, ByRef dwRoiY11 As Short) As Integer
	'// Gets the region of interest of the camera. X0, Y0 start at 1. X1, Y1 end with max. sensor size.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wXRes -> Pointer to a WORD variable to receive the x resolution of the image in segment
	'//     WORD *wYRes -> Pointer to a WORD variable to receive the y resolution of the image in segment
	'//     WORD *wBinHorz -> Pointer to a WORD variable to receive the horizontal binning of the image in segment
	'//     WORD *wBinVert -> Pointer to a WORD variable to receive the vertical binning of the image in segment
	'//     WORD *wRoiX0 -> Pointer to a WORD variable to receive the left x offset of the image in segment
	'//     WORD *wRoiY0 -> Pointer to a WORD variable to receive the upper y offset of the image in segment
	'//     WORD *wRoiX1 -> Pointer to a WORD variable to receive the right x offset of the image in segment
	'//     WORD *wRoiY1 -> Pointer to a WORD variable to receive the lower y offset of the image in segment
	'//      x0,y0----------|
	'//      |     ROI      |
	'//      ---------------x1,y1
	'// Out: int -> Error message.
	'
	'
	'
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetNumberOfImagesInSegment(HANDLE ph,
	'                                             WORD wSegment,
	'                                             DWORD* dwValidImageCnt,
	'                                             DWORD* dwMaxImageCnt);
  Public Declare Function PCO_GetNumberOfImagesInSegment Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wSegment As Short, ByRef dwValidImageCnt As Integer, ByRef dwMaxImageCnt As Integer) As Integer
	'// Gets the number of images in the addressed segment.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD *dwValidImageCnt -> Pointer to a DWORD varibale to receive the valid image count.
	'//     DWORD *dwMaxImageCnt -> Pointer to a DWORD varibale to receive the max image count.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetBitAlignment(HANDLE ph, WORD* wBitAlignment);
  Public Declare Function PCO_GetBitAlignment Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wBitAlignment As Short) As Integer
	'// Gets the bit alignment.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD *wBitAlignment-> Pointer to a WORD variable to receive the bit alignment.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetBitAlignment(HANDLE ph, WORD wBitAlignment);
  Public Declare Function PCO_SetBitAlignment Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wBitAlignment As Short) As Integer
	'// Sets the bit alignment.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wBitAlignment-> WORD variable which holds the bit alignment.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_WriteHotPixelList(HANDLE ph, WORD wListNo, WORD wNumValid,
	'                                 WORD* wHotPixX, WORD* wHotPixY);
	'// Writes a hotpixel list to the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wListNo -> WORD variable which holds the number of the list (zero based).
	'//     WORD wNumValid -> WORD variable which holds the number of valid members
	'//     WORD *wHotPixX -> WORD array which holds the x coordinates of a hotpixel list
	'//     WORD *wHotPixY -> WORD array which holds the y coordinates of a hotpixel list
	'//     x and y coordinates have to be conistent, that means corresponding coordinate pairs
	'//     must have the same index!
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_ReadHotPixelList(HANDLE ph, WORD wListNo, WORD wArraySize, WORD* wNumValid, WORD* wNumMax,
	'                                 WORD* wHotPixX, WORD* wHotPixY);
	'// Reads a hotpixel list from the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wListNo -> WORD variable which holds the number of the list (zero based).
	'//     WORD wArraySize -> WORD variable which holds the number of members, which can be transferred
	'//                        to the list
	'//     WORD *wNumValid -> Pointer to a WORD variable to receive the number of valid hotpixel.
	'//     WORD *wNumMax -> Pointer to a WORD variable to receive the max. possible number of hotpixel.
	'//     WORD *wHotPixX -> WORD array which gets the x coordinates of a hotpixel list
	'//                       This ptr can be set to ZERO if only the valid and max number
	'//                       have to be read.
	'//     WORD *wHotPixY -> WORD array which gets the y coordinates of a hotpixel list
	'//                       This ptr can be set to ZERO if only the valid and max number
	'//                       have to be read.
	'//     x and y coordinates are conistent, that means corresponding coordinate pairs
	'//     have the same index!
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_ClearHotPixelList(HANDLE ph,
	'                                              WORD wListNo,
	'                                              DWORD dwMagic1,
	'                                              DWORD dwMagic2);
	'// Clears a hotpixel list in the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wListNo -> WORD variable which holds the number of the list (zero based).
	'//     DWORD dwMagic1 -> DWORD variable which holds the unlock code 1.
	'//     DWORD dwMagic2 -> DWORD variable which holds the unlock code 2.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetHotPixelCorrectionMode(HANDLE ph,
	'                                                      WORD *wHotPixelCorrectionMode);
	'// Sets the time hot pixel correction mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD* wHotPixelCorrectionMode -> Pointer to a WORD variable to receive the hot pixel correction mode.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetHotPixelCorrectionMode(HANDLE ph,
	'                                                      WORD wHotPixelCorrectionMode);
	'// Sets the time hot pixel correction mode of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wHotPixelCorrectionMode -> WORD variable to hold the hot pixel correction mode.
	'// Out: int -> Error message.
	'
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: Image commands /////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// API Management commands /////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'SC2_SDK_FUNC int WINAPI PCO_OpenCamera(HANDLE *ph, WORD wCamNum);
  Public Declare Function PCO_OpenCamera Lib "sc2_cam.dll" (ByRef hdriver As IntPtr, ByRef wCamNum As Short) As Integer
	'// Opens a new camera object. Gets the description and sets the date and time.
	'// In: HANDLE* ph -> Pointer to a handle to receive the camera handle
	'//     WORD wCamNum -> Current number of the camera, starting with 0.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  int err = PCO_OpenCamera(&hCamera, 0);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_OpenCameraEx(HANDLE *ph, PCO_OpenStruct* strOpenStruct);
	'// Opens a new camera object. Gets the description and sets the date and time.
	'// In: HANDLE* ph -> Pointer to a handle to receive the camera handle
	'//     PCO_OpenStruct* strOpenStruct -> Structure which contains infos about the opening process
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  PCO_OpenStruct strOpenStruct;
	'  ...
	'  strOpenStruct.wSize = sizeof(PCO_OpenStruct);// Sizeof this struct
	'  strOpenStruct.wInterfaceType = PCO_INTERFACE_FW;
	'                                       // 1: Firewire, 2: CamLink with Matrox, 3: CamLink with Silicon SW
	'  strOpenStruct.wCameraNumber = 0;
	'  //strOpenStruct.wCameraNumAtInterface will be filled by the OpenCameraEx call;
	'                                       // Current number of camera at the interface
	'  strOpenStruct.wOpenFlags[0] = InitMode; // Used for setting up camlink with Silicon SW
	'  // Following defines exist:
	'  // #define PCO_SC2_CL_ME3_LOAD_SINGLE_AREA 0x0100
	'  // #define PCO_SC2_CL_ME3_LOAD_DUAL_AREA   0x0200
	'  // #define PCO_SC2_CL_ME3_LOAD_SINGLE_LINE 0x0300
	'  // #define PCO_SC2_CL_ME3_LOAD_DUAL_LINE   0x0400 -> this is the default setting
	'
	'  //strOpenStruct.wOpenFlags[1...19] are not used up to now
	'
	'  int err = PCO_OpenCamera(&hCamera, &strOpenStruct);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_CloseCamera(HANDLE ph);
  Public Declare Function PCO_CloseCamera Lib "sc2_cam.dll" (ByVal hdriver As IntPtr) As Integer
	
	'// Closes a camera object.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hCamera;
	'  ...
	'  int err = PCO_OpenCamera(&hCamera, 0);
	'  ...
	'  err = PCO_CloseCamera(hCamera);
	'*/
	'
	'SC2_SDK_FUNC int WINAPI PCO_AllocateBuffer(HANDLE ph,
	'                                           SHORT* sBufNr,
	'                                           DWORD size,
	'                                           WORD** wBuf,
	'                                           HANDLE *hEvent);
	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="FAE78A8D-8978-4FD4-8208-5B7324A8F795"'
  Public Declare Function PCO_AllocateBuffer Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef sBufNr As Short, ByVal size As Integer, ByRef pwbuf As Long, ByRef hevent As Integer) As Integer

    '// Allocates an image buffer to receive the image data.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     SHORT *sBufNr -> Pointer to a SHORT variable to hold and receive the buffer number.
    '//                      If a new buffer has to be assigned, set sBufNr to -1.
    '//                      If an existing buffer should be changed, set sBufNr to the desired nr.
    '//     DWORD size -> Size of the buffer to be created, or to be changed to.
    '//     WORD** wBuf -> Pointer to a pointer to a WORD to receive the image data pointer.
    '//     HANDLE* hEvent -> Pointer to an event handle to receive or to hold an event.
    '//                       If hEvent set to NULL, a new event will be created and
    '//                       will be returned through this pointer.
    '//                       You can create an event handle externally, if you wish, and you can set this
    '//                       externally created event handle to become this buffer event handle.
    '// Out: int -> Error message.
    '/* Example:
    '  HANDLE hHandleCam;
    '  SHORT sBufNr;
    '  WORD *wBuf;                          // wBuf[0...size] represents the image data
    '  HANDLE hEvent;
    '  DWORD size, newsize;
    '  ...
    '  WORD wXResAct;                       // Actual X Resolution
    '  WORD wYResAct;                       // Actual Y Resolution
    '  WORD wXResMax;                       // Maximum X Resolution
    '  WORD wYResMax;                       // Maximum Y Resolution
    '  int err = PCO_GetSizes(hCamera, &wXResAct, &wYResAct, &wXResMax, &wYResMax);
    '  size = wXResMax * wYResMax * sizeof(WORD);
    '  sBufNr = -1;
    '  hEvent = NULL;                       // hEvent must be set to either NULL
    '  // or if you like to create your own event: hEvent = CreateEvent(0, TRUE, FALSE, NULL);
    '  // wBuf will receive the pointer to the image data.
    '  err = PCO_AllocateBuffer(hHandleCam, &sBufNr, size, &wBuf, &hEvent);
    '  // Get some image here...
    '  WORD wPixelValuePixel100 = wBuf[100];// Direct access to image data.
    '  ...
    '  newsize = wXResAct * wYResAct * sizeof(WORD);// reallocate buffer to a new size.
    '  err = PCO_AllocateBuffer(hHandleCam, &sBufNr, newsize, &wBuf, NULL);
    '  ...
    '*/
    'SC2_SDK_FUNC int WINAPI PCO_WaitforBuffer(HANDLE ph, int nr_of_buffer, PCO_Buflist *bl, int timeout);
    Public Declare Function PCO_WaitforBuffer Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal nr_of_buffer As Integer, PCO_Buflist As Long, timeout As Integer) As Integer
    '// Waits for one image buffer in bl and returns if one of the buffers is ready. This function is mainly
    '// used in Linux. In Windows it is implemented for platform independence.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     int nr_of_buffer -> number of buffers in PCO_Buflist.
    '//     PCO_Buflist *bl -> Pointer to a buffer list, which holds the buffers to process.
    '//     int timeout -> Timeout in milliseconds.
    '// Out: int -> Error message.
    '
    '
    'SC2_SDK_FUNC int WINAPI PCO_FreeBuffer(HANDLE ph, SHORT sBufNr);
    Public Declare Function PCO_FreeBuffer Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal sBufNr As Short) As Integer
	
	'// Frees an previously allocated image buffer.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     SHORT sBufNr -> SHORT variable to hold the buffer number.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_AddBuffer(HANDLE ph, DWORD dw1stImage, DWORD dwLastImage, SHORT sBufNr);
	'SC2_SDK_FUNC int WINAPI PCO_AddBufferEx(HANDLE ph, DWORD dw1stImage, DWORD dwLastImage, SHORT sBufNr,
	'                                        WORD wXRes, WORD wYRes, WORD wBitPerPixel);
  Public Declare Function PCO_AddBuffer Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dw1stImage As Integer, ByRef dwLastImage As Integer, ByRef sBufNr As Short) As Integer
  Public Declare Function PCO_AddBufferEx Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal dw1stImage As Integer, ByVal dwLastImage As Integer, ByVal sBufNr As Short, ByVal Xres As Short, ByVal Yres As Short, ByVal wBitPerPixel As Short) As Integer
	'// Adds an image buffer to the driver queue and tries to get the requested images. If the function
	'// has been done, the event will be fired. This function returns immediately.
	'// If you've allocated the buffer externally, you have to call the Ex function with the correct sizes.
	'// This call sets all temporary flags of the buffer status to 0;
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD dw1stImage -> DWORD variable to hold the image number of the first image to be retrieved
	'//                         This value has to be set to 0, if you are running in preview mode.
	'//     DWORD dwLastImage -> DWORD variable to hold the image number of the last image to be retrieved
	'//                         This value has to be set to 0, if you are running in preview mode.
	'//     SHORT sBufNr -> SHORT variable to hold the buffer number which should be used to get the images.
	'// InEx: WORD wXRes, WORD wYRes, WORD wBytePerPixel -> Used to calculate the size of the image in RAM
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetBufferStatus(HANDLE ph,
	'                                            SHORT sBufNr,
	'                                            DWORD *dwStatusDll,
	'                                            DWORD *dwStatusDrv);
  Public Declare Function PCO_GetBufferStatus Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal sBufNr As Short, ByRef dwStatusDll As Integer, ByRef dwStatusDrv As Integer) As Integer
	
	'// Gets the status info about the buffer.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     SHORT sBufNr -> SHORT variable to hold the number of the buffer to query.
	'//     DWORD *dwStatusDll -> Pointer to a DWORD variable to receive the status in the SC2_Cam.dll
	'//                           The status is separated into two groups of flags. 0xFFFF0000 reflect
	'//                           the static flags and 0x0000FFFF the dynamic flags. The dynamic flags
	'//                           will be reset by Allocate- and AddBuffer.
	'//                           0x80000000: Buffer is allocated
	'//                           0x40000000: Buffer event created internally
	'//                           0x00008000: Buffer event is set.
	'//     DWORD *dwStatusDrv -> Pointer to a DWORD variable to receive the status in the driver.
	'//                           In case of a successful execution this parameter will be set to
	'//                           PCO_NOERROR (= 0). If an error occurs this parameter will be set
	'//                           to some PCO_errormessage.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_RemoveBuffer(HANDLE ph);
  Public Declare Function PCO_RemoveBuffer Lib "sc2_cam.dll" (ByVal hdriver As IntPtr) As Integer
	'// Removes any buffer from the driver queue.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI (HANDLE ph, WORD wSegment, DWORD dw1stImage, DWORD dwLastImage, SHORT sBufNr);
  Public Declare Function PCO_GetImage Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wSegment As Short, ByRef dw1stImage As Integer, ByRef dwLastImage As Integer, ByRef sBufNr As Short) As Integer
	'SC2_SDK_FUNC int WINAPI PCO_GetImageEx(HANDLE ph, WORD wSegment, DWORD dw1stImage, DWORD dwLastImage, SHORT sBufNr,
	'                                        WORD wXRes, WORD wYRes, WORD wBitPerPixel);
  Public Declare Function PCO_GetImageEx Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wSegment As Short, ByVal dw1stImage As Integer, ByVal dwLastImage As Integer, ByVal sBufNr As Short, ByVal Xres As Short, ByVal Yres As Short, ByVal wBitPerPixel As Short) As Integer
	'// Gets images from the camera. The images will be transferred to a previously
	'// allocated buffer addressed by the sBufNr. This buffer has to be big enough to hold
	'// all the requested images. This function returns after the images are processed.
	'// If you've allocated the buffer externally, you have to call the Ex function with the correct sizes.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD dw1stImage -> DWORD variable to hold the image number of the first image to be retrieved
	'//     DWORD dwLastImage -> DWORD variable to hold the image number of the last image to be retrieved
	'//     SHORT sBufNr -> SHORT variable to hold the buffer number which should be used to get the images.
	'// InEx: WORD wXRes, WORD wYRes, WORD wBytePerPixel -> Used to calculate the size of the image in RAM
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetPendingBuffer(HANDLE ph, int *count);
  Public Declare Function PCO_GetPendingBuffer Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef count As Short) As Integer
	'// Gets the number of buffers which were previously added by PCO_AddBuffer and which are
	'// left in the driver queue for getting images.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     int *count -> Pointer to an int variable to receive the buffer count.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_CancelImages(HANDLE ph);
  Public Declare Function PCO_CancelImages Lib "sc2_cam.dll" (ByVal hdriver As IntPtr) As Integer
	'// Cancels the image processing.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_CheckDeviceAvailability(HANDLE ph, WORD wNum);
  Public Declare Function PCO_CheckDeviceAvailability Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef wNum As Short) As Integer
	'// Checks whether the device is still available.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wNum -> Current number of the device to check
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetDeviceStatus(HANDLE ph, WORD wNum, DWORD *dwStatus, WORD wStatusLen);
  Public Declare Function PCO_GetDeviceStatus Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef dwStatus As Integer, ByRef wStatusLen As Short) As Integer
	'// Gets the DeviceAvailability and the Generation count in case of 1394
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wNum -> Current number of the device to check
	'//     DWORD *dwStatus -> Pointer to an array with at least 1 DWORD
	'//     WORD wStatusLen -> WORD to hold the number of members in the dwStatus array.
	'//     dwStatus[0]-> 0x80000000: Device is available (0: not available)
	'//     dwStatus[1]-> In case of 1394: Generation count (maybe different data with other medias)
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetTransferParameter(HANDLE ph, void* buffer, int ilen);
	'// Gets the transfer parameters for the transfer media
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     void *buffer -> Pointer to an array to receive the transfer parameters.
	'//     int ilen -> Total length of the buffer in bytes.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_SetTransferParameter(HANDLE ph, void* buffer, int ilen);
	'// Sets the transfer parameters for the transfer media
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     void *buffer -> Pointer to an array to set the transfer parameters.
	'//     int ilen -> Total length of the buffer in bytes.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_ControlCommandCall(HANDLE ph,
	'                         void *buf_in,unsigned int size_in,
	'                         void *buf_out,unsigned int size_out);
	'// Issues a low level command to the camera. This call is part of most of the other calls.
	'// Normally you do not need to call this function. It can be used to cover those camera
	'// commands, which are not part of this Dll up to now. Please use the other functions while
	'// their purpose is easier to understand. Furthermore this function is not part of
	'// the SDK description.
	'

  Public Declare Function PCO_CamLinkSetImageParameters Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal wxres As Int16, ByVal wyres As Int16) As Integer
    '// Neccessary while using a CamLink interface
    '// If there is a change in buffer size (ROI, binning) this function has to be called
    '// with the new x and y resolution. Additionally this function has to be called, if you
    '// switch to another camRAM segment and like to get images.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     WORD wxres -> X Resolution of the images to be transferred
    '//     WORD wyres -> Y Resolution of the images to be transferred
    '// Out: int -> Error message.
    '
    'SC2_SDK_FUNC int WINAPI PCO_SetTimeouts(HANDLE ph, void *buf_in,unsigned int size_in);
    Public Declare Function PCO_SetTimeouts Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByRef buf_in As IntPtr, ByVal size_in As Int16) As Integer
    '// Here you can set the timeouts for the driver.
    '// In: HANDLE ph -> Handle to a previously opened camera.
    '//     void *buffer -> Pointer to an array to set the timeout parameters.
    '//     int ilen -> Total length of the buffer in bytes.
    '// [0]: command-timeout,   200ms default, Time to wait while a command is sent.
    '// [1]: image-timeout,    3000ms default, Time to wait while an image is transferred.
    '// [2]: transfer-timeout, 1000ms default, Time to wait till the transfer channel expires.
    '// Out: int -> Error message.
    '
    'SC2_SDK_FUNC int WINAPI PCO_GetBuffer(HANDLE ph, SHORT sBufNr, WORD** wBuf, HANDLE *hEvent);
    Public Declare Function PCO_GetBuffer Lib "sc2_cam.dll" (ByVal hdriver As IntPtr, ByVal sbuf As Short, ByRef wbuf As Integer, ByRef hevent As Integer) As Integer
	'// Gets the data pointer and the event of a buffer.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     SHORT sBufNr -> SHORT variable to hold the buffer number.
	'//     WORD** wBuf -> Pointer to a pointer to a WORD to receive the image data pointer.
	'//     HANDLE* hEvent -> Pointer to an event handle to receive or to hold an event.
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_GetInfoString(HANDLE ph, DWORD dwinfoflags,
	'                         char *buf_in, WORD size_in);
	'// Gets the name of the camera.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     DWORD dwinfoflags -> Not used, now. Set to ZERO!
	'//     char *buf_in -> Pointer to a string, to receive the info string.
	'//     WORD size_in -> WORD variable which holds the maximum length of the string.
	'// Out: int -> Error message.
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: API Management commands ////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// FirmWare commands ///////////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'
	'SC2_SDK_FUNC int WINAPI PCO_ReadHeadEEProm(HANDLE ph, WORD wAddress, BYTE* bData, WORD wLen);
	'// Reads an EEProm data byte at the requested address.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wAddress -> WORD variable to hold the eeprom address
	'//     BYTE* bData -> Pointer to a byte variable to receive the eeprom data.
	'//     WORD wLen -> Length parameter (not used up to now)
	'// Out: int -> Error message.
	'
	'SC2_SDK_FUNC int WINAPI PCO_WriteHeadEEProm(HANDLE ph, WORD wAddress, BYTE bData, WORD wLen);
	'// Writes an EEProm data byte to the requested address.
	'// In: HANDLE ph -> Handle to a previously opened camera.
	'//     WORD wAddress -> WORD variable to hold the eeprom address
	'//     BYTE bData -> Byte variable to hold the eeprom data.
	'//     WORD wLen -> Length parameter (not used up to now)
	'// Out: int -> Error message.
	'
	'/////////////////////////////////////////////////////////////////////
	'/////// End: FirmWare commands //////////////////////////////////////
	'/////////////////////////////////////////////////////////////////////
	'#ifdef __cplusplus
	'}       //  Assume C declarations for C++
	'#endif  //C++
	'
	'//-----------------------------------------------------------------//
	'// Name        | SC2_DialogExport.cpp        | Type: ( ) source    //
	'//-------------------------------------------|       (*) header    //
	'// Project     | PCO SC2 Camera Dialog       |       ( ) others    //
	'//-----------------------------------------------------------------//
	'// Platform    | PC                                                //
	'//-----------------------------------------------------------------//
	'// Environment | Visual 'C++'                                      //
	'//-----------------------------------------------------------------//
	'// Purpose     | PCO - SC2 Camera Dialog DLL Function definitions  //
	'//-----------------------------------------------------------------//
	'// Author      | FRE, PCO AG                                       //
	'//-----------------------------------------------------------------//
	'// Revision    |  rev. 1.05 rel. 1.05                              //
	'//-----------------------------------------------------------------//
	'// Notes       |                                                   //
	'//-----------------------------------------------------------------//
	'// (c) 2002 PCO AG * Donaupark 11 *                                //
	'// D-93309      Kelheim / Germany * Phone: +49 (0)9441 / 2005-0 *  //
	'// Fax: +49 (0)9441 / 2005-20 * Email: info@pco.de                 //
	'//-----------------------------------------------------------------//
	'
	'
	'//-----------------------------------------------------------------//
	'// Revision History:                                               //
	'//-----------------------------------------------------------------//
	'// Rev.:     | Date:      | Changed:                               //
	'// --------- | ---------- | ---------------------------------------//
	'//  0.10     | 03.07.2003 | new file, FRE                          //
	'//-----------------------------------------------------------------//
	'//  0.13     | 08.12.2003 | Adapted some functions.                //
	'//           |            | Changed the functionality.             //
	'//-----------------------------------------------------------------//
	'//  0.16     | 23.03.2004 | changed SDK-structures, FRE            //
	'//-----------------------------------------------------------------//
	'//  1.0      | 04.05.2004 | Released to market                     //
	'//           |            |                                        //
	'//-----------------------------------------------------------------//
	'//  1.03     | xx.yy.2005 |  CLSer eingebaut, MBL                  //
	'//-----------------------------------------------------------------//
	'//  1.04     | 21.04.2005 |  Noisefilter eingebaut, FRE            //
	'//-----------------------------------------------------------------//
	'//  1.05     | 27.02.2006 |  Added PCO_GetCameraName, FRE          //
	'//-----------------------------------------------------------------//
	'
	'#if !defined SC2_STRUCTURES_H
	'#error Include SC2_SDKStructures.h first!
	'#End If
	'
	'#ifdef SC2_DLG_EXPORTS
	'#define SC2_SDK_FUNC_DLG __declspec(dllexport)
	'#Else
	'#define SC2_SDK_FUNC_DLG __declspec(dllimport)
	'#End If
	'
	'#ifdef __cplusplus
	'extern "C" {            //  Assume C declarations for C++
	'#endif  //C++
	'// Init flags
	'#define SC2_SDK_DEF_DLG_PASSIV  0x00000001
	'// Status flags
	'#define SC2_SDK_DEF_DLG_OFF     0x00000000 // Dialog is off
	'#define SC2_SDK_DEF_DLG_ON      0x00000001 // Dialog is on
	'#define SC2_SDK_DEF_DLG_ENABLED 0x00000002 // Dialog is enabled
	'#define SC2_SDK_DEF_DLG_CHANGED 0x00000004 // Dialog value has changed
	'#define SC2_SDK_DEF_DLG_EXTERN  0x00010000 // Dialog changed externally
	'
	'SC2_SDK_FUNC_DLG int WINAPI PCO_OpenDialogCam(HANDLE *ptr,
	'                                              HANDLE hHandleCam,
	'                                              HWND hParent,
	'                                              UINT uiFlags,
	'                                              UINT uiMsgArm,
	'                                              UINT uiMsgCtrl,
	'                                              int iXPos,
	'                                              int iYPos,
	'                                              char *pcCaption);
  Public Declare Function PCO_OpenDialogCam Lib "sc2_dlg.dll" (ByRef hdialog As IntPtr, ByVal hdriver As IntPtr, ByVal hWindow As IntPtr, ByVal uiFlags As Short, ByVal uiMsgArm As Short, ByVal uiMsgCtrl As Short, ByVal iXpos As Integer, ByVal iYpos As Integer, ByVal strTitle As String) As Integer
	'// Opens a new SC2 control dialog and loads the dialog values from the camera structures.
	'// In: HANDLE* ptr -> Pointer to a handle to receive the dialog handle
	'//     HANDLE hHandleCam -> Handle of the camera got by PCO_OpenCamera(HANDLE *ph, WORD wCamNum);
	'//     HWND hParent -> Main application window handle
	'//     UINT uiFlags -> Flags to control the behaviour of the dialog.
	'//          0x00000001 : Passive mode only. The dialog does not send any changed values
	'//                       to the camera.
	'//     UINT uiMsgArm -> Message which is sent to the main application window in case of
	'//                      the accept button being pressed, can be 0 (no message sent)
	'//     UINT uiMsgCtrl -> Message which is sent to the main application window in case of
	'//                      some values are changed, can be 0 (no message sent)
	'//     int iXPos -> X position of the upper left corner of the dialog.
	'//     int iYPos -> Y position of the upper left corner of the dialog.
	'//     char *pcCaption -> String to be displayed in the caption bar of the dialog.
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hDialog;
	'  HANDLE hCamera;
	'  ...
	'  int err = PCO_OpenCamera(&hCamera, 0);
	'  ...
	'  if(err == PCO_NOERROR)
	'    PCO_OpenDialogCam(&hDialog, hCamera, AfxGetApp()->m_pMainWnd->GetSafeHWnd(), 0,
	'                      WM_APP + 10, WM_APP + 11, 200, 300, "SC2 Camera control window");
	'  ...
	'*/
	'
	'SC2_SDK_FUNC_DLG int WINAPI PCO_CloseDialogCam(HANDLE ptr);
  Public Declare Function PCO_CloseDialogCam Lib "sc2_dlg.dll" (ByVal hdialog As IntPtr) As Integer
  '// Closes a previously opened camera control dialog.
  '// In: HANDLE ptr -> Handle of the dialog
  '// Out: int -> Error message.
  '/* Example:
  '  HANDLE hDialog;
  '  ...
  '  int err = PCO_CloseDialogCam(hDialog);
  '  ...
  '*/
  '
  'SC2_SDK_FUNC_DLG int WINAPI PCO_EnableDialogCam(HANDLE ptr, bool bEnable);
  Public Declare Function PCO_EnableDialogCam Lib "sc2_dlg.dll" (ByVal hdialog As IntPtr, ByVal bEnable As Boolean) As Integer
	'// Enables or diables a camera control dialog.
	'// In: HANDLE ptr -> Handle of the dialog
	'//     bool bEnable -> FALSE: Disables dialog, TRUE: Enables dialog
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hDialog;
	'  ...
	'  int err = PCO_EnableDialogCam(hDialog, FALSE);// disable dialog
	'  ...
	'*/
	'
	'SC2_SDK_FUNC_DLG int WINAPI PCO_GetDialogCam(HANDLE ptr, PCO_Camera* strCam);
	'// Gets the dialog settings.
	'// In: HANDLE ptr -> Handle of the dialog
	'//     PCO_Camera* strCam -> Camera struct to receive the values.
	'//                           The strCam->wSize parameter has to be set to sizeof(PCO_Camera),
	'//                           before calling this function. This is in order to check the
	'//                           correct structure size.
	'// Out: int -> Error message.
	'// If the error message shows: PCO_ERROR_SDKDLL_BUFFERSIZE + SC2_ERROR_SDKDLL,
	'// you have to check the version of your SDK header files. The may not fit to the correct
	'// structure size used inside the SC2_Dlg.dll and/or SC2_Camera.dll.
	'/* Example:
	'  HANDLE hDialog;
	'  PCO_Camera strCam;
	'  ...
	'  strCam.wSize = sizeof(PCO_Camera);
	'  int err = PCO_GetDialogCam(hDialog, &strCam);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC_DLG int WINAPI PCO_SetDialogCam(HANDLE ptr, PCO_Camera* strCam);
	'// Sets the dialog settings.
	'// In: HANDLE ptr -> Handle of the dialog
	'//     PCO_Camera* strCam -> Camera struct to hold the values which should be set.
	'//                           The strCam->wSize parameter has to be set to sizeof(PCO_Camera),
	'//                           before calling this function. This is in order to check the
	'//                           correct structure size.
	'//                           It would be best, to get the actual settings from the dialog, first.
	'//                           This parameter can be NULL. In this case the dialog reloads its
	'//                           settings directly from the camera structures.
	'// Out: int -> Error message.
	'// If the error message shows: PCO_ERROR_SDKDLL_BUFFERSIZE + SC2_ERROR_SDKDLL,
	'// you have to check the version of your SDK header files. The may not fit to the correct
	'// structure size used inside the SC2_Dlg.dll and/or SC2_Camera.dll.
	'/* Example 1:
	'  HANDLE hDialog;
	'  PCO_Camera strCam;
	'  ...
	'  strCam.wSize = sizeof(PCO_Camera);
	'  int err = PCO_GetDialogCam(hDialog, &strCam);
	'  ...
	'  strCam.... = xxx;                    // change some values
	'  int err = PCO_SetDialogCam(hDialog, &strCam);
	'  ...
	'*/
	'/* Example 2:
	'  HANDLE hDialog;
	'  ...
	'  int err = PCO_SetDialogCam(hDialog, NULL);
	'  ...
	'*/
	'
	'SC2_SDK_FUNC_DLG DWORD WINAPI PCO_GetStatusDialogCam(HANDLE ptr, DWORD *dwStatus);
	'// Gets the status of the camera control dialog.
	'// In: HANDLE ptr -> Handle of the dialog
	'//     DWORD* dwStatus -> DWORD pointer to receive the status
	'// Stat: 0xnnnnmmmm - n: auto reset during PCO_GetStatusDialogCam, m: static
	'//       0x00000000 - Dialog off
	'//       0x00000001 - Dialog on
	'//       0x00000002 - Dialog enabled
	'//       0x00000004 - Dialog value has changed
	'//       0x00010000 - Dialog changed externally
	'// Out: int -> Error message.
	'/* Example:
	'  HANDLE hDialog;
	'  ...
	'  int status, err;
	'  err = PCO_GetStatusDialogCam(hDialog, &status);
	'  ...
	'*/
	'#ifdef __cplusplus
	'}
	'#endif  //C++
End Module