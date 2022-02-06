using AxisVideoTransformLib;
using Coever.Lib.Axis.Core;
using Coever.Lib.CoPlatform.IPCam.Core.Model;
using Coever.Lib.WPF.Axis.Controls;

namespace DMS.Module.Map.ViewModel
{
    public class IPCam_PANO_360_ViewModel : IPCamViewModelBase
    {
        #region Properties
        public override CameraTypes CameraType => CameraTypes.PANO_360;

        public new AxisFisheyeCamera CameraControl => base.CameraControl as AxisFisheyeCamera;

        private TiltOrientation _Orientation;
        public TiltOrientation Orientation
        {
            get => _Orientation;
            set
            {
                _Orientation = value;
                RaisePropertyChanged("Orientation");
            }
        }

        private bool _IsShowOverview = false;
        public bool IsShowOverview
        {
            get => _IsShowOverview;
            set
            {
                _IsShowOverview = value;
                RaisePropertyChanged("IsShowOverview");
            }
        }

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
        internal IPCam_PANO_360_ViewModel(
            string address, string account, string password, TiltOrientation orientation)
            : base(address, account, password)
        {
            _Orientation = orientation;
        }
        #endregion

        #region Methods
        internal override void SelectArea(IIPCamAreaModel area)
        {
            IPCam_PAN_360_AreaModel castedArea = area as IPCam_PAN_360_AreaModel;
            if (IsConnected && castedArea != null)
            {
                CameraControl?.SetPTZPosition(
                    castedArea.Pan, castedArea.Tilt, castedArea.EffectLevel, castedArea.FieldOfView);

                ShowAlarmPopup(castedArea.Name);
            }
        }
        #endregion

        #region Commands

        #endregion
    }
}
