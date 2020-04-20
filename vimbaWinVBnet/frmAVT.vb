Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading.Tasks
Imports System.Timers
Imports System.ComponentModel
Imports System.Net.Http
Imports System.Threading
Imports System.Collections.Specialized
Imports vimbaWinVBnet.vimbaWinVBnet

Public Class frmAVT
    Inherits frmMaster
    'Dim v As New AVT.VmbAPINET.Vimba
    ' Dim WithEvents myCam As AVT.VmbAPINET.Camera
    Dim v As AsynchronousGrab.VimbaHelper
    Dim b As Bitmap
    Dim myCamID As String

    Function darkFunction(pixeld As Long, arg As Long, slope As Long) As Int16
        Dim subtract

        subtract = Math.Max(0, arg - pixeld * slope)
        Return Math.Max(0, subtract)
    End Function
    Private Sub received_frame(sender As Object, args As FrameEventArgs)

        b = New Bitmap(CInt(args.Frame.Width), CInt(args.Frame.Height), PixelFormat.Format24bppRgb)


        'receive raw data and convert to color
        Dim f As Frame
        f = args.Frame

        ' b = args.Image
        ' b.Tag = "orig"
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
        ' Dim d2 As Bitmap

        If Me.cbUseDarks.Checked And lblDayNight.Text = "night" Then
            'd2 = Bitmap.FromFile(Application.StartupPath & "\dark.png")
            Dim pixValue As Int16 = 0
            Dim darkValue As Int16 = 0
            Dim x
            If dark Is Nothing Then
                Dim fs As New FileStream(Application.StartupPath & "\dark_" & v.m_Camera.Id & ".raw", FileMode.Open)
                'read dark from file

                ReDim dark(b.Width * b.Height * 2)
                fs.Read(dark, 0, dark.Count)
                fs.Close()
                Dim slope
                Dim arg As Int16 = Val(tbMultiplier.Text)
                slope = 4096 / (4096 - arg)
                Dim lut(4095) As Int16
                For i = 0 To 4095
                    lut(i) = Math.Max(0, CInt(i * slope) - arg * slope)
                Next
                For x = 0 To dark.Length - 2 Step 2


                    darkValue = (dark(x + 1) * 256) + dark(x)

                    ' If darkValue > 500 Then
                    darkValue = lut(darkValue)

                    dark(x + 1) = (darkValue And &HFF00) >> 8
                    dark(x) = darkValue And &HFF
                    ' End If

                Next
            End If

            Try

                Dim t As TimeSpan
                Dim t2 As Date
                t2 = Now


                For x = 0 To dark.Length - 2 Step 2

                    pixValue = (f.Buffer(x + 1) * 256) + f.Buffer(x)
                    darkValue = (dark(x + 1) * 256) + dark(x)

                    ' If darkValue > 500 Then

                    pixValue = Math.Max(0, pixValue - darkValue)
                    ' pixValue = darkFunction(pixValue, darkValue, 100)
                    f.Buffer(x + 1) = Int(pixValue / 256)
                    f.Buffer(x) = pixValue And 255

                    ' End If

                Next
                t = Now - t2
                Debug.Print(t.ToString)
            Catch ex As Exception
                Debug.Print(x)
            End Try





        Else '

            dark = Nothing



        End If

        Dim iTotBytes As Integer = 0
        Dim sResponse As String = ""

        'convert frame to bitmap

        f.Fill(b)


        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
        Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
        Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)

        Dim gr As Graphics = Graphics.FromImage(b)
        Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
        Dim myBrushLabels As New SolidBrush(Color.White)

        gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
        gr.Dispose()



        Dim filename As String
        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "img_" & myCamID, DateTime.Now)
        filename = Path.Combine(Me.tbPath.Text, folderName, filename)



        If Me.cbSaveImages.Checked = True And Me.lblDayNight.Text = "night" Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            b.Save(filename, myImageCodecInfo, myEncoderParameters)


            If t_cleanup.ThreadState = ThreadState.Unstarted Or t_cleanup.ThreadState = ThreadState.Stopped Then
                t_cleanup = New Thread(AddressOf cleanFolders)

                t_cleanup.Start()
            Else

                Debug.WriteLine("threadstate:" & t_cleanup.ThreadState)
            End If

        End If
        If cbMeteors.Checked And lblDayNight.Text = "night" Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            b.Save(ms, myImageCodecInfo, myEncoderParameters)

            Dim contents = ms.ToArray()
            Dim qe As New queueEntry
            qe.img = contents
            qe.filename = Path.GetFileName(filename)
            qe.dateTaken = Now
            qe.cameraID = "AVT-" & v.MyCamera.Id
            qe.width = b.Width
            qe.height = b.Height

            If myDetectionQueue.Count < 10 Then
                myDetectionQueue.Enqueue(qe)
            End If
            'myDetectionQueue.Enqueue(New queueEntry(contents,))
            'Functions.CallAzureMeteorDetection(contents, Path.GetFileName(filename))
            ms.Close()
        Else
            ' md.examine(bm)
            ' md.drawBoxesOnly(bm)
        End If
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

    Public Sub writeline(s As String)
        'Console.WriteLine("AVT GigE: " & v.m_Camera.Id & ":" & s)
    End Sub

    Private Sub startup()
        v = New VimbaHelper

        v.Startup()


    End Sub
    Private Sub shutdown()
        v.Shutdown()


    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles TimerDayNight.Tick

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


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load



        Dim cams As List(Of CameraInfo)



        startup()
        cams = v.CameraList
        For Each c As CameraInfo In cams
            '    'only one camera so grab it
            cmbCam.Items.Add(c.ID)
        Next
        'load defaults

        tbPort.Text = "8099"
        tbPath.Text = "e:\image_avt"
        tbDayTimeExp.Text = "500"
        tbNightExp.Text = "7500000"
        tbDayGain.Text = "0"
        tbNightAgain.Text = "27"
        MyBase.Form_Load(sender, e)

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
        'average the pictures (for 12 bit images)
        Dim imageValueTotal
        Dim newValue
        For i = 0 To f.BufferSize - 1 Step 2
            imageValueTotal = 0
            For x = 0 To 9
                imageValueTotal = imageValueTotal + (darks(x).Buffer(i + 1) * 256) + darks(x).Buffer(i)

            Next
            newValue = Int(imageValueTotal / 10)
            f.Buffer(i + 1) = Int(newValue / 256)
            f.Buffer(i) = newValue And 255
        Next


        Dim fs As New FileStream(Application.StartupPath & "\dark_" & v.m_Camera.Id & ".raw", FileMode.Create)

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



    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseDarks.CheckedChanged
        If Not myWebServer Is Nothing Then
            If cbUseDarks.Checked Then
                myWebServer.useDarks = True
            Else
                myWebServer.useDarks = False

            End If
        End If

    End Sub



    Private Sub btnStopWeb_Click(sender As Object, e As EventArgs) Handles btnStopWeb.Click

        btnStartWeb.Enabled = True
        btnStopWeb.Enabled = False
        myWebServer.StopWebServer()

    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

        'If myCam Is Nothing Then
        '    MsgBox("select a camera")
        '    cbCam.Focus()
        '    Exit Sub

        btnStart.Enabled = False
        btnStop.Enabled = True
        startTime = Now
        TimerDayNight.Enabled = True
        TimerFPS.Enabled = True
        meteorCheckRunning = True
        If t Is Nothing Then

            t = New Thread(AddressOf processDetection)
            t.Start()

        Else
            If Not t.IsAlive Then
                t = New Thread(AddressOf processDetection)
                t.Start()
            End If
        End If



        v.StartContinuousImageAcquisition(AddressOf Me.received_frame)

    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        btnStart.Enabled = True
        btnStop.Enabled = False

        v.StopContinuousImageAcquisition()
        meteorCheckRunning = False

    End Sub







    Private Sub cmbCam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCam.SelectedIndexChanged

        v.OpenCamera(cmbCam.SelectedItem)
        loadProfile(cmbCam.SelectedItem)
    End Sub








    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        FolderBrowserDialog1.ShowDialog()
        tbPath.Text = FolderBrowserDialog1.SelectedPath

    End Sub


    Private Sub tbMultiplier_TextChanged(sender As Object, e As EventArgs) Handles tbMultiplier.TextChanged
        dark = Nothing

    End Sub



    Private Sub frmAVT_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        On Error Resume Next
        myWebServer.StopWebServer()
        v.StopContinuousImageAcquisition()
        myWebServer = Nothing
        v = Nothing

    End Sub



    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        If Not v Is Nothing Then
            If lblDayNight.Text = "night" Then
                v.m_Camera.LoadCameraSettings(Application.StartupPath & "\night_gc1380ch.xml")
            Else
                v.m_Camera.LoadCameraSettings(Application.StartupPath & "\day_gc1380ch.xml")
            End If
        End If

    End Sub


End Class
