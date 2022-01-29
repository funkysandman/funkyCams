Public Class ApogeeCam

    Public Shared c As New APOGEELib.Camera2
    Public imageData As Array
    Public ccdWidth As Integer = 0
    Public ccdHeight As Integer = 0
    Private FindDlg As APOGEELib.CamDiscover
    Sub New()
        FindDlg = New APOGEELib.CamDiscover()
        c = New APOGEELib.Camera2
        Debug.Print("new camera")
        Dim tempImage As Array
        FindDlg.DlgCheckEthernet = False
        FindDlg.DlgCheckUsb = True

        FindDlg.ShowDialog(True)

        If FindDlg.ValidSelection Then
            Debug.WriteLine("here we are")
            c.Init(FindDlg.SelectedInterface, FindDlg.SelectedCamIdOne, FindDlg.SelectedCamIdTwo, 0)
            c.ResetSystem()
            c.ImageCount = 0
            c.RoiBinningH = 1
            c.RoiBinningV = 1
            c.RoiStartX = 0
            c.RoiStartY = 0
            ccdWidth = c.RoiPixelsH
            ccdHeight = c.RoiPixelsV
            Debug.WriteLine(c.ImagingStatus)
            '  AltaCamera.Expose(0.001, False)
            '  Debug.WriteLine(AltaCamera.ImagingStatus)

            ' Do
            '     Debug.WriteLine(AltaCamera.ImagingStatus)
            ' Loop Until AltaCamera.ImagingStatus = APOGEELib.Apn_Status.Apn_Status_ImageReady
            ' Debug.WriteLine(AltaCamera.ImagingStatus)
            ' imageData = AltaCamera.Image
            Debug.WriteLine(c.RoiPixelsH)
            Debug.WriteLine(c.RoiPixelsV)
        End If
        Debug.WriteLine(c.ImagingStatus)
        Debug.WriteLine(c.Sensor)
        Debug.WriteLine(c.CameraModel)
    End Sub

    Public Sub Expose(t As Double, light As Boolean)
        'Dim tempImage As Array
        c.ResetSystem()
        'Debug.Print("flusing last image")
        'imageData = c.Image


        c.ImageCount = 0
        c.Expose(t, light)
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
