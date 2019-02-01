
'Imports PylonC.NET
'Imports System.Collections.Generic

'Namespace NETSupportLibrary
'    ' Provides methods for listing all available devices. 

'    Public NotInheritable Class DeviceEnumerator
'        Private Sub New()
'        End Sub
'        ' Data class used for holding device data. 

'        Public Class Device
'            Public Name As String
'            ' The friendly name of the device. 
'            Public FullName As String
'            ' The full name string which is unique. 
'            Public Index As UInteger
'            ' The index of the device. 
'            Public Tooltip As String
'            ' The displayed tooltip 

'        End Class

'        ' Queries the number of available devices and creates a list with device data. 

'        Public Shared Function EnumerateDevices() As List(Of Device)
'            ' Create a list for the device data. 

'            Dim list As New List(Of Device)()

'            ' Enumerate all camera devices. You must call
'            '            PylonEnumerateDevices() before creating a device. 

'            Dim count As UInteger = PylonC.NET.Pylon.EnumerateDevices()

'            ' Get device data from all devices. 

'            For i As UInteger = 0 To count - 1
'                ' Create a new data packet. 

'                Dim device As New Device()
'                ' Get the device info handle of the device. 

'                Dim hDi As PYLON_DEVICE_INFO_HANDLE = PylonC.NET.Pylon.GetDeviceInfoHandle(i)
'                ' Get the name. 

'                device.Name = PylonC.NET.Pylon.DeviceInfoGetPropertyValueByName(hDi, PylonC.NET.Pylon.cPylonDeviceInfoFriendlyNameKey)
'                ' Get the serial number 

'                device.FullName = PylonC.NET.Pylon.DeviceInfoGetPropertyValueByName(hDi, PylonC.NET.Pylon.cPylonDeviceInfoFullNameKey)
'                ' Set the index. 

'                device.Index = i

'                ' Create tooltip 

'                Dim tooltip As String = ""
'                Dim propertyCount As UInteger = PylonC.NET.Pylon.DeviceInfoGetNumProperties(hDi)

'                If propertyCount > 0 Then
'                    For j As UInteger = 0 To propertyCount - 1
'                        tooltip += PylonC.NET.Pylon.DeviceInfoGetPropertyName(hDi, j) + ": " + PylonC.NET.Pylon.DeviceInfoGetPropertyValueByIndex(hDi, j)
'                        If j <> propertyCount - 1 Then
'                            tooltip += vbLf
'                        End If
'                    Next
'                End If
'                device.Tooltip = tooltip
'                ' Add to the list. 

'                list.Add(device)
'            Next
'            Return list
'        End Function
'    End Class
'End Namespace

''=======================================================
''Service provided by Telerik (www.telerik.com)
''Conversion powered by NRefactory.
''Twitter: @telerik
''Facebook: facebook.com/telerik
''=======================================================
