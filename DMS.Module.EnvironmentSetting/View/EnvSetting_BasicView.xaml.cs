using DMS.Module.EnvironmentSetting.ViewModel;
using System.Windows.Controls;

namespace DMS.Module.EnvironmentSetting.View
{
    /// <summary>
    /// EnvSetting_Basic.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class EnvSetting_BasicView : UserControl
    {
        public EnvSetting_BasicViewModel VM { get; private set; }

        public EnvSetting_BasicView()
        {
            InitializeComponent();

            VM = new EnvSetting_BasicViewModel();
            DataContext = VM;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Load();
        }

        private void UserControl_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Unload();
        }
    }
}
