using Newtonsoft.Json;
using PrismWPF.Common.MVVM;
using System.Windows;

namespace DMS.Module.Management.Model.RestAPI
{
    public class DmComponentMst : DMViewModelBase
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

        private int _mapType;
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

        private double _coordinateX;
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
        private double _coordinateY;
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
        private int? _zindex;
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
                if (_propertyM == null)
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
    }
}
