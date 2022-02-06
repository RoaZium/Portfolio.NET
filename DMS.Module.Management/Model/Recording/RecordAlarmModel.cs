using PrismWPF.Common.MVVM;
using System;

namespace DMS.Module.Management.Model
{
    public class RecordAlarmModel : DMViewModelBase
    {
        private string _SensorID;
        public string SensorID
        {
            get => _SensorID;
            set
            {
                _SensorID = value;
                RaisePropertyChanged("SensorID");
            }
        }

        private string _SensorName;
        public string SensorName
        {
            get => _SensorName;
            set
            {
                _SensorName = value;
                RaisePropertyChanged("SensorName");
            }
        }

        private DateTime _AlarmTime;
        public DateTime AlarmTime
        {
            get => _AlarmTime;
            set
            {
                _AlarmTime = value;
                RaisePropertyChanged("AlarmTime");
            }
        }

        private int _AlarmLevel;
        public int AlarmLevel
        {
            get => _AlarmLevel;
            set
            {
                _AlarmLevel = value;
                RaisePropertyChanged("AlarmLevel");
            }
        }

        private int _BlinkLevel;
        public int BlinkLevel
        {
            get => _BlinkLevel;
            set
            {
                _BlinkLevel = value;
                RaisePropertyChanged("BlinkLevel");
            }
        }
    }
}
