/*
  This example describes the FIRST STEPS of handling Baumer-GAPI SDK.
  The given source code applies to handling one system, one camera and twelfe images.
  Please see "Baumer-GAPI SDK Programmer's Guide" chapter 5.1 and chapter 5.2
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices; // Marshal.Copy()
using System.IO;
namespace BaumerAPI
{

    public delegate void FrameReceivedHandler(object sender, FrameEventArgs args);

    public class FrameEventArgs : EventArgs
    {
        /// <summary>
        /// The Image (data)
        /// </summary>
        private Image m_Image = null;

        /// <summary>
        /// The Exception (data)
        /// </summary>
        private Exception m_Exception = null;

        /// <summary>
        /// Initializes a new instance of the FrameEventArgs class. 
        /// </summary>
        /// <param name="image">The Image to transfer</param>
        public FrameEventArgs(Image image)
        {
            if (null == image)
            {
                throw new ArgumentNullException("image");
            }

            m_Image = image;
        }

        /// <summary>
        /// Initializes a new instance of the FrameEventArgs class. 
        /// </summary>
        /// <param name="exception">The Exception to transfer</param>
        public FrameEventArgs(Exception exception)
        {
            if (null == exception)
            {
                throw new ArgumentNullException("exception");
            }

            m_Exception = exception;
        }

        /// <summary>
        /// Gets the image 
        /// </summary>
        public Image Image
        {
            get
            {
                return m_Image;
            }
        }

        /// <summary>
        /// Gets the Exception
        /// </summary>
        public Exception Exception
        {
            get
            {
                return m_Exception;
            }
        }
    }

    public class GIGEGrabber
    {
      
       

        public  int gImageCounter = 0;
        BGAPI2.Device mDevice = null;
        int returnCode = 0;
        BGAPI2.SystemList systemList = null;
        BGAPI2.System mSystem = null;
        string sSystemID = "";
        string m_camID = "";
        BGAPI2.InterfaceList interfaceList = null;
        BGAPI2.Interface mInterface = null;
        string sInterfaceID = "";

        BGAPI2.DeviceList deviceList = null;
        int retries = 0;
        string sDeviceID = "";

        BGAPI2.DataStreamList datastreamList = null;
        BGAPI2.DataStream mDataStream = null;
        string sDataStreamID = "";
        
        BGAPI2.BufferList bufferList = null;
        BGAPI2.Buffer mBuffer = null;

        BGAPI2.ImageProcessor  imgProcessor = null;
         byte[] imageBufferCopy;
         byte[] transformImageBufferCopy;
         Bitmap b;
         FrameReceivedHandler m_frh;
        Boolean running = false;
        public byte[] masterDark;
        public bool useDarks = false;
        public int pixelCutOff = 3000;
        public int darkMultiplier = 1;
        //public GIGEGRABBER()
        //{
        //    openCamera();

        //}
        //static int Main(string[] args)
        //{
        //    GIGEGRABBER g = new GIGEGRABBER();
        //    g.openCamera();
        //    return 1;

        //}
        public void stopAcquisition()
        {
            try
            {
                mDevice.RemoteNodeList["AcquisitionStop"].Execute();
                System.Console.Write("5.1.12   {0} started \r\n", mDevice.Model);
                running = false;
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
                return;
            }

        }
        public void startAcquisition()
        {
            //START CAMERA
            try
            {
                mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                System.Console.Write("5.1.12   {0} started \r\n", mDevice.Model);
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
                return;
            }
            int retryClose = 0;
            int retryOpen = 0;
            int retryStart = 0;
            BGAPI2.Buffer mBufferFilled = null;
            running = true;

                try
                    { 
                    while (running) 
                    {
                        mBufferFilled = mDataStream.GetFilledBuffer(15000); // image polling timeout 1000 msec
                        retryClose = 0;
                        retryOpen = 0;
                        retryStart = 0;
                        if (mBufferFilled == null)
                        {
                            System.Console.Write("Error: Buffer Timeout after 15000 msec\r\n");
                        //check to see if camera is still connected
                        System.Console.WriteLine("device isOpen {0}", mDevice.IsOpen);
                        System.Console.WriteLine("device AccessStatus {0}", mDevice.AccessStatus);
                        System.Console.WriteLine("datastream isOpen {0}", mDataStream.IsOpen);
                        System.Console.WriteLine("datastream EventMode {0}", mDataStream.EventMode);
                        System.Console.WriteLine("datastream isGrabbing {0}", mDataStream.IsGrabbing);

                            //make sure things are shut down
                        retryClose = 0;
                        while (retryClose < 5)
                        { 
                        try
                            {
                                retryClose = retryClose + 1;
                                    
                                System.Console.WriteLine("closing datastream...");
                                mDataStream.Close();

//                                    System.AccessViolationException
//  HResult = 0x80004003
//  Message = Attempted to read or write protected memory.This is often an indication that other memory is corrupt.
//   Source=<Cannot evaluate the exception source>
//  StackTrace:
//<Cannot evaluate the exception stack trace>

                                System.Console.WriteLine("closing device...");
                                mDevice.Close();
                                System.Console.WriteLine("closing interface...");
                                mInterface.Close();
                                System.Console.WriteLine("closing system...");
                                foreach (KeyValuePair<string, BGAPI2.System> sys_pair in BGAPI2.SystemList.Instance)
                                    if (sys_pair.Key == sSystemID)
                                        sys_pair.Value.Close();
                                retryClose = 5;
                            }
                            catch {
                                System.Console.WriteLine("problem closing datastream...retries {0}",retryClose);
                                Thread.Sleep(5000); //wait 5 seconds & try again
                            }
                        }
                        //
                        while (retryOpen < 5)
                        {
                            try
                            {
                                retryOpen = retryOpen + 1;
                                System.Console.WriteLine("restart open camera...");
                                openCamera(m_camID);
                                
                                
                                running = true;
                                retryOpen = 5;//leave while loop
                            }
                            catch
                            {
                                System.Console.WriteLine("problem openning datastream...retries {0}", retryOpen);
                                Thread.Sleep(5000); //wait 5 seconds & try again

                            }
                        }

                        while (retryStart < 5)
                        {
                            try
                            {
                                retryStart = retryStart + 1;
 
                                System.Console.WriteLine("restart capture...");
                                startCapture(m_frh);
                                mDevice.RemoteNodeList["AcquisitionStart"].Execute();
                                System.Console.Write("5.1.12   {0} started \r\n", mDevice.Model);
                                running = true;
                                retryStart = 5;//leave while loop
                            }
                            catch
                            {
                                System.Console.WriteLine("problem openning datastream...retries {0}", retryOpen);
                                Thread.Sleep(5000); //wait 5 seconds & try again

                            }
                        }



                        // System.Console.WriteLine("start capture...");
                        //startCapture(m_frh);
                        //while (true)
                        //{
                        //    System.Console.WriteLine("pause for 10 secs");
                        //    Thread.Sleep(10000);
                        //    mDevice.DataStreams.Refresh();
                        //    System.Console.WriteLine("device isOpen {0}", mDevice.IsOpen);
                        //    System.Console.WriteLine("device AccessStatus {0}", mDevice.AccessStatus);
                        //    System.Console.WriteLine("datastream isOpen {0}", mDataStream.IsOpen);
                        //    System.Console.WriteLine("datastream EventMode {0}", mDataStream.EventMode);
                        //    System.Console.WriteLine("datastream isGrabbing {0}", mDataStream.IsGrabbing);
                        //    System.Console.WriteLine("-----");
                        //}
                        //System.Console.WriteLine("try to stop");
                        //stopAcquisition();
                        //System.Console.WriteLine("try to restart");
                        //startAcquisition();
                    }


                    
                        else if (mBufferFilled.IsIncomplete == true)
                        {
                            System.Console.Write("Error: Image is incomplete\r\n");
                            // queue buffer again
                            mBufferFilled.QueueBuffer();
                        }
                        else
                        {
                            System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", mBufferFilled.FrameID, (ulong)mBufferFilled.MemPtr);
                        //send image to user
                            retries = 0;
                            processImage(mBufferFilled);
                            // queue buffer again
                            mBufferFilled.QueueBuffer();
                        }
                    }
                }
                catch (BGAPI2.Exceptions.IException ex)
                {
                    returnCode = (0 == returnCode) ? 1 : returnCode;
                    System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                    System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                    System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
                }
                System.Console.Write("\r\n");

            
        }
        public void setParams(int duration, int aGain)
        {

            mDevice.RemoteNodeList["ExposureTime"].Value = duration;
            mDevice.RemoteNodeList["GainRaw"].Value = aGain;
            mDevice.RemoteNodeList["AcquisitionMode"].Value = "Continuous";

           

        }

        public void openCamera(String camid)
        {
            //DECLARATIONS OF VARIABLES

            m_camID = camid;

            System.Console.Write("\r\n");
            System.Console.Write("##############################################################\r\n");
            System.Console.Write("# PROGRAMMER'S GUIDE Example 011_ImageCaptureMode_Handler.cs #\r\n");
            System.Console.Write("##############################################################\r\n");
            System.Console.Write("\r\n\r\n");


            System.Console.Write("SYSTEM LIST\r\n");
            System.Console.Write("###########\r\n\r\n");

            //COUNTING AVAILABLE SYSTEMS (TL producers)
            try
            {
                systemList = BGAPI2.SystemList.Instance;
                systemList.Refresh();
                System.Console.Write("5.1.2   Detected systems:  {0}\r\n", systemList.Count);

                //SYSTEM DEVICE INFORMATION
                foreach (KeyValuePair<string, BGAPI2.System> sys_pair in BGAPI2.SystemList.Instance)
                {
                    System.Console.Write("  5.2.1   System Name:     {0}\r\n", sys_pair.Value.FileName);
                    System.Console.Write("          System Type:     {0}\r\n", sys_pair.Value.TLType);
                    System.Console.Write("          System Version:  {0}\r\n", sys_pair.Value.Version);
                    System.Console.Write("          System PathName: {0}\r\n\r\n", sys_pair.Value.PathName);
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }


            //OPEN THE FIRST SYSTEM IN THE LIST WITH A CAMERA CONNECTED
            try
            {
                foreach (KeyValuePair<string, BGAPI2.System> sys_pair in BGAPI2.SystemList.Instance)
                {
                    System.Console.Write("SYSTEM\r\n");
                    System.Console.Write("######\r\n\r\n");

                    try
                    {
                        sys_pair.Value.Open();
                        System.Console.Write("5.1.3   Open next system \r\n");
                        System.Console.Write("  5.2.1   System Name:     {0}\r\n", sys_pair.Value.FileName);
                        System.Console.Write("          System Type:     {0}\r\n", sys_pair.Value.TLType);
                        System.Console.Write("          System Version:  {0}\r\n", sys_pair.Value.Version);
                        System.Console.Write("          System PathName: {0}\r\n\r\n", sys_pair.Value.PathName);
                        sSystemID = sys_pair.Key;
                        System.Console.Write("        Opened system - NodeList Information \r\n");
                        System.Console.Write("          GenTL Version:   {0}.{1}\r\n\r\n", (long)sys_pair.Value.NodeList["GenTLVersionMajor"].Value, (long)sys_pair.Value.NodeList["GenTLVersionMinor"].Value);


                        System.Console.Write("INTERFACE LIST\r\n");
                        System.Console.Write("##############\r\n\r\n");

                        try
                        {
                            interfaceList = sys_pair.Value.Interfaces;
                            //COUNT AVAILABLE INTERFACES
                            interfaceList.Refresh(100); // timeout of 100 msec
                            System.Console.Write("5.1.4   Detected interfaces: {0}\r\n", interfaceList.Count);

                            //INTERFACE INFORMATION
                            foreach (KeyValuePair<string, BGAPI2.Interface> ifc_pair in interfaceList)
                            {
                                System.Console.Write("  5.2.2   Interface ID:      {0}\r\n", ifc_pair.Value.Id);
                                System.Console.Write("          Interface Type:    {0}\r\n", ifc_pair.Value.TLType);
                                System.Console.Write("          Interface Name:    {0}\r\n\r\n", ifc_pair.Value.DisplayName);
                            }
                        }
                        catch (BGAPI2.Exceptions.IException ex)
                        {
                            returnCode = (0 == returnCode) ? 1 : returnCode;
                            System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                            System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                            System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
                        }


                        System.Console.Write("INTERFACE\r\n");
                        System.Console.Write("#########\r\n\r\n");

                        //OPEN THE NEXT INTERFACE IN THE LIST
                        try
                        {
                            foreach (KeyValuePair<string, BGAPI2.Interface> ifc_pair in interfaceList)
                            {
                                try
                                {
                                    System.Console.Write("5.1.5   Open interface \r\n");
                                    System.Console.Write("  5.2.2   Interface ID:      {0}\r\n", ifc_pair.Key);
                                    System.Console.Write("          Interface Type:    {0}\r\n", ifc_pair.Value.TLType);
                                    System.Console.Write("          Interface Name:    {0}\r\n", ifc_pair.Value.DisplayName);
                                    ifc_pair.Value.Open();
                                    //search for any camera is connetced to this interface
                                    deviceList = ifc_pair.Value.Devices;
                                    deviceList.Refresh(100);
                                    if (deviceList.Count == 0)
                                    {
                                        System.Console.Write("5.1.13   Close interface ({0} cameras found) \r\n\r\n", deviceList.Count);
                                        ifc_pair.Value.Close();//close interface
                                        sys_pair.Value.Close();//close system too
                                        throw new ArgumentNullException("no camera found");
                                       
                                    }
                                    else
                                    {
                                        sInterfaceID = ifc_pair.Key;
                                        System.Console.Write("  \r\n");
                                        System.Console.Write("        Opened interface - NodeList Information \r\n");
                                        if (ifc_pair.Value.TLType == "GEV")
                                        {
                                            long iIPAddress = (long)ifc_pair.Value.NodeList["GevInterfaceSubnetIPAddress"].Value;
                                            System.Console.Write("          GevInterfaceSubnetIPAddress: {0}.{1}.{2}.{3}\r\n", (iIPAddress & 0xff000000) >> 24,
                                                                                                                            (iIPAddress & 0x00ff0000) >> 16,
                                                                                                                            (iIPAddress & 0x0000ff00) >> 8,
                                                                                                                            (iIPAddress & 0x000000ff));
                                            long iSubnetMask = (long)ifc_pair.Value.NodeList["GevInterfaceSubnetMask"].Value;
                                            System.Console.Write("          GevInterfaceSubnetMask:      {0}.{1}.{2}.{3}\r\n", (iSubnetMask & 0xff000000) >> 24,
                                                                                                                            (iSubnetMask & 0x00ff0000) >> 16,
                                                                                                                            (iSubnetMask & 0x0000ff00) >> 8,
                                                                                                                            (iSubnetMask & 0x000000ff));
                                        }
                                        if (ifc_pair.Value.TLType == "U3V")
                                        {
                                            //System.Console.Write("          NodeListCount:               {0}\r\n", ifc_pair.Value.NodeList.Count);
                                        }
                                        System.Console.Write("  \r\n");
                                        break;
                                    }
                                }
                                catch (BGAPI2.Exceptions.ResourceInUseException ex)
                                {
                                    returnCode = (0 == returnCode) ? 1 : returnCode;
                                    System.Console.Write(" Interface {0} already opened \r\n", ifc_pair.Key);
                                    System.Console.Write(" ResourceInUseException {0} \r\n", ex.GetErrorDescription());
                                }
                            }
                        }
                        catch (BGAPI2.Exceptions.IException ex)
                        {
                            returnCode = (0 == returnCode) ? 1 : returnCode;
                            System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                            System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                            System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
                        }

                        //if a camera is connected to the system interface then leave the system loop
                        if (sInterfaceID != "")
                        {
                            break;
                        }
                    }
                    catch (BGAPI2.Exceptions.ResourceInUseException ex)
                    {
                        returnCode = (0 == returnCode) ? 1 : returnCode;
                        System.Console.Write(" System {0} already opened \r\n", sys_pair.Key);
                        System.Console.Write(" ResourceInUseException {0} \r\n", ex.GetErrorDescription());
                    }
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }

            if (sSystemID == "")
            {
                System.Console.Write(" No System found \r\n");
                System.Console.Write(" Input any number to close the program:\r\n");
                //Console.Read();
                return;
                //return returnCode;
            }
            else
            {
                mSystem = systemList[sSystemID];
            }


            if (sInterfaceID == "")
            {
                System.Console.Write(" No Interface of TLType 'GEV' found \r\n");
                System.Console.Write("\r\nEnd\r\nInput any number to close the program:\r\n");
                //Console.Read();
                mSystem.Close();
                return;
            }
            else
            {
                mInterface = interfaceList[sInterfaceID];
            }


            System.Console.Write("DEVICE LIST\r\n");
            System.Console.Write("###########\r\n\r\n");

            try
            {
                //COUNTING AVAILABLE CAMERAS
                retries = 0;
                while (retries < 5)
                { 
                    deviceList = mInterface.Devices;
                    deviceList.Refresh(5000);
                    System.Console.Write("5.1.6   Detected devices:         {0}\r\n", deviceList.Count);
                    if (deviceList.Count==0)
                    {
                        Thread.Sleep(5000);
                        retries = retries + 1;
                    }
                    else
                    {
                        retries = 5;
                    }
                }

                //DEVICE INFORMATION BEFORE OPENING
                foreach (KeyValuePair<string, BGAPI2.Device> dev_pair in deviceList)
                {
                    System.Console.Write("  5.2.3   Device DeviceID:        {0}\r\n", dev_pair.Key);
                    System.Console.Write("          Device Model:           {0}\r\n", dev_pair.Value.Model);
                    System.Console.Write("          Device SerialNumber:    {0}\r\n", dev_pair.Value.SerialNumber);
                    System.Console.Write("          Device Vendor:          {0}\r\n", dev_pair.Value.Vendor);
                    System.Console.Write("          Device TLType:          {0}\r\n", dev_pair.Value.TLType);
                    System.Console.Write("          Device AccessStatus:    {0}\r\n", dev_pair.Value.AccessStatus);
                    System.Console.Write("          Device UserID:          {0}\r\n\r\n", dev_pair.Value.DisplayName);
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }


            System.Console.Write("DEVICE\r\n");
            System.Console.Write("######\r\n\r\n");

            //OPEN THE FIRST CAMERA IN THE LIST
            try
            {
                foreach (KeyValuePair<string, BGAPI2.Device> dev_pair in deviceList)
                {
                    try
                    {
                        System.Console.Write("5.1.7   Open first device \r\n");
                        System.Console.Write("          Device DeviceID:        {0}\r\n", dev_pair.Value.Id);
                        System.Console.Write("          Device Model:           {0}\r\n", dev_pair.Value.Model);
                        System.Console.Write("          Device SerialNumber:    {0}\r\n", dev_pair.Value.SerialNumber);
                        System.Console.Write("          Device Vendor:          {0}\r\n", dev_pair.Value.Vendor);
                        System.Console.Write("          Device TLType:          {0}\r\n", dev_pair.Value.TLType);
                        System.Console.Write("          Device AccessStatus:    {0}\r\n", dev_pair.Value.AccessStatus);
                        System.Console.Write("          Device UserID:          {0}\r\n\r\n", dev_pair.Value.DisplayName);
                        if (dev_pair.Value.Model==camid)
                        {
                        dev_pair.Value.Open();
                        sDeviceID = dev_pair.Key;
                        System.Console.Write("        Opened device - RemoteNodeList Information \r\n");
                        System.Console.Write("          Device AccessStatus:    {0}\r\n", dev_pair.Value.AccessStatus);

                        //SERIAL NUMBER
                        if (dev_pair.Value.RemoteNodeList.GetNodePresent("DeviceSerialNumber") == true)
                            System.Console.Write("          DeviceSerialNumber:     {0}\r\n", (string)dev_pair.Value.RemoteNodeList["DeviceSerialNumber"].Value);
                        else if (dev_pair.Value.RemoteNodeList.GetNodePresent("DeviceID") == true)
                            System.Console.Write("          DeviceID (SN):          {0}\r\n", (string)dev_pair.Value.RemoteNodeList["DeviceID"].Value);
                        else
                            System.Console.Write("          SerialNumber:           Not Available.\r\n");

                        //DISPLAY DEVICEMANUFACTURERINFO
                        if (dev_pair.Value.RemoteNodeList.GetNodePresent("DeviceManufacturerInfo") == true)
                            System.Console.Write("          DeviceManufacturerInfo: {0}\r\n", (string)dev_pair.Value.RemoteNodeList["DeviceManufacturerInfo"].Value);

                        //DISPLAY DEVICEFIRMWAREVERSION OR DEVICEVERSION
                        if (dev_pair.Value.RemoteNodeList.GetNodePresent("DeviceFirmwareVersion") == true)
                            System.Console.Write("          DeviceFirmwareVersion:  {0}\r\n", (string)dev_pair.Value.RemoteNodeList["DeviceFirmwareVersion"].Value);
                        else if (dev_pair.Value.RemoteNodeList.GetNodePresent("DeviceVersion") == true)
                            System.Console.Write("          DeviceVersion:          {0}\r\n", (string)dev_pair.Value.RemoteNodeList["DeviceVersion"].Value);
                        else
                            System.Console.Write("          DeviceVersion:          Not Available.\r\n");

                        if (dev_pair.Value.TLType == "GEV")
                        {
                            System.Console.Write("          GevCCP:                 {0}\r\n", (string)dev_pair.Value.RemoteNodeList["GevCCP"].Value);
                            System.Console.Write("          GevCurrentIPAddress:    {0}.{1}.{2}.{3}\r\n", ((long)dev_pair.Value.RemoteNodeList["GevCurrentIPAddress"].Value & 0xff000000) >> 24, ((long)dev_pair.Value.RemoteNodeList["GevCurrentIPAddress"].Value & 0x00ff0000) >> 16, ((long)dev_pair.Value.RemoteNodeList["GevCurrentIPAddress"].Value & 0x0000ff00) >> 8, ((long)dev_pair.Value.RemoteNodeList["GevCurrentIPAddress"].Value & 0x000000ff));
                            System.Console.Write("          GevCurrentSubnetMask:   {0}.{1}.{2}.{3}\r\n", ((long)dev_pair.Value.RemoteNodeList["GevCurrentSubnetMask"].Value & 0xff000000) >> 24, ((long)dev_pair.Value.RemoteNodeList["GevCurrentSubnetMask"].Value & 0x00ff0000) >> 16, ((long)dev_pair.Value.RemoteNodeList["GevCurrentSubnetMask"].Value & 0x0000ff00) >> 8, ((long)dev_pair.Value.RemoteNodeList["GevCurrentSubnetMask"].Value & 0x000000ff));
                        }
                        System.Console.Write("          \r\n");
                        break;
                        }
                    }
                    catch (BGAPI2.Exceptions.ResourceInUseException ex)
                    {
                        returnCode = (0 == returnCode) ? 1 : returnCode;
                        System.Console.Write(" Device {0} already opened \r\n", dev_pair.Key);
                        System.Console.Write(" ResourceInUseException {0} \r\n", ex.GetErrorDescription());
                    }
                    catch (BGAPI2.Exceptions.AccessDeniedException ex)
                    {
                        returnCode = (0 == returnCode) ? 1 : returnCode;
                        System.Console.Write(" Device {0} already opened \r\n", dev_pair.Key);
                        System.Console.Write(" AccessDeniedException {0} \r\n", ex.GetErrorDescription());
                    }
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }

            if (sDeviceID == "")
            {
                System.Console.Write(" No Device found \r\n");
                System.Console.Write("\r\nEnd\r\nInput any number to close the program:\r\n");
                //Console.Read();
                mInterface.Close();
                mSystem.Close();
                return;
            }
            else
            {
                mDevice = deviceList[sDeviceID];
            }


            System.Console.Write("DEVICE PARAMETER SETUP\r\n");
            System.Console.Write("######################\r\n\r\n");
        }

        public void closeCamera()
        { 
                mDevice.Close();
                mInterface.Close();
                mSystem.Close();
        }

        public void startCapture(FrameReceivedHandler frh)
        {
            m_frh = frh;
            datastreamList = mDevice.DataStreams;
            datastreamList.Refresh();
            try
            {
                mDevice.RemoteNodeList["TriggerMode"].Value = "Off";
            }
            catch (BGAPI2.Exceptions.IException ex)
            { returnCode = (0 == returnCode) ? 1 : returnCode; }
            try
            {
                foreach (KeyValuePair<string, BGAPI2.DataStream> dst_pair in datastreamList)
                {
                    System.Console.Write("5.1.9   Open first datastream \r\n");
                    System.Console.Write("          DataStream ID:          {0}\r\n\r\n", dst_pair.Key);
                    dst_pair.Value.Open();
                    sDataStreamID = dst_pair.Key;
                    System.Console.Write("        Opened datastream - NodeList Information \r\n");
                    System.Console.Write("          StreamAnnounceBufferMinimum:  {0}\r\n", dst_pair.Value.NodeList["StreamAnnounceBufferMinimum"].Value);
                    if (dst_pair.Value.TLType == "GEV")
                    {
                        System.Console.Write("          StreamDriverModel:            {0}\r\n", dst_pair.Value.NodeList["StreamDriverModel"].Value);
                    }
                    System.Console.Write("  \r\n");
                    break;
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }
            //
            if (sDataStreamID == "")
            {
                System.Console.Write(" No DataStream found \r\n");
                System.Console.Write("\r\nEnd\r\nInput any number to close the program:\r\n");
                Console.Read();
                mDevice.Close();
                mInterface.Close();
                mSystem.Close();
                return;
            }
            else
            {
                mDataStream = datastreamList[sDataStreamID];
            }
            //setup buffers
            try
            {
                //BufferList
                bufferList = mDataStream.BufferList;

                // 4 buffers using internal buffer mode
                for (int i = 0; i < 4; i++)
                {
                    mBuffer = new BGAPI2.Buffer();
                    bufferList.Add(mBuffer);
                }
                System.Console.Write("5.1.10   Announced buffers:       {0} using {1} [bytes]\r\n", bufferList.AnnouncedCount, mBuffer.MemSize * bufferList.AnnouncedCount);
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }


            try
            {
                foreach (KeyValuePair<string, BGAPI2.Buffer> buf_pair in bufferList)
                {
                    buf_pair.Value.QueueBuffer();
                }
                System.Console.Write("5.1.11   Queued buffers:          {0}\r\n", bufferList.QueuedCount);
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }
            System.Console.Write("\r\n");


            System.Console.Write("REGISTER NEW BUFFER EVENT TO: EVENTMODE_EVENT_HANDLER\r\n");
            System.Console.Write("#####################################################\r\n\r\n");

            //comment out the next section to disable event driven
            //try
            //{
            //    mDataStream.NewBufferEvent += new BGAPI2.Events.DataStreamEventControl.NewBufferEventHandler(mDataStream_NewBufferEvent);
            //    mDataStream.RegisterNewBufferEvent(BGAPI2.Events.EventMode.EVENT_HANDLER);
            //    System.Console.Write("        Register Event Mode to:   {0}\r\n\r\n", mDataStream.EventMode.ToString());
            //}
            //catch (BGAPI2.Exceptions.IException ex)
            //{
            //    returnCode = (0 == returnCode) ? 1 : returnCode;
            //    System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
            //    System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
            //    System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            //}


            System.Console.Write("CAMERA START\r\n");
            System.Console.Write("############\r\n\r\n");

            //START DATASTREAM ACQUISITION
            try
            {
                mDataStream.StartAcquisition();
                System.Console.Write("5.1.12   DataStream started \r\n");
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }

            //START CAMERA
            //try
            //{
            //    mDevice.RemoteNodeList["AcquisitionStart"].Execute();
            //    System.Console.Write("5.1.12   {0} started \r\n", mDevice.Model);
            //}
            //catch (BGAPI2.Exceptions.IException ex)
            //{
            //    returnCode = (0 == returnCode) ? 1 : returnCode;
            //    System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
            //    System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
            //    System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            //}


            //LOAD IMAGE PROCESSOR
            try
            {
                imgProcessor = new BGAPI2.ImageProcessor();
                System.Console.Write("ImageProcessor version:    {0} \r\n", imgProcessor.GetVersion());
                if (imgProcessor.NodeList.GetNodePresent("DemosaicingMethod") == true)
                {
                    imgProcessor.NodeList["DemosaicingMethod"].Value = "NearestNeighbor"; // NearestNeighbor, Bilinear3x3, Baumer5x5
                    System.Console.Write("    Demosaicing method:    {0} \r\n", (string)imgProcessor.NodeList["DemosaicingMethod"].Value);
                }
                System.Console.Write("\r\n");
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }

            loadMasterDark();
            //    //CAPTURE 12 IMAGES
            //    System.Console.Write("\r\n");
            //    System.Console.Write("CAPTURE 12 IMAGES BY IMAGE CALLBACK\r\n");
            //    System.Console.Write("###################################\r\n\r\n");

        }
            //EVENT HANDLER
           void mDataStream_NewBufferEvent(object sender, BGAPI2.Events.NewBufferEventArgs mDSEvent)
            {
                gImageCounter++;
            System.Console.Write(" new bufferEvent for {0}] ", ((BGAPI2.DataStream)sender).Parent.Model); // device
            System.Console.Write(" [event of {0}] ", ((BGAPI2.DataStream)sender).Parent.Model); // device
                try
                {
                    BGAPI2.Buffer mBufferFilled = null;
                    mBufferFilled = mDSEvent.BufferObj;
                    if (mBufferFilled == null)
                    {
                        System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
                    }
                    else if (mBufferFilled.IsIncomplete == true)
                    {
                        System.Console.WriteLine("Error: Image is incomplete\r\n");
                        // queue buffer again
                        mBufferFilled.QueueBuffer();
                        System.Console.WriteLine("QueueBuffer called\r\n");
                    return;//leave handler
                    }
                    else
                    {
                   
                }



                System.Console.Write(" Image {0, 5:d} received in memory address {1:X}\r\n", mBufferFilled.FrameID, (ulong)mBufferFilled.MemPtr);
                        // queue buffer again

                        mBufferFilled.QueueBuffer();
                  
                        
                    

                    //

                }
                catch (BGAPI2.Exceptions.IException ex)
                {
                    System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                    System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                    System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
                }
                return;
            }



        private void loadMasterDark()
        {
            masterDark = File.ReadAllBytes("masterdark.raw");

        }

         int reversebits(int aNum,bool lsb)
        {
            int newNum = 0;
            if (lsb)
            {
                newNum = Math.Sign(aNum & 0b1000_0000_0000) * (1);
                newNum = newNum + (Math.Sign(aNum & 0b0100_0000_0000) * (2));
                newNum = newNum + (Math.Sign(aNum & 0b0010_0000_0000) * (4));
                newNum = newNum + (Math.Sign(aNum & 0b0001_0000_0000) * (8));
                newNum = newNum + (Math.Sign(aNum & 0b0000_1000_0000) * (16));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0100_0000) * (32));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0010_0000) * (64));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0001_0000) * (128));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_1000) * (256));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_0100) * (512));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_0010) * (1024));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_0001) * (2048));
            }
            else
            {
                newNum = Math.Sign(aNum & 0b1000_0000_0000) * (2048);
                newNum = newNum + (Math.Sign(aNum & 0b0100_0000_0000) * (1024));
                newNum = newNum + (Math.Sign(aNum & 0b0010_0000_0000) * (512));
                newNum = newNum + (Math.Sign(aNum & 0b0001_0000_0000) * (256));
                newNum = newNum + (Math.Sign(aNum & 0b0000_1000_0000) * (128));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0100_0000) * (64));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0010_0000) * (32));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0001_0000) * (16));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_1000) * (8));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_0100) * (4));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_0010) * (2));
                newNum = newNum + (Math.Sign(aNum & 0b0000_0000_0001) * (1));
            }


            return newNum;
        }
        void processImage(BGAPI2.Buffer mBufferFilled)
    {
        //load image into Bitmap
        BGAPI2.Image mTransformImage = null;
        BGAPI2.Image mImage = imgProcessor.CreateImage((uint)mBufferFilled.Width, (uint)mBufferFilled.Height, (string)mBufferFilled.PixelFormat, mBufferFilled.MemPtr, (ulong)mBufferFilled.MemSize);
        System.Console.Write("  mImage.Pixelformat:             {0}\r\n", mImage.PixelFormat);
        System.Console.Write("  mImage.Width:                   {0}\r\n", mImage.Width);
        System.Console.Write("  mImage.Height:                  {0}\r\n", mImage.Height);
        System.Console.Write("  mImage.Buffer:                  {0:X8}\r\n", (ulong)mImage.Buffer);

        double fBytesPerPixel = (double)mImage.NodeList["PixelFormatBytes"].Value;
        System.Console.Write("  Bytes per image:                {0}\r\n", (long)((uint)mImage.Width * (uint)mImage.Height * fBytesPerPixel));
        System.Console.Write("  Bytes per pixel:                {0}\r\n", fBytesPerPixel);

            //COPY UNMANAGED IMAGEBUFFER TO A MANAGED BYTE ARRAY
            imageBufferCopy = new byte[(uint)((uint)mImage.Width * (uint)mImage.Height * fBytesPerPixel)];
            Marshal.Copy(mImage.Buffer, imageBufferCopy, 0, (int)((int)mImage.Width * (int)mImage.Height * fBytesPerPixel));
            //apply darks:
            //if (useDarks)
            // {
            int pixel1, pixel2, pixel3, pixel4;
            int dpixel1, dpixel2, dpixel3, dpixel4;
            byte byte1, byte2, byte3, byte4, byte5, byte6;
            byte dbyte1, dbyte2, dbyte3, dbyte4, dbyte5, dbyte6;
            string filename;

            filename = String.Format("{0}{1:ddMMMyyyy-HHmmss}.raw", "dark_", DateTime.Now);
          // File.WriteAllBytes(filename,imageBufferCopy);
            for (int k = 0; k < imageBufferCopy.Length-1; k=k+1)
            {
                //unpack 2 pixels in 3 bytes
                byte1 = imageBufferCopy[k];
               // byte2 = imageBufferCopy[k+1]; 
               // byte3 = imageBufferCopy[k+2];


                dbyte1 = masterDark[k];
                //  dbyte2 = masterDark[k + 1];
                //  dbyte3 = masterDark[k + 2];


                //  pixel1 = (byte1 << 4) | ((byte2 & 0b1111_0000) >> 4);
                //  pixel2 =  ((byte2 & 0b0000_1111) << 8 ) | byte3;


                //  dpixel1 = (dbyte1 << 4) | ((dbyte2 & 0b1111_0000) >> 4);
                //  dpixel2 = ((dbyte2 & 0b0000_1111) << 8) | dbyte3;

                //pixel1 = reversebits(pixel1,true);
                //pixel2 = reversebits(pixel2,true);
                //dpixel1 = reversebits(dpixel1,true);
                //dpixel2 = reversebits(dpixel2,true);
                pixel1 = (byte1 & 0b1111_0000) >> 4;
                dpixel1 = (dbyte1 & 0b1111_0000) >> 4;
                pixel2 = (byte1 & 0b0000_1111);
                dpixel2 = (dbyte1 & 0b0000_1111);
                if (useDarks)
                {

                    //pixel1 = Math.Min(pixel1 + 50, 4095);
                   // if (dpixel1 > pixelCutOff) { 
                    pixel1 = Math.Max(pixel1 - dpixel1, 0);
                  //  }

                 //   if (dpixel2 > pixelCutOff)
                 //   {
                        pixel2 = Math.Max(pixel2 - dpixel2, 0);
                 //   }
                }
                //pixel1 = reversebits(pixel1,true);
                //pixel2 = reversebits(pixel2,true);

                byte1 = (byte)(pixel1 << 4);
                byte1 = (byte)(byte1 + (pixel2));
               

                imageBufferCopy[k] = byte1;



                //if ((masterDark[k]) > 250)
                //    imageBufferCopy[k] = (byte)Math.Max(0, imageBufferCopy[k] - 0.75 * (masterDark[k]));

            }
            //copy back to buffer
            Marshal.Copy( imageBufferCopy,0, mImage.Buffer,  (int)((int)mImage.Width * (int)mImage.Height * fBytesPerPixel));
        
            // }


            ulong imageBufferAddress = (ulong)mImage.Buffer;
        mTransformImage = imgProcessor.CreateTransformedImage(mImage, "BGR8");
        System.Console.Write(" Image {0, 5:d} transformed to BGR8\r\n", mBufferFilled.FrameID);
        System.Console.Write("  mTransformImage.Pixelformat:    {0}\r\n", mTransformImage.PixelFormat);
        System.Console.Write("  mTransformImage.Width:          {0}\r\n", mTransformImage.Width);
        System.Console.Write("  mTransformImage.Height:         {0}\r\n", mTransformImage.Height);
        System.Console.Write("  mTransformImage.Buffer:         {0:X8}\r\n", (ulong)mTransformImage.Buffer);
        System.Console.Write("  Bytes per image:                {0}\r\n", (long)((uint)mTransformImage.Width * (uint)mTransformImage.Height * 3.0));
        System.Console.Write("  Bytes per pixel:                {0}\r\n", 3.0);



        transformImageBufferCopy = new byte[(uint)((uint)mTransformImage.Width * (uint)mTransformImage.Height * 3.0)];
        Marshal.Copy(mTransformImage.Buffer, transformImageBufferCopy, 0, (int)((int)mTransformImage.Width * (int)mTransformImage.Height * 3.0));
        ulong transformImageBufferAddress = (ulong)mTransformImage.Buffer;

        // display first 6 pixel values of first 6 lines of the transformed image
        //========================================================================
        System.Console.Write("  Address    B  G  R  B  G  R  B  G  R  B  G  R  B  G  R  B  G  R\r\n");
        for (int j = 0; j < 6; j++) // first 6 lines
        {
            transformImageBufferAddress = (ulong)mTransformImage.Buffer + (ulong)((int)mTransformImage.Width * (int)j * 3.0);
            System.Console.Write("  {0:X8} ", transformImageBufferAddress);
            for (int k = 0; k < 6; k++) // bytes of first 6 pixels of that line
            {
                System.Console.Write(" {0:X2}", transformImageBufferCopy[(uint)mTransformImage.Width * 3 * j + k * 3 + 0]); // Blue value of pixel
                System.Console.Write(" {0:X2}", transformImageBufferCopy[(uint)mTransformImage.Width * 3 * j + k * 3 + 1]); // Green value of pixel
                System.Console.Write(" {0:X2}", transformImageBufferCopy[(uint)mTransformImage.Width * 3 * j + k * 3 + 2]); // Red value of pixel
            }
            System.Console.Write(" ...\r\n");
        }
        System.Console.Write(" \r\n");
        //copy into bitmap
        Rectangle rect = new Rectangle(0, 0, (int)mTransformImage.Width, (int)mTransformImage.Height);
        b = new Bitmap((int)mTransformImage.Width, (int)mTransformImage.Height, PixelFormat.Format24bppRgb);
        System.Drawing.Imaging.BitmapData bmpData = b.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, b.PixelFormat);
        //                            int bytes2 = Math.Abs(bmpData2.Stride) * dark.Height;
        //                            byte[] camRGBValues = new byte[bytes2];
        IntPtr ptr = bmpData.Scan0;
        System.Runtime.InteropServices.Marshal.Copy(transformImageBufferCopy, 0, ptr, transformImageBufferCopy.Length);
        //
        //report image to handler
        //FrameReceivedHandler frameReceivedHandler = this.m_FrameReceivedHandler;
        Console.WriteLine("setup frameReceiveHandler");
        if (null != m_frh && null != b)
        {
            // Report image to user
            m_frh(this, new FrameEventArgs(b));


        }
        //
        if (mImage != null) mImage.Release();
        if (mTransformImage != null) mTransformImage.Release();
        }
    }
}
