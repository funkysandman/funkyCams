<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmToupcam
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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.cbUseDarks = New System.Windows.Forms.CheckBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.tbMultiplier = New System.Windows.Forms.TextBox()
        Me.tbPort = New System.Windows.Forms.TextBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.cbCam = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.cbSaveImages = New System.Windows.Forms.CheckBox()
        Me.tbMessage = New System.Windows.Forms.TextBox()
        Me.txtFps = New System.Windows.Forms.TextBox()
        Me.cbMeteors = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.tbGain = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.tbNightAgain = New System.Windows.Forms.TextBox()
        Me.lblDayNight = New System.Windows.Forms.Label()
        Me.tbExposureTime = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.tbDayGain = New System.Windows.Forms.TextBox()
        Me.tbNightExp = New System.Windows.Forms.TextBox()
        Me.tbDayTimeExp = New System.Windows.Forms.TextBox()
        Me.cboNight = New System.Windows.Forms.ComboBox()
        Me.cboDay = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(49, 37)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(449, 378)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(537, 557)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 28)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "take darks"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(551, 368)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 17)
        Me.Label1.TabIndex = 5
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(157, 474)
        Me.Button5.Margin = New System.Windows.Forms.Padding(4)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(151, 28)
        Me.Button5.TabIndex = 6
        Me.Button5.Text = "start webserver"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'cbUseDarks
        '
        Me.cbUseDarks.AutoSize = True
        Me.cbUseDarks.Location = New System.Drawing.Point(537, 593)
        Me.cbUseDarks.Margin = New System.Windows.Forms.Padding(4)
        Me.cbUseDarks.Name = "cbUseDarks"
        Me.cbUseDarks.Size = New System.Drawing.Size(143, 21)
        Me.cbUseDarks.TabIndex = 7
        Me.cbUseDarks.Text = "use darks at night"
        Me.cbUseDarks.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 10000
        '
        'tbMultiplier
        '
        Me.tbMultiplier.Location = New System.Drawing.Point(689, 582)
        Me.tbMultiplier.Margin = New System.Windows.Forms.Padding(4)
        Me.tbMultiplier.Name = "tbMultiplier"
        Me.tbMultiplier.Size = New System.Drawing.Size(47, 22)
        Me.tbMultiplier.TabIndex = 12
        Me.tbMultiplier.Text = "1.0"
        '
        'tbPort
        '
        Me.tbPort.Location = New System.Drawing.Point(316, 476)
        Me.tbPort.Margin = New System.Windows.Forms.Padding(4)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(47, 22)
        Me.tbPort.TabIndex = 13
        Me.tbPort.Text = "8070"
        '
        'Button6
        '
        Me.Button6.Enabled = False
        Me.Button6.Location = New System.Drawing.Point(372, 473)
        Me.Button6.Margin = New System.Windows.Forms.Padding(4)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(151, 28)
        Me.Button6.TabIndex = 14
        Me.Button6.Text = "stop webserver"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(535, 454)
        Me.Button7.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(103, 31)
        Me.Button7.TabIndex = 15
        Me.Button7.Text = "start capture"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Enabled = False
        Me.Button8.Location = New System.Drawing.Point(660, 454)
        Me.Button8.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(103, 31)
        Me.Button8.TabIndex = 16
        Me.Button8.Text = "stop capture"
        Me.Button8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Timer3
        '
        Me.Timer3.Interval = 500
        '
        'cbCam
        '
        Me.cbCam.FormattingEnabled = True
        Me.cbCam.Location = New System.Drawing.Point(636, 83)
        Me.cbCam.Margin = New System.Windows.Forms.Padding(4)
        Me.cbCam.Name = "cbCam"
        Me.cbCam.Size = New System.Drawing.Size(207, 24)
        Me.cbCam.TabIndex = 17
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(534, 90)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 17)
        Me.Label4.TabIndex = 18
        Me.Label4.Text = "camera"
        '
        'tbPath
        '
        Me.tbPath.Location = New System.Drawing.Point(99, 431)
        Me.tbPath.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tbPath.Name = "tbPath"
        Me.tbPath.Size = New System.Drawing.Size(335, 22)
        Me.tbPath.TabIndex = 21
        Me.tbPath.Text = "c:\image_toupcam"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(52, 434)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 17)
        Me.Label5.TabIndex = 22
        Me.Label5.Text = "path:"
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(451, 431)
        Me.Button10.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(48, 22)
        Me.Button10.TabIndex = 23
        Me.Button10.Text = "..."
        Me.Button10.UseVisualStyleBackColor = True
        '
        'cbSaveImages
        '
        Me.cbSaveImages.AutoSize = True
        Me.cbSaveImages.Location = New System.Drawing.Point(533, 497)
        Me.cbSaveImages.Margin = New System.Windows.Forms.Padding(4)
        Me.cbSaveImages.Name = "cbSaveImages"
        Me.cbSaveImages.Size = New System.Drawing.Size(109, 21)
        Me.cbSaveImages.TabIndex = 24
        Me.cbSaveImages.Text = "save images"
        Me.cbSaveImages.UseVisualStyleBackColor = True
        '
        'tbMessage
        '
        Me.tbMessage.Location = New System.Drawing.Point(51, 526)
        Me.tbMessage.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tbMessage.Multiline = True
        Me.tbMessage.Name = "tbMessage"
        Me.tbMessage.Size = New System.Drawing.Size(447, 93)
        Me.tbMessage.TabIndex = 25
        '
        'txtFps
        '
        Me.txtFps.Location = New System.Drawing.Point(689, 523)
        Me.txtFps.Margin = New System.Windows.Forms.Padding(4)
        Me.txtFps.Name = "txtFps"
        Me.txtFps.Size = New System.Drawing.Size(63, 22)
        Me.txtFps.TabIndex = 26
        '
        'cbMeteors
        '
        Me.cbMeteors.AutoSize = True
        Me.cbMeteors.Location = New System.Drawing.Point(533, 525)
        Me.cbMeteors.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMeteors.Name = "cbMeteors"
        Me.cbMeteors.Size = New System.Drawing.Size(124, 21)
        Me.cbMeteors.TabIndex = 46
        Me.cbMeteors.Text = "detect meteors"
        Me.cbMeteors.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(685, 502)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(27, 17)
        Me.Label6.TabIndex = 47
        Me.Label6.Text = "fps"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(533, 373)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(119, 17)
        Me.Label11.TabIndex = 128
        Me.Label11.Text = "current exposure:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(533, 336)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(88, 17)
        Me.Label7.TabIndex = 127
        Me.Label7.Text = "current gain:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(533, 300)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(96, 17)
        Me.Label8.TabIndex = 126
        Me.Label8.Text = "current mode:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(533, 232)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(70, 17)
        Me.Label9.TabIndex = 125
        Me.Label9.Text = "exposure:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(533, 194)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(38, 17)
        Me.Label10.TabIndex = 124
        Me.Label10.Text = "time:"
        '
        'tbGain
        '
        Me.tbGain.Location = New System.Drawing.Point(666, 332)
        Me.tbGain.Margin = New System.Windows.Forms.Padding(4)
        Me.tbGain.Name = "tbGain"
        Me.tbGain.Size = New System.Drawing.Size(72, 22)
        Me.tbGain.TabIndex = 123
        Me.tbGain.Text = "1000"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(801, 133)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(39, 17)
        Me.Label12.TabIndex = 122
        Me.Label12.Text = "night"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(633, 133)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(31, 17)
        Me.Label13.TabIndex = 121
        Me.Label13.Text = "day"
        '
        'tbNightAgain
        '
        Me.tbNightAgain.Location = New System.Drawing.Point(778, 158)
        Me.tbNightAgain.Margin = New System.Windows.Forms.Padding(4)
        Me.tbNightAgain.Name = "tbNightAgain"
        Me.tbNightAgain.Size = New System.Drawing.Size(71, 22)
        Me.tbNightAgain.TabIndex = 120
        Me.tbNightAgain.Text = "1000"
        '
        'lblDayNight
        '
        Me.lblDayNight.AutoSize = True
        Me.lblDayNight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDayNight.Location = New System.Drawing.Point(668, 300)
        Me.lblDayNight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDayNight.Name = "lblDayNight"
        Me.lblDayNight.Size = New System.Drawing.Size(44, 17)
        Me.lblDayNight.TabIndex = 119
        Me.lblDayNight.Text = "night"
        '
        'tbExposureTime
        '
        Me.tbExposureTime.Enabled = False
        Me.tbExposureTime.Location = New System.Drawing.Point(666, 369)
        Me.tbExposureTime.Margin = New System.Windows.Forms.Padding(4)
        Me.tbExposureTime.Name = "tbExposureTime"
        Me.tbExposureTime.Size = New System.Drawing.Size(72, 22)
        Me.tbExposureTime.TabIndex = 118
        Me.tbExposureTime.Text = "5000000"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(533, 162)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(86, 17)
        Me.Label15.TabIndex = 117
        Me.Label15.Text = "analog gain:"
        '
        'tbDayGain
        '
        Me.tbDayGain.Location = New System.Drawing.Point(634, 158)
        Me.tbDayGain.Margin = New System.Windows.Forms.Padding(4)
        Me.tbDayGain.Name = "tbDayGain"
        Me.tbDayGain.Size = New System.Drawing.Size(61, 22)
        Me.tbDayGain.TabIndex = 116
        Me.tbDayGain.Text = "100"
        '
        'tbNightExp
        '
        Me.tbNightExp.Location = New System.Drawing.Point(781, 224)
        Me.tbNightExp.Margin = New System.Windows.Forms.Padding(4)
        Me.tbNightExp.Name = "tbNightExp"
        Me.tbNightExp.Size = New System.Drawing.Size(68, 22)
        Me.tbNightExp.TabIndex = 115
        Me.tbNightExp.Text = "5000000"
        '
        'tbDayTimeExp
        '
        Me.tbDayTimeExp.Location = New System.Drawing.Point(636, 224)
        Me.tbDayTimeExp.Margin = New System.Windows.Forms.Padding(4)
        Me.tbDayTimeExp.Name = "tbDayTimeExp"
        Me.tbDayTimeExp.Size = New System.Drawing.Size(59, 22)
        Me.tbDayTimeExp.TabIndex = 114
        Me.tbDayTimeExp.Text = "125"
        '
        'cboNight
        '
        Me.cboNight.DisplayMember = "6"
        Me.cboNight.FormattingEnabled = True
        Me.cboNight.Items.AddRange(New Object() {"15", "16", "17", "18", "19", "20", "21", "22", "23"})
        Me.cboNight.Location = New System.Drawing.Point(778, 190)
        Me.cboNight.Margin = New System.Windows.Forms.Padding(4)
        Me.cboNight.Name = "cboNight"
        Me.cboNight.Size = New System.Drawing.Size(71, 24)
        Me.cboNight.TabIndex = 113
        Me.cboNight.ValueMember = "6"
        '
        'cboDay
        '
        Me.cboDay.DisplayMember = "displayMember"
        Me.cboDay.FormattingEnabled = True
        Me.cboDay.Items.AddRange(New Object() {"4", "5", "6", "7", "8", "9"})
        Me.cboDay.Location = New System.Drawing.Point(634, 190)
        Me.cboDay.Margin = New System.Windows.Forms.Padding(4)
        Me.cboDay.Name = "cboDay"
        Me.cboDay.Size = New System.Drawing.Size(61, 24)
        Me.cboDay.TabIndex = 112
        Me.cboDay.ValueMember = "displayMember"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(636, 300)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(0, 17)
        Me.Label16.TabIndex = 111
        '
        'frmToupcam
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(885, 647)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.tbGain)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.tbNightAgain)
        Me.Controls.Add(Me.lblDayNight)
        Me.Controls.Add(Me.tbExposureTime)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.tbDayGain)
        Me.Controls.Add(Me.tbNightExp)
        Me.Controls.Add(Me.tbDayTimeExp)
        Me.Controls.Add(Me.cboNight)
        Me.Controls.Add(Me.cboDay)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cbMeteors)
        Me.Controls.Add(Me.txtFps)
        Me.Controls.Add(Me.tbMessage)
        Me.Controls.Add(Me.cbSaveImages)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbPath)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cbCam)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.tbPort)
        Me.Controls.Add(Me.tbMultiplier)
        Me.Controls.Add(Me.cbUseDarks)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmToupcam"
        Me.Text = "GigE camera"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents cbUseDarks As System.Windows.Forms.CheckBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents tbMultiplier As System.Windows.Forms.TextBox
    Friend WithEvents tbPort As System.Windows.Forms.TextBox
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents Timer3 As Timer
    Friend WithEvents cbCam As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents tbPath As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents Button10 As Button
    Friend WithEvents cbSaveImages As CheckBox
    Friend WithEvents tbMessage As TextBox
    Friend WithEvents txtFps As TextBox
    Friend WithEvents cbMeteors As CheckBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents tbGain As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents tbNightAgain As TextBox
    Friend WithEvents lblDayNight As Label
    Friend WithEvents tbExposureTime As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents tbDayGain As TextBox
    Friend WithEvents tbNightExp As TextBox
    Friend WithEvents tbDayTimeExp As TextBox
    Friend WithEvents cboNight As ComboBox
    Friend WithEvents cboDay As ComboBox
    Friend WithEvents Label16 As Label
End Class
