Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading

Public Class frmSVSVistek

    Dim night As Boolean = False
    Private myWebServer As WebServer
    'Private myVistekImageGrabber As New SVS_Wrapper.SVS_Vistek_Grabber
    Private mySVCam As SVCamApi.SVCamGrabber
    Private killing As Boolean = False
    Private b As Bitmap
    Private running As Boolean
    Private frames As Integer
    Private startTime As DateTime
    Private gotFrameTime As DateTime
    Private dark() As Byte
    ' Private md As New ObjectDetection.TFDetector()
    Private myImageCodecInfo As ImageCodecInfo
    Private myEncoder As System.Drawing.Imaging.Encoder
    Private myEncoderParameter As EncoderParameter
    Private myEncoderParameters As EncoderParameters
    Private camThread As Thread


    Private Sub createCam()
        mySVCam = New SVCamApi.SVCamGrabber

    End Sub

    Private Sub received_frame(sender As Object, args As SVCamApi.FrameEventArgs)

        ' b = New Bitmap(CInt(args.Frame.Width), CInt(args.Frame.Height), PixelFormat.Format24bppRgb)
        b = args.Image
        b.Tag = Now
        running = True
        'start timeout timer
        writeline("received frame")
        'myTimer.Stop()
        'myTimer.Start()

        frames = frames + 1
        If frames Mod 100 = 0 Then
            startTime = Now
            frames = 0
        End If

        writeline("got image " & Now)

        'darks
        Dim d2 As Bitmap

        If Me.cbUseDarks.Checked Then
            'd2 = Bitmap.FromFile(Application.StartupPath & "\dark.png")
            'If dark Is Nothing Then
            '    Dim fs As New FileStream(Application.StartupPath & "\dark.drk", FileMode.Open)
            '    'read dark from file

            '    ReDim dark(b.Width * b.Height * 3)
            '    fs.Read(dark, 0, dark.Count)
            '    fs.Close()
            'End If

            Dim raw As System.Drawing.Imaging.BitmapData = Nothing
            ' 'Freeze the image in memory
            raw = b.LockBits(New Rectangle(0, 0,
             b.Width, b.Height),
             System.Drawing.Imaging.ImageLockMode.ReadOnly,
            b.PixelFormat)
            Dim size As Integer = b.Width * b.Height

            Dim rawImage() As Byte = New Byte(size - 1) {}
            ''Copy the image into the byte()
            System.Runtime.InteropServices.Marshal.Copy(raw.Scan0, rawImage, 0, size)


            'Dim multiplier
            'multiplier = Val(Me.tbMultiplier.Text)
            ''
            ''subtract the dark
            'Dim aByte As Integer
            'Try

            '    Dim aNewValue As Byte
            '    Dim offset As Integer
            '    For aByte = 0 To size - 1

            '        aNewValue = CByte(Math.Max(0, CLng(rawImage(aByte)) - CLng(dark(aByte)) * 0.75))
            '        rawImage(aByte) = aNewValue

            '    Next
            '    writeline("subtracted dark")
            'Catch ex As Exception
            '    MsgBox(ex.Message)
            'End Try
            Dim raw2 As System.Drawing.Imaging.BitmapData = Nothing


            ' 'Freeze the image in memory

            'raw2 = d2.LockBits(New Rectangle(0, 0,
            ' d2.Width, d2.Height),
            ' System.Drawing.Imaging.ImageLockMode.ReadOnly,
            'd2.PixelFormat)
            'size = raw2.Height * raw2.Stride

            ' Dim rawImage2() As Byte = New Byte(size - 1) {}
            ' 'Copy the image into the byte()
            System.Runtime.InteropServices.Marshal.Copy(rawImage, 0, raw.Scan0, size)

            'If Not raw2 Is Nothing Then
            '    ' Unfreeze the memory for the image
            '    d2.UnlockBits(raw2)
            'End If


            'copy buffer into bitmap


            '' Lock the bitmap's bits.  
            'Dim rect As New Rectangle(0, 0, b.Width, b.Height)
            'Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b.PixelFormat)

            '    ' Get the address of the first line.
            '    Dim ptr As IntPtr = bmpData.Scan0

            '    ' Declare an array to hold the bytes of the bitmap.
            '    ' This code is specific to a bitmap with 24 bits per pixels.
            '    Dim bytes As Integer = Math.Abs(bmpData.Stride) * b.Height
            '    Dim rgbValues(bytes - 1) As Byte

            '    '' Copy the RGB values into the array.
            '    'System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)

            '    '' Set every third value to 255. A 24bpp image will look red.
            '    'For counter As Integer = 2 To rgbValues.Length - 1 Step 3
            '    '    rgbValues(counter) = 255
            '    'Next

            '    ' Copy the RGB values back to the bitmap
            '    System.Runtime.InteropServices.Marshal.Copy(dark, 0, ptr, dark.Count)

            ' Unlock the bits.
            b.UnlockBits(raw)





        Else '





        End If

        'imageInUse = imageInUse + 1
        Dim iTotBytes As Integer = 0
        Dim sResponse As String = ""
        'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
        '



        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
        Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
        Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)

        Dim gr As Graphics = Graphics.FromImage(b)
        Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
        Dim myBrushLabels As New SolidBrush(Color.White)

        gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
        gr.Dispose()

        'object detection section test
        '
        'Dim t As New Threading.Thread(AddressOf checkForThings)
        ''t.Start()
        'If frames Mod 3 = 0 Then


        'End If

        Dim filename As String
        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "img_", DateTime.Now)
        filename = Path.Combine(Me.tbPath.Text, folderName, filename)



        If Me.cbSaveImages.Checked = True Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            b.Save(filename, myImageCodecInfo, myEncoderParameters)


        End If
        'If cbMeteors.Checked Then
        '    md.examine(b, filename)
        'Else
        '    md.examine(b)
        '    md.drawBoxesOnly(b)
        'End If
        running = False
        gotFrameTime = Now

    End Sub

    Public Function getLastImage() As Bitmap

        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While running AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()
        If DateDiff(DateInterval.Minute, Now, b.Tag) > 1 Then
            Return Nothing
        Else
            Return b
        End If

    End Function
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        camThread = New Thread(AddressOf createCam)
        camThread.Name = "camera thread"
        camThread.Start()


        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1
        While mySVCam Is Nothing
            Thread.Sleep(1)
        End While
        For Each c In mySVCam.getCameraList()
            cmbCam.Items.Add(c.devInfo.displayName)
        Next

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




    Public Sub writeline(s As String)
        Console.WriteLine("SVS Vistek: " & s)
    End Sub





    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Dim w As Integer
        Dim h As Integer

        w = 1388
        h = 1040
        Dim b = New Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)

        Dim ncp As System.Drawing.Imaging.ColorPalette = b.Palette
        For i As Integer = 0 To 255
            ncp.Entries(i) = System.Drawing.Color.FromArgb(255, i, i, i)
        Next
        b.Palette = ncp
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub initCamera()

        'myVistekImageGrabber = New SVS_Wrapper.SVS_Vistek_Grabber
        'myVistekImageGrabber.OpenCamera()

    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button4.Enabled = True
        Button6.Enabled = False
        myWebServer = WebServer.getWebServer

        ' initCamera()


        myWebServer.StartWebServer(mySVCam, Me, Val(Me.tbPort.Text))

    End Sub







    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Button6.Enabled = True
        Button4.Enabled = False
        myWebServer.StopWebServer()

    End Sub


    Protected Overrides Sub Finalize()
        'myVistekImageGrabber.close()
        MyBase.Finalize()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try

            Timer1.Enabled = False


            Dim currentMode As Boolean
            currentMode = False

            If Now.Hour >= cboNight.SelectedItem Or Now.Hour <= cboDay.SelectedItem Then
                currentMode = True
            Else
                currentMode = False
            End If
            If currentMode <> night Then
                night = currentMode
                If night Then

                    tbExposureTime.Text = tbNightExp.Text
                    lblDayNight.Text = "night"
                    'night mode
                    ' If Not myWebServer Is Nothing Then
                    If cbUseDarks.Checked Then
                        mySVCam.useDarks = True
                    Else
                        mySVCam.useDarks = False
                    End If
                    'End If
                    mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

                Else
                    'day mode

                    tbExposureTime.Text = tbDayTimeExp.Text

                    lblDayNight.Text = "day"
                    mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))


                End If
                'End If



            End If

        Catch ex As Exception



        End Try
        Timer1.Enabled = True
    End Sub





    'Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
    '    initCamera()
    'End Sub

    'Private Sub AxFGControlCtrl1_ImageReceived(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedEvent) Handles AxFGControlCtrl1.ImageReceived
    '    AxFGControlCtrl1.Acquisition = 0


    'End Sub

    'Private Sub AxFGControlCtrl1_ImageReceivedExt(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedExtEvent) Handles AxFGControlCtrl1.ImageReceivedExt
    '    Dim rawBytesCount

    '    rawBytesCount = AxFGControlCtrl1.GetPayloadSize()
    '    '  Dim rawData(rawBytesCount) As Byte



    '    MsgBox(AxFGControlCtrl1.GetReceivedFrameCount)
    'End Sub

    'Private Sub AxFGControlCtrl1_DeviceEventCallback(sender As Object, e As AxFGControlLib._IFGControlEvents_DeviceEventCallbackEvent) Handles AxFGControlCtrl1.DeviceEventCallback
    '    MsgBox("deviceEventCallback")
    'End Sub

    'Private Sub AxFGControlCtrl1_JobCompleted(sender As Object, e As AxFGControlLib._IFGControlEvents_JobCompletedEvent) Handles AxFGControlCtrl1.JobCompleted
    '    MsgBox("job completed")
    'End Sub


    Private Sub cbSave_CheckedChanged(sender As Object, e As EventArgs) Handles cbSaveImages.CheckedChanged
        If cbSaveImages.Checked Then
            ' myVistekImageGrabber.saveFiles = True
        Else
            ' myVistekImageGrabber.saveFiles = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button3.Enabled = True
        Button1.Enabled = False
        ' Timer2.Enabled = True
        startTime = Now
        gotFrameTime = Now 'reset time

        Timer3.Enabled = True

        If Now.Hour >= cboNight.SelectedItem Or Now.Hour <= cboDay.SelectedItem Then
            night = True
        Else
            night = False
        End If
        If night Then

            tbExposureTime.Text = tbNightExp.Text
            lblDayNight.Text = "night"
            'night mode
            ' If Not myWebServer Is Nothing Then
            If cbUseDarks.Checked Then
                mySVCam.useDarks = True
            Else
                mySVCam.useDarks = False
            End If
            'End If

        Else
            'day mode

            tbExposureTime.Text = tbDayTimeExp.Text

            lblDayNight.Text = "day"


        End If
        If LCase(Me.lblDayNight.Text) = "day" Then
            mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))
        Else
            mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

        End If
        ' myVistekImageGrabber.useDarks = cbUseDarks.Checked
        ' myVistekImageGrabber.stopStreamingFF()
        ' myVistekImageGrabber.startStreamingFF()
        mySVCam._darkmultiplier = Val(Me.tbMultiplier.Text)
        mySVCam.m_saveLocal = True
        mySVCam.startAcquisitionThread(AddressOf Me.received_frame)


    End Sub

    Private Sub tbNightExp_TextChanged(sender As Object, e As EventArgs) Handles tbNightExp.TextChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))


        MsgBox("cover lens")

        mySVCam.startAcquisitionThreadForDarks(AddressOf Me.received_frame)

    End Sub

    Private Sub cbUseDarks_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseDarks.CheckedChanged
        If cbUseDarks.Checked Then
            mySVCam.useDarks = True
        Else
            mySVCam.useDarks = False
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        'heartbeat
        '  myVistekImageGrabber.checkCamera()
        If killing Then
            writeline("still killing camera")
            Exit Sub
        End If

        'mySVCam.current_selected_cam.CheckCamera()
        If Math.Abs(DateDiff(DateInterval.Second, gotFrameTime, Now)) > 10 Then
            gotFrameTime = Now

            killing = True
            mySVCam.killCapture()

            'restart
            mySVCam.startAcquisitionThread(AddressOf Me.received_frame)
        End If
        killing = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        '   myVistekImageGrabber.stopStreamingFF()
        Button3.Enabled = False
        Button1.Enabled = True
        mySVCam.stopAcquisitionThread()
        Timer2.Enabled = False

    End Sub

    Private Sub frmSVSVistek_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        On Error Resume Next

        mySVCam.stopAcquisitionThread()
        mySVCam.closeCamera()
        mySVCam = Nothing
        'md = Nothing
        myWebServer.StopWebServer()
        myWebServer = Nothing

    End Sub

    Private Sub tbMultiplier_TextChanged(sender As Object, e As EventArgs) Handles tbMultiplier.TextChanged
        ' Me.myVistekImageGrabber.darkMultiplier = Val(tbMultiplier.Text)
    End Sub

    Private Sub tbDayGamma_TextChanged(sender As Object, e As EventArgs) Handles tbDayGamma.TextChanged

    End Sub

    Private Sub cmbCam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCam.SelectedIndexChanged
        mySVCam.openCamera(cmbCam.SelectedIndex)
        mySVCam.prepareCameraForTriggerWidth(mySVCam.current_selected_cam)

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

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Dim seconds As Integer

        seconds = DateDiff(DateInterval.Second, startTime, Now)
        txtFps.Text = frames / seconds
    End Sub
End Class