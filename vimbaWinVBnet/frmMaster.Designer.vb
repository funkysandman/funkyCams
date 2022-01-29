<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMaster
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.TimerDayNight = New System.Windows.Forms.Timer(Me.components)
        Me.TimerFPS = New System.Windows.Forms.Timer(Me.components)
        Me.Label13 = New System.Windows.Forms.Label()
        Me.tbLost = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tbUpper = New System.Windows.Forms.TextBox()
        Me.tbLower = New System.Windows.Forms.TextBox()
        Me.cbUseTrigger = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtFps = New System.Windows.Forms.TextBox()
        Me.cbMeteors = New System.Windows.Forms.CheckBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tbPath = New System.Windows.Forms.TextBox()
        Me.cmbCam = New System.Windows.Forms.ComboBox()
        Me.tbMultiplier = New System.Windows.Forms.TextBox()
        Me.btnStop = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.cbSaveImages = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbNightAgain = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblDayNight = New System.Windows.Forms.Label()
        Me.tbExposureTime = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbDayGain = New System.Windows.Forms.TextBox()
        Me.tbNightExp = New System.Windows.Forms.TextBox()
        Me.tbDayTimeExp = New System.Windows.Forms.TextBox()
        Me.btnStopWeb = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboNight = New System.Windows.Forms.ComboBox()
        Me.cboDay = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cbUseDarks = New System.Windows.Forms.CheckBox()
        Me.tbPort = New System.Windows.Forms.TextBox()
        Me.btnStartWeb = New System.Windows.Forms.Button()
        Me.tbGain = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tbDarkCutOff = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbURL = New System.Windows.Forms.TextBox()
        Me.lblURL = New System.Windows.Forms.Label()
        Me.Image_timer = New System.Windows.Forms.Timer(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Timer2
        '
        Me.Timer2.Interval = 1000
        '
        'TimerDayNight
        '
        Me.TimerDayNight.Enabled = True
        Me.TimerDayNight.Interval = 2000
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(307, 484)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(57, 13)
        Me.Label13.TabIndex = 150
        Me.Label13.Text = "frames lost"
        '
        'tbLost
        '
        Me.tbLost.Location = New System.Drawing.Point(369, 484)
        Me.tbLost.Name = "tbLost"
        Me.tbLost.Size = New System.Drawing.Size(48, 20)
        Me.tbLost.TabIndex = 149
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(120, 495)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 148
        Me.Label11.Text = "upper"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(3, 495)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(32, 13)
        Me.Label12.TabIndex = 147
        Me.Label12.Text = "lower"
        '
        'tbUpper
        '
        Me.tbUpper.Location = New System.Drawing.Point(158, 492)
        Me.tbUpper.Name = "tbUpper"
        Me.tbUpper.Size = New System.Drawing.Size(62, 20)
        Me.tbUpper.TabIndex = 146
        Me.tbUpper.Text = "4095"
        '
        'tbLower
        '
        Me.tbLower.Location = New System.Drawing.Point(41, 492)
        Me.tbLower.Name = "tbLower"
        Me.tbLower.Size = New System.Drawing.Size(62, 20)
        Me.tbLower.TabIndex = 145
        Me.tbLower.Text = "1"
        '
        'cbUseTrigger
        '
        Me.cbUseTrigger.AutoSize = True
        Me.cbUseTrigger.Location = New System.Drawing.Point(178, 37)
        Me.cbUseTrigger.Name = "cbUseTrigger"
        Me.cbUseTrigger.Size = New System.Drawing.Size(115, 17)
        Me.cbUseTrigger.TabIndex = 144
        Me.cbUseTrigger.Text = "use external trigger"
        Me.cbUseTrigger.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(312, 373)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(21, 13)
        Me.Label10.TabIndex = 143
        Me.Label10.Text = "fps"
        '
        'txtFps
        '
        Me.txtFps.Location = New System.Drawing.Point(315, 390)
        Me.txtFps.Name = "txtFps"
        Me.txtFps.Size = New System.Drawing.Size(48, 20)
        Me.txtFps.TabIndex = 142
        '
        'cbMeteors
        '
        Me.cbMeteors.AutoSize = True
        Me.cbMeteors.Location = New System.Drawing.Point(199, 397)
        Me.cbMeteors.Name = "cbMeteors"
        Me.cbMeteors.Size = New System.Drawing.Size(96, 17)
        Me.cbMeteors.TabIndex = 141
        Me.cbMeteors.Text = "detect meteors"
        Me.cbMeteors.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(305, 424)
        Me.Button10.Margin = New System.Windows.Forms.Padding(2)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(36, 18)
        Me.Button10.TabIndex = 140
        Me.Button10.Text = "..."
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(7, 427)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(31, 13)
        Me.Label9.TabIndex = 139
        Me.Label9.Text = "path:"
        '
        'tbPath
        '
        Me.tbPath.Location = New System.Drawing.Point(41, 424)
        Me.tbPath.Margin = New System.Windows.Forms.Padding(2)
        Me.tbPath.Name = "tbPath"
        Me.tbPath.Size = New System.Drawing.Size(252, 20)
        Me.tbPath.TabIndex = 138
        Me.tbPath.Text = "c:\imageSVS"
        '
        'cmbCam
        '
        Me.cmbCam.FormattingEnabled = True
        Me.cmbCam.Location = New System.Drawing.Point(177, 12)
        Me.cmbCam.Name = "cmbCam"
        Me.cmbCam.Size = New System.Drawing.Size(188, 21)
        Me.cmbCam.TabIndex = 137
        '
        'tbMultiplier
        '
        Me.tbMultiplier.Location = New System.Drawing.Point(153, 373)
        Me.tbMultiplier.Margin = New System.Windows.Forms.Padding(2)
        Me.tbMultiplier.Name = "tbMultiplier"
        Me.tbMultiplier.Size = New System.Drawing.Size(35, 20)
        Me.tbMultiplier.TabIndex = 136
        Me.tbMultiplier.Text = "1.0"
        '
        'btnStop
        '
        Me.btnStop.Location = New System.Drawing.Point(246, 325)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(62, 23)
        Me.btnStop.TabIndex = 135
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(315, 325)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(64, 38)
        Me.Button2.TabIndex = 134
        Me.Button2.Text = "take darks"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(177, 325)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(62, 23)
        Me.btnStart.TabIndex = 133
        Me.btnStart.Text = "Start snapshots"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'cbSaveImages
        '
        Me.cbSaveImages.AutoSize = True
        Me.cbSaveImages.Location = New System.Drawing.Point(199, 374)
        Me.cbSaveImages.Name = "cbSaveImages"
        Me.cbSaveImages.Size = New System.Drawing.Size(97, 17)
        Me.cbSaveImages.TabIndex = 132
        Me.cbSaveImages.Text = "Save snaphots"
        Me.cbSaveImages.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(327, 57)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 13)
        Me.Label8.TabIndex = 131
        Me.Label8.Text = "night"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(271, 57)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(24, 13)
        Me.Label7.TabIndex = 130
        Me.Label7.Text = "day"
        '
        'tbNightAgain
        '
        Me.tbNightAgain.Location = New System.Drawing.Point(330, 84)
        Me.tbNightAgain.Name = "tbNightAgain"
        Me.tbNightAgain.Size = New System.Drawing.Size(47, 20)
        Me.tbNightAgain.TabIndex = 127
        Me.tbNightAgain.Text = "27"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(15, 24)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(142, 116)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 122
        Me.PictureBox1.TabStop = False
        '
        'lblDayNight
        '
        Me.lblDayNight.AutoSize = True
        Me.lblDayNight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDayNight.Location = New System.Drawing.Point(175, 293)
        Me.lblDayNight.Name = "lblDayNight"
        Me.lblDayNight.Size = New System.Drawing.Size(35, 13)
        Me.lblDayNight.TabIndex = 121
        Me.lblDayNight.Text = "night"
        '
        'tbExposureTime
        '
        Me.tbExposureTime.Enabled = False
        Me.tbExposureTime.Location = New System.Drawing.Point(329, 291)
        Me.tbExposureTime.Name = "tbExposureTime"
        Me.tbExposureTime.Size = New System.Drawing.Size(45, 20)
        Me.tbExposureTime.TabIndex = 120
        Me.tbExposureTime.Text = "4996100"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(175, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 119
        Me.Label4.Text = "analog gain"
        '
        'tbDayGain
        '
        Me.tbDayGain.Location = New System.Drawing.Point(273, 84)
        Me.tbDayGain.Name = "tbDayGain"
        Me.tbDayGain.Size = New System.Drawing.Size(47, 20)
        Me.tbDayGain.TabIndex = 118
        Me.tbDayGain.Text = "0"
        '
        'tbNightExp
        '
        Me.tbNightExp.Location = New System.Drawing.Point(330, 237)
        Me.tbNightExp.Name = "tbNightExp"
        Me.tbNightExp.Size = New System.Drawing.Size(45, 20)
        Me.tbNightExp.TabIndex = 117
        Me.tbNightExp.Text = "4996100"
        '
        'tbDayTimeExp
        '
        Me.tbDayTimeExp.Location = New System.Drawing.Point(330, 202)
        Me.tbDayTimeExp.Name = "tbDayTimeExp"
        Me.tbDayTimeExp.Size = New System.Drawing.Size(45, 20)
        Me.tbDayTimeExp.TabIndex = 116
        Me.tbDayTimeExp.Text = "125"
        '
        'btnStopWeb
        '
        Me.btnStopWeb.Location = New System.Drawing.Point(177, 450)
        Me.btnStopWeb.Name = "btnStopWeb"
        Me.btnStopWeb.Size = New System.Drawing.Size(116, 22)
        Me.btnStopWeb.TabIndex = 115
        Me.btnStopWeb.Text = "stop web"
        Me.btnStopWeb.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(175, 237)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 114
        Me.Label3.Text = "night"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(174, 183)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 13)
        Me.Label2.TabIndex = 113
        Me.Label2.Text = "day"
        '
        'cboNight
        '
        Me.cboNight.DisplayMember = "6"
        Me.cboNight.FormattingEnabled = True
        Me.cboNight.Items.AddRange(New Object() {"12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23"})
        Me.cboNight.Location = New System.Drawing.Point(177, 253)
        Me.cboNight.Name = "cboNight"
        Me.cboNight.Size = New System.Drawing.Size(109, 21)
        Me.cboNight.TabIndex = 112
        Me.cboNight.ValueMember = "6"
        '
        'cboDay
        '
        Me.cboDay.DisplayMember = "displayMember"
        Me.cboDay.FormattingEnabled = True
        Me.cboDay.Items.AddRange(New Object() {"4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.cboDay.Location = New System.Drawing.Point(177, 198)
        Me.cboDay.Name = "cboDay"
        Me.cboDay.Size = New System.Drawing.Size(109, 21)
        Me.cboDay.TabIndex = 111
        Me.cboDay.ValueMember = "displayMember"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(188, 203)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 110
        '
        'cbUseDarks
        '
        Me.cbUseDarks.AutoSize = True
        Me.cbUseDarks.Location = New System.Drawing.Point(309, 454)
        Me.cbUseDarks.Name = "cbUseDarks"
        Me.cbUseDarks.Size = New System.Drawing.Size(110, 17)
        Me.cbUseDarks.TabIndex = 109
        Me.cbUseDarks.Text = "use darks at night"
        Me.cbUseDarks.UseVisualStyleBackColor = True
        '
        'tbPort
        '
        Me.tbPort.Location = New System.Drawing.Point(122, 452)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(50, 20)
        Me.tbPort.TabIndex = 108
        Me.tbPort.Text = "8050"
        '
        'btnStartWeb
        '
        Me.btnStartWeb.Location = New System.Drawing.Point(13, 452)
        Me.btnStartWeb.Name = "btnStartWeb"
        Me.btnStartWeb.Size = New System.Drawing.Size(104, 23)
        Me.btnStartWeb.TabIndex = 107
        Me.btnStartWeb.Text = "Start webserver"
        Me.btnStartWeb.UseVisualStyleBackColor = True
        '
        'tbGain
        '
        Me.tbGain.Location = New System.Drawing.Point(273, 290)
        Me.tbGain.Name = "tbGain"
        Me.tbGain.Size = New System.Drawing.Size(45, 20)
        Me.tbGain.TabIndex = 151
        Me.tbGain.Text = "27"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(239, 484)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(66, 27)
        Me.btnSave.TabIndex = 152
        Me.btnSave.Text = "save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'tbDarkCutOff
        '
        Me.tbDarkCutOff.Location = New System.Drawing.Point(29, 374)
        Me.tbDarkCutOff.Margin = New System.Windows.Forms.Padding(2)
        Me.tbDarkCutOff.Name = "tbDarkCutOff"
        Me.tbDarkCutOff.Size = New System.Drawing.Size(88, 20)
        Me.tbDarkCutOff.TabIndex = 153
        Me.tbDarkCutOff.Text = "50000"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(26, 359)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 13)
        Me.Label5.TabIndex = 154
        Me.Label5.Text = "hot pixel cut off"
        '
        'tbURL
        '
        Me.tbURL.Location = New System.Drawing.Point(178, 13)
        Me.tbURL.Name = "tbURL"
        Me.tbURL.Size = New System.Drawing.Size(239, 20)
        Me.tbURL.TabIndex = 155
        Me.tbURL.Visible = False
        '
        'lblURL
        '
        Me.lblURL.AutoSize = True
        Me.lblURL.Location = New System.Drawing.Point(100, 15)
        Me.lblURL.Name = "lblURL"
        Me.lblURL.Size = New System.Drawing.Size(72, 13)
        Me.lblURL.TabIndex = 156
        Me.lblURL.Text = "IP camera url:"
        Me.lblURL.Visible = False
        '
        'Image_timer
        '
        Me.Image_timer.Interval = 5000
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(294, 148)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(98, 35)
        Me.Button1.TabIndex = 157
        Me.Button1.Text = "define exclusion"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmMaster
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 525)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.lblURL)
        Me.Controls.Add(Me.tbURL)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbDarkCutOff)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.tbGain)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.tbLost)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.tbUpper)
        Me.Controls.Add(Me.tbLower)
        Me.Controls.Add(Me.cbUseTrigger)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtFps)
        Me.Controls.Add(Me.cbMeteors)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.tbPath)
        Me.Controls.Add(Me.cmbCam)
        Me.Controls.Add(Me.tbMultiplier)
        Me.Controls.Add(Me.btnStop)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.cbSaveImages)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbNightAgain)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblDayNight)
        Me.Controls.Add(Me.tbExposureTime)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbDayGain)
        Me.Controls.Add(Me.tbNightExp)
        Me.Controls.Add(Me.tbDayTimeExp)
        Me.Controls.Add(Me.btnStopWeb)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboNight)
        Me.Controls.Add(Me.cboDay)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbUseDarks)
        Me.Controls.Add(Me.tbPort)
        Me.Controls.Add(Me.btnStartWeb)
        Me.Name = "frmMaster"
        Me.Text = "frmMaster"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Timer2 As Timer
    Friend WithEvents TimerDayNight As Timer
    Friend WithEvents TimerFPS As Timer
    Friend WithEvents Label13 As Label
    Friend WithEvents tbLost As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents tbUpper As TextBox
    Friend WithEvents tbLower As TextBox
    Friend WithEvents cbUseTrigger As CheckBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtFps As TextBox
    Friend WithEvents cbMeteors As CheckBox
    Friend WithEvents Button10 As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents tbPath As TextBox
    Friend WithEvents cmbCam As ComboBox
    Friend WithEvents tbMultiplier As TextBox
    Friend WithEvents btnStop As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents btnStart As Button
    Friend WithEvents cbSaveImages As CheckBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents tbNightAgain As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblDayNight As Label
    Friend WithEvents tbExposureTime As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents tbDayGain As TextBox
    Friend WithEvents tbNightExp As TextBox
    Friend WithEvents tbDayTimeExp As TextBox
    Friend WithEvents btnStopWeb As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents cboNight As ComboBox
    Friend WithEvents cboDay As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cbUseDarks As CheckBox
    Friend WithEvents tbPort As TextBox
    Friend WithEvents btnStartWeb As Button
    Friend WithEvents tbGain As TextBox
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents btnSave As Button
    Friend WithEvents tbDarkCutOff As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents tbURL As TextBox
    Friend WithEvents lblURL As Label
    Friend WithEvents Image_timer As Timer
    Friend WithEvents Button1 As Button
End Class
