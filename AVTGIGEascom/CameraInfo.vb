Imports System


Public Class CameraInfo
        Private m_Name As String = Nothing
        Private m_ID As String = Nothing

        Public Sub New(ByVal name As String, ByVal id As String)
            If name Is Nothing Then
                Throw New ArgumentNullException("name")
            End If

            If name Is Nothing Then
                Throw New ArgumentNullException("id")
            End If

            Me.m_Name = name
            Me.m_ID = id
        End Sub

        Public ReadOnly Property Name As String
            Get
                Return Me.m_Name
            End Get
        End Property

        Public ReadOnly Property ID As String
            Get
                Return Me.m_ID
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return Me.m_Name
        End Function
    End Class

