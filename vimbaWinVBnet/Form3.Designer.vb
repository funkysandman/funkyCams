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
        Me.btnIS = New System.Windows.Forms.Button()
        Me.SuspendLayout()
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
        Me.Name = "Form3"
        Me.Text = "sky watcher"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnIS As Button
End Class
