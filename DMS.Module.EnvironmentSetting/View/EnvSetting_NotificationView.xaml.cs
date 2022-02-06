using DMS.Module.EnvironmentSetting.ViewModel;
using System.Windows.Controls;

namespace DMS.Module.EnvironmentSetting.View
{
    /// <summary>
    /// EnvSetting_NotificationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EnvSetting_NotificationView : UserControl
    {
        public EnvSetting_NotificationViewModel VM { get; private set; }

        public EnvSetting_NotificationView()
        {
            InitializeComponent();

            this.VM = new EnvSetting_NotificationViewModel();
            this.DataContext = this.VM;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.VM == null)
            {
                return;
            }

            this.VM.Load();
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.VM == null)
            {
                return;
            }

            this.VM.Unload();
        }
    }
}
