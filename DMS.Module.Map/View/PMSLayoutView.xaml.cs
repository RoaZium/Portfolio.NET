using DevExpress.Xpf.Docking;
using DMS.Module.Map.ViewModel;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// MES 공정 구성 레이아웃 View
/// </summary>
namespace DMS.Module.Map.View
{
    /// <summary>
    /// MESLayoutView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PMSLayoutView : UserControl, IMVVMDockingProperties
    {
        string IMVVMDockingProperties.TargetName
        {
            get => "PageHost";
            set { }
        }

        public PMSLayoutViewModel VM { get; private set; }

        public PMSLayoutView()
        {
            InitializeComponent();

            VM = new PMSLayoutViewModel();
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
