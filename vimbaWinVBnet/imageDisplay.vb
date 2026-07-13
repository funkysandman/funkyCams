Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices


Public Module ImageDisplay


    ''' <summary>
    ''' Converts a 16-bit RGB image into an 8-bit display bitmap.
    ''' </summary>
    Public Function CreateBitmap(
        image As RgbImage16,
        blackPoint As UShort,
        whitePoint As UShort) As Bitmap


        If whitePoint <= blackPoint Then
            Throw New ArgumentException(
                "White point must be greater than black point.")
        End If


        Dim bmp As New Bitmap(
            image.Width,
            image.Height,
            PixelFormat.Format24bppRgb)


        Dim bitmapData =
            bmp.LockBits(
                New Rectangle(
                    0,
                    0,
                    image.Width,
                    image.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format24bppRgb)


        Dim stride = bitmapData.Stride

        Dim bytes(stride * image.Height - 1) As Byte


        For y = 0 To image.Height - 1

            For x = 0 To image.Width - 1


                Dim index =
                    y * image.Width + x


                Dim displayIndex =
                    y * stride + x * 3


                bytes(displayIndex + 0) =
    ScaleToByte(image.B.Data(index),
                blackPoint,
                whitePoint)

                bytes(displayIndex + 1) =
    ScaleToByte(image.G.Data(index),
                blackPoint,
                whitePoint)

                bytes(displayIndex + 2) =
    ScaleToByte(image.R.Data(index),
                blackPoint,
                whitePoint)

            Next

        Next


        Marshal.Copy(
            bytes,
            0,
            bitmapData.Scan0,
            bytes.Length)


        bmp.UnlockBits(bitmapData)


        Return bmp

    End Function



    ''' <summary>
    ''' Converts a single 16-bit image to grayscale bitmap.
    ''' </summary>
    Public Function CreateBitmap(
        image As Image16,
        blackPoint As UShort,
        whitePoint As UShort) As Bitmap


        Dim bmp As New Bitmap(
            image.Width,
            image.Height,
            PixelFormat.Format8bppIndexed)


        Dim palette = bmp.Palette

        For i = 0 To 255
            palette.Entries(i) =
                Color.FromArgb(i, i, i)
        Next

        bmp.Palette = palette


        Dim bitmapData =
            bmp.LockBits(
                New Rectangle(
                    0,
                    0,
                    image.Width,
                    image.Height),
                ImageLockMode.WriteOnly,
                bmp.PixelFormat)


        Dim stride = bitmapData.Stride

        Dim bytes(stride * image.Height - 1) As Byte


        For y = 0 To image.Height - 1

            For x = 0 To image.Width - 1

                bytes(y * stride + x) =
                    ScaleToByte(
                        image(x, y),
                        blackPoint,
                        whitePoint)

            Next

        Next


        Marshal.Copy(
            bytes,
            0,
            bitmapData.Scan0,
            bytes.Length)


        bmp.UnlockBits(bitmapData)


        Return bmp

    End Function



    ''' <summary>
    ''' Linear 16-bit to 8-bit conversion.
    ''' </summary>
    Private Function ScaleToByte(
        value As UShort,
        blackPoint As UShort,
        whitePoint As UShort) As Byte


        If value <= blackPoint Then
            Return 0
        End If


        If value >= whitePoint Then
            Return 255
        End If


        Dim scaled =
            (value - blackPoint) /
            CDbl(whitePoint - blackPoint) *
            255.0


        Return CByte(scaled)

    End Function


End Module