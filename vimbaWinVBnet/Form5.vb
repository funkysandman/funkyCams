
Public Class Form5

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim x As New BaslerWrapper.Grabber

        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

        x.Open()
        PictureBox1.Image = x.getImage(Val(TextBox1.Text), 0)
        x.close()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim x As New BaslerWrapper.Grabber

        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage

        x.Open()
        PictureBox1.Image = x.getImage(Val(TextBox1.Text), 0)
        x.close()
    End Sub

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class