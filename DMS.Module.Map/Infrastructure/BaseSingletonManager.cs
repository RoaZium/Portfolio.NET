using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Network;
using DMS.Module.Map.Properties;
using DMS.Module.Map.ViewModel;
using DMS.Module.Map.ViewModel.Component;
using Newtonsoft.Json;
using PrismWPF.Common;
using PrismWPF.Common.Broadcast;
using PrismWPF.Common.Converter;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DMS.Module.Map.Infrastructure
{
    public class BaseSingletonManager : DMViewModelBase
    {
        public BaseSingletonManager()
        {
        }

        public override void Load()
        {
            //1.DMS 기본 컴포넌트 속성 마스터 데이터 셋팅
            SetDMSBaseComponentProperty();

            //2.DMS 마스터 데이터 조회
            SetDMSMasterData();

            //3.ResourceSensor 마스터 데이터 조회
            SetResourceMasterData();

            //4.타입/항목 중 타입 데이터 조회
            SetBaseInfoType();
        }

        #region Singleton

        private static BaseSingletonManager _Instance;
        public static BaseSingletonManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new BaseSingletonManager();
                        }
                    }
                }
                return _Instance;
            }
        }

        #endregion

        #region Fields

        private static readonly object locker = new object();

        #endregion

        #region Properties

        /// <summary>
        /// 설명: 전체 DMS 자산 항목 관리
        /// 목적: 전체 DMS 자산 항목
        /// </summary>
        private ObservableCollection<DmComponentMst> _assetList;
        public ObservableCollection<DmComponentMst> AssetList
        {
            get
            {
                if (_assetList == null)
                {
                    _assetList = new ObservableCollection<DmComponentMst>();
                }
                return _assetList;
            }
            set => _assetList = value;
        }

        private Dictionary<string, CP_CommonBaseViewModel> _assetVMList;
        public Dictionary<string, CP_CommonBaseViewModel> AssetVMList
        {
            get
            {
                if (_assetVMList == null)
                {
                    _assetVMList = new Dictionary<string, CP_CommonBaseViewModel>();
                }
                return _assetVMList;
            }
            set => _assetVMList = value;
        }

        /// <summary>
        /// 설명: 타입별 리스트
        /// 목적: 컴포넌트 타입/항목에 사용되는 타입 리스트
        /// </summary>
        private List<DmBaseInfoTypeModel> _baseInfoTypeList;
        public List<DmBaseInfoTypeModel> BaseInfoTypeList
        {
            get
            {
                if (_baseInfoTypeList == null)
                {
                    _baseInfoTypeList = new List<DmBaseInfoTypeModel>();
                }
                return _baseInfoTypeList;
            }
            set
            {
                _baseInfoTypeList = value;
                RaisePropertyChanged("BaseInfoTypeList");
            }
        }

        /// <summary>
        /// Component Base
        /// </summary>
        private ObservableCollection<DmComponentMst> _componentBaseM;
        public ObservableCollection<DmComponentMst> ComponentBaseM
        {
            get
            {
                if (_componentBaseM == null)
                {
                    _componentBaseM = new ObservableCollection<DmComponentMst>();
                }
                return _componentBaseM;
            }
            set { _componentBaseM = value; }
        }

        /// <summary>
        /// 설명: 전체 ResourceSensor 리스트
        /// 목적: 관리치/한계치 관리
        /// </summary>
        private List<DmResourceSensorModel> _resourceSensorList;
        public List<DmResourceSensorModel> ResourceSensorList
        {
            get
            {
                if (_resourceSensorList == null)
                {
                    _resourceSensorList = new List<DmResourceSensorModel>();
                }
                return _resourceSensorList;
            }
            set => _resourceSensorList = value;
        }

        /// <summary>
        /// 시나리오 리스트
        /// </summary>
        private ObservableCollection<ScenarioModel> _scenarioList;
        public ObservableCollection<ScenarioModel> ScenarioList
        {
            get
            {
                if (_scenarioList == null)
                {
                    _scenarioList = new ObservableCollection<ScenarioModel>();
                }
                return _scenarioList;
            }
            set
            {
                _scenarioList = value;
                RaisePropertyChanged("ScenarioList");
            }
        }

        private ScenarioModel _selectedScenario;
        public ScenarioModel SelectedScenario
        {
            get
            {
                if (_selectedScenario == null)
                {
                    _selectedScenario = new ScenarioModel();
                }
                return _selectedScenario;
            }
            set
            {
                _selectedScenario = value;
                RaisePropertyChanged("SelectedScenario");
            }
        }

        /// <summary>
        /// 시나리오 액션 리스트
        /// </summary>
        private ObservableCollection<ActionPackerModel> _actionList;
        public ObservableCollection<ActionPackerModel> ActionList
        {
            get
            {
                if (_actionList == null)
                {
                    _actionList = new ObservableCollection<ActionPackerModel>();
                }
                return _actionList;
            }
            set
            {
                _actionList = value;
                RaisePropertyChanged("ActionList");
            }
        }

        private bool _isFullScreenMode;
        public bool IsFullScreenMode
        {

            get => _isFullScreenMode;
            set
            {
                _isFullScreenMode = value;
                BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_FULLSCREEN, _isFullScreenMode, null);
                RaisePropertyChanged("IsFullScreenMode");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 설명: Component Base Json Load
        /// 목적: Component Base Json Data Load
        /// </summary>
        private void SetDMSBaseComponentProperty()
        {
            try
            {
                string themePath = string.Empty;

                switch (EnvironmentSetting.Properties.Settings.Default.Theme)
                {
                    case nameof(ThemeType.Black):
                        {
                            themePath = ConstantManager.DIRECTORY_PROGRAM + ConstantManager.BLACK_COMPONENTPROPERTY_JSON;
                            break;
                        }
                    case nameof(ThemeType.Blue):
                        {
                            themePath = ConstantManager.DIRECTORY_PROGRAM + ConstantManager.BLUE_COMPONENTPROPERTY_JSON;
                            break;
                        }
                    case nameof(ThemeType.White):
                        {
                            themePath = ConstantManager.DIRECTORY_PROGRAM + ConstantManager.WHITE_COMPONENTPROPERTY_JSON;
                            break;
                        }
                }

                using (StreamReader r = new StreamReader(themePath))
                {
                    string json = r.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<ObservableCollection<DmComponentMst>>(json);

                    ComponentBaseM = result;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ConstantManager.LogMag_Error_DMSBaseComponentPropertyData);
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 설명: DMS Master Data
        /// 목적: 작화 데이테 셋팅
        /// </summary>
        private void SetDMSMasterData()
        {
            try
            {
                List<DmComponentMst> result = WebRequests.GetComponentMst(CoPlatformWebClient.Instance, null);

                if (result == null || result.Count == 0)
                {
                    return;
                }

                AssetList = result.ToObservableCollection();

                SetAssetVMList();

                //알람상태 정보 할당
                SetAlarmStateData();

                //이미지 바이너리 할당
                SetImageComponentInfo();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ConstantManager.LogMsg_Error_DMSMasterData);
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 설명: Resource Master Data
        /// 목적: Resource 데이터 셋팅(관리치, 한계치)
        /// </summary>
        private void SetResourceMasterData()
        {
            try
            {
                ResourceSensorList = WebRequests.GetResourceSensors(CoPlatformWebClient.Instance, null);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ConstantManager.LogMag_Error_ResourceMasterData);
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 설명: BaseInfoType List 조회
        /// 목적: 타입에 필요한 BaseInfoType 리스트 조회
        /// </summary>
        public void SetBaseInfoType()
        {
            try
            {
                LanguageType languageType = ConvertLanguageType();

                List<DmBaseInfoTypeModel> resultApi = WebRequests.GetBaseInfoType(CoPlatformWebClient.Instance, languageType);

                DmBaseInfoTypeModel dmBaseInfoTypeM = new DmBaseInfoTypeModel()
                {
                    Name = Res.StrScreen,
                    Type = Res.StrScreen
                };

                resultApi.Add(dmBaseInfoTypeM);

                BaseInfoTypeList = resultApi.OrderBy(p => p.Name).ToList();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ConstantManager.LogMag_Error_BaseInfoTypeData);
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 설명: DMS Master Data -> ViewModel 변경
        /// 목적: View 데이터 호출을 위한 ViewModel 사전 셋팅
        /// </summary>
        private void SetAssetVMList()
        {
            foreach (DmComponentMst item in AssetList)
            {
                var result = ConvertDMSMasterDataViewModel((ComponentType)Enum.Parse(typeof(ComponentType), item.RoutingType));

                if (result != null)
                {
                    result.DmComponentM = item;
                    AssetVMList.Add(result.DmComponentM.RoutingCode, result);
                }
            }
        }

        /// <summary>
        /// 설명: Master 데이터 -> ViewModel 할당
        /// 목적: View 데이터 호출을 위한  ViewModel 사전 셋팅
        /// </summary>
        /// <param name="componentType"></param>
        /// <returns></returns>
        public CP_CommonBaseViewModel ConvertDMSMasterDataViewModel(ComponentType componentType)
        {
            CP_CommonBaseViewModel vm = null;

            switch (componentType)
            {
                case ComponentType.DM701000:
                    {
                        vm = new CP_VirtualViewModel();
                        break;
                    }
                case ComponentType.DM701001:
                    {
                        vm = new CP_ImageViewModel();
                        break;
                    }
                case ComponentType.DM701002:
                    {
                        vm = new CP_VideoViewMode();
                        break;
                    }
                case ComponentType.DM701003:
                    {
                        vm = new CP_IPCameraViewModel();
                        break;
                    }
                case ComponentType.DM701005:
                    {
                        vm = new CP_TextBoxViewModel();
                        break;
                    }
                case ComponentType.DM801001:
                    {
                        vm = new CP_LineGraphViewModel();
                        break;
                    }
                case ComponentType.DM801002:
                    {
                        vm = new CP_VerticalGraphViewModel();
                        break;
                    }
                case ComponentType.DM801003:
                    {
                        vm = new CP_DataBoxViewModel();
                        break;
                    }
                case ComponentType.Gauge:
                    {
                        vm = new CP_GaugeViewModel();
                        break;
                    }
                case ComponentType.Indicator:
                    {
                        vm = new CP_IndicatorViewModel();
                        break;
                    }
                case ComponentType.ParetoChart:
                    {
                        vm = new CP_ParetoChartViewModel();
                        break;
                    }
                case ComponentType.PieChart:
                    {
                        vm = new CP_PieSeriesViewModel();
                        break;
                    }
                case ComponentType.ScatterChart:
                    {
                        vm = new CP_ScatterChartViewModel();
                        break;
                    }
                case ComponentType.XbarChart:
                    {
                        vm = new CP_XbarChartViewModel();
                        break;
                    }
                case ComponentType.LineSeries:
                    {
                        vm = new CP_LineSeriesChartViewModel();
                        break;
                    }
                case ComponentType.BarSeries:
                    {
                        vm = new CP_BarSeriesChartViewModel();
                        break;
                    }
                case ComponentType.DataGrid:
                    {
                        vm = new CP_DataGridViewModel();
                        break;
                    }
                case ComponentType.RadialProgressBar:
                    {
                        vm = new CP_RadialProgressBarViewModel();
                        break;
                    }
                case ComponentType.NestedDonutSeries:
                    {
                        vm = new CP_NestedDonutSeriesViewModel();
                        break;
                    }
                case ComponentType.ProgressBar:
                    {
                        vm = new CP_ProgressBarViewModel();
                        break;
                    }
                case ComponentType.Move:
                    {
                        vm = new CP_MoveViewModel();
                        break;
                    }
                case ComponentType.Layout:
                    {
                        vm = new CP_LayoutViewModel();
                        break;
                    }
                case ComponentType.Streaming:
                    {
                        vm = new CP_StreamingViewModel();
                        break;
                    }
                case ComponentType.ImageViewer:
                    {
                        vm = new CP_ImageViewerViewModel();
                        break;
                    }
                case ComponentType.AlarmHistory:
                    {
                        vm = new CP_AlarmHistoryViewModel();
                        break;
                    }
                case ComponentType.Gif:
                    {
                        vm = new CP_GifViewModel();
                        break;
                    }
                case ComponentType.AutoMode:
                    {
                        vm = new CP_AutoModeViewModel();
                        break;
                    }
                case ComponentType.DetailInfo:
                    {
                        vm = new CP_DetailInformationViewModel();
                        break;
                    }
                case ComponentType.AlarmState:
                    {
                        vm = new CP_AlarmStateViewModel();
                        break;
                    }
                case ComponentType.Animation:
                    {
                        vm = new CP_AnimationViewModel();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return vm;
        }


        /// <summary>
        /// 설명: 언어 리소스 명칭 변경
        /// 목적: 언어 리소스 파라미터 정의
        /// </summary>
        /// <returns></returns>
        private LanguageType ConvertLanguageType()
        {
            LanguageType languageType = LanguageType.Ko;

            if (CultureInfo.DefaultThreadCurrentCulture.Name == "ko-KR")
            {
                languageType = LanguageType.Ko;
            }
            else if (CultureInfo.DefaultThreadCurrentCulture.Name == "en-US")
            {
                languageType = LanguageType.En;
            }
            else if (CultureInfo.DefaultThreadCurrentCulture.Name == "ja-JP")
            {
                languageType = LanguageType.Ja;
            }

            return languageType;
        }

        public void SetAlarmStateData()
        {
            try
            {
                //API 호출
                List<DmRoutingConfigurationAlarmStatusModel> resultAPI = CoPlatformWebClient.Instance.GetRoutingConfigutaionAlarmStatus(null);

                if (resultAPI == null)
                {
                    return;
                }

                // API return 값 각 항목마다 저장
                foreach (DmRoutingConfigurationAlarmStatusModel item in resultAPI)
                {
                    foreach (DmComponentMst item2 in LinqManager.FilterWhereAllAlarmStatus())
                    {
                        //SI -> 추후 삭제 해야됨
                        if (item2.RoutingType == ComponentType.DM801003.ToString()
                            && item2.PropertyM.TargetCode != null)
                        {
                            item2.PropertyM.ItemCode = item2.PropertyM.TargetCode;
                        }

                        if (item.RoutingCode != item2.PropertyM.ItemCode)
                        {
                            continue;
                        }

                        if (item.AlarmLevel != null)
                        {
                            item2.PropertyM.AlarmState = (AlarmState)item.AlarmLevel;
                        }

                        if (item.CollectionValue != null)
                        {
                            item2.PropertyM.BlinkingLevel = item.BlinkingLevel;
                        }

                        if (item.CollectionValue != null)
                        {
                            item2.PropertyM.TextContent = item.CollectionValue.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "[DataBox.LoadAlarmState]" + ex.ToString());
            }
        }

        private void SetImageComponentInfo()
        {
            try
            {
                //1. 이미지 항목 필터
                //2. 필터 데이터에서 이미지 코드 추출
                //3. 추출한 코드로 이미지 바이너리 조회
                //4. 조회된 이미지 바이너리를 각각의 이미지 컴포넌트 할당
                IEnumerable<DmComponentMst> filterResult = LinqManager.FilterWhereImageComponent();

                if (filterResult == null)
                {
                    return;
                }

                string imageCodeList = null;

                foreach (DmComponentMst item in filterResult)
                {
                    if (item.PropertyM.ItemCode == null)
                    {
                        continue;
                    }

                    imageCodeList += "fileNames=" + item.PropertyM.ItemCode + "&";
                }

                List<CustomFileManager> resultAPIData = WebRequests.GetFileInfo(CoPlatformWebClient.Instance, imageCodeList);

                if (resultAPIData == null)
                {
                    return;
                }

                foreach (CustomFileManager item in resultAPIData)
                {
                    foreach (DmComponentMst list2 in filterResult)
                    {
                        if (item.FileName == list2.PropertyM.ItemCode)
                        {
                            list2.PropertyM.Imagebinary = item.FileData;
                            list2.PropertyM.ImageSource = ObjectConverter.ByteArrayToImageSource(item.FileData);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 설명: 타입/항목 리소스 코드 Topic 등록
        /// 목적: MQTT Topic Code 등록
        /// </summary>
        /// <param name="code"></param>
        public void RegisterResourceSensor(string code)
        {
            try
            {
                bool resultLinq = ResourceSensorList.Exists(p => p.ResourceCode == code);

                if (resultLinq == true)
                {
                    return;
                }

                var resultApi = WebRequests.GetResourceSensors(CoPlatformWebClient.Instance, "resourceCode=" + code);

                if (resultApi == null)
                {
                    return;
                }

                ResourceSensorList.AddRange(resultApi);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 설명: 타입/항목 리소스 코드 Topic 해제
        /// 목적: MQTT Topic Code 해제
        /// </summary>
        /// <param name="code"></param>
        public void UnRegisterResourceSensor(string code)
        {
            int resultLinq = ResourceSensorList.Count(p => p.ResourceCode == code);

            if (resultLinq > 1)
            {
            }
        }

        #endregion
    }
}
