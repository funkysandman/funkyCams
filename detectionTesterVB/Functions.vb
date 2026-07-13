
Imports System.Collections.Specialized
Imports System.Net.Http
Imports System.Net.Http.Headers


Public Module Functions
    Public Async Function CallAzureMeteorDetection(qe As queueEntry) As Task
        '        Dim apiURL As String = "https://azuremeteordetect20181212113628.azurewebsites.net/api/detection?code=zi3Lrr58mJB3GTut0lktSLIzb08E1dLkHXAbX6s07bd46IoZmm1vqQ==&file=" + file
        Dim apiURL As String = "http://192.168.1.199:7071/api/detection"
        Dim myUriBuilder As New UriBuilder(apiURL)


        Dim query As NameValueCollection = Web.HttpUtility.ParseQueryString(String.Empty)

        query("file") = qe.filename
        query("dateTaken") = qe.dateTaken.ToString("MM/dd/yyyy hh:mm:ss tt")
        query("cameraID") = qe.cameraID
        query("width") = qe.width
        query("height") = qe.height

        If Rects.Count > 0 Then
            'add rectangles
            query("rectangles") = Rects.Count
            For i = 0 To Rects.Count - 1
                query("r_" + Trim(Str(i)) + "_x") = Rects(i).x
                query("r_" + Trim(Str(i)) + "_y") = Rects(i).y
                query("r_" + Trim(Str(i)) + "_w") = Rects(i).width
                query("r_" + Trim(Str(i)) + "_h") = Rects(i).height
            Next
        End If

        myUriBuilder.Query = query.ToString


        Dim handler As New HttpClientHandler()
        handler.UseProxy = False
        Dim client As New HttpClient(handler)

        Dim byteContent = New ByteArrayContent(qe.img)
        byteContent.Headers.ContentType = New MediaTypeHeaderValue("image/jpeg")
        Try
            Dim response = client.PostAsync(myUriBuilder.ToString(), byteContent).Result
            Dim responseString = response.Content.ReadAsStringAsync().Result
        Catch ex As AggregateException
            For Each inner In ex.InnerExceptions
                Console.WriteLine(inner.Message)
                Console.WriteLine(inner.StackTrace)
            Next
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Console.WriteLine(ex.StackTrace)
        End Try
    End Function
    End Module



