using System;
using System.Runtime.InteropServices;
using Photometrics.Pvcam;

namespace Photometrics
{
    namespace Pvcam
    {
        public static partial class PvTypes
        {
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void PMCallBack();

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void PMCallBack_Ex(IntPtr context);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void PMCallBack_Ex2(ref FRAME_INFO frameInfo);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void PMCallBack_Ex3(ref FRAME_INFO frameInfo, IntPtr context);

            public enum ReturnValue
            {
                PV_FAIL,
                PV_OK
            }

            public const Boolean BIG_ENDIAN = false; /* TRUE for Motorola byte order, FALSE for Intel */
        }
    }
}
