Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'if you're here it means one of the dlls for BGAPI2 is missing ...usually bhapi2_img.dll
        Try
            Dim imgProcessor As New BGAPI2.ImageProcessor()
        Catch ex As Exception
            MessageBox.Show($"Failed to initialize BGAPI2.ImageProcessor: {ex.Message}{vbCrLf}{vbCrLf}Inner: {ex.InnerException?.Message}",
                       "Initialization Error",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error)
        End Try
    End Sub
End Class