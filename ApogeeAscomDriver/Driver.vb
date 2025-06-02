'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Camera driver for Apogee
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
' Your driver's ID is ASCOM.Apogee.Camera
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

<Guid("f7ea5b9b-68e4-4d46-a21e-5f1f911c827c")>
<ClassInterface(ClassInterfaceType.None)>
Public Class Camera

    ' The Guid attribute sets the CLSID for ASCOM.Apogee.Camera
    ' The ClassInterface/None addribute prevents an empty interface called
    ' _Apogee from being created and used as the [default] interface

    ' TODO Replace the not implemented exceptions with code to implement the function or
    ' throw the appropriate ASCOM exception.
    '
    Implements ICameraV2

    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Friend Shared driverID As String = "ASCOM.Apogee.Camera"
    Private Shared driverDescription As String = "Apogee Camera"

    Friend Shared comPortProfileName As String = "COM Port" 'Constants used for Profile persistence
    Friend Shared traceStateProfileName As String = "Trace Level"
    Friend Shared comPortDefault As String = "COM1"
    Friend Shared traceStateDefault As String = "False"

    Friend Shared comPort As String ' Variables to hold the currrent device configuration
    Friend Shared traceState As Boolean



    Friend Shared xStartProfileName As String = "ROIxStart" '
    Friend Shared yStartProfileName As String = "ROIyStart" '
    Friend Shared xWidthProfileName As String = "ROIxWidth" '
    Friend Shared yHeightProfileName As String = "ROIyHeight" '
    Friend Shared useROIProfileName As String = "useROI" '
    Friend Shared useROI As Boolean = False
    Friend Shared xStart As Integer = "0" '
    Friend Shared xWidth As Integer = "4096" '
    Friend Shared yStart As Integer = "0" '
    Friend Shared yHeight As Integer = "4096" '

    Private connectedState As Boolean ' Private variable to hold the connected state
    Private utilities As Util ' Private variable to hold an ASCOM Utilities object
    Private astroUtilities As AstroUtils ' Private variable to hold an AstroUtils object to provide the Range method
    Private TL As TraceLogger ' Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
    Friend Shared myCam As ApogeeCam
    Private _readoutModes = New ArrayList(2)
    Private t As Threading.Thread
    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()
        initCamera()
    End Sub
    Private Sub initCamera()
        Debug.Print("enter new constructor of driver")
        ReadProfile() ' Read device configuration from the ASCOM Profile store
        TL = New TraceLogger("", "Apogee")
        TL.Enabled = traceState
        TL.LogMessage("Camera", "Starting initialisation")

        connectedState = False ' Initialise connected to false
        utilities = New Util() ' Initialise util object
        astroUtilities = New AstroUtils 'Initialise new astro utiliites object

        'TODO: Implement your additional construction here

        _readoutModes.add("slow")
        _readoutModes.add("fast")
        TL.LogMessage("Camera", "Completed initialisation")

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
        'If IsConnected Then
        '    System.Windows.Forms.MessageBox.Show("Already connected, just press OK")
        'End If

        Using F As SetupDialogForm = New SetupDialogForm()
            If IsConnected Then F.c = ApogeeCam.c

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

                TL.LogMessage("Connected Set", "Connecting to port " + comPort)

                myCam = New ApogeeCam()
                connectedState = True
                If useROI Then
                    myCam.c.RoiStartX = Camera.xStart
                    myCam.c.RoiStartY = Camera.yStart
                    myCam.c.RoiPixelsH = Camera.xWidth
                    myCam.c.RoiPixelsV = Camera.yHeight
                    myCam.ccdWidth = Camera.xWidth
                    myCam.ccdHeight = Camera.yHeight
                End If




            Else
                connectedState = False
                TL.LogMessage("Connected Set", "Disconnecting from port " + comPort)
                myCam.c.Close()
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
            Dim s_name As String = "Short driver name - please customise"
            TL.LogMessage("Name Get", s_name)
            Return s_name
        End Get
    End Property

    Public Sub Dispose() Implements ICameraV2.Dispose
        ' Clean up the tracelogger and util objects
        TL.Enabled = False
        connectedState = False
        'TL.Dispose()
        'TL = Nothing
        utilities.Dispose()
        utilities = Nothing
        astroUtilities.Dispose()
        astroUtilities = Nothing
    End Sub

#End Region

#Region "ICamera Implementation"

    'Private Const ccdWidth As Integer = 2184 '2048 ' Constants to define the ccd pixel dimenstions
    'Private Const ccdHeight As Integer = 1472 '2048
    'rivate Const pixelSize As Double = 6.8 ' Constant for the pixel physical dimension

    Private cameraNumX As Integer = 0 ' Initialise variables to hold values required for functionality tested by Conform
    Private cameraNumY As Integer = 0
    Private cameraStartX As Integer = 0
    Private cameraStartY As Integer = 0
    Private exposureStart As DateTime = DateTime.MinValue
    Private cameraLastExposureDuration As Double = 0.0
    Private cameraImageReady As Boolean = False
    Private cameraImageArray As Integer(,)
    Private cameraImageArrayVariant As Object(,)

    Public Sub AbortExposure() Implements ICameraV2.AbortExposure
        TL.LogMessage("AbortExposure", "Not implemented")
        myCam.c.StopExposure(False)
    End Sub

    Public ReadOnly Property BayerOffsetX() As Short Implements ICameraV2.BayerOffsetX
        Get
            TL.LogMessage("BayerOffsetX Get", "Not implemented")
            ' Throw New PropertyNotImplementedException("BayerOffsetX", False)
        End Get
    End Property

    Public ReadOnly Property BayerOffsetY() As Short Implements ICameraV2.BayerOffsetY
        Get
            TL.LogMessage("BayerOffsetY Get", "Not implemented")
            ' Throw New ASCOM.PropertyNotImplementedException("BayerOffsetY", False)
        End Get
    End Property

    Public Property BinX() As Short Implements ICameraV2.BinX
        Get
            TL.LogMessage("BinX Get", "1")
            Return myCam.c.RoiBinningH

        End Get
        Set(value As Short)
            TL.LogMessage("BinX Set", value.ToString())
            myCam.c.RoiBinningH = value
            myCam.c.RoiPixelsH = myCam.ccdWidth / value
            Debug.Print("roiPixelsH " & myCam.c.RoiPixelsH)

        End Set

    End Property

    Public Property BinY() As Short Implements ICameraV2.BinY
        Get
            TL.LogMessage("BinY Get", "1")
            Return myCam.c.RoiBinningV
        End Get
        Set(value As Short)
            TL.LogMessage("BinY Set", value.ToString())
            myCam.c.RoiBinningV = value
            myCam.c.RoiPixelsV = myCam.ccdHeight / value


        End Set
    End Property

    Public ReadOnly Property CCDTemperature() As Double Implements ICameraV2.CCDTemperature
        Get
            If IsConnected Then
                Return myCam.c.TempCCD
            End If
        End Get
    End Property

    Public ReadOnly Property CameraState() As CameraStates Implements ICameraV2.CameraState
        Get
            If IsConnected Then
                Return myCam.c.CameraMode
            End If

        End Get
    End Property

    Public ReadOnly Property CameraXSize() As Integer Implements ICameraV2.CameraXSize
        Get
            If IsConnected Then
                Return myCam.c.ImagingColumns
            End If
        End Get
    End Property

    Public ReadOnly Property CameraYSize() As Integer Implements ICameraV2.CameraYSize
        Get
            If IsConnected Then
                Return myCam.c.ImagingRows
            End If
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
            Return False
        End Get
    End Property

    Public ReadOnly Property CanGetCoolerPower() As Boolean Implements ICameraV2.CanGetCoolerPower
        Get
            TL.LogMessage("CanGetCoolerPower Get", False.ToString())
            Return True
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
            TL.LogMessage("CoolerOn Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("CoolerOn", False)
            If IsConnected Then
                Return myCam.c.CoolerEnable
            End If

        End Get
        Set(value As Boolean)
            TL.LogMessage("CoolerOn Set", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("CoolerOn", True)
            If IsConnected Then
                myCam.c.CoolerEnable = value
            End If

        End Set
    End Property

    Public ReadOnly Property CoolerPower() As Double Implements ICameraV2.CoolerPower
        Get
            TL.LogMessage("AbortExposure Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("CoolerPower", False)
            If IsConnected Then
                Return myCam.c.CoolerDrive
            End If

        End Get
    End Property

    Public ReadOnly Property ElectronsPerADU() As Double Implements ICameraV2.ElectronsPerADU
        Get
            TL.LogMessage("ElectronsPerADU Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("ElectronsPerADU", False)

        End Get
    End Property

    Public ReadOnly Property ExposureMax() As Double Implements ICameraV2.ExposureMax
        Get
            TL.LogMessage("ExposureMax Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("ExposureMax", False)
            If IsConnected Then
                If Not myCam.c Is Nothing Then
                    Return myCam.c.MaxExposure
                End If
            End If
        End Get
    End Property

    Public ReadOnly Property ExposureMin() As Double Implements ICameraV2.ExposureMin
        Get
            TL.LogMessage("ExposureMin Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("ExposureMax", False)
            If IsConnected Then

                If Not myCam.c Is Nothing Then
                    Return myCam.c.MinExposure
                End If
            End If

        End Get
    End Property

    Public ReadOnly Property ExposureResolution() As Double Implements ICameraV2.ExposureResolution
        Get
            TL.LogMessage("ExposureResolution Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("ExposureResolution", False)
        End Get
    End Property

    Public Property FastReadout() As Boolean Implements ICameraV2.FastReadout
        Get
            TL.LogMessage("FastReadout Get", "Not implemented")
            Return myCam.c.FastSequence
        End Get
        Set(value As Boolean)
            TL.LogMessage("FastReadout Set", "Not implemented")
            myCam.c.FastSequence = value
        End Set
    End Property

    Public ReadOnly Property FullWellCapacity() As Double Implements ICameraV2.FullWellCapacity
        Get
            TL.LogMessage("FullWellCapacity Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("FullWellCapacity", False)
        End Get
    End Property

    Public Property Gain() As Short Implements ICameraV2.Gain
        Get
            TL.LogMessage("Gain Get", "Not implemented")
            Dim g As Short
            If IsConnected Then
                myCam.c.GetAdGain(g, 1, 1)
                Return g
            End If
            Return -1
        End Get
        Set(value As Short)
            TL.LogMessage("Gain Set", "Not implemented")
            'If IsConnected Then
            '    myCam.c.SetAdGain(value, 1, 1)
            'End If
        End Set
    End Property

    Public ReadOnly Property GainMax() As Short Implements ICameraV2.GainMax
        Get
            TL.LogMessage("GainMax Get", "Not implemented")
            ' Throw New ASCOM.PropertyNotImplementedException("GainMax", False)
        End Get
    End Property

    Public ReadOnly Property GainMin() As Short Implements ICameraV2.GainMin
        Get
            Return 0
            ' Throw New ASCOM.PropertyNotImplementedException("GainMin", False)
        End Get
    End Property

    Public ReadOnly Property Gains() As ArrayList Implements ICameraV2.Gains
        Get
            TL.LogMessage("Gains Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("Gains", False)
        End Get
    End Property

    Public ReadOnly Property HasShutter() As Boolean Implements ICameraV2.HasShutter
        Get
            TL.LogMessage("HasShutter Get", False.ToString())
            If IsConnected Then
                'Return myCam.c.SetA
            End If
        End Get
    End Property

    Public ReadOnly Property HeatSinkTemperature() As Double Implements ICameraV2.HeatSinkTemperature
        Get
            TL.LogMessage("HeatSinkTemperature Get", "Not implemented")
            If IsConnected Then
                Return myCam.c.TempHeatsink
            End If

        End Get
    End Property

    Public ReadOnly Property ImageArray() As Object Implements ICameraV2.ImageArray
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!")
            End If

            'ReDim cameraImageArray(cameraNumX - 1, cameraNumY - 1)
            Return cameraImageArray
        End Get
    End Property

    Public ReadOnly Property ImageArrayVariant() As Object Implements ICameraV2.ImageArrayVariant
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!")
            End If

            ReDim cameraImageArrayVariant(cameraNumX - 1, cameraNumY - 1)
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
            ' Throw New ASCOM.PropertyNotImplementedException("IsPulseGuiding", False)
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
            Try
                If IsConnected Then
                    Return myCam.c.MaxBinningH
                End If

            Catch
                Return 8
            End Try

        End Get
    End Property

    Public ReadOnly Property MaxBinY() As Short Implements ICameraV2.MaxBinY
        Get
            Try
                If IsConnected Then
                    Return myCam.c.MaxBinningV
                End If
            Catch
                Return 8
            End Try
        End Get
    End Property
    Public Property NumX() As Integer Implements ICameraV2.NumX
        Get
            TL.LogMessage("NumX Get", cameraNumX.ToString())

            cameraNumX = myCam.c.RoiPixelsH
            Return cameraNumX
        End Get
        Set(value As Integer)
            cameraNumX = value
            ' myCam.c.RoiPixelsH = value
            TL.LogMessage("NumX set", value.ToString())
        End Set
    End Property

    Public Property NumY() As Integer Implements ICameraV2.NumY
        Get
            TL.LogMessage("NumY Get", cameraNumY.ToString())
            cameraNumY = myCam.c.RoiPixelsV
            Return cameraNumY
        End Get
        Set(value As Integer)
            cameraNumY = value
            'myCam.c.RoiPixelsV = value
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
            Return myCam.c.PixelSizeX
        End Get
    End Property

    Public ReadOnly Property PixelSizeY() As Double Implements ICameraV2.PixelSizeY
        Get

            Return myCam.c.PixelSizeY
        End Get
    End Property

    Public Sub PulseGuide(Direction As GuideDirections, Duration As Integer) Implements ICameraV2.PulseGuide
        TL.LogMessage("PulseGuide", "Not implemented - " & Direction.ToString)
        ' Throw New ASCOM.MethodNotImplementedException("Direction")
    End Sub

    Public Property ReadoutMode() As Short Implements ICameraV2.ReadoutMode
        Get
            TL.LogMessage("ReadoutMode Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", False)
        End Get
        Set(value As Short)
            TL.LogMessage("ReadoutMode Set", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("ReadoutMode", True)
        End Set
    End Property

    Public ReadOnly Property ReadoutModes() As ArrayList Implements ICameraV2.ReadoutModes
        Get
            TL.LogMessage("ReadoutModes Get", "Not implemented")
            'Throw New ASCOM.PropertyNotImplementedException("ReadoutModes", False)
            Return _readoutModes
        End Get
    End Property

    Public ReadOnly Property SensorName() As String Implements ICameraV2.SensorName
        Get
            Return myCam.c.Sensor
        End Get
    End Property

    Public ReadOnly Property SensorType() As SensorType Implements ICameraV2.SensorType
        Get
            Return myCam.c.SensorTypeCCD
        End Get
    End Property

    Public Property SetCCDTemperature() As Double Implements ICameraV2.SetCCDTemperature
        Get
            Return myCam.c.CoolerSetPoint
        End Get
        Set(value As Double)
            myCam.c.CoolerSetPoint = value
        End Set
    End Property
    Private Sub Expose(params As Object)
        Dim Duration = params(0)
        Dim Light = params(1)
        myCam.Expose(Duration, Light)
        Debug.Print(myCam.c.ImagingStatus)
        Debug.Print("exposure finished")
        ' If myCam.c.ImagingStatus = APOGEELib.Apn_Status.Apn_Status_ImageReady Then
        Debug.Print(myCam.c.ImagingStatus)
        If Not myCam.imageData Is Nothing Then
            cameraImageArray = ConvertFrameToImageArray(myCam.imageData, myCam.c.RoiPixelsH, myCam.c.RoiPixelsV)
        End If

        Debug.Print("converted image")
        ' End If
        cameraImageReady = True
    End Sub
    Public Sub StartExposure(Duration As Double, Light As Boolean) Implements ICameraV2.StartExposure
        cameraImageReady = False
        If (Duration < 0.0) Then Throw New InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards")
        'If (cameraNumX > myCam.ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraNumX.ToString(), myCam.ccdWidth.ToString())
        'If (cameraNumY > myCam.ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraNumY.ToString(), myCam.ccdHeight.ToString())
        'If (cameraStartX > myCam.ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraStartX.ToString(), myCam.ccdWidth.ToString())
        'If (cameraStartY > myCam.ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraStartY.ToString(), myCam.ccdHeight.ToString())


        cameraLastExposureDuration = Duration
        exposureStart = DateTime.Now

        'startAltaExposure(Duration)
        t = New Threading.Thread(AddressOf Expose)
        Dim Parameters = New Object() {Duration, Light}

        t.Start(parameter:=Parameters)

        'Expose(Parameters)

        TL.LogMessage("StartExposure", Duration.ToString() + " " + Light.ToString())

    End Sub
    Private Shared Function ConvertFrameToImageArray(ByVal frame As UShort(,), w As Integer, h As Integer) As Integer(,)
        Dim imgArr As Integer(,)
        Dim pixelX As Integer = 0
        Dim pixelY As Integer = 0
        ReDim imgArr(w - 1, h - 1)
        If frame Is Nothing Then
            Debug.WriteLine("frame is nothing")
            Throw New ArgumentNullException("frame")
        End If
        Debug.Print("width is " & w)

        For y As Integer = 0 To CInt(h) - 1
            pixelX = 0
            For x As Integer = 0 To CInt(w) - 1
                imgArr(pixelX, y) = frame(x, y)

                pixelX = pixelX + 1

            Next

        Next

        Return imgArr



    End Function
    Public Property StartX() As Integer Implements ICameraV2.StartX
        Get
            TL.LogMessage("StartX Get", cameraStartX.ToString())
            Return myCam.c.RoiStartX
        End Get
        Set(value As Integer)
            ''myCam.c.RoiStartX = value
            TL.LogMessage("StartX set", value.ToString())
        End Set
    End Property

    Public Property StartY() As Integer Implements ICameraV2.StartY
        Get
            TL.LogMessage("StartY Get", cameraStartY.ToString())
            Return myCam.c.RoiStartY
        End Get
        Set(value As Integer)
            ''myCam.c.RoiStartY = value
            TL.LogMessage("StartY set", value.ToString())
        End Set
    End Property

    Public Sub StopExposure() Implements ICameraV2.StopExposure
        Debug.Print("stopping exposure")
        myCam.c.StopExposure(False)
        Debug.Print("stopped exposure")
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
            xStart = driverProfile.GetValue(driverID, xStartProfileName, String.Empty, 0)
            xWidth = driverProfile.GetValue(driverID, xWidthProfileName, String.Empty, 4096)
            yStart = driverProfile.GetValue(driverID, yStartProfileName, String.Empty, 0)
            yHeight = driverProfile.GetValue(driverID, yHeightProfileName, String.Empty, 4096)
            useROI = Convert.ToBoolean(driverProfile.GetValue(driverID, useROIProfileName, String.Empty, "False"))

        End Using
    End Sub

    ''' <summary>
    ''' Write the device configuration to the  ASCOM  Profile store
    ''' </summary>
    Friend Sub WriteProfile()
        Using driverProfile As New Profile()

            driverProfile.DeviceType = "Camera"
            driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString())
            If comPort IsNot Nothing Then
                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString())
            End If
            driverProfile.WriteValue(driverID, xStartProfileName, xStart.ToString())
            driverProfile.WriteValue(driverID, xWidthProfileName, xWidth.ToString())
            driverProfile.WriteValue(driverID, yStartProfileName, yStart.ToString())
            driverProfile.WriteValue(driverID, yHeightProfileName, yHeight.ToString())
            driverProfile.WriteValue(driverID, useROIProfileName, useROI.ToString())


        End Using

    End Sub

    Protected Overrides Sub Finalize()
        ' myCam.c.Close()
        '  MyBase.Finalize()
    End Sub

#End Region

End Class
