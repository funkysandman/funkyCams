Imports Basler.Pylon

' Bring extension methods of the pylon API into scope.
Imports Basler.Pylon.IIntegerParameterExtensions
Imports Basler.Pylon.IBooleanParameterExtensions
Imports Basler.Pylon.ICommandParameterExtensions
Imports Basler.Pylon.IEnumParameterExtensions
Imports Basler.Pylon.IFloatParameterExtensions
Imports Basler.Pylon.IImageExtensions
Imports Basler.Pylon.IStringParameterExtensions
Public Class Form4
    Private myCam As New Basler.Pylon.Camera()
    ' Private m_imageProvider As New NETSupportLibrary.ImageProvider()
    Private Sub Form4_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        myCam.StreamGrabber.Stop()
        '' m_imageProvider.Close()
        myCam.Close()
    End Sub
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '

        ' Register for the events of the image provider needed for proper operation. */
        'm_imageProvider.GrabErrorEvent += New ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback)
        '    m_imageProvider.DeviceRemovedEvent += new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback);
        '    m_imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
        '    m_imageProvider.DeviceClosedEvent += new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback);
        '    m_imageProvider.GrabbingStartedEvent += new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback);
        '    m_imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
        '    m_imageProvider.GrabbingStoppedEvent += new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback);

        'Dim device As NETSupportLibrary.DeviceEnumerator.Device
        'Dim list As List(Of NETSupportLibrary.DeviceEnumerator.Device) = NETSupportLibrary.DeviceEnumerator.EnumerateDevices()
        ''Dim items As ListView.ListViewItemCollection = ListView1.Items

        ' Create a camera object that selects the first camera device found.
        ' More constructors are available for selecting a specific camera device.
        ' myCam.Open()
        'Dim item As ListViewItem = items(0)
        ''/* Get the attached device data. */
        'device = CType(item.Tag, NETSupportLibrary.DeviceEnumerator.Device)
        Try
            '{
            '    /* Open the image provider using the index from the device data. */
            '    m_imageProvider.Open(device.Index);
            '}
            '  m_imageProvider.Open(list(0).Index)
            '************************************************************************
            '* Accessing camera parameters                                          *
            '************************************************************************


            ' Before accessing camera device parameters the camera must be opened.
            myCam.Open()
            'set packetsize
            ' m_imageProvider.OneShot()
            myCam.Parameters(PLCamera.PacketSize).SetValue(1500)
            '
            myCam.Parameters(PLCamera.AcquisitionMode).SetValue(PLCamera.AcquisitionMode.Continuous)
            'camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber)
            myCam.Parameters(PLCamera.TriggerMode).SetValue(PLCamera.TriggerMode.On)

            myCam.Parameters(PLCamera.TriggerSource).SetValue(PLCamera.TriggerSource.Software)
            myCam.Parameters(PLCamera.ExposureMode).SetValue(PLCamera.ExposureMode.TriggerWidth)




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        myCam.Parameters(PLCamera.AcquisitionMode).SetValue(PLCamera.AcquisitionMode.Continuous)
        myCam.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        myCam.Parameters(PLCamera.TriggerActivation).SetValue(PLCamera.TriggerActivation.FallingEdge)

        myCam.Parameters(PLCamera.TriggerActivation).SetValue(PLCamera.TriggerActivation.RisingEdge)




    
    End Sub

End Class