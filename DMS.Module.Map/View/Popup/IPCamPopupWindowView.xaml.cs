using DMS.Module.Map.Infrastructure;
using Prism.Commands;
using PrismWPF.Common.Broadcast;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace DynamicMonitoring.View
{
    /// <summary>
    /// IPCamPopupWindowView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class IPCamPopupWindowView : Window
    {
        #region Private variables
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion

        public string componentID = string.Empty;

        #region Properties
        public DMViewModelBase VM { get; private set; }
        #endregion

        #region Constructor
        public IPCamPopupWindowView()
        {
            InitializeComponent();
        }
        #endregion

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

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

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            var data = ((System.Windows.FrameworkElement)sender).DataContext as DMS.Module.Map.ViewModel.IPCamPopupWindowViewModel;

            if (data == null)
            {
                BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_IPCAMERA_HIDE, componentID, null);
            }
            else
            {
                BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_IPCAMERA_HIDE, data._CompoID, null);
            }
        }
    }
}
