using PrismWPF.Common.MVVM;
using System.Windows;
using System.Windows.Controls;


namespace DMS.Module.Map.View
{
    /// <summary>
    /// IPCamView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IPCamView : UserControl
    {
        public DMViewModelBase VM { get; private set; }

        public IPCamView()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "DataContext")
            {
                if (e.NewValue != null)
                {
                    VM = e.NewValue as DMViewModelBase;
                }
                else
                {
                    VM = null;
                }
            }
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
