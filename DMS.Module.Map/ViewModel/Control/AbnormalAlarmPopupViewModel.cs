using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using Prism.Commands;
using PrismWPF.Common.MVVM;
using System;
using System.Linq;
using System.Windows;

namespace DMS.Module.Map.ViewModel.Control
{
    public class AbnormalAlarmPopupViewModel : DMViewModelBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _code;
        public string Code
        {
            get => _code;
            set => _code = value;
        }

        public DelegateCommand<Window> OKCommand => new DelegateCommand<Window>(OKCommandExecute);
        public DelegateCommand<Window> MoveCommand => new DelegateCommand<Window>(MoveCommandExecute);

        private void OKCommandExecute(Window sender)
        {
            try
            {
                sender.Close();
                MapFrameViewModel.Instance.MqttReceiveManager.IsMqttMessage = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void MoveCommandExecute(Window sender)
        {
            var resultLinq = BaseSingletonManager.Instance.AssetList.FirstOrDefault(p => p.PropertyM.AlarmCode != null && p.PropertyM.AlarmCode.Contains(Code));

            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                MapFrameViewModel.Instance.SelectedComponentConfig(resultLinq);
            }));

            sender.Close();
            MapFrameViewModel.Instance.MqttReceiveManager.IsMqttMessage = false;
        }
    }
}
