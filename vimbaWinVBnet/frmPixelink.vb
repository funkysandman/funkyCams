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
Imports Photometrics.Pvcam
Imports System.Collections.Specialized
Imports vimbaWinVBnet.vimbaWinVBnet
Imports PixeLINK

Public Class frmPixelink
    Dim myDetectionQueue As New Queue(Of queueEntry)

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
    Private dark() As Byte
    Private t As Thread
    Private t_imaging As Thread
    Private h_camera As Integer
    Private rc As ReturnCode
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
    Private frameDesc As FrameDescriptor
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

    Private Function InitCamera() As Boolean
        'Dim ccdType As UInteger = 0
        'Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        ''msgbox("initCamera")
        'mSettings = New QCamM_SettingsEx()
        ''msgbox("new settings")
        'Try
        '    QCam.QCamM_CreateCameraSettingsStruct(mSettings)
        '    'msgbox("created settings")
        'Catch ex As Exception
        '    'msgbox(ex.Message)
        'End Try


        'Try
        '    'msgbox("init cam settings")
        '    ' QCam.QCamM_ReadSettingsFromCam(mhCamera, mSettings)
        '    'QCam.QCamM_InitializeCameraSettings(mhCamera, mSettings)
        'Catch ex As Exception
        '    'msgbox(ex.Message)
        'End Try

        ''msgbox("read settings")
        'Try
        '    QCam.QCamM_ReadDefaultSettings(mhCamera, mSettings)
        'Catch ex As Exception
        '    'msgbox(ex.Message)
        'End Try

        ''msgbox("stage1")
        ''err = QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfCcdType, ccdType)
        '''msgbox("stage1a")
        ''If ccdType = Convert.ToUInt32(QCamM_qcCcdType.qcCcdColorBayer) Then
        ''    err = QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32(QCamM_ImageFormat.qfmtBayer8))
        ''    mIsMono = False
        ''ElseIf ccdType = Convert.ToUInt32(QCamM_qcCcdType.qcCcdMonochrome) Then
        ''    err = QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32(QCamM_ImageFormat.qfmtMono8))
        ''    'msgbox("stage1b")
        ''    mIsMono = True
        ''Else
        ''    Return False
        ''End If
        ''msgbox("stage2")
        'mIsMono = True
        'Dim frameSize As UInteger = 0
        'QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, frameSize)
        'frameSize = frameSize '* 2
        ''msgbox("making mFrame1")
        'mFrame1 = New QCamM_Frame()
        'mFrame1.bufferSize = frameSize
        'mFrame1.pBuffer = QCam.QCamM_Malloc(mFrame1.bufferSize)
        'mFrame2 = New QCamM_Frame()
        'mFrame2.bufferSize = frameSize
        'mFrame2.pBuffer = QCam.QCamM_Malloc(mFrame2.bufferSize)
        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmReadoutSpeed, 1)

        ''QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmOffset, 258)
        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmCoolerActive, 1)
        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmEMGain, tbGain.Text)
        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
        ''msgbox("stage3")
        ''QCam.QCamM_SetParam(mSettings, QCamM_Param., tbExposureTime.Text)
        'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        ''If Not mIsMono Then
        ''    mRgbFrame = New QCamM_Frame()
        ''    mRgbFrame.bufferSize = frameSize * 3
        ''    mRgbFrame.pBuffer = QCam.QCamM_Malloc(mRgbFrame.bufferSize)
        ''    mRgbFrame.format = CUInt(QCamM_ImageFormat.qfmtBgr24)
        ''End If

        ''If QCam.QCamM_IsParamSupported(mhCamera, QCamM_Param.qprmGain) = QCamM_Err.qerrSuccess Then
        ''    gbGain.Enabled = True
        ''    Dim val As UInteger = 0
        ''    QCam.QCamM_GetParamMin(mSettings, QCamM_Param.qprmGain, val)
        ''    tbGain.Minimum = CInt(val)
        ''    QCam.QCamM_GetParamMax(mSettings, QCamM_Param.qprmGain, val)
        ''    tbGain.Maximum = CInt(val)
        ''    QCam.QCamM_GetParam(mSettings, QCamM_Param.qprmGain, val)
        ''    tbGain.Value = CInt(val)
        ''Else
        ''    gbGain.Enabled = False
        ''End If

        ''If QCam.QCamM_IsParamSupported(mhCamera, QCamM_Param.qprmExposure) = QCamM_Err.qerrSuccess Then
        ''    gbExposure.Enabled = True
        'Dim val As UInteger = 0
        'QCam.QCamM_GetParam(mSettings, QCamM_Param.qprmExposure, val)
        '' tbExposure.Value = CInt((val / 1000))
        ''Else
        ''    gbExposure.Enabled = False
        ''End If

        ''msgbox("stage4")
        ''  lblCameraModel.Text = "Model: " & (CType((mCamList(0).cameraType), QCamM_qcCameraType)).ToString().Remove(0, 8)
        '' Dim serNum As String = ""
        '' QCam.QCamM_GetSerialString(mhCamera, serNum)
        ''  lblSerNum.Text = "Serial: " & serNum
        'mFrameCallback = New QCamM_AsyncCallback(AddressOf frameCallback)
        'Return True
    End Function
    'Private Sub frameCallback(ByVal userPtr As IntPtr, ByVal userData As UInteger, ByVal errcode As QCamM_Err, ByVal flags As UInteger)
    '    If errcode <> QCamM_Err.qerrSuccess Then
    '        Debug.Print("framecallback error:" & errcode)
    '        Exit Sub
    '    End If
    '    Dim myFrame As QCamM_Frame
    '    If running Then Exit Sub

    '    running = True
    '    If userData = 1 Then
    '        myFrame = mFrame1
    '    ElseIf userData = 2 Then
    '        myFrame = mFrame2
    '    Else
    '        Return
    '    End If
    '    Debug.Print("frame arrived")
    '    Try
    '        'Dim width As UInteger = myFrame.width
    '        'Dim height As UInteger = myFrame.height

    '        'bmp = New Bitmap(width, height, PixelFormat.Format8bppIndexed)
    '        'Dim rawData(mFrame1.bufferSize) As Byte
    '        'ReDim imageBytes(mFrame1.bufferSize)
    '        If mIsMono Then

    '            If m_pics Is Nothing Then
    '                m_pics = New RingBitmap(5)
    '            End If
    '            m_pics.FillNextBitmap(myFrame)
    '            m_pics.FillNextBitmap(myCam.LastBMP)

    '        End If



    '        Dim filename As String
    '        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
    '        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgq_", DateTime.Now)
    '        filename = Path.Combine(Me.tbPath.Text, folderName, filename)



    '        If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
    '            ' md.examine(bm, filename)
    '            'call azure service
    '            Dim ms As New MemoryStream()
    '            m_pics.Image.Save(ms, ImageFormat.Bmp)

    '            Dim contents = ms.ToArray()
    '            Dim qe As New queueEntry
    '            qe.img = contents
    '            qe.filename = Path.GetFileName(filename)
    '            If myDetectionQueue.Count < 10 Then
    '                myDetectionQueue.Enqueue(qe)
    '            End If

    '            ms.Close()

    '        End If
    '        If Me.cbSaveImages.Checked = True Then
    '            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


    '            m_pics.Image.Save(filename, myImageCodecInfo, myEncoderParameters)


    '        End If

    '        QueueFrame(userData)
    '        running = False
    '    Catch ex As Exception
    '        Debug.Print(ex.Message)
    '        running = False
    '    End Try
    '    Dim err As QCamM_Err
    '    QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Text)))
    '    QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
    '    err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
    '    mDisplayPanel.Invalidate()
    'End Sub
    'Private Sub singleframeCallback(ByVal userPtr As IntPtr, ByVal userData As UInteger, ByVal errcode As QCamM_Err, ByVal flags As UInteger)
    '    'Debug.Print("singleframecallback")
    '    'If errcode <> QCamM_Err.qerrSuccess Then
    '    '    Debug.Print("framecallback error:" & errcode)
    '    '    Exit Sub
    '    'End If
    '    'Dim myFrame As QCamM_Frame
    '    'If running Then Exit Sub

    '    'running = True
    '    'If userData = 1 Then
    '    '    myFrame = mFrame1
    '    'ElseIf userData = 2 Then
    '    '    myFrame = mFrame2
    '    'Else
    '    '    Return
    '    'End If
    '    'Debug.Print("frame arrived")
    '    'Try
    '    '    'Dim width As UInteger = myFrame.width
    '    '    'Dim height As UInteger = myFrame.height

    '    '    'bmp = New Bitmap(width, height, PixelFormat.Format8bppIndexed)
    '    '    'Dim rawData(mFrame1.bufferSize) As Byte
    '    '    'ReDim imageBytes(mFrame1.bufferSize)
    '    '    mIsMono = True
    '    '    If mIsMono Then
    '    '        'Dim ncp As System.Drawing.Imaging.ColorPalette = bmp.Palette
    '    '        'For j As Integer = 0 To 255
    '    '        '    ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
    '    '        'Next
    '    '        'bmp.Palette = ncp
    '    '        'Marshal.Copy(myFrame.pBuffer, rawData, 0, mFrame1.bufferSize)
    '    '        'Marshal.Copy(myFrame.pBuffer, imageBytes, 0, mFrame1.bufferSize)

    '    '        ''
    '    '        'Dim BoundsRect = New Rectangle(0, 0, width, height)
    '    '        'Dim bmpData As System.Drawing.Imaging.BitmapData = bmp.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], bmp.PixelFormat)

    '    '        'Dim ptr As IntPtr = bmpData.Scan0

    '    '        'Dim bytes As Integer = mFrame1.bufferSize
    '    '        If m_pics Is Nothing Then
    '    '            m_pics = New RingBitmap(5)
    '    '        End If
    '    '        m_pics.FillNextBitmap(myFrame)

    '    '        'Marshal.Copy(rawData, 0, ptr, bytes)
    '    '        'bmp.UnlockBits(bmpData)

    '    '        ' debug.Print("image")



    '    '        ' bmp = New Bitmap(New MemoryStream(rawData))

    '    '    End If

    '    '    'Else
    '    '    '    QCamImgfnc.QCamM_BayerToRgb(QCamM_qcBayerInterp.qcBayerInterpFast, myFrame, mRgbFrame)
    '    '    '    bmp = New Bitmap(CInt(width), CInt(height), CInt(width) * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer)
    '    '    'End If

    '    '    'Using fs As FileStream = New FileStream("image.raw", FileMode.Create)
    '    '    '    Dim bw As BinaryWriter = New System.IO.BinaryWriter(fs)
    '    '    '    Dim b As Byte
    '    '    '    For i As Integer = 0 To mFrame1.size - 1
    '    '    '        b = Marshal.ReadByte(myFrame.pBuffer, i)
    '    '    '        bw.Write(b)

    '    '    '    Next
    '    '    '    bw.Close()
    '    '    'End Using
    '    '    '    mDisplayBitmap = bmp

    '    '    ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
    '    '    Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
    '    '    Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)
    '    '    'b = bm.Clone


    '    '    'try to draw on bitmap
    '    '    'Dim tempbmp As Bitmap
    '    '    'tempbmp = New Bitmap(bmp.Width, bmp.Height)


    '    '    'From this bitmap, the graphics can be obtained, because it has the right PixelFormat
    '    '    ' Dim gr As Graphics = Graphics.FromImage(tempbmp)
    '    '    ' gr.DrawImage(bmp, 0, 0)
    '    '    'Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
    '    '    'Dim myBrushLabels As New SolidBrush(Color.White)
    '    '    'bmp.RotateFlip(RotateFlipType.Rotate180FlipNone)

    '    '    'Dim odata As BitmapData
    '    '    'Dim odata2 As BitmapData

    '    '    'odata = bmp.LockBits(New Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat)
    '    '    'odata2 = bmp2.LockBits(New Rectangle(0, 0, bmp2.Width, bmp2.Height), ImageLockMode.ReadWrite, bmp2.PixelFormat)
    '    '    'Marshal.Copy()

    '    '    ' gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
    '    '    ' gr.Dispose()
    '    '    ' myFontLabels.Dispose()
    '    '    ' bmp = tempbmp



    '    '    'PictureBox1.Image = bmp

    '    '    Dim filename As String
    '    '    Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
    '    '    filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgq_", DateTime.Now)
    '    '    filename = Path.Combine(Me.tbPath.Text, folderName, filename)



    '    '    If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
    '    '        ' md.examine(bm, filename)
    '    '        'call azure service
    '    '        Dim ms As New MemoryStream()
    '    '        m_pics.Image.Save(ms, ImageFormat.Bmp)

    '    '        Dim contents = ms.ToArray()
    '    '        Dim qe As New queueEntry
    '    '        qe.img = contents
    '    '        qe.filename = Path.GetFileName(filename)
    '    '        myDetectionQueue.Enqueue(qe)

    '    '        ms.Close()

    '    '    End If
    '    '    If Me.cbSaveImages.Checked = True Then
    '    '        System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


    '    '        m_pics.Image.Save(filename, myImageCodecInfo, myEncoderParameters)


    '    '    End If


    '    '    running = False
    '    'Catch ex As Exception
    '    '    Debug.Print(ex.Message)
    '    '    running = False
    '    'End Try
    '    'Dim err As QCamM_Err
    '    'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Text)))
    '    'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
    '    'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
    '    'mDisplayPanel.Invalidate()
    'End Sub
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
        rc = Api.SetStreamState(h_camera, StreamState.Stop)
        TimerAcquistionRate.Enabled = False
        t.Abort()

        meteorCheckRunning = False
        Button7.Enabled = True
        Button8.Enabled = False
    End Sub

    Private Sub frmPixelink_Load(sender As Object, e As EventArgs) Handles Me.Load
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
    Private Sub SetPixelFormat(ByVal hCamera As Integer, ByVal pixelFormat As PixeLINK.PixelFormat)
        Dim flags As FeatureFlags = 0
        Dim numParms As Integer = 1
        Dim parms As Single() = New Single(numParms - 1) {}
        Api.GetFeature(hCamera, Feature.PixelFormat, flags, numParms, parms)
        parms(0) = CSng(pixelFormat)
        Api.SetFeature(hCamera, Feature.PixelFormat, flags, numParms, parms)
    End Sub
    Private Sub SetExposure(ByVal hCamera As Integer, ByVal expTime As Integer)
        Dim flags As FeatureFlags = 0
        Dim numParms As Integer = 1
        Dim parms As Single() = New Single(numParms - 1) {}

        rc = Api.GetFeature(hCamera, Feature.FrameRate, flags, 1, parms)
        If (expTime > 1000) Then 'slow frame rate

            rc = Api.SetFeature(hCamera, Feature.FrameRate, flags.Off, 0, parms)
        Else
            ' rc = Api.SetFeature(hCamera, Feature.FrameRate, flags.Manual, 0, parms)
            parms(0) = 0.2 'slow during the daytime
            rc = Api.SetFeature(hCamera, Feature.FrameRate, flags.Manual, 1, parms)
        End If

        rc = Api.GetFeature(hCamera, Feature.Exposure, flags, numParms, parms)
        parms(0) = CSng(expTime / 1000)
        rc = Api.SetFeature(hCamera, Feature.Exposure, flags, numParms, parms)
    End Sub
    Private Sub SetGain(ByVal hCamera As Integer, ByVal gain As Integer)
        Dim flags As FeatureFlags = 0
        Dim numParms As Integer = 1
        Dim parms As Single() = New Single(numParms - 1) {}

        rc = Api.GetFeature(hCamera, Feature.Gain, flags, 1, parms)

        parms(0) = gain
        rc = Api.SetFeature(hCamera, Feature.FrameRate, flags.Manual, 1, parms)

    End Sub
    Private Sub getCameraReady()
        'try to open camera


        Dim rc As ReturnCode


        rc = Api.Initialize(0, h_camera)
        If Not Api.IsSuccess(rc) Then
                MessageBox.Show("ERROR: Unable to initialize a camera (Error " + rc.ToString() + ")")
                Return
            End If

        helper = New SnapshotHelper(h_camera)

        'set pixeltype
        SetPixelFormat(h_camera, PixeLINK.PixelFormat.Bayer8)



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

    Private Sub frmPixelink_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
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

    Private Sub TimerAcquistionRate_Tick(sender As Object, e As EventArgs) Handles TimerAcquistionRate.Tick
        'take a picture
        If m_grabbing Then
            Exit Sub
        End If
        'm_grabbing = True
        'Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        'Dim sizeInBytes As UInteger = 0
        'Dim flags As Integer = 0
        ''QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, sizeInBytes)
        'Try
        '    If Not mFrame1 Is Nothing Then
        '        'msgbox("grabbing frame")
        '        Dim frameSize As UInteger = 0
        '        QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, frameSize)
        '        QCam.QCamM_Free(mFrame1.pBuffer)
        '        mFrame1.pBuffer = QCam.QCamM_Malloc(mFrame1.bufferSize)
        '        Debug.Print("grab frame")
        '        Dim t_grab As Thread
        '        m_grabbedframe = False
        '        t_grab = New Thread(AddressOf grabAframe)
        '        t_grab.Start()
        '        Dim stopWatch As Stopwatch = New Stopwatch()
        '        stopWatch.Start()

        '        While Not m_grabbedframe AndAlso stopWatch.ElapsedMilliseconds < 20000
        '            Threading.Thread.Sleep(1000)
        '            Application.DoEvents()
        '        End While

        '        stopWatch.[Stop]()
        '        If m_grabbedframe_err = QCamM_Err.qerrDriverFault Then
        '            m_grabbedframe = False
        '        End If
        '        If Not m_grabbedframe Then
        '            QCam.QCamM_Abort(mhCamera)
        '            t_grab.Abort()

        '            err = QCamM_Err.qerrCancelled
        '            Debug.Print("programmatically aborted grabframe thread")
        '            'what to do?  close camera?
        '            QCam.QCamM_CloseCamera(mhCamera)
        '            Debug.Print("closed camera")
        '            QCam.QCamM_Free(mFrame1.pBuffer)
        '            Debug.Print("released mframe1 buffer")
        '            QCam.QCamM_Free(mFrame2.pBuffer)
        '            Debug.Print("released mframe2 buffer")
        '            'QCam.QCamM_ReleaseDriver()
        '            'Debug.Print("released driver")
        '            'now reopen
        '            getCameraReady()
        '            Debug.Print("recovering....")
        '            m_grabbing = False
        '            Exit Sub
        '        End If


        '    Else
        '            TimerAcquistionRate.Enabled = False

        '        'msgbox("mframe1 is noting")
        '    End If

        'Catch ex As Exception
        '    'msgbox(ex.Message)
        '    'msgbox("QCam error:" & err)
        '    TimerAcquistionRate.Enabled = False

        'End Try

        'If err = QCamM_Err.qerrSuccess And m_grabbedframe_err = 0 Then
        '    Call Me.singleframeCallback(mFrame1.pBuffer, 1, err, flags)
        'End If
        m_grabbing = False

    End Sub


    Private Function GetSimpleFeature(ByVal hCamera As Integer, ByVal featureId As Feature) As Single
        Dim flags As FeatureFlags = 0
        Dim numParams As Integer = 1
        Dim params(numParams - 1) As Single
        rc = Api.GetFeature(hCamera, featureId, flags, numParams, params)
        Return params(0)

    End Function

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        'use settings

        m_camRunning = True

        'start camera
        'txtExposure.Text = System.Convert.ToString(GetSimpleFeature(h_camera, Feature.Shutter) * 1000)
        'txtGain.Text = System.Convert.ToString(GetSimpleFeature(h_camera, Feature.Gain))




        SetExposure(h_camera, tbExposureTime.Text)








        Button7.Enabled = False
        Button8.Enabled = True

        startTime = Now
        meteorCheckRunning = True
        Timer2.Enabled = True
        t = New Thread(AddressOf processDetection)
        t.Start()
        t_imaging = New Thread(AddressOf pullImages)
        t_imaging.Start()
    End Sub

    Sub pullImages()
        Dim s_callbackDelegate
        m_camRunning = True
        s_callbackDelegate = New Api.Callback(AddressOf getFrame)
        Api.SetCallback(h_camera, Overlays.Frame, 0, s_callbackDelegate)
        rc = Api.SetStreamState(h_camera, StreamState.Start)

        If Not Api.IsSuccess(rc) Then
            System.Console.WriteLine("ERROR: Unable to start streaming (Error " + rc.ToString() + ")")
            Return
        End If
        'rc = Api.GetClip(h_camera, 1, "test.bmp", callbackDelegate)
        Do While (m_camRunning)

            '  rc = Api.GetNextFrame(h_camera, bufferSize, rawImage, frameDesc)

            ' If (Not Api.IsSuccess(rc)) Then
            '     System.Console.WriteLine("ERROR: Unable to setup clip")
            '     '       Exit Sub
            ' End If

            '// Poll here until we can see that the callback function has been called

            System.Threading.Thread.Sleep(500)

        Loop

    End Sub
    Function getFrame(hc As Integer, pBuf As IntPtr, pf As PixeLINK.PixelFormat, ByRef frameDesc As FrameDescriptor, userData As Integer)

        'put image into rin                bmBild = New Bitmap(iXres, iYres, Imaging.PixelFormat.Format24bppRgb)
        SyncLock m_syncLock
            Dim rect As New Rectangle(0, 0, frameDesc.RoiWidth, frameDesc.RoiHeight)
            bmBild = New Bitmap(frameDesc.RoiWidth, frameDesc.RoiHeight, Imaging.PixelFormat.Format24bppRgb)
            Dim bmpData As System.Drawing.Imaging.BitmapData = bmBild.LockBits(rect,
                        Drawing.Imaging.ImageLockMode.ReadWrite, bmBild.PixelFormat)
            iWidth = frameDesc.RoiWidth
            iHeight = frameDesc.RoiHeight

            Dim isize As Integer
            Dim iPtr As IntPtr
            iPtr = bmpData.Scan0
            isize = iWidth * iHeight * 3
            Dim Image24(iWidth * iHeight) As Byte

            Dim newsize As Integer
            Marshal.Copy(pBuf, Image24, 0, iWidth * iHeight - 1)
            'convert from bayer8 to 24rgb
            'File.WriteAllBytes("image.raw", Image24)

            rc = Api.FormatImage(Image24, frameDesc, PixeLINK.ImageFormat.Bmp, Nothing, newsize)
            Dim outImage(newsize) As Byte

            rc = Api.FormatImage(Image24, frameDesc, PixeLINK.ImageFormat.Bmp, outImage, newsize)





            'File.WriteAllBytes("imageout.raw", outImage)
            Marshal.Copy(outImage, 0, iPtr, isize - 1)
            bmBild.UnlockBits(bmpData)

            bmBild.Dispose()

            If m_pics Is Nothing Then
                m_pics = New RingBitmap(5)
            End If

            m_pics.FillNextBitmap(outImage)


            ' imageCnt += 1




            ' Must manually release the image to prevent buffers on the camera stream from filling up
            '  image.Release()
            Dim filename As String

            Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
            filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgpl_", DateTime.Now)
            filename = Path.Combine(tbPath.Text, folderName, filename)



            If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
                ' md.examine(bm, filename)
                'call azure service
                Dim ms As New MemoryStream()
                ' convertedImage.ConvertToWriteAbleBitmap()
                Dim b As Bitmap
                b = getLastImage()

                b.Save(ms, myImageCodecInfo, myEncoderParameters)
                b.Dispose()

                Dim contents = ms.ToArray()
                Dim qe As New queueEntry
                qe.img = contents
                qe.filename = Path.GetFileName(filename)
                qe.dateTaken = Now
                qe.cameraID = "PixeLINK"
                qe.width = frameDesc.RoiWidth
                qe.height = frameDesc.RoiHeight
                If myDetectionQueue.Count < 10 Then
                    myDetectionQueue.Enqueue(qe)

                End If

                ms.Close()

            End If
            If cbSaveImages.Checked = True Then
                System.IO.Directory.CreateDirectory(Path.Combine(tbPath.Text, folderName))
                Dim x As Bitmap
                x = getLastImage()

                x.Save(filename, myImageCodecInfo, myEncoderParameters)
                x.Dispose()

            End If


        End SyncLock

        Debug.WriteLine("got frame")
    End Function
    Sub grabImages()

        Dim BpP As Object
        Dim seg As Short
        Dim dwFrst, dwlast As Object
        Dim sbuf As Short
        Dim dwStatusDll As Integer
        Dim dwStatusDrv As Integer
        Dim loopcount As Integer
        Dim check As Integer
        Const mask As Integer = &H8000 'assign 0x00008000 to mask
        '^-& is essential!! Otherwise VB converts this to an int.
        '  This will give 0xFFFF8000 to mask, which is not intended.
        Dim filename As String
        Dim i As Integer
        Dim j As Integer
        Dim span As Integer
        Dim value As Integer
        iXres = Camera.Sensor.Resolution.xAct
        iYres = Camera.Sensor.Resolution.yAct
        m_camRunning = True
        span = (Val(tbUpper.Text) - Val(tbLower.Text)) * divide
        Do While (m_camRunning)


            check = 0
            seg = 1
            dwFrst = 0
            dwlast = 0
            BpP = 16
            sbuf = 0

            errorCode = PCO_AddBufferEx(hdriver, dwFrst, dwlast, sbuf, Camera.Sensor.Resolution.xAct, Camera.Sensor.Resolution.yAct, BpP)
            m_grabbing = True
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

            Dim stopWatch As Stopwatch = New Stopwatch()
            stopWatch.Start()

            While m_grabbing AndAlso stopWatch.ElapsedMilliseconds < 60000 And Not (check)
                errorCode = PCO_GetBufferStatus(hdriver, sbuf, dwStatusDll, dwStatusDrv)
                check = Not ((dwStatusDll And mask) <> mask) ' event flag set?
            End While

            stopWatch.[Stop]()
            If errorCode = 0 Then
                ReDim b(iXres * iYres * 2)

                'Looks easy, but this took some time to work...
                Marshal.Copy(pwbuf, b, 0, iXres * iYres * 2)

                '  Text1.Text = "0x" & Hex(b(10)) & "   0x" & Hex(b(100)) & "   0x" & Hex(b(1000))

                ' span = (maxval.Value - minval.Value) * divide

                If bmBildUsed <> 0 Then
                    bmBild.Dispose()
                End If

                ' Create bitmap
                bmBild = New Bitmap(iXres, iYres, Imaging.PixelFormat.Format24bppRgb)
                Dim rect As New Rectangle(0, 0, bmBild.Width, bmBild.Height)
                Dim bmpData As System.Drawing.Imaging.BitmapData = bmBild.LockBits(rect,
                    Drawing.Imaging.ImageLockMode.ReadWrite, bmBild.PixelFormat)

                ' Create intermediate buffer for direct copying to the bitmap.
                Dim isize As Integer
                isize = Math.Abs(bmpData.Stride) * iYres
                Dim Image24(isize) As Byte
                bmBildUsed = 1
                Dim range As Integer
                range = tbUpper.Text - tbLower.Text
                Dim multiplier As Single
                multiplier = 256 / range
                Dim lower, upper As Integer
                lower = CInt(tbLower.Text)
                upper = CInt(tbUpper.Text)
                j = 0
                For i = 0 To iXres * iYres - 1  ' This loop converts from 16bit to 8bit using min and max
                    value = b(j + 1) * 256 + b(j)
                    value = value >> 2

                    'Debug.Print(value)
                    If value < 0 Then ' Type cast from short to ushort? Forget it: Not with VB
                        value = value * -1
                        value = value + &H8000
                    End If
                    value = value - lower
                    If value < 0 Then
                        value = 0
                    End If
                    value = CInt(CSng(value) * multiplier)


                    'value = value * 255 / span
                    If value > 255 Then
                        value = 255
                    End If
                    If value < 0 Then
                        value = 0
                    End If

                    'stretch image



                    Image24(i * 3 + 0) = value ' Set the values to the intermediate buffer
                    Image24(i * 3 + 1) = value
                    Image24(i * 3 + 2) = value
                    j = j + 2
                Next

                Marshal.Copy(Image24, 0, bmpData.Scan0, isize) ' Copy intermediate buffer to the bitmap

                bmBild.UnlockBits(bmpData)
                If m_pics Is Nothing Then
                    m_pics = New RingBitmap(5)
                End If
                m_pics.FillNextBitmap(Image24)
                Debug.Print("got pco image")
                Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
                filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgpco_", DateTime.Now)
                filename = Path.Combine(Me.tbPath.Text, folderName, filename)


                m_grabbing = False
                If Me.cbMeteors.Checked And Me.lblDayNight.Text.ToLower = "night" Then
                    ' md.examine(bm, filename)
                    'call azure service
                    Dim ms As New MemoryStream()
                    ' convertedImage.ConvertToWriteAbleBitmap()
                    Dim b As Bitmap
                    b = Me.getLastImage

                    b.Save(ms, Me.myImageCodecInfo, Me.myEncoderParameters)
                    b.Dispose()

                    Dim contents = ms.ToArray()
                    Dim qe As New queueEntry
                    qe.img = contents
                    qe.filename = Path.GetFileName(filename)
                    qe.dateTaken = Now
                    qe.cameraID = "PCO.2000"
                    qe.width = iXres
                    qe.height = iYres
                    If Me.myDetectionQueue.Count < 10 Then
                        Me.myDetectionQueue.Enqueue(qe)

                    End If

                    ms.Close()

                End If
                If Me.cbSaveImages.Checked = True Then
                    System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


                    bmBild.Save(filename, Me.myImageCodecInfo, Me.myEncoderParameters)
                    bmBild.Dispose()

                End If




                'Picture.SizeMode = PictureBoxSizeMode.StretchImage ' Stretch it!
                'Picture.Image = bmBild ' Set bitmap to PictureBox
                'Picture.Refresh()  ' Show da image
                'bmBild.Dispose() Don't do this here, since it will be still in use by the PictureBox
                ' and guess what might happen...
            End If
        Loop
    End Sub



End Class

