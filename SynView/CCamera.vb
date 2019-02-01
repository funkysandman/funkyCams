Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Windows.Forms
Imports System.Runtime.InteropServices ' for Marshal
Imports NewElectronicTechnology.SynView
Namespace Genicam
    Public Class CCamera

        Private Const NumberOfBuffers As Int32 = 10
        Public WithEvents m_pSystem As LvSystem
        Public WithEvents m_pInterface As LvInterface
        Public WithEvents m_pDevice As LvDevice
        Private WithEvents m_pStream As LvStream
        Private m_pRenderer As LvRenderer
        Private m_Buffers(NumberOfBuffers) As LvBuffer
        Private m_hDisplayWnd As IntPtr
        Private WithEvents m_pEvent As LvEvent
        Private m_bDoProcessing As Boolean
        Private m_id As String
        Private m_FrameHandler As FrameReceivedHandler = Nothing
        Public Delegate Sub FrameReceivedHandler(ByVal sender As Object, ByVal args As LvNewBufferEventArgs)
        ' Private m_pFeatureEvent As LvEvent = Nothing
        Public Sub New()
            m_pSystem = Nothing
            m_pInterface = Nothing
            m_pDevice = Nothing
            m_pStream = Nothing
            m_pRenderer = Nothing
            m_pEvent = Nothing
            For Each Buffer As LvBuffer In m_Buffers
                Buffer = Nothing
            Next
            m_hDisplayWnd = 0
            m_bDoProcessing = False
            m_pSystem = Nothing
            LvLibrary.ThrowErrorEnable = True
            Try
                LvLibrary.OpenLibrary()
                LvSystem.Open("", m_pSystem)
                Dim pInterface As LvInterface = Nothing
                Dim pDevice As LvDevice = Nothing
                m_pSystem.UpdateInterfaceList()


                m_pSystem.OpenInterface("GigE Interface", m_pInterface)

                m_pInterface.UpdateDeviceList()
            Catch ex As LvException
                Console.WriteLine(ex.Message)

            End Try
        End Sub
        Private Sub reNew()
            m_pSystem = Nothing
            m_pInterface = Nothing
            m_pDevice = Nothing
            m_pStream = Nothing
            m_pRenderer = Nothing
            m_pEvent = Nothing
            For Each Buffer As LvBuffer In m_Buffers
                Buffer = Nothing
            Next
            m_hDisplayWnd = 0
            m_bDoProcessing = False
            m_pSystem = Nothing
            LvLibrary.ThrowErrorEnable = True
            Try
                LvLibrary.OpenLibrary()
                LvSystem.Open("", m_pSystem)

            Catch ex As LvException
                Console.WriteLine(ex.Message)

            End Try
        End Sub

        Public Sub setGainExposure(g As Long, e As Long)


            ' --- Open interface and camera ---
            ' This code for opening the device expects there is already the SynView library open by
            ' the LvLibrary.OpenLibrary() and the system opened by the LvSystem.Open() and represented
            ' by the pSystem pointer.


            Try
                Me.StopAcquisition()
                m_pDevice.SetInt(LvDeviceFtr.Gain, g)
                m_pDevice.SetFloat(LvDeviceFtr.ExposureTime, e)
                Me.StartAcquisition()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try


        End Sub
        Public ReadOnly Property id As String
            Get
                Return m_id
            End Get
        End Property

        Public Sub OpenCamera(id As String, myHandler As FrameReceivedHandler)
            Try
                If m_pDevice IsNot Nothing Then CloseCamera()
                'm_pSystem = pSystem
                ' m_hDisplayWnd = hDisplayWnd
                m_FrameHandler = myHandler
                m_id = id


                'm_pInterface.OpenDevice(id, m_pDevice, LvDeviceAccess.ReadOnly)
                m_pInterface.OpenDevice(id, m_pDevice)

                'm_pInterface = pInterface
                'm_pDevice = pDevice

                m_pDevice.SetEnum(LvDeviceFtr.LvUniProcessMode, LvUniProcessMode.Auto)
                ' SetOptimalUniPixelFormat()

                m_pDevice.OpenStream("", m_pStream)
                m_pStream.OpenEvent(LvEventType.NewBuffer, m_pEvent)
                Dim i As Integer = 0
                Do While i < NumberOfBuffers
                    m_pStream.OpenBuffer(0, 0, 0, 0, m_Buffers(i))
                    m_pStream.SetInt32(LvStreamFtr.LvPostponeQueueBuffers, 3)
                    i += 1
                Loop
                ' m_pStream.OpenRenderer(m_pRenderer)
                'm_pRenderer.SetWindow(m_hDisplayWnd)
                Dim thing As IntPtr
                m_pEvent.SetCallbackNewBuffer(True, 0)
                m_pDevice.RegisterFeatureCallback(LvDeviceFtr.LvDeviceIsAcquiring, True, 0, 0)
                'm_pDevice.StartPollingThread(4000)
                ' m_pDevice.OpenEvent(LvEventType.FeatureDevEvent, m_pFeatureEvent)
                'm_pSystem.RegisterFeatureCallback(LvSystemFtr.Info, True, 0, 0)
                m_pEvent.StartThread()

                'm_pEvent.RegisterFeatureCallback(f, True, 0, 0)

                'm_pStream.RegisterFeatureCallback(LvStreamFtr., True, 0, thing)
                ' m_pStream.StartPollingThread(1000)
            Catch ex As LvException
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub

        'Private Sub FeatureChangedHandler(ByVal sender As System.Object, ByVal e As LvFeatureChangedArgs) Handles m_pDevice.OnFeatureChanged
        '    Try
        '        Select Case CInt(e.pUserParam)
        '            Case CInt(LvDeviceFtr.EventLvTriggerDropped)
        '                'm_iDroppedTriggerCount += 1
        '                Exit Select
        '            Case CInt(LvDeviceFtr.EventLvSmartAppLogMessage)
        '                Dim Val_EventLvSmartAppLogMessage As String = ""
        '                Dim Val_EventLvSmartAppLogTimestamp As Int64 = 0
        '                m_pDevice.GetString(LvDeviceFtr.EventLvSmartAppLogMessage, Val_EventLvSmartAppLogMessage)
        '                m_pDevice.GetInt(LvDeviceFtr.EventLvSmartAppLogTimestamp, Val_EventLvSmartAppLogTimestamp)
        '                'If m_LastLogTimestamp <> Val_EventLvSmartAppLogTimestamp Then
        '                '    m_LastLogTimestamp = Val_EventLvSmartAppLogTimestamp
        '                '    m_sSmartAppLog += Val_EventLvSmartAppLogMessage
        '                'End If

        '                Exit Select
        '        End Select
        '    Catch __unusedLvException1__ As LvException
        '    End Try
        'End Sub



        '=======================================================
        'Service provided by Telerik (www.telerik.com)
        'Conversion powered by Refactoring Essentials.
        'Twitter: @telerik
        'Facebook: facebook.com/telerik
        '=======================================================

        Public Sub StartAcquisition()
            Try
                m_pStream.FlushQueue(LvQueueOperation.AllToInput)
                If m_pDevice Is Nothing Then Exit Sub
                m_pDevice.AcquisitionStart()
            Catch ex As LvException
                'MessageBox.Show(ex.Message, "Error", 'MessageBoxButtons.OK, 'MessageBoxIcon.Exclamation)
            End Try
        End Sub

        Public Sub StopAcquisition()
            Try
                If m_pStream Is Nothing Then Exit Sub
                m_pDevice.AcquisitionStop()
            Catch ex As LvException
                'MessageBox.Show(ex.Message, "Error", 'MessageBoxButtons.OK, 'MessageBoxIcon.Exclamation)
            End Try
        End Sub


        Public Sub CloseCamera()
            Try
                If m_pDevice Is Nothing Then Exit Sub
                If IsAcquiring() Then StopAcquisition()
                m_pEvent.StopThread()
                m_pEvent.SetCallbackNewBuffer(False, 0)
                m_pStream.CloseEvent(m_pEvent)
                '  m_pStream.CloseRenderer(m_pRenderer)
                Dim i As Integer = 0
                m_pStream.FlushQueue(LvQueueOperation.AllDiscard)
                Do While i < NumberOfBuffers
                    If m_Buffers(i) IsNot Nothing Then
                        m_pStream.CloseBuffer(m_Buffers(i))
                    End If
                    i += 1
                Loop
                m_pDevice.CloseStream(m_pStream)
                m_pInterface.CloseDevice(m_pDevice)
                m_pSystem.CloseInterface(m_pInterface)
            Catch ex As LvException
                'MessageBox.Show(ex.Message, "Error", 'MessageBoxButtons.OK, 'MessageBoxIcon.Exclamation)
            End Try
        End Sub
        Public Sub CloseCameraSafe()
            Try
                If m_pDevice Is Nothing Then Exit Sub
                If IsAcquiring() Then StopAcquisition()
                ' m_pEvent.StopThread()
                '  m_pEvent.SetCallbackNewBuffer(False, 0)
                'm_pStream.CloseEvent(m_pEvent)
                'm_pStream.CloseRenderer(m_pRenderer)
                Dim i As Integer = 0
                m_pStream.FlushQueue(LvQueueOperation.AllDiscard)
                Do While i < NumberOfBuffers
                    If m_Buffers(i) IsNot Nothing Then
                        m_pStream.CloseBuffer(m_Buffers(i))
                    End If
                    i += 1
                Loop
                ' m_pDevice.CloseStream(m_pStream)
                'm_pInterface.CloseDevice(m_pDevice)
                'm_pSystem.CloseInterface(m_pInterface)
            Catch ex As LvException
                'MessageBoà.Show(ex.Message, "Error", 'MessageBoxButtons.OK, 'MessageBoxIcon.Exclamation)
            End Try
        End Sub
        Public Function IsOpen() As Boolean
            If m_pDevice IsNot Nothing Then
                Return True
            End If
            Return False
        End Function

        Public Function IsAcquiring() As Boolean
            If m_pDevice Is Nothing Then
                Return False
            End If
            Dim bIsAcquiring As Boolean = False
            Try
                m_pDevice.GetBool(LvDeviceFtr.LvDeviceIsAcquiring, bIsAcquiring)
            Catch ex As LvException
                'no message
                bIsAcquiring = False
            End Try
            Return bIsAcquiring
        End Function
        Public Function mode() As Long
            If m_pDevice Is Nothing Then
                Return False
            End If
            Dim bmode As Long = 0
            Try
                m_pDevice.GetInt(LvDeviceFtr.GevGVCPPendingTimeout, bmode)
            Catch ex As LvException
                'no message
                bmode = 0
            End Try
            Return bmode
        End Function

        Public Sub setExposureTime(t As Long)
            'Try
            '    m_pDevice.SetFloat(LvDeviceFtr.ExposureTime, t)
            'Catch ex As Exception

            '    m_pInterface.UpdateDeviceList()
            '    m_pInterface.CloseDevice(m_pDevice)
            'End Try


        End Sub

        Public Sub SetProcessing(ByVal bDoProcessing As Boolean)
            m_bDoProcessing = bDoProcessing
        End Sub

        Private Sub SetOptimalUniPixelFormat()
            Dim PixelFormat As UInt32 = LvPixelFormat.Mono8
            m_pDevice.GetEnum(LvDeviceFtr.PixelFormat, PixelFormat)
            Dim UniPixelFormat As UInt32 = PixelFormat
            Select Case PixelFormat
                Case LvPixelFormat.Mono8
                    UniPixelFormat = LvPixelFormat.Mono8
                Case LvPixelFormat.Mono10
                    UniPixelFormat = LvPixelFormat.Mono8
                Case LvPixelFormat.Mono12
                    UniPixelFormat = LvPixelFormat.Mono8
                Case LvPixelFormat.Mono16
                    UniPixelFormat = LvPixelFormat.Mono8
                Case LvPixelFormat.BayerGR8
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerRG8
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerGB8
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerBG8
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerGR10
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerRG10
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerGB10
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerBG10
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerGR12
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerRG12
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerGB12
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.BayerBG12
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.RGB8Packed
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case LvPixelFormat.RGBA8Packed
                    UniPixelFormat = LvPixelFormat.BGRA8Packed
                Case Else
                    UniPixelFormat = LvPixelFormat.Mono8
            End Select
            m_pDevice.SetEnum(LvDeviceFtr.LvUniPixelFormat, UniPixelFormat)
        End Sub

        Public Sub Repaint()
            Try
                If m_pRenderer IsNot Nothing Then
                    m_pRenderer.Repaint()
                End If
            Catch ex As LvException
                'no message
            End Try
        End Sub

        Public Sub NewBufferEventHandler(ByVal sender As System.Object, ByVal e As LvNewBufferEventArgs) Handles m_pEvent.OnEventNewBuffer
            Me.m_FrameHandler(sender, e)
            '    Try
            '        If e.Buffer Is Nothing Then Exit Sub

            '        Dim pData As IntPtr
            '        Dim iImageOffset As Int64
            '        e.Buffer.GetPtr(LvBufferFtr.UniBase, pData)
            '        e.Buffer.GetInt(LvBufferFtr.UniImageOffset, iImageOffset)
            '        pData = pData.ToInt64() + iImageOffset
            '        If m_bDoProcessing And pData <> 0 Then
            '            ' we will do some easy processing - invert the pixel values in an area
            '            Dim iWidth As Int32 = 0
            '            Dim iHeight As Int32 = 0
            '            Dim iLinePitch As Int32 = 0
            '            Dim iPixelFormat As UInt32 = 0    ' LvPixelFormat enumeration value
            '            m_pDevice.GetInt32(LvDeviceFtr.Width, iWidth)
            '            m_pDevice.GetInt32(LvDeviceFtr.Height, iHeight)
            '            m_pDevice.GetEnum(LvDeviceFtr.LvUniPixelFormat, iPixelFormat)
            '            m_pDevice.GetInt32(LvDeviceFtr.LvUniLinePitch, iLinePitch)
            '            Dim iBytesPerPixel As Int32 = ((iPixelFormat And &HFF0000) >> 16) \ 8

            '            Dim j As Integer = 0
            '            Do While j < (iHeight \ 2)
            '                Dim BaseOffset As Int32
            '                BaseOffset = (iHeight \ 4 + j) * iLinePitch + (iWidth \ 4) * iBytesPerPixel
            '                Dim i As Integer
            '                i = 0
            '                Do While i < (iBytesPerPixel * iWidth / 2)
            '                    Dim k As Integer
            '                    k = 0
            '                    Do While k < iBytesPerPixel
            '                        k += 1
            '                        ' In VB we can use the Marshal class to access data pointed
            '                        ' by unmanaged pointers. However, this way of accessing unmanaged
            '                        ' data is ineffective (may slow down the acquisition).
            '                        Dim Pixel As Byte = Marshal.ReadByte(pData, BaseOffset + i + k)
            '                        Pixel = Not Pixel
            '                        Marshal.WriteByte(pData, BaseOffset + i + k, Pixel)
            '                    Loop
            '                    i += iBytesPerPixel
            '                Loop
            '                j += 1
            '            Loop
            '        End If
            '        'save image

            ' m_pRenderer.DisplayImage(e.Buffer)
            '        e.Buffer.Queue()
            '    Catch ex As LvException
            '        ' no message
            '    End Try
        End Sub

        'Private Sub onFeatureChanged(Sender As Object, E As LvFeatureChangedArgs) Handles m_pDevice.OnFeatureChanged
        '    '  If Not IsAcquiring() Then
        '    setExposureTime(120)
        '    '  End If
        'End Sub
    End Class

End Namespace
