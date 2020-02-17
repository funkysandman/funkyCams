'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Camera driver for PCO
'
' Description:	Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam 
'				nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam 
'				erat, sed diam voluptua. At vero eos et accusam et justo duo 
'				dolores et ea rebum. Stet clita kasd gubergren, no sea takimata 
'				sanctus est Lorem ipsum dolor sit amet.
'
' Implements:	ASCOM Camera interface version: 1.0
' Author:		(XXX) Your N. Here <your@email.here>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	1.0.0	Initial edit, from Camera template
' ---------------------------------------------------------------------------------
'
'
' Your driver's ID is ASCOM.PCO.Camera
'
' The Guid attribute sets the CLSID for ASCOM.DeviceName.Camera
' The ClassInterface/None addribute prevents an empty interface called
' _Camera from being created and used as the [default] interface
'

' This definition is used to select code that's only applicable for one device type
#Const Device = "Camera"

Imports ASCOM
Imports ASCOM.Astrometry
Imports ASCOM.Astrometry.AstroUtils
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading

<Guid("f9cacceb-5a6c-469d-9b35-acd31b107ce9")>
<ClassInterface(ClassInterfaceType.None)>
Public Class Camera

    ' The Guid attribute sets the CLSID for ASCOM.PCO.Camera
    ' The ClassInterface/None addribute prevents an empty interface called
    ' _PCO from being created and used as the [default] interface

    ' TODO Replace the not implemented exceptions with code to implement the function or
    ' throw the appropriate ASCOM exception.
    '
    Implements ICameraV2

    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Friend Shared driverID As String = "ASCOM.PCO.Camera"
    Private Shared driverDescription As String = "PCO Camera"

    Friend Shared comPortProfileName As String = "COM Port" 'Constants used for Profile persistence
    Friend Shared traceStateProfileName As String = "Trace Level"
    Friend Shared comPortDefault As String = "COM1"
    Friend Shared traceStateDefault As String = "False"

    Friend Shared comPort As String ' Variables to hold the currrent device configuration
    Friend Shared traceState As Boolean

    Private connectedState As Boolean ' Private variable to hold the connected state
    Private utilities As Util ' Private variable to hold an ASCOM Utilities object
    Private astroUtilities As AstroUtils ' Private variable to hold an AstroUtils object to provide the Range method
    Private TL As TraceLogger ' Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
    Private m_fastreadout As Boolean = False
    Private m_ccdTemp As Double = 0
    Private m_ccdTargetTemp As Double = 0
    Private m_camTemp As Double = 0
    Private m_pwrTemp As Double = 0
    Private m_xbin As Integer
    Private m_ybin As Integer
    Private m_coolerOn As Boolean = True
    'm_ccdTemp, m_camTemp, m_pwrTemp
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        ReadProfile() ' Read device configuration from the ASCOM Profile store
        TL = New TraceLogger("", "PCO")
        TL.Enabled = traceState
        TL.LogMessage("Camera", "Starting initialisation")

        connectedState = False ' Initialise connected to false
        utilities = New Util() ' Initialise util object
        astroUtilities = New AstroUtils 'Initialise new astro utiliites object

        'TODO: Implement your additional construction here
        getCameraReady()
        TL.LogMessage("Camera", "Completed initialisation")
    End Sub
    Private Sub getCameraReady()
        'try to open camera
        Dim sizeL As Integer
        Dim dwWarn As Integer
        Dim dwErr As Integer
        Dim dwStatus As Integer

        hdriver = 0
        hdialog = 0
        errorCode = PCO_OpenCamera(hdriver, 0)
        If errorCode <> 0 Then
            MsgBox("Error detected: code: 0x" & Hex(Str(errorCode)))
            Return
        End If

        bmBildUsed = 0

        camDesc.wSize = Marshal.SizeOf(camDesc)

        errorCode = PCO_GetCameraDescription(hdriver, camDesc)
        Dim wPowerDownMode As Integer
        Dim dwTime As Integer
        Dim cFactor As Integer
        errorCode = PCO_GetConversionFactor(hdriver, cFactor)
        errorCode = PCO_SetConversionFactor(hdriver, 110)
        errorCode = PCO_GetConversionFactor(hdriver, cFactor)
        errorCode = PCO_SetPowerDownMode(hdriver, 0) 'auto
        'errorCode = PCO_GetUserPowerDownTime(hdriver, dwTime)
        'errorCode = PCO_SetUserPowerDownTime(hdriver, 10)
        If errorCode >= 0 Then
            'Text1.Text = " Camera openened, " + hdriver.ToString
        Else
            MsgBox(" Error opening camera0x" & Hex(Str(errorCode)), MsgBoxStyle.Critical)
            Exit Sub
        End If

        'minval.Value = 0
        'maxval.Value = 1 << (camDesc.wDynResDESC) - 1
        divide = 1 << (16 - camDesc.wDynResDESC)

        'errorCode = PCO_ResetSettingsToDefault(hdriver)

        '  errorCode = PCO_ArmCamera(hdriver)


        dwWarn = 0
        dwErr = 0
        dwStatus = 0
        errorCode = PCO_GetCameraHealthStatus(hdriver, dwWarn, dwErr, dwStatus)
        If dwErr <> 0 Then
            ' tbStatus.Text = "Camera status error 0x" & Hex(Str(dwErr)) & " please switch off camera"
        End If

        'sensor
        errorCode = PCO_GetSensorFormat(hdriver, PCO.Camera.Sensor.format_Renamed)
        If errorCode < 0 Then
            ' tbStatus.Text = " Error while retrieving sensor format 0x" & Hex(Str(errorCode))
        End If

        errorCode = PCO_GetSizes(hdriver, PCO.Camera.Sensor.Resolution.xAct, PCO.Camera.Sensor.Resolution.yAct, PCO.Camera.Sensor.Resolution.xMax, PCO.Camera.Sensor.Resolution.yMax)

        If errorCode < 0 Then
            ' tbStatus.Text = " Error while retrieving sensor sizes 0x" & Hex(Str(errorCode))
        End If


        errorCode = PCO_GetROI(hdriver, PCO.Camera.Sensor.ROI.x0, PCO.Camera.Sensor.ROI.y0, PCO.Camera.Sensor.ROI.X1, PCO.Camera.Sensor.ROI.Y1)

        If errorCode < 0 Then
            ' tbStatus.Text = " Error while retrieving roi 0x" & Hex(Str(errorCode))
        End If

        iXres = PCO.Camera.Sensor.Resolution.xAct
        iYres = PCO.Camera.Sensor.Resolution.yAct

        'errorCode = PCO_CamLinkSetImageParameters(hdriver, iXres, iYres) 'Mandatory for Cameralink and GigE

        '' Don't care for all other interfaces, so leave it intact here.
        'If errorCode < 0 Then
        '    ' tbStatus.Text = " Error while setting CamLinkImageParameters" & Hex(Str(errorCode))
        'End If

        sizeL = CDbl(iXres) * CDbl(iYres) * 2
        nBuf = -1
        errorCode = PCO_AllocateBuffer(hdriver, nBuf, sizeL, pwbuf, hevent)
        'pwbuf already holds the address of the buffer

        If errorCode = 0 Then
            '  tbStatus.Text = " Opened; buffer address: 0x" & Hex(pwbuf)
        Else
            'tbStatus.Text = " Buffer allocation error 0x" & Hex(Str(errorCode))
        End If

        'Dim ret As Integer = PCO_OpenDialogCam(hdialog, hdriver, Me.Handle, 0, 0, 0, Me.Right, Me.Top, "Camera Settings")
        errorCode = PCO_SetPixelRate(hdriver, 10000000)
        errorCode = PCO_SetFPSExposureMode(hdriver, 0, 0)
        'slowest clock speed


    End Sub
    '
    ' PUBLIC COM INTERFACE ICameraV2 IMPLEMENTATION
    '

#Region "Common properties and methods"
    ''' <summary>
    ''' Displays the Setup Dialog form.
    ''' If the user clicks the OK button to dismiss the form, then
    ''' the new settings are saved, otherwise the old values are reloaded.
    ''' THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
    ''' </summary>
    Public Sub SetupDialog() Implements ICameraV2.SetupDialog
        ' consider only showing the setup dialog if not connected
        ' or call a different dialog if connected
        If IsConnected Then
            System.Windows.Forms.MessageBox.Show("Already connected, just press OK")
        End If

        Using F As SetupDialogForm = New SetupDialogForm()
            Dim result As System.Windows.Forms.DialogResult = F.ShowDialog()
            If result = DialogResult.OK Then
                WriteProfile() ' Persist device configuration values to the ASCOM Profile store
            End If
        End Using
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements ICameraV2.SupportedActions
        Get
            TL.LogMessage("SupportedActions Get", "Returning empty arraylist")
            Return New ArrayList()
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements ICameraV2.Action
        Throw New ActionNotImplementedException("Action " & ActionName & " is not supported by this driver")
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements ICameraV2.CommandBlind
        CheckConnected("CommandBlind")
        ' Call CommandString and return as soon as it finishes
        Me.CommandString(Command, Raw)
        ' or
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements ICameraV2.CommandBool
        CheckConnected("CommandBool")
        Dim ret As String = CommandString(Command, Raw)
        ' TODO decode the return string and return true or false
        ' or
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements ICameraV2.CommandString
        CheckConnected("CommandString")
        ' it's a good idea to put all the low level communication with the device here,
        ' then all communication calls this function
        ' you need something to ensure that only one command is in progress at a time
        Throw New MethodNotImplementedException("CommandString")
    End Function

    Public Property Connected() As Boolean Implements ICameraV2.Connected
        Get
            TL.LogMessage("Connected Get", IsConnected.ToString())
            Return IsConnected
        End Get
        Set(value As Boolean)
            TL.LogMessage("Connected Set", value.ToString())
            If value = IsConnected Then
                Return
            End If

            If value Then
                connectedState = True
                TL.LogMessage("Connected Set", "Connecting to port " + comPort)
                ' TODO connect to the device
            Else
                connectedState = False
                TL.LogMessage("Connected Set", "Disconnecting from port " + comPort)
                ' TODO disconnect from the device
            End If
        End Set
    End Property

    Public ReadOnly Property Description As String Implements ICameraV2.Description
        Get
            ' this pattern seems to be needed to allow a public property to return a private field
            Dim d As String = driverDescription
            TL.LogMessage("Description Get", d)
            Return d
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements ICameraV2.DriverInfo
        Get
            Dim m_version As Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
            ' TODO customise this driver description
            Dim s_driverInfo As String = "Information about the driver itself. Version: " + m_version.Major.ToString() + "." + m_version.Minor.ToString()
            TL.LogMessage("DriverInfo Get", s_driverInfo)
            Return s_driverInfo
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements ICameraV2.DriverVersion
        Get
            ' Get our own assembly and report its version number
            TL.LogMessage("DriverVersion Get", Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2))
            Return Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2)
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements ICameraV2.InterfaceVersion
        Get
            TL.LogMessage("InterfaceVersion Get", "2")
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name As String Implements ICameraV2.Name
        Get
            Dim s_name As String = "PCO cam driver"
            TL.LogMessage("Name Get", s_name)
            Return s_name
        End Get
    End Property

    Public Sub Dispose() Implements ICameraV2.Dispose
        ' Clean up the tracelogger and util objects
        TL.Enabled = False
        TL.Dispose()
        TL = Nothing
        utilities.Dispose()
        utilities = Nothing
        astroUtilities.Dispose()
        astroUtilities = Nothing
    End Sub

#End Region

#Region "ICamera Implementation"

    Private Const ccdWidth As Integer = 2048 ' Constants to define the ccd pixel dimenstions
    Private Const ccdHeight As Integer = 2048
    Private Const pixelSize As Double = 7.4 ' Constant for the pixel physical dimension

    Private cameraNumX As Integer = ccdWidth ' Initialise variables to hold values required for functionality tested by Conform
    Private cameraNumY As Integer = ccdHeight
    Private cameraStartX As Integer = 0
    Private cameraStartY As Integer = 0
    Private exposureStart As DateTime = DateTime.MinValue
    Private cameraLastExposureDuration As Double = 0.0
    Private cameraImageReady As Boolean = False
    Private cameraImageArray As Integer(,)
    Private cameraImageArrayVariant As Object(,)
    Private exposing As Boolean

    Public Sub AbortExposure() Implements ICameraV2.AbortExposure
        TL.LogMessage("AbortExposure", "Not implemented")
        exposing = False
    End Sub

    Public ReadOnly Property BayerOffsetX() As Short Implements ICameraV2.BayerOffsetX
        Get
            TL.LogMessage("BayerOffsetX Get", "Not implemented")
            Throw New PropertyNotImplementedException("BayerOffsetX", False)
        End Get
    End Property

    Public ReadOnly Property BayerOffsetY() As Short Implements ICameraV2.BayerOffsetY
        Get
            TL.LogMessage("BayerOffsetY Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("BayerOffsetY", False)
        End Get
    End Property

    Public Property BinX() As Short Implements ICameraV2.BinX
        Get
            TL.LogMessage("BinX Get", "1")
            Return m_xbin
        End Get
        Set(value As Short)
            m_xbin = value
            'errorCode = PCO_GetBinning(hdriver, m_xbin, m_ybin)
            errorCode = PCO_SetBinning(hdriver, value, value)
            errorCode = PCO_SetROI(hdriver, 1, 1, ccdWidth / value, ccdHeight / value)

            'iXres = ccdWidth / value
            ' iYres = ccdHeight / value

            Debug.WriteLine("xAct is now {0}", PCO.Camera.Sensor.Resolution.xAct)
            Debug.WriteLine("yAct is now {0}", PCO.Camera.Sensor.Resolution.yAct)
        End Set
    End Property

    Public Property BinY() As Short Implements ICameraV2.BinY
        Get
            TL.LogMessage("BinX Get", "1")
            Return m_ybin
        End Get
        Set(value As Short)
            m_ybin = value
            'errorCode = PCO_GetBinning(hdriver, m_xbin, m_ybin)
            errorCode = PCO_SetBinning(hdriver, value, value)
            errorCode = PCO_SetROI(hdriver, 1, 1, ccdWidth / value, ccdHeight / value)

            PCO.Camera.Sensor.Resolution.xAct = ccdWidth / value
            PCO.Camera.Sensor.Resolution.yAct = ccdHeight / value

        End Set
    End Property

    Public ReadOnly Property CCDTemperature() As Double Implements ICameraV2.CCDTemperature
        Get

            errorCode = PCO_GetTemperature(hdriver, m_ccdTemp, m_camTemp, m_pwrTemp)
            Debug.WriteLine("ccd temp is {0}", m_ccdTemp / 10)
            Debug.WriteLine("cam temp is {0}", m_camTemp / 10)
            Debug.WriteLine("pwr temp is {0}", m_pwrTemp / 10)
            Return m_ccdTemp / 10
        End Get
    End Property

    Public ReadOnly Property CameraState() As CameraStates Implements ICameraV2.CameraState
        Get
            TL.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString())
            Return CameraStates.cameraIdle
        End Get
    End Property

    Public ReadOnly Property CameraXSize() As Integer Implements ICameraV2.CameraXSize
        Get
            Return iXres
        End Get
    End Property

    Public ReadOnly Property CameraYSize() As Integer Implements ICameraV2.CameraYSize
        Get
            Return iYres
        End Get
    End Property

    Public ReadOnly Property CanAbortExposure() As Boolean Implements ICameraV2.CanAbortExposure
        Get
            TL.LogMessage("CanAbortExposure Get", False.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanAsymmetricBin() As Boolean Implements ICameraV2.CanAsymmetricBin
        Get
            TL.LogMessage("CanAsymmetricBin Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanFastReadout() As Boolean Implements ICameraV2.CanFastReadout
        Get
            TL.LogMessage("CanFastReadout Get", False.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanGetCoolerPower() As Boolean Implements ICameraV2.CanGetCoolerPower
        Get
            TL.LogMessage("CanGetCoolerPower Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanPulseGuide() As Boolean Implements ICameraV2.CanPulseGuide
        Get
            TL.LogMessage("CanPulseGuide Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetCCDTemperature() As Boolean Implements ICameraV2.CanSetCCDTemperature
        Get
            TL.LogMessage("CanSetCCDTemperature Get", False.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanStopExposure() As Boolean Implements ICameraV2.CanStopExposure
        Get
            TL.LogMessage("CanStopExposure Get", False.ToString())
            Return True
        End Get
    End Property

    Public Property CoolerOn() As Boolean Implements ICameraV2.CoolerOn
        Get
            Return m_coolerOn
        End Get
        Set(value As Boolean)
            m_coolerOn = value
        End Set
    End Property

    Public ReadOnly Property CoolerPower() As Double Implements ICameraV2.CoolerPower
        Get
            TL.LogMessage("AbortExposure Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("CoolerPower", False)
        End Get
    End Property

    Public ReadOnly Property ElectronsPerADU() As Double Implements ICameraV2.ElectronsPerADU
        Get
            TL.LogMessage("ElectronsPerADU Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ElectronsPerADU", False)
        End Get
    End Property

    Public ReadOnly Property ExposureMax() As Double Implements ICameraV2.ExposureMax
        Get
            TL.LogMessage("ExposureMax Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ExposureMax", False)
        End Get
    End Property

    Public ReadOnly Property ExposureMin() As Double Implements ICameraV2.ExposureMin
        Get
            TL.LogMessage("ExposureMin Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ExposureMin", False)
        End Get
    End Property

    Public ReadOnly Property ExposureResolution() As Double Implements ICameraV2.ExposureResolution
        Get
            TL.LogMessage("ExposureResolution Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ExposureResolution", False)
        End Get
    End Property

    Public Property FastReadout() As Boolean Implements ICameraV2.FastReadout
        Get

            Return m_fastreadout
        End Get
        Set(value As Boolean)
            m_fastreadout = value
            If m_fastreadout Then
                errorCode = PCO_SetPixelRate(hdriver, 40000000)
            Else
                errorCode = PCO_SetPixelRate(hdriver, 10000000)
            End If

        End Set
    End Property

    Public ReadOnly Property FullWellCapacity() As Double Implements ICameraV2.FullWellCapacity
        Get
            TL.LogMessage("FullWellCapacity Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("FullWellCapacity", False)
        End Get
    End Property

    Public Property Gain() As Short Implements ICameraV2.Gain
        Get
            TL.LogMessage("Gain Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Gain", False)
        End Get
        Set(value As Short)
            TL.LogMessage("Gain Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Gain", True)
        End Set
    End Property

    Public ReadOnly Property GainMax() As Short Implements ICameraV2.GainMax
        Get
            TL.LogMessage("GainMax Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("GainMax", False)
        End Get
    End Property

    Public ReadOnly Property GainMin() As Short Implements ICameraV2.GainMin
        Get
            TL.LogMessage("GainMin Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("GainMin", False)
        End Get
    End Property

    Public ReadOnly Property Gains() As ArrayList Implements ICameraV2.Gains
        Get
            TL.LogMessage("Gains Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Gains", False)
        End Get
    End Property

    Public ReadOnly Property HasShutter() As Boolean Implements ICameraV2.HasShutter
        Get
            TL.LogMessage("HasShutter Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property HeatSinkTemperature() As Double Implements ICameraV2.HeatSinkTemperature
        Get
            TL.LogMessage("HeatSinkTemperature Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("HeatSinkTemperature", False)
        End Get
    End Property

    Public ReadOnly Property ImageArray() As Object Implements ICameraV2.ImageArray
        Get
            If (cameraImageReady) Then
                '    TL.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!")
                '    Throw New ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!")
                'Else


                Return cameraImageArray
            End If

        End Get
    End Property

    Public ReadOnly Property ImageArrayVariant() As Object Implements ICameraV2.ImageArrayVariant
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!")
            End If


            For i As Integer = 0 To cameraImageArray.GetLength(1) - 1
                For j As Integer = 0 To cameraImageArray.GetLength(0) - 1
                    cameraImageArrayVariant(j, i) = cameraImageArray(j, i)
                Next
            Next

            Return cameraImageArrayVariant
        End Get
    End Property

    Public ReadOnly Property ImageReady() As Boolean Implements ICameraV2.ImageReady
        Get
            TL.LogMessage("ImageReady Get", cameraImageReady.ToString())
            Return cameraImageReady
        End Get
    End Property

    Public ReadOnly Property IsPulseGuiding() As Boolean Implements ICameraV2.IsPulseGuiding
        Get
            TL.LogMessage("IsPulseGuiding Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("IsPulseGuiding", False)
        End Get
    End Property

    Public ReadOnly Property LastExposureDuration() As Double Implements ICameraV2.LastExposureDuration
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("LastExposureDuration Get", "Throwing InvalidOperationException because of a call to LastExposureDuration before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to LastExposureDuration before the first image has been taken!")
            End If
            TL.LogMessage("LastExposureDuration Get", cameraLastExposureDuration.ToString())
            Return cameraLastExposureDuration
        End Get
    End Property

    Public ReadOnly Property LastExposureStartTime() As String Implements ICameraV2.LastExposureStartTime
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("LastExposureStartTime Get", "Throwing InvalidOperationException because of a call to LastExposureStartTime before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to LastExposureStartTime before the first image has been taken!")
            End If
            Dim exposureStartString As String = exposureStart.ToString("yyyy-MM-ddTHH:mm:ss")
            TL.LogMessage("LastExposureStartTime Get", exposureStartString.ToString())
            Return exposureStartString
        End Get
    End Property

    Public ReadOnly Property MaxADU() As Integer Implements ICameraV2.MaxADU
        Get
            TL.LogMessage("MaxADU Get", "20000")
            Return 20000
        End Get
    End Property

    Public ReadOnly Property MaxBinX() As Short Implements ICameraV2.MaxBinX
        Get
            TL.LogMessage("MaxBinX Get", "1")
            Return 2
        End Get
    End Property

    Public ReadOnly Property MaxBinY() As Short Implements ICameraV2.MaxBinY
        Get
            TL.LogMessage("MaxBinY Get", "1")
            Return 2
        End Get
    End Property

    Public Property NumX() As Integer Implements ICameraV2.NumX
        Get
            TL.LogMessage("NumX Get", cameraNumX.ToString())
            Return cameraNumX
        End Get
        Set(value As Integer)
            cameraNumX = value
            TL.LogMessage("NumX set", value.ToString())
        End Set
    End Property

    Public Property NumY() As Integer Implements ICameraV2.NumY
        Get
            TL.LogMessage("NumY Get", cameraNumY.ToString())
            Return cameraNumY
        End Get
        Set(value As Integer)
            cameraNumY = value
            TL.LogMessage("NumY set", value.ToString())
        End Set
    End Property

    Public ReadOnly Property PercentCompleted() As Short Implements ICameraV2.PercentCompleted
        Get
            TL.LogMessage("PercentCompleted Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("PercentCompleted", False)
        End Get
    End Property

    Public ReadOnly Property PixelSizeX() As Double Implements ICameraV2.PixelSizeX
        Get
            TL.LogMessage("PixelSizeX Get", pixelSize.ToString())
            Return pixelSize
        End Get
    End Property

    Public ReadOnly Property PixelSizeY() As Double Implements ICameraV2.PixelSizeY
        Get
            TL.LogMessage("PixelSizeY Get", pixelSize.ToString())
            Return pixelSize
        End Get
    End Property

    Public Sub PulseGuide(Direction As GuideDirections, Duration As Integer) Implements ICameraV2.PulseGuide
        TL.LogMessage("PulseGuide", "Not implemented - " & Direction.ToString)
        Throw New ASCOM.MethodNotImplementedException("Direction")
    End Sub

    Public Property ReadoutMode() As Short Implements ICameraV2.ReadoutMode
        Get
            TL.LogMessage("ReadoutMode Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", False)
        End Get
        Set(value As Short)
            TL.LogMessage("ReadoutMode Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", True)
        End Set
    End Property

    Public ReadOnly Property ReadoutModes() As ArrayList Implements ICameraV2.ReadoutModes
        Get
            TL.LogMessage("ReadoutModes Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("ReadoutModes", False)
        End Get
    End Property

    Public ReadOnly Property SensorName() As String Implements ICameraV2.SensorName
        Get
            TL.LogMessage("SensorName Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SensorName", False)
        End Get
    End Property

    Public ReadOnly Property SensorType() As SensorType Implements ICameraV2.SensorType
        Get
            TL.LogMessage("SensorType Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("SensorType", False)

        End Get
    End Property

    Public Property SetCCDTemperature() As Double Implements ICameraV2.SetCCDTemperature
        Get
            errorCode = PCO_GetCoolingSetpointTemperature(hdriver, m_ccdTargetTemp)
            Return m_ccdTargetTemp

        End Get
        Set(value As Double)
            errorCode = PCO_SetCoolingSetpointTemperature(hdriver, value)
            m_ccdTargetTemp = value

        End Set
    End Property

    Public Sub StartExposure(Duration As Double, Light As Boolean) Implements ICameraV2.StartExposure
        Dim sizeL As Integer
        If exposing Then
            Debug.WriteLine("already exposing-exit sub")
            Exit Sub
        End If
        Debug.WriteLine("start exposure for {0}", Duration)
        errorCode = PCO_ArmCamera(hdriver)


        '1 = nanoseconds
        '2 = milliseconds

        Dim units As Integer = 2


        If Duration = 0 Then
            Duration = 0.5 'fastest this camera can do (pco.2000)

            units = 0 'nano secconds

        End If

        errorCode = PCO_SetDelayExposureTime(hdriver, 0, Duration * 1000, 0, units)

        Debug.WriteLine("exposure set to {0} ms", Duration * 1000)

        'If (Duration < 0.0) Then Throw New InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards")
        'If (cameraNumX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraNumX.ToString(), ccdWidth.ToString())
        'If (cameraNumY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraNumY.ToString(), ccdHeight.ToString())
        'If (cameraStartX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraStartX.ToString(), ccdWidth.ToString())
        'If (cameraStartY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraStartY.ToString(), ccdHeight.ToString())
        exposing = True
        cameraLastExposureDuration = Duration
        exposureStart = DateTime.Now
        'System.Threading.Thread.Sleep(Duration * 1000) ' Sleep for the duration to simulate exposure 
        'TL.LogMessage("StartExposure", Duration.ToString() + " " + Light.ToString())
        'cameraImageReady = True

        Dim t As Thread
        t = New Thread(AddressOf grabImage)
        t.Start()

    End Sub

    Sub grabImage()

        Dim BpP As Object


        Dim seg As Short
        Dim dwFrst, dwlast As Object
        Dim sbuf As Short
        Dim dwStatusDll As Integer
        Dim dwStatusDrv As Integer
        Dim check As Integer
        Dim sizeL As Integer
        Const mask As Integer = &H8000 'assign 0x00008000 to mask
        '^-& is essential!! Otherwise VB converts this to an int.
        '  This will give 0xFFFF8000 to mask, which is not intended.



        Dim j As Integer

        Dim value As Integer


        cameraImageReady = False

        check = 0

        dwFrst = 0
        dwlast = 0
        BpP = 16
        sbuf = 0

        errorCode = PCO_ArmCamera(hdriver)

        errorCode = PCO_GetSizes(hdriver, PCO.Camera.Sensor.Resolution.xAct, PCO.Camera.Sensor.Resolution.yAct, PCO.Camera.Sensor.Resolution.xMax, PCO.Camera.Sensor.Resolution.yMax)


        If (iXres <> PCO.Camera.Sensor.Resolution.xAct) Or (iYres <> PCO.Camera.Sensor.Resolution.yAct) Then
            iXres = PCO.Camera.Sensor.Resolution.xAct
            iYres = PCO.Camera.Sensor.Resolution.yAct

            errorCode = PCO_CamLinkSetImageParameters(hdriver, iXres, iYres) 'Mandatory for Cameralink and GigE
            ' Don't care for all other interfaces, so leave it intact here.


            If nBuf >= 0 Then
                errorCode = PCO_FreeBuffer(hdriver, nBuf)
            End If

            sizeL = CDbl(iXres) * CDbl(iYres) * 2
            nBuf = -1
            errorCode = PCO_AllocateBuffer(hdriver, nBuf, sizeL, pwbuf, hevent)
            'pwbuf already holds the address of the buffer
        End If
        Dim camstate As Integer
        errorCode = PCO_ArmCamera(hdriver)
        Debug.WriteLine("arm camera code is {0}", errorCode)
        errorCode = PCO_GetRecordingState(hdriver, camstate)
        If camstate = 1 Then
            errorCode = PCO_SetRecordingState(hdriver, 0)
        End If
        'set timeout
        'Dim buf_in(3) As UInt32
        'buf_in(0) = cameraLastExposureDuration * 1000 + 2000
        'buf_in(1) = cameraLastExposureDuration * 1000 + 2000
        'buf_in(2) = cameraLastExposureDuration * 1000 + 2000

        'Dim buf_bytes(12) As Byte
        'Dim result1 As Byte() = BitConverter.GetBytes(buf_in(0))
        'Dim result2 As Byte() = BitConverter.GetBytes(buf_in(1))
        'Dim result3 As Byte() = BitConverter.GetBytes(buf_in(2))
        'buf_bytes(0) = result1(0)
        'buf_bytes(1) = result1(1)
        'buf_bytes(2) = result1(2)
        'buf_bytes(3) = result1(3)
        'buf_bytes(4) = result2(0)
        'buf_bytes(5) = result2(1)
        'buf_bytes(6) = result2(2)
        'buf_bytes(7) = result2(3)
        'buf_bytes(8) = result3(0)
        'buf_bytes(9) = result3(1)
        'buf_bytes(10) = result3(2)
        'buf_bytes(11) = result3(3)




        'Dim buf_ptr As IntPtr = Marshal.AllocHGlobal(12)
        'Marshal.Copy(buf_bytes, 0, buf_ptr, 12)

        'errorCode = PCO_SetTimeouts(hdriver, buf_ptr, buf_in.Length)

        errorCode = PCO_SetRecordingState(hdriver, 1)
        Debug.Print("setrecordingstate: {0}", Hex(Str(errorCode)))
        errorCode = PCO_AddBufferEx(hdriver, dwFrst, dwlast, sbuf, PCO.Camera.Sensor.Resolution.xAct, PCO.Camera.Sensor.Resolution.yAct, BpP)
        Debug.Print("addbuffer: {0}", Hex(Str(errorCode)))
        errorCode = PCO_GetActiveRamSegment(hdriver, seg)
        Debug.Print("segment is {0}", seg)
        ' errorCode = PCO_GetImageEx(hdriver, seg, dwFrst, dwlast, sbuf, PCO.Camera.Sensor.Resolution.xAct, PCO.Camera.Sensor.Resolution.yAct, BpP)


        'Debug.Print("get image: {0}", Hex(Str(errorCode)))
        'loopcount = 0
        'Do While Not (check) ' status of the dll must be checked or you use waitforsingleobject instead
        '    errorCode = PCO_GetBufferStatus(hdriver, sbuf, dwStatusDll, dwStatusDrv)
        '    check = Not ((dwStatusDll And mask) <> mask) ' event flag set?
        '    loopcount = loopcount + 1
        '    If loopcount > 600000 Then
        '        errorCode = -1
        '        Debug.Print("timeout")
        '        Exit Do
        '    End If
        'Loop

        'Dim stopWatch As Stopwatch = New Stopwatch()
        '    stopWatch.Start()
        'wait for image





        While exposing And Not (check)
            errorCode = PCO_GetBufferStatus(hdriver, sbuf, dwStatusDll, dwStatusDrv)
            check = Not ((dwStatusDll And mask) <> mask) ' event flag set?
            Thread.Sleep(500)
            Debug.Print("sleeping")
            Debug.Print("dwStatusDll {0}", dwStatusDll)
            Debug.Print("dwStatusDrv {0}", dwStatusDrv)
        End While

        errorCode = PCO_SetRecordingState(hdriver, 0)

        exposing = False
        ' stopWatch.[Stop]()
        If errorCode = 0 Then
            ReDim b(iXres * iYres * 2)

            'Looks easy, but this took some time to work...
            Marshal.Copy(pwbuf, b, 0, iXres * iYres * 2)
            ReDim cameraImageArray(iXres - 1, iYres - 1)
            j = 0
            For x = 0 To iXres - 1
                For y = 0 To iYres - 1
                    value = b(j + 1) * 256 + b(j)
                    value = value >> 2

                    cameraImageArray(y, x) = value
                    j = j + 2
                Next
            Next
        End If
        cameraImageReady = True
        Debug.WriteLine("image is ready")

    End Sub

    Public Property StartX() As Integer Implements ICameraV2.StartX
        Get
            TL.LogMessage("StartX Get", cameraStartX.ToString())
            Return cameraStartX
        End Get
        Set(value As Integer)
            cameraStartX = value
            TL.LogMessage("StartX set", value.ToString())
        End Set
    End Property

    Public Property StartY() As Integer Implements ICameraV2.StartY
        Get
            TL.LogMessage("StartY Get", cameraStartY.ToString())
            Return cameraStartY
        End Get
        Set(value As Integer)
            cameraStartY = value
            TL.LogMessage("StartY set", value.ToString())
        End Set
    End Property

    Public Sub StopExposure() Implements ICameraV2.StopExposure
        TL.LogMessage("StopExposure", "Not implemented")
        Throw New MethodNotImplementedException("StopExposure")
    End Sub

#End Region

#Region "Private properties and methods"
    ' here are some useful properties and methods that can be used as required
    ' to help with

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Using P As New Profile() With {.DeviceType = "Camera"}
            If bRegister Then
                P.Register(driverID, driverDescription)
            Else
                P.Unregister(driverID)
            End If
        End Using

    End Sub

    <ComRegisterFunction()>
    Public Shared Sub RegisterASCOM(ByVal T As Type)

        RegUnregASCOM(True)

    End Sub

    <ComUnregisterFunction()>
    Public Shared Sub UnregisterASCOM(ByVal T As Type)

        RegUnregASCOM(False)

    End Sub

#End Region

    ''' <summary>
    ''' Returns true if there is a valid connection to the driver hardware
    ''' </summary>
    Private ReadOnly Property IsConnected As Boolean
        Get
            ' TODO check that the driver hardware connection exists and is connected to the hardware
            Return connectedState
        End Get
    End Property

    ''' <summary>
    ''' Use this function to throw an exception if we aren't connected to the hardware
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub CheckConnected(ByVal message As String)
        If Not IsConnected Then
            Throw New NotConnectedException(message)
        End If
    End Sub

    ''' <summary>
    ''' Read the device configuration from the ASCOM Profile store
    ''' </summary>
    Friend Sub ReadProfile()
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Camera"
            traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, String.Empty, traceStateDefault))
            comPort = driverProfile.GetValue(driverID, comPortProfileName, String.Empty, comPortDefault)
        End Using
    End Sub

    ''' <summary>
    ''' Write the device configuration to the  ASCOM  Profile store
    ''' </summary>
    Friend Sub WriteProfile()
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Camera"
            driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString())
            driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString())
        End Using

    End Sub

#End Region

End Class
