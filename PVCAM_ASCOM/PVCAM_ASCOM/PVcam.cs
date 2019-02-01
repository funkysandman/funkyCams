using System;
using System.Collections.Generic;
using System.Text;

namespace Photometrics
{
    namespace Pvcam
    {
        public static class PVCAM
        {
            private static bool m_is64bit;

            /// <summary>
            /// A static constructor is called automatically to initialize the class before the first instance
            /// is created or any static members are referenced.
            /// </summary>
            static PVCAM()
            {
                if (IntPtr.Size == 4)
                {
                    m_is64bit = false;
                }
                else if (IntPtr.Size == 8)
                {
                    m_is64bit = true;
                }
                else
                {
                    throw new NotImplementedException( "Unsupported bitness" );
                }
            }


            #region PVCAM CLASS 0

            public static bool pl_pvcam_init()
            {
                if (m_is64bit)
                    return PVCAM64.pl_pvcam_init();
                else
                    return PVCAM32.pl_pvcam_init();
            }

            public static bool pl_pvcam_uninit()
            {
                if (m_is64bit)
                    return PVCAM64.pl_pvcam_uninit();
                else
                    return PVCAM32.pl_pvcam_uninit();
            }

            public static bool pl_pvcam_get_ver(out UInt16 version)
            {
                if (m_is64bit)
                    return PVCAM64.pl_pvcam_get_ver(out version);
                else
                    return PVCAM32.pl_pvcam_get_ver(out version);
            }

            public static bool pl_cam_close(Int16 hCam)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_close(hCam);
                else
                    return PVCAM32.pl_cam_close(hCam);
            }

            public static bool pl_cam_get_name(Int16 cameraNumber, StringBuilder cameraName)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_get_name(cameraNumber, cameraName);
                else
                    return PVCAM32.pl_cam_get_name(cameraNumber, cameraName);
            }

            public static bool pl_cam_get_total(out Int16 hCam)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_get_total(out hCam);
                else
                    return PVCAM32.pl_cam_get_total(out hCam);
            }

            public static bool pl_cam_open(StringBuilder cameraName, out Int16 hCam, PvTypes.CameraOpenMode o_mode)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_open(cameraName, out hCam, o_mode);
                else
                    return PVCAM32.pl_cam_open(cameraName, out hCam, o_mode);
            }

            [Obsolete("This function has been depricated now, use pl_cam_register_callback_ex3 instead")]
            public static bool pl_cam_register_callback(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                        PvTypes.PMCallBack callback)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_register_callback(hCam, callBackEvent, callback);
                else
                    return PVCAM32.pl_cam_register_callback(hCam, callBackEvent, callback);
            }

            [Obsolete("This function has been depricated now, use pl_cam_register_callback_ex3 instead")]
            public static bool pl_cam_register_callback_ex(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                           PvTypes.PMCallBack_Ex callback, IntPtr context)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_register_callback_ex(hCam, callBackEvent, callback, context);
                else
                    return PVCAM32.pl_cam_register_callback_ex(hCam, callBackEvent, callback, context);
            }

            [Obsolete("This function has been depricated now, use pl_cam_register_callback_ex3 instead")]
            public static bool pl_cam_register_callback_ex2(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                            PvTypes.PMCallBack_Ex2 callback)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_register_callback_ex2(hCam, callBackEvent, callback);
                else
                    return PVCAM32.pl_cam_register_callback_ex2(hCam, callBackEvent, callback);
            }

            public static bool pl_cam_register_callback_ex3(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                            PvTypes.PMCallBack_Ex3 callback, IntPtr context)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_register_callback_ex3(hCam, callBackEvent, callback, context);
                else
                    return PVCAM32.pl_cam_register_callback_ex3(hCam, callBackEvent, callback, context);
            }

            public static bool pl_cam_deregister_callback(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent)
            {
                if (m_is64bit)
                    return PVCAM64.pl_cam_deregister_callback(hCam, callBackEvent);
                else
                    return PVCAM32.pl_cam_deregister_callback(hCam, callBackEvent);
            }

            #endregion

            #region PVCAM CLASS 1

            public static Int16 pl_error_code()
            {
                if (m_is64bit)
                    return PVCAM64.pl_error_code();
                else
                    return PVCAM32.pl_error_code();
            }

            public static bool pl_error_message(Int16 err_code, StringBuilder msg)
            {
                if (m_is64bit)
                    return PVCAM64.pl_error_message(err_code, msg);
                else
                    return PVCAM32.pl_error_message(err_code, msg);
            }

            #endregion

            #region PVCAM CLASS 2

            public static bool pl_get_param(Int16 hcam, UInt32 param_id, Int16 param_attrib, IntPtr param_value)
            {
                if (m_is64bit)
                    return PVCAM64.pl_get_param(hcam, param_id, param_attrib, param_value);
                else
                    return PVCAM32.pl_get_param(hcam, param_id, param_attrib, param_value);
            }

            public static bool pl_set_param(Int16 hcam, UInt32 param_id, IntPtr param_value)
            {
                if (m_is64bit)
                    return PVCAM64.pl_set_param(hcam, param_id, param_value);
                else
                    return PVCAM32.pl_set_param(hcam, param_id, param_value);
            }

            public static bool pl_get_enum_param(Int16 hcam, UInt32 param_id, UInt32 index, out Int32 value,
                                                 StringBuilder desc, UInt32 length)
            {
                if (m_is64bit)
                    return PVCAM64.pl_get_enum_param(hcam, param_id, index, out value, desc, length);
                else
                    return PVCAM32.pl_get_enum_param(hcam, param_id, index, out value, desc, length);
            }

            public static bool pl_enum_str_length(Int16 hcam, UInt32 param_id, UInt32 index, out UInt32 length)
            {
                if (m_is64bit)
                    return PVCAM64.pl_enum_str_length(hcam, param_id, index, out length);
                else
                    return PVCAM32.pl_enum_str_length(hcam, param_id, index, out length);
            }

            public static bool pl_pp_reset(Int16 hcam)
            {
                if (m_is64bit)
                    return PVCAM64.pl_pp_reset(hcam);
                else
                    return PVCAM32.pl_pp_reset(hcam);
            }

            public static bool pl_create_smart_stream_struct(out PvTypes.smart_stream_type smtStreamStruct,
                                                             UInt16 entries)
            {
                if (m_is64bit)
                    return PVCAM64.pl_create_smart_stream_struct(out smtStreamStruct, entries);
                else
                    return PVCAM32.pl_create_smart_stream_struct(out smtStreamStruct, entries);
            }

           public static bool pl_release_smart_stream_struct(out PvTypes.smart_stream_type smtStreamStruct)
            {
                if (m_is64bit)
                    return PVCAM64.pl_release_smart_stream_struct(out smtStreamStruct);
                else
                    return PVCAM32.pl_release_smart_stream_struct(out smtStreamStruct);
            }

            public static bool pl_create_frame_info_struct(out PvTypes.FRAME_INFO new_frame)
            {
                if (m_is64bit)
                    return PVCAM64.pl_create_frame_info_struct(out new_frame);
                else
                    return PVCAM32.pl_create_frame_info_struct(out new_frame);
            }

            public static bool pl_release_frame_info_struct(PvTypes.FRAME_INFO frame_to_delete)
            {
                if (m_is64bit)
                    return PVCAM64.pl_release_frame_info_struct(frame_to_delete);
                else
                    return PVCAM32.pl_release_frame_info_struct(frame_to_delete);
            }

            #endregion

            #region PVCAM CLASS 3

            [Obsolete("This function has been deprecated now, no need to use this anymore")]
            public static bool pl_exp_init_seq()
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_init_seq();
                else
                    return PVCAM32.pl_exp_init_seq();
            }

            public static bool pl_exp_setup_seq(Int16 hcam, UInt16 exp_total, UInt16 rgn_total,
                                                ref PvTypes.RegionType rgn_array, Int16 mode,
                                                UInt32 exposure_time, out UInt32 stream_size)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_setup_seq(hcam, exp_total, rgn_total, ref rgn_array,
                                                    mode, exposure_time, out stream_size);
                else
                    return PVCAM32.pl_exp_setup_seq(hcam, exp_total, rgn_total, ref rgn_array,
                                                    mode, exposure_time, out stream_size);
            }

            public static bool pl_exp_start_seq(Int16 hcam, IntPtr pixel_stream_ptr)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_start_seq(hcam, pixel_stream_ptr);
                else
                    return PVCAM32.pl_exp_start_seq(hcam, pixel_stream_ptr);
            }

            public static bool pl_exp_check_status(Int16 hcam, out Int16 status, out UInt32 byte_cnt)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_check_status(hcam, out status, out byte_cnt);
                else
                    return PVCAM32.pl_exp_check_status(hcam, out status, out byte_cnt);
            }

            [Obsolete("This function has been deprecated now,no need to use this anymore")]
            public static bool pl_exp_uninit_seq()
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_uninit_seq();
                else
                    return PVCAM32.pl_exp_uninit_seq();
            }

            public static bool pl_exp_finish_seq(Int16 hcam, IntPtr pixel_stream_ptr, Int16 hbuf)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_finish_seq(hcam, pixel_stream_ptr, hbuf);
                else
                    return PVCAM32.pl_exp_finish_seq(hcam, pixel_stream_ptr, hbuf);
            }

            public static bool pl_exp_abort(Int16 hcam, Int16 cam_state)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_abort(hcam, cam_state);
                else
                    return PVCAM32.pl_exp_abort(hcam, cam_state);
            }

            public static bool pl_exp_start_cont(Int16 hcam, IntPtr pixel_stream, UInt32 size)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_start_cont(hcam, pixel_stream, size);
                else
                    return PVCAM32.pl_exp_start_cont(hcam, pixel_stream, size);
            }

            public static bool pl_exp_setup_cont(Int16 hcam, UInt16 rgn_total, ref PvTypes.RegionType rgn_array,
                                                 Int16 mode, UInt32 exposure_time, out UInt32 stream_size,
                                                 Int16 circ_mode)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_setup_cont(hcam, rgn_total, ref rgn_array, mode, exposure_time,
                                                     out stream_size, circ_mode);
                else
                    return PVCAM32.pl_exp_setup_cont(hcam, rgn_total, ref rgn_array, mode, exposure_time,
                                                     out stream_size, circ_mode);
            }

            public static bool pl_exp_get_latest_frame(Int16 hcam, out IntPtr frame)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_get_latest_frame(hcam, out frame);
                else
                    return PVCAM32.pl_exp_get_latest_frame(hcam, out frame);
            }

            public static bool pl_exp_get_latest_frame_ex(Int16 hcam, out IntPtr frame,
                                                          out PvTypes.FRAME_INFO pFrameInfo)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_get_latest_frame_ex(hcam, out frame, out pFrameInfo);
                else
                    return PVCAM32.pl_exp_get_latest_frame_ex(hcam, out frame, out pFrameInfo);
            }

            public static bool pl_exp_get_oldest_frame(Int16 hcam, out IntPtr frame)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_get_oldest_frame(hcam, out frame);
                else
                    return PVCAM32.pl_exp_get_oldest_frame(hcam, out frame);
            }

            public static bool pl_exp_get_oldest_frame_ex(Int16 hcam, out IntPtr frame,
                                                          out PvTypes.FRAME_INFO pFrameInfo)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_get_oldest_frame_ex(hcam, out frame, out pFrameInfo);
                else
                    return PVCAM32.pl_exp_get_oldest_frame_ex(hcam, out frame, out pFrameInfo);
            }

            public static bool pl_exp_check_cont_status(Int16 hcam, out Int16 status, out UInt32 byte_cnt,
                                                        out UInt32 buffer_cnt)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_check_cont_status(hcam, out status, out byte_cnt, out buffer_cnt);
                else
                    return PVCAM32.pl_exp_check_cont_status(hcam, out status, out byte_cnt, out buffer_cnt);
            }

            public static bool pl_exp_check_cont_status_ex(Int16 hcam, out Int16 status, out UInt32 byte_cnt,
                                                           out UInt32 buffer_cnt, out PvTypes.FRAME_INFO pFrameInfo)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_check_cont_status_ex(hcam, out status, out byte_cnt,
                                                               out buffer_cnt, out pFrameInfo);
                else
                    return PVCAM32.pl_exp_check_cont_status_ex(hcam, out status, out byte_cnt,
                                                               out buffer_cnt, out pFrameInfo);
            }

            public static bool pl_exp_stop_cont(Int16 hcam, Int16 cam_state)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_stop_cont(hcam, cam_state);
                else
                    return PVCAM32.pl_exp_stop_cont(hcam, cam_state);
            }

            public static bool pl_exp_unlock_oldest_frame(Int16 hcam)
            {
                if (m_is64bit)
                    return PVCAM64.pl_exp_unlock_oldest_frame(hcam);
                else
                    return PVCAM32.pl_exp_unlock_oldest_frame(hcam);
            }

            /*********************Frame Metadata methods ********************/

            public static bool pl_md_frame_decode(IntPtr pDstFrame, IntPtr pSrcBuf, UInt32 srcBufSize)
            {
                if (m_is64bit)
                    return PVCAM64.pl_md_frame_decode(pDstFrame, pSrcBuf, srcBufSize);
                else
                    return PVCAM32.pl_md_frame_decode(pDstFrame, pSrcBuf, srcBufSize);
            }

            public static bool pl_md_frame_recompose(IntPtr pDstBuf, UInt16 offX, UInt16 offY,
                                                     UInt16 dstWidth, UInt16 dstHeight, ref PvTypes.MD_Frame pSrcFrame)
            {
                if (m_is64bit)
                    return PVCAM64.pl_md_frame_recompose(pDstBuf, offX, offY, dstWidth, dstHeight,  ref pSrcFrame);
                else
                    return PVCAM32.pl_md_frame_recompose(pDstBuf, offX, offY, dstWidth, dstHeight,  ref pSrcFrame);
            }

            public static bool pl_md_create_frame_struct_cont(ref IntPtr pFrame, UInt16 roiCount)
            {
                if (m_is64bit)
                    return PVCAM64.pl_md_create_frame_struct_cont(ref pFrame, roiCount);
                else
                    return PVCAM32.pl_md_create_frame_struct_cont(ref pFrame, roiCount);
            }

            public static bool pl_md_create_frame_struct(ref IntPtr pFrame, IntPtr pSrcBuf, UInt32 srcBufSize)
            {
                if (m_is64bit)
                    return PVCAM64.pl_md_create_frame_struct(ref pFrame, pSrcBuf, srcBufSize);
                else
                    return PVCAM32.pl_md_create_frame_struct(ref pFrame, pSrcBuf, srcBufSize);
            }

            public static bool pl_md_release_frame_struct(IntPtr pFrame)
            {
                if (m_is64bit)
                    return PVCAM64.pl_md_release_frame_struct(pFrame);
                else
                    return PVCAM32.pl_md_release_frame_struct(pFrame);
            }

            #endregion
        }
    }
}
