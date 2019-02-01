/*=============================================================================
  Copyright (C) 2012 - 2016 Allied Vision Technologies.  All Rights Reserved.

  Redistribution of this file, in original or modified form, without
  prior written consent of Allied Vision Technologies is prohibited.

-------------------------------------------------------------------------------

  File:        RingBitmap.cs

  Description: Implementation file for the RingBitmap class.
               Contains a configurable ring bitmap array.
               Each bitmap will only be created one time and reused afterwards.

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
    using System.Drawing;
    using System.Drawing.Imaging;
    using AVT.VmbAPINET;
    using System;
    /// <summary>
    /// Helper class to provide necessary bitmap functions
    /// A ring buffer of bitmaps
    /// </summary>
    internal class RingBitmap
    {
        /// <summary>
        /// The bitmap size
        /// </summary>
        private int m_Size = 0;

        /// <summary>
        /// Bitmaps to display images
        /// </summary>
        private Bitmap[] m_Bitmaps = null;

        /// <summary>
        /// selects Bitmap
        /// </summary>
        private int m_BitmapSelector = 0;

        /// <summary>
        /// Initializes a new instance of the RingBitmap class.
        /// </summary>
        /// <param name="size">The bitmap size</param>
        public RingBitmap(int size)
        {
            m_Size = size;
            m_Bitmaps = new Bitmap[m_Size];
        }

        /// <summary>
        /// Gets the current bitmap as image
        /// </summary>
        public Image Image
        {
            get
            {
                return m_Bitmaps[m_BitmapSelector];
            }
        }

        /// <summary>
        /// Fill Frame in 8bppIndexed bitmap
        /// </summary>
        /// <param name="frame">The Vimba frame</param>
        public void FillNextBitmap(Frame frame)
        {
            // switch to Bitmap object which is currently not in use by GUI
            SwitchBitmap();
            try
            {
                if (m_Bitmaps[m_BitmapSelector] is null)
                {
                    m_Bitmaps[m_BitmapSelector] = new Bitmap((int)frame.Width, (int)frame.Height, PixelFormat.Format24bppRgb);
                }
            }

                catch
            {
                Console.WriteLine("error during new Bitmap");
            }
            try { 
            frame.Fill(ref m_Bitmaps[m_BitmapSelector]);
            }
            catch
            {
                Console.WriteLine("error during frame fill");
            }


        }

        /// <summary>
        /// Bitmap rotation selector
        /// </summary>
        private void SwitchBitmap()
        {
            m_BitmapSelector++;

            if (m_Size == m_BitmapSelector)
            {
                m_BitmapSelector = 0;
            }
        }
    }
}