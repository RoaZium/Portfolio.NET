using DMS.Module.EnvironmentSetting.Infrastructure;
using DMS.Module.EnvironmentSetting.Properties;
using PrismWPF.Common.Broadcast;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace DMS.Module.EnvironmentSetting.ViewModel
{
    public class EnvSetting_BasicViewModel : DMViewModelBase
    {
        public override void Load()
        {
        }

        public override void Unload()
        {
        }

        private string _Title = Application.Current.MainWindow.Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _systemVersion;
        public string SystemVersion
        {
            get => System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

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

        private readonly ListCollectionView _themeTypeList;
        public ListCollectionView ThemeTypeList => _themeTypeList;

        private string _selectedThemeType;
        public string SelectedThemeType
        {
            get => _selectedThemeType;
            set
            {
                _selectedThemeType = value;
                RaisePropertyChanged("SelectedThemeType");
            }
        }

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

        private string _ServerPort = "8080";
        public string ServerPort
        {
            get => _ServerPort;
            set
            {
                _ServerPort = value;
                RaisePropertyChanged("ServerPort");
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

        public EnvSetting_BasicViewModel()
        {
            _LanguageListView = new ListCollectionView(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("", Res.StrDefaultLanguage),
                new KeyValuePair<string, string>("en", Res.StrEnglish),
                new KeyValuePair<string, string>("ko", Res.StrKorean),
                new KeyValuePair<string, string>("ja", Res.StrJapanese),
            });

            _SelectedLang = Settings.Default.Lang;

            Uri serverUrl = Settings.Default.ServerUrl;
            if (serverUrl != null)
            {
                _ServerHostName = serverUrl.Host;
                _ServerPort = serverUrl.Port.ToString();
            }
            _MqttPort = Settings.Default.MqttPort == 0 ? null : Settings.Default.MqttPort.ToString();

            _themeTypeList = new ListCollectionView(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Black", Res.StrBlack),
                new KeyValuePair<string, string>("Blue", Res.StrBlue),
                new KeyValuePair<string, string>("White", Res.StrWhite),
            });

            _selectedThemeType = Settings.Default.Theme;
        }

        public void SaveSystemTitle()
        {
            try
            {
                IOHelper.Write(ConstantManager.FILE_GENERAL_SETTINGS, Title);
                BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_TITLE_CHANGED, Title, null);

                if (Settings.Default.Lang != _SelectedLang)
                {
                    Settings.Default.Lang = _SelectedLang;

                    MessageBox.Show(Res.MsgLanguageChanged);
                }

                if (Settings.Default.Theme != _selectedThemeType)
                {
                    Settings.Default.Theme = _selectedThemeType;

                    MessageBox.Show(Res.MsgThemeChanged);
                }

                Settings.Default.ServerUrl = new Uri($"http://{ServerHostName}:{ServerPort}");
                ushort.TryParse(MqttPort, out ushort mqttPort);
                Settings.Default.MqttPort = mqttPort;

                Settings.Default.Save();
            }
            catch (Exception e)
            {
                Settings.Default.Reload();
                MessageBox.Show(e.Message);
            }
        }
    }
}
