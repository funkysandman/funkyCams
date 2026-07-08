Option Strict On
Option Explicit On

Public Module ImageResampler

    ''' <summary>
    ''' Resamples an image using bilinear interpolation.
    ''' </summary>
    Public Function Resize(
        source As Image16,
        scale As Double) As Image16

        If scale <= 0 Then
            Throw New ArgumentOutOfRangeException(NameOf(scale))
        End If

        Dim newWidth As Integer = Math.Max(1, CInt(Math.Round(source.Width * scale)))
        Dim newHeight As Integer = Math.Max(1, CInt(Math.Round(source.Height * scale)))

        Return Resize(source, newWidth, newHeight)

    End Function


    ''' <summary>
    ''' Resamples an image using bilinear interpolation.
    ''' </summary>
    Public Function Resize(
        source As Image16,
        newWidth As Integer,
        newHeight As Integer) As Image16

        Dim dest As New Image16(newWidth, newHeight, source.MaxValue)

        Dim xScale As Double = source.Width / CDbl(newWidth)
        Dim yScale As Double = source.Height / CDbl(newHeight)

        For y = 0 To newHeight - 1

            Dim srcY = (y + 0.5) * yScale - 0.5

            Dim y0 = CInt(Math.Floor(srcY))
            Dim y1 = y0 + 1

            Dim fy = srcY - y0

            If y0 < 0 Then
                y0 = 0
                fy = 0
            End If

            If y1 >= source.Height Then
                y1 = source.Height - 1
            End If

            For x = 0 To newWidth - 1

                Dim srcX = (x + 0.5) * xScale - 0.5

                Dim x0 = CInt(Math.Floor(srcX))
                Dim x1 = x0 + 1

                Dim fx = srcX - x0

                If x0 < 0 Then
                    x0 = 0
                    fx = 0
                End If

                If x1 >= source.Width Then
                    x1 = source.Width - 1
                End If

                Dim p00 = source.Data(y0 * source.Width + x0)
                Dim p10 = source.Data(y0 * source.Width + x1)
                Dim p01 = source.Data(y1 * source.Width + x0)
                Dim p11 = source.Data(y1 * source.Width + x1)

                Dim value =
                    p00 * (1 - fx) * (1 - fy) +
                    p10 * fx * (1 - fy) +
                    p01 * (1 - fx) * fy +
                    p11 * fx * fy

                dest.Data(y * newWidth + x) = CUShort(Math.Round(value))

            Next
        Next

        Return dest

    End Function


    ''' <summary>
    ''' Returns the centered intersection of two images.
    ''' </summary>
    Public Sub CenterIntersection(
        image1 As Image16,
        image2 As Image16,
        ByRef crop1 As Image16,
        ByRef crop2 As Image16)

        Dim w = Math.Min(image1.Width, image2.Width)
        Dim h = Math.Min(image1.Height, image2.Height)

        crop1 = CropCenter(image1, w, h)
        crop2 = CropCenter(image2, w, h)

    End Sub


    ''' <summary>
    ''' Crops an image about its center.
    ''' </summary>
    Public Function CropCenter(
        source As Image16,
        width As Integer,
        height As Integer) As Image16

        If width > source.Width OrElse
           height > source.Height Then

            Throw New ArgumentException("Crop exceeds source image.")

        End If

        Dim startX = (source.Width - width) \ 2
        Dim startY = (source.Height - height) \ 2

        Dim dest As New Image16(width, height, source.MaxValue)

        For y = 0 To height - 1

            Array.Copy(
                source.Data,
                (startY + y) * source.Width + startX,
                dest.Data,
                y * width,
                width)

        Next

        Return dest

    End Function


    ''' <summary>
    ''' Copies a translated region from an image.
    ''' Areas outside the image become zero.
    ''' Positive offsets move the image right/down.
    ''' </summary>
    Public Function Translate(
        source As Image16,
        offsetX As Integer,
        offsetY As Integer) As Image16

        Dim dest As New Image16(source.Width, source.Height, source.MaxValue)

        For y = 0 To source.Height - 1

            Dim srcY = y - offsetY

            If srcY < 0 OrElse srcY >= source.Height Then
                Continue For
            End If

            For x = 0 To source.Width - 1

                Dim srcX = x - offsetX

                If srcX < 0 OrElse srcX >= source.Width Then
                    Continue For
                End If

                dest(x, y) = source(srcX, srcY)

            Next
        Next

        Return dest

    End Function


    ''' <summary>
    ''' Calculates the resampling scale required to match
    ''' one camera's angular pixel scale to another.
    ''' </summary>
    Public Function CalculateScale(
        sourcePixelSizeUm As Double,
        sourceFocalLengthMm As Double,
        targetPixelSizeUm As Double,
        targetFocalLengthMm As Double) As Double

        Return (targetPixelSizeUm / targetFocalLengthMm) /
               (sourcePixelSizeUm / sourceFocalLengthMm)

    End Function

    Public Sub GetIntersection(
    mono As Image16,
    rgb As RgbImage16,
    ByRef monoOut As Image16,
    ByRef rgbOut As RgbImage16)


        Dim width =
            Math.Min(mono.Width, rgb.Width)

        Dim height =
            Math.Min(mono.Height, rgb.Height)


        monoOut =
            CropCenter(
                mono,
                width,
                height)


        rgbOut =
            New RgbImage16(
                CropCenter(rgb.R, width, height),
                CropCenter(rgb.G, width, height),
                CropCenter(rgb.B, width, height))

    End Sub

End Module