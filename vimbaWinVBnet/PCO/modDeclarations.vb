Option Strict Off
Option Explicit On
Module modDeclarations
	
	' public vars
  Public hdriver As IntPtr
  Public hdialog As IntPtr
	Public errorCode As Integer
	
  Public camDesc As PCO_Description
  Public sensDesc As PCO_Sensor
  Public storeDesc As PCO_storage
	
	Public sX, sY As Object
	Public hevent As Integer
    Public b() As Byte 'Array is a WORD array and not a byte array!
    Public nBuf As Short
  Public bufAddr(10) As Integer
  Public iXres As Integer
  Public iYres As Integer

    Public pwbuf As Int64
    Public psbuf As Integer
	Public iTriggerMode As Short
  Public bmBild As Bitmap
  Public bmBildUsed As Integer
  Public divide As Integer
	' my camera types
	Public Structure HealthType
    Dim Error_Renamed As Integer
		Dim Warning As Integer
		Dim Status As Integer
		Dim ErrorTxt As String
		Dim WarningTxt As String
		Dim StatusTxt As String
	End Structure
	
	Public Structure TemperatureType
		Dim CCDindex As Short
		Dim CCDReal As Double
		Dim cameraindex As Short
		Dim CameraReal As Double
		Dim powerindex As Short
		Dim PowerReal As Double
	End Structure
	
	Public Structure ResolutionType
		Dim xAct As Short
		Dim yAct As Short
		Dim xMax As Short
		Dim yMax As Short
	End Structure
	
	Public Structure ROIType
		Dim x0 As Short
		Dim X1 As Short
		Dim y0 As Short
		Dim Y1 As Short
	End Structure
	
	Public Structure BinningType
		Dim Horizontal As Short
		Dim Vertical As Short
	End Structure
	
	Public Structure SensorType
    Dim format_Renamed As Short ' 0 = normal, 1 = extended
		Dim Resolution As ResolutionType
		Dim ROI As ROIType
		Dim Binning As BinningType
		Dim Pixelrate As Integer
		Dim ConvFactor As Short
		Dim DoubleImageMode As Short ' 0 = not, 1 = yes
		Dim bDoubleImage As Boolean
		Dim TempSetpoint As Short
		Dim OffsetMode As Short
		Dim NoiseFilterMode As Short
	End Structure
	
	Public Structure TimeType
		Dim TimeLong As Integer
		Dim TimeBase As Short
		Dim TimeReal As Double
	End Structure
	
	Public Structure TimingType
		Dim Exposure As TimeType
		Dim Delay As TimeType
		Dim TriggerMode As Short
	End Structure
	
	Public Structure StorageType
		Dim Mode As Short ' 0 = recorder / 1 = fifo
		Dim Submode As Short ' 0 = sequence / 1 = ring buffer
		Dim RecState As Short ' 1 = running / 0 = idle
		Dim AcquireMode As Short
	End Structure

    Public Structure PCOCameraType
        Dim Health As HealthType
        Dim Temperature As TemperatureType
        Dim Sensor As SensorType
        Dim Time As TimingType
        Dim Storage As StorageType
    End Structure


    Public PCOCamera As PCOCameraType




    ' my camera subroutines
    Public Sub TranslateHealth()

        Select Case PCOCamera.Health.Warning
            Case Is = 0
                PCOCamera.Health.WarningTxt = "No warnings"
            Case Is = 1
                PCOCamera.Health.WarningTxt = "Power supply voltage range"
            Case Is = 2
                PCOCamera.Health.WarningTxt = "Power supply temperature"
            Case Is = 4
                PCOCamera.Health.WarningTxt = "Camera temperature "
            Case Is = 8
                PCOCamera.Health.WarningTxt = "Image sensor temperature"
        End Select


        Select Case PCOCamera.Health.Error_Renamed
            Case Is = 0
                PCOCamera.Health.ErrorTxt = "No error"
            Case Is = 1
                PCOCamera.Health.ErrorTxt = "Power supply voltage range"
            Case Is = 2
                PCOCamera.Health.ErrorTxt = "Power supply temperature"
            Case Is = 4
                PCOCamera.Health.ErrorTxt = "Camera temperature"
            Case Is = 8
                PCOCamera.Health.ErrorTxt = "Image Sensor temperature"
            Case Is = 10000
                PCOCamera.Health.ErrorTxt = "Camera interface failure"
            Case Is = 20000
                PCOCamera.Health.ErrorTxt = "Camera ram module failure"
            Case Is = 40000
                PCOCamera.Health.ErrorTxt = "Camera main board failure"
            Case Is = 80000
                PCOCamera.Health.ErrorTxt = "Camera head boards failure"
        End Select

        Select Case PCOCamera.Health.Status
            Case Is = 1
                PCOCamera.Health.ErrorTxt = "Default"
            Case Is = 2
                PCOCamera.Health.ErrorTxt = "Settings Valid"
            Case Is = 4
                PCOCamera.Health.ErrorTxt = "Recording state"
        End Select


    End Sub
	
	
	Public Sub TranslateTemps()
        PCOCamera.Temperature.CameraReal = CDbl(PCOCamera.Temperature.cameraindex) / 10
        PCOCamera.Temperature.CCDReal = CDbl(PCOCamera.Temperature.CCDindex) / 10
        PCOCamera.Temperature.PowerReal = CDbl(PCOCamera.Temperature.powerindex) / 10
    End Sub
End Module