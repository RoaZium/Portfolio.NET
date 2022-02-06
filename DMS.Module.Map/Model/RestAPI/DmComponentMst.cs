using DMS.Module.Map.Infrastructure;
using Newtonsoft.Json;
using PrismWPF.Common.MVVM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows;

namespace DMS.Module.Map.Model.RestAPI
{
    public class DmComponentMst : DMViewModelBase
    {
        private ActionType _actionType = ActionType.N;
        [JsonIgnore]
        public ActionType Action
        {
            get => _actionType;
            set
            {
                _actionType = value;
                RaisePropertyChanged("Action");
            }
        }

        private int _userId = 0;
        public int UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        private int _mapType = 1;
        public int MapType
        {
            get => _mapType;
            set
            {
                _mapType = value;
                RaisePropertyChanged("MapType");
            }
        }

        private string _routingCode;
        public string RoutingCode
        {
            get => _routingCode;
            set
            {
                _routingCode = value;
                RaisePropertyChanged("RoutingCode");
            }
        }

        private string _pRoutingCode;
        public string PRoutingCode
        {
            get => _pRoutingCode;
            set
            {
                _pRoutingCode = value;
                RaisePropertyChanged("PRoutingCode");
            }
        }

        private string _routingType;
        public string RoutingType
        {
            get => _routingType;
            set
            {
                _routingType = value;
                RaisePropertyChanged("RoutingType");
            }
        }

        private string _routingName;
        public string RoutingName
        {
            get => _routingName;
            set
            {
                _routingName = value;
                RaisePropertyChanged("RoutingName");
            }
        }

        private string _baseInfoCode;
        public string BaseInfoCode
        {
            get => PropertyM.ItemCode;
            set => _baseInfoCode = PropertyM.ItemCode;
        }

        private double _coordinateX = 0;
        public double CoordinateX
        {
            get => _coordinateX;
            set
            {
                _coordinateX = value;
                RaisePropertyChanged("CoordinateX");
                RaisePropertyChanged("Margin");
            }
        }

        /// <summary>
		/// 수직 위치
		/// </summary>
        private double _coordinateY = 0;
        public double CoordinateY
        {
            get => _coordinateY;
            set
            {
                _coordinateY = value;
                RaisePropertyChanged("CoordinateY");
                RaisePropertyChanged("Margin");
            }
        }

        /// <summary>
        /// Z 좌표
        /// </summary>
        private int? _zindex = 0;
        public int? Zindex
        {
            get => _zindex;
            set
            {
                _zindex = value;
                RaisePropertyChanged("Zindex");
            }
        }

        /// <summary>
		/// 높이
		/// </summary>
        private float? _height = 150;
        public float? Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
            }
        }

        /// <summary>
        /// 너비
        /// </summary>
        private float? _width = 150;
        public float? Width
        {
            get => _width;
            set
            {
                _width = value;
                RaisePropertyChanged("Width");
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

        private Thickness _Margin;
        public Thickness Margin
        {
            get => new Thickness(CoordinateX, CoordinateY, 0, 0);
            set
            {
                _Margin = value;

                CoordinateX = value.Left;
                CoordinateY = value.Top;

                RaisePropertyChanged("Margin");
            }
        }

        private bool _IsEditMode;
        public bool IsEditMode
        {
            get => _IsEditMode;
            set
            {
                _IsEditMode = value;
                RaisePropertyChanged("IsEditMode");
            }
        }

        private Visibility _isSelectMode = Visibility.Collapsed;
        public Visibility IsSelectMode
        {
            get => _isSelectMode;
            set
            {
                _isSelectMode = value;
                RaisePropertyChanged("IsSelectMode");
            }
        }

        private Visibility _isVisible = Visibility.Collapsed;
        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        private string _componentProperty;
        public string ComponentProperty
        {
            get => _componentProperty;
            set => _componentProperty = value;
        }

        private DmComponentProperty _propertyM;
        public DmComponentProperty PropertyM
        {
            get
            {
                if(_propertyM == null)
                {
                    _propertyM = new DmComponentProperty();
                }
                return _propertyM;
            }
            set
            {
                _propertyM = value;
                RaisePropertyChanged("PropertyM");
            }
        }

        private DmResourceSensorModel _resourceM;
        public DmResourceSensorModel ResourceM
        {
            get
            {
                if (_resourceM == null)
                {
                    _resourceM = new DmResourceSensorModel();
                }
                return _resourceM;
            }
            set
            {
                _resourceM = value;
                RaisePropertyChanged("ResourceM");
            }
        }

        public ObservableCollection<DmChartDataModel> _chartSeriesList;
        public ObservableCollection<DmChartDataModel> ChartSeriesList
        {
            get
            {
                if (_chartSeriesList == null)
                {
                    _chartSeriesList = new ObservableCollection<DmChartDataModel>();
                }
                return _chartSeriesList;
            }
            set
            {
                _chartSeriesList = value;
                RaisePropertyChanged("ChartSeriesList");
            }
        }

        private DataTable _dataGridList;
        public DataTable DataGridList
        {
            get
            {
                if (_dataGridList == null)
                {
                    _dataGridList = new DataTable();
                }
                return _dataGridList;
            }
            set
            {
                _dataGridList = value;
                RaisePropertyChanged("DataGridList");
            }
        }

        private ObservableCollection<DmAlarmStatus> _dmAlarmStatusList;
        public ObservableCollection<DmAlarmStatus> DmAlarmStatusList
        {
            get
            {
                if(_dmAlarmStatusList == null)
                {
                    _dmAlarmStatusList = new ObservableCollection<DmAlarmStatus>();
                }
                return _dmAlarmStatusList;
            }
            set
            {
                _dmAlarmStatusList = value;
                RaisePropertyChanged("DmAlarmStatusList");
            }
        }
    }
}
