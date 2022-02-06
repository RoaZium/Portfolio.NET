using DynamicMonitoring.Common.Converter;
using FFmpeg.AutoGen;
using Prism.Commands;
using PrismWPF.Common.Streaming;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_StreamingViewModel : CP_CommonBaseViewModel
    {
        public CP_StreamingViewModel()
        {
        }

        public override void Load()
        {
            IsPlayState = DmComponentM.PropertyM.PlaybackM.Play;
            PlayCommandExecute();
        }

        public override void Unload()
        {
            SetJoinThread();
        }

        protected override void OnComponentData()
        {
        }

        private bool? _isPlayState;
        public bool? IsPlayState
        {
            get => _isPlayState;
            set
            {
                _isPlayState = value;
                RaisePropertyChanged("IsPlayState");
            }
        }

        private Thread thread;

        private bool isThreadRunning;

        private unsafe void ProcessThread()
        {
            if(string.IsNullOrEmpty(DmComponentM.PropertyM.PlaybackM.Url))
            {
                return;
            }

            using (VideoStreamDecoder decoder = new VideoStreamDecoder(DmComponentM.PropertyM.PlaybackM.Url))
            {
                IReadOnlyDictionary<string, string> contextInfoDictionary = decoder.GetContextInfoDictionary();

                contextInfoDictionary.ToList().ForEach(x => Console.WriteLine($"{x.Key} = {x.Value}"));

                Size sourceSize = decoder.FrameSize;
                AVPixelFormat sourcePixelFormat = decoder.PixelFormat;
                Size targetSize = sourceSize;
                AVPixelFormat targetPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;

                using (VideoFrameConverter converter = new VideoFrameConverter(sourceSize, sourcePixelFormat, targetSize, targetPixelFormat))
                {
                    int frameNumber = 0;

                    while (decoder.TryDecodeNextFrame(out AVFrame sourceFrame) && isThreadRunning)
                    {
                        AVFrame targetFrame = converter.Convert(sourceFrame);

                        System.Drawing.Bitmap bitmap;

                        bitmap = new System.Drawing.Bitmap
                        (
                            targetFrame.width,
                            targetFrame.height,
                            targetFrame.linesize[0],
                            System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                            (IntPtr)targetFrame.data[0]
                        );

                        SetImageSource(bitmap);

                        frameNumber++;
                    }
                }
            }
        }

        private void SetImageSource(System.Drawing.Bitmap bitmap)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    if (this.thread.IsAlive)
                    {
                        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);

                        memoryStream.Position = 0;

                        BitmapImage bitmapimage = new BitmapImage();

                        bitmapimage.BeginInit();

                        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapimage.StreamSource = memoryStream;

                        bitmapimage.EndInit();

                        DmComponentM.PropertyM.ImageSource = bitmapimage;
                    }
                }
            }));
        }

        /// <summary>
        /// 해당 쓰레드 제어 처리
        /// </summary>
        private void SetJoinThread()
        {
            if (this.thread != null && this.thread.IsAlive)
            {
                this.isThreadRunning = false;

                this.thread.Join();

                DmComponentM.PropertyM.ImageSource = null;
            }
        }

        public DelegateCommand PlayCommand => new DelegateCommand(PlayCommandExecute);
        private void PlayCommandExecute()
        {
            if (IsPlayState == true)
            {
                if (this.isThreadRunning)
                {
                    this.isThreadRunning = false;
                }
                else
                {
                    this.isThreadRunning = true;

                    this.thread = new Thread(ProcessThread);

                    this.thread.Start();
                }
            }
            else
            {
                SetJoinThread();
            }
        }
    }
}
