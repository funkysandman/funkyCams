
Imports System.Collections.Generic
Imports PylonC.NET
Imports System.Threading

Namespace NETSupportLibrary
    ' The ImageProvider is responsible for opening and closing a device, it takes care of the grabbing and buffer handling,
    '     it notifies the user via events about state changes, and provides access to GenICam parameter nodes of the device.
    '     The grabbing is done in an internal thread. After an image is grabbed the image ready event is fired by the grab
    '     thread. The image can be acquired using GetCurrentImage(). After processing of the image it can be released via ReleaseImage.
    '     The image is then queued for the next grab.  

    Public Class ImageProvider
        ' Simple data class for holding image data. 

        Public Class Image
            Public Sub New(newWidth As Integer, newHeight As Integer, newBuffer As [Byte](), color__1 As Boolean)
                Width = newWidth
                Height = newHeight
                Buffer = newBuffer
                Color = color__1
            End Sub

            Public ReadOnly Width As Integer
            ' The width of the image. 
            Public ReadOnly Height As Integer
            ' The height of the image. 
            Public ReadOnly Buffer As [Byte]()
            ' The raw image data. 
            Public ReadOnly Color As Boolean
            ' If false the buffer contains a Mono8 image. Otherwise, RGBA8packed is provided. 
        End Class

        ' The class GrabResult is used internally to queue grab results. 

        Protected Class GrabResult
            Public ImageData As Image
            ' Holds the taken image. 
            Public Handle As PYLON_STREAMBUFFER_HANDLE
            ' Holds the handle of the image registered at the stream grabber. It is used to queue the buffer associated with itself for the next grab. 
        End Class

        ' The members of ImageProvider: 

        Protected m_converterOutputFormatIsColor As Boolean = False
        ' The output format of the format converter. 
        Protected m_hConverter As PYLON_IMAGE_FORMAT_CONVERTER_HANDLE
        ' The format converter is used mainly for coverting color images. It is not used for Mono8 or RGBA8packed images. 
        Protected m_convertedBuffers As Dictionary(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte]))
        ' Holds handles and buffers used for converted images. It is not used for Mono8 or RGBA8packed images.
        Protected m_hDevice As PYLON_DEVICE_HANDLE
        ' Handle for the pylon device. 
        Protected m_hGrabber As PYLON_STREAMGRABBER_HANDLE
        ' Handle for the pylon stream grabber. 
        Protected m_hRemovalCallback As PYLON_DEVICECALLBACK_HANDLE
        ' Required for deregistering the callback. 
        Protected m_hWait As PYLON_WAITOBJECT_HANDLE
        ' Handle used for waiting for a grab to be finished. 
        Protected m_numberOfBuffersUsed As UInteger = 5
        ' Number of m_buffers used in grab. 
        Protected m_grabThreadRun As Boolean = False
        ' Indicates that the grab thread is active.
        Protected m_open As Boolean = False
        ' Indicates that the device is open and ready to grab.
        Protected m_grabOnce As Boolean = False
        ' Use for single frame mode. 
        Protected m_removed As Boolean = False
        ' Indicates that the device has been removed from the PC. 
        Protected m_grabThread As Thread
        ' Thread for grabbing the images. 
        Protected m_lockObject As [Object]
        ' Lock object used for thread synchronization. 
        Protected m_buffers As Dictionary(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte]))
        ' Holds handles and buffers used for grabbing. 
        Protected m_grabbedBuffers As List(Of GrabResult)
        ' List of grab results already grabbed. 
        Protected m_callbackHandler As DeviceCallbackHandler
        ' Handles callbacks from a device .
        Protected m_lastError As String = ""
        ' Holds the error information belonging to the last exception thrown. 

        ' Creates the last error text from message and detailed text. 

        Private Function GetLastErrorText() As String
            Dim lastErrorMessage As String = GenApi.GetLastErrorMessage()
            Dim lastErrorDetail As String = GenApi.GetLastErrorDetail()

            Dim lastErrorText As String = lastErrorMessage
            If lastErrorDetail.Length > 0 Then
                lastErrorText += vbLf & vbLf & "Details:" & vbLf
            End If
            lastErrorText += lastErrorDetail
            Return lastErrorText
        End Function

        ' Sets the internal last error variable. 

        Private Sub UpdateLastError()
            m_lastError = GetLastErrorText()
        End Sub

        ' Constructor with creation of basic objects. 

        Public Sub New()
            ' Create a thread for image grabbing. 

            m_grabThread = New Thread(AddressOf Grab)
            ' Create objects used for buffer handling. 

            m_lockObject = New [Object]()
            m_buffers = New Dictionary(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte]))()
            m_grabbedBuffers = New List(Of GrabResult)()
            ' Create handles. 

            m_hGrabber = New PYLON_STREAMGRABBER_HANDLE()
            m_hDevice = New PYLON_DEVICE_HANDLE()
            m_hRemovalCallback = New PYLON_DEVICECALLBACK_HANDLE()
            m_hConverter = New PYLON_IMAGE_FORMAT_CONVERTER_HANDLE()
            ' Create callback handler and attach the method. 

            m_callbackHandler = New DeviceCallbackHandler()

            ' m_callbackHandler.CallbackEvent += New DeviceCallbackHandler.DeviceCallback(AddressOf RemovalCallbackHandler)
        End Sub

        ' Indicates that ImageProvider and device are open. 

        Public ReadOnly Property IsOpen() As Boolean
            Get
                Return m_open
            End Get
        End Property

        ' Open using index. Before ImageProvider can be opened using the index, Pylon.EnumerateDevices() needs to be called. 

        Public Sub Open(index As UInteger)
            ' Get a handle for the device and proceed. 

            Open(PylonC.NET.Pylon.CreateDeviceByIndex(index))
        End Sub

        ' Close the device 

        Public Sub Close()
            ' Notify that ImageProvider is about to close the device to give other objects the chance to do clean up operations. 

            OnDeviceClosingEvent()

            ' Try to close everything even if exceptions occur. Keep the last exception to throw when it is done. 

            Dim lastException As Exception = Nothing

            ' Reset the removed flag. 

            m_removed = False

            If m_hGrabber.IsValid Then
                ' Try to close the stream grabber. 

                Try

                    PylonC.NET.Pylon.StreamGrabberClose(m_hGrabber)
                Catch e As Exception
                    lastException = e
                    UpdateLastError()
                End Try
            End If

            If m_hDevice.IsValid Then
                ' Try to deregister the removal callback. 

                Try
                    If m_hRemovalCallback.IsValid Then
                        PylonC.NET.Pylon.DeviceDeregisterRemovalCallback(m_hDevice, m_hRemovalCallback)
                    End If
                Catch e As Exception
                    lastException = e
                    UpdateLastError()
                End Try

                ' Try to close the device. 

                Try
                    ' ... Close and release the pylon device. 

                    If PylonC.NET.Pylon.DeviceIsOpen(m_hDevice) Then
                        PylonC.NET.Pylon.DeviceClose(m_hDevice)
                    End If
                Catch e As Exception
                    lastException = e
                    UpdateLastError()
                End Try

                ' Try to destroy the device. 

                Try
                    PylonC.NET.Pylon.DestroyDevice(m_hDevice)
                Catch e As Exception
                    lastException = e
                    UpdateLastError()
                End Try
            End If

            m_hGrabber.SetInvalid()
            m_hRemovalCallback.SetInvalid()
            m_hDevice.SetInvalid()

            ' Notify that ImageProvider is now closed.

            OnDeviceClosedEvent()

            ' If an exception occurred throw it. 

            If lastException IsNot Nothing Then
                Throw lastException
            End If
        End Sub

        ' Start the grab of one image. 

        Public Sub OneShot()
            If m_open AndAlso Not m_grabThread.IsAlive Then
                ' Only start when open and not grabbing already. 
                ' Set up the grabbing and start. 

                m_numberOfBuffersUsed = 1
                m_grabOnce = True
                m_grabThreadRun = True
                m_grabThread = New Thread(AddressOf Grab)
                m_grabThread.Start()
            End If
        End Sub

        ' Start the grab of images until stopped. 

        Public Sub ContinuousShot()
            If m_open AndAlso Not m_grabThread.IsAlive Then
                ' Only start when open and not grabbing already. 
                ' Set up the grabbing and start. 

                m_numberOfBuffersUsed = 5
                m_grabOnce = False
                m_grabThreadRun = True
                m_grabThread = New Thread(AddressOf Grab)
                m_grabThread.Start()
            End If
        End Sub

        ' Stops the grabbing of images. 

        Public Sub [Stop]()
            If m_open AndAlso m_grabThread.IsAlive Then
                ' Only start when open and grabbing. 
                m_grabThreadRun = False
                ' Causes the grab thread to stop. 
                ' Wait for it to stop. 
                m_grabThread.Join()
            End If
        End Sub

        ' Returns the next available image in the grab result queue. Null is returned if no result is available.
        '           An image is available when the ImageReady event is fired. 

        Public Function GetCurrentImage() As Image
            SyncLock m_lockObject
                ' Lock the grab result queue to avoid that two threads modify the same data. 
                If m_grabbedBuffers.Count > 0 Then
                    ' If images available. 
                    Return m_grabbedBuffers(0).ImageData
                End If
            End SyncLock
            Return Nothing
            ' No image available. 
        End Function

        ' Returns the latest image in the grab result queue. All older images are removed. Null is returned if no result is available.
        '           An image is available when the ImageReady event is fired. 

        Public Function GetLatestImage() As Image
            SyncLock m_lockObject
                ' Lock the grab result queue to avoid that two threads modify the same data. 
                ' Release all images but the latest. 

                While m_grabbedBuffers.Count > 1
                    ReleaseImage()
                End While
                If m_grabbedBuffers.Count > 0 Then
                    ' If images available. 
                    Return m_grabbedBuffers(0).ImageData
                End If
            End SyncLock
            Return Nothing
            ' No image available. 
        End Function

        ' After the ImageReady event has been received and the image was acquired by using GetCurrentImage,
        '        the image must be removed from the grab result queue and added to the stream grabber queue for the next grabs. 

        Public Function ReleaseImage() As Boolean
            SyncLock m_lockObject
                ' Lock the grab result queue to avoid that two threads modify the same data. 
                If m_grabbedBuffers.Count > 0 Then
                    ' If images are available and grabbing is in progress.
                    If m_grabThreadRun Then
                        ' Requeue the buffer. 

                        PylonC.NET.Pylon.StreamGrabberQueueBuffer(m_hGrabber, m_grabbedBuffers(0).Handle, 0)
                    End If
                    ' Remove it from the grab result queue. 

                    m_grabbedBuffers.RemoveAt(0)
                    Return True
                End If
            End SyncLock
            Return False
        End Function

        ' Returns the last error message. Usually called after catching an exception. 

        Public Function GetLastErrorMessage() As String
            If m_lastError.Length = 0 Then
                ' No error set. 
                ' Try to get error information from the GenApi. 
                UpdateLastError()
            End If
            Dim text As String = m_lastError
            m_lastError = ""
            Return text
        End Function

        ' Returns a GenICam parameter node handle of the device identified by the name of the node. 

        Public Function GetNodeFromDevice(name As String) As NODE_HANDLE
            If m_open AndAlso Not m_removed Then
                Dim hNodemap As NODEMAP_HANDLE = PylonC.NET.Pylon.DeviceGetNodeMap(m_hDevice)
                Return GenApi.NodeMapGetNode(hNodemap, name)
            End If
            Return New NODE_HANDLE()
        End Function

        ' Open using device.

        Protected Sub Open(device As PYLON_DEVICE_HANDLE)
            Try
                ' Use provided device. 

                m_hDevice = device

                ' Before using the device, it must be opened. Open it for configuring
                '                parameters and for grabbing images. 

                PylonC.NET.Pylon.DeviceOpen(m_hDevice, PylonC.NET.Pylon.cPylonAccessModeControl Or PylonC.NET.Pylon.cPylonAccessModeStream)

                ' Register the callback function. 

                m_hRemovalCallback = PylonC.NET.Pylon.DeviceRegisterRemovalCallback(m_hDevice, m_callbackHandler)

                ' For GigE cameras, we recommend increasing the packet size for better
                '                   performance. When the network adapter supports jumbo frames, set the packet
                '                   size to a value > 1500, e.g., to 8192. In this sample, we only set the packet size
                '                   to 1500. 

                ' ... Check first to see if the GigE camera packet size parameter is supported and if it is writable. 

                If PylonC.NET.Pylon.DeviceFeatureIsWritable(m_hDevice, "GevSCPSPacketSize") Then
                    ' ... The device supports the packet size feature. Set a value. 

                    PylonC.NET.Pylon.DeviceSetIntegerFeature(m_hDevice, "GevSCPSPacketSize", 1500)
                End If

                ' The sample does not work in chunk mode. It must be disabled. 

                If PylonC.NET.Pylon.DeviceFeatureIsWritable(m_hDevice, "ChunkModeActive") Then
                    ' Disable the chunk mode. 

                    PylonC.NET.Pylon.DeviceSetBooleanFeature(m_hDevice, "ChunkModeActive", False)
                End If

                ' Disable acquisition start trigger if available. 

                If PylonC.NET.Pylon.DeviceFeatureIsAvailable(m_hDevice, "EnumEntry_TriggerSelector_AcquisitionStart") Then
                    PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "TriggerSelector", "AcquisitionStart")
                    PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "TriggerMode", "Off")
                End If

                ' Disable frame burst start trigger if available 

                If PylonC.NET.Pylon.DeviceFeatureIsAvailable(m_hDevice, "EnumEntry_TriggerSelector_FrameBurstStart") Then
                    PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "TriggerSelector", "FrameBurstStart")
                    PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "TriggerMode", "Off")
                End If

                ' Disable frame start trigger if available. 

                If PylonC.NET.Pylon.DeviceFeatureIsAvailable(m_hDevice, "EnumEntry_TriggerSelector_FrameStart") Then
                    PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "TriggerSelector", "FrameStart")
                    PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "TriggerMode", "Off")
                End If

                ' Image grabbing is done using a stream grabber.
                '                  A device may be able to provide different streams. A separate stream grabber must
                '                  be used for each stream. In this sample, we create a stream grabber for the default
                '                  stream, i.e., the first stream ( index == 0 ).
                '                  


                ' Get the number of streams supported by the device and the transport layer. 

                If PylonC.NET.Pylon.DeviceGetNumStreamGrabberChannels(m_hDevice) < 1 Then
                    Throw New Exception("The transport layer doesn't support image streams.")
                End If

                ' Create and open a stream grabber for the first channel. 

                m_hGrabber = PylonC.NET.Pylon.DeviceGetStreamGrabber(m_hDevice, 0)
                PylonC.NET.Pylon.StreamGrabberOpen(m_hGrabber)

                ' Get a handle for the stream grabber's wait object. The wait object
                '                   allows waiting for m_buffers to be filled with grabbed data. 

                m_hWait = PylonC.NET.Pylon.StreamGrabberGetWaitObject(m_hGrabber)
            Catch
                ' Get the last error message here, because it could be overwritten by cleaning up. 

                UpdateLastError()

                Try
                    ' Try to close any open handles. 
                    Close()
                    ' Another exception cannot be handled. 

                Catch
                End Try
                Throw
            End Try

            ' Notify that the ImageProvider is open and ready for grabbing and configuration. 

            OnDeviceOpenedEvent()
        End Sub

        ' Prepares everything for grabbing. 

        Protected Sub SetupGrab()
            ' Clear the grab result queue. This is not done when cleaning up to still be able to provide the
            '             images, e.g. in single frame mode.

            SyncLock m_lockObject
                ' Lock the grab result queue to avoid that two threads modify the same data. 
                m_grabbedBuffers.Clear()
            End SyncLock

            ' Set the acquisition mode 

            If m_grabOnce Then
                ' We will use the single frame mode, to take one image. 

                PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "AcquisitionMode", "SingleFrame")
            Else
                ' We will use the Continuous frame mode, i.e., the camera delivers
                '                images continuously. 

                PylonC.NET.Pylon.DeviceFeatureFromString(m_hDevice, "AcquisitionMode", "Continuous")
            End If

            ' Clear the grab buffers to assure proper operation (because they may
            '             still be filled if the last grab has thrown an exception). 

            For Each pair As KeyValuePair(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte])) In m_buffers
                pair.Value.Dispose()
            Next
            m_buffers.Clear()

            ' Determine the required size of the grab buffer. 

            Dim payloadSize As UInteger = CUInt(PylonC.NET.Pylon.DeviceGetIntegerFeature(m_hDevice, "PayloadSize"))

            ' We must tell the stream grabber the number and size of the m_buffers
            '                we are using. 

            ' .. We will not use more than NUM_m_buffers for grabbing. 

            PylonC.NET.Pylon.StreamGrabberSetMaxNumBuffer(m_hGrabber, m_numberOfBuffersUsed)

            ' .. We will not use m_buffers bigger than payloadSize bytes. 

            PylonC.NET.Pylon.StreamGrabberSetMaxBufferSize(m_hGrabber, payloadSize)

            '  Allocate the resources required for grabbing. After this, critical parameters
            '                that impact the payload size must not be changed until FinishGrab() is called. 

            PylonC.NET.Pylon.StreamGrabberPrepareGrab(m_hGrabber)

            ' Before using the m_buffers for grabbing, they must be registered at
            '               the stream grabber. For each buffer registered, a buffer handle
            '               is returned. After registering, these handles are used instead of the
            '               buffer objects pointers. The buffer objects are held in a dictionary,
            '               that provides access to the buffer using a handle as key.
            '             

            For i As UInteger = 0 To m_numberOfBuffersUsed - 1
                Dim buffer As PylonBuffer(Of [Byte]) = New PylonBuffer(Of Byte)(payloadSize, True)
                Dim handle As PYLON_STREAMBUFFER_HANDLE = PylonC.NET.Pylon.StreamGrabberRegisterBuffer(m_hGrabber, buffer)
                m_buffers.Add(handle, buffer)
            Next

            ' Feed the m_buffers into the stream grabber's input queue. For each buffer, the API
            '               allows passing in an integer as additional context information. This integer
            '               will be returned unchanged when the grab is finished. In our example, we use the index of the
            '               buffer as context information. 

            For Each pair As KeyValuePair(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte])) In m_buffers
                PylonC.NET.Pylon.StreamGrabberQueueBuffer(m_hGrabber, pair.Key, 0)
            Next

            ' The stream grabber is now prepared. As soon the camera starts acquiring images,
            '               the image data will be grabbed into the provided m_buffers.  


            ' Set the handle of the image converter invalid to assure proper operation (because it may
            '             still be valid if the last grab has thrown an exception). 

            m_hConverter.SetInvalid()

            ' Let the camera acquire images. 

            PylonC.NET.Pylon.DeviceExecuteCommandFeature(m_hDevice, "AcquisitionStart")
        End Sub

        ' This method is executed using the grab thread and is responsible for grabbing, possible conversion of the image
        '        ,and queuing the image to the result queue. 

        Protected Sub Grab()
            ' Notify that grabbing has started. This event can be used to update the state of the GUI. 

            OnGrabbingStartedEvent()
            Try
                ' Set up everything needed for grabbing. 

                SetupGrab()

                While m_grabThreadRun
                    ' Is set to false when stopping to end the grab thread. 
                    ' Wait for the next buffer to be filled. Wait up to 15000 ms. 

                    If Not PylonC.NET.Pylon.WaitObjectWait(m_hWait, 15000) Then
                        SyncLock m_lockObject
                            If m_grabbedBuffers.Count <> m_numberOfBuffersUsed Then
                                ' A timeout occurred. This can happen if an external trigger is used or
                                '                                   if the programmed exposure time is longer than the grab timeout. 

                                Throw New Exception("A grab timeout occurred.")
                            End If
                            Continue While
                        End SyncLock
                    End If

                    Dim grabResult As PylonGrabResult_t
                    ' Stores the result of a grab operation. 
                    ' Since the wait operation was successful, the result of at least one grab
                    '                       operation is available. Retrieve it. 

                    If Not PylonC.NET.Pylon.StreamGrabberRetrieveResult(m_hGrabber, grabResult) Then
                        ' Oops. No grab result available? We should never have reached this point.
                        '                           Since the wait operation above returned without a timeout, a grab result
                        '                           should be available. 

                        Throw New Exception("Failed to retrieve a grab result.")
                    End If

                    ' Check to see if the image was grabbed successfully. 

                    If grabResult.Status = EPylonGrabStatus.Grabbed Then
                        ' Add result to the ready list. 

                        EnqueueTakenImage(grabResult)

                        ' Notify that an image has been added to the output queue. The receiver of the event can use GetCurrentImage() to acquire and process the image
                        '                         and ReleaseImage() to remove the image from the queue and return it to the stream grabber.

                        OnImageReadyEvent()

                        ' Exit here for single frame mode. 

                        If m_grabOnce Then
                            m_grabThreadRun = False
                            Exit While
                        End If
                    ElseIf grabResult.Status = EPylonGrabStatus.Failed Then
                        '
                        '                            Grabbing an image can fail if the used network hardware, i.e. network adapter,
                        '                            switch or Ethernet cable, experiences performance problems.
                        '                            Increase the Inter-Packet Delay to reduce the required bandwidth.
                        '                            It is recommended to enable Jumbo Frames on the network adapter and switch.
                        '                            Adjust the Packet Size on the camera to the highest supported frame size.
                        '                            If this did not resolve the problem, check if the recommended hardware is used.
                        '                            Aggressive power saving settings for the CPU can also cause the image grab to fail.
                        '                        

                        Throw New Exception(String.Format("A grab failure occurred. See the method ImageProvider::Grab for more information. The error code is {0:X08}.", grabResult.ErrorCode))
                    End If
                End While

                ' Tear down everything needed for grabbing. 

                CleanUpGrab()
            Catch e As Exception
                ' The grabbing stops due to an error. Set m_grabThreadRun to false to avoid that any more buffers are queued for grabbing. 

                m_grabThreadRun = False

                ' Get the last error message here, because it could be overwritten by cleaning up. 

                Dim lastErrorMessage As String = GetLastErrorText()

                Try
                    ' Try to tear down everything needed for grabbing. 

                    CleanUpGrab()
                    ' Another exception cannot be handled. 

                Catch
                End Try

                ' Notify that grabbing has stopped. This event could be used to update the state of the GUI. 

                OnGrabbingStoppedEvent()

                If Not m_removed Then
                    ' In case the device was removed from the PC suppress the notification. 
                    ' Notify that the grabbing had errors and deliver the information. 

                    OnGrabErrorEvent(e, lastErrorMessage)
                End If
                Return
            End Try
            ' Notify that grabbing has stopped. This event could be used to update the state of the GUI. 

            OnGrabbingStoppedEvent()
        End Sub

        Protected Sub EnqueueTakenImage(grabResult As PylonGrabResult_t)
            Dim buffer As PylonBuffer(Of [Byte])
            ' Reference to the buffer attached to the grab result. 

            ' Get the buffer from the dictionary. 

            If Not m_buffers.TryGetValue(grabResult.hBuffer, buffer) Then
                ' Oops. No buffer available? We should never have reached this point. Since all buffers are
                '                   in the dictionary. 

                Throw New Exception("Failed to find the buffer associated with the handle returned in grab result.")
            End If

            ' Create a new grab result to enqueue to the grabbed buffers list. 

            Dim newGrabResultInternal As New GrabResult()
            newGrabResultInternal.Handle = grabResult.hBuffer
            ' Add the handle to requeue the buffer in the stream grabber queue. 

            ' If already in output format add the image data. 

            If grabResult.PixelType = EPylonPixelType.PixelType_Mono8 OrElse grabResult.PixelType = EPylonPixelType.PixelType_RGBA8packed Then
                newGrabResultInternal.ImageData = New Image(grabResult.SizeX, grabResult.SizeY, buffer.Array, grabResult.PixelType = EPylonPixelType.PixelType_RGBA8packed)
            Else
                ' Conversion is required. 
                ' Create a new format converter if needed. 

                If Not m_hConverter.IsValid Then
                    m_convertedBuffers = New Dictionary(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of Byte))()
                    ' Create a new dictionary for the converted buffers. 
                    m_hConverter = PylonC.NET.Pylon.ImageFormatConverterCreate()
                    ' Create the converter. 
                    m_converterOutputFormatIsColor = Not PylonC.NET.Pylon.IsMono(grabResult.PixelType) OrElse PylonC.NET.Pylon.IsBayer(grabResult.PixelType)
                End If
                ' Reference to the buffer attached to the grab result handle. 

                Dim convertedBuffer As PylonBuffer(Of [Byte]) = Nothing
                ' Look up if a buffer is already attached to the handle. 

                Dim bufferListed As Boolean = m_convertedBuffers.TryGetValue(grabResult.hBuffer, convertedBuffer)
                ' Perform the conversion. If the buffer is null a new one is automatically created. 

                PylonC.NET.Pylon.ImageFormatConverterSetOutputPixelFormat(m_hConverter, If(m_converterOutputFormatIsColor, EPylonPixelType.PixelType_BGRA8packed, EPylonPixelType.PixelType_Mono8))
                PylonC.NET.Pylon.ImageFormatConverterConvert(m_hConverter, convertedBuffer, buffer, grabResult.PixelType, CUInt(grabResult.SizeX), CUInt(grabResult.SizeY), _
                    CUInt(grabResult.PaddingX), EPylonImageOrientation.ImageOrientation_TopDown)
                If Not bufferListed Then
                    ' A new buffer has been created. Add it to the dictionary. 
                    m_convertedBuffers.Add(grabResult.hBuffer, convertedBuffer)
                End If
                ' Add the image data. 

                newGrabResultInternal.ImageData = New Image(grabResult.SizeX, grabResult.SizeY, convertedBuffer.Array, m_converterOutputFormatIsColor)
            End If
            SyncLock m_lockObject
                ' Lock the grab result queue to avoid that two threads modify the same data. 
                ' Add the new grab result to the queue. 
                m_grabbedBuffers.Add(newGrabResultInternal)
            End SyncLock
        End Sub

        Protected Sub CleanUpGrab()
            '  ... Stop the camera. 

            PylonC.NET.Pylon.DeviceExecuteCommandFeature(m_hDevice, "AcquisitionStop")

            ' Destroy the format converter if one was used. 

            If m_hConverter.IsValid Then
                ' Destroy the converter. 

                PylonC.NET.Pylon.ImageFormatConverterDestroy(m_hConverter)
                ' Set the handle invalid. The next grab cycle may not need a converter. 

                m_hConverter.SetInvalid()
                ' Release the converted image buffers. 

                For Each pair As KeyValuePair(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte])) In m_convertedBuffers
                    pair.Value.Dispose()
                Next
                m_convertedBuffers = Nothing
            End If

            ' ... We must issue a cancel call to ensure that all pending m_buffers are put into the
            '               stream grabber's output queue. 

            PylonC.NET.Pylon.StreamGrabberCancelGrab(m_hGrabber)

            ' ... The m_buffers can now be retrieved from the stream grabber. 

            If True Then
                Dim isReady As Boolean
                ' Used as an output parameter. 
                Do
                    Dim grabResult As PylonGrabResult_t
                    ' Stores the result of a grab operation. 

                    isReady = PylonC.NET.Pylon.StreamGrabberRetrieveResult(m_hGrabber, grabResult)
                Loop While isReady
            End If

            ' ... When all m_buffers are retrieved from the stream grabber, they can be deregistered.
            '                   After deregistering the m_buffers, it is safe to free the memory. 


            For Each pair As KeyValuePair(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte])) In m_buffers
                PylonC.NET.Pylon.StreamGrabberDeregisterBuffer(m_hGrabber, pair.Key)
            Next

            ' The buffers can now be released. 

            For Each pair As KeyValuePair(Of PYLON_STREAMBUFFER_HANDLE, PylonBuffer(Of [Byte])) In m_buffers
                pair.Value.Dispose()
            Next
            m_buffers.Clear()

            ' ... Release grabbing related resources. 

            PylonC.NET.Pylon.StreamGrabberFinishGrab(m_hGrabber)

            ' After calling PylonStreamGrabberFinishGrab(), parameters that impact the payload size (e.g.,
            '            the AOI width and height parameters) are unlocked and can be modified again. 

        End Sub

        ' This callback is called by the pylon layer using DeviceCallbackHandler. 

        Protected Sub RemovalCallbackHandler(hDevice As PYLON_DEVICE_HANDLE)
            ' Notify that the device has been removed from the PC. 

            OnDeviceRemovedEvent()
        End Sub

        ' The events fired by ImageProvider. See the invocation methods below for further information, e.g. OnGrabErrorEvent. 

        Public Delegate Sub DeviceOpenedEventHandler()
        Public Event DeviceOpenedEvent As DeviceOpenedEventHandler

        Public Delegate Sub DeviceClosingEventHandler()
        Public Event DeviceClosingEvent As DeviceClosingEventHandler

        Public Delegate Sub DeviceClosedEventHandler()
        Public Event DeviceClosedEvent As DeviceClosedEventHandler

        Public Delegate Sub GrabbingStartedEventHandler()
        Public Event GrabbingStartedEvent As GrabbingStartedEventHandler

        Public Delegate Sub ImageReadyEventHandler()
        Public Event ImageReadyEvent As ImageReadyEventHandler

        Public Delegate Sub GrabbingStoppedEventHandler()
        Public Event GrabbingStoppedEvent As GrabbingStoppedEventHandler

        Public Delegate Sub GrabErrorEventHandler(grabException As Exception, additionalErrorMessage As String)
        Public Event GrabErrorEvent As GrabErrorEventHandler

        Public Delegate Sub DeviceRemovedEventHandler()
        Public Event DeviceRemovedEvent As DeviceRemovedEventHandler

        ' Notify that ImageProvider is open and ready for grabbing and configuration. 

        Protected Sub OnDeviceOpenedEvent()
            m_open = True
            RaiseEvent DeviceOpenedEvent()
        End Sub

        ' Notify that ImageProvider is about to close the device to give other objects the chance to do clean up operations. 

        Protected Sub OnDeviceClosingEvent()
            m_open = False
            RaiseEvent DeviceClosingEvent()
        End Sub

        ' Notify that ImageProvider is now closed.

        Protected Sub OnDeviceClosedEvent()
            m_open = False
            RaiseEvent DeviceClosedEvent()
        End Sub

        ' Notify that grabbing has started. This event could be used to update the state of the GUI. 

        Protected Sub OnGrabbingStartedEvent()
            RaiseEvent GrabbingStartedEvent()
        End Sub

        ' Notify that an image has been added to the output queue. The receiver of the event can use GetCurrentImage() to acquire and process the image
        '         and ReleaseImage() to remove the image from the queue and return it to the stream grabber.

        Protected Sub OnImageReadyEvent()
            RaiseEvent ImageReadyEvent()
        End Sub

        ' Notify that grabbing has stopped. This event could be used to update the state of the GUI. 

        Protected Sub OnGrabbingStoppedEvent()
            RaiseEvent GrabbingStoppedEvent()
        End Sub

        ' Notify that the grabbing had errors and deliver the information. 

        Protected Sub OnGrabErrorEvent(grabException As Exception, additionalErrorMessage As String)
            RaiseEvent GrabErrorEvent(grabException, additionalErrorMessage)
        End Sub

        ' Notify that the device has been removed from the PC. 

        Protected Sub OnDeviceRemovedEvent()
            m_removed = True
            m_grabThreadRun = False
            RaiseEvent DeviceRemovedEvent()
        End Sub
    End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
