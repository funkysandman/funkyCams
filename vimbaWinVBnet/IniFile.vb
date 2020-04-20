Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text


Class IniFile
        Private Path As String
        Private EXE As String = Assembly.GetExecutingAssembly().GetName().Name
    Private Declare Function GetPrivateProfileString _
   Lib "kernel32" Alias "GetPrivateProfileStringA" _
  (ByVal lpSectionName As String,
   <System.Runtime.InteropServices.MarshalAsAttribute(
        System.Runtime.InteropServices.UnmanagedType.AsAny)>
        ByVal lpKeyName As Object,
   ByVal lpDefault As String,
   ByVal lpbuffurnedString As String,
   ByVal nBuffSize As Long,
   ByVal lpFileName As String) As Long

    Private Declare Function WritePrivateProfileString _
   Lib "kernel32" Alias "WritePrivateProfileStringA" _
  (ByVal lpSectionName As String,
   <System.Runtime.InteropServices.MarshalAsAttribute(
        System.Runtime.InteropServices.UnmanagedType.AsAny)>
        ByVal lpKeyName As Object,
   <System.Runtime.InteropServices.MarshalAsAttribute(
        System.Runtime.InteropServices.UnmanagedType.AsAny)>
        ByVal lpString As Object,
   ByVal lpFileName As String) As Long

    Public Sub New(ByVal Optional IniPath As String = Nothing)
        Path = New FileInfo(If(IniPath, EXE & ".ini")).FullName.ToString()
    End Sub

    Public Function Read(ByVal Key As String, ByVal Optional Section As String = Nothing) As String
        Dim RetVal As String = ""
        GetPrivateProfileString(If(Section, EXE), Key, "", RetVal, 255, Path)
            Return RetVal.ToString()
        End Function

        Public Sub Write(ByVal Key As String, ByVal Value As String, ByVal Optional Section As String = Nothing)
            WritePrivateProfileString(If(Section, EXE), Key, Value, Path)
        End Sub

        Public Sub DeleteKey(ByVal Key As String, ByVal Optional Section As String = Nothing)
            Write(Key, Nothing, If(Section, EXE))
        End Sub

        Public Sub DeleteSection(ByVal Optional Section As String = Nothing)
            Write(Nothing, Nothing, If(Section, EXE))
        End Sub

        Public Function KeyExists(ByVal Key As String, ByVal Optional Section As String = Nothing) As Boolean
            Return Read(Key, Section).Length > 0
        End Function
    End Class
