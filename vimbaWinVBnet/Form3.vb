Imports System.Threading
Public Class Form3
    Public t As Thread

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f As New frmAVT
        f.ShowDialog()
        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        frmFoculs.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'frmBasler.Show()
        'Me.Hide()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        't = New Thread(AddressOf showFrmSVSVistek)
        't.ApartmentState = ApartmentState.STA
        't.Start()
        'showFrmSVSVistek()
        frmSVSVistek.ShowDialog()
        Me.Close()

    End Sub



    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim f As New frmAVTastro
        f.Show()
        Me.Hide()
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        frmGIGE.ShowDialog()
        Me.Close()

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim f As New frmQ
        f.Show()
        Me.Hide()
    End Sub
End Class