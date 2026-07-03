Public Class frmFusion
    Private m_pointGreyForm As frmPointGrey
    Private m_scoutForm As frmScout

    Private Sub frmFusion_Load(sender As Object, e As EventArgs) Handles Me.Load
        m_pointGreyForm = New frmPointGrey()
        m_scoutForm = New frmScout()

        HostChildForm(m_pointGreyForm, pnlPointGrey)
        HostChildForm(m_scoutForm, pnlScout)
    End Sub

    Private Sub HostChildForm(child As Form, hostPanel As Panel)
        child.TopLevel = False
        child.FormBorderStyle = FormBorderStyle.None
        child.Dock = DockStyle.Fill
        hostPanel.Controls.Add(child)
        child.Show()
    End Sub

    Private Sub btnCombine_Click(sender As Object, e As EventArgs) Handles btnCombine.Click
        Dim pointGreyImage As Bitmap = Nothing
        Dim scoutImage As Bitmap = Nothing
        Dim colorFusionBitmap As Bitmap = Nothing
        Dim displayBitmap As Bitmap = Nothing

        Try
            If Not m_pointGreyForm.TryGetLatestRawBitmap(pointGreyImage) Then
                MessageBox.Show("Point Grey has no frame available yet.")
                Return
            End If

            If Not m_scoutForm.TryGetLatestRawBitmap(scoutImage) Then
                MessageBox.Show("Scout has no frame available yet.")
                Return
            End If

            Dim offsetX As Integer = 0
            Dim offsetY As Integer = 0
            Integer.TryParse(Me.tbX.Text, offsetX)
            Integer.TryParse(Me.tbY.Text, offsetY)

            colorFusionBitmap = CreateColorFusionBitmap(pointGreyImage, scoutImage, offsetX, offsetY)
            If colorFusionBitmap Is Nothing Then
                MessageBox.Show("Unable to produce color fusion preview.")
                Return
            End If

            displayBitmap = CombineThreeImages(pointGreyImage, colorFusionBitmap, scoutImage)
            If displayBitmap Is Nothing Then
                MessageBox.Show("Unable to compose preview images.")
                Return
            End If

            If picFusion.Image IsNot Nothing Then
                picFusion.Image.Dispose()
            End If

            picFusion.Image = displayBitmap
            displayBitmap = Nothing

            Dim savePath As String = System.IO.Path.Combine(Application.StartupPath, "fusion_image.png")
            picFusion.Image.Save(savePath, System.Drawing.Imaging.ImageFormat.Png)

        Finally
            If displayBitmap IsNot Nothing Then
                displayBitmap.Dispose()
            End If

            If colorFusionBitmap IsNot Nothing Then
                colorFusionBitmap.Dispose()
            End If

            If pointGreyImage IsNot Nothing Then
                pointGreyImage.Dispose()
            End If

            If scoutImage IsNot Nothing Then
                scoutImage.Dispose()
            End If
        End Try
    End Sub
    Private Function CreateColorFusionBitmap(pointGreySource As Bitmap,
                                             scoutSource As Bitmap,
                                             offsetX As Integer,
                                             offsetY As Integer) As Bitmap
        If pointGreySource Is Nothing OrElse scoutSource Is Nothing Then
            Return Nothing
        End If

        If pointGreySource.Width <= 2 OrElse pointGreySource.Height <= 2 OrElse scoutSource.Width <= 2 OrElse scoutSource.Height <= 2 Then
            Return Nothing
        End If

        Const pgPixelSizeMicrons As Double = 5.86
        Const scoutPixelSizeMicrons As Double = 6.45

        Dim intersectionPhysicalWidthMicrons As Double = Math.Min(pointGreySource.Width * pgPixelSizeMicrons, scoutSource.Width * scoutPixelSizeMicrons)
        Dim intersectionPhysicalHeightMicrons As Double = Math.Min(pointGreySource.Height * pgPixelSizeMicrons, scoutSource.Height * scoutPixelSizeMicrons)

        Dim pgIntersectionWidth As Integer = CInt(Math.Floor(intersectionPhysicalWidthMicrons / pgPixelSizeMicrons))
        Dim pgIntersectionHeight As Integer = CInt(Math.Floor(intersectionPhysicalHeightMicrons / pgPixelSizeMicrons))
        Dim scoutIntersectionWidth As Integer = CInt(Math.Floor(intersectionPhysicalWidthMicrons / scoutPixelSizeMicrons))
        Dim scoutIntersectionHeight As Integer = CInt(Math.Floor(intersectionPhysicalHeightMicrons / scoutPixelSizeMicrons))

        If pgIntersectionWidth <= 0 OrElse pgIntersectionHeight <= 0 OrElse scoutIntersectionWidth <= 0 OrElse scoutIntersectionHeight <= 0 Then
            Return Nothing
        End If

        Dim pointGreyCrop As Bitmap = Nothing
        Dim scoutCrop As Bitmap = Nothing
        Dim scoutResized As Bitmap = Nothing

        Try
            pointGreyCrop = CropCenteredBitmap(pointGreySource, pgIntersectionWidth, pgIntersectionHeight)
            scoutCrop = CropCenteredBitmap(scoutSource, scoutIntersectionWidth, scoutIntersectionHeight)

            If pointGreyCrop Is Nothing OrElse scoutCrop Is Nothing Then
                Return Nothing
            End If

            scoutResized = ResizeBitmapTo24bpp(scoutCrop, pgIntersectionWidth, pgIntersectionHeight)
            If scoutResized Is Nothing Then
                Return Nothing
            End If

            Return FuseColorWithRegisteredMono(pointGreyCrop, scoutResized, offsetX, offsetY)
        Finally
            If pointGreyCrop IsNot Nothing Then
                pointGreyCrop.Dispose()
            End If

            If scoutCrop IsNot Nothing Then
                scoutCrop.Dispose()
            End If

            If scoutResized IsNot Nothing Then
                scoutResized.Dispose()
            End If
        End Try
    End Function

    Private Function CropCenteredBitmap(source As Bitmap, targetWidth As Integer, targetHeight As Integer) As Bitmap
        If source Is Nothing OrElse targetWidth <= 0 OrElse targetHeight <= 0 Then
            Return Nothing
        End If

        If source.Width < targetWidth OrElse source.Height < targetHeight Then
            Return Nothing
        End If

        Dim pixelFormat As System.Drawing.Imaging.PixelFormat = source.PixelFormat
        If pixelFormat <> System.Drawing.Imaging.PixelFormat.Format24bppRgb AndAlso pixelFormat <> System.Drawing.Imaging.PixelFormat.Format8bppIndexed Then
            Return Nothing
        End If

        Dim cropX As Integer = (source.Width - targetWidth) \ 2
        Dim cropY As Integer = (source.Height - targetHeight) \ 2
        Dim cropRect As New Rectangle(cropX, cropY, targetWidth, targetHeight)
        Dim output As New Bitmap(targetWidth, targetHeight, pixelFormat)

        If pixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed Then
            output.Palette = source.Palette
        End If

        Dim sourceData As System.Drawing.Imaging.BitmapData = Nothing
        Dim outputData As System.Drawing.Imaging.BitmapData = Nothing

        Try
            sourceData = source.LockBits(cropRect, System.Drawing.Imaging.ImageLockMode.ReadOnly, pixelFormat)
            outputData = output.LockBits(New Rectangle(0, 0, targetWidth, targetHeight), System.Drawing.Imaging.ImageLockMode.WriteOnly, pixelFormat)

            Dim bytesPerPixel As Integer = If(pixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb, 3, 1)
            Dim rowBytes As Integer = targetWidth * bytesPerPixel
            Dim rowBuffer(rowBytes - 1) As Byte

            For y As Integer = 0 To targetHeight - 1
                Dim srcRowPtr As IntPtr = IntPtr.Add(sourceData.Scan0, y * sourceData.Stride)
                Dim dstRowPtr As IntPtr = IntPtr.Add(outputData.Scan0, y * outputData.Stride)
                System.Runtime.InteropServices.Marshal.Copy(srcRowPtr, rowBuffer, 0, rowBytes)
                System.Runtime.InteropServices.Marshal.Copy(rowBuffer, 0, dstRowPtr, rowBytes)
            Next
        Finally
            If sourceData IsNot Nothing Then
                source.UnlockBits(sourceData)
            End If

            If outputData IsNot Nothing Then
                output.UnlockBits(outputData)
            End If
        End Try

        Return output
    End Function

    Private Function ResizeBitmapTo24bpp(source As Bitmap, targetWidth As Integer, targetHeight As Integer) As Bitmap
        If source Is Nothing OrElse targetWidth <= 0 OrElse targetHeight <= 0 Then
            Return Nothing
        End If

        Dim output As New Bitmap(targetWidth, targetHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
        Using g As Graphics = Graphics.FromImage(output)
            g.Clear(System.Drawing.Color.Black)
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality
            g.DrawImage(source, New Rectangle(0, 0, targetWidth, targetHeight))
        End Using

        Return output
    End Function

    Private Function FuseColorWithRegisteredMono(colorSource As Bitmap,
                                                  monoSource As Bitmap,
                                                  offsetX As Integer,
                                                  offsetY As Integer) As Bitmap
        If colorSource Is Nothing OrElse monoSource Is Nothing Then
            Return Nothing
        End If

        If colorSource.Width <> monoSource.Width OrElse colorSource.Height <> monoSource.Height Then
            Return Nothing
        End If

        If colorSource.PixelFormat <> System.Drawing.Imaging.PixelFormat.Format24bppRgb OrElse monoSource.PixelFormat <> System.Drawing.Imaging.PixelFormat.Format24bppRgb Then
            Return Nothing
        End If

        Dim width As Integer = colorSource.Width
        Dim height As Integer = colorSource.Height
        Dim output As New Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb)

        Dim colorData As System.Drawing.Imaging.BitmapData = Nothing
        Dim monoData As System.Drawing.Imaging.BitmapData = Nothing
        Dim outputData As System.Drawing.Imaging.BitmapData = Nothing

        Try
            colorData = colorSource.LockBits(New Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, colorSource.PixelFormat)
            monoData = monoSource.LockBits(New Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.ReadOnly, monoSource.PixelFormat)
            outputData = output.LockBits(New Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, output.PixelFormat)

            Dim colorBytes((colorData.Stride * height) - 1) As Byte
            Dim monoBytes((monoData.Stride * height) - 1) As Byte
            Dim outputBytes((outputData.Stride * height) - 1) As Byte

            System.Runtime.InteropServices.Marshal.Copy(colorData.Scan0, colorBytes, 0, colorBytes.Length)
            System.Runtime.InteropServices.Marshal.Copy(monoData.Scan0, monoBytes, 0, monoBytes.Length)

            For y As Integer = 0 To height - 1
                For x As Integer = 0 To width - 1
                    Dim colorOffset As Integer = (y * colorData.Stride) + (x * 3)
                    Dim outputOffset As Integer = (y * outputData.Stride) + (x * 3)

                    Dim srcB As Double = colorBytes(colorOffset)
                    Dim srcG As Double = colorBytes(colorOffset + 1)
                    Dim srcR As Double = colorBytes(colorOffset + 2)

                    Dim cb As Double = 128.0 - (0.168736 * srcR) - (0.331264 * srcG) + (0.5 * srcB)
                    Dim cr As Double = 128.0 + (0.5 * srcR) - (0.418688 * srcG) - (0.081312 * srcB)

                    Dim shiftedMonoX As Integer = x - offsetX
                    Dim shiftedMonoY As Integer = y - offsetY
                    Dim yValue As Double = (0.299 * srcR) + (0.587 * srcG) + (0.114 * srcB)

                    If shiftedMonoX >= 0 AndAlso shiftedMonoX < width AndAlso shiftedMonoY >= 0 AndAlso shiftedMonoY < height Then
                        Dim monoOffset As Integer = (shiftedMonoY * monoData.Stride) + (shiftedMonoX * 3)
                        yValue = (CDbl(monoBytes(monoOffset)) + CDbl(monoBytes(monoOffset + 1)) + CDbl(monoBytes(monoOffset + 2))) / 3.0
                    End If

                    Dim outR As Integer = CInt(Math.Round(yValue + (1.402 * (cr - 128.0))))
                    Dim outG As Integer = CInt(Math.Round(yValue - (0.344136 * (cb - 128.0)) - (0.714136 * (cr - 128.0))))
                    Dim outB As Integer = CInt(Math.Round(yValue + (1.772 * (cb - 128.0))))

                    If outR < 0 Then outR = 0
                    If outR > 255 Then outR = 255
                    If outG < 0 Then outG = 0
                    If outG > 255 Then outG = 255
                    If outB < 0 Then outB = 0
                    If outB > 255 Then outB = 255

                    outputBytes(outputOffset) = CByte(outB)
                    outputBytes(outputOffset + 1) = CByte(outG)
                    outputBytes(outputOffset + 2) = CByte(outR)
                Next
            Next

            System.Runtime.InteropServices.Marshal.Copy(outputBytes, 0, outputData.Scan0, outputBytes.Length)
        Finally
            If colorData IsNot Nothing Then
                colorSource.UnlockBits(colorData)
            End If

            If monoData IsNot Nothing Then
                monoSource.UnlockBits(monoData)
            End If

            If outputData IsNot Nothing Then
                output.UnlockBits(outputData)
            End If
        End Try

        Return output
    End Function

    Private Sub GetBayerRgbAt(source() As UShort,
                          width As Integer,
                          height As Integer,
                          x As Integer,
                          y As Integer,
                          minValue As UShort,
                          valueRange As Integer,
                          ByRef r As Integer,
                          ByRef g As Integer,
                          ByRef b As Integer)

        Dim isEvenRow As Boolean = (y Mod 2 = 0)
        Dim isEvenCol As Boolean = (x Mod 2 = 0)

        If isEvenRow AndAlso isEvenCol Then
            r = SampleBayer(source, width, height, x, y, minValue, valueRange)
            g = (SampleBayer(source, width, height, x - 1, y, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y, minValue, valueRange) +
             SampleBayer(source, width, height, x, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x, y + 1, minValue, valueRange)) \ 4
            b = (SampleBayer(source, width, height, x - 1, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x - 1, y + 1, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y + 1, minValue, valueRange)) \ 4

        ElseIf (Not isEvenRow) AndAlso (Not isEvenCol) Then
            b = SampleBayer(source, width, height, x, y, minValue, valueRange)
            g = (SampleBayer(source, width, height, x - 1, y, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y, minValue, valueRange) +
             SampleBayer(source, width, height, x, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x, y + 1, minValue, valueRange)) \ 4
            r = (SampleBayer(source, width, height, x - 1, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x - 1, y + 1, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y + 1, minValue, valueRange)) \ 4

        ElseIf isEvenRow AndAlso (Not isEvenCol) Then
            g = SampleBayer(source, width, height, x, y, minValue, valueRange)
            r = (SampleBayer(source, width, height, x - 1, y, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y, minValue, valueRange)) \ 2
            b = (SampleBayer(source, width, height, x, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x, y + 1, minValue, valueRange)) \ 2

        Else
            g = SampleBayer(source, width, height, x, y, minValue, valueRange)
            r = (SampleBayer(source, width, height, x, y - 1, minValue, valueRange) +
             SampleBayer(source, width, height, x, y + 1, minValue, valueRange)) \ 2
            b = (SampleBayer(source, width, height, x - 1, y, minValue, valueRange) +
             SampleBayer(source, width, height, x + 1, y, minValue, valueRange)) \ 2
        End If
    End Sub

    Private Function SampleBayer(source() As UShort,
                             width As Integer,
                             height As Integer,
                             x As Integer,
                             y As Integer,
                             minValue As UShort,
                             valueRange As Integer) As Integer
        If x < 0 Then x = 0
        If y < 0 Then y = 0
        If x >= width Then x = width - 1
        If y >= height Then y = height - 1

        Dim raw As Integer = CInt(source((y * width) + x))
        Dim normalized As Integer = (raw - CInt(minValue)) * 255 \ valueRange

        If normalized < 0 Then normalized = 0
        If normalized > 255 Then normalized = 255

        Return normalized
    End Function
    Private Function CreateCenteredIntersectionArrays(pgImageArray() As UShort,
                                                      pgWidth As Integer,
                                                      pgHeight As Integer,
                                                      scoutImageArray() As UShort,
                                                      scoutWidth As Integer,
                                                      scoutHeight As Integer,
                                                      ByRef pgIntersectionArray() As UShort,
                                                      ByRef scoutIntersectionArray() As UShort,
                                                      ByRef pgIntersectionWidth As Integer,
                                                      ByRef pgIntersectionHeight As Integer,
                                                      ByRef scoutIntersectionWidth As Integer,
                                                      ByRef scoutIntersectionHeight As Integer) As Boolean
        If pgImageArray Is Nothing OrElse scoutImageArray Is Nothing Then
            Return False
        End If

        If pgWidth <= 0 OrElse pgHeight <= 0 OrElse scoutWidth <= 0 OrElse scoutHeight <= 0 Then
            Return False
        End If

        If pgImageArray.Length < (pgWidth * pgHeight) OrElse scoutImageArray.Length < (scoutWidth * scoutHeight) Then
            Return False
        End If

        Const pgPixelSizeMicrons As Double = 5.86
        Const scoutPixelSizeMicrons As Double = 6.45

        Dim intersectionPhysicalWidthMicrons As Double = Math.Min(pgWidth * pgPixelSizeMicrons, scoutWidth * scoutPixelSizeMicrons)
        Dim intersectionPhysicalHeightMicrons As Double = Math.Min(pgHeight * pgPixelSizeMicrons, scoutHeight * scoutPixelSizeMicrons)

        pgIntersectionWidth = CInt(Math.Floor(intersectionPhysicalWidthMicrons / pgPixelSizeMicrons))
        pgIntersectionHeight = CInt(Math.Floor(intersectionPhysicalHeightMicrons / pgPixelSizeMicrons))
        scoutIntersectionWidth = CInt(Math.Floor(intersectionPhysicalWidthMicrons / scoutPixelSizeMicrons))
        scoutIntersectionHeight = CInt(Math.Floor(intersectionPhysicalHeightMicrons / scoutPixelSizeMicrons))

        If pgIntersectionWidth <= 0 OrElse pgIntersectionHeight <= 0 OrElse scoutIntersectionWidth <= 0 OrElse scoutIntersectionHeight <= 0 Then
            Return False
        End If

        Dim pgStartX As Integer = (pgWidth - pgIntersectionWidth) \ 2
        Dim pgStartY As Integer = (pgHeight - pgIntersectionHeight) \ 2
        Dim scoutStartX As Integer = (scoutWidth - scoutIntersectionWidth) \ 2
        Dim scoutStartY As Integer = (scoutHeight - scoutIntersectionHeight) \ 2

        ReDim pgIntersectionArray((pgIntersectionWidth * pgIntersectionHeight) - 1)
        ReDim scoutIntersectionArray((scoutIntersectionWidth * scoutIntersectionHeight) - 1)

        For y As Integer = 0 To pgIntersectionHeight - 1
            Dim srcOffset As Integer = ((pgStartY + y) * pgWidth) + pgStartX
            Dim dstOffset As Integer = y * pgIntersectionWidth
            Array.Copy(pgImageArray, srcOffset, pgIntersectionArray, dstOffset, pgIntersectionWidth)
        Next

        For y As Integer = 0 To scoutIntersectionHeight - 1
            Dim srcOffset As Integer = ((scoutStartY + y) * scoutWidth) + scoutStartX
            Dim dstOffset As Integer = y * scoutIntersectionWidth
            Array.Copy(scoutImageArray, srcOffset, scoutIntersectionArray, dstOffset, scoutIntersectionWidth)
        Next

        Return True
    End Function

    Private Function ResampleUShortArray(source() As UShort,
                                     sourceWidth As Integer,
                                     sourceHeight As Integer,
                                     targetWidth As Integer,
                                     targetHeight As Integer,
                                     Optional offsetX As Integer = 0,
                                     Optional offsetY As Integer = 0) As UShort()

        If source Is Nothing OrElse sourceWidth <= 0 OrElse sourceHeight <= 0 OrElse targetWidth <= 0 OrElse targetHeight <= 0 Then
            Return Nothing
        End If

        Dim output((targetWidth * targetHeight) - 1) As UShort

        For y As Integer = 0 To targetHeight - 1
            Dim srcY As Integer = CInt(Math.Floor((CDbl(y) * sourceHeight) / targetHeight)) - offsetY
            If srcY < 0 OrElse srcY >= sourceHeight Then Continue For

            For x As Integer = 0 To targetWidth - 1
                Dim srcX As Integer = CInt(Math.Floor((CDbl(x) * sourceWidth) / targetWidth)) - offsetX
                If srcX < 0 OrElse srcX >= sourceWidth Then Continue For

                output((y * targetWidth) + x) = source((srcY * sourceWidth) + srcX)
            Next
        Next

        Return output
    End Function

    Private Function Scale12BitTo16Bit(source() As UShort) As UShort()
        If source Is Nothing Then
            Return Nothing
        End If

        Dim output(source.Length - 1) As UShort

        For i As Integer = 0 To source.Length - 1
            Dim scaled As Integer = (CInt(source(i)) * 65535) \ 4095
            If scaled < 0 Then scaled = 0
            If scaled > UShort.MaxValue Then scaled = UShort.MaxValue
            output(i) = CUShort(scaled)
        Next

        Return output
    End Function

    Private Function AverageUShortArrays(left() As UShort, right() As UShort) As UShort()
        If left Is Nothing OrElse right Is Nothing OrElse left.Length <> right.Length Then
            Return Nothing
        End If

        Dim output(left.Length - 1) As UShort
        For i As Integer = 0 To left.Length - 1
            output(i) = CUShort((CInt(left(i)) + CInt(right(i))) \ 2)
            ' output(i) = right(i)
        Next

        Return output
    End Function

    Private Function ConvertUShortArrayToBitmap(source() As UShort, width As Integer, height As Integer) As Bitmap
        If source Is Nothing OrElse width <= 0 OrElse height <= 0 OrElse source.Length < (width * height) Then
            Return Nothing
        End If

        Dim minValue As UShort = UShort.MaxValue
        Dim maxValue As UShort = UShort.MinValue

        For i As Integer = 0 To source.Length - 1
            If source(i) < minValue Then minValue = source(i)
            If source(i) > maxValue Then maxValue = source(i)
        Next

        Dim range As Integer = Math.Max(1, CInt(maxValue) - CInt(minValue))
        Dim bytes((width * height) - 1) As Byte

        For i As Integer = 0 To bytes.Length - 1
            Dim normalized As Integer = (CInt(source(i)) - CInt(minValue)) * 255 \ range
            If normalized < 0 Then normalized = 0
            If normalized > 255 Then normalized = 255
            bytes(i) = CByte(normalized)
        Next

        Dim output As New Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
        Dim pal = output.Palette
        For i As Integer = 0 To 255
            pal.Entries(i) = System.Drawing.Color.FromArgb(i, i, i)
        Next
        output.Palette = pal

        Dim bmpData As System.Drawing.Imaging.BitmapData = output.LockBits(New Rectangle(0, 0, width, height),
                                                                           System.Drawing.Imaging.ImageLockMode.WriteOnly,
                                                                           output.PixelFormat)

        For y As Integer = 0 To height - 1
            Dim srcOffset As Integer = y * width
            Dim dstPtr As IntPtr = IntPtr.Add(bmpData.Scan0, y * bmpData.Stride)
            System.Runtime.InteropServices.Marshal.Copy(bytes, srcOffset, dstPtr, width)
        Next

        output.UnlockBits(bmpData)
        Return output
    End Function

    Private Function DebayerBayerRg16ToBitmap(source() As UShort, width As Integer, height As Integer) As Bitmap
        If source Is Nothing OrElse width <= 2 OrElse height <= 2 OrElse source.Length < (width * height) Then
            Return Nothing
        End If

        Dim output As New Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb)

        Dim minValue As UShort = UShort.MaxValue
        Dim maxValue As UShort = UShort.MinValue

        For i As Integer = 0 To source.Length - 1
            If source(i) < minValue Then minValue = source(i)
            If source(i) > maxValue Then maxValue = source(i)
        Next

        Dim range As Integer = Math.Max(1, CInt(maxValue) - CInt(minValue))

        For y As Integer = 1 To height - 2
            For x As Integer = 1 To width - 2
                Dim idx As Integer = (y * width) + x

                Dim r As Integer
                Dim g As Integer
                Dim b As Integer

                Dim isEvenRow As Boolean = (y Mod 2 = 0)
                Dim isEvenCol As Boolean = (x Mod 2 = 0)

                If isEvenRow AndAlso isEvenCol Then
                    r = source(idx)
                    g = (CInt(source(idx - 1)) + CInt(source(idx + 1)) + CInt(source(idx - width)) + CInt(source(idx + width))) \ 4
                    b = (CInt(source(idx - width - 1)) + CInt(source(idx - width + 1)) + CInt(source(idx + width - 1)) + CInt(source(idx + width + 1))) \ 4
                ElseIf (Not isEvenRow) AndAlso (Not isEvenCol) Then
                    b = source(idx)
                    g = (CInt(source(idx - 1)) + CInt(source(idx + 1)) + CInt(source(idx - width)) + CInt(source(idx + width))) \ 4
                    r = (CInt(source(idx - width - 1)) + CInt(source(idx - width + 1)) + CInt(source(idx + width - 1)) + CInt(source(idx + width + 1))) \ 4
                ElseIf isEvenRow AndAlso (Not isEvenCol) Then
                    g = source(idx)
                    r = (CInt(source(idx - 1)) + CInt(source(idx + 1))) \ 2
                    b = (CInt(source(idx - width)) + CInt(source(idx + width))) \ 2
                Else
                    g = source(idx)
                    r = (CInt(source(idx - width)) + CInt(source(idx + width))) \ 2
                    b = (CInt(source(idx - 1)) + CInt(source(idx + 1))) \ 2
                End If

                r = (r - CInt(minValue)) * 255 \ range
                g = (g - CInt(minValue)) * 255 \ range
                b = (b - CInt(minValue)) * 255 \ range

                If r < 0 Then r = 0
                If r > 255 Then r = 255
                If g < 0 Then g = 0
                If g > 255 Then g = 255
                If b < 0 Then b = 0
                If b > 255 Then b = 255

                output.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b))
            Next
        Next

        Return output
    End Function

    Private Function CombineThreeImages(leftImage As Bitmap, middleImage As Bitmap, rightImage As Bitmap) As Bitmap
        If leftImage Is Nothing OrElse middleImage Is Nothing OrElse rightImage Is Nothing Then
            Return Nothing
        End If

        Dim outputWidth As Integer = leftImage.Width + middleImage.Width + rightImage.Width
        Dim outputHeight As Integer = Math.Max(leftImage.Height, Math.Max(middleImage.Height, rightImage.Height))
        Dim output As New Bitmap(outputWidth, outputHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb)

        Using g As Graphics = Graphics.FromImage(output)
            g.Clear(System.Drawing.Color.Black)
            g.DrawImage(leftImage, New Rectangle(0, 0, leftImage.Width, leftImage.Height))
            g.DrawImage(middleImage, New Rectangle(leftImage.Width, 0, middleImage.Width, middleImage.Height))
            g.DrawImage(rightImage, New Rectangle(leftImage.Width + middleImage.Width, 0, rightImage.Width, rightImage.Height))
        End Using

        Return output
    End Function

    Private Sub frmFusion_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If m_pointGreyForm IsNot Nothing AndAlso Not m_pointGreyForm.IsDisposed Then
            m_pointGreyForm.Close()
        End If

        If m_scoutForm IsNot Nothing AndAlso Not m_scoutForm.IsDisposed Then
            m_scoutForm.Close()
        End If

        If picFusion.Image IsNot Nothing Then
            picFusion.Image.Dispose()
            picFusion.Image = Nothing
        End If
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click

    End Sub
End Class
