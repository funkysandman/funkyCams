/*
  This example describes the FIRST STEPS of handling Baumer-GAPI SDK.
  The given source code applies to handling one system, one camera and twelfe images.
  Please see "Baumer-GAPI SDK Programmer's Guide" chapter 5.1 and chapter 5.2
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace _001_ImageCaptureMode_Polling
{
    class Program
    {
        static int Main(string[] args)
        {
            //DECLARATIONS OF VARIABLES
            BGAPI2.SystemList systemList = null;
            BGAPI2.System mSystem = null;
            string sSystemID = "";

            BGAPI2.InterfaceList interfaceList = null;
            BGAPI2.Interface mInterface = null;
            string sInterfaceID = "";

            BGAPI2.DeviceList deviceList = null;
            BGAPI2.Device mDevice = null;
            string sDeviceID = "";

            BGAPI2.DataStreamList datastreamList = null;
            BGAPI2.DataStream mDataStream = null;
            string sDataStreamID = "";

            BGAPI2.BufferList bufferList = null;
            BGAPI2.Buffer mBuffer = null;
            int returnCode = 0;

            System.Console.Write("\r\n");
            System.Console.Write("##############################################################\r\n");
            System.Console.Write("# PROGRAMMER'S GUIDE Example 001_ImageCaptureMode_Polling.cs #\r\n");
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
                returnCode = (0 == returnCode) ? 1 : returnCode;
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
                                        ifc_pair.Value.Close();
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
                Console.Read();
                return returnCode;
            }
            else
            {
                mSystem = systemList[sSystemID];
            }


            if (sInterfaceID == "")
            {
                System.Console.Write(" No Interface of TLType 'GEV' found \r\n");
                System.Console.Write("\r\nEnd\r\nInput any number to close the program:\r\n");
                Console.Read();
                mSystem.Close();
                return returnCode;
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
                deviceList = mInterface.Devices;
                deviceList.Refresh(100);
                System.Console.Write("5.1.6   Detected devices:         {0}\r\n", deviceList.Count);

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
                Console.Read();
                mInterface.Close();
                mSystem.Close();
                return returnCode;
            }
            else
            {
                mDevice = deviceList[sDeviceID];
            }


            System.Console.Write("DEVICE PARAMETER SETUP\r\n");
            System.Console.Write("######################\r\n\r\n");

            try
            {
                //SET TRIGGER MODE OFF (FreeRun)
                mDevice.RemoteNodeList["TriggerMode"].Value = "Off";
                System.Console.Write("         TriggerMode:             {0}\r\n", (string)mDevice.RemoteNodeList["TriggerMode"].Value);
                System.Console.Write("  \r\n");
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }


            System.Console.Write("DATA STREAM LIST\r\n");
            System.Console.Write("################\r\n\r\n");

            try
            {
                //COUNTING AVAILABLE DATASTREAMS
                datastreamList = mDevice.DataStreams;
                datastreamList.Refresh();
                System.Console.Write("5.1.8   Detected datastreams:     {0}\r\n", datastreamList.Count);

                //DATASTREAM INFORMATION BEFORE OPENING
                foreach (KeyValuePair<string, BGAPI2.DataStream> dst_pair in datastreamList)
                {
                    System.Console.Write("  5.2.4   DataStream ID:          {0}\r\n\r\n", dst_pair.Key);
                }
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }


            System.Console.Write("DATA STREAM\r\n");
            System.Console.Write("###########\r\n\r\n");

            //OPEN THE FIRST DATASTREAM IN THE LIST
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

            if (sDataStreamID == "")
            {
                System.Console.Write(" No DataStream found \r\n");
                System.Console.Write("\r\nEnd\r\nInput any number to close the program:\r\n");
                Console.Read();
                mDevice.Close();
                mInterface.Close();
                mSystem.Close();
                return returnCode;
            }
            else
            {
                mDataStream = datastreamList[sDataStreamID];
            }


            System.Console.Write("BUFFER LIST\r\n");
            System.Console.Write("###########\r\n\r\n");

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
            }

            //CAPTURE 12 IMAGES
            System.Console.Write("\r\n");
            System.Console.Write("CAPTURE 12 IMAGES BY IMAGE POLLING\r\n");
            System.Console.Write("##################################\r\n\r\n");

            BGAPI2.Buffer mBufferFilled = null;
            try
            {
                for (int i = 0; i < 12; i++)
                {
                    mBufferFilled = mDataStream.GetFilledBuffer(1000); // image polling timeout 1000 msec
                    if (mBufferFilled == null)
                    {
                        System.Console.Write("Error: Buffer Timeout after 1000 msec\r\n");
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


            System.Console.Write("CAMERA STOP\r\n");
            System.Console.Write("###########\r\n\r\n");

            //STOP CAMERA
            try
            {
                if (mDevice.RemoteNodeList.GetNodePresent("AcquisitionAbort") == true)
                {
                    mDevice.RemoteNodeList["AcquisitionAbort"].Execute();
                    System.Console.Write("5.1.12   {0} aborted\r\n", mDevice.Model);
                }

                mDevice.RemoteNodeList["AcquisitionStop"].Execute();
                System.Console.Write("5.1.12   {0} stopped \r\n", mDevice.Model);
                System.Console.Write("\r\n");

                String sExposureNodeName = "";
                if (mDevice.GetRemoteNodeList().GetNodePresent("ExposureTime"))
                {
                    sExposureNodeName = "ExposureTime";
                }
                else if (mDevice.GetRemoteNodeList().GetNodePresent("ExposureTimeAbs"))
                {
                    sExposureNodeName = "ExposureTimeAbs";
                }
                System.Console.Write("         ExposureTime:                   {0} [{1}]\r\n", (double)mDevice.RemoteNodeList[sExposureNodeName].Value, (string)mDevice.RemoteNodeList[sExposureNodeName].Unit);
                if (mDevice.TLType == "GEV")
                {
                    if (mDevice.RemoteNodeList.GetNodePresent("DeviceStreamChannelPacketSize") == true)
                        System.Console.Write("         DeviceStreamChannelPacketSize:  {0} [bytes]\r\n", (long)mDevice.RemoteNodeList["DeviceStreamChannelPacketSize"].Value);
                    else
                        System.Console.Write("         GevSCPSPacketSize:              {0} [bytes]\r\n", (long)mDevice.RemoteNodeList["GevSCPSPacketSize"].Value);
                    System.Console.Write("         GevSCPD (PacketDelay):          {0} [tics]\r\n", (long)mDevice.RemoteNodeList["GevSCPD"].Value);
                }
                System.Console.Write("\r\n");
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }


            //STOP DataStream acquisition & release buffers
            try
            {
                if (mDataStream.TLType == "GEV")
                {
                    //DataStream Statistics
                    System.Console.Write("         DataStream Statistics \r\n");
                    System.Console.Write("           DataBlockComplete:              {0}\r\n", (long)mDataStream.NodeList["DataBlockComplete"].Value);
                    System.Console.Write("           DataBlockInComplete:            {0}\r\n", (long)mDataStream.NodeList["DataBlockInComplete"].Value);
                    System.Console.Write("           DataBlockMissing:               {0}\r\n", (long)mDataStream.NodeList["DataBlockMissing"].Value);
                    System.Console.Write("           PacketResendRequestSingle:      {0}\r\n", (long)mDataStream.NodeList["PacketResendRequestSingle"].Value);
                    System.Console.Write("           PacketResendRequestRange:       {0}\r\n", (long)mDataStream.NodeList["PacketResendRequestRange"].Value);
                    System.Console.Write("           PacketResendReceive:            {0}\r\n", (long)mDataStream.NodeList["PacketResendReceive"].Value);
                    System.Console.Write("           DataBlockDroppedBufferUnderrun: {0}\r\n", (long)mDataStream.NodeList["DataBlockDroppedBufferUnderrun"].Value);
                    System.Console.Write("           Bitrate:                        {0}\r\n", (double)mDataStream.NodeList["Bitrate"].Value);
                    System.Console.Write("           Throughput:                     {0}\r\n", (double)mDataStream.NodeList["Throughput"].Value);
                    System.Console.Write("\r\n");
                }
                if (mDataStream.TLType == "U3V")
                {
                    //DataStream Statistics
                    System.Console.Write("         DataStream Statistics \r\n");
                    System.Console.Write("           GoodFrames:            {0}\r\n", (long)mDataStream.NodeList["GoodFrames"].Value);
                    System.Console.Write("           CorruptedFrames:       {0}\r\n", (long)mDataStream.NodeList["CorruptedFrames"].Value);
                    System.Console.Write("           LostFrames:            {0}\r\n", (long)mDataStream.NodeList["LostFrames"].Value);
                    System.Console.Write("\r\n");
                }
                //BufferList Information
                System.Console.Write("         BufferList Information \r\n");
                System.Console.Write("           DeliveredCount:        {0}\r\n", (long)bufferList.DeliveredCount);
                System.Console.Write("           UnderrunCount:         {0}\r\n", (long)bufferList.UnderrunCount);
                System.Console.Write("\r\n");

                mDataStream.StopAcquisition();
                System.Console.Write("5.1.12   DataStream stopped \r\n");
                bufferList.DiscardAllBuffers();
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }
            System.Console.Write("\r\n");


            System.Console.Write("RELEASE\r\n");
            System.Console.Write("#######\r\n\r\n");

            //Release buffers
            System.Console.Write("5.1.13   Releasing the resources\r\n");
            try
            {
                while (bufferList.Count > 0)
                {
                    mBuffer = (BGAPI2.Buffer)bufferList.Values.First();
                    bufferList.RevokeBuffer(mBuffer);
                }
                System.Console.Write("         buffers after revoke:    {0}\r\n", bufferList.Count);

                mDataStream.Close();
                mDevice.Close();
                mInterface.Close();
                mSystem.Close();
            }
            catch (BGAPI2.Exceptions.IException ex)
            {
                returnCode = (0 == returnCode) ? 1 : returnCode;
                System.Console.Write("ExceptionType:    {0} \r\n", ex.GetType());
                System.Console.Write("ErrorDescription: {0} \r\n", ex.GetErrorDescription());
                System.Console.Write("in function:      {0} \r\n", ex.GetFunctionName());
            }

            System.Console.Write("\r\nEnd\r\n\r\n");
            System.Console.Write("Input any number to close the program:\r\n");
            Console.Read();
            return returnCode;
        }
    }
}
