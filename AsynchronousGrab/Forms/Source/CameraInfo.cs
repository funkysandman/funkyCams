/*=============================================================================
  Copyright (C) 2014 Allied Vision Technologies.  All Rights Reserved.
  Redistribution of this file, in original or modified form, without
  prior written consent of Allied Vision Technologies is prohibited.
-------------------------------------------------------------------------------
  File:         CameraInfo.cs
  Description:  Implementation file for the CameraInfo class that wraps details
                of a camera.
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

namespace AsynchronousGrab
{
    using System;

    /// <summary>
    /// A simple container class for infos (name and ID) about a camera
    /// </summary>
    public class CameraInfo
    {
        /// <summary>
        /// The camera name 
        /// </summary>
        private string m_Name = null;

        /// <summary>
        /// The camera ID
        /// </summary>
        private string m_ID = null;

        /// <summary>
        /// Initializes a new instance of the CameraInfo class.
        /// </summary>
        /// <param name="name">The camera name</param>
        /// <param name="id">The camera ID</param>
        public CameraInfo(string name, string id)
        {
            if (null == name)
            {
                throw new ArgumentNullException("name");
            }

            if (null == name)
            {
                throw new ArgumentNullException("id");
            }

            this.m_Name = name;
            this.m_ID = id;
        }

        /// <summary>
        /// Gets the name of the camera
        /// </summary>
        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        /// <summary>
        /// Gets the ID
        /// </summary>
        public string ID
        {
            get
            {
                return this.m_ID;
            }
        }

        /// <summary>
        /// Overrides the toString Method for the CameraInfo class (this)
        /// </summary>
        /// <returns>The Name of the camera</returns>
        public override string ToString()
        {
            return this.m_Name;
        }
    }
}