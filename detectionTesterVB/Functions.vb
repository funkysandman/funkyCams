
Imports System.Collections.Specialized
Imports System.Net.Http


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
    End Module



