using DMS.Module.Map.ViewModel.Component;
using Prism.Commands;
using PrismWPF.Common.Infrastructure;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Map.View.Component
{
    /// <summary>
    /// CP_IPCameraView_N.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CP_IPCameraView : UserControl
    {
        public CP_IPCameraViewModel VM { get; private set; }

        public CP_IPCameraView()
        {
            InitializeComponent();

            VM = new CP_IPCameraViewModel();
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

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == "DataContext")
            {
                if (e.NewValue != null)
                {
                    VM = e.NewValue as CP_IPCameraViewModel;
                }
                else
                {
                    VM = null;
                }
            }
        }

        private void OnIsVisibleChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement control = sender as FrameworkElement;

            if (control.DataContext != null)
            {
                object isVisibleChangedCommand = control.DataContext.GetPropertyObject("IsVisibleChangedCommand");

                if (isVisibleChangedCommand != null && isVisibleChangedCommand is DelegateCommand<UIElement>)
                {
                    (isVisibleChangedCommand as DelegateCommand<UIElement>).Execute(control);
                }
            }
        }
    }
}
