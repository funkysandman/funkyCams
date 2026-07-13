Imports System.Environment
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports BitMiracle.LibTiff.Classic
Imports SpinnakerNET
Imports SpinnakerNET.GenApi
Imports SVCamApi

Public Class frmPointGrey
    Inherits frmMaster

    'Private ReadOnly m_astroCalibration As New AstroCalibration.CalibrationPipeline()
    Private m_loadedCalibrationFile As String = Nothing

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
    Private ReadOnly m_lastFrameLock As New Object()
    Private m_lastFrameBytes As Byte()
    Private m_lastFramePixels As UShort()
    Private m_lastFrameWidth As Integer
    Private m_lastFrameHeight As Integer

    Shared m_imageCnt As Integer = 0

    Enum eventType
        Generic
        Specific
    End Enum

    Shared chosenEvent As eventType = eventType.Specific

    Public Class DeviceEventListener
        Inherits ManagedDeviceEventHandler

        Private specificEvent As String
        Private count As Integer

        ' This constructor registers an event name to be used on device
        ' events.
        Sub New(eventName As String)
            specificEvent = eventName
            count = 0
        End Sub

        'Protected Overrides Sub OnDeviceEvent(eventName As String)
        '    m_imageCnt = m_imageCnt + 1
        '    ' Check that device event is registered
        '    If eventName = specificEvent Then
        '        count += 1

        '        ' Print information on specified device event
        '        Console.WriteLine("{0}Device event {1} with ID {2} number {3}...", vbTab, GetDeviceEventName(), GetDeviceEventID(), count)

        '    Else
        '        ' Print no information on non-specified information
        '        Console.WriteLine("{0}Device event occurred; not {1}; ignoring...", specificEvent)

        '    End If
        '    Dim deviceSerialNumber As String = "grasshopper"

        '    Try
        '        ' Retrieve next received image
        '        Using rawImage As IManagedImage = m_cam.GetNextImage()


        '            ' Ensure image completion
        '            If rawImage.IsIncomplete Then
        '                Console.WriteLine("Image incomplete with image status {0}...", rawImage.ImageStatus)
        '            Else

        '                ' Print image information; width and height
        '                ' recorded in pixels
        '                Dim width As UInteger = rawImage.Width
        '                Dim height As UInteger = rawImage.Height

        '                Console.WriteLine("Grabbed image {0}, width = {1}, height = {2}", m_imageCnt, width, height)

        '                ' Convert image to mono 8
        '                Using convertedImage As IManagedImage = rawImage.Convert(PixelFormatEnums.BayerRG8)

        '                    ' Create a unique filename
        '                    Dim filename As String = "Exposure-VB-"

        '                    If deviceSerialNumber <> "" Then
        '                        filename = filename + deviceSerialNumber + "-"
        '                    End If

        '                    filename = filename + CStr(m_imageCnt) + ".jpg"

        '                    ' Save image
        '                    convertedImage.Save(filename)

        '                    Console.WriteLine("Image saved at {0}{1}", filename, NewLine)
        '                End Using
        '            End If
        '        End Using

        '    Catch ex As SpinnakerException
        '        Console.WriteLine("Error: {0}", ex.Message)

        '    End Try

        'End Sub

        'Protected Sub OnImageEvent(image As ManagedImage)
        '    'If imageCnt < NumImages Then
        '    Dim deviceSerialNumber As String = "grasshopper"
        '    Console.WriteLine("Image event occurred...")

        '    If image.IsIncomplete Then
        '        Console.WriteLine("Image incomplete with image status {0}...{1}", image.ImageStatus, NewLine)

        '    Else
        '        ' Convert image
        '        Using convertedImage As IManagedImage = image.Convert(PixelFormatEnums.BayerRG8, ColorProcessingAlgorithm.HQ_LINEAR)

        '            ' Print image information
        '            Console.WriteLine("Grabbed image {0}, width = {1}, height = {2}", m_imageCnt, convertedImage.Width, convertedImage.Height)

        '            ' Create unique filename in order to save file
        '            Dim filename As String = "ImageEvents-VB-"

        '            If deviceSerialNumber <> "" Then
        '                filename = filename + deviceSerialNumber + "-"
        '            End If

        '            filename = filename + Convert.ToString(m_imageCnt) + ".jpg"

        '            ' Save image
        '            convertedImage.Save(filename)

        '            Console.WriteLine("Image saved at {0}{1}", filename, NewLine)

        '            ' Increjemnt image counter
        '            m_imageCnt += 1

        '        End Using
        '    End If

        '    ' Must manually release the image to prevent buffers on the camera stream from filling up
        '    image.Release()

        '    ' End If
        'End Sub
    End Class
    Public Class ImageEventListener
        Inherits ManagedImageEventHandler

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
            cam = Nothing

        End Sub

        'Private Sub ApplyAstroCalibration(image As ManagedImage)
        '    Try
        '        If image Is Nothing OrElse image.ImageStatus <> ImageStatus.IMAGE_NO_ERROR Then
        '            Return
        '        End If

        '        If image.PixelFormat <> PixelFormatEnums.BayerRG16 Then
        '            Return
        '        End If

        '        Dim width As Integer = image.Width
        '        Dim height As Integer = image.Height
        '        Dim stride As Integer = image.Stride
        '        Dim ptr As IntPtr = image.DataPtr
        '        Dim bufferSize As Integer = image.GetBufferSize

        '        If width <= 0 OrElse height <= 0 OrElse ptr = IntPtr.Zero Then
        '            Return
        '        End If

        '        If stride <= 0 OrElse bufferSize < (height * stride) OrElse stride < (width * 2) Then
        '            Return
        '        End If

        '        Dim bytes() As Byte = image.ManagedData
        '        If bytes Is Nothing OrElse bytes.Length < (height * stride) Then
        '            Return
        '        End If

        '        Dim rawFrame(height - 1, width - 1) As Single

        '        For y As Integer = 0 To height - 1
        '            Dim rowOffset As Integer = y * stride
        '            For x As Integer = 0 To width - 1
        '                Dim idx As Integer = rowOffset + (x * 2)
        '                Dim hi As Byte = bytes(idx)
        '                Dim lo As Byte = bytes(idx + 1)
        '                rawFrame(y, x) = CSng((CInt(hi) << 8) Or CInt(lo))
        '            Next
        '        Next

        '        Dim sensorTemp As Single = 0.0F
        '        If frmPointGrey.m_cam IsNot Nothing Then
        '            sensorTemp = CSng(frmPointGrey.m_cam.DeviceTemperature.Value)
        '        End If

        '        Dim gain As Single = 0.0F
        '        Single.TryParse(myForm.tbGain.Text, NumberStyles.Float, CultureInfo.InvariantCulture, gain)

        '        Dim exposure As Single = 0.0F
        '        Single.TryParse(myForm.tbExposureTime.Text, NumberStyles.Float, CultureInfo.InvariantCulture, exposure)

        '        Dim frame As New AstroCalibration.Frame With {
        '            .Image = rawFrame,
        '            .Temperature = sensorTemp,
        '            .Gain = gain,
        '            .Exposure = exposure,
        '            .Timestamp = DateTime.Now
        '        }

        '        Dim calibratedFrame As Single(,)
        '        SyncLock myForm.m_astroCalibration
        '            calibratedFrame = myForm.m_astroCalibration.Process(frame)
        '        End SyncLock

        '        For y As Integer = 0 To height - 1
        '            Dim rowOffset As Integer = y * stride
        '            For x As Integer = 0 To width - 1
        '                Dim idx As Integer = rowOffset + (x * 2)
        '                Dim value As Integer = CInt(Math.Round(calibratedFrame(y, x)))
        '                If value < 0 Then value = 0
        '                If value > UShort.MaxValue Then value = UShort.MaxValue

        '                bytes(idx) = CByte((value >> 8) And &HFF)
        '                bytes(idx + 1) = CByte(value And &HFF)
        '            Next
        '        Next

        '        Marshal.Copy(bytes, 0, ptr, Math.Min(bytes.Length, bufferSize))
        '    Catch ex As Exception
        '        Debug.WriteLine("ApplyAstroCalibration failed: " & ex.ToString())
        '    End Try
        'End Sub

        Sub SubtractDark_StrideSafe(image As ManagedImage, darkBytes() As Byte)
            If darkBytes Is Nothing Then Throw New ArgumentException("dark is Nothing")
            Dim width As Integer = image.Width
            Dim height As Integer = image.Height
            Dim stride As Integer = image.Stride
            Dim ptr As IntPtr = image.DataPtr

            Dim darkHasStride As Boolean = (darkBytes.Length = image.GetBufferSize)
            Dim darkNoStride As Boolean = (darkBytes.Length = width * height * 2)
            If Not darkHasStride AndAlso Not darkNoStride Then
                Throw New ArgumentException($"dark length {darkBytes.Length} does not match image size ({image.GetBufferSize}) nor width*height*2 ({width * height * 2})")
            End If

            For y As Integer = 0 To height - 1
                Dim rowPtr As IntPtr = IntPtr.Add(ptr, y * stride)
                Dim darkRowOffset As Integer
                If darkHasStride Then
                    darkRowOffset = y * stride
                Else
                    darkRowOffset = y * width * 2
                End If

                For x As Integer = 0 To width - 1
                    Dim idx As Integer = x * 2
                    Dim darkIdx As Integer = darkRowOffset + idx

                    ' read image big-endian
                    Dim hi As Byte = Marshal.ReadByte(rowPtr, idx)
                    Dim lo As Byte = Marshal.ReadByte(rowPtr, idx + 1)
                    Dim imgVal As Integer = (hi << 8) Or lo

                    ' read dark big-endian from appropriate layout
                    Dim dhi As Byte = darkBytes(darkIdx)
                    Dim dlo As Byte = darkBytes(darkIdx + 1)
                    Dim darkVal As Integer = (dhi << 8) Or dlo

                    ' subtract and clamp
                    Dim r As Integer = imgVal ' - darkVal
                    If r < 0 Then r = 0
                    If r > UShort.MaxValue Then r = UShort.MaxValue
                    Dim outVal As UShort = CUShort(r)

                    ' write back big-endian
                    Marshal.WriteByte(rowPtr, idx, CByte((outVal >> 8) And &HFF))
                    Marshal.WriteByte(rowPtr, idx + 1, CByte(outVal And &HFF))
                Next
            Next
        End Sub

        Public Sub SaveDarkSubtractedTiff(image As SpinnakerNET.IManagedImage, dark() As Byte, outputPath As String)
            Dim width As Integer = image.Width
            Dim height As Integer = image.Height
            Dim stride As Integer = image.Stride
            Dim pixelCount As Integer = width * height

            Dim ptr As IntPtr = image.DataPtr

            ' Prepare output buffer for 16-bit TIFF
            ' Assuming outputBytes has been initialized to store the output image data
            Dim outputBytes(pixelCount * 2 - 1) As Byte
            Dim outzero = 0
            For idx = 1 To pixelCount * 2 - 2 Step 2




                ' Read big-endian from image
                Dim hi As Byte = Marshal.ReadByte(ptr, idx)
                Dim lo As Byte = Marshal.ReadByte(ptr, idx + 1)
                Dim val As Integer = (CInt(hi) << 8) Or CInt(lo)
                Dim val2 As Integer = (CInt(lo) << 8) Or CInt(hi)

                ' Read big-endian from dark frame
                Dim dhi As Byte = dark(idx)       ' Assuming dark is an array of bytes
                Dim dlo As Byte = dark(idx + 1)
                Dim dval As Integer = (CInt(dhi) << 8) Or CInt(dlo)
                Dim dval2 As Integer = (CInt(dlo) << 8) Or CInt(dhi)
                ' Subtract and clamp



                Dim r As Integer = val - dval
                If r < 0 Then
                    r = 0
                    outzero = outzero + 1
                    'Debug.Print("hi:{0}, lo:{1}, dhi:{2}, dlo:{3}", hi, lo, dhi, dlo)
                End If
                If r > &HFFFF Then r = &HFFFF

                Dim outVal As UShort = CUShort(r)
                Dim outHigh As Byte = CByte((outVal >> 8) And &HFF)
                Dim outLow As Byte = CByte(outVal And &HFF)

                ' --- Write to output buffer (tight array, no stride) ---
                outputBytes(idx) = outHigh
                outputBytes(idx + 1) = outLow

                If outHigh <= 0 And outLow <= 0 Then

                    '
                End If

                Dim xxxx = 0

            Next
            Debug.Print("outzero: " + outzero.ToString)
            ' Write 16-bit TIFF using LibTiff.NET
            Using tiff As Tiff = Tiff.Open(outputPath, "w")
                tiff.SetField(TiffTag.IMAGEWIDTH, width)
                tiff.SetField(TiffTag.IMAGELENGTH, height)
                tiff.SetField(TiffTag.BITSPERSAMPLE, 16)
                tiff.SetField(TiffTag.SAMPLESPERPIXEL, 1)
                tiff.SetField(TiffTag.ROWSPERSTRIP, height)
                tiff.SetField(TiffTag.COMPRESSION, Compression.NONE)
                tiff.SetField(TiffTag.PHOTOMETRIC, Photometric.MINISBLACK)
                tiff.SetField(TiffTag.PLANARCONFIG, PlanarConfig.CONTIG)

                For y As Integer = 0 To height - 1
                    ' Write each row (row length = width * 2 bytes)
                    tiff.WriteScanline(outputBytes, y * width * 2, y, 0)
                Next
            End Using
        End Sub

        ' This method defines an image event. In it, the image that
        ' triggered the event is converted and saved before incrementing
        ' the count. Please see Acquisition_CSharp example for more
        ' in-depth comments on the acquisition of images.


        Protected Overrides Sub OnImageEvent(image As ManagedImage)
            Dim convertedImage As ManagedImage = Nothing
            Dim convertedImageTemp As ManagedImage = Nothing
            Dim pixelCount As Integer = image.Width * image.Height
            Dim pixelArray(pixelCount - 1) As UShort
            Dim ptr As IntPtr = image.DataPtr
            Dim i As Integer = 0
            Try
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

                'check temperature

                'fetch temperature
                'Try
                '    myForm.txtTemp.Invoke(Sub()
                '                              myForm.txtTemp.Text = m_cam.DeviceTemperature.Value.ToString("0.0")
                '                          End Sub)
                'Catch
                'End Try

                'If String.Equals(myForm.lblDayNight.Text, "night", StringComparison.OrdinalIgnoreCase) Then
                '    ApplyAstroCalibration(image)
                'End If

                If myForm.cbUseDarks.Checked And myForm.lblDayNight.Text = "night" Then
                    If myForm.dark Is Nothing Then
                        myForm.dark = System.IO.File.ReadAllBytes("pgdark" + m_cam.DeviceSerialNumber.Value + ".raw")

                        'For i = 0 To myForm.dark.Length - 1
                        '    myForm.dark(i) = CByte(CInt(myForm.dark(i)) * mult)
                        'Next
                    End If

                    'SaveDarkSubtractedTiff(image, myForm.dark, "dark_subtracted_libtiff.tiff")

                    'SubtractDark_StrideSafe(image, myForm.dark)
                    '    Dim mult As Decimal
                    '    Dim range As Integer
                    '    range = myForm.tbUpper.Text - myForm.tbLower.Text
                    '    Dim multiplier As Single
                    '    multiplier = 65355 / range
                    '    'Dim lower, upper As Integer
                    '    'lower = CInt(myForm.tbLower.Text)
                    '    'upper = CInt(myForm.tbUpper.Text)
                    '    Dim darkCutOff As Integer = myForm.tbDarkCutOff.Text
                    '    mult = Val(myForm.tbMultiplier.Text)
                    '    Debug.WriteLine("PixelFormat = " & image.PixelFormat.ToString())
                    '    'images are stored as bayerRG16
                    ' assume image.GetBufferSize and ManagedData already valid
                    '--- Assume:
                    ' image        = your Spinnaker IManagedImage (16-bit BayerRG, big-endian)
                    ' dark         = byte() array, contiguous big-endian dark frame, Width*Height*2 bytes
                    ' convertedImg = IManagedImage for BGR8 output

                    '--- Assume:
                    ' image = your Spinnaker IManagedImage (16-bit BayerRG, big-endian)
                    ' dark  = byte() array, contiguous big-endian dark frame, Width*Height*2 bytes

                    Dim width As Integer = image.Width
                    Dim height As Integer = image.Height
                    Dim stride As Integer = image.Stride


                    ' Create output buffer for 16-bit image (big-endian)
                    Dim outputBytes(width * height * 2 - 1) As Byte
                    Dim mult = myForm.tbMultiplier.Text
                    Dim cutoff = myForm.tbDarkCutOff.Text

                    For idx As Integer = 1 To pixelCount * 2 - 2 Step 2


                        ' --- Read light pixel (big-endian) ---
                        Dim hi As Byte = Marshal.ReadByte(ptr, idx)
                        Dim lo As Byte = Marshal.ReadByte(ptr, idx + 1)
                        Dim val As Integer = (CInt(hi) << 8) Or CInt(lo)

                        ' --- Read dark pixel (big-endian) ---
                        Dim darkHi As Byte = myForm.dark(idx)
                        Dim darkLo As Byte = myForm.dark(idx + 1)
                        Dim dval As Integer = (CInt(darkHi) << 8) Or CInt(darkLo)

                        ' --- Subtract and clamp ---
                        ' reduce dark by multiplier - multiplier is a number between 0 and 1


                        If dval < cutoff Then dval = 0
                        Dim r As Integer = val - CInt(dval * mult)
                        If r < 0 Then r = 0
                        If r > UShort.MaxValue Then r = UShort.MaxValue
                        Dim outVal As UShort = CUShort(r)
                        Dim outHigh As Byte = CByte((outVal >> 8) And &HFF)
                        Dim outLow As Byte = CByte(outVal And &HFF)

                        ' --- Write to output buffer (tight array, no stride) ---
                        pixelArray(i) = outVal
                        outputBytes(idx) = outHigh
                        outputBytes(idx + 1) = outLow
                        i = i + 1
                    Next

                    ' Copy back to unmanaged buffer
                    Marshal.Copy(outputBytes, 0, ptr, outputBytes.Length)

                End If
                ''stretch image
                'image.Save("image.raw", ImageFileFormat.Tiff)
                'Dim value As Integer



                ''For i = 0 To image.GetBufferSize - 1 Step 2  ' This loop converts from 16bit to 8bit using min and max
                ''    pixel = image.ManagedData(i) + image.ManagedData(i + 1) * 256
                ''    'value = value >> 2

                ''    ''Debug.Print(value)
                ''    'If value < 0 Then ' Type cast from short to ushort? Forget it: Not with VB
                ''    '    value = value * -1
                ''    '    value = value + &H8000
                ''    'End If
                ''    'value = value - lower
                ''    'If value < 0 Then
                ''    '    value = 0
                ''    'End If



                ''    image.ManagedData(i + 1) = CByte(pixel >> 8)

                ''    image.ManagedData(i) = CByte(pixel And &HFF)



                ''Next

                '' Marshal.Copy(Image24, 0, bmpData.Scan0, isize) ' Copy intermediate buffer to the bitmap




                Dim mTransformImage As BGAPI2.Image = Nothing
                Dim mImage As BGAPI2.Image = Nothing
                ' Dim buff As BGAPI2.Buffer = New BGAPI2.Buffer()
                'Dim imgProcessor As New BGAPI2.ImageProcessor()


                'fetch temperature
                If Not m_cam Is Nothing Then

                    If myForm.cbFan.Checked Then
                        m_cam.LineSelector.Value = LineSelectorEnums.Line1
                        m_cam.LineMode.Value = LineModeEnums.Output
                        m_cam.LineSource.Value = 2
                        m_cam.UserOutputValue.Value = True
                        m_cam.V3_3Enable.Value = True
                    Else
                        m_cam.LineSelector.Value = LineSelectorEnums.Line1
                        m_cam.LineMode.Value = LineModeEnums.Output
                        m_cam.LineSource.Value = 2
                        m_cam.UserOutputValue.Value = False
                        'm_cam.V3_3Enable.Value = False
                    End If
                End If

                convertedImage = New ManagedImage()
                convertedImageTemp = New ManagedImage()
                Dim processor As IManagedImageProcessor
                ' image.Convert(PixelFormatEnums.RGB8, ColorProcessingAlgorithm.NEAREST_NEIGHBOR_AVG)
                'image.ConvertToBitmapSource(PixelFormatEnums.RGB8, ColorProcessingAlgorithm.NEAREST_NEIGHBOR_AVG)
                processor = New ManagedImageProcessor
                If image.ImageStatus = ImageStatus.IMAGE_NO_ERROR Then
                    If image.PixelFormat <> PixelFormatEnums.BayerRG16 Then
                        image = processor.Convert(image, PixelFormatEnums.BayerRG16)
                    End If
                    convertedImageTemp = processor.Convert(image, PixelFormatEnums.BGR8)
                    convertedImageTemp.ConvertToBitmapSource(PixelFormatEnums.BGR8, convertedImage, ColorProcessingAlgorithm.HQ_LINEAR)
                    i = 0
                    For idx As Integer = 1 To pixelCount * 2 - 2 Step 2


                        ' --- Read light pixel (big-endian) ---
                        Dim hi As Byte = Marshal.ReadByte(ptr, idx)
                        Dim lo As Byte = Marshal.ReadByte(ptr, idx + 1)
                        Dim val As Integer = (CInt(hi) << 8) Or CInt(lo)
                        pixelArray(i) = val
                        i = i + 1
                    Next

                    ' Print image information
                    Console.WriteLine("Grabbed image {0}, width = {1}, height = {2}", imageCnt, image.Width, image.Height)


                    'store in ring bitmap


                    If myForm.m_pics Is Nothing Then
                        myForm.m_pics = New frmMaster.RingBitmap(5)
                    End If

                    myForm.m_pics.FillNextBitmap(convertedImage)

                    myForm.StoreLatestRawFrame(convertedImage.ManagedData, pixelArray, convertedImage.Width, convertedImage.Height)

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

                            ' Debug.WriteLine("threadstate:" & myForm.t_cleanup.ThreadState)
                        End If
                    End If
                Else
                    Console.WriteLine("Image incomplete with image status {0}...{1}", image.ImageStatus, NewLine)
                End If
            Catch ex As Exception
                Debug.WriteLine("OnImageEvent failed: " & ex.ToString())
            Finally
                If image IsNot Nothing Then
                    Try
                        image.Release()
                    Catch
                    End Try
                End If

                If convertedImageTemp IsNot Nothing Then
                    Try
                        convertedImageTemp.Dispose()
                    Catch
                    End Try
                End If

                If convertedImage IsNot Nothing Then
                    Try
                        convertedImage.Dispose()
                    Catch
                    End Try
                End If

                myForm.running = False
            End Try


        End Sub
    End Class


    Private Function OpenCamera() As Boolean

        Dim managedCamera As IManagedCamera
        managedCamera = m_camList.Item(cmbCam.SelectedIndex)

        Try
            ' Run example
            managedCamera.Init()
            'If managedCamera.DeviceSerialNumber.ToString() = cmbCam.SelectedItem.ToString().Split(" "c)(UBound(cmbCam.SelectedItem.ToString().Split(" "c))) Then
            Console.WriteLine("Opening camera {0}...{1}", managedCamera.DeviceSerialNumber, NewLine)

            m_cam = managedCamera
            m_cam.Init()
            m_nodeMap = m_cam.GetNodeMap()
            m_nodeMapTLDevice = m_cam.GetTLDeviceNodeMap()
            m_cam.PixelFormat.Value = "BayerRG16"

            ' End If

        Catch ex As SpinnakerException
            Console.WriteLine("Error: {0}", ex.Message)
            Return False
        End Try

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

        Dim iAcquisitionFrameRateEnable As IBool = m_nodeMap.GetNode(Of IBool)("AcquisitionFrameRateEnable")

        If iAcquisitionFrameRateEnable Is Nothing OrElse Not iAcquisitionFrameRateEnable.IsWritable Then
            iAcquisitionFrameRateEnable = m_nodeMap.GetNode(Of IBool)("AcquisitionFrameRateEnabled") 'might like the letter d
        End If
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
            Try
                iAcquisitionFrameRateEnable.Value = False
            Catch ex As Exception
            End Try
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
        'start a thread to stop acquisition and call m_cam.EndAcquisition()
        Dim tstop As New Thread(AddressOf m_cam.EndAcquisition)
        tstop.Start()

        'SaveAstroCalibrationForCurrentCamera()

        meteorCheckRunning = False
        btnStart.Enabled = True
        btnStop.Enabled = False
    End Sub

    Private Sub frmPointGrey_Load(sender As Object, e As EventArgs) Handles Me.Load
        loadCameraList()

        Me.cmbCam.Visible = True
        Me.cbUseTrigger.Visible = False
        MyBase.Form_Load(sender, e)
        'load defaults
        tbPort.Text = "8060"
        tbPath.Text = "e:\image_pg"
        tbDayTimeExp.Text = "500"
        tbNightExp.Text = "7500000"
        tbDayGain.Text = "0"
        tbNightAgain.Text = "27"



    End Sub
    Private Sub loadCameraList()

        m_system = New ManagedSystem()
        ' Retrieve list of cameras from the system
        m_camList = m_system.GetCameras()
        Console.WriteLine("Number of cameras detected: {0}{1}{1}", m_camList.Count, NewLine)
        ' Finish if there are no cameras
        If m_camList.Count > 0 Then
            ' populate camera list cmbCam
            For Each managedCamera As IManagedCamera In m_camList
                ' get properties of camera
                ' camera may be occupied by another application
                Try
                    managedCamera.Init()
                    Me.cmbCam.Items.Add(managedCamera.DeviceModelName.ToString() & " " & managedCamera.DeviceSerialNumber.ToString())
                    managedCamera.DeInit()
                Catch ex As SpinnakerException
                    Me.cmbCam.Items.Add("occupied camera")
                    Console.WriteLine("Camera   is not available. It may be in use by another application.")
                    Continue For
                End Try


            Next
        End If
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
            'LoadAstroCalibrationForCurrentCamera()
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
    'Public Function getLastImage() As Bitmap
    '    Dim stopWatch As Stopwatch = New Stopwatch()
    '    stopWatch.Start()

    '    'While running AndAlso stopWatch.ElapsedMilliseconds < 20000

    '    'End While

    '    stopWatch.[Stop]()

    '    'Dim x As New Bitmap(b)
    '    Debug.Print("get last image")

    '    Dim x As New Bitmap(m_pics.width, m_pics.height, PixelFormat.Format24bppRgb)
    '    Dim BoundsRect = New Rectangle(0, 0, m_pics.width, m_pics.height)
    '    Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], x.PixelFormat)
    '    Dim ptr As IntPtr = bmpData.Scan0
    '    System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, m_pics.dataSize) 'copy into bitmap


    '    x.UnlockBits(bmpData)
    '    Return x

    '    'Return m_pics.Bitmap
    'End Function
    Friend Sub StoreLatestRawFrame(source As Byte(), original As UShort(), width As Integer, height As Integer)
        If source Is Nothing OrElse width <= 0 OrElse height <= 0 Then
            Return
        End If

        Dim required As Integer = width * height * 3
        If source.Length < required Then
            Return
        End If

        Dim copy(required - 1) As Byte
        Buffer.BlockCopy(source, 0, copy, 0, required)

        SyncLock m_lastFrameLock
            m_lastFrameBytes = copy
            m_lastFramePixels = original
            m_lastFrameWidth = width
            m_lastFrameHeight = height
        End SyncLock
    End Sub

    Public Function TryGetLatestRawBitmap(ByRef image As Bitmap) As Boolean
        Dim localBytes() As Byte = Nothing
        Dim width As Integer = 0
        Dim height As Integer = 0

        SyncLock m_lastFrameLock
            If m_lastFrameBytes Is Nothing OrElse m_lastFrameBytes.Length = 0 Then
                Return False
            End If

            localBytes = CType(m_lastFrameBytes.Clone(), Byte())
            width = m_lastFrameWidth
            height = m_lastFrameHeight
        End SyncLock

        Dim required As Integer = width * height * 3
        If width <= 0 OrElse height <= 0 OrElse localBytes.Length < required Then
            Return False
        End If

        Dim output As New Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
        Dim bmpData As System.Drawing.Imaging.BitmapData = output.LockBits(New Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, output.PixelFormat)
        Dim rowBytes As Integer = width * 3

        For y As Integer = 0 To height - 1
            Dim srcOffset As Integer = y * rowBytes
            Dim dstPtr As IntPtr = IntPtr.Add(bmpData.Scan0, y * bmpData.Stride)
            Marshal.Copy(localBytes, srcOffset, dstPtr, rowBytes)
        Next

        output.UnlockBits(bmpData)
        image = output
        Return True
    End Function
    Friend Function getLastImageArray(ByRef scoutImageArray As UShort()) As Boolean
        SyncLock m_lastFrameLock
            If m_lastFrameBytes Is Nothing OrElse m_lastFrameBytes.Length = 0 Then
                Return False
            End If

            scoutImageArray = CType(m_lastFramePixels.Clone(), UShort())
        End SyncLock

        Return True
    End Function

    'Public Function getLastImageArray() As Byte()
    '    'Dim stopWatch As Stopwatch = New Stopwatch()
    '    'stopWatch.Start()

    '    'While running AndAlso stopWatch.ElapsedMilliseconds < 20000

    '    'End While

    '    'stopWatch.[Stop]()

    '    'Dim x As New Bitmap(b)
    '    Debug.Print("get last image")
    '    Return m_pics.ImageBytes



    'End Function

    Private Sub btnStopWeb_Click(sender As Object, e As EventArgs) Handles btnStopWeb.Click
        btnStartWeb.Enabled = True
        btnStopWeb.Enabled = False
        'start a thread to stop web server
        Dim tstop As New Thread(AddressOf myWebServer.StopWebServer)
        tstop.Start()

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


    Private Overloads Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged


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
                cam.RegisterEventHandler(deviceEventListener)

                Console.WriteLine("Device event listener registered generally...")

            ElseIf chosenEvent = eventType.Specific Then
                ' Device event listeners registered to a specific event
                ' will only be triggered by the type of event that is
                ' registered.
                cam.RegisterEventHandler(deviceEventListener, "EventExposureEnd")

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
            cam.RegisterEventHandler(imageEventListener)

        Catch ex As SpinnakerException
            Console.WriteLine("Error: {0}", ex.Message)
            result = -1
        End Try

        Return result

    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'take ten darks
        m_cam.UnregisterEventHandler(m_imageEventListener)
        Dim numDarks As Integer = 1
        Dim numBytes As Integer = 0
        MsgBox("cover lens")
        setExposure(CDbl(tbExposureTime.Text))
        Dim rawImage As IManagedImage
        m_cam.BeginAcquisition()
        rawImage = m_cam.GetNextImage()
        numBytes = rawImage.GetBufferSize
        Dim darks(numBytes) As Integer
        Dim darkBytes(numBytes) As Byte

        darkBytes = rawImage.ManagedData
        For i = 1 To numDarks
            rawImage = m_cam.GetNextImage()
            Debug.Print("image - {0}", i)
            Dim ptr As IntPtr = rawImage.DataPtr
            Dim stride As Integer = rawImage.Stride
            Dim width As Integer = rawImage.Width

            Dim dstIndex As Integer = 0

            For y = 0 To rawImage.Height - 1
                Dim rowPtr As IntPtr = ptr + y * stride
                For x = 0 To width - 1
                    Dim idx = x * 2

                    ' BIG-ENDIAN READ (matches light)
                    Dim hi As Byte = Marshal.ReadByte(rowPtr, idx)
                    Dim lo As Byte = Marshal.ReadByte(rowPtr, idx + 1)

                    darks(dstIndex) = hi
                    darks(dstIndex + 1) = lo

                    dstIndex += 2
                Next
            Next

            rawImage.Release()
        Next
        m_cam.EndAcquisition()
        m_cam.RegisterEventHandler(m_imageEventListener)
        For i = 0 To numBytes - 1
            darkBytes(i) = CByte(darks(i) / numDarks)
        Next
        System.IO.File.WriteAllBytes("pgdark" + m_cam.DeviceSerialNumber.Value + ".raw", darkBytes)
        MsgBox("finished darks")
    End Sub
    Private Sub cmbCam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCam.SelectedIndexChanged
        'SaveAstroCalibrationForCurrentCamera()
        getCameraReady()
        loadProfile(m_cam.DeviceUserID.ToString().Replace(" ", ""))
    End Sub
    'Private Sub InitializeComponent()
    '    Me.SuspendLayout()
    '    '
    '    'frmPointGrey
    '    '
    '    Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
    '    Me.ClientSize = New System.Drawing.Size(422, 525)
    '    Me.Name = "frmPointGrey"
    '    Me.ResumeLayout(False)
    '    Me.PerformLayout()

    'End Sub

    Private Function GetCalibrationFilePathForCurrentCamera() As String
        If m_cam Is Nothing Then
            Return Nothing
        End If

        Dim model As String = m_cam.DeviceModelName.ToString()
        If String.IsNullOrWhiteSpace(model) Then
            model = "unknown_model"
        End If

        For Each c As Char In Path.GetInvalidFileNameChars()
            model = model.Replace(c, "_"c)
        Next

        model = model.Replace(" "c, "_"c)

        Dim fileName = $"astrocal_{model}.bin"
        Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
    End Function

    'Private Sub LoadAstroCalibrationForCurrentCamera()
    '    Dim filePath = GetCalibrationFilePathForCurrentCamera()
    '    If String.IsNullOrWhiteSpace(filePath) Then
    '        Return
    '    End If

    '    Try
    '        SyncLock m_astroCalibration
    '            m_astroCalibration.LoadFromFile(filePath)
    '        End SyncLock
    '        m_loadedCalibrationFile = filePath
    '    Catch ex As Exception
    '        Debug.WriteLine("LoadAstroCalibrationForCurrentCamera failed: " & ex.Message)
    '    End Try
    'End Sub

    'Private Sub SaveAstroCalibrationForCurrentCamera()
    '    Exit Sub
    '    Dim filePath = GetCalibrationFilePathForCurrentCamera()
    '    If String.IsNullOrWhiteSpace(filePath) Then
    '        Return
    '    End If

    '    Try
    '        SyncLock m_astroCalibration
    '            m_astroCalibration.SaveToFile(filePath)
    '        End SyncLock
    '        m_loadedCalibrationFile = filePath
    '    Catch ex As Exception
    '        Debug.WriteLine("SaveAstroCalibrationForCurrentCamera failed: " & ex.Message)
    '    End Try
    'End Sub

    Private Sub frmPointGrey_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        'LoadAstroCalibrationForCurrentCamera()
    End Sub

    Private Sub frmPointGrey_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'SaveAstroCalibrationForCurrentCamera()
    End Sub

End Class