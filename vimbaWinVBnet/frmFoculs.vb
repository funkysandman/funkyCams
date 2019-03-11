Imports System.Runtime.InteropServices

Public Class frmFoculs

    Dim night As Boolean = False
    Private myWebServer As WebServer


    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Button1.Enabled = False
        cboNight.SelectedIndex = 1
        cboDay.SelectedIndex = 1
        AxFGControlCtrl2.BeginInit()
        AxFGControlCtrl2.EndInit()
        'AxFGControlCtrl2.Camera = 0

        ComboBox1.DataSource = AxFGControlCtrl2.GetCameraList()

        Button1.Enabled = True


    End Sub

    Public Sub writeline(s As String)
        Console.WriteLine("Foculus: " & s)
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        AxFGControlCtrl2.Camera = 0
        '        


        '  AxFGControlCtrl2.SetGain("", 400)

        AxFGControlCtrl2.ShowPropertyPage()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        AxFGControlCtrl2.PixelFormat = 8
        AxFGControlCtrl2.BytePerPacket = 1200
        AxFGControlCtrl2.AcquisitionMode = "SingleFrame"
        AxFGControlCtrl2.SetExposureTimeString("5s")
        AxFGControlCtrl2.Acquisition = 1

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AxFGControlCtrl2.Acquisition = 0
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



        AxFGControlCtrl2.AcquisitionMode = "Continuous"
        AxFGControlCtrl2.Trigger = 1
        AxFGControlCtrl2.TriggerMode = 0
        AxFGControlCtrl2.TriggerSource = "Software"
        '        AxFGControlCtrl2.TriggerActivation = "RisingEdge"
        AxFGControlCtrl2.PixelFormat = 8
        AxFGControlCtrl2.BytePerPacket = 1000
        AxFGControlCtrl2.SetLUTKneePoint(0, 525, 1290)
        AxFGControlCtrl2.SetLUTKneePoint(1, 1290, 1980)
        AxFGControlCtrl2.SetLUTKneePoint(2, 1860, 2265)
        AxFGControlCtrl2.SetLUTKneePoint(3, 2475, 2715)
        AxFGControlCtrl2.SetExposureTimeString(tbNightExp.Text)
        AxFGControlCtrl2.SetExposureTimeString("135us")
        AxFGControlCtrl2.SetGain("", 0)
        AxFGControlCtrl2.Acquisition = 1
        'AxFGControlCtrl2.SetExposureTimeString("3s")
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        myWebServer = WebServer.getWebServer

        initCamera()


        myWebServer.StartWebServer(Me.AxFGControlCtrl2, Me, Val(Me.tbPort.Text))

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
                    'AxFGControlCtrl2.ExposureTimeAuto = "Off"
                    '  AxFGControlCtrl2.AcquisitionMode = "Continuous"

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



        AxFGControlCtrl2.Acquisition = 1

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        AxFGControlCtrl2.Camera = ComboBox1.SelectedIndex
    End Sub

    Private Sub frmFoculs_Activated(sender As Object, e As EventArgs) Handles Me.Activated

    End Sub

    'Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
    '    initCamera()
    'End Sub

    'Private Sub AxFGControlCtrl2_ImageReceived(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedEvent) Handles AxFGControlCtrl2.ImageReceived
    '    AxFGControlCtrl2.Acquisition = 0


    'End Sub

    'Private Sub AxFGControlCtrl2_ImageReceivedExt(sender As Object, e As AxFGControlLib._IFGControlEvents_ImageReceivedExtEvent) Handles AxFGControlCtrl2.ImageReceivedExt
    '    Dim rawBytesCount

    '    rawBytesCount = AxFGControlCtrl2.GetPayloadSize()
    '    '  Dim rawData(rawBytesCount) As Byte



    '    MsgBox(AxFGControlCtrl2.GetReceivedFrameCount)
    'End Sub

    'Private Sub AxFGControlCtrl2_DeviceEventCallback(sender As Object, e As AxFGControlLib._IFGControlEvents_DeviceEventCallbackEvent) Handles AxFGControlCtrl2.DeviceEventCallback
    '    MsgBox("deviceEventCallback")
    'End Sub

    'Private Sub AxFGControlCtrl2_JobCompleted(sender As Object, e As AxFGControlLib._IFGControlEvents_JobCompletedEvent) Handles AxFGControlCtrl2.JobCompleted
    '    MsgBox("job completed")
    'End Sub


End Class