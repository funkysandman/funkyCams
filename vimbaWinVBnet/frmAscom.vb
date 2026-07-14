Imports System.Environment
Imports System.Collections.Specialized
Imports vimbaWinVBnet.vimbaWinVBnet
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Threading
Imports ASCOM.DriverAccess
Imports ASCOM.Utilities

Public Class frmAscom
    Inherits frmMaster

    ' ASCOM camera objects
    Private m_camera As ASCOM.DriverAccess.Camera
    Private m_cameraChooser As ASCOM.Utilities.Chooser
    Private m_selectedCameraId As String
    Private m_acquiringImages As Boolean = False
    Private m_acquisitionThread As Thread
    Private m_ascomPics As AscomRingBitmap ' Local ring bitmap for ASCOM images

    ' RingBitmap class for buffering images
    Public Class AscomRingBitmap
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
                Return m_Bitmaps(m_BitmapSelector)
            End Get
        End Property

        Public ReadOnly Property ImageBytes As Byte()
            Get
                Return m_buffers(m_BitmapSelector)
            End Get
        End Property

        Public ReadOnly Property width As Integer
            Get
                If m_Bitmaps(m_BitmapSelector) IsNot Nothing Then
                    Return m_Bitmaps(m_BitmapSelector).Width
                End If
                Return 0
            End Get
        End Property

        Public ReadOnly Property height As Integer
            Get
                If m_Bitmaps(m_BitmapSelector) IsNot Nothing Then
                    Return m_Bitmaps(m_BitmapSelector).Height
                End If
                Return 0
            End Get
        End Property

        Public ReadOnly Property dataSize As Integer
            Get
                If m_buffers(m_BitmapSelector) IsNot Nothing Then
                    Return m_buffers(m_BitmapSelector).Length
                End If
                Return 0
            End Get
        End Property

        Public Sub FillNextBitmap(b As Bitmap)
            SwitchBitmap()
            m_Bitmaps(m_BitmapSelector) = b
            ' Copy raw data into m_buffers
            Try
                Dim rawData(b.Width * b.Height * 3 - 1) As Byte ' 3 bytes per pixel for 24bpp
                Dim BoundsRect = New Rectangle(0, 0, b.Width, b.Height)
                Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.ReadOnly, b.PixelFormat)
                Dim ptr As IntPtr = bmpData.Scan0
                System.Runtime.InteropServices.Marshal.Copy(ptr, rawData, 0, rawData.Length)
                m_buffers(m_BitmapSelector) = rawData
                b.UnlockBits(bmpData)
            Catch ex As Exception
                Console.WriteLine("Error in FillNextBitmap: " & ex.Message)
            End Try
        End Sub

        Private Sub SwitchBitmap()
            m_BitmapSelector += 1
            If m_Size = m_BitmapSelector Then
                m_BitmapSelector = 0
            End If
        End Sub
    End Class



    ' Form Load - Initialize camera list
    Private Sub frmAscom_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            MyBase.Form_Load(sender, e)

            ' Load defaults
            tbPort.Text = "8999"
            tbPath.Text = "e:\image_ascom"
            tbDayTimeExp.Text = "0.1"
            tbNightExp.Text = "5"
            tbDayGain.Text = "100"
            tbNightAgain.Text = "300"

            ' Populate available ASCOM cameras
            PopulateCameraList()

        Catch ex As Exception
            MessageBox.Show("Error initializing ASCOM camera form: " & ex.Message, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Populate the camera list dropdown with available ASCOM cameras
    Private Sub PopulateCameraList()
        Try
            cmbCam.Items.Clear()

            ' Use ASCOM Profile to get list of registered cameras
            Dim profile As New ASCOM.Utilities.Profile()
            Dim cameras As ArrayList = profile.RegisteredDevices("Camera")

            For Each kvp As ASCOM.Utilities.KeyValuePair In cameras
                cmbCam.Items.Add(kvp.Key & " - " & kvp.Value)
            Next

            If cmbCam.Items.Count > 0 Then
                cmbCam.SelectedIndex = 0
            Else
                MessageBox.Show("No ASCOM cameras found. Please ensure ASCOM camera drivers are installed.", "No Cameras", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Error enumerating ASCOM cameras: " & ex.Message, "Camera List Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Handle camera selection change
    Private Sub cmbCam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCam.SelectedIndexChanged
        If cmbCam.SelectedIndex >= 0 Then
            ' Extract camera ID from the combo box text (format: "ID - Name")
            Dim selectedText As String = cmbCam.SelectedItem.ToString()
            If selectedText.Contains(" - ") Then
                m_selectedCameraId = selectedText.Substring(0, selectedText.IndexOf(" - "))
            End If
            loadProfile(m_selectedCameraId)
        End If
    End Sub

    ' Refresh camera list
    Private Sub btnRefreshCameras_Click(sender As Object, e As EventArgs) Handles btnRefreshCameras.Click
        PopulateCameraList()
    End Sub

    ' Choose camera using ASCOM chooser dialog
    Private Sub btnChooseCamera_Click(sender As Object, e As EventArgs)
        Try
            If m_cameraChooser Is Nothing Then
                m_cameraChooser = New ASCOM.Utilities.Chooser()
                m_cameraChooser.DeviceType = "Camera"
            End If

            m_selectedCameraId = m_cameraChooser.Choose(m_selectedCameraId)

            If Not String.IsNullOrEmpty(m_selectedCameraId) Then
                ' Update combo box to show selected camera
                For i As Integer = 0 To cmbCam.Items.Count - 1
                    If cmbCam.Items(i).ToString().StartsWith(m_selectedCameraId) Then
                        cmbCam.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("Error choosing camera: " & ex.Message, "Camera Chooser Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Connect to selected camera
    Private Function ConnectCamera() As Boolean
        Try
            If String.IsNullOrEmpty(m_selectedCameraId) Then
                MessageBox.Show("Please select a camera first.", "No Camera Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            If m_camera IsNot Nothing Then
                If m_camera.Connected Then
                    m_camera.Connected = False
                End If
                m_camera.Dispose()
                m_camera = Nothing
            End If

            ' Create and connect to camera
            m_camera = New ASCOM.DriverAccess.Camera(m_selectedCameraId)
            m_camera.Connected = True

            ' Display camera information
            Try
                If m_camera.CanSetCCDTemperature Then
                    Console.WriteLine("Camera temperature: " & m_camera.CCDTemperature.ToString("F1") & "°C")
                End If
            Catch
                ' Temperature not available
            End Try

            Console.WriteLine("Connected to: " & m_camera.Name)
            Console.WriteLine("Camera resolution: " & m_camera.CameraXSize & " x " & m_camera.CameraYSize)
            'hard code binning to 2x2 for now

            m_camera.BinX = 2
            m_camera.BinY = 2
            '
            'set cooling point to 0C
            m_camera.SetCCDTemperature = 0
            m_camera.CoolerOn = True
            Return True

        Catch ex As Exception
            MessageBox.Show("Error connecting to camera: " & ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' Set exposure time
    Private Sub SetExposure(exposureSeconds As Double)
        Try
            If m_camera IsNot Nothing AndAlso m_camera.Connected Then
                ' Exposure is set when calling StartExposure
                Console.WriteLine("Exposure set to " & exposureSeconds.ToString() & " seconds")
            End If
        Catch ex As Exception
            Console.WriteLine("Error setting exposure: " & ex.Message)
        End Try
    End Sub

    ' Set gain
    Private Sub SetGain(gain As Integer)
        Try
            If m_camera IsNot Nothing AndAlso m_camera.Connected Then
                ' Not all ASCOM cameras support gain control
                ' Try to set it, catch exception if not supported
                Try
                    m_camera.Gain = CShort(gain)
                    Console.WriteLine("Gain set to " & gain.ToString())
                Catch ex As Exception
                    Console.WriteLine("Camera does not support gain control: " & ex.Message)
                End Try
            End If
        Catch ex As Exception
            Console.WriteLine("Error setting gain: " & ex.Message)
        End Try
    End Sub

    ' Start image acquisition
    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Try
            If Not ConnectCamera() Then
                Return
            End If

            m_acquiringImages = True
            lost = 0
            running = True

            ' Set initial exposure and gain
            Dim exposureTime As Double = CDbl(tbExposureTime.Text)
            Dim gainValue As Integer = CInt(tbGain.Text)

            SetExposure(exposureTime)
            SetGain(gainValue)

            ' Start acquisition thread
            m_acquisitionThread = New Thread(AddressOf AcquireImages)
            m_acquisitionThread.Start()

            TimerFPS.Enabled = True
            btnStart.Enabled = False
            btnStop.Enabled = True

            startTime = Now
            meteorCheckRunning = True

            t = New Thread(AddressOf processDetection)
            t.Start()

        Catch ex As Exception
            MessageBox.Show("Error starting acquisition: " & ex.Message, "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Stop image acquisition
    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        Try
            m_acquiringImages = False
            running = False
            TimerFPS.Enabled = False
            meteorCheckRunning = False

            ' Wait for acquisition thread to finish
            If m_acquisitionThread IsNot Nothing AndAlso m_acquisitionThread.IsAlive Then
                m_acquisitionThread.Join(5000)
            End If

            ' Disconnect camera
            If m_camera IsNot Nothing AndAlso m_camera.Connected Then
                If m_camera.ImageReady Then
                    m_camera.StopExposure()
                End If
                m_camera.Connected = False
            End If

            btnStart.Enabled = True
            btnStop.Enabled = False

        Catch ex As Exception
            MessageBox.Show("Error stopping acquisition: " & ex.Message, "Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnStartWeb_Click(sender As Object, e As EventArgs) Handles btnStartWeb.Click
        btnStopWeb.Enabled = True
        btnStartWeb.Enabled = False
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"
    End Sub
    Private Sub btnStopWeb_Click(sender As Object, e As EventArgs) Handles btnStopWeb.Click
        btnStartWeb.Enabled = True
        btnStopWeb.Enabled = False
        myWebServer.StopWebServer()
    End Sub
    ' Acquire images continuously
    Private Sub AcquireImages()
        Try
            While m_acquiringImages
                If m_camera IsNot Nothing AndAlso m_camera.Connected Then
                    Dim exposureTime As Double = CDbl(tbExposureTime.Text)

                    ' Start exposure
                    m_camera.StartExposure(exposureTime, True) ' True = light frame

                    ' Wait for exposure to complete
                    While Not m_camera.ImageReady
                        Thread.Sleep(100)
                    End While

                    ' Get image data
                    Dim imageArray As Object = m_camera.ImageArray

                    ' Convert image array to bitmap
                    Dim bmp As Bitmap = ConvertImageArrayToBitmap(imageArray)

                    If bmp IsNot Nothing Then
                        ' Update the display
                        If m_ascomPics Is Nothing Then
                            m_ascomPics = New AscomRingBitmap(5)
                        End If
                        m_ascomPics.FillNextBitmap(bmp)

                        ' Handle image saving and processing
                        ProcessImage(bmp, exposureTime)
                    Else
                        Console.WriteLine("Failed to convert image array to bitmap")
                    End If

                    ' Update frame counter
                    running = True
                    frames = frames + 1
                    If frames Mod 100 = 0 Then
                        startTime = Now
                        frames = 0
                    End If

                    running = False
                Else
                    Thread.Sleep(1000)
                End If
            End While

        Catch ex As Exception
            Console.WriteLine("Error in acquisition loop: " & ex.Message)
        End Try
    End Sub

    ' Convert ASCOM image array to bitmap
    Private Function ConvertImageArrayToBitmap(imageArray As Object) As Bitmap
        Try
            If imageArray Is Nothing Then
                Return Nothing
            End If

            Dim width As Integer = m_camera.CameraXSize / m_camera.BinX
            Dim height As Integer = m_camera.CameraYSize / m_camera.BinY

            Dim bmp As New Bitmap(width, height, PixelFormat.Format24bppRgb)
            Dim imageData As Integer(,) = CType(imageArray, Integer(,))

            ' Find min and max values for scaling
            Dim minVal As Integer = Integer.MaxValue
            Dim maxVal As Integer = Integer.MinValue

            For y As Integer = 0 To height - 1
                For x As Integer = 0 To width - 1
                    Dim pixelValue As Integer = imageData(x, y)
                    If pixelValue < minVal Then minVal = pixelValue
                    If pixelValue > maxVal Then maxVal = pixelValue
                Next
            Next

            ' Convert to 8-bit and create bitmap
            Dim range As Integer = maxVal - minVal
            If range = 0 Then range = 1

            For y As Integer = 0 To height - 1
                For x As Integer = 0 To width - 1
                    Dim pixelValue As Integer = imageData(x, y)
                    Dim scaled As Byte = CByte(((pixelValue - minVal) * 255) \ range)
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(scaled, scaled, scaled))
                Next
            Next

            Return bmp

        Catch ex As Exception
            Console.WriteLine("Error converting image: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ' Get the last image from ring buffer for web server / detection
    Public Function getLastImage() As Bitmap
        If m_ascomPics IsNot Nothing AndAlso m_ascomPics.Image IsNot Nothing Then
            Return New Bitmap(m_ascomPics.Image)
        End If
        Return Nothing
    End Function

    Public Function getLastImageArray() As Byte()
        If m_ascomPics IsNot Nothing Then
            Return m_ascomPics.ImageBytes
        End If
        Return Nothing
    End Function

    ' Process and save image
    Private Sub ProcessImage(bmp As Bitmap, exposureTime As Double)
        Try
            Dim folderName As String = Now.ToString("ddMMMyyyy")
            Dim filename As String = Path.Combine(tbPath.Text, folderName, "img_" & Now.ToString("ddMMMyyyy-HHmmss") & ".jpg")

            ' Send to detection queue if meteor detection is enabled
            If cbMeteors.Checked AndAlso lblDayNight.Text = "night" Then
                Dim ms As New MemoryStream()
                bmp.Save(ms, myImageCodecInfo, myEncoderParameters)

                Dim contents = ms.ToArray()
                Dim qe As New queueEntry
                qe.img = contents
                qe.filename = Path.GetFileName(filename)
                qe.dateTaken = Now
                qe.cameraID = "ASCOM Camera: " & m_camera.Name
                qe.width = bmp.Width
                qe.height = bmp.Height

                If myDetectionQueue.Count < 10 Then
                    myDetectionQueue.Enqueue(qe)
                End If

                ms.Close()
            End If

            ' Save image if enabled
            If cbSaveImages.Checked AndAlso lblDayNight.Text = "night" Then
                System.IO.Directory.CreateDirectory(Path.Combine(tbPath.Text, folderName))
                bmp.Save(filename, myImageCodecInfo, myEncoderParameters)

                If t_cleanup.ThreadState = ThreadState.Unstarted OrElse t_cleanup.ThreadState = ThreadState.Stopped Then
                    t_cleanup = New Thread(AddressOf cleanFolders)
                    t_cleanup.Start()
                End If
            End If

        Catch ex As Exception
            Console.WriteLine("Error processing image: " & ex.Message)
        End Try
    End Sub

    ' Handle day/night mode changes
    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        If m_camera IsNot Nothing AndAlso m_camera.Connected Then
            SetExposure(CDbl(tbExposureTime.Text))
            SetGain(CInt(tbGain.Text))
        End If
    End Sub
    Private Sub TimerDayNight_Tick(sender As Object, e As EventArgs) Handles TimerDayNight.Tick

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

        Catch ex As Exception

        End Try
    End Sub

    ' Cleanup when form closes
    Private Sub frmAscom_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            m_acquiringImages = False
            running = False
            meteorCheckRunning = False

            ' Wait for threads to finish
            If m_acquisitionThread IsNot Nothing AndAlso m_acquisitionThread.IsAlive Then
                m_acquisitionThread.Join(2000)
            End If

            ' Disconnect and cleanup camera
            If m_camera IsNot Nothing Then
                If m_camera.Connected Then
                    If m_camera.ImageReady Then
                        m_camera.StopExposure()
                    End If
                    m_camera.Connected = False
                End If
                m_camera.CoolerOn = False
                m_camera.Dispose()
                m_camera = Nothing
            End If

        Catch ex As Exception
            Console.WriteLine("Error during form closing: " & ex.Message)
        End Try
    End Sub

End Class
