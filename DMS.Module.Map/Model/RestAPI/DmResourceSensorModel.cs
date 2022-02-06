using Newtonsoft.Json;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Module.Map.Model.RestAPI
{
    public class DmResourceSensorModel : DMViewModelBase
    {
        private string _resourceMstCode;
        public string ResourceMstCode
        {
            get => _resourceMstCode;
            set
            {
                _resourceMstCode = value;
                RaisePropertyChanged("ResourceMstCode");
            }
        }

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

        private string _sensorType;
        public string SensorType
        {
            get => _sensorType;
            set
            {
                _sensorType = value;
                RaisePropertyChanged("SensorType");
            }
        }

        private string _iconCode;
        public string IconCode
        {
            get => _iconCode;
            set
            {
                _iconCode = value;
                RaisePropertyChanged("IconCode");
            }
        }

        private string _iotCode;
        public string IotCode
        {
            get => _iotCode;
            set
            {
                _iotCode = value;
                RaisePropertyChanged("IotCode");
            }
        }

        private int? _sensorFloatDigit;
        public int? SensorFloatDigit
        {
            get => _sensorFloatDigit;
            set
            {
                _sensorFloatDigit = value;
                RaisePropertyChanged("SensorFloatDigit");
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

        private int? _dspSort;
        public int? DspSort
        {
            get => _dspSort;
            set
            {
                _dspSort = value;
                RaisePropertyChanged("DspSort");
            }
        }

        private string _remark;
        public string Remark
        {
            get => _remark;
            set
            {
                _remark = value;
                RaisePropertyChanged("Remark");
            }
        }

        private string _useYn;
        public string UseYn
        {
            get => _useYn;
            set
            {
                _useYn = value;
                RaisePropertyChanged("UseYn");
            }
        }

        private DateTime _regDate;
        [JsonIgnore]
        public DateTime RegDate
        {
            get => _regDate;
            set
            {
                _regDate = value;
                RaisePropertyChanged("RegDate");
            }
        }

        private string _regUser;
        public string RegUser
        {
            get => _regUser;
            set
            {
                _regUser = value;
                RaisePropertyChanged("RegUser");
            }
        }

        private DateTime _upDate;
        [JsonIgnore]
        public DateTime UpDate
        {
            get => _upDate;
            set
            {
                _upDate = value;
                RaisePropertyChanged("UpDate");
            }
        }

        private string _upUser;
        public string UpUser
        {
            get => _upUser;
            set
            {
                _upUser = value;
                RaisePropertyChanged("UpUser");
            }
        }
    }
}
