using Coever.Lib.CoPlatform.Core.Network;
using DMS.Module.EnvironmentSetting.View;
using DMS.Module.Management.View;
using DMS.Module.Management.ViewModel;
using DMS.Module.Map.View;
using DMS.Module.Map.ViewModel;
using DynamicMonitoring.Infrastructure;
using DynamicMonitoring.Model;
using DynamicMonitoring.Network;
using DynamicMonitoring.Properties;
using PrismWPF.Common;
using PrismWPF.Common.Broadcast;
using System;
using System.Windows;

namespace DynamicMonitoring.View
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel VM { get; private set; }

        private readonly MapFrameView mapFrameV = new MapFrameView();
        public ManagementFrameView managementFrameV = new ManagementFrameView();
        public SystemUseCheckModel SystemUseCheckM = new SystemUseCheckModel();

        private ClientModuleType clientType;

        public MainWindow()
        {
            InitializeComponent();

            VM = new MainWindowViewModel();
            DataContext = VM;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            BroadcastReceiver.RegisterReceiver(ConstantManager.BROADCAST_MODULE, OnSettingModule);
            BroadcastReceiver.RegisterReceiver(ConstantManager.BROADCAST_MAPEDIT, OnSettingMAPEDIT);
            BroadcastReceiver.RegisterReceiver(ConstantManager.BROADCAST_FULLSCREEN, OnSettingFullScreen);

            VM.Load();

            SetDMSModule();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            BroadcastReceiver.UnregisterReceiver(ConstantManager.BROADCAST_MODULE, OnSettingModule);
            BroadcastReceiver.UnregisterReceiver(ConstantManager.BROADCAST_MAPEDIT, OnSettingMAPEDIT);
            BroadcastReceiver.UnregisterReceiver(ConstantManager.BROADCAST_FULLSCREEN, OnSettingFullScreen);

            VM.Unload();
        }

        private void SetDMSModule()
        {
            // 기본 설정 값
            xContentControl.Content = mapFrameV;
            (mapFrameV.DataContext as MapFrameViewModel).MapFrameV = mapFrameV;
            clientType = ClientModuleType.Map;

            xManagementContentControl.Content = managementFrameV;
            xManagementContentControl.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Map 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Map_Click(object sender, RoutedEventArgs e)
        {
            clientType = ClientModuleType.Map;
            SetVisibleMenu();

            xManagementContentControl.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Management 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Management_Click(object sender, RoutedEventArgs e)
        {
            clientType = ClientModuleType.Management;
            SetVisibleMenu();

            xManagementContentControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 환경 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnvormentSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnvSettingFrameView EnvFrameV = new EnvSettingFrameView();
                EnvFrameV.ShowDialog();

                if (EnvFrameV.DialogResult.Value)
                {
                    EnvFrameV.Owner.Close();
                    return;
                }
            }
            catch(Exception ex)
            {
                LogManager.Instance.WriteLine(ModuleType.DMLog, LOG_LEVEL.ErrorLevel, "EnvSetting" + ex.ToString());
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
            LogManager.Instance.WriteLine(ModuleType.DMLog, LOG_LEVEL.InfoLevel, "DMS END");
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            SystemUseCheckM.Account = string.IsNullOrEmpty(DMS.Module.Map.Properties.Settings.Default.Account) ? "DMS" : DMS.Module.Map.Properties.Settings.Default.Account;

            WebRequests.DeleteSystemUseCheck(CoPlatformWebClient.Instance, SystemUseCheckM);

            Application.Current.Shutdown();
        }

        private void ButtonMaximized_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonMenu_Click(object sender, RoutedEventArgs e)
        {
            SetVisibleMenu();
        }

        private void SetVisibleMenu()
        {
            if (clientType == ClientModuleType.Management)
            {
                ((managementFrameV.DataContext as ManagementFrameViewModel).ManagementListV.DataContext as ManagementListViewModel).IsShowMenu = Settings.Default.IsShowMapMenu;
            }

            // 저장 하고 값 변경해야 됨.
            Settings.Default.Save();
            Settings.Default.IsShowMapMenu = !Settings.Default.IsShowMapMenu;
        }

        private void OnSettingModule(string actionName, object sender, object e)
        {
            if (sender.ToString() == "Map")
            {
                xContentControl.Content = mapFrameV;
                clientType = ClientModuleType.Map;
                SetVisibleMenu();
            }
            else
            {
                xContentControl.Content = managementFrameV;
                clientType = ClientModuleType.Management;
                SetVisibleMenu();
            }
        }

        private void OnSettingMAPEDIT(string actionName, object sender, object e)
        {
            if(Boolean.Parse(sender.ToString()) == true)
            {
                xRevertGrid.Visibility = Visibility.Visible;
            }
            else
            {
                xRevertGrid.Visibility = Visibility.Hidden;
            }
        }

        private void OnSettingFullScreen(string actionName, object sender, object e)
        {
            if (Boolean.Parse(sender.ToString()) == true)
            {
                xContentControl.Margin = new Thickness(0, -30, 0, 0);
            }
            else
            {
                xContentControl.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void Button_RevertBack(object sender, RoutedEventArgs e)
        {
            (mapFrameV.DataContext as MapFrameViewModel).PMSLayoutVM.RevertBackComponent();
        }

        private void Button_RevertForward(object sender, RoutedEventArgs e)
        {
            (mapFrameV.DataContext as MapFrameViewModel).PMSLayoutVM.RevertForwardComponent();
        }
    }
}
