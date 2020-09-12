Imports System.IO
Imports System.Threading

Public Class frmScout
    Inherits frmMaster

    Private myBaslerImageGrabber As New BaslerWrapper.Grabber
    Private Sub frmScout_Load(sender As Object, e As EventArgs) Handles Me.Load




        Me.cmbCam.Visible = False
        Me.cbUseTrigger.Visible = False


        'setup camera
        myBaslerImageGrabber.Open()

        tbPort.Text = "8199"
        tbPath.Text = "e:\image_scout"
        tbDayTimeExp.Text = "500"
        tbNightExp.Text = "7500000"
        tbDayGain.Text = "0"
        tbNightAgain.Text = "27"
        MyBase.Form_Load(sender, e)
        loadProfile("scout")
    End Sub



    Public Function getLastImage() As Bitmap
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

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseDarks.CheckedChanged
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
            myBaslerImageGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))
        Else
            myBaslerImageGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

        End If
        myBaslerImageGrabber.startAcquisition(AddressOf received_frame)




    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        btnStart.Enabled = True
        btnStop.Enabled = False

        myBaslerImageGrabber.stopAcquisition()
        meteorCheckRunning = False

    End Sub
    Private Overloads Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        'stop stream
        If myBaslerImageGrabber Is Nothing Then Exit Sub


        'mySVCam.stopAcquisitionThread()

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
            myBaslerImageGrabber.stopAcquisition()

            myBaslerImageGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

            myBaslerImageGrabber.startAcquisition(AddressOf Me.received_frame)
        Else
            'day mode
            tbGain.Text = tbDayGain.Text
            tbExposureTime.Text = tbDayTimeExp.Text

            myBaslerImageGrabber.stopAcquisition()

            myBaslerImageGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))

            myBaslerImageGrabber.startAcquisition(AddressOf Me.received_frame)
        End If
        'start stream
        'If Me.cbUseTrigger.Checked Then

        '    mySVCam.startAcquisitionTriggerWidthThread(AddressOf Me.received_frame)
        'Else
        '    mySVCam.startAcquisitionThread(AddressOf Me.received_frame)
        'End If
    End Sub
    Private Sub received_frame(sender As Object, args As BaslerWrapper.FrameEventArgs)

        b = New Bitmap(CInt(args.image.Width), CInt(args.image.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
        Dim colorPalette As System.Drawing.Imaging.ColorPalette
        colorPalette = b.Palette
        For i = 0 To 255
            colorPalette.Entries(i) = System.Drawing.Color.FromArgb(i, i, i)
        Next
        b.Palette = colorPalette


        Dim BoundsRect = New Rectangle(0, 0, b.Width, b.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], b.PixelFormat)
        Dim rawData(b.Height * bmpData.Stride) As Byte
        Dim ptr As IntPtr = bmpData.Scan0



        System.Runtime.InteropServices.Marshal.Copy(args.image.Buffer, 0, ptr, rawData.Length - 1)
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



        If Me.cbSaveImages.Checked = True And Me.lblDayNight.Text.ToLower = "night" Then
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

    Private Sub frmScout_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        myBaslerImageGrabber.close()

    End Sub
End Class