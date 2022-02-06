
using System;
using System.IO;

namespace DMS.Module.Map.Infrastructure
{
    public class ConstantManager
    {
        #region API Key

        /// <summary>
        /// 공정 구성 조회
        /// </summary>
        public static readonly string APIKey_RoutingConfiguration = $"RoutingConfiguration/NonHierarchy?useYn=Y";

        /// <summary>
        /// 알람 시나리오 타겟 정보 조회
        /// </summary>
        public static readonly string APIKey_AlarmMonitoringTargets = $"AlarmMonitoringTargets?";

        /// <summary>
        /// DMS 컴포넌트 조회
        /// </summary>
        public static readonly string APIKey_DmComponentMst = $"DmComponentMst?";

        /// <summary>
        /// 알람 상태 조회 - 리소스
        /// </summary>
        public static readonly string APIKey_DmAlarmStatusList = $"DmAlarmStatusList?";

        /// <summary>
        /// 알람 상태 조회 - 상위 레벨(공정, 작업장, 설비 등등)
        /// </summary>
        public static readonly string APIKey_RoutingConfigutaionAlarmStatus = $"RoutingConfigutaionAlarmStatus?";

        /// <summary>
        /// Resource_Sensor
        /// </summary>
        public static readonly string APIKey_ResourceSensor = $"ResourceSensor?";

        /// <summary>
        /// 파일 관리
        /// </summary>
        public static readonly string APIKey_File = $"File?";

        /// <summary>
        /// 타겟
        /// </summary>
        public static readonly string APIKey_BaseInfoType = $"BaseInfoType?language=";

        /// <summary>
        /// 항목
        /// </summary>
        public static readonly string APIKey_BaseInfo = $"BaseInfo?type=";

        /// <summary>
        /// 사용자별 화면 작화 체크
        /// </summary>
        public static readonly string APIKey_SystemUseCheck = $"SystemUseCheck?";

        /// <summary>
        /// 센서 데이터
        /// </summary>
        public static readonly string APIKey_DataCollection = $"DataCollection?resourceCode=";

        /// <summary>
        /// 공정 마스터
        /// </summary>
        public static readonly string APIKey_Process = $"Process?id=";

        /// <summary>
        /// 설비 마스터
        /// </summary>
        public static readonly string APIKey_Equipment = $"Equipment?id=";

        #endregion

        #region BroadCast

        public const string BROADCAST_IPCAMERA_HIDE = "BROADCAST_IPCAMERA_HIDE";
        public const string BROADCAST_MAPEDIT = "MapEdit";
        public const string BROADCAST_FULLSCREEN = "FullScreen";
        public const string BROADCAST_ALARMCODE = "AlarmCode";
        public const string BROADCAST_ALARMLAYOUT = "AlarmLayout";

        #endregion

        #region

        /// <summary>
        /// DMS 마스터 데이터 조회 오류
        /// </summary>
        public const string LogMsg_Error_DMSMasterData = "[DMS Master Data Load Error]";

        /// <summary>
        /// DMS 기본 컴포넌트 속성 값 조회 오류
        /// </summary>
        public const string LogMag_Error_DMSBaseComponentPropertyData = "[DMS Base Component Property Data Load Error]";

        /// <summary>
        /// ResourceSensor 마스터 데이터 조회 오류
        /// </summary>
        public const string LogMag_Error_ResourceMasterData = "[Resource Master Data Load Error]";

        /// <summary>
        /// 타입/항목 중 타입 데이터 조회
        /// </summary>
        public const string LogMag_Error_BaseInfoTypeData = "[BaseInfoType Data Load Error]";

        #endregion

        #region File Path

        /// <summary>
        /// Environment.CurrentDirectory는 해당 프로그램에서의 경로(외부 프로그램에서 해당 프로그램 실행 시 경로 오류 발생)
        /// AppDomain.CurrentDomain.BaseDirectory는 실행 프로그램에서부터 경로(외부 프로그램에서 실행 시 오류 발생 안함)
        /// </summary>
        public static readonly string DIRECTORY_PROGRAM = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string DIRECTORY_SETTINGS = Path.Combine(DIRECTORY_PROGRAM, "Settings");
        public static readonly string DIRECTORY_GIFs = Path.Combine(DIRECTORY_PROGRAM, "Resource\\GIFs");
        public static readonly string FILE_MULTIPLEPANELS = Path.Combine(DIRECTORY_SETTINGS, "Multiplepanels");

        #endregion

        #region Json File

        public const string ASSET_CONSTRUCTURE_JSON = @"/Asset/AssetStructure.json";

        //Demo 차트 속성 데이터
        public const string BLACK_DEMO_CHART_JSON = @"/Asset/Theme/Black/Demo_Chart.json";
        public const string BLUE_DEMO_CHART_JSON = @"/Asset/Theme/Blue/Demo_Chart.json";
        public const string WHITE_DEMO_CHART_JSON = @"/Asset/Theme/White/Demo_Chart.json";

        //Component 기본 속성 데이터
        public const string BLACK_COMPONENTPROPERTY_JSON = @"/Asset/Theme/Black/Base_Component.json";
        public const string BLUE_COMPONENTPROPERTY_JSON = @"/Asset/Theme/Blue/Base_Component.json";
        public const string WHITE_COMPONENTPROPERTY_JSON = @"/Asset/Theme/White/Base_Component.json";

        #endregion

        #region MQTT Topic

        /// <summary>
        /// 알람 상태
        /// </summary>
        public const string MQTTTopic_AlarmState = "/event/c/alarm_state/";

        /// <summary>
        /// 센서 데이터
        /// </summary>
        public const string MQTTTopic_SensorData = "/event/c/data_collection_digit/";

        /// <summary>
        /// 차트
        /// </summary>
        public const string MQTTTopic_Chart = "/event/c/KPI/";

        /// <summary>
        /// 데이터 그리드
        /// </summary>
        public const string MQTTTopic_DataGrid = "/event/c/DataGrid/";

        /// <summary>
        /// 이미지
        /// </summary>
        public const string MQTTTopic_Image = "/event/c/image/";

        #endregion

        #region Size

        public const int MAX_LIST_SIZE = 50;
        public const int MAX_ABNORMALLIST_SIZE = 100;

        #endregion

        public const string ImageComponentDialogTitle = "Select a picture";

        public const string ImageDialogFilter = "Image files (png,jpg,jpeg,bmp)|*.png;*.jpg;*.jpeg;*bmp|All files (*.*)|*.*";
        public const string GifDialogFilter = "gif files |*.gif|All files (*.*)|*.*";

        public const string StringContent_MediaType = "application/json";

        /// <summary>
        /// MQTT Data 처리 주기
        /// </summary>
        public const int LOAD_DATA_TERM_MILIS = 100;
    }
}