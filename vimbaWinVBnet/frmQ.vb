Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Runtime.InteropServices
Imports QCamManagedDriver
Imports System.Threading
Imports System.Net.Http

Public Class frmQ
    Dim myDetectionQueue As New Queue(Of QueueEntry)
    Private mCamList As QCamM_CamListItem()
    Private mhCamera As IntPtr
    Private mDisplayPanel As myPanel
    Private mIsMono As Boolean
    Private mDisplayBitmap As Bitmap
    Private mFrameCallback As QCamM_AsyncCallback
    Private mFrame1 As QCamM_Frame
    Private mFrame2 As QCamM_Frame
    Private mRgbFrame As QCamM_Frame
    Private mSettings As QCamM_SettingsEx
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
    Private Class queueEntry

        Public img As Byte()
        Public filename As String

    End Class
    Private Sub StopStream()
        QCam.QCamM_Abort(mhCamera)
        QCam.QCamM_SetStreaming(mhCamera, 0)
    End Sub

    Private Sub StartStream()
        QCam.QCamM_Abort(mhCamera)
        QCam.QCamM_SetStreaming(mhCamera, 1)
        QueueFrame(1)
        'QueueFrame(2)
    End Sub

    Private Function QueueFrame(ByVal frameNum As UInteger) As Boolean
        Dim err As QCamM_Err

        If frameNum = 1 Then
            err = QCam.QCamM_QueueFrame(Me.mhCamera, mFrame1, mFrameCallback, CUInt(QCamM_qcCallbackFlags.qcCallbackDone), IntPtr.Zero, frameNum)
        ElseIf frameNum = 2 Then
            err = QCam.QCamM_QueueFrame(Me.mhCamera, mFrame2, mFrameCallback, CUInt(QCamM_qcCallbackFlags.qcCallbackDone), IntPtr.Zero, frameNum)
        Else
            Return False
        End If

        If err = QCamM_Err.qerrSuccess Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Function OpenCamera() As Boolean
        mCamList = New QCamM_CamListItem(9) {}
        Dim listLen As UInteger = 10
        QCam.QCamM_ListCameras(mCamList, listLen)

        If (listLen > 0) AndAlso (mCamList(0).isOpen = 0) Then

            If QCam.QCamM_OpenCamera(mCamList(0).cameraId, mhCamera) <> QCamM_Err.qerrSuccess Then
                Return False
            End If

            Return True
        Else
            Return False
        End If
    End Function

    Private Function InitCamera() As Boolean
        Dim ccdType As UInteger = 0
        Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        mSettings = New QCamM_SettingsEx()
        QCam.QCamM_CreateCameraSettingsStruct(mSettings)
        QCam.QCamM_InitializeCameraSettings(mhCamera, mSettings)
        QCam.QCamM_ReadDefaultSettings(mhCamera, mSettings)
        err = QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfCcdType, ccdType)

        If ccdType = Convert.ToUInt32(QCamM_qcCcdType.qcCcdColorBayer) Then
            err = QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32(QCamM_ImageFormat.qfmtBayer8))
            mIsMono = False
        ElseIf ccdType = Convert.ToUInt32(QCamM_qcCcdType.qcCcdMonochrome) Then
            err = QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32(QCamM_ImageFormat.qfmtMono8))
            mIsMono = True
        Else
            Return False
        End If


        Dim frameSize As UInteger = 0
        QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, frameSize)
        frameSize = frameSize '* 2
        mFrame1 = New QCamM_Frame()
        mFrame1.bufferSize = frameSize
        mFrame1.pBuffer = QCam.QCamM_Malloc(mFrame1.bufferSize)
        mFrame2 = New QCamM_Frame()
        mFrame2.bufferSize = frameSize
        mFrame2.pBuffer = QCam.QCamM_Malloc(mFrame2.bufferSize)
        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmReadoutSpeed, 1)

        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmOffset, 258)
        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmCoolerActive, 1)
        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmEMGain, tbGain.Text)
        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
        'QCam.QCamM_SetParam(mSettings, QCamM_Param., tbExposureTime.Text)
        err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        'If Not mIsMono Then
        '    mRgbFrame = New QCamM_Frame()
        '    mRgbFrame.bufferSize = frameSize * 3
        '    mRgbFrame.pBuffer = QCam.QCamM_Malloc(mRgbFrame.bufferSize)
        '    mRgbFrame.format = CUInt(QCamM_ImageFormat.qfmtBgr24)
        'End If

        'If QCam.QCamM_IsParamSupported(mhCamera, QCamM_Param.qprmGain) = QCamM_Err.qerrSuccess Then
        '    gbGain.Enabled = True
        '    Dim val As UInteger = 0
        '    QCam.QCamM_GetParamMin(mSettings, QCamM_Param.qprmGain, val)
        '    tbGain.Minimum = CInt(val)
        '    QCam.QCamM_GetParamMax(mSettings, QCamM_Param.qprmGain, val)
        '    tbGain.Maximum = CInt(val)
        '    QCam.QCamM_GetParam(mSettings, QCamM_Param.qprmGain, val)
        '    tbGain.Value = CInt(val)
        'Else
        '    gbGain.Enabled = False
        'End If

        'If QCam.QCamM_IsParamSupported(mhCamera, QCamM_Param.qprmExposure) = QCamM_Err.qerrSuccess Then
        '    gbExposure.Enabled = True
        Dim val As UInteger = 0
        QCam.QCamM_GetParam(mSettings, QCamM_Param.qprmExposure, val)
        ' tbExposure.Value = CInt((val / 1000))
        'Else
        '    gbExposure.Enabled = False
        'End If


        '  lblCameraModel.Text = "Model: " & (CType((mCamList(0).cameraType), QCamM_qcCameraType)).ToString().Remove(0, 8)
        ' Dim serNum As String = ""
        ' QCam.QCamM_GetSerialString(mhCamera, serNum)
        '  lblSerNum.Text = "Serial: " & serNum
        mFrameCallback = New QCamM_AsyncCallback(AddressOf frameCallback)
        Return True
    End Function
    Private Sub frameCallback(ByVal userPtr As IntPtr, ByVal userData As UInteger, ByVal errcode As QCamM_Err, ByVal flags As UInteger)
        Dim myFrame As QCamM_Frame
        running = True
        If userData = 1 Then
            myFrame = mFrame1
        ElseIf userData = 2 Then
            myFrame = mFrame2
        Else
            Return
        End If

        Dim width As UInteger = myFrame.width
        Dim height As UInteger = myFrame.height
        bmp = New Bitmap(width, height, PixelFormat.Format8bppIndexed)
        Dim rawData(mFrame1.bufferSize) As Byte
        If mIsMono Then
            Marshal.Copy(myFrame.pBuffer, rawData, 0, mFrame1.bufferSize)
            Dim dptr As IntPtr

            '
            Dim BoundsRect = New Rectangle(0, 0, width, height)
            Dim bmpData As System.Drawing.Imaging.BitmapData = bmp.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], bmp.PixelFormat)

            Dim ptr As IntPtr = bmpData.Scan0

            Dim bytes As Integer = mFrame1.bufferSize



            Marshal.Copy(rawData, 0, ptr, bytes)
            bmp.UnlockBits(bmpData)

            Debug.Print("image")
            For i = 1 To 512
                Debug.Print(rawData(i))
            Next


            ' bmp = New Bitmap(New MemoryStream(rawData))
            Dim pt As ColorPalette = bmp.Palette

            For i As Integer = 0 To pt.Entries.Length - 1
                pt.Entries(i) = Color.FromArgb(i, i, i)
            Next

            bmp.Palette = pt
        End If

        'Else
        '    QCamImgfnc.QCamM_BayerToRgb(QCamM_qcBayerInterp.qcBayerInterpFast, myFrame, mRgbFrame)
        '    bmp = New Bitmap(CInt(width), CInt(height), CInt(width) * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer)
        'End If

        'Using fs As FileStream = New FileStream("image.raw", FileMode.Create)
        '    Dim bw As BinaryWriter = New System.IO.BinaryWriter(fs)
        '    Dim b As Byte
        '    For i As Integer = 0 To mFrame1.size - 1
        '        b = Marshal.ReadByte(myFrame.pBuffer, i)
        '        bw.Write(b)

        '    Next
        '    bw.Close()
        'End Using
        '    mDisplayBitmap = bmp

        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
        Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
        Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)
        'b = bm.Clone
        Try

            'try to draw on bitmap
            'Dim tempbmp As Bitmap
            'tempbmp = New Bitmap(bmp.Width, bmp.Height)


            'From this bitmap, the graphics can be obtained, because it has the right PixelFormat
            ' Dim gr As Graphics = Graphics.FromImage(tempbmp)
            ' gr.DrawImage(bmp, 0, 0)
            'Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
            'Dim myBrushLabels As New SolidBrush(Color.White)
            bmp.RotateFlip(RotateFlipType.Rotate180FlipNone)

            ' gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
            ' gr.Dispose()
            ' myFontLabels.Dispose()
            ' bmp = tempbmp

        Catch ex As Exception

        End Try

        'PictureBox1.Image = bmp

        Dim filename As String
        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgq_", DateTime.Now)
        filename = Path.Combine(Me.tbPath.Text, folderName, filename)



        If cbMeteors.Checked Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            bmp.Save(ms, ImageFormat.Bmp)

            Dim contents = ms.ToArray()
            Dim qe As New QueueEntry
            qe.img = contents
            qe.filename = Path.GetFileName(filename)
            myDetectionQueue.Enqueue(qe)

            ms.Close()

        End If
        If Me.cbSaveImages.Checked = True Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            bmp.Save(filename, myImageCodecInfo, myEncoderParameters)


        End If
        QueueFrame(userData)
        running = False
        'mDisplayPanel.Invalidate()
    End Sub
    Public Sub processDetection()
        Dim aQE As queueEntry
        While (meteorCheckRunning)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()
                CallAzureMeteorDetection(aQE.img, aQE.filename)

                aQE = Nothing

            End If
            Console.WriteLine("in the queue:{0}", myDetectionQueue.Count)
            Thread.Sleep(100)
        End While

    End Sub
    Public Async Function CallAzureMeteorDetection(contents As Byte(), file As String) As Task


        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.192:7071/api/detection?file=" + file

        Dim client As New HttpClient()

        Dim byteContent = New ByteArrayContent(contents)

        Dim response = client.PostAsync(apiURL, byteContent)
        Dim responseString = response.Result

    End Function

    Public Sub GrabFrame()
        Dim width, height As UInteger
        Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        Dim sizeInBytes As UInteger = 0
        QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, sizeInBytes)
        err = QCam.QCamM_GrabFrame(mhCamera, mFrame1)
        width = mFrame1.width
        height = mFrame1.height
        Dim bmp As Bitmap = Nothing

        If mIsMono Then
            bmp = New Bitmap(CInt(width), CInt(height), CInt(width), PixelFormat.Format8bppIndexed, mFrame1.pBuffer)
            Dim pt As ColorPalette = bmp.Palette

            For i As Integer = 0 To pt.Entries.Length - 1
                pt.Entries(i) = Color.FromArgb(i, i, i)
            Next

            bmp.Palette = pt
        Else
            QCamImgfnc.QCamM_BayerToRgb(QCamM_qcBayerInterp.qcBayerInterpFast, mFrame1, mRgbFrame)
            bmp = New Bitmap(CInt(width), CInt(height), CInt(width) * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer)
        End If

        mDisplayBitmap = bmp
        PictureBox1.Image = bmp

        Try

            Using fs As FileStream = New FileStream("image.raw", FileMode.Create)
                Dim bw As BinaryWriter = New System.IO.BinaryWriter(fs)

                For i As Integer = 0 To mFrame1.size - 1 Step 1
                    ' bw.Write(Marshal.ReadByte(mFrame1.pBuffer, i + 1))
                    bw.Write(Marshal.ReadByte(mFrame1.pBuffer, i))
                Next

                bw.Close()
            End Using

        Catch e As Exception
            MessageBox.Show("Unable to save the image data: " & e.Message)
        End Try
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        'use settings
        Dim err As QCamM_Err


        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbNightAgain.Text)))


        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
        err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)

        StartStream()
        Button7.Enabled = False
        Button8.Enabled = True
        TimerAcquistionRate.Enabled = True
        startTime = Now
        meteorCheckRunning = True
        Timer2.Enabled = True
        t = New Thread(AddressOf processDetection)
        t.Start()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        StopStream()
        meteorCheckRunning = False
        Button7.Enabled = True
        Button8.Enabled = False
    End Sub

    Private Sub frmQ_Load(sender As Object, e As EventArgs) Handles Me.Load
        If QCamM_Err.qerrSuccess <> QCam.QCamM_LoadDriver() Then
            System.Windows.Forms.MessageBox.Show("The application was unable to load the QCam driver.")
            System.Environment.[Exit](0)
        End If

        If Not OpenCamera() Then
            System.Windows.Forms.MessageBox.Show("The application was unable to connect to a QImaging camera.  Please ensure one is connected and turned on before running this application.")
            System.Environment.[Exit](0)
        End If

        If Not InitCamera() Then
            System.Windows.Forms.MessageBox.Show("Failed to initialize the camera")
            System.Environment.[Exit](0)
        End If
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
        myEncoderParameter = New EncoderParameter(myEncoder, CType(85L, Int32))
        myEncoderParameters.Param(0) = myEncoderParameter
        ' md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")
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
        If DateDiff(DateInterval.Minute, Now, bmp.Tag) > 1 Then
            Return Nothing
        Else
            'Dim x As New Bitmap(b)
            Return bmp
        End If

    End Function

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button5.Enabled = True
        Button6.Enabled = False
        myWebServer.StopWebServer()
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

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
                tbGain.Text = "1"
                lblDayNight.Text = "day"


            End If
            'End If
            Dim err As QCamM_Err
            'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbNightAgain.Text)))
            ' QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
            'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        Catch ex As Exception

        End Try
    End Sub
End Class