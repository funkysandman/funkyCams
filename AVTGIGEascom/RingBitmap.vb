Imports System.Drawing
Imports System.Drawing.Imaging
Imports AVT.VmbAPINET


Friend Class RingBitmap
        Private m_Size As Integer = 0
        Private m_Bitmaps As Bitmap() = Nothing
        Private m_BitmapSelector As Integer = 0

        Public Sub New(ByVal size As Integer)
            m_Size = size
            m_Bitmaps = New Bitmap(m_Size - 1) {}
        End Sub

        Public ReadOnly Property Image As Image
            Get
                Return m_Bitmaps(m_BitmapSelector)
            End Get
        End Property

        Public Sub FillNextBitmap(ByVal frame As Frame)
            SwitchBitmap()
            m_Bitmaps(m_BitmapSelector) = New Bitmap(CInt(frame.Width), CInt(frame.Height), PixelFormat.Format24bppRgb)
            frame.Fill(m_Bitmaps(m_BitmapSelector))
        End Sub

        Private Sub SwitchBitmap()
            m_BitmapSelector += 1

            If m_Size = m_BitmapSelector Then
                m_BitmapSelector = 0
            End If
        End Sub
    End Class

