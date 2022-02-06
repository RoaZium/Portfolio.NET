using DMS.Module.EnvironmentSetting.ViewModel;
using System.Windows;

namespace DMS.Module.EnvironmentSetting.View
{
    /// <summary>
    /// EnvSettingFrameView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EnvSettingFrameView : Window
    {
        public EnvSettingFrameViewModel VM { get; private set; }

        public EnvSettingFrameView()
        {
            InitializeComponent();

            VM = new EnvSettingFrameViewModel();
            DataContext = VM;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Load();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Unload();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
