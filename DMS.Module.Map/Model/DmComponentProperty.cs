using DMS.Module.Map.Model.Component;
using DMS.Module.Map.Model.Component.Gauge;
using DMS.Module.Map.Model.DataGrid;
using Newtonsoft.Json;
using Prism.Mvvm;
using PrismWPF.Common.Converter;
using System;
using System.Windows.Media;

namespace DMS.Module.Map.Model
{
    public class DmComponentProperty : BindableBase
    {
        #region 타겟/항목

        /// <summary>
        /// 분류 타입
        /// </summary>
        private string _categoryType;
        public string CategoryType
        {
            get => _categoryType;
            set
            {
                _categoryType = value;
                RaisePropertyChanged("CategoryType");
            }
        }

        /// <summary>
        /// 분류 코드
        /// </summary>
        private string _categoryCode;
        public string CategoryCode
        {
            get => _categoryCode;
            set
            {
                _categoryCode = value;
                RaisePropertyChanged("CategoryCode");
            }
        }

        /// <summary>
        /// 분류 이름
        /// </summary>
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                _categoryName = value;
                RaisePropertyChanged("CategoryName");
            }
        }

        /// <summary>
        /// 항목 타입
        /// </summary>
        private string _itemType;
        public string ItemType
        {
            get => _itemType;
            set
            {
                _itemType = value;
                RaisePropertyChanged("ItemType");
            }
        }

        /// <summary>
        /// 항목 코드
        /// </summary>
        private string _itemCode;
        public string ItemCode
        {
            get => _itemCode;
            set
            {
                _itemCode = value;
                RaisePropertyChanged("ItemCode");
            }
        }

        /// <summary>
        /// 항목 이름
        /// </summary>
        private string _itemName;
        public string ItemName
        {
            get => _itemName;
            set
            {
                _itemName = value;
                RaisePropertyChanged("ItemName");
            }
        }

        private double? _zoom = 1;
        public double? Zoom
        {
            get => _zoom;
            set
            {
                _zoom = Math.Truncate((double)value * 100) / 100;
                RaisePropertyChanged("Zoom");
            }
        }

        #endregion

        private AlarmStateModel _alarmStateM;
        public AlarmStateModel AlarmStateM
        {
            get => _alarmStateM;
            set
            {
                _alarmStateM = value;
                RaisePropertyChanged("AlarmStateM");
            }
        }

        private AnimationModel _animationM;
        public AnimationModel AnimationM
        {
            get => _animationM;
            set
            {
                _animationM = value;
                RaisePropertyChanged("AnimationM");
            }
        }

        private ArcModel _arcM;
        public ArcModel ArcM
        {
            get => _arcM;
            set
            {
                _arcM = value;
                RaisePropertyChanged("ArcM");
            }
        }

        private AutoModeModel _autoModeM;
        public AutoModeModel AutoModeM
        {
            get => _autoModeM;
            set
            {
                _autoModeM = value;
                RaisePropertyChanged("AutoModeM");
            }
        }

        private ArcPanel _arcPanelM;
        public ArcPanel ArcPanelM
        {
            get => _arcPanelM;
            set
            {
                _arcPanelM = value;
                RaisePropertyChanged("ArcPanelM");
            }
        }

        private StartLabelModel _startLabelM;
        public StartLabelModel StartLabelM
        {
            get => _startLabelM;
            set
            {
                _startLabelM = value;
                RaisePropertyChanged("StartLabelM");
            }
        }

        private StartLabelModel _endLabelM;
        public StartLabelModel EndLabelM
        {
            get => _endLabelM;
            set
            {
                _endLabelM = value;
                RaisePropertyChanged("EndLabelM");
            }
        }

        private AxisX2DModel _axisX2DM;
        public AxisX2DModel AxisX2DM
        {
            get => _axisX2DM;
            set
            {
                _axisX2DM = value;
                RaisePropertyChanged("AxisX2DM");
            }
        }

        private AxisXLabelModel _axisXLabelM;
        public AxisXLabelModel AxisXLabelM
        {
            get => _axisXLabelM;
            set
            {
                _axisXLabelM = value;
                RaisePropertyChanged("AxisXLabelM");
            }
        }

        private AxisXRangeModel _axisXRangeM;
        public AxisXRangeModel AxisXRangeM
        {
            get => _axisXRangeM;
            set
            {
                _axisXRangeM = value;
                RaisePropertyChanged("AxisXRangeM");
            }
        }

        private AxisXTitleModel _axisXTitleM;
        public AxisXTitleModel AxisXTitleM
        {
            get => _axisXTitleM;
            set
            {
                _axisXTitleM = value;
                RaisePropertyChanged("AxisXTitleM");
            }
        }

        private AxisY2DModel _axisY2DM;
        public AxisY2DModel AxisY2DM
        {
            get => _axisY2DM;
            set
            {
                _axisY2DM = value;
                RaisePropertyChanged("AxisY2DM");
            }
        }

        private AxisYLabelModel _axisYLabelM;
        public AxisYLabelModel AxisYLabelM
        {
            get => _axisYLabelM;
            set
            {
                _axisYLabelM = value;
                RaisePropertyChanged("AxisYLabelM");
            }
        }

        private AxisYRangeModel _axisYRangeM;
        public AxisYRangeModel AxisYRangeM
        {
            get => _axisYRangeM;
            set
            {
                _axisYRangeM = value;
                RaisePropertyChanged("AxisYRangeM");
            }
        }

        private AxisYTitleModel _axisYTitleM;
        public AxisYTitleModel AxisYTitleM
        {
            get => _axisYTitleM;
            set
            {
                _axisYTitleM = value;
                RaisePropertyChanged("AxisYTitleM");
            }
        }

        private BarSideBySideSeries2DModel _barSideBySideSeries2DM;
        public BarSideBySideSeries2DModel BarSideBySideSeries2DM
        {
            get => _barSideBySideSeries2DM;
            set
            {
                _barSideBySideSeries2DM = value;
                RaisePropertyChanged("BarSideBySideSeries2DM");
            }
        }

        private ChartControlModel _chartControlM;
        public ChartControlModel ChartControlM
        {
            get => _chartControlM;
            set
            {
                _chartControlM = value;
                RaisePropertyChanged("ChartControlM");
            }
        }

        private DataBoxModel _dataBoxM;
        public DataBoxModel DataBoxM
        {
            get => _dataBoxM;
            set
            {
                _dataBoxM = value;
                RaisePropertyChanged("DataBoxM");
            }
        }

        private DataGridCellModel _dataGridCellM;
        public DataGridCellModel DataGridCellM
        {
            get => _dataGridCellM;
            set
            {
                _dataGridCellM = value;
                RaisePropertyChanged("DataGridCellM");
            }
        }

        private DataGridColumnHeaderModel _dataGridColumnHeaderM;
        public DataGridColumnHeaderModel DataGridColumnHeaderM
        {
            get => _dataGridColumnHeaderM;
            set
            {
                _dataGridColumnHeaderM = value;
                RaisePropertyChanged("DataGridColumnHeaderM");
            }
        }

        private DataGridModel _dataGridM;
        public DataGridModel DataGridM
        {
            get => _dataGridM;
            set
            {
                _dataGridM = value;
                RaisePropertyChanged("DataGridM");
            }
        }

        private DataGridRowModel _dataGridRowM;
        public DataGridRowModel DataGridRowM
        {
            get => _dataGridRowM;
            set
            {
                _dataGridRowM = value;
                RaisePropertyChanged("DataGridRowM");
            }
        }

        private DetailInfoModel _detailInfoM;
        public DetailInfoModel DetailInfoM
        {
            get => _detailInfoM;
            set
            {
                _detailInfoM = value;
                RaisePropertyChanged("DetailInfoM");
            }
        }

        private DetailInfoTitleModel _detailInfoTitleM;
        public DetailInfoTitleModel DetailInfoTitleM
        {
            get => _detailInfoTitleM;
            set
            {
                _detailInfoTitleM = value;
                RaisePropertyChanged("DetailInfoTitleM");
            }
        }

        private DetailInfoValueModel _detailInfoValueM;
        public DetailInfoValueModel DetailInfoValueM
        {
            get => _detailInfoValueM;
            set
            {
                _detailInfoValueM = value;
                RaisePropertyChanged("DetailInfoValueM");
            }
        }

        private GifModel _gifM;
        public GifModel GifM
        {
            get => _gifM;
            set
            {
                _gifM = value;
                RaisePropertyChanged("GifM");
            }
        }

        private ImageViewerModel _imageViewerM;
        public ImageViewerModel ImageViewerM
        {
            get => _imageViewerM;
            set
            {
                _imageViewerM = value;
                RaisePropertyChanged("ImageViewerM");
            }
        }

        private LayoutModel _layoutM;
        public LayoutModel LayoutM
        {
            get => _layoutM;
            set
            {
                _layoutM = value;
                RaisePropertyChanged("LayoutM");
            }
        }

        private LegendModel _legendM;
        public LegendModel LegendM
        {
            get => _legendM;
            set
            {
                _legendM = value;
                RaisePropertyChanged("LegendM");
            }
        }

        private LineSeries2DModel _lineSeries2DM;
        public LineSeries2DModel LineSeries2DM
        {
            get => _lineSeries2DM;
            set
            {
                _lineSeries2DM = value;
                RaisePropertyChanged("LineSeries2DM");
            }
        }

        private MoveModel _moveM;
        public MoveModel MoveM
        {
            get => _moveM;
            set
            {
                _moveM = value;
                RaisePropertyChanged("MoveM");
            }
        }

        private NestedDonutSeries2DModel _nestedDonutSeries2DM;
        public NestedDonutSeries2DModel NestedDonutSeries2DM
        {
            get => _nestedDonutSeries2DM;
            set
            {
                _nestedDonutSeries2DM = value;
                RaisePropertyChanged("NestedDonutSeries2DM");
            }
        }

        private PaneModel _paneM;
        public PaneModel PaneM
        {
            get => _paneM;
            set
            {
                _paneM = value;
                RaisePropertyChanged("PaneM");
            }
        }

        private PieSeries2DModel _pieSeries2DM;
        public PieSeries2DModel PieSeries2DM
        {
            get => _pieSeries2DM;
            set
            {
                _pieSeries2DM = value;
                RaisePropertyChanged("PieSeries2DM");
            }
        }

        private PieTotalLabelModel _pieTotalLabelM;
        public PieTotalLabelModel PieTotalLabelM
        {
            get => _pieTotalLabelM;
            set
            {
                _pieTotalLabelM = value;
                RaisePropertyChanged("PieTotalLabelM");
            }
        }

        private PlaybackModel _playbackM;
        public PlaybackModel PlaybackM
        {
            get => _playbackM;
            set
            {
                _playbackM = value;
                RaisePropertyChanged("PlaybackM");
            }
        }

        private PointSeries2DModel _pointSeries2DM;
        public PointSeries2DModel PointSeries2DM
        {
            get => _pointSeries2DM;
            set
            {
                _pointSeries2DM = value;
                RaisePropertyChanged("PieSeries2DM");
            }
        }

        private ProgressBarModel _progressBarM;
        public ProgressBarModel ProgressBarM
        {
            get => _progressBarM;
            set
            {
                _progressBarM = value;
                RaisePropertyChanged("ProgressBarM");
            }
        }

        private SeriesLabelModel _seriesLabelM;
        public SeriesLabelModel SeriesLabelM
        {
            get => _seriesLabelM;
            set
            {
                _seriesLabelM = value;
                RaisePropertyChanged("PieSeries2DM");
            }
        }

        private SimpleDiagram2DModel _simpleDiagram2DM;
        public SimpleDiagram2DModel SimpleDiagram2DM
        {
            get => _simpleDiagram2DM;
            set
            {
                _simpleDiagram2DM = value;
                RaisePropertyChanged("SimpleDiagram2DM");
            }
        }

        private TextBoxModel _textBoxM;
        public TextBoxModel TextBoxM
        {
            get => _textBoxM;
            set
            {
                _textBoxM = value;
                RaisePropertyChanged("TextBoxM");
            }
        }

        private TitleModel _titleM;
        public TitleModel TitleM
        {
            get => _titleM;
            set
            {
                _titleM = value;
                RaisePropertyChanged("TitleM");
            }
        }

        private TotalValueModel _totalValueM;
        public TotalValueModel TotalValueM
        {
            get => _totalValueM;
            set
            {
                _totalValueM = value;
                RaisePropertyChanged("TotalValueM");
            }
        }

        private XYDiagram2DModel _xYDiagram2DM;
        public XYDiagram2DModel XYDiagram2DM
        {
            get => _xYDiagram2DM;
            set
            {
                _xYDiagram2DM = value;
                RaisePropertyChanged("XYDiagram2DM");
            }
        }

        #region 레이아웃

        /// <summary>
        /// 배경색
        /// </summary>
        private string _layoutBackground;
        public string LayoutBackground
        {
            get => _layoutBackground;
            set
            {
                _layoutBackground = value;
                RaisePropertyChanged("LayoutBackground");
            }
        }

        #endregion

        /// <summary>
        /// IP카메라 코드
        /// </summary>
        private string _targetCode;
        public string TargetCode
        {
            get => _targetCode;
            set
            {
                _targetCode = value;
                RaisePropertyChanged("TargetCode");
            }
        }

        /// <summary>
        /// 타겟 경로
        /// </summary>
        private string _targetPath;
        [JsonIgnore]
        public string TargetPath
        {
            get => _targetPath;
            set
            {
                _targetPath = value;
                RaisePropertyChanged("TargetPath");
            }
        }

        /// <summary>
        /// 아이콘 코드
        /// </summary>
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

        /// <summary>
        /// 모양 크기
        /// </summary>
        private int? _shapeSize;
        public int? ShapeSize
        {
            get => _shapeSize;
            set
            {
                _shapeSize = value;
                RaisePropertyChanged("ShapeSize");
            }
        }


        #region 알람 상태 정보(점멸 색상, 점멸 속도)

        /// <summary>
        /// 알람 점멸 속도
        /// </summary>
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

        /// <summary>
        /// 알람 이상 상태(정상, 이상, 위험, 정지)
        /// </summary>
        private AlarmState? _AlarmState;
        public AlarmState? AlarmState
        {
            get => _AlarmState;
            set
            {
                _AlarmState = value;
                RaisePropertyChanged("AlarmState");
            }
        }

        #endregion

        #region 이미지 정보(바이너리)

        /// <summary>
        /// 이미지 경로
        /// </summary>
        private string _imagePath;
        [JsonIgnore]
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }

        private string _imageName;
        public string ImageName
        {
            get => _imageName;
            set
            {
                _imageName = value;
                RaisePropertyChanged("ImageName");
            }
        }

        /// <summary>
        /// 바이너리 정보
        /// </summary>
        private byte[] _imagebinary;
        [JsonIgnore]
        public byte[] Imagebinary
        {
            get => _imagebinary;
            set
            {
                _imagebinary = value;
                RaisePropertyChanged("Imagebinary");
            }
        }

        /// <summary>
        /// 이미지 정보
        /// </summary>
        private ImageSource _imageSource;
        [JsonIgnore]
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                RaisePropertyChanged("ImageSource");
            }
        }

        #endregion


        /// <summary>
        /// 텍스트 내용
        /// </summary>
        private string _textContent;
        public string TextContent
        {
            get => _textContent;
            set
            {
                _textContent = value;
                RaisePropertyChanged("TextContent");
            }
        }

        /// <summary>
        /// 파일 경로
        /// </summary>
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                RaisePropertyChanged("FilePath");
            }
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        private string _alarmCode;
        public string AlarmCode
        {
            get => _alarmCode;
            set => _alarmCode = value;
        }

        private bool _isAlarmLayout;
        public bool IsAlarmLayout
        {
            get => _isAlarmLayout;
            set
            {
                _isAlarmLayout = value;
                RaisePropertyChanged("IsAlarmLayout");
            }
        }
    }
}
