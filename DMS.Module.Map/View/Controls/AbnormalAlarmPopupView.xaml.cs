using DMS.Module.Map.ViewModel.Control;
using System.Windows;

namespace DMS.Module.Map.View.Controls
{
    /// <summary>
    /// AbnormalAlarmPopupView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AbnormalAlarmPopupView : Window
    {
        private AbnormalAlarmPopupViewModel VM { get; set; }

        public AbnormalAlarmPopupView()
        {
            InitializeComponent();

            this.VM = new AbnormalAlarmPopupViewModel();
            this.DataContext = this.VM;
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
    }
}
