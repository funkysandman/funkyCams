Public Class ApogeeCam

    Public Shared c As APOGEELib.Camera2
    Public imageData As Array
    Public ccdWidth As Integer = 0
    Public ccdHeight As Integer = 0
    Private FindDlg As APOGEELib.CamDiscover
    Sub New(ByRef inter As APOGEELib.Apn_Interface, ByRef selectedModel As String, ByRef selectedDevice As Integer, ByRef camIdOne As Integer, ByRef camIdTwo As Integer)
        c = New APOGEELib.Camera2
        If selectedModel = "" Then
            'no camera selected
            FindDlg = New APOGEELib.CamDiscover()

            Debug.Print("new camera")

            FindDlg.DlgCheckEthernet = False
            FindDlg.DlgCheckUsb = True

            FindDlg.ShowDialog(True)


            If FindDlg.ValidSelection Then
                Debug.WriteLine("here we are")
                camIdOne = FindDlg.SelectedCamIdOne
                camIdTwo = FindDlg.SelectedCamIdTwo
                inter = FindDlg.SelectedInterface
                selectedModel = FindDlg.SelectedModel

            End If


        End If
        'connect to rememberd camera
        c.Init(inter, camIdOne, camIdTwo, 0)
        c.ResetSystem()
        c.ImageCount = 0

        ccdWidth = c.RoiPixelsH
        ccdHeight = c.RoiPixelsV


        Debug.WriteLine(c.RoiPixelsH)
        Debug.WriteLine(c.RoiPixelsV)


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

    Protected Overrides Sub Finalize()
        Debug.WriteLine("finalize")
        'MyBase.Finalize()
    End Sub
End Class
