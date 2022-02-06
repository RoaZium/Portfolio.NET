using PrismWPF.Common.MVVM;

namespace DMS.Module.Map.Model
{
    public class DmChartDataModel : DMViewModelBase
    {
        /// <summary>
        /// 카테고리 이름
        /// </summary>
        private string _seriesNameMember;
        public string SeriesNameMember
        {
            get => _seriesNameMember;
            set
            {
                _seriesNameMember = value;
                RaisePropertyChanged("SeriesNameMember");
            }
        }

        /// <summary>
        /// 카테고리 구분
        /// </summary>
        private string _seriesDataMember;
        public string SeriesDataMember

        {
            get => _seriesDataMember;
            set
            {
                _seriesDataMember = value;
                RaisePropertyChanged("SeriesDataMember");
            }
        }

        /// <summary>
        /// 카테고리 타입
        /// </summary>
        private string _seriesScaleType;
        public string SeriesScaleType
        {
            get => _seriesScaleType;
            set
            {
                _seriesScaleType = value;
                RaisePropertyChanged("SeriesScaleType");
            }
        }

        /// <summary>
        /// 인자 이름
        /// </summary>
        private string _argumentNameMember;
        public string ArgumentNameMember
        {
            get => _argumentNameMember;
            set
            {
                _argumentNameMember = value;
                RaisePropertyChanged("ArgumentNameMember");
            }
        }

        /// <summary>
        /// 인자 값
        /// </summary>
        private string _argumentDataMember;
        public string ArgumentDataMember
        {
            get => _argumentDataMember;
            set
            {
                _argumentDataMember = value;
                RaisePropertyChanged("ArgumentDataMember");
            }
        }

        /// <summary>
        /// 인자 타입
        /// </summary>
        private string _argumentScaleType;
        public string ArgumentScaleType
        {
            get => _argumentScaleType;
            set
            {
                _argumentScaleType = value;
                RaisePropertyChanged("ArgumentScaleType");
            }
        }

        /// <summary>
        /// 값 이름
        /// </summary>
        private string _valueNameMember;
        public string ValueNameMember
        {
            get => _valueNameMember;
            set
            {
                _valueNameMember = value;
                RaisePropertyChanged("ValueNameMember");
            }
        }

        /// <summary>
        /// 값
        /// </summary>
        private string _valueDataMember;
        public string ValueDataMember
        {
            get => _valueDataMember;
            set
            {
                _valueDataMember = value;
                RaisePropertyChanged("ValueDataMember");
            }
        }

        /// <summary>
        /// 값 타입
        /// </summary>
        private string _valueScaleMember;
        public string ValueScaleMember
        {
            get => _valueScaleMember;
            set
            {
                _valueScaleMember = value;
                RaisePropertyChanged("ValueScaleMember");
            }
        }

        /// <summary>
        /// 값 이름
        /// </summary>
        private string _colorNameMember;
        public string ColorNameMember
        {
            get => _colorNameMember;
            set
            {
                _colorNameMember = value;
                RaisePropertyChanged("ColorNameMember");
            }
        }

        /// <summary>
        /// 값
        /// </summary>
        private string _colorDataMember;
        public string ColorDataMember
        {
            get => _colorDataMember;
            set
            {
                _colorDataMember = value;
                RaisePropertyChanged("ColorDataMember");
            }
        }

        /// <summary>
        /// 값 타입
        /// </summary>
        private string _colorScaleMember;
        public string ColorScaleMember
        {
            get => _colorScaleMember;
            set
            {
                _colorScaleMember = value;
                RaisePropertyChanged("ColorScaleMember");
            }
        }
    }
}
