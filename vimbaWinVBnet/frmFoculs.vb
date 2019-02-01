Imports System.Runtime.InteropServices

Public Class frmFoculs

    Dim night As Boolean = False
    Private myWebServer As WebServer


    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Button1.Enabled = False
        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1
        AxFGControlCtrl1.BeginInit()
        AxFGControlCtrl1.EndInit()
        'AxFGControlCtrl1.Camera = 0


        Button1.Enabled = True

        Me.ComboBox1.DataSource = AxFGControlCtrl1.GetCameraList()
    End Sub

    Public Sub writeline(s As String)
        Console.WriteLine("Foculus: " & s)
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        AxFGControlCtrl1.Camera = 0
        '        


        '  AxFGControlCtrl1.SetGain("", 400)

        AxFGControlCtrl1.ShowPropertyPage()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        AxFGControlCtrl1.PixelFormat = 8
        AxFGControlCtrl1.BytePerPacket = 1200
        AxFGControlCtrl1.AcquisitionMode = "Single"
        AxFGControlCtrl1.SetExposureTimeString("5s")
        AxFGControlCtrl1.Acquisition = 1

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AxFGControlCtrl1.Acquisition = 0
    End Sub



    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Dim w As Integer
        Dim h As Integer

        w = 1388
        h = 1040
        Dim b = New Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale)

        Dim ncp As System.Drawing.Imaging.ColorPalette = b.Palette
        For i As Integer = 0 To 255
            ncp.Entries(i) = System.Drawing.Color.FromArgb(255, i, i, i)
        Next
        b.Palette = ncp
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub initCamera()



        AxFGControlCtrl1.AcquisitionMode = "Continuous"
        AxFGControlCtrl1.Trigger = 1
        AxFGControlCtrl1.TriggerMode = 0
        AxFGControlCtrl1.TriggerSource = "Software"
        '        AxFGControlCtrl1.TriggerActivation = "RisingEdge"
        AxFGControlCtrl1.PixelFormat = 8
        AxFGControlCtrl1.BytePerPacket = 1200
        AxFGControlCtrl1.SetLUTKneePoint(0, 525, 1290)
        AxFGControlCtrl1.SetLUTKneePoint(1, 1290, 1980)
        AxFGControlCtrl1.SetLUTKneePoint(2, 1860, 2265)
        AxFGControlCtrl1.SetLUTKneePoint(3, 2475, 2715)
        AxFGControlCtrl1.SetExposureTimeString(tbNightExp.Text)
        AxFGControlCtrl1.SetExposureTimeString("135us")
        AxFGControlCtrl1.SetGain("", 0)
        AxFGControlCtrl1.Acquisition = 1
        'AxFGControlCtrl1.SetExposureTimeString("3s")
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        myWebServer = WebServer.getWebServer

        initCamera()


        myWebServer.StartWebServer(Me.AxFGControlCtrl1, Me, Val(Me.tbPort.Text))

    End Sub







    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        myWebServer.StopWebServer()
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try


            If Not myWebServer Is Nothing Then
                Dim currentMode As Boolean
                currentMode = False

                If Now.Hour >= cboNight.SelectedItem Or Now.Hour <= cboDay.SelectedItem Then
                    night = True
                Else
                    night = False
                End If
                ' If currentMode <> night Then

                If night Then
                    'AxFGControlCtrl1.ExposureTimeAuto = "Off"
                    '  AxFGControlCtrl1.AcquisitionMode = "Continuous"

                    tbExposureTime.Text = tbNightExp.Text
                    tbGain.Text = "490"
                    lblDayNight.Text = "night"
                    'night mode
                    If CheckBox1.Checked Then
                        myWebServer.useDarks = True
                    Else
                        myWebServer.useDarks = False
                    End If
                Else
                    'day mode

                    tbExposureTime.Text = tbDayTimeExp.Text
                    tbGain.Text = "0"
                    lblDayNight.Text = "day"


                End If
                'End If
            End If
        Catch ex As Exception

        End Try
    End Sub




    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click



        AxFGControlCtrl1.Acquisition = 1

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        AxFGControlCtrl1.Camera = ComboBox1.SelectedIndex
    End Sub

    'Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
    '    initCamera()
    'End Sub

    'Private Sub AxFGControlCtrl1_ImageReceived(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedEvent) Handles AxFGControlCtrl1.ImageReceived
    '    AxFGControlCtrl1.Acquisition = 0


    'End Sub

    'Private Sub AxFGControlCtrl1_ImageReceivedExt(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedExtEvent) Handles AxFGControlCtrl1.ImageReceivedExt
    '    Dim rawBytesCount

    '    rawBytesCount = AxFGControlCtrl1.GetPayloadSize()
    '    '  Dim rawData(rawBytesCount) As Byte



    '    MsgBox(AxFGControlCtrl1.GetReceivedFrameCount)
    'End Sub

    'Private Sub AxFGControlCtrl1_DeviceEventCallback(sender As Object, e As AxFGControlLib._IFGControlEvents_DeviceEventCallbackEvent) Handles AxFGControlCtrl1.DeviceEventCallback
    '    MsgBox("deviceEventCallback")
    'End Sub

    'Private Sub AxFGControlCtrl1_JobCompleted(sender As Object, e As AxFGControlLib._IFGControlEvents_JobCompletedEvent) Handles AxFGControlCtrl1.JobCompleted
    '    MsgBox("job completed")
    'End Sub


End Class