using DMS.Module.Map.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Map.View
{
    /// <summary>
    /// Asset_ListView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Asset_ListView : UserControl
    {
        private Asset_ListViewModel VM { get; set; }

        public Asset_ListView()
        {
            InitializeComponent();

            this.VM = new Asset_ListViewModel();
            this.DataContext = this.VM;
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
