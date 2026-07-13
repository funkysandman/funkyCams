<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAscom
    Inherits frmMaster

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblCameraSelect = New System.Windows.Forms.Label()
        Me.btnRefreshCameras = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cmbCam
        '
        Me.cmbCam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCam.FormattingEnabled = True
        Me.cmbCam.Location = New System.Drawing.Point(98, 34)
        Me.cmbCam.Name = "cmbCam"
        Me.cmbCam.Size = New System.Drawing.Size(243, 21)
        Me.cmbCam.TabIndex = 161
        Me.cmbCam.Visible = True
        '
        'lblCameraSelect
        '
        Me.lblCameraSelect.AutoSize = True
        Me.lblCameraSelect.Location = New System.Drawing.Point(12, 37)
        Me.lblCameraSelect.Name = "lblCameraSelect"
        Me.lblCameraSelect.Size = New System.Drawing.Size(80, 13)
        Me.lblCameraSelect.TabIndex = 162
        Me.lblCameraSelect.Text = "ASCOM Camera:"
        '
        'btnRefreshCameras
        '
        Me.btnRefreshCameras.Location = New System.Drawing.Point(347, 32)
        Me.btnRefreshCameras.Name = "btnRefreshCameras"
        Me.btnRefreshCameras.Size = New System.Drawing.Size(60, 23)
        Me.btnRefreshCameras.TabIndex = 163
        Me.btnRefreshCameras.Text = "Refresh"
        Me.btnRefreshCameras.UseVisualStyleBackColor = True
        '
        'frmAscom
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 525)
        Me.Controls.Add(Me.btnRefreshCameras)
        Me.Controls.Add(Me.lblCameraSelect)
        Me.Name = "frmAscom"
        Me.Text = "ASCOM Camera Control"
        Me.Controls.SetChildIndex(Me.lblCameraSelect, 0)
        Me.Controls.SetChildIndex(Me.btnRefreshCameras, 0)
        Me.Controls.SetChildIndex(Me.cmbCam, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblCameraSelect As Label
    Friend WithEvents btnRefreshCameras As Button

End Class
