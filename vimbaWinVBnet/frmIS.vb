
Imports System.IO

Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Threading

Imports TIS.Imaging
Public Class frmIS
    Inherits frmMaster

    Private VCDProp As TIS.Imaging.VCDHelpers.VCDSimpleProperty
    'Private WithEvents IcImagingControl1 As New TIS.Imaging.ICImagingControl
    'Private mhCamera As IntPtr
    'Private mDisplayPanel As myPanel
    'Private mIsMono As Boolean
    'Private mDisplayBitmap As Bitmap


    'Private checkBox1 As CheckBox
    'Private tbExposure As TrackBar
    'Private lblExposureVal As Label
    'Private panel1 As Panel
    'Private gbAcquisition As GroupBox
    'Private gbExposure As GroupBox
    'Private gbGain As GroupBox
    'Private lblGainVal As Label

    'Private gbInfo As GroupBox
    'Private lblCameraModel As Label
    'Private bmp As Bitmap
    'Private imageBytes As Byte()
    'Private bmp2 As Bitmap
    'Private lblSerNum As Label

    'Private t As Thread
    'Private t_imaging As Thread
    'Private h_camera As Integer




    Private m_camRunning As Boolean = False
    Private m_grabbing As Boolean = False
    'Private rawImage As Byte()

    Dim fh As FrameHandlerSink
    'Private m_grabbedframe As Boolean
    'Private m_grabbedframe_err As Integer = 0

    Private m_syncLock As Object = New Object   '// Sync object protecting the following member data
    Public Shared iWidth As Integer
    Friend WithEvents IcImagingControl1 As ICImagingControl

    Public Shared iHeight As Integer


    '//
    '// Callback function for Api.GetClip
    '// Called by a thread other than that which called Api.GetClip, therefore we syncronize access to the member data modified
    '//


    Private Function ToUInt16(ByVal s As Int16) As UInt16
        If (s And &H8000) = 0 Then
            Return CType(s, UInt16)
        Else
            Return CType(UInt16.MaxValue + 1 + CType(s, Int32), UInt16)
        End If
    End Function

    Private Function ReadY16(ByVal buf As TIS.Imaging.ImageBuffer, ByVal row As Integer, ByVal col As Integer) As UInt16
        ' Y16 is top-down, the first line has index 0
        Dim offset As Integer = row * buf.BytesPerLine + col * 2

        Dim val As Int16 = System.Runtime.InteropServices.Marshal.ReadInt16(buf.GetIntPtr(), offset)

        Return ToUInt16(val)
    End Function
    Private Function ToInt16(ByVal us As UInt16) As Int16
        If (us And &H8000) = 0 Then
            Return CType(us, Int16)
        Else
            Return CType(CType(us, Int32) - UInt16.MaxValue - 1, Int16)
        End If
    End Function

    Private Sub WriteY16(ByVal buf As TIS.Imaging.ImageBuffer, ByVal row As Integer, ByVal col As Integer, ByVal value As UInt16)

        Dim offset As Integer = row * buf.BytesPerLine + col * 2

        System.Runtime.InteropServices.Marshal.WriteInt16(buf.GetIntPtr(), offset, ToInt16(value))
    End Sub

    Public Sub loadMasterDark()
        Dim filename As String
        Dim bReader As BinaryReader
        Dim multiplier As Decimal


        Try

            If IcImagingControl1.Device = "" Then Exit Sub

            multiplier = Val(tbMultiplier.Text)
            filename = String.Format("masterDark{0}.raw", IcImagingControl1.Device)
            Dim fStream As New FileStream(filename, FileMode.Open)
            bReader = New BinaryReader(fStream)
            ReDim dark(fStream.Length / 2)
            For i = 0 To fStream.Length / 2 - 1
                dark(i) = bReader.ReadUInt16()
                dark(i) = dark(i) * multiplier
            Next
            fStream.Close()

        Catch ex As Exception
            MsgBox("problem reading dark file")
        End Try




    End Sub









    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        m_camRunning = False
        ' rc = Api.SetStreamState(h_camera, StreamState.Stop)
        'TimerAcquistionRate.Enabled = False
        't.Abort()
        IcImagingControl1.LiveStop()
        meteorCheckRunning = False
        btnStart.Enabled = True
        btnStop.Enabled = False
    End Sub

    Private Sub frmIS_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.cmbCam.Visible = False
        Me.cbUseTrigger.Visible = False
        initIS()

        'setup camera
        getCameraReady()

        tbPort.Text = "8199"
        tbPath.Text = "e:\image_scout"
        tbDayTimeExp.Text = "500"
        tbNightExp.Text = "5000000"
        tbDayGain.Text = "0"
        tbNightAgain.Text = "32"
        MyBase.Form_Load(sender, e)
        loadProfile("IS")
        myEncoderParameter = New EncoderParameter(myEncoder, CType(95L, Int32)) 'overriding to bring image size down a little
        myEncoderParameters.Param(0) = myEncoderParameter
    End Sub


    Private Sub getCameraReady()
        'try to open camera
        initIS()

        If Not IcImagingControl1.DeviceValid() Then
            IcImagingControl1.ShowDeviceSettingsDialog()

            If Not IcImagingControl1.DeviceValid Then
                MsgBox("No device was selected.", MsgBoxStyle.Information, "Grabbing an Image")

                Me.Close()
            End If
        End If

        ' IcImagingControl1.DeviceFrameFilters.Clear()
        'IcImagingControl1.LoadDeviceStateFromFile("device.dat", False)
        'IcImagingControl1.VideoFormat = "RGB32 (3072x2048)"
        IcImagingControl1.DeviceFrameRate = 0.2
        IcImagingControl1.DeviceFlipHorizontal = False
        IcImagingControl1.Update()


        IcImagingControl1.Sink = New FrameQueueSink(AddressOf gotBuffer, MediaSubtypes.Y16, 5)
        ' VCDProp = TIS.Imaging.VCDHelpers.VCDSimpleModule.GetSimplePropertyContainer(IcImagingControl1.VCDPropertyItems)
    End Sub
    Private Function gotBuffer(arg As IFrameQueueBuffer) As FrameQueuedResult
        'image is here...

        Debug.Print("image arrived")
        If m_pics Is Nothing Then
            m_pics = New RingBitmap(5)
        End If
        Dim img() As Byte
        Dim iBuffer As ImageBuffer
        Dim pixelValue As UShort
        Dim darkBuffer() As UShort



        'TIS.Imaging.FrameSnapSink snapsink = IcImagingControl1.Sink as TIS.Imaging.FrameSnapSink;

        'Dim snapsink As TIS.Imaging.FrameHandlerSink
        'snapsink = IcImagingControl1.Sink
        'Dim frm As TIS.Imaging.IFrameQueueBuffer
        'snapsink.SnapImage()
        iWidth = arg.FrameType.Width
        iHeight = arg.FrameType.Height

        ReDim img(arg.ActualDataSize)
        'IcImagingControl1.VideoFormat = "Y16 (3072x2048)"

        'copy image buffer into byte array
        Marshal.Copy(arg.GetIntPtr, img, 0, arg.ActualDataSize)



        File.WriteAllBytes("ISraw.raw", img)

        'IcImagingControl1.LiveSuspend()
        'IcImagingControl1.VideoFormat = "RGB24 (3072x2048)"
        Dim newPixelValue As UInt16
        Dim dPixelValue As UInt16
        Dim neigborPixelValue As UInt16
        Dim multiplier

        multiplier = Val(tbMultiplier.Text)
        'subtract dark here while in 16bit gray mode
        If cbUseDarks.Checked And lblDayNight.Text = "night" Then
            If dark Is Nothing Then
                loadMasterDark()
            End If
            'go through image buffer
            For x = 0 To iWidth - 1
                For y = 0 To iHeight - 1
                    pixelValue = ReadY16(iBuffer, y, x)
                    If x = iWidth - 1 Then
                        neigborPixelValue = ReadY16(iBuffer, y, x - 1)
                    Else
                        neigborPixelValue = ReadY16(iBuffer, y, x + 2)
                    End If

                    dPixelValue = dark(y * iBuffer.PixelPerLine + x)
                    ' If dPixelValue / multiplier Then
                    If dPixelValue > 12000 Then
                        newPixelValue = neigborPixelValue
                    Else
                        newPixelValue = pixelValue
                    End If
                    ' newPixelValue = Math.Max(0, CInt(pixelValue) - CInt(dPixelValue)) Then
                    WriteY16(iBuffer, y, x, newPixelValue)
                    ' End If


                Next
            Next
            Debug.WriteLine("subtracted dark")
        End If

        'Dim ffi As TIS.Imaging.FrameFilterInfo = CType(lstFrameFilters.SelectedItem, TIS.Imaging.FrameFilterInfo)
        'Dim newFrameFilter As TIS.Imaging.FrameFilter = IcImagingControl1.FrameFilterInfos


        'debayer to rgb24
        Dim mTransformImage As BGAPI2.Image = Nothing
        Dim mImage As BGAPI2.Image = Nothing
        ' Dim buff As BGAPI2.Buffer = New BGAPI2.Buffer()
        Dim imgProcessor As New BGAPI2.ImageProcessor()

        If (imgProcessor.NodeList.GetNodePresent("DemosaicingMethod")) Then

            imgProcessor.NodeList("DemosaicingMethod").Value = "Baumer5x5"


        End If

        Dim outImage(iWidth * iHeight * 3) As Byte
        mImage = imgProcessor.CreateImage(iWidth, iHeight, "BayerBG16", arg.GetIntPtr, arg.ActualDataSize)
        'mImage = imgProcessor.CreateImage(iWidth, iHeight, "BGR8", e.ImageBuffer.GetImageDataPtr, e.ImageBuffer.ActualDataSize)


        mTransformImage = imgProcessor.CreateTransformedImage(mImage, "BayerBG8")






        mTransformImage = imgProcessor.CreateTransformedImage(mTransformImage, "RGB8")



        Marshal.Copy(mTransformImage.Buffer, outImage, 0, iWidth * iHeight * 3 - 1)

        ' Array.Reverse(outImage)

        ' Marshal.Copy(e.ImageBuffer.GetImageDataPtr, img, 0, e.ImageBuffer.ActualDataSize)
        'Dim reverseImage(iWidth * iHeight * 3) As Byte
        'Dim p1, p2, p3 As Byte
        'Dim rIndex As Integer
        'For i = 0 To iWidth * iHeight * 3 - 1 Step 3
        '    p1 = outImage(i)
        '    p2 = outImage(i + 1)
        '    p3 = outImage(i + 2)
        '    rIndex = (iWidth * iHeight * 3 - 3) - i
        '    reverseImage(rIndex) = p1
        '    reverseImage(rIndex + 1) = p2
        '    reverseImage(rIndex + 2) = p3

        'Next



        m_pics.FillNextBitmap(outImage)

        Dim filename As String

        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgIS_", DateTime.Now)
        filename = Path.Combine(tbPath.Text, folderName, filename)



        If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            ' convertedImage.ConvertToWriteAbleBitmap()
            Dim b As Bitmap
            b = getLastImage()

            b.Save(ms, myImageCodecInfo, myEncoderParameters)
            b.Dispose()

            Dim contents = ms.ToArray()
            Dim qe As New queueEntry
            qe.img = contents
            qe.filename = Path.GetFileName(filename)
            qe.dateTaken = Now
            qe.cameraID = "Imaging Source"
            qe.width = arg.FrameType.Width
            qe.height = arg.FrameType.Height
            If myDetectionQueue.Count < 10 Then
                myDetectionQueue.Enqueue(qe)

            End If

            ms.Close()

        End If
        If cbSaveImages.Checked = True And lblDayNight.Text = "night" Then
            System.IO.Directory.CreateDirectory(Path.Combine(tbPath.Text, folderName))
            Dim x As Bitmap
            x = getLastImage()

            x.Save(filename, myImageCodecInfo, myEncoderParameters)
            x.Dispose()
            Try
                Dim dtCreated As DateTime
                Dim dtToday As DateTime = Today.Date
                Dim diObj As DirectoryInfo
                Dim ts As TimeSpan
                Dim lstDirsToDelete As New List(Of String)

                For Each sSubDir As String In Directory.GetDirectories(Me.tbPath.Text)
                    diObj = New DirectoryInfo(sSubDir)
                    dtCreated = diObj.CreationTime

                    ts = dtToday - dtCreated

                    'Add whatever storing you want here for all folders...

                    If ts.Days >= 1 Then
                        lstDirsToDelete.Add(sSubDir)
                        'Store whatever values you want here... like how old the folder is
                        diObj.Delete(True) 'True for recursive deleting
                    End If
                Next
            Catch ex As Exception
                'MessageBox.Show(ex.Message, "Error Deleting Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If



    End Function




    Private Sub btnStartWeb_Click(sender As Object, e As EventArgs) Handles btnStartWeb.Click
        btnStartWeb.Enabled = False
        btnStopWeb.Enabled = True
        myWebServer = WebServer.getWebServer

        myWebServer.StartWebServer(Me, Val(Me.tbPort.Text))
        myWebServer.ImageDirectory = "c:\web\images\"
        myWebServer.VirtualRoot = "c:\web\"
    End Sub

    Public Overloads Function getLastImage() As Bitmap
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While m_grabbing AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()
        Dim x As New Bitmap(iWidth, iHeight, Imaging.PixelFormat.Format24bppRgb)
        Dim BoundsRect = New Rectangle(0, 0, iWidth, iHeight)
        Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], x.PixelFormat)
        Dim ptr As IntPtr = bmpData.Scan0
        System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, iWidth * iHeight * 3 - 1) 'copy into bitmap


        x.UnlockBits(bmpData)
        Return x



    End Function
    Public Function getLastImageArray() As Byte()
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While m_grabbing AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()
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
            'End If
            ' Dim err As QCamM_Err
            'QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbNightAgain.Text)))
            ' QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, tbExposureTime.Text)
            'err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        Catch ex As Exception

        End Try
    End Sub



    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged

        If m_camRunning Then


            Dim AbsValItf As TIS.Imaging.VCDAbsoluteValueProperty



            ' Retrieve an absolute value interface for exposure
            AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" +
                                                                        TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                     TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)

            AbsValItf.Value = String.Format("{0,16:0.000000e+00}", tbExposureTime.Text / 1000)
            ' AbsValItf.Value = "3.33000003593042492866516113281e-04"



            ' Retrieve an absolute value interface for gain
            AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Gain + ":" +
                                                                        TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                     TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
            AbsValItf.Value = tbGain.Text


            AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Saturation + ":" +
                                                                        TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                     TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
            AbsValItf.Value = 125
            IcImagingControl1.DeviceFrameRate = 0.2
            IcImagingControl1.ImageRingBufferSize = 1

            IcImagingControl1.Update()
            'Dim fh As FrameHandlerSink
        End If
        'fh = IcImagingControl1.Sink

        'fh.SnapMode = True
        'If Me.tbExposureTime.Text < 3000 Then

        ' IcImagingControl1.MemorySnapImage()
        'Else
        'IcImagingControl1.LiveStart()

    End Sub

    Private Sub frmIS_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim i As Object

        If hdialog <> 0 Then
            PCO_CloseDialogCam(hdialog)
        End If

        PCO_SetRecordingState(hdriver, 0)
        PCO_FreeBuffer(hdriver, nBuf) 'essential call, otherwise you'll get a memory leak
        errorCode = PCO_CloseCamera(hdriver)
        hdriver = 0
        i = errorCode



    End Sub



    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        'use settings
        'IcImagingControl1.ImageRingBufferSize = 1
        m_camRunning = True


        'VCDProp.RangeValue(VCDIDs.VCDID_Exposure) = -1
        Dim AbsValItf As TIS.Imaging.VCDAbsoluteValueProperty


        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)

        AbsValItf.Value = String.Format("{0,16:0.000000e+00}", tbExposureTime.Text / 1000)
        ' AbsValItf.Value = "3.33000003593042492866516113281e-04"



        ' Retrieve an absolute value interface for gain
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Gain + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = tbGain.Text


        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Saturation + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = 125
        IcImagingControl1.DeviceFrameRate = 0.2
        ' IcImagingControl1.ImageRingBufferSize = 1

        IcImagingControl1.Update()
        'Dim fh As FrameHandlerSink

        'fh = IcImagingControl1.Sink

        'fh.SnapMode = True
        'If Me.tbExposureTime.Text < 3000 Then

        ' IcImagingControl1.MemorySnapImage()
        'Else
        IcImagingControl1.LiveStart()

        'End If

        ' IcImagingControl1.

        btnStart.Enabled = False
        btnStop.Enabled = True

        startTime = Now
        meteorCheckRunning = True
        Timer2.Enabled = True
        t = New Thread(AddressOf processDetection)
        t.Start()
    End Sub



    Private Sub IcImagingControl1_ImageAvailable(sender As Object, e As ICImagingControl.ImageAvailableEventArgs) Handles IcImagingControl1.ImageAvailable
        'image is here...

        Debug.Print("image arrived {0}", e.frameNumber)
        If m_pics Is Nothing Then
            m_pics = New RingBitmap(5)
        End If
        Dim img() As Byte
        Dim iBuffer As ImageBuffer
        Dim pixelValue As UShort
        Dim darkBuffer() As UShort

        iBuffer = IcImagingControl1.ImageActiveBuffer

        'TIS.Imaging.FrameSnapSink snapsink = IcImagingControl1.Sink as TIS.Imaging.FrameSnapSink;

        Dim snapsink As TIS.Imaging.FrameHandlerSink
        snapsink = IcImagingControl1.Sink
        Dim frm As TIS.Imaging.IFrameQueueBuffer
        'snapsink.SnapImage()
        iWidth = e.ImageBuffer.Size.Width
        iHeight = e.ImageBuffer.Size.Height

        ReDim img(e.ImageBuffer.ActualDataSize)
        'IcImagingControl1.VideoFormat = "Y16 (3072x2048)"

        'copy image buffer into byte array
        Marshal.Copy(e.ImageBuffer.GetImageDataPtr, img, 0, e.ImageBuffer.ActualDataSize)



        File.WriteAllBytes("ISraw.raw", img)

        'IcImagingControl1.LiveSuspend()
        'IcImagingControl1.VideoFormat = "RGB24 (3072x2048)"
        Dim newPixelValue As UInt16
        Dim dPixelValue As UInt16
        Dim neigborPixelValue As UInt16
        Dim multiplier

        multiplier = Val(tbMultiplier.Text)
        'subtract dark here while in 16bit gray mode
        If cbUseDarks.Checked And lblDayNight.Text = "night" Then
            If dark Is Nothing Then
                loadMasterDark()
            End If
            'go through image buffer
            For x = 0 To iWidth - 1
                For y = 0 To iHeight - 1
                    pixelValue = ReadY16(iBuffer, y, x)
                    If x = iWidth - 1 Then
                        neigborPixelValue = ReadY16(iBuffer, y, x - 1)
                    Else
                        neigborPixelValue = ReadY16(iBuffer, y, x + 2)
                    End If

                    dPixelValue = dark(y * iBuffer.PixelPerLine + x)
                    ' If dPixelValue / multiplier Then
                    If dPixelValue > 12000 Then
                        newPixelValue = neigborPixelValue
                    Else
                        newPixelValue = pixelValue
                    End If
                    ' newPixelValue = Math.Max(0, CInt(pixelValue) - CInt(dPixelValue)) Then
                    WriteY16(iBuffer, y, x, newPixelValue)
                    ' End If


                Next
            Next
            Debug.WriteLine("subtracted dark")
        End If

        'Dim ffi As TIS.Imaging.FrameFilterInfo = CType(lstFrameFilters.SelectedItem, TIS.Imaging.FrameFilterInfo)
        'Dim newFrameFilter As TIS.Imaging.FrameFilter = IcImagingControl1.FrameFilterInfos


        'debayer to rgb24
        Dim mTransformImage As BGAPI2.Image = Nothing
        Dim mImage As BGAPI2.Image = Nothing
        ' Dim buff As BGAPI2.Buffer = New BGAPI2.Buffer()
        Dim imgProcessor As New BGAPI2.ImageProcessor()

        If (imgProcessor.NodeList.GetNodePresent("DemosaicingMethod")) Then

            imgProcessor.NodeList("DemosaicingMethod").Value = "Baumer5x5"


        End If

        Dim outImage(iWidth * iHeight * 3) As Byte
        mImage = imgProcessor.CreateImage(iWidth, iHeight, "BayerBG16", e.ImageBuffer.GetImageDataPtr, e.ImageBuffer.ActualDataSize)
        'mImage = imgProcessor.CreateImage(iWidth, iHeight, "BGR8", e.ImageBuffer.GetImageDataPtr, e.ImageBuffer.ActualDataSize)


        mTransformImage = imgProcessor.CreateTransformedImage(mImage, "BayerBG8")






        mTransformImage = imgProcessor.CreateTransformedImage(mTransformImage, "RGB8")



        Marshal.Copy(mImage.Buffer, outImage, 0, iWidth * iHeight * 3 - 1)

        ' Array.Reverse(outImage)

        ' Marshal.Copy(e.ImageBuffer.GetImageDataPtr, img, 0, e.ImageBuffer.ActualDataSize)
        Dim reverseImage(iWidth * iHeight * 3) As Byte
        Dim p1, p2, p3 As Byte
        Dim rIndex As Integer
        For i = 0 To iWidth * iHeight * 3 - 1 Step 3
            p1 = outImage(i)
            p2 = outImage(i + 1)
            p3 = outImage(i + 2)
            rIndex = (iWidth * iHeight * 3 - 3) - i
            reverseImage(rIndex) = p1
            reverseImage(rIndex + 1) = p2
            reverseImage(rIndex + 2) = p3

        Next



        m_pics.FillNextBitmap(reverseImage)

        Dim filename As String

        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgIS_", DateTime.Now)
        filename = Path.Combine(tbPath.Text, folderName, filename)



        If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            ' convertedImage.ConvertToWriteAbleBitmap()
            Dim b As Bitmap
            b = getLastImage()

            b.Save(ms, myImageCodecInfo, myEncoderParameters)
            b.Dispose()

            Dim contents = ms.ToArray()
            Dim qe As New queueEntry
            qe.img = contents
            qe.filename = Path.GetFileName(filename)
            qe.dateTaken = Now
            qe.cameraID = "Imaging Source"
            qe.width = e.ImageBuffer.Size.Width
            qe.height = e.ImageBuffer.Size.Height
            If myDetectionQueue.Count < 10 Then
                myDetectionQueue.Enqueue(qe)

            End If

            ms.Close()

        End If
        If cbSaveImages.Checked = True And lblDayNight.Text = "night" Then
            System.IO.Directory.CreateDirectory(Path.Combine(tbPath.Text, folderName))
            Dim x As Bitmap
            x = getLastImage()

            x.Save(filename, myImageCodecInfo, myEncoderParameters)
            x.Dispose()
            Try
                Dim dtCreated As DateTime
                Dim dtToday As DateTime = Today.Date
                Dim diObj As DirectoryInfo
                Dim ts As TimeSpan
                Dim lstDirsToDelete As New List(Of String)

                For Each sSubDir As String In Directory.GetDirectories(Me.tbPath.Text)
                    diObj = New DirectoryInfo(sSubDir)
                    dtCreated = diObj.CreationTime

                    ts = dtToday - dtCreated

                    'Add whatever storing you want here for all folders...

                    If ts.Days >= 1 Then
                        lstDirsToDelete.Add(sSubDir)
                        'Store whatever values you want here... like how old the folder is
                        diObj.Delete(True) 'True for recursive deleting
                    End If
                Next
            Catch ex As Exception
                'MessageBox.Show(ex.Message, "Error Deleting Folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If



    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'make 10 darks and average them
        MsgBox("cover lens")
        'setup camera for darks
        Dim AbsValItf As TIS.Imaging.VCDAbsoluteValueProperty

        Dim buffers() As ImageBuffer
        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = tbExposureTime.Text / 1000

        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Gain + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = tbGain.Text


        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Saturation + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = 125
        Dim fh As FrameHandlerSink

        fh = IcImagingControl1.Sink

        fh.SnapMode = True
        Dim numDarks As Integer = 10
        ReDim buffers(numDarks)
        Dim b As Integer
        Dim dBuffer() As UInt16
        Dim pixelValue As UInt32

        For x = 0 To numDarks - 1
            'snap image
            IcImagingControl1.MemorySnapImage()
            buffers(x) = IcImagingControl1.ImageActiveBuffer
        Next
        ReDim dBuffer(IcImagingControl1.ImageActiveBuffer.ActualDataSize)
        fh.SnapMode = False
        For x = 0 To buffers(0).Size.Width - 1
            For y = 0 To buffers(0).Size.Height - 1
                pixelValue = 0
                For b = 0 To numDarks - 1
                    pixelValue = pixelValue + ReadY16(buffers(0), y, x)
                Next
                pixelValue = pixelValue / numDarks
                dBuffer(y * buffers(0).PixelPerLine + x) = pixelValue
            Next
        Next
        Dim filename As String



        filename = String.Format("masterDark{0}.raw", IcImagingControl1.Device)
        Dim fStream As New FileStream(filename, FileMode.Create)
        Dim bWriter As New BinaryWriter(fStream)
        For i = 0 To dBuffer.Length - 1
            bWriter.Write(dBuffer(i))
        Next
        fStream.Close()
        bWriter.Close()
        MsgBox("done")
    End Sub

    Private Sub tbMultiplier_TextChanged(sender As Object, e As EventArgs)
        '      loadMasterDark()
    End Sub

    Private Sub initIS()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIS))
        Me.IcImagingControl1 = New TIS.Imaging.ICImagingControl()
        'CType(Me.IcImagingControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        ' Me.SuspendLayout()
        '
        'IcImagingControl1
        ''
        'Me.IcImagingControl1.BackColor = System.Drawing.Color.White
        'Me.IcImagingControl1.DeviceListChangedExecutionMode = TIS.Imaging.EventExecutionMode.Invoke
        'Me.IcImagingControl1.DeviceLostExecutionMode = TIS.Imaging.EventExecutionMode.AsyncInvoke
        Me.IcImagingControl1.DeviceState = "<device_state libver=""3.5"" filemajor=""1"" fileminor=""0"">" & vbLf & "    <device name=""DBK 33UX178"" base_name=""DBK 33UX178"" unique_name=""DBK 33UX178 48710122"">" & vbLf & "        <videoformat>Y16 (3072x2048)</videoformat>" & vbLf & "        <fps>1.</fps>" & vbLf & "        <fliph>0</fliph>" & vbLf & "        <vcdpropertyitems>" & vbLf & "            <item guid=""{8E972004-3B94-4AA4-A899-65F6A1432713}"" name=""Startup Behavior"" />" & vbLf & "            <item guid=""{284C0E06-010B-45BF-8291-09D90A459B28}"" name=""Brightness"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""240"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{284C0E08-010B-45BF-8291-09D90A459B28}"" name=""Hue"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{284C0E09-010B-45BF-8291-09D90A459B28}"" name=""Saturation"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E08}"" value=""125."" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{284C0E0A-010B-45BF-8291-09D90A459B28}"" name=""Sharpness"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{284C0E0B-010B-45BF-8291-09D90A459B28}"" name=""Gamma"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E08}"" value=""0.979999999999999982236431605997"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{284C0E0D-010B-45BF-8291-09D90A459B28}"" name=""WhiteBalance"">" & vbLf & "                <element guid=""{B57D3001-0AC6-4819-A609-272A33140ACA}"" name=""Auto"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{8407E480-175A-498C-8171-08BD987CC1AC}"" name=""White Balance Green"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""94"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{6519038A-1AD8-4E91-9021-66D64090CC85}"" name=""White Balance Blue"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""140"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{6519038B-1AD8-4E91-9021-66D64090CC85}"" name=""White Balance Red"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""115"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{AB98F78D-18A6-4EB2-A556-C11010EC9DF7}"" name=""WhiteBalance Mode"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E06}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{284C0E0F-010B-45BF-8291-09D90A459B28}"" name=""Gain"">" & vbLf & "                <element guid=""{B57D3001-0AC6-4819-A609-272A33140ACA}"" name=""Auto"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E08}"" value=""32."" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{90D5702E-E43B-4366-AAEB-7A7A10B448B4}"" name=""Exposure"">" & vbLf & "                <element guid=""{B57D3001-0AC6-4819-A609-272A33140ACA}"" name=""Auto"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E08}"" value=""5."" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{65190390-1AD8-4E91-9021-66D64090CC85}"" name=""Auto"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{6519038C-1AD8-4E91-9021-66D64090CC85}"" name=""Auto Reference"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""128"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{90D57031-E43B-4366-AAEB-7A7A10B448B4}"" name=""Trigger"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enable"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{C3C9944A-E6F6-4E25-A0BE-53C066AB65D8}"" name=""Denoise"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{E33B9C58-0BF8-442D-8035-B4ABD7AF44AA}"" name=""Flip Horizontal"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enable"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{86D89D69-9880-4618-9BF6-DED5E8383449}"" name=""GPIO"">" & vbLf & "                <element guid=""{7D006621-761D-4B88-9C5F-8B906857A501}"" name=""GP Out"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E03}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{4F95A06D-9C15-407B-96AB-CF3FED047BA4}"" name=""Binning factor"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Value"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E06}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{A3E64DA6-736A-489B-8425-04BD2B4AC57A}"" name=""IMX178 LLP Gain"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enabled"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{546541AD-C815-4D82-AFA9-9D59AF9F399E}"" name=""Highlight Reduction"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enable"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{3D505AC4-1A28-428B-83E5-85AA8EB441C1}"" name=""Tone Mapping"">" & vbLf & "                <element guid=""{B57D3001-0AC6-4819-A609-272A33140ACA}"" name=""Auto"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enable"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{BD2F432A-02C1-4F32-9AEB-687CA117D2E7}"" name=""Intensity"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E08}"" value=""0.5"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{D1451FED-C2D8-42CE-910B-2CB566836A77}"" name=""Global Brightness Factor"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E08}"" value=""0."" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{2CED6FD6-AB4D-4C74-904C-D682E53B9CC5}"" name=""Partial scan"">" & vbLf & "                <element guid=""{36EAA683-3321-44BE-9D73-E1FD4C3FDB87}"" name=""Auto-center"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""1"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{DC320EDE-DF2E-4A90-B926-71417C71C57C}"" name=""Strobe"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enable"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{7F7E24E3-7162-42EF-BF5D-99A359CB32F2}"" name=""Color Matrix"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enabled"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "            <item guid=""{124922E5-81C7-4587-867D-7BA16AF79079}"" name=""Auto Functions ROI"">" & vbLf & "                <element guid=""{B57D3000-0AC6-4819-A609-272A33140ACA}"" name=""Enabled"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E04}"" value=""0"" />" & vbLf & "                </element>" & vbLf & "                <element guid=""{93D840ED-B7B8-45FE-91E2-18E68C41AFC3}"" name=""Preset"">" & vbLf & "                    <itf guid=""{99B44940-BFE1-4083-ADA1-BE703F4B8E06}"" value=""2"" />" & vbLf & "                </element>" & vbLf & "            </item>" & vbLf & "        </vcdpropertyitems>" & vbLf & "    </device>" & vbLf & "</device_state>" & vbLf
        'Me.IcImagingControl1.ImageAvailableExecutionMode = TIS.Imaging.EventExecutionMode.MultiThreaded
        'Me.IcImagingControl1.LiveDisplayPosition = New System.Drawing.Point(0, 0)
        'Me.IcImagingControl1.Location = New System.Drawing.Point(10, 148)
        'Me.IcImagingControl1.Name = "IcImagingControl1"
        'Me.IcImagingControl1.Size = New System.Drawing.Size(150, 150)
        'Me.IcImagingControl1.TabIndex = 158
        'Me.IcImagingControl1.Update()

        '
        'frmIS
        '
        'Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        'Me.ClientSize = New System.Drawing.Size(422, 525)
        'Me.Controls.Add(Me.IcImagingControl1)
        'Me.Name = "frmIS"
        ' Me.Controls.SetChildIndex(Me.IcImagingControl1, 0)
        'CType(Me.IcImagingControl1, System.ComponentModel.ISupportInitialize).EndInit()
        'Me.ResumeLayout(False)
        'Me.PerformLayout()

    End Sub


End Class

