Imports System.Threading
Public Class Form3



    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub



    Private Sub btnIS_Click(sender As Object, e As EventArgs) Handles btnIS.Click
        Dim f As New frmIS
        Me.Hide()
        f.ShowDialog()
        Me.Close()
    End Sub

End Class