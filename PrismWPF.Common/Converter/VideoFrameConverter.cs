using System;
using System.Runtime.InteropServices;
using System.Windows;

using FFmpeg.AutoGen;

namespace DynamicMonitoring.Common.Converter
{
    /// <summary>
    /// 비디오 프레임 컨버터
    /// </summary>
    public sealed unsafe class VideoFrameConverter : IDisposable
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////// Field
        ////////////////////////////////////////////////////////////////////////////////////////// Private

        #region Field

        /// <summary>
        /// 타겟 크기
        /// </summary>
        private readonly Size targetSize;

        /// <summary>
        /// 컨텍스트
        /// </summary>
        private readonly SwsContext* context;

        /// <summary>
        /// 버퍼 핸들
        /// </summary>
        private readonly IntPtr buferHandle;

        /// <summary>
        /// 임시 프레임 데이터
        /// </summary>
        private readonly byte_ptrArray4 temporaryFrameData;

        /// <summary>
        /// 임시 프레임 라인 크기
        /// </summary>
        private readonly int_array4 temporaryFrameLineSize;

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Constructor
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 생성자 - VideoFrameConverter(sourceSize, sourcePixelFormat, targetSize, targetPixelFormat)

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="sourceSize">소스 크기</param>
        /// <param name="sourcePixelFormat">소스 픽셀 포맷</param>
        /// <param name="targetSize">타겟 크기</param>
        /// <param name="targetPixelFormat">타겟 픽셀 포맷</param>
        public VideoFrameConverter(Size sourceSize, AVPixelFormat sourcePixelFormat, Size targetSize, AVPixelFormat targetPixelFormat)
        {
            this.targetSize = targetSize;

            this.context = ffmpeg.sws_getContext
            (
                (int)sourceSize.Width,
                (int)sourceSize.Height,
                sourcePixelFormat,
                (int)targetSize.Width,
                (int)targetSize.Height,
                targetPixelFormat,
                ffmpeg.SWS_FAST_BILINEAR,
                null,
                null,
                null
            );

            if(this.context == null)
            {
                throw new ApplicationException("Could not initialize the conversion context.");
            }

            int bufferSize = ffmpeg.av_image_get_buffer_size(targetPixelFormat, (int)targetSize.Width, (int)targetSize.Height, 1);

            this.buferHandle = Marshal.AllocHGlobal(bufferSize);

            this.temporaryFrameData = new byte_ptrArray4();

            this.temporaryFrameLineSize = new int_array4();

            ffmpeg.av_image_fill_arrays
            (
                ref this.temporaryFrameData,
                ref this.temporaryFrameLineSize,
                (byte*)this.buferHandle,
                targetPixelFormat,
                (int)targetSize.Width,
                (int)targetSize.Height,
                1
            );
        }

        #endregion

        //////////////////////////////////////////////////////////////////////////////////////////////////// Method
        ////////////////////////////////////////////////////////////////////////////////////////// Public

        #region 변환하기 - Convert(sourceFrame)

        /// <summary>
        /// 변환하기
        /// </summary>
        /// <param name="sourceFrame">소스 프레임</param>
        /// <returns>프레임</returns>
        public AVFrame Convert(AVFrame sourceFrame)
        {
            ffmpeg.sws_scale
            (
                this.context,
                sourceFrame.data,
                sourceFrame.linesize,
                0,
                sourceFrame.height,
                this.temporaryFrameData,
                this.temporaryFrameLineSize
            );

            byte_ptrArray8 targetFrameData = new byte_ptrArray8();

            targetFrameData.UpdateFrom(this.temporaryFrameData);

            int_array8 targetFrameLineSize = new int_array8();

            targetFrameLineSize.UpdateFrom(this.temporaryFrameLineSize);

            return new AVFrame
            {
                data     = targetFrameData,
                linesize = targetFrameLineSize,
                width    = (int)this.targetSize.Width,
                height   = (int)this.targetSize.Height
            };
        }

        #endregion

        #region 리소스 해제하기 - Dispose()

        /// <summary>
        /// 리소스 해제하기
        /// </summary>
        public void Dispose()
        {
            Marshal.FreeHGlobal(this.buferHandle);

            ffmpeg.sws_freeContext(this.context);
        }

        #endregion
    }
}