// TODO fill in this information for your driver, then remove this line!
//
// ASCOM Camera hardware class for ATIKVS
//
// Description:	 <To be completed by driver developer>
//
// Implements:	ASCOM Camera interface version: <To be completed by driver developer>
// Author:		(XXX) Your N. Here <your@email.here>

// TODO: Customise the SetConnected and InitialiseHardware methods as needed for your hardware

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Astrometry.NOVAS;
using ASCOM.DeviceInterface;
using ASCOM.Utilities;
using ATIKCAM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
namespace ASCOM.ATIKVS.Camera
{
    //
    // TODO Customise the InitialiseHardware() method with code to set up a communication path to your hardware and validate that the hardware exists
    //
    // TODO Customise the SetConnected() method with code to connect to and disconnect from your hardware
    // NOTE You should not need to customise the code in the Connecting, Connect() and Disconnect() members as these are already fully implemented and call SetConnected() when appropriate.
    //
    // TODO Replace the not implemented exceptions with code to implement the functions or throw the appropriate ASCOM exceptions.
    //

    /// <summary>
    /// ASCOM Camera hardware class for ATIKVS.
    /// </summary>
    [HardwareClass()] // Class attribute flag this as a device hardware class that needs to be disposed by the local server when it exits.
    internal static class CameraHardware
    {
        // Constants used for Profile persistence
        internal const string comPortProfileName = "COM Port";
        internal const string comPortDefault = "COM1";
        internal const string offsetProfileName = "Offset";
        internal const string offsetDefault = "202";
        internal const string traceStateProfileName = "Trace Level";
        internal const string traceStateDefault = "true";

        private static string DriverProgId = ""; // ASCOM DeviceID (COM ProgID) for this driver, the value is set by the driver's class initialiser.
        private static string DriverDescription = ""; // The value is set by the driver's class initialiser.
        internal static string comPort; // COM port name (if required)
        internal static int offset;//sensor offset
        private static bool connectedState; // Local server's connected state
        private static bool runOnce = false; // Flag to enable "one-off" activities only to run once.
        internal static Util utilities; // ASCOM Utilities object for use as required
        internal static AstroUtils astroUtilities; // ASCOM AstroUtilities object for use as required
        internal static TraceLogger tl; // Local server's trace logger object for diagnostic log with information that you specify

        private static List<Guid> uniqueIds = new List<Guid>(); // List of driver instance unique IDs

        /// <summary>
        /// Initializes a new instance of the device Hardware class.
        /// </summary>
        static CameraHardware()
        {
            try
            {
                // Create the hardware trace logger in the static initialiser.
                // All other initialisation should go in the InitialiseHardware method.
                tl = new TraceLogger("", "ATIKVS.Hardware");

                // DriverProgId has to be set here because it used by ReadProfile to get the TraceState flag.
                DriverProgId = Camera.DriverProgId; // Get this device's ProgID so that it can be used to read the Profile configuration values

                // ReadProfile has to go here before anything is written to the log because it loads the TraceLogger enable / disable state.
                ReadProfile(); // Read device configuration from the ASCOM Profile store, including the trace state
                //

                AtikPInvoke.ArtemisRefreshDevicesCount();
                Thread.Sleep(1000); // The sleep is here to account for our CMOS cameras
                int cam_index = 0;
                for (var i = 0; i < 16; ++i)
                {
                    if (AtikPInvoke.ArtemisDevicePresent(i))
                    {
                        StringBuilder sbName = new StringBuilder();
                        StringBuilder sbSerial = new StringBuilder();
                        AtikPInvoke.ArtemisDeviceName(i, sbName);
                        AtikPInvoke.ArtemisDeviceSerial(i, sbSerial);
                        cam_index = i;
                        //ComboBoxCameras.Add(new ComboBoxCamera(i, $"{sbName} {sbSerial}"));

                    }
                }
                // assume camera is at index 0 for now
                handle = AtikPInvoke.ArtemisConnect(cam_index);
                if (handle.ToInt32() != 0)
                {

                    // Using 0 for the sensor checks the number of sensors
                    // rather than checking for a temperature
                    int success = 0;
                    int num = 0;
                    success = AtikPInvoke.ArtemisTemperatureSensorInfo(handle, 0, ref num);
                    //int x=0, y=0, width=0, height=0,binx=0, biny=0;
                    //success = AtikPInvoke.ArtemisGetImageData(handle, ref x, ref y, ref width,ref height,ref binx, ref biny);
                    success = AtikPInvoke.ArtemisProperties(handle, ref cameraProperties);
                    NumX = cameraProperties.NPixelsX;
                    NumY = cameraProperties.NPixelsY;
                    SetOffset(offset);



                    //if (num != 0)
                    //{
                    //    StartCoolingButton.Enabled = true;
                    //    checkTemperature = true;
                    //}
                    //else
                    //{
                    //    Temperature.Text = "No Cooling";
                    //}

                }

                //CameraComboBox.Items.Clear();
                //foreach (var camera in ComboBoxCameras)
                //    CameraComboBox.Items.Add(camera);


                //
                LogMessage("CameraHardware", $"Static initialiser completed.");
            }
            catch (Exception ex)
            {
                try { LogMessage("CameraHardware", $"Initialisation exception: {ex}"); } catch { }
                MessageBox.Show($"CameraHardware - {ex.Message}\r\n{ex}", $"Exception creating {Camera.DriverProgId}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        /// <summary>
        /// Place device initialisation code here
        /// </summary>
        /// <remarks>Called every time a new instance of the driver is created.</remarks>
        internal static void InitialiseHardware()
        {
            // This method will be called every time a new ASCOM client loads your driver
            LogMessage("InitialiseHardware", $"Start.");

            // Add any code that you want to run every time a client connects to your driver here

            // Add any code that you only want to run when the first client connects in the if (runOnce == false) block below
            if (runOnce == false)
            {
                LogMessage("InitialiseHardware", $"Starting one-off initialisation.");

                DriverDescription = Camera.DriverDescription; // Get this device's Chooser description

                LogMessage("InitialiseHardware", $"ProgID: {DriverProgId}, Description: {DriverDescription}");

                connectedState = false; // Initialise connected to false
                utilities = new Util(); //Initialise ASCOM Utilities object
                astroUtilities = new AstroUtils(); // Initialise ASCOM Astronomy Utilities object

                LogMessage("InitialiseHardware", "Completed basic initialisation");

                // Add your own "one off" device initialisation here e.g. validating existence of hardware and setting up communications
                // If you are using a serial COM port you will find the COM port name selected by the user through the setup dialogue in the comPort variable.

                LogMessage("InitialiseHardware", $"One-off initialisation complete.");
                runOnce = true; // Set the flag to ensure that this code is not run again
            }
        }

        // PUBLIC COM INTERFACE ICameraV4 IMPLEMENTATION

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialogue form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public static void SetupDialog()
        {
            // Don't permit the setup dialogue if already connected
            if (IsConnected)
            {
                MessageBox.Show("Already connected, just press OK");
                return; // Exit the method if already connected
            }

            using (SetupDialogForm F = new SetupDialogForm(tl))
            {
                var result = F.ShowDialog();
                if (result == DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        /// <summary>Returns the list of custom action names supported by this driver.</summary>
        /// <value>An ArrayList of strings (SafeArray collection) containing the names of supported actions.</value>
        public static ArrayList SupportedActions
        {
            get
            {
                LogMessage("SupportedActions Get", "Returning empty ArrayList");
                return new ArrayList();
            }
        }

        /// <summary>Invokes the specified device-specific custom action.</summary>
        /// <param name="ActionName">A well known name agreed by interested parties that represents the action to be carried out.</param>
        /// <param name="ActionParameters">List of required parameters or an <see cref="String.Empty">Empty String</see> if none are required.</param>
        /// <returns>A string response. The meaning of returned strings is set by the driver author.
        /// <para>Suppose filter wheels start to appear with automatic wheel changers; new actions could be <c>QueryWheels</c> and <c>SelectWheel</c>. The former returning a formatted list
        /// of wheel names and the second taking a wheel name and making the change, returning appropriate values to indicate success or failure.</para>
        /// </returns>
        public static string Action(string actionName, string actionParameters)
        {
            LogMessage("Action", $"Action {actionName}, parameters {actionParameters} is not implemented");
            throw new ActionNotImplementedException("Action " + actionName + " is not implemented by this driver");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and does not wait for a response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        public static void CommandBlind(string command, bool raw)
        {
            CheckConnected("CommandBlind");
            // TODO The optional CommandBlind method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBlind must send the supplied command to the mount and return immediately without waiting for a response

            throw new MethodNotImplementedException($"CommandBlind - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a boolean response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the interpreted boolean response received from the device.
        /// </returns>
        public static bool CommandBool(string command, bool raw)
        {
            CheckConnected("CommandBool");
            // TODO The optional CommandBool method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandBool must send the supplied command to the mount, wait for a response and parse this to return a True or False value

            throw new MethodNotImplementedException($"CommandBool - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Transmits an arbitrary string to the device and waits for a string response.
        /// Optionally, protocol framing characters may be added to the string before transmission.
        /// </summary>
        /// <param name="Command">The literal command string to be transmitted.</param>
        /// <param name="Raw">
        /// if set to <c>true</c> the string is transmitted 'as-is'.
        /// If set to <c>false</c> then protocol framing characters may be added prior to transmission.
        /// </param>
        /// <returns>
        /// Returns the string response received from the device.
        /// </returns>
        public static string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");
            // TODO The optional CommandString method should either be implemented OR throw a MethodNotImplementedException
            // If implemented, CommandString must send the supplied command to the mount and wait for a response before returning this to the client

            throw new MethodNotImplementedException($"CommandString - Command:{command}, Raw: {raw}.");
        }

        /// <summary>
        /// Deterministically release both managed and unmanaged resources that are used by this class.
        /// </summary>
        /// <remarks>
        /// TODO: Release any managed or unmanaged resources that are used in this class.
        /// 
        /// Do not call this method from the Dispose method in your driver class.
        ///
        /// This is because this hardware class is decorated with the <see cref="HardwareClassAttribute"/> attribute and this Dispose() method will be called 
        /// automatically by the  local server executable when it is irretrievably shutting down. This gives you the opportunity to release managed and unmanaged 
        /// resources in a timely fashion and avoid any time delay between local server close down and garbage collection by the .NET runtime.
        ///
        /// For the same reason, do not call the SharedResources.Dispose() method from this method. Any resources used in the static shared resources class
        /// itself should be released in the SharedResources.Dispose() method as usual. The SharedResources.Dispose() method will be called automatically 
        /// by the local server just before it shuts down.
        /// 
        /// </remarks>
        public static void Dispose()
        {
            try { LogMessage("Dispose", $"Disposing of assets and closing down."); } catch { }

            try
            {
                // Clean up the trace logger and utility objects
                tl.Enabled = false;
                tl.Dispose();
                tl = null;
            }
            catch { }

            try
            {
                utilities.Dispose();
                utilities = null;
            }
            catch { }

            try
            {
                astroUtilities.Dispose();
                astroUtilities = null;
            }
            catch { }
        }

        /// <summary>
        /// Synchronously connects to or disconnects from the hardware
        /// </summary>
        /// <param name="uniqueId">Driver's unique ID</param>
        /// <param name="newState">New state: Connected or Disconnected</param>
        public static void SetConnected(Guid uniqueId, bool newState)
        {
            // Check whether we are connecting or disconnecting
            if (newState) // We are connecting
            {
                // Check whether this driver instance has already connected
                if (uniqueIds.Contains(uniqueId)) // Instance already connected
                {
                    // Ignore the request, the unique ID is already in the list
                    LogMessage("SetConnected", $"Ignoring request to connect because the device is already connected.");
                }
                else // Instance not already connected, so connect it
                {
                    // Check whether this is the first connection to the hardware
                    if (uniqueIds.Count == 0) // This is the first connection to the hardware so initiate the hardware connection
                    {
                        //
                        // Add hardware connect logic here
                        //
                        LogMessage("SetConnected", $"Connecting to hardware.");
                    }
                    else // Other device instances are connected so the hardware is already connected
                    {
                        // Since the hardware is already connected no action is required
                        LogMessage("SetConnected", $"Hardware already connected.");
                    }

                    // The hardware either "already was" or "is now" connected, so add the driver unique ID to the connected list
                    uniqueIds.Add(uniqueId);
                    LogMessage("SetConnected", $"Unique id {uniqueId} added to the connection list.");
                }
            }
            else // We are disconnecting
            {
                // Check whether this driver instance has already disconnected
                if (!uniqueIds.Contains(uniqueId)) // Instance not connected so ignore request
                {
                    // Ignore the request, the unique ID is not in the list
                    LogMessage("SetConnected", $"Ignoring request to disconnect because the device is already disconnected.");
                }
                else // Instance currently connected so disconnect it
                {
                    // Remove the driver unique ID to the connected list
                    uniqueIds.Remove(uniqueId);
                    LogMessage("SetConnected", $"Unique id {uniqueId} removed from the connection list.");

                    // Check whether there are now any connected driver instances 
                    if (uniqueIds.Count == 0) // There are no connected driver instances so disconnect from the hardware
                    {
                        //
                        // Add hardware disconnect logic here
                        //

                        AtikPInvoke.ArtemisDisconnect(handle);
                    }
                    else // Other device instances are connected so do not disconnect the hardware
                    {
                        // No action is required
                        LogMessage("SetConnected", $"Hardware already connected.");
                    }
                }
            }

            // Log the current connected state
            LogMessage("SetConnected", $"Currently connected driver ids:");
            foreach (Guid id in uniqueIds)
            {
                LogMessage("SetConnected", $" ID {id} is connected");
            }
        }

        /// <summary>
        /// Returns a description of the device, such as manufacturer and model number. Any ASCII characters may be used.
        /// </summary>
        /// <value>The description.</value>
        public static string Description
        {
            // TODO customise this device description if required
            get
            {
                LogMessage("Description Get", DriverDescription);
                return DriverDescription;
            }
        }

        /// <summary>
        /// Descriptive and version information about this ASCOM driver.
        /// </summary>
        public static string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                // TODO customise this driver description if required
                string driverInfo = $"Information about the driver itself. Version: {version.Major}.{version.Minor}";
                LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        /// <summary>
        /// A string containing only the major and minor version of the driver formatted as 'm.n'.
        /// </summary>
        public static string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = $"{version.Major}.{version.Minor}";
                LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        /// <summary>
        /// The interface version number that this device supports.
        /// </summary>
        public static short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "4");
                return Convert.ToInt16("4");
            }
        }

        /// <summary>
        /// The short name of the driver, for display purposes
        /// </summary>
        public static string Name
        {
            // TODO customise this device name as required
            get
            {
                string name = "Short driver name - please customise";
                LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region ICamera Implementation

        static private int ccdWidth = 2750; // Constants to define the CCD pixel dimensions
        static private int ccdHeight = 2200;
        private const double pixelSize = 4.54; // Constant for the pixel physical dimension

        static private int cameraNumX = ccdWidth; // Initialise variables to hold values required for functionality
        static private int cameraNumY = ccdHeight;

        static private short binx = 1;
        static private short biny = 1;
        static private int cameraStartX = 0;
        static private int cameraStartY = 0;
        static private DateTime exposureStart = DateTime.MinValue;
        static private double cameraLastExposureDuration = 0.0;
        static private bool cameraImageReady = false;
        static private int[,] cameraImageArray;
        static private object[,] cameraImageArrayVariant;

        static private IntPtr handle;
        static private ArtemisPropertiesWrapper cameraProperties;
        static Bitmap image;
        static BitmapSource bitmapSource; // this allows us to save the 16bpp image as a TIFF
        /// <summary>
        /// Aborts the current exposure, if any, and returns the camera to Idle state.
        /// </summary>
        static internal void AbortExposure()
        {
            LogMessage("AbortExposure", "Not implemented");
            AtikPInvoke.ArtemisAbortExposure(handle);
        }

        /// <summary>
        /// Returns the X offset of the Bayer matrix, as defined in <see cref="SensorType" />.
        /// </summary>
        /// <returns>The Bayer colour matrix X offset, as defined in <see cref="SensorType" />.</returns>
        static internal short BayerOffsetX
        {
            get
            {
                LogMessage("BayerOffsetX Get", "Not implemented");
                throw new PropertyNotImplementedException("BayerOffsetX", false);
            }
        }

        /// <summary>
        /// Returns the Y offset of the Bayer matrix, as defined in <see cref="SensorType" />.
        /// </summary>
        /// <returns>The Bayer colour matrix Y offset, as defined in <see cref="SensorType" />.</returns>
        static internal short BayerOffsetY
        {
            get
            {
                LogMessage("BayerOffsetY Get", "Not implemented");
                throw new PropertyNotImplementedException("BayerOffsetX", true);
            }
        }

        /// <summary>
        /// Sets the binning factor for the X axis, also returns the current value.
        /// </summary>
        /// <value>The X binning value</value>
        static internal short BinX
        {
            get
            {
                LogMessage("BinX Get", "1");
                return binx;
            }
            set
            {
                LogMessage("BinX Set", value.ToString());
                binx = value;
                AtikPInvoke.ArtemisBin(handle, binx, biny);
            }
        }

        /// <summary>
        /// Sets the binning factor for the Y axis, also returns the current value.
        /// </summary>
        /// <value>The Y binning value.</value>
        static internal short BinY
        {
            get
            {
                LogMessage("BinY Get", "1");
                return biny;
            }
            set
            {
                LogMessage("BinY Set", value.ToString());
                biny = value;
                AtikPInvoke.ArtemisBin(handle, binx, biny);
            }
        }

        /// <summary>
        /// Returns the current CCD temperature in degrees Celsius.
        /// </summary>
        /// <value>The CCD temperature.</value>
        static internal double CCDTemperature
        {
            get
            {
                int t = 0;
                AtikPInvoke.ArtemisTemperatureSensorInfo(handle, 1, ref t);
                double tD = t * 0.01;
                return tD;
            }
        }

        /// <summary>
        /// Returns the current camera operational state
        /// </summary>
        /// <value>The state of the camera.</value>
        static internal CameraStates CameraState
        {
            get
            {
                LogMessage("CameraState Get", CameraStates.cameraIdle.ToString());
                return CameraStates.cameraIdle;
            }
        }

        /// <summary>
        /// Returns the width of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera X.</value>
        static internal int CameraXSize
        {
            get
            {
                LogMessage("CameraXSize Get", cameraNumX.ToString());
                return cameraNumX;
            }
        }

        /// <summary>
        /// Returns the height of the CCD camera chip in unbinned pixels.
        /// </summary>
        /// <value>The size of the camera Y.</value>
        static internal int CameraYSize
        {
            get
            {
                LogMessage("CameraYSize Get", cameraNumY.ToString());
                return cameraNumY;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the camera can abort exposures; <c>false</c> if not.
        /// </summary>
        /// <value>
        static internal bool CanAbortExposure
        {
            get
            {
                LogMessage("CanAbortExposure Get", false.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns a flag showing whether this camera supports asymmetric binning
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can asymmetric bin; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanAsymmetricBin
        {
            get
            {
                LogMessage("CanAsymmetricBin Get", false.ToString());
                return false;
            }
        }

        /// <summary>
        /// Camera has a fast readout mode
        /// </summary>
        /// <returns><c>true</c> when the camera supports a fast readout mode</returns>
        static internal bool CanFastReadout
        {
            get
            {
                LogMessage("CanFastReadout Get", false.ToString());
                return false;
            }
        }

        /// <summary>
        /// If <c>true</c>, the camera's cooler power setting can be read.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can get cooler power; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanGetCoolerPower
        {
            get
            {
                LogMessage("CanGetCoolerPower Get", false.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera supports pulse guiding
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can pulse guide; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanPulseGuide
        {
            get
            {
                LogMessage("CanPulseGuide Get", false.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera supports setting the CCD temperature
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance can set CCD temperature; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanSetCCDTemperature
        {
            get
            {
                LogMessage("CanSetCCDTemperature Get", false.ToString());
                return true;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera can stop an exposure that is in progress
        /// </summary>
        /// <value>
        /// <c>true</c> if the camera can stop the exposure; otherwise, <c>false</c>.
        /// </value>
        static internal bool CanStopExposure
        {
            get
            {
                LogMessage("CanStopExposure Get", false.ToString());
                return false;
            }
        }

        /// <summary>
        /// Turns on and off the camera cooler, and returns the current on/off state.
        /// </summary>
        /// <value><c>true</c> if the cooler is on; otherwise, <c>false</c>.</value>
        static internal bool CoolerOn
        {
            get
            {
                int flags = 0,level=0,minLevel=0,maxLevel=0,setPoint=0;
                AtikPInvoke.ArtemisCoolingInfo(handle, ref flags, ref level, ref minLevel, ref maxLevel, ref setPoint);
                return  flags==1;
            }
            set
            {
                
            }
        }

        /// <summary>
        /// Returns the present cooler power level, in percent.
        /// </summary>
        /// <value>The cooler power.</value>
        static internal double CoolerPower
        {
            get
            {
                LogMessage("CoolerPower Get", "Not implemented");
                throw new PropertyNotImplementedException("CoolerPower", false);
            }
        }

        /// <summary>
        /// Returns the gain of the camera in photoelectrons per A/D unit.
        /// </summary>
        /// <value>The electrons per ADU.</value>
        static internal double ElectronsPerADU
        {
            get
            {
                LogMessage("ElectronsPerADU Get", "Not implemented");
                throw new PropertyNotImplementedException("ElectronsPerADU", false);
            }
        }

        /// <summary>
        /// Returns the maximum exposure time supported by <see cref="StartExposure">StartExposure</see>.
        /// </summary>
        /// <returns>The maximum exposure time, in seconds, that the camera supports</returns>
        static internal double ExposureMax
        {
            get
            {
                LogMessage("ExposureMax Get", "Not implemented");
                throw new PropertyNotImplementedException("ExposureMax", false);
            }
        }

        /// <summary>
        /// Minimum exposure time
        /// </summary>
        /// <returns>The minimum exposure time, in seconds, that the camera supports through <see cref="StartExposure">StartExposure</see></returns>
        static internal double ExposureMin
        {
            get
            {
                LogMessage("ExposureMin Get", "Not implemented");
                throw new PropertyNotImplementedException("ExposureMin", false);
            }
        }

        /// <summary>
        /// Exposure resolution
        /// </summary>
        /// <returns>The smallest increment in exposure time supported by <see cref="StartExposure">StartExposure</see>.</returns>
        static internal double ExposureResolution
        {
            get
            {
                LogMessage("ExposureResolution Get", "Not implemented");
                throw new PropertyNotImplementedException("ExposureResolution", false);
            }
        }

        /// <summary>
        /// Gets or sets Fast Readout Mode
        /// </summary>
        /// <value><c>true</c> for fast readout mode, <c>false</c> for normal mode</value>
        static internal bool FastReadout
        {
            get
            {
                LogMessage("FastReadout Get", "Not implemented");
                throw new PropertyNotImplementedException("FastReadout", false);
            }
            set
            {
                LogMessage("FastReadout Set", "Not implemented");
                throw new PropertyNotImplementedException("FastReadout", true);
            }
        }

        /// <summary>
        /// Reports the full well capacity of the camera in electrons, at the current camera settings (binning, SetupDialog settings, etc.)
        /// </summary>
        /// <value>The full well capacity.</value>
        static internal double FullWellCapacity
        {
            get
            {
                LogMessage("FullWellCapacity Get", "Not implemented");
                throw new PropertyNotImplementedException("FullWellCapacity", false);
            }
        }


        /// <summary>
        /// The camera's gain (GAIN VALUE MODE) OR the index of the selected camera gain description in the <see cref="Gains" /> array (GAINS INDEX MODE)
        /// </summary>
        /// <returns><para><b> GAIN VALUE MODE:</b> The current gain value.</para>
        /// <p style="color:red"><b>OR</b></p>
        /// <b>GAINS INDEX MODE:</b> Index into the Gains array for the current camera gain
        /// </returns>
        static internal short Gain
        {
            get
            {
                LogMessage("Gain Get", "Not implemented");
                throw new PropertyNotImplementedException("Gain", false);
            }
            set
            {
                LogMessage("Gain Set", "Not implemented");
                throw new PropertyNotImplementedException("Gain", true);
            }
        }

        /// <summary>
        /// Maximum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The maximum gain value that this camera supports</returns>
        static internal short GainMax
        {
            get
            {
                LogMessage("GainMax Get", "Not implemented");
                throw new PropertyNotImplementedException("GainMax", false);
            }
        }

        /// <summary>
        /// Minimum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The minimum gain value that this camera supports</returns>
        static internal short GainMin
        {
            get
            {
                LogMessage("GainMin Get", "Not implemented");
                throw new PropertyNotImplementedException("GainMin", true);
            }
        }

        /// <summary>
        /// Minimum <see cref="Gain" /> value of that this camera supports
        /// </summary>
        /// <returns>The minimum gain value that this camera supports</returns>
        static internal ArrayList Gains
        {
            get
            {
                LogMessage("Gains Get", "Not implemented");
                throw new PropertyNotImplementedException("Gains", true);
            }
        }

        /// <summary>
        /// Returns a flag indicating whether this camera has a mechanical shutter
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has shutter; otherwise, <c>false</c>.
        /// </value>
        static internal bool HasShutter
        {
            get
            {
                LogMessage("HasShutter Get", false.ToString());
                return false;
            }
        }

        /// <summary>
        /// Returns the current heat sink temperature (called "ambient temperature" by some manufacturers) in degrees Celsius.
        /// </summary>
        /// <value>The heat sink temperature.</value>
        static internal double HeatSinkTemperature
        {
            get
            {
                LogMessage("HeatSinkTemperature Get", "Not implemented");
                throw new PropertyNotImplementedException("HeatSinkTemperature", false);
            }
        }

        /// <summary>
        /// Returns a safearray of integers of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
        /// </summary>
        /// <value>The image array.</value>
        static internal object ImageArray
        {
            get
            {   
                if (!cameraImageReady)
                {
                    LogMessage("ImageArray Get", "Throwing InvalidOperationException because of a call to ImageArray before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArray before the first image has been taken!");
                }

                //cameraImageArray = new int[cameraNumX, cameraNumY];
                return cameraImageArray;
            }
        }

        /// <summary>
        /// Returns a safearray of Variant of size <see cref="NumX" /> * <see cref="NumY" /> containing the pixel values from the last exposure.
        /// </summary>
        /// <value>The image array variant.</value>
        static internal object ImageArrayVariant
        {
            get
            {
                if (!cameraImageReady)
                {
                    LogMessage("ImageArrayVariant Get", "Throwing InvalidOperationException because of a call to ImageArrayVariant before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to ImageArrayVariant before the first image has been taken!");
                }
                cameraImageArrayVariant = new object[cameraNumX, cameraNumY];
                for (int i = 0; i < cameraImageArray.GetLength(1); i++)
                {
                    for (int j = 0; j < cameraImageArray.GetLength(0); j++)
                    {
                        cameraImageArrayVariant[j, i] = cameraImageArray[j, i];
                    }

                }

                return cameraImageArrayVariant;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the image is ready to be downloaded from the camera
        /// </summary>
        /// <value><c>true</c> if [image ready]; otherwise, <c>false</c>.</value>
        static internal bool ImageReady
        {
            get
            {
                LogMessage("ImageReady Get", cameraImageReady.ToString());
                return cameraImageReady;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the camera is currently in a <see cref="PulseGuide">PulseGuide</see> operation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pulse guiding; otherwise, <c>false</c>.
        /// </value>
        static internal bool IsPulseGuiding
        {
            get
            {
                LogMessage("IsPulseGuiding Get", "Not implemented");
                throw new PropertyNotImplementedException("IsPulseGuiding", false);
            }
        }

        /// <summary>
        /// Reports the actual exposure duration in seconds (i.e. shutter open time).
        /// </summary>
        /// <value>The last duration of the exposure.</value>
        static internal double LastExposureDuration
        {
            get
            {
                if (!cameraImageReady)
                {
                    LogMessage("LastExposureDuration Get", "Throwing InvalidOperationException because of a call to LastExposureDuration before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to LastExposureDuration before the first image has been taken!");
                }
                LogMessage("LastExposureDuration Get", cameraLastExposureDuration.ToString());
                return cameraLastExposureDuration;
            }
        }

        /// <summary>
        /// Reports the actual exposure start in the FITS-standard CCYY-MM-DDThh:mm:ss[.sss...] format.
        /// The start time must be UTC.
        /// </summary>
        /// <value>The last exposure start time in UTC.</value>
        static internal string LastExposureStartTime
        {
            get
            {
                if (!cameraImageReady)
                {
                    LogMessage("LastExposureStartTime Get", "Throwing InvalidOperationException because of a call to LastExposureStartTime before the first image has been taken!");
                    throw new ASCOM.InvalidOperationException("Call to LastExposureStartTime before the first image has been taken!");
                }
                string exposureStartString = exposureStart.ToString("yyyy-MM-ddTHH:mm:ss");
                LogMessage("LastExposureStartTime Get", exposureStartString.ToString());
                return exposureStartString;
            }
        }

        /// <summary>
        /// Reports the maximum ADU value the camera can produce.
        /// </summary>
        /// <value>The maximum ADU.</value>
        static internal int MaxADU
        {
            get
            {
                LogMessage("MaxADU Get", "20000");
                return 20000;
            }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the X camera axis
        /// </summary>
        /// <value>The maximum bin X.</value>
        static internal short MaxBinX
        {
            get
            {
                LogMessage("MaxBinX Get", "1");
                return 4;
            }
        }

        /// <summary>
        /// Returns the maximum allowed binning for the Y camera axis
        /// </summary>
        /// <value>The maximum bin Y.</value>
        static internal short MaxBinY
        {
            get
            {
                LogMessage("MaxBinY Get", "1");
                return 4;
            }
        }

        /// <summary>
        /// Sets the subframe width. Also returns the current value.
        /// </summary>
        /// <value>The subframe width.</value>
        static internal int NumX
        {
            get
            {
                LogMessage("NumX Get", cameraNumX.ToString());
                return cameraNumX;
            }
            set
            {
                cameraNumX = value;
                LogMessage("NumX set", value.ToString());
            }
        }

        /// <summary>
        /// Sets the subframe height. Also returns the current value.
        /// </summary>
        /// <value>The subframe height.</value>
        static internal int NumY
        {
            get
            {
                LogMessage("NumY Get", cameraNumY.ToString());
                return cameraNumY;
            }
            set
            {
                cameraNumY = value;
                LogMessage("NumY set", value.ToString());
            }
        }

        /// <summary>
        /// The camera's offset (OFFSET VALUE MODE) OR the index of the selected camera offset description in the <see cref="Offsets" /> array (OFFSETS INDEX MODE)
        /// </summary>
        /// <returns><para><b> OFFSET VALUE MODE:</b> The current offset value.</para>
        /// <p style="color:red"><b>OR</b></p>
        /// <b>OFFSETS INDEX MODE:</b> Index into the Offsets array for the current camera offset
        /// </returns>
        static internal int Offset
        {
            get
            {

                return offset;
            }
            set
            {
                offset = value;
            }
        }

        static internal void SetOffset(int off) //set actual hardware offset
        {
            int _gain = 0;
            int _offset = 0;
            _ = AtikPInvoke.ArtemisGetGain(handle, false, ref _gain, ref _offset);
            _ = AtikPInvoke.ArtemisSetGain(handle, false, _gain, off);
            offset = off;

        }
        static internal int GetOffset() //get actual hardware offset
        {
            int _gain = 0;
            int _offset = 0;
            _ = AtikPInvoke.ArtemisGetGain(handle, false, ref _gain, ref _offset);
            return _offset;

        }

        /// <summary>
        /// Maximum <see cref="Offset" /> value that this camera supports
        /// </summary>
        /// <returns>The maximum offset value that this camera supports</returns>
        static internal int OffsetMax
        {
            get
            {
                LogMessage("OffsetMax Get", "Not implemented");
                throw new PropertyNotImplementedException("OffsetMax", false);
            }
        }

        /// <summary>
        /// Minimum <see cref="Offset" /> value that this camera supports
        /// </summary>
        /// <returns>The minimum offset value that this camera supports</returns>
        static internal int OffsetMin
        {
            get
            {
                LogMessage("OffsetMin Get", "Not implemented");
                throw new PropertyNotImplementedException("OffsetMin", true);
            }
        }

        /// <summary>
        /// List of Offset names supported by the camera
        /// </summary>
        /// <returns>The list of supported offset names as an ArrayList of strings</returns>
        static internal ArrayList Offsets
        {
            get
            {
                LogMessage("Offsets Get", "Not implemented");
                throw new PropertyNotImplementedException("Offsets", true);
            }
        }

        /// <summary>
        /// Percent completed, Interface Version 2 and later
        /// </summary>
        /// <returns>A value between 0 and 100% indicating the completeness of this operation</returns>
        static internal short PercentCompleted
        {
            get
            {
                LogMessage("PercentCompleted Get", "Not implemented");
                throw new PropertyNotImplementedException("PercentCompleted", false);
            }
        }

        /// <summary>
        /// Returns the width of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size X.</value>
        static internal double PixelSizeX
        {
            get
            {
                LogMessage("PixelSizeX Get", pixelSize.ToString());
                return pixelSize;
            }
        }

        /// <summary>
        /// Returns the height of the CCD chip pixels in microns.
        /// </summary>
        /// <value>The pixel size Y.</value>
        static internal double PixelSizeY
        {
            get
            {
                LogMessage("PixelSizeY Get", pixelSize.ToString());
                return pixelSize;
            }
        }

        /// <summary>
        /// Activates the Camera's mount control system to instruct the mount to move in a particular direction for a given period of time
        /// </summary>
        /// <param name="Direction">The direction of movement.</param>
        /// <param name="Duration">The duration of movement in milli-seconds.</param>
        static internal void PulseGuide(GuideDirections Direction, int Duration)
        {
            LogMessage("PulseGuide", "Not implemented");
            throw new MethodNotImplementedException("PulseGuide");
        }

        /// <summary>
        /// Readout mode, Interface Version 2 only
        /// </summary>
        /// <value></value>
        /// <returns>Short integer index into the <see cref="ReadoutModes">ReadoutModes</see> array of string readout mode names indicating
        /// the camera's current readout mode.</returns>
        static internal short ReadoutMode
        {
            get
            {
                LogMessage("ReadoutMode Get", "Not implemented");
                throw new PropertyNotImplementedException("ReadoutMode", false);
            }
            set
            {
                LogMessage("ReadoutMode Set", "Not implemented");
                throw new PropertyNotImplementedException("ReadoutMode", true);
            }
        }

        /// <summary>
        /// List of available readout modes, Interface Version 2 only
        /// </summary>
        /// <returns>An ArrayList of readout mode names</returns>
        static internal ArrayList ReadoutModes
        {
            get
            {
                LogMessage("ReadoutModes Get", "Not implemented");
                throw new PropertyNotImplementedException("ReadoutModes", false);
            }
        }

        /// <summary>
        /// Sensor name, Interface Version 2 and later
        /// </summary>
        /// <returns>The name of the sensor used within the camera.</returns>
        static internal string SensorName
        {
            get
            {
                LogMessage("SensorName Get", "Not implemented");
                throw new PropertyNotImplementedException("SensorName", false);
            }
        }

        /// <summary>
        /// Type of colour information returned by the camera sensor, Interface Version 2 and later
        /// </summary>
        /// <value>The type of sensor used by the camera.</value>
        internal static SensorType SensorType
        {
            get
            {
                LogMessage("SensorType Get", "Not implemented");
                throw new PropertyNotImplementedException("SensorType", false);
            }
        }

        /// <summary>
        /// Sets the camera cooler set point in degrees Celsius, and returns the current set point.
        /// </summary>
        /// <value>The set CCD temperature.</value>
        static internal double SetCCDTemperature
        {
            get
            {
                int setPoint = 0,flags=0,level=0,minLevel=0,maxLevel=0;
                AtikPInvoke.ArtemisCoolingInfo(handle, ref flags, ref  level, ref  minLevel, ref  maxLevel, ref  setPoint);
                return (double)setPoint;
            }
            set
            {
                
                AtikPInvoke.ArtemisSetCooling(handle, (int) value*100);
              
            }
        }

        /// <value>The set CCD temperature.</value>
        static internal void warmup()
        {
           
                AtikPInvoke.ArtemisCoolerWarmUp(handle);
            
        }
        
        /// <summary>
        /// Starts an exposure. Use <see cref="ImageReady" /> to check when the exposure is complete.
        /// </summary>
        /// <param name="Duration">Duration of exposure in seconds, can be zero if <see cref="StartExposure">Light</see> is <c>false</c></param>
        /// <param name="Light"><c>true</c> for light frame, <c>false</c> for dark frame (ignored if no shutter)</param>
        static internal void StartExposure(double Duration, bool Light)
        {
            cameraImageReady = false;

            if (Duration < 0.0) throw new InvalidValueException("StartExposure", Duration.ToString(), "0.0 upwards");
            if (cameraNumX > ccdWidth) throw new InvalidValueException("StartExposure", cameraNumX.ToString(), ccdWidth.ToString());
            if (cameraNumY > ccdHeight) throw new InvalidValueException("StartExposure", cameraNumY.ToString(), ccdHeight.ToString());
            if (cameraStartX > ccdWidth) throw new InvalidValueException("StartExposure", cameraStartX.ToString(), ccdWidth.ToString());
            if (cameraStartY > ccdHeight) throw new InvalidValueException("StartExposure", cameraStartY.ToString(), ccdHeight.ToString());
            if (CameraHardware.handle.ToInt32() != 0)
            {
                if (AtikPInvoke.ArtemisStartExposure(handle, (float)Duration) == (int)ArtemisError.OK)
                {

                    while (!AtikPInvoke.ArtemisImageReady(handle))
                    {
                        //if (stopProcessing)
                        //{
                        //    stopProcessing = false;
                        //    return;
                        //}

                        Thread.Sleep(100);
                    }

                    int x = 0, y = 0, w = 0, h = 0, binX = 0, binY = 0;
                    AtikPInvoke.ArtemisGetImageData(handle, ref x, ref y,
                                                            ref w, ref h,
                                                            ref binX, ref binY);


                    // Create memory to copy pixels into
                    ushort[] pixels = new ushort[w * h];

                   

                    var intPtr = AtikPInvoke.ArtemisImageBuffer(handle);
                    unsafe
                    {
                        var ptr = (ushort*)intPtr;
                        for (var i = 0; i < w * h; ++i)
                        {
                            pixels[i] = *ptr++;
                        }
                    }

                   

                    //bitmapSource = BitmapSource.Create(w, h, 96d, 96d, PixelFormats.Gray16, null, pixels, (w * 16 + 7) / 8);

                    //var bitmap = new FormatConvertedBitmap();
                    //bitmap.BeginInit();
                    //bitmap.Source = bitmapSource.Clone();
                    //bitmap.DestinationFormat = PixelFormats.Bgra32;
                    //bitmap.EndInit();

                    //image = new Bitmap(bitmap.PixelWidth, bitmap.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    //BitmapData data = image.LockBits(new Rectangle(Point.Empty, image.Size), ImageLockMode.WriteOnly, image.PixelFormat);
                    //bitmap.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                    //image.UnlockBits(data);

                    //// this allows us to access the image to save it on the ui thread
                    //bitmapSource.Freeze();
                    cameraImageArray = new int[w, h];
                    //int n = 0;
                    for (int yy = 0; yy < h; yy++)
                    {
                        for (int xx = 0; xx < w; xx++)
                        {
                            int index = yy * w + xx;
                            if (index < pixels.Length)
                            {
                                cameraImageArray[xx, yy] = pixels[index];
                            }
                            else
                            {
                                cameraImageArray[xx, yy] = 0;
                            }
                        }
                    }





                }
                else
                {
                    
                }
            }
            cameraLastExposureDuration = Duration;
            exposureStart = DateTime.Now;

            //copy image frame
            ////check if roi in use

            //int tempW = (myCam.Region[0].s2 - myCam.Region[0].s1 + 1) / myCam.Region[0].sbin;

            //int tempH = (myCam.Region[0].p2 - myCam.Region[0].p1 + 1) / myCam.Region[0].pbin;


            
          

            LogMessage("StartExposure", Duration.ToString() + " " + Light.ToString());
            cameraImageReady = true;
        }

        /// <summary>
        /// Sets the subframe start position for the X axis (0 based) and returns the current value.
        /// </summary>
        static internal int StartX
        {
            get
            {
                LogMessage("StartX Get", cameraStartX.ToString());
                return cameraStartX;
            }
            set
            {
                cameraStartX = value;
                LogMessage("StartX Set", value.ToString());
            }
        }

        /// <summary>
        /// Sets the subframe start position for the Y axis (0 based). Also returns the current value.
        /// </summary>
        static internal int StartY
        {
            get
            {
                LogMessage("StartY Get", cameraStartY.ToString());
                return cameraStartY;
            }
            set
            {
                cameraStartY = value;
                LogMessage("StartY set", value.ToString());
            }
        }

        /// <summary>
        /// Stops the current exposure, if any.
        /// </summary>
        static internal void StopExposure()
        {
            LogMessage("StopExposure", "Not implemented");
            throw new MethodNotImplementedException("StopExposure");
        }

        /// <summary>
        /// Camera's sub-exposure interval
        /// </summary>
        static internal double SubExposureDuration
        {
            get
            {
                LogMessage("SubExposureDuration Get", "Not implemented");
                throw new PropertyNotImplementedException("SubExposureDuration", false);
            }
            set
            {
                LogMessage("SubExposureDuration Set", "Not implemented");
                throw new PropertyNotImplementedException("SubExposureDuration", true);
            }
        }

        #endregion

        #region Private properties and methods
        // Useful methods that can be used as required to help with driver development

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private static bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private static void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal static void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                tl.Enabled = false;// Convert.ToBoolean(driverProfile.GetValue(DriverProgId, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(DriverProgId, comPortProfileName, string.Empty, comPortDefault);
                offset = int.Parse(driverProfile.GetValue(DriverProgId, offsetProfileName, string.Empty, offsetDefault));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal static void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "Camera";
                driverProfile.WriteValue(DriverProgId, traceStateProfileName, tl.Enabled.ToString());
                driverProfile.WriteValue(DriverProgId, comPortProfileName, comPort.ToString());
                driverProfile.WriteValue(DriverProgId, offsetProfileName, offset.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes identifier and message strings
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        internal static void LogMessage(string identifier, string message)
        {
            tl.LogMessageCrLf(identifier, message);
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            LogMessage(identifier, msg);
        }
        #endregion
    }
}

