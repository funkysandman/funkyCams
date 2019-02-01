/*=============================================================================
  Copyright (C) 2013 Allied Vision Technologies.  All Rights Reserved.

  Redistribution of this file, in original or modified form, without
  prior written consent of Allied Vision Technologies is prohibited.

-------------------------------------------------------------------------------

  File:        Program.cs

  Description: The main entry point of the AsynchronousGrabConsole example of VimbaNET.

-------------------------------------------------------------------------------

  THIS SOFTWARE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR IMPLIED
  WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF TITLE,
  NON-INFRINGEMENT, MERCHANTABILITY AND FITNESS FOR A PARTICULAR  PURPOSE ARE
  DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT,
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
  AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR
  TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

=============================================================================*/

namespace AsynchronousGrabConsole
{
    using System;
    using System.Collections.Generic;

    using AVT.VmbAPINET;

    /// <summary>
    /// The Program class 
    /// </summary>
    internal static class Program
    {
       /// <summary>
        /// The main entry point for the application.
       /// </summary>
       /// <param name="args">The command line arguments</param>
        private static void Main(string[] args)
        {
            VimbaHelper.FrameInfos showFrameInfos = VimbaHelper.FrameInfos.Off;           // Show frame info's
            bool printhelp = false;                // Output help?
            string cameraID = null;                // The camera ID

            Console.WriteLine();
            Console.WriteLine("///////////////////////////////////////////");
            Console.WriteLine("/// Vimba API Asynchronous Grab Example ///");
            Console.WriteLine("///////////////////////////////////////////");
            Console.WriteLine();

            try
            {
                ParseCommandLine(args, ref showFrameInfos, ref printhelp, ref cameraID);

                // Print out help and end program
                if (printhelp)
                {
                    Console.WriteLine("Usage: AsynchronousGrab [CameraID] [/i] [/h]");
                    Console.WriteLine("Parameters:   CameraID    ID of the camera to use (using first camera if not specified)");
                    Console.WriteLine("              /i          Show frame info's");
                    Console.WriteLine("              /a          Automatically only show frame info's of corrupt frames\n");
                    Console.WriteLine("              /h          Print out help");
                }
                else
                {
                    // Create a new Vimba entry object
                    VimbaHelper vimbaHelper = new VimbaHelper();
                    vimbaHelper.Startup(); // Startup API
                    Console.WriteLine("Vimba .NET API Version {0}",vimbaHelper.GetVersion());

                    // Open camera
                    try
                    {
                        if (null == cameraID)
                        {
                            // Open first available camera

                            // Fetch all cameras known to Vimba
                            List<Camera> cameras = vimbaHelper.CameraList;
                            if (cameras.Count < 0)
                            {
                                throw new Exception("No camera available.");
                            }

                            foreach (Camera currentCamera in cameras)
                            {
                                // Check if we can open the camera in full mode
                                VmbAccessModeType accessMode = currentCamera.PermittedAccess;
                                if (VmbAccessModeType.VmbAccessModeFull == (VmbAccessModeType.VmbAccessModeFull & accessMode))
                                {
                                    // Now get the camera ID
                                    cameraID = currentCamera.Id;
                                    break;
                                }
                            }

                            if (null == cameraID)
                            {
                                throw new Exception("Could not open any camera.");
                            }
                        }

                        Console.WriteLine("Opening camera with ID: " + cameraID);

                        // Start the continuous image acquisition 
                        vimbaHelper.StartContinuousImageAcquisition(cameraID, showFrameInfos);

                        Console.WriteLine("Press <enter> to stop acquisition...");
                        Console.WriteLine("Frame received");
                        Console.ReadKey();

                        // Stop the image acquisition
                        vimbaHelper.StopContinuousImageAcquisition(); 
                        Console.WriteLine("\nAcquisition stopped.");
                    }
                    finally
                    {
                        // shutdown the vimba Api
                        vimbaHelper.Shutdown();   
                    }
                }
            }
            catch (VimbaException ve)
            {
                // Output in case of a vimba Exception 
                Console.WriteLine(ve.Message);
                Console.WriteLine("Return Code: " + ve.ReturnCode.ToString() + " (" + ve.MapReturnCodeToString() + ")");
            }
            catch (Exception e)
            {
                // Output in case of a System.Exception
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Press any Key to exit!");
            Console.ReadKey();
        }

        /// <summary>
        /// Parses the Command Line Arguments
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <param name="showFrameInfos">Flag to show frame information or not</param>
        /// <param name="printhelp">Flag to decide if help information is shown</param>
        /// <param name="cameraID">The camera ID</param>
        private static void ParseCommandLine(string[] args, ref VimbaHelper.FrameInfos showFrameInfos, ref bool printhelp, ref string cameraID)
        {
            // Parse command line
            foreach (string parameter in args)
            {
                if (parameter.Length < 0)
                {
                    throw new ArgumentException("Invalid parameter found.");
                }

                if (parameter.StartsWith("/"))
                {
                    if (string.Compare(parameter, "/i", StringComparison.Ordinal) == 0)
                    {
                        if (showFrameInfos != VimbaHelper.FrameInfos.Off || printhelp)
                        {
                            throw new ArgumentException("Invalid parameter found.");
                        }

                        showFrameInfos = VimbaHelper.FrameInfos.Show;
                    }
                    else
                        if (string.Compare(parameter, "/a", StringComparison.Ordinal) == 0)
                        {
                            if (showFrameInfos != VimbaHelper.FrameInfos.Off || printhelp)
                            {
                                throw new ArgumentException("Invalid parameter found.");
                            }

                            showFrameInfos = VimbaHelper.FrameInfos.Automatic;
                        }
                        else
                            if (string.Compare(parameter, "/h", StringComparison.Ordinal) == 0)
                            {
                                if (null != cameraID || printhelp || showFrameInfos != VimbaHelper.FrameInfos.Off)
                                {
                                    throw new ArgumentException("Invalid parameter found.");
                                }

                                printhelp = true;
                            }
                            else
                                {
                                    throw new ArgumentException("Invalid parameter found.");
                                }
                }
                else
                {
                    if (null != cameraID)
                    {
                        throw new ArgumentException("Invalid parameter found.");
                    }

                    cameraID = parameter;
                }
            }
        }
    }
}