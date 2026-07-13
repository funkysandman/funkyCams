'Public Structure Image16
'    Public Data() As UShort
'    Public Width As Integer
'    Public Height As Integer

'    Public Sub New(data() As UShort, width As Integer, height As Integer)
'        Me.Data = data
'        Me.Width = width
'        Me.Height = height
'    End Sub
'End Structure



'Public Module FusionModule


'    Public Function FuseCentered(
'        mono As Image16,
'        color As Image16,
'        monoPixelSizeUm As Double,
'        colorPixelSizeUm As Double,
'        monoFocalLengthMm As Double,
'        colorFocalLengthMm As Double,
'        monoWeight As Double) As Image16


'        '-------------------------------------------------------
'        ' Calculate angular pixel scale ratio
'        '
'        ' Smaller number means mono must be reduced
'        '-------------------------------------------------------

'        Dim monoAngularPixel =
'            monoPixelSizeUm / monoFocalLengthMm

'        Dim colorAngularPixel =
'            colorPixelSizeUm / colorFocalLengthMm


'        Dim monoScale =
'            colorAngularPixel / monoAngularPixel


'        ' Resample mono onto color camera grid
'        'Dim scaledMono =
'        '    Resize16(
'        '        mono,
'        '        mono.Width * monoScale,
'        '        mono.Height * monoScale)

'        Dim scale =
'    (colorPixelSizeUm / colorFocalLengthMm) /
'    (monoPixelSizeUm / monoFocalLengthMm)


'        Dim scaledMono = Resize16(mono, scale)

'        '-------------------------------------------------------
'        ' Find centered intersection
'        '-------------------------------------------------------

'        Dim outWidth =
'            Math.Min(scaledMono.Width, color.Width)

'        Dim outHeight =
'            Math.Min(scaledMono.Height, color.Height)


'        Dim monoX =
'            (scaledMono.Width - outWidth) \ 2

'        Dim monoY =
'            (scaledMono.Height - outHeight) \ 2


'        Dim colorX =
'            (color.Width - outWidth) \ 2

'        Dim colorY =
'            (color.Height - outHeight) \ 2



'        Dim output(outWidth * outHeight - 1) As UShort



'        For y = 0 To outHeight - 1

'            For x = 0 To outWidth - 1


'                Dim monoIndex =
'                    (y + monoY) * scaledMono.Width +
'                    (x + monoX)


'                Dim colorIndex =
'                    (y + colorY) * color.Width +
'                    (x + colorX)


'                Dim outputIndex =
'                    y * outWidth + x



'                output(outputIndex) =
'                    Blend(
'                        color.Data(colorIndex),
'                        scaledMono.Data(monoIndex),
'                        monoWeight)


'            Next

'        Next


'        Return New Image16(
'            output,
'            outWidth,
'            outHeight)

'    End Function




'    Private Function Resize16(
'        input As Image16,
'        newWidth As Double,
'        newHeight As Double) As Image16


'        Dim w = CInt(newWidth)
'        Dim h = CInt(newHeight)


'        Dim output(w * h - 1) As UShort



'        Dim scaleX =
'            input.Width / CDbl(w)

'        Dim scaleY =
'            input.Height / CDbl(h)



'        For y = 0 To h - 1

'            For x = 0 To w - 1


'                Dim srcX =
'                    CInt(x * scaleX)

'                Dim srcY =
'                    CInt(y * scaleY)


'                If srcX >= input.Width Then srcX = input.Width - 1
'                If srcY >= input.Height Then srcY = input.Height - 1


'                output(y * w + x) =
'                    input.Data(srcY * input.Width + srcX)

'            Next

'        Next


'        Return New Image16(output, w, h)

'    End Function




'    Private Function Blend(
'        color As UShort,
'        mono As UShort,
'        weight As Double) As UShort


'        Dim value =
'            CDbl(color) * (1 - weight) +
'            CDbl(mono) * weight


'        If value < 0 Then value = 0
'        If value > 65535 Then value = 65535


'        Return CUShort(value)

'    End Function
'    Private Function Resize16(
'    input As Image16,
'    scale As Double) As Image16


'        Dim newWidth As Integer =
'            CInt(input.Width * scale)

'        Dim newHeight As Integer =
'            CInt(input.Height * scale)


'        Dim output(newWidth * newHeight - 1) As UShort


'        Dim xRatio =
'            input.Width / CDbl(newWidth)

'        Dim yRatio =
'            input.Height / CDbl(newHeight)


'        For y = 0 To newHeight - 1

'            Dim srcY =
'                y * yRatio

'            Dim y0 =
'                CInt(Math.Floor(srcY))

'            Dim y1 =
'                Math.Min(y0 + 1, input.Height - 1)

'            Dim fy =
'                srcY - y0


'            For x = 0 To newWidth - 1

'                Dim srcX =
'                    x * xRatio

'                Dim x0 =
'                    CInt(Math.Floor(srcX))

'                Dim x1 =
'                    Math.Min(x0 + 1, input.Width - 1)

'                Dim fx =
'                    srcX - x0


'                Dim p00 =
'                    input.Data(y0 * input.Width + x0)

'                Dim p10 =
'                    input.Data(y0 * input.Width + x1)

'                Dim p01 =
'                    input.Data(y1 * input.Width + x0)

'                Dim p11 =
'                    input.Data(y1 * input.Width + x1)


'                Dim value =
'                    p00 * (1 - fx) * (1 - fy) +
'                    p10 * fx * (1 - fy) +
'                    p01 * (1 - fx) * fy +
'                    p11 * fx * fy


'                output(y * newWidth + x) =
'                    CUShort(value)

'            Next
'        Next


'        Return New Image16(
'            output,
'            newWidth,
'            newHeight)

'    End Function

'End Module