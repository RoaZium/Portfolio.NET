using DynamicMonitoring.Infrastructure;
using PrismWPF.Common.Broadcast;
using PrismWPF.Common.Converter;
using PrismWPF.Common.MVVM;
using System.Windows;
using System.Windows.Media;

namespace DynamicMonitoring
{
    public class MainWindowViewModel : DMViewModelBase
    {
        public string Title { get => DMConfigManager.Instance.Title; set { } }

        private Visibility _revertVisibility;
        public Visibility RevertVisibility
        {
            get => _revertVisibility;
            set
            {
                _revertVisibility = value;
                RaisePropertyChanged("RevertVisibility");
            }
        }

        private ImageSource _logo;
        public ImageSource Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                RaisePropertyChanged("Logo");
            }
        }

        public MainWindowViewModel()
        {
        }

        public override void Load()
        {
            BroadcastReceiver.RegisterReceiver(
                DMS.Module.EnvironmentSetting.Infrastructure.ConstantManager.BROADCAST_TITLE_CHANGED, OnTitleChanged);

            Logo = ObjectConverter.ToImageSource(ConstantManager.DIRECTORY_PROGRAM + ConstantManager.Logo);
        }

        public override void Unload()
        {
            BroadcastReceiver.UnregisterReceiver(
                DMS.Module.EnvironmentSetting.Infrastructure.ConstantManager.BROADCAST_TITLE_CHANGED, OnTitleChanged);
        }

        private void OnTitleChanged(string actionName, object sender, object e)
        {
            DMConfigManager.Instance.Title = sender.ToString();
            RaisePropertyChanged("Title");
        }
    }
}
