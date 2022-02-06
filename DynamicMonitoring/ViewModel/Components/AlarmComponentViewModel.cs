using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Common.Model;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Resources;
using DynamicMonitoring.Utils;
using DynamicMonitoring.View;
using Prism.Commands;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace DynamicMonitoring.ViewModel
{
    public class AlarmComponentViewModel : DMViewModelBase
    {
        private AlarmComponentModel _Model;
        public AlarmComponentModel Model
        {
            get { return _Model; }
            set
            {
                _Model = value;
                RaisePropertyChanged("Model");
            }
        }

        public AlarmComponentViewModel()
        {
        }

        public override void Load()
        {
            _IsLoaded = true;
            StartDataPolling();
            Init();
        }

        public override void Unload()
        {
            IsOpenPopup = false;
        }

        /// <summary>
        /// View가 로드 되었는지 여부
        /// </summary>
        private bool _IsLoaded;

        /// <summary>
        /// 데이터 폴링 쓰레드가 돌아가고 있는지 여부
        /// </summary>
        private bool _IsPollingTaskRunning = false;

        /// <summary>
        /// 데이터 폴링 메소드가 호출되는 주기
        /// </summary>
        private int _PollingTermMilis = 30000;

        /// <summary>
        /// 리셋중인지 여부
        /// </summary>
        private bool _isResetting = false;

        private AlarmComponentPopupView _AlarmCpPopupV;
        public AlarmComponentPopupView AlarmCpPopupV
        {
            get{ return _AlarmCpPopupV; }
            set
            {
                _AlarmCpPopupV = value;
                RaisePropertyChanged("AlarmCpPopupV");
            }
        }

        private RealTimePopupViewModel _RealTimePopupViewModel;
        public RealTimePopupViewModel RealTimePopupViewModel
        {
            get
            {
                if (_RealTimePopupViewModel == null)
                {
                    string sensorCode = Model.TargetType == "TG002" ? Model.TargetCode : null;
                    _RealTimePopupViewModel = new RealTimePopupViewModel(sensorCode);
                }
                return _RealTimePopupViewModel;
            }
        }

        private bool _IsOpenPopup;
        public bool IsOpenPopup
        {
            get { return _IsOpenPopup; }
            set
            {
                _IsOpenPopup = value;

                if (_IsOpenPopup == true)
                {
                    AlarmCpPopupV = new AlarmComponentPopupView();
                    (AlarmCpPopupV.DataContext as AlarmComponentPopupViewModel).TargetType = Model.TargetType;
                    (AlarmCpPopupV.DataContext as AlarmComponentPopupViewModel).TargetCode = Model.TargetCode;
                }
                else
                {
                    AlarmCpPopupV = null;
                }

                RaisePropertyChanged("IsOpenPopup");
            }
        }

        private bool _IsOpenRealTimePopup;
        public bool IsOpenRealTimePopup
        {
            get { return _IsOpenRealTimePopup; }
            set
            {
                _IsOpenRealTimePopup = value;
                RaisePropertyChanged("IsOpenRealTimePopup");
            }
        }

        public void Init()
        {
            _AlarmState = AlarmState.None;
            RealTimePopupViewModel.SensorCode = Model.TargetType == "TG002" ? Model.TargetCode : null;

            RaisePropertyChanged(string.Empty);
        }

        public async void Reset()
        {
            _isResetting = true;

            await Task.Run(() =>
            {
                while (_IsPollingTaskRunning)
                {
                }
            });

            Init();
            _isResetting = false;
            if (_IsLoaded)
            {
                StartDataPolling();
            }

            (AlarmCpPopupV.DataContext as AlarmComponentPopupViewModel).Reset();
            RealTimePopupViewModel.Reset();
        }

        /// <summary>
        /// 주기적으로 데이터를 폴링하는 쓰레드를 시작한다.
        /// _IsLoaded가 false가 되거나 _isResetting이 true가 되면 쓰레드가 종료된다.
        /// </summary>
        private async void StartDataPolling()
        {
            if (_IsPollingTaskRunning)
            {
                return;
            }

            _IsPollingTaskRunning = true;

            while (_IsLoaded && !_isResetting)
            {
                LoadAlarm();
                await Task.Run(() =>
                {
                    long endTimeMilis = DateTimeUtils.GetCurrentTimeStampMilis() + _PollingTermMilis;
                    while (endTimeMilis > DateTimeUtils.GetCurrentTimeStampMilis() && _IsLoaded && !_isResetting)
                    {
                        Thread.Sleep(100);
                    }
                });
            }

            _IsPollingTaskRunning = false;
        }

        /// <summary>
        /// 실시간 알람 데이터를 가져온다
        /// </summary>
        private async void LoadAlarm()
        {
            try
            {
                string targetType = Model.TargetType == "TG002" ? "S" : "P";

                if (_isResetting)
                {
                    return;
                }

                DataTable dt = await DBHelper.GetAlramState(targetType, Model.TargetCode);

                if (_isResetting)
                {
                    return;
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    #region alarmLevel
                    int alarmLevel = (int)AlarmState.None;
                    if (!row.Table.Columns.Contains("alarm_level")
                    || !int.TryParse(row["alarm_level"].ToString(), out alarmLevel))
                    {
                        alarmLevel = (int)AlarmState.Unknown;
                    }

                    AlarmState = (AlarmState)alarmLevel;
                    #endregion
                }
            }
            catch { }
        }

        private string GetAlarmStateColor(AlarmState state)
        {
            switch (state)
            {
                case AlarmState.Normal:
                    {
                        return Res.ColorStateNormal;
                    }
                case AlarmState.Abnormal:
                    {
                        return Res.ColorStateAbnormal;
                    }
                case AlarmState.Danger:
                    {
                        return Res.ColorStateDanger;
                    }
                case AlarmState.None:
                    {
                        return Res.ColorStateNone;
                    }
                case AlarmState.Unknown:
                default:
                    {
                        return Res.ColorStateUnknown;
                    }
            }
        }

        #region Commands

        public DelegateCommand MouseEnterCommand
        {
            get { return new DelegateCommand(MouseEnterCommandExecute); }
        }

        public DelegateCommand MouseLeaveCommand
        {
            get { return new DelegateCommand(MouseLeaveCommandExecute); }
        }

        public DelegateCommand MouseLeftButtonUpCommand
        {
            get { return new DelegateCommand(MouseLeftButtonUpCommandExecute); }
        }

        private void MouseEnterCommandExecute()
        {
            if (IsOpenPopup == true)
                return;

            IsOpenPopup = true;
        }

        private void MouseLeaveCommandExecute()
        {
            IsOpenPopup = false;
        }

        private void MouseLeftButtonUpCommandExecute()
        {
            if (RealTimePopupViewModel.SensorCode != null)
            {
                IsOpenPopup = false;
                IsOpenRealTimePopup = true;
            }
        }
        #endregion



        #region 리팩토링 대상

        private AlarmState _AlarmState;
        public AlarmState AlarmState
        {
            get { return _AlarmState; }
            set
            {
                _AlarmState = value;
                RaisePropertyChanged("AlarmState");
                RaisePropertyChanged("AlarmStateColor");
            }
        }

        public Brush AlarmStateColor
        {
            get { return new SolidColorBrush((Color)ColorConverter.ConvertFromString(GetAlarmStateColor(AlarmState))); }
        }

        #endregion
    }
}
