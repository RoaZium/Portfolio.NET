using DynamicMonitoring.Common;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DynamicMonitoring.ViewModel
{
    public class ImageComponentViewModel: DMViewModelBase
    {
        private ImageComponentModel _Model;
        public ImageComponentModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
                RaisePropertyChanged("Model");
            }
        }

        public ImageComponentViewModel()
        {
        }

        public override void Load()
        {
            ImageSource = null;
            LoadImage();
        }

        public override void Unload()
        {
        }

        /// <summary>
        /// 데이터 폴링 쓰레드가 돌아가고 있는지 여부
        /// </summary>
        private bool _IsImageLoading = false;

        /// <summary>
		/// 리셋중인지 여부
		/// </summary>
		private bool _isResetting = false;

        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set
            {
                _ImageSource = value;
                RaisePropertyChanged("ImageSource");
            }
        }

        public async void Reset()
        {
            _isResetting = true;

            await Task.Run(() =>
            {
                while (_IsImageLoading)
                {
                }
            });

            ImageSource = null;
            _isResetting = false;

            LoadImage();
        }

        /// <summary>
        /// 실시간 알람 데이터를 가져온다
        /// </summary>
        private async void LoadImage()
        {
            _IsImageLoading = true;

            if (string.IsNullOrEmpty(Model.ImagePath))
            {
                if (Model.ImageID != null)
                {
                    try
                    {
                        if (_isResetting)
                        {
                            return;
                        }

                        DataRow dr = await DBHelper.GetImage(Model.ImageID.Value);

                        if (_isResetting)
                        {
                            return;
                        }

                        byte[] imageData = (byte[])dr["ImageBinary"];

                        MemoryStream ms = new MemoryStream(imageData);

                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.EndInit();

                        ImageSource = image;
                    }
                    catch
                    {
                        ImageSource = ObjectConverter.ToImageSource(Res.ImgNan);
                    }
                }
                else
                {
                    ImageSource = null;
                }
            }
            else
            {
                ImageSource = new BitmapImage(new Uri(Model.ImagePath));
            }

            _IsImageLoading = false;
        }
    }
}
