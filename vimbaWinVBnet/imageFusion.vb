Option Strict On
Option Explicit On

Public Module ImageFusion


    ''' <summary>
    ''' Fuses mono luminance into a color RGB image.
    ''' Mono provides detail; RGB provides chroma.
    ''' </summary>
    Public Function FuseLuminance(
        mono As Image16,
        color As RgbImage16,
        monoWeight As Double,
        offsetX As Integer,
        offsetY As Integer) As RgbImage16


        If mono.Width <> color.Width OrElse
           mono.Height <> color.Height Then

            Throw New ArgumentException(
                "Images must have identical dimensions.")

        End If


        Dim output As New RgbImage16(
            color.Width,
            color.Height, mono.MaxValue)


        For y = 0 To color.Height - 1

            For x = 0 To color.Width - 1


                Dim mx = x + offsetX
                Dim my = y + offsetY


                If mx < 0 OrElse
                   my < 0 OrElse
                   mx >= mono.Width OrElse
                   my >= mono.Height Then

                    ' Outside mono area
                    output.R(x, y) = color.R(x, y)
                    output.G(x, y) = color.G(x, y)
                    output.B(x, y) = color.B(x, y)

                    Continue For

                End If


                Dim monoValue As Double =
                    mono(mx, my)


                Dim rValue As Double =
                    color.R(x, y)

                Dim gValue As Double =
                    color.G(x, y)

                Dim bValue As Double =
                    color.B(x, y)



                ' Calculate color luminance
                Dim luminance As Double =
                    0.2126 * rValue +
                    0.7152 * gValue +
                    0.0722 * bValue


                If luminance < 1 Then

                    output.R(x, y) = Clamp16(monoValue)
                    output.G(x, y) = Clamp16(monoValue)
                    output.B(x, y) = Clamp16(monoValue)

                    Continue For

                End If


                ' Blend mono into luminance
                Dim fusedLuminance =
                    luminance * (1 - monoWeight) +
                    monoValue * monoWeight


                ' Preserve chroma ratios

                output.R(x, y) =
                    Clamp16(
                        rValue *
                        fusedLuminance /
                        luminance)


                output.G(x, y) =
                    Clamp16(
                        gValue *
                        fusedLuminance /
                        luminance)


                output.B(x, y) =
                    Clamp16(
                        bValue *
                        fusedLuminance /
                        luminance)


            Next

        Next


        Return output

    End Function



    ''' <summary>
    ''' Simple RGB blend method.
    ''' Useful for testing only.
    ''' </summary>
    Public Function BlendRGB(
        mono As Image16,
        color As RgbImage16,
        weight As Double) As RgbImage16


        Dim output As New RgbImage16(
            color.Width,
            color.Height, color.MaxValue)


        For i = 0 To mono.PixelCount - 1

            output.R.Data(i) =
                Blend(color.R.Data(i),
                      mono.Data(i),
                      weight)

            output.G.Data(i) =
                Blend(color.G.Data(i),
                      mono.Data(i),
                      weight)

            output.B.Data(i) =
                Blend(color.B.Data(i),
                      mono.Data(i),
                      weight)

        Next


        Return output

    End Function



    Private Function Blend(
        color As UShort,
        mono As UShort,
        weight As Double) As UShort


        Dim value =
            CDbl(color) * (1 - weight) +
            CDbl(mono) * weight

        Return Clamp16(value)

    End Function



    Private Function Clamp16(
        value As Double) As UShort

        If value <= 0 Then Return 0
        If value >= 65535 Then Return 65535

        Return CUShort(value)

    End Function


End Module