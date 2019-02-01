Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports ASCOM.Utilities
Imports ASCOM.AVTGIGE

<ComVisible(False)> _
Public Class SetupDialogForm
    Public v As AVT.VmbAPINET.Vimba
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click ' OK button event handler
        ' Persist new values of user settings to the ASCOM profile
        Camera.gigeCamID = ComboBox1.SelectedItem
        Camera.m_exposureTime = Val(tbExposure.Text)
        Camera.m_gain = Val(tbGain.Text)
        Camera.m_chosenPort = ComboBox2.SelectedItem
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click 'Cancel button event handler
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Camera.gigeCamID = ""
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

        ' set the list of com ports to those that are currently available
        ComboBox1.Items.Clear()
        If v Is Nothing Then
            v = New AVT.VmbAPINET.Vimba
            v.Startup()
        End If


        Dim cl As AVT.VmbAPINET.CameraCollection
        cl = v.Cameras
        For Each c As AVT.VmbAPINET.Camera In cl
            ComboBox1.Items.Add(c.Id)
        Next
        cl = Nothing

        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBox2.Items.Add(sp)
        Next


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ''load pixel formats
        'v = New AVT.VmbAPINET.Vimba
        'v.Startup()
        'Dim cl As AVT.VmbAPINET.CameraCollection
        'Dim c As AVT.VmbAPINET.Camera
        'c = v.GetCameraByID(ComboBox1.SelectedItem)
        ''Dim list() As String
        ''list = c.Features("PixelFormat").EnumValues
        ''For Each pf As String In c.Features("PixelFormat").EnumValues
        ''    ComboBox3.Items.Add(c.Id)
        ''Next
        'Dim entries As AVT.VmbAPINET.EnumEntryCollection
        'MsgBox(c.Features("PixelFormat").StringValue)


        'For Each entry As AVT.VmbAPINET.EnumEntry In entries
        '    ComboBox3.Items.Add(entry.Name)
        'Next


        '        v.Shutdown()
    End Sub
End Class
