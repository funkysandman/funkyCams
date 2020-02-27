Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Net.Http
Imports System.Collections.Specialized
Imports vimbaWinVBnet.vimbaWinVBnet
Imports TIS.Imaging
Public Class frmIS
    Dim myDetectionQueue As New Queue(Of queueEntry)
    Private VCDProp As TIS.Imaging.VCDHelpers.VCDSimpleProperty

    Private mhCamera As IntPtr
    Private mDisplayPanel As myPanel
    Private mIsMono As Boolean
    Private mDisplayBitmap As Bitmap
    'Private mFrameCallback As QCamM_AsyncCallback
    'Private mFrame1 As QCamM_Frame
    ' Private mFrame2 As QCamM_Frame
    'Private mRgbFrame As QCamM_Frame
    ' Private mSettings As QCamM_SettingsEx
    Private myWebServer As WebServer
    Private checkBox1 As CheckBox
    Private tbExposure As TrackBar
    Private lblExposureVal As Label
    Private panel1 As Panel
    Private gbAcquisition As GroupBox
    Private gbExposure As GroupBox
    Private gbGain As GroupBox
    Private lblGainVal As Label
    Private running As Boolean
    Private gbInfo As GroupBox
    Private lblCameraModel As Label
    Private bmp As Bitmap
    Private imageBytes As Byte()
    Private bmp2 As Bitmap
    Private lblSerNum As Label
    Private frames As Integer
    Private startTime As DateTime
    Private gotFrameTime As DateTime
    Private dark() As UShort
    Private t As Thread
    Private t_imaging As Thread
    Private h_camera As Integer

    Private helper As SnapshotHelper
    Private night As Boolean = False
    ' Private md As New ObjectDetection.TFDetector()
    Private myImageCodecInfo As ImageCodecInfo
    Private myEncoder As System.Drawing.Imaging.Encoder
    Private myEncoderParameter As EncoderParameter
    Private myEncoderParameters As EncoderParameters
    Private meteorCheckRunning As Boolean = False
    Private m_camRunning As Boolean = False
    Private m_grabbing As Boolean = False
    Private rawImage As Byte()

    Shared m_pics As RingBitmap
    Private m_grabbedframe As Boolean
    Private m_grabbedframe_err As Integer = 0

    Private m_syncLock As Object = New Object   '// Sync object protecting the following member data
    Public Shared iWidth As Integer
    Public Shared iHeight As Integer


    '//
    '// Callback function for Api.GetClip
    '// Called by a thread other than that which called Api.GetClip, therefore we syncronize access to the member data modified
    '//


    Public Class RingBitmap

        Private m_Size As Integer = 0

        Private m_Bitmaps As Bitmap()

        Private m_BitmapSelector As Integer = 0

        Private m_buffers()() As Byte
        Public Sub New(s As Integer)

            m_Size = s
            m_Bitmaps = New Bitmap(m_Size - 1) {}
            ReDim m_buffers(m_Size - 1)
        End Sub
        Public ReadOnly Property Image As Image
            Get
                Debug.Print("getting bitmap " & m_BitmapSelector)
                Return m_Bitmaps(m_BitmapSelector)
            End Get
        End Property
        Public ReadOnly Property ImageBytes As Byte()
            Get
                Debug.Print("getting bitmap " & m_BitmapSelector)
                'copy raw data to byte array


                Return m_buffers(m_BitmapSelector)
            End Get
        End Property

        Public Sub FillNextBitmap(b As Byte())
            SwitchBitmap()
            m_buffers(m_BitmapSelector) = b

        End Sub
        'Public Sub FillNextBitmap(frame As UShort())

        '    ' switch to Bitmap object which Is currently Not in use by GUI
        '    SwitchBitmap()
        '    Debug.Print("fillnextbitmap bitmapselector: " & m_BitmapSelector)
        '    Try

        '        If (m_Bitmaps(m_BitmapSelector) Is Nothing) Then
        '            Debug.Print("making new bitmap")
        '            m_Bitmaps(m_BitmapSelector) = New Bitmap(iXres, iYres, PixelFormat.Format24bppRgb)
        '            Dim ncp As System.Drawing.Imaging.ColorPalette = m_Bitmaps(m_BitmapSelector).Palette
        '            For j As Integer = 0 To 255
        '                ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
        '            Next
        '            m_Bitmaps(m_BitmapSelector).Palette = ncp
        '        End If


        '    Catch
        '    End Try

        '    Try
        '        'copy frame into bitmap
        '        Dim rawData(frame.bufferSize) As Byte



        '        Marshal.Copy(frame.pBuffer, rawData, 0, frame.bufferSize)

        '        m_buffers(m_BitmapSelector) = rawData

        '        Dim BoundsRect = New Rectangle(0, 0, frame.width, frame.height)
        '        Dim bmpData As System.Drawing.Imaging.BitmapData = m_Bitmaps(m_BitmapSelector).LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], m_Bitmaps(m_BitmapSelector).PixelFormat)

        '        Dim ptr As IntPtr = bmpData.Scan0

        '        Dim bytes As Integer = frame.bufferSize
        '        For i = 1 To 100
        '            Debug.Print(rawData(i))
        '        Next


        '        Marshal.Copy(rawData, 0, ptr, bytes)
        '        m_Bitmaps(m_BitmapSelector).UnlockBits(bmpData)
        '        m_Bitmaps(m_BitmapSelector).RotateFlip(RotateFlipType.Rotate180FlipNone)

        '    Catch

        '        Console.WriteLine("error during frame fill")
        '    End Try


        'End Sub
        Private Sub SwitchBitmap()
            m_BitmapSelector += 1

            If m_Size = m_BitmapSelector Then
                m_BitmapSelector = 0
            End If
        End Sub
    End Class
    Private Sub StopStream()
        'QCam.QCamM_Abort(mhCamera)
        'QCam.QCamM_SetStreaming(mhCamera, 0)
    End Sub

    Private Sub StartStream()
        'QCam.QCamM_Abort(mhCamera)
        'QCam.QCamM_SetStreaming(mhCamera, 0)
        'QCam.QCamM_SetStreaming(mhCamera, 1)
        'QueueFrame(1)
        'QueueFrame(2)
    End Sub

    Private Function QueueFrame(ByVal frameNum As UInteger) As Boolean
        'Dim err As QCamM_Err

        'If frameNum = 1 Then
        '    err = QCam.QCamM_QueueFrame(Me.mhCamera, mFrame1, mFrameCallback, CUInt(QCamM_qcCallbackFlags.qcCallbackDone), IntPtr.Zero, frameNum)
        'ElseIf frameNum = 2 Then
        '    err = QCam.QCamM_QueueFrame(Me.mhCamera, mFrame2, mFrameCallback, CUInt(QCamM_qcCallbackFlags.qcCallbackDone), IntPtr.Zero, frameNum)
        'Else

        '    Return False
        'End If

        'If err = QCamM_Err.qerrSuccess Then
        '    Return True
        'Else
        '    Debug.Print("err is:" & err)
        '    Return False
        'End If
    End Function
    Private Function OpenCamera() As Boolean
        'mCamList = New QCamM_CamListItem(9) {}
        'Dim listLen As UInteger = 10
        'QCam.QCamM_ListCameras(mCamList, listLen)

        'If (listLen > 0) AndAlso (mCamList(0).isOpen = 0) Then

        '    If QCam.QCamM_OpenCamera(mCamList(0).cameraId, mhCamera) <> QCamM_Err.qerrSuccess Then
        '        Return False
        '    End If

        '    Return True
        'Else
        '    If (listLen > 0) And (mCamList(0).isOpen = 1) Then
        '        Debug.Print("camera already open")
        '        Return True 'already open

        '    Else
        '        'msgbox("no camera")
        '    End If
        'End If

    End Function
    Private Function ToUInt16(ByVal s As Int16) As UInt16
        If (s And &H8000) = 0 Then
            Return CType(s, UInt16)
        Else
            Return CType(UInt16.MaxValue + 1 + CType(s, Int32), UInt16)
        End If
    End Function

    Private Function ReadY16(ByVal buf As TIS.Imaging.ImageBuffer, ByVal row As Integer, ByVal col As Integer) As UInt16
        ' Y16 is top-down, the first line has index 0
        Dim offset As Integer = row * buf.BytesPerLine + col * 2

        Dim val As Int16 = System.Runtime.InteropServices.Marshal.ReadInt16(buf.GetIntPtr(), offset)

        Return ToUInt16(val)
    End Function
    Private Function ToInt16(ByVal us As UInt16) As Int16
        If (us And &H8000) = 0 Then
            Return CType(us, Int16)
        Else
            Return CType(CType(us, Int32) - UInt16.MaxValue - 1, Int16)
        End If
    End Function

    Private Sub WriteY16(ByVal buf As TIS.Imaging.ImageBuffer, ByVal row As Integer, ByVal col As Integer, ByVal value As UInt16)

        Dim offset As Integer = row * buf.BytesPerLine + col * 2

        System.Runtime.InteropServices.Marshal.WriteInt16(buf.GetIntPtr(), offset, ToInt16(value))
    End Sub

    Public Sub loadMasterDark()
        Dim filename As String
        Dim bReader As BinaryReader
        Dim multiplier As Decimal


        Try

            If IcImagingControl1.Device = "" Then Exit Sub

            multiplier = Val(tbMultiplier.Text)
            filename = String.Format("masterDark{0}.raw", IcImagingControl1.Device)
            Dim fStream As New FileStream(filename, FileMode.Open)
            bReader = New BinaryReader(fStream)
            ReDim dark(fStream.Length / 2)
            For i = 0 To fStream.Length / 2 - 1
                dark(i) = bReader.ReadUInt16()
                dark(i) = dark(i) * multiplier
            Next
            fStream.Close()

        Catch ex As Exception
            MsgBox("problem reading dark file")
        End Try




    End Sub


    Public Sub processDetection()
        Dim aQE As queueEntry
        While (meteorCheckRunning)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()

                Functions.CallAzureMeteorDetection(aQE)


                aQE = Nothing

            End If
            ' Console.WriteLine("in the queue:{0}", myDetectionQueue.Count)
            Thread.Sleep(100)
        End While

    End Sub
    Public Async Function CallAzureMeteorDetection(qe As queueEntry) As Task


        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.199:7071/api/detection"
        Dim myUriBuilder As New UriBuilder(apiURL)


        Dim query As NameValueCollection = Web.HttpUtility.ParseQueryString(String.Empty)

        query("file") = qe.filename
        query("dateTaken") = qe.dateTaken.ToString("MM/dd/yyyy hh:mm tt")
        query("cameraID") = qe.cameraID
        query("width") = qe.width
        query("height") = qe.height
        myUriBuilder.Query = query.ToString


        Dim client As New HttpClient()

        Dim byteContent = New ByteArrayContent(qe.img)
        Try


            Dim response = client.PostAsync(myUriBuilder.ToString, byteContent)
            Dim responseString = response.Result
            byteContent = Nothing

        Catch ex As Exception
            Console.WriteLine("calling meteor detection:" & ex.Message)
        End Try
    End Function


    'Public Sub GrabFrame()
    '    Dim width, height As UInteger
    '    Dim err As QCamM_Err = QCamM_Err.qerrSuccess
    '    Dim sizeInBytes As UInteger = 0
    '    QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, sizeInBytes)
    '    err = QCam.QCamM_GrabFrame(mhCamera, mFrame1)
    '    width = mFrame1.width
    '    height = mFrame1.height
    '    Dim bmp As Bitmap = Nothing

    '    If mIsMono Then
    '        bmp = New Bitmap(CInt(width), CInt(height), CInt(width), PixelFormat.Format8bppIndexed, mFrame1.pBuffer)
    '        Dim pt As ColorPalette = bmp.Palette

    '        For i As Integer = 0 To pt.Entries.Length - 1
    '            pt.Entries(i) = Color.FromArgb(i, i, i)
    '        Next

    '        bmp.Palette = pt
    '    Else
    '        QCamImgfnc.QCamM_BayerToRgb(QCamM_qcBayerInterp.qcBayerInterpFast, mFrame1, mRgbFrame)
    '        bmp = New Bitmap(CInt(width), CInt(height), CInt(width) * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer)
    '    End If

    '    mDisplayBitmap = bmp
    '    PictureBox1.Image = bmp

    '    Try

    '        Using fs As FileStream = New FileStream("image.raw", FileMode.Create)
    '            Dim bw As BinaryWriter = New System.IO.BinaryWriter(fs)

    '            For i As Integer = 0 To mFrame1.size - 1 Step 1
    '                ' bw.Write(Marshal.ReadByte(mFrame1.pBuffer, i + 1))
    '                bw.Write(Marshal.ReadByte(mFrame1.pBuffer, i))
    '            Next

    '            bw.Close()
    '        End Using

    '    Catch e As Exception
    '        MessageBox.Show("Unable to save the image data: " & e.Message)
    '    End Try
    'End Sub

    'Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
    '    myCam.ExposureTime = Val(tbExposureTime.Text) / 1000 ' expecting ms
    '    ' myCam.ReadCameraParams()

    '    If Not myCam.AcqSetup(pvcam_helper.PVCamCamera.AcqTypes.ACQ_TYPE_CONTINUOUS) Then
    '        Return
    '    End If

    '    If myCam.ReadoutTime <> 0 Then
    '    Else
    '    End If
    '    ' myCam.ReadCameraParams()
    '    myCam.FramesToGet = myCam.RUN_UNTIL_STOPPED
    '    If Not myCam.StartContinuousAcquisition() Then
    '        Return
    '    End If
    '    ''use settings
    '    'Dim err As QCamM_Err
    '    'm_camRunning = True

    '    'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Text)))


    '    'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
    '    'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)

    '    'If lblDayNight.Text = "night" Then
    '    '    StartStream()
    '    'Else
    '    '    TimerAcquistionRate.Enabled = True
    '    'End If

    '    Button7.Enabled = False
    '    Button8.Enabled = True

    '    startTime = Now
    '    meteorCheckRunning = True
    '    Timer2.Enabled = True
    '    t = New Thread(AddressOf processDetection)
    '    t.Start()
    'End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        m_camRunning = False
        ' rc = Api.SetStreamState(h_camera, StreamState.Stop)
        TimerAcquistionRate.Enabled = False
        't.Abort()
        IcImagingControl1.LiveStop()
        meteorCheckRunning = False
        Button7.Enabled = True
        Button8.Enabled = False
    End Sub

    Private Sub frmIS_Load(sender As Object, e As EventArgs) Handles Me.Load
        getCameraReady()
        myImageCodecInfo = GetEncoderInfo("image/jpeg")

        ' Create an Encoder object based on the GUID
        ' for the Quality parameter category.
        myEncoder = System.Drawing.Imaging.Encoder.Quality

        ' Create an EncoderParameters object.
        ' An EncoderParameters object has an array of EncoderParameter
        ' objects. In this case, there is only one
        ' EncoderParameter object in the array.
        myEncoderParameters = New EncoderParameters(1)

        ' Save the bitmap as a JPEG file with quality level 25.
        myEncoderParameter = New EncoderParameter(myEncoder, CType(99L, Int32))
        myEncoderParameters.Param(0) = myEncoderParameter
        ' md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")
    End Sub


    Private Sub getCameraReady()
        'try to open camera

        If Not IcImagingControl1.DeviceValid() Then
            IcImagingControl1.ShowDeviceSettingsDialog()

            If Not IcImagingControl1.DeviceValid Then
                MsgBox("No device was selected.", MsgBoxStyle.Information, "Grabbing an Image")

                Me.Close()
            End If
        End If
        IcImagingControl1.DeviceFrameRate = 0.2
        IcImagingControl1.DeviceFrameFilters.Clear()
        VCDProp = TIS.Imaging.VCDHelpers.VCDSimpleModule.GetSimplePropertyContainer(IcImagingControl1.VCDPropertyItems)
    End Sub





    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Dim seconds As Integer

        seconds = DateDiff(DateInterval.Second, startTime, Now)
        txtFps.Text = frames / seconds
    End Sub
    Private Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
        Dim j As Integer
        Dim encoders() As ImageCodecInfo
        encoders = ImageCodecInfo.GetImageEncoders()

        j = 0
        While j < encoders.Length
            If encoders(j).MimeType = mimeType Then
                Return encoders(j)
            End If
            j += 1
        End While
        Return Nothing

    End Function 'GetEncoderInfo



    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Button5.Enabled = False
        Button6.Enabled = True
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"
    End Sub

    Public Function getLastImage() As Bitmap
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While m_grabbing AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()
        Dim x As New Bitmap(iWidth, iHeight, Imaging.PixelFormat.Format24bppRgb)
        Dim BoundsRect = New Rectangle(0, 0, iWidth, iHeight)
        Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], x.PixelFormat)
        Dim ptr As IntPtr = bmpData.Scan0
        System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, iWidth * iHeight * 3 - 1) 'copy into bitmap


        x.UnlockBits(bmpData)
        Return x



    End Function
    Public Function getLastImageArray() As Byte()
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While m_grabbing AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()
        Return m_pics.ImageBytes


    End Function
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button5.Enabled = True
        Button6.Enabled = False
        myWebServer.StopWebServer()
    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Try



            Dim currentMode As Boolean
            currentMode = False

            If Now.Hour >= cboNight.SelectedItem Or Now.Hour <= cboDay.SelectedItem Then
                night = True
            Else
                night = False
            End If
            ' If currentMode <> night Then

            If night Then
                'axfgcontrolctrl2.ExposureTimeAuto = "Off"
                '  axfgcontrolctrl2.AcquisitionMode = "Continuous"

                tbExposureTime.Text = tbNightExp.Text
                tbGain.Text = tbNightAgain.Text
                lblDayNight.Text = "night"
                'night mode


            Else
                'day mode

                tbExposureTime.Text = tbDayTimeExp.Text
                tbGain.Text = tbDayGain.Text
                lblDayNight.Text = "day"


            End If
            'End If
            ' Dim err As QCamM_Err
            'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbNightAgain.Text)))
            ' QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
            'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        Catch ex As Exception

        End Try
    End Sub



    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        'Dim err As QCamM_Err

        'If Not mSettings Is Nothing Then
        '    Debug.Print("setting gain to " & tbGain.Text)
        '    Debug.Print("setting exposure to " & tbExposureTime.Text)
        '    If lblDayNight.Text = "day" Then
        '        QCam.QCamM_Abort(mhCamera) 'stop exposing
        '        QCam.QCamM_SetStreaming(mhCamera, 0) 'stop streaming
        '        If m_camRunning Then
        '            TimerAcquistionRate.Enabled = True
        '        End If

        '    Else
        '        TimerAcquistionRate.Enabled = False
        '        If m_camRunning Then
        '            StartStream()

        '        End If
        '    End If

        '    QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Text)))
        '    QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
        '    err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        'End If

    End Sub

    Private Sub frmIS_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim i As Object

        If hdialog <> 0 Then
            PCO_CloseDialogCam(hdialog)
        End If

        PCO_SetRecordingState(hdriver, 0)
        PCO_FreeBuffer(hdriver, nBuf) 'essential call, otherwise you'll get a memory leak
        errorCode = PCO_CloseCamera(hdriver)
        hdriver = 0
        i = errorCode



    End Sub



    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        'use settings

        m_camRunning = True


        'VCDProp.RangeValue(VCDIDs.VCDID_Exposure) = -1
        Dim AbsValItf As TIS.Imaging.VCDAbsoluteValueProperty


        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = tbExposureTime.Text / 1000

        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Gain + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = tbGain.Text


        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Saturation + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = 125

        IcImagingControl1.LiveStart()




        Button7.Enabled = False
        Button8.Enabled = True

        startTime = Now
        meteorCheckRunning = True
        Timer2.Enabled = True

    End Sub

    Private Sub IcImagingControl1_SizeChanged(sender As Object, e As EventArgs) Handles IcImagingControl1.SizeChanged

    End Sub

    Friend WithEvents IcImagingControl1 As TIS.Imaging.ICImagingControl


End Class

