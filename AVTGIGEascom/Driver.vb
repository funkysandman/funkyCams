'tabs=4
' --------------------------------------------------------------------------------
' TODO fill in this information for your driver, then remove this line!
'
' ASCOM Camera driver for AVTGIGE
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
' Your driver's ID is ASCOM.AVTGIGE.Camera
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
Imports System.IO





Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports System.Drawing
Imports System.Drawing.Imaging
Imports AVT.VmbAPINET


<Guid("51caaf50-e955-4784-9822-826e2e2d9b68")>
<ClassInterface(ClassInterfaceType.None)>
Public Class Camera

    ' The Guid attribute sets the CLSID for ASCOM.AVTGIGE.Camera
    ' The ClassInterface/None addribute prevents an empty interface called
    ' _AVTGIGE from being created and used as the [default] interface

    ' TODO Replace the not implemented exceptions with code to implement the function or
    ' throw the appropriate ASCOM exception.
    '
    Implements ICameraV2

    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Friend Shared driverID As String = "ASCOM.AVTGIGE.Camera"
    Private Shared driverDescription As String = "AVTGIGE Camera"

    Friend Shared gigeCamIDProfileName As String = "Gige Camera" 'Constants used for Profile persistence
    Friend Shared traceStateProfileName As String = "Trace Level"
    Friend Shared gigeCamIDDefault As String
    Friend Shared traceStateDefault As String = "False"

    Friend Shared gigeCamID As String ' Variables to hold the currrent device configuration
    Friend Shared traceState As Boolean

    Private connectedState As Boolean ' Private variable to hold the connected state
    Private utilities As Util ' Private variable to hold an ASCOM Utilities object
    Private astroUtilities As AstroUtils ' Private variable to hold an AstroUtils object to provide the Range method
    Private TL As TraceLogger ' Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
    Private v As VimbaHelper
    Friend Shared m_exposureTime As Integer
    Friend Shared m_gain As Integer
    Friend Shared m_binning As Short
    Private camID As String
    Private f As AVT.VmbAPINET.Frame
    Friend Shared m_chosenPort As String
    Private serialPort As New IO.Ports.SerialPort
    Private isCoolerOn As Boolean = False
    Private setTemperature As Double
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        ReadProfile() ' Read device configuration from the ASCOM Profile store
        TL = New TraceLogger("", "AVTGIGE")
        TL.Enabled = traceState
        TL.LogMessage("Camera", "Starting initialisation")

        connectedState = False ' Initialise connected to false
        utilities = New Util() ' Initialise util object
        astroUtilities = New AstroUtils
        'TODO: Implement your additional construction here
        Dim cams As List(Of CameraInfo)
        v = New VimbaHelper
        v.Startup()

        SetupDialog(v.v)
        If gigeCamID = "" Then
            connectedState = False
        Else
            connectedState = True
        End If
        If connectedState Then
            If Not IsNothing(m_chosenPort) Then
                serialPort.PortName = m_chosenPort
                serialPort.Open()
            End If





            cams = v.CameraList

            For cam = 0 To cams.Count - 1
                If cams(cam).ID = gigeCamID Then
                    v.OpenCamera(cams(cam).ID)
                End If
            Next





            Try
                'v.m_Camera.Features("GVSPAdjustPacketSize").RunCommand()
                'While (Not v.m_Camera.Features("GVSPAdjustPacketSize").IsCommandDone())

                'End While
                'set properties

                Me.ccdWidth = v.m_Camera.Features("Width").IntValue
                Me.ccdHeight = v.m_Camera.Features("Height").IntValue
                Me.Gain = m_gain
                Me.ExposureTime = m_exposureTime 'microseconds
                Me.BinX = m_binning
                Me.BinY = m_binning


            Catch ex As Exception
                MsgBox(ex.Message)
            End Try



            pixelSize = 3.75 ' Constant for the pixel physical dimension

            TL.LogMessage("Camera", "Completed initialisation")
        End If

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
    Public Sub SetupDialog(v As Vimba)
        ' consider only showing the setup dialog if not connected
        ' or call a different dialog if connected
        If IsConnected Then
            System.Windows.Forms.MessageBox.Show("Already connected, just press OK")
        End If

        Using F As SetupDialogForm = New SetupDialogForm()
            F.v = v
            Dim result As System.Windows.Forms.DialogResult = F.ShowDialog()
            If result = DialogResult.OK Then
                WriteProfile() ' Persist device configuration values to the ASCOM Profile store
            End If
        End Using
    End Sub
    'Private Sub received_frame(sender As Object, args As FrameEventArgs)

    Private Sub StartExpose()
        'v.StartSingleFrameAcquisition(AddressOf Me.received_frame)
        v.StartContinuousFrameAcquisition(AddressOf Me.received_frame)
        myCameraState = CameraStates.cameraExposing


    End Sub
    Private Sub StopExpose()

        '  System.Threading.Thread.Sleep(m_exposureTime * 1000)

        v.StopContinuousImageAcquisition()
        myCameraState = CameraStates.cameraIdle

    End Sub
    Private Sub received_frame(f As Frame)
        Debug.WriteLine("received frame")
        Debug.WriteLine("frame status:" & f.ReceiveStatus)
        ' MsgBox("got a frame:" & Now)
        ReDim cameraImageArray(f.Width - 1, f.Height - 1)
        cameraImageArray = ConvertFrameToImageAray(f)
        cameraImageReady = True
        Debug.WriteLine("processed frame")
        ' v.m_Camera.AnnounceFrame(f)
        v.m_Camera.QueueFrame(f)
        'v.m_Camera.StartContinuousImageAcquisition(1)
        'v.m_Camera.StartCapture()
        'v.m_Camera.Features("AcquisitionStart").RunCommand()
        'Try
        '    v.m_Camera.EndCapture()
        '    MsgBox("got a frame:" & Now)
        'Catch ex As Exception
        '    ' MsgBox(ex.Message)
        'End Try
        'Me.v.m_Camera.Features("AcquisitionStop").RunCommand()
        'While (Not v.m_Camera.Features("AcquisitionStop").IsCommandDone())

        'End While
        'Try
        '    v.m_Camera.EndCapture()
        '    ' MsgBox("got a frame:" & Now)
        'Catch ex As Exception
        '    ' MsgBox(ex.Message)
        'End Try

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
                ' connectedState = True
                TL.LogMessage("Connected Set", "Connecting to port " + gigeCamID)
                ' TODO connect to the device
            Else
                connectedState = False
                v.m_Camera.Close()
                v.Shutdown()
                TL.LogMessage("Connected Set", "Disconnecting from port " + gigeCamID)
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
            Dim s_name As String = "Short driver name - please customise"
            TL.LogMessage("Name Get", s_name)
            Return s_name
        End Get
    End Property

    Public Sub Dispose() Implements ICameraV2.Dispose
        ' Clean up the tracelogger and util objects

        '  v.Shutdown()
        MsgBox("shutdown vimba")
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

    Private ccdWidth As Integer = 3380 ' Constants to define the ccd pixel dimenstions
    Private ccdHeight As Integer = 2704
    Private pixelSize As Double = 3.69 ' Constant for the pixel physical dimension
    Private myCameraState As CameraStates = CameraStates.cameraIdle
    Private cameraNumX As Integer = ccdWidth ' Initialise variables to hold values required for functionality tested by Conform
    Private cameraNumY As Integer = ccdHeight
    Private cameraStartX As Integer = 0
    Private cameraStartY As Integer = 0
    Private exposureStart As DateTime = DateTime.MinValue
    Private cameraLastExposureDuration As Double = 0.0
    Private cameraImageReady As Boolean = False
    Private cameraImageArray As Integer(,)
    Private cameraImageArrayVariant As Object(,)

    Public Sub AbortExposure() Implements ICameraV2.AbortExposure
        TL.LogMessage("AbortExposure", "Not implemented")
        Throw New MethodNotImplementedException("AbortExposure")
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
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("BinningHorizontal") Then
                f = v.m_Camera.Features("BinningHorizontal")
                Return f.IntValue
            End If
        End Get
        Set(value As Short)
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("BinningHorizontal") Then
                f = v.m_Camera.Features("BinningHorizontal")
                f.IntValue = Convert.ToSingle(value)
                v.m_Camera.Features("BinningHorizontal").IntValue = f.IntValue
            End If
        End Set
    End Property

    Public Property BinY() As Short Implements ICameraV2.BinY
        Get
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("BinningVertical") Then
                f = v.m_Camera.Features("BinningVertical")
                Return f.IntValue
            End If
        End Get
        Set(value As Short)
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("BinningVertical") Then
                f = v.m_Camera.Features("BinningVertical")
                f.IntValue = Convert.ToSingle(value)
                v.m_Camera.Features("BinningVertical").IntValue = f.IntValue
            End If
        End Set
    End Property

    Public ReadOnly Property CCDTemperature() As Double Implements ICameraV2.CCDTemperature
        Get
            TL.LogMessage("CCDTemperature Get", "Not implemented")
            Return v.m_Camera.Features("DeviceTemperature").FloatValue

        End Get
    End Property

    Public ReadOnly Property CameraState() As CameraStates Implements ICameraV2.CameraState
        Get
            TL.LogMessage("CameraState Get", CameraStates.cameraIdle.ToString())

            Return myCameraState

        End Get
    End Property

    Public ReadOnly Property CameraXSize() As Integer Implements ICameraV2.CameraXSize
        Get
            TL.LogMessage("CameraXSize Get", ccdWidth.ToString())
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("SensorWidth") Then
                f = v.m_Camera.Features("SensorWidth")
                Return f.IntValue
            End If
        End Get
    End Property

    Public ReadOnly Property CameraYSize() As Integer Implements ICameraV2.CameraYSize
        Get
            TL.LogMessage("CameraYSize Get", ccdHeight.ToString())
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("SensorHeight") Then
                f = v.m_Camera.Features("SensorHeight")
                Return f.IntValue
            End If
        End Get
    End Property

    Public ReadOnly Property CanAbortExposure() As Boolean Implements ICameraV2.CanAbortExposure
        Get
            TL.LogMessage("CanAbortExposure Get", False.ToString())
            Return False
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
            Return True 'because we have an arduino cooler controller
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
            Return isCoolerOn
        End Get
        Set(value As Boolean)
            TL.LogMessage("CoolerOn Set", "Not implemented")
            '            Throw New ASCOM.PropertyNotImplementedException("CoolerOn", True)

            'turn on the fan and tec 
            isCoolerOn = value
            Dim t As New Thread(AddressOf checkTemperature)
            t.Start()

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
            Return 600000000
        End Get
    End Property

    Public ReadOnly Property ExposureMin() As Double Implements ICameraV2.ExposureMin
        Get
            TL.LogMessage("ExposureMin Get", "Not implemented")
            Return 0.001
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
            TL.LogMessage("FastReadout Get", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("FastReadout", False)
        End Get
        Set(value As Boolean)
            TL.LogMessage("FastReadout Set", "Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("FastReadout", True)
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
            Return m_gain
        End Get
        Set(value As Short)
            m_gain = value
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("Gain") Then
                f = v.m_Camera.Features("Gain")
                f.FloatValue = Convert.ToSingle(value)
            End If
            If v.m_Camera.Features.ContainsName("GainRaw") Then
                f = v.m_Camera.Features("GainRaw")
                f.IntValue = Convert.ToSingle(value)
            End If


        End Set
    End Property
    Public Property Binning() As Short 'Implements ICameraV2
        Get
            Return m_binning
        End Get
        Set(value As Short)
            m_binning = value
            Dim f As Feature
            If v.m_Camera.Features.ContainsName("BinningHorizontal") Then
                f = v.m_Camera.Features("BinningHorizontal")
                f.FloatValue = Convert.ToSingle(value)
            End If
            If v.m_Camera.Features.ContainsName("BinningVertical") Then
                f = v.m_Camera.Features("BinningVertical")
                f.IntValue = Convert.ToSingle(value)
            End If


        End Set
    End Property
    Public Property ExposureTime() As Long
        Get
            Return m_exposureTime
        End Get
        Set(value As Long)
            Try
                m_exposureTime = value 'stored as seconds
                v.m_Camera.Features("ExposureTimeAbs").FloatValue = Convert.ToDouble(m_exposureTime * 1000000) 'uses microseconds
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

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
            If (Not cameraImageReady) Then
                Debug.WriteLine("image array not ready")
                TL.LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!")
            End If
            Debug.WriteLine("reading image array")

            Return cameraImageArray
        End Get
    End Property

    Public ReadOnly Property ImageArrayVariant() As Object Implements ICameraV2.ImageArrayVariant
        Get
            If (Not cameraImageReady) Then
                TL.LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!")
                Throw New ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!")
            End If

            ReDim cameraImageArrayVariant(cameraNumX * 3 - 1, cameraNumY - 1)
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
            Return 8
        End Get
    End Property

    Public ReadOnly Property MaxBinY() As Short Implements ICameraV2.MaxBinY
        Get
            TL.LogMessage("MaxBinY Get", "1")
            Return 8
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
            Return "Funkycam"
        End Get
    End Property

    Public ReadOnly Property SensorType() As SensorType Implements ICameraV2.SensorType
        Get
            TL.LogMessage("SensorType Get", "Not implemented")
            ' Throw New ASCOM.PropertyNotImplementedException("SensorType", False)
            Return SensorType.Monochrome
        End Get
    End Property

    Public Property SetCCDTemperature() As Double Implements ICameraV2.SetCCDTemperature
        Get
            TL.LogMessage("SetCCDTemperature Get", "Not implemented")
            ' Throw New ASCOM.PropertyNotImplementedException("SetCCDTemperature", False)
        End Get
        Set(value As Double)
            setTemperature = value
            If Me.CCDTemperature > value Then
                'turn on cooler


                serialPort.Write("A")

            Else
                serialPort.Write("B")

            End If

        End Set
    End Property

    Private Sub checkTemperature()
        While (CoolerOn)
            If Me.CCDTemperature > setTemperature Then
                'turn on cooler

                If serialPort.IsOpen Then
                    serialPort.Write("A")
                End If

            Else
                If serialPort.IsOpen Then
                    serialPort.Write("B")
                End If
            End If
            Thread.Sleep(5000)
        End While
    End Sub




    Public Sub StartExposure(Duration As Double, Light As Boolean) Implements ICameraV2.StartExposure
        If (Duration < 0.0) Then Throw New InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards")
        If (cameraNumX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraNumX.ToString(), ccdWidth.ToString())
        If (cameraNumY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraNumY.ToString(), ccdHeight.ToString())
        If (cameraStartX > ccdWidth) Then Throw New InvalidValueException("StartExposure", cameraStartX.ToString(), ccdWidth.ToString())
        If (cameraStartY > ccdHeight) Then Throw New InvalidValueException("StartExposure", cameraStartY.ToString(), ccdHeight.ToString())

        If Not Me.CameraState = CameraStates.cameraExposing Then
            ExposureTime = Duration
            'MsgBox("start:" & Now)
            cameraLastExposureDuration = Duration
            exposureStart = DateTime.Now
            'System.Threading.Thread.Sleep(Duration * 1000) ' Sleep for the duration to simulate exposure 
            'TL.LogMessage("StartExposure", Duration.ToString() + " " + Light.ToString())
            'Dim f As Frame
            'Dim i As Image

            'Try
            '    v.m_Camera.AcquireSingleImage(f, 10000)

            '    'i = ConvertFrame(f)
            '    'put image into imageArray
            '    ReDim cameraImageArray(f.Width - 1, f.Height - 1)
            '    cameraImageArray = ConvertFrameToImageAray(f)

            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try

            'cameraImageReady = True
            'Debug.Print("grabbing")

            cameraImageReady = False
            'Dim threadBegin As New Thread(AddressOf Me.StartExpose)
            'threadBegin.Start()
            StartExpose()
            'Dim threadEnd As New Thread(AddressOf Me.StopExpose)
            'threadEnd.Start()
            ' myCameraState = CameraStates.cameraIdle
            'While Not cameraImageReady
            '    System.Threading.Thread.Sleep(Duration * 100)
            'End While
        End If

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
        '  MsgBox("stopExposure")

        v.StopContinuousImageAcquisition()

        myCameraState = CameraStates.cameraIdle
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
            gigeCamID = driverProfile.GetValue(driverID, gigeCamIDProfileName, String.Empty, gigeCamIDDefault)
        End Using
    End Sub

    ''' <summary>
    ''' Write the device configuration to the  ASCOM  Profile store
    ''' </summary>
    Friend Sub WriteProfile()
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Camera"
            driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString())
            driverProfile.WriteValue(driverID, gigeCamIDProfileName, gigeCamID.ToString())
        End Using

    End Sub

    Protected Overrides Sub Finalize()

        MyBase.Finalize()
    End Sub

#End Region
    Private Shared Function ConvertFrame(ByVal frame As Frame) As Image
        If frame Is Nothing Then
            Throw New ArgumentNullException("frame")
        End If

        If VmbFrameStatusType.VmbFrameStatusComplete <> frame.ReceiveStatus Then
            Throw New Exception("Invalid frame received. Reason: " & frame.ReceiveStatus.ToString())
        End If

        Dim image As Image = Nothing

        Select Case frame.PixelFormat
            Case VmbPixelFormatType.VmbPixelFormatMono8
                Dim bitmap As Bitmap = New Bitmap(CInt(frame.Width), CInt(frame.Height), PixelFormat.Format8bppIndexed)
                Dim palette As ColorPalette = bitmap.Palette

                For i As Integer = 0 To palette.Entries.Length - 1
                    palette.Entries(i) = Color.FromArgb(i, i, i)
                Next

                bitmap.Palette = palette
                Dim bitmapData As BitmapData = bitmap.LockBits(New Rectangle(0, 0, CInt(frame.Width), CInt(frame.Height)), ImageLockMode.[WriteOnly], PixelFormat.Format8bppIndexed)

                Try

                    For y As Integer = 0 To CInt(frame.Height) - 1
                        System.Runtime.InteropServices.Marshal.Copy(frame.Buffer, y * CInt(frame.Width), New IntPtr(bitmapData.Scan0.ToInt64() + y * bitmapData.Stride), CInt(frame.Width))
                    Next

                Finally
                    bitmap.UnlockBits(bitmapData)
                End Try

                image = bitmap
            Case VmbPixelFormatType.VmbPixelFormatBgr8
                Dim bitmap As Bitmap = New Bitmap(CInt(frame.Width), CInt(frame.Height), PixelFormat.Format24bppRgb)
                Dim bitmapData As BitmapData = bitmap.LockBits(New Rectangle(0, 0, CInt(frame.Width), CInt(frame.Height)), ImageLockMode.[WriteOnly], PixelFormat.Format24bppRgb)

                Try

                    For y As Integer = 0 To CInt(frame.Height) - 1
                        System.Runtime.InteropServices.Marshal.Copy(frame.Buffer, y * (CInt(frame.Width)) * 3, New IntPtr(bitmapData.Scan0.ToInt64() + y * bitmapData.Stride), (CInt((frame.Width)) * 3))
                    Next

                Finally
                    bitmap.UnlockBits(bitmapData)
                End Try

                image = bitmap
            Case Else
                Throw New Exception("Current pixel format is not supported by this example (only Mono8 and BRG8Packed are supported).")
        End Select

        Return image
    End Function
    Private Shared Function ConvertFrameToImageAray(ByVal frame As Frame) As Object
        Dim imgArr As Integer(,)
        Dim pixelX As Integer = 0

        ReDim imgArr(frame.Width - 1, frame.Height - 1)
        If frame Is Nothing Then
            Debug.WriteLine("frame is nothing")
            Throw New ArgumentNullException("frame")
        End If

        If VmbFrameStatusType.VmbFrameStatusComplete <> frame.ReceiveStatus Then
            Debug.WriteLine("Invalid frame received. Reason: " & frame.ReceiveStatus.ToString())
            Debug.WriteLine("try anyways")
            'Throw New Exception("Invalid frame received. Reason: " & frame.ReceiveStatus.ToString())
            'Throw New ASCOM.InvalidValueException

        End If

        Dim image As Image = Nothing

        Select Case frame.PixelFormat
            Case VmbPixelFormatType.VmbPixelFormatMono14, VmbPixelFormatType.VmbPixelFormatMono12
                Try

                    For y As Integer = 0 To CInt(frame.Height) - 1
                        pixelX = 0
                        For x As Integer = 0 To CInt(frame.Width * 2) - 1 Step 2
                            'imgArr(pixelX, y) = frame.Buffer(x + y * frame.Width)

                            imgArr(pixelX, y) = (frame.Buffer(x + y * frame.Width * 2) + frame.Buffer(x + 1 + y * frame.Width * 2) * 256)  ' stretch to 16bits

                            pixelX = pixelX + 1

                        Next

                    Next
                    Return imgArr
                Catch ex As Exception

                    MsgBox(ex.Message)
                End Try
            Case VmbPixelFormatType.VmbPixelFormatMono8
                Try

                    For y As Integer = 0 To CInt(frame.Height) - 1
                        pixelX = 0
                        For x As Integer = 0 To CInt(frame.Width) - 1
                            imgArr(pixelX, y) = frame.Buffer(x + y * frame.Width)

                            ' imgArr(pixelX, y) = (frame.Buffer(x + y * frame.Width * 2) + frame.Buffer(x + 1 + y * frame.Width * 2) * 256)  ' stretch to 16bits

                            pixelX = pixelX + 1

                        Next

                    Next
                    Return imgArr
                Catch ex As Exception

                    MsgBox(ex.Message)
                End Try
            Case VmbPixelFormatType.VmbPixelFormatBayerRG12

                For y As Integer = 0 To CInt(frame.Height) - 1
                    pixelX = 0
                    For x As Integer = 0 To CInt(frame.Width * 2) - 1 Step 2

                        '                       imgArr(pixelX, y, 0) = (frame.Buffer(x + y * frame.Width * 2) + frame.Buffer(x + 1 + y * frame.Width * 2) * 256) * 16 ' stretch to 16bits
                        pixelX = pixelX + 1


                    Next

                Next
                Return imgArr
                'Case VmbPixelFormatType.VmbPixelFormatBgr8
                '    Dim bitmap As Bitmap = New Bitmap(CInt(frame.Width), CInt(frame.Height), PixelFormat.Format24bppRgb)
                '    Dim bitmapData As BitmapData = bitmap.LockBits(New Rectangle(0, 0, CInt(frame.Width), CInt(frame.Height)), ImageLockMode.[WriteOnly], PixelFormat.Format24bppRgb)

                '    Try

                '        For y As Integer = 0 To CInt(frame.Height) - 1
                '            System.Runtime.InteropServices.Marshal.Copy(frame.Buffer, y * (CInt(frame.Width)) * 3, New IntPtr(bitmapData.Scan0.ToInt64() + y * bitmapData.Stride), (CInt((frame.Width)) * 3))
                '        Next

                '    Finally
                '        bitmap.UnlockBits(bitmapData)
                '    End Try

                '    image = bitmap
            Case VmbPixelFormatType.VmbPixelFormatRgb8
                ' ReDim imgArr(frame.Width - 1, frame.Height - 1, 2)
                For y As Integer = 0 To CInt(frame.Height) - 1
                    pixelX = 0
                    For x As Integer = 0 To CInt(frame.Width * 3) - 1 Step 3

                        'imgArr(pixelX, y, 0) = frame.Buffer(x + y * frame.Width * 3)  ' red
                        'imgArr(pixelX, y, 1) = frame.Buffer(x + 1 + y * frame.Width * 3)  ' green
                        'imgArr(pixelX, y, 2) = frame.Buffer(x + 2 + y * frame.Width * 3)  ' blue

                        pixelX = pixelX + 1


                    Next

                Next
                Return imgArr
                'Dim bitmap As Bitmap = New Bitmap(CInt(frame.Width), CInt(frame.Height), PixelFormat.Format24bppRgb)
                'Dim bitmapData As BitmapData = bitmap.LockBits(New Rectangle(0, 0, CInt(frame.Width), CInt(frame.Height)), ImageLockMode.[WriteOnly], PixelFormat.Format24bppRgb)

                'Try

                '    For y As Integer = 0 To CInt(frame.Height) - 1
                '        System.Runtime.InteropServices.Marshal.Copy(frame.Buffer, y * (CInt(frame.Width)) * 3, New IntPtr(bitmapData.Scan0.ToInt64() + y * bitmapData.Stride), (CInt((frame.Width)) * 3))
                '    Next

                'Finally
                '    bitmap.UnlockBits(bitmapData)
                'End Try


                'Return bitmap
            Case Else
                Throw New Exception("Current pixel format is not supported by this example (only Mono8 and BRG8Packed are supported).")
        End Select


    End Function
End Class
