Imports TIS.Imaging

Public Class Form1
    'Friend WithEvents IcImagingControl1 As ICImagingControl
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not IcImagingControl1.DeviceValid() Then
            IcImagingControl1.ShowDeviceSettingsDialog()

            If Not IcImagingControl1.DeviceValid Then
                MsgBox("No device was selected.", MsgBoxStyle.Information, "Grabbing an Image")

                Me.Close()
            End If
        End If

        'VCDProp.RangeValue(VCDIDs.VCDID_Exposure) = -1
        Dim AbsValItf As TIS.Imaging.VCDAbsoluteValueProperty


        ' Retrieve an absolute value interface for exposure
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Exposure + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)

        AbsValItf.Value = String.Format("{0,16:0.000000e+00}", 14)
        ' AbsValItf.Value = "3.33000003593042492866516113281e-04"



        ' Retrieve an absolute value interface for gain
        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Gain + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = 22


        AbsValItf = IcImagingControl1.VCDPropertyItems.FindInterface(TIS.Imaging.VCDIDs.VCDID_Saturation + ":" +
                                                                    TIS.Imaging.VCDIDs.VCDElement_Value + ":" +
                                                                 TIS.Imaging.VCDIDs.VCDInterface_AbsoluteValue)
        AbsValItf.Value = 125
        IcImagingControl1.DeviceFrameRate = 0.2
        'IcImagingControl1.ImageRingBufferSize = 1

        IcImagingControl1.Update()


        ' IcImagingControl1.DeviceFrameFilters.Clear()
        'IcImagingControl1.LoadDeviceStateFromFile("device.dat", False)
        'IcImagingControl1.VideoFormat = "RGB32 (3072x2048)"
        IcImagingControl1.DeviceFrameRate = 0.2
        IcImagingControl1.DeviceFlipHorizontal = True
        IcImagingControl1.Update()
        Dim fh As FrameHandlerSink

        'fh = IcImagingControl1.Sink

        ' fh.SnapMode = False
        ' VCDProp = TIS.Imaging.VCDHelpers.VCDSimpleModule.GetSimplePropertyContainer(IcImagingControl1.VCDPropertyItems)
        IcImagingControl1.LiveStart()
    End Sub

    Private Sub IcImagingControl1_ImageAvailable(sender As Object, e As ICImagingControl.ImageAvailableEventArgs) Handles IcImagingControl1.ImageAvailable
        Dim img() As Byte
        Dim iBuffer As ImageBuffer
        Dim pixelValue As UShort
        Dim darkBuffer() As UShort

        iBuffer = IcImagingControl1.ImageActiveBuffer
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.IcImagingControl1 = New TIS.Imaging.ICImagingControl()
        IcImagingControl1.Sink = New FrameQueueSink(AddressOf showBuffer, MediaSubtypes.Y16, 5)
        '        Public Sub New(frameQueued As Func(Of IFrameQueueBuffer, FrameQueuedResult), frameType As FrameType, initialBufferCount As Integer)
    End Sub

    Private Function showBuffer(arg As IFrameQueueBuffer) As FrameQueuedResult
        Dim x As Integer
        x = 0

    End Function
End Class