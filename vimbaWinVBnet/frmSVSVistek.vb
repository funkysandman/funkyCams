Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading
Imports System.Net.Http
Imports System.Collections.Specialized
Imports vimbaWinVBnet.vimbaWinVBnet

Public Class frmSVSVistek
    Inherits frmMaster

    Private mySVCamGrabber As SVCamApi.SVCamGrabber
    'Private killing As Boolean = False
    'Private b As Bitmap
    'Private running As Boolean
    'Private frames As Integer
    'Private startTime As DateTime
    'Private gotFrameTime As DateTime
    'Private dark() As Byte


    'Private camThread As Thread
    'Private t As Thread
    'Private lost As Integer = 0




    Private Sub createCam()
        mySVCamGrabber = New SVCamApi.SVCamGrabber

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

        Dim gr As Graphics = Graphics.FromImage(b)
        Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
        Dim myBrushLabels As New SolidBrush(Color.White)

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
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgsvs_", DateTime.Now)
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
            qe.cameraID = "SVS Vistek Camera"
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

    Public Overloads Function getLastImage() As Bitmap
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        'While running AndAlso stopWatch.ElapsedMilliseconds < 20000

        'End While

        stopWatch.[Stop]()

        'Dim x As New Bitmap(b)
        Debug.Print("get last image")

        Dim x As New Bitmap(m_pics.width, m_pics.height, PixelFormat.Format24bppRgb)
        Dim BoundsRect = New Rectangle(0, 0, m_pics.width, m_pics.height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], x.PixelFormat)
        Dim ptr As IntPtr = bmpData.Scan0
        System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, m_pics.dataSize - 1) 'copy into bitmap


        x.UnlockBits(bmpData)
        Return x

        'Return m_pics.Bitmap
    End Function
    Private Overloads Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load

        camThread = New Thread(AddressOf createCam)
        camThread.Name = "camera thread"
        camThread.Start()
        'load defaults

        tbPort.Text = "8050"
        tbPath.Text = "e:\image_svs"
        tbDayTimeExp.Text = "125"
        tbNightExp.Text = "4996100"
        tbDayGain.Text = "0"
        tbNightAgain.Text = "27"


        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1
        While mySVCamGrabber Is Nothing
            Thread.Sleep(1)
        End While
        For Each c In mySVCamGrabber.getCameraList()
            cmbCam.Items.Add(c.devInfo.displayName)
        Next

        MyBase.Form_Load(sender, e)


    End Sub













    Private Sub btnStartWeb_Click(sender As Object, e As EventArgs) Handles btnStartWeb.Click
        btnStopWeb.Enabled = True
        btnStartWeb.Enabled = False
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(mySVCamGrabber, Me, Val(Me.tbPort.Text))

    End Sub



    Private Sub btnStopWeb_Click(sender As Object, e As EventArgs) Handles btnStopWeb.Click

        btnStartWeb.Enabled = True
        btnStopWeb.Enabled = False
        myWebServer.StopWebServer()

    End Sub


    Protected Overrides Sub Finalize()
        'myVistekImageGrabber.close()
        MyBase.Finalize()
    End Sub

    Private Sub TimerDayNight_Tick(sender As Object, e As EventArgs) Handles TimerDayNight.Tick
        Try
            Debug.Print("day/night tick")
            TimerDayNight.Enabled = False

            mySVCamGrabber.upper = Val(tbUpper.Text)
            mySVCamGrabber.lower = Val(tbLower.Text)

            If Not mySVCamGrabber.current_selected_cam Is Nothing Then
                Me.tbLost.Text = mySVCamGrabber.current_selected_cam.framesLost
            End If


            Dim currentMode As Boolean
            currentMode = False

            If Now.Hour >= cboNight.SelectedItem Or Now.Hour <= cboDay.SelectedItem Then
                currentMode = True
            Else
                currentMode = False
            End If
            '  If currentMode <> night Then
            night = currentMode
            If night Then

                tbExposureTime.Text = tbNightExp.Text
                lblDayNight.Text = "night"
                'night mode


            Else
                'day mode

                tbExposureTime.Text = tbDayTimeExp.Text

                lblDayNight.Text = "day"


            End If
            'End If



            ' End If

        Catch ex As Exception

            Me.Label10.Text = ex.Message

        End Try
        TimerDayNight.Enabled = True
    End Sub





    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        btnStop.Enabled = True
        btnStart.Enabled = False
        ' Timer2.Enabled = True
        startTime = Now
        gotFrameTime = Now 'reset time

        TimerFPS.Enabled = True
        mySVCamGrabber.upper = Val(tbUpper.Text)
        mySVCamGrabber.lower = Val(tbLower.Text)

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
                mySVCamGrabber.useDarks = True
            Else
                mySVCamGrabber.useDarks = False
            End If
            'End If

        Else
            'day mode

            tbExposureTime.Text = tbDayTimeExp.Text

            lblDayNight.Text = "day"
            mySVCamGrabber.useDarks = False

        End If
        If LCase(Me.lblDayNight.Text) = "day" Then
            mySVCamGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbDayGain.Text))
        Else
            mySVCamGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))

        End If

        mySVCamGrabber._darkmultiplier = Val(Me.tbMultiplier.Text)
        mySVCamGrabber.m_saveLocal = True
        If Me.cbUseTrigger.Checked Then
            mySVCamGrabber.prepareCameraForTriggerWidth(mySVCamGrabber.current_selected_cam)
        Else

            mySVCamGrabber.prepareCameraForTimed(mySVCamGrabber.current_selected_cam)
        End If
        If Me.cbUseTrigger.Checked Then

            mySVCamGrabber.startAcquisitionTriggerWidthThread(AddressOf Me.received_frame)
        Else
            mySVCamGrabber.startAcquisitionThread(AddressOf Me.received_frame)
        End If

        meteorCheckRunning = True
        t = New Thread(AddressOf processDetection)
        t.Start()

    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        mySVCamGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))


        MsgBox("cover lens")
        If cbUseTrigger.Checked Then
            mySVCamGrabber.startAcquisitionThreadTriggerWidthForDarks(AddressOf Me.received_frame)
        Else
            mySVCamGrabber.startAcquisitionThreadForDarks(AddressOf Me.received_frame)
        End If


    End Sub

    Private Sub cbUseDarks_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseDarks.CheckedChanged
        If cbUseDarks.Checked Then
            mySVCamGrabber.useDarks = True
        Else
            mySVCamGrabber.useDarks = False
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
            mySVCamGrabber.killCapture()

            'restart
            mySVCamGrabber.startAcquisitionThread(AddressOf Me.received_frame)
        End If
        killing = False
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        '   myVistekImageGrabber.stopStreamingFF()
        btnStop.Enabled = False
        btnStart.Enabled = True
        mySVCamGrabber.stopAcquisitionThread()
        TimerFPS.Enabled = False
        Timer2.Enabled = False

    End Sub

    Private Sub frmSVSVistek_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        On Error Resume Next

        mySVCamGrabber.stopAcquisitionThread()
        mySVCamGrabber.closeCamera()
        mySVCamGrabber = Nothing
        'md = Nothing
        If Not myWebServer Is Nothing Then
            myWebServer.StopWebServer()
        End If

        myWebServer = Nothing
        meteorCheckRunning = False
    End Sub



    Private Sub cmbCam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCam.SelectedIndexChanged
        mySVCamGrabber.openCamera(cmbCam.SelectedIndex)
        loadProfile(mySVCamGrabber.current_selected_cam.devInfo.model)

    End Sub




    Private Sub TimerFPS_Tick(sender As Object, e As EventArgs) Handles TimerFPS.Tick
        Dim seconds As Integer

        seconds = DateDiff(DateInterval.Second, startTime, Now)
        txtFps.Text = frames / seconds
    End Sub



    Private Overloads Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        'stop stream

        Debug.Print("day to night / night to day")
        If mySVCamGrabber Is Nothing Then Exit Sub
        If Not mySVCamGrabber.acqThreadIsRuning Then Exit Sub
        mySVCamGrabber.stopAcquisitionThread()
        'wait for thread to stop
        While (mySVCamGrabber.acqThreadIsRuning)
            Application.DoEvents()
        End While





        'halt camera and wait for it to stop
        'mySVCam.stopAcquisitionThread()
        'While (mySVCam.current_selected_cam.isGrabbing)
        '    Application.DoEvents()
        'End While
        Dim webserverWasrunning As Boolean
        webserverWasrunning = False
        If Not myWebServer Is Nothing Then

            If myWebServer.running Then
                webserverWasrunning = True


                myWebServer.StopWebServer()

            End If
        End If

        'wait 15 seconds 
        Thread.Sleep(15000)

        Application.DoEvents()
        If lblDayNight.Text = "night" Then

            tbExposureTime.Text = tbNightExp.Text
            tbGain.Text = tbNightAgain.Text

            'night mode
            ' If Not myWebServer Is Nothing Then
            If cbUseDarks.Checked Then
                mySVCamGrabber.useDarks = True
            Else
                mySVCamGrabber.useDarks = False
            End If
            'End If
            'if the camera is running...stop exposing

            '
            'stop webserver if it running





            ' mySVCamGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))



        Else
            '        'day mode
            tbGain.Text = tbDayGain.Text
            tbExposureTime.Text = tbDayTimeExp.Text





        End If

        mySVCamGrabber.current_selected_cam.closeConnection()
        mySVCamGrabber.current_selected_cam.openConnection()

        mySVCamGrabber.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbGain.Text))
        mySVCamGrabber.prepareCameraForTimed(mySVCamGrabber.current_selected_cam)
        mySVCamGrabber.startAcquisitionThread(AddressOf Me.received_frame)
        If webserverWasrunning Then

            myWebServer.StartWebServer(mySVCamGrabber, Me, Val(Me.tbPort.Text))
        End If
        'start Stream
        'If Me.cbUseTrigger.Checked Then

        '    mySVCam.startAcquisitionTriggerWidthThread(AddressOf Me.received_frame)
        'Else
        '    mySVCam.startAcquisitionThread(AddressOf Me.received_frame)
        ' End If
        ' mySVCam.startAcquisitionThread(AddressOf Me.received_frame)
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'frmSVSVistek
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(422, 525)
        Me.Name = "frmSVSVistek"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub frmSVSVistek_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub




    'Private Sub InitializeComponent()
    '    Me.SuspendLayout()
    '    '
    '    'frmSVSVistek
    '    '
    '    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    '    Me.ClientSize = New System.Drawing.Size(422, 525)
    '    Me.Name = "frmSVSVistek"
    '    Me.ResumeLayout(False)
    '    Me.PerformLayout()

    'End Sub

End Class