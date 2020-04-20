
Imports System.Environment
Imports SpinnakerNET
Imports SpinnakerNET.GenApi
Imports System.Collections.Specialized
Imports vimbaWinVBnet.vimbaWinVBnet
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Threading

Public Class frmPointGrey
    Inherits frmMaster

    'Private mhCamera As IntPtr
    'Private mDisplayPanel As myPanel
    'Private mIsMono As Boolean
    'Private mDisplayBitmap As Bitmap

    'Private myWebServer As WebServer
    'Private checkBox1 As CheckBox
    'Private tbExposure As TrackBar
    'Private lblExposureVal As Label
    'Private panel1 As Panel
    'Private gbAcquisition As GroupBox
    'Private gbExposure As GroupBox
    'Private gbGain As GroupBox
    'Private lblGainVal As Label
    'Private running As Boolean
    'Private gbInfo As GroupBox

    'Private lblCameraModel As Label
    'Private bmp As Bitmap
    'Private imageBytes As Byte()
    'Private bmp2 As Bitmap
    'Private lblSerNum As Label
    'Private frames As Integer

    'Private meteorCheckRunning As Boolean = False
    'Private m_camRunning As Boolean = False
    'Private m_grabbing As Boolean = False
    'Public lost_image As Integer = 0


    Private m_grabbedframe As Boolean
    Private m_grabbedframe_err As Integer = 0
    Private m_system As ManagedSystem
    Private m_camList As List(Of IManagedCamera)
    Public Shared m_cam As IManagedCamera
    Private m_nodeMap As INodeMap
    Private m_nodeMapTLDevice As INodeMap
    Private m_deviceListener As DeviceEventListener = Nothing
    Private m_imageEventListener As ImageEventListener = Nothing

    Shared m_imageCnt As Integer = 0

    Enum eventType
        Generic
        Specific
    End Enum

    Shared chosenEvent As eventType = eventType.Specific

    Public Class DeviceEventListener
        Inherits ManagedDeviceEvent

        Private specificEvent As String
        Private count As Integer

        ' This constructor registers an event name to be used on device
        ' events.
        Sub New(eventName As String)
            specificEvent = eventName
            count = 0
        End Sub

        Protected Overrides Sub OnDeviceEvent(eventName As String)
            m_imageCnt = m_imageCnt + 1
            ' Check that device event is registered
            If eventName = specificEvent Then
                count += 1

                ' Print information on specified device event
                Console.WriteLine("{0}Device event {1} with ID {2} number {3}...", vbTab, GetDeviceEventName(), GetDeviceEventID(), count)

            Else
                ' Print no information on non-specified information
                Console.WriteLine("{0}Device event occurred; not {1}; ignoring...", specificEvent)

            End If
            Dim deviceSerialNumber As String = "grasshopper"

            Try
                ' Retrieve next received image
                Using rawImage As IManagedImage = m_cam.GetNextImage()


                    ' Ensure image completion
                    If rawImage.IsIncomplete Then
                        Console.WriteLine("Image incomplete with image status {0}...", rawImage.ImageStatus)
                    Else

                        ' Print image information; width and height
                        ' recorded in pixels
                        Dim width As UInteger = rawImage.Width
                        Dim height As UInteger = rawImage.Height

                        Console.WriteLine("Grabbed image {0}, width = {1}, height = {2}", m_imageCnt, width, height)

                        ' Convert image to mono 8
                        Using convertedImage As IManagedImage = rawImage.Convert(PixelFormatEnums.BayerRG8)

                            ' Create a unique filename
                            Dim filename As String = "Exposure-VB-"

                            If deviceSerialNumber <> "" Then
                                filename = filename + deviceSerialNumber + "-"
                            End If

                            filename = filename + CStr(m_imageCnt) + ".jpg"

                            ' Save image
                            convertedImage.Save(filename)

                            Console.WriteLine("Image saved at {0}{1}", filename, NewLine)
                        End Using
                    End If
                End Using

            Catch ex As SpinnakerException
                Console.WriteLine("Error: {0}", ex.Message)

            End Try

        End Sub

        Protected Sub OnImageEvent(image As ManagedImage)
            'If imageCnt < NumImages Then
            Dim deviceSerialNumber As String = "grasshopper"
            Console.WriteLine("Image event occurred...")

            If image.IsIncomplete Then
                Console.WriteLine("Image incomplete with image status {0}...{1}", image.ImageStatus, NewLine)

            Else
                ' Convert image
                Using convertedImage As IManagedImage = image.Convert(PixelFormatEnums.BayerRG8, ColorProcessingAlgorithm.HQ_LINEAR)

                    ' Print image information
                    Console.WriteLine("Grabbed image {0}, width = {1}, height = {2}", m_imageCnt, convertedImage.Width, convertedImage.Height)

                    ' Create unique filename in order to save file
                    Dim filename As String = "ImageEvents-VB-"

                    If deviceSerialNumber <> "" Then
                        filename = filename + deviceSerialNumber + "-"
                    End If

                    filename = filename + Convert.ToString(m_imageCnt) + ".jpg"

                    ' Save image
                    convertedImage.Save(filename)

                    Console.WriteLine("Image saved at {0}{1}", filename, NewLine)

                    ' Incrememnt image counter
                    m_imageCnt += 1

                End Using
            End If

            ' Must manually release the image to prevent buffers on the camera stream from filling up
            image.Release()

            ' End If
        End Sub
    End Class
    Public Class ImageEventListener
        Inherits ManagedImageEvent

        Private deviceSerialNumber As String
        Public Const NumImages As Integer = 10
        Public imageCnt As Integer
        Private myForm As frmMaster
        ' The constructor retrieves the serial number and initializes the
        ' image counter to 0.
        Sub New(cam As IManagedCamera, f As frmMaster)
            myForm = f
            ' Initializes image counter to 0
            imageCnt = 0

            ' Retrieve device serial number
            Dim nodeMap As INodeMap = cam.GetTLDeviceNodeMap()

            deviceSerialNumber = ""

            Dim iDeviceSerialNumber As IString = nodeMap.GetNode(Of IString)("DeviceSerialNumber")

            If iDeviceSerialNumber IsNot Nothing AndAlso iDeviceSerialNumber.IsReadable Then
                deviceSerialNumber = iDeviceSerialNumber.Value
            End If
            cam = Nothing

        End Sub

        ' This method defines an image event. In it, the image that
        ' triggered the event is converted and saved before incrementing
        ' the count. Please see Acquisition_CSharp example for more
        ' in-depth comments on the acquisition of images.
        Protected Overrides Sub OnImageEvent(image As ManagedImage)

            myForm.running = True
            myForm.frames = myForm.frames + 1
            If myForm.frames Mod 100 = 0 Then
                myForm.startTime = Now
                myForm.frames = 0
            End If
            Console.WriteLine("Image event occurred...{0}", image.TimeStamp)

            If image.IsIncomplete Then
                myForm.lost = myForm.lost + 1

                Console.WriteLine("Image incomplete with image status {0}...{1}", image.ImageStatus, NewLine)
                'image.Release()
                'myForm.running = False

                'Exit Sub
            End If

            'image.Save("pgDark.raw")
            'darks
            If myForm.cbUseDarks.Checked And myForm.lblDayNight.Text = "night" Then
                If myForm.dark Is Nothing Then
                    myForm.dark = System.IO.File.ReadAllBytes("pgdark.raw")

                    'For i = 0 To myForm.dark.Length - 1
                    '    myForm.dark(i) = CByte(CInt(myForm.dark(i)) * mult)
                    'Next
                End If
                Dim mult As Decimal
                mult = Val(myForm.tbMultiplier.Text)
                For i = 0 To image.DataSize - 1
                    image.ManagedData(i) = CByte(Math.Max(0, CInt(image.ManagedData(i)) - CInt(myForm.dark(i)) * mult))
                Next
                '



                'copy managedData back to image
                System.Runtime.InteropServices.Marshal.Copy(image.ManagedData, 0, image.DataPtr, image.DataSize - 1)
                ' Convert image
                'image.PixelFormat = PixelFormatEnums.BayerRG16

            End If

            Dim mTransformImage As BGAPI2.Image = Nothing
            Dim mImage As BGAPI2.Image = Nothing
            ' Dim buff As BGAPI2.Buffer = New BGAPI2.Buffer()
            Dim imgProcessor As New BGAPI2.ImageProcessor()


            '            //copy back to imageInfo
            'Marshal.Copy(rawImage.imagebytes, 0, ImageInfo.pImagePtr, imageSizeX * imageSizeY);

            ''//debayer buffer into RGB
            ''//myApi.SVS_UtilBufferBayerToRGB(ImageInfo, ref imagebufferRGB[currentIdex].imagebytes[0], imageSizeX * imageSizeY );
            ''BGAPI2.Image mTransformImage = null;
            ''BGAPI2.Buffer mBufferFilled = New BGAPI2.Buffer();
            Dim pImagePtr As IntPtr
            Dim convertedImage As IManagedImage = image.Convert(PixelFormatEnums.RGB8, ColorProcessingAlgorithm.NEAREST_NEIGHBOR_AVG)

            mImage = imgProcessor.CreateImage(image.Width, image.Height, "BayerRG8", image.DataPtr, image.Width * image.Height)

            'ULong imageBufferAddress = (ULong)ImageInfo.pImagePtr;
            mTransformImage = imgProcessor.CreateTransformedImage(mImage, "BGR8")

            System.Runtime.InteropServices.Marshal.Copy(mTransformImage.Buffer, convertedImage.ManagedData, 0, image.Width * image.Height * 3)

            System.IO.File.WriteAllBytes("pgxxx.raw", image.ManagedData)
            System.IO.File.WriteAllBytes("pgxxxyy.raw", convertedImage.ManagedData)

            'Dim convertedImage As IManagedImage = image.Convert(PixelFormatEnums.RGB8, ColorProcessingAlgorithm.NEAREST_NEIGHBOR_AVG)
            System.IO.File.WriteAllBytes("pgconvert.raw", image.ManagedData)
            'convertedImage.ConvertToWriteAbleBitmap(PixelFormatEnums.BGR8, convertedImage)




            ' Print image information
            Console.WriteLine("Grabbed image {0}, width = {1}, height = {2}", imageCnt, image.Width, image.Height)


            'store in ring bitmap


            If myForm.m_pics Is Nothing Then
                myForm.m_pics = New frmMaster.RingBitmap(5)
            End If

            myForm.m_pics.FillNextBitmap(convertedImage)


            imageCnt += 1




            ' Must manually release the image to prevent buffers on the camera stream from filling up
            '  image.Release()
            Dim filename As String

            Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
            filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgpg_", DateTime.Now)
            filename = Path.Combine(myForm.tbPath.Text, folderName, filename)



            If myForm.cbMeteors.Checked And myForm.lblDayNight.Text.ToLower = "night" Then
                ' md.examine(bm, filename)
                'call azure service
                Dim ms As New MemoryStream()
                ' convertedImage.ConvertToWriteAbleBitmap()
                Dim b As Bitmap
                b = myForm.getLastImage

                b.Save(ms, myForm.myImageCodecInfo, myForm.myEncoderParameters)
                b.Dispose()

                Dim contents = ms.ToArray()
                Dim qe As New queueEntry
                qe.img = contents
                qe.filename = Path.GetFileName(filename)
                qe.dateTaken = Now
                qe.cameraID = "Point Grey Camera"
                qe.width = image.Width
                qe.height = image.Height
                If myForm.myDetectionQueue.Count < 10 Then
                    myForm.myDetectionQueue.Enqueue(qe)

                End If

                ms.Close()

            End If
            If myForm.cbSaveImages.Checked = True And myForm.lblDayNight.Text = "night" Then
                System.IO.Directory.CreateDirectory(Path.Combine(myForm.tbPath.Text, folderName))
                Dim x As Bitmap
                x = myForm.getLastImage

                x.Save(filename, myForm.myImageCodecInfo, myForm.myEncoderParameters)
                x.Dispose()


                If myForm.t_cleanup.ThreadState = ThreadState.Unstarted Or myForm.t_cleanup.ThreadState = ThreadState.Stopped Then
                    myForm.t_cleanup = New Thread(AddressOf myForm.cleanFolders)

                    myForm.t_cleanup.Start()
                Else

                    Debug.WriteLine("threadstate:" & myForm.t_cleanup.ThreadState)
                End If
            End If
            image.Release()

            convertedImage.Dispose()
            myForm.running = False


        End Sub
    End Class


    Private Function OpenCamera() As Boolean

        For Each managedCamera As IManagedCamera In m_camList


            Try
                ' Run example
                m_cam = managedCamera
                m_cam.Init()
                m_nodeMap = m_cam.GetNodeMap()
                m_nodeMapTLDevice = m_cam.GetTLDeviceNodeMap()
            Catch ex As SpinnakerException
                Console.WriteLine("Error: {0}", ex.Message)
                Return False
            End Try


        Next
        Return True
    End Function

    Function AcquireImages(ByRef cam As IManagedCamera, ByRef nodeMap As INodeMap, ByRef nodeMapGenTL As INodeMap) As Integer
        Dim result As Integer = 0

        Console.WriteLine("{0}*** IMAGE ACQUISITION ***{0}", NewLine)

        Try
            ' Set acquisition mode to continuous
            Dim iAcquisitionMode As IEnum = nodeMap.GetNode(Of IEnum)("AcquisitionMode")

            If iAcquisitionMode Is Nothing OrElse Not iAcquisitionMode.IsWritable Then
                Console.WriteLine("Unable to set acquisition mode to continuous (node retrieval). Aborting...{0}", NewLine)
                Return -1
            End If

            Dim iAcquisitionModeContinuous As IEnumEntry = iAcquisitionMode.GetEntryByName("Continuous")

            If iAcquisitionMode Is Nothing OrElse Not iAcquisitionModeContinuous.IsReadable Then
                Console.WriteLine("Unable to set acquisition mode to continuous (entry retrieval). Aborting...{0}", NewLine)
                Return -1
            End If

            iAcquisitionMode.Value = iAcquisitionModeContinuous.Symbolic

            Console.WriteLine("Acquisition mode set to continuous...")

            ' Begin acquiring images
            cam.BeginAcquisition()

            Console.WriteLine("Acquiring images...")

            ' Retrieve device serial number for filename
            Dim deviceSerialNumber As String = ""

            Dim iDeviceSerialNumber As IString = nodeMapGenTL.GetNode(Of IString)("DeviceSerialNumber")
            If iDeviceSerialNumber IsNot Nothing AndAlso iDeviceSerialNumber.IsReadable Then
                deviceSerialNumber = iDeviceSerialNumber.Value

                Console.WriteLine("Device serial number retrieved as {0}...", deviceSerialNumber)
            End If
            Console.WriteLine()

            ' Retrieve, convert, and save images
            Const NumImages As Integer = 5

            For imageCnt As Integer = 0 To NumImages - 1

            Next

        Catch ex As SpinnakerException
            Console.WriteLine("Error: {0}", ex.Message)
            result = -1
        End Try

        ' End acquisition
        ' cam.EndAcquisition()

        Return result
    End Function

    '

    Sub setExposure(ExposureTimeToSet As Double)
        Dim iExposureAuto As IEnum = m_nodeMap.GetNode(Of IEnum)("ExposureAuto")

        If iExposureAuto Is Nothing OrElse Not iExposureAuto.IsWritable Then
            Console.WriteLine("Unable to disable automatic exposure (enum retrieval). Aborting...{0}", NewLine)
            Exit Sub
        End If

        Dim iExposureAutoOff As IEnumEntry = iExposureAuto.GetEntryByName("Off")

        If iExposureAutoOff Is Nothing OrElse Not iExposureAutoOff.IsReadable Then
            Console.WriteLine("Unable to disable automatic exposure (entry retrieval). Aborting...{0}", NewLine)
            Exit Sub
        End If
        'turn off autoexposure
        iExposureAuto.Value = iExposureAutoOff.Symbolic

        'iExposureAuto.Value = iExposureAutoOff.Value
        Dim iExposureTime As IFloat = m_nodeMap.GetNode(Of IFloat)("ExposureTime")

        If iExposureTime Is Nothing OrElse Not iExposureTime.IsWritable Then
            Console.WriteLine("Unable to set exposure time. Aborting...{0}", NewLine)
            Exit Sub
        End If
        '
        'if exposure is less than 1 second then turn on framerate

        Dim iAcquisitionFrameRateEnable As IBool = m_nodeMap.GetNode(Of IBool)("AcquisitionFrameRateEnabled")
        Dim iAcquisitionFrameRateAuto As IEnum = m_nodeMap.GetNode(Of IEnum)("AcquisitionFrameRateAuto")
        Dim iAcquisitionFrameRateAutoModeOff As IEnumEntry = iAcquisitionFrameRateAuto.GetEntryByName("Off")

        Dim iAcquisitionFrameRate As IFloat = m_nodeMap.GetNode(Of IFloat)("AcquisitionFrameRate")
        If iAcquisitionFrameRateAuto.IsWritable Then
            iAcquisitionFrameRateAuto.Value = iAcquisitionFrameRateAutoModeOff.Symbolic

        End If

        If ExposureTimeToSet < 1000000 Then


            ' Dim iAcquisitionFrameRateOn As I
            iAcquisitionFrameRateEnable.Value = True
            'iAcquisitionFrameRateOn.Value = "On"







            If iAcquisitionFrameRate Is Nothing OrElse Not iAcquisitionFrameRate.IsReadable Then
                Console.WriteLine("Unable to retrieve frame rate. Aborting...")

            Else

                iAcquisitionFrameRate.Value = 1

                Console.WriteLine("Frame rate to be set to {0}", 1)

            End If

        Else 'long exposure
            iAcquisitionFrameRateEnable.Value = False

        End If

        ' Ensure desired exposure time does not exceed the maximum
        iExposureTime.Value = ExposureTimeToSet

        Console.WriteLine("Exposure time set to {0} us...{1}", iExposureTime.Value, NewLine)

    End Sub

    Sub setGain(GainToSet As Double)
        Dim iGainAuto As IEnum = m_nodeMap.GetNode(Of IEnum)("GainAuto")

        If iGainAuto Is Nothing OrElse Not iGainAuto.IsWritable Then
            Console.WriteLine("Unable to disable automatic gain (enum retrieval). Aborting...{0}", NewLine)
            Exit Sub
        End If

        Dim iGainAutoOff As IEnumEntry = iGainAuto.GetEntryByName("Off")

        If iGainAutoOff Is Nothing OrElse Not iGainAuto.IsReadable Then
            Console.WriteLine("Unable to disable automatic exposure (entry retrieval). Aborting...{0}", NewLine)
            Exit Sub
        End If

        iGainAuto.Value = iGainAutoOff.Value
        Dim iGain As IFloat = m_nodeMap.GetNode(Of IFloat)("Gain")

        If iGain Is Nothing OrElse Not iGain.IsWritable Then
            Console.WriteLine("Unable to set iGain. Aborting...{0}", NewLine)
            Exit Sub
        End If

        ' Ensure desired exposure time does not exceed the maximum
        iGain.Value = GainToSet

        Console.WriteLine("Gain set to {0} us...{1}", iGain.Value, NewLine)

    End Sub


    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        lost = 0
        running = True
        setExposure(CDbl(tbExposureTime.Text))
        setGain(CDbl(tbGain.Text))
        AcquireImages(m_cam, m_nodeMap, m_nodeMapTLDevice)





        TimerFPS.Enabled = True
        btnStart.Enabled = False
        btnStop.Enabled = True

        startTime = Now
        meteorCheckRunning = True

        t = New Thread(AddressOf processDetection)
        t.Start()
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        running = False
        TimerFPS.Enabled = False
        m_cam.EndAcquisition()
        meteorCheckRunning = False
        btnStart.Enabled = True
        btnStop.Enabled = False
    End Sub

    Private Sub frmPointGrey_Load(sender As Object, e As EventArgs) Handles Me.Load
        getCameraReady()

        MyBase.Form_Load(sender, e)
        'load defaults
        tbPort.Text = "8060"
        tbPath.Text = "e:\image_pg"
        tbDayTimeExp.Text = "500"
        tbNightExp.Text = "7500000"
        tbDayGain.Text = "0"
        tbNightAgain.Text = "27"
        loadProfile(m_cam.DeviceModelName.ToString().Replace(" ", ""))


    End Sub

    Private Sub getCameraReady()

        m_system = New ManagedSystem()



        ' Retrieve list of cameras from the system
        m_camList = m_system.GetCameras()

        Console.WriteLine("Number of cameras detected: {0}{1}{1}", m_camList.Count, NewLine)

        ' Finish if there are no cameras
        If m_camList.Count = 0 Then
            ' Clear camera list before releasing system
            m_camList.Clear()

            ' Release system
            m_system.Dispose()

            Console.WriteLine("Not enough cameras!")
            Console.WriteLine("Done! Press Enter to exit...")
            Console.ReadLine()

            Exit Sub
        End If


        If Not OpenCamera() Then
            'msgbox("cannot open camera")
            System.Windows.Forms.MessageBox.Show("The application was unable to connect to a Point Grey camera.  Please ensure one is connected and turned on before running this application.")
            System.Environment.[Exit](0)
        Else
            'msgbox("openned camera")
        End If
        'setup events

        'ConfigureDeviceEvents(m_nodeMap, m_cam, m_deviceListener)
        ConfigureImageEvents(m_cam, m_imageEventListener, Me)

    End Sub



    Private Sub btnStartWeb_Click(sender As Object, e As EventArgs) Handles btnStartWeb.Click
        btnStopWeb.Enabled = True
        btnStartWeb.Enabled = False
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"
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
        System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, m_pics.dataSize) 'copy into bitmap


        x.UnlockBits(bmpData)
        Return x

        'Return m_pics.Bitmap
    End Function
    Public Function getLastImageArray() As Byte()
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While running AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()

        'Dim x As New Bitmap(b)
        Debug.Print("get last image")
        Return m_pics.ImageBytes



    End Function

    Private Sub btnStopWeb_Click(sender As Object, e As EventArgs) Handles btnStopWeb.Click
        btnStartWeb.Enabled = True
        btnStopWeb.Enabled = False
        myWebServer.StopWebServer()
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



    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged

        If Not m_cam Is Nothing Then
            setExposure(CDbl(tbExposureTime.Text))
            setGain(CDbl(tbGain.Text))
        End If

    End Sub



    Private Sub TimerFPS_Tick(sender As Object, e As EventArgs) Handles TimerFPS.Tick

        Dim seconds As Integer

        seconds = DateDiff(DateInterval.Second, startTime, Now)
        txtFps.Text = frames / seconds
        tbLost.Text = Me.lost

    End Sub


    Function ConfigureDeviceEvents(ByRef nodeMap As INodeMap, ByRef cam As IManagedCamera, ByRef deviceEventListener As DeviceEventListener) As Integer
        Dim result As Integer = 0

        Console.WriteLine("{0}{0}***CONFIGURING DEVICE EVENT ***{0}", NewLine)

        Try
            '
            ' Retrieve device event selector
            '
            ' *** NOTES ***
            ' Each type of device event must be enabled individually. This
            ' is done by retrieving "EventSelector" (an enumeration node)
            ' and then enabling the device event on "EventNotification"
            ' (another enumeration node).
            '
            ' This example only deals with exposure end events. However,
            ' instead of only enabling exposure end events with a simpler
            ' device event function, all device events are enabled while
            ' the device event handler deals with ensuring that only
            ' exposure end events are considered. A more standard use-case
            ' might be to enable only the events of interest.
            '
            Dim iEventSelector As IEnum = nodeMap.GetNode(Of IEnum)("EventSelector")

            If iEventSelector Is Nothing OrElse Not iEventSelector.IsReadable Then
                Console.WriteLine("Unable to fetch event enumeration entries. Aborting...")
                Return -1
            End If

            Dim entries As EnumEntry() = iEventSelector.Entries

            Console.WriteLine("Enabling event selector entries...")

            '
            ' Enable device events
            '
            ' *** NOTES ***
            ' In order to enable a device event, the event selector and
            ' event notification nodes (both of type enumeration) must work
            ' in unison. The desired event must first be selected on the
            ' event selector node and then enabled on the event
            ' notification node.
            '
            For Each entry As IEnumEntry In entries

                ' Select entry on selector node
                If Not entry.IsAvailable OrElse Not entry.IsReadable Then

                    ' Skip if node fails
                    result = -1
                    Continue For

                End If

                If Not iEventSelector.IsWritable Then
                    Console.WriteLine("Unable to write to event selector node. Aborting...")
                    Return -1
                End If

                iEventSelector.Value = entry.Value

                ' Retrieve event notification node (an enumeration node)
                Dim iEventNotification As IEnum = nodeMap.GetNode(Of IEnum)("EventNotification")

                If iEventNotification Is Nothing OrElse Not iEventNotification.IsWritable Then
                    ' Skip if node fails
                    result = -1
                    Continue For
                End If

                ' Retrieve entry node to enable device event
                Dim iEventNotificationOn As IEnumEntry = iEventNotification.GetEntryByName("On")

                If iEventNotificationOn Is Nothing OrElse Not iEventNotificationOn.IsReadable Then
                    ' Skip if node fails
                    result = -1
                    Continue For
                End If

                iEventNotification.Value = iEventNotificationOn.Value

                Console.WriteLine("{0}{1}: enabled...", vbTab, entry.DisplayName)
            Next

            '
            ' Create device event
            '
            ' *** NOTES ***
            ' The class has been designed to take in the name of an event.
            ' If all events are registered generically, all event types
            ' will trigger a device event; on the other hand, if an event
            ' is registered specifically, only that event will trigger an
            ' event.
            '
            deviceEventListener = New DeviceEventListener("EventExposureEnd")

            '
            ' Register device event
            '
            ' *** NOTES ***
            ' Device events are registered to cameras. If there are multiple
            ' cameras, each camera must have any device events registered to
            ' it separately. Also, multiple device events may be registered
            ' to a single camera.
            '
            ' *** LATER ***
            ' Device events need to be unregistered manually. This must be
            ' done prior to releasing the system and while the device events
            ' are still in scope.
            '
            If chosenEvent = eventType.Generic Then
                ' Device event listeners registered generally will be
                ' triggered by any device events.
                cam.RegisterEvent(deviceEventListener)

                Console.WriteLine("Device event listener registered generally...")

            ElseIf chosenEvent = eventType.Specific Then
                ' Device event listeners registered to a specific event
                ' will only be triggered by the type of event that is
                ' registered.
                cam.RegisterEvent(deviceEventListener, "EventExposureEnd")

                Console.WriteLine("Device event listener registered specifically to EventExposureEnd events...")
            End If

        Catch ex As SpinnakerException
            Console.WriteLine("Error: {0}", ex.Message)
            result = -1
        End Try

        Return result

    End Function
    Shared Function ConfigureImageEvents(ByRef cam As IManagedCamera, ByRef imageEventListener As ImageEventListener, f As frmPointGrey) As Integer
        Dim result As Integer = 0

        Try
            '
            ' Create image event
            '
            ' *** NOTES ***
            ' The class has been constructed to accept a managed camera
            ' in order to allow the saving of images with the device
            ' serial number.
            '
            imageEventListener = New ImageEventListener(cam, f)

            '
            ' Register image event handler
            '
            ' *** NOTES ***
            ' Image events are registered to cameras. If there are
            ' multiple cameras, each camera must have the image events
            ' registered to it separately. Also, multiple image events may
            ' be registered to a single camera.
            '
            ' *** LATER ***
            ' Image events must be unregistered manually. This must be
            ' done prior to releasing the system and while the image
            ' events are still in scope.
            '
            cam.RegisterEvent(imageEventListener)

        Catch ex As SpinnakerException
            Console.WriteLine("Error: {0}", ex.Message)
            result = -1
        End Try

        Return result

    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'take ten darks
        m_cam.UnregisterEvent(m_imageEventListener)
        Dim numDarks As Integer = 10
        Dim numBytes As Integer = 0
        MsgBox("cover lens")
        setExposure(CDbl(tbExposureTime.Text))
        Dim rawImage As IManagedImage
        m_cam.BeginAcquisition()
        rawImage = m_cam.GetNextImage()
        numBytes = rawImage.DataSize
        Dim darks(numBytes) As Integer
        Dim darkBytes(numBytes) As Byte

        darkBytes = rawImage.ManagedData
        For i = 1 To numDarks
            rawImage = m_cam.GetNextImage()
            Debug.Print("image - {0}", i)
            For x = 0 To numBytes - 1
                darks(x) = darks(x) + CInt(rawImage.ManagedData(x))
            Next
            rawImage.Release()
        Next
        m_cam.EndAcquisition()
        m_cam.RegisterEvent(m_imageEventListener)
        For i = 0 To numBytes - 1
            darkBytes(i) = CByte(darks(i) / numDarks)
        Next
        System.IO.File.WriteAllBytes("pgDark.raw", darkBytes)
        MsgBox("finished darks")
    End Sub




End Class