Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading
Imports System.Net.Http
Imports AxFGControlLib

Public Class frmFoculs
    Dim myDetectionQueue As New Queue(Of queueEntry)
    Dim night As Boolean = False
    Private myWebServer As WebServer
    Private b8 As Bitmap
    Private bTakingPic As Boolean
    Private bProcessingPic As Boolean = False
    Private myImageCodecInfo As ImageCodecInfo
    Private myEncoder As System.Drawing.Imaging.Encoder
    Private myEncoderParameter As EncoderParameter
    Private myEncoderParameters As EncoderParameters
    Private running As Boolean = False
    Private meteorCheckRunning As Boolean = False
    Private bmp As Bitmap
    Private t As Thread
    Private Class queueEntry

        Public img As Byte()
        Public filename As String

    End Class
    Public Function getLastImage() As Bitmap

        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        While bProcessingPic AndAlso stopWatch.ElapsedMilliseconds < 20000

        End While

        stopWatch.[Stop]()
        If DateDiff(DateInterval.Minute, Now, b8.Tag) > 1 Then
            Return Nothing
        Else
            'Dim x As New Bitmap(b)
            Return b8
        End If

    End Function
    'Private Sub myFirewireCam_ImageReceivedExt(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedExtEvent) Handles AxFGControlCtrl2.ImageReceivedExt
    '    If bProcessingPic Then
    '        Exit Sub
    '    End If
    '    bProcessingPic = True
    '    'myForm.writeline("firewire image received event")
    '    Try


    '        Dim rawBytesCount As Integer
    '        Dim w As Integer
    '        Dim h As Integer

    '        w = AxFGControlCtrl2.SizeX
    '        h = AxFGControlCtrl2.SizeY
    '        b8 = New Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
    '        'b8 = New Bitmap(w, h)
    '        Dim ncp As System.Drawing.Imaging.ColorPalette = b8.Palette
    '        For j As Integer = 0 To 255
    '            ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
    '        Next
    '        b8.Palette = ncp


    '        rawBytesCount = AxFGControlCtrl2.GetPayloadSize()
    '        Dim rawData(rawBytesCount) As Byte

    '        Dim dptr As IntPtr

    '        '
    '        dptr = AxFGControlCtrl2.GetRawDataPointer()

    '        Marshal.Copy(dptr, rawData, 0, rawBytesCount)







    '        'darks
    '        'Dim fs As FileStream
    '        'If useDarks Then

    '        '    Dim multiplier
    '        '    multiplier = Val(myForm.tbMultiplier.Text)
    '        '    Dim imageValue
    '        '    Dim darkValue
    '        '    Dim newvalue
    '        '    For i = 0 To rawData.Length - 1 Step 2
    '        '        imageValue = rawData(i + 1) * 256 + rawData(i)
    '        '        darkValue = rawDark(i + 1) * 256 + rawDark(i)
    '        '        If darkValue * multiplier > imageValue Then

    '        '            rawData(i) = 0
    '        '            rawData(i + 1) = 0
    '        '        Else
    '        '            newvalue = imageValue - darkValue * multiplier
    '        '            rawData(i) = newvalue And &HFF
    '        '            rawData(i + 1) = (newvalue And &HFF00) >> 8

    '        '        End If

    '        '    Next



    '        'End If
    '        'copy raw data into bitmap

    '        Dim BoundsRect = New Rectangle(0, 0, w, h)
    '        Dim bmpData As System.Drawing.Imaging.BitmapData = b8.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], b8.PixelFormat)

    '        Dim ptr As IntPtr = bmpData.Scan0

    '        Dim bytes As Integer = bmpData.Stride * b8.Height
    '        'Dim rgbValues = New Byte(bytes - 1) {}

    '        '' fill in rgbValues, e.g. with a for loop over an input array
    '        'For i = 0 To bytes - 1
    '        '    rgbValues(i) = Rnd(255)
    '        'Next


    '        Marshal.Copy(rawData, 0, ptr, bytes)
    '        b8.UnlockBits(bmpData)
    '        b8.RotateFlip(RotateFlipType.Rotate180FlipX)
    '        b8.RotateFlip(RotateFlipType.Rotate180FlipY)
    '        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
    '        Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
    '        Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)
    '        'b = bm.Clone
    '        'Dim gr As Graphics = Graphics.FromImage(b8)
    '        'Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
    '        'Dim myBrushLabels As New SolidBrush(Color.White)

    '        'gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
    '        'gr.Dispose()
    '        'myFontLabels.Dispose()
    '        'object detection section test
    '        '
    '        'Dim t As New Threading.Thread(AddressOf checkForThings)
    '        ''t.Start()
    '        'If frames Mod 3 = 0 Then


    '        'End If

    '        Dim filename As String
    '        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
    '        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "img_foc", DateTime.Now)
    '        filename = Path.Combine(Me.tbPath.Text, folderName, filename)


    '        If Me.cbSaveImages.Checked = True Then
    '            System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


    '            b8.Save(filename, myImageCodecInfo, myEncoderParameters)


    '        End If
    '        If Me.cbxMeteor.Checked Then
    '            Dim ms As New MemoryStream()
    '            b8.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)

    '            Dim contents = ms.ToArray()
    '            CallAzureMeteorDetection(contents, filename)
    '            ms.Close()
    '        End If

    '        bTakingPic = False
    '        bProcessingPic = False

    '        'PictureBox1.Image = b
    '    Catch ex As Exception
    '        bTakingPic = False
    '        bProcessingPic = False
    '        ' bFault = True
    '        ' myForm.writeline("error on firewire image received:" & ex.Message)
    '    End Try
    'End Sub
    Public Async Function CallAzureMeteorDetection(contents As Byte(), file As String) As Task


        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.192:7071/api/detection?file=" + file

        Dim client As New HttpClient()

        Dim byteContent = New ByteArrayContent(contents)

        Dim response = client.PostAsync(apiURL, byteContent)
        Dim responseString = response.Result

    End Function

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Button1.Enabled = False
        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1
        AxFGControlCtrl2.BeginInit()
        AxFGControlCtrl2.EndInit()
        'axfgcontrolctrl2.Camera = 0


        Button1.Enabled = True

        Me.ComboBox1.DataSource = AxFGControlCtrl2.GetCameraList()

    End Sub

    Public Sub writeline(s As String)
        Console.WriteLine("Foculus: " & s)
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        AxFGControlCtrl2.Camera = 0
        '        


        '  axfgcontrolctrl2.SetGain("", 400)

        AxFGControlCtrl2.ShowPropertyPage()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        '
        Button2.Enabled = False
        Call Timer1_Tick(sender, e)


        AxFGControlCtrl2.ExposureTimeAuto = "Off"
        AxFGControlCtrl2.SetLUTKneePoint(0, 525, 1290)
        AxFGControlCtrl2.SetLUTKneePoint(1, 1290, 1980)
        AxFGControlCtrl2.SetLUTKneePoint(2, 1860, 2265)
        AxFGControlCtrl2.SetLUTKneePoint(3, 2475, 2715)


        AxFGControlCtrl2.PixelFormat = 8
        AxFGControlCtrl2.Flip = 1
        AxFGControlCtrl2.BytePerPacket = 600
        AxFGControlCtrl2.AcquisitionMode = "Continuous"
        AxFGControlCtrl2.SetExposureTimeString("75ms")
        Me.AxFGControlCtrl2.SetGain("", Val(Me.tbGain.Text))
        Me.AxFGControlCtrl2.SetExposureTimeString(tbExposureTime.Text)
        If (night) Then
            AxFGControlCtrl2.KneeLUTEnable = Me.cbKneeLut.Checked
            AxFGControlCtrl2.ExposureTimeAuto = "Off"
        Else
            AxFGControlCtrl2.KneeLUTEnable = Me.cbKneeLut.Checked
            AxFGControlCtrl2.ExposureTimeAuto = "Off"
            AxFGControlCtrl2.AcquisitionMode = "Continuous"
            AxFGControlCtrl2.AutoExposure = 20


        End If
        AxFGControlCtrl2.Acquisition = 1
        meteorCheckRunning = True
        t = New Thread(AddressOf processDetection)
        t.Start()
        Button3.Enabled = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AxFGControlCtrl2.Acquisition = 0
        meteorCheckRunning = False
        Button3.Enabled = False
        Button2.Enabled = True
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



        'axfgcontrolctrl2.AcquisitionMode = "Continuous"
        'axfgcontrolctrl2.Trigger = 1
        'axfgcontrolctrl2.TriggerMode = 0
        'axfgcontrolctrl2.TriggerSource = "Software"
        ''        axfgcontrolctrl2.TriggerActivation = "RisingEdge"
        'axfgcontrolctrl2.PixelFormat = 8
        'axfgcontrolctrl2.BytePerPacket = 1200
        'axfgcontrolctrl2.SetLUTKneePoint(0, 525, 1290)
        'axfgcontrolctrl2.SetLUTKneePoint(1, 1290, 1980)
        'axfgcontrolctrl2.SetLUTKneePoint(2, 1860, 2265)
        'axfgcontrolctrl2.SetLUTKneePoint(3, 2475, 2715)
        'axfgcontrolctrl2.SetExposureTimeString(tbNightExp.Text)
        'axfgcontrolctrl2.SetExposureTimeString("135us")
        'axfgcontrolctrl2.SetGain("", 0)
        'axfgcontrolctrl2.Acquisition = 1
        ''axfgcontrolctrl2.SetExposureTimeString("3s")
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Button4.Enabled = True
        Button6.Enabled = False

        myWebServer = WebServer.getWebServer

        initCamera()


        myWebServer.StartWebServer(Me, Val(Me.tbPort.Text))

    End Sub







    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Button4.Enabled = False
        Button6.Enabled = True

        myWebServer.StopWebServer()
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
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
                tbGain.Text = "490"
                lblDayNight.Text = "night"
                'night mode
                If Not myWebServer Is Nothing Then
                    If CheckBox1.Checked Then
                        myWebServer.useDarks = True
                    Else
                        myWebServer.useDarks = False
                    End If
                End If

            Else
                'day mode

                tbExposureTime.Text = tbDayTimeExp.Text
                tbGain.Text = "0"
                lblDayNight.Text = "day"


            End If
            'End If

        Catch ex As Exception

        End Try
    End Sub




    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click



        AxFGControlCtrl2.Acquisition = 1

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        AxFGControlCtrl2.Camera = ComboBox1.SelectedIndex
    End Sub
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
    Private Sub AxFGControlCtrl2_ImageReceived(sender As Object, e As _IFGControlEvents_ImageReceivedEvent) Handles AxFGControlCtrl2.ImageReceived
        If bProcessingPic Then
            Exit Sub
        End If
        bProcessingPic = True
        'myForm.writeline("firewire image received event")
        Try


            Dim rawBytesCount As Integer
            Dim w As Integer
            Dim h As Integer

            w = AxFGControlCtrl2.SizeX
            h = AxFGControlCtrl2.SizeY
            b8 = New Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            'b8 = New Bitmap(w, h)
            Dim ncp As System.Drawing.Imaging.ColorPalette = b8.Palette
            For j As Integer = 0 To 255
                ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
            Next
            b8.Palette = ncp


            rawBytesCount = AxFGControlCtrl2.GetPayloadSize()
            Dim rawData(rawBytesCount) As Byte

            Dim dptr As IntPtr

            '
            dptr = AxFGControlCtrl2.GetRawDataPointer()

            Marshal.Copy(dptr, rawData, 0, rawBytesCount)







            'darks
            'Dim fs As FileStream
            'If useDarks Then

            '    Dim multiplier
            '    multiplier = Val(myForm.tbMultiplier.Text)
            '    Dim imageValue
            '    Dim darkValue
            '    Dim newvalue
            '    For i = 0 To rawData.Length - 1 Step 2
            '        imageValue = rawData(i + 1) * 256 + rawData(i)
            '        darkValue = rawDark(i + 1) * 256 + rawDark(i)
            '        If darkValue * multiplier > imageValue Then

            '            rawData(i) = 0
            '            rawData(i + 1) = 0
            '        Else
            '            newvalue = imageValue - darkValue * multiplier
            '            rawData(i) = newvalue And &HFF
            '            rawData(i + 1) = (newvalue And &HFF00) >> 8

            '        End If

            '    Next



            'End If
            'copy raw data into bitmap

            Dim BoundsRect = New Rectangle(0, 0, w, h)
            Dim bmpData As System.Drawing.Imaging.BitmapData = b8.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], b8.PixelFormat)

            Dim ptr As IntPtr = bmpData.Scan0

            Dim bytes As Integer = bmpData.Stride * b8.Height
            'Dim rgbValues = New Byte(bytes - 1) {}

            '' fill in rgbValues, e.g. with a for loop over an input array
            'For i = 0 To bytes - 1
            '    rgbValues(i) = Rnd(255)
            'Next


            Marshal.Copy(rawData, 0, ptr, bytes)
            b8.UnlockBits(bmpData)
            b8.RotateFlip(RotateFlipType.Rotate180FlipX)
            b8.RotateFlip(RotateFlipType.Rotate180FlipY)
            ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)
            Dim firstLocation As PointF = New PointF(10.0F, 10.0F)
            Dim firstText As String = String.Format("{0:dd-MMM-yyyy HH:mm:ss}", DateTime.Now)
            'b = bm.Clone
            'Dim gr As Graphics = Graphics.FromImage(b8)
            'Dim myFontLabels As New Font("Arial", 16, GraphicsUnit.Pixel)
            'Dim myBrushLabels As New SolidBrush(Color.White)

            'gr.DrawString(firstText, myFontLabels, Brushes.GreenYellow, firstLocation) '# last 2 number are X and Y coords.
            'gr.Dispose()
            'myFontLabels.Dispose()
            'object detection section test
            '
            'Dim t As New Threading.Thread(AddressOf checkForThings)
            ''t.Start()
            'If frames Mod 3 = 0 Then


            'End If

            Dim filename As String
            Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
            filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "img_foc", DateTime.Now)
            filename = Path.Combine(Me.tbPath.Text, folderName, filename)


            If Me.cbSaveImages.Checked = True Then
                System.IO.Directory.CreateDirectory(Path.Combine(Me.tbPath.Text, folderName))


                b8.Save(filename, myImageCodecInfo, myEncoderParameters)


            End If
            If Me.cbxMeteor.Checked Then 'And lblDayNight.Text.ToLower = "night" Then
                ' md.examine(bm, filename)
                'call azure service
                Dim ms As New MemoryStream()
                b8.Save(ms, ImageFormat.Bmp)

                Dim contents = ms.ToArray()
                Dim qe As New queueEntry
                qe.img = contents
                qe.filename = Path.GetFileName(filename)
                If myDetectionQueue.Count < 10 Then
                    myDetectionQueue.Enqueue(qe)
                End If

                ms.Close()

            End If

            bTakingPic = False
            bProcessingPic = False

            'PictureBox1.Image = b
        Catch ex As Exception
            bTakingPic = False
            bProcessingPic = False
            ' bFault = True
            ' myForm.writeline("error on firewire image received:" & ex.Message)
        End Try
    End Sub

    Private Sub lblDayNight_TextChanged(sender As Object, e As EventArgs) Handles lblDayNight.TextChanged
        Try
            Me.AxFGControlCtrl2.SetGain("", Val(Me.tbGain.Text))
            Me.AxFGControlCtrl2.SetExposureTimeString(tbExposureTime.Text)
        Catch
        End Try


    End Sub

    'Private Sub lblDayNight_Click(sender As Object, e As EventArgs) Handles lblDayNight.Click

    'End Sub

    'Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
    '    initCamera()
    'End Sub

    'Private Sub axfgcontrolctrl2_ImageReceived(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedEvent) Handles axfgcontrolctrl2.ImageReceived
    '    axfgcontrolctrl2.Acquisition = 0


    'End Sub

    'Private Sub axfgcontrolctrl2_ImageReceivedExt(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedExtEvent) Handles axfgcontrolctrl2.ImageReceivedExt
    '    Dim rawBytesCount

    '    rawBytesCount = axfgcontrolctrl2.GetPayloadSize()
    '    '  Dim rawData(rawBytesCount) As Byte



    '    MsgBox(axfgcontrolctrl2.GetReceivedFrameCount)
    'End Sub

    'Private Sub axfgcontrolctrl2_DeviceEventCallback(sender As Object, e As AxFGControlLib._IFGControlEvents_DeviceEventCallbackEvent) Handles axfgcontrolctrl2.DeviceEventCallback
    '    MsgBox("deviceEventCallback")
    'End Sub

    'Private Sub axfgcontrolctrl2_JobCompleted(sender As Object, e As AxFGControlLib._IFGControlEvents_JobCompletedEvent) Handles axfgcontrolctrl2.JobCompleted
    '    MsgBox("job completed")
    'End Sub


End Class