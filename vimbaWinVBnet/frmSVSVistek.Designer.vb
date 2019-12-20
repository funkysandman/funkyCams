<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSVSVistek
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
        Me.Button6 = New System.Windows.Forms.Button()
        Me.tbPort = New System.Windows.Forms.TextBox()
        Me.cbUseDarks = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboNight = New System.Windows.Forms.ComboBox()
        Me.cboDay = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.tbDayTimeExp = New System.Windows.Forms.TextBox()
        Me.tbNightExp = New System.Windows.Forms.TextBox()
        Me.tbDayGain = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbExposureTime = New System.Windows.Forms.TextBox()
        Me.lblDayNight = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbDayDgain = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.tbDayGamma = New System.Windows.Forms.TextBox()
        Me.tbNightGamma = New System.Windows.Forms.TextBox()
        Me.tbNightDgain = New System.Windows.Forms.TextBox()
        Me.tbNightAgain = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbSaveImages = New System.Windows.Forms.CheckBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Button3 = New System.Windows.Forms.Button()
        Me.tbMultiplier = New System.Windows.Forms.TextBox()
        Me.cmbCam = New System.Windows.Forms.ComboBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.tbPath = New System.Windows.Forms.TextBox()
        Me.cbMeteors = New System.Windows.Forms.CheckBox()
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtFps = New System.Windows.Forms.TextBox()
        Me.cbUseTrigger = New System.Windows.Forms.CheckBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tbUpper = New System.Windows.Forms.TextBox()
        Me.tbLower = New System.Windows.Forms.TextBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(20, 451)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(104, 23)
        Me.Button6.TabIndex = 8
        Me.Button6.Text = "Start webserver"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'tbPort
        '
        Me.tbPort.Location = New System.Drawing.Point(129, 451)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(50, 20)
        Me.tbPort.TabIndex = 9
        Me.tbPort.Text = "8050"
        '
        'cbUseDarks
        '
        Me.cbUseDarks.AutoSize = True
        Me.cbUseDarks.Location = New System.Drawing.Point(316, 453)
        Me.cbUseDarks.Name = "cbUseDarks"
        Me.cbUseDarks.Size = New System.Drawing.Size(110, 17)
        Me.cbUseDarks.TabIndex = 10
        Me.cbUseDarks.Text = "use darks at night"
        Me.cbUseDarks.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(182, 236)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 13)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "night"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(181, 182)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "day"
        '
        'cboNight
        '
        Me.cboNight.DisplayMember = "6"
        Me.cboNight.FormattingEnabled = True
        Me.cboNight.Items.AddRange(New Object() {"15", "16", "17", "18", "19", "20", "21", "22", "23"})
        Me.cboNight.Location = New System.Drawing.Point(184, 252)
        Me.cboNight.Name = "cboNight"
        Me.cboNight.Size = New System.Drawing.Size(109, 21)
        Me.cboNight.TabIndex = 14
        Me.cboNight.ValueMember = "6"
        '
        'cboDay
        '
        Me.cboDay.DisplayMember = "displayMember"
        Me.cboDay.FormattingEnabled = True
        Me.cboDay.Items.AddRange(New Object() {"4", "5", "6", "7", "8", "9"})
        Me.cboDay.Location = New System.Drawing.Point(184, 197)
        Me.cboDay.Name = "cboDay"
        Me.cboDay.Size = New System.Drawing.Size(109, 21)
        Me.cboDay.TabIndex = 13
        Me.cboDay.ValueMember = "displayMember"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(195, 202)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 13)
        Me.Label1.TabIndex = 12
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(184, 449)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(116, 22)
        Me.Button4.TabIndex = 17
        Me.Button4.Text = "stop web"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 10000
        '
        'tbDayTimeExp
        '
        Me.tbDayTimeExp.Location = New System.Drawing.Point(337, 201)
        Me.tbDayTimeExp.Name = "tbDayTimeExp"
        Me.tbDayTimeExp.Size = New System.Drawing.Size(45, 20)
        Me.tbDayTimeExp.TabIndex = 18
        Me.tbDayTimeExp.Text = "125"
        '
        'tbNightExp
        '
        Me.tbNightExp.Location = New System.Drawing.Point(337, 236)
        Me.tbNightExp.Name = "tbNightExp"
        Me.tbNightExp.Size = New System.Drawing.Size(45, 20)
        Me.tbNightExp.TabIndex = 19
        Me.tbNightExp.Text = "4996100"
        '
        'tbDayGain
        '
        Me.tbDayGain.Location = New System.Drawing.Point(280, 83)
        Me.tbDayGain.Name = "tbDayGain"
        Me.tbDayGain.Size = New System.Drawing.Size(47, 20)
        Me.tbDayGain.TabIndex = 22
        Me.tbDayGain.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(182, 86)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "analog gain"
        '
        'tbExposureTime
        '
        Me.tbExposureTime.Enabled = False
        Me.tbExposureTime.Location = New System.Drawing.Point(336, 290)
        Me.tbExposureTime.Name = "tbExposureTime"
        Me.tbExposureTime.Size = New System.Drawing.Size(45, 20)
        Me.tbExposureTime.TabIndex = 24
        Me.tbExposureTime.Text = "4996100"
        '
        'lblDayNight
        '
        Me.lblDayNight.AutoSize = True
        Me.lblDayNight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDayNight.Location = New System.Drawing.Point(182, 292)
        Me.lblDayNight.Name = "lblDayNight"
        Me.lblDayNight.Size = New System.Drawing.Size(35, 13)
        Me.lblDayNight.TabIndex = 25
        Me.lblDayNight.Text = "night"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(22, 23)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(142, 116)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 26
        Me.PictureBox1.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(182, 113)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 13)
        Me.Label5.TabIndex = 27
        Me.Label5.Text = "digital gain"
        '
        'tbDayDgain
        '
        Me.tbDayDgain.Location = New System.Drawing.Point(280, 109)
        Me.tbDayDgain.Name = "tbDayDgain"
        Me.tbDayDgain.Size = New System.Drawing.Size(47, 20)
        Me.tbDayDgain.TabIndex = 28
        Me.tbDayDgain.Text = "1"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(182, 142)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(91, 13)
        Me.Label6.TabIndex = 29
        Me.Label6.Text = "gamma correction"
        '
        'tbDayGamma
        '
        Me.tbDayGamma.Location = New System.Drawing.Point(280, 135)
        Me.tbDayGamma.Name = "tbDayGamma"
        Me.tbDayGamma.Size = New System.Drawing.Size(47, 20)
        Me.tbDayGamma.TabIndex = 30
        Me.tbDayGamma.Text = "1"
        '
        'tbNightGamma
        '
        Me.tbNightGamma.Location = New System.Drawing.Point(337, 135)
        Me.tbNightGamma.Name = "tbNightGamma"
        Me.tbNightGamma.Size = New System.Drawing.Size(47, 20)
        Me.tbNightGamma.TabIndex = 33
        Me.tbNightGamma.Text = "1.2"
        '
        'tbNightDgain
        '
        Me.tbNightDgain.Location = New System.Drawing.Point(337, 109)
        Me.tbNightDgain.Name = "tbNightDgain"
        Me.tbNightDgain.Size = New System.Drawing.Size(47, 20)
        Me.tbNightDgain.TabIndex = 32
        Me.tbNightDgain.Text = "1"
        '
        'tbNightAgain
        '
        Me.tbNightAgain.Location = New System.Drawing.Point(337, 83)
        Me.tbNightAgain.Name = "tbNightAgain"
        Me.tbNightAgain.Size = New System.Drawing.Size(47, 20)
        Me.tbNightAgain.TabIndex = 31
        Me.tbNightAgain.Text = "27"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(278, 56)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(24, 13)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "day"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(334, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 13)
        Me.Label8.TabIndex = 35
        Me.Label8.Text = "night"
        '
        'cbSaveImages
        '
        Me.cbSaveImages.AutoSize = True
        Me.cbSaveImages.Location = New System.Drawing.Point(206, 373)
        Me.cbSaveImages.Name = "cbSaveImages"
        Me.cbSaveImages.Size = New System.Drawing.Size(97, 17)
        Me.cbSaveImages.TabIndex = 36
        Me.cbSaveImages.Text = "Save snaphots"
        Me.cbSaveImages.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(184, 324)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(62, 23)
        Me.Button1.TabIndex = 37
        Me.Button1.Text = "Start snapshots"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(322, 324)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(64, 38)
        Me.Button2.TabIndex = 38
        Me.Button2.Text = "take darks"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Timer2
        '
        Me.Timer2.Interval = 1000
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(253, 324)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(62, 23)
        Me.Button3.TabIndex = 39
        Me.Button3.Text = "Stop"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'tbMultiplier
        '
        Me.tbMultiplier.Location = New System.Drawing.Point(160, 372)
        Me.tbMultiplier.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.tbMultiplier.Name = "tbMultiplier"
        Me.tbMultiplier.Size = New System.Drawing.Size(35, 20)
        Me.tbMultiplier.TabIndex = 40
        Me.tbMultiplier.Text = "1.0"
        '
        'cmbCam
        '
        Me.cmbCam.FormattingEnabled = True
        Me.cmbCam.Location = New System.Drawing.Point(184, 11)
        Me.cmbCam.Name = "cmbCam"
        Me.cmbCam.Size = New System.Drawing.Size(188, 21)
        Me.cmbCam.TabIndex = 41
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(312, 423)
        Me.Button10.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(36, 18)
        Me.Button10.TabIndex = 44
        Me.Button10.Text = "..."
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(14, 426)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(31, 13)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "path:"
        '
        'tbPath
        '
        Me.tbPath.Location = New System.Drawing.Point(48, 423)
        Me.tbPath.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.tbPath.Name = "tbPath"
        Me.tbPath.Size = New System.Drawing.Size(252, 20)
        Me.tbPath.TabIndex = 42
        Me.tbPath.Text = "c:\imageSVS"
        '
        'cbMeteors
        '
        Me.cbMeteors.AutoSize = True
        Me.cbMeteors.Location = New System.Drawing.Point(206, 396)
        Me.cbMeteors.Name = "cbMeteors"
        Me.cbMeteors.Size = New System.Drawing.Size(96, 17)
        Me.cbMeteors.TabIndex = 45
        Me.cbMeteors.Text = "detect meteors"
        Me.cbMeteors.UseVisualStyleBackColor = True
        '
        'Timer3
        '
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(319, 372)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(21, 13)
        Me.Label10.TabIndex = 49
        Me.Label10.Text = "fps"
        '
        'txtFps
        '
        Me.txtFps.Location = New System.Drawing.Point(322, 389)
        Me.txtFps.Name = "txtFps"
        Me.txtFps.Size = New System.Drawing.Size(48, 20)
        Me.txtFps.TabIndex = 48
        '
        'cbUseTrigger
        '
        Me.cbUseTrigger.AutoSize = True
        Me.cbUseTrigger.Location = New System.Drawing.Point(185, 36)
        Me.cbUseTrigger.Name = "cbUseTrigger"
        Me.cbUseTrigger.Size = New System.Drawing.Size(115, 17)
        Me.cbUseTrigger.TabIndex = 50
        Me.cbUseTrigger.Text = "use external trigger"
        Me.cbUseTrigger.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(127, 494)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 104
        Me.Label11.Text = "upper"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(10, 494)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(32, 13)
        Me.Label12.TabIndex = 103
        Me.Label12.Text = "lower"
        '
        'tbUpper
        '
        Me.tbUpper.Location = New System.Drawing.Point(165, 491)
        Me.tbUpper.Name = "tbUpper"
        Me.tbUpper.Size = New System.Drawing.Size(62, 20)
        Me.tbUpper.TabIndex = 102
        Me.tbUpper.Text = "16383"
        '
        'tbLower
        '
        Me.tbLower.Location = New System.Drawing.Point(48, 491)
        Me.tbLower.Name = "tbLower"
        Me.tbLower.Size = New System.Drawing.Size(62, 20)
        Me.tbLower.TabIndex = 101
        Me.tbLower.Text = "1000"
        '
        'frmSVSVistek
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(428, 523)
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
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.cbSaveImages)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbNightGamma)
        Me.Controls.Add(Me.tbNightDgain)
        Me.Controls.Add(Me.tbNightAgain)
        Me.Controls.Add(Me.tbDayGamma)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.tbDayDgain)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblDayNight)
        Me.Controls.Add(Me.tbExposureTime)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbDayGain)
        Me.Controls.Add(Me.tbNightExp)
        Me.Controls.Add(Me.tbDayTimeExp)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboNight)
        Me.Controls.Add(Me.cboDay)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbUseDarks)
        Me.Controls.Add(Me.tbPort)
        Me.Controls.Add(Me.Button6)
        Me.Name = "frmSVSVistek"
        Me.Text = "SVS Vistek camera"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents tbPort As System.Windows.Forms.TextBox
    Friend WithEvents cbUseDarks As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cboNight As System.Windows.Forms.ComboBox
    Friend WithEvents cboDay As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents tbDayTimeExp As System.Windows.Forms.TextBox
    Friend WithEvents tbNightExp As System.Windows.Forms.TextBox
    Friend WithEvents tbDayGain As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbExposureTime As System.Windows.Forms.TextBox
    Friend WithEvents lblDayNight As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents tbDayDgain As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbDayGamma As System.Windows.Forms.TextBox
    Friend WithEvents tbNightGamma As System.Windows.Forms.TextBox
    Friend WithEvents tbNightDgain As System.Windows.Forms.TextBox
    Friend WithEvents tbNightAgain As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbSaveImages As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Timer2 As Timer
    Friend WithEvents Button3 As Button
    Friend WithEvents tbMultiplier As TextBox
    Friend WithEvents cmbCam As ComboBox
    Friend WithEvents Button10 As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents tbPath As TextBox
    Friend WithEvents cbMeteors As CheckBox
    Friend WithEvents Timer3 As Timer
    Friend WithEvents Label10 As Label
    Friend WithEvents txtFps As TextBox
    Friend WithEvents cbUseTrigger As CheckBox
    Friend WithEvents Label11 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents tbUpper As TextBox
    Friend WithEvents tbLower As TextBox
End Class
