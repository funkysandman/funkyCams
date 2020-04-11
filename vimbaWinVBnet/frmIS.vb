Imports System.IO
Imports System.Runtime.InteropServices
Imports QCamManagedDriver
Imports TIS.Imaging

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmIS
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            'mCamList = New QCamM_CamListItem(9) {}
            'Dim listLen As UInteger = 10
            'QCam.QCamM_ListCameras(mCamList, listLen)

            'If (listLen > 0) AndAlso (mCamList(0).isOpen = 1) Then
            '    Debug.Print("closing camera")
            '    QCam.QCamM_CloseCamera(mhCamera)
            '    Debug.Print("camera closed")
            'End If
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmIS))
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbMeteors = New System.Windows.Forms.CheckBox()
        Me.txtFps = New System.Windows.Forms.TextBox()
        Me.cbSaveImages = New System.Windows.Forms.CheckBox()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.tbPath = New System.Windows.Forms.TextBox()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.tbPort = New System.Windows.Forms.TextBox()
        Me.tbMultiplier = New System.Windows.Forms.TextBox()
        Me.cbUseDarks = New System.Windows.Forms.CheckBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.tbNightAgain = New System.Windows.Forms.TextBox()
        Me.lblDayNight = New System.Windows.Forms.Label()
        Me.tbExposureTime = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.tbDayGain = New System.Windows.Forms.TextBox()
        Me.tbNightExp = New System.Windows.Forms.TextBox()
        Me.tbDayTimeExp = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cboNight = New System.Windows.Forms.ComboBox()
        Me.cboDay = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.tbGain = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.TimerAcquistionRate = New System.Windows.Forms.Timer(Me.components)
        Me.tbStatus = New System.Windows.Forms.TextBox()
        Me.tbLower = New System.Windows.Forms.TextBox()
        Me.tbUpper = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.IcImagingControl1 = New TIS.Imaging.ICImagingControl()
        CType(Me.IcImagingControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(664, 384)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(27, 17)
        Me.Label6.TabIndex = 74
        Me.Label6.Text = "fps"
        '
        'cbMeteors
        '
        Me.cbMeteors.AutoSize = True
        Me.cbMeteors.Location = New System.Drawing.Point(512, 407)
        Me.cbMeteors.Margin = New System.Windows.Forms.Padding(4)
        Me.cbMeteors.Name = "cbMeteors"
        Me.cbMeteors.Size = New System.Drawing.Size(124, 21)
        Me.cbMeteors.TabIndex = 73
        Me.cbMeteors.Text = "detect meteors"
        Me.cbMeteors.UseVisualStyleBackColor = True
        '
        'txtFps
        '
        Me.txtFps.Location = New System.Drawing.Point(668, 405)
        Me.txtFps.Margin = New System.Windows.Forms.Padding(4)
        Me.txtFps.Name = "txtFps"
        Me.txtFps.Size = New System.Drawing.Size(63, 22)
        Me.txtFps.TabIndex = 72
        '
        'cbSaveImages
        '
        Me.cbSaveImages.AutoSize = True
        Me.cbSaveImages.Location = New System.Drawing.Point(512, 379)
        Me.cbSaveImages.Margin = New System.Windows.Forms.Padding(4)
        Me.cbSaveImages.Name = "cbSaveImages"
        Me.cbSaveImages.Size = New System.Drawing.Size(109, 21)
        Me.cbSaveImages.TabIndex = 71
        Me.cbSaveImages.Text = "save images"
        Me.cbSaveImages.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Location = New System.Drawing.Point(427, 416)
        Me.Button10.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(48, 22)
        Me.Button10.TabIndex = 70
        Me.Button10.Text = "..."
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(27, 418)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 17)
        Me.Label5.TabIndex = 69
        Me.Label5.Text = "path:"
        '
        'tbPath
        '
        Me.tbPath.Location = New System.Drawing.Point(75, 416)
        Me.tbPath.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tbPath.Name = "tbPath"
        Me.tbPath.Size = New System.Drawing.Size(335, 22)
        Me.tbPath.TabIndex = 68
        Me.tbPath.Text = "c:\image_img_src"
        '
        'Button8
        '
        Me.Button8.Enabled = False
        Me.Button8.Location = New System.Drawing.Point(639, 336)
        Me.Button8.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(103, 31)
        Me.Button8.TabIndex = 63
        Me.Button8.Text = "stop capture"
        Me.Button8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(515, 336)
        Me.Button7.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(103, 31)
        Me.Button7.TabIndex = 62
        Me.Button7.Text = "start capture"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Enabled = False
        Me.Button6.Location = New System.Drawing.Point(347, 458)
        Me.Button6.Margin = New System.Windows.Forms.Padding(4)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(151, 28)
        Me.Button6.TabIndex = 61
        Me.Button6.Text = "stop webserver"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'tbPort
        '
        Me.tbPort.Location = New System.Drawing.Point(291, 462)
        Me.tbPort.Margin = New System.Windows.Forms.Padding(4)
        Me.tbPort.Name = "tbPort"
        Me.tbPort.Size = New System.Drawing.Size(47, 22)
        Me.tbPort.TabIndex = 60
        Me.tbPort.Text = "8070"
        '
        'tbMultiplier
        '
        Me.tbMultiplier.Location = New System.Drawing.Point(668, 464)
        Me.tbMultiplier.Margin = New System.Windows.Forms.Padding(4)
        Me.tbMultiplier.Name = "tbMultiplier"
        Me.tbMultiplier.Size = New System.Drawing.Size(47, 22)
        Me.tbMultiplier.TabIndex = 59
        Me.tbMultiplier.Text = "1.0"
        '
        'cbUseDarks
        '
        Me.cbUseDarks.AutoSize = True
        Me.cbUseDarks.Location = New System.Drawing.Point(516, 475)
        Me.cbUseDarks.Margin = New System.Windows.Forms.Padding(4)
        Me.cbUseDarks.Name = "cbUseDarks"
        Me.cbUseDarks.Size = New System.Drawing.Size(143, 21)
        Me.cbUseDarks.TabIndex = 54
        Me.cbUseDarks.Text = "use darks at night"
        Me.cbUseDarks.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(132, 459)
        Me.Button5.Margin = New System.Windows.Forms.Padding(4)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(151, 28)
        Me.Button5.TabIndex = 53
        Me.Button5.Text = "start webserver"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(531, 250)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 17)
        Me.Label1.TabIndex = 52
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(516, 439)
        Me.Button2.Margin = New System.Windows.Forms.Padding(4)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(104, 28)
        Me.Button2.TabIndex = 49
        Me.Button2.Text = "take darks"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(695, 11)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(39, 17)
        Me.Label8.TabIndex = 94
        Me.Label8.Text = "night"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(620, 11)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(31, 17)
        Me.Label7.TabIndex = 93
        Me.Label7.Text = "day"
        '
        'tbNightAgain
        '
        Me.tbNightAgain.Location = New System.Drawing.Point(699, 44)
        Me.tbNightAgain.Margin = New System.Windows.Forms.Padding(4)
        Me.tbNightAgain.Name = "tbNightAgain"
        Me.tbNightAgain.Size = New System.Drawing.Size(61, 22)
        Me.tbNightAgain.TabIndex = 90
        Me.tbNightAgain.Text = "32"
        '
        'lblDayNight
        '
        Me.lblDayNight.AutoSize = True
        Me.lblDayNight.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDayNight.Location = New System.Drawing.Point(492, 302)
        Me.lblDayNight.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDayNight.Name = "lblDayNight"
        Me.lblDayNight.Size = New System.Drawing.Size(26, 17)
        Me.lblDayNight.TabIndex = 85
        Me.lblDayNight.Text = "---"
        '
        'tbExposureTime
        '
        Me.tbExposureTime.Enabled = False
        Me.tbExposureTime.Location = New System.Drawing.Point(699, 299)
        Me.tbExposureTime.Margin = New System.Windows.Forms.Padding(4)
        Me.tbExposureTime.Name = "tbExposureTime"
        Me.tbExposureTime.Size = New System.Drawing.Size(59, 22)
        Me.tbExposureTime.TabIndex = 84
        Me.tbExposureTime.Text = "5000"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(492, 48)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 17)
        Me.Label4.TabIndex = 83
        Me.Label4.Text = "analog gain"
        '
        'tbDayGain
        '
        Me.tbDayGain.Location = New System.Drawing.Point(624, 44)
        Me.tbDayGain.Margin = New System.Windows.Forms.Padding(4)
        Me.tbDayGain.Name = "tbDayGain"
        Me.tbDayGain.Size = New System.Drawing.Size(61, 22)
        Me.tbDayGain.TabIndex = 82
        Me.tbDayGain.Text = "1"
        '
        'tbNightExp
        '
        Me.tbNightExp.Location = New System.Drawing.Point(699, 252)
        Me.tbNightExp.Margin = New System.Windows.Forms.Padding(4)
        Me.tbNightExp.Name = "tbNightExp"
        Me.tbNightExp.Size = New System.Drawing.Size(59, 22)
        Me.tbNightExp.TabIndex = 81
        Me.tbNightExp.Text = "5000"
        '
        'tbDayTimeExp
        '
        Me.tbDayTimeExp.Location = New System.Drawing.Point(697, 185)
        Me.tbDayTimeExp.Margin = New System.Windows.Forms.Padding(4)
        Me.tbDayTimeExp.Name = "tbDayTimeExp"
        Me.tbDayTimeExp.Size = New System.Drawing.Size(59, 22)
        Me.tbDayTimeExp.TabIndex = 80
        Me.tbDayTimeExp.Text = ".225"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(492, 231)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(39, 17)
        Me.Label9.TabIndex = 79
        Me.Label9.Text = "night"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(491, 166)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(31, 17)
        Me.Label10.TabIndex = 78
        Me.Label10.Text = "day"
        '
        'cboNight
        '
        Me.cboNight.DisplayMember = "6"
        Me.cboNight.FormattingEnabled = True
        Me.cboNight.Items.AddRange(New Object() {"15", "16", "17", "18", "19", "20", "21", "22", "23"})
        Me.cboNight.Location = New System.Drawing.Point(495, 252)
        Me.cboNight.Margin = New System.Windows.Forms.Padding(4)
        Me.cboNight.Name = "cboNight"
        Me.cboNight.Size = New System.Drawing.Size(144, 24)
        Me.cboNight.TabIndex = 77
        Me.cboNight.ValueMember = "6"
        '
        'cboDay
        '
        Me.cboDay.DisplayMember = "displayMember"
        Me.cboDay.FormattingEnabled = True
        Me.cboDay.Items.AddRange(New Object() {"4", "5", "6", "7", "8", "9"})
        Me.cboDay.Location = New System.Drawing.Point(496, 185)
        Me.cboDay.Margin = New System.Windows.Forms.Padding(4)
        Me.cboDay.Name = "cboDay"
        Me.cboDay.Size = New System.Drawing.Size(144, 24)
        Me.cboDay.TabIndex = 76
        Me.cboDay.ValueMember = "displayMember"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(509, 190)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(0, 17)
        Me.Label11.TabIndex = 75
        '
        'tbGain
        '
        Me.tbGain.Location = New System.Drawing.Point(579, 299)
        Me.tbGain.Margin = New System.Windows.Forms.Padding(4)
        Me.tbGain.Name = "tbGain"
        Me.tbGain.Size = New System.Drawing.Size(61, 22)
        Me.tbGain.TabIndex = 95
        Me.tbGain.Text = "4000"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'Timer2
        '
        Me.Timer2.Interval = 1000
        '
        'Timer3
        '
        '
        'TimerAcquistionRate
        '
        Me.TimerAcquistionRate.Interval = 1000
        '
        'tbStatus
        '
        Me.tbStatus.Location = New System.Drawing.Point(493, 124)
        Me.tbStatus.Margin = New System.Windows.Forms.Padding(4)
        Me.tbStatus.Name = "tbStatus"
        Me.tbStatus.Size = New System.Drawing.Size(263, 22)
        Me.tbStatus.TabIndex = 96
        '
        'tbLower
        '
        Me.tbLower.Location = New System.Drawing.Point(135, 512)
        Me.tbLower.Margin = New System.Windows.Forms.Padding(4)
        Me.tbLower.Name = "tbLower"
        Me.tbLower.Size = New System.Drawing.Size(81, 22)
        Me.tbLower.TabIndex = 97
        Me.tbLower.Text = "10"
        '
        'tbUpper
        '
        Me.tbUpper.Location = New System.Drawing.Point(291, 512)
        Me.tbUpper.Margin = New System.Windows.Forms.Padding(4)
        Me.tbUpper.Name = "tbUpper"
        Me.tbUpper.Size = New System.Drawing.Size(81, 22)
        Me.tbUpper.TabIndex = 98
        Me.tbUpper.Text = "4096"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(84, 516)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 17)
        Me.Label2.TabIndex = 99
        Me.Label2.Text = "lower"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(240, 516)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 17)
        Me.Label3.TabIndex = 100
        Me.Label3.Text = "upper"
        '
        'IcImagingControl1
        '
        Me.IcImagingControl1.AutoScroll = True
        Me.IcImagingControl1.AutoScrollMinSize = New System.Drawing.Size(150, 150)
        Me.IcImagingControl1.BackColor = System.Drawing.Color.White
        Me.IcImagingControl1.DeviceListChangedExecutionMode = TIS.Imaging.EventExecutionMode.Invoke
        Me.IcImagingControl1.DeviceLostExecutionMode = TIS.Imaging.EventExecutionMode.AsyncInvoke
        Me.IcImagingControl1.DeviceState = resources.GetString("IcImagingControl1.DeviceState")
        Me.IcImagingControl1.ImageAvailableExecutionMode = TIS.Imaging.EventExecutionMode.AsyncInvoke
        Me.IcImagingControl1.LiveCaptureContinuous = True
        Me.IcImagingControl1.LiveDisplayPosition = New System.Drawing.Point(0, 0)
        Me.IcImagingControl1.Location = New System.Drawing.Point(31, 21)
        Me.IcImagingControl1.Margin = New System.Windows.Forms.Padding(4)
        Me.IcImagingControl1.MemoryCurrentGrabberColorformat = TIS.Imaging.ICImagingControlColorformats.ICY16
        Me.IcImagingControl1.Name = "IcImagingControl1"
        Me.IcImagingControl1.ScrollbarsEnabled = True
        Me.IcImagingControl1.Size = New System.Drawing.Size(444, 379)
        Me.IcImagingControl1.TabIndex = 101
        '
        'frmIS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 550)
        Me.Controls.Add(Me.IcImagingControl1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.tbUpper)
        Me.Controls.Add(Me.tbLower)
        Me.Controls.Add(Me.tbStatus)
        Me.Controls.Add(Me.tbGain)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.tbNightAgain)
        Me.Controls.Add(Me.lblDayNight)
        Me.Controls.Add(Me.tbExposureTime)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.tbDayGain)
        Me.Controls.Add(Me.tbNightExp)
        Me.Controls.Add(Me.tbDayTimeExp)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.cboNight)
        Me.Controls.Add(Me.cboDay)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cbMeteors)
        Me.Controls.Add(Me.txtFps)
        Me.Controls.Add(Me.cbSaveImages)
        Me.Controls.Add(Me.Button10)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.tbPath)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.tbPort)
        Me.Controls.Add(Me.tbMultiplier)
        Me.Controls.Add(Me.cbUseDarks)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button2)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmIS"
        Me.Text = "frmImagingSource"
        CType(Me.IcImagingControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label6 As Label
    Friend WithEvents cbMeteors As CheckBox
    Friend WithEvents txtFps As TextBox
    Friend WithEvents cbSaveImages As CheckBox
    Friend WithEvents Button10 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents tbPath As TextBox
    Friend WithEvents Button8 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents tbPort As TextBox
    Friend WithEvents tbMultiplier As TextBox
    Friend WithEvents cbUseDarks As CheckBox
    Friend WithEvents Button5 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents tbNightAgain As TextBox
    Friend WithEvents lblDayNight As Label
    Friend WithEvents tbExposureTime As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents tbDayGain As TextBox
    Friend WithEvents tbNightExp As TextBox
    Friend WithEvents tbDayTimeExp As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents cboNight As ComboBox
    Friend WithEvents cboDay As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents tbGain As TextBox
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Timer2 As Timer
    Friend WithEvents Timer3 As Timer
    Friend WithEvents TimerAcquistionRate As Timer
    Friend WithEvents tbStatus As TextBox
    Friend WithEvents tbLower As TextBox
    Friend WithEvents tbUpper As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label



    Private Sub IcImagingControl1_ImageAvailable(sender As Object, e As ICImagingControl.ImageAvailableEventArgs) Handles IcImagingControl1.ImageAvailable
        'image is here...

        Debug.Print("image arrived {0}", e.frameNumber)
        If m_pics Is Nothing Then
            m_pics = New RingBitmap(5)
        End If
        Dim img() As Byte
        Dim iBuffer As ImageBuffer
        Dim pixelValue As UShort
        Dim darkBuffer() As UShort

        iBuffer = IcImagingControl1.ImageActiveBuffer
        iWidth = e.ImageBuffer.Size.Width
        iHeight = e.ImageBuffer.Size.Height

        ReDim img(e.ImageBuffer.ActualDataSize)
        ' IcImagingControl1.VideoFormat = "Y16 (3072x2048)"

        'copy image buffer into byte array
        Marshal.Copy(e.ImageBuffer.GetImageDataPtr, img, 0, e.ImageBuffer.ActualDataSize)

        File.WriteAllBytes("ISraw.raw", img)

        'IcImagingControl1.LiveSuspend()
        'IcImagingControl1.VideoFormat = "RGB24 (3072x2048)"
        Dim newPixelValue As UInt16
        Dim dPixelValue As UInt16
        Dim multiplier As Decimal

        multiplier = Val(tbMultiplier.Text)
        'subtract dark here while in 16bit gray mode
        If cbUseDarks.Checked Then
            If dark Is Nothing Then
                loadMasterDark()
            End If
            'go through image buffer
            For x = 0 To iWidth - 1
                For y = 0 To iHeight - 1
                    pixelValue = ReadY16(iBuffer, y, x)
                    dPixelValue = dark(y * iBuffer.PixelPerLine + x)
                    If dPixelValue / multiplier > 60000 Then
                        newPixelValue = Math.Max(0, CInt(pixelValue) - CInt(dPixelValue))
                        WriteY16(iBuffer, y, x, newPixelValue)
                    End If


                Next
            Next

        End If

        'debayer to rgb24
        Dim mTransformImage As BGAPI2.Image = Nothing
        Dim mImage As BGAPI2.Image = Nothing
        ' Dim buff As BGAPI2.Buffer = New BGAPI2.Buffer()
        Dim imgProcessor As New BGAPI2.ImageProcessor()



        Dim outImage(iWidth * iHeight * 3) As Byte
        mImage = imgProcessor.CreateImage(iWidth, iHeight, "BayerBG16", e.ImageBuffer.GetImageDataPtr, e.ImageBuffer.ActualDataSize)

        mTransformImage = imgProcessor.CreateTransformedImage(mImage, "BayerBG12")

        mTransformImage = imgProcessor.CreateTransformedImage(mTransformImage, "RGB8")



        Marshal.Copy(mTransformImage.Buffer, outImage, 0, iWidth * iHeight * 3 - 1)



        ' Marshal.Copy(e.ImageBuffer.GetImageDataPtr, img, 0, e.ImageBuffer.ActualDataSize)




        m_pics.FillNextBitmap(outImage)

        Dim filename As String

        Dim folderName = String.Format("{0:yyyy-MMM-dd}", DateTime.Now)
        filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.jpg", "imgIS_", DateTime.Now)
        filename = Path.Combine(tbPath.Text, folderName, filename)



        If cbMeteors.Checked And lblDayNight.Text.ToLower = "night" Then
            ' md.examine(bm, filename)
            'call azure service
            Dim ms As New MemoryStream()
            ' convertedImage.ConvertToWriteAbleBitmap()
            Dim b As Bitmap
            b = getLastImage()

            b.Save(ms, myImageCodecInfo, myEncoderParameters)
            b.Dispose()

            Dim contents = ms.ToArray()
            Dim qe As New queueEntry
            qe.img = contents
            qe.filename = Path.GetFileName(filename)
            qe.dateTaken = Now
            qe.cameraID = "Imaging Source"
            qe.width = e.ImageBuffer.Size.Width
            qe.height = e.ImageBuffer.Size.Height
            If myDetectionQueue.Count < 10 Then
                myDetectionQueue.Enqueue(qe)

            End If

            ms.Close()

        End If
        If cbSaveImages.Checked = True Then
            System.IO.Directory.CreateDirectory(Path.Combine(tbPath.Text, folderName))
            Dim x As Bitmap
            x = getLastImage()

            x.Save(filename, myImageCodecInfo, myEncoderParameters)
            x.Dispose()

        End If



    End Sub

    Private Sub IcImagingControl1_Load(sender As Object, e As EventArgs) Handles IcImagingControl1.Load

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'make 10 darks and average them

        'setup camera for darks
        Dim AbsValItf As TIS.Imaging.VCDAbsoluteValueProperty

        Dim buffers() As ImageBuffer
        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = tbExposureTime.Text / 1000

        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Gain + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = tbGain.Text


        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Saturation + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = 125
        Dim fh As FrameHandlerSink

        fh = IcImagingControl1.Sink

        fh.SnapMode = True
        Dim numDarks As Integer = 10
        ReDim buffers(numDarks)
        Dim b As Integer
        Dim dBuffer() As UInt16
        Dim pixelValue As UInt32

        For x = 0 To numDarks - 1
            'snap image
            IcImagingControl1.MemorySnapImage()
            buffers(x) = IcImagingControl1.ImageActiveBuffer
        Next
        ReDim dBuffer(IcImagingControl1.ImageActiveBuffer.ActualDataSize)
        fh.SnapMode = False
        For x = 0 To buffers(0).Size.Width - 1
            For y = 0 To buffers(0).Size.Height - 1
                pixelValue = 0
                For b = 0 To numDarks - 1
                    pixelValue = pixelValue + ReadY16(buffers(0), y, x)
                Next
                pixelValue = pixelValue / numDarks
                dBuffer(y * buffers(0).PixelPerLine + x) = pixelValue
            Next
        Next
        Dim filename As String



        filename = String.Format("masterDark{0}.raw", IcImagingControl1.Device)
        Dim fStream As New FileStream(filename, FileMode.Create)
        Dim bWriter As New BinaryWriter(fStream)
        For i = 0 To dBuffer.Length - 1
            bWriter.Write(dBuffer(i))
        Next
        fStream.Close()
        bWriter.Close()

    End Sub

    Private Sub tbMultiplier_TextChanged(sender As Object, e As EventArgs) Handles tbMultiplier.TextChanged
        loadMasterDark()
    End Sub
End Class
