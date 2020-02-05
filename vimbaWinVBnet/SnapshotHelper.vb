
Imports System.IO
Imports PixeLINK


Public Class SnapshotHelper

    Private m_hCamera As Integer

    '// Constructor
    Public Sub New(ByVal hCamera As Integer)

        m_hCamera = hCamera

    End Sub

    '// Query the camera for the current pixel format
    Private Function GetPixelFormat() As PixelFormat

        Dim flags As FeatureFlags
        Dim numParams As Integer
        numParams = 1
        Dim params(numParams) As Single

        Api.GetFeature(m_hCamera, Feature.PixelFormat, flags, numParams, params)

        GetPixelFormat = CType(params(0), PixelFormat)

    End Function

    '// 
    '// Determine the number of pixels in a frame
    '// 
    '// # pixels = height * width
    '// When we query the ROI feature, the camera reports the undecimated ROI
    '// Therefore we have to take the camera's current decimation (pixel addressing value)
    '// mode into account
    Private Function GetNumPixels() As Integer

        Dim flags As FeatureFlags
        Dim numParams As Integer = 4
        Dim params(numParams) As Single

        Api.GetFeature(m_hCamera, Feature.ROI, flags, numParams, params)

        Dim width As Integer
        Dim height As Integer

        width = System.Convert.ToInt32(params(FeatureParameterIndex.RoiWidth))
        height = System.Convert.ToInt32(params(FeatureParameterIndex.RoiHeight))

        '// Take the decimation (pixel addressing value) into account
        numParams = 2
        Api.GetFeature(m_hCamera, Feature.PixelAddressing, flags, numParams, params)
        Dim pixelAddressingValue As Integer = System.Convert.ToInt32(params(FeatureParameterIndex.PixelAddressingValue))

        width = System.Convert.ToInt32(width / pixelAddressingValue)
        height = System.Convert.ToInt32(height / pixelAddressingValue)
        Return width * height


    End Function

    Private Function DetermineRawImageSize() As Integer

        Dim bytesPerPixel As Single
        bytesPerPixel = Api.BytesPerPixel(GetPixelFormat())
        Return System.Convert.ToInt32(bytesPerPixel * System.Convert.ToSingle(GetNumPixels()))

    End Function

    Public Function GetSnapshot(ByVal format As ImageFormat, ByVal filename As String) As Boolean

        Dim rc As ReturnCode

        Dim rawImageSize As Integer
        rawImageSize = DetermineRawImageSize()
        Dim buf(rawImageSize - 1) As Byte

        rc = Api.SetStreamState(m_hCamera, StreamState.Start)
        If (Not Api.IsSuccess(rc)) Then
            Return False
        End If

        Dim frameDesc As New FrameDescriptor

        rc = GetNextFrame(frameDesc, buf)

        Api.SetStreamState(m_hCamera, StreamState.Stop)

        If (Not Api.IsSuccess(rc)) Then
            Return False
        End If

        '// Now convert to the desired image format (BMP, JPEG, etc)
        Dim destBufferSize As Integer
        rc = Api.FormatImage(buf, frameDesc, format, Nothing, destBufferSize)
        Dim dstBuf(System.Convert.ToInt32(destBufferSize) - 1) As Byte
        rc = Api.FormatImage(buf, frameDesc, format, dstBuf, destBufferSize)

        '// Save the converted image to a binary file
        Dim fStream As New FileStream(filename, FileMode.OpenOrCreate)
        Dim bw As New BinaryWriter(fStream)
        bw.Write(dstBuf)
        bw.Close()
        fStream.Close()

        GetSnapshot = True

    End Function

    '//
    '// It's possible for some calls to Api.GetNextFrame to 
    '// report CameraTimeoutError, but the next call will be successful. 
    '// 
    Private Function GetNextFrame(ByRef frameDesc As FrameDescriptor, ByRef buf As Byte()) As ReturnCode

        Dim rc As ReturnCode = ReturnCode.UnknownError
        Dim MAX_NUM_TRIES As Integer = 4
        Dim i As Integer
        For i = 0 To MAX_NUM_TRIES
            rc = Api.GetNextFrame(m_hCamera, buf.GetLength(0), buf, frameDesc)
            If Api.IsSuccess(rc) Then
                Exit For
            End If
        Next

        Return rc

    End Function


End Class
