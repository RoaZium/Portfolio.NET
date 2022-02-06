using AxisVideoTransformLib;
using Coever.Lib.Axis.Core;
using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.WF.Axis.Controls;
using Coever.Lib.WF.Axis.Controls.Enum;
using Coever.Lib.WF.Axis.Controls.Event;
using Coever.Lib.WPF.Axis.Controls;
using DMS.Module.Management.Events;
using DMS.Module.Management.Model;
using DMS.Module.Management.Model.Recording;
using DMS.Module.Management.Network;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.Async;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class IPCamRecordPalyerViewModel : DMViewModelBase
    {
        #region Private variables
        private readonly AsyncTask<DateTime, IList<RecordAlarmModel>> _loadListTask;
        #endregion

        #region Properties
        private readonly Recording _RecordModel;
        public Recording RecordModel => _RecordModel;

        private IAxisCameraControl _CameraControl;
        public IAxisCameraControl CameraControl
        {
            get => _CameraControl;
            set
            {
                _CameraControl = value;
                RaisePropertyChanged("CameraControl");
            }
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private readonly List<RecordAlarmModel> _RecordAlarmList;
        private readonly ListCollectionView _RecordAlarmListView;
        public ListCollectionView RecordAlarmListView => _RecordAlarmListView;

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get => _StartTime;
            set
            {
                _StartTime = value;
                RaisePropertyChanged("StartTime");
            }
        }

        private DateTime? _EndTime;
        public DateTime? EndTime
        {
            get => _EndTime;
            set
            {
                _EndTime = value;
                RaisePropertyChanged("EndTime");
            }
        }
        #endregion

        #region Constructor
        internal IPCamRecordPalyerViewModel(Recording recordModel)
        {
            if (recordModel == null)
            {
                throw new ArgumentNullException("recordModel");
            }

            _RecordModel = recordModel;

            switch (recordModel.IPCam.Type)
            {
                case nameof(CameraTypes.FLAT):
                    {
                        CameraControl = new AxisFlatCamera();
                        break;
                    }
                case nameof(CameraTypes.PANO_360):
                    {
                        CameraControl = new AxisFisheyeCamera()
                        {
                            CameraOrientation = (TiltOrientation)Enum.Parse(
                                typeof(TiltOrientation), recordModel.IPCam.Orientation),
                        };
                        break;
                    }
                case nameof(CameraTypes.PTZ):
                    {
                        CameraControl = new AxisPTZCamera();
                        break;
                    }
            }

            if (CameraControl != null)
            {
                CameraControl.Address = recordModel.IPCam.Address;
                CameraControl.User = recordModel.IPCam.Account;
                CameraControl.Password = recordModel.IPCam.Pwd;
                CameraControl.RecordingID = RecordModel.RecordingId;
                CameraControl.IsKeepAspectRatio = true;
                CameraControl.IsShowControlBar = false;
                CameraControl.IsActivateMouseControl = true;

                CameraControl.OnPropertyChangedListeners += OnPropertyChangedListener;
                CameraControl.OnMediaPositionChangedListeners += OnMediaPositionListener;
            }

            _RecordAlarmList = new List<RecordAlarmModel>();
            _RecordAlarmListView = new ListCollectionView(_RecordAlarmList);

            _loadListTask = new AsyncTask<DateTime, IList<RecordAlarmModel>>(
                null,
                (task, inputs) =>
                {
                    List<RecordAlarmModel> result = CoPlatformWebClient.Instance.GetRecordingAlarmAPI(_RecordModel.IPCam.Code.Value, inputs[0], inputs[1]);
                    return task.Status == AsyncTaskStatus.RUNNING ? result : null;
                },
                (task, result, inputs) =>
                {
                    if (result != null)
                    {
                        _RecordAlarmList.Clear();
                        _RecordAlarmList.AddRange(result);
                        RecordAlarmListView.Refresh();
                    }
                });
        }
        #endregion

        #region Methods
        #endregion

        #region Commands
        public DelegateCommand CloseCommand => new DelegateCommand(CloseCommandExecute);

        private void CloseCommandExecute()
        {
            EventAggregator?.GetEvent<PubSubEvent<IPCamRecordPlayerCloseEvent>>().Publish(
                new IPCamRecordPlayerCloseEvent());
            try
            {
                CameraControl?.Stop();
            }
            catch { }
        }

        public DelegateCommand<RoutedEventArgs> LoadedCommand => new DelegateCommand<RoutedEventArgs>(LoadedCommandExecute);

        private void LoadedCommandExecute(RoutedEventArgs e)
        {
        }

        public DelegateCommand UnloadedCommand => new DelegateCommand(UnloadedCommandExecute);

        private void UnloadedCommandExecute()
        {
            try
            {
                CameraControl?.Stop();
            }
            catch { }
        }

        protected override void DisposeManaged()
        {
            CameraControl?.Dispose();

            base.DisposeManaged();
        }

        private void OnPropertyChangedListener(object sender, AxisPropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "State":
                    {
                        if ((AxisControlState)e.NewValue == AxisControlState.Palying)
                        {
                            StartTime = new DateTime(e.Control.BaseTimeTicks, DateTimeKind.Local);
                            EndTime = new DateTime(e.Control.BaseTimeTicks + e.Control.Duration * 10000, DateTimeKind.Local);

                            _loadListTask.execute(StartTime.Value, EndTime.Value);
                        }
                        else if ((AxisControlState)e.NewValue == AxisControlState.Stopped)
                        {
                            _RecordAlarmList.Clear();
                            RecordAlarmListView.Refresh();

                            StartTime = null;
                            EndTime = null;
                        }
                        break;
                    }
            }
        }

        private void OnMediaPositionListener(object sender, AxisMediaPositionChangedEventArgs e)
        {
            EndTime = new DateTime(e.Control.BaseTimeTicks + e.Control.Duration * 10000);
        }
        #endregion
    }
}
