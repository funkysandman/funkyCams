Imports System
Imports System.IO

Namespace AstroCalibration

    Public Class Frame
        Public Property Image As Single(,)
        Public Property Temperature As Single
        Public Property Gain As Single
        Public Property Exposure As Single
        Public Property Timestamp As DateTime
    End Class

    Public Module FrameStats

        Public Function Mean(img As Single(,)) As Single
            Dim sum As Double = 0
            Dim h = img.GetLength(0)
            Dim w = img.GetLength(1)

            For y = 0 To h - 1
                For x = 0 To w - 1
                    sum += img(y, x)
                Next
            Next

            Return CSng(sum / (h * w))
        End Function

        Public Function Std(img As Single(,), mean As Single) As Single
            Dim sum As Double = 0
            Dim h = img.GetLength(0)
            Dim w = img.GetLength(1)

            For y = 0 To h - 1
                For x = 0 To w - 1
                    Dim d = img(y, x) - mean
                    sum += d * d
                Next
            Next

            Return CSng(Math.Sqrt(sum / (h * w)))
        End Function

        Public Function HighFreqScore(img As Single(,)) As Single
            Dim h = img.GetLength(0)
            Dim w = img.GetLength(1)

            Dim sum As Double = 0

            For y = 1 To h - 2
                For x = 1 To w - 2
                    Dim v =
                        -4 * img(y, x) +
                        img(y - 1, x) +
                        img(y + 1, x) +
                        img(y, x - 1) +
                        img(y, x + 1)

                    sum += Math.Abs(v)
                Next
            Next

            Return CSng(sum / (h * w))
        End Function

    End Module

    Public Class FrameQualityGate
        Public Function IsCloudy(img As Single(,)) As Boolean
            Dim mean = FrameStats.Mean(img)
            Dim std = FrameStats.Std(img, mean)
            Dim hf = FrameStats.HighFreqScore(img)

            Return hf < 0.5F AndAlso std < 1.0F
        End Function
    End Class

    Public Class NightStateManager
        Private _stableCount As Integer = 0

        Public Function UpdateAndCheck(img As Single(,)) As Boolean
            Dim mean = FrameStats.Mean(img)
            Dim isNight = mean < 50.0F

            If isNight Then
                _stableCount += 1
            Else
                _stableCount = 0
            End If

            Return _stableCount > 200
        End Function
    End Class

    Public Class StatisticalCalibrator
        Private _base(,) As Single
        Private _tempCoeff(,) As Single
        Private _init As Boolean = False
        Private _count As Integer = 0

        Public Sub SaveState(writer As BinaryWriter)
            writer.Write(_init)
            writer.Write(_count)

            If Not _init OrElse _base Is Nothing OrElse _tempCoeff Is Nothing Then
                Return
            End If

            Dim h = _base.GetLength(0)
            Dim w = _base.GetLength(1)

            writer.Write(h)
            writer.Write(w)

            For y = 0 To h - 1
                For x = 0 To w - 1
                    writer.Write(_base(y, x))
                Next
            Next

            For y = 0 To h - 1
                For x = 0 To w - 1
                    writer.Write(_tempCoeff(y, x))
                Next
            Next
        End Sub

        Public Sub LoadState(reader As BinaryReader)
            Dim init = reader.ReadBoolean()
            Dim count = reader.ReadInt32()

            If Not init Then
                _init = False
                _count = 0
                _base = Nothing
                _tempCoeff = Nothing
                Return
            End If

            Dim h = reader.ReadInt32()
            Dim w = reader.ReadInt32()

            Dim baseArr(h - 1, w - 1) As Single
            Dim coeffArr(h - 1, w - 1) As Single

            For y = 0 To h - 1
                For x = 0 To w - 1
                    baseArr(y, x) = reader.ReadSingle()
                Next
            Next

            For y = 0 To h - 1
                For x = 0 To w - 1
                    coeffArr(y, x) = reader.ReadSingle()
                Next
            Next

            _base = baseArr
            _tempCoeff = coeffArr
            _count = Math.Max(0, count)
            _init = True
        End Sub

        Public Sub Update(frame As Frame)
            Dim img = frame.Image
            Dim T = frame.Temperature

            Dim h = img.GetLength(0)
            Dim w = img.GetLength(1)

            If Not _init OrElse _base.GetLength(0) <> h OrElse _base.GetLength(1) <> w Then
                _base = CType(img.Clone(), Single(,))
                ReDim _tempCoeff(h - 1, w - 1)
                _count = 0
                _init = True
                Return
            End If

            _count += 1
            Dim alpha As Single = 1.0F / Math.Min(_count, 1000)

            For y = 0 To h - 1
                For x = 0 To w - 1
                    Dim v = img(y, x)

                    _base(y, x) =
                        (1 - alpha) * _base(y, x) + alpha * v

                    _tempCoeff(y, x) +=
                        alpha * (v - _base(y, x)) * T
                Next
            Next
        End Sub

        Public Function Calibrate(frame As Frame) As Single(,)
            Dim img = frame.Image
            Dim T = frame.Temperature

            Dim h = img.GetLength(0)
            Dim w = img.GetLength(1)

            If Not _init OrElse _base.GetLength(0) <> h OrElse _base.GetLength(1) <> w Then
                Update(frame)
                Return CType(img.Clone(), Single(,))
            End If

            Dim output(h - 1, w - 1) As Single

            For y = 0 To h - 1
                For x = 0 To w - 1
                    Dim predicted =
                        _base(y, x) +
                        _tempCoeff(y, x) * T

                    output(y, x) = Math.Max(0, img(y, x) - predicted)
                Next
            Next

            Return output
        End Function
    End Class

    Public Class CalibrationPipeline
        Private _calib As New StatisticalCalibrator()
        Private _quality As New FrameQualityGate()
        Private _night As New NightStateManager()

        Private _learningEnabled As Boolean = False

        Public Sub SaveToFile(filePath As String)
            Using fs As New FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None)
                Using bw As New BinaryWriter(fs)
                    bw.Write("ASTROCAL1")
                    _calib.SaveState(bw)
                End Using
            End Using
        End Sub

        Public Function LoadFromFile(filePath As String) As Boolean
            If Not File.Exists(filePath) Then
                Return False
            End If

            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Using br As New BinaryReader(fs)
                    Dim magic = br.ReadString()
                    If magic <> "ASTROCAL1" Then
                        Return False
                    End If

                    _calib.LoadState(br)
                    Return True
                End Using
            End Using
        End Function

        Private Function HasTransientBrightEvent(img As Single(,)) As Boolean
            Dim h = img.GetLength(0)
            Dim w = img.GetLength(1)
            Dim totalPixels As Integer = h * w

            If totalPixels = 0 Then
                Return False
            End If

            Dim mean = FrameStats.Mean(img)
            Dim std = FrameStats.Std(img, mean)

            Dim threshold As Single = Math.Min(65000.0F, mean + Math.Max(500.0F, 8.0F * std))

            Dim brightCount As Integer = 0
            Dim maxVal As Single = 0.0F

            For y = 0 To h - 1
                For x = 0 To w - 1
                    Dim v = img(y, x)
                    If v > maxVal Then
                        maxVal = v
                    End If

                    If v >= threshold Then
                        brightCount += 1
                    End If
                Next
            Next

            Dim brightFraction As Single = CSng(brightCount) / totalPixels

            Return maxVal >= threshold AndAlso brightFraction > 0.0F AndAlso brightFraction < 0.02F
        End Function

        Public Function Process(frame As Frame) As Single(,)
            Dim img = frame.Image

            Dim nightStable = _night.UpdateAndCheck(img)
            Dim cloudy = _quality.IsCloudy(img)
            Dim transientEvent = HasTransientBrightEvent(img)

            _learningEnabled = nightStable AndAlso Not cloudy AndAlso Not transientEvent

            If _learningEnabled Then
                _calib.Update(frame)
            End If

            Return _calib.Calibrate(frame)
        End Function
    End Class

End Namespace
