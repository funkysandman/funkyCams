Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports System.Runtime.InteropServices
Imports QCamManagedDriver


Public Class frmQimaging
        Inherits System.Windows.Forms.Form

        Private mCamList As QCamM_CamListItem()
        Private mhCamera As IntPtr
        Private mDisplayPanel As myPanel
        Private mIsMono As Boolean
        Private mDisplayBitmap As Bitmap
        Private mFrameCallback As QCamM_AsyncCallback
        Private mFrame1 As QCamM_Frame
        Private mFrame2 As QCamM_Frame
        Private mRgbFrame As QCamM_Frame
        Private mSettings As QCamM_SettingsEx
        Private components As Container = Nothing
        Private button1 As Button
        Private checkBox1 As CheckBox
        Private tbExposure As TrackBar
        Private lblExposureVal As Label
        Private panel1 As Panel
        Private gbAcquisition As GroupBox
        Private gbExposure As GroupBox
        Private gbGain As GroupBox
        Private lblGainVal As Label
        Private tbGain As TrackBar
        Private gbInfo As GroupBox
        Private lblCameraModel As Label
        Private pictureBox1 As PictureBox
        Private lblSerNum As Label

        Public Sub New()
            InitializeComponent()

            If QCamM_Err.qerrSuccess <> QCam.QCamM_LoadDriver() Then
                System.Windows.Forms.MessageBox.Show("The application was unable to load the QCam driver.")
                System.Environment.[Exit](0)
            End If

            If Not OpenCamera() Then
                System.Windows.Forms.MessageBox.Show("The application was unable to connect to a QImaging camera.  Please ensure one is connected and turned on before running this application.")
                System.Environment.[Exit](0)
            End If

            If Not InitCamera() Then
                System.Windows.Forms.MessageBox.Show("Failed to initialize the camera")
                System.Environment.[Exit](0)
            End If

            GrabFrame()
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                QCam.QCamM_Abort(mhCamera)
                If mFrame1 IsNot Nothing Then QCam.QCamM_Free(mFrame1.pBuffer)
                If mFrame2 IsNot Nothing Then QCam.QCamM_Free(mFrame2.pBuffer)
                If mRgbFrame IsNot Nothing Then QCam.QCamM_Free(mRgbFrame.pBuffer)
                QCam.QCamM_ReleaseCameraSettingsStruct(mSettings)
                QCam.QCamM_CloseCamera(mhCamera)
                QCam.QCamM_ReleaseDriver()

                If components IsNot Nothing Then
                    components.Dispose()
                End If
            End If

            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
        Me.button1 = New System.Windows.Forms.Button()
        Me.checkBox1 = New System.Windows.Forms.CheckBox()
        Me.tbExposure = New System.Windows.Forms.TrackBar()
        Me.lblExposureVal = New System.Windows.Forms.Label()
        Me.panel1 = New System.Windows.Forms.Panel()
        Me.gbInfo = New System.Windows.Forms.GroupBox()
        Me.lblSerNum = New System.Windows.Forms.Label()
        Me.lblCameraModel = New System.Windows.Forms.Label()
        Me.gbGain = New System.Windows.Forms.GroupBox()
        Me.lblGainVal = New System.Windows.Forms.Label()
        Me.tbGain = New System.Windows.Forms.TrackBar()
        Me.gbExposure = New System.Windows.Forms.GroupBox()
        Me.gbAcquisition = New System.Windows.Forms.GroupBox()
        Me.pictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.tbExposure, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panel1.SuspendLayout()
        Me.gbInfo.SuspendLayout()
        Me.gbGain.SuspendLayout()
        CType(Me.tbGain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbExposure.SuspendLayout()
        Me.gbAcquisition.SuspendLayout()
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(6, 15)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(80, 32)
        Me.button1.TabIndex = 1
        Me.button1.Text = "GrabFrame"
        '
        'checkBox1
        '
        Me.checkBox1.Location = New System.Drawing.Point(6, 53)
        Me.checkBox1.Name = "checkBox1"
        Me.checkBox1.Size = New System.Drawing.Size(80, 20)
        Me.checkBox1.TabIndex = 3
        Me.checkBox1.Text = "Streaming"
        '
        'tbExposure
        '
        Me.tbExposure.Location = New System.Drawing.Point(3, 19)
        Me.tbExposure.Maximum = 1000
        Me.tbExposure.Minimum = 10
        Me.tbExposure.Name = "tbExposure"
        Me.tbExposure.Size = New System.Drawing.Size(104, 45)
        Me.tbExposure.TabIndex = 4
        Me.tbExposure.TickFrequency = 100
        Me.tbExposure.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.tbExposure.Value = 10
        '
        'lblExposureVal
        '
        Me.lblExposureVal.AutoSize = True
        Me.lblExposureVal.Location = New System.Drawing.Point(116, 32)
        Me.lblExposureVal.Name = "lblExposureVal"
        Me.lblExposureVal.Size = New System.Drawing.Size(35, 13)
        Me.lblExposureVal.TabIndex = 6
        Me.lblExposureVal.Text = "10 ms"
        '
        'panel1
        '
        Me.panel1.Controls.Add(Me.gbInfo)
        Me.panel1.Controls.Add(Me.gbGain)
        Me.panel1.Controls.Add(Me.gbExposure)
        Me.panel1.Controls.Add(Me.gbAcquisition)
        Me.panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panel1.Location = New System.Drawing.Point(0, 533)
        Me.panel1.Name = "panel1"
        Me.panel1.Size = New System.Drawing.Size(698, 74)
        Me.panel1.TabIndex = 7
        '
        'gbInfo
        '
        Me.gbInfo.Controls.Add(Me.lblSerNum)
        Me.gbInfo.Controls.Add(Me.lblCameraModel)
        Me.gbInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbInfo.Location = New System.Drawing.Point(425, 0)
        Me.gbInfo.Name = "gbInfo"
        Me.gbInfo.Size = New System.Drawing.Size(273, 74)
        Me.gbInfo.TabIndex = 10
        Me.gbInfo.TabStop = False
        Me.gbInfo.Text = "Info"
        '
        'lblSerNum
        '
        Me.lblSerNum.AutoSize = True
        Me.lblSerNum.Location = New System.Drawing.Point(7, 33)
        Me.lblSerNum.Name = "lblSerNum"
        Me.lblSerNum.Size = New System.Drawing.Size(36, 13)
        Me.lblSerNum.TabIndex = 2
        Me.lblSerNum.Text = "Serial:"
        '
        'lblCameraModel
        '
        Me.lblCameraModel.AutoSize = True
        Me.lblCameraModel.Location = New System.Drawing.Point(6, 16)
        Me.lblCameraModel.Name = "lblCameraModel"
        Me.lblCameraModel.Size = New System.Drawing.Size(39, 13)
        Me.lblCameraModel.TabIndex = 1
        Me.lblCameraModel.Text = "Model:"
        '
        'gbGain
        '
        Me.gbGain.Controls.Add(Me.lblGainVal)
        Me.gbGain.Controls.Add(Me.tbGain)
        Me.gbGain.Dock = System.Windows.Forms.DockStyle.Left
        Me.gbGain.Location = New System.Drawing.Point(270, 0)
        Me.gbGain.Name = "gbGain"
        Me.gbGain.Size = New System.Drawing.Size(155, 74)
        Me.gbGain.TabIndex = 9
        Me.gbGain.TabStop = False
        Me.gbGain.Text = "Gain"
        '
        'lblGainVal
        '
        Me.lblGainVal.AutoSize = True
        Me.lblGainVal.Location = New System.Drawing.Point(111, 33)
        Me.lblGainVal.Name = "lblGainVal"
        Me.lblGainVal.Size = New System.Drawing.Size(13, 13)
        Me.lblGainVal.TabIndex = 1
        Me.lblGainVal.Text = "0"
        '
        'tbGain
        '
        Me.tbGain.Location = New System.Drawing.Point(2, 19)
        Me.tbGain.Name = "tbGain"
        Me.tbGain.Size = New System.Drawing.Size(104, 45)
        Me.tbGain.TabIndex = 0
        Me.tbGain.TickFrequency = 200
        Me.tbGain.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'gbExposure
        '
        Me.gbExposure.Controls.Add(Me.tbExposure)
        Me.gbExposure.Controls.Add(Me.lblExposureVal)
        Me.gbExposure.Dock = System.Windows.Forms.DockStyle.Left
        Me.gbExposure.Location = New System.Drawing.Point(94, 0)
        Me.gbExposure.Name = "gbExposure"
        Me.gbExposure.Size = New System.Drawing.Size(176, 74)
        Me.gbExposure.TabIndex = 8
        Me.gbExposure.TabStop = False
        Me.gbExposure.Text = "Exposure"
        '
        'gbAcquisition
        '
        Me.gbAcquisition.Controls.Add(Me.button1)
        Me.gbAcquisition.Controls.Add(Me.checkBox1)
        Me.gbAcquisition.Dock = System.Windows.Forms.DockStyle.Left
        Me.gbAcquisition.Location = New System.Drawing.Point(0, 0)
        Me.gbAcquisition.Name = "gbAcquisition"
        Me.gbAcquisition.Size = New System.Drawing.Size(94, 74)
        Me.gbAcquisition.TabIndex = 7
        Me.gbAcquisition.TabStop = False
        Me.gbAcquisition.Text = "Acquisition"
        '
        'pictureBox1
        '
        Me.pictureBox1.Location = New System.Drawing.Point(37, 35)
        Me.pictureBox1.Name = "pictureBox1"
        Me.pictureBox1.Size = New System.Drawing.Size(506, 407)
        Me.pictureBox1.TabIndex = 8
        Me.pictureBox1.TabStop = False
        '
        'frmQimaging
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(698, 607)
        Me.Controls.Add(Me.pictureBox1)
        Me.Controls.Add(Me.panel1)
        Me.Name = "frmQimaging"
        Me.Text = "QCamManagedDriver Demo"
        CType(Me.tbExposure, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panel1.ResumeLayout(False)
        Me.gbInfo.ResumeLayout(False)
        Me.gbInfo.PerformLayout()
        Me.gbGain.ResumeLayout(False)
        Me.gbGain.PerformLayout()
        CType(Me.tbGain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbExposure.ResumeLayout(False)
        Me.gbExposure.PerformLayout()
        Me.gbAcquisition.ResumeLayout(False)
        CType(Me.pictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub


    Private Function OpenCamera() As Boolean
        mCamList = New QCamM_CamListItem(9) {}
        Dim listLen As UInteger = 10
        QCam.QCamM_ListCameras(mCamList, listLen)

        If (listLen > 0) AndAlso (mCamList(0).isOpen = 0) Then

            If QCam.QCamM_OpenCamera(mCamList(0).cameraId, mhCamera) <> QCamM_Err.qerrSuccess Then
                Return False
            End If

            Return True
        Else
            Return False
        End If
    End Function

    Private Function InitCamera() As Boolean
        Dim ccdType As UInteger = 0
        Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        mSettings = New QCamM_SettingsEx()
        QCam.QCamM_CreateCameraSettingsStruct(mSettings)
        QCam.QCamM_InitializeCameraSettings(mhCamera, mSettings)
        QCam.QCamM_ReadDefaultSettings(mhCamera, mSettings)
        err = QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfCcdType, ccdType)

        If ccdType = Convert.ToUInt32(QCamM_qcCcdType.qcCcdColorBayer) Then
            err = QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32(QCamM_ImageFormat.qfmtBayer8))
            mIsMono = False
        ElseIf ccdType = Convert.ToUInt32(QCamM_qcCcdType.qcCcdMonochrome) Then
            err = QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmImageFormat, Convert.ToUInt32(QCamM_ImageFormat.qfmtMono8))
            mIsMono = True
        Else
            Return False
        End If

        err = QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
        Dim frameSize As UInteger = 0
        QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, frameSize)
        mFrame1 = New QCamM_Frame()
        mFrame1.bufferSize = frameSize
        mFrame1.pBuffer = QCam.QCamM_Malloc(mFrame1.bufferSize)
        mFrame2 = New QCamM_Frame()
        mFrame2.bufferSize = frameSize
        mFrame2.pBuffer = QCam.QCamM_Malloc(mFrame2.bufferSize)

        If Not mIsMono Then
            mRgbFrame = New QCamM_Frame()
            mRgbFrame.bufferSize = frameSize * 3
            mRgbFrame.pBuffer = QCam.QCamM_Malloc(mRgbFrame.bufferSize)
            mRgbFrame.format = CUInt(QCamM_ImageFormat.qfmtBgr24)
        End If

        If QCam.QCamM_IsParamSupported(mhCamera, QCamM_Param.qprmGain) = QCamM_Err.qerrSuccess Then
            gbGain.Enabled = True
            Dim val As UInteger = 0
            QCam.QCamM_GetParamMin(mSettings, QCamM_Param.qprmGain, val)
            tbGain.Minimum = CInt(val)
            QCam.QCamM_GetParamMax(mSettings, QCamM_Param.qprmGain, val)
            tbGain.Maximum = CInt(val)
            QCam.QCamM_GetParam(mSettings, QCamM_Param.qprmGain, val)
            tbGain.Value = CInt(val)
        Else
            gbGain.Enabled = False
        End If

        If QCam.QCamM_IsParamSupported(mhCamera, QCamM_Param.qprmExposure) = QCamM_Err.qerrSuccess Then
            gbExposure.Enabled = True
            Dim val As UInteger = 0
            QCam.QCamM_GetParam(mSettings, QCamM_Param.qprmExposure, val)
            tbExposure.Value = CInt((val / 1000))
        Else
            gbExposure.Enabled = False
        End If

        lblCameraModel.Text = "Model: " & (CType((mCamList(0).cameraType), QCamM_qcCameraType)).ToString().Remove(0, 8)
        Dim serNum As String = ""
        QCam.QCamM_GetSerialString(mhCamera, serNum)
        lblSerNum.Text = "Serial: " & serNum
        mFrameCallback = New QCamM_AsyncCallback(AddressOf frameCallback)
        Return True
    End Function

    Private Sub StopStream()
        QCam.QCamM_Abort(mhCamera)
        QCam.QCamM_SetStreaming(mhCamera, 0)
    End Sub

    Private Sub StartStream()
        QCam.QCamM_Abort(mhCamera)
        QCam.QCamM_SetStreaming(mhCamera, 1)
        QueueFrame(1)
        QueueFrame(2)
    End Sub

    Public Sub GrabFrame()
        Dim width, height As UInteger
        Dim err As QCamM_Err = QCamM_Err.qerrSuccess
        Dim sizeInBytes As UInteger = 0
        QCam.QCamM_GetInfo(mhCamera, QCamM_Info.qinfImageSize, sizeInBytes)
        err = QCam.QCamM_GrabFrame(mhCamera, mFrame1)
        width = mFrame1.width
        height = mFrame1.height
        Dim bmp As Bitmap = Nothing

        If mIsMono Then
            bmp = New Bitmap(CInt(width), CInt(height), CInt(width), PixelFormat.Format8bppIndexed, mFrame1.pBuffer)
            Dim pt As ColorPalette = bmp.Palette

            For i As Integer = 0 To pt.Entries.Length - 1
                pt.Entries(i) = Color.FromArgb(i, i, i)
            Next

            bmp.Palette = pt
        Else
            QCamImgfnc.QCamM_BayerToRgb(QCamM_qcBayerInterp.qcBayerInterpFast, mFrame1, mRgbFrame)
            bmp = New Bitmap(CInt(width), CInt(height), CInt(width) * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer)
        End If

        mDisplayBitmap = bmp
        pictureBox1.Image = bmp

        Try

            Using fs As FileStream = New FileStream("image.raw", FileMode.Create)
                Dim bw As BinaryWriter = New System.IO.BinaryWriter(fs)

                For i As Integer = 0 To mFrame1.size - 1
                    bw.Write(Marshal.ReadByte(mFrame1.pBuffer, i))
                Next

                bw.Close()
            End Using

        Catch e As Exception
            MessageBox.Show("Unable to save the image data: " & e.Message)
        End Try
    End Sub

    Private Sub frameCallback(ByVal userPtr As IntPtr, ByVal userData As UInteger, ByVal errcode As QCamM_Err, ByVal flags As UInteger)
        Dim myFrame As QCamM_Frame

        If userData = 1 Then
            myFrame = mFrame1
        ElseIf userData = 2 Then
            myFrame = mFrame2
        Else
            Return
        End If

        Dim width As UInteger = myFrame.width
        Dim height As UInteger = myFrame.height
        Dim bmp As Bitmap

        If mIsMono Then
            bmp = New Bitmap(CInt(width), CInt(height), CInt(width), PixelFormat.Format8bppIndexed, myFrame.pBuffer)
            Dim pt As ColorPalette = bmp.Palette

            For i As Integer = 0 To pt.Entries.Length - 1
                pt.Entries(i) = Color.FromArgb(i, i, i)
            Next

            bmp.Palette = pt
        Else
            QCamImgfnc.QCamM_BayerToRgb(QCamM_qcBayerInterp.qcBayerInterpFast, myFrame, mRgbFrame)
            bmp = New Bitmap(CInt(width), CInt(height), CInt(width) * 3, PixelFormat.Format24bppRgb, mRgbFrame.pBuffer)
        End If

        mDisplayBitmap = bmp
        QueueFrame(userData)
        mDisplayPanel.Invalidate()
    End Sub

    Private Sub panel1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        If mDisplayBitmap IsNot Nothing Then
            Dim g As Graphics = e.Graphics
            g.DrawImage(mDisplayBitmap, 0, 0, mDisplayPanel.Width, mDisplayPanel.Height)
        End If
    End Sub

    Private Sub OnGrabFrame(ByVal sender As Object, ByVal e As System.EventArgs)
        GrabFrame()
    End Sub

    Private Function QueueFrame(ByVal frameNum As UInteger) As Boolean
        Dim err As QCamM_Err

        If frameNum = 1 Then
            err = QCam.QCamM_QueueFrame(Me.mhCamera, mFrame1, mFrameCallback, CUInt(QCamM_qcCallbackFlags.qcCallbackDone), IntPtr.Zero, frameNum)
        ElseIf frameNum = 2 Then
            err = QCam.QCamM_QueueFrame(Me.mhCamera, mFrame2, mFrameCallback, CUInt(QCamM_qcCallbackFlags.qcCallbackDone), IntPtr.Zero, frameNum)
        Else
            Return False
        End If

        If err = QCamM_Err.qerrSuccess Then
            Return True
        Else
            Return False
        End If
    End Function

    Private streaming As Boolean = False

    Private Sub OnStream(ByVal sender As Object, ByVal e As System.EventArgs)
        streaming = Not streaming
        button1.Enabled = Not streaming

        If streaming Then
            StartStream()
        Else
            StopStream()
        End If
    End Sub

    Private Sub Form1_SizeChanged(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Private Sub tbGain_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblGainVal.Text = tbGain.Value.ToString()
        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmGain, CUInt((tbGain.Value)))
        QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
    End Sub

    Private Sub tbExposure_ValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblExposureVal.Text = tbExposure.Value.ToString() & " ms"
        QCam.QCamM_SetParam(mSettings, QCamM_Param.qprmExposure, CUInt((tbExposure.Value * 1000)))
        QCam.QCamM_SendSettingsToCam(mhCamera, mSettings)
    End Sub

    Private Sub frmQimaging_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


End Class

Public Class myPanel
        Inherits System.Windows.Forms.Panel

        Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
        End Sub
    End Class

