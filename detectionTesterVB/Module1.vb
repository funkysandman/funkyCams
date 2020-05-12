Imports System.Threading
Imports System.Net.Http
Imports System.Collections.Specialized
Imports System.Web
Imports System.IO
Imports System.Drawing

Module Module1

    Sub Main()
        'loop through files and generate qe as well
        Dim from_date = DateTime.Now.AddHours(-1969999)
        Dim to_date = DateTime.Now.AddHours(0)
        Dim c As Bitmap
        Dim fName As String
        Dim myMS As MemoryStream
        Dim b As Byte()
        'Dim files = directory.GetFiles("*.jpg").Where(f >= f.LastWriteTime >= from_date && f.LastWriteTime <= to_date)
        Dim filedir As New DirectoryInfo("d:\images")
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

End Module
