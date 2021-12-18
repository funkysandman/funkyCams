Public Class ApogeeCam

    Public Shared c As New APOGEELib.Camera2
    Public imageData As Array
    Private FindDlg As APOGEELib.CamDiscover
    Sub New()
        FindDlg = New APOGEELib.CamDiscover()
        c = New APOGEELib.Camera2
        Debug.Print("new camera")
        Dim tempImage As Array
        FindDlg.DlgCheckEthernet = False
        FindDlg.DlgCheckUsb = True
        c.
        FindDlg.ShowDialog(True)

        If FindDlg.ValidSelection Then
            Debug.WriteLine("here we are")
            c.Init(FindDlg.SelectedInterface, FindDlg.SelectedCamIdOne, FindDlg.SelectedCamIdTwo, 0)
            c.ResetSystem()
            c.ImageCount = 0

            Debug.WriteLine(c.ImagingStatus)
            '  AltaCamera.Expose(0.001, False)
            '  Debug.WriteLine(AltaCamera.ImagingStatus)

            ' Do
            '     Debug.WriteLine(AltaCamera.ImagingStatus)
            ' Loop Until AltaCamera.ImagingStatus = APOGEELib.Apn_Status.Apn_Status_ImageReady
            ' Debug.WriteLine(AltaCamera.ImagingStatus)
            ' imageData = AltaCamera.Image

        End If
        Debug.WriteLine(c.ImagingStatus)
        Debug.WriteLine(c.Sensor)
        Debug.WriteLine(c.CameraModel)
    End Sub

    Public Sub Expose(t As Double)
        'Dim tempImage As Array
        c.ResetSystem()
        'Debug.Print("flusing last image")
        'imageData = c.Image


        c.ImageCount = 0
        c.Expose(t, False)
        Debug.WriteLine(c.ImagingStatus)

        While c.ImagingStatus = APOGEELib.Apn_Status.Apn_Status_Exposing Or c.ImagingStatus = APOGEELib.Apn_Status.Apn_Status_ImagingActive
            Debug.WriteLine(c.ImagingStatus)
        End While
        Debug.WriteLine(c.ImagingStatus)

        If c.ImagingStatus = APOGEELib.Apn_Status.Apn_Status_ImageReady Then
            imageData = c.Image
        End If

        Debug.WriteLine(c.ImagingStatus)

    End Sub







End Class
