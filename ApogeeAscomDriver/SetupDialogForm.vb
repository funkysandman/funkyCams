Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities
Imports ASCOM.Apogee

<ComVisible(False)>
Public Class SetupDialogForm

    Public c As APOGEELib.Camera2

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click ' OK button event handler
        ' Persist new values of user settings to the ASCOM profile
        Camera.comPort = ComboBoxComPort.SelectedItem ' Update the state variables with results from the dialogue
        Camera.traceState = chkTrace.Checked
        Camera.useROI = cbUseROI.Checked
        Camera.xStart = tbXstart.Text
        Camera.xWidth = tbXwidth.Text
        Camera.yStart = tbYstart.Text
        Camera.yHeight = tbYheight.Text
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click 'Cancel button event handler
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ShowAscomWebPage(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox1.DoubleClick, PictureBox1.Click
        ' Click on ASCOM logo event handler
        Try
            System.Diagnostics.Process.Start("http://ascom-standards.org/")
        Catch noBrowser As System.ComponentModel.Win32Exception
            If noBrowser.ErrorCode = -2147467259 Then
                MessageBox.Show(noBrowser.Message)
            End If
        Catch other As System.Exception
            MessageBox.Show(other.Message)
        End Try
    End Sub

    Private Sub SetupDialogForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load ' Form load event handler
        ' Retrieve current values of user settings from the ASCOM Profile
        InitUI()
    End Sub

    Private Sub InitUI()
        If Camera.useROI Then
            cbUseROI.Checked = True
            tbXstart.Text = Camera.xStart
            tbXwidth.Text = Camera.xWidth
            tbYstart.Text = Camera.yStart
            tbYheight.Text = Camera.yHeight
        End If
        If c IsNot Nothing Then
            'get gain
            Dim g As Integer
            Dim o As Integer
            c.GetAdGain(g, 1, 1)
            c.GetAdOffset(o, 1, 1)
            tbGain1.Text = g
            tbOffset1.Text = o
        End If
        chkTrace.Checked = Camera.traceState
        ' set the list of com ports to those that are currently available
        ComboBoxComPort.Items.Clear()
        ComboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames())       ' use System.IO because it's static
        ' select the current port if possible
        If ComboBoxComPort.Items.Contains(Camera.comPort) Then
            ComboBoxComPort.SelectedItem = Camera.comPort
        End If
    End Sub

    Private Sub btnTemp_Click(sender As Object, e As EventArgs) Handles btnTemp.Click
        If c IsNot Nothing Then
            c.ShowTempDialog()

        End If
    End Sub

    Private Sub btnIO_Click(sender As Object, e As EventArgs) Handles btnIO.Click
        If c IsNot Nothing Then
            c.ShowIoDialog()

        End If
    End Sub

    Private Sub btnLED_Click(sender As Object, e As EventArgs) Handles btnLED.Click
        If c IsNot Nothing Then
            c.ShowLedDialog()

        End If
    End Sub

    Private Sub btnSetGain1_Click(sender As Object, e As EventArgs) Handles btnSetGain1.Click
        If c IsNot Nothing Then
            Try
                c.SetAdGain(tbGain1.Text, 1, 1)
                MsgBox("gain set")
            Catch ex As Exception
                MsgBox("cannot set gain")
            End Try


        End If
    End Sub

    Private Sub btnSetOffset1_Click(sender As Object, e As EventArgs) Handles btnSetOffset1.Click
        If c IsNot Nothing Then
            Try
                c.SetAdOffset(tbOffset1.Text, 1, 1)
                MsgBox("offset set")
            Catch ex As Exception
                MsgBox("could not set gain")
            End Try


        End If
    End Sub
End Class
