using Basler.Pylon;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

namespace BaslerWrapper
{
    public delegate void FrameReceivedHandler(object sender, FrameEventArgs args);

    public class FrameEventArgs : EventArgs
    {
        public ushort[] Data { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Exception Exception { get; private set; }

        public FrameEventArgs(ushort[] data, int width, int height)
        {
            if (data == null) throw new ArgumentNullException("data");

            Data = data;
            Width = width;
            Height = height;
        }

        public FrameEventArgs(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            Exception = ex;
        }
    }

    public class Grabber : IDisposable
    {
        private Camera _camera;
        private volatile bool _running;
        private Thread _grabThread;
        private readonly object _stateLock = new object();

        private FrameReceivedHandler _handler;
        private PixelDataConverter _converter = new PixelDataConverter();

        public long SizeX { get; private set; }
        public long SizeY { get; private set; }

        public bool isRunning
        {
            get
            {
                lock (_stateLock)
                {
                    return _running;
                }
            }
        }

        public void Open(int index)
        {
            var devices = CameraFinder.Enumerate();

            if (devices == null || devices.Count == 0)
                throw new Exception("No cameras found");

            if (index < 0 || index >= devices.Count)
                throw new ArgumentOutOfRangeException("index");

            _camera = new Camera(devices[index]);
            _camera.Open();

            _camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.Off);
            _camera.Parameters[PLCamera.PixelFormat].SetValue(PLCamera.PixelFormat.Mono16);

            SizeX = _camera.Parameters[PLCamera.Width].GetValue();
            SizeY = _camera.Parameters[PLCamera.Height].GetValue();
        }
        ~Grabber()
        {
            try
            {
                Close();
            }
            catch { }
        }
        public void SetGain(long gain)
        {
            EnsureOpen();

            if (_camera.Parameters.Contains(PLCamera.GainRaw))
                _camera.Parameters[PLCamera.GainRaw].SetValue(gain);
        }

        public void SetExposure(long microseconds)
        {
            EnsureOpen();
            if (_camera.Parameters.Contains(PLCamera.ExposureTimeBaseAbs))
               // _camera.Parameters[PLCamera.ExposureTimeBaseAbs].SetValue(1);//2441
                _camera.Parameters[PLCamera.ExposureTimeBaseAbs].SetValue(2441);//2441
            if (_camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                _camera.Parameters[PLCamera.ExposureTimeAbs].SetValue((double)microseconds);
        }

        public void SetParams(int durationMs, long gain)
        {
            SetGain(gain);
            SetExposure(durationMs*1000);
            //SetExposure(durationMs);
        }

        public void StartAcquisition(FrameReceivedHandler handler)
        {
            EnsureOpen();

            lock (_stateLock)
            {
                if (_grabThread != null && _grabThread.IsAlive)
                    throw new InvalidOperationException("Acquisition is already running");

                _handler = handler;
                _running = true;

                _camera.StreamGrabber.Start();

                _grabThread = new Thread(GrabLoop)
                {
                    IsBackground = true,
                    Name = "BaslerGrabLoop"
                };
                _grabThread.Start();
            }
        }

        public void StopAcquisition()
        {
            Thread threadToJoin = null;

            lock (_stateLock)
            {
                _running = false;

                if (_camera != null && _camera.IsOpen)
                {
                    try
                    {
                        _camera.StreamGrabber.Stop();
                    }
                    catch
                    {
                    }
                }

                threadToJoin = _grabThread;
                _grabThread = null;
            }

            if (threadToJoin != null &&
                threadToJoin.IsAlive &&
                Thread.CurrentThread != threadToJoin)
            {
                threadToJoin.Join(6000);
            }
        }

        private ushort[] ConvertToMono16(IGrabResult result)
        {
            int pixelCount = result.Width * result.Height;
            ushort[] buffer = new ushort[pixelCount];

            var converter = new PixelDataConverter();
            converter.OutputPixelFormat = PixelType.Mono16;

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                converter.Convert(handle.AddrOfPinnedObject(), buffer.Length * 2, result);
            }
            finally
            {
                handle.Free();
            }

            return buffer;
        }


        private void GrabLoop()
        {
            try
            {
                while (_running)
                {
                    using (IGrabResult result =
                        _camera.StreamGrabber.RetrieveResult(10000, TimeoutHandling.ThrowException))
                    {
                        if (result == null)
                            continue;

                        if (!result.GrabSucceeded)
                            continue;

                        ushort[] data = ConvertToMono16(result);

                        _handler?.Invoke(this, new FrameEventArgs(data, result.Width, result.Height));
                    }
                }
            }
            catch (Exception ex)
            {
                if (_handler != null)
                    _handler(this, new FrameEventArgs(ex));
            }
        }

        private Bitmap ConvertToBitmap(IGrabResult result)
        {
            Bitmap bitmap = new Bitmap(result.Width, result.Height, PixelFormat.Format16bppGrayScale);

            BitmapData data = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);

            _converter.OutputPixelFormat = PixelType.Mono16;

            _converter.Convert(data.Scan0, result.PayloadSize, result);

            bitmap.UnlockBits(data);

            return bitmap;
        }

        private void EnsureOpen()
        {
            if (_camera == null || !_camera.IsOpen)
                throw new InvalidOperationException("Camera not open");
        }

        public void Close()
        {
            StopAcquisition();

            if (_camera != null)
            {
                _camera.Close();
                _camera.Dispose();
                _camera = null;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}