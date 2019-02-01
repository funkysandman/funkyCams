
Imports System.Xml
Imports System.Text





Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'load all xml files and save to csv file
        Dim aPath As String = "C:\annotate\VOC2012\Annotations"
        Dim aSecondPath As String = "C:\meteor_detect\data"
        Dim outfileName As String
        outfileName = Path.Combine(aSecondPath, "meteor.csv")
        Dim outFile As New FileStream(outfileName, FileMode.OpenOrCreate)
        Dim xmldoc As New XmlDataDocument()
        Dim xmlnode As XmlNodeList

        Dim di As New DirectoryInfo(aPath)
        ' Get a reference to each file in that directory.
        Dim fiArr As FileInfo() = di.GetFiles().OrderBy(Function(fi) fi.CreationTime).ToArray()
        ' Display the names of the files.
        Dim f As FileInfo
        Dim fs As FileStream

        'setup a datatable

        Dim dt As New DataTable


        dt.Columns.Add("filename")
        dt.Columns.Add("width")
        dt.Columns.Add("height")
        dt.Columns.Add("class")
        dt.Columns.Add("xmin")
        dt.Columns.Add("ymin")
        dt.Columns.Add("xmax")
        dt.Columns.Add("ymax")


        For Each f In fiArr
            'read each xml file

            fs = New FileStream(f.FullName, FileMode.Open, FileAccess.Read)
            xmldoc.Load(fs)
            Dim dr As DataRow = dt.NewRow
            xmlnode = xmldoc.GetElementsByTagName("filename")
            dr("filename") = xmlnode(0).FirstChild.Value
            xmlnode = xmldoc.GetElementsByTagName("width")
            dr("width") = xmlnode(0).FirstChild.Value
            xmlnode = xmldoc.GetElementsByTagName("height")
            dr("height") = xmlnode(0).FirstChild.Value
            xmlnode = xmldoc.GetElementsByTagName("name")
            dr("class") = xmlnode(0).FirstChild.Value
            xmlnode = xmldoc.GetElementsByTagName("xmin")
            dr("xmin") = xmlnode(0).FirstChild.Value
            xmlnode = xmldoc.GetElementsByTagName("ymin")
            dr("ymin") = xmlnode(0).FirstChild.Value
            xmlnode = xmldoc.GetElementsByTagName("xmax")
            dr("xmax") = xmlnode(0).FirstChild.Value
            xmlnode = xmldoc.GetElementsByTagName("ymax")
            dr("ymax") = xmlnode(0).FirstChild.Value
            xmldoc.RemoveAll()
            dt.Rows.Add(dr)
            fs.Close()
        Next
        'save data table to csv
        Dim things() As Byte
        things = csvBytesWriter(dt)
        outFile.Write(things, 0, things.Count)
        outFile.Close()

        MsgBox("done")


    End Sub
    Function csvBytesWriter(ByRef dTable As DataTable) As Byte()

        '--------Columns Name--------------------------------------------------------------------------- 

        Dim sb As StringBuilder = New StringBuilder()
        Dim intClmn As Integer = dTable.Columns.Count

        Dim i As Integer = 0
        For i = 0 To intClmn - 1 Step i + 1
            sb.Append(dTable.Columns(i).ColumnName.ToString())
            If i = intClmn - 1 Then
                sb.Append("")
            Else
                sb.Append(",")
            End If
        Next
        sb.Append(vbNewLine)

        '--------Data By  Columns--------------------------------------------------------------------------- 

        Dim row As DataRow
        For Each row In dTable.Rows

            Dim ir As Integer = 0
            For ir = 0 To intClmn - 1 Step ir + 1
                sb.Append(row(ir).ToString().Replace("""", """"""))
                If ir = intClmn - 1 Then
                    sb.Append("")
                Else
                    sb.Append(",")
                End If

            Next
            sb.Append(vbNewLine)
        Next

        Return System.Text.Encoding.UTF8.GetBytes(sb.ToString)


    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'read csv
        Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser("C:\meteor_detect\data\testing.csv")
            MyReader.TextFieldType = FileIO.FieldType.Delimited
            MyReader.SetDelimiters(",")
            Dim currentRow As String()
            Dim filename As String
            Dim width As Integer
            Dim height As Integer
            Dim xmin As Integer
            Dim ymin As Integer
            Dim xmax As Integer
            Dim ymax As Integer
            Dim classname As String
            Dim b As Bitmap
            currentRow = MyReader.ReadFields() 'ignore header
            While Not MyReader.EndOfData
                Try
                    currentRow = MyReader.ReadFields()
                    Dim currentField As String
                    'image,id,name,xMin,xMax,yMin,yMax
                    filename = currentRow(0)
                    Label1.Text = filename
                    ' width = currentRow(1)
                    ' height = currentRow(2)
                    classname = currentRow(2)
                    xmin = currentRow(3)
                    xmax = currentRow(4)
                    ymin = currentRow(5)
                    ymax = currentRow(6)
                    b = Bitmap.FromFile("C:\meteor_detect\images\" & filename)
                    Dim newGraphics As Graphics = Graphics.FromImage(b)


                    newGraphics.DrawRectangle(Pens.Aquamarine, xmin, ymin, xmax - xmin, ymax - ymin)
                    ' Draw image to screen.

                    Me.PictureBox1.Image = b
                    MsgBox("ok")
                Catch ex As Microsoft.VisualBasic.
                  FileIO.MalformedLineException
                    MsgBox("Line " & ex.Message &
        "is not valid and will be skipped.")
                End Try
            End While
        End Using
    End Sub
End Class
