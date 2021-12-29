Imports System.IO
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Threading
Imports System.Net.Http
Imports System.Collections.Specialized
Imports vimbaWinVBnet.vimbaWinVBnet
Imports SpinnakerNET
Imports SpinnakerNET.GenApi
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports Newtonsoft.Json
Imports System.Web.Script.Serialization

Public Class frmMaster
    Public night As Boolean = False
    Public myWebServer As WebServer
    Public myDetectionQueue As New Queue(Of queueEntry)
    Public killing As Boolean = False
    Public b As Bitmap
    Public running As Boolean
    Public frames As Integer
    Public startTime As DateTime
    Public gotFrameTime As DateTime
    Public dark() As Byte
    Public meteorCheckRunning As Boolean = False
    Public myImageCodecInfo As ImageCodecInfo
    Public myEncoder As System.Drawing.Imaging.Encoder
    Public myEncoderParameter As EncoderParameter
    Public myEncoderParameters As EncoderParameters
    Public camThread As Thread
    Public t As Thread
    Public lost As Integer = 0
    Public t_cleanup As Thread
    Public mySettings As CameraSettings
    Public Shared m_pics As RingBitmap

    Public Class CameraSettings

        Public Property ModelName As String
        Public Property port As Integer
        Public Property DayExposure As Integer
        Public Property NightExposure As Integer

        Public Property DayGain As Integer
        Public Property NightGain As Integer
        Public Property ImagePath As String
        Public Property morningHour As Integer
        Public Property eveningHour As Integer
        Public Property useDarks As Boolean
        Public Property detectMeteors As Boolean
        Public Property saveImages As Boolean
        Public Property minValue As Integer
        Public Property maxValue As Integer
        Public Property darkMultiplier As String
        Public Property darkCutOff As Integer
        Public Property url As String
        Public Property Rects As New List(Of MyRectangle)

        Public Sub readSettings()

            'try to read settings file
            Dim filename As String = "profile_" & Me.ModelName & ".json"
            Dim settingsJSON As String
            Dim r As MyRectangle
            Try

                settingsJSON = File.ReadAllText(filename)
                Dim jsonResulttodict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(settingsJSON)
                Dim jss As New JavaScriptSerializer()

                Me.ModelName = jsonResulttodict.Item("ModelName")
                Me.ImagePath = jsonResulttodict.Item("ImagePath")
                Me.DayExposure = jsonResulttodict.Item("DayExposure")
                Me.NightExposure = jsonResulttodict.Item("NightExposure")
                Me.DayGain = jsonResulttodict.Item("DayGain")
                Me.NightGain = jsonResulttodict.Item("NightGain")
                Me.port = jsonResulttodict.Item("port")
                Me.detectMeteors = jsonResulttodict.Item("detectMeteors")
                Me.morningHour = jsonResulttodict.Item("morningHour")
                Me.eveningHour = jsonResulttodict.Item("eveningHour")
                Me.useDarks = jsonResulttodict.Item("useDarks")
                Me.saveImages = jsonResulttodict.Item("saveImages")
                Me.maxValue = jsonResulttodict.Item("maxValue")
                Me.minValue = jsonResulttodict.Item("minValue")
                Me.darkMultiplier = jsonResulttodict.Item("darkMultiplier")
                Me.darkCutOff = jsonResulttodict.Item("darkCutOff")
                Me.url = jsonResulttodict.Item("url")
                Dim rectJS As Object = jsonResulttodict.Item("Rects")
                Me.Rects = New List(Of MyRectangle)
                For Each item In rectJS
                    r = New MyRectangle()
                    r.x = item("_x").value
                    r.y = item("_y").value
                    r.width = item("_width").value
                    r.height = item("_height").value
                    Rects.Add(r)
                Next


            Catch ex As Exception

            End Try

        End Sub

        Public Sub writeSettings()
            '
            Dim settings = New DataContractJsonSerializerSettings()
            Dim ser As New DataContractJsonSerializer(Me.GetType())
            Dim ms As New MemoryStream
            Dim json As String

            ser.WriteObject(ms, Me)

            ms.Position = 0

            json = Encoding.Default.GetString(ms.ToArray())

            File.WriteAllText("profile_" & Me.ModelName & ".json", json)

        End Sub

    End Class

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
        Public Sub FillNextBitmap(b As SpinnakerNET.IManagedImage)
            SwitchBitmap()

            ' m_ManagedImages(m_BitmapSelector) = b
            'copy raw data into m_buffers
            Dim rawData(b.DataSize) As Byte
            ' Dim BoundsRect = New Rectangle(0, 0, b.Width, b.Height)
            ' Dim bmpData As System.Drawing.Imaging.BitmapData = m_Bitmaps(m_BitmapSelector).LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], m_Bitmaps(m_BitmapSelector).PixelFormat)
            'Dim ptr As IntPtr = bmpData.Scan0
            'System.Runtime.InteropServices.Marshal.Copy(b.DataPtr, ptr, 0, b.DataSize) 'copy into bitmap
            'System.Runtime.InteropServices.Marshal.Copy(b.ManagedData, 0, rawData, b.DataSize) 'copy into array

            m_buffers(m_BitmapSelector) = b.ManagedData
            m_width = b.Width
            m_height = b.Height
            m_dataSize = b.DataSize


        End Sub
        Public Sub FillNextBitmap(b As Bitmap)
            SwitchBitmap()

            ' m_ManagedImages(m_BitmapSelector) = b
            'copy raw data into m_buffers

            Dim BoundsRect = New Rectangle(0, 0, b.Width, b.Height)
            Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], b.PixelFormat)
            Dim rawData(b.Height * bmpData.Stride) As Byte
            Dim ptr As IntPtr = bmpData.Scan0
            'System.Runtime.InteropServices.Marshal.Copy(b.DataPtr, ptr, 0, b.DataSize) 'copy into bitmap
            System.Runtime.InteropServices.Marshal.Copy(ptr, rawData, 0, rawData.Length - 1) 'copy into array

            m_buffers(m_BitmapSelector) = rawData
            m_width = b.Width
            m_height = b.Height
            m_dataSize = rawData.Length


        End Sub

        Private Sub SwitchBitmap()
            m_BitmapSelector += 1

            If m_Size = m_BitmapSelector Then
                m_BitmapSelector = 0
            End If
        End Sub
    End Class




    Public Function getLastImage() As Bitmap
        Dim stopWatch As Stopwatch = New Stopwatch()
        stopWatch.Start()

        stopWatch.[Stop]()


        Debug.Print("get last image")

        Dim x As New Bitmap(m_pics.width, m_pics.height, PixelFormat.Format24bppRgb)
        Dim BoundsRect = New Rectangle(0, 0, m_pics.width, m_pics.height)
        Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], x.PixelFormat)
        Dim ptr As IntPtr = bmpData.Scan0
        System.Runtime.InteropServices.Marshal.Copy(m_pics.ImageBytes, 0, ptr, m_pics.dataSize - 1) 'copy into bitmap


        x.UnlockBits(bmpData)
        Return x


    End Function
    Public Sub Form_Load(sender As Object, e As EventArgs) Handles Me.Load



        t_cleanup = New Thread(AddressOf cleanFolders)



        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1

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

        mySettings = New CameraSettings()
        mySettings.readSettings()



    End Sub


    Public Sub loadProfile(camModel)

        mySettings.ModelName = camModel
        mySettings.readSettings()
        tbPort.Text = mySettings.port
        tbPath.Text = mySettings.ImagePath
        tbDayTimeExp.Text = mySettings.DayExposure
        tbNightExp.Text = mySettings.NightExposure
        tbDayGain.Text = mySettings.DayGain
        tbNightAgain.Text = mySettings.NightGain
        cbMeteors.Checked = mySettings.detectMeteors
        cbSaveImages.Checked = mySettings.saveImages
        cboDay.SelectedItem = CStr(mySettings.morningHour)
        cboNight.SelectedItem = CStr(mySettings.eveningHour)
        cbUseDarks.Checked = mySettings.useDarks
        tbMultiplier.Text = mySettings.darkMultiplier
        tbLower.Text = mySettings.minValue
        tbUpper.Text = mySettings.maxValue
        tbDarkCutOff.Text = mySettings.darkCutOff
        tbURL.Text = mySettings.url


    End Sub

    Public Sub writeline(s As String)
        Console.WriteLine("camera: " & s)
    End Sub












    Protected Overrides Sub Finalize()

        MyBase.Finalize()
    End Sub



    Private Sub frmSVSVistek_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        On Error Resume Next

        'md = Nothing
        If Not myWebServer Is Nothing Then
            myWebServer.StopWebServer()
        End If

        myWebServer = Nothing
        meteorCheckRunning = False
    End Sub




    Public Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
        Dim j As Integer
        Dim encoders() As ImageCodecInfo
        encoders = ImageCodecInfo.GetImageEncoders()

        j = 0
        While j <encoders.Length
            If encoders(j).MimeType = mimeType Then
                Return encoders(j)
            End If
            j += 1
        End While
        Return Nothing

    End Function 'GetEncoderInfo




    Public Sub processDetection()
        Dim aQE As queueEntry
        While (meteorCheckRunning)
            If myDetectionQueue.Count > 0 Then
                aQE = myDetectionQueue.Dequeue()

                CallAzureMeteorDetection(aQE)


                aQE = Nothing

            End If
            'Console.WriteLine("in the queue:{0}", myDetectionQueue.Count)
            Thread.Sleep(200)
        End While

    End Sub
    Public Async Function CallAzureMeteorDetection(qe As queueEntry) As Task


        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.199:7071/api/detection"
        Dim myUriBuilder As New UriBuilder(apiURL)


        Dim query As NameValueCollection = Web.HttpUtility.ParseQueryString(String.Empty)

        query("file") = qe.filename
        query("dateTaken") = qe.dateTaken.ToString("MM/dd/yyyy hh:mm tt")
        query("cameraID") = qe.cameraID
        query("width") = qe.width
        query("height") = qe.height

        If mySettings.Rects.Count > 0 Then
            'add rectangles
            query("rectangles") = mySettings.Rects.Count
            For i = 0 To mySettings.Rects.Count - 1
                query("r_" + Trim(Str(i)) + "_x") = mySettings.Rects(i).x
                query("r_" + Trim(Str(i)) + "_y") = mySettings.Rects(i).y
                query("r_" + Trim(Str(i)) + "_w") = mySettings.Rects(i).width
                query("r_" + Trim(Str(i)) + "_h") = mySettings.Rects(i).height
            Next
        End If

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


    Public Sub cleanFolders()
        'delete all files 24 hrs older underneath path
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
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        mySettings.NightGain = tbNightAgain.Text
        mySettings.DayGain = tbDayGain.Text
        mySettings.port = tbPort.Text
        mySettings.ImagePath = tbPath.Text
        mySettings.DayExposure = tbDayTimeExp.Text
        mySettings.NightExposure = tbNightExp.Text
        mySettings.morningHour = cboDay.SelectedItem
        mySettings.eveningHour = cboNight.SelectedItem
        mySettings.useDarks = cbUseDarks.Checked
        mySettings.saveImages = cbSaveImages.Checked
        mySettings.detectMeteors = cbMeteors.Checked
        mySettings.maxValue = tbUpper.Text
        mySettings.minValue = tbLower.Text
        mySettings.darkMultiplier = tbMultiplier.Text
        mySettings.darkCutOff = tbDarkCutOff.Text
        mySettings.url = tbURL.Text

        mySettings.writeSettings()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f As New frmExclude
        Dim result

        f.Rects = mySettings.Rects
        result = f.ShowDialog()
        If result = DialogResult.OK Then
            mySettings.Rects = f.Rects
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        'test detection

    End Sub
End Class