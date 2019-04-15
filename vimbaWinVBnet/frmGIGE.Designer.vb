<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGIGE
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tbMessage = New System.Windows.Forms.TextBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbPath = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.tbPort = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtFps = New System.Windows.Forms.TextBox()
        Me.cbMeteors = New System.Windows.Forms.CheckBox()
        Me.cmbCam = New System.Windows.Forms.ComboBox()
        Me.tbMultiplier = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.cbSaveImages = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbNightGamma = New System.Windows.Forms.TextBox()
        Me.tbNightDgain = New System.Windows.Forms.TextBox()
        Me.tbNightAgain = New System.Windows.Forms.TextBox()
        Me.tbDayGamma = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbDayDgain = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblDayNight = New System.Windows.Forms.Label()
        Me.tbExposureTime = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbDayGain = New System.Windows.Forms.TextBox()
        Me.tbNightExp = New System.Windows.Forms.TextBox()
        Me.tbDayTimeExp = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboNight = New System.Windows.Forms.ComboBox()
        Me.cboDay = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cbUseDarks = New System.Windows.Forms.CheckBox()
        Me.TimerAcquistionRate = New System.Windows.Forms.Timer(Me.components)
        Me.Button7 = New System.Windows.Forms.Button()
        Me.tbGain = New System.Windows.Forms.TextBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbMessage
        '
        Me.tbMessage.Location = New System.Drawing.Point(29, 414)
        Me.tbMessage.Margin = New System.Windows.Forms.Padding(2)
        Me.tbMessage.Multiline = True
        Me.tbMessage.Name = "tbMessage"
        Me.tbMessage.Size = New System.Drawing.Size(336, 76)
        Me.tbMessage.TabIndex = 72
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(329, 337)
        Me.Button10.Margin = New System.Windows.Forms.Padding(2)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(36, 18)
        Me.Button10.TabIndex = 70
        Me.Button10.Text = "..."
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(30, 340)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 69
        Me.Label5.Text = "path:"
        '
        'tbPath
        '
        Me.tbPath.Location = New System.Drawing.Point(65, 337)
        Me.tbPath.Margin = New System.Windows.Forms.Padding(2)
        Me.tbPath.Name = "tbPath"
        Me.tbPath.Size = New System.Drawing.Size(252, 20)
        Me.tbPath.TabIndex = 68
        Me.tbPath.Text = "c:\image"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(295, -2)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(70, 46)
        Me.Button1.TabIndex = 66
        Me.Button1.Text = "start"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'tbPort
        '
        Me.tbPort.Location = New System.Drawing.Point(200, 377)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(36, 20)
        Me.tbPort.TabIndex = 60
        Me.tbPort.Text = "8083"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 20000
        '
        'Button6
        '
        Me.Button6.Enabled = False
        Me.Button6.Location = New System.Drawing.Point(252, 374)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(113, 23)
        Me.Button6.TabIndex = 61
        Me.Button6.Text = "stop webserver"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(65, 374)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(113, 23)
        Me.Button5.TabIndex = 53
        Me.Button5.Text = "start webserver"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(28, 17)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(337, 307)
        Me.PictureBox1.TabIndex = 48
        Me.PictureBox1.TabStop = False
        '
        'Timer2
        '
        Me.Timer2.Interval = 500
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(552, 352)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(21, 13)
        Me.Label10.TabIndex = 101
        Me.Label10.Text = "fps"
        '
        'txtFps
        '
        Me.txtFps.Location = New System.Drawing.Point(555, 369)
        Me.txtFps.Name = "txtFps"
        Me.txtFps.Size = New System.Drawing.Size(48, 20)
        Me.txtFps.TabIndex = 100
        '
        'cbMeteors
        '
        Me.cbMeteors.AutoSize = True
        Me.cbMeteors.Location = New System.Drawing.Point(439, 376)
        Me.cbMeteors.Name = "cbMeteors"
        Me.cbMeteors.Size = New System.Drawing.Size(96, 17)
        Me.cbMeteors.TabIndex = 99
        Me.cbMeteors.Text = "detect meteors"
        Me.cbMeteors.UseVisualStyleBackColor = True
        '
        'cmbCam
        '
        Me.cmbCam.FormattingEnabled = True
        Me.cmbCam.Location = New System.Drawing.Point(427, 12)
        Me.cmbCam.Name = "cmbCam"
        Me.cmbCam.Size = New System.Drawing.Size(188, 21)
        Me.cmbCam.TabIndex = 98
        '
        'tbMultiplier
        '
        Me.tbMultiplier.Location = New System.Drawing.Point(393, 352)
        Me.tbMultiplier.Margin = New System.Windows.Forms.Padding(2)
        Me.tbMultiplier.Name = "tbMultiplier"
        Me.tbMultiplier.Size = New System.Drawing.Size(35, 20)
        Me.tbMultiplier.TabIndex = 97
        Me.tbMultiplier.Text = "1.0"
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(486, 304)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(62, 23)
        Me.Button3.TabIndex = 96
        Me.Button3.Text = "Stop"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(555, 304)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(64, 38)
        Me.Button2.TabIndex = 95
        Me.Button2.Text = "take darks"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(418, 304)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(62, 23)
        Me.Button4.TabIndex = 94
        Me.Button4.Text = "Start snapshots"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'cbSaveImages
        '
        Me.cbSaveImages.AutoSize = True
        Me.cbSaveImages.Location = New System.Drawing.Point(439, 353)
        Me.cbSaveImages.Name = "cbSaveImages"
        Me.cbSaveImages.Size = New System.Drawing.Size(97, 17)
        Me.cbSaveImages.TabIndex = 93
        Me.cbSaveImages.Text = "Save snaphots"
        Me.cbSaveImages.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(567, 36)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 13)
        Me.Label8.TabIndex = 92
        Me.Label8.Text = "night"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(511, 36)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(24, 13)
        Me.Label7.TabIndex = 91
        Me.Label7.Text = "day"
        '
        'tbNightGamma
        '
        Me.tbNightGamma.Location = New System.Drawing.Point(570, 115)
        Me.tbNightGamma.Name = "tbNightGamma"
        Me.tbNightGamma.Size = New System.Drawing.Size(47, 20)
        Me.tbNightGamma.TabIndex = 90
        Me.tbNightGamma.Text = "1.2"
        '
        'tbNightDgain
        '
        Me.tbNightDgain.Location = New System.Drawing.Point(570, 89)
        Me.tbNightDgain.Name = "tbNightDgain"
        Me.tbNightDgain.Size = New System.Drawing.Size(47, 20)
        Me.tbNightDgain.TabIndex = 89
        Me.tbNightDgain.Text = "1"
        '
        'tbNightAgain
        '
        Me.tbNightAgain.Location = New System.Drawing.Point(570, 63)
        Me.tbNightAgain.Name = "tbNightAgain"
        Me.tbNightAgain.Size = New System.Drawing.Size(47, 20)
        Me.tbNightAgain.TabIndex = 88
        Me.tbNightAgain.Text = "27"
        '
        'tbDayGamma
        '
        Me.tbDayGamma.Location = New System.Drawing.Point(514, 115)
        Me.tbDayGamma.Name = "tbDayGamma"
        Me.tbDayGamma.Size = New System.Drawing.Size(47, 20)
        Me.tbDayGamma.TabIndex = 87
        Me.tbDayGamma.Text = "1"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(415, 122)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(91, 13)
        Me.Label6.TabIndex = 86
        Me.Label6.Text = "gamma correction"
        '
        'tbDayDgain
        '
        Me.tbDayDgain.Location = New System.Drawing.Point(514, 89)
        Me.tbDayDgain.Name = "tbDayDgain"
        Me.tbDayDgain.Size = New System.Drawing.Size(47, 20)
        Me.tbDayDgain.TabIndex = 85
        Me.tbDayDgain.Text = "1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(415, 93)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 84
        Me.Label1.Text = "digital gain"
        '
        'lblDayNight
        '
        Me.lblDayNight.AutoSize = True
        Me.lblDayNight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDayNight.Location = New System.Drawing.Point(415, 273)
        Me.lblDayNight.Name = "lblDayNight"
        Me.lblDayNight.Size = New System.Drawing.Size(35, 13)
        Me.lblDayNight.TabIndex = 83
        Me.lblDayNight.Text = "night"
        '
        'tbExposureTime
        '
        Me.tbExposureTime.Enabled = False
        Me.tbExposureTime.Location = New System.Drawing.Point(569, 270)
        Me.tbExposureTime.Name = "tbExposureTime"
        Me.tbExposureTime.Size = New System.Drawing.Size(45, 20)
        Me.tbExposureTime.TabIndex = 82
        Me.tbExposureTime.Text = "4996100"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(415, 66)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 81
        Me.Label4.Text = "analog gain"
        '
        'tbDayGain
        '
        Me.tbDayGain.Location = New System.Drawing.Point(514, 63)
        Me.tbDayGain.Name = "tbDayGain"
        Me.tbDayGain.Size = New System.Drawing.Size(47, 20)
        Me.tbDayGain.TabIndex = 80
        Me.tbDayGain.Text = "0"
        '
        'tbNightExp
        '
        Me.tbNightExp.Location = New System.Drawing.Point(570, 216)
        Me.tbNightExp.Name = "tbNightExp"
        Me.tbNightExp.Size = New System.Drawing.Size(45, 20)
        Me.tbNightExp.TabIndex = 79
        Me.tbNightExp.Text = "4996100"
        '
        'tbDayTimeExp
        '
        Me.tbDayTimeExp.Location = New System.Drawing.Point(570, 181)
        Me.tbDayTimeExp.Name = "tbDayTimeExp"
        Me.tbDayTimeExp.Size = New System.Drawing.Size(45, 20)
        Me.tbDayTimeExp.TabIndex = 78
        Me.tbDayTimeExp.Text = "125"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(415, 216)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 77
        Me.Label3.Text = "night"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(414, 162)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 13)
        Me.Label2.TabIndex = 76
        Me.Label2.Text = "day"
        '
        'cboNight
        '
        Me.cboNight.DisplayMember = "6"
        Me.cboNight.FormattingEnabled = True
        Me.cboNight.Items.AddRange(New Object() {"15", "16", "17", "18", "19", "20", "21"})
        Me.cboNight.Location = New System.Drawing.Point(417, 232)
        Me.cboNight.Name = "cboNight"
        Me.cboNight.Size = New System.Drawing.Size(109, 21)
        Me.cboNight.TabIndex = 75
        Me.cboNight.ValueMember = "6"
        '
        'cboDay
        '
        Me.cboDay.DisplayMember = "displayMember"
        Me.cboDay.FormattingEnabled = True
        Me.cboDay.Items.AddRange(New Object() {"4", "5", "6", "7", "8", "9"})
        Me.cboDay.Location = New System.Drawing.Point(418, 178)
        Me.cboDay.Name = "cboDay"
        Me.cboDay.Size = New System.Drawing.Size(109, 21)
        Me.cboDay.TabIndex = 74
        Me.cboDay.ValueMember = "displayMember"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(428, 182)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(0, 13)
        Me.Label9.TabIndex = 73
        '
        'cbUseDarks
        '
        Me.cbUseDarks.AutoSize = True
        Me.cbUseDarks.Location = New System.Drawing.Point(396, 399)
        Me.cbUseDarks.Name = "cbUseDarks"
        Me.cbUseDarks.Size = New System.Drawing.Size(110, 17)
        Me.cbUseDarks.TabIndex = 102
        Me.cbUseDarks.Text = "use darks at night"
        Me.cbUseDarks.UseVisualStyleBackColor = True
        '
        'TimerAcquistionRate
        '
        Me.TimerAcquistionRate.Interval = 5000
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(211, 403)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(154, 38)
        Me.Button7.TabIndex = 103
        Me.Button7.Text = "Button7"
        Me.Button7.UseVisualStyleBackColor = True
        Me.Button7.Visible = False
        '
        'tbGain
        '
        Me.tbGain.Location = New System.Drawing.Point(489, 270)
        Me.tbGain.Name = "tbGain"
        Me.tbGain.Size = New System.Drawing.Size(47, 20)
        Me.tbGain.TabIndex = 104
        Me.tbGain.Text = "27"
        '
        'frmGIGE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(627, 417)
        Me.Controls.Add(Me.tbGain)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.cbUseDarks)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtFps)
        Me.Controls.Add(Me.cbMeteors)
        Me.Controls.Add(Me.cmbCam)
        Me.Controls.Add(Me.tbMultiplier)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.cbSaveImages)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbNightGamma)
        Me.Controls.Add(Me.tbNightDgain)
        Me.Controls.Add(Me.tbNightAgain)
        Me.Controls.Add(Me.tbDayGamma)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbDayDgain)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblDayNight)
        Me.Controls.Add(Me.tbExposureTime)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbDayGain)
        Me.Controls.Add(Me.tbNightExp)
        Me.Controls.Add(Me.tbDayTimeExp)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboNight)
        Me.Controls.Add(Me.cboDay)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.tbMessage)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbPath)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.tbPort)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "frmGIGE"
        Me.Text = "frmGIGE"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tbMessage As TextBox
    Friend WithEvents Button10 As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents Label5 As Label
    Friend WithEvents tbPath As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Timer3 As Timer
    Friend WithEvents tbPort As TextBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Button6 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Timer2 As Timer
    Friend WithEvents Label10 As Label
    Friend WithEvents txtFps As TextBox
    Friend WithEvents cbMeteors As CheckBox
    Friend WithEvents cmbCam As ComboBox
    Friend WithEvents tbMultiplier As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents cbSaveImages As CheckBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents tbNightGamma As TextBox
    Friend WithEvents tbNightDgain As TextBox
    Friend WithEvents tbNightAgain As TextBox
    Friend WithEvents tbDayGamma As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents tbDayDgain As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblDayNight As Label
    Friend WithEvents tbExposureTime As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents tbDayGain As TextBox
    Friend WithEvents tbNightExp As TextBox
    Friend WithEvents tbDayTimeExp As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cboNight As ComboBox
    Friend WithEvents cboDay As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cbUseDarks As CheckBox
    Friend WithEvents TimerAcquistionRate As Timer
    Friend WithEvents Button7 As Button
    Friend WithEvents tbGain As TextBox
End Class
