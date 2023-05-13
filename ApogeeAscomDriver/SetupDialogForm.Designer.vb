<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SetupDialogForm
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.chkTrace = New System.Windows.Forms.CheckBox()
        Me.ComboBoxComPort = New System.Windows.Forms.ComboBox()
        Me.tbXwidth = New System.Windows.Forms.TextBox()
        Me.tbYheight = New System.Windows.Forms.TextBox()
        Me.tbYstart = New System.Windows.Forms.TextBox()
        Me.tbXstart = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cbUseROI = New System.Windows.Forms.CheckBox()
        Me.btnTemp = New System.Windows.Forms.Button()
        Me.btnIO = New System.Windows.Forms.Button()
        Me.btnLED = New System.Windows.Forms.Button()
        Me.tbGain1 = New System.Windows.Forms.TextBox()
        Me.tbOffset1 = New System.Windows.Forms.TextBox()
        Me.btnSetGain1 = New System.Windows.Forms.Button()
        Me.btnSetOffset1 = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(314, 347)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Image = Global.ASCOM.Apogee.My.Resources.Resources.ASCOM
        Me.PictureBox1.Location = New System.Drawing.Point(411, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 56)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(309, 135)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(58, 13)
        Me.label2.TabIndex = 7
        Me.label2.Text = "Comm Port"
        '
        'chkTrace
        '
        Me.chkTrace.AutoSize = True
        Me.chkTrace.Location = New System.Drawing.Point(373, 159)
        Me.chkTrace.Name = "chkTrace"
        Me.chkTrace.Size = New System.Drawing.Size(69, 17)
        Me.chkTrace.TabIndex = 8
        Me.chkTrace.Text = "Trace on"
        Me.chkTrace.UseVisualStyleBackColor = True
        '
        'ComboBoxComPort
        '
        Me.ComboBoxComPort.FormattingEnabled = True
        Me.ComboBoxComPort.Location = New System.Drawing.Point(373, 132)
        Me.ComboBoxComPort.Name = "ComboBoxComPort"
        Me.ComboBoxComPort.Size = New System.Drawing.Size(84, 21)
        Me.ComboBoxComPort.TabIndex = 9
        '
        'tbXwidth
        '
        Me.tbXwidth.Location = New System.Drawing.Point(112, 48)
        Me.tbXwidth.Name = "tbXwidth"
        Me.tbXwidth.Size = New System.Drawing.Size(87, 20)
        Me.tbXwidth.TabIndex = 10
        Me.tbXwidth.Text = "4096"
        '
        'tbYheight
        '
        Me.tbYheight.Location = New System.Drawing.Point(112, 103)
        Me.tbYheight.Name = "tbYheight"
        Me.tbYheight.Size = New System.Drawing.Size(87, 20)
        Me.tbYheight.TabIndex = 11
        Me.tbYheight.Text = "4096"
        '
        'tbYstart
        '
        Me.tbYstart.Location = New System.Drawing.Point(19, 103)
        Me.tbYstart.Name = "tbYstart"
        Me.tbYstart.Size = New System.Drawing.Size(87, 20)
        Me.tbYstart.TabIndex = 12
        Me.tbYstart.Text = "0"
        '
        'tbXstart
        '
        Me.tbXstart.Location = New System.Drawing.Point(19, 48)
        Me.tbXstart.Name = "tbXstart"
        Me.tbXstart.Size = New System.Drawing.Size(87, 20)
        Me.tbXstart.TabIndex = 13
        Me.tbXstart.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "X start"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(109, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(36, 13)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "height"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(16, 87)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Y start"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(109, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "width"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.tbXstart)
        Me.Panel1.Controls.Add(Me.tbYstart)
        Me.Panel1.Controls.Add(Me.tbYheight)
        Me.Panel1.Controls.Add(Me.tbXwidth)
        Me.Panel1.Location = New System.Drawing.Point(46, 70)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(225, 165)
        Me.Panel1.TabIndex = 18
        '
        'cbUseROI
        '
        Me.cbUseROI.AutoSize = True
        Me.cbUseROI.Location = New System.Drawing.Point(46, 38)
        Me.cbUseROI.Name = "cbUseROI"
        Me.cbUseROI.Size = New System.Drawing.Size(117, 17)
        Me.cbUseROI.TabIndex = 19
        Me.cbUseROI.Text = "use subframe (ROI)"
        Me.cbUseROI.UseVisualStyleBackColor = True
        '
        'btnTemp
        '
        Me.btnTemp.Location = New System.Drawing.Point(46, 251)
        Me.btnTemp.Name = "btnTemp"
        Me.btnTemp.Size = New System.Drawing.Size(94, 32)
        Me.btnTemp.TabIndex = 20
        Me.btnTemp.Text = "Temp control"
        Me.btnTemp.UseVisualStyleBackColor = True
        '
        'btnIO
        '
        Me.btnIO.Location = New System.Drawing.Point(46, 289)
        Me.btnIO.Name = "btnIO"
        Me.btnIO.Size = New System.Drawing.Size(94, 32)
        Me.btnIO.TabIndex = 21
        Me.btnIO.Text = "IO Control"
        Me.btnIO.UseVisualStyleBackColor = True
        '
        'btnLED
        '
        Me.btnLED.Location = New System.Drawing.Point(46, 327)
        Me.btnLED.Name = "btnLED"
        Me.btnLED.Size = New System.Drawing.Size(94, 32)
        Me.btnLED.TabIndex = 22
        Me.btnLED.Text = "LED control"
        Me.btnLED.UseVisualStyleBackColor = True
        '
        'tbGain1
        '
        Me.tbGain1.Location = New System.Drawing.Point(317, 251)
        Me.tbGain1.Name = "tbGain1"
        Me.tbGain1.Size = New System.Drawing.Size(81, 20)
        Me.tbGain1.TabIndex = 23
        '
        'tbOffset1
        '
        Me.tbOffset1.Location = New System.Drawing.Point(317, 289)
        Me.tbOffset1.Name = "tbOffset1"
        Me.tbOffset1.Size = New System.Drawing.Size(81, 20)
        Me.tbOffset1.TabIndex = 24
        '
        'btnSetGain1
        '
        Me.btnSetGain1.Location = New System.Drawing.Point(404, 251)
        Me.btnSetGain1.Name = "btnSetGain1"
        Me.btnSetGain1.Size = New System.Drawing.Size(53, 20)
        Me.btnSetGain1.TabIndex = 25
        Me.btnSetGain1.Text = "Set"
        Me.btnSetGain1.UseVisualStyleBackColor = True
        '
        'btnSetOffset1
        '
        Me.btnSetOffset1.Location = New System.Drawing.Point(404, 289)
        Me.btnSetOffset1.Name = "btnSetOffset1"
        Me.btnSetOffset1.Size = New System.Drawing.Size(53, 20)
        Me.btnSetOffset1.TabIndex = 26
        Me.btnSetOffset1.Text = "Set"
        Me.btnSetOffset1.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(271, 251)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(29, 13)
        Me.Label6.TabIndex = 27
        Me.Label6.Text = "Gain"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(271, 289)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 13)
        Me.Label7.TabIndex = 28
        Me.Label7.Text = "Offset"
        '
        'SetupDialogForm
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(472, 388)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.btnSetOffset1)
        Me.Controls.Add(Me.btnSetGain1)
        Me.Controls.Add(Me.tbOffset1)
        Me.Controls.Add(Me.tbGain1)
        Me.Controls.Add(Me.btnLED)
        Me.Controls.Add(Me.btnIO)
        Me.Controls.Add(Me.btnTemp)
        Me.Controls.Add(Me.cbUseROI)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ComboBoxComPort)
        Me.Controls.Add(Me.chkTrace)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetupDialogForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Apogee Camera Setup"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Private WithEvents label2 As System.Windows.Forms.Label
    Friend WithEvents chkTrace As System.Windows.Forms.CheckBox
    Friend WithEvents ComboBoxComPort As System.Windows.Forms.ComboBox
    Friend WithEvents tbXwidth As TextBox
    Friend WithEvents tbYheight As TextBox
    Friend WithEvents tbYstart As TextBox
    Friend WithEvents tbXstart As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents cbUseROI As CheckBox
    Friend WithEvents btnTemp As Button
    Friend WithEvents btnIO As Button
    Friend WithEvents btnLED As Button
    Friend WithEvents tbGain1 As TextBox
    Friend WithEvents tbOffset1 As TextBox
    Friend WithEvents btnSetGain1 As Button
    Friend WithEvents btnSetOffset1 As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
End Class
