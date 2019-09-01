Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading.Tasks
Imports System.Timers
Imports System.ComponentModel
Imports System.Net.Http
Imports System.Threading

Public Class frmToupcam
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
    Shared m_pics As RingBitmap
    Dim dark() As Byte
    Dim frames As Integer
    Dim startTime As Date
    Dim myImageCodecInfo As ImageCodecInfo
    Dim myEncoder As System.Drawing.Imaging.Encoder
    Dim myEncoderParameter As EncoderParameter
    Dim myEncoderParameters As EncoderParameters
    Dim t As Thread
    Dim meteorCheckRunning As Boolean = False
    Dim takingDarks As Boolean = False

    Private cam_ As Toupcam = Nothing
    Private bmp_ As Bitmap = Nothing
    Private MSG_CAMEVENT As UInteger = &H8001 ' WM_APP = 0x8000

    Public Class RingBitmap

        Private m_Size As Integer = 0

        Private m_Bitmaps As Bitmap()

        Private m_BitmapSelector As Integer = 0

        Private m_buffers()() As Byte
        Public Sub New(s As Integer)

            m_Size = s
            m_Bitmaps = New Bitmap(m_Size - 1) {}
            ReDim m_buffers(m_Size - 1)
        End Sub
        Public ReadOnly Property Image As Image
            Get
                Debug.Print("getting bitmap " & m_BitmapSelector)
                Return m_Bitmaps(m_BitmapSelector)
            End Get
        End Property
        Public ReadOnly Property ImageBytes As Byte()
            Get
                Debug.Print("getting bitmap " & m_BitmapSelector)
                'copy raw data to byte array


                Return m_buffers(m_BitmapSelector)
            End Get
        End Property

        Public Sub FillNextBitmap(b As Bitmap)
            SwitchBitmap()
            m_Bitmaps(m_BitmapSelector) = b
            'copy raw data into m_buffers
            Dim rawData(b.Width * b.Height) As Byte
            Dim BoundsRect = New Rectangle(0, 0, b.Width, b.Height)
            Dim bmpData As System.Drawing.Imaging.BitmapData = m_Bitmaps(m_BitmapSelector).LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], m_Bitmaps(m_BitmapSelector).PixelFormat)
            Dim ptr As IntPtr = bmpData.Scan0
            System.Runtime.InteropServices.Marshal.Copy(ptr, rawData, 0, b.Width * b.Height)
            m_buffers(m_BitmapSelector) = rawData
            m_Bitmaps(m_BitmapSelector).UnlockBits(bmpData)
            'subtract darks
            'For i = 0 To rawData.Length - 1
            '    rawData(i) = Math.
            'Next
        End Sub
        'Public Sub FillNextBitmap(info As Toupcam.FrameInfoV2)

        '    ' switch to Bitmap object which Is currently Not in use by GUI
        '    SwitchBitmap()
        '    Debug.Print("fillnextbitmap bitmapselector: " & m_BitmapSelector)
        '    Try

        '        If (m_Bitmaps(m_BitmapSelector) Is Nothing) Then
        '            Debug.Print("making new bitmap")
        '            m_Bitmaps(m_BitmapSelector) = New Bitmap(info.width, info.height, PixelFormat.Format8bppIndexed)
        '            Dim ncp As System.Drawing.Imaging.ColorPalette = m_Bitmaps(m_BitmapSelector).Palette
        '            For j As Integer = 0 To 255
        '                ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
        '            Next
        '            m_Bitmaps(m_BitmapSelector).Palette = ncp
        '        End If


        '    Catch
        '    End Try

        '    Try
        '        'copy frame into bitmap
        '        Dim rawData(info.) As Byte



        '        Marshal.Copy(Frame.pBuffer, rawData, 0, Frame.BufferSize)

        '        m_buffers(m_BitmapSelector) = rawData

        '        Dim BoundsRect = New Rectangle(0, 0, Frame.Width, Frame.Height)
        '        Dim bmpData As System.Drawing.Imaging.BitmapData = m_Bitmaps(m_BitmapSelector).LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], m_Bitmaps(m_BitmapSelector).PixelFormat)

        '        Dim ptr As IntPtr = bmpData.Scan0

        '        Dim bytes As Integer = Frame.BufferSize
        '        For i = 1 To 100
        '            Debug.Print(rawData(i))
        '        Next


        '        Marshal.Copy(rawData, 0, ptr, bytes)
        '        m_Bitmaps(m_BitmapSelector).UnlockBits(bmpData)
        '        m_Bitmaps(m_BitmapSelector).RotateFlip(RotateFlipType.Rotate180FlipNone)

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


    Private Sub OnEventError()
        If cam_ IsNot Nothing Then
            cam_.Close()
            cam_ = Nothing
        End If
        MessageBox.Show("Error")
    End Sub

    Private Sub OnEventDisconnected()
        If cam_ IsNot Nothing Then
            cam_.Close()
            cam_ = Nothing
        End If
        MessageBox.Show("The camera is disconnected, maybe has been pulled out.")
    End Sub

    Private Sub OnEventExposure()
        If cam_ IsNot Nothing Then
            Dim nTime As UInteger = 0
            If cam_.get_ExpoTime(nTime) Then
                'trackBar1.Value = CInt(nTime)
                'Label1.Text = (nTime \ 1000).ToString() & " ms"
            End If
        End If
    End Sub

    Private Sub OnEventImage()
        running = True
        If m_pics Is Nothing Then
            m_pics = New RingBitmap(5)
        End If

        If bmp_ IsNot Nothing Then
            Dim bmpdata As BitmapData = bmp_.LockBits(New Rectangle(0, 0, bmp_.Width, bmp_.Height), ImageLockMode.[WriteOnly], bmp_.PixelFormat)

            Dim info As New Toupcam.FrameInfoV2
            Dim ptr As IntPtr
            ptr = bmpdata.Scan0
            cam_.PullImageV2(ptr, 8, info)
            Dim numBytes As Integer = info.width * info.height
            Dim rawData(numBytes) As Byte
            System.Runtime.InteropServices.Marshal.Copy(ptr, rawData, 0, numBytes)
            'subtract darks
            If cbUseDarks.Checked Then
                dark = File.ReadAllBytes("toupdark.raw")


                '
                For x = 0 To numBytes - 1
                    rawData(x) = CByte(Math.Max(0, CLng(rawData(x)) - CLng(dark(x))))

                Next

            End If
            System.Runtime.InteropServices.Marshal.Copy(rawData, 0, bmpdata.Scan0, numBytes)
            bmp_.UnlockBits(bmpdata)
            Dim ncp As System.Drawing.Imaging.ColorPalette = bmp_.Palette
            For j As Integer = 0 To 255
                ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
            Next
            bmp_.Palette = ncp
            m_pics.FillNextBitmap(bmp_)

            Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
            Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)

            'Dim gr As Graphics = Graphics.FromImage(bmp_)
            'Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
            'Dim myBrushLabels As New SolidBrush(Color.White)

            'gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
            'gr.Dispose()



            Dim filename As String
            Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
            filename = String.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "img_" & myCamID, DateTime.Now)
            filename = Path.Combine(Me.tbPath.Text, folderName, filename)



            If Me.cbSaveImages.Checked = True Then
                System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


                bmp_.Save(filename, myImageCodecInfo, myEncoderParameters)


            End If

            If cbMeteors.Checked And lblDayNight.Text = "night" Then
                ' md.examine(bm, filename)
                'call azure service
                Dim ms As New MemoryStream()
                bmp_.Save(ms, System.Drawing.Imaging.ImageFormat.Png)

                Dim contents = ms.ToArray()
                Dim qe As New queueEntry
                qe.img = contents
                qe.filename = Path.GetFileName(filename)
                If myDetectionQueue.Count < 10 Then
                    myDetectionQueue.Enqueue(qe)
                End If
                'myDetectionQueue.Enqueue(New queueEntry(contents,))
                'callAzureMeteorDetection(contents, Path.GetFileName(filename))
                ms.Close()
            Else
                ' md.examine(bm)
                ' md.drawBoxesOnly(bm)
            End If


            PictureBox1.Image = bmp_
            PictureBox1.Invalidate()
            running = False
        End If
    End Sub

    Private Sub OnEventStillImage()
        Dim info As New Toupcam.FrameInfoV2
        If cam_.PullStillImageV2(IntPtr.Zero, 24, info) Then ' peek the width and height 
            Dim sbmp As New Bitmap(CInt(info.width), CInt(info.height), PixelFormat.Format24bppRgb)

            Dim bmpdata As BitmapData = sbmp.LockBits(New Rectangle(0, 0, sbmp.Width, sbmp.Height), ImageLockMode.[WriteOnly], sbmp.PixelFormat)
            cam_.PullStillImageV2(bmpdata.Scan0, 24, info)
            sbmp.UnlockBits(bmpdata)

            'sbmp.Save("demowinformvb.jpg")
        End If
    End Sub
    <System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name:="FullTrust")>
    Protected Overrides Sub WndProc(ByRef m As Message)
        If MSG_CAMEVENT = m.Msg Then
            Select Case m.WParam.ToInt32()
                Case Toupcam.eEVENT.EVENT_ERROR
                    OnEventError()
                    Exit Select
                Case Toupcam.eEVENT.EVENT_DISCONNECTED
                    OnEventDisconnected()
                    Exit Select
                Case Toupcam.eEVENT.EVENT_EXPOSURE
                    OnEventExposure()
                    Exit Select
                Case Toupcam.eEVENT.EVENT_IMAGE
                    If Not takingDarks Then
                        OnEventImage()
                    End If

                    Exit Select
                Case Toupcam.eEVENT.EVENT_STILLIMAGE
                    OnEventStillImage()
                    Exit Select
                Case Toupcam.eEVENT.EVENT_TEMPTINT
                    OnEventTempTint()
                    Exit Select
            End Select
            Return
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub OnEventTempTint()
        If cam_ IsNot Nothing Then
            Dim nTemp As Integer = 0, nTint As Integer = 0
            If cam_.get_TempTint(nTemp, nTint) Then
                'Label2.Text = nTemp.ToString()
                'Label3.Text = nTint.ToString()
                'trackBar2.Value = nTemp
                'trackBar3.Value = nTint
            End If
        End If
    End Sub
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
    'Private Sub received_frame(sender As Object, args As FrameEventArgs)

    '    b = New Bitmap(CInt(args.Frame.Width), CInt(args.Frame.Height), PixelFormat.Format24bppRgb)


    '    'receive raw data and convert to color
    '    Dim f As Frame
    '    f = args.Frame

    '    ' b = args.Image
    '    ' b.Tag = "orig"
    '    running = True
    '    'start timeout timer
    '    writeline("received frame")
    '    'myTimer.Stop()
    '    'myTimer.Start()

    '    frames = frames + 1
    '    If frames Mod 100 = 0 Then
    '        startTime = Now
    '        frames = 0
    '    End If

    '    writeline("got image " & Now)

    '    'darks
    '    ' Dim d2 As Bitmap

    '    If Me.cbUseDarks.Checked And lblDayNight.Text = "night" Then
    '        'd2 = Bitmap.FromFile(Application.StartupPath & "\dark.png")
    '        If dark Is Nothing Then
    '            Dim fs As New FileStream(Application.StartupPath & "\dark_" & v.m_Camera.Id & ".raw", FileMode.Open)
    '            'read dark from file

    '            ReDim dark(b.Width * b.Height * 2)
    '            fs.Read(dark, 0, dark.Count)
    '            fs.Close()
    '        End If
    '        Dim pixValue = 0
    '        Dim darkValue = 0
    '        Dim x
    '        Try


    '            For x = 0 To dark.Length - 2 Step 2

    '                pixValue = (f.Buffer(x + 1) * 256) + f.Buffer(x)
    '                darkValue = (dark(x + 1) * 256) + dark(x)
    '                If darkValue > 500 Then
    '                    pixValue = Math.Max(0, pixValue - darkValue)
    '                    f.Buffer(x + 1) = Int(pixValue / 256)
    '                    f.Buffer(x) = pixValue And 255
    '                End If

    '            Next
    '        Catch ex As Exception
    '            Debug.Print(x)
    '        End Try


    '        'Dim raw As System.Drawing.Imaging.BitmapData = Nothing
    '        '' 'Freeze the image in memory
    '        'raw = b.LockBits(New Rectangle(0, 0,
    '        ' b.Width, b.Height),
    '        ' System.Drawing.Imaging.ImageLockMode.ReadOnly,
    '        'b.PixelFormat)
    '        'Dim size As Integer = b.Width * b.Height * 3

    '        'Dim rawImage() As Byte = New Byte(size - 1) {}
    '        '''Copy the image into the byte()
    '        'System.Runtime.InteropServices.Marshal.Copy(raw.Scan0, rawImage, 0, size)


    '        'Dim multiplier
    '        'multiplier = Val(Me.tbMultiplier.Text)
    '        ''
    '        ''subtract the dark
    '        'If cbUseDarks.Checked Then
    '        '    Dim aByte As Integer
    '        '    Try

    '        '        Dim aNewValue As Byte
    '        '        Dim offset As Integer
    '        '        For aByte = 0 To size - 1
    '        '            If dark(aByte) > 220 Then
    '        '                aNewValue = CByte(Math.Max(0, CLng(rawImage(aByte)) - CLng(dark(aByte))))
    '        '                rawImage(aByte) = aNewValue
    '        '            End If

    '        '        Next
    '        '        writeline("subtracted dark")
    '        '    Catch ex As Exception
    '        '        MsgBox(ex.Message)
    '        '    End Try
    '        'End If

    '        'Dim raw2 As System.Drawing.Imaging.BitmapData = Nothing


    '        '' 'Freeze the image in memory

    '        ''raw2 = d2.LockBits(New Rectangle(0, 0,
    '        '' d2.Width, d2.Height),
    '        '' System.Drawing.Imaging.ImageLockMode.ReadOnly,
    '        ''d2.PixelFormat)
    '        ''size = raw2.Height * raw2.Stride

    '        '' Dim rawImage2() As Byte = New Byte(size - 1) {}
    '        '' 'Copy the image into the byte()
    '        'System.Runtime.InteropServices.Marshal.Copy(rawImage, 0, raw.Scan0, size)

    '        ''If Not raw2 Is Nothing Then
    '        ''    ' Unfreeze the memory for the image
    '        ''    d2.UnlockBits(raw2)
    '        ''End If



    '        'b.UnlockBits(raw)



    '    Else '
    '        'copy buffer into bitmap
    '        'b = New Bitmap(CInt(args.Frame.Width), CInt(args.Frame.Height), PixelFormat.Format24bppRgb)

    '        '' Lock the bitmap's bits.  
    '        'Dim rect As New Rectangle(0, 0, b.Width, b.Height)
    '        'Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b.PixelFormat)

    '        '' Get the address of the first line.
    '        'Dim ptr As IntPtr = bmpData.Scan0

    '        '' Declare an array to hold the bytes of the bitmap.
    '        '' This code is specific to a bitmap with 24 bits per pixels.
    '        'Dim bytes As Integer = Math.Abs(bmpData.Stride) * b.Height
    '        'Dim rgbValues(bytes - 1) As Byte

    '        ''' Copy the RGB values into the array.
    '        ''System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)

    '        ''' Set every third value to 255. A 24bpp image will look red.
    '        ''For counter As Integer = 2 To rgbValues.Length - 1 Step 3
    '        ''    rgbValues(counter) = 255
    '        ''Next

    '        '' Copy the RGB values back to the bitmap
    '        'System.Runtime.InteropServices.Marshal.Copy(args.Frame.Buffer, 0, ptr, args.Frame.BufferSize)

    '        '' Unlock the bits.
    '        'b.UnlockBits(bmpData)



    '    End If

    '    Dim iTotBytes As Integer = 0
    '    Dim sResponse As String = ""

    '    'convert frame to bitmap

    '    f.Fill(b)

    '    ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
    '    Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
    '    Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)

    '    Dim gr As Graphics = Graphics.FromImage(b)
    '    Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
    '    Dim myBrushLabels As New SolidBrush(Color.White)

    '    gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
    '    gr.Dispose()



    '    Dim filename As String
    '    Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
    '    filename = String.Format("{0}-{1:ddMMMyyyy-HHmmss}.jpg", "img_" & myCamID, DateTime.Now)
    '    filename = Path.Combine(Me.tbPath.Text, folderName, filename)



    '    If Me.cbSaveImages.Checked = True Then
    '        System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


    '        b.Save(filename, myImageCodecInfo, myEncoderParameters)


    '    End If
    '    If cbMeteors.Checked And lblDayNight.Text = "night" Then
    '        ' md.examine(bm, filename)
    '        'call azure service
    '        Dim ms As New MemoryStream()
    '        b.Save(ms, System.Drawing.Imaging.ImageFormat.Png)

    '        Dim contents = ms.ToArray()
    '        Dim qe As New queueEntry
    '        qe.img = contents
    '        qe.filename = Path.GetFileName(filename)
    '        If myDetectionQueue.Count < 10 Then
    '            myDetectionQueue.Enqueue(qe)
    '        End If
    '        'myDetectionQueue.Enqueue(New queueEntry(contents,))
    '        'callAzureMeteorDetection(contents, Path.GetFileName(filename))
    '        ms.Close()
    '    Else
    '        ' md.examine(bm)
    '        ' md.drawBoxesOnly(bm)
    '    End If
    '    running = False

    'End Sub
    Public Sub processDetection()
        Dim aQE As queueEntry
        While (meteorCheckRunning)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()
                callAzureMeteorDetection(aQE.img, aQE.filename)

                aQE = Nothing

            End If
            ' Console.WriteLine(myDetectionQueue.Count)
            Application.DoEvents()
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
        Return bmp_
    End Function

    Public Sub writeline(s As String)
        'Console.WriteLine("AVT GigE: " & v.m_Camera.Id & ":" & s)
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try


            Timer1.Enabled = False


            Dim currentMode As Boolean
            currentMode = False

            If Now.Hour >= Me.cboNight.SelectedItem Or Now.Hour <= cboDay.SelectedItem Then
                currentMode = True
            Else
                currentMode = False
            End If

            If currentMode <> night Or Not nightset Then
                night = currentMode
                nightset = True

                If night Then
                    tbGain.Text = tbNightAgain.Text
                    tbExposureTime.Text = tbNightExp.Text
                    lblDayNight.Text = "night"
                    ''night mode
                    cam_.put_ExpoTime(Val(Me.tbExposureTime.Text))
                    cam_.put_ExpoAGain(Val(tbGain.Text))

                Else
                    'v.m_Camera.LoadCameraSettings(Application.StartupPath & "\day_gc1380ch.xml")
                    'day mode

                    tbExposureTime.Text = tbDayTimeExp.Text
                    tbGain.Text = tbDayGain.Text
                    lblDayNight.Text = "day"
                    '' m_CCamera.setGainExposure(Val(Me.tbDayGain.Text), Val(Me.tbExposureTime.Text))
                    cam_.put_ExpoTime(Val(Me.tbExposureTime.Text))
                    cam_.put_ExpoAGain(Val(tbGain.Text))


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



        Dim arr As Toupcam.DeviceV2() = Toupcam.[EnumV2]()
        If arr.Length <= 0 Then
            MessageBox.Show("no device")
        Else
            For Each x In arr
                cbCam.Items.Add(x)
            Next
        End If




        If cam_ IsNot Nothing Then
            'checkBox1.Enabled = True
            'trackBar1.Enabled = True
            'trackBar2.Enabled = True
            'trackBar3.Enabled = True
            'ComboBox1.Enabled = True
            'Button2.Enabled = True
            'Button3.Enabled = True
            'Button2.ContextMenuStrip = Nothing
            'InitSnapContextMenuAndExpoTimeRange()

            'trackBar2.SetRange(2000, 15000)
            'trackBar3.SetRange(200, 2500)
            OnEventTempTint()

                Dim resnum As UInteger = cam_.ResolutionNumber
                Dim eSize As UInteger = 0
                If cam_.get_eSize(eSize) Then
                    For i As UInteger = 0 To resnum - 1
                        Dim w As Integer = 0, h As Integer = 0
                        If cam_.get_Resolution(i, w, h) Then
                        ' cboDay.Items.Add(w.ToString() & "*" & h.ToString())
                    End If
                    Next
                ' cboDay.SelectedIndex = CInt(eSize)

                Dim width As Integer = 0, height As Integer = 0
                    If cam_.get_Size(width, height) Then
                    bmp_ = New Bitmap(width, height, PixelFormat.Format8bppIndexed)
                    If Not cam_.StartPullModeWithWndMsg(Me.Handle, MSG_CAMEVENT) Then
                            MessageBox.Show("failed to start device")
                        Else
                            Dim autoexpo As Boolean = True
                            cam_.get_AutoExpoEnable(autoexpo)

                    End If
                    End If
                End If
            End If




        ' Dim cams As List(Of CameraInfo)
        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1


        'startup()
        ' cams = v.CameraList
        'md.LoadModel("c:\tmp\frozen_inference_graph.pb", "c:\tmp\object-detection.pbtxt")
        '  md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")




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
        takingDarks = True
        Try


            Dim numDarks As Integer = 10
            Dim numBytes As Integer = 0
            MsgBox("cover lens")
            cam_.put_ExpoTime(Val(Me.tbExposureTime.Text))
            cam_.put_ExpoAGain(Val(tbGain.Text))

            Dim ptr As IntPtr
            Dim pInfo As Toupcam.FrameInfoV2
            Dim w, h As Integer
            cam_.StartPullModeWithWndMsg(Me.Handle, MSG_CAMEVENT)
            While Not (cam_.PullImageV2(ptr, 8, pInfo))
                Application.DoEvents()
            End While

            bmp_ = New Bitmap(pInfo.width, pInfo.height, PixelFormat.Format8bppIndexed)
            numBytes = pInfo.width * pInfo.height
            Dim bmpdata As BitmapData = bmp_.LockBits(New Rectangle(0, 0, pInfo.width, pInfo.height), ImageLockMode.[WriteOnly], bmp_.PixelFormat)
            ptr = bmpdata.Scan0

            Dim darks(numBytes) As Integer
            Dim darkBytes(numBytes) As Byte
            System.Runtime.InteropServices.Marshal.Copy(ptr, darkBytes, 0, numBytes)

            For i = 1 To numDarks
                cam_.put_ExpoTime(Val(Me.tbExposureTime.Text))
                cam_.put_ExpoAGain(Val(tbGain.Text))
                cam_.StartPullModeWithWndMsg(Me.Handle, MSG_CAMEVENT)
                While Not (cam_.PullImageV2(ptr, 8, pInfo))
                    Application.DoEvents()
                End While
                System.Runtime.InteropServices.Marshal.Copy(ptr, darkBytes, 0, numBytes)
                Debug.Print("image - {0}", i)
                For x = 0 To numBytes - 1
                    darks(x) = darks(x) + CInt(darkBytes(x))
                Next

            Next
            ptr = Nothing
            bmp_.UnlockBits(bmpdata)
            For i = 0 To numBytes - 1
                darkBytes(i) = CByte(darks(i) / numDarks)
            Next
            File.WriteAllBytes("toupDark.raw", darkBytes)
            cam_.Stop()
            MsgBox("finished darks")
            takingDarks = False
        Catch ex As Exception
            MsgBox("error taking darks:" & ex.Message)
            takingDarks = False
        End Try
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

        myWebServer.StartWebServer(cam_, Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"

        If Now.Hour > Me.cboNight.SelectedItem Or Now.Hour < cboDay.SelectedItem Then
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
    Function getLastImageArray()
        Return m_pics.ImageBytes
    End Function

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

        Button7.Enabled = False
        Button8.Enabled = True
        startTime = Now
        Timer1.Enabled = True
        Timer3.Enabled = True
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


        'If Now.Hour >= ComboBox2.SelectedItem Or Now.Hour <= ComboBox1.SelectedItem Then
        '    night = True 'night
        'Else
        '    night = False 'day
        'End If
        If cam_ IsNot Nothing Then

            ' OnEventTempTint()

            Dim resnum As UInteger = cam_.ResolutionNumber
            Dim eSize As UInteger = 0
            If cam_.get_eSize(eSize) Then
                For i As UInteger = 0 To resnum - 1
                    Dim w As Integer = 0, h As Integer = 0
                    If cam_.get_Resolution(i, w, h) Then
                        '  ComboBox1.Items.Add(w.ToString() & "*" & h.ToString())
                    End If
                Next
                ' ComboBox1.SelectedIndex = CInt(eSize)
                Dim worked As Boolean
                Dim _min, _max, _def As UShort
                worked = cam_.get_ExpoAGainRange(_min, _max, _def)
                Dim width As Integer = 0, height As Integer = 0
                If cam_.get_Size(width, height) Then
                    bmp_ = New Bitmap(width, height, PixelFormat.Format8bppIndexed)
                    If Not cam_.StartPullModeWithWndMsg(Me.Handle, MSG_CAMEVENT) Then
                        MessageBox.Show("failed to start device")
                    Else
                        Dim autoexpo As Boolean = False
                        cam_.put_AutoExpoEnable(autoexpo)
                        cam_.put_ExpoTime(Val(Me.tbExposureTime.Text))
                        cam_.put_ExpoAGain(Val(tbGain.Text))
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Button7.Enabled = True
        Button8.Enabled = False
        cam_.Stop()
        ' v.StopContinuousImageAcquisition()
        meteorCheckRunning = False
        'myCam.StopContinuousImageAcquisition()
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
        cam_ = Toupcam.Open(cbCam.SelectedItem.id)
        'myCamID = myCam.Id
        'myCam.Open(VmbAccessModeType.VmbAccessModeFull)
        'v.OpenCamera(cbCam.SelectedItem)
    End Sub



    Private Sub frmToupcam_Leave(sender As Object, e As EventArgs) Handles Me.Leave

    End Sub




    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click

        FolderBrowserDialog1.ShowDialog()
        tbPath.Text = FolderBrowserDialog1.SelectedPath

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

    Private Sub frmToupcam_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        On Error Resume Next
        myWebServer.StopWebServer()
        ' v.StopContinuousImageAcquisition()
        myWebServer = Nothing
        ' v = Nothing

    End Sub



    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        If Not cam_ Is Nothing Then
            'If lblDayNight.Text = "night" Then
            cam_.put_ExpoTime(Val(Me.tbExposureTime.Text))
            cam_.put_ExpoAGain(Val(tbGain.Text))
            '
        End If

    End Sub



    Private Sub frmToupcam_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If cam_ IsNot Nothing Then
            cam_.Close()
            cam_ = Nothing
        End If
    End Sub

    Private Sub lblDayNight_Click(sender As Object, e As EventArgs) Handles lblDayNight.Click

    End Sub
End Class
