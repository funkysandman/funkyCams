﻿Imports System.Drawing

Public Class queueEntry
    Public img As Byte()
    Public filename As String
    Public cameraID As String
    Public dateTaken As DateTime
    Public width As Int16
    Public height As Int16
    Public rectangles As List(Of Rectangle)
End Class