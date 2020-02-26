<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.btnToup = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.btnPCO = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.btnIS = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(16, 39)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(112, 38)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "GigE Camera"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Enabled = False
        Me.Button2.Location = New System.Drawing.Point(168, 115)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(112, 38)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "FoculusCamera"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(320, 39)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(112, 38)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "Basler Camera"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(475, 39)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(112, 38)
        Me.Button4.TabIndex = 3
        Me.Button4.Text = "Point Grey Camera"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Enabled = False
        Me.Button5.Location = New System.Drawing.Point(16, 115)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(112, 38)
        Me.Button5.TabIndex = 4
        Me.Button5.Text = "GigE Astro Camera"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.Location = New System.Drawing.Point(475, 115)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(112, 38)
        Me.Button6.TabIndex = 5
        Me.Button6.Text = "Baumer Gige Camera"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Location = New System.Drawing.Point(168, 39)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(112, 38)
        Me.Button7.TabIndex = 6
        Me.Button7.Text = "Coolsnap Camera"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'btnToup
        '
        Me.btnToup.Location = New System.Drawing.Point(320, 115)
        Me.btnToup.Name = "btnToup"
        Me.btnToup.Size = New System.Drawing.Size(112, 38)
        Me.btnToup.TabIndex = 7
        Me.btnToup.Text = "Toupcam Camera"
        Me.btnToup.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(631, 39)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(112, 38)
        Me.Button8.TabIndex = 8
        Me.Button8.Text = "svs Camera"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'btnPCO
        '
        Me.btnPCO.Location = New System.Drawing.Point(631, 115)
        Me.btnPCO.Name = "btnPCO"
        Me.btnPCO.Size = New System.Drawing.Size(112, 38)
        Me.btnPCO.TabIndex = 9
        Me.btnPCO.Text = "PCO.2000"
        Me.btnPCO.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Location = New System.Drawing.Point(168, 196)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(112, 38)
        Me.Button9.TabIndex = 10
        Me.Button9.Text = "PixeLink"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'btnIS
        '
        Me.btnIS.Location = New System.Drawing.Point(320, 196)
        Me.btnIS.Name = "btnIS"
        Me.btnIS.Size = New System.Drawing.Size(112, 38)
        Me.btnIS.TabIndex = 11
        Me.btnIS.Text = "Imaging Source"
        Me.btnIS.UseVisualStyleBackColor = True
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(807, 246)
        Me.Controls.Add(Me.btnIS)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.btnPCO)
        Me.Controls.Add(Me.Button8)
        Me.Controls.Add(Me.btnToup)
        Me.Controls.Add(Me.Button7)
        Me.Controls.Add(Me.Button6)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form3"
        Me.Text = "sky watcher"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents btnToup As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents btnPCO As Button
    Friend WithEvents Button9 As Button
    Friend WithEvents btnIS As Button
End Class
