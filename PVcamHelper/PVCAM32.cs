using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Photometrics
{
    namespace Pvcam
    {
        internal static class PVCAM32
        {

            #region PVCAM CLASS 0

            [DllImport("Pvcam32.dll", EntryPoint = "pl_pvcam_init",
            ExactSpelling = false)]
            public static extern bool pl_pvcam_init();

            [DllImport("Pvcam32.dll", EntryPoint = "pl_pvcam_uninit",
            ExactSpelling = false)]
            public static extern bool pl_pvcam_uninit();

            [DllImport("Pvcam32.dll", EntryPoint = "pl_pvcam_get_ver",
            ExactSpelling = false)]
            public static extern bool pl_pvcam_get_ver(out UInt16 version);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_close",
            ExactSpelling = false)]
            public static extern bool pl_cam_close(Int16 hCam);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_get_name", CharSet = CharSet.Ansi,
            ExactSpelling = false)]
            public static extern bool pl_cam_get_name(Int16 cameraNumber, StringBuilder cameraName);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_get_total",
            ExactSpelling = false)]
            public static extern bool pl_cam_get_total(out Int16 hCam);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_open",
            ExactSpelling = false)]
            public static extern bool pl_cam_open(StringBuilder cameraName, out Int16 hCam,
                                                  PvTypes.CameraOpenMode o_mode);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_register_callback",
            ExactSpelling = false)]
            public static extern bool pl_cam_register_callback(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                               PvTypes.PMCallBack callback);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_register_callback_ex",
            ExactSpelling = false)]
            public static extern bool pl_cam_register_callback_ex(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                                  PvTypes.PMCallBack_Ex callback, IntPtr context);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_register_callback_ex2",
            ExactSpelling = false)]
            public static extern bool pl_cam_register_callback_ex2(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                                   PvTypes.PMCallBack_Ex2 callback);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_register_callback_ex3",
            ExactSpelling = false)]
            public static extern bool pl_cam_register_callback_ex3(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent,
                                                                   PvTypes.PMCallBack_Ex3 callback, IntPtr context);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_cam_deregister_callback",
            ExactSpelling = false)]
            public static extern bool pl_cam_deregister_callback(Int16 hCam, PvTypes.PL_CALLBACK_EVENT callBackEvent);

            #endregion

            #region PVCAM CLASS 1

            [DllImport("Pvcam32.dll", EntryPoint = "pl_error_code",
            ExactSpelling = false)]
            public static extern Int16 pl_error_code();

            [DllImport("Pvcam32.dll", EntryPoint = "pl_error_message",
            ExactSpelling = false)]
            public static extern bool pl_error_message(Int16 err_code, StringBuilder msg);

            #endregion

            #region PVCAM CLASS 2

            [DllImport("Pvcam32.dll", EntryPoint = "pl_get_param",
            ExactSpelling = false)]
            public static extern bool pl_get_param(Int16 hcam, UInt32 param_id,
                                                   Int16 param_attrib, IntPtr param_value);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_set_param",
            ExactSpelling = false)]
            public static extern bool pl_set_param(Int16 hcam, UInt32 param_id,
                                                   IntPtr param_value);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_get_enum_param",
            ExactSpelling = false)]
            public static extern bool pl_get_enum_param(Int16 hcam, UInt32 param_id,
                                                        UInt32 index, out Int32 value,
                                                        StringBuilder desc, UInt32 length);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_enum_str_length",
            ExactSpelling = false)]
            public static extern bool pl_enum_str_length(Int16 hcam, UInt32 param_id,
                                                         UInt32 index, out UInt32 length);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_pp_reset",
            ExactSpelling = false)]
            public static extern bool pl_pp_reset(Int16 hcam);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_create_smart_stream_struct",
            ExactSpelling = false)]
            public static extern bool pl_create_smart_stream_struct(out PvTypes.smart_stream_type smtStreamStruct,
                                                                    UInt16 entries);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_release_smart_stream_struct",
            ExactSpelling = false)]
            public static extern bool pl_release_smart_stream_struct(out PvTypes.smart_stream_type smtStreamStruct);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_create_frame_info_struct",
            ExactSpelling = false)]
            public static extern bool pl_create_frame_info_struct(out PvTypes.FRAME_INFO new_frame);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_release_frame_info_struct",
            ExactSpelling = false)]
            public static extern bool pl_release_frame_info_struct(PvTypes.FRAME_INFO frame_to_delete);

            #endregion

            #region PVCAM CLASS 3

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_init_seq",
            ExactSpelling = false)]
            public static extern bool pl_exp_init_seq();

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_setup_seq",
            ExactSpelling = false)]
            public static extern bool pl_exp_setup_seq(Int16 hcam, UInt16 exp_total,
                                                       UInt16 rgn_total, ref PvTypes.RegionType rgn_array,
                                                       Int16 mode, UInt32 exposure_time,
                                                       out UInt32 stream_size);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_start_seq",
            ExactSpelling = false)]
            public static extern bool pl_exp_start_seq(Int16 hcam, IntPtr pixel_stream_ptr);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_check_status",
            ExactSpelling = false)]
            public static extern bool pl_exp_check_status(Int16 hcam, out Int16 status,
                                                          out UInt32 byte_cnt);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_finish_seq",
            ExactSpelling = false)]
            public static extern bool pl_exp_finish_seq(Int16 hcam, IntPtr pixel_stream_ptr, Int16 hbuf);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_uninit_seq",
            ExactSpelling = false)]
            public static extern bool pl_exp_uninit_seq();

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_abort",
            ExactSpelling = false)]
            public static extern bool pl_exp_abort(Int16 hcam, Int16 cam_state);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_start_cont",
            ExactSpelling = false)]
            public static extern bool pl_exp_start_cont(Int16 hcam, IntPtr pixel_stream, UInt32 size);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_setup_cont",
            ExactSpelling = false)]
            public static extern bool pl_exp_setup_cont(Int16 hcam, UInt16 rgn_total,
                                                        ref PvTypes.RegionType rgn_array,
                                                        Int16 mode, UInt32 exposure_time,
                                                        out UInt32 stream_size, Int16 circ_mode);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_get_latest_frame",
            ExactSpelling = false)]
            public static extern bool pl_exp_get_latest_frame(Int16 hcam, out IntPtr frame);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_get_latest_frame_ex",
            ExactSpelling = false)]
            public static extern bool pl_exp_get_latest_frame_ex(Int16 hcam, out IntPtr frame,
                                                                 out PvTypes.FRAME_INFO pFrameInfo);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_get_oldest_frame",
            ExactSpelling = false)]
            public static extern bool pl_exp_get_oldest_frame(Int16 hcam, out IntPtr frame);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_get_oldest_frame_ex",
            ExactSpelling = false)]
            public static extern bool pl_exp_get_oldest_frame_ex(Int16 hcam, out IntPtr frame,
                                                                 out PvTypes.FRAME_INFO pFrameInfo);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_check_cont_status",
            ExactSpelling = false)]
            public static extern bool pl_exp_check_cont_status(Int16 hcam, out Int16 status,
                                                               out UInt32 byte_cnt, out UInt32 buffer_cnt);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_check_cont_status_ex",
            ExactSpelling = false)]
            public static extern bool pl_exp_check_cont_status_ex(Int16 hcam, out Int16 status,
                                                                  out UInt32 byte_cnt, out UInt32 buffer_cnt,
                                                                  out PvTypes.FRAME_INFO pFrameInfo);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_stop_cont",
            ExactSpelling = false)]
            public static extern bool pl_exp_stop_cont(Int16 hcam, Int16 cam_state);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_exp_unlock_oldest_frame",
            ExactSpelling = false)]
            public static extern bool pl_exp_unlock_oldest_frame(Int16 hcam);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_md_frame_decode",
            ExactSpelling = false)]
            public static extern bool pl_md_frame_decode(IntPtr pDstFrame, IntPtr pSrcBuf, UInt32 srcBufSize);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_md_frame_recompose",
            ExactSpelling = false)]
            public static extern bool pl_md_frame_recompose(IntPtr pDstBuf, UInt16 offX, UInt16 offY,
                                                            UInt16 dstWidth, UInt16 dstHeight,
                                                            ref PvTypes.MD_Frame pSrcFrame);
           
            [DllImport("Pvcam32.dll", EntryPoint = "pl_md_create_frame_struct_cont",
            ExactSpelling = false)]
            public static extern bool pl_md_create_frame_struct_cont(ref IntPtr pFrame, UInt16 roiCount);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_md_create_frame_struct",
            ExactSpelling = false)]
            public static extern bool pl_md_create_frame_struct(ref IntPtr pFrame, IntPtr pSrcBuf, UInt32 srcBufSize);

            [DllImport("Pvcam32.dll", EntryPoint = "pl_md_release_frame_struct",
            ExactSpelling = false)]
            public static extern bool pl_md_release_frame_struct(IntPtr pFrame);

            #endregion

        }
    }
}
