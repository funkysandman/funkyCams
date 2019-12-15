Option Strict Off
Option Explicit On
Module Declares
	
	'Windows API constants
	Public Const BITSPIXEL As Short = 12
	Public Const SYSTEM_FONT As Short = 13
	Public Const LR_LOADFROMFILE As Integer = &H10
	Public Const CAPS1 As Short = 94
	Public Const C1_TRANSPARENT As Integer = &H1
	Public Const NEWTRANSPARENT As Short = 3
	Public Const BI_RGB As Short = 0
	Public Const DIB_PAL_COLORS As Short = 1 ' color table in RGBs
	'Math Constants
	Public Const PI As Double = 3.14159265358979
	
	'Other DDL's
  Public Declare Function VarPtr Lib "msvbvm60.dll" Alias "VarPtr" (ByRef Ptr() As Short) As Integer

	
	Public Structure BITMAPINFOHEADER '40 bytes
		Dim biSize As Integer
		Dim biWidth As Integer
		Dim biHeight As Integer
		Dim biPlanes As Short
		Dim biBitCount As Short
		Dim biCompression As Integer
		Dim biSizeImage As Integer
		Dim biXPelsPerMeter As Integer
		Dim biYPelsPerMeter As Integer
		Dim biClrUsed As Integer
		Dim biClrImportant As Integer
	End Structure
	Public Structure RGBQUAD
		Dim rgbBlue As Byte
		Dim rgbGreen As Byte
		Dim rgbRed As Byte
		Dim rgbReserved As Byte
	End Structure
	Public Structure BITMAPINFO
		Dim bmiHeader As BITMAPINFOHEADER
		Dim bmiColors As RGBQUAD
	End Structure
	
	'Declarations
  Public Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Long) As Integer
	Public Declare Sub ReleaseCapture Lib "user32" ()
	Public Const WM_NCLBUTTONDOWN As Integer = &HA1
	Public Const HTCAPTION As Short = 2
	
	
	'Windows API structures
	Public Structure OSVERSIONINFO
		Dim dwOSVersionInfoSize As Integer
		Dim dwMajorVersion As Integer
		Dim dwMinorVersion As Integer
		Dim dwBuildNumber As Integer
		Dim dwPlatformId As Integer
    <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst:=128)> Public szCSDVersion() As Char
	End Structure
	
	
	
	
	Public Structure udtScene
		Dim Start As Integer
    Dim Stop_Renamed As Integer
	End Structure
	Public Structure RECT_API
    Dim Left_Renamed As Integer
		Dim Top As Integer
    Dim Right_Renamed As Integer
		Dim Bottom As Integer
	End Structure
	
	Public Structure Point
		Dim x As Integer
		Dim y As Integer
	End Structure
	
	
	'Other structures
	
	
	
	
	'Windows API functions
	Public Declare Function BitBlt Lib "gdi32" (ByVal hDestDC As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal xSrc As Integer, ByVal ySrc As Integer, ByVal dwRop As Integer) As Integer
	
  Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByVal lpvDest As Long, ByVal lpvSource As Long, ByVal cbCopy As Long)
	
	Public Declare Function CreateCompatibleBitmap Lib "gdi32" (ByVal hdc As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer) As Integer
	
	Public Declare Function CreateCompatibleDC Lib "gdi32" (ByVal hdc As Integer) As Integer
	
	Public Declare Function CreatePen Lib "gdi32" (ByVal nPenStyle As Integer, ByVal nWidth As Integer, ByVal crColor As Integer) As Integer
	
	Public Declare Function DeleteDC Lib "gdi32" (ByVal hdc As Integer) As Integer
	
	Public Declare Function DeleteObject Lib "gdi32" (ByVal hObject As Integer) As Integer
	
	Public Declare Function Ellipse Lib "gdi32" (ByVal hdc As Integer, ByVal X1 As Integer, ByVal Y1 As Integer, ByVal X2 As Integer, ByVal Y2 As Integer) As Integer
	
  Public Declare Function GetBitmapBits Lib "gdi32" (ByVal hBitmap As Integer, ByVal dwCount As Integer, ByRef lpBits As Long) As Integer
	
  Public Declare Function GetClientRect Lib "user32" (ByVal hWnd As Integer, ByRef lpRect As RECT_API) As Integer
	
	Public Declare Function GetDC Lib "user32" (ByVal hWnd As Integer) As Integer
	
	Public Declare Function GetDesktopWindow Lib "user32" () As Integer
	
	Public Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Integer, ByVal nIndex As Integer) As Integer
	
  Public Declare Function GetObjectA Lib "gdi32" (ByVal hObject As Integer, ByVal nCount As Integer, ByRef lpObject As Long) As Integer
	
  Public Declare Function GetObjectW Lib "gdi32" (ByVal hObject As Integer, ByVal nCount As Integer, ByRef lpObject As Long) As Integer
	
	Public Declare Function GetStockObject Lib "gdi32" (ByVal nIndex As Integer) As Integer
	
	Public Declare Function GetPixel Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer) As Integer
	
	Public Declare Function GetTickCount Lib "kernel32" () As Integer
	
  Public Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" (ByRef lpVersionInformation As OSVERSIONINFO) As Integer
	
  Public Declare Function IntersectRect Lib "user32" (ByRef lpDestRect As RECT_API, ByRef lpSrc1Rect As RECT_API, ByRef lpSrc2Rect As RECT_API) As Integer
	
	Public Declare Function LineTo Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer) As Integer
	
	Public Declare Function LoadImage Lib "user32"  Alias "LoadImageA"(ByVal hInst As Integer, ByVal Filename As String, ByVal un1 As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal opmode As Integer) As Integer
	
  Public Declare Function MoveTo Lib "gdi32" Alias "MoveToEx" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer, ByRef lpPoint As Point) As Integer
	
  Public Declare Function Polyline Lib "gdi32" (ByVal hdc As Integer, ByRef lpPoint As Point, ByVal nCount As Integer) As Integer
	
	Public Declare Function SelectObject Lib "gdi32" (ByVal hdc As Integer, ByVal hObject As Integer) As Integer
	
	Public Declare Function SetBkColor Lib "gdi32" (ByVal hdc As Integer, ByVal crColor As Integer) As Integer
	
	Public Declare Function SetBkMode Lib "gdi32" (ByVal hdc As Integer, ByVal nBkMode As Integer) As Integer
	
  Public Declare Function SetBitmapBits Lib "gdi32" (ByVal hBitmap As Integer, ByVal dwCount As Integer, ByRef lpBits As Long) As Integer
	
	Public Declare Function SetPixel Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer, ByVal crColor As Integer) As Integer
	
	Public Declare Function SetTextColor Lib "gdi32" (ByVal hdc As Integer, ByVal crColor As Integer) As Integer
	
	Public Declare Function StretchBlt Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hSrcDC As Integer, ByVal xSrc As Integer, ByVal ySrc As Integer, ByVal nSrcWidth As Integer, ByVal nSrcHeight As Integer, ByVal dwRop As Integer) As Integer
	
	Public Declare Function TextOutA Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer, ByVal lpString As String, ByVal nCount As Integer) As Integer
	
	Public Declare Function TextOutW Lib "gdi32" (ByVal hdc As Integer, ByVal x As Integer, ByVal y As Integer, ByVal lpString As String, ByVal nCount As Integer) As Integer
	
  Public Declare Function ValidateRect Lib "user32" (ByVal hWnd As Integer, ByRef lpRect As RECT_API) As Integer
	
	
	
	
  Public Declare Function SetDIBColorTable Lib "gdi32" (ByVal hdc As Integer, ByVal un1 As Integer, ByVal un2 As Integer, ByRef pcRGBQuad As RGBQUAD) As Integer
  Public Declare Function CreateDIBSection Lib "gdi32" (ByVal hdc As Integer, ByRef pBitmapInfo As BITMAPINFOHEADER, ByVal un As Integer, ByRef lpVoid As Long, ByVal handle As Integer, ByVal dw As Integer) As Integer
	
	
	Public Const WINDING As Short = 2
	Public Const ALTERNATE As Short = 1
	Public Const RGN_OR As Short = 2
	Public Const SRCCOPY As Integer = &HCC0020
	Public Const SRCPAINT As Integer = &HEE0086
	Public Const SRCAND As Integer = &H8800C6
	
	
  Declare Function CreatePolygonRgn Lib "gdi32" (ByRef lpPoint As Point, ByVal nCount As Integer, ByVal nPolyFillMode As Integer) As Integer
	Declare Function CreateRoundRectRgn Lib "gdi32" (ByVal X1 As Integer, ByVal Y1 As Integer, ByVal X2 As Integer, ByVal Y2 As Integer, ByVal X3 As Integer, ByVal Y3 As Integer) As Integer
	Declare Function CombineRgn Lib "gdi32" (ByVal hDestRgn As Integer, ByVal hSrcRgn1 As Integer, ByVal hSrcRgn2 As Integer, ByVal nCombineMode As Integer) As Integer
	Declare Function SetWindowRgn Lib "user32" (ByVal hWnd As Integer, ByVal hRgn As Integer, ByVal bRedraw As Boolean) As Integer
	
	Declare Function CreateEllipticRgn Lib "gdi32" (ByVal X1 As Integer, ByVal Y1 As Integer, ByVal X2 As Integer, ByVal Y2 As Integer) As Integer
	
  Declare Function CreatePolyPolygonRgn Lib "gdi32" (ByRef lpPoint As Point, ByRef lpPolyCounts As Integer, ByVal nCount As Integer, ByVal nPolyFillMode As Integer) As Integer
	
  Public Sub DrawLine(ByVal lHDC As Integer, ByRef Left_Renamed As Integer, ByVal Top As Integer, ByVal Right_Renamed As Integer, ByVal Bottom As Integer, ByVal Color As Integer)

    Dim pt As Point
    Dim hPen As Integer
    Dim hOldPen As Integer

    'create custom drawing pen and select it
    hPen = CreatePen(0, 1, Color)
    hOldPen = SelectObject(lHDC, hPen)

    'set starting point of line
    MoveTo(lHDC, Left_Renamed, Top, pt)

    'draw line to destination point
    LineTo(lHDC, Right_Renamed, Bottom)

    'delete the custom pen
    SelectObject(lHDC, hOldPen)
    DeleteObject(hPen)
  End Sub
	Public Sub DrawArrow(ByVal lHDC As Integer, ByRef xfrom As Integer, ByVal yfrom As Integer, ByVal dx As Integer, ByVal dy As Integer, ByRef HeadSize As Double, ByVal Color As Integer)
		
		Dim pt As Point
		Dim hPen As Integer
		Dim hOldPen As Integer
		Dim py0, px0, xto As Object
		Dim yto As Double
		Dim py1, px1, px2 As Object
		Dim py2 As Double
		Dim n As Double
		
		n = HeadSize
		
		'calculate arrowheadpoints
    xto = xfrom + dx
		yto = yfrom + dy
		
    px0 = xto - n * dx
    py0 = yto - n * dy
		
    px1 = CInt(px0 - 0.25 * n * dy)
    py1 = CInt(py0 + 0.25 * n * dx)
    px2 = CInt(px0 + 0.25 * n * dy)
    py2 = CInt(py0 - 0.25 * n * dx)
		
		'create custom drawing pen and select it
		hPen = CreatePen(0, 1, Color)
		hOldPen = SelectObject(lHDC, hPen)
		
		'set starting point of line
		MoveTo(lHDC, xfrom, yfrom, pt)
		
		'draw line to destination point
    LineTo(lHDC, xto, yto)
    LineTo(lHDC, px1, py1)
    LineTo(lHDC, px2, py2)
    LineTo(lHDC, xto, yto)
		
		'delete the custom pen
		SelectObject(lHDC, hOldPen)
		DeleteObject(hPen)
		
	End Sub
End Module