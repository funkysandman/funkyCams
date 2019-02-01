Public Class Algebra
    'Author: Yves Vander Haeghen (Yves.VanderHaeghen@UGent.be)
    'Version: 1.0
    'VersionDate": 13 june 2003


    'Class of helper functions for simple algebra operations on 1 and 2 dimensional single arrays
    'Although speed is not essential, we try to avoid recreating and reallocating output arrays 
    'on every call as this could slow things down a lot. This means that usually the output arrays MUST
    'be allocated and passed to the functions, except when they are passed on by reference.
    'All matrices are supposedly ordered ROW x COLUMN

    Enum NormOrder As Integer
        AbsoluteValue = 1
        Euclidean = 2
        Max = 16
    End Enum

    Public Overloads Shared Function Solve(ByVal sA(,) As Single, ByVal sX(,) As Single, ByVal sY(,) As Single) As Boolean
        'Solve A.X = Y, FOR every column of Y!!!
        'This is useful because we only have to decompose A once, 
        'and then use this decomposition to compute X = inv(A).Y for every column of Y
        'The results are stored in the corresponding columns of X
        'See overloaded Solve for general explanation about the solver.
        Dim sU(,) As Single, sW() As Single, sV(,) As Single, i As Integer
        Dim strError As String

        If SVDDecomposition(sA, sU, sW, sV, strError) = False Then
            MsgBox("Algebra.Solve: SVD gives error '" & strError & "'", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Return False
        End If

        SVDRemoveSingularValues(sW, 0.0001)

        'Run though every column of sY, compute the result, and store it in the corresponding column of sX.
        Dim iNrEquationSets As Integer = sY.GetUpperBound(1) + 1
        Dim iNrVariables As Integer = sA.GetUpperBound(1) + 1
        Dim iNrEquationsPerSet As Integer = sA.GetUpperBound(0) + 1
        Dim sXCol(iNrVariables - 1), sYCol(iNrEquationsPerSet - 1) As Single
        For i = 0 To iNrEquationSets - 1
            GetMatrixColumn(sY, sYCol, i)
            Solve(sA, sXCol, sYCol)
            SetMatrixColumn(sX, sXCol, i)
        Next
    End Function

    Public Overloads Shared Function Solve(ByVal sA(,) As Single, ByVal sX() As Single, ByVal sY() As Single) As Boolean
        'Solve the set of linear equations represented by A.x = y.
        'The number of equations can be larger than the number of variables (overdetermined):
        'i.e. the number of rows in A > number of cols in A. In that case the solution is 
        'a solution in the least-squares sense.
        'This routine uses singular value decomposition, translated from "Numerical recipes in C"
        Dim sU(,) As Single, sW() As Single, sV(,) As Single
        Dim strError As String

        Console.WriteLine("Solving linear set of equations A.x = y with A" &
          vbNewLine & Algebra.ToString(sA) &
          vbNewLine & "y" &
          vbNewLine & Algebra.ToString(sY))

        If SVDDecomposition(sA, sU, sW, sV, strError) = False Then
            MsgBox("Algebra.Solve: SVD gives error '" & strError & "'", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            Return False
        End If

        SVDRemoveSingularValues(sW, 0.0001)

        'Compute pseudo-inverse multiplied with sY
        SVDInvert(sU, sW, sV, sY, sX)
        Return True
    End Function

    Private Shared Sub SVDRemoveSingularValues(ByVal sW() As Single, ByVal sThresholdFactor As Single)
        'Set singular values to zero by compairing them to
        'the highest value in w. 
        Dim iNrVariables As Integer = sW.GetUpperBound(0) + 1
        Dim i As Integer, sWMax As Single = 0.0

        For i = 0 To iNrVariables - 1
            If sW(i) > sWMax Then sWMax = sW(i)
        Next i
        Dim sThreshold As Single = sThresholdFactor * sWMax
        For i = 0 To iNrVariables - 1
            If sW(i) < sThreshold Then sW(i) = 0.0
        Next i
    End Sub

    Private Shared Sub SVDInvert(ByVal sU(,) As Single,
                                 ByVal sW() As Single,
                                 ByVal sV(,) As Single,
                                 ByVal sY() As Single,
                                 ByVal sX() As Single)
        'Computes Y = inv(A).Y using the SVD decomposition of A = U.W.Vt
        Dim jj, j, i, m, n As Integer
        Dim s As Single

        m = sU.GetUpperBound(0) + 1
        n = sU.GetUpperBound(1) + 1

        Dim tmp(n - 1) As Single
        For j = 1 To n
            s = 0.0
            If sW(j - 1) <> 0.0 Then
                For i = 1 To m
                    s = s + sU(i - 1, j - 1) * sY(i - 1)
                Next i
                s = s / sW(j - 1)
            End If

            tmp(j - 1) = s
        Next j

        For j = 1 To n
            s = 0.0
            For jj = 1 To n
                s = s + sV(j - 1, jj - 1) * tmp(jj - 1)
            Next jj
            sX(j - 1) = s
        Next j
    End Sub

    Private Shared Function SVDDecomposition(ByVal sA(,) As Single,
                                             ByRef sU(,) As Single,
                                             ByRef sW() As Single,
                                             ByRef sV(,) As Single,
                                             ByVal strError As String) As Boolean

        'Compute the singular value decomposition of
        'an m sx n matrix A: A = U.W.Vt
        'None of the byref matrices must be allocated here.
        'If something goes wrong it returns false with a message in strError
        Dim Flag As Boolean, i As Integer, its As Integer
        Dim j As Integer, jj As Integer, k As Integer
        Dim l As Integer, nm As Integer
        Dim c As Single, f As Single, h As Single, s As Single
        Dim sX As Single, sY As Single, sz As Single, rv1() As Single
        Dim anorm As Single, g As Single, hhscale As Single
        'Extra variables for VBasic.
        Dim sTemp1 As Single, n As Integer, m As Integer

        m = sA.GetUpperBound(0) + 1
        n = sA.GetUpperBound(1) + 1

        If m < n Then
            strError = "Not enough rows in A (underdetermined system)"
            Return False
        End If

        ReDim sU(m - 1, n - 1)
        ReDim sW(n - 1)
        ReDim sV(n - 1, n - 1)
        ReDim rv1(n - 1)

        'Copy the matrix A in U.
        sA.Copy(sA, sU, sA.Length)

        'Householder reduction to bidiagonal form
        anorm = 0.0#
        For i = 1 To n
            l = i + 1
            rv1(i - 1) = hhscale * g
            g = 0.0#
            s = 0.0#
            hhscale = 0.0#
            If i <= m Then
                For k = i To m
                    hhscale = hhscale + Math.Abs(sU(k - 1, i - 1))
                Next k

                If hhscale <> 0.0# Then
                    For k = i To m
                        sU(k - 1, i - 1) = sU(k - 1, i - 1) / hhscale
                        s = s + sU(k - 1, i - 1) * sU(k - 1, i - 1)
                    Next k

                    f = sU(i - 1, i - 1)
                    If f >= 0 Then
                        g = -Math.Sqrt(s)
                    Else
                        g = Math.Sqrt(s)
                    End If

                    h = f * g - s
                    sU(i - 1, i - 1) = f - g
                    If i <> n Then
                        For j = l To n
                            s = 0.0#
                            For k = i To m
                                s = s + sU(k - 1, i - 1) * sU(k - 1, j - 1)
                            Next k
                            f = s / h
                            For k = i To m
                                sU(k - 1, j - 1) = sU(k - 1, j - 1) + f * sU(k - 1, i - 1)
                            Next k
                        Next j
                    End If

                    For k = i To m
                        sU(k - 1, i - 1) = sU(k - 1, i - 1) * hhscale
                    Next k

                End If
            End If

            sW(i - 1) = hhscale * g
            g = 0.0#
            s = 0.0#
            hhscale = 0.0#
            If i <= m And i <> n Then
                For k = l To n
                    hhscale = hhscale + Math.Abs(sU(i - 1, k - 1))
                Next k

                If hhscale <> 0.0# Then
                    For k = l To n
                        sU(i - 1, k - 1) = sU(i - 1, k - 1) / hhscale
                        s = s + sU(i - 1, k - 1) * sU(i - 1, k - 1)
                    Next k

                    f = sU(i - 1, l - 1)
                    If f >= 0 Then
                        g = -Math.Sqrt(s)
                    Else
                        g = Math.Sqrt(s)
                    End If
                    h = f * g - s
                    sU(i - 1, l - 1) = f - g

                    For k = l To n
                        rv1(k - 1) = sU(i - 1, k - 1) / h
                    Next k

                    If i <> m Then
                        For j = l To m
                            s = 0.0#
                            For k = l To n
                                s = s + sU(j - 1, k - 1) * sU(i - 1, k - 1)
                            Next k

                            For k = l To n
                                sU(j - 1, k - 1) = sU(j - 1, k - 1) + s * rv1(k - 1)
                            Next k
                        Next j
                    End If

                    For k = l To n
                        sU(i - 1, k - 1) = sU(i - 1, k - 1) * hhscale
                    Next k

                End If
            End If

            sTemp1 = Math.Abs(sW(i - 1)) + Math.Abs(rv1(i - 1))
            If anorm < sTemp1 Then anorm = sTemp1
        Next i
        'Call DisplayMatrix("Bidiagonal form", a())

        'Accumulation of right-hand transformations
        For i = n To 1 Step -1
            If i < n Then
                If g <> 0.0# Then
                    For j = l To n
                        sV(j - 1, i - 1) = (sU(i - 1, j - 1) / sU(i - 1, l - 1)) / g
                    Next j

                    For j = l To n
                        s = 0.0#
                        For k = l To n
                            s = s + sU(i - 1, k - 1) * sV(k - 1, j - 1)
                        Next k

                        For k = l To n
                            sV(k - 1, j - 1) = sV(k - 1, j - 1) + s * sV(k - 1, i - 1)
                        Next k
                    Next j
                End If

                For j = l To n
                    sV(i - 1, j - 1) = 0.0#
                    sV(j - 1, i - 1) = 0.0#
                Next j

            End If

            sV(i - 1, i - 1) = 1.0#
            g = rv1(i - 1)
            l = i
        Next i

        'Accumulation of left-hand transformations
        For i = n To 1 Step -1
            l = i + 1
            g = sW(i - 1)
            If i < n Then
                For j = l To n
                    sU(i - 1, j - 1) = 0.0#
                Next j
            End If

            If g <> 0.0# Then
                g = 1.0# / g
                If i <> n Then
                    For j = l To n
                        s = 0.0#
                        For k = l To m
                            s = s + sU(k - 1, i - 1) * sU(k - 1, j - 1)
                        Next k

                        f = (s / sU(i - 1, i - 1)) * g
                        For k = i To m
                            sU(k - 1, j - 1) = sU(k - 1, j - 1) + f * sU(k - 1, i - 1)
                        Next k
                    Next j
                End If

                For j = i To m
                    sU(j - 1, i - 1) = sU(j - 1, i - 1) * g
                Next j
            Else
                For j = i To m
                    sU(j - 1, i - 1) = 0.0#
                Next j
            End If

            sU(i - 1, i - 1) = sU(i - 1, i - 1) + 1.0#
        Next i

        'Diagonalization of the bidiagonal form (QR algorythm)
        For k = n To 1 Step -1
            For its = 1 To 30
                'Debug.Print "Iteration " & its
                Flag = True
                For l = k To 1 Step -1
                    nm = l - 1
                    If Math.Abs(rv1(l - 1)) + anorm = anorm Then
                        Flag = False
                        Exit For
                    End If

                    If Math.Abs(sW(nm - 1)) + anorm = anorm Then
                        Exit For
                    End If
                Next l

                If Flag = True Then
                    c = 0.0#
                    s = 1.0#
                    For i = l To k
                        f = s * rv1(i - 1)
                        If (Math.Abs(f) + anorm) <> anorm Then
                            g = sW(i - 1)
                            h = Pythagoras(f, g)
                            sW(i - 1) = h
                            h = 1.0# / h
                            c = g * h
                            s = (-f * h)
                            For j = 1 To m
                                sY = sU(j - 1, nm - 1)
                                sz = sU(j - 1, i - 1)
                                sU(j - 1, nm - 1) = sY * c + sz * s
                                sU(j - 1, i - 1) = sz * c - sY * s
                            Next j
                        End If
                    Next i
                End If
                sz = sW(k - 1)

                'Test for convergence
                If l = k Then
                    If sz < 0.0# Then
                        sW(k - 1) = -sz
                        For j = 1 To n
                            sV(j - 1, k - 1) = -sV(j - 1, k - 1)
                        Next j
                    End If
                    Exit For
                End If

                If its = 30 Then
                    strError = "Too many iterations"
                    Return False
                End If

                sX = sW(l - 1)
                nm = k - 1
                sY = sW(nm - 1)
                g = rv1(nm - 1)
                h = rv1(k - 1)
                f = ((sY - sz) * (sY + sz) + (g - h) * (g + h)) / (2.0# * h * sY)
                g = Pythagoras(f, 1.0#)
                If f > 0.0# Then
                    f = ((sX - sz) * (sX + sz) + h * ((sY / (f + Math.Abs(g))) - h)) / sX
                Else
                    f = ((sX - sz) * (sX + sz) + h * ((sY / (f - Math.Abs(g))) - h)) / sX
                End If

                c = 1.0#
                s = 1.0#
                For j = l To nm
                    i = j + 1
                    g = rv1(i - 1)
                    sY = sW(i - 1)
                    h = s * g
                    g = c * g
                    sz = Pythagoras(f, h)
                    rv1(j - 1) = sz
                    c = f / sz
                    s = h / sz
                    f = sX * c + g * s
                    g = g * c - sX * s
                    h = sY * s
                    sY = sY * c
                    For jj = 1 To n
                        sX = sV(jj - 1, j - 1)
                        sz = sV(jj - 1, i - 1)
                        sV(jj - 1, j - 1) = sX * c + sz * s
                        sV(jj - 1, i - 1) = sz * c - sX * s
                    Next jj
                    sz = Pythagoras(f, h)
                    sW(j - 1) = sz
                    If sz <> 0.0# Then
                        sz = 1.0# / sz
                        c = f * sz
                        s = h * sz
                    End If
                    f = c * g + s * sY
                    sX = c * sY - s * g
                    For jj = 1 To m
                        sY = sU(jj - 1, j - 1)
                        sz = sU(jj - 1, i - 1)
                        sU(jj - 1, j - 1) = sY * c + sz * s
                        sU(jj - 1, i - 1) = sz * c - sY * s
                    Next jj
                Next j
                rv1(l - 1) = 0.0#
                rv1(k - 1) = f
                sW(k - 1) = sX
            Next its
        Next k
        Return True
    End Function

    Private Shared Function Pythagoras(ByVal a As Single, ByVal b As Single) As Single
        Dim at As Single, bt As Single, ct As Single

        at = Math.Abs(a)
        bt = Math.Abs(b)
        If at > bt Then
            ct = bt / at
            Pythagoras = at * Math.Sqrt(1.0# + ct * ct)
        Else
            If bt = 0.0# Then
                'Means a is also 0
                Pythagoras = 0.0#
            Else
                ct = at / bt
                Pythagoras = bt * Math.Sqrt(1.0# + ct * ct)
            End If
        End If
    End Function

    Public Overloads Shared Sub Add(ByVal sV1() As Single, ByVal sV2() As Single, ByVal sR() As Single)
        Dim i, iHiCol As Integer
        iHiCol = sV1.GetUpperBound(0)
        For i = 0 To iHiCol
            sR(i) = sV1(i) + sV2(i)
        Next
    End Sub

    Public Overloads Shared Sub Add(ByVal sM1(,) As Single, ByVal sM2(,) As Single, ByVal sMR(,) As Single)
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sM1, iHiRow, iHiCol)
        For j = 0 To iHiCol
            For i = 0 To iHiRow
                sMR(i, j) = sM1(i, j) + sM2(i, j)
            Next i
        Next j
    End Sub

    Public Overloads Shared Sub Subtract(ByVal sV1() As Single, ByVal sV2() As Single, ByVal sR() As Single)
        Dim i As Integer, iHiCol As Integer
        iHiCol = sV1.GetUpperBound(0)
        For i = 0 To iHiCol
            sR(i) = sV1(i) - sV2(i)
        Next
    End Sub

    Public Overloads Shared Sub Subtract(ByVal sM1(,) As Single, ByVal sM2(,) As Single, ByVal sMR(,) As Single)
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sM1, iHiRow, iHiCol)
        For j = 0 To iHiCol
            For i = 0 To iHiRow
                sMR(i, j) = sM1(i, j) - sM2(i, j)
            Next i
        Next j
    End Sub

    Public Overloads Shared Function Norm(ByVal sV1() As Single) As Single
        Return Norm(sV1, NormOrder.Euclidean)
    End Function

    Public Overloads Shared Function Norm(ByVal sV1() As Single, ByVal iOrder As NormOrder) As Single
        'Compute norm of given order
        Dim i As Integer, sNorm As Single = 0.0, iHiCol As Integer
        iHiCol = sV1.GetUpperBound(0)
        Select Case iOrder
            Case NormOrder.AbsoluteValue
                For i = 0 To iHiCol
                    sNorm += Math.Abs(sV1(i))
                Next
            Case NormOrder.Euclidean
                For i = 0 To iHiCol
                    sNorm += sV1(i) ^ 2
                Next
                sNorm = sNorm ^ 0.5
            Case NormOrder.Max
                sNorm = 0
                For i = 0 To iHiCol
                    Dim sTemp As Single = Math.Abs(sV1(i))
                    If sTemp > sNorm Then sNorm = sTemp
                Next
        End Select
        Return sNorm
    End Function

    Public Overloads Shared Sub Mean(ByVal sM(,) As Single, ByVal sV() As Single)
        'Compute columnwise mean
        Dim i, iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)
        Sum(sM, sV)
        For i = 0 To iHiCol
            sV(i) = sV(i) / (iHiCol + 1)
        Next i
    End Sub

    Public Overloads Shared Function Mean(ByVal sV() As Single) As Single
        'Compute average of a vector
        Dim sMean As Single
        sMean = Sum(sV)
        sMean /= sV.GetLength(0)
        Return sMean
    End Function


    Public Overloads Shared Sub Sum(ByVal sM(,) As Single, ByVal sV() As Single)
        'Compute columnwise sum of matrix
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sM, iHiRow, iHiCol)

        For j = 0 To iHiCol
            sV(j) = 0.0
            For i = 0 To iHiRow
                sV(j) += sM(j, i)
            Next i
        Next j
    End Sub

    Public Overloads Shared Function Sum(ByVal sV() As Single) As Single
        'Compute sum of elements of vector
        Dim sSum As Single = 0
        Dim i, iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)

        For i = 0 To iHiCol
            sSum = sSum + sV(i)
        Next i
        Return sSum
    End Function

    Public Overloads Shared Function Max(ByVal sV() As Single, ByRef iPos As Integer) As Single
        'Find max of a vector
        Dim i As Integer, sMax As Single = 0.0
        Dim iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)

        For i = 0 To iHiCol
            If sV(i) > sMax Then
                iPos = i
                sMax = sV(i)
            End If
        Next i
        Return sMax
    End Function

    Public Overloads Shared Function Max(ByVal sV() As Single) As Single
        Dim iPos As Integer
        Return Max(sV, iPos)
    End Function

    Public Overloads Shared Function Max(ByVal sM(,) As Single) As Single
        'Find max of a matrix
        Dim i, j As Integer
        Return Max(sM, i, j)
    End Function

    Public Overloads Shared Function Max(ByVal sM(,) As Single, ByRef iCol As Integer, ByRef iRow As Integer) As Single
        'Find max of a matrix
        Dim sMAx As Single = 0
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sM, iHiRow, iHiCol)
        For j = 0 To iHiCol
            For i = 0 To iHiRow
                If sM(i, j) > sMAx Then
                    iCol = j
                    iRow = i
                    sMAx = sM(i, j)
                End If
            Next i
        Next j
        Return sMAx
    End Function

    Public Overloads Shared Function Scale(ByVal sX As Single, ByVal sOffset As Single, ByVal sScale As Single) As Single
        'Scale a scalar with an offset. For vectors and matrices this would lead to too many 
        'different versions, so use Subtract to have an offset.
        Return (sX - sOffset) * sScale
    End Function

    Public Overloads Shared Sub Scale(ByVal sScale As Single,
                                      ByVal sV2() As Single,
                                      ByVal sY() As Single)
        'Scale elements of vector V2 using the scalar sScale
        Dim i As Integer, iHiRow As Integer
        iHiRow = UBound(sV2)
        For i = 0 To iHiRow
            sY(i) = sScale * sV2(i)
        Next i
    End Sub

    Public Overloads Shared Sub Scale(ByVal sV1() As Single,
                                      ByVal sV2() As Single,
                                      ByVal sY() As Single)
        'Scale elements of vector V2 using the elements of V1
        Dim i As Integer, iHiRow As Integer
        iHiRow = UBound(sV2)
        For i = 0 To iHiRow
            sY(i) = sV1(i) * sV2(i)
        Next i
    End Sub

    Public Overloads Shared Sub Scale(ByVal sScale As Single,
                                    ByVal sB(,) As Single,
                                    ByVal sY(,) As Single)
        'Scale elements of matrix sB using  sScale
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sB, iHiRow, iHiCol)
        For i = 0 To iHiRow
            For j = 0 To iHiCol
                sY(i, j) = sScale * sB(i, j)
            Next j
        Next i
    End Sub

    Public Overloads Shared Sub Scale(ByVal sA(,) As Single,
                                    ByVal sB(,) As Single,
                                    ByVal sY(,) As Single)
        'Scale elements of matrix sB using the corresponding elements of sA
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sB, iHiRow, iHiCol)
        For i = 0 To iHiRow
            For j = 0 To iHiCol
                sY(i, j) = sA(i, j) * sB(i, j)
            Next j
        Next i
    End Sub

    Public Overloads Shared Sub Scale(ByVal sRowScales() As Single,
                                      ByVal sB(,) As Single,
                                      ByVal sY(,) As Single)
        'Scale elements of matrix sB using the corresponding elements of sRowScales, per ROW
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sB, iHiRow, iHiCol)
        For i = 0 To iHiRow
            For j = 0 To iHiCol
                sY(i, j) = sRowScales(i) * sB(i, j)
            Next j
        Next i
    End Sub

    Public Overloads Shared Sub Scale(ByVal sB(,) As Single,
                                      ByVal sColScales() As Single,
                                      ByVal sY(,) As Single)
        'Scale elements of matrix sB using the corresponding elements of sColScales, per col
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sB, iHiRow, iHiCol)
        For i = 0 To iHiRow
            For j = 0 To iHiCol
                sY(i, j) = sColScales(j) * sB(i, j)
            Next j
        Next i
    End Sub

    Public Overloads Shared Sub Product(ByVal sA(,) As Single,
                     ByVal sB(,) As Single,
                     ByVal sC(,) As Single)
        'Compute A * B and store in C. 
        'Raise a fatal run-time error if any errors (no return value)!
        Dim i, j, k, iAHiRow, iAHiCol As Integer
        GetBounds(sA, iAHiRow, iAHiCol)
        Dim iBHiRow, iBHiCol As Integer
        GetBounds(sB, iBHiRow, iBHiCol)
        Dim iCHiRow, iCHiCol As Integer
        GetBounds(sC, iCHiRow, iCHiCol)

        If (((iAHiCol) <> (iBHiRow)) Or
            ((iAHiRow) <> (iCHiRow)) Or
            ((iBHiCol) <> (iCHiCol))) Then
            MsgBox("Algebra.Product: Incompatible matrix dimensions", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
        End If

        For i = 0 To iCHiRow
            For j = 0 To iCHiCol
                sC(i, j) = 0.0
                For k = 0 To iAHiCol
                    sC(i, j) += sA(i, k) * sB(k, j)
                Next k
            Next j
        Next i
    End Sub

    Public Overloads Shared Function Product(ByVal sV1() As Single, ByVal sV2() As Single) As Single
        'Return the scalar product of two vectors.
        Dim i As Integer, iHiRow As Integer, sResult As Single

        iHiRow = UBound(sV1)
        For i = 0 To iHiRow
            sResult = sResult + sV1(i) * sV2(i)
        Next i
        Return sResult
    End Function

    Public Overloads Shared Sub Product(ByVal sM() As Single,
                                        ByVal sX() As Single,
                                        ByVal sY(,) As Single)
        'Multiply a vector times a vector (Y = M.Y), by interpreting the vector M as a columnmatrix,
        'and X as a rowmatrix. Result is a matrix
        Dim sA(0, sM.GetUpperBound(0)) As Single, sB(sX.GetUpperBound(0), 0) As Single

        SetMatrixColumn(sA, sM, 0)
        SetMatrixRow(sB, sX, 0)
        Product(sA, sB, sY)
    End Sub

    Public Overloads Shared Sub Product(ByVal sM(,) As Single,
                                        ByVal sX() As Single,
                                        ByVal sY() As Single)
        'Multiply a matrix times a vector (y = M.x), by interpreting the vector X as a columnmatrix.
        Dim sB(sX.GetUpperBound(0), 0), sC(sM.GetUpperBound(0), 0) As Single

        SetMatrixColumn(sB, sX, 0)
        Product(sM, sB, sC)
        GetMatrixColumn(sC, sY, 0)
    End Sub

    Public Overloads Shared Sub Product(ByVal sX() As Single,
                                        ByVal sM(,) As Single,
                                        ByVal sY() As Single)
        'Multiply a vector with a matrix (y = x.M), by interpreting the vector X as a rowmatrix.
        Dim iHiCol As Integer = sX.GetUpperBound(0)
        Dim sB(0, iHiCol), sC(0, iHiCol) As Single

        SetMatrixRow(sB, sX, 0)
        Product(sM, sB, sC)
        GetMatrixRow(sC, sY, 0)
    End Sub

    Public Shared Sub SubMatrix(ByVal sA(,) As Single,
                         ByVal sB(,) As Single,
                         ByVal iRow As Integer,
                         ByVal iCol As Integer)
        'Extract submatrix of the dimensions of B using row and col
        'as start values in sA. sA and sB can be mixed one and zero-
        'based, but iRow and iCol are interpreted according to sA
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sB, iHiRow, iHiCol)
        For i = 0 To iHiRow
            For j = 0 To iHiCol
                sB(i, j) = sA(i + iRow, j + iCol)
            Next j
        Next i
    End Sub
    Public Overloads Shared Sub GetMatrixColumn(ByVal sM(,) As Single,
                            ByVal sV() As Single,
                            ByVal iCol As Integer)
        GetMatrixColumn(sM, sV, iCol, 0)
    End Sub

    Public Overloads Shared Sub GetMatrixColumn(ByVal sM(,) As Single,
                            ByVal sV() As Single,
                            ByVal iCol As Integer,
                            ByVal iStartRow As Integer)
        'Fill vector with matrix col
        Dim i, iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)
        For i = 0 To iHiCol
            sV(i) = sM(i + iStartRow, iCol)
        Next i
    End Sub

    Public Overloads Shared Sub GetMatrixRow(ByVal sM(,) As Single,
                          ByVal sV() As Single,
                          ByVal iRow As Integer)
        GetMatrixRow(sM, sV, iRow, 0)
    End Sub

    Public Overloads Shared Sub GetMatrixRow(ByVal sM(,) As Single,
                            ByVal sV() As Single,
                            ByVal iRow As Integer,
                            ByVal iStartCol As Integer)
        'Fill vector with matrix row. 
        Dim i, iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)
        For i = 0 To iHiCol
            sV(i) = sM(iRow, i + iStartCol)
        Next i
    End Sub

    Public Overloads Shared Sub SetMatrixColumn(ByVal sM(,) As Single,
                              ByVal sV() As Single,
                              ByVal iCol As Integer,
                              ByVal iStartRow As Integer)
        'Fill matrix col with vector
        Dim i, iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)
        For i = 0 To iHiCol
            sM(i + iStartRow, iCol) = sV(i)
        Next i
    End Sub

    Public Overloads Shared Sub SetMatrixColumn(ByVal sM(,) As Single,
                               ByVal sV() As Single,
                               ByVal iCol As Integer)
        SetMatrixColumn(sM, sV, iCol, 0)
    End Sub

    Public Overloads Shared Sub SetMatrixRow(ByVal sM(,) As Single,
                          ByVal sV() As Single,
                          ByVal iRow As Integer)
        SetMatrixRow(sM, sV, iRow, 0)
    End Sub

    Public Overloads Shared Sub SetMatrixRow(ByVal sM(,) As Single,
                            ByVal sV() As Single,
                            ByVal iRow As Integer,
                            ByVal iStartCol As Integer)
        Dim i, iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)
        For i = 0 To iHiCol
            sM(iRow, i + iStartCol) = sV(i)
        Next i
    End Sub

    Public Shared Sub Transpose(ByVal sA(,) As Single, ByVal sAt(,) As Single)
        'Transpose matrix A and put result in At. Output has
        'same base as input. Input arguments must be different!
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sA, iHiRow, iHiCol)

        For i = 0 To iHiRow
            For j = 0 To iHiCol
                sAt(j, i) = sA(i, j)
            Next j
        Next i
    End Sub

    Public Shared Function Load(ByVal strFile As String, ByRef sM(,) As Single) As Boolean
        'Read a tex file with a matrix or vector stored separated by spaces and
        'newlines. sM will be redimensioned as necessary and must be
        'a dynamic array. Redimensioning can only affect the last dimension!
        'When a vector is read in the matrix will be of size n x 1, and can easily 
        'be converted to a vector
        Dim iNrCols As Integer
        Dim iRowNr As Integer, iNrRows As Integer, iColNr As Integer
        Dim sMt(,) As Single, strText As String, strTextItems() As String

        Try
            FileOpen(5, strFile, OpenMode.Input, OpenAccess.Read)
        Catch e As Exception
            MsgBox("Algebra.Load:" & e.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
            Return False
        End Try

        iRowNr = 0
        iNrCols = 0
        Do While Not EOF(5)
            'Read first line to count number of columns
            strText = Trim(LineInput(5))

            If strText.Length > 0 Then
                strText = strText.Replace("  ", " ") 'Make sure no 2 spaces are in the string ...
                strText = strText.Replace("   ", " ") 'Make sure no 3 spaces are in the string ...
                strTextItems = strText.Split()

                'Redimension the array if the nr of cols is known, i.e. after
                'reading the first line.
                If iRowNr = 0 Then
                    iNrCols = (strTextItems.GetUpperBound(0) + 1)
                    ReDim sMt(iNrCols - 1, 0)
                Else
                    ReDim Preserve sMt(iNrCols - 1, iRowNr)
                End If

                'Read values into transposed matrix
                For iColNr = 0 To iNrCols - 1
                    'sMt(iColNr, iRowNr) = CSng(strTextItems(iColNr))
                    sMt(iColNr, iRowNr) = Val(strTextItems(iColNr))
                Next
                iRowNr += 1
            End If
        Loop

        'close file
        FileClose(5)

        'Transpose matrix to output format
        ReDim sM(iRowNr - 1, iNrCols - 1)
        Transpose(sMt, sM)
        Return True
    End Function

    Public Overloads Shared Sub Save(ByVal strFile As String,
                        ByVal sM(,) As Single)
        Save(strFile, sM, 16, 2)
    End Sub

    Public Overloads Shared Sub Save(ByVal strFile As String,
                          ByVal sM(,) As Single,
                          ByVal iPrecBeforeDec As Integer,
                          ByVal iPrecAfterDec As Integer)
        'Save a matrix to file.
        Dim strF As String
        Dim i, j, iHiRow, iHiCol As Integer

        If iPrecAfterDec = -1 Then
            strF = "0."
        Else
            For i = 1 To iPrecAfterDec
                strF = strF & "0"
            Next
            strF = strF & "."
        End If

        For i = 1 To iPrecBeforeDec
            strF = strF & "#"
        Next

        If System.IO.File.Exists(strFile) Then System.IO.File.Delete(strFile)

        Try
            FileOpen(5, strFile, OpenMode.Output, OpenAccess.Write)
            GetBounds(sM, iHiRow, iHiCol)
            For i = 0 To iHiRow
                For j = 0 To iHiCol - 1
                    Print(5, Format(sM(i, j), strF), SPC(1))
                Next j
                PrintLine(5, SPC(1), Format(sM(i, iHiCol), strF))
            Next i
            FileClose(5)
        Catch e As Exception
            MsgBox("Algebra.Save (file = " & strFile & "):" & e.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Shared Sub GetBounds(ByVal sM(,) As Single,
                                 ByRef iHiRow As Integer,
                                 ByRef iHiCol As Integer)
        iHiRow = sM.GetUpperBound(0)
        iHiCol = sM.GetUpperBound(1)
    End Sub

    Public Overloads Shared Function ToString(ByVal sM(,) As Single) As String
        Dim strText As String
        Dim i, j, iHiRow, iHiCol As Integer
        GetBounds(sM, iHiRow, iHiCol)
        For i = 0 To iHiRow
            For j = 0 To iHiCol - 1
                strText = strText & sM(i, j).ToString & " "
            Next j
            strText = strText & sM(i, iHiCol).ToString & vbNewLine
        Next i
        Return strText
    End Function

    Public Overloads Shared Function ToString(ByVal sV() As Single) As String
        Dim strText As String
        Dim i, iHiCol As Integer
        iHiCol = sV.GetUpperBound(0)
        For i = 0 To iHiCol - 1
            strText = strText & sV(i).ToString & " "
        Next i
        strText = strText & sV(iHiCol).ToString & vbNewLine
        Return strText
    End Function
End Class

