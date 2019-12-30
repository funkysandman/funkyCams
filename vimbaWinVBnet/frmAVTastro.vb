Imports System.IO

Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading.Tasks
Imports System.Timers

Public Class frmAVTastro
    'Dim v As New AVT.VmbAPINET.Vimba
    ' Dim WithEvents myCam As AVT.VmbAPINET.Camera
    Dim night As Boolean
    Private myWebServer As WebServer
    Dim rawDark() As Byte
    Dim running As Boolean = False
    Dim b As Bitmap
    Dim myTimer As New System.Timers.Timer
    Dim myCamID As String
    Dim nightset As Boolean
    '  Dim md As New ObjectDetection.TFDetector()
    Dim v As AsynchronousGrab.VimbaHelper

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
    Private Sub received_frame(sender As Object, args As FrameEventArgs)
        running = True
        'start timeout timer
        writeline("received frame")
        'myTimer.Stop()
        'myTimer.Start()




        'If f.ReceiveStatus <> AVT.VmbAPINET.VmbFrameStatusType.VmbFrameStatusComplete Then
        '    writeline("bad image returned " & Now)
        '    writeline("status:" & f.ReceiveStatus)
        '    myCam.RevokeFrame(f)
        '    running = False
        '    Exit Sub


        'End If

        'myCam.QueueFrame(f)
        Dim filename As String
        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


        filename = String.Format("{0}-{1:ddMMMyyyy-HHmmss}.tif", "img_" & v.m_Camera.Id, DateTime.Now)
        filename = Path.Combine(Me.tbPath.Text, folderName, filename)


        writeline("got image " & Now)

        'darks

        If Me.cbUseDarks.Checked Then
            ''d2 = Bitmap.FromFile(Application.StartupPath & "\dark.png")


            ''' 'Freeze the image in memory
            ''' raw = b.LockBits(New Rectangle(0, 0,
            ''' b.Width, b.Height),
            ''' System.Drawing.Imaging.ImageLockMode.ReadOnly,
            '''b.PixelFormat)
            ''' Dim size As Integer = raw.Height * raw.Stride

            ''' Dim rawImage() As Byte = New Byte(size - 1) {}
            ''' 'Copy the image into the byte()
            ''' System.Runtime.InteropServices.Marshal.Copy(raw.Scan0, rawImage, 0, size)



            ''' Dim raw2 As System.Drawing.Imaging.BitmapData = Nothing


            ''' 'Freeze the image in memory
            ''' raw2 = d2.LockBits(New Rectangle(0, 0,
            ''' d2.Width, d2.Height),
            ''' System.Drawing.Imaging.ImageLockMode.ReadOnly,
            '''d2.PixelFormat)
            ''' size = raw2.Height * raw2.Stride

            ''' Dim rawImage2() As Byte = New Byte(size - 1) {}
            ''' 'Copy the image into the byte()
            ''' System.Runtime.InteropServices.Marshal.Copy(raw2.Scan0, rawImage2, 0, size)

            ''' If Not raw2 Is Nothing Then
            '''     'Unfreeze the memory for the image
            '''     d2.UnlockBits(raw2)
            ''' End If
            'Dim multiplier
            'multiplier = Val(Me.tbMultiplier.Text)
            'Dim imageValue
            'Dim darkValue
            'Dim newvalue
            'For i = 0 To f.BufferSize - 1 Step 2
            '    imageValue = f.Buffer(i + 1) * 256 + f.Buffer(i)
            '    darkValue = rawDark(i + 1) * 256 + rawDark(i)
            '    If darkValue * multiplier > imageValue Then

            '        f.Buffer(i) = 0
            '        f.Buffer(i + 1) = 0
            '    Else
            '        newvalue = imageValue - darkValue * multiplier
            '        f.Buffer(i) = newvalue And &HFF
            '        f.Buffer(i + 1) = (newvalue And &HFF00) >> 8

            '    End If

            'Next

            ''System.Runtime.InteropServices.Marshal.Copy(rawImage, 0, raw.Scan0, size)

            ' '' Unlock the bits.
            ' '' bmp.UnlockBits(bmpData)

            ''If Not raw Is Nothing Then
            ''    'Unfreeze the memory for the image
            ''    b.UnlockBits(raw)
            ''End If
            '  b = v.
            ' b = New Bitmap(f.Width, f.Height, PixelFormat.Format24bppRgb)
            ' f.Fill(b)

        Else
            b = args.Image

        End If

        'imageInUse = imageInUse + 1
        Dim iTotBytes As Integer = 0
        Dim sResponse As String = ""
        'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
        '
        Dim myImageCodecInfo As ImageCodecInfo
        Dim myEncoder As System.Drawing.Imaging.Encoder
        Dim myEncoderParameter As EncoderParameter
        Dim myEncoderParameters As EncoderParameters

        ' Create a Bitmap object based on a BMP file.


        ' Get an ImageCodecInfo object that represents the JPEG codec.
        myImageCodecInfo = GetEncoderInfo("image/tiff")

        ' Create an Encoder object based on the GUID
        ' for the Quality parameter category.
        myEncoder = System.Drawing.Imaging.Encoder.Quality

        ' Create an EncoderParameters object.
        ' An EncoderParameters object has an array of EncoderParameter
        ' objects. In this case, there is only one
        ' EncoderParameter object in the array.
        myEncoderParameters = New EncoderParameters(1)

        ' Save the bitmap as a JPEG file with quality level 25.
        myEncoderParameter = New EncoderParameter(myEncoder, CType(100L, Int32))
        myEncoderParameters.Param(0) = myEncoderParameter
        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
        Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
        Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)

        Dim gr As Graphics = Graphics.FromImage(b)
        Dim myFontLabels As New Font("Arial", 16)
        Dim myBrushLabels As New SolidBrush(Color.White)

        'gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.






        b.Save(filename, myImageCodecInfo, myEncoderParameters)

        'md.examine(filename)
        running = False

    End Sub
    Public Function getLastImage() As Bitmap
        Dim s As Stopwatch
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()
        While running AndAlso stopWatch.ElapsedMilliseconds < 20000
        End While

        stopWatch.[Stop]()
        Return b
    End Function
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

    End Sub
    Public Sub writeline(s As String)
        Console.WriteLine("AVT GigE: " & v.m_Camera.Id & ":" & s)
    End Sub

    Private Sub startup()
        v = New VimbaHelper

        v.Startup()
        Dim cl As List(Of CameraInfo)
        cl = v.CameraList
        cbCam.Items.Clear()

        For Each c As CameraInfo In cl
            cbCam.Items.Add(c.ID)
        Next

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
        'Vimba sys = new Vimba(); CameraCollection cameras = null;
        'try { sys.Startup(); cameras = sys.Cameras;
        'foreach (Camera camera in cameras) { try { camera.Open( VmbAccessModeType.VmbAccessModeFull ); Console.WriteLine( "Camera opened" ); camera.Close(); } catch ( VimbaException ve ) { Console.WriteLine( "Error : " + ve.MapReturnCodeToString() ); } }
        '} finally { sys.Shutdown(); }


        ''setup camera timer
        'myTimer.Interval = 20000

        'AddHandler myTimer.Elapsed, AddressOf Me.Health_Tick

        'startup()


        'm_cam.SaveImage(Application.StartupPath & "\junk.jpg", 10)


        Dim cams As List(Of CameraInfo)
        ComboBox2.SelectedIndex = 1
        ComboBox1.SelectedIndex = 1


        startup()
        cams = v.CameraList
        'md.LoadModel("c:\tmp\frozen_inference_graph.pb", "c:\tmp\object-detection.pbtxt")
        'For Each c As CameraInfo In cams
        '    '    'only one camera so grab it
        '    cbCam.Items.Add(c.ID)
        'Next



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
        'MsgBox("cover lens cap")
        'Button2.Enabled = False
        'Dim f As Frame
        'Dim darks(10) As Frame


        'For i = 0 To 9
        '    myCam.AcquireSingleImage(f, 10000)

        '    darks(i) = f
        '    'Dim reader As New BinaryReader(ms)
        '    'Dim bytes() As Byte = New Byte(ms.Length) {}
        '    'reader.BaseStream.Position = 0

        '    'While reader.BaseStream.Position < reader.BaseStream.Length
        '    '    reader.Read(bytes, 0, bytes.Length)

        '    '    'sResponse = sResponse & Encoding.ASCII.GetString(bytes, 0, reader.BaseStream.Length)
        '    '    'iTotBytes = reader.BaseStream.Length
        '    'End While
        '    'reader.Close()
        '    'b.Save(Application.StartupPath & "\dark.tif", System.Drawing.Imaging.ImageFormat.Tiff)
        'Next
        ''average the pictures
        'Dim imageValueTotal
        'Dim newValue
        'For i = 0 To f.BufferSize - 1 Step 2
        '    imageValueTotal = 0
        '    For x = 0 To 9
        '        imageValueTotal = imageValueTotal + darks(x).Buffer(i + 1) * 256 + darks(x).Buffer(i)

        '    Next
        '    newValue = Int(imageValueTotal / 10)
        '    f.Buffer(i) = newValue And &HFF
        '    f.Buffer(i + 1) = (newValue And &HFF00) >> 8
        'Next


        'Dim fs As New FileStream(Application.StartupPath & "\dark.drk", FileMode.Create)

        'Dim ms As New MemoryStream()
        'fs.Write(f.Buffer, 0, f.BufferSize)
        'fs.Close()
        'MsgBox("done")
        'Button2.Enabled = True
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






    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseDarks.CheckedChanged
        If Not myWebServer Is Nothing Then
            If cbUseDarks.Checked Then
                myWebServer.useDarks = True
            Else
                myWebServer.useDarks = False

            End If
        End If

    End Sub



    Private Async Sub Health_Tick(sender As Object, e As EventArgs)
        'Await Task.Run(Sub()

        '                   'Dim currentMode As Boolea
        '                   myTimer.Stop()

        '                   Me.writeline("timeout-frame not received -" & Now)
        '                   v.Dispose()
        '                   v.Startup()


        '                   'myCam = Nothing

        '                   myCam = v.GetCameraByID(myCamID)
        '                   Me.writeline("got camera -" & Now)
        '                   'myCam.EndCapture()

        '                   'myCam.Open(VmbAccessModeType.VmbAccessModeNone)

        '                   myCam.Open(VmbAccessModeType.VmbAccessModeFull)
        '                   ' Me.writeline("opened camera -" & Now)
        '                   'Me.writeline("flushed queue -" & Now)
        '                   'myCam.StopContinuousImageAcquisition()
        '                   myCam.StartContinuousImageAcquisition(3)
        '                   Me.writeline("restarted camera " & Now)
        '                   ' currentMode = False

        '                   'If Now.Hour >= ComboBox2.SelectedItem Or Now.Hour <= ComboBox1.SelectedItem Then
        '                   '    currentMode = True 'night
        '                   'Else
        '                   '    currentMode = False 'day
        '                   'End If
        '                   'If night <> currentMode Then

        '                   '    night = currentMode
        '                   '    Try


        '                   '        If night Then
        '                   '            myCam.LoadCameraSettings(Application.StartupPath & "\night_gc1380ch.xml")
        '                   '            'If cbUseDarks.Checked Then
        '                   '            '    ' myWebServer.useDarks = True
        '                   '            'Else
        '                   '            '    ' myWebServer.useDarks = False
        '                   '            'End If
        '                   '        Else
        '                   '            myCam.LoadCameraSettings(Application.StartupPath & "\day_gc1380ch.xml")
        '                   '            ' myWebServer.useDarks = False

        '                   '        End If
        '                   '    Catch ex As Exception
        '                   '        'continue anyway
        '                   '        Debug.Print("could not load settings")
        '                   '    End Try
        '                   '  End If

        '                   ' End If
        '               End Sub)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        myWebServer.StopWebServer()
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        'If myCam Is Nothing Then
        '    MsgBox("select a camera")
        '    cbCam.Focus()
        '    Exit Sub

        'End If



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
        startup()
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
End Class
