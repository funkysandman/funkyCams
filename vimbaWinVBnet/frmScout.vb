Imports System.IO
Imports System.Threading
Imports Color = System.Drawing.Color

Public Class frmScout
    Inherits frmMaster

    Private myBaslerImageGrabber As New BaslerWrapper.Grabber
    Private ReadOnly m_lastFrameLock As New Object()
    Private m_lastFrameBytes As Byte()
    Private m_lastFrameWidth As Integer
    Private m_lastFrameHeight As Integer
    Private m_lastFramePixels As UShort()

    Private Sub frmScout_Load(sender As Object, e As EventArgs) Handles Me.Load




        Me.cmbCam.Visible = False
        Me.cbUseTrigger.Visible = False


        'setup camera
        myBaslerImageGrabber.Open(0)

        tbPort.Text = "8199"
        tbPath.Text = "e:\image_scout"
        tbDayTimeExp.Text = "500"
        tbNightExp.Text = "7500000"
        tbDayGain.Text = "0"
        tbNightAgain.Text = "27"
        MyBase.Form_Load(sender, e)
        loadProfile("scout")
    End Sub



    Public Overloads Function getLastImage() As Bitmap
        Dim x As New Bitmap(m_pics.width, m_pics.height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
        Dim BoundsRect = New Rectangle(0, 0, m_pics.width, m_pics.height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], x.PixelFormat)
        Dim ptr As IntPtr = bmpData.Scan0
        Dim ncp As System.Drawing.Imaging.ColorPalette = x.Palette

        For i = 0 To 255

            ncp.Entries(i) = System.Drawing.Color.FromArgb(255, i, i, i)
        Next
        x.Palette = ncp
        System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, m_pics.dataSize - 1) 'copy into bitmap


        x.UnlockBits(bmpData)
        Return x

    End Function


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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MsgBox("cover lens cap")
        Button2.Enabled = False
        Dim f As Frame
        Dim darks(10) As Frame


        For i = 0 To 9

            ' v.m_Camera.AcquireSingleImage(f, 10000)
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


        Dim fs As New FileStream(Application.StartupPath & "\dark_scout.raw", FileMode.Create)

        Dim ms As New MemoryStream()
        fs.Write(f.Buffer, 0, f.BufferSize)
        fs.Close()
        MsgBox("done")
        Button2.Enabled = True
    End Sub


    Private Sub cbUseDarks_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseDarks.CheckedChanged
        If Not myWebServer Is Nothing Then
            If cbUseDarks.Checked Then
                myWebServer.useDarks = True
            Else
                myWebServer.useDarks = False

            End If
        End If

    End Sub
    Private Sub btnStartWeb_Click(sender As Object, e As EventArgs) Handles btnStartWeb.Click
        btnStartWeb.Enabled = False
        btnStopWeb.Enabled = True
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(Me.myBaslerImageGrabber, Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"



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
        If LCase(Me.lblDayNight.Text) = "day" Then
            myBaslerImageGrabber.SetParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))
        Else
            myBaslerImageGrabber.SetParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

        End If
        myBaslerImageGrabber.StartAcquisition(AddressOf received_frame)




    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        btnStart.Enabled = True
        btnStop.Enabled = False

        myBaslerImageGrabber.StopAcquisition()
        meteorCheckRunning = False

    End Sub
    Private Overloads Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        'stop stream
        If myBaslerImageGrabber Is Nothing Then Exit Sub

        Dim isRunning As Boolean = False

        isRunning = myBaslerImageGrabber.isRunning


        If lblDayNight.Text = "night" Then

            tbExposureTime.Text = tbNightExp.Text
            tbGain.Text = tbNightAgain.Text

            'night mode
            ' If Not myWebServer Is Nothing Then
            'If cbUseDarks.Checked Then
            '    myBaslerImageGrabber.useDarks = True
            'Else
            '    myBaslerImageGrabber.useDarks = False
            'End If
            'End If
            'if the camera is running...stop exposing

            If isRunning Then
                myBaslerImageGrabber.StopAcquisition()
            End If

            myBaslerImageGrabber.SetParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

            If isRunning Then
                myBaslerImageGrabber.StartAcquisition(AddressOf Me.received_frame)
            End If

        Else
            'day mode
            tbGain.Text = tbDayGain.Text
            tbExposureTime.Text = tbDayTimeExp.Text

            If isRunning Then
                myBaslerImageGrabber.StopAcquisition()
            End If

            myBaslerImageGrabber.SetParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))

            If isRunning Then
                myBaslerImageGrabber.StartAcquisition(AddressOf Me.received_frame)
            End If
        End If
        'start stream
        'If Me.cbUseTrigger.Checked Then

        '    mySVCam.startAcquisitionTriggerWidthThread(AddressOf Me.received_frame)
        'Else
        '    mySVCam.startAcquisitionThread(AddressOf Me.received_frame)
        'End If
    End Sub
    Private Sub received_frame(sender As Object, args As BaslerWrapper.FrameEventArgs)

        Dim width As Integer = args.Width
        Dim height As Integer = args.Height

        Dim src() As UShort = args.Data

        Dim b As New Bitmap(width, height, Imaging.PixelFormat.Format8bppIndexed)

        Dim pal = b.Palette
        For i = 0 To 255
            pal.Entries(i) = Color.FromArgb(i, i, i)
        Next
        b.Palette = pal

        Dim bmpData As Imaging.BitmapData =
        b.LockBits(New Rectangle(0, 0, width, height),
               Imaging.ImageLockMode.WriteOnly,
               b.PixelFormat)

        Dim ptr As IntPtr = bmpData.Scan0

        Dim outBytes(width * height - 1) As Byte

        ' scale 16-bit → 8-bit (IMPORTANT)
        For i = 0 To src.Length - 1
            Dim v As Integer = CInt(src(i))
            outBytes(i) = CByte((v * 255) \ 4095)
        Next

        SyncLock m_lastFrameLock
            m_lastFrameWidth = width
            m_lastFrameHeight = height
            m_lastFrameBytes = CType(outBytes.Clone(), Byte())
            m_lastFramePixels = CType(src.Clone(), UShort())
        End SyncLock

        System.Runtime.InteropServices.Marshal.Copy(outBytes, 0, ptr, outBytes.Length)

        b.UnlockBits(bmpData)
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



        'imageInUse = imageInUse + 1
        Dim iTotBytes As Integer = 0
        Dim sResponse As String = ""
        'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
        '


        If m_pics Is Nothing Then
            m_pics = New RingBitmap(5)
        End If

        'm_pics.FillNextBitmap(rawData, b.Width, b.Height, rawData.Length)
        m_pics.FillNextBitmap(b)

        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
        Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
        Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)

        'Dim gr As Graphics = Graphics.FromImage(b)
        'Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
        'Dim myBrushLabels As New SolidBrush(System.Drawing.Color.White)

        'gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
        'gr.Dispose()

        'object detection section test
        '
        'Dim t As New Threading.Thread(AddressOf checkForThings)
        ''t.Start()
        'If frames Mod 3 = 0 Then


        'End If

        Dim filename As String
        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgscout_", DateTime.Now)
        filename = Path.Combine(Me.tbPath.Text, folderName, filename)



        If Me.cbSaveImages.Checked = True Then ' And Me.lblDayNight.Text.ToLower = "night" Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            b.Save(filename, myImageCodecInfo, myEncoderParameters)

            If t_cleanup.ThreadState = ThreadState.Unstarted Or t_cleanup.ThreadState = ThreadState.Stopped Then
                t_cleanup = New Thread(AddressOf cleanFolders)

                t_cleanup.Start()
            Else

                Debug.WriteLine("threadstate:" & t_cleanup.ThreadState)
            End If



        End If
        If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            ' convertedImage.ConvertToWriteAbleBitmap()
            b.Save(ms, myImageCodecInfo, myEncoderParameters)




            Dim contents = ms.ToArray()
            Dim qe As New queueEntry
            qe.img = contents 'jpeg sent to detection 
            qe.filename = Path.GetFileName(filename)
            qe.dateTaken = Now
            qe.cameraID = "Basler Scout Camera"
            qe.width = b.Width
            qe.height = b.Height
            b.Dispose()
            If myDetectionQueue.Count < 10 Then
                myDetectionQueue.Enqueue(qe)
            End If

            ms.Close()

        End If
        running = False
        gotFrameTime = Now

    End Sub

    Public Function TryGetLatestRawBitmap(ByRef image As Bitmap) As Boolean
        Dim latest() As Byte = Nothing
        Dim width As Integer = 0
        Dim height As Integer = 0

        SyncLock m_lastFrameLock
            If m_lastFrameBytes Is Nothing OrElse m_lastFrameBytes.Length = 0 Then
                Return False
            End If

            latest = CType(m_lastFrameBytes.Clone(), Byte())
            width = m_lastFrameWidth
            height = m_lastFrameHeight
        End SyncLock

        If width <= 0 OrElse height <= 0 OrElse latest.Length < (width * height) Then
            Return False
        End If

        Dim output As New Bitmap(width, height, Imaging.PixelFormat.Format8bppIndexed)
        Dim pal = output.Palette
        For i = 0 To 255
            pal.Entries(i) = Color.FromArgb(i, i, i)
        Next
        output.Palette = pal

        Dim bmpData As Imaging.BitmapData = output.LockBits(New Rectangle(0, 0, width, height), Imaging.ImageLockMode.WriteOnly, output.PixelFormat)
        System.Runtime.InteropServices.Marshal.Copy(latest, 0, bmpData.Scan0, width * height)
        output.UnlockBits(bmpData)

        image = output
        Return True
    End Function

    Private Sub frmScout_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        myBaslerImageGrabber.Close()

    End Sub

    Friend Function getLastImageArray(ByRef scoutImageArray As UShort()) As Boolean
        If m_lastFramePixels Is Nothing OrElse m_lastFramePixels.Length = 0 Then
            Return False
        End If

        scoutImageArray = CType(m_lastFramePixels.Clone(), UShort())
        Return True
    End Function





    'Private Sub InitializeComponent()
    '    Me.SuspendLayout()
    '    '
    '    'frmScout
    '    '
    '    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    '    Me.ClientSize = New System.Drawing.Size(422, 525)
    '    Me.Name = "frmScout"
    '    Me.ResumeLayout(False)
    '    Me.PerformLayout()

    'End Sub


End Class