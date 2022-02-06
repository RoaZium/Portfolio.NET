using PrismWPF.Common.Converter;
using PrismWPF.Common.MVVM;
using System;

namespace DMS.Module.Management.Model.RestAPI
{
    public class DmRoutingConfigurationStatusModel : DMViewModelBase
    {
        private int _userId;
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        private string _dM601001;
        public string DM601001
        {
            get => _dM601001;
            set
            {
                _dM601001 = value;
                RaisePropertyChanged("DM601001");
            }
        }

        private string _dM601001Code;
        public string DM601001Code
        {
            get => _dM601001Code;
            set
            {
                _dM601001Code = value;
                RaisePropertyChanged("DM601001Code");
            }
        }

        private string _dM601001Name;
        public string DM601001Name
        {
            get => _dM601001Name;
            set
            {
                _dM601001Name = value;
                RaisePropertyChanged("DM601001Name");
            }
        }

        private string _dM601002;
        public string DM601002
        {
            get => _dM601002;
            set
            {
                _dM601002 = value;
                RaisePropertyChanged("DM601002");
            }
        }

        private string _dM601002Code;
        public string DM601002Code
        {
            get => _dM601002Code;
            set
            {
                _dM601002Code = value;
                RaisePropertyChanged("DM601002Code");
            }
        }

        private string _dM601002Name;
        public string DM601002Name
        {
            get => _dM601002Name;
            set
            {
                _dM601002Name = value;
                RaisePropertyChanged("DM601002Name");
            }
        }

        private string _dM601003;
        public string DM601003
        {
            get => _dM601003;
            set
            {
                _dM601003 = value;
                RaisePropertyChanged("DM601003");
            }
        }

        private string _dM601003Code;
        public string DM601003Code
        {
            get => _dM601003Code;
            set
            {
                _dM601003Code = value;
                RaisePropertyChanged("DM601003Code");
            }
        }

        private string _dM601003Name;
        public string DM601003Name
        {
            get => _dM601003Name;
            set
            {
                _dM601003Name = value;
                RaisePropertyChanged("DM601003Name");
            }
        }

        private string _dM601004;
        public string DM601004
        {
            get => _dM601004;
            set
            {
                _dM601004 = value;
                RaisePropertyChanged("DM601004");
            }
        }

        private string _dM601004Code;
        public string DM601004Code
        {
            get => _dM601004Code;
            set
            {
                _dM601004Code = value;
                RaisePropertyChanged("DM601004Code");
            }
        }

        private string _dM601004Name;
        public string DM601004Name
        {
            get => _dM601004Name;
            set
            {
                _dM601004Name = value;
                RaisePropertyChanged("DM601004Name");
            }
        }

        private string _dM601005;
        public string DM601005
        {
            get => _dM601005;
            set
            {
                _dM601005 = value;
                RaisePropertyChanged("DM601005");
            }
        }

        private string _dM601005Code;
        public string DM601005Code
        {
            get => _dM601005Code;
            set
            {
                _dM601005Code = value;
                RaisePropertyChanged("DM601005Code");
            }
        }

        private string _dM601005Name;
        public string DM601005Name
        {
            get => _dM601005Name;
            set
            {
                _dM601005Name = value;
                RaisePropertyChanged("DM601005Name");
            }
        }

        private string _sensorUnit;
        public string SensorUnit
        {
            get => _sensorUnit;
            set
            {
                _sensorUnit = value;
                RaisePropertyChanged("SensorUnit");
            }
        }

        /// <summary>
        /// 관리치 하한
        /// </summary>
        private float? _mLimitLow;
        public float? MLimitLow
        {
            get => _mLimitLow;
            set
            {
                _mLimitLow = value;
                RaisePropertyChanged("MLimitLow");
            }
        }

        /// <summary>
        /// 관리치 상한
        /// </summary>
        private float? _mLimitHigh;
        public float? MLimitHigh
        {
            get => _mLimitHigh;
            set
            {
                _mLimitHigh = value;
                RaisePropertyChanged("MLimitHigh");
            }
        }

        /// <summary>
        /// 한계치 하한
        /// </summary>
        private float? _limitLow;
        public float? LimitLow
        {
            get => _limitLow;
            set
            {
                _limitLow = value;
                RaisePropertyChanged("LimitLow");
            }
        }

        /// <summary>
        /// 한계치 상한
        /// </summary>
        private float? _limitHigh;
        public float? LimitHigh
        {
            get => _limitHigh;
            set
            {
                _limitHigh = value;
                RaisePropertyChanged("LimitHigh");
            }
        }

        private string _collectionValue;
        public string CollectionValue
        {
            get => _collectionValue;
            set
            {
                _collectionValue = value;
                RaisePropertyChanged("CollectionValue");
            }
        }

        private int? _alarmLevel = (int)AlarmState.None;
        public int? AlarmLevel
        {
            get => _alarmLevel;
            set
            {
                _alarmLevel = value;
                RaisePropertyChanged("AlarmLevel");
            }
        }

        private int? _blinkingLevel;
        public int? BlinkingLevel
        {
            get => _blinkingLevel;
            set
            {
                _blinkingLevel = value;
                RaisePropertyChanged("BlinkingLevel");
            }
        }

        private DateTime? _alarmDate;
        public DateTime? AlarmDate
        {
            get => _alarmDate;
            set
            {
                _alarmDate = value;
                RaisePropertyChanged("AlarmDate");
            }
        }
    }
}
