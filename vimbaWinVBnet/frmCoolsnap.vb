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

Public Class frmCoolsnap
    Dim myDetectionQueue As New Queue(Of queueEntry)
    Public myCam As pvcam_helper.PVCamCamera
    '  Private mCamList As QCamM_CamListItem()
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
    Dim night As Boolean = False
    ' Private md As New ObjectDetection.TFDetector()
    Private myImageCodecInfo As ImageCodecInfo
    Private myEncoder As System.Drawing.Imaging.Encoder
    Private myEncoderParameter As EncoderParameter
    Private myEncoderParameters As EncoderParameters
    Private meteorCheckRunning As Boolean = False
    Private m_camRunning As Boolean = False
    Private m_grabbing As Boolean = False

    Shared m_pics As RingBitmap
    Private m_grabbedframe As Boolean
    Private m_grabbedframe_err As Integer = 0



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

        Public Sub FillNextBitmap(b As Bitmap)
            SwitchBitmap()
            m_Bitmaps(m_BitmapSelector) = b

        End Sub
        'Public Sub FillNextBitmap(frame As UShort())

        '    ' switch to Bitmap object which Is currently Not in use by GUI
        '    SwitchBitmap()
        '    Debug.Print("fillnextbitmap bitmapselector: " & m_BitmapSelector)
        '    Try

        '        If (m_Bitmaps(m_BitmapSelector) Is Nothing) Then
        '            Debug.Print("making new bitmap")
        '            m_Bitmaps(m_BitmapSelector) = New Bitmap(pvcam_helper.PVCamCamera.width, frame.height, PixelFormat.Format8bppIndexed)
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

                CallAzureMeteorDetection(aQE)


                aQE = Nothing

            End If
            Console.WriteLine("in the queue:{0}", myDetectionQueue.Count)
            Thread.Sleep(100)
        End While

    End Sub
    Public Async Function CallAzureMeteorDetection(qe As queueEntry) As Task


        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.192:7071/api/detection"
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

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        myCam.ExposureTime = Val(tbExposureTime.Text) / 1000 ' expecting ms
        ' myCam.ReadCameraParams()

        If Not myCam.AcqSetup(pvcam_helper.PVCamCamera.AcqTypes.ACQ_TYPE_CONTINUOUS) Then
            Return
        End If

        If myCam.ReadoutTime <> 0 Then
        Else
        End If
        ' myCam.ReadCameraParams()
        myCam.FramesToGet = myCam.RUN_UNTIL_STOPPED
        If Not myCam.StartContinuousAcquisition() Then
            Return
        End If
        ''use settings
        'Dim err As QCamM_Err
        'm_camRunning = True

        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Text)))


        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
        'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)

        'If lblDayNight.Text = "night" Then
        '    StartStream()
        'Else
        '    TimerAcquistionRate.Enabled = True
        'End If

        Button7.Enabled = False
        Button8.Enabled = True

        startTime = Now
        meteorCheckRunning = True
        Timer2.Enabled = True
        t = New Thread(AddressOf processDetection)
        t.Start()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        m_camRunning = False
        TimerAcquistionRate.Enabled = False
        t.Abort()
        myCam.StopAcquisition()
        meteorCheckRunning = False
        Button7.Enabled = True
        Button8.Enabled = False
    End Sub

    Private Sub frmCoolsnap_Load(sender As Object, e As EventArgs) Handles Me.Load
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
        'If QCamM_Err.qerrSuccess <> QCam.QCamM_LoadDriver() Then
        '    System.Windows.Forms.MessageBox.Show("The application was unable to load the QCam driver.")
        '    System.Environment.[Exit](0)
        'End If

        'If Not OpenCamera() Then
        '    'msgbox("cannot open camera")
        '    System.Windows.Forms.MessageBox.Show("The application was unable to connect to a QImaging camera.  Please ensure one is connected and turned on before running this application.")
        '    System.Environment.[Exit](0)
        'Else
        '    'msgbox("openned camera")
        'End If

        'If Not InitCamera() Then
        '    'msgbox("can't init camaera")


        '    System.Windows.Forms.MessageBox.Show("Failed to initialize the camera")
        '    System.Environment.[Exit](0)

        'Else
        '    If Not mFrame1 Is Nothing Then
        '        'msgbox("frame is not nothing")
        '    Else
        '        'msgbox("frame is ok")
        '    End If

        'End If
        myCam = New pvcam_helper.PVCamCamera()
        AddHandler myCam.ReportMsg, AddressOf ReportReceived
        AddHandler myCam.CamNotif, AddressOf CameraNotificationReceived
        pvcam_helper.PVCamCamera.RefreshCameras(myCam)
        If (pvcam_helper.PVCamCamera.OpenCamera(pvcam_helper.PVCamCamera.CameraList(0), myCam)) Then
            myCam.ReadCameraParams()



            'SubscribeToReportMessages(myCam)
            'SubscribeToAcquisitionNotifications(myCam)

            'If (pvcam_helper.PVCamCamera.OpenCamera(pvcam_helper.PVCamCamera.CameraList(0), myCam)) Then
            ' highest gain
            'myCam.ReadCameraParams()
            myCam.SetClockingMode("Alternate Normal")
            myCam.SetClearMode("Pre-Exposure")
            myCam.SetClearCycles(2)
            myCam.SetEMGain(1)
            myCam.SetReadoutSpeed(1) '10Mhz
            myCam.SetGainState(2)


        End If


    End Sub
    Private Sub ReportReceived(ByVal pvcc As pvcam_helper.PVCamCamera, ByVal rm As pvcam_helper.ReportMessage)
        rm.Type = rm.Type
        Console.WriteLine(rm.Message)
        Debug.Print(rm.Message)
        ' tl.LogMessage("Camera", rm.Message)
    End Sub

    Private Sub CameraNotificationReceived(ByVal pvcc As pvcam_helper.PVCamCamera, ByVal evtType As pvcam_helper.ReportEvent)
        If evtType.NotifEvent = pvcam_helper.CameraNotifications.ACQ_SINGLE_FINISHED Then
            ' cameraImageReady = True
        End If
        Debug.Print("notificationReceived")
        If evtType.NotifEvent = pvcam_helper.CameraNotifications.ACQ_NEW_FRAME_RECEIVED Then
            'Dim tempW As Integer = (myCam.Region(0).s2 - myCam.Region(0).s1 + 1) / myCam.Region(0).sbin
            'Dim tempH As Integer = (myCam.Region(0).p2 - myCam.Region(0).p1 + 1) / myCam.Region(0).pbin
            'cameraImageArray = New Integer(tempW - 1, tempH - 1) {}
            'Dim n As Integer = 0

            'For y As Integer = 0 To tempH - 1

            '    For x As Integer = 0 To tempW - 1
            '        cameraImageArray(x, y) = CType(myCam.FrameDataShorts(n), UInt16)
            '        n += 1
            '    Next
            'Next
            Debug.Print("new image received")
            If m_pics Is Nothing Then
                m_pics = New RingBitmap(5)
            End If
            ' myCam.FrameToBMP(myCam.FrameDataShorts, myCam.XSize / myCam.Region(0).sbin, myCam.YSize / myCam.Region(0).pbin, True)
            myCam.FrameToBMP(myCam.FrameDataShorts, (myCam.Region(0).s2 - myCam.Region(0).s1 + 1) / myCam.Region(0).sbin, (myCam.Region(0).p2 - myCam.Region(0).p1 + 1) / myCam.Region(0).pbin, False)

            m_pics.FillNextBitmap(myCam.LastBMP)
            Dim filename As String
            Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
            filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgq_", DateTime.Now)
            filename = Path.Combine(Me.tbPath.Text, folderName, filename)



            If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
                ' md.examine(bm, filename)
                'call azure service
                Dim ms As New MemoryStream()
                m_pics.Image.Save(ms, ImageFormat.Bmp)

                Dim contents = ms.ToArray()
                Dim qe As New queueEntry
                qe.img = contents
                qe.filename = Path.GetFileName(filename)
                qe.dateTaken = Now
                qe.cameraID = "Coolsnap Camera"
                qe.width = myCam.LastBMP.Width
                qe.height = myCam.LastBMP.Height

                If myDetectionQueue.Count < 10 Then
                    myDetectionQueue.Enqueue(qe)
                End If

                ms.Close()

            End If
            If Me.cbSaveImages.Checked = True Then
                System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


                m_pics.Image.Save(filename, myImageCodecInfo, myEncoderParameters)


            End If
        End If

        'If evtType.NotifEvent = pvcam_helper.CameraNotifications.CAMERA_REFRESH_DONE Then

        '    If pvcam_helper.PVCamCamera.NrOfCameras > 0 Then
        '        If pvcam_helper.PVCamCamera.OpenCamera(pvcam_helper.PVCamCamera.CameraList(0), myCam) Then myCam.ReadCameraParams()
        '    End If
        'End If
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

    Private Sub cbMeteors_CheckedChanged(sender As Object, e As EventArgs) Handles cbMeteors.CheckedChanged
        'If Not cbMeteors.Checked Then
        '    md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")

        'Else
        '    md.LoadModel("c:\tmp\frozen_inference_graph.pb", "c:\tmp\object-detection.pbtxt")
        'End If
    End Sub

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

        While running AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()

        'Dim x As New Bitmap(b)
        Debug.Print("get last image")
        Dim x As New Bitmap(m_pics.Image)
        Return x



    End Function
    Public Function getLastImageArray() As Byte()
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While running AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()

        'Dim x As New Bitmap(b)
        Debug.Print("get last image")
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

    Private Sub frmCoolsnap_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        myCam.CloseCamera()
        'QCam.QCamM_CloseCamera(mhCamera)
        'QCam.QCamM_Free(mFrame1.pBuffer)
        'QCam.QCamM_Free(mFrame2.pBuffer)
        'QCam.QCamM_ReleaseDriver()



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
    Private Sub grabAframe()

        'Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        'Try
        '    QCam.QCamM_Abort(mhCamera)
        '    err = QCam.QCamM_GrabFrame(mhCamera, mFrame1)
        '    Debug.Print("grabAframe err:" & err)
        '    m_grabbedframe = True
        '    m_grabbedframe_err = err
        'Catch ex As Exception
        '    m_grabbedframe = True
        '    m_grabbedframe_err = -1
        '    Debug.Print("grabAframe err:" & err)
        '    Debug.Print("grabAframe exception:" & ex.Message)
        'End Try

    End Sub

    Private Sub tbExposureTime_AutoSizeChanged(sender As Object, e As EventArgs) Handles tbExposureTime.AutoSizeChanged

    End Sub
End Class