using DynamicMonitoring.ViewModel;
using System.Windows;

namespace DynamicMonitoring.View
{
    /// <summary>
    /// Interaction logic for SettingsDialogView.xaml
    /// </summary>
    public partial class SettingsDialogView : Window
    {
        public SettingsDialogView()
        {
            InitializeComponent();
            Title = DynamicMonitoring.Properties.Res.StrSettings;
            DataContext = new SettingsDialogViewModel();
        }

        private void OnCloseBtnClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
