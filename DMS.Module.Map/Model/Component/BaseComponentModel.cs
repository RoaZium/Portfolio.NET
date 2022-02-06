using Prism.Mvvm;

namespace DMS.Module.Map.Model.Component
{
    public class BaseComponentModel : BindableBase
    {
        /// <summary>
        /// 축 정렬
        /// </summary>
        private string _alignment;
        public virtual string Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                RaisePropertyChanged("Alignment");
            }
        }

        /// <summary>
        /// 회전 허용
        /// </summary>
        private string _allowRotate;
        public virtual string AllowRotate
        {
            get => _allowRotate;
            set
            {
                _allowRotate = value;
                RaisePropertyChanged("AllowRotate");
            }
        }

        /// <summary>
        /// 비틀기 허용
        /// </summary>
        private string _allowStagger;
        public virtual string AllowStagger
        {
            get => _allowStagger;
            set
            {
                _allowStagger = value;
                RaisePropertyChanged("AllowStagger");
            }
        }

        /// <summary>
        /// 각도
        /// </summary>
        private string _angle;
        public virtual string Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                RaisePropertyChanged("Angle");
            }
        }

        /// <summary>
        /// 애니메이션 모드
        /// </summary>
        private string _animationMode;
        public virtual string AnimationMode
        {
            get => _animationMode;
            set
            {
                _animationMode = value;
                RaisePropertyChanged("AnimationMode");
            }
        }

        /// <summary>
        /// GIF Speed
        /// </summary>
        private double? _animationSpeedRatio;
        public virtual double? AnimationSpeedRatio
        {
            get => _animationSpeedRatio;
            set
            {
                _animationSpeedRatio = value;
                RaisePropertyChanged("AnimationSpeedRatio");
            }
        }

        /// <summary>
        /// 타원 두께
        /// </summary>
        private string _arcThickness;
        public virtual string ArcThickness
        {
            get => _arcThickness;
            set
            {
                _arcThickness = value;
                RaisePropertyChanged("ArcThickness");
            }
        }

        /// <summary>
        /// 타원 타입
        /// </summary>
        private string _arcThicknessUnit;
        public virtual string ArcThicknessUnit
        {
            get => _arcThicknessUnit;
            set
            {
                _arcThicknessUnit = value;
                RaisePropertyChanged("ArcThicknessUnit");
            }
        }

        /// <summary>
        /// 자동 컬럼 생성
        /// </summary>
        private string _autoGenerateColumns;
        public virtual string AutoGenerateColumns
        {
            get => _autoGenerateColumns;
            set
            {
                _autoGenerateColumns = value;
                RaisePropertyChanged("AutoGenerateColumns");
            }
        }

        /// <summary>
        /// 자동 그리드
        /// </summary>
        private string _autoGrid;
        public virtual string AutoGrid
        {
            get => _autoGrid;
            set
            {
                _autoGrid = value;
                RaisePropertyChanged("AutoGrid");
            }
        }

        /// <summary>
        /// 적응형 레이아웃
        /// </summary>
        private string _autoLayout;
        public virtual string AutoLayout
        {
            get => _autoLayout;
            set
            {
                _autoLayout = value;
                RaisePropertyChanged("AutoLayout");
            }
        }

        /// <summary>
        /// 측면 여백 자동 조정
        /// </summary>
        private bool? _autoSideMargins;
        public virtual bool? AutoSideMargins
        {
            get => _autoSideMargins;
            set
            {
                _autoSideMargins = value;
                RaisePropertyChanged("AutoSideMargins");
            }
        }

        /// <summary>
        /// 배경색
        /// </summary>
        private string _background;
        public virtual string Background
        {
            get => _background;
            set
            {
                _background = value;
                RaisePropertyChanged("Background");
            }
        }

        /// <summary>
        /// 배경색 알람
        /// </summary>
        private bool? _backgroundAlarmVisibility;
        public virtual bool? BackgroundAlarmVisibility
        {
            get => _backgroundAlarmVisibility;
            set
            {
                _backgroundAlarmVisibility = value;
                RaisePropertyChanged("BackgroundAlarmVisibility");
            }
        }

        /// <summary>
        /// 바 거리
        /// </summary>
        private string _barDistance;
        public virtual string BarDistance
        {
            get => _barDistance;
            set
            {
                _barDistance = value;
                RaisePropertyChanged("BarDistance");
            }
        }

        /// <summary>
        /// 막대 너비
        /// </summary>
        private string _barWidth;
        public virtual string BarWidth
        {
            get => _barWidth;
            set
            {
                _barWidth = value;
                RaisePropertyChanged("BarWidth");
            }
        }

        /// <summary>
        /// 테두리 알람
        /// </summary>
        private bool? _borderAlarmVisibility;
        public virtual bool? BorderAlarmVisibility
        {
            get => _borderAlarmVisibility;
            set
            {
                _borderAlarmVisibility = value;
                RaisePropertyChanged("BorderAlarmVisibility");
            }
        }

        /// <summary>
        /// 테두리 색상
        /// </summary>
        private string _borderBrush;
        public virtual string BorderBrush
        {
            get => _borderBrush;
            set
            {
                _borderBrush = value;
                RaisePropertyChanged("BorderBrush");
            }
        }

        /// <summary>
        /// 테두리 두께
        /// </summary>
        private int? _borderThickness;
        public virtual int? BorderThickness
        {
            get => _borderThickness;
            set
            {
                _borderThickness = value;
                RaisePropertyChanged("BorderThickness");
            }
        }

        /// <summary>
        /// 색상
        /// </summary>
        private string _brush;
        public virtual string Brush
        {
            get => _brush;
            set
            {
                _brush = value;
                RaisePropertyChanged("Brush");
            }
        }

        /// <summary>
        /// 테두리 알람
        /// </summary>
        private bool? _brushAlarmVisibility;
        public virtual bool? BrushAlarmVisibility
        {
            get => _brushAlarmVisibility;
            set
            {
                _brushAlarmVisibility = value;
                RaisePropertyChanged("BrushAlarmVisibility");
            }
        }

        /// <summary>
        /// Row 데이터 추가 유무
        /// </summary>
        private string _canUserAddRows;
        public virtual string CanUserAddRows
        {
            get => _canUserAddRows;
            set
            {
                _canUserAddRows = value;
                RaisePropertyChanged("CanUserAddRows");
            }
        }

        /// <summary>
        /// Row Resize 유무
        /// </summary>
        private string _canUserResizeRows;
        public virtual string CanUserResizeRows
        {
            get => _canUserResizeRows;
            set
            {
                _canUserResizeRows = value;
                RaisePropertyChanged("CanUserResizeRows");
            }
        }

        /// <summary>
        /// X축 중심
        /// </summary>
        private string _centerX;
        public virtual string CenterX
        {
            get => _centerX;
            set
            {
                _centerX = value;
                RaisePropertyChanged("CenterX");
            }
        }

        /// <summary>
        /// Y축 중심
        /// </summary>
        private string _centerY;
        public virtual string CenterY
        {
            get => _centerY;
            set
            {
                _centerY = value;
                RaisePropertyChanged("CenterY");
            }
        }

        /// <summary>
        /// 범례 체크박스 사용 여부
        /// </summary>
        private string _checkableInLegend;
        public virtual string CheckableInLegend
        {
            get => _checkableInLegend;
            set
            {
                _checkableInLegend = value;
                RaisePropertyChanged("CheckableInLegend");
            }
        }

        /// <summary>
        /// 범례 체크 가능 여부
        /// </summary>
        private string _checkedInLegend;
        public virtual string CheckedInLegend
        {
            get => _checkedInLegend;
            set
            {
                _checkedInLegend = value;
                RaisePropertyChanged("CheckedInLegend");
            }
        }

        /// <summary>
        /// 컬럼 너비
        /// </summary>
        private string _columnWidth;
        public virtual string ColumnWidth
        {
            get => _columnWidth;
            set
            {
                _columnWidth = value;
                RaisePropertyChanged("ColumnWidth");
            }
        }

        /// <summary>
        /// 연결 선 두께
        /// </summary>
        private string _connectorThickness;
        public virtual string ConnectorThickness
        {
            get => _connectorThickness;
            set
            {
                _connectorThickness = value;
                RaisePropertyChanged("ConnectorThickness");
            }
        }

        /// <summary>
        /// 연결 선 보이기 여부
        /// </summary>
        private string _connectorVisible;
        public virtual string ConnectorVisible
        {
            get => _connectorVisible;
            set
            {
                _connectorVisible = value;
                RaisePropertyChanged("ConnectorVisible");
            }
        }

        /// <summary>
        /// 내용
        /// </summary>
        private string _content;
        public virtual string Content
        {
            get => _content;
            set
            {
                _content = value;
                RaisePropertyChanged("Content");
            }
        }

        /// <summary>
        /// 코너 반경
        /// </summary>
        private string _cornerRadius;
        public virtual string CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                RaisePropertyChanged("CornerRadius");
            }
        }

        /// <summary>
        /// 십자형 커서 사용 여부
        /// </summary>
        private string _crosshairEnabled;
        public virtual string CrosshairEnabled
        {
            get => _crosshairEnabled;
            set
            {
                _crosshairEnabled = value;
                RaisePropertyChanged("CrosshairEnabled");
            }
        }

        /// <summary>
        /// 십자형 커서 텍스트 패턴
        /// </summary>
        private string _crosshairLabelPattern;
        public virtual string CrosshairLabelPattern
        {
            get => _crosshairLabelPattern;
            set
            {
                _crosshairLabelPattern = value;
                RaisePropertyChanged("CrosshairLabelPattern");
            }
        }

        /// <summary>
        /// 십자형 커서 보이기 유무
        /// </summary>
        private string _crosshairLabelVisibility;
        public virtual string CrosshairLabelVisibility
        {
            get => _crosshairLabelVisibility;
            set
            {
                _crosshairLabelVisibility = value;
                RaisePropertyChanged("CrosshairLabelVisibility");
            }
        }

        /// <summary>
        /// 대시
        /// </summary>
        private string _dashes;
        public virtual string Dashes
        {
            get => _dashes;
            set
            {
                _dashes = value;
                RaisePropertyChanged("Dashes");
            }
        }

        /// <summary>
        /// 데이터 타입
        /// </summary>
        private string _dataType;
        public virtual string DataType
        {
            get => _dataType;
            set
            {
                _dataType = value;
                RaisePropertyChanged("DataType");
            }
        }

        /// <summary>
        /// Y축 범위 고정
        /// </summary>
        private string _dependentAxesYRange;
        public virtual string DependentAxesYRange
        {
            get => _dependentAxesYRange;
            set
            {
                _dependentAxesYRange = value;
                RaisePropertyChanged("DependentAxesYRange");
            }
        }

        /// <summary>
        /// 패널-테두리 색상
        /// </summary>
        private string _domainBorderBrush;
        public virtual string DomainBorderBrush
        {
            get => _domainBorderBrush;
            set
            {
                _domainBorderBrush = value;
                RaisePropertyChanged("DomainBorderBrush");
            }
        }

        /// <summary>
        /// 패널-배경색
        /// </summary>
        private string _domainBrush;
        public virtual string DomainBrush
        {
            get => _domainBrush;
            set
            {
                _domainBrush = value;
                RaisePropertyChanged("DomainBrush");
            }
        }

        /// <summary>
        /// X축 네비게이션 활성화 여부
        /// </summary>
        private string _enableAxisXNavigation;
        public virtual string EnableAxisXNavigation
        {
            get => _enableAxisXNavigation;
            set
            {
                _enableAxisXNavigation = value;
                RaisePropertyChanged("EnableAxisXNavigation");
            }
        }

        /// <summary>
        /// Y축 네비게이션 활성화 여부
        /// </summary>
        private string _enableAxisYNavigation;
        public virtual string EnableAxisYNavigation
        {
            get => _enableAxisYNavigation;
            set
            {
                _enableAxisYNavigation = value;
                RaisePropertyChanged("EnableAxisYNavigation");
            }
        }

        /// <summary>
        /// 끝 지점 Angle 값
        /// </summary>
        private string _endAngle;
        public virtual string EndAngle
        {
            get => _endAngle;
            set
            {
                _endAngle = value;
                RaisePropertyChanged("EndAngle");
            }
        }

        /// <summary>
        /// 동일 바 너비
        /// </summary>
        private string _equalBarWidth;
        public virtual string EqualBarWidth
        {
            get => _equalBarWidth;
            set
            {
                _equalBarWidth = value;
                RaisePropertyChanged("EqualBarWidth");
            }
        }

        /// <summary>
        /// 흘러가는 방향
        /// </summary>
        private string _flowDirection;
        public virtual string FlowDirection
        {
            get => _flowDirection;
            set
            {
                _flowDirection = value;
                RaisePropertyChanged("FlowDirection");
            }
        }

        /// <summary>
        /// 가득
        /// </summary>
        private string _fill;
        public virtual string Fill
        {
            get => _fill;
            set
            {
                _fill = value;
                RaisePropertyChanged("Fill");
            }
        }

        /// <summary>
        /// 글꼴 크기
        /// </summary>
        private string _fontSize;
        public virtual string FontSize
        {
            get => _fontSize;
            set
            {
                _fontSize = value;
                RaisePropertyChanged("FontSize");
            }
        }

        /// <summary>
        /// 글꼴 스타일
        /// </summary>
        private string _fontStyle;
        public virtual string FontStyle
        {
            get => _fontStyle;
            set
            {
                _fontStyle = value;
                RaisePropertyChanged("FontStyle");
            }
        }
            
        /// <summary>
        /// 글씨 두께
        /// </summary>
        private string _fontWeight;
        public virtual string FontWeight
        {
            get => _fontWeight;
            set
            {
                _fontWeight = value;
                RaisePropertyChanged("FontWeight");
            }
        }

        /// <summary>
        /// 글꼴 색상
        /// </summary>
        private string _foreground;
        public virtual string Foreground
        {
            get => _foreground;
            set
            {
                _foreground = value;
                RaisePropertyChanged("Foreground");
            }
        }

        /// <summary>
        /// 글씨 알람
        /// </summary>
        private bool? _foregroundAlarmVisibility;
        public virtual bool? ForegroundAlarmVisibility
        {
            get => _foregroundAlarmVisibility;
            set
            {
                _foregroundAlarmVisibility = value;
                RaisePropertyChanged("ForegroundAlarmVisibility");
            }
        }

        /// <summary>
        /// 그리드 정렬
        /// </summary>
        private string _gridAlignment;
        public virtual string GridAlignment
        {
            get => _gridAlignment;
            set
            {
                _gridAlignment = value;
                RaisePropertyChanged("GridAlignment");
            }
        }

        /// <summary>
        /// 그리드 선 색상
        /// </summary>
        private string _gridLinesBrush;
        public virtual string GridLinesBrush
        {
            get => _gridLinesBrush;
            set
            {
                _gridLinesBrush = value;
                RaisePropertyChanged("GridLinesBrush");
            }
        }

        /// <summary>
        /// 그리드 선 보이기
        /// </summary>
        private string _gridLinesVisibility;
        public virtual string GridLinesVisibility
        {
            get => _gridLinesVisibility;
            set
            {
                _gridLinesVisibility = value;
                RaisePropertyChanged("GridLinesVisibility");
            }
        }

        /// <summary>
        /// 그리드 작은 선 색상
        /// </summary>
        private string _gridLinesMinorBrush;
        public virtual string GridLinesMinorBrush
        {
            get => _gridLinesMinorBrush;
            set
            {
                _gridLinesMinorBrush = value;
                RaisePropertyChanged("GridLinesMinorBrush");
            }
        }

        /// <summary>
        /// 그리드 작은 선 보이기
        /// </summary>
        private bool? _gridLinesMinorVisible;
        public virtual bool? GridLinesMinorVisible
        {
            get => _gridLinesMinorVisible;
            set
            {
                _gridLinesMinorVisible = value;
                RaisePropertyChanged("GridLinesMinorVisible");
            }
        }

        /// <summary>
        /// 그리드 선 보이기
        /// </summary>
        private bool? _gridLinesVisible;
        public virtual bool? GridLinesVisible
        {
            get => _gridLinesVisible;
            set
            {
                _gridLinesVisible = value;
                RaisePropertyChanged("GridLinesVisible");
            }
        }

        /// <summary>
        /// 들여쓰기
        /// </summary>
        private string _indent;
        public virtual string Indent
        {
            get => _indent;
            set
            {
                _indent = value;
                RaisePropertyChanged("Indent");
            }
        }

        /// <summary>
        /// 짜맞추기 사용 유무
        /// </summary>
        private bool? _interlaced;
        public virtual bool? Interlaced
        {
            get => _interlaced;
            set
            {
                _interlaced = value;
                RaisePropertyChanged("Interlaced");
            }
        }

        /// <summary>
        /// 짜맞추기 색상
        /// </summary>
        private string _interlacedBrush;
        public virtual string InterlacedBrush
        {
            get => _interlacedBrush;
            set
            {
                _interlacedBrush = value;
                RaisePropertyChanged("InterlacedBrush");
            }
        }

        /// <summary>
        /// Expander 확장 여부
        /// </summary>
        private bool? _isExpanded;
        public virtual bool? IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        /// <summary>
        /// 레이블 위치
        /// </summary>
        private string _labelPosition;
        public virtual string LabelPosition
        {
            get => _labelPosition;
            set
            {
                _labelPosition = value;
                RaisePropertyChanged("LabelPosition");
            }
        }

        /// <summary>
        /// 인접한 두 레이블 사이의 최소 들여쓰기
        /// </summary>
        private string _labelsResolveOverlappingMinIndent;
        public virtual string LabelsResolveOverlappingMinIndent
        {
            get => _labelsResolveOverlappingMinIndent;
            set
            {
                _labelsResolveOverlappingMinIndent = value;
                RaisePropertyChanged("LabelsResolveOverlappingMinIndent");
            }
        }

        /// <summary>
        /// 레이블 보이기 유무
        /// </summary>
        private string _labelsVisibility;
        public virtual string LabelsVisibility
        {
            get => _labelsVisibility;
            set
            {
                _labelsVisibility = value;
                RaisePropertyChanged("LabelsVisibility");
            }
        }

        /// <summary>
        /// 범례 텍스트 패턴
        /// </summary>
        private string _legendTextPattern;
        public virtual string LegendTextPattern
        {
            get => _legendTextPattern;
            set
            {
                _legendTextPattern = value;
                RaisePropertyChanged("LegendTextPattern");
            }
        }

        /// <summary>
        /// 라인스타일 두께
        /// </summary>
        private string _lineStyleThickness;
        public virtual string LineStyleThickness
        {
            get => _lineStyleThickness;
            set
            {
                _lineStyleThickness = value;
                RaisePropertyChanged("LineStyleThickness");
            }
        }

        /// <summary>
        /// 마커 보이기 유무
        /// </summary>
        private string _markerVisible;
        public virtual string MarkerVisible
        {
            get => _markerVisible;
            set
            {
                _markerVisible = value;
                RaisePropertyChanged("MarkerVisible");
            }
        }

        /// <summary>
        /// 마커 크기
        /// </summary>
        private string _markerSize;
        public virtual string MarkerSize
        {
            get => _markerSize;
            set
            {
                _markerSize = value;
                RaisePropertyChanged("MarkerSize");
            }
        }

        /// <summary>
        /// 측정 단위
        /// </summary>
        private string _measureUnit;
        public virtual string MeasureUnit
        {
            get => _measureUnit;
            set
            {
                _measureUnit = value;
                RaisePropertyChanged("MeasureUnit");
            }
        }

        /// <summary>
        /// 작은 눈금 갯수
        /// </summary>
        private string _minorCount;
        public virtual string MinorCount
        {
            get => _minorCount;
            set
            {
                _minorCount = value;
                RaisePropertyChanged("MinorCount");
            }
        }

        /// <summary>
        /// 높이
        /// </summary>
        private string _height;
        public virtual string Height
        {
            get => _height;
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
            }
        }

        /// <summary>
        /// 내부 구멍 크기
        /// </summary>
        private int? _holeRadiusPercent;
        public virtual int? HoleRadiusPercent
        {
            get => _holeRadiusPercent;
            set
            {
                _holeRadiusPercent = value;
                RaisePropertyChanged("HoleRadiusPercent");
            }
        }

        /// <summary>
        /// 수평 정렬
        /// </summary>
        private string _horizontalAlignment;
        public virtual string HorizontalAlignment
        {
            get => _horizontalAlignment;
            set
            {
                _horizontalAlignment = value;
                RaisePropertyChanged("HorizontalAlignment");
            }
        }

        /// <summary>
        /// 수평 내용 정렬
        /// </summary>
        private string _horizontalContentAlignment;
        public virtual string HorizontalContentAlignment
        {
            get => _horizontalContentAlignment;
            set
            {
                _horizontalContentAlignment = value;
                RaisePropertyChanged("HorizontalContentAlignment");
            }
        }

        /// <summary>
        /// 수평 위치
        /// </summary>
        private string _horizontalPosition;
        public virtual string HorizontalPosition
        {
            get => _horizontalPosition;
            set
            {
                _horizontalPosition = value;
                RaisePropertyChanged("HorizontalPosition");
            }
        }

        /// <summary>
        /// 수평스크롤 활성화
        /// </summary>
        private string _horizontalScrollBarVisibility;
        public virtual string HorizontalScrollBarVisibility
        {
            get => _horizontalScrollBarVisibility;
            set
            {
                _horizontalScrollBarVisibility = value;
                RaisePropertyChanged("HorizontalScrollBarVisibility");
            }
        }

        /// <summary>
        /// 내부 들여쓰기(구멍간의 간격)
        /// </summary>
        private int? _innerIndent;
        public virtual int? InnerIndent
        {
            get => _innerIndent;
            set
            {
                _innerIndent = value;
                RaisePropertyChanged("InnerIndent");
            }
        }

        /// <summary>
        /// 여백
        /// </summary>
        private string _margin;
        public virtual string Margin
        {
            get => _margin;
            set
            {
                _margin = value;
                RaisePropertyChanged("Margin");
            }
        }

        /// <summary>
        /// 마커 유형
        /// </summary>
        private string _markerMode;
        public virtual string MarkerMode
        {
            get => _markerMode;
            set
            {
                _markerMode = value;
                RaisePropertyChanged("MarkerMode");
            }
        }

        /// <summary>
        /// 최대 높이
        /// </summary>
        private string _maxHeight;
        public virtual string MaxHeight
        {
            get => _maxHeight;
            set
            {
                _maxHeight = value;
                RaisePropertyChanged("MaxHeight");
            }
        }

        /// <summary>
        /// 최대값
        /// </summary>
        private string _maximum;
        public virtual string Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                RaisePropertyChanged("Maximum");
            }
        }

        /// <summary>
        /// 최대 너비
        /// </summary>
        private string _maxWidth;
        public virtual string MaxWidth
        {
            get => _maxWidth;
            set
            {
                _maxWidth = value;
                RaisePropertyChanged("MaxWidth");
            }
        }

        /// <summary>
        /// 최대 범위
        /// </summary>
        private string _maxValue;
        public virtual string MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                RaisePropertyChanged("MaxValue");
            }
        }

        /// <summary>
        /// 최소 높이
        /// </summary>
        private string _minHeight;
        public virtual string MinHeight
        {
            get => _minHeight;
            set
            {
                _minHeight = value;
                RaisePropertyChanged("MinHeight");
            }
        }

        /// <summary>
        /// 최소값
        /// </summary>
        private string _minimum;
        public virtual string Minimum
        {
            get => _minimum;
            set
            {
                _minimum = value;
                RaisePropertyChanged("Minimum");
            }
        }

        /// <summary>
        /// 최소 너비
        /// </summary>
        private string _minWidth;
        public virtual string MinWidth
        {
            get => _minWidth;
            set
            {
                _minWidth = value;
                RaisePropertyChanged("MinWidth");
            }
        }

        /// <summary>
        /// 최소 범위
        /// </summary>
        private string _minValue;
        public virtual string MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                RaisePropertyChanged("MinValue");
            }
        }

        /// <summary>
        /// 창 미러 높이
        /// </summary>
        private string _mirrorHeight;
        public virtual string MirrorHeight
        {
            get => _mirrorHeight;
            set
            {
                _mirrorHeight = value;
                RaisePropertyChanged("MirrorHeight");
            }
        }

        /// <summary>
        /// 명칭
        /// </summary>
        private string _name;
        public virtual string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// 알람-정상
        /// </summary>
        private bool? _normalAlarmVisibility;
        public virtual bool? NormalAlarmVisibility
        {
            get => _normalAlarmVisibility;
            set
            {
                _normalAlarmVisibility = value;
                RaisePropertyChanged("NormalAlarmVisibility");
            }
        }

        /// <summary>
        /// 투명도
        /// </summary>
        private string _opacity;
        public virtual string Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                RaisePropertyChanged("Opacity");
            }
        }

        /// <summary>
        /// 투명도 색상
        /// </summary>
        private string _opacityMask;
        public virtual string OpacityMask
        {
            get => _opacityMask;
            set
            {
                _opacityMask = value;
                RaisePropertyChanged("OpacityMask");
            }
        }

        /// <summary>
        /// 범례-방향
        /// </summary>
        private string _orientation;
        public virtual string Orientation
        {
            get => _orientation;
            set
            {
                _orientation = value;
                RaisePropertyChanged("Orientation");
            }
        }

        /// <summary>
        /// 여백 메우기
        /// </summary>
        private string _padding;
        public virtual string Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                RaisePropertyChanged("Padding");
            }
        }

        /// <summary>
        /// 패널 방향
        /// </summary>
        private string _paneOrientation;
        public virtual string PaneOrientation
        {
            get => _paneOrientation;
            set
            {
                _paneOrientation = value;
                RaisePropertyChanged("PaneOrientation");
            }
        }

        /// <summary>
        /// 파일 경로
        /// </summary>
        private string _filePath;
        public virtual string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                RaisePropertyChanged("FilePath");
            }
        }

        /// <summary>
        /// 재생
        /// </summary>
        private bool? _play;
        public virtual bool? Play
        {
            get => _play;
            set
            {
                _play = value;
                RaisePropertyChanged("Play");
            }
        }

        /// <summary>
        /// RadiusX
        /// </summary>
        private string _radiusX;
        public virtual string RadiusX
        {
            get => _radiusX;
            set
            {
                _radiusX = value;
                RaisePropertyChanged("RadiusX");
            }
        }

        /// <summary>
        /// RadiusY
        /// </summary>
        private string _radiusY;
        public virtual string RadiusY
        {
            get => _radiusY;
            set
            {
                _radiusY = value;
                RaisePropertyChanged("RadiusY");
            }
        }

        /// <summary>
        /// 겹침 모드 해결
        /// </summary>
        private string _resolveOverlappingMode;
        public virtual string ResolveOverlappingMode
        {
            get => _resolveOverlappingMode;
            set
            {
                _resolveOverlappingMode = value;
                RaisePropertyChanged("ResolveOverlappingMode");
            }
        }

        /// <summary>
        /// 회전
        /// </summary>
        private string _rotated;
        public virtual string Rotated
        {
            get => _rotated;
            set
            {
                _rotated = value;
                RaisePropertyChanged("Rotated");
            }
        }

        /// <summary>
        /// 회전
        /// </summary>
        private string _rotation;
        public virtual string Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                RaisePropertyChanged("Rotation");
            }
        }

        /// <summary>
        /// Row Header 너비
        /// </summary>
        private string _rowHeaderWidth;
        public virtual string RowHeaderWidth
        {
            get => _rowHeaderWidth;
            set
            {
                _rowHeaderWidth = value;
                RaisePropertyChanged("RowHeaderWidth");
            }
        }

        /// <summary>
        /// X축 폼 변형
        /// </summary>
        private string _scaleX;
        public virtual string ScaleX
        {
            get => _scaleX;
            set
            {
                _scaleX = value;
                RaisePropertyChanged("ScaleX");
            }
        }

        /// <summary>
        /// Y축 폼 변형
        /// </summary>
        private string _scaleY;
        public virtual string ScaleY
        {
            get => _scaleY;
            set
            {
                _scaleY = value;
                RaisePropertyChanged("ScaleY");
            }
        }

        /// <summary>
        /// 범례 데이터 보이기
        /// </summary>
        private string _showInLegend;
        public virtual string ShowInLegend
        {
            get => _showInLegend;
            set
            {
                _showInLegend = value;
                RaisePropertyChanged("ShowInLegend");
            }
        }

        /// <summary>
        /// 측면 여백
        /// </summary>
        private string _sideMarginsValue;
        public virtual string SideMarginsValue
        {
            get => _sideMarginsValue;
            set
            {
                _sideMarginsValue = value;
                RaisePropertyChanged("SideMarginsValue");
            }
        }

        /// <summary>
        /// 글꼴 비틀기
        /// </summary>
        private string _staggered;
        public virtual string Staggered
        {
            get => _staggered;
            set
            {
                _staggered = value;
                RaisePropertyChanged("Staggered");
            }
        }

        /// <summary>
        /// 시작 지점 Angle 값
        /// </summary>
        private string _startAngle;
        public virtual string StartAngle
        {
            get => _startAngle;
            set
            {
                _startAngle = value;
                RaisePropertyChanged("StartAngle");
            }
        }

        /// <summary>
        /// 전체 범위
        /// </summary>
        private string _stickToEnd;
        public virtual string StickToEnd
        {
            get => _stickToEnd;
            set
            {
                _stickToEnd = value;
                RaisePropertyChanged("StickToEnd");
            }
        }

        /// <summary>
        /// 획
        /// </summary>
        private string _stroke;
        public virtual string Stroke
        {
            get => _stroke;
            set
            {
                _stroke = value;
                RaisePropertyChanged("Stroke");
            }
        }

        /// <summary>
        /// 호 선 타입
        /// </summary>
        private string _strokeLineJoin;
        public virtual string StrokeLineJoin
        {
            get => _strokeLineJoin;
            set
            {
                _strokeLineJoin = value;
                RaisePropertyChanged("StrokeLineJoin");
            }
        }

        /// <summary>
        /// 획 두께
        /// </summary>
        private string _strokeThickness;
        public virtual string StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                RaisePropertyChanged("StrokeThickness");
            }
        }

        /// <summary>
        /// 스웝 방향
        /// </summary>
        private string _sweepDirection;
        public virtual string SweepDirection
        {
            get => _sweepDirection;
            set
            {
                _sweepDirection = value;
                RaisePropertyChanged("SweepDirection");
            }
        }

        /// <summary>
        /// 텍스트
        /// </summary>
        private string _text;
        public virtual string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        /// <summary>
        /// 텍스트 정렬
        /// </summary>
        private string _textAlignment;
        public virtual string TextAlignment
        {
            get => _textAlignment;
            set
            {
                _textAlignment = value;
                RaisePropertyChanged("TextAlignment");
            }
        }

        /// <summary>
        /// 텍스트 패턴
        /// </summary>
        private string _textPattern;
        public virtual string TextPattern
        {
            get => _textPattern;
            set
            {
                _textPattern = value;
                RaisePropertyChanged("TextPattern");
            }
        }

        /// <summary>
        /// 텍스트 줄 바꿈
        /// </summary>
        private string _textWrapping;
        public virtual string TextWrapping
        {
            get => _textWrapping;
            set
            {
                _textWrapping = value;
                RaisePropertyChanged("TextWrapping");
            }
        }

        /// <summary>
        /// 두께
        /// </summary>
        private string _thickness;
        public virtual string Thickness
        {
            get => _thickness;
            set
            {
                _thickness = value;
                RaisePropertyChanged("Thickness");
            }
        }

        /// <summary>
        /// 눈금 표시 교차 여부
        /// </summary>
        private bool? _tickmarksCrossAxis;
        public virtual bool? TickmarksCrossAxis
        {
            get => _tickmarksCrossAxis;
            set
            {
                _tickmarksCrossAxis = value;
                RaisePropertyChanged("TickmarksCrossAxis");
            }
        }

        /// <summary>
        /// 눈금 길이
        /// </summary>
        private string _tickmarksLength;
        public virtual string TickmarksLength
        {
            get => _tickmarksLength;
            set
            {
                _tickmarksLength = value;
                RaisePropertyChanged("TickmarksLength");
            }
        }

        /// <summary>
        /// 작은 눈금 두께
        /// </summary>
        private string _tickmarksMinorThickness;
        public virtual string TickmarksMinorThickness
        {
            get => _tickmarksMinorThickness;
            set
            {
                _tickmarksMinorThickness = value;
                RaisePropertyChanged("TickmarksMinorThickness");
            }
        }

        /// <summary>
        /// 눈금 두께
        /// </summary>
        private string _tickmarksThickness;
        public virtual string TickmarksThickness
        {
            get => _tickmarksThickness;
            set
            {
                _tickmarksThickness = value;
                RaisePropertyChanged("TickmarksThickness");
            }
        }

        /// <summary>
        /// AxisX2D-X축 큰 단위 눈금 표시
        /// </summary>
        private string _tickmarksVisible;
        public virtual string TickmarksVisible
        {
            get => _tickmarksVisible;
            set
            {
                _tickmarksVisible = value;
                RaisePropertyChanged("TickmarksVisible");
            }
        }

        /// <summary>
        /// AxisX2D-X축 작은 단위 눈금 표시
        /// </summary>
        private string _tickmarksMinorVisible;
        public virtual string TickmarksMinorVisible
        {
            get => _tickmarksMinorVisible;
            set
            {
                _tickmarksMinorVisible = value;
                RaisePropertyChanged("TickmarksMinorVisible");
            }
        }

        /// <summary>
        /// 툴팁 활성화
        /// </summary>
        private string _toolTipEnabled;
        public virtual string ToolTipEnabled
        {
            get => _toolTipEnabled;
            set
            {
                _toolTipEnabled = value;
                RaisePropertyChanged("ToolTipEnabled");
            }
        }

        /// <summary>
        /// 전체 값
        /// </summary>
        private string _totalValue;
        public virtual string TotalValue
        {
            get => _totalValue;
            set
            {
                _totalValue = value;
                RaisePropertyChanged("TotalValue");
            }
        }

        /// <summary>
        /// 단위
        /// </summary>
        private string _unit;
        public virtual string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                RaisePropertyChanged("Unit");
            }
        }

        /// <summary>
        /// 주소
        /// </summary>
        private string _url;
        public virtual string Url
        {
            get => _url;
            set
            {
                _url = value;
                RaisePropertyChanged("Url");
            }
        }

        /// <summary>
        /// 값
        /// </summary>
        private string _value;
        public virtual string Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }

        }
        /// <summary>
        /// 수직 정렬
        /// </summary>
        private string _verticalAlignment;
        public virtual string VerticalAlignment
        {
            get => _verticalAlignment;
            set
            {
                _verticalAlignment = value;
                RaisePropertyChanged("VerticalAlignment");
            }
        }

        /// <summary>
        /// 수직 내용 정렬
        /// </summary>
        private string _verticalContentAlignment;
        public virtual string VerticalContentAlignment
        {
            get => _verticalContentAlignment;
            set
            {
                _verticalContentAlignment = value;
                RaisePropertyChanged("VerticalContentAlignment");
            }
        }

        /// <summary>
        /// 수직 위치
        /// </summary>
        private string _verticalPosition;
        public virtual string VerticalPosition
        {
            get => _verticalPosition;
            set
            {
                _verticalPosition = value;
                RaisePropertyChanged("VerticalPosition");
            }
        }

        /// <summary>
        /// 수직 스크롤 활성화
        /// </summary>
        private string _verticalScrollBarVisibility;
        public virtual string VerticalScrollBarVisibility
        {
            get => _verticalScrollBarVisibility;
            set
            {
                _verticalScrollBarVisibility = value;
                RaisePropertyChanged("VerticalScrollBarVisibility");
            }
        }

        /// <summary>
        /// 보이기
        /// </summary>
        private string _visibility;
        public virtual string Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                RaisePropertyChanged("Visibility");
            }
        }

        /// <summary>
        /// 보이기
        /// </summary>
        private string _visible;
        public virtual string Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                RaisePropertyChanged("Visible");
            }
        }

        /// <summary>
        /// 너비
        /// </summary>
        private string _width;
        public virtual string Width
        {
            get => _width;
            set
            {
                _width = value;
                RaisePropertyChanged("Width");
            }
        }

        /// <summary>
        /// 줌
        /// </summary>
        private string _zoom;
        public virtual string Zoom
        {
            get => _zoom;
            set
            {
                _zoom = value;
                RaisePropertyChanged("Zoom");
            }
        }
    }
}
