Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Xml
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices


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

    Private _running As Boolean

    Private myISForm As frmIS

    Private restart As Boolean = False

    Private myPixeLinkForm As Object
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
    Public Property running() As Boolean
        Get
            Return _running
        End Get
        Set(ByVal Value As Boolean)
            _running = Value
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

    Public Sub StartWebServer(f As frmIS, port As Integer)
        Try
            LocalPort = port
            myISform = f

            'loadGigEDarks()
            LocalTCPListener = New TcpListener(LocalAddress, LocalPort)
            LocalTCPListener.Start()
            WebThread = New Thread(AddressOf StartListenIS)
            WebThread.Start()

        Catch ex As Exception

        End Try
    End Sub
    ' Here is where we check our XML file and see what MIME types are defined and handle the accordingly.
    Private Sub loadGigEDarks()
        'Dim fs As FileStream
        'fs = New FileStream(Application.StartupPath & "\darkGigE.drk", FileMode.Open)

        ''
        'rawDark = New Byte(2457656) {}
        'fs.Read(rawDark, 0, rawDark.Length - 1)
        'fs.Close()
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
            running = False
            Application.DoEvents()
            LocalTCPListener.Stop()
            WebThread.Abort()



            'If Not (myFirewireCam Is Nothing) Then
            '    myFirewireCam.Acquisition = 0
            'End If
        Catch ex As Exception

            'mySVSVistekForm.writeline(ex.Message)
        End Try
    End Sub

    Private Sub StartListenIS()
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
                        b = myISForm.getLastImage()
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

                    End Try
                End If

                ' End If
                mySocket.Close()
                mySocket = Nothing
                LocalTCPListener.Stop()

            End If
        Loop

    End Sub


    'Private Sub StartListenPixeLINK()
    '    Dim iStartPos As Integer

    '    Dim sErrorMessage As String

    '    Dim sWebserverRoot = LocalVirtualRoot

    '    Dim sPhysicalFilePath As String = ""
    '    Dim sFormattedMessage As String = ""

    '    'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
    '    '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
    '    'Else
    '    '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

    '    'End If
    '    'If Not mySVSVistekCam.isStreaming Then
    '    '    mySVSVistekCam.startStreamingFF()
    '    'End If


    '    Do While True
    '        'accept new socket connection
    '        LocalTCPListener.Start()
    '        'mySVSVistekForm.writeline("starting SVS Vistek listener")
    '        Dim mySocket As Socket = LocalTCPListener.AcceptSocket
    '        If mySocket.Connected Then
    '            Dim bReceive() As Byte = New [Byte](1024) {}
    '            Dim i As Integer = mySocket.Receive(bReceive, bReceive.Length, 0)
    '            Dim sBuffer As String = Encoding.ASCII.GetString(bReceive)
    '            'find the GET request.
    '            ' mySVSVistekForm.writeline("SVS Vistek image server connected")
    '            If sBuffer.Contains("GET") And sBuffer.Contains("HTTP") Then


    '                iStartPos = sBuffer.IndexOf("HTTP", 1)
    '                Dim sHttpVersion = sBuffer.Substring(iStartPos, 8)


    '                Try
    '                    'grab image from cam

    '                    Dim b As Byte()

    '                    Dim myWidth As Integer = myPixeLinkForm.iWidth
    '                    Dim myHeight As Integer = myPixeLinkForm.iHeight




    '                    'myWidth = mySVSVistekCam.getSizeX
    '                    'myHeight = mySVSVistekCam.getSizeY

    '                    'mySVSVistekForm.writeline("request for SVS Vistek image")
    '                    'we know this camera has the following params:
    '                    '
    '                    'Dim bytes() As Byte = New Byte(myBaslerCam.getSizeX() * myBaslerCam.getSizeY()) {}
    '                    'If LCase(mySVSVistekForm.lblDayNight.Text) = "day" Then
    '                    '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbDayGain.Text), Val(mySVSVistekForm.tbDayDgain.Text), Val(mySVSVistekForm.tbDayGamma.Text), 0)
    '                    'Else
    '                    '    mySVSVistekCam.setParams(Val(mySVSVistekForm.tbExposureTime.Text), Val(mySVSVistekForm.tbNightAgain.Text), Val(mySVSVistekForm.tbNightDgain.Text), Val(mySVSVistekForm.tbNightGamma.Text), 0)

    '                    'End If
    '                    'mySVSVistekCam.useDarks = Me.useDarks
    '                    b = myPixeLinkForm.getLastImageArray()

    '                    Dim x As New Bitmap(myWidth, myHeight, Imaging.PixelFormat.Format24bppRgb)
    '                    Dim BoundsRect = New Rectangle(0, 0, frmPixelink.iWidth, frmPixelink.iHeight)
    '                    Dim bmpData As System.Drawing.Imaging.BitmapData = x.LockBits(BoundsRect, System.Drawing.Imaging.ImageLockMode.[WriteOnly], x.PixelFormat)
    '                    Dim ptr As IntPtr = bmpData.Scan0
    '                    System.Runtime.InteropServices.Marshal.Copy(b, 0, ptr, myWidth * myHeight * 3 - 1) 'copy into bitmap


    '                    x.UnlockBits(bmpData)




    '                    ' mySVSVistekForm.writeline("acquired last SVS Vistek image")


    '                    'Dim BoundsRect = New Rectangle(0, 0, myWidth, myHeight)
    '                    'Dim bmpDataSrc As BitmapData = b.LockBits(BoundsRect, ImageLockMode.[ReadOnly], b.PixelFormat)
    '                    'Dim bytes As Integer = bmpDataSrc.Stride * b.Height
    '                    'Dim ptr As IntPtr = bmpDataSrc.Scan0

    '                    'Dim rawData = New Byte(bytes - 1) {}
    '                    ''copy source pic to byte array

    '                    'Marshal.Copy(ptr, rawData, 0, bytes)

    '                    'Dim b2 = New Bitmap(myWidth, myHeight, PixelFormat.Format8bppIndexed)
    '                    'Dim bmpData As BitmapData = b2.LockBits(BoundsRect, ImageLockMode.[WriteOnly], b2.PixelFormat)

    '                    ''b contains original
    '                    ''b2 is to be the copy
    '                    ''Dim ncp As ColorPalette = b2.Palette

    '                    ''For i = 0 To 255

    '                    ''    ncp.Entries(i) = Color.FromArgb(255, i, i, i)
    '                    ''Next
    '                    'b2.Palette = b.Palette
    '                    'Dim ptr2 As IntPtr = bmpData.Scan0
    '                    'Marshal.Copy(rawData, 0, ptr2, bytes)
    '                    ''from, to
    '                    'b2.UnlockBits(bmpData)

    '                    'b.UnlockBits(bmpDataSrc)

    '                    ''=======================================================
    '                    ''Service provided by Telerik (www.telerik.com)
    '                    ''Conversion powered by NRefactory.
    '                    ''Twitter: @telerik
    '                    ''Facebook: facebook.com/telerik
    '                    ''=======================================================





    '                    ''myBaslerForm.PictureBox1.Image = b2


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
    '                    x.Save(ms, myImageCodecInfo, myEncoderParameters)
    '                    ' d2.Save(ms2, Imaging.ImageFormat.Bmp)
    '                    ' mySVSVistekForm.PictureBox1.Image = b
    '                    'Dim reader As New BinaryReader(ms)
    '                    '  Dim reader2 As New BinaryReader(ms2)
    '                    ' Dim bytes2() As Byte = New Byte(ms.Length) {}


    '                    'reader.BaseStream.Position = 0
    '                    '' reader2.BaseStream.Position = 0



    '                    'While reader.BaseStream.Position < reader.BaseStream.Length
    '                    '    reader.Read(bytes2, 0, bytes2.Length)

    '                    'End While
    '                    '' While reader2.BaseStream.Position < reader2.BaseStream.Length
    '                    ''     reader2.Read(bytesDarks, 0, bytesDarks.Length)

    '                    '' End While
    '                    '' Dim aVal As Integer


    '                    'sResponse = sResponse & Encoding.ASCII.GetString(bytes2, 0, reader.BaseStream.Length)
    '                    'iTotBytes = reader.BaseStream.Length
    '                    'reader.Close()
    '                    'ms.Close()

    '                    SendHeader(sHttpVersion, "image/jpeg", ms.Length, " 200 OK", mySocket)
    '                    SendToBrowser(ms.ToArray(), mySocket)
    '                    ms.Close()

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

#End Region



End Class

