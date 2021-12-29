Public Class frmExclude
    Private x = 0, y = 0
    Private x1 = 0, y1 = 0
    Private width, height As Integer
    Public Rects As List(Of MyRectangle)
    Private xFactor
    Private yFactor


    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        'get x, y from mouse
        'MsgBox(e.Location)
        x = e.Location.X
        y = e.Location.Y

    End Sub

    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        'get width, height
        'draw rect
        width = Math.Abs(x - e.Location.X)
        height = Math.Abs(y - e.Location.Y)
        x1 = e.Location.X
        y1 = e.Location.Y

        Dim r As New MyRectangle
        r.X = x * xFactor
        r.Y = y * yFactor
        r.Width = (x1 - x) * xFactor
        r.Height = (y1 - y) * yFactor
        Rects.Add(r)
        PictureBox1.Refresh()
    End Sub

    Private Sub loadRects()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Rects = New List(Of MyRectangle)
        PictureBox1.Refresh()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub


    Private Sub frmExclude_Load(sender As Object, e As EventArgs) Handles Me.Load
        'get image from file dialog
        Dim filename As String

        Dim fd As FileDialog
        Dim result As DialogResult = OpenFileDialog1.ShowDialog()

        ' Test result.
        If result = Windows.Forms.DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If

        xFactor = PictureBox1.Image.Width / PictureBox1.Width
        yFactor = PictureBox1.Image.Height / PictureBox1.Height
    End Sub

    Private Sub PictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox1.Paint
        Dim r As New Rectangle
        Using pen As New System.Drawing.Pen(System.Drawing.Color.Black)
            For i = 0 To Rects.Count - 1
                r.X = Rects(i).X / xFactor
                r.Y = Rects(i).Y / yFactor
                r.Width = Rects(i).Width / xFactor
                r.Height = Rects(i).Height / yFactor

                e.Graphics.DrawRectangle(pen, r)
            Next


        End Using
    End Sub
End Class