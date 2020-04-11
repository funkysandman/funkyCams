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

    Dim night As Boolean = False
    Private myWebServer As WebServer
    Dim myDetectionQueue As New Queue(Of queueEntry)
    Private mySVCam As SVCamApi.SVCamGrabber
    Private killing As Boolean = False
    Private b As Bitmap
    Private running As Boolean
    Private frames As Integer
    Private startTime As DateTime
    Private gotFrameTime As DateTime
    Private dark() As Byte
    Private meteorCheckRunning As Boolean = False
    Private myImageCodecInfo As ImageCodecInfo
    Private myEncoder As System.Drawing.Imaging.Encoder
    Private myEncoderParameter As EncoderParameter
    Private myEncoderParameters As EncoderParameters
    Private camThread As Thread
    Private t As Thread
    Private lost As Integer = 0

    Shared m_pics As RingBitmap


    Public Class RingBitmap

        Private m_Size As Integer = 0
        Private m_width As Integer = 0
        Private m_height As Integer = 0
        Private m_dataSize As Integer = 0
        ' Private m_ManagedImages As ManagedImage()
        'Private m_Bitmaps As Bitmap()
        Private m_BitmapSelector As Integer = 0

        Private m_buffers()() As Byte
        Public Sub New(s As Integer)

            m_Size = s
            ' m_ManagedImages = New ManagedImage(m_Size - 1) {}
            ' m_Bitmaps = New Bitmap(m_Size - 1) {}
            ReDim m_buffers(m_Size - 1)
        End Sub

        'Public ReadOnly Property Bitmap As Bitmap
        '    Get
        '        Debug.Print("getting bitmap " & m_BitmapSelector)
        '        Return m_Bitmaps(m_BitmapSelector)
        '    End Get
        'End Property
        Public ReadOnly Property ImageBytes As Byte()
            Get
                Debug.Print("getting bitmap " & m_BitmapSelector)
                'copy raw data to byte array


                Return m_buffers(m_BitmapSelector)
            End Get
        End Property
        Public ReadOnly Property width As Integer
            Get

                Return m_width
            End Get
        End Property
        Public ReadOnly Property height As Integer
            Get

                Return m_height
            End Get
        End Property
        Public ReadOnly Property dataSize As Integer
            Get

                Return m_dataSize
            End Get
        End Property
        Public Sub FillNextBitmap(b As Bitmap)
            SwitchBitmap()

            ' m_ManagedImages(m_BitmapSelector) = b
            'copy raw data into m_buffers
            Dim rawData(b.Width * b.Height * 3) As Byte
            Dim BoundsRect = New Rectangle(0, 0, b.Width, b.Height)
            Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], b.PixelFormat)
            Dim ptr As IntPtr = bmpData.Scan0
            'System.Runtime.InteropServices.Marshal.Copy(b.DataPtr, ptr, 0, b.DataSize) 'copy into bitmap
            System.Runtime.InteropServices.Marshal.Copy(ptr, rawData, 0, rawData.Length - 1) 'copy into array

            m_buffers(m_BitmapSelector) = rawData
            m_width = b.Width
            m_height = b.Height
            m_dataSize = rawData.Length
            ' m_Bitmaps(m_BitmapSelector).UnlockBits(bmpData)
            'subtract darks
            'For i = 0 To rawData.Length - 1
            '    rawData(i) = Math.
            'Next
            'Dim bmp As New Bitmap(1920, 1200, PixelFormat.Format24bppRgb)
            ''Dim ncp As System.Drawing.Imaging.ColorPalette = b.Palette
            ''For j As Integer = 0 To 255
            ''    ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
            ''Next
            ''b.Palette = ncp




            'Dim BoundsRect = New Rectangle(0, 0, 1920 - 1, 1200 - 1)
            'Dim bmpData As System.Drawing.Imaging.BitmapData = bmp.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], bmp.PixelFormat)

            'Dim ptr As IntPtr = bmpData.Scan0





            'Marshal.Copy(m_buffers(m_BitmapSelector), 0, ptr, m_buffers(m_BitmapSelector).Length - 1)
            'bmp.UnlockBits(bmpData)
            'm_Bitmaps(m_BitmapSelector) = bmp

        End Sub
        'Public Sub FillNextBitmap(frame As QCamM_Frame)


















        '    ' switch to Bitmap object which Is currently Not in use by GUI
        '    SwitchBitmap()
        '    Debug.Print("fillnextbitmap bitmapselector: " & m_BitmapSelector)
        '    Try

        '        If (m_Bitmaps(m_BitmapSelector) Is Nothing) Then
        '            Debug.Print("making new bitmap")
        '            m_Bitmaps(m_BitmapSelector) = New Bitmap(frame.width, frame.height, PixelFormat.Format8bppIndexed)
        '            Dim ncp As System.Drawing.Imaging.ColorPalette = m_Bitmaps(m_BitmapSelector).Palette
        '            For j As Integer = 0 To 255
        '                ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
        '            Next
        '            m_Bitmaps(m_BitmapSelector).Palette = ncp
        '        End If

        '        'If (m_buffers(m_BitmapSelector) Is Nothing) Then
        '        '    m_buffers(m_BitmapSelector) = New Byte()
        '        'End If
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
        '        ''Dim b As New Bitmap(m_Bitmaps(m_BitmapSelector))
        '        '' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
        '        'Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
        '        'Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)
        '        ''b = bm.Clone
        '        'Dim gr As Graphics = Graphics.FromImage(m_Bitmaps(m_BitmapSelector))
        '        'Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
        '        'Dim myBrushLabels As New SolidBrush(Color.White)

        '        'gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
        '        'gr.Dispose()
        '        'myFontLabels.Dispose()
        '        'm_Bitmaps(m_BitmapSelector) = New Bitmap(b)
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

        'If Me.cbUseDarks.Checked Then
        '    'd2 = Bitmap.FromFile(Application.StartupPath & "\dark.png")
        '    'If dark Is Nothing Then
        '    '    Dim fs As New FileStream(Application.StartupPath & "\dark.drk", FileMode.Open)
        '    '    'read dark from file

        '    '    ReDim dark(b.Width * b.Height * 3)
        '    '    fs.Read(dark, 0, dark.Count)
        '    '    fs.Close()
        '    'End If

        '    Dim raw As System.Drawing.Imaging.BitmapData = Nothing
        '    ' 'Freeze the image in memory
        '    raw = b.LockBits(New Rectangle(0, 0,
        '     b.Width, b.Height),
        '     System.Drawing.Imaging.ImageLockMode.ReadOnly,
        '    b.PixelFormat)
        '    Dim size As Integer = b.Width * b.Height

        '    Dim rawImage() As Byte = New Byte(size - 1) {}
        '    ''Copy the image into the byte()
        '    System.Runtime.InteropServices.Marshal.Copy(raw.Scan0, rawImage, 0, size)


        '    'Dim multiplier
        '    'multiplier = Val(Me.tbMultiplier.Text)
        '    ''
        '    ''subtract the dark
        '    'Dim aByte As Integer
        '    'Try

        '    '    Dim aNewValue As Byte
        '    '    Dim offset As Integer
        '    '    For aByte = 0 To size - 1

        '    '        aNewValue = CByte(Math.Max(0, CLng(rawImage(aByte)) - CLng(dark(aByte)) * 0.75))
        '    '        rawImage(aByte) = aNewValue

        '    '    Next
        '    '    writeline("subtracted dark")
        '    'Catch ex As Exception
        '    '    MsgBox(ex.Message)
        '    'End Try
        '    Dim raw2 As System.Drawing.Imaging.BitmapData = Nothing


        '    ' 'Freeze the image in memory

        '    'raw2 = d2.LockBits(New Rectangle(0, 0,
        '    ' d2.Width, d2.Height),
        '    ' System.Drawing.Imaging.ImageLockMode.ReadOnly,
        '    'd2.PixelFormat)
        '    'size = raw2.Height * raw2.Stride

        '    ' Dim rawImage2() As Byte = New Byte(size - 1) {}
        '    ' 'Copy the image into the byte()
        '    System.Runtime.InteropServices.Marshal.Copy(rawImage, 0, raw.Scan0, size)

        '    'If Not raw2 Is Nothing Then
        '    '    ' Unfreeze the memory for the image
        '    '    d2.UnlockBits(raw2)
        '    'End If


        '    'copy buffer into bitmap


        '    '' Lock the bitmap's bits.  
        '    'Dim rect As New Rectangle(0, 0, b.Width, b.Height)
        '    'Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b.PixelFormat)

        '    '    ' Get the address of the first line.
        '    '    Dim ptr As IntPtr = bmpData.Scan0

        '    '    ' Declare an array to hold the bytes of the bitmap.
        '    '    ' This code is specific to a bitmap with 24 bits per pixels.
        '    '    Dim bytes As Integer = Math.Abs(bmpData.Stride) * b.Height
        '    '    Dim rgbValues(bytes - 1) As Byte

        '    '    '' Copy the RGB values into the array.
        '    '    'System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)

        '    '    '' Set every third value to 255. A 24bpp image will look red.
        '    '    'For counter As Integer = 2 To rgbValues.Length - 1 Step 3
        '    '    '    rgbValues(counter) = 255
        '    '    'Next

        '    '    ' Copy the RGB values back to the bitmap
        '    '    System.Runtime.InteropServices.Marshal.Copy(dark, 0, ptr, dark.Count)

        '    ' Unlock the bits.
        '    b.UnlockBits(raw)





        'Else '





        'End If

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



        If Me.cbSaveImages.Checked = True Then
            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


            b.Save(filename, myImageCodecInfo, myEncoderParameters)


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

    Public Function getLastImage() As Bitmap
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
        myEncoderParameter = New EncoderParameter(myEncoder, CType(100L, Int32))
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

            mySVCam.upper = Val(tbUpper.Text)
            mySVCam.lower = Val(tbLower.Text)

            Me.tbLost.Text = mySVCam.current_selected_cam.framesLost


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
        mySVCam.upper = Val(tbUpper.Text)
        mySVCam.lower = Val(tbLower.Text)

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

        mySVCam._darkmultiplier = Val(Me.tbMultiplier.Text)
        mySVCam.m_saveLocal = True
        If Me.cbUseTrigger.Checked Then
            mySVCam.prepareCameraForTriggerWidth(mySVCam.current_selected_cam)
        Else

            mySVCam.prepareCameraForTimed(mySVCam.current_selected_cam)
        End If
        If Me.cbUseTrigger.Checked Then

            mySVCam.startAcquisitionTriggerWidthThread(AddressOf Me.received_frame)
        Else
            mySVCam.startAcquisitionThread(AddressOf Me.received_frame)
        End If

        meteorCheckRunning = True
        t = New Thread(AddressOf processDetection)
        t.Start()

    End Sub

    Private Sub tbNightExp_TextChanged(sender As Object, e As EventArgs) Handles tbNightExp.TextChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        mySVCam.setParams(Val(Me.tbExposureTime.Text), Val(Me.tbNightAgain.Text))


        MsgBox("cover lens")
        If cbUseTrigger.Checked Then
            mySVCam.startAcquisitionThreadTriggerWidthForDarks(AddressOf Me.received_frame)
        Else
            mySVCam.startAcquisitionThreadForDarks(AddressOf Me.received_frame)
        End If


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
        If Not myWebServer Is Nothing Then
            myWebServer.StopWebServer()
        End If

        myWebServer = Nothing
        meteorCheckRunning = False
    End Sub

    Private Sub tbMultiplier_TextChanged(sender As Object, e As EventArgs) Handles tbMultiplier.TextChanged
        ' Me.myVistekImageGrabber.darkMultiplier = Val(tbMultiplier.Text)
    End Sub

    Private Sub tbDayGamma_TextChanged(sender As Object, e As EventArgs) Handles tbDayGamma.TextChanged

    End Sub

    Private Sub cmbCam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCam.SelectedIndexChanged
        mySVCam.openCamera(cmbCam.SelectedIndex)


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
    Public Sub processDetection()
        Dim aQE As queueEntry
        While (meteorCheckRunning)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()

                Functions.CallAzureMeteorDetection(aQE)


                aQE = Nothing

            End If
            'Console.WriteLine("in the queue:{0}", myDetectionQueue.Count)
            Thread.Sleep(200)
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

End Class