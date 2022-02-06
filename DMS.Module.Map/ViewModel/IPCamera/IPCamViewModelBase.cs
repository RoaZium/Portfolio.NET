using Coever.Lib.Axis.Core;
using Coever.Lib.CoPlatform.IPCam.Core.Model;
using Coever.Lib.WF.Axis.Controls.Enum;
using Coever.Lib.WF.Axis.Controls.Event;
using Prism.Commands;
using PrismWPF.Common.MVVM;
using System.Threading.Tasks;
using System.Windows;

namespace DMS.Module.Map.ViewModel
{
    public abstract class IPCamViewModelBase : DMViewModelBase
    {
        #region Private variables
        protected readonly string _Address;
        protected readonly string _Account;
        protected readonly string _Password;
        #endregion

        #region Properties
        protected Coever.Lib.WF.Axis.Controls.IAxisCameraControl CameraControl { get; set; }

        public abstract CameraTypes CameraType { get; }

        private bool _IsConnected = false;
        public bool IsConnected
        {
            get => _IsConnected;
            private set
            {
                _IsConnected = value;
                RaisePropertyChanged("IsConnected");
            }
        }

        private bool _IsCameraControlVisible;
        public bool IsCameraControlVisible
        {
            get => _IsCameraControlVisible;
            set
            {
                _IsCameraControlVisible = value;
                RaisePropertyChanged("IsCameraControlVisible");

                OnIsVisibleChanged();
            }
        }

        private bool _IsShowAlarmPopup = false;
        public bool IsShowAlarmPopup
        {
            get => _IsShowAlarmPopup;
            set
            {
                _IsShowAlarmPopup = value;
                RaisePropertyChanged("IsShowAlarmPopup");
            }
        }

        private string _PopupName;
        public string PopupName
        {
            get => _PopupName;
            set
            {
                _PopupName = value;
                RaisePropertyChanged("PopupName");
            }
        }

        public abstract bool IsActivateControl { get; set; }
        #endregion

        #region Constructor
        internal IPCamViewModelBase(
            string address, string account, string password)
        {
            _Address = address;
            _Account = account;
            _Password = password;
        }
        #endregion

        #region Methods
        internal abstract void SelectArea(IIPCamAreaModel area);

        protected void ShowAlarmPopup(string popupName)
        {
            IsShowAlarmPopup = true;
            PopupName = popupName;
        }

        protected void CloseAlarmPopup()
        {
            IsShowAlarmPopup = false;
        }
        #endregion

        #region Event handlers
        private void OnIsVisibleChanged()
        {
            if (IsCameraControlVisible)
            {
                CameraControl?.Play(10);
            }
            else
            {
                CameraControl?.Stop();
            }
        }
        #endregion

        #region Commands
        public DelegateCommand<RoutedEventArgs> CameraLoadedCommand => new DelegateCommand<RoutedEventArgs>(CameraLoadedCommandExecute);

        private void CameraLoadedCommandExecute(RoutedEventArgs e)
        {
            try
            {
                CameraControl = e.Source as Coever.Lib.WF.Axis.Controls.IAxisCameraControl;
                CameraControl.Address = _Address;
                CameraControl.User = _Account;
                CameraControl.Password = _Password;

                Task.Factory.StartNew(() =>
                {
                    CameraControl.Play(10);
                });
            }
            catch { }
        }

        public DelegateCommand<AxisPropertyChangedEventArgs> CameraPropertyChangedCommand => new DelegateCommand<AxisPropertyChangedEventArgs>(CameraPropertyChangedCommandExecute);

        private void CameraPropertyChangedCommandExecute(AxisPropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "State":
                    {
                        if (e.Control.State == AxisControlState.Palying)
                        {
                            IsConnected = true;
                        }
                        else if (e.Control.State == AxisControlState.Stopped)
                        {
                            IsConnected = false;

                            CloseAlarmPopup();
                        }

                        break;
                    }
            }
        }
        #endregion
    }
}
