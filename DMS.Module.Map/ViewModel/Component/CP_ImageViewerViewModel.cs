using DMS.Module.Map.Infrastructure;
using PrismWPF.Common;
using PrismWPF.Common.Converter;
using System;
using System.Windows.Media.Imaging;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_ImageViewerViewModel : CP_CommonBaseViewModel
    {
        protected override void OnComponentData()
        {
            try
            {
                if (string.IsNullOrEmpty(DmComponentM.PropertyM.ImagePath))
                {
                    DmComponentM.PropertyM.ImageSource = ObjectConverter.ByteArrayToImageSource(DmComponentM.PropertyM.Imagebinary);
                }
                else
                {
                    DmComponentM.PropertyM.ImageSource = new BitmapImage(new Uri(DmComponentM.PropertyM.ImagePath));
                }
            }
            catch (Exception ex)
            {
                DmComponentM.PropertyM.ImageSource = null;
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }
    }
}
