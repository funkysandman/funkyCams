Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Xml
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports AVT.VmbAPINET


Public Class WebServer
#Region "Declarations"
    Private singleWebserver As WebServer
    Private blnFlag As Boolean
    Private bTakingPic As Boolean
    Private bFault As Boolean
    Private bProcessingPic As Boolean = False
    Private LocalTCPListener As TcpListener
    Private LocalPort As Integer = 80
    Private LocalAddress As IPAddress = GetIPAddress()
    Private localUseDarks As String = True
    Private DefaultDoc As String = "index.html"
    Private WebThread As Thread
    Private LocalImageDir As String
    Private LocalVirtualRoot As String
    Private imageInUse As Integer = 0
    '  Private WithEvents myGigECam As CCamera
    Private WithEvents myGigECam As AVT.VmbAPINET.Camera
    Private myQICam As QCamManagedDriver.QCam
    '' Private myBaslerCam As BaslerWrapper.Grabber
    Private mySVSVistekCam As SVCamApi.SVCamGrabber

    Private myForm As frmAVT
    Private myFWform As frmFoculs
    Private myQIform As frmQ
    Private restart As Boolean = False
    '  Private myBaslerForm As frmBasler
    Private mySVSVistekForm As frmSVSVistek
    Private mySVSVistekBaumerForm As frmGIGE

    Dim rawDark() As Byte
#End Region

#Region "Properties"
    Public Property ListenWebPort() As Integer
        Get
            Return LocalPort
        End Get
        Set(ByVal Value As Integer)
            LocalPort = Value
        End Set
    End Property
    Public ReadOnly Property ImageCount() As Integer
        Get
            Return imageInUse
        End Get
    End Property

    Public ReadOnly Property ListenIPAddress() As IPAddress
        Get
            Return LocalAddress
        End Get
    End Property


    Public Property DefaultDocument() As String
        Get
            Return DefaultDoc
        End Get
        Set(ByVal Value As String)
            DefaultDoc = Value
        End Set
    End Property

    Public Property ImageDirectory() As String
        Get
            Return LocalImageDir
        End Get
        Set(ByVal Value As String)
            LocalImageDir = Value
        End Set
    End Property

    Public Property useDarks() As Boolean
        Get
            Return localUseDarks
        End Get
        Set(ByVal Value As Boolean)
            localUseDarks = Value
        End Set
    End Property

    Public Property VirtualRoot() As String
        Get
            Return LocalVirtualRoot
        End Get
        Set(ByVal Value As String)
            LocalVirtualRoot = Value
        End Set
    End Property
#End Region

#Region "Methods"

    Private Function GetIPAddress() As IPAddress
        Dim oAddr As System.Net.IPAddress = Nothing

        With System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
            If .AddressList.Length > 0 Then
                oAddr = New IPAddress(.AddressList.GetLowerBound(0))
            End If
        End With
        GetIPAddress = oAddr
    End Function


    Friend Shared Function getWebServer() As WebServer
        ' If Not blnFlag Then
        '  singleWebserver = New WebServer
        ' blnFlag = True
        Return New WebServer
        'Return singleWebserver
        'End If
    End Function
    'Public Sub StartWebServer(aCam As BaslerWrapper.Grabber, f As frmBasler, port As Integer)
    '    Try
    '        LocalPort = port
    '        myBaslerForm = f
    '        myBaslerCam = aCam
    '        'loadGigEDarks()
    '        LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
    '        LocalTCPListener.Start()
    '        WebThread = New Thread(AddressOf StartListenBasler)
    '        WebThread.Start()
    '        f.writeline("starting Basler web server")
    '    Catch ex As Exception
    '        f.WriteLine(ex.Message)
    '    End Try
    'End Sub
    Public Sub StartWebServer(f As frmGIGE, port As Integer)
        Try
            LocalPort = port
            mySVSVistekBaumerForm = f

            'loadGigEDarks()
            LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
            LocalTCPListener.Start()
            WebThread = New Thread(AddressOf StartListenSVSVistekBaumer)
            WebThread.Start()
            f.writeline("starting SVS Vistek web server")
        Catch ex As Exception
            f.writeline(ex.Message)
        End Try
    End Sub
    Public Sub StartWebServer(f As frmQ, port As Integer)
        Try
            LocalPort = port
            myQIform = f


            LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
            LocalTCPListener.Start()
            WebThread = New Thread(AddressOf StartListenQIFirewire)
            WebThread.Start()

        Catch ex As Exception

        End Try
    End Sub
    Public Sub StartWebServer(aCam As SVCamApi.SVCamGrabber, f As frmSVSVistek, port As Integer)
        Try
            LocalPort = port
            mySVSVistekForm = f
            mySVSVistekCam = aCam
            'loadGigEDarks()
            LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
            LocalTCPListener.Start()
            WebThread = New Thread(AddressOf StartListenSVSVistek)
            WebThread.Start()
            f.writeline("starting SVS Vistek web server")
        Catch ex As Exception
            f.writeline(ex.Message)
        End Try
    End Sub
    'Public Sub StartWebServer(aCam As CCamera, f As frmAVT, port As Integer)
    '    Try
    '        LocalPort = port
    '        myForm = f
    '        myGigECam = aCam
    '        loadGigEDarks()
    '        LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
    '        LocalTCPListener.Start()
    '        WebThread = New Thread(AddressOf StartListenGigE)
    '        WebThread.Start()
    '        f.writeline("starting Allied Vision web server")
    '    Catch ex As Exception
    '        f.writeline(ex.Message)
    '    End Try
    'End Sub
    Public Sub StartWebServer(aCam As AVT.VmbAPINET.Camera, f As frmAVT, port As Integer)
        Try
            LocalPort = port
            myForm = f
            myGigECam = aCam
            loadGigEDarks()
            LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
            LocalTCPListener.Start()
            WebThread = New Thread(AddressOf StartListenGigE)
            WebThread.Start()
            f.writeline("starting Allied Vision web server")
        Catch ex As Exception
            f.writeline(ex.Message)
        End Try
    End Sub
    Public Sub StartWebServer(aCam As QCamManagedDriver.QCam, f As frmQ, port As Integer)
        Try
            LocalPort = port
            myQIform = f
            myQICam = aCam
            loadGigEDarks()
            LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
            LocalTCPListener.Start()
            WebThread = New Thread(AddressOf StartListenGigE)
            WebThread.Start()

        Catch ex As Exception

        End Try
    End Sub
    Public Sub StartWebServer(f As frmFoculs, port As Integer)
        Try
            ' md.LoadModel("c:\tmp\frozen_inference_graph_orig.pb", "c:\tmp\mscoco_label_map.pbtxt")
            LocalPort = port
            myFWform = f
            'myFirewireCam = aCam
            useDarks = False
            ' loadFirewireEDarks()
            LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
            LocalTCPListener.Start()
            WebThread = New Thread(AddressOf StartListenFirewire)
            WebThread.Start()
            f.writeline("starting Foculus web server")
            'myImageCodecInfo = GetEncoderInfo("image/jpeg")

            '' Create an Encoder object based on the GUID
            '' for the Quality parameter category.
            'myEncoder = System.Drawing.Imaging.Encoder.Quality

            '' Create an EncoderParameters object.
            '' An EncoderParameters object has an array of EncoderParameter
            '' objects. In this case, there is only one
            '' EncoderParameter object in the array.
            'myEncoderParameters = New EncoderParameters(1)

            '' Save the bitmap as a JPEG file with quality level 25.
            'myEncoderParameter = New EncoderParameter(myEncoder, CType(85L, Int32))
            'myEncoderParameters.Param(0) = myEncoderParameter
        Catch ex As Exception
            f.writeline(ex.Message)
        End Try
    End Sub

    'Here is where we check our XML file and see what MIME types are defined and handle the accordingly.
    Private Sub loadGigEDarks()
        Dim fs As FileStream
        fs = New FileStream(Application.StartupPath & "\darkGigE.drk", FileMode.Open)

        '
        rawDark = New Byte(2457656) {}
        fs.Read(rawDark, 0, rawDark.Length - 1)
        fs.Close()
    End Sub
    Private Sub loadFirewireEDarks()
        Dim fs As FileStream
        Try
            fs = New FileStream(Application.StartupPath & "\darkFireWire.drk", FileMode.Open)

            '
            rawDark = New Byte(2457656) {}
            fs.Read(rawDark, 0, rawDark.Length - 1)
            fs.Close()
        Catch ex As Exception
        End Try

    End Sub
    Public Function GetMimeType(ByVal sRequestFile As String) As String

        Dim sLine As String = ""
        Dim sMimeType As String = ""
        Dim sFileExt As String = ""
        Dim sMimeExt As String = ""
        sRequestFile = sRequestFile.ToLower
        Dim iStartPos As Integer = sRequestFile.IndexOf(".") + 1
        sFileExt = sRequestFile.Substring(iStartPos)
        'now go through the mime definitions and apply to the request.
        Dim dom As New XmlDocument
        dom.Load(Application.StartupPath & "\Settings.xml")
        Dim objCurrentNode As XmlNode
        objCurrentNode = dom.SelectSingleNode("//mimetypes")
        'now go through all child nodes.
        If objCurrentNode.HasChildNodes Then
            'loop
            Dim xmlMimeType As XmlNode
            For Each xmlMimeType In objCurrentNode
                sMimeExt = xmlMimeType.Name
                sMimeType = xmlMimeType.InnerText
                If (sMimeExt = sFileExt) Then
                    Exit For
                End If
            Next
        End If
        If sMimeExt = sFileExt Then
            Return sMimeType
        Else
            Return ""
        End If
    End Function

    Public Function GetTheDefaultFileName(ByVal sLocalDirectory As String) As String
        Return "index.html"
    End Function

    Public Function GetLocalPath(ByVal sWebServerRoot As String, ByVal sDirName As String) As String
        'Dim sr As StreamReader
        'Dim sLine As String = ""
        Dim sVirtualDir As String = ""
        Dim sRealDir As String = ""
        Dim iStartPos As Integer = 0
        sDirName.Trim()
        sWebServerRoot = sWebServerRoot.ToLower
        sDirName = sDirName.ToLower
        Select Case sDirName
            Case "/"
                sRealDir = LocalVirtualRoot
            Case Else
                If Mid$(sDirName, 1, 1) = "/" Then
                    sDirName = Mid$(sDirName, 2, Len(sDirName))
                End If
                sRealDir = LocalVirtualRoot & sDirName.Replace("/", "\")
        End Select
        Return sRealDir
    End Function

    Public Sub SendHeader(ByVal sHttpVersion As String, ByVal sMimeHeader As String,
              ByVal iTotalBytes As Integer, ByVal sStatusCode As String, ByRef thisSocket As Socket)
        Dim sBuffer As String = ""
        If Len(sMimeHeader) = 0 Then
            sMimeHeader = "text/html"
        End If
        sBuffer = sHttpVersion & sStatusCode & vbCrLf &
            "Server: X10CamControl" & vbCrLf &
            "Content-Type: " & sMimeHeader & vbCrLf &
            "Accept-Ranges: bytes" & vbCrLf &
            "Content-Length: " & iTotalBytes & vbCrLf & vbCrLf

        Dim bSendData As [Byte]() = Encoding.ASCII.GetBytes(sBuffer)
        SendToBrowser(bSendData, thisSocket)
    End Sub

    Public Overloads Sub SendToBrowser(ByVal sData As String, ByRef thisSocket As Socket)
        SendToBrowser(Encoding.ASCII.GetBytes(sData), thisSocket)
    End Sub

    Public Overloads Sub SendToBrowser(ByVal bSendData As [Byte](), ByRef thisSocket As Socket)
        Dim iNumBytes As Integer = 0
        If thisSocket.Connected Then
            Try


                If (iNumBytes = thisSocket.Send(bSendData, bSendData.Length, 0)) = -1 Then
                    'socket error can't send packet
                    Debug.Print("socket error")
                Else
                    'If (iNumBytes = 0) Then
                    '    thisSocket.Close()
                    '    restart = True

                    'Else


                    '    'number of bytes sent.
                    '    Debug.Print("sent " & iNumBytes & " bytes ")
                    'End If
                End If
            Catch ex As Exception
                'connection problem?
                Debug.Print("sendToBrowser: " & ex.Message)
                thisSocket.Close()
                restart = True
            End Try
        Else
            'connection dropped.
        End If
    End Sub

    Private Sub New()
        'create a singleton



    End Sub

    Private Sub StartListenGigE()
        Dim iStartPos As Integer

        Dim sErrorMessage As String

        Dim sWebserverRoot = LocalVirtualRoot

        Dim sPhysicalFilePath As String = ""
        Dim sFormattedMessage As String = ""


        Do While True
            'accept new socket connection
            LocalTCPListener.Start()
            myForm.writeline("starting listener")
            Dim mySocket As Socket = LocalTCPListener.AcceptSocket
            If mySocket.Connected Then
                Dim bReceive() As Byte = New [Byte](1024) {}
                Dim i As Integer = mySocket.Receive(bReceive, bReceive.Length, 0)
                Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
                'find the GET request.
                myForm.writeline("connected")
                If sBuffer.Contains("GET") And sBuffer.Contains("HTTP") Then


                    iStartPos = sBuffer.IndexOf("HTTP", 1)
                    Dim sHttpVersion = sBuffer.Substring(iStartPos, 8)
                    'sRequest = sBuffer.Substring(0, iStartPos - 1)
                    'sRequest.Replace("\\", "/")
                    'If (sRequest.IndexOf(".") < 1) And (Not (sRequest.EndsWith("/"))) Then
                    '    sRequest = sRequest & "/"
                    'End If
                    ''get the file name
                    'iStartPos = sRequest.LastIndexOf("/") + 1
                    'sRequestedFile = sRequest.Substring(iStartPos)
                    'If InStr(sRequest, "?") <> 0 Then
                    '    iStartPos = sRequest.IndexOf("?") + 1
                    '    sQueryString = sRequest.Substring(iStartPos)
                    '    sRequestedFile = Replace(sRequestedFile, "?" & sQueryString, "")
                    'End If
                    ''get the directory
                    'sDirName = sRequest.Substring(sRequest.IndexOf("/"), sRequest.LastIndexOf("/") - 3)
                    ''identify the physical directory.
                    'If (sDirName = "/") Then
                    '    sLocalDir = sWebserverRoot
                    'Else
                    '    sLocalDir = GetLocalPath(sWebserverRoot, sDirName)
                    'End If
                    ''if the directory isn't there then display error.
                    'If sLocalDir.Length = 0 Then
                    '    sErrorMessage = "Error!! Requested Directory does not exists"
                    '    SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                    '    SendToBrowser(sErrorMessage, mySocket)
                    '    mySocket.Close()
                    'End If

                    'If sRequestedFile.Length = 0 Then
                    '    sRequestedFile = GetTheDefaultFileName(sLocalDir)
                    '    If sRequestedFile = "" Then
                    '        sErrorMessage = "Error!! No Default File Name Specified"
                    '        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                    '        SendToBrowser(sErrorMessage, mySocket)
                    '        mySocket.Close()
                    '        Return
                    '    End If
                    'End If

                    'Dim sMimeType As String = GetMimeType(sRequestedFile)
                    'sPhysicalFilePath = sLocalDir & sRequestedFile
                    'While Not File.Exists(sPhysicalFilePath) And InStr(sPhysicalFilePath, "image.jpg") > 0 'wait for file to show up
                    'End While



                    'If Not File.Exists(sPhysicalFilePath) Then
                    '    sErrorMessage = "404 Error! File Does Not Exists..."
                    '    SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                    '    SendToBrowser(sErrorMessage, mySocket)
                    'Else

                    Try
                        'grab image from cam
                        Dim f As Frame
                        Dim b As Bitmap
                        'make sure camera still connected


                        myForm.writeline("taking pic")
                        b = myForm.getLastImage
                        myForm.writeline("acquired")



                        'darks

                        'If useDarks Then
                        '    'd2 = Bitmap.FromFile(Application.StartupPath & "\dark.png")


                        '    '' 'Freeze the image in memory
                        '    '' raw = b.LockBits(New Rectangle(0, 0,
                        '    '' b.Width, b.Height),
                        '    '' System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        '    ''b.PixelFormat)
                        '    '' Dim size As Integer = raw.Height * raw.Stride

                        '    '' Dim rawImage() As Byte = New Byte(size - 1) {}
                        '    '' 'Copy the image into the byte()
                        '    '' System.Runtime.InteropServices.Marshal.Copy(raw.Scan0, rawImage, 0, size)



                        '    '' Dim raw2 As System.Drawing.Imaging.BitmapData = Nothing


                        '    '' 'Freeze the image in memory
                        '    '' raw2 = d2.LockBits(New Rectangle(0, 0,
                        '    '' d2.Width, d2.Height),
                        '    '' System.Drawing.Imaging.ImageLockMode.ReadOnly,
                        '    ''d2.PixelFormat)
                        '    '' size = raw2.Height * raw2.Stride

                        '    '' Dim rawImage2() As Byte = New Byte(size - 1) {}
                        '    '' 'Copy the image into the byte()
                        '    '' System.Runtime.InteropServices.Marshal.Copy(raw2.Scan0, rawImage2, 0, size)

                        '    '' If Not raw2 Is Nothing Then
                        '    ''     'Unfreeze the memory for the image
                        '    ''     d2.UnlockBits(raw2)
                        '    '' End If
                        '    Dim multiplier
                        '    multiplier = Val(myForm.tbMultiplier.Text)
                        '    Dim imageValue
                        '    Dim darkValue
                        '    Dim newvalue
                        '    For i = 0 To f.BufferSize - 1 Step 2
                        '        imageValue = f.Buffer(i + 1) * 256 + f.Buffer(i)
                        '        darkValue = rawDark(i + 1) * 256 + rawDark(i)
                        '        If darkValue * multiplier > imageValue Then

                        '            f.Buffer(i) = 0
                        '            f.Buffer(i + 1) = 0
                        '        Else
                        '            newvalue = imageValue - darkValue * multiplier
                        '            f.Buffer(i) = newvalue And &HFF
                        '            f.Buffer(i + 1) = (newvalue And &HFF00) >> 8

                        '        End If

                        '    Next

                        '    ''System.Runtime.InteropServices.Marshal.Copy(rawImage, 0, raw.Scan0, size)

                        '    ' '' Unlock the bits.
                        '    ' '' bmp.UnlockBits(bmpData)

                        '    ''If Not raw Is Nothing Then
                        '    ''    'Unfreeze the memory for the image
                        '    ''    b.UnlockBits(raw)
                        '    ''End If
                        '    b = New Bitmap(f.Width, f.Height, PixelFormat.Format24bppRgb)
                        '    f.Fill(b)

                        'Else
                        '    b = New Bitmap(f.Width, f.Height, PixelFormat.Format24bppRgb)
                        '    f.Fill(b)

                        'End If

                        'imageInUse = imageInUse + 1
                        Dim iTotBytes As Integer = 0
                        Dim sResponse As String = ""
                        'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                        '
                        Dim myImageCodecInfo As ImageCodecInfo
                        Dim myEncoder As System.Drawing.Imaging.Encoder
                        Dim myEncoderParameter As EncoderParameter
                        Dim myEncoderParameters As EncoderParameters

                        ' Create a Bitmap object based on a BMP file.


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
                        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)


                        '
                        Dim ms As New MemoryStream()
                        '  Dim ms2 As New MemoryStream()
                        b.Save(ms, myImageCodecInfo, myEncoderParameters)
                        ' d2.Save(ms2, Imaging.ImageFormat.Bmp)
                        ' myForm.PictureBox1.Image = b

                        SendHeader(sHttpVersion, "image/jpeg", ms.Length, " 200 OK", mySocket)
                        SendToBrowser(ms.ToArray(), mySocket)
                        ms.Close()

                    Catch ex As Exception
                        myForm.writeline(ex.Message)
                        imageInUse = imageInUse - 1
                        sErrorMessage = "404 Error! File Does Not Exists..."
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                        SendToBrowser(sErrorMessage, mySocket)
                    End Try
                End If

                ' End If
                mySocket.Close()
                mySocket = Nothing
                LocalTCPListener.Stop()
                myForm.writeline("socket disconnected")
            End If
        Loop

    End Sub
    'Private Sub StartListenBasler()
    '    Dim iStartPos As Integer

    '    Dim sErrorMessage As String

    '    Dim sWebserverRoot = LocalVirtualRoot

    '    Dim sPhysicalFilePath As String = ""
    '    Dim sFormattedMessage As String = ""


    '    Do While True
    '        'accept new socket connection
    '        LocalTCPListener.Start()
    '        myBaslerForm.writeline("starting Basler GigE listener")
    '        Dim mySocket As Socket = LocalTCPListener.AcceptSocket
    '        If mySocket.Connected Then
    '            Dim bReceive() As Byte = New [Byte](1024) {}
    '            Dim i As Integer = mySocket.Receive(bReceive, bReceive.Length, 0)
    '            Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
    '            'find the GET request.
    '            myBaslerForm.writeline("Basler image server connected")
    '            If sBuffer.Contains("GET") And sBuffer.Contains("HTTP") Then


    '                iStartPos = sBuffer.IndexOf("HTTP", 1)
    '                Dim sHttpVersion = sBuffer.Substring(iStartPos, 8)


    '                Try
    '                    'grab image from cam

    '                    Dim b As Bitmap

    '                    Dim myWidth As Integer = 1392
    '                    Dim myHeight As Integer


    '                    myWidth = myBaslerCam.getSizeX
    '                    myHeight = myBaslerCam.getSizeY
    '                    myBaslerForm.writeline(myWidth)

    '                    myBaslerForm.writeline("get Basler camera image")
    '                    'we know this camera has the following params:
    '                    '
    '                    'Dim bytes() As Byte = New Byte(myBaslerCam.getSizeX() * myBaslerCam.getSizeY()) {}

    '                    b = myBaslerCam.getImage(myBaslerForm.tbExposureTime.Text, Val(myBaslerForm.tbGain.Text)) '30 second timeout

    '                    myBaslerForm.writeline("acquired Basler camera image")



    '                    Dim BoundsRect = New Rectangle(0, 0, myWidth, myHeight)
    '                    Dim bmpDataSrc As BitmapData = b.LockBits(BoundsRect, ImageLockMode.[ReadOnly], b.PixelFormat)
    '                    Dim bytes As Integer = bmpDataSrc.Stride * b.Height
    '                    Dim ptr As IntPtr = bmpDataSrc.Scan0

    '                    Dim rawData = New Byte(bytes - 1) {}
    '                    'copy source pic to byte array

    '                    Marshal.Copy(ptr, rawData, 0, bytes)

    '                    Dim b2 = New Bitmap(myWidth, myHeight, PixelFormat.Format8bppIndexed)
    '                    Dim bmpData As BitmapData = b2.LockBits(BoundsRect, ImageLockMode.[WriteOnly], b2.PixelFormat)

    '                    'b contains original
    '                    'b2 is to be the copy
    '                    'Dim ncp As ColorPalette = b2.Palette

    '                    'For i = 0 To 255

    '                    '    ncp.Entries(i) = Color.FromArgb(255, i, i, i)
    '                    'Next
    '                    b2.Palette = b.Palette
    '                    Dim ptr2 As IntPtr = bmpData.Scan0
    '                    Marshal.Copy(rawData, 0, ptr2, bytes)
    '                    'from, to
    '                    b2.UnlockBits(bmpData)

    '                    b.UnlockBits(bmpDataSrc)

    '                    '=======================================================
    '                    'Service provided by Telerik (www.telerik.com)
    '                    'Conversion powered by NRefactory.
    '                    'Twitter: @telerik
    '                    'Facebook: facebook.com/telerik
    '                    '=======================================================





    '                    'myBaslerForm.PictureBox1.Image = b2


    '                    Dim iTotBytes As Integer = 0
    '                    Dim sResponse As String = ""
    '                    'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
    '                    '
    '                    Dim myImageCodecInfo As ImageCodecInfo
    '                    Dim myEncoder As System.Drawing.Imaging.Encoder
    '                    Dim myEncoderParameter As EncoderParameter
    '                    Dim myEncoderParameters As EncoderParameters

    '                    ' Create a Bitmap object based on a BMP file.


    '                    ' Get an ImageCodecInfo object that represents the JPEG codec.
    '                    myImageCodecInfo = GetEncoderInfo("image/jpeg")

    '                    ' Create an Encoder object based on the GUID
    '                    ' for the Quality parameter category.
    '                    myEncoder = System.Drawing.Imaging.Encoder.Quality

    '                    ' Create an EncoderParameters object.
    '                    ' An EncoderParameters object has an array of EncoderParameter
    '                    ' objects. In this case, there is only one
    '                    ' EncoderParameter object in the array.
    '                    myEncoderParameters = New EncoderParameters(1)

    '                    ' Save the bitmap as a JPEG file with quality level 25.
    '                    myEncoderParameter = New EncoderParameter(myEncoder, CType(95L, Int32))
    '                    myEncoderParameters.Param(0) = myEncoderParameter
    '                    ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)


    '                    '
    '                    Dim ms As New MemoryStream()
    '                    '  Dim ms2 As New MemoryStream()
    '                    b2.Save(ms, myImageCodecInfo, myEncoderParameters)
    '                    ' d2.Save(ms2, Imaging.ImageFormat.Bmp)
    '                    myBaslerForm.PictureBox1.Image = b2
    '                    Dim reader As New BinaryReader(ms)
    '                    '  Dim reader2 As New BinaryReader(ms2)
    '                    Dim bytes2() As Byte = New Byte(ms.Length) {}
    '                    '  Dim bytesDarks() As Byte = New Byte(ms2.Length) {}


    '                    reader.BaseStream.Position = 0
    '                    ' reader2.BaseStream.Position = 0



    '                    While reader.BaseStream.Position < reader.BaseStream.Length
    '                        reader.Read(bytes2, 0, bytes2.Length)

    '                    End While
    '                    ' While reader2.BaseStream.Position < reader2.BaseStream.Length
    '                    '     reader2.Read(bytesDarks, 0, bytesDarks.Length)

    '                    ' End While
    '                    ' Dim aVal As Integer


    '                    sResponse = sResponse & Encoding.ASCII.GetString(bytes2, 0, reader.BaseStream.Length)
    '                    iTotBytes = reader.BaseStream.Length
    '                    reader.Close()
    '                    ms.Close()

    '                    SendHeader(sHttpVersion, "image/jpeg", iTotBytes, " 200 OK", mySocket)
    '                    SendToBrowser(bytes2, mySocket)

    '                Catch ex As Exception
    '                    imageInUse = imageInUse - 1
    '                    sErrorMessage = "404 Error! File Does Not Exists..."
    '                    SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
    '                    SendToBrowser(sErrorMessage, mySocket)
    '                End Try
    '            End If

    '            ' End If
    '            mySocket.Close()
    '            mySocket = Nothing
    '            LocalTCPListener.Stop()

    '        End If
    '    Loop

    'End Sub


    Private Sub StartListenFirewire()
        Dim iStartPos As Integer

        Dim sErrorMessage As String

        Dim sWebserverRoot = LocalVirtualRoot

        Dim sPhysicalFilePath As String = ""
        Dim sFormattedMessage As String = ""

        'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
        'Else
        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

        'End If
        'If Not mySVSVistekCam.isStreaming Then
        '    mySVSVistekCam.startStreamingFF()
        'End If


        Do While True
            'accept new socket connection
            LocalTCPListener.Start()
            'mySVSVistekForm.writeline("starting SVS Vistek listener")
            Dim mySocket As Socket = LocalTCPListener.AcceptSocket
            If mySocket.Connected Then
                Dim bReceive() As Byte = New [Byte](1024) {}
                Dim i As Integer = mySocket.Receive(bReceive, bReceive.Length, 0)
                Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
                'find the GET request.
                ' mySVSVistekForm.writeline("SVS Vistek image server connected")
                If sBuffer.Contains("GET") And sBuffer.Contains("HTTP") Then


                    iStartPos = sBuffer.IndexOf("HTTP", 1)
                    Dim sHttpVersion = sBuffer.Substring(iStartPos, 8)


                    Try
                        'grab image from cam

                        Dim b As Bitmap

                        Dim myWidth As Integer = 1392
                        Dim myHeight As Integer




                        'myWidth = mySVSVistekCam.getSizeX
                        'myHeight = mySVSVistekCam.getSizeY

                        'mySVSVistekForm.writeline("request for SVS Vistek image")
                        'we know this camera has the following params:
                        '
                        'Dim bytes() As Byte = New Byte(myBaslerCam.getSizeX() * myBaslerCam.getSizeY()) {}
                        'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
                        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
                        'Else
                        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

                        'End If
                        'mySVSVistekCam.useDarks = Me.useDarks
                        b = myFWform.getLastImage()
                        ' mySVSVistekForm.writeline("acquired last SVS Vistek image")


                        'Dim BoundsRect = New Rectangle(0, 0, myWidth, myHeight)
                        'Dim bmpDataSrc As BitmapData = b.LockBits(BoundsRect, ImageLockMode.[ReadOnly], b.PixelFormat)
                        'Dim bytes As Integer = bmpDataSrc.Stride * b.Height
                        'Dim ptr As IntPtr = bmpDataSrc.Scan0

                        'Dim rawData = New Byte(bytes - 1) {}
                        ''copy source pic to byte array

                        'Marshal.Copy(ptr, rawData, 0, bytes)

                        'Dim b2 = New Bitmap(myWidth, myHeight, PixelFormat.Format8bppIndexed)
                        'Dim bmpData As BitmapData = b2.LockBits(BoundsRect, ImageLockMode.[WriteOnly], b2.PixelFormat)

                        ''b contains original
                        ''b2 is to be the copy
                        ''Dim ncp As ColorPalette = b2.Palette

                        ''For i = 0 To 255

                        ''    ncp.Entries(i) = Color.FromArgb(255, i, i, i)
                        ''Next
                        'b2.Palette = b.Palette
                        'Dim ptr2 As IntPtr = bmpData.Scan0
                        'Marshal.Copy(rawData, 0, ptr2, bytes)
                        ''from, to
                        'b2.UnlockBits(bmpData)

                        'b.UnlockBits(bmpDataSrc)

                        ''=======================================================
                        ''Service provided by Telerik (www.telerik.com)
                        ''Conversion powered by NRefactory.
                        ''Twitter: @telerik
                        ''Facebook: facebook.com/telerik
                        ''=======================================================





                        ''myBaslerForm.PictureBox1.Image = b2


                        Dim iTotBytes As Integer = 0
                        Dim sResponse As String = ""
                        'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                        '
                        Dim myImageCodecInfo As ImageCodecInfo
                        Dim myEncoder As System.Drawing.Imaging.Encoder
                        Dim myEncoderParameter As EncoderParameter
                        Dim myEncoderParameters As EncoderParameters

                        ' Create a Bitmap object based on a BMP file.


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
                        myEncoderParameter = New EncoderParameter(myEncoder, CType(95L, Int32))
                        myEncoderParameters.Param(0) = myEncoderParameter
                        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)


                        '
                        Dim ms As New MemoryStream()
                        '  Dim ms2 As New MemoryStream()
                        b.Save(ms, myImageCodecInfo, myEncoderParameters)
                        ' d2.Save(ms2, Imaging.ImageFormat.Bmp)
                        ' mySVSVistekForm.PictureBox1.Image = b
                        'Dim reader As New BinaryReader(ms)
                        '  Dim reader2 As New BinaryReader(ms2)
                        ' Dim bytes2() As Byte = New Byte(ms.Length) {}


                        'reader.BaseStream.Position = 0
                        '' reader2.BaseStream.Position = 0



                        'While reader.BaseStream.Position < reader.BaseStream.Length
                        '    reader.Read(bytes2, 0, bytes2.Length)

                        'End While
                        '' While reader2.BaseStream.Position < reader2.BaseStream.Length
                        ''     reader2.Read(bytesDarks, 0, bytesDarks.Length)

                        '' End While
                        '' Dim aVal As Integer


                        'sResponse = sResponse & Encoding.ASCII.GetString(bytes2, 0, reader.BaseStream.Length)
                        'iTotBytes = reader.BaseStream.Length
                        'reader.Close()
                        'ms.Close()

                        SendHeader(sHttpVersion, "image/jpeg", ms.Length, " 200 OK", mySocket)
                        SendToBrowser(ms.ToArray(), mySocket)
                        ms.Close()

                    Catch ex As Exception
                        imageInUse = imageInUse - 1
                        sErrorMessage = "404 Error! File Does Not Exists..."
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                        SendToBrowser(sErrorMessage, mySocket)
                        ' mySVSVistekBaumerForm.writeline("error encountered: " & ex.Message)
                    End Try
                End If

                ' End If
                mySocket.Close()
                mySocket = Nothing
                LocalTCPListener.Stop()

            End If
        Loop

    End Sub

    Private Sub StartListenQIFirewire()
        Dim iStartPos As Integer

        Dim sErrorMessage As String

        Dim sWebserverRoot = LocalVirtualRoot

        Dim sPhysicalFilePath As String = ""
        Dim sFormattedMessage As String = ""

        'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
        'Else
        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

        'End If
        'If Not mySVSVistekCam.isStreaming Then
        '    mySVSVistekCam.startStreamingFF()
        'End If


        Do While Not restart
            'accept new socket connection
            LocalTCPListener.Start()
            'mySVSVistekForm.writeline("starting SVS Vistek listener")
            Dim mySocket As Socket = LocalTCPListener.AcceptSocket
            If mySocket.Connected Then
                Dim bReceive() As Byte = New [Byte](1024) {}
                Dim i As Integer
                Try
                    i = mySocket.Receive(bReceive, bReceive.Length, 0)
                Catch ex As Exception
                    'socket blewup
                    Debug.Print(ex.Message)
                    restart = True
                    Exit Do
                End Try

                Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
                'find the GET request.
                ' mySVSVistekForm.writeline("SVS Vistek image server connected")
                If sBuffer.Contains("GET") And sBuffer.Contains("HTTP") Then


                    iStartPos = sBuffer.IndexOf("HTTP", 1)
                    Dim sHttpVersion = sBuffer.Substring(iStartPos, 8)


                    Try
                        'grab image from cam



                        'Dim myWidth As Integer = 1360
                        'Dim myHeight As Integer = 1036

                        Dim b As New Bitmap(1360, 1036, PixelFormat.Format8bppIndexed)
                        Dim ncp As System.Drawing.Imaging.ColorPalette = b.Palette
                        For j As Integer = 0 To 255
                            ncp.Entries(j) = System.Drawing.Color.FromArgb(255, j, j, j)
                        Next
                        b.Palette = ncp


                        Dim bytes() As Byte

                        bytes = myQIform.getLastImageArray()


                        Dim BoundsRect = New Rectangle(0, 0, 1360, 1036)
                        Dim bmpData As System.Drawing.Imaging.BitmapData = b.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], b.PixelFormat)

                        Dim ptr As IntPtr = bmpData.Scan0


                        ' b = frmQ.getLastImage


                        Marshal.Copy(bytes, 0, ptr, bytes.Length)
                        b.UnlockBits(bmpData)
                        b.RotateFlip(RotateFlipType.Rotate180FlipNone) 'camera is upside down
                        If Not b Is Nothing Then
                            ' mySVSVistekForm.writeline("acquired last SVS Vistek image")


                            'Dim BoundsRect = New Rectangle(0, 0, myWidth, myHeight)
                            'Dim bmpDataSrc As BitmapData = b.LockBits(BoundsRect, ImageLockMode.[ReadOnly], b.PixelFormat)
                            'Dim bytes As Integer = bmpDataSrc.Stride * b.Height
                            'Dim ptr As IntPtr = bmpDataSrc.Scan0

                            'Dim rawData = New Byte(bytes - 1) {}
                            ''copy source pic to byte array

                            'Marshal.Copy(ptr, rawData, 0, bytes)

                            'Dim b2 = New Bitmap(myWidth, myHeight, PixelFormat.Format8bppIndexed)
                            'Dim bmpData As BitmapData = b2.LockBits(BoundsRect, ImageLockMode.[WriteOnly], b2.PixelFormat)

                            ''b contains original
                            ''b2 is to be the copy
                            ''Dim ncp As ColorPalette = b2.Palette

                            ''For i = 0 To 255

                            ''    ncp.Entries(i) = Color.FromArgb(255, i, i, i)
                            ''Next
                            'b2.Palette = b.Palette
                            'Dim ptr2 As IntPtr = bmpData.Scan0
                            'Marshal.Copy(rawData, 0, ptr2, bytes)
                            ''from, to
                            'b2.UnlockBits(bmpData)

                            'b.UnlockBits(bmpDataSrc)

                            ''=======================================================
                            ''Service provided by Telerik (www.telerik.com)
                            ''Conversion powered by NRefactory.
                            ''Twitter: @telerik
                            ''Facebook: facebook.com/telerik
                            ''=======================================================





                            ''myBaslerForm.PictureBox1.Image = b2


                            Dim iTotBytes As Integer = 0
                            Dim sResponse As String = ""
                            'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                            '
                            Dim myImageCodecInfo As ImageCodecInfo
                            Dim myEncoder As System.Drawing.Imaging.Encoder
                            Dim myEncoderParameter As EncoderParameter
                            Dim myEncoderParameters As EncoderParameters

                            ' Create a Bitmap object based on a BMP file.


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
                            myEncoderParameter = New EncoderParameter(myEncoder, CType(95L, Int32))
                            myEncoderParameters.Param(0) = myEncoderParameter
                            ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)


                            '
                            Dim ms As New MemoryStream()
                            '  Dim ms2 As New MemoryStream()

                            b.Save(ms, myImageCodecInfo, myEncoderParameters)
                            ' d2.Save(ms2, Imaging.ImageFormat.Bmp)
                            ' mySVSVistekForm.PictureBox1.Image = b
                            'Dim reader As New BinaryReader(ms)
                            '  Dim reader2 As New BinaryReader(ms2)
                            ' Dim bytes2() As Byte = New Byte(ms.Length) {}


                            'reader.BaseStream.Position = 0
                            '' reader2.BaseStream.Position = 0



                            'While reader.BaseStream.Position < reader.BaseStream.Length
                            '    reader.Read(bytes2, 0, bytes2.Length)

                            'End While
                            '' While reader2.BaseStream.Position < reader2.BaseStream.Length
                            ''     reader2.Read(bytesDarks, 0, bytesDarks.Length)

                            '' End While
                            '' Dim aVal As Integer


                            'sResponse = sResponse & Encoding.ASCII.GetString(bytes2, 0, reader.BaseStream.Length)
                            'iTotBytes = reader.BaseStream.Length
                            'reader.Close()
                            'ms.Close()
                            b.Dispose()
                            SendHeader(sHttpVersion, "image/jpeg", ms.Length, " 200 OK", mySocket)
                            SendToBrowser(ms.ToArray(), mySocket)
                            ms.Close()
                        Else
                            sErrorMessage = "problem receiving images"
                            SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                            SendToBrowser(sErrorMessage, mySocket)
                        End If

                    Catch ex As Exception
                        imageInUse = imageInUse - 1
                        sErrorMessage = "404 Error! File Does Not Exists..."
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                        SendToBrowser(sErrorMessage, mySocket)
                        'mySVSVistekBaumerForm.writeline("error encountered: " & ex.Message)
                    End Try
                End If

                ' End If
                mySocket.Close()
                mySocket = Nothing
                LocalTCPListener.Stop()

            End If
        Loop
        restart = False
        StartListenQIFirewire()
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
    Public Sub StopWebServer()
        Try
            LocalTCPListener.Stop()
            WebThread.Abort()


            'If Not (myFirewireCam Is Nothing) Then
            '    myFirewireCam.Acquisition = 0
            'End If
        Catch ex As Exception

            'mySVSVistekForm.writeline(ex.Message)
        End Try
    End Sub
    Private Sub StartListenSVSVistek()
        Dim iStartPos As Integer

        Dim sErrorMessage As String

        Dim sWebserverRoot = LocalVirtualRoot

        Dim sPhysicalFilePath As String = ""
        Dim sFormattedMessage As String = ""

        If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
            mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text))
        Else
            mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text))

        End If
        'If Not mySVSVistekCam.isStreaming Then
        '    mySVSVistekCam.startStreamingFF()
        'End If


        Do While True
            'accept new socket connection
            LocalTCPListener.Start()
            'mySVSVistekForm.writeline("starting SVS Vistek listener")
            Dim mySocket As Socket = LocalTCPListener.AcceptSocket
            If mySocket.Connected Then
                Dim bReceive() As Byte = New [Byte](1024) {}
                Dim i As Integer = mySocket.Receive(bReceive, bReceive.Length, 0)
                Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
                'find the GET request.
                ' mySVSVistekForm.writeline("SVS Vistek image server connected")
                If sBuffer.Contains("GET") And sBuffer.Contains("HTTP") Then


                    iStartPos = sBuffer.IndexOf("HTTP", 1)
                    Dim sHttpVersion = sBuffer.Substring(iStartPos, 8)


                    Try
                        'grab image from cam

                        Dim b As Bitmap

                        Dim myWidth As Integer = 1392
                        Dim myHeight As Integer




                        'myWidth = mySVSVistekCam.getSizeX
                        'myHeight = mySVSVistekCam.getSizeY

                        'mySVSVistekForm.writeline("request for SVS Vistek image")
                        'we know this camera has the following params:
                        '
                        'Dim bytes() As Byte = New Byte(myBaslerCam.getSizeX() * myBaslerCam.getSizeY()) {}
                        'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
                        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
                        'Else
                        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

                        'End If
                        'mySVSVistekCam.useDarks = Me.useDarks
                        b = mySVSVistekForm.getLastImage()
                        ' mySVSVistekForm.writeline("acquired last SVS Vistek image")


                        'Dim BoundsRect = New Rectangle(0, 0, myWidth, myHeight)
                        'Dim bmpDataSrc As BitmapData = b.LockBits(BoundsRect, ImageLockMode.[ReadOnly], b.PixelFormat)
                        'Dim bytes As Integer = bmpDataSrc.Stride * b.Height
                        'Dim ptr As IntPtr = bmpDataSrc.Scan0

                        'Dim rawData = New Byte(bytes - 1) {}
                        ''copy source pic to byte array

                        'Marshal.Copy(ptr, rawData, 0, bytes)

                        'Dim b2 = New Bitmap(myWidth, myHeight, PixelFormat.Format8bppIndexed)
                        'Dim bmpData As BitmapData = b2.LockBits(BoundsRect, ImageLockMode.[WriteOnly], b2.PixelFormat)

                        ''b contains original
                        ''b2 is to be the copy
                        ''Dim ncp As ColorPalette = b2.Palette

                        ''For i = 0 To 255

                        ''    ncp.Entries(i) = Color.FromArgb(255, i, i, i)
                        ''Next
                        'b2.Palette = b.Palette
                        'Dim ptr2 As IntPtr = bmpData.Scan0
                        'Marshal.Copy(rawData, 0, ptr2, bytes)
                        ''from, to
                        'b2.UnlockBits(bmpData)

                        'b.UnlockBits(bmpDataSrc)

                        ''=======================================================
                        ''Service provided by Telerik (www.telerik.com)
                        ''Conversion powered by NRefactory.
                        ''Twitter: @telerik
                        ''Facebook: facebook.com/telerik
                        ''=======================================================





                        ''myBaslerForm.PictureBox1.Image = b2


                        Dim iTotBytes As Integer = 0
                        Dim sResponse As String = ""
                        'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                        '
                        Dim myImageCodecInfo As ImageCodecInfo
                        Dim myEncoder As System.Drawing.Imaging.Encoder
                        Dim myEncoderParameter As EncoderParameter
                        Dim myEncoderParameters As EncoderParameters

                        ' Create a Bitmap object based on a BMP file.


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
                        myEncoderParameter = New EncoderParameter(myEncoder, CType(95L, Int32))
                        myEncoderParameters.Param(0) = myEncoderParameter
                        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)


                        '
                        Dim ms As New MemoryStream()
                        '  Dim ms2 As New MemoryStream()
                        b.Save(ms, myImageCodecInfo, myEncoderParameters)
                        ' d2.Save(ms2, Imaging.ImageFormat.Bmp)
                        ' mySVSVistekForm.PictureBox1.Image = b
                        Dim reader As New BinaryReader(ms)
                        '  Dim reader2 As New BinaryReader(ms2)
                        Dim bytes2() As Byte = New Byte(ms.Length) {}


                        reader.BaseStream.Position = 0
                        ' reader2.BaseStream.Position = 0



                        While reader.BaseStream.Position < reader.BaseStream.Length
                            reader.Read(bytes2, 0, bytes2.Length)

                        End While
                        ' While reader2.BaseStream.Position < reader2.BaseStream.Length
                        '     reader2.Read(bytesDarks, 0, bytesDarks.Length)

                        ' End While
                        ' Dim aVal As Integer


                        sResponse = sResponse & Encoding.ASCII.GetString(bytes2, 0, reader.BaseStream.Length)
                        iTotBytes = reader.BaseStream.Length
                        reader.Close()
                        ms.Close()

                        SendHeader(sHttpVersion, "image/jpeg", iTotBytes, " 200 OK", mySocket)
                        SendToBrowser(bytes2, mySocket)

                    Catch ex As Exception
                        imageInUse = imageInUse - 1
                        sErrorMessage = "404 Error! File Does Not Exists..."
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                        SendToBrowser(sErrorMessage, mySocket)
                        mySVSVistekForm.writeline("error encountered: " & ex.Message)
                    End Try
                End If

                ' End If
                mySocket.Close()
                mySocket = Nothing
                LocalTCPListener.Stop()

            End If
        Loop

    End Sub

    Private Sub StartListenSVSVistekBaumer()
        Dim iStartPos As Integer

        Dim sErrorMessage As String

        Dim sWebserverRoot = LocalVirtualRoot

        Dim sPhysicalFilePath As String = ""
        Dim sFormattedMessage As String = ""

        'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
        'Else
        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

        'End If
        'If Not mySVSVistekCam.isStreaming Then
        '    mySVSVistekCam.startStreamingFF()
        'End If


        Do While True
            'accept new socket connection
            LocalTCPListener.Start()
            'mySVSVistekForm.writeline("starting SVS Vistek listener")
            Dim mySocket As Socket = LocalTCPListener.AcceptSocket
            If mySocket.Connected Then
                Dim bReceive() As Byte = New [Byte](1024) {}
                Dim i As Integer = mySocket.Receive(bReceive, bReceive.Length, 0)
                Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
                'find the GET request.
                ' mySVSVistekForm.writeline("SVS Vistek image server connected")
                If sBuffer.Contains("GET") And sBuffer.Contains("HTTP") Then


                    iStartPos = sBuffer.IndexOf("HTTP", 1)
                    Dim sHttpVersion = sBuffer.Substring(iStartPos, 8)


                    Try
                        'grab image from cam

                        Dim b As Bitmap

                        Dim myWidth As Integer = 1392
                        Dim myHeight As Integer




                        'myWidth = mySVSVistekCam.getSizeX
                        'myHeight = mySVSVistekCam.getSizeY

                        'mySVSVistekForm.writeline("request for SVS Vistek image")
                        'we know this camera has the following params:
                        '
                        'Dim bytes() As Byte = New Byte(myBaslerCam.getSizeX() * myBaslerCam.getSizeY()) {}
                        'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
                        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
                        'Else
                        '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

                        'End If
                        'mySVSVistekCam.useDarks = Me.useDarks
                        b = mySVSVistekBaumerForm.getLastImage()
                        ' mySVSVistekForm.writeline("acquired last SVS Vistek image")


                        'Dim BoundsRect = New Rectangle(0, 0, myWidth, myHeight)
                        'Dim bmpDataSrc As BitmapData = b.LockBits(BoundsRect, ImageLockMode.[ReadOnly], b.PixelFormat)
                        'Dim bytes As Integer = bmpDataSrc.Stride * b.Height
                        'Dim ptr As IntPtr = bmpDataSrc.Scan0

                        'Dim rawData = New Byte(bytes - 1) {}
                        ''copy source pic to byte array

                        'Marshal.Copy(ptr, rawData, 0, bytes)

                        'Dim b2 = New Bitmap(myWidth, myHeight, PixelFormat.Format8bppIndexed)
                        'Dim bmpData As BitmapData = b2.LockBits(BoundsRect, ImageLockMode.[WriteOnly], b2.PixelFormat)

                        ''b contains original
                        ''b2 is to be the copy
                        ''Dim ncp As ColorPalette = b2.Palette

                        ''For i = 0 To 255

                        ''    ncp.Entries(i) = Color.FromArgb(255, i, i, i)
                        ''Next
                        'b2.Palette = b.Palette
                        'Dim ptr2 As IntPtr = bmpData.Scan0
                        'Marshal.Copy(rawData, 0, ptr2, bytes)
                        ''from, to
                        'b2.UnlockBits(bmpData)

                        'b.UnlockBits(bmpDataSrc)

                        ''=======================================================
                        ''Service provided by Telerik (www.telerik.com)
                        ''Conversion powered by NRefactory.
                        ''Twitter: @telerik
                        ''Facebook: facebook.com/telerik
                        ''=======================================================





                        ''myBaslerForm.PictureBox1.Image = b2


                        Dim iTotBytes As Integer = 0
                        Dim sResponse As String = ""
                        'Dim fs As New FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                        '
                        Dim myImageCodecInfo As ImageCodecInfo
                        Dim myEncoder As System.Drawing.Imaging.Encoder
                        Dim myEncoderParameter As EncoderParameter
                        Dim myEncoderParameters As EncoderParameters

                        ' Create a Bitmap object based on a BMP file.


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
                        myEncoderParameter = New EncoderParameter(myEncoder, CType(95L, Int32))
                        myEncoderParameters.Param(0) = myEncoderParameter
                        ' myBitmap.Save("Shapes025.jpg", myImageCodecInfo, myEncoderParameters)


                        '
                        Dim ms As New MemoryStream()
                        '  Dim ms2 As New MemoryStream()
                        b.Save(ms, myImageCodecInfo, myEncoderParameters)
                        ' d2.Save(ms2, Imaging.ImageFormat.Bmp)
                        ' mySVSVistekForm.PictureBox1.Image = b
                        'Dim reader As New BinaryReader(ms)
                        '  Dim reader2 As New BinaryReader(ms2)
                        ' Dim bytes2() As Byte = New Byte(ms.Length) {}


                        'reader.BaseStream.Position = 0
                        '' reader2.BaseStream.Position = 0



                        'While reader.BaseStream.Position < reader.BaseStream.Length
                        '    reader.Read(bytes2, 0, bytes2.Length)

                        'End While
                        '' While reader2.BaseStream.Position < reader2.BaseStream.Length
                        ''     reader2.Read(bytesDarks, 0, bytesDarks.Length)

                        '' End While
                        '' Dim aVal As Integer


                        'sResponse = sResponse & Encoding.ASCII.GetString(bytes2, 0, reader.BaseStream.Length)
                        'iTotBytes = reader.BaseStream.Length
                        'reader.Close()
                        'ms.Close()

                        SendHeader(sHttpVersion, "image/jpeg", ms.Length, " 200 OK", mySocket)
                        SendToBrowser(ms.ToArray(), mySocket)
                        ms.Close()

                    Catch ex As Exception
                        imageInUse = imageInUse - 1
                        sErrorMessage = "404 Error! File Does Not Exists..."
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", mySocket)
                        SendToBrowser(sErrorMessage, mySocket)
                        mySVSVistekBaumerForm.writeline("error encountered: " & ex.Message)
                    End Try
                End If

                ' End If
                mySocket.Close()
                mySocket = Nothing
                LocalTCPListener.Stop()

            End If
        Loop

    End Sub



    'Private WithEvents myFirewireCam As AxFGControlLib.AxFGControlCtrl

#End Region



End Class

