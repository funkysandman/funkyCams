Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Net.Http
Imports System.Environment
Imports SpinnakerNET
Imports SpinnakerNET.GenApi
Public Class frmPointGrey
    Dim myDetectionQueue As New Queue(Of queueEntry)

    Private mhCamera As IntPtr
    Private mDisplayPanel As myPanel
    Private mIsMono As Boolean
    Private mDisplayBitmap As Bitmap

    Private myWebServer As WebServer
    Private checkBox1 As CheckBox
    Private tbExposure As TrackBar
    Private lblExposureVal As Label
    Private panel1 As Panel
    Private gbAcquisition As GroupBox
    Private gbExposure As GroupBox
    Private gbGain As GroupBox
    Private lblGainVal As Label
    Private running As Boolean
    Private gbInfo As GroupBox
    Private lblCameraModel As Label
    Private bmp As Bitmap
    Private imageBytes As Byte()
    Private bmp2 As Bitmap
    Private lblSerNum As Label
    Private frames As Integer
    Private startTime As DateTime
    Private gotFrameTime As DateTime
    Private dark() As Byte
    Private t As Thread
    Dim night As Boolean = False
    ' Private md As New ObjectDetection.TFDetector()
    Private myImageCodecInfo As ImageCodecInfo
    Private myEncoder As System.Drawing.Imaging.Encoder
    Private myEncoderParameter As EncoderParameter
    Private myEncoderParameters As EncoderParameters
    Private meteorCheckRunning As Boolean = False
    Private m_camRunning As Boolean = False
    Private m_grabbing As Boolean = False

    Shared m_pics As RingBitmap
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


    Private Class queueEntry

        Public img As Byte()
        Public filename As String

    End Class
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
        Private myForm As frmPointGrey
        ' The constructor retrieves the serial number and initializes the
        ' image counter to 0.
        Sub New(cam As IManagedCamera, f As frmPointGrey)
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
        End Sub

        ' This method defines an image event. In it, the image that
        ' triggered the event is converted and saved before incrementing
        ' the count. Please see Acquisition_CSharp example for more
        ' in-depth comments on the acquisition of images.
        Protected Overrides Sub OnImageEvent(image As ManagedImage)


            Console.WriteLine("Image event occurred...")

            If image.IsIncomplete Then
                Console.WriteLine("Image incomplete with image status {0}...{1}", image.ImageStatus, NewLine)
                image.Release()
                Exit Sub
            End If

            'image.Save("pgDark.raw")
            'darks
            If myForm.cbUseDarks.Checked Then
                    Dim dark As Byte()
                    dark = File.ReadAllBytes("pgdark.raw")
                    For i = 0 To image.DataSize - 1
                        image.ManagedData(i) = CByte(Math.Max(0, CInt(image.ManagedData(i)) - CInt(dark(i))))
                    Next
                    '



                    'copy managedData back to image
                    Marshal.Copy(image.ManagedData, 0, image.DataPtr, image.DataSize - 1)
                    ' Convert image
                    'image.PixelFormat = PixelFormatEnums.BayerRG16

                End If


                Dim convertedImage As IManagedImage = image.Convert(PixelFormatEnums.BayerRG8, ColorProcessingAlgorithm.NEAREST_NEIGHBOR)

                convertedImage.ConvertToBitmapSource(PixelFormatEnums.RGB8, convertedImage)


                'red/blue channels mixed up
                Dim channelR As Byte
                For i = 0 To convertedImage.ManagedData.Length - 3 Step 3
                    channelR = convertedImage.ManagedData(i + 2)
                    convertedImage.ManagedData(i + 2) = convertedImage.ManagedData(i)
                    convertedImage.ManagedData(i) = channelR
                Next

                Marshal.Copy(convertedImage.ManagedData, 0, convertedImage.DataPtr, convertedImage.DataSize - 1)


                ' Print image information
                Console.WriteLine("Grabbed image {0}, width = {1}, height = {2}", imageCnt, image.Width, image.Height)


                'store in ring bitmap

                'running = True
                If m_pics Is Nothing Then
                    m_pics = New RingBitmap(5)
                End If

                m_pics.FillNextBitmap(convertedImage)

                '' Create unique filename in order to save file
                'Dim filename As String = "ImageEvents-VB-"

                'If deviceSerialNumber <> "" Then
                '    filename = filename + deviceSerialNumber + "-"
                'End If

                'filename = filename + Convert.ToString(imageCnt) + ".jpg"

                '' Save image
                'convertedImage.Save(filename)

                'Console.WriteLine("Image saved at {0}{1}", filename, NewLine)

                ' Incrememnt image counter
                imageCnt += 1




            ' Must manually release the image to prevent buffers on the camera stream from filling up
            '  image.Release()


            image.Release()
        End Sub
    End Class

    Public Class RingBitmap

        Private m_Size As Integer = 0

        Private m_Bitmaps As ManagedImage()

        Private m_BitmapSelector As Integer = 0

        Private m_buffers()() As Byte
        Public Sub New(s As Integer)

            m_Size = s
            m_Bitmaps = New ManagedImage(m_Size - 1) {}
            ReDim m_buffers(m_Size - 1)
        End Sub
        Public ReadOnly Property Image As ManagedImage
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
        Public Sub FillNextBitmap(b As ManagedImage)
            SwitchBitmap()
            m_Bitmaps(m_BitmapSelector) = b
            'copy raw data into m_buffers
            Dim rawData(b.DataSize) As Byte
            ' Dim BoundsRect = New Rectangle(0, 0, b.Width, b.Height)
            ' Dim bmpData As System.Drawing.Imaging.BitmapData = m_Bitmaps(m_BitmapSelector).LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], m_Bitmaps(m_BitmapSelector).PixelFormat)
            'Dim ptr As IntPtr = bmpData.Scan0
            'System.Runtime.InteropServices.Marshal.Copy(b.DataPtr, ptr, 0, b.DataSize) 'copy into bitmap
            System.Runtime.InteropServices.Marshal.Copy(b.DataPtr, rawData, 0, b.DataSize) 'copy into array

            m_buffers(m_BitmapSelector) = rawData
            ' m_Bitmaps(m_BitmapSelector).UnlockBits(bmpData)
            'subtract darks
            'For i = 0 To rawData.Length - 1
            '    rawData(i) = Math.
            'Next
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

    'Private Sub frameCallback(ByVal userPtr As IntPtr, ByVal userData As UInteger, ByVal errcode, ByVal flags As UInteger)
    '    'If errcode <> QCamM_Err.qerrSuccess Then
    '    '    Debug.Print("framecallback error:" & errcode)
    '    '    Exit Sub
    '    'End If
    '    'Dim myFrame As QCamM_Frame
    '    'If running Then Exit Sub

    '    'running = True
    '    'If userData = 1 Then
    '    '    myFrame = mFrame1
    '    'ElseIf userData = 2 Then
    '    '    myFrame = mFrame2
    '    'Else
    '    '    Return
    '    'End If
    '    'Debug.Print("frame arrived")
    '    'Try
    '    '    'Dim width As UInteger = myFrame.width
    '    '    'Dim height As UInteger = myFrame.height

    '    '    'bmp = New Bitmap(width, height, PixelFormat.Format8bppIndexed)
    '    '    'Dim rawData(mFrame1.bufferSize) As Byte
    '    '    'ReDim imageBytes(mFrame1.bufferSize)
    '    '    If mIsMono Then
    '    '        'Dim ncp As System.Drawing.Imaging.ColorPalette = bmp.Palette
    '    '        'For j As Integer = 0 To 255
    '    '        '    ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
    '    '        'Next
    '    '        'bmp.Palette = ncp
    '    '        'Marshal.Copy(myFrame.pBuffer, rawData, 0, mFrame1.bufferSize)
    '    '        'Marshal.Copy(myFrame.pBuffer, imageBytes, 0, mFrame1.bufferSize)

    '    '        ''
    '    '        'Dim BoundsRect = New Rectangle(0, 0, width, height)
    '    '        'Dim bmpData As System.Drawing.Imaging.BitmapData = bmp.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], bmp.PixelFormat)

    '    '        'Dim ptr As IntPtr = bmpData.Scan0

    '    '        'Dim bytes As Integer = mFrame1.bufferSize
    '    '        If m_pics Is Nothing Then
    '    '            m_pics = New RingBitmap(5)
    '    '        End If
    '    '        m_pics.FillNextBitmap(myFrame)

    '    '        'Marshal.Copy(rawData, 0, ptr, bytes)
    '    '        'bmp.UnlockBits(bmpData)

    '    '        ' debug.Print("image")
    '    '        'For i = 1 To 512
    '    '        '    Debug.Print(rawData(i))
    '    '        'Next


    '    '        ' bmp = New Bitmap(New MemoryStream(rawData))

    '    '    End If

    '    '    'Else
    '    ''    QCamImgfnc.QCamM_BayerToRgb(QCamM_qcBayerInterp.qcBayerInterpFast, myFrame, mRgbFrame)
    '    ''    bmp = New Bitmap(CInt(width), CInt(height), CInt(width) * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer)
    '    ''End If

    '    ''Using fs As FileStream = New FileStream("image.raw", FileMode.Create)
    '    ''    Dim bw As BinaryWriter = New System.IO.BinaryWriter(fs)
    '    ''    Dim b As Byte
    '    ''    For i As Integer = 0 To mFrame1.size - 1
    '    ''        b = Marshal.ReadByte(myFrame.pBuffer, i)
    '    ''        bw.Write(b)

    '    ''    Next
    '    ''    bw.Close()
    '    ''End Using
    '    ''    mDisplayBitmap = bmp

    '    ''' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
    '    ''Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
    '    ''Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)
    '    '''b = bm.Clone


    '    '''try to draw on bitmap
    '    ''Dim tempbmp As Bitmap
    '    ''tempbmp = New Bitmap(m_pics.Image.Width, m_pics.Image.Height)


    '    '''From this bitmap, the graphics can be obtained, because it has the right PixelFormat
    '    ''Dim gr As Graphics = Graphics.FromImage(tempbmp)
    '    ''gr.DrawImage(m_pics.Image, 0, 0)
    '    ''Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)'
    'End Sub
    'Private Sub singleframeCallback(ByVal userPtr As IntPtr, ByVal userData As UInteger, ByVal errcode As QCamM_Err, ByVal flags As UInteger)
    '    '
    'End Sub
    Public Sub processDetection()
        Dim aQE As queueEntry
        While (meteorCheckRunning)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()

                CallAzureMeteorDetection(aQE.img, aQE.filename)


                aQE = Nothing

            End If
            Console.WriteLine("in the queue:{0}", myDetectionQueue.Count)
            Thread.Sleep(100)
        End While

    End Sub
    Public Async Function CallAzureMeteorDetection(contents As Byte(), file As String) As Task


        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.192:7071/api/detection?file=" + file

        Dim client As New HttpClient()

        Dim byteContent = New ByteArrayContent(contents)

        Dim response = client.PostAsync(apiURL, byteContent)
        Dim responseString = response.Result

    End Function
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

        iExposureAuto.Value = iExposureAutoOff.Value
        Dim iExposureTime As IFloat = m_nodeMap.GetNode(Of IFloat)("ExposureTime")

        If iExposureTime Is Nothing OrElse Not iExposureTime.IsWritable Then
            Console.WriteLine("Unable to set exposure time. Aborting...{0}", NewLine)
            Exit Sub
        End If
        '
        'if exposure is less than 1 second then turn on framerate
        Dim iAcquisitionFrameRateEnable As IBool = m_nodeMap.GetNode(Of IBool)("AcquisitionFrameRateEnabled")
        Dim iAcquisitionFrameRateOn As IEnum = m_nodeMap.GetNode(Of IEnum)("AcquisitionFrameRateAuto")
        Dim iAcquisitionFrameRate As IFloat = m_nodeMap.GetNode(Of IFloat)("AcquisitionFrameRate")
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
           ' iAcquisitionFrameRateOn.Value = "Off"

            'If iAcquisitionFrameRate Is Nothing OrElse Not iAcquisitionFrameRate.IsReadable Then
            '    Console.WriteLine("Unable to retrieve frame rate. Aborting...")

            'Else

            '    iAcquisitionFrameRate.Value = 1 / (CInt(tbExposureTime.Text) / 1000000)

            '    Console.WriteLine("Frame rate to be set to {0}", 1)

            'End If

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


    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        'use settings
        '  Dim err As QCamM_Err
        m_camRunning = True
        setExposure(CDbl(tbExposureTime.Text))
        setGain(CDbl(tbGain.Text))
        AcquireImages(m_cam, m_nodeMap, m_nodeMapTLDevice)




        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Text)))


        'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
        'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)

        If lblDayNight.Text = "night" Then
            'StartStream()
        Else
            TimerAcquistionRate.Enabled = True
        End If

        Button7.Enabled = False
        Button8.Enabled = True

        startTime = Now
        meteorCheckRunning = True
        Timer2.Enabled = True
        't = New Thread(AddressOf processDetection)
        't.Start()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        m_camRunning = False
        TimerAcquistionRate.Enabled = False
        m_cam.EndAcquisition()
        meteorCheckRunning = False
        Button7.Enabled = True
        Button8.Enabled = False
    End Sub

    Private Sub frmPointGrey_Load(sender As Object, e As EventArgs) Handles Me.Load
        getCameraReady()
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
        ' md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")
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

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Dim seconds As Integer

        seconds = DateDiff(DateInterval.Second, startTime, Now)
        txtFps.Text = frames / seconds
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

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Button5.Enabled = False
        Button6.Enabled = True
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"
    End Sub
    Public Function getLastImage() As Bitmap
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While running AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()

        'Dim x As New Bitmap(b)
        Debug.Print("get last image")

        Dim x As New Bitmap(m_pics.Image.Width, m_pics.Image.Height, PixelFormat.Format32bppArgb)
        Dim BoundsRect = New Rectangle(0, 0, m_pics.Image.Width, m_pics.Image.Height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], PixelFormat.Format32bppArgb)
        Dim ptr As IntPtr = bmpData.Scan0
        System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, m_pics.Image.DataSize) 'copy into bitmap


        x.UnlockBits(bmpData)
        Return x


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

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button5.Enabled = True
        Button6.Enabled = False
        myWebServer.StopWebServer()
    End Sub



    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

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



    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        'Dim err As QCamM_Err

        'If Not mSettings Is Nothing Then
        '    Debug.Print("setting gain to " & tbGain.Text)
        ''    Debug.Print("setting exposure to " & tbExposureTime.Text)
        'If lblDayNight.Text = "day" Then
        '    'set exposure and gain


        'Else
        '    TimerAcquistionRate.Enabled = False
        '    If m_camRunning Then
        '        StartStream()

        '    End If
        'End If

        '    QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Text)))
        '    QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
        '    err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        'End If
        If Not m_cam Is Nothing Then
            setExposure(CDbl(tbExposureTime.Text))
        End If

    End Sub

    Private Sub frmPointGrey_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing



    End Sub

    Private Sub TimerAcquistionRate_Tick(sender As Object, e As EventArgs) Handles TimerAcquistionRate.Tick
        ''take a picture
        'If m_grabbing Then
        '    Exit Sub
        'End If
        'm_grabbing = True
        'Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        'Dim sizeInBytes As UInteger = 0
        'Dim flags As Integer = 0
        ''QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, sizeInBytes)
        'Try
        '    If Not mFrame1 Is Nothing Then
        '        'msgbox("grabbing frame")
        '        Dim frameSize As UInteger = 0
        '        QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, frameSize)
        '        QCam.QCamM_Free(mFrame1.pBuffer)
        '        mFrame1.pBuffer = QCam.QCamM_Malloc(mFrame1.bufferSize)
        '        Debug.Print("grab frame")
        '        Dim t_grab As Thread
        '        m_grabbedframe = False
        '        t_grab = New Thread(AddressOf grabAframe)
        '        t_grab.Start()
        '        Dim stopWatch As Stopwatch = New Stopwatch()
        '        stopWatch.Start()

        '        While Not m_grabbedframe AndAlso stopWatch.ElapsedMilliseconds < 20000
        '            Threading.Thread.Sleep(1000)
        '            Application.DoEvents()
        '        End While

        '        stopWatch.[Stop]()
        '        If m_grabbedframe_err = QCamM_Err.qerrDriverFault Then
        '            m_grabbedframe = False
        '        End If
        '        If Not m_grabbedframe Then
        '            QCam.QCamM_Abort(mhCamera)
        '            t_grab.Abort()

        '            err = QCamM_Err.qerrCancelled
        '            Debug.Print("programmatically aborted grabframe thread")
        '            'what to do?  close camera?
        '            QCam.QCamM_CloseCamera(mhCamera)
        '            Debug.Print("closed camera")
        '            QCam.QCamM_Free(mFrame1.pBuffer)
        '            Debug.Print("released mframe1 buffer")
        '            QCam.QCamM_Free(mFrame2.pBuffer)
        '            Debug.Print("released mframe2 buffer")
        '            'QCam.QCamM_ReleaseDriver()
        '            'Debug.Print("released driver")
        '            'now reopen
        '            getCameraReady()
        '            Debug.Print("recovering....")
        '            m_grabbing = False
        '            Exit Sub
        '        End If


        '    Else
        '            TimerAcquistionRate.Enabled = False

        '        'msgbox("mframe1 is noting")
        '    End If

        'Catch ex As Exception
        '    'msgbox(ex.Message)
        '    'msgbox("QCam error:" & err)
        '    TimerAcquistionRate.Enabled = False

        'End Try

        'If err = QCamM_Err.qerrSuccess And m_grabbedframe_err = 0 Then
        '    Call Me.singleframeCallback(mFrame1.pBuffer, 1, err, flags)
        'End If
        'm_grabbing = False

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
        File.WriteAllBytes("pgDark.raw", darkBytes)
        MsgBox("finished darks")
    End Sub


End Class