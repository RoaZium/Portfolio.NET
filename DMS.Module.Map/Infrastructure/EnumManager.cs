using DMS.Module.Map.Properties;
using PrismWPF.Common.Converter;
using PrismWPF.Common.Validation;
using System.ComponentModel;

namespace DMS.Module.Map.Infrastructure
{
    public enum ActionType
    {
        C,  //Create
        U,  //Update
        D,  //Delete
        N   //None
    }

    public enum ComponentType
    {
        /// <summary>
        /// 공정
        /// </summary>
        DM601001,

        /// <summary>
        /// 작업장(워크센터)
        /// </summary>
        DM601002,

        /// <summary>
        /// 설비
        /// </summary>
        DM601003,

        /// <summary>
        /// 단말기
        /// </summary>
        DM601004,

        /// <summary>
        /// 리소스
        /// </summary>
        DM601005,

        /// <summary>
        /// 가상
        /// </summary>
        DM701000,

        /// <summary>
        /// 이미지
        /// </summary>
        DM701001,

        /// <summary>
        /// 동영상
        /// </summary>
        DM701002,

        /// <summary>
        /// IPCamera
        /// </summary>
        DM701003,

        /// <summary>
        /// TextBox
        /// </summary>
        DM701005,

        /// <summary>
        /// Line Graph
        /// </summary>
        DM801001,

        /// <summary>
        /// VerticalBar Graph
        /// </summary>
        DM801002,

        Asset,

        /// <summary>
        /// 막대형 시리즈
        /// </summary>
        BarSeries,

        /// <summary>
        /// DataBox
        /// </summary>
        DM801003,

        /// <summary>
        /// 원형 차트
        /// </summary>
        PieChart,

        /// <summary>
        /// 산포도 차트
        /// </summary>
        ScatterChart,

        /// <summary>
        /// X-bar 차트
        /// </summary>
        XbarChart,

        /// <summary>
        /// Pareto 차트
        /// </summary>
        ParetoChart,

        /// <summary>
        /// 게이지
        /// </summary>
        Gauge,

        /// <summary>
        /// Indicator
        /// </summary>
        Indicator,

        /// <summary>
        /// 선형시리즈
        /// </summary>
        LineSeries,

        /// <summary>
        /// 데이터 Grid
        /// </summary>
        DataGrid,

        /// <summary>
        /// GIF
        /// </summary>
        Gif,

        /// <summary>
        /// 원형 게이지
        /// </summary>
        RadialProgressBar,

        /// <summary>
        /// 중첩 도넛 시리즈
        /// </summary>
        NestedDonutSeries,

        /// <summary>
        /// 프로그레스 바
        /// </summary>
        ProgressBar,

        /// <summary>
        /// 레이아웃
        /// </summary>
        Layout,

        /// <summary>
        /// 레이아웃 이동
        /// </summary>
        Move,

        /// <summary>
        /// 실시간 IP 카메라 영상
        /// </summary>
        Streaming,

        /// <summary>
        /// 이미지 뷰어
        /// </summary>
        ImageViewer,

        /// <summary>
        /// 알람 이상 이력
        /// </summary>
        AlarmHistory,

        /// <summary>
        /// 자동 모드
        /// </summary>
        AutoMode,

        /// <summary>
        /// 상세정보
        /// </summary>
        DetailInfo,

        /// <summary>
        /// 알람 상태
        /// </summary>
        AlarmState,

        /// <summary>
        /// 화면
        /// </summary>
        Screen,

        /// <summary>
        /// 애니메이션
        /// </summary>
        Animation
    }

    public enum LOG
    {
        LOG,
        API
    }

    /// <summary>
    /// MQTT Topic
    /// </summary>
    public enum MQTTTopic
    {
        data_collection_digit,
        sensor,
        alarm_state,
        KPI,
        DataGrid,
        image,
        state_alarm
    }

    public enum SensorType
    {
        CD140001,   // Numerical - 수치형(0, 0.333 같은 수치형 데이터 수집)
        CD140002,   // Categorical - 카테고리형(0, 1, 2 같은 카테고리형 데이터 수집)
        CD140003    // Zone 데이터형
    }

    public enum Arrange
    {
        Remove,         // 제거
        BringToFront,   // 맨 앞으로 가져오기
        BringForward,   // 앞으로 가져오기
        SendToBack,     // 맨 뒤로 보내기
        SendBackward    // 뒤로 보내기
    }

    public enum ThemeType
    {
        Black,
        Blue,
        White
    }

    public enum LanguageType
    {
        En,
        Ja,
        Ko
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ResourceIcon
    {
        [LocalizedDescription("Str0000", typeof(Res))]
        A000,
        [LocalizedDescription("StrA010", typeof(Res))]
        A010,
        [LocalizedDescription("StrA020", typeof(Res))]
        A020,
        [LocalizedDescription("StrA030", typeof(Res))]
        A030,
        [LocalizedDescription("StrA040", typeof(Res))]
        A040,
        [LocalizedDescription("StrA050", typeof(Res))]
        A050,
        [LocalizedDescription("StrA060", typeof(Res))]
        A060,
        [LocalizedDescription("StrA070", typeof(Res))]
        A070,
        [LocalizedDescription("StrA080", typeof(Res))]
        A080,
        [LocalizedDescription("StrA090", typeof(Res))]
        A090,
        [LocalizedDescription("StrA100", typeof(Res))]
        A100,
        [LocalizedDescription("StrA110", typeof(Res))]
        A110,
        [LocalizedDescription("StrA120", typeof(Res))]
        A120,
        [LocalizedDescription("StrA130", typeof(Res))]
        A130,
        [LocalizedDescription("StrA140", typeof(Res))]
        A140,
        [LocalizedDescription("StrA150", typeof(Res))]
        A150,
        [LocalizedDescription("StrA160", typeof(Res))]
        A160,
        [LocalizedDescription("StrA170", typeof(Res))]
        A170,
        [LocalizedDescription("StrA180", typeof(Res))]
        A180,
        [LocalizedDescription("StrA190", typeof(Res))]
        A190,
        [LocalizedDescription("StrA200", typeof(Res))]
        A200,
        [LocalizedDescription("StrA210", typeof(Res))]
        A210,
        [LocalizedDescription("StrA220", typeof(Res))]
        A220,
        [LocalizedDescription("StrA230", typeof(Res))]
        A230,
        [LocalizedDescription("StrA240", typeof(Res))]
        A240,
        [LocalizedDescription("StrA250", typeof(Res))]
        A250,
        [LocalizedDescription("StrA260", typeof(Res))]
        A260,
        [LocalizedDescription("StrA270", typeof(Res))]
        A270,
        [LocalizedDescription("StrA280", typeof(Res))]
        A280,
        [LocalizedDescription("StrA290", typeof(Res))]
        A290,
        [LocalizedDescription("StrA300", typeof(Res))]
        A300,
        [LocalizedDescription("StrA310", typeof(Res))]
        A310,
        [LocalizedDescription("StrA320", typeof(Res))]
        A320,
        [LocalizedDescription("StrA330", typeof(Res))]
        A330,
        [LocalizedDescription("StrA340", typeof(Res))]
        A340,
        [LocalizedDescription("StrA350", typeof(Res))]
        A350,
        [LocalizedDescription("StrA360", typeof(Res))]
        A360,
        [LocalizedDescription("StrA370", typeof(Res))]
        A370,
        [LocalizedDescription("StrB010", typeof(Res))]
        B010,
        [LocalizedDescription("StrB020", typeof(Res))]
        B020,
        [LocalizedDescription("StrB030", typeof(Res))]
        B030,
        [LocalizedDescription("StrB040", typeof(Res))]
        B040,
        [LocalizedDescription("StrB050", typeof(Res))]
        B050,
        [LocalizedDescription("StrB060", typeof(Res))]
        B060,
        [LocalizedDescription("StrB070", typeof(Res))]
        B070,
        [LocalizedDescription("StrB080", typeof(Res))]
        B080,
        [LocalizedDescription("StrB090", typeof(Res))]
        B090,
        [LocalizedDescription("StrB100", typeof(Res))]
        B100,
        [LocalizedDescription("StrB110", typeof(Res))]
        B110,
        [LocalizedDescription("StrB120", typeof(Res))]
        B120,
        [LocalizedDescription("StrB130", typeof(Res))]
        B130,
        [LocalizedDescription("StrB140", typeof(Res))]
        B140,
        [LocalizedDescription("StrB150", typeof(Res))]
        B150,
        [LocalizedDescription("StrB160", typeof(Res))]
        B160,
        [LocalizedDescription("StrB170", typeof(Res))]
        B170,
        [LocalizedDescription("StrC010", typeof(Res))]
        C010,
        [LocalizedDescription("StrC020", typeof(Res))]
        C020,
        [LocalizedDescription("StrC030", typeof(Res))]
        C030,
        [LocalizedDescription("StrC040", typeof(Res))]
        C040,
        [LocalizedDescription("StrC050", typeof(Res))]
        C050,
        [LocalizedDescription("StrC060", typeof(Res))]
        C060,
        [LocalizedDescription("StrC070", typeof(Res))]
        C070,
        [LocalizedDescription("StrC080", typeof(Res))]
        C080,
    }
}
