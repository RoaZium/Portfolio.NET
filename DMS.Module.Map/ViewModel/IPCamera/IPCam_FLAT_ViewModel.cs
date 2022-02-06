using Coever.Lib.Axis.Core;
using Coever.Lib.CoPlatform.IPCam.Core.Model;
using Coever.Lib.WPF.Axis.Controls;

namespace DMS.Module.Map.ViewModel
{
    public class IPCam_FLAT_ViewModel : IPCamViewModelBase
    {
        #region Properties
        public override CameraTypes CameraType => CameraTypes.FLAT;

        public new AxisFlatCamera CameraControl => base.CameraControl as AxisFlatCamera;

        private bool _IsActivateControl = true;
        public override bool IsActivateControl
        {
            get => _IsActivateControl;
            set
            {
                _IsActivateControl = value;
                RaisePropertyChanged("IsActivateControl");
            }
        }
        #endregion

        #region Constructor
        internal IPCam_FLAT_ViewModel(
            string address, string account, string password)
            : base(address, account, password)
        {
        }
        #endregion

        #region Methods
        internal override void SelectArea(IIPCamAreaModel area)
        {
            IPCam_FLAT_AreaModel castedArea = area as IPCam_FLAT_AreaModel;
            if (IsConnected && castedArea != null)
            {
                CameraControl?.SetPTZPosition(
                    castedArea.Left, castedArea.Top, castedArea.Right, castedArea.Bottom);

                ShowAlarmPopup(castedArea.Name);
            }
        }
        #endregion

        #region Commands

        #endregion
    }
}
