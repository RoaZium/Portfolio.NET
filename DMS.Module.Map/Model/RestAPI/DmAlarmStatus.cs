using PrismWPF.Common.MVVM;
using System;

namespace DMS.Module.Map.Model.RestAPI
{
    public class DmAlarmStatus : DMViewModelBase
    {
        private string _resourceCode;
        public string ResourceCode
        {
            get => _resourceCode;
            set
            {
                _resourceCode = value;
                RaisePropertyChanged("ResourceCode");
            }
        }

        private string _resourceName;
        public string ResourceName
        {
            get => _resourceName;
            set
            {
                _resourceName = value;
                RaisePropertyChanged("ResourceName");
            }
        }

        private long? _collectionId;
        public long? CollectionId
        {
            get => _collectionId;
            set
            {
                _collectionId = value;
                RaisePropertyChanged("CollectionId");
            }
        }

        /// <summary>
        /// AlarmDateTime
        /// </summary>
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

        private double? _collectionValue;
        public double? CollectionValue
        {
            get => _collectionValue;
            set
            {
                _collectionValue = value;
                RaisePropertyChanged("CollectionValue");
            }
        }

        private int? _alarmLevel;
        public int? AlarmLevel
        {
            get => _alarmLevel;
            set
            {
                _alarmLevel = value;
                RaisePropertyChanged("AlarmLevel");
            }
        }

        private int? _alarmProgress;
        public int? AlarmProgress
        {
            get => _alarmProgress;
            set
            {
                _alarmProgress = value;
                RaisePropertyChanged("AlarmProgress");
            }
        }

        private decimal? _mLimitLow;
        public decimal? MLimitLow
        {
            get => _mLimitLow;
            set
            {
                _mLimitLow = value;
                RaisePropertyChanged("MLimitLow");
            }
        }

        private decimal? _mLimitHigh;
        public decimal? MLimitHigh
        {
            get => _mLimitHigh;
            set
            {
                _mLimitHigh = value;
                RaisePropertyChanged("MLimitHigh");
            }
        }

        private decimal? _limitLow;
        public decimal? LimitLow
        {
            get => _limitLow;
            set
            {
                _limitLow = value;
                RaisePropertyChanged("LimitLow");
            }
        }

        private decimal? _limitHigh;
        public decimal? LimitHigh
        {
            get => _limitHigh;
            set
            {
                _limitHigh = value;
                RaisePropertyChanged("LimitHigh");
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
    }
}
