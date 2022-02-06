using DMS.Module.Management.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// RoutingConfigStatusBoard.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RoutingConfigStatusView : UserControl
    {
        public RoutingConfigStatusViewModel VM { get; private set; }

        public RoutingConfigStatusView()
        {
            InitializeComponent();

            VM = new RoutingConfigStatusViewModel();
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
