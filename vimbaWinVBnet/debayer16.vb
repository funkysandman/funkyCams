Option Strict On
Option Explicit On

Public Module Debayer16

    Public Function Debayer(
        bayer As Image16,
        pattern As BayerPattern) As RgbImage16

        Dim rgb As New RgbImage16(bayer.Width, bayer.Height, 65535)

        For y = 0 To bayer.Height - 1

            For x = 0 To bayer.Width - 1

                Dim isEvenX = (x And 1) = 0
                Dim isEvenY = (y And 1) = 0

                Dim r As UShort
                Dim g As UShort
                Dim b As UShort

                Select Case pattern

                    Case BayerPattern.RGGB

                        If isEvenY Then

                            If isEvenX Then
                                ' R
                                r = Sample(bayer, x, y)
                                g = Avg4(bayer, x - 1, y,
                                                x + 1, y,
                                                x, y - 1,
                                                x, y + 1)
                                b = Avg4(bayer,
                                         x - 1, y - 1,
                                         x + 1, y - 1,
                                         x - 1, y + 1,
                                         x + 1, y + 1)

                            Else
                                ' G on R row
                                r = Avg2(bayer, x - 1, y,
                                                 x + 1, y)
                                g = Sample(bayer, x, y)
                                b = Avg2(bayer, x, y - 1,
                                                 x, y + 1)
                            End If

                        Else

                            If isEvenX Then
                                ' G on B row
                                r = Avg2(bayer, x, y - 1,
                                                 x, y + 1)
                                g = Sample(bayer, x, y)
                                b = Avg2(bayer, x - 1, y,
                                                 x + 1, y)
                            Else
                                ' B
                                r = Avg4(bayer,
                                         x - 1, y - 1,
                                         x + 1, y - 1,
                                         x - 1, y + 1,
                                         x + 1, y + 1)
                                g = Avg4(bayer,
                                         x - 1, y,
                                         x + 1, y,
                                         x, y - 1,
                                         x, y + 1)
                                b = Sample(bayer, x, y)
                            End If

                        End If

                    Case Else
                        Throw New NotImplementedException(
                            "Only RGGB implemented in first version.")

                End Select

                rgb.R(x, y) = r
                rgb.G(x, y) = g
                rgb.B(x, y) = b

            Next
        Next

        Return rgb

    End Function


    Private Function Sample(img As Image16,
                            x As Integer,
                            y As Integer) As UShort

        x = Math.Max(0, Math.Min(img.Width - 1, x))
        y = Math.Max(0, Math.Min(img.Height - 1, y))

        Return img(x, y)

    End Function


    Private Function Avg2(img As Image16,
                          x1 As Integer, y1 As Integer,
                          x2 As Integer, y2 As Integer) As UShort

        Return CUShort(
            (CInt(Sample(img, x1, y1)) +
             CInt(Sample(img, x2, y2))) \ 2)

    End Function


    Private Function Avg4(img As Image16,
                          x1 As Integer, y1 As Integer,
                          x2 As Integer, y2 As Integer,
                          x3 As Integer, y3 As Integer,
                          x4 As Integer, y4 As Integer) As UShort

        Return CUShort(
            (CInt(Sample(img, x1, y1)) +
             CInt(Sample(img, x2, y2)) +
             CInt(Sample(img, x3, y3)) +
             CInt(Sample(img, x4, y4))) \ 4)

    End Function

End Module