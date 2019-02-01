Imports System
Imports System.Drawing
Imports AVT.VmbAPINET

Public Class FrameEventArgs
        Inherits EventArgs

        Private m_Image As Image = Nothing
        Private m_Frame As Frame = Nothing
        Private m_Exception As Exception = Nothing

        Public Sub New(ByVal image As Image)
            If image Is Nothing Then
                Throw New ArgumentNullException("image")
            End If

            m_Image = image
        End Sub

        Public Sub New(ByVal frame As Frame)
            If frame Is Nothing Then
                Throw New ArgumentNullException("frame")
            End If

            m_Frame = frame
        End Sub

        Public Sub New(ByVal exception As Exception)
            If exception Is Nothing Then
                Throw New ArgumentNullException("exception")
            End If

            m_Exception = exception
        End Sub

        Public ReadOnly Property Image As Image
            Get
                Return m_Image
            End Get
        End Property

        Public ReadOnly Property Frame As Frame
            Get
                Return m_Frame
            End Get
        End Property

        Public ReadOnly Property Exception As Exception
            Get
                Return m_Exception
            End Get
        End Property
    End Class

