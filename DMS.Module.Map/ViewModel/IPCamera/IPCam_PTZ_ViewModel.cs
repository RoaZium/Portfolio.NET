using Coever.Lib.Axis.Core;
using Coever.Lib.CoPlatform.IPCam.Core.Model;
using Coever.Lib.WPF.Axis.Controls;
using DMS.Module.Map.Properties;
using System.Windows;

namespace DMS.Module.Map.ViewModel
{
    public class IPCam_PTZ_ViewModel : IPCamViewModelBase
    {
        #region Properties
        public override CameraTypes CameraType => CameraTypes.PTZ;

        public new AxisPTZCamera CameraControl => base.CameraControl as AxisPTZCamera;

        private bool _IsActivateControl = false;
        public override bool IsActivateControl
        {
            get => _IsActivateControl;
            set
            {
                if (value
                    && MessageBox.Show(Res.MsgWarnPTZControl, Res.StrIPCamera,
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                {
                    value = false;
                }
                _IsActivateControl = value;
                RaisePropertyChanged("IsActivateControl");
            }
        }
        #endregion

        #region Constructor
        internal IPCam_PTZ_ViewModel(
            string address, string account, string password)
            : base(address, account, password)
        {
        }
        #endregion

        #region Methods
        internal override void SelectArea(IIPCamAreaModel area)
        {
            IPCam_PTZ_AreaModel castedArea = area as IPCam_PTZ_AreaModel;
            if (IsConnected && castedArea != null)
            {
                CameraControl?.SetPTZPosition(
                    castedArea.Pan, castedArea.Tilt, castedArea.Zoom, 0);

                ShowAlarmPopup(castedArea.Name);
            }
        }
        #endregion

        #region Commands

        #endregion
    }
}
