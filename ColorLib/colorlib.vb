

Public Class colorlib
    'MS surely fucked up its implementation of color and color transforms!!!!
    'Some methods are not static and need the class to be instantiated. 

    'Enums used as array indices to make code more readable
    Public Enum RGB
        red = 0
        green = 1
        blue = 2
    End Enum

    Public Enum CIEXYZ
        X = 0
        Y = 1
        Z = 2
    End Enum

    Public Enum CIELab
        L = 0
        a = 1
        b = 2
    End Enum

    Public Enum WhitePoint
        CIED65 = 0
        CIED55 = 1
        CIEA = 2
        CIEC = 3
        CIED93 = 4
        CIED50 = 5
        HDTVD65 = 6
    End Enum

    Public Enum ColorSpace
        GenericRGB = 0
        GenericGammaRGB
        sRGB
        sGammaRGB
        CIEXYZ
        CIELab
    End Enum

    Public Enum ColorSpaceTransform
        'They use the number of terms as ID, so we can decude them from the 
        'transformation matrix by counting the rows
        Linear = 3
        NonLinear6Term = 6
        NonLinear8Term = 8
        NonLinear9Term = 9
        NonLinear11Term = 11
        NonLinear14Term = 14
        NonLinear20Term = 20
    End Enum

    'The Gamma LUT, Rec709 transforms and white point matrices
    'Class variables
    Private sWhite(6, 2), sGammaLut(255), sXYZToSRGB(2, 2), sSRGBtoXYZ(2, 2) As Single

    Public Sub New()
        'Object constructor
        MyBase.New()

        'Initalise the LUTs and whitepoint arrays
        Dim iPixel As Integer, sScaledPixel As Single

        'Whitepoints.
        sWhite(WhitePoint.HDTVD65, CIEXYZ.X) = 94.825
        sWhite(WhitePoint.HDTVD65, CIEXYZ.Y) = 100.0# 'Poynton
        sWhite(WhitePoint.HDTVD65, CIEXYZ.Z) = 107.391

        sWhite(WhitePoint.CIED65, CIEXYZ.X) = 95.017
        sWhite(WhitePoint.CIED65, CIEXYZ.Y) = 100.0# 'Wyszecki
        sWhite(WhitePoint.CIED65, CIEXYZ.Z) = 108.813

        sWhite(WhitePoint.CIED55, CIEXYZ.X) = 95.642
        sWhite(WhitePoint.CIED55, CIEXYZ.Y) = 100.0# 'Wyszecki
        sWhite(WhitePoint.CIED55, CIEXYZ.Z) = 92.085

        sWhite(WhitePoint.CIEA, CIEXYZ.X) = 109.828
        sWhite(WhitePoint.CIEA, CIEXYZ.Y) = 100.0# 'Wyszecki
        sWhite(WhitePoint.CIEA, CIEXYZ.Z) = 35.547

        sWhite(WhitePoint.CIEC, CIEXYZ.X) = 98.041
        sWhite(WhitePoint.CIEC, CIEXYZ.Y) = 100.0# 'Wyszecki
        sWhite(WhitePoint.CIEC, CIEXYZ.Z) = 118.103

        sWhite(WhitePoint.CIED50, CIEXYZ.X) = 96.3963
        sWhite(WhitePoint.CIED50, CIEXYZ.Y) = 100.0# 'Wyszecki
        sWhite(WhitePoint.CIED50, CIEXYZ.Z) = 82.4145

        'Transform 3x3 from XYZ to sRGB using Rec 709 primaries
        sXYZToSRGB(0, 0) = 3.240479 / 100.0#
        sXYZToSRGB(1, 0) = -0.969256 / 100.0#
        sXYZToSRGB(2, 0) = 0.055648 / 100.0#
        sXYZToSRGB(0, 1) = -1.53715 / 100.0#
        sXYZToSRGB(1, 1) = 1.875992 / 100.0#
        sXYZToSRGB(2, 1) = -0.204043 / 100.0#
        sXYZToSRGB(0, 2) = -0.498535 / 100.0#
        sXYZToSRGB(1, 2) = 0.041556 / 100.0#
        sXYZToSRGB(2, 2) = 1.057311 / 100.0#

        'Transform 3x3 from sRGB to XYZ using Rec 709 primaries
        sSRGBtoXYZ(0, 0) = 0.412453 * 100.0#
        sSRGBtoXYZ(1, 0) = 0.212671 * 100.0#
        sSRGBtoXYZ(2, 0) = 0.019334 * 100.0#
        sSRGBtoXYZ(0, 1) = 0.35758 * 100.0#
        sSRGBtoXYZ(1, 1) = 0.71516 * 100.0#
        sSRGBtoXYZ(2, 1) = 0.119193 * 100.0#
        sSRGBtoXYZ(0, 2) = 0.180423 * 100.0#
        sSRGBtoXYZ(1, 2) = 0.072169 * 100.0#
        sSRGBtoXYZ(2, 2) = 0.950227 * 100.0#

        'Rec709 Gamma function LUT
        'from non-linear GammaSRGB [0 255] to linear SRGB [0.0 1.0]
        For iPixel = 0 To 255
            sScaledPixel = iPixel / 255.0#
            If sScaledPixel <= 0.081 Then
                sGammaLut(iPixel) = sScaledPixel / 4.5
            Else
                sGammaLut(iPixel) = ((sScaledPixel + 0.099) / 1.099) ^ (1 / 0.45)
            End If
        Next iPixel
    End Sub

    Public Shared Function RGBName(ByVal i As Integer)
        'Return the name associated with a color component
        Select Case i
            Case RGB.blue
                Return "Blue"
            Case RGB.green
                Return "Green"
            Case RGB.red
                Return "Red"
        End Select
    End Function

    Public Shared Function CIEXYZName(ByVal i As Integer)
        'Return the name associated with a color  component 
        Select Case i
            Case CIEXYZ.X
                Return "X"
            Case CIEXYZ.Y
                Return "Y"
            Case CIEXYZ.Z
                Return "Z"
        End Select
    End Function

    Public Overloads Shared Sub RGBTosRGB(ByVal sRGB() As Single,
                                          ByVal sSRGB() As Single,
                                          ByVal sTransform(,) As Single)
        'Transform linear RGB using a polynomial mapping technique.
        'RGB both in [0.0 1.0] range. Convenience function, it just makes code more readable.
        TransformColor(sRGB, sSRGB, sTransform)
    End Sub

    Public Overloads Shared Sub RGBTosRGB(ByVal sRGB(,) As Single,
                                          ByVal sSRGB(,) As Single,
                                          ByVal sTransform(,) As Single)
        'Transform linear RGB using a polynomial mapping technique.
        'RGB both in [0.0 1.0] range. Convenience function, it just makes code more readable.
        TransformColor(sRGB, sSRGB, sTransform)
    End Sub

    Public Overloads Shared Sub TransformColor(ByVal sC1(,) As Single,
                            ByVal sC2(,) As Single,
                            ByVal sTransform(,) As Single)
        Dim sC1Row(2), sC2Row(2) As Single
        Dim iHiRow, i As Integer
        iHiRow = sC1.GetUpperBound(0)
        For i = 0 To iHiRow
            Algebra.GetMatrixRow(sC1, sC1Row, i)
            TransformColor(sC1Row, sC2Row, sTransform)
            Algebra.SetMatrixRow(sC2, sC2Row, i)
        Next
    End Sub

    Public Overloads Shared Sub TransformColor(ByVal sC1() As Single,
                                               ByVal sC2() As Single,
                                               ByVal sTransform(,) As Single)
        'Transform linear tristimulus sC1 to sC2 using a polynomial mapping technique.
        'Both tristimuli in [0.0 1.0] range
        Dim sCNL(sTransform.GetUpperBound(1)) As Single

        'Compute color N-tuple 
        ColorTripletToNTuple(sC1, sCNL)

        'Left multiply color N-tuple with transform
        Algebra.Product(sTransform, sCNL, sC2)
    End Sub

    Public Overloads Sub XYZToCIELab(ByVal sXYZ() As Single,
                         ByVal sLab() As Single)
        XYZToCIELab(sXYZ, sLab, WhitePoint.CIED65)
    End Sub

    Public Overloads Sub XYZToCIELab(ByVal sXYZ() As Single,
                           ByVal sLab() As Single,
                           ByVal iWhite As Integer)
        'XYZ to CIELab transform.
        Dim sY As Single

        sY = sXYZ(CIEXYZ.Y) / sWhite(iWhite, CIEXYZ.Y)
        sLab(CIELab.L) = 116 * CIEf(sY) - 16.0#
        sLab(CIELab.a) = 500 * (CIEf(sXYZ(CIEXYZ.X) / sWhite(iWhite, CIEXYZ.X)) - CIEf(sY))
        sLab(CIELab.b) = 200 * (CIEf(sY) - CIEf(sXYZ(CIEXYZ.Z) / sWhite(iWhite, CIEXYZ.Z)))
    End Sub

    Public Overloads Sub CIELabToXYZ(ByVal sLab() As Single,
                                      ByVal sXYZ() As Single)
        CIELabToXYZ(sLab, sXYZ, WhitePoint.CIED65)
    End Sub

    Public Overloads Sub CIELabToXYZ(ByVal sLab() As Single,
                           ByVal sXYZ() As Single,
                           ByVal iWhite As Integer)
        'CIELab to XYZ transform.
        Dim sTemp As Single, i As Integer

        sTemp = (sLab(0) + 16.0#) / 116.0#
        sXYZ(CIEXYZ.Y) = CIEfInverse(sTemp) * sWhite(iWhite, CIEXYZ.Y)
        sXYZ(CIEXYZ.X) = CIEfInverse(sLab(1) / 500.0# + sTemp) * sWhite(iWhite, CIEXYZ.X)
        sXYZ(CIEXYZ.Z) = CIEfInverse(-sLab(2) / 200.0# + sTemp) * sWhite(iWhite, CIEXYZ.Z)
        For i = 0 To 2
            If sXYZ(i) < 0 Then sXYZ(i) = 0
        Next i
    End Sub

    Public Shared Function CIELabTodE(ByVal sLab1() As Single,
                                      ByVal sLab2() As Single)
        Dim sDeltaLab(2) As Single
        Algebra.Subtract(sLab1, sLab2, sDeltaLab)
        Return Algebra.Norm(sDeltaLab)
    End Function

    Public Shared Function CIELabTodC(ByVal sLab1() As Single,
                               ByVal sLab2() As Single)
        'Use only chromatic info, not luminance
        CIELabTodC = ((sLab1(CIELab.a) - sLab2(CIELab.a)) ^ 2 + (sLab1(CIELab.b) - sLab2(CIELab.b)) ^ 2) ^ 0.5
    End Function

    Public Sub CIELabTosRGB(ByVal sLab() As Single,
                            ByVal sRGB() As Single)
        'Input must have D65 whitepoint!
        Dim sXYZ(2) As Single
        CIELabToXYZ(sLab, sXYZ, WhitePoint.CIED65)
        XYZToSRGB(sXYZ, sRGB)
    End Sub

    Public Sub sRGBToCIELab(ByVal sRGB() As Single,
                            ByVal sLab() As Single)
        'Output has D65 whitepoint!
        Dim sXYZ(2) As Single
        sRGBtoXYZ(sRGB, sXYZ)
        XYZToCIELab(sXYZ, sLab, WhitePoint.CIED65)
    End Sub

    Public Overloads Function sRGBTodE(ByVal sSRGB1() As Single, ByVal sSRGB2() As Single) As Single
        'Output has D65 whitepoint!
        Dim sLab1(2), sLab2(2) As Single
        sRGBToCIELab(sSRGB1, sLab1)
        sRGBToCIELab(sSRGB2, sLab2)
        Return CIELabTodE(sLab1, sLab2)
    End Function

    Public Overloads Sub sRGBTodE(ByVal sSRGB1(,) As Single, ByVal sSRGB2(,) As Single, ByVal sdE() As Single)
        'Output has D65 whitepoint!
        Dim i As Integer, iMax As Integer = sSRGB1.GetUpperBound(0)

        For i = 0 To iMax
            Dim sRGB1(2), sRGB2(2) As Single

            Algebra.GetMatrixRow(sSRGB1, sRGB1, i)
            Algebra.GetMatrixRow(sSRGB2, sRGB2, i)
            sdE(i) = sRGBTodE(sRGB1, sRGB2)
        Next
    End Sub

    Public Overloads Function XYZTodE(ByVal sXYZ1() As Single,
                          ByVal sXYZ2() As Single) As Single
        Return XYZTodE(sXYZ1, sXYZ2, WhitePoint.CIED65)
    End Function

    Public Overloads Function XYZTodE(ByVal sXYZ1() As Single,
                            ByVal sXYZ2() As Single,
                            ByVal iWhite As Integer) As Single
        'Compute dE from two XYZ tristimulus values. We take some shortcuts here for speed
        Dim sDLab(2) As Single

        sDLab(CIELab.L) = 116 * (CIEf(sXYZ1(CIEXYZ.Y) / sWhite(iWhite, CIEXYZ.Y)) -
          CIEf(sXYZ2(CIEXYZ.Y) / sWhite(iWhite, CIEXYZ.Y)))
        sDLab(CIELab.a) = 500 * (CIEf(sXYZ1(CIEXYZ.X) / sWhite(iWhite, CIEXYZ.X)) -
          CIEf(sXYZ1(CIEXYZ.Y) / sWhite(iWhite, CIEXYZ.Y)) -
          CIEf(sXYZ2(CIEXYZ.X) / sWhite(iWhite, CIEXYZ.X)) +
          CIEf(sXYZ2(CIEXYZ.Y) / sWhite(iWhite, CIEXYZ.Y)))
        sDLab(CIELab.b) = 200 * (CIEf(sXYZ1(CIEXYZ.Y) / sWhite(iWhite, CIEXYZ.Y)) -
          CIEf(sXYZ1(CIEXYZ.Z) / sWhite(iWhite, CIEXYZ.Z)) -
          CIEf(sXYZ2(CIEXYZ.Y) / sWhite(iWhite, CIEXYZ.Y)) +
          CIEf(sXYZ2(CIEXYZ.Z) / sWhite(iWhite, CIEXYZ.Z)))
        Return Algebra.Norm(sDLab)
    End Function

    Private Function CIEf(ByVal sValue As Single) As Single
        'CIE f function for the computation of CIE Lab values

        If sValue >= 0.008856 Then
            CIEf = sValue ^ 0.3333333333
        Else
            CIEf = 7.787 * sValue + 0.1379310344828
        End If
    End Function

    Private Function CIEfInverse(ByVal sValue As Single) As Single
        'CIE f function for the computation of CIE Lab values

        If sValue >= 0.206892706482 Then
            CIEfInverse = sValue ^ 3
            'Debug.Print "CIE f inverse of " & sValue & ": " & CIEfInverse
        Else
            CIEfInverse = (sValue - 0.1379310344828) / 7.787
            'Debug.Print "CIE f inverse (linear part) of " & sValue & ": " & CIEfInverse
        End If
    End Function

    Public Sub XYZToSRGB(ByVal sXYZ() As Single, ByVal sRGB() As Single)
        'Transform CIE XYZ to RGB using the Rec 709 primaries (sRGB space)
        'RGB in [0.0 1.0] range, XYZ in [0.0 1xx.x] range
        Algebra.Product(sXYZToSRGB, sXYZ, sRGB)
    End Sub

    Public Sub sRGBtoXYZ(ByVal sRGB() As Single, ByVal sXYZ() As Single)
        'Transform RGB to XYZ using the Rec 709 primaries (sRGB space)
        'RGB in [0.0 1.0] range, XYZ in [0.0 1xx.X] range
        Algebra.Product(sSRGBtoXYZ, sRGB, sXYZ)
    End Sub

    Public Sub XYZToxyY(ByVal sXYZ() As Single, ByVal sXYY() As Single)
        Dim sSum As Single

        sSum = sXYZ(CIEXYZ.X) + sXYZ(CIEXYZ.Y) + sXYZ(CIEXYZ.Z)
        sXYY(0) = sXYZ(CIEXYZ.X) / sSum
        sXYY(1) = sXYZ(CIEXYZ.Y) / sSum
        sXYY(2) = sXYZ(CIEXYZ.Y)
    End Sub

    Public Shared Function GammaCorrection(ByVal sIn As Single) As Single
        'Apply gamma correction from input range [0.0 1.0] to [0.0 1.0]
        'Do clipping
        If sIn >= 1.0# Then
            GammaCorrection = 1.0#
        ElseIf sIn < 0.0# Then
            GammaCorrection = 0.0#
        ElseIf sIn <= 0.018 Then
            GammaCorrection = 4.5 * sIn
        Else
            GammaCorrection = 1.099 * sIn ^ 0.45 - 0.099
        End If
    End Function

    Public Shared Function InverseGammaCorrection(ByVal sIn As Single) As Single
        'Apply gamma correction from input range [0.0 1.0] to [0.0 1.0]
        If sIn <= 0.081 Then
            InverseGammaCorrection = sIn / 4.5
        Else
            InverseGammaCorrection = ((sIn + 0.099) / 1.099) ^ 2.22222222
        End If
    End Function

    Public Shared Sub Windowing(ByVal sRGB() As Single,
                                ByVal sOffsetRGB() As Single,
                                ByVal sScaleRGB() As Single,
                                ByVal sWindowedRGB() As Single)
        'Apply subwindow selection to a RGB value: sWRGB = (sRGB - sOffsetRGB) .* sScaleRGB
        sWindowedRGB(RGB.red) = Algebra.Scale(sRGB(RGB.red), sOffsetRGB(RGB.red), sScaleRGB(RGB.red))
        sWindowedRGB(RGB.green) = Algebra.Scale(sRGB(RGB.green), sOffsetRGB(RGB.green), sScaleRGB(RGB.green))
        sWindowedRGB(RGB.blue) = Algebra.Scale(sRGB(RGB.blue), sOffsetRGB(RGB.blue), sScaleRGB(RGB.blue))
    End Sub

    Public Shared Sub Windowing2(ByVal sRGB() As Single,
                              ByVal sLowRGB() As Single,
                              ByVal sHighRGB() As Single,
                              ByVal sWindowedRGB() As Single)
        'Apply subwindow selection to a RGB value: sWRGB = (sRGB - sLowRGB) .* (1 / (sHigh - sLowRGB))
        'Use only for small groups of values, otherwise precompute the scale and use Windowing
        sWindowedRGB(RGB.red) = Algebra.Scale(sRGB(RGB.red), sLowRGB(RGB.red), 1 / (sHighRGB(RGB.red) - sLowRGB(RGB.red)))
        sWindowedRGB(RGB.green) = Algebra.Scale(sRGB(RGB.green), sLowRGB(RGB.green), 1 / (sHighRGB(RGB.green) - sLowRGB(RGB.green)))
        sWindowedRGB(RGB.blue) = Algebra.Scale(sRGB(RGB.blue), sLowRGB(RGB.blue), 1 / (sHighRGB(RGB.blue) - sLowRGB(RGB.blue)))
    End Sub


    Public Shared Sub RGBToGammaRGB(ByVal sRGB() As Single, ByVal byGammaRGB() As Byte)
        'Gamma correction. Input RGB in [0.0 1.0], output RGB [0 255].
        byGammaRGB(RGB.red) = CByte(GammaCorrection(sRGB(RGB.red)) * 255)
        byGammaRGB(RGB.green) = CByte(GammaCorrection(sRGB(RGB.green)) * 255)
        byGammaRGB(RGB.blue) = CByte(GammaCorrection(sRGB(RGB.blue)) * 255)
    End Sub

    Public Shared Sub RGBToGammaRGB(ByVal sRGB() As Single, ByVal sGammaRGB() As Single)
        'Gamma correction. Input RGB in [0.0 1.0], output RGB [0 255].
        sGammaRGB(RGB.red) = GammaCorrection(sRGB(RGB.red)) * 255.0
        sGammaRGB(RGB.green) = GammaCorrection(sRGB(RGB.green)) * 255.0
        sGammaRGB(RGB.blue) = GammaCorrection(sRGB(RGB.blue)) * 255.0
    End Sub

    Public Overloads Shared Sub GammaRGBToRGB(ByVal sGammaRGB() As Single, ByVal sRGB() As Single)
        'Inverse gamma correction, single precision input.
        'Output RGB in [0.0 1.0], input RGB [0.0 255.0].
        'Input maybe same array as output.
        sRGB(RGB.red) = InverseGammaCorrection(sGammaRGB(RGB.red) / 255.0#)
        sRGB(RGB.green) = InverseGammaCorrection(sGammaRGB(RGB.green) / 255.0#)
        sRGB(RGB.blue) = InverseGammaCorrection(sGammaRGB(RGB.blue) / 255)
    End Sub

    Public Overloads Shared Sub GammaRGBToRGB(ByVal sGammaRGB() As Single, ByVal sRGB() As Single, ByVal sLUT(,) As Single)
        'Inverse gamma correction, single precision input, but with a LUT BEFORE the gamma-correction
        'Output RGB in [0.0 1.0], input RGB [0.0 255.0].
        'Input maybe same array as output.
        sRGB(RGB.red) = InverseGammaCorrection(sLUT(CInt(sGammaRGB(RGB.red)), RGB.red) / 255.0#)
        sRGB(RGB.green) = InverseGammaCorrection(sLUT(CInt(sGammaRGB(RGB.green)), RGB.green) / 255.0#)
        sRGB(RGB.blue) = InverseGammaCorrection(sLUT(CInt(sGammaRGB(RGB.blue)), RGB.blue) / 255)
    End Sub

    Public Overloads Shared Sub GammaRGBToRGB(ByVal byGammaRGB() As Byte, ByVal sRGB() As Single, ByVal sLUT(,) As Single)
        'Inverse gamma correction, single precision input, but with a LUT BEFORE the gamma-correction
        'Output RGB in [0.0 1.0], input RGB [0.0 255.0].
        'Input maybe same array as output.
        sRGB(RGB.red) = InverseGammaCorrection(sLUT(byGammaRGB(RGB.red), RGB.red) / 255.0#)
        sRGB(RGB.green) = InverseGammaCorrection(sLUT(byGammaRGB(RGB.green), RGB.green) / 255.0#)
        sRGB(RGB.blue) = InverseGammaCorrection(sLUT(byGammaRGB(RGB.blue), RGB.blue) / 255.0#)
    End Sub

    Public Overloads Sub GammaRGBToRGBbyLUT(ByVal byGammaRGB() As Byte, ByVal sRGB() As Single)
        'Inverse gamma correction, byte input. Faster with LUT, but not shared!
        'Output RGB in [0.0 1.0], input RGB [0 255].
        sRGB(RGB.red) = sGammaLut(byGammaRGB(RGB.red))
        sRGB(RGB.green) = sGammaLut(byGammaRGB(RGB.green))
        sRGB(RGB.blue) = sGammaLut(byGammaRGB(RGB.blue))
    End Sub

    Public Overloads Sub GammaRGBToRGBbyLUT(ByVal byGammaRGB() As Byte, ByVal sRGB() As Single, ByVal sLUT(,) As Single)
        'Inverse gamma correction, byte input. Faster with LUT, but not shared!
        'Output RGB in [0.0 1.0], input RGB [0 255].
        sRGB(RGB.red) = sGammaLut(sLUT(byGammaRGB(RGB.red), RGB.red))
        sRGB(RGB.green) = sGammaLut(sLUT(byGammaRGB(RGB.green), RGB.green))
        sRGB(RGB.blue) = sGammaLut(sLUT(byGammaRGB(RGB.blue), RGB.blue))
    End Sub

    Public Overloads Shared Function ToString(ByVal sColor() As Single) As String
        Return ToString(sColor, 2)
    End Function

    Public Overloads Shared Function ToString(ByVal sColor() As Single,
                                       ByVal iPrecision As Integer) As String
        'Transform a 3 element color to a string for screen output
        Dim strFormat As String, i As Integer

        If iPrecision > 0 Then
            strFormat = "##0."
            For i = 1 To iPrecision
                strFormat = strFormat & "#"
            Next i
        Else
            strFormat = "#"
        End If
        Return sColor(0).ToString(strFormat) & " " & sColor(1).ToString(strFormat) & " " & sColor(2).ToString(strFormat)
    End Function

    Public Overloads Shared Function ToString(ByVal byColor() As Byte) As String
        'Transform a 3 element color to a string for screen output
        Return byColor(0) & " " & byColor(1) & " " & byColor(2)
    End Function

    Public Shared Sub ComputeColorSpaceTransform(ByVal sC1(,) As Single,
                                          ByVal sC2(,) As Single,
                                          ByRef sTransform(,) As Single,
                                          ByVal iTransformType As ColorSpaceTransform)
        'Compute the polynomial transforms from R3 to R3, both representing LINEAR color spaces.
        'This is based on a set of N color triplets in sC1, which have to be mapped to N color triplets in sC2.
        'This means both sC1 and sC2 are N x 3 matrices. 
        'This also means there are actually 3 transforms to be computed, i.e. one for every color coordinate.
        'The passed on iTransformType is equal to the number of terms in the transform (3 for linear,etc..), 
        'and can be termed the order of the transform.
        'The resulting polynomial transform  is stored in a 3 x M matrix, which can be right multiplied with
        'a color N-tuple to obtain the desired color tristimulus value.
        'NOTE: Usually the color transform is used to transform from RGB to sRGB or XYZ
        Dim iNrColors, i As Integer

        iNrColors = sC1.GetUpperBound(0) + 1
        Console.WriteLine("Computing " & iTransformType &
          "-term transform using " & iNrColors & " tristimulus values")

        'Compute sA using the input color triplets
        Dim sCNL(iTransformType - 1), sA(iNrColors - 1, iTransformType - 1), sC(2) As Single

        For i = 0 To iNrColors - 1
            Algebra.GetMatrixRow(sC1, sC, i)
            ColorTripletToNTuple(sC, sCNL)

            'Copy color N-tuple to sA
            Algebra.SetMatrixRow(sA, sCNL, i)
        Next i
        Console.WriteLine("Design matrix is " & vbNewLine & Algebra.ToString(sA))

        'Solve the 3 sets of linear equations (they are linear in their unknowns, even if they use
        'coefficient which are non-linear functions of the input color triplets!)
        'The resulting matrix needs to be transposed to be consistent with the fixed linear transform
        'matrices already used in the color class (they all use Tf.X = Y, NOT X.Tf = Y)
        Dim sX(iTransformType - 1, 2) As Single
        Algebra.Solve(sA, sX, sC2)

        ReDim sTransform(2, iTransformType - 1)
        Algebra.Transpose(sX, sTransform)
    End Sub

    Private Shared Sub ColorTripletToNTuple(ByVal sC() As Single,
                                            ByVal sCNL() As Single)
        'Compute a color N-tuple using higher order and cross terms of the input color triplet.
        'N depends on the number of elements in sCNL.
        'This N-tuple can then be multiplied by a transform matrix representing the corresponding 
        'polynomial transform to obtain a new color triplet.
        Dim i, iNrTerms As Integer

        'The number of terms or base functions depends on the size of sCNL
        iNrTerms = sCNL.Length

        'All base function sets are at least linear in RGB
        For i = RGB.red To RGB.blue
            sCNL(i) = sC(i)
        Next i

        Select Case iNrTerms
            Case ColorSpaceTransform.Linear
        'Linear transform
            Case ColorSpaceTransform.NonLinear20Term
                ' 20 terms:
                ' tf = a0 + a1*X + a2*y + a3*z + a4*X*y + a5*y*z + a6*z*X + a7*X^2 + a8*y^2 + a9*z^2 + a10 * X*y*z
                '      + a11*X^3 + a12*y^3 + a13*z^3 + a14*X*y^2 + a15*X^2*y + a16*y*z^2 + a17*y^2*z + a18*z*X^2
                '      + a19*z^2*X
                sCNL(3) = sCNL(0) * sCNL(1)
                sCNL(4) = sCNL(2) * sCNL(1)
                sCNL(5) = sCNL(0) * sCNL(2)

                sCNL(6) = sCNL(0) * sCNL(0)
                sCNL(7) = sCNL(1) * sCNL(1)
                sCNL(8) = sCNL(2) * sCNL(2)

                sCNL(9) = sCNL(0) * sCNL(1) * sCNL(2)
                sCNL(10) = sCNL(6) * sCNL(0)
                sCNL(11) = sCNL(7) * sCNL(1)
                sCNL(12) = sCNL(8) * sCNL(2)

                sCNL(13) = sCNL(3) * sCNL(1)
                sCNL(14) = sCNL(3) * sCNL(0)
                sCNL(15) = sCNL(4) * sCNL(2)
                sCNL(16) = sCNL(4) * sCNL(1)
                sCNL(17) = sCNL(5) * sCNL(0)
                sCNL(18) = sCNL(5) * sCNL(2)
                sCNL(19) = 1.0#
            Case ColorSpaceTransform.NonLinear14Term
                ' 14 terms:
                ' tf = a13 + a0*X + a1*y + a2*z + a3*X*y + a4*y*z + a5*z*X +
                '      a6*X^2 + a7*y^2 + a8*z^2 + a9 * X*y*z
                '      + a10*X^3 + a11*y^3 + a12*z^3
                sCNL(3) = sCNL(0) * sCNL(1)
                sCNL(4) = sCNL(2) * sCNL(1)
                sCNL(5) = sCNL(0) * sCNL(2)

                sCNL(6) = sCNL(0) * sCNL(0)
                sCNL(7) = sCNL(1) * sCNL(1)
                sCNL(8) = sCNL(2) * sCNL(2)

                sCNL(9) = sCNL(0) * sCNL(1) * sCNL(2)
                sCNL(10) = sCNL(6) * sCNL(0)
                sCNL(11) = sCNL(7) * sCNL(1)
                sCNL(12) = sCNL(8) * sCNL(2)

                sCNL(13) = 1.0
            Case ColorSpaceTransform.NonLinear11Term
                ' 11 terms:
                ' tf = a10 + a0*X + a1*y + a2*z + a3*X*y + a4*y*z + a5*z*X +
                '      a6*X^2 + a7*y^2 + a8*z^2 + a9*X*y*z
                sCNL(3) = sCNL(0) * sCNL(1)
                sCNL(4) = sCNL(2) * sCNL(1)
                sCNL(5) = sCNL(0) * sCNL(2)

                sCNL(6) = sCNL(0) * sCNL(0)
                sCNL(7) = sCNL(1) * sCNL(1)
                sCNL(8) = sCNL(2) * sCNL(2)

                sCNL(9) = sCNL(0) * sCNL(1) * sCNL(2)
                sCNL(10) = 1.0#
            Case ColorSpaceTransform.NonLinear6Term
                ' 6 terms:
                ' tf = a0*X + a1*y + a2*z + a3*X*y + a4*y*z + a5*z*X
                sCNL(3) = sCNL(0) * sCNL(1)
                sCNL(4) = sCNL(2) * sCNL(1)
                sCNL(5) = sCNL(0) * sCNL(2)
            Case ColorSpaceTransform.NonLinear8Term
                ' 8 terms:
                ' tf = a7 + a0*X + a1*y + a2*z + a3*X*y +
                '      a4*y*z + a5*z*X + a6*X*y*z
                sCNL(3) = sCNL(0) * sCNL(1)
                sCNL(4) = sCNL(2) * sCNL(1)
                sCNL(5) = sCNL(0) * sCNL(2)

                sCNL(6) = sCNL(0) * sCNL(1) * sCNL(2)
                sCNL(7) = 1.0
            Case ColorSpaceTransform.NonLinear9Term
                ' 9 terms:
                ' tf = a0*X + a1*y + a2*z + a3*X*y + a4*y*z + a5*z*X +
                '      a6*X^2 + a7*y^2 + a8*z^2
                sCNL(3) = sCNL(0) * sCNL(1)
                sCNL(4) = sCNL(2) * sCNL(1)
                sCNL(5) = sCNL(0) * sCNL(2)

                sCNL(6) = sCNL(0) * sCNL(0)
                sCNL(7) = sCNL(1) * sCNL(1)
                sCNL(8) = sCNL(2) * sCNL(2)
            Case Else
        End Select

        'Console.WriteLine("Converted color triplet:" & _
        '  vbNewLine & Algebra.ToString(sC) & _
        '  "to " & _
        '  vbNewLine & Algebra.ToString(sCNL))
    End Sub

    '------------------- Some functions for use with Windows GDI+ Get and SetPixel ----------------------

    Public Shared Sub GammaRGBToLUTGamma(ByVal sGammaRGB() As Single,
                                              ByVal sGammaLUTRGB() As Single,
                                              ByVal sLUT(,) As Single)
        sGammaLUTRGB(RGB.red) = sLUT(CInt(sGammaRGB(0)), RGB.red)
        sGammaLUTRGB(RGB.green) = sLUT(CInt(sGammaRGB(0)), RGB.green)
        sGammaLUTRGB(RGB.blue) = sLUT(CInt(sGammaRGB(0)), RGB.blue)
    End Sub

    Public Overloads Shared Function GammaRGBToColor(ByVal sGammaRGB() As Single) As System.Drawing.Color
        'Return a color structure from gamma-corrected RGB
        Dim objColor As System.Drawing.Color
        objColor = System.Drawing.Color.FromArgb(CInt(sGammaRGB(RGB.red)), CInt(sGammaRGB(RGB.green)), CInt(sGammaRGB(RGB.blue)))
    End Function

    Public Overloads Shared Function GammaRGBToColor(ByVal byGammaRGB() As Byte) As System.Drawing.Color
        'Return a color structure from gamma-corrected RGB
        Return System.Drawing.Color.FromArgb(byGammaRGB(RGB.red), byGammaRGB(RGB.green), byGammaRGB(RGB.blue))
    End Function

    Public Shared Function RGBToColor(ByVal sRGB() As Single) As System.Drawing.Color
        'Return a color structure from LINEAR RGB
        Dim iRGB(2) As Byte
        RGBToGammaRGB(sRGB, iRGB)
        Return System.Drawing.Color.FromArgb(iRGB(RGB.red), iRGB(RGB.green), iRGB(RGB.blue))
    End Function

    Public Overloads Shared Sub ColorToRGB(ByVal objColor As System.Drawing.Color, ByVal sRGB() As Single)
        sRGB(RGB.red) = objColor.R
        sRGB(RGB.green) = objColor.G
        sRGB(RGB.blue) = objColor.B
        GammaRGBToRGB(sRGB, sRGB)
    End Sub

    Public Overloads Shared Sub ColorToRGB(ByVal objColor As System.Drawing.Color, ByVal sLUT(,) As Single, ByVal sRGB() As Single)
        sRGB(RGB.red) = sLUT(objColor.R, RGB.red)
        sRGB(RGB.green) = sLUT(objColor.G, RGB.green)
        sRGB(RGB.blue) = sLUT(objColor.B, RGB.blue)
        GammaRGBToRGB(sRGB, sRGB)
    End Sub

    Public Overloads Shared Function RGBColorTosRGBColor(ByVal objColor As System.Drawing.Color, ByVal sTransform(,) As Single) As System.Drawing.Color
        'Conveniece function that tranforms an color struct containing gamma-corrected
        'RGB values to sRGB values
        Dim sRGB(2), sSRGB(2) As Single

        ColorToRGB(objColor, sRGB)
        TransformColor(sRGB, sSRGB, sTransform)
        Return RGBToColor(sSRGB)
    End Function

    Public Overloads Shared Function RGBColorTosRGBColor(ByVal objColor As System.Drawing.Color, ByVal sTransform(,) As Single, ByVal sLUT(,) As Single) As System.Drawing.Color
        'Conveniece function that tranforms an color struct containing gamma-corrected
        'RGB values to sRGB values
        Dim sRGB(2), sSRGB(2) As Single

        ColorToRGB(objColor, sLUT, sRGB)
        TransformColor(sRGB, sSRGB, sTransform)
        Return RGBToColor(sSRGB)
    End Function

    '------------------- Some functions for use with unmanaged code in C# ----------------------
    '                    These bypass the color structure and workt with bytes

    Public Overloads Shared Sub GammaRGBToGammasRGB(ByVal byGammaRGB() As Byte, ByVal sTransform(,) As Single, ByVal sLUT(,) As Single)
        'Conveniece function that tranforms 3 bytes containing gamma-corrected 
        'RGB values to sRGB values
        Dim sRGB(2), sSRGB(2) As Single

        GammaRGBToRGB(byGammaRGB, sRGB, sLUT)
        ' TransformColor(sRGB, sSRGB, sTransform)
        ' RGBToGammaRGB(sSRGB, byGammaRGB)
    End Sub

End Class
