Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading.Tasks
Imports System.Timers
Imports System.ComponentModel
Imports System.Net.Http
Imports System.Threading




Public Class frmAVT
    'Dim v As New AVT.VmbAPINET.Vimba
    ' Dim WithEvents myCam As AVT.VmbAPINET.Camera
    Dim myDetectionQueue As New Queue(Of queueEntry)
    Dim client As New HttpClient()
    Dim night As Boolean
    Private myWebServer As WebServer
    Dim rawDark() As Byte
    Dim running As Boolean = False
    Dim b As Bitmap
    Dim myTimer As New System.Timers.Timer
    Dim myCamID As String
    Dim nightset As Boolean
    ' Dim md As New ObjectDetection.TFDetector()
    Dim v As AsynchronousGrab.VimbaHelper
    Dim dark() As Byte
    Dim frames As Integer
    Dim startTime As Date
    Dim myImageCodecInfo As ImageCodecInfo
    Dim myEncoder As System.Drawing.Imaging.Encoder
    Dim myEncoderParameter As EncoderParameter
    Dim myEncoderParameters As EncoderParameters
    Private Class queueEntry

        Public img As Byte()
        Public filename As String

    End Class
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'setup dark

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
    Private Sub checkForThings()
        'copy of b
        'Dim c As New Bitmap(b)
        'c.Tag = "copy"
        'If Not md.isExamining Then
        '    md.examine(b)
        'End If


    End Sub
    Private Sub received_frame(sender As Object, args As FrameEventArgs)

        ' b = New Bitmap(CInt(args.Frame.Width), CInt(args.Frame.Height), PixelFormat.Format24bppRgb)
        b = args.Image
        b.Tag = "orig"
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
            If dark Is Nothing Then
                Dim fs As New FileStream(Application.StartupPath & "\dark.drk", FileMode.Open)
                'read dark from file

                ReDim dark(b.Width * b.Height * 3)
                fs.Read(dark, 0, dark.Count)
                fs.Close()
            End If

            Dim raw As System.Drawing.Imaging.BitmapData = Nothing
            ' 'Freeze the image in memory
            raw = b.LockBits(New Rectangle(0, 0,
             b.Width, b.Height),
             System.Drawing.Imaging.ImageLockMode.ReadOnly,
            b.PixelFormat)
            Dim size As Integer = b.Width * b.Height * 3

            Dim rawImage() As Byte = New Byte(size - 1) {}
            ''Copy the image into the byte()
            System.Runtime.InteropServices.Marshal.Copy(raw.Scan0, rawImage, 0, size)


            Dim multiplier
            multiplier = Val(Me.tbMultiplier.Text)
            '
            'subtract the dark
            If cbUseDarks.Checked Then
                Dim aByte As Integer
                Try

                    Dim aNewValue As Byte
                    Dim offset As Integer
                    For aByte = 0 To size - 1
                        If dark(aByte) > 220 Then
                            aNewValue = CByte(Math.Max(0, CLng(rawImage(aByte)) - CLng(dark(aByte))))
                            rawImage(aByte) = aNewValue
                        End If

                    Next
                    writeline("subtracted dark")
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            End If

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



            b.UnlockBits(raw)



        Else '
            'copy buffer into bitmap
            'b = New Bitmap(CInt(args.Frame.Width), CInt(args.Frame.Height), PixelFormat.Format24bppRgb)

            '' Lock the bitmap's bits.  
            'Dim rect As New Rectangle(0, 0, b.Width, b.Height)
            'Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b.PixelFormat)

            '' Get the address of the first line.
            'Dim ptr As IntPtr = bmpData.Scan0

            '' Declare an array to hold the bytes of the bitmap.
            '' This code is specific to a bitmap with 24 bits per pixels.
            'Dim bytes As Integer = Math.Abs(bmpData.Stride) * b.Height
            'Dim rgbValues(bytes - 1) As Byte

            ''' Copy the RGB values into the array.
            ''System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)

            ''' Set every third value to 255. A 24bpp image will look red.
            ''For counter As Integer = 2 To rgbValues.Length - 1 Step 3
            ''    rgbValues(counter) = 255
            ''Next

            '' Copy the RGB values back to the bitmap
            'System.Runtime.InteropServices.Marshal.Copy(args.Frame.Buffer, 0, ptr, args.Frame.BufferSize)

            '' Unlock the bits.
            'b.UnlockBits(bmpData)



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

        Dim filename As String
        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "img_" & myCamID, DateTime.Now)
        filename = Path.Combine(Me.tbPath.Text, folderName, filename)



        If Me.cbSaveImages.Checked = True Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            b.Save(filename, myImageCodecInfo, myEncoderParameters)


        End If
        If cbMeteors.Checked Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Png)

            Dim contents = ms.ToArray()
            Dim qe As New queueEntry
            qe.img = contents
            qe.filename = Path.GetFileName(filename)
            myDetectionQueue.Enqueue(qe)
            'myDetectionQueue.Enqueue(New queueEntry(contents,))
            'callAzureMeteorDetection(contents, Path.GetFileName(filename))
            ms.Close()
        Else
            ' md.examine(bm)
            ' md.drawBoxesOnly(bm)
        End If
        running = False

    End Sub
    Public Sub processDetection()
        Dim aQE As queueEntry
        While (True)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()
                callAzureMeteorDetection(aQE.img, aQE.filename)

                aQE = Nothing

            End If
            Console.WriteLine(myDetectionQueue.Count)

        End While

    End Sub


    Public Async Function callAzureMeteorDetection(contents As Byte(), file As String) As Task

        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.192:7071/api/detection?file=" + file



        Using byteContent = New ByteArrayContent(contents)

            Dim response = client.PostAsync(apiURL, byteContent)
            Dim responseString = response.Result
            Console.WriteLine(contents.Length)
        End Using

    End Function
    Public Function getLastImage() As Bitmap
        Dim s As Stopwatch
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While running AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()
        Return b
    End Function

    Public Sub writeline(s As String)
        Console.WriteLine("AVT GigE: " & v.m_Camera.Id & ":" & s)
    End Sub

    Private Sub startup()
        v = New VimbaHelper

        v.Startup()

        'Dim cl As List(Of CameraInfo)
        'cl = v.CameraList
        'cbCam.Items.Clear()

        'For Each c As CameraInfo In cl
        '    cbCam.Items.Add(c.ID)
        'Next

    End Sub
    Private Sub shutdown()
        v.Shutdown()
        cbCam.Items.Clear()
        MsgBox("shutdown complete")

    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try


            Timer1.Enabled = False


            Dim currentMode As Boolean
            currentMode = False

            If Now.Hour >= Me.ComboBox2.SelectedItem Or Now.Hour <= ComboBox1.SelectedItem Then
                currentMode = True
            Else
                currentMode = False
            End If

            If currentMode <> night Or Not nightset Then
                night = currentMode
                nightset = True

                If night Then

                    'tbExposureTime.Text = tbNightExp.Text
                    'lblDayNight.Text = "night"
                    ''night mode
                    ''   m_CCamera.setGainExposure(Val(Me.tbNightAgain.Text), Val(Me.tbExposureTime.Text))
                    v.m_Camera.LoadCameraSettings(Application.StartupPath & "\night_gc1380ch.xml")
                Else
                    v.m_Camera.LoadCameraSettings(Application.StartupPath & "\day_gc1380ch.xml")
                    'day mode

                    'tbExposureTime.Text = tbDayTimeExp.Text

                    'lblDayNight.Text = "day"
                    '' m_CCamera.setGainExposure(Val(Me.tbDayGain.Text), Val(Me.tbExposureTime.Text))



                End If
                'End If

                writeline("set gain & exposure")

            End If

        Catch ex As Exception

            Timer1.Enabled = True

        End Try
        Timer1.Enabled = True
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Vimba sys = new Vimba(); CameraCollection cameras = null;qu
        'try { sys.Startup(); cameras = sys.Cameras;
        'foreach (Camera camera in cameras) { try { camera.Open( VmbAccessModeType.VmbAccessModeFull ); Console.WriteLine( "Camera opened" ); camera.Close(); } catch ( VimbaException ve ) { Console.WriteLine( "Error : " + ve.MapReturnCodeToString() ); } }
        '} finally { sys.Shutdown(); }


        ''setup camera timer
        'myTimer.Interval = 20000

        'AddHandler myTimer.Elapsed, AddressOf Me.Health_Tick

        'startup()


        'm_cam.SaveImage(Application.StartupPath & "\junk.jpg", 10)
        ' Get an ImageCodecInfo object that represents the JPEG codec.
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

        Dim cams As List(Of CameraInfo)
        ComboBox2.SelectedIndex = 1
        ComboBox1.SelectedIndex = 1


        startup()
        cams = v.CameraList
        'md.LoadModel("c:\tmp\frozen_inference_graph.pb", "c:\tmp\object-detection.pbtxt")
        '  md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")
        For Each c As CameraInfo In cams
            '    'only one camera so grab it
            cbCam.Items.Add(c.ID)
        Next



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        'Dim f As Frame
        'Dim b As Bitmap

        'myCam.AcquireSingleImage(f, 100000)
        'f.Fill(b)
        'PictureBox1.Image = b
        'PictureBox1.SizeMode = PictureBoxSizeMode.Zoom


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MsgBox("cover lens cap")
        Button2.Enabled = False
        Dim f As Frame
        Dim darks(10) As Frame


        For i = 0 To 9

            v.m_Camera.AcquireSingleImage(f, 10000)
            darks(i) = f
            'Dim reader As New BinaryReader(ms)
            'Dim bytes() As Byte = New Byte(ms.Length) {}
            'reader.BaseStream.Position = 0

            'While reader.BaseStream.Position < reader.BaseStream.Length
            '    reader.Read(bytes, 0, bytes.Length)

            '    'sResponse = sResponse & Encoding.ASCII.GetString(bytes, 0, reader.BaseStream.Length)
            '    'iTotBytes = reader.BaseStream.Length
            'End While
            'reader.Close()
            'b.Save(Application.StartupPath & "\dark.tif", System.Drawing.Imaging.ImageFormat.Tiff)
        Next
        'average the pictures
        Dim imageValueTotal
        Dim newValue
        For i = 0 To f.BufferSize - 1
            imageValueTotal = 0
            For x = 0 To 9
                imageValueTotal = imageValueTotal + darks(x).Buffer(i)

            Next
            newValue = Int(imageValueTotal / 10)
            f.Buffer(i) = newValue

        Next


        Dim fs As New FileStream(Application.StartupPath & "\dark.drk", FileMode.Create)

        Dim ms As New MemoryStream()
        fs.Write(f.Buffer, 0, f.BufferSize)
        fs.Close()
        MsgBox("done")
        Button2.Enabled = True
    End Sub

    'Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click


    '    If v.m_Camera Is Nothing Then
    '        MsgBox("pick a camera")
    '    Else
    '        v.m_Camera.LoadCameraSettings(Application.StartupPath & "\day_gc1380ch.xml")

    '    End If
    'End Sub

    'Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
    '    If v.m_Camera Is Nothing Then
    '        MsgBox("pick a camera")
    '    Else
    '        v.m_Camera.LoadCameraSettings(Application.StartupPath & "\night_gc1380ch.xml")
    '    End If

    'End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Button5.Enabled = False
        Button6.Enabled = True
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(v.m_Camera, Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"

        If Now.Hour > Me.ComboBox2.SelectedItem Or Now.Hour < ComboBox1.SelectedItem Then
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

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseDarks.CheckedChanged
        If Not myWebServer Is Nothing Then
            If cbUseDarks.Checked Then
                myWebServer.useDarks = True
            Else
                myWebServer.useDarks = False

            End If
        End If

    End Sub



    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button5.Enabled = True
        Button6.Enabled = False
        myWebServer.StopWebServer()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        'If myCam Is Nothing Then
        '    MsgBox("select a camera")
        '    cbCam.Focus()
        '    Exit Sub

        'End If
        Button7.Enabled = False
        Button8.Enabled = True
        startTime = Now
        Timer1.Enabled = True
        Timer3.Enabled = True

        Dim t As New Thread(AddressOf processDetection)
        t.Start()

        'If Now.Hour >= ComboBox2.SelectedItem Or Now.Hour <= ComboBox1.SelectedItem Then
        '    night = True 'night
        'Else
        '    night = False 'day
        'End If
        v.StartContinuousImageAcquisition(AddressOf Me.received_frame)

        'myCam.StartContinuousImageAcquisition(1)
        'myCam.StartCapture()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Button7.Enabled = True
        Button8.Enabled = False
        v.StopContinuousImageAcquisition()
        'myCam.StopContinuousImageAcquisition()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs)
        'Dim f As Frame

        'myCam.AcquireSingleImage(f, 10000)
        'MsgBox("frame acquired")
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs)
        'shutter timer
        'Try
        '    myCam.StopContinuousImageAcquisition()
        'Catch ex As Exception

        'End Try

        ' myCam.StartContinuousImageAcquisition(1)

    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick

        Dim seconds As Integer

        seconds = DateDiff(DateInterval.Second, startTime, Now)
        txtFps.Text = frames / seconds
        '
        'writeline("timeout - close and restart")
        'Timer3.Enabled = False
        'myCam.Close()
        'myCam.Open(VmbAccessModeType.VmbAccessModeFull)
        'myCam.StartContinuousImageAcquisition(1)
    End Sub

    Private Sub cbCam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCam.SelectedIndexChanged
        'myCam = v.GetCameraByID(cbCam.SelectedItem)
        'myCamID = myCam.Id
        'myCam.Open(VmbAccessModeType.VmbAccessModeFull)
        v.OpenCamera(cbCam.SelectedItem)
    End Sub



    Private Sub frmAVT_Leave(sender As Object, e As EventArgs) Handles Me.Leave

    End Sub

    Private Sub Button9_Click_1(sender As Object, e As EventArgs)
        'Next

    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim vimba As New Vimba

        vimba = New Vimba()

        vimba.Startup()

    End Sub

    Private Sub Button9_Click_2(sender As Object, e As EventArgs) Handles Button9.Click
        shutdown()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        FolderBrowserDialog1.ShowDialog()
        tbPath.Text = FolderBrowserDialog1.SelectedPath

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

    End Sub

    Private Sub tbMultiplier_TextChanged(sender As Object, e As EventArgs) Handles tbMultiplier.TextChanged

    End Sub

    Private Sub cbMeteors_CheckedChanged(sender As Object, e As EventArgs) Handles cbMeteors.CheckedChanged
        'If Not cbMeteors.Checked Then
        '    md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")

        'Else
        '    md.LoadModel("c:\tmp\frozen_inference_graph.pb", "c:\tmp\object-detection.pbtxt")
        'End If
    End Sub

    Private Sub frmAVT_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        On Error Resume Next
        myWebServer.StopWebServer()
        v.StopContinuousImageAcquisition()
        myWebServer = Nothing
        v = Nothing

    End Sub








End Class
