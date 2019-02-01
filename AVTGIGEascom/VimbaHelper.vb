Imports System.Threading
Imports System.Diagnostics
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports AVT.VmbAPINET

Public Delegate Sub CameraListChangedHandler(ByVal sender As Object, ByVal args As EventArgs)
Public Delegate Sub OnFrameReceivedHandler(ByVal sender As Object, ByVal args As FrameEventArgs)

Public Class VimbaHelper
    Private Const m_RingBitmapSize As Integer = 5
    Private Shared ReadOnly m_ImageInUseSyncLock As Object = New Object()
    Private Shared m_RingBitmap As RingBitmap = Nothing
    Private Shared m_ImageInUse As Boolean = True
    Public v As Vimba = Nothing
    Private m_CameraListChangedHandler As CameraListChangedHandler = Nothing
    Public m_Camera As AVT.VmbAPINET.Camera = Nothing
    Private m_isWaitingForImage As Boolean = False
    Private m_timeoutThread As Thread
    Private m_IsTriggerAvailable As Boolean = False
    Private b As Byte()
    Private camFrame As Frame
    Public ReadOnly Property IsTriggerAvailable As Boolean
        Get
            Return m_IsTriggerAvailable
        End Get
    End Property

    Private m_Acquiring As Boolean = False
    Private m_FrameReceivedHandler As OnFrameReceivedHandler = Nothing

    Public Sub New()
        m_RingBitmap = New RingBitmap(m_RingBitmapSize)
    End Sub

    Protected Overrides Sub Finalize()
        Me.ReleaseVimba()
    End Sub

    Public Shared Property ImageInUse As Boolean
        Get

            SyncLock m_ImageInUseSyncLock
                Return m_ImageInUse
            End SyncLock
        End Get
        Set(ByVal value As Boolean)

            SyncLock m_ImageInUseSyncLock
                m_ImageInUse = value
            End SyncLock
        End Set
    End Property

    Public ReadOnly Property CameraList As List(Of CameraInfo)
        Get

            If Me.v Is Nothing Then
                Throw New Exception("Vimba is not started.")
            End If

            Dim cl As List(Of CameraInfo) = New List(Of CameraInfo)()
            Dim cameras As CameraCollection
            cameras = Me.v.Cameras

            For Each aCam As AVT.VmbAPINET.Camera In cameras
                cl.Add(New CameraInfo(aCam.Name, aCam.Id))
            Next

            Return cl
        End Get
    End Property

    'Public Sub Startup(ByVal cameraListChangedHandler As CameraListChangedHandler)
    '    Dim vimba As Vimba = New Vimba()
    '    vimba.Startup()
    '    Me.v = vimba
    '    Dim bError As Boolean = True

    '    Try

    '        If cameraListChangedHandler IsNot Nothing Then
    '            Me.v.OnCameraListChanged += AddressOf Me.OnCameraListChange
    '            Me.m_CameraListChangedHandler = cameraListChangedHandler
    '        End If

    '        bError = False
    '    Finally

    '        If True = bError Then
    '            Me.ReleaseVimba()
    '        End If
    '    End Try
    'End Sub

    Public Sub Startup()
        v = New AVT.VmbAPINET.Vimba
        v.Startup()
        Me.v = v
    End Sub

    Public Sub Shutdown()
        If Me.v Is Nothing Then
            Throw New Exception("Vimba has not been started.")
        End If

        Me.ReleaseVimba()
    End Sub

    Public Function GetVersion() As String
        If Me.v Is Nothing Then
            Throw New Exception("Vimba has not been started.")
        End If

        Dim version_info As VmbVersionInfo_t = Me.v.Version
        Return String.Format("{0:D}.{1:D}.{2:D}", version_info.major, version_info.minor, version_info.patch)
    End Function

    Public Sub OpenCamera(ByVal id As String)
        If id Is Nothing Then
            Throw New ArgumentNullException("id")
        End If

        If Me.v Is Nothing Then
            Throw New Exception("Vimba is not started.")
        End If

        If Me.m_Camera Is Nothing Then
            Me.m_Camera = v.OpenCameraByID(id, VmbAccessModeType.VmbAccessModeFull)

            If Me.m_Camera Is Nothing Then
                Throw New NullReferenceException("No camera retrieved.")
            End If
        End If

        m_IsTriggerAvailable = False
        'Me.m_Camera.Features("AcquisitionMode").StringValue = "Single"
        Me.m_Camera.Features("TriggerActivation").StringValue = "RisingEdge"
        'If Me.m_Camera.Features("ExposureMode").EnumValues Then
        '    Me.m_Camera.Features("ExposureMode").StringValue = "TriggerWidth"
        'End If
        Me.m_Camera.Features("TriggerSource").StringValue = "Line1"
        If m_Camera.Features.ContainsName("SensorDigitizationTaps") Then
            Me.m_Camera.Features("SensorDigitizationTaps").StringValue = "One"
        End If

        Me.m_Camera.Features("PixelFormat").StringValue = "Mono14"
        If m_Camera.Features.ContainsName("BlackLevel") Then
            Me.m_Camera.Features("BlackLevel").FloatValue = 100
        End If
        '  Me.m_Camera.Features("ExposureMode").StringValue = "TriggerWidth"
        If Me.m_Camera.Features.ContainsName("TriggerSoftware") AndAlso Me.m_Camera.Features("TriggerSoftware").IsWritable() Then
            Dim entries As EnumEntryCollection = Me.m_Camera.Features("TriggerSelector").EnumEntries

            For Each entry As EnumEntry In entries

                If entry.Name = "FrameStart" Then
                    m_IsTriggerAvailable = True
                    Exit For
                End If
            Next
        End If

        Try
            Me.m_Camera.Features("GevSCPSPacketSize").IntValue = 1500
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Public Sub CloseCamera()
        ReleaseCamera()
    End Sub

    Public Sub StartContinuousImageAcquisition(ByVal frameReceivedHandler As OnFrameReceivedHandler)
        Dim bError As Boolean = False

        Try

            If frameReceivedHandler IsNot Nothing Then
                AddHandler Me.m_Camera.OnFrameReceived, AddressOf Me.OnImageReceived

                Me.m_FrameReceivedHandler = frameReceivedHandler
            End If

            m_RingBitmap = New RingBitmap(m_RingBitmapSize)
            m_ImageInUse = True
            Me.m_Acquiring = True
            Me.m_Camera.StartContinuousImageAcquisition(3)
            bError = False
        Finally

            If True = bError Then

                Try
                    Me.ReleaseCamera()
                Catch
                End Try
            End If
        End Try
    End Sub

    Public Sub StartContinuousFrameAcquisition(ByVal frameReceivedHandler As OnFrameReceivedHandler)
        Dim bError As Boolean = True

        Try

            If frameReceivedHandler IsNot Nothing Then
                AddHandler Me.m_Camera.OnFrameReceived, AddressOf Me.OnFrameReceived

                Me.m_FrameReceivedHandler = frameReceivedHandler
            End If

            m_RingBitmap = New RingBitmap(m_RingBitmapSize)
            m_ImageInUse = True
            Me.m_Acquiring = True
            Me.m_Camera.StartCapture()
            ' Me.m_Camera.StartContinuousImageAcquisition(3)
            bError = False
        Finally

            If True = bError Then

                Try
                    Me.ReleaseCamera()
                Catch
                End Try
            End If
        End Try
    End Sub
    Public Sub StartSingleFrameAcquisition(aFrameReceivedHandler As AVT.VmbAPINET.Camera.OnFrameReceivedHandler)
        Dim bError As Boolean = True

        Try

            ''  If FrameReceivedHandler IsNot Nothing Then
            'AddHandler Me.m_Camera.OnFrameReceived, AddressOf Me.OnFrameReceived
            AddHandler Me.m_Camera.OnFrameReceived, aFrameReceivedHandler
            'Me.m_FrameReceivedHandler = aFrameReceivedHandler
            '   End If

            b = New Byte(Me.m_Camera.Features("PayloadSize").IntValue) {}
            'b = New Byte(8355840) {}
            camFrame = New Frame(b)
            Me.m_Camera.AnnounceFrame(camFrame)

            Debug.WriteLine("frame status:" & camFrame.ReceiveStatus)
            ' m_RingBitmap = New RingBitmap(m_RingBitmapSize)
            m_ImageInUse = True
            Me.m_Acquiring = True
            'Me.m_Camera.StartContinuousImageAcquisition(3)
            ' Me.m_Camera.Features("TriggerSelector").StringValue = "FrameStart"
            'Me.m_Camera.Features("TriggerActivation").StringValue = "RisingEdge"
            'Me.m_Camera.Features("TriggerActivation").StringValue = "RisingEdge"

            ' Me.m_Camera.Features("ExposureMode").StringValue = "TriggerWidth"
            Me.m_Camera.Features("TriggerSource").StringValue = "Line1"

            Try
                Me.m_Camera.StartCapture() 'press play
            Catch
            End Try
            Me.m_Camera.Features("StreamHoldEnable").StringValue = "On"

            'Me.m_Camera.Features("AcquisitionStop").RunCommand()
            'send software trigger

            'Me.m_Camera.StartContinuousImageAcquisition(3)

            Me.m_Camera.Features("AcquisitionStart").RunCommand()
            While (Not m_Camera.Features("AcquisitionStart").IsCommandDone())

            End While
            Me.m_Camera.QueueFrame(camFrame)
            flipEdge()


            bError = False
        Finally

            If True = bError Then
                MsgBox("error")
                Try
                    Me.ReleaseCamera()
                Catch
                End Try
            End If
        End Try
    End Sub
    Public Sub StopContinuousImageAcquisition()
        If Me.v Is Nothing Then
            Throw New Exception("Vimba is not started.")
        End If

        If Me.m_Camera Is Nothing Then
            Throw New Exception("No camera open.")
        End If


    End Sub
    Public Sub flipEdge()
        Debug.WriteLine("flipping edge")
        If Me.m_Camera.Features("TriggerActivation").StringValue = "RisingEdge" Then
            Me.m_Camera.Features("TriggerActivation").StringValue = "FallingEdge"
        Else
            Me.m_Camera.Features("TriggerActivation").StringValue = "RisingEdge"
        End If

    End Sub
    Public Sub StopSingleFrameAcquisition()
        If Me.v Is Nothing Then
            Throw New Exception("Vimba is not started.")
        End If

        If Me.m_Camera Is Nothing Then
            Throw New Exception("No camera open.")
        End If
        flipEdge()
        'wait for image to be received
        Me.m_Camera.Features("StreamHoldEnable").StringValue = "Off"


    End Sub

    Public Sub EnableSoftwareTrigger(ByVal enable As Boolean)
        If Me.m_Camera IsNot Nothing Then
            Dim featureValueSource As String = String.Empty
            Dim featureValueMode As String = String.Empty

            If enable Then
                featureValueMode = "On"
            Else
                featureValueMode = "Off"
            End If

            Me.m_Camera.Features("TriggerSelector").EnumValue = "FrameStart"
            Me.m_Camera.Features("TriggerSource").EnumValue = "Software"
            Me.m_Camera.Features("TriggerMode").EnumValue = featureValueMode
        End If
    End Sub

    Public Sub TriggerSoftwareTrigger()
        If Me.m_Camera IsNot Nothing Then
            Me.m_Camera.Features("TriggerSoftware").RunCommand()
        End If
    End Sub

    Private Shared Function ConvertFrame(ByVal frame As Frame) As Image
        If frame Is Nothing Then
            Console.WriteLine("null frame")
            Throw New ArgumentNullException("frame")
        End If

        If VmbFrameStatusType.VmbFrameStatusComplete <> frame.ReceiveStatus Then
            Console.WriteLine("invalid frame")
            Throw New Exception("Invalid frame received. Reason: " & frame.ReceiveStatus.ToString())
        End If

        Dim image As Image = Nothing
        Console.WriteLine("fill next bmp")
        m_RingBitmap.FillNextBitmap(frame)
        image = m_RingBitmap.Image
        ImageInUse = False
        Console.WriteLine("returning image")
        Return image
    End Function

    Private Sub ReleaseVimba()
        If Me.v IsNot Nothing Then

            Try

                Try

                    Try
                        Me.ReleaseCamera()
                    Finally

                        'If Me.m_CameraListChangedHandler IsNot Nothing Then
                        '    Me.v.OnCameraListChanged -= AddressOf Me.OnCameraListChange
                        'End If
                    End Try

                Finally
                    Me.m_CameraListChangedHandler = Nothing
                    Me.v.Shutdown()
                End Try

            Finally
                Me.v = Nothing
            End Try
        End If
    End Sub

    Private Sub ReleaseCamera()
        If Me.m_Camera IsNot Nothing Then

            Try

                Try

                    Try

                        If Me.m_FrameReceivedHandler IsNot Nothing Then
                            'Me.m_Camera.OnFrameReceived -= AddressOf Me.OnFrameReceived
                        End If

                    Finally
                        Me.m_FrameReceivedHandler = Nothing

                        If True = Me.m_Acquiring Then
                            Me.m_Acquiring = False
                            Me.m_Camera.StopContinuousImageAcquisition()

                            If Me.IsTriggerAvailable Then
                                Me.EnableSoftwareTrigger(False)
                            End If
                        End If
                    End Try

                Finally
                    Me.m_Camera.Close()
                End Try

            Finally
                Me.m_Camera = Nothing
            End Try
        End If
    End Sub

    Public Sub OnFrameReceived(ByVal frame As Frame)

        m_isWaitingForImage = False
        Console.WriteLine("got image")

        'If m_timeoutThread IsNot Nothing Then
        '    m_timeoutThread.Abort()
        '    Console.WriteLine("aborted thread")
        'End If

        Try
            Dim frameReceivedHandler As OnFrameReceivedHandler = Me.m_FrameReceivedHandler
            Console.WriteLine("setup frameReceiveHandler")

            If frameReceivedHandler IsNot Nothing AndAlso frame IsNot Nothing Then
                frameReceivedHandler(Me, New FrameEventArgs(frame))
                Console.WriteLine("Report image to user")
            End If

        Catch exception As Exception
            Dim frameReceivedHandler As OnFrameReceivedHandler = Me.m_FrameReceivedHandler

            If frameReceivedHandler IsNot Nothing Then
                frameReceivedHandler(Me, New FrameEventArgs(exception))
                Console.WriteLine("frameReceivedHandler")
            End If

        Finally

            If True = Me.m_Acquiring Then

                Try
                    Me.m_Camera.QueueFrame(frame)
                    Console.WriteLine("queue frame")
                    m_timeoutThread = New Thread(AddressOf Me.timeOut)
                    m_timeoutThread.Start()
                Catch exception As Exception
                    Dim frameReceivedHandler As OnFrameReceivedHandler = Me.m_FrameReceivedHandler

                    If frameReceivedHandler IsNot Nothing Then
                        frameReceivedHandler(Me, New FrameEventArgs(exception))
                        Console.WriteLine("frameReceivedHandler " & exception.Message)
                    End If
                End Try
            End If
        End Try
    End Sub

    Private Sub OnImageReceived(ByVal frame As Frame)
        m_isWaitingForImage = False
        Console.WriteLine("got image")

        If m_timeoutThread IsNot Nothing Then
            m_timeoutThread.Abort()
            Console.WriteLine("aborted thread")
        End If

        Try
            Dim image As Image = ConvertFrame(frame)
            Console.WriteLine("converted frame")
            Dim frameReceivedHandler As OnFrameReceivedHandler = Me.m_FrameReceivedHandler
            Console.WriteLine("setup frameReceiveHandler")

            If frameReceivedHandler IsNot Nothing AndAlso image IsNot Nothing Then
                frameReceivedHandler(Me, New FrameEventArgs(image))
                Console.WriteLine("Report image to user")
            End If

        Catch exception As Exception
            Dim frameReceivedHandler As OnFrameReceivedHandler = Me.m_FrameReceivedHandler

            If frameReceivedHandler IsNot Nothing Then
                frameReceivedHandler(Me, New FrameEventArgs(exception))
                Console.WriteLine("frameReceivedHandler")
            End If

        Finally

            If True = Me.m_Acquiring Then

                Try
                    Me.m_Camera.QueueFrame(frame)
                    Console.WriteLine("queue frame")
                    m_timeoutThread = New Thread(AddressOf Me.timeOut)
                    m_timeoutThread.Start()
                Catch exception As Exception
                    Dim frameReceivedHandler As OnFrameReceivedHandler = Me.m_FrameReceivedHandler

                    If frameReceivedHandler IsNot Nothing Then
                        frameReceivedHandler(Me, New FrameEventArgs(exception))
                        Console.WriteLine("frameReceivedHandler " & exception.Message)
                    End If
                End Try
            End If
        End Try
    End Sub

    Private Sub timeOut()
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While stopWatch.ElapsedMilliseconds < 20000
            System.Threading.Thread.Sleep(1000)
            Console.WriteLine("waiting...")
        End While

        Dim b = New Byte(1392639) {}
        Dim f As Frame = New Frame(b)
        Console.WriteLine("timedout-try queue frame")
        Me.m_Camera.QueueFrame(f)
    End Sub

    Private Sub OnCameraListChange(ByVal reason As VmbUpdateTriggerType)
        'Select Case reason
        '    Case VmbUpdateTriggerType.VmbUpdateTriggerPluggedIn, VmbUpdateTriggerType.VmbUpdateTriggerPluggedOut
        '        Dim cameraListChangedHandler As CameraListChangedHandler = Me.m_CameraListChangedHandler
        '        RaiseEvent CameraListChangedHandler(Me, EventArgs.Empty)
        '    Case Else
        'End Select
    End Sub
End Class

