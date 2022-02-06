using Coever.Lib.WPF.Core.ViewModel;
using DynamicMonitoring.Properties;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace DynamicMonitoring.ViewModel
{
    public class SettingsDialogViewModel : CoeverViewModelBase
    {
        private string _SelectedLang;
        public string SelectedLang
        {
            get => _SelectedLang;
            set
            {
                _SelectedLang = value;
                RaisePropertyChanged("SelectedLang");
            }
        }

        private readonly ListCollectionView _LanguageListView;
        public ListCollectionView LanguageListView => _LanguageListView;

        private string _ServerHostName;
        public string ServerHostName
        {
            get => _ServerHostName;
            set
            {
                _ServerHostName = value;
                RaisePropertyChanged("ServerHostName");
            }
        }

        private string _HttpPort;
        public string HttpPort
        {
            get => _HttpPort;
            set
            {
                _HttpPort = value;
                RaisePropertyChanged("HttpPort");
            }
        }

        private string _MqttPort;
        public string MqttPort
        {
            get => _MqttPort;
            set
            {
                _MqttPort = value;
                RaisePropertyChanged("MqttPort");
            }
        }

        public SettingsDialogViewModel()
        {
            _LanguageListView = new ListCollectionView(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("", Res.StrSystemDefault),
                new KeyValuePair<string, string>("en", Res.StrEnglish),
                new KeyValuePair<string, string>("ko", Res.StrKorean),
                new KeyValuePair<string, string>("ja", Res.StrJapanese),
            });

            _SelectedLang = DMS.Module.EnvironmentSetting.Properties.Settings.Default.Lang;
            Uri serverUrl = DMS.Module.EnvironmentSetting.Properties.Settings.Default.ServerUrl;
            if (serverUrl != null)
            {
                _ServerHostName = serverUrl.Host;
                _HttpPort = serverUrl.Port.ToString();
            }
            _MqttPort = DMS.Module.EnvironmentSetting.Properties.Settings.Default.MqttPort == 0
                ? null : DMS.Module.EnvironmentSetting.Properties.Settings.Default.MqttPort.ToString();
        }

        public DelegateCommand<Window> OkCommand => new DelegateCommand<Window>(OkCommandExecute);

        private void OkCommandExecute(Window window)
        {
            try
            {
                if (DMS.Module.EnvironmentSetting.Properties.Settings.Default.Lang != _SelectedLang)
                {
                    DMS.Module.EnvironmentSetting.Properties.Settings.Default.Lang = _SelectedLang;
                }
                DMS.Module.EnvironmentSetting.Properties.Settings.Default.ServerUrl
                    = new Uri($"http://{ServerHostName}:{HttpPort}");
                ushort.TryParse(MqttPort, out ushort mqttPort);
                DMS.Module.EnvironmentSetting.Properties.Settings.Default.MqttPort = mqttPort;
                DMS.Module.EnvironmentSetting.Properties.Settings.Default.Save();

                if (window.Owner != null)
                {
                    MessageBox.Show(Res.MsgSettingChanged);
                }

                try
                {
                    window.DialogResult = true;
                }
                catch { }

                window.Close();
            }
            catch
            {
                DMS.Module.EnvironmentSetting.Properties.Settings.Default.Reload();

                MessageBox.Show(Res.MsgFailedConfirmSetting);
            }
        }

        public DelegateCommand<Window> CancelCommand => new DelegateCommand<Window>(CancelCommandExecute);

        private void CancelCommandExecute(Window window)
        {
            window.Close();
        }
    }
}
