using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.Mqtt.Core;
using DMS.Module.EnvironmentSetting.Properties;
using DynamicMonitoring.Infrastructure;
using DynamicMonitoring.Model;
using DynamicMonitoring.Modules;
using DynamicMonitoring.Network;
using DynamicMonitoring.View;
using PrismWPF.Common;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Control;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace DynamicMonitoring
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        internal static bool IsStandAlone = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            LogManager.Instance.WriteLine(ModuleType.DMLog, LOG_LEVEL.ErrorLevel, "DMS START");

            if (e.Args.Count() > 0)
            {
                try
                {
                    if (e.Args[0] != null)
                    {

                    }

                    if (e.Args[1] != null)
                    {
                        string account = e.Args[1];
                        DMS.Module.Map.Properties.Settings.Default.Account = account;
                    }

                    if (e.Args[2] != null)
                    {

                    }

                    if (e.Args[4] != null)
                    {
                        string language = e.Args[4];
                        Settings.Default.Lang = language;
                    }

                    if (e.Args[5] != null)
                    {
                        string serverUrl = e.Args[5];

                        Settings.Default.ServerUrl = new Uri(serverUrl);
                    }

                    if (e.Args[6] != null)
                    {
                        ushort mqttPort = e?.Args?.Length > 6 ? ushort.Parse(e.Args[6]) : (ushort)1883;
                        Settings.Default.MqttPort = mqttPort;
                    }

                    if (e.Args[7] != null)
                    {
                        string themetype = e.Args[7];
                        Settings.Default.Theme = themetype;
                        SetTheme();
                    }

                    StartMain();
                    DeleteSystemUseCheck();
                    return;
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show(DynamicMonitoring.Properties.Res.MsgArgumentError + Environment.NewLine + ex.Message + " : " + ex.StackTrace,
                        DynamicMonitoring.Properties.Res.StrWarning, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        Current.Shutdown();
                        return;
                    }
                }
            }

            IsStandAlone = true;
            SetTheme();
            CheckSettings();
            DeleteSystemUseCheck();
        }

        private void LoadLanguage()
        {
            string lang = DMS.Module.EnvironmentSetting.Properties.Settings.Default.Lang;

            if (string.IsNullOrEmpty(lang))
            {
                lang = CultureInfo.InstalledUICulture.Name;
            }

            DMConfigManager.Instance.ChangeCultureInfo(lang);
        }

        private void CheckSettings()
        {
            if (DMS.Module.EnvironmentSetting.Properties.Settings.Default.ServerUrl == null)
            {
                SettingsDialogView dlg = new SettingsDialogView
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                dlg.Closing += (s, ev) =>
                {
                    if (DMS.Module.EnvironmentSetting.Properties.Settings.Default.ServerUrl != null)
                    {
                        StartMain();
                    }
                };
                dlg.Show();
                return;
            }
            StartMain();
        }

        private void LoadGeneralSettings()
        {
            if (File.Exists(DMS.Module.EnvironmentSetting.Infrastructure.ConstantManager.FILE_GENERAL_SETTINGS))
            {
                try
                {
                    DMConfigManager.Instance.Title = IOHelper.Read(
                        DMS.Module.EnvironmentSetting.Infrastructure.ConstantManager.FILE_GENERAL_SETTINGS);
                    return;
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLine(ModuleType.DMLog, LOG_LEVEL.ErrorLevel, ex.ToString());
                }
            }
        }

        private void StartMain()
        {
            // 타이틀 설정 정보 -> Local에 저장
            LoadGeneralSettings();

            LoadLanguage();

            DMS.Module.Management.ModuleManager.DataManager = ManagementDataBroker.Instance;

            Uri httpUri = DMS.Module.EnvironmentSetting.Properties.Settings.Default.ServerUrl;
            int mqttPort = DMS.Module.EnvironmentSetting.Properties.Settings.Default.MqttPort == 0
                ? 1883 : Settings.Default.MqttPort;
            CoPlatformWebClient.Instance.Init(httpUri.ToString());
            MqttService.Instance.Init(httpUri.Host, mqttPort, false, null, null, 0);
            try
            {
                MqttService.Instance.Client.ConnectRandomId("DMS");
                MqttService.Instance.Client.Subscribe(
                        new[] 
                        {
                            ConstantManager.MQTTTOPIC_IPCAMERA,
                            ConstantManager.MQTTTOPIC_SENSOR
                        },
                        new byte[] { 0, 0 });
            }
            catch
            {
                MessageBox.Show(DynamicMonitoring.Properties.Res.MsgMqttError,
                    DynamicMonitoring.Properties.Res.StrWarning, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            MainWindow window = new MainWindow();
            window.Show();
        }

        private void SetTheme()
        {
            ThemeType themeType = ThemeType.Black;

            switch (Settings.Default.Theme)
            {
                case "Black":
                    {
                        themeType = ThemeType.Black;
                        break;
                    }
                case "Blue":
                    {
                        themeType = ThemeType.Blue;
                        break;
                    }
                case "White":
                    {
                        themeType = ThemeType.White;
                        break;
                    }
                default:
                    {
                        themeType = ThemeType.Black;
                        Settings.Default.Theme = themeType.ToString();
                        break;
                    }
            }

            PrismWPF.Control.ThemeManagement tm = new PrismWPF.Control.ThemeManagement();
            tm.SetThemeType(themeType);
        }

        private void DeleteSystemUseCheck()
        {
            try
            {
                SystemUseCheckModel SystemUseCheckM = new SystemUseCheckModel
                {
                    Account = string.IsNullOrEmpty(DMS.Module.Map.Properties.Settings.Default.Account) ? "DMS" : DMS.Module.Map.Properties.Settings.Default.Account
                };

                WebRequests.DeleteSystemUseCheck(CoPlatformWebClient.Instance, SystemUseCheckM);
            }
            catch(Exception ex)
            {
                LogManager.Instance.WriteLine(ModuleType.DMLog, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            try
            {
                MqttService.Instance.Client.Disconnect();
            }
            catch { }
        }
    }
}
