<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFusion
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
        Me.btnCombine = New System.Windows.Forms.Button()
        Me.splitCameras = New System.Windows.Forms.SplitContainer()
        Me.pnlPointGrey = New System.Windows.Forms.Panel()
        Me.pnlScout = New System.Windows.Forms.Panel()
        Me.picFusion = New System.Windows.Forms.PictureBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.tbX = New System.Windows.Forms.TextBox()
        Me.tbY = New System.Windows.Forms.TextBox()
        CType(Me.splitCameras, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitCameras.Panel1.SuspendLayout()
        Me.splitCameras.Panel2.SuspendLayout()
        Me.splitCameras.SuspendLayout()
        CType(Me.picFusion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCombine
        '
        Me.btnCombine.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnCombine.Location = New System.Drawing.Point(0, 0)
        Me.btnCombine.Name = "btnCombine"
        Me.btnCombine.Size = New System.Drawing.Size(1264, 32)
        Me.btnCombine.TabIndex = 0
        Me.btnCombine.Text = "Combine Latest Raw Images"
        Me.btnCombine.UseVisualStyleBackColor = True
        '
        'splitCameras
        '
        Me.splitCameras.Dock = System.Windows.Forms.DockStyle.Top
        Me.splitCameras.Location = New System.Drawing.Point(0, 32)
        Me.splitCameras.Name = "splitCameras"
        '
        'splitCameras.Panel1
        '
        Me.splitCameras.Panel1.Controls.Add(Me.pnlPointGrey)
        '
        'splitCameras.Panel2
        '
        Me.splitCameras.Panel2.Controls.Add(Me.pnlScout)
        Me.splitCameras.Size = New System.Drawing.Size(1264, 420)
        Me.splitCameras.SplitterDistance = 632
        Me.splitCameras.TabIndex = 1
        '
        'pnlPointGrey
        '
        Me.pnlPointGrey.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlPointGrey.Location = New System.Drawing.Point(0, 0)
        Me.pnlPointGrey.Name = "pnlPointGrey"
        Me.pnlPointGrey.Size = New System.Drawing.Size(632, 420)
        Me.pnlPointGrey.TabIndex = 0
        '
        'pnlScout
        '
        Me.pnlScout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlScout.Location = New System.Drawing.Point(0, 0)
        Me.pnlScout.Name = "pnlScout"
        Me.pnlScout.Size = New System.Drawing.Size(628, 420)
        Me.pnlScout.TabIndex = 0
        '
        'picFusion
        '
        Me.picFusion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picFusion.Location = New System.Drawing.Point(0, 452)
        Me.picFusion.Name = "picFusion"
        Me.picFusion.Size = New System.Drawing.Size(1264, 229)
        Me.picFusion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picFusion.TabIndex = 2
        Me.picFusion.TabStop = False
        '
        'btnStart
        '
        Me.btnStart.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnStart.Location = New System.Drawing.Point(0, 452)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(1264, 32)
        Me.btnStart.TabIndex = 3
        Me.btnStart.Text = "Start cameras"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'tbX
        '
        Me.tbX.Location = New System.Drawing.Point(33, 6)
        Me.tbX.Name = "tbX"
        Me.tbX.Size = New System.Drawing.Size(100, 20)
        Me.tbX.TabIndex = 4
        Me.tbX.Text = "0"
        '
        'tbY
        '
        Me.tbY.Location = New System.Drawing.Point(139, 6)
        Me.tbY.Name = "tbY"
        Me.tbY.Size = New System.Drawing.Size(100, 20)
        Me.tbY.TabIndex = 5
        Me.tbY.Text = "0"
        '
        'frmFusion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 681)
        Me.Controls.Add(Me.tbY)
        Me.Controls.Add(Me.tbX)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.picFusion)
        Me.Controls.Add(Me.splitCameras)
        Me.Controls.Add(Me.btnCombine)
        Me.Name = "frmFusion"
        Me.Text = "Fusion Camera"
        Me.splitCameras.Panel1.ResumeLayout(False)
        Me.splitCameras.Panel2.ResumeLayout(False)
        CType(Me.splitCameras, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitCameras.ResumeLayout(False)
        CType(Me.picFusion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnCombine As Button
    Friend WithEvents splitCameras As SplitContainer
    Friend WithEvents pnlPointGrey As Panel
    Friend WithEvents pnlScout As Panel
    Friend WithEvents picFusion As PictureBox
    Friend WithEvents btnStart As Button
    Friend WithEvents tbX As TextBox
    Friend WithEvents tbY As TextBox
End Class
