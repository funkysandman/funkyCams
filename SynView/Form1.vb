Imports NewElectronicTechnology.SynView

Public Class Form1
    Inherits System.Windows.Forms.Form
    Private m_pCamera As Genicam.CCamera

    Friend WithEvents CheckBoxProcessing As System.Windows.Forms.CheckBox
    Private m_hDisplayWindow As IntPtr

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()
        'This call is required by the Windows Form Designer.
        InitializeComponent()
        m_pSystem = Nothing
        LvLibrary.ThrowErrorEnable = True
        Try
            LvLibrary.OpenLibrary()
            LvSystem.Open("", m_pSystem)
            '  m_pCamera = New CCamera()
            UpdateControls()
        Catch ex As LvException
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Close()
        End Try

        ' The PictureBoxLive does not accept to be accessed from another thread
        ' However, the window handle should not have a problem with multithreading
        m_hDisplayWindow = PictureBoxLive.Handle

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            '  If m_pCamera IsNot Nothing Then m_pCamera.CloseCamera()
            If m_pSystem IsNot Nothing Then LvSystem.Close(m_pSystem)
            LvLibrary.CloseLibrary()
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents StatusBar As System.Windows.Forms.StatusBar
    Friend WithEvents PictureBoxLive As System.Windows.Forms.PictureBox
    Friend WithEvents ControlPanel As System.Windows.Forms.Panel
    Friend WithEvents ButtonAcquisitionStop As System.Windows.Forms.Button
    Friend WithEvents ButtonExit As System.Windows.Forms.Button
    Friend WithEvents ButtonAcquisitionStart As System.Windows.Forms.Button
    Friend WithEvents ButtonDisconnectCamera As System.Windows.Forms.Button
    Friend WithEvents ButtonConnectCamera As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.StatusBar = New System.Windows.Forms.StatusBar()
        Me.PictureBoxLive = New System.Windows.Forms.PictureBox()
        Me.ControlPanel = New System.Windows.Forms.Panel()
        Me.CheckBoxProcessing = New System.Windows.Forms.CheckBox()
        Me.ButtonAcquisitionStop = New System.Windows.Forms.Button()
        Me.ButtonExit = New System.Windows.Forms.Button()
        Me.ButtonAcquisitionStart = New System.Windows.Forms.Button()
        Me.ButtonDisconnectCamera = New System.Windows.Forms.Button()
        Me.ButtonConnectCamera = New System.Windows.Forms.Button()
        CType(Me.PictureBoxLive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ControlPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusBar
        '
        Me.StatusBar.Location = New System.Drawing.Point(0, 395)
        Me.StatusBar.Name = "StatusBar"
        Me.StatusBar.Size = New System.Drawing.Size(624, 19)
        Me.StatusBar.TabIndex = 6
        Me.StatusBar.Text = "-"
        '
        'PictureBoxLive
        '
        Me.PictureBoxLive.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBoxLive.BackColor = System.Drawing.SystemColors.Control
        Me.PictureBoxLive.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBoxLive.Location = New System.Drawing.Point(0, 0)
        Me.PictureBoxLive.Name = "PictureBoxLive"
        Me.PictureBoxLive.Size = New System.Drawing.Size(461, 393)
        Me.PictureBoxLive.TabIndex = 5
        Me.PictureBoxLive.TabStop = False
        '
        'ControlPanel
        '
        Me.ControlPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ControlPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ControlPanel.Controls.Add(Me.CheckBoxProcessing)
        Me.ControlPanel.Controls.Add(Me.ButtonAcquisitionStop)
        Me.ControlPanel.Controls.Add(Me.ButtonExit)
        Me.ControlPanel.Controls.Add(Me.ButtonAcquisitionStart)
        Me.ControlPanel.Controls.Add(Me.ButtonDisconnectCamera)
        Me.ControlPanel.Controls.Add(Me.ButtonConnectCamera)
        Me.ControlPanel.Location = New System.Drawing.Point(462, -2)
        Me.ControlPanel.Name = "ControlPanel"
        Me.ControlPanel.Size = New System.Drawing.Size(163, 395)
        Me.ControlPanel.TabIndex = 4
        '
        'CheckBoxProcessing
        '
        Me.CheckBoxProcessing.AutoSize = True
        Me.CheckBoxProcessing.Location = New System.Drawing.Point(18, 157)
        Me.CheckBoxProcessing.Name = "CheckBoxProcessing"
        Me.CheckBoxProcessing.Size = New System.Drawing.Size(145, 21)
        Me.CheckBoxProcessing.TabIndex = 8
        Me.CheckBoxProcessing.Text = "Simple processing"
        Me.CheckBoxProcessing.UseVisualStyleBackColor = True
        '
        'ButtonAcquisitionStop
        '
        Me.ButtonAcquisitionStop.Location = New System.Drawing.Point(10, 76)
        Me.ButtonAcquisitionStop.Name = "ButtonAcquisitionStop"
        Me.ButtonAcquisitionStop.Size = New System.Drawing.Size(144, 27)
        Me.ButtonAcquisitionStop.TabIndex = 1
        Me.ButtonAcquisitionStop.Text = "Stop acquisition"
        '
        'ButtonExit
        '
        Me.ButtonExit.Location = New System.Drawing.Point(10, 219)
        Me.ButtonExit.Name = "ButtonExit"
        Me.ButtonExit.Size = New System.Drawing.Size(144, 27)
        Me.ButtonExit.TabIndex = 7
        Me.ButtonExit.Text = "Exit"
        '
        'ButtonAcquisitionStart
        '
        Me.ButtonAcquisitionStart.Location = New System.Drawing.Point(10, 43)
        Me.ButtonAcquisitionStart.Name = "ButtonAcquisitionStart"
        Me.ButtonAcquisitionStart.Size = New System.Drawing.Size(144, 26)
        Me.ButtonAcquisitionStart.TabIndex = 5
        Me.ButtonAcquisitionStart.Text = "Start acquisition"
        '
        'ButtonDisconnectCamera
        '
        Me.ButtonDisconnectCamera.Location = New System.Drawing.Point(10, 110)
        Me.ButtonDisconnectCamera.Name = "ButtonDisconnectCamera"
        Me.ButtonDisconnectCamera.Size = New System.Drawing.Size(144, 26)
        Me.ButtonDisconnectCamera.TabIndex = 2
        Me.ButtonDisconnectCamera.Text = "Disconnect camera"
        '
        'ButtonConnectCamera
        '
        Me.ButtonConnectCamera.Location = New System.Drawing.Point(10, 9)
        Me.ButtonConnectCamera.Name = "ButtonConnectCamera"
        Me.ButtonConnectCamera.Size = New System.Drawing.Size(144, 27)
        Me.ButtonConnectCamera.TabIndex = 0
        Me.ButtonConnectCamera.Text = "Connect camera"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(624, 414)
        Me.Controls.Add(Me.StatusBar)
        Me.Controls.Add(Me.PictureBoxLive)
        Me.Controls.Add(Me.ControlPanel)
        Me.Name = "Form1"
        Me.Text = "Sample application in VB with SynView .Net Class Library"
        CType(Me.PictureBoxLive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ControlPanel.ResumeLayout(False)
        Me.ControlPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub UpdateStatusLine()
        If m_pCamera.IsOpen() Then
            If m_pCamera.IsAcquiring() Then
                StatusBar.Text = "Camera connected, acquiring"
            Else
                StatusBar.Text = "Camera connected, not acquiring"
            End If
        Else
            StatusBar.Text = "Camera not connected"
        End If
    End Sub

    Private Sub UpdateControls()
        ButtonConnectCamera.Enabled = (Not m_pCamera.IsOpen())
        Dim IsAcquiring As Boolean = m_pCamera.IsAcquiring()
        Dim IsOpen As Boolean = m_pCamera.IsOpen()
        ButtonDisconnectCamera.Enabled = IsOpen And Not IsAcquiring
        ButtonAcquisitionStart.Enabled = IsOpen And Not IsAcquiring
        ButtonAcquisitionStop.Enabled = IsOpen And IsAcquiring
        ButtonExit.Enabled = Not IsOpen
        UpdateStatusLine()
    End Sub

    Private Sub ButtonConnectCamera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonConnectCamera.Click
        Cursor = Cursors.WaitCursor
        'm_pCamera.OpenCamera()
        UpdateControls()
        Cursor = Cursors.Default
    End Sub

    Private Sub ButtonAcquisitionStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAcquisitionStart.Click
        m_pCamera.StartAcquisition()
        UpdateControls()
    End Sub

    Private Sub ButtonAcquisitionStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAcquisitionStop.Click
        m_pCamera.StopAcquisition()
        UpdateControls()
    End Sub

    Private Sub ButtonDisconnectCamera_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDisconnectCamera.Click
        m_pCamera.CloseCamera()
        PictureBoxLive.Invalidate()
        UpdateControls()
    End Sub

    Private Sub ButtonExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExit.Click
        m_pCamera.CloseCamera()
        Close()
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        ' disable closing if the camera is open
        e.Cancel = m_pCamera.IsOpen()
    End Sub

    Private Sub PictureBoxLive_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBoxLive.Paint
        m_pCamera.Repaint()
    End Sub

    Private Sub CheckBoxProcessing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxProcessing.CheckedChanged
        m_pCamera.SetProcessing(CheckBoxProcessing.Checked)
    End Sub

    Private Sub ControlPanel_Paint(sender As Object, e As PaintEventArgs) Handles ControlPanel.Paint

    End Sub
End Class
