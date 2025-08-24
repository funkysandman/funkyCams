Imports System.Threading
Imports System.Net.Http
Imports System.Collections.Specialized
Imports System.Web
Imports System.IO
Imports System.Drawing
Imports System.Runtime.Serialization.Json
Imports System.Web.Script.Serialization
Imports Newtonsoft.Json

Module Module1
    Public Property Rects As New List(Of MyRectangle)

    Sub Main()
        'loop through files and generate qe as well
        Dim from_date = DateTime.Now.AddHours(-19)
        Dim to_date = DateTime.Now.AddHours(0)
        Dim c As Bitmap
        Dim fName As String
        Dim myMS As MemoryStream
        Dim b As Byte()
        'Dim files = directory.GetFiles("*.jpg").Where(f >= f.LastWriteTime >= from_date && f.LastWriteTime <= to_date)
        Dim filedir As New DirectoryInfo("C:\missed")
        Dim fileList = filedir.GetFiles("*.jpg*")
        Dim queryMatchingFiles = From file In fileList
                                 Where file.LastWriteTime >= from_date And file.LastWriteTime <= to_date
        Dim datetaken As String
        Dim r As MyRectangle
        Dim qe As queueEntry
        readSettings()

        For Each afile In queryMatchingFiles


            fName = afile.FullName
            b = File.ReadAllBytes(fName)
            c = New Bitmap(fName)

            qe = New queueEntry
            qe.height = c.Height
            qe.width = c.Width
            qe.cameraID = "svs"
            datetaken = Left(fName, (InStr(fName, ".j") - 1))
            datetaken = Right(datetaken, 16)
            Try
                qe.dateTaken = DateTime.ParseExact(datetaken, "ddMMMyyyy-HHmmss", Nothing)
            Catch ex As Exception
                qe.dateTaken = DateTime.Now
            End Try

            qe.filename = fName
            qe.img = b

            CallAzureMeteorDetection(qe)

            c.Dispose()

            Console.WriteLine(fName)

        Next



    End Sub
    Public Sub readSettings()

        'try to read settings file
        Dim filename As String = "profile_EXO174CBGEC.json"
        Dim settingsJSON As String
        Dim r As MyRectangle
        Try

            settingsJSON = File.ReadAllText(filename)
            Dim jsonResulttodict = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(settingsJSON)
            Dim jss As New JavaScriptSerializer()


            Dim rectJS As Object = jsonResulttodict.Item("Rects")
            Rects = New List(Of MyRectangle)
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
    Public Class MyRectangle
        Public Property x As Integer
        Public Property y As Integer
        Public Property width As Integer
        Public Property height As Integer

    End Class
End Module
