using Coever.Lib.CoPlatform.Core.Models;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.View.Popup;
using DMS.Module.Map.ViewModel.Popup;
using Prism.Commands;
using System.Windows.Controls;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_VirtualViewModel : CP_CommonBaseViewModel
    {
        protected override void OnComponentData()
        {
        }

        private UserControl _ResourcePopupV;
        public UserControl ResourcePopupV
        {
            get => _ResourcePopupV;
            set
            {
                _ResourcePopupV = value;
                RaisePropertyChanged("ResourcePopupV");
            }
        }

        private string sensorType = string.Empty;

        private bool _IsOpenPopup;
        public bool IsOpenPopup
        {
            get => _IsOpenPopup;
            set
            {
                _IsOpenPopup = value;
                SetPopupView();
                RaisePropertyChanged("IsOpenPopup");
            }
        }

        private void SetPopupView()
        {
            if (DmComponentM.RoutingType == nameof(ComponentType.DM601005))
            {
                ResourcePopupV = new CP_RoutingConfigurationPopupView();
                (ResourcePopupV.DataContext as CP_CommonPopupViewModel).ResourceSensorM.ResourceCode = DmComponentM.RoutingType == ComponentType.DM701000.ToString() ? DmComponentM.PropertyM.ItemCode : DmComponentM.RoutingCode;
            }
            else
            {
                ResourcePopupV = new CP_RoutingConfigurationPopupView();
                (ResourcePopupV.DataContext as CP_CommonPopupViewModel).DmComponentM = DmComponentM;
            }
        }

        public DelegateCommand MouseEnterCommand => new DelegateCommand(MouseEnterCommandExecute);
        public DelegateCommand MouseLeaveCommand => new DelegateCommand(MouseLeaveCommandExecute);

        private void MouseEnterCommandExecute()
        {
            if (IsOpenPopup == true)
            {
                return;
            }

            IsOpenPopup = true;
        }

        private void MouseLeaveCommandExecute()
        {
            IsOpenPopup = false;
        }
    }
}
