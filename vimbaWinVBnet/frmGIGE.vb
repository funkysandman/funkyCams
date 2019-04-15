
Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading
Imports System.Net.Http

Public Class frmGIGE
    Dim myDetectionQueue As New Queue(Of QueueEntry)
    Dim client As New HttpClient()
    Private gigeGrabber As BaumerAPI.GIGEGrabber
    Private mySVCam As SVCamApi.SVCamGrabber 'just for setting properties
    Private b As Bitmap
    Private bm As Bitmap
    Private running As Boolean
    Private frames As Integer
    Private startTime As DateTime
    Dim night As Boolean
    Private myWebServer As WebServer
    Private dark() As Byte
    '  Private md As New ObjectDetection.TFDetector()
    Private myImageCodecInfo As ImageCodecInfo
    Private myEncoder As System.Drawing.Imaging.Encoder
    Private myEncoderParameter As EncoderParameter
    Private myEncoderParameters As EncoderParameters
    Private mThread As Thread
    Private meteorCheckRunning As Boolean
    Private t As Thread
    Private Class queueEntry

        Public img As Byte()
        Public filename As String

    End Class
    Private Sub received_frame(sender As Object, args As BaumerAPI.FrameEventArgs)

        ' b = New Bitmap(CInt(args.Frame.Width), CInt(args.Frame.Height), PixelFormat.Format24bppRgb)

        b = args.Image
        bm = b.Clone
        running = True
        'start timeout timer
        writeline("received frame")
        'myTimer.Stop()
        'myTimer.Start()

        frames = frames + 1
        If frames Mod 40 = 0 Then
            startTime = Now
            frames = 0
        End If

        writeline("got image " & Now)

        'darks
        Dim d2 As Bitmap

        If Me.cbUseDarks.Checked And False Then 'darks are processed before they arrive here...
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
            raw = bm.LockBits(New Rectangle(0, 0,
             bm.Width, bm.Height),
             System.Drawing.Imaging.ImageLockMode.ReadOnly,
            bm.PixelFormat)
            Dim size As Integer = bm.Width * b.Height

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
            bm.UnlockBits(raw)





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
        'b = bm.Clone
        Try

            'try to draw on bitmap
            Dim gr As Graphics = Graphics.FromImage(bm)
            Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
            'Dim myBrushLabels As New SolidBrush(Color.White)

            gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
            gr.Dispose()
            myFontLabels.Dispose()
        Catch ex As Exception

        End Try
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



        If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            bm.Save(ms, ImageFormat.Bmp)

            Dim contents = ms.ToArray()
            Dim qe As New queueEntry
            qe.img = contents
            qe.filename = Path.GetFileName(filename)
            myDetectionQueue.Enqueue(qe)

            ms.Close()
        Else
            ' md.examine(bm)
            ' md.drawBoxesOnly(bm)
        End If
        If Me.cbSaveImages.Checked = True Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            bm.Save(filename, myImageCodecInfo, myEncoderParameters)


        End If
        running = False

    End Sub
    Public Async Function callAzureMeteorDetection(contents As Byte(), file As String) As Task


        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.192:7071/api/detection?file=" + file

        Dim client As New HttpClient()

        Dim byteContent = New ByteArrayContent(contents)

        Dim response = client.PostAsync(apiURL, byteContent)
        Dim responseString = response.Result

    End Function
    Public Sub processDetection()
        Dim aQE As queueEntry
        While (meteorCheckRunning)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()
                callAzureMeteorDetection(aQE.img, aQE.filename)

                aQE = Nothing

            End If
            Console.WriteLine("in the queue:{0}", myDetectionQueue.Count)
            Thread.Sleep(100)
        End While

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
            'Dim x As New Bitmap(b)
            Return bm
        End If

    End Function
    Private Sub frmGIGE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mySVCam = New SVCamApi.SVCamGrabber
        For Each c In mySVCam.getCameraList()
            cmbCam.Items.Add(c.devInfo.displayName)
        Next

        dark = File.ReadAllBytes("masterdark.raw")

        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1
        'For Each c In mySVCam.getCameraList()
        '    cmbCam.Items.Add(c.devInfo.displayName)
        'Next

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'open the camera

    End Sub

    Public Sub writeline(s As String)
        Console.WriteLine("SVS Vistek: " & s)
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

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim seconds As Integer

        seconds = DateDiff(DateInterval.Second, startTime, Now)
        txtFps.Text = frames / seconds
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Button5.Enabled = False
        Button6.Enabled = True
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"

        If Now.Hour > Me.cboDay.SelectedItem Or Now.Hour < cboNight.SelectedItem Then
            night = True
        Else
            night = False
        End If


        If night Then
            '        myCam.LoadCameraSettings(Application.StartupPath & "\night_gc1380ch.xml")
            If cbUseDarks.Checked Then
                myWebServer.useDarks = True
            Else
                myWebServer.useDarks = False
            End If
        Else
            '         myCam.LoadCameraSettings(Application.StartupPath & "\day_gc1380ch.xml")
            myWebServer.useDarks = False

        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button6.Enabled = True
        Button4.Enabled = False
        myWebServer.StopWebServer()
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

                    'night mode
                    ' If Not myWebServer Is Nothing Then
                    If cbUseDarks.Checked Then
                        ' gigeGrabber.useDarks = True
                    Else
                        ' gigeGrabber.useDarks = False
                    End If
                    'End If
                    tbGain.Text = tbNightAgain.Text
                    lblDayNight.Text = "night"
                    ' gigeGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

                Else
                    'day mode

                    tbExposureTime.Text = tbDayTimeExp.Text


                    tbGain.Text = tbDayGain.Text
                    lblDayNight.Text = "day"



                End If
                'End If



            End If

        Catch ex As Exception



        End Try
        Timer1.Enabled = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Button2.Enabled = True
        Button1.Enabled = False
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


        For Each c In mySVCam.getCameraList()
            cmbCam.Items.Add(c.devInfo.displayName)
        Next
        'cmbCam.SelectedIndex = 0
        mySVCam.openCamera(cmbCam.SelectedIndex)

        If LCase(Me.lblDayNight.Text) = "day" Then
            mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))
        Else
            mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))
        End If

        mySVCam.closeCamera()

        gigeGrabber = New BaumerAPI.GIGEGrabber()
        gigeGrabber.openCamera(cmbCam.SelectedItem)


        'Timer2.Enabled = True


        If LCase(Me.lblDayNight.Text) = "day" Then
            gigeGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))
        Else
            gigeGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

        End If
        ' myVistekImageGrabber.useDarks = cbUseDarks.Checked
        ' myVistekImageGrabber.stopStreamingFF()
        ' myVistekImageGrabber.startStreamingFF()
        'gigeGrabber._darkmultiplier = Val(Me.tbMultiplier.Text)

        gigeGrabber.startCapture(AddressOf received_frame)
        mThread = New Thread(AddressOf startAcquisitionThread)
        mThread.Name = "Camera thread"
        mThread.Start()

        TimerAcquistionRate.Enabled = True
        startTime = Now
        Timer2.Enabled = True
        meteorCheckRunning = True
        t = New Thread(AddressOf processDetection)
        t.Start()

    End Sub
    Private Sub startAcquisitionThread()
        Console.WriteLine("starting camera thread")
        gigeGrabber.startAcquisition()
    End Sub

    Private Sub TimerAcquistionRate_Tick(sender As Object, e As EventArgs) Handles TimerAcquistionRate.Tick

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        gigeGrabber.stopAcquisition()
        mThread.Abort()
        meteorCheckRunning = False

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim test As New Bitmap("img_09Nov2018-021150.bmp")
        Dim filename As String
        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "img_", DateTime.Now)
        filename = Path.Combine(Me.tbPath.Text, folderName, filename)
        If Me.cbSaveImages.Checked = True Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            test.Save(filename, myImageCodecInfo, myEncoderParameters)


        End If
        If cbMeteors.Checked Then
            'md.examine(test, filename)

            test.Save(filename, myImageCodecInfo, myEncoderParameters)
        Else
            ' md.examine(b)
            ' md.drawBoxesOnly(b)
        End If
    End Sub

    Private Sub cbMeteors_CheckedChanged(sender As Object, e As EventArgs) Handles cbMeteors.CheckedChanged
        If Not cbMeteors.Checked Then
            ' md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")

        Else
            ' md.LoadModel("c:\tmp\frozen_inference_graph.pb", "c:\tmp\object-detection.pbtxt")
        End If
    End Sub

    Private Sub tbGain_TextChanged(sender As Object, e As EventArgs) Handles tbGain.TextChanged
        If Not gigeGrabber Is Nothing Then
            gigeGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbGain.Text))
        End If

    End Sub
End Class