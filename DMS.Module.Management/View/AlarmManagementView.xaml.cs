using DMS.Module.Management.Infrastructure;
using DMS.Module.Management.Model.RestAPI;
using DMS.Module.Management.ViewModel;
using PrismWPF.Common.Broadcast;
using System.Windows;
using System.Windows.Controls;

namespace DMS.Module.Management.View
{
    /// <summary>
    /// AlarmManagementView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AlarmManagementView : UserControl
    {
        private AlarmManagementViewModel VM { get; set; }

        public AlarmManagementView()
        {
            InitializeComponent();

            this.VM = new AlarmManagementViewModel();
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DmComponentMst mst = ((System.Windows.FrameworkElement)sender).DataContext as DmComponentMst;

            if(mst == null)
            {
                return;
            }

            BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_ALARMLAYOUT, mst.RoutingCode, mst.PropertyM.IsAlarmLayout);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DmComponentMst mst = ((System.Windows.FrameworkElement)sender).DataContext as DmComponentMst;

            if (mst == null)
            {
                return;
            }

            BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_ALARMLAYOUT, mst.RoutingCode, mst.PropertyM.IsAlarmLayout);
        }
    }
}
