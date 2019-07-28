Imports System.Threading
Public Class Form3


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f As New frmAVT
        Me.Hide()
        f.ShowDialog()
        Me.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim f As New frmFoculs
        Me.Hide()
        f.ShowDialog()
        Me.Close()
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
        'frmSVSVistek.ShowDialog()
        Me.Close()

    End Sub



    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        'Dim f As New frmAVTastro
        'f.Show()
        'Me.Hide()
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim f As New frmGIGE
        Me.Hide()
        f.ShowDialog()
        Me.Close()

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim f As New frmCoolsnap
        Me.Hide()
        f.ShowDialog()
        Me.Close()
    End Sub
End Class