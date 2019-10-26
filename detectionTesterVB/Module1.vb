Imports System.Threading
Imports System.Net.Http
Imports System.Collections.Specialized
Imports System.Web
Imports System.IO
Imports System.Drawing

Module Module1

    Sub Main()
        'loop through files and generate qe as well
        Dim from_date = DateTime.Now.AddHours(-196)
        Dim to_date = DateTime.Now.AddHours(-10)
        Dim c As Bitmap
        Dim fName As String
        Dim myMS As MemoryStream
        Dim b As Byte()
        'Dim files = directory.GetFiles("*.jpg").Where(f >= f.LastWriteTime >= from_date && f.LastWriteTime <= to_date)
        Dim filedir As New DirectoryInfo("E:\found_backup")
        Dim fileList = filedir.GetFiles("*.jpg")
        Dim queryMatchingFiles = From file In fileList
                                 Where file.LastWriteTime >= from_date And file.LastWriteTime <= to_date
        Dim datetaken As String

        Dim qe As queueEntry
        For Each afile In queryMatchingFiles


            fName = afile.FullName
            b = File.ReadAllBytes(fName)
            c = New Bitmap(fName)

            qe = New queueEntry
            qe.height = c.Height
            qe.width = c.Width
            qe.cameraID = "notsaved"
            datetaken = Left(fName, (InStr(fName, ".") - 1))
            datetaken = Right(datetaken, 16)
            qe.dateTaken = DateTime.ParseExact(datetaken, "ddMMMyyyy-HHmmss", Nothing)
            qe.filename = fName
            qe.img = b
            CallAzureMeteorDetection(qe)

            c.Dispose()


        Next



    End Sub
    Public Sub CallAzureMeteorDetection(qe As queueEntry)

        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.192:7071/api/detection"
        Dim myUriBuilder As New UriBuilder(apiURL)


        Dim query As NameValueCollection = Web.HttpUtility.ParseQueryString(String.Empty)

        query("file") = qe.filename
        query("dateTaken") = qe.dateTaken.ToString("MM/dd/yyyy hh:mm tt")
        query("cameraID") = qe.cameraID
        query("width") = qe.width
        query("height") = qe.height
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
    End Sub
End Module
