using PrismWPF.Common.MVVM;

namespace DMS.Module.EnvironmentSetting.Model
{
    public class EnvSetting_NotificationModel : DMViewModelBase
    {
        private string _Action = "R";
        public string Action
        {
            get => _Action;
            set => _Action = value;
        }

        private int _AlarmStateCode;
        public int AlarmStateCode
        {
            get => _AlarmStateCode;
            set
            {
                _AlarmStateCode = value;
                RaisePropertyChanged("AlarmStateCode");
            }
        }

        private string _AlarmStateName;
        public string AlarmStateName
        {
            get => _AlarmStateName;
            set
            {
                _AlarmStateName = value;
                RaisePropertyChanged("AlarmStateName");
            }
        }

        private bool _UseMail;
        public bool UseMail
        {
            get => _UseMail;
            set
            {
                _UseMail = value;
                RaisePropertyChanged("UseMail");
            }
        }

        private bool _UseAlarm;
        public bool UseAlarm
        {
            get => _UseAlarm;
            set
            {
                _UseAlarm = value;
                RaisePropertyChanged("UseAlarm");
            }
        }
    }
}
