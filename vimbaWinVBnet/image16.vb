Option Strict On
Option Explicit On

''' <summary>
''' Represents a single-channel 16-bit image.
''' Pixel data is stored in row-major order.
''' </summary>
Public Structure Image16

    Public Data() As UShort
    Public Width As Integer
    Public Height As Integer
    Public MaxValue As Integer

    Public Sub New(width As Integer, height As Integer, max As Integer)
        Me.Width = width
        Me.Height = height
        Me.MaxValue = max
        ReDim Me.Data(width * height - 1)
    End Sub

    Public Sub New(data() As UShort, width As Integer, height As Integer, max As Integer)

        If data Is Nothing Then
            Throw New ArgumentNullException(NameOf(data))
        End If

        If data.Length <> width * height Then
            Throw New ArgumentException(
                "Data length does not match image dimensions.")
        End If

        Me.Data = data
        Me.Width = width
        Me.Height = height
        Me.MaxValue = max

    End Sub

    Public ReadOnly Property PixelCount As Integer
        Get
            Return Width * Height
        End Get
    End Property

    Default Public Property Pixel(x As Integer, y As Integer) As UShort
        Get
            Return Data(y * Width + x)
        End Get
        Set(value As UShort)
            Data(y * Width + x) = value
        End Set
    End Property

    Public Function Clone() As Image16

        Dim copy(PixelCount - 1) As UShort
        Array.Copy(Data, copy, PixelCount)

        Return New Image16(copy, Width, Height, MaxValue)

    End Function

End Structure


''' <summary>
''' Represents a 16-bit RGB image.
''' Each channel is stored independently.
''' </summary>
Public Structure RgbImage16

    Public R As Image16
    Public G As Image16
    Public B As Image16
    Public MaxValue As Integer
    Public Sub New(width As Integer, height As Integer, max As Integer)

        R = New Image16(width, height, max)
        G = New Image16(width, height, max)
        B = New Image16(width, height, max)
        MaxValue = max
    End Sub

    Public Sub New(r As Image16,
                   g As Image16,
                   b As Image16)

        If r.Width <> g.Width OrElse
           r.Width <> b.Width OrElse
           r.Height <> g.Height OrElse
           r.Height <> b.Height Then

            Throw New ArgumentException(
                "RGB channels must have identical dimensions.")

        End If

        Me.R = r
        Me.G = g
        Me.B = b

    End Sub

    Public ReadOnly Property Width As Integer
        Get
            Return R.Width
        End Get
    End Property

    Public ReadOnly Property Height As Integer
        Get
            Return R.Height
        End Get
    End Property

End Structure


''' <summary>
''' Bayer pattern enumeration.
''' </summary>
Public Enum BayerPattern
    RGGB
    BGGR
    GBRG
    GRBG
End Enum