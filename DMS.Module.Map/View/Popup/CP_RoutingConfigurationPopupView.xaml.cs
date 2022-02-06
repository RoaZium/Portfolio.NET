using DMS.Module.Map.ViewModel.Popup;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Map.View.Popup
{
    /// <summary>
    /// CP_RoutingConfigurationPopupView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CP_RoutingConfigurationPopupView : UserControl
    {
        public CP_CommonPopupViewModel VM { get; private set; }

        public CP_RoutingConfigurationPopupView()
        {
            InitializeComponent();

            VM = new CP_CommonPopupViewModel();
            DataContext = VM;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Load();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (VM == null)
            {
                return;
            }

            VM.Unload();
        }
    }
}
