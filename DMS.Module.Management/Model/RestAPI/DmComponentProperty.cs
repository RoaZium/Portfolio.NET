using Newtonsoft.Json;
using Prism.Mvvm;
using PrismWPF.Common.Converter;
using System.Windows;
using System.Windows.Media;

namespace DMS.Module.Management.Model.RestAPI
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

        #endregion

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

        #region 레이아웃(수평, 수직)

        /// <summary>
        /// 수평
        /// </summary>
        private string _layoutHorizontalcontentaligment;
        public string LayoutHorizontalcontentaligment
        {
            get => _layoutHorizontalcontentaligment;
            set
            {
                _layoutHorizontalcontentaligment = value;
                RaisePropertyChanged("LayoutHorizontalcontentaligment");
            }
        }

        /// <summary>
        /// 수직
        /// </summary>
        private string _layoutVerticalcontentaligment;
        public string LayoutVerticalcontentaligment
        {
            get => _layoutVerticalcontentaligment;
            set
            {
                _layoutVerticalcontentaligment = value;
                RaisePropertyChanged("LayoutVerticalcontentaligment");
            }
        }

        #endregion

        #region 모양

        /// <summary>
        /// 투명도
        /// </summary>
        private string _shapeOpacity;
        public string ShapeOpacity
        {
            get => _shapeOpacity;
            set
            {
                _shapeOpacity = value;
                RaisePropertyChanged("ShapeOpacity");
            }
        }

        /// <summary>
        /// 보이기
        /// </summary>
        private bool? _shapeVisibility;
        public bool? ShapeVisibility
        {
            get => _shapeVisibility;
            set
            {
                _shapeVisibility = value;
                RaisePropertyChanged("ShapeVisibility");
            }
        }

        /// <summary>
        /// 테두리 두께
        /// </summary>
        private int? _shapeBorderthickness;
        public int? ShapeBorderthickness
        {
            get => _shapeBorderthickness;
            set
            {
                _shapeBorderthickness = value;
                RaisePropertyChanged("ShapeBorderthickness");
            }
        }

        /// <summary>
        /// 모서리 반경
        /// </summary>
        private int? _shapeCornerRadius;
        public int? ShapeCornerRadius
        {
            get => _shapeCornerRadius;
            set
            {
                _shapeCornerRadius = value;
                RaisePropertyChanged("ShapeCornerRadius");
            }
        }

        /// <summary>
        /// 라벨 위치
        /// </summary>
        private string _labelPosition;
        public string LabelPosition
        {
            get => _labelPosition;
            set
            {
                _labelPosition = value;
                RaisePropertyChanged("LabelPosition");
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

        /// <summary>
        /// 모양 색상
        /// </summary>
        private string _shapeColor;
        public string ShapeColor
        {
            get => _shapeColor;
            set
            {
                _shapeColor = value;
                RaisePropertyChanged("ShapeColor");
            }
        }

        #endregion

        #region 브러시(배경색, 테두리 배경색, 글씨 색상)

        /// <summary>
        /// 배경색
        /// </summary>
        private string _brushBackground;
        public string BrushBackground
        {
            get => _brushBackground;
            set
            {
                _brushBackground = value;
                RaisePropertyChanged("BrushBackground");
            }
        }

        /// <summary>
        /// 테두리 배경색
        /// </summary>
        private string _brushBorderbrush;
        public string BrushBorderbrush
        {
            get => _brushBorderbrush;
            set
            {
                _brushBorderbrush = value;
                RaisePropertyChanged("BrushBorderbrush");
            }
        }

        /// <summary>
        /// 글씨 색상
        /// </summary>
        private string _brushForeground;
        public string BrushForeground
        {
            get => _brushForeground;
            set
            {
                _brushForeground = value;
                RaisePropertyChanged("BrushForeground");
            }
        }

        #endregion

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

        #region 차트 속성 정보(그래프 범위, 관리치, 한계치)

        /// <summary>
        /// 최소 범위
        /// </summary>
        private decimal? _minimumRange;
        public decimal? MinimumRange
        {
            get => _minimumRange;
            set
            {
                _minimumRange = value;
                RaisePropertyChanged("MinimumRange");
            }
        }

        /// <summary>
        /// 최대 범위
        /// </summary>
        private decimal? _maximumRange;
        public decimal? MaximumRange
        {
            get => _maximumRange;
            set
            {
                _maximumRange = value;
                RaisePropertyChanged("MaximumRange");
            }
        }

        /// <summary>
        /// 타이틀
        /// </summary>
        private string _chartTitle;
        public string ChartTitle
        {
            get => _chartTitle;
            set
            {
                _chartTitle = value;

                if(string.IsNullOrEmpty(_chartTitle))
                {
                    _chartTitle = ItemName;
                }

                RaisePropertyChanged("ChartTitle");
            }
        }

        /// <summary>
        /// X축 타이틀
        /// </summary>
        private string _axisXTitle;
        public string AxisXTitle
        {
            get => _axisXTitle;
            set
            {
                _axisXTitle = value;
                RaisePropertyChanged("AxisXTitle");
            }
        }

        /// <summary>
        /// Y축 타이틀
        /// </summary>
        private string _axisYTitle;
        public string AxisYTitle
        {
            get => _axisYTitle;
            set
            {
                _axisYTitle = value;
                RaisePropertyChanged("AxisYTitle");
            }
        }

        #endregion

        #region 텍스트(내용, 크기, 두께)

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
        /// 글씨 크기
        /// </summary>
        private byte? _textFontsize;
        public byte? TextFontsize
        {
            get => _textFontsize;
            set
            {
                _textFontsize = value;
                RaisePropertyChanged("TextFontsize");
            }
        }

        /// <summary>
        /// 글씨 두께
        /// </summary>
        private string _textFontweight;
        public string TextFontweight
        {
            get => _textFontweight;
            set
            {
                _textFontweight = value;
                RaisePropertyChanged("TextFontweight");
            }
        }

        private TextWrapping _textWrapping;
        public TextWrapping TextWrapping
        {
            get => _textWrapping;
            set
            {
                _textWrapping = value;
                RaisePropertyChanged("TextWrapping");
            }
        }

        #endregion


        #region 차트

        /// <summary>
        /// 차트-애니메이션모드
        /// </summary>
        private string _chartAnimationMode;
        public string ChartAnimationMode
        {
            get => _chartAnimationMode;
            set
            {
                _chartAnimationMode = value;
                RaisePropertyChanged("ChartAnimationMode");
            }
        }

        /// <summary>
        /// 차트-배경색
        /// </summary>
        private string _chartBackground;
        public string ChartBackground
        {
            get => _chartBackground;
            set
            {
                _chartBackground = value;
                RaisePropertyChanged("ChartBackground");
            }
        }

        /// <summary>
        /// 차트-투명도
        /// </summary>
        private string _chartOpacity;
        public string ChartOpacity
        {
            get => _chartOpacity;
            set
            {
                _chartOpacity = value;
                RaisePropertyChanged("ChartOpacity");
            }
        }

        #endregion

        #region 차트 타이틀

        /// <summary>
        /// 차트-이름
        /// </summary>
        private string _chartContent;
        public string ChartContent
        {
            get => _chartContent;
            set
            {
                _chartContent = value;
                RaisePropertyChanged("ChartContent");
            }
        }

        /// <summary>
        /// 차트-글꼴 크기
        /// </summary>
        private string _chartFontSize;
        public string ChartFontSize
        {
            get => _chartFontSize;
            set
            {
                _chartFontSize = value;
                RaisePropertyChanged("ChartFontSize");
            }
        }

        /// <summary>
        /// 차트-글꼴 색상
        /// </summary>
        private string _chartForeground;
        public string ChartForeground
        {
            get => _chartForeground;
            set
            {
                _chartForeground = value;
                RaisePropertyChanged("ChartForeground");
            }
        }

        /// <summary>
        /// 차트-수평 위치
        /// </summary>
        private string _chartHorizontalAlignment;
        public string ChartHorizontalAlignment
        {
            get => _chartHorizontalAlignment;
            set
            {
                _chartHorizontalAlignment = value;
                RaisePropertyChanged("ChartHorizontalAlignment");
            }
        }

        /// <summary>
        /// 차트-수직 위치
        /// </summary>
        private string _chartVerticalAlignment;
        public string ChartVerticalAlignment
        {
            get => _chartVerticalAlignment;
            set
            {
                _chartVerticalAlignment = value;
                RaisePropertyChanged("ChartVerticalAlignment");
            }
        }

        /// <summary>
        /// 차트-보이기
        /// </summary>
        private string _chartVisibility;
        public string ChartVisibility
        {
            get => _chartVisibility;
            set
            {
                _chartVisibility = value;
                RaisePropertyChanged("ChartVisibility");
            }
        }

        #endregion

        #region 범례(Legend)

        /// <summary>
        /// 범례-배경색
        /// </summary>
        private string _legendBackground;
        public string LegendBackground
        {
            get => _legendBackground;
            set
            {
                _legendBackground = value;
                RaisePropertyChanged("LegendBackground");
            }
        }

        /// <summary>
        /// 범례-글꼴 크기
        /// </summary>
        private string _legendFontSize;
        public string LegendFontSize
        {
            get => _legendFontSize;
            set
            {
                _legendFontSize = value;
                RaisePropertyChanged("LegendFontSize");
            }
        }

        /// <summary>
        /// 범례-글꼴 색상
        /// </summary>
        private string _legendForeground;
        public string LegendForeground
        {
            get => _legendForeground;
            set
            {
                _legendForeground = value;
                RaisePropertyChanged("LegendForeground");
            }
        }

        /// <summary>
        /// 범례-방향
        /// </summary>
        private string _legendOrientation;
        public string LegendOrientation
        {
            get => _legendOrientation;
            set
            {
                _legendOrientation = value;
                RaisePropertyChanged("LegendOrientation");
            }
        }

        /// <summary>
        /// 범례-수평 위치
        /// </summary>
        private string _legendHorizontalPosition;
        public string LegendHorizontalPosition
        {
            get => _legendHorizontalPosition;
            set
            {
                _legendHorizontalPosition = value;
                RaisePropertyChanged("LegendHorizontalPosition");
            }
        }

        /// <summary>
        /// 범례-수직 위치
        /// </summary>
        private string _legendVerticalPosition;
        public string LegendVerticalPosition
        {
            get => _legendVerticalPosition;
            set
            {
                _legendVerticalPosition = value;
                RaisePropertyChanged("LegendVerticalPosition");
            }
        }

        /// <summary>
        /// 범례-보이기
        /// </summary>
        private string _legendVisibility;
        public string LegendVisibility
        {
            get => _legendVisibility;
            set
            {
                _legendVisibility = value;
                RaisePropertyChanged("LegendVisibility");
            }
        }

        #endregion

        #region XYDiagram2D

        /// <summary>
        /// XYDiagram2D-스크롤 가능 여부
        /// </summary>
        private string _xYDiagram2DEnableAxisXNavigation;
        public string XYDiagram2DEnableAxisXNavigation
        {
            get => _xYDiagram2DEnableAxisXNavigation;
            set
            {
                _xYDiagram2DEnableAxisXNavigation = value;
                RaisePropertyChanged("XYDiagram2DEnableAxisXNavigation");
            }
        }

        #endregion

        #region 패널(Pane)

        /// <summary>
        /// 패널-배경색
        /// </summary>
        private string _paneDomainBrush;
        public string PaneDomainBrush
        {
            get => _paneDomainBrush;
            set
            {
                _paneDomainBrush = value;
                RaisePropertyChanged("PaneDomainBrush");
            }
        }

        #endregion

        #region AxisX2D

        /// <summary>
        /// AxisX2D-시작 또는 끝 고정 여부
        /// </summary>
        private string _axisX2DStickToEnd;
        public string AxisX2DStickToEnd
        {
            get => _axisX2DStickToEnd;
            set
            {
                _axisX2DStickToEnd = value;
                RaisePropertyChanged("AxisX2DStickToEnd");
            }
        }

        /// <summary>
        /// AxisX2D-X축 큰 단위 눈금 표시
        /// </summary>
        private string _axisX2DTickmarksVisible;
        public string AxisX2DTickmarksVisible
        {
            get => _axisX2DTickmarksVisible;
            set
            {
                _axisX2DTickmarksVisible = value;
                RaisePropertyChanged("AxisX2DTickmarksVisible");
            }
        }

        /// <summary>
        /// AxisX2D-X축 작은 단위 눈금 표시
        /// </summary>
        private string _axisX2DTickmarksMinorVisible;
        public string AxisX2DTickmarksMinorVisible
        {
            get => _axisX2DTickmarksMinorVisible;
            set
            {
                _axisX2DTickmarksMinorVisible = value;
                RaisePropertyChanged("AxisX2DTickmarksMinorVisible");
            }
        }

        #endregion

        #region AxisXTitle

        /// <summary>
        /// AxisXTitle-내용
        /// </summary>
        private string _axisXTitleContent;
        public string AxisXTitleContent
        {
            get => _axisXTitleContent;
            set
            {
                _axisXTitleContent = value;
                RaisePropertyChanged("AxisXTitleContent");
            }
        }

        /// <summary>
        /// AxisXTitleFontSize-글꼴 크기
        /// </summary>
        private string _axisXTitleFontSize;
        public string AxisXTitleFontSize
        {
            get => _axisXTitleFontSize;
            set
            {
                _axisXTitleFontSize = value;
                RaisePropertyChanged("AxisXTitleFontSize");
            }
        }

        /// <summary>
        /// AxisXTitleForeground-내용
        /// </summary>
        private string _axisXTitleForeground;
        public string AxisXTitleForeground
        {
            get => _axisXTitleForeground;
            set
            {
                _axisXTitleForeground = value;
                RaisePropertyChanged("AxisXTitleForeground");
            }
        }

        #endregion

        #region AxisXLabel

        /// <summary>
        /// AxisXLabel-글꼴 색상
        /// </summary>
        private string _axisXLabelForeground;
        public string AxisXLabelForeground
        {
            get => _axisXLabelForeground;
            set
            {
                _axisXLabelForeground = value;
                RaisePropertyChanged("AxisXLabelForeground");
            }
        }

        private string _axisXLabelFontSize;
        public string AxisXLabelFontSize
        {
            get => _axisXLabelFontSize;
            set
            {
                _axisXLabelFontSize = value;
                RaisePropertyChanged("AxisXLabelFontSize");
            }
        }

        /// <summary>
        /// AxisXLabelStaggered-글꼴 비틀기
        /// </summary>
        private string _axisXLabelStaggered;
        public string AxisXLabelStaggered
        {
            get => _axisXLabelStaggered;
            set
            {
                _axisXLabelStaggered = value;
                RaisePropertyChanged("AxisXLabelStaggered");
            }
        }

        /// <summary>
        /// AxisXLabel-보이기
        /// </summary>
        private string _axisXLabelVisible;
        public string AxisXLabelVisible
        {
            get => _axisXLabelVisible;
            set
            {
                _axisXLabelVisible = value;
                RaisePropertyChanged("AxisXLabelVisible");
            }
        }

        #endregion

        #region AxisY2D

        /// <summary>
        /// AxisY2D-축 정렬
        /// </summary>
        private string _axisY2DAlignment;
        public string AxisY2DAlignment
        {
            get => _axisY2DAlignment;
            set
            {
                _axisY2DAlignment = value;
                RaisePropertyChanged("AxisY2DAlignment");
            }
        }

        /// <summary>
        /// AxisY2D-그리드 선 색상
        /// </summary>
        private string _axisY2DGridLinesBrush;
        public string AxisY2DGridLinesBrush
        {
            get => _axisY2DGridLinesBrush;
            set
            {
                _axisY2DGridLinesBrush = value;
                RaisePropertyChanged("AxisY2DGridLinesBrush");
            }
        }

        /// <summary>
        /// AxisY2D-짜맞추기
        /// </summary>
        private string _axisY2DInterlaced;
        public string AxisY2DInterlaced
        {
            get => _axisY2DInterlaced;
            set
            {
                _axisY2DInterlaced = value;
                RaisePropertyChanged("AxisY2DInterlaced");
            }
        }

        /// <summary>
        /// AxisY2D-짜맞추기 색상
        /// </summary>
        private string _axisY2DInterlacedBrush;
        public string AxisY2DInterlacedBrush
        {
            get => _axisY2DInterlacedBrush;
            set
            {
                _axisY2DInterlacedBrush = value;
                RaisePropertyChanged("AxisY2DInterlacedBrush");
            }
        }

        /// <summary>
        /// AxisY2D-Y축 큰 단위 눈금 표시
        /// </summary>
        private string _axisY2DTickmarksVisible;
        public string AxisY2DTickmarksVisible
        {
            get => _axisY2DTickmarksVisible;
            set
            {
                _axisY2DTickmarksVisible = value;
                RaisePropertyChanged("AxisY2DTickmarksVisible");
            }
        }

        /// <summary>
        /// AxisY2D-Y축 작은 단위 눈금 표시
        /// </summary>
        private string _axisY2DTickmarksMinorVisible;
        public string AxisY2DTickmarksMinorVisible
        {
            get => _axisY2DTickmarksMinorVisible;
            set
            {
                _axisY2DTickmarksMinorVisible = value;
                RaisePropertyChanged("AxisY2DTickmarksMinorVisible");
            }
        }

        /// <summary>
        /// AxisY2D-선 두께
        /// </summary>
        private string _axisY2DLineThickness;
        public string AxisY2DLineThickness
        {
            get => _axisY2DLineThickness;
            set
            {
                _axisY2DLineThickness = value;
                RaisePropertyChanged("AxisY2DLineThickness");
            }
        }

        /// <summary>
        /// AxisY2D-선 간격
        /// </summary>
        private string _axisY2DLineDashes;
        public string AxisY2DLineDashes
        {
            get => _axisY2DLineDashes;
            set
            {
                _axisY2DLineDashes = value;
                RaisePropertyChanged("AxisY2DLineDashes");
            }
        }

        #endregion

        #region AxisYTitle

        /// <summary>
        /// AxisYTitleContent-내용
        /// </summary>
        private string _axisYTitleContent;
        public string AxisYTitleContent
        {
            get => _axisYTitleContent;
            set
            {
                _axisYTitleContent = value;
                RaisePropertyChanged("AxisYTitleContent");
            }
        }

        /// <summary>
        /// AxisYTitleFontSize-글꼴 크기
        /// </summary>
        private string _axisYTitleFontSize;
        public string AxisYTitleFontSize
        {
            get => _axisYTitleFontSize;
            set
            {
                _axisYTitleFontSize = value;
                RaisePropertyChanged("AxisYTitleFontSize");
            }
        }

        /// <summary>
        /// AxisYForeground-내용
        /// </summary>
        private string _axisYTitleForeground;
        public string AxisYTitleForeground
        {
            get => _axisYTitleForeground;
            set
            {
                _axisYTitleForeground = value;
                RaisePropertyChanged("AxisYTitleForeground");
            }
        }

        #endregion

        #region AxisYLabel

        /// <summary>
        /// AxisYLabel-글꼴 색상
        /// </summary>
        private string _axisYLabelForeground;
        public string AxisYLabelForeground
        {
            get => _axisYLabelForeground;
            set
            {
                _axisYLabelForeground = value;
                RaisePropertyChanged("AxisYLabelForeground");
            }
        }

        private string _axisYLabelFontSize;
        public string AxisYLabelFontSize
        {
            get => _axisYLabelFontSize;
            set
            {
                _axisYLabelFontSize = value;
                RaisePropertyChanged("AxisYLabelFontSize");
            }
        }

        /// <summary>
        /// AxisYLabelStaggered-글씨 비틀기
        /// </summary>
        private string _axisYLabelStaggered;
        public string AxisYLabelStaggered
        {
            get => _axisYLabelStaggered;
            set
            {
                _axisYLabelStaggered = value;
                RaisePropertyChanged("AxisYLabelStaggered");
            }
        }

        /// <summary>
        /// AxisYLabelVisible-보이기
        /// </summary>
        private string _axisYLabelVisible;
        public string AxisYLabelVisible
        {
            get => _axisYLabelVisible;
            set
            {
                _axisYLabelVisible = value;
                RaisePropertyChanged("AxisYLabelVisible");
            }
        }

        #endregion

        #region X축 Range

        /// <summary>
        /// X축 Range-최소 범위
        /// </summary>
        private string _axisXRangeMinValue;
        public string AxisXRangeMinValue
        {
            get => _axisXRangeMinValue;
            set
            {
                _axisXRangeMinValue = value;
                RaisePropertyChanged("AxisXRangeMinValue");
            }
        }

        /// <summary>
        /// X축 Range-최대 범위
        /// </summary>
        private string _axisXRangeMaxValue;
        public string AxisXRangeMaxValue
        {
            get => _axisXRangeMaxValue;
            set
            {
                _axisXRangeMaxValue = value;
                RaisePropertyChanged("AxisXRangeMaxValue");
            }
        }

        /// <summary>
        /// X축 Range-측면 여백
        /// </summary>
        private string _axisXRangeSideMarginsValue;
        public string AxisXRangeSideMarginsValue
        {
            get => _axisXRangeSideMarginsValue;
            set
            {
                _axisXRangeSideMarginsValue = value;
                RaisePropertyChanged("AxisXRangeSideMarginsValue");
            }
        }

        #endregion

        #region Y축 Range

        /// <summary>
        /// Y축 Range-최소 범위
        /// </summary>
        private string _axisYRangeMinValue;
        public string AxisYRangeMinValue
        {
            get => _axisYRangeMinValue;
            set
            {
                _axisYRangeMinValue = value;
                RaisePropertyChanged("AxisYRangeMinValue");
            }
        }

        /// <summary>
        /// Y축 Range-최대 범위
        /// </summary>
        private string _axisYRangeMaxValue;
        public string AxisYRangeMaxValue
        {
            get => _axisYRangeMaxValue;
            set
            {
                _axisYRangeMaxValue = value;
                RaisePropertyChanged("AxisYRangeMaxValue");
            }
        }

        /// <summary>
        /// Y축 Range-측면 여백
        /// </summary>
        private string _axisYRangeSideMarginsValue;
        public string AxisYRangeSideMarginsValue
        {
            get => _axisYRangeSideMarginsValue;
            set
            {
                _axisYRangeSideMarginsValue = value;
                RaisePropertyChanged("AxisYRangeSideMarginsValue");
            }
        }

        #endregion

        #region SeriesLabel(복합 막대형/복합 선형/중첩도넛/원형/산포도)

        /// <summary>
        /// SeriesLabel-위치
        /// </summary>
        private string _seriesLabelPosition;
        public string SeriesLabelPosition
        {
            get => _seriesLabelPosition;
            set
            {
                _seriesLabelPosition = value;
                RaisePropertyChanged("SeriesLabelPosition");
            }
        }

        /// <summary>
        /// SeriesLabel-배경색
        /// </summary>
        private string _seriesLabelBackground;
        public string SeriesLabelBackground
        {
            get => _seriesLabelBackground;
            set
            {
                _seriesLabelBackground = value;
                RaisePropertyChanged("SeriesLabelBackground");
            }
        }

        /// <summary>
        /// SeriesLabel-보이기
        /// </summary>
        private string _seriesLabelVisible;
        public string SeriesLabelVisible
        {
            get => _seriesLabelVisible;
            set
            {
                _seriesLabelVisible = value;
                RaisePropertyChanged("SeriesLabelVisible");
            }
        }

        /// <summary>
        /// 글씨 색상
        /// </summary>
        private string _seriesLabelForeground;
        public string SeriesLabelForeground
        {
            get => _seriesLabelForeground;
            set
            {
                _seriesLabelForeground = value;
                RaisePropertyChanged("SeriesLabelForeground");
            }
        }

        /// <summary>
        /// 글씨 크기
        /// </summary>
        private string _seriesLabelFontSize;
        public string SeriesLabelFontSize
        {
            get => _seriesLabelFontSize;
            set
            {
                _seriesLabelFontSize = value;
                RaisePropertyChanged("SeriesLabelFontSize");
            }
        }

        #endregion

        #region BarSideBySideSeries2D

        /// <summary>
        /// 막대형 - 바 너비
        /// </summary>
        private string _barSideBySideSeries2DBarWidth;
        public string BarSideBySideSeries2DBarWidth
        {
            get => _barSideBySideSeries2DBarWidth;
            set
            {
                _barSideBySideSeries2DBarWidth = value;
                RaisePropertyChanged("BarSideBySideSeries2DBarWidth");
            }
        }

        /// <summary>
        /// 막대형 - 바 색상
        /// </summary>
        private string _barSideBySideSeries2DBrush;
        public string BarSideBySideSeries2DBrush
        {
            get => _barSideBySideSeries2DBrush;
            set
            {
                _barSideBySideSeries2DBrush = value;
                RaisePropertyChanged("BarSideBySideSeries2DBrush");
            }
        }

        #endregion

        #region LineSeries2D

        /// <summary>
        /// 선형 - 선 색상
        /// </summary>
        private string _lineSeries2DBrush;
        public string LineSeries2DBrush
        {
            get => _lineSeries2DBrush;
            set
            {
                _lineSeries2DBrush = value;
                RaisePropertyChanged("LineSeries2DBrush");
            }
        }

        /// <summary>
        /// Series Point Marker Visible
        /// </summary>
        private string _lineSeriesMarkerVisible;
        public string LineSeriesMarkerVisible
        {
            get => _lineSeriesMarkerVisible;
            set
            {
                _lineSeriesMarkerVisible = value;
                RaisePropertyChanged("LineSeriesMarkerVisible");
            }
        }

        /// <summary>
        /// Series Point Marker Size
        /// </summary>
        private int? _lineSeriesMarkerSize;
        public int? LineSeriesMarkerSize
        {
            get => _lineSeriesMarkerSize;
            set
            {
                _lineSeriesMarkerSize = value;
                RaisePropertyChanged("LineSeriesMarkerSize");
            }
        }

        #endregion

        #region NestedDonutSeries2D

        /// <summary>
        /// 내부 구멍 크기
        /// </summary>
        private int? _holeRadiusPercent;
        public int? HoleRadiusPercent
        {
            get => _holeRadiusPercent;
            set
            {
                _holeRadiusPercent = value;
                RaisePropertyChanged("HoleRadiusPercent");
            }
        }

        /// <summary>
        /// 내부 들여쓰기(구멍간의 간격)
        /// </summary>
        private int? _nestedDonutSeries2DInnerIndent;
        public int? NestedDonutSeries2DInnerIndent
        {
            get => _nestedDonutSeries2DInnerIndent;
            set
            {
                _nestedDonutSeries2DInnerIndent = value;
                RaisePropertyChanged("NestedDonutSeries2DInnerIndent");
            }
        }

        /// <summary>
        /// 회전
        /// </summary>
        private string _rotation;
        public string Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                RaisePropertyChanged("Rotation");
            }
        }

        /// <summary>
        /// 툴팁 활성화
        /// </summary>
        private string _nestedDonutSeries2DToolTipEnabled;
        public string NestedDonutSeries2DToolTipEnabled
        {
            get => _nestedDonutSeries2DToolTipEnabled;
            set
            {
                _nestedDonutSeries2DToolTipEnabled = value;
                RaisePropertyChanged("NestedDonutSeries2DToolTipEnabled");
            }
        }

        #endregion

        #region PointSeries2D

        /// <summary>
        /// 산포도 색상
        /// </summary>
        private string _pointSeries2DBrush;
        public string PointSeries2DBrush
        {
            get => _pointSeries2DBrush;
            set
            {
                _pointSeries2DBrush = value;
                RaisePropertyChanged("PointSeries2DBrush");
            }
        }

        /// <summary>
        /// 산포도 마크 크기
        /// </summary>
        private string _pointSeries2DMarkerSize;
        public string PointSeries2DMarkerSize
        {
            get => _pointSeries2DMarkerSize;
            set
            {
                _pointSeries2DMarkerSize = value;
                RaisePropertyChanged("PointSeries2DMarkerSize");
            }
        }

        #endregion


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

        #region 알람 표시

        /// <summary>
        /// 테두리 알람
        /// </summary>
        private bool? _borderAlarmVisibility;
        public bool? BorderAlarmVisibility
        {
            get => _borderAlarmVisibility;
            set
            {
                _borderAlarmVisibility = value;
                RaisePropertyChanged("BorderAlarmVisibility");
            }
        }

        /// <summary>
        /// 배경색 알람
        /// </summary>
        private bool? _backgroundAlarmVisibility;
        public bool? BackgroundAlarmVisibility
        {
            get => _backgroundAlarmVisibility;
            set
            {
                _backgroundAlarmVisibility = value;
                RaisePropertyChanged("BackgroundAlarmVisibility");
            }
        }

        /// <summary>
        /// 글씨 알람
        /// </summary>
        private bool? _foregroundAlarmVisibility;
        public bool? ForegroundAlarmVisibility
        {
            get => _foregroundAlarmVisibility;
            set
            {
                _foregroundAlarmVisibility = value;
                RaisePropertyChanged("ForegroundAlarmVisibility");
            }
        }

        #endregion

        private string _alarmCode;
        public string AlarmCode
        {
            get => _alarmCode;
            set => _alarmCode = value;
        }

        private bool _isAlarmLayout = false;
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
