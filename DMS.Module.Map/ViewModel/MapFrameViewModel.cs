using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using Coever.Lib.Mqtt.Core;
using DevExpress.Xpf.Docking;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Network;
using DMS.Module.Map.Properties;
using DMS.Module.Map.Requests;
using DMS.Module.Map.View;
using DMS.Module.Map.View.Component;
using DMS.Module.Map.View.Controls;
using DMS.Module.Map.ViewModel.Component;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common;
using PrismWPF.Common.Broadcast;
using PrismWPF.Common.Converter;
using PrismWPF.Common.Helpder;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DMS.Module.Map.ViewModel
{
    public class MapFrameViewModel : DMViewModelBase
    {
        public MapFrameViewModel()
        {
        }

        public override void Load()
        {
            FFMpegHelper.Register();

            BaseSingletonManager.Instance.Load();

            MqttReceiveManager = new MqttReceiveManager();

            AbnormalAlarmPopupView alarmPopupView = new AbnormalAlarmPopupView();

            MqttService.Instance.Client.ConnectionClosed += Client_ConnectionClosed;

            BroadcastReceiver.RegisterReceiver(ConstantManager.BROADCAST_ALARMCODE, OnAlarmCode);
            BroadcastReceiver.RegisterReceiver(ConstantManager.BROADCAST_ALARMLAYOUT, OnAlarmLayout);
        }

        public override void Unload()
        {
            IsAutoMonitoringMode = false;
            IsMapEdit = false;

            MqttService.Instance.Client.ConnectionClosed -= Client_ConnectionClosed;

            BroadcastReceiver.UnregisterReceiver(ConstantManager.BROADCAST_ALARMCODE, OnAlarmCode);
            BroadcastReceiver.UnregisterReceiver(ConstantManager.BROADCAST_ALARMLAYOUT, OnAlarmLayout);
        }

        private static readonly object locker = new object();

        private static MapFrameViewModel _Instance;
        public static MapFrameViewModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new MapFrameViewModel();
                        }
                    }
                }
                return _Instance;
            }
        }

        private ObservableCollection<UserControl> _Workspace;
        public ObservableCollection<UserControl> Workspace
        {
            get
            {
                if (_Workspace == null)
                {
                    _Workspace = new ObservableCollection<UserControl>();
                }
                return _Workspace;
            }
            set
            {
                _Workspace = value;
                RaisePropertyChanged("Workspace");
            }
        }

        private Visibility _visibilityLoading = Visibility.Hidden;
        public Visibility VisibilityLoading
        {
            get => _visibilityLoading;
            set
            {
                _visibilityLoading = value;
                RaisePropertyChanged("VisibilityLoading");
            }
        }

        private MapFrameView _MapFrameV;
        public MapFrameView MapFrameV
        {
            get
            {
                if (_MapFrameV == null)
                {
                    _MapFrameV = new MapFrameView();
                }
                return _MapFrameV;
            }
            set
            {
                _MapFrameV = value;
                RaisePropertyChanged("MapFrameV");
            }
        }

        private UserControl _assetListV;
        public UserControl AssetListV
        {
            get
            {
                if (_assetListV == null)
                {
                    _assetListV = new Asset_ListView();
                }
                return _assetListV;
            }
            set
            {
                _assetListV = value;
                RaisePropertyChanged("AssetListV");
            }
        }

        private PMSLayoutViewModel _PMSLayoutVM;
        public PMSLayoutViewModel PMSLayoutVM
        {
            get
            {
                if (_PMSLayoutVM == null)
                {
                    _PMSLayoutVM = new PMSLayoutViewModel();
                }
                return _PMSLayoutVM;
            }
            set => _PMSLayoutVM = value;
        }

        public MqttReceiveManager MqttReceiveManager;

        private bool _IsMapEdit;
        public bool IsMapEdit
        {
            get => _IsMapEdit;
            set
            {
                if (_IsMapEdit != value)
                {
                    _IsMapEdit = value;
                    RaisePropertyChanged("IsMapEdit");
                }
            }
        }

        private ScenarioTypes _RoutingConfigType;
        public ScenarioTypes RoutingConfigType
        {
            get => _RoutingConfigType;
            set => _RoutingConfigType = value;
        }

        private List<DmComponentMst> _routingConfigurationList;
        public List<DmComponentMst> RoutingConfigurationList
        {
            get
            {
                if (_routingConfigurationList == null)
                {
                    _routingConfigurationList = new List<DmComponentMst>();
                }
                return _routingConfigurationList;
            }
            set
            {
                _routingConfigurationList = value;
                RaisePropertyChanged("RoutingConfigurationList");
            }
        }

        private double _mapListOpacity = Settings.Default.MapListOpacity;
        public double MapListOpacity
        {
            get => _mapListOpacity;
            set
            {
                if (_mapListOpacity != value)
                {
                    Settings.Default.MapListOpacity = value;
                    Settings.Default.Save();
                }

                _mapListOpacity = value;
                RaisePropertyChanged("MapListOpacity");
            }
        }

        private double _mapPropertyOpacity = Settings.Default.MapPropertyOpacity;
        public double MapPropertyOpacity
        {
            get => _mapPropertyOpacity;
            set
            {
                if (_mapPropertyOpacity != value)
                {
                    Settings.Default.MapPropertyOpacity = value;
                    Settings.Default.Save();
                }

                _mapPropertyOpacity = value;
                RaisePropertyChanged("MapPropertyOpacity");
            }
        }

        #region Methods

        private void AddWorkspace(UserControl workspace)
        {
            if (workspace == null)
            {
                return;
            }

            Workspace.Add(workspace);
        }

        private void RemoveWorkspace(UserControl workspace)
        {
            if (workspace == null)
            {
                return;
            }

            if (Workspace.Contains(workspace))
            {
                Workspace.Remove(workspace);
            }
        }

        public void SelectedComponentConfig(DmComponentMst model)
        {
            if (model == null)
            {
                return;
            }

            UserControl overlapResult = Workspace.Where(p =>
            {
                PMSLayoutViewModel layoutVM = (p.DataContext as PMSLayoutViewModel);

                string key = layoutVM != null && layoutVM.DmComponentM != null ? layoutVM.DmComponentM.RoutingCode : null;

                return key == model.RoutingCode;
            }).FirstOrDefault();

            if (overlapResult != null)
            {
                (overlapResult.DataContext as PMSLayoutViewModel).IsActive = true;
                return;
            }

            PMSLayoutView view = new PMSLayoutView();
            (view.DataContext as PMSLayoutViewModel).DmComponentM = model;
            (view.DataContext as PMSLayoutViewModel).IsActive = true;

            AddWorkspace(view);
        }

        /// <summary>
        /// PMS 공정구성 항목 조회
        /// </summary>
        public void LoadRoutingConfiguration()
        {
            if (RoutingConfigurationList != null)
            {
                RoutingConfigurationList.Clear();
            }

            List<DmRoutingConfigurationModel> resultApi = WebRequests.GetRoutingConfiguration(CoPlatformWebClient.Instance);

            if (resultApi == null)
            {
                return;
            }

            SetBaseRoutingConfigurationInfo(resultApi); //조회
            SetRoutingConfigurationInfo(); //동기화
            RefreshRoutingConfigurationInfo(); //CRUD
        }

        /// <summary>
        /// Convert: DmRoutingConfigurationModel -> DmComponentMst
        /// </summary>
        /// <param name="dmRoutingConfigurationModels"></param>
        private void SetBaseRoutingConfigurationInfo(List<DmRoutingConfigurationModel> dmRoutingConfigurationModels)
        {
            if (RoutingConfigurationList != null)
            {
                RoutingConfigurationList.Clear();
            }

            foreach (DmRoutingConfigurationModel item in dmRoutingConfigurationModels)
            {
                DmComponentMst dmComponentMst = new DmComponentMst();

                dmComponentMst.RoutingCode = item.Id;
                dmComponentMst.PRoutingCode = item.ParentId;
                dmComponentMst.RoutingName = item.Name;
                dmComponentMst.PropertyM.ItemCode = item.MappingCode;
                dmComponentMst.PropertyM.ItemName = item.Name;
                dmComponentMst.PropertyM.ShapeSize = 40;

                if (item.Type.Contains("1"))
                {
                    dmComponentMst.RoutingType = ComponentType.DM601001.ToString();
                }
                else if (item.Type.Contains("2"))
                {
                    dmComponentMst.RoutingType = ComponentType.DM601003.ToString();
                }
                else if (item.Type.Contains("3"))
                {
                    dmComponentMst.RoutingType = ComponentType.DM601005.ToString();
                }

                RoutingConfigurationList.Add(dmComponentMst);
            }
        }

        /// <summary>
        /// PMS와 DMS 공정 구성 정보 동기화(수동)
        /// </summary>
        public void SetRoutingConfigurationInfo()
        {
            //mst 전체 항목 중 공정구성만 해당되는거 필터
            List<DmComponentMst> resultLinqMst = LinqManager.FilterWhereRoutingConfiguration(1).ToList();

            foreach (DmComponentMst item in RoutingConfigurationList)
            {
                //1.추가
                var resultLinq1 = resultLinqMst.Exists(p =>
                {
                    return p.RoutingCode == item.RoutingCode;
                });

                if (resultLinq1 == false)
                {
                    item.Action = ActionType.C;
                    continue;
                }

                //2.수정(부모 코드가 다르거나 명칭이 다른 경우 갱신 -> 자기 자신 코드는 같아야 함)
                var resultLinq2 = resultLinqMst.Exists(p =>
                {
                    return p.RoutingCode == item.RoutingCode &&
                    ((p.PRoutingCode != item.PRoutingCode &&
                    item.PRoutingCode != null) ||
                    (p.RoutingName != item.RoutingName));
                });

                if (resultLinq2 == true)
                {
                    item.Action = ActionType.U;
                    continue;
                }

                //3.동일
                var resultLinq3 = resultLinqMst.Exists(p =>
                {
                    return p.RoutingCode == item.RoutingCode && (p.PRoutingCode == item.PRoutingCode || item.PRoutingCode == null);
                });

                if (resultLinq3 == true)
                {
                    item.Action = ActionType.N;
                    continue;
                }
            }

            //4.삭제
            foreach (DmComponentMst item in resultLinqMst)
            {
                var resultLinq = RoutingConfigurationList.Exists(p =>
                {
                    return p.RoutingCode == item.RoutingCode;
                });

                if (resultLinq == true)
                {
                    continue;
                }

                DmComponentMst dmComponentMst = new DmComponentMst()
                {
                    Action = ActionType.D,
                    RoutingCode = item.RoutingCode
                };

                RoutingConfigurationList.Add(dmComponentMst);
            }
        }

        /// <summary>
        /// 공정 구성 항목 mst에 저장
        /// </summary>
        public void RefreshRoutingConfigurationInfo()
        {
            List<DmComponentMst> addComponentMstList = new List<DmComponentMst>();
            List<DmComponentMst> updateComponentMstList = new List<DmComponentMst>();
            List<DmComponentMst> deleteComponentMstList = new List<DmComponentMst>();

            foreach (DmComponentMst item in RoutingConfigurationList)
            {
                switch (item.Action)
                {
                    case ActionType.C:
                        {
                            addComponentMstList.Add(item);
                            break;
                        }
                    case ActionType.D:
                        {
                            deleteComponentMstList.Add(item);
                            break;
                        }
                    case ActionType.U:
                        {
                            updateComponentMstList.Add(item);
                            break;
                        }
                }
            }

            if (addComponentMstList.Count > 0)
            {
                WebRequests.PostComponentMst(CoPlatformWebClient.Instance, addComponentMstList);
            }

            if (updateComponentMstList.Count > 0)
            {
                WebRequests.PutComponentMst(CoPlatformWebClient.Instance, updateComponentMstList);
            }

            if (deleteComponentMstList.Count > 0)
            {
                WebRequests.DeleteComponentMst(CoPlatformWebClient.Instance, deleteComponentMstList);
            }
        }

        public UserControl MakeComponent(ComponentType componentType)
        {
            UserControl uc = null;
            switch (componentType)
            {
                case ComponentType.DM701003:
                    {
                        uc = new CP_IPCameraView();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return uc;
        }

        #endregion

        #region Commands

        public DelegateCommand<DocumentGroup> SelectedMapChangedCommand => new DelegateCommand<DocumentGroup>(SelectedMapChangedCommandExecute);

        public DelegateCommand<object> CloseCommand => new DelegateCommand<object>(CloseCommandExecute);

        private void SelectedMapChangedCommandExecute(DocumentGroup docGroup)
        {
            if (docGroup == null || docGroup.SelectedItem == null)
            {
                return;
            }

            IsMapEdit = false;

            PMSLayoutVM = docGroup.SelectedItem.DataContext as PMSLayoutViewModel;
            docGroup.SelectedTabIndex = docGroup.SelectedTabIndex;

            DmComponentMst resultLinq = LinqManager.FilterFirstRoutingCode(PMSLayoutVM.DmComponentM.RoutingCode);

            if (resultLinq != null)
            {
                if (AssetListV.DataContext is Asset_ListViewModel vm)
                {
                    vm.SelectedItemLayoutList = resultLinq;
                }
            }
        }

        private void CloseCommandExecute(object sender)
        {

            if (!((sender as DocumentPanel).DataContext is PMSLayoutViewModel viewModel))
            {
                return;
            }

            if (viewModel.DmComponentM != null)
            {
                PMSLayoutView workspace = (sender as DocumentPanel).Content as PMSLayoutView;
                RemoveWorkspace(workspace);
            }
        }

        #endregion

        #region Auto Monitoring

        #region Properties
        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<ExecuteActionRequest>>().Unsubscribe(OnExecuteAction);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<ExecuteActionRequest>>().Subscribe(OnExecuteAction);
                }
            }
        }

        private bool _IsAutoMonitoringMode = false;
        public bool IsAutoMonitoringMode
        {
            get => _IsAutoMonitoringMode;
            set
            {
                _IsAutoMonitoringMode = value;
                RaisePropertyChanged("IsAutoMonitoringMode");
            }
        }

        #endregion

        #region Methods

        private async void ExecuteAction(ExecuteActionRequest req)
        {
            try
            {
                ScenarioTypes mapType = req.Scenario.ScenarioType;
                IActionModel action = req.Action;

                IList<DmComponentMst> compList = null;
                compList = BaseSingletonManager.Instance.AssetList;

                switch (action.ActionType)
                {
                    case ActionTypes.layout:
                        {
                            LayoutActionModel parsedAction = action as LayoutActionModel;

                            DmComponentMst layout = compList?.FirstOrDefault(p => p.RoutingCode == parsedAction.LayoutID);
                            if (layout != null)
                            {
                                SelectedComponentConfig(layout);
                            }
                            break;
                        }
                    case ActionTypes.alarm_layout:
                        {
                            AlarmLayoutActionModel parsedAction = action as AlarmLayoutActionModel;

                            DmComponentMst comp = compList?.FirstOrDefault(p => p.RoutingCode == req.TargetCode);
                            if (comp != null)
                            {
                                if (comp.RoutingType == ComponentType.DM601005.ToString()
                                    || comp.RoutingType == ComponentType.DM701001.ToString()
                                    || comp.RoutingType == ComponentType.DM701002.ToString()
                                    || comp.RoutingType == ComponentType.DM701003.ToString())
                                {
                                    DmComponentMst layout = compList?.FirstOrDefault(p => p.RoutingCode == comp.PRoutingCode);
                                    if (layout != null)
                                    {
                                        SelectedComponentConfig(layout);
                                    }
                                }
                                else
                                {
                                    SelectedComponentConfig(comp);
                                }
                            }
                            break;
                        }
                    case ActionTypes.ipcam_compo:
                        {
                            IPCamCompoActionModel parsedAction = action as IPCamCompoActionModel;

                            DmComponentMst comp = compList?.FirstOrDefault(p => p.RoutingCode == parsedAction.ComponentID);
                            if (comp != null)
                            {
                                DmComponentMst layout = compList?.FirstOrDefault(p => p.RoutingCode == comp.PRoutingCode);
                                if (layout != null)
                                {
                                    MapFrameViewModel.Instance.SelectedComponentConfig(layout);

                                    if (Workspace.Where(p =>
                                    {
                                        if (p.DataContext is ILayoutViewModel)
                                        {
                                            ILayoutViewModel vm = (p.DataContext as ILayoutViewModel);

                                            if (vm.DmComponentM != null)
                                            {
                                                return layout.RoutingCode == vm.DmComponentM.RoutingCode;
                                            }
                                        }
                                        return false;
                                    }).FirstOrDefault()?.DataContext is ILayoutViewModel layoutVM)
                                    {
                                        await layoutVM.ExecuteComponentAction(action);
                                    }
                                }
                            }
                            break;
                        }
                }

                req.IsFinished = true;
            }
            catch
            {
                req.IsCanceled = true;
            }
        }

        private void InitializeIPcameraActivation()
        {
            try
            {
                var resultLinq = PMSLayoutVM.VMList.Where(p =>
                {
                    return p.DmComponentM.RoutingType == ComponentType.DM701003.ToString();
                });

                if (resultLinq == null)
                {
                    return;
                }

                foreach (var item in resultLinq)
                {
                    (item as CP_IPCameraViewModel).IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        #endregion

        #region Events

        private void OnExecuteAction(ExecuteActionRequest req)
        {
            //ExecuteAction(req);
        }

        //private void OnSettingMenu(string actionName, object sender, object e)
        //{
        //    IsMenuCheck = Convert.ToBoolean(sender);

        //    if(IsMenuCheck == false)
        //    {
        //        VisibilityLayoutList = Visibility.Collapsed;
        //    }
        //    else
        //    {
        //        VisibilityLayoutList = Visibility.Visible;
        //    }
        //}

        private void Client_ConnectionClosed(object sender, EventArgs e)
        {
            MqttReceiveManager = new MqttReceiveManager();
        }

        #endregion

        #endregion

        #region Chart Data

        public void LoadResourceSensorData(DmComponentMst dmComponentMst)
        {
            try
            {
                dmComponentMst.ChartSeriesList = null;

                List<DmDataCollectionModel> resultAPI = CoPlatformWebClient.Instance.GetResourceSensorData(dmComponentMst.PropertyM.ItemCode);

                if (resultAPI == null)
                {
                    return;
                }

                var data = resultAPI.OrderBy(p => p.CollectionDate);

                foreach (DmDataCollectionModel item in data)
                {
                    DmChartDataModel dmChartDataModel = new DmChartDataModel
                    {
                        ArgumentDataMember = item.CollectionDate.ToString("HH:mm:ss"),
                        ValueDataMember = item.CollectionValue
                    };

                    if (dmComponentMst.ChartSeriesList.Count > ConstantManager.MAX_LIST_SIZE)
                    {
                        dmComponentMst.ChartSeriesList.RemoveAt(0);
                    }

                    dmComponentMst.ChartSeriesList.Add(dmChartDataModel);
                }
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, e.ToString());
            }
        }

        /// <summary>
        /// 실시간 발생하는 차트 데이터 처리
        /// </summary>
        /// <param name="dmComponentMst"></param>
        /// <param name="data"></param>
        public void SetResourceSensorData(DmComponentMst dmComponentMst, DmDataCollectionModel data)
        {
        }

        #endregion

        #region Properties

        private DmComponentMst _dmComponentM;
        public DmComponentMst DmComponentM
        {
            get
            {
                if (_dmComponentM == null)
                {
                    _dmComponentM = new DmComponentMst();
                }
                return _dmComponentM;
            }
            set
            {
                _dmComponentM = value;
                RaisePropertyChanged("DmComponentM");
            }
        }

        #endregion


        #region DelegateCommands

        public DelegateCommand<ComponentType?> AddComponentCommand => new DelegateCommand<ComponentType?>(AddComponentCommandExecute);
        public DelegateCommand ImageFileCommand => new DelegateCommand(ImageFileCommandExecute);
        public DelegateCommand GifFileCommand => new DelegateCommand(GifFileCommandExecute);
        public DelegateCommand LostFocusCommand => new DelegateCommand(LostFocusCommandExecute);
        public DelegateCommand VideoFileCommand => new DelegateCommand(VideoFileCommandExecute);

        #endregion

        #region Executes

        private void AddComponentCommandExecute(ComponentType? obj)
        {
            if (obj == null)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "AddComponentCommandExecute Error");
                return;
            }

            if (DmComponentM != null)
            {
                DmComponentM = null;
            }

            ComponentType cpType = (ComponentType)Enum.Parse(typeof(ComponentType), obj.ToString());

            SetBasicComponentInfo(cpType);

            //신규 코드 생성
            DmComponentM.RoutingCode = Guid.NewGuid().ToString();

            //컴포넌트 저장-PMSLayoutViewModel
            PMSLayoutVM.SaveComponentmst((ComponentType)Enum.Parse(typeof(ComponentType), DmComponentM.RoutingType), DmComponentM);

            //컴포넌트 속성창 활성화-LayoutPropertyViewModel
            SetCPView(cpType, DmComponentM);

            DmComponentM.Action = ActionType.C;

            if (PMSLayoutVM.OriginComponentData == null)
            {
                return;
            }

            //기존 Origin Data 저장 - OriginData가 없으면(N)이면 리스트에 추가 안함)
            if (PMSLayoutVM.OriginComponentData.RoutingCode != DmComponentM.RoutingCode)
            {
                PMSLayoutVM.RevertBackList.Add(PMSLayoutVM.OriginComponentData.CopyObject());
            }

            PMSLayoutVM.RevertBackList.Add(DmComponentM.CopyObject());
            DmComponentM.Action = ActionType.U;
            PMSLayoutVM.OriginComponentData = DmComponentM.CopyObject();
        }

        public void SetBasicComponentInfo(ComponentType cpType)
        {
            try
            {
                DmComponentM = BaseSingletonManager.Instance.ComponentBaseM.FirstOrDefault(p => p.RoutingType == cpType.ToString()).CopyObject<DmComponentMst>();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 이미지 파일
        /// </summary>
        private async void ImageFileCommandExecute()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = ConstantManager.ImageComponentDialogTitle,
                    Filter = ConstantManager.ImageDialogFilter
                };

                int statusCode = 0;

                if (openFileDialog.ShowDialog() == true)
                {
                    ComponentMst.PropertyM.ImageName = openFileDialog.SafeFileName; // 파일명
                    ComponentMst.PropertyM.ImagePath = openFileDialog.FileName; // 파일경로
                    ComponentMst.PropertyM.ImageSource = ObjectConverter.ToImageSource(openFileDialog.FileName);

                    if (ComponentMst.PropertyM.ItemCode == null)
                    {
                        ComponentMst.PropertyM.ItemCode = DateTime.UtcNow.ToFileTime().ToString();
                        statusCode = await Task.Run(() => WebRequests.PostFileInfo(CoPlatformWebClient.Instance, new List<DmComponentMst> { ComponentMst }));
                    }
                    else
                    {
                        statusCode = await Task.Run(() => WebRequests.PutFileInfo(CoPlatformWebClient.Instance, new List<DmComponentMst> { ComponentMst }));
                    }

                    LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "PutFileInfo: " + statusCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Res.MsgCantLoadImage);
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 이미지 파일
        /// </summary>
        private async void GifFileCommandExecute()
        {
            try
            {
                await Task.Run(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Title = ConstantManager.ImageComponentDialogTitle,
                        Filter = ConstantManager.GifDialogFilter,
                        InitialDirectory = ConstantManager.DIRECTORY_GIFs
                    };

                    if (openFileDialog.ShowDialog() == true)
                    {
                        ComponentMst.PropertyM.GifM.Name = openFileDialog.SafeFileName; // 파일명
                        ComponentMst.PropertyM.GifM.FilePath = ConstantManager.DIRECTORY_GIFs + "\\" + openFileDialog.SafeFileName; // 파일경로
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(Res.MsgCantLoadImage);
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        /// <summary>
        /// 컴포넌트 속성 저장 - 마우스 포커스 잃어버린 시점 저장
        /// </summary>
        private void LostFocusCommandExecute()
        {
            string data = JsonConvert.SerializeObject(ComponentMst, Formatting.Indented);
            string data1 = JsonConvert.SerializeObject(PMSLayoutVM.OriginComponentData, Formatting.Indented);

            if (data != data1)
            {
                if(PMSLayoutVM.OriginComponentData == null)
                {
                    return;
                }

                PMSLayoutVM.OriginComponentData.Action = ActionType.U;

                PMSLayoutVM.RevertBackList.Add(PMSLayoutVM.OriginComponentData.CopyObject());

                //Origin 값 Revert에 넣고 다시 복사
                PMSLayoutVM.OriginComponentData = ComponentMst.CopyObject();
            }

            PMSLayoutVM.UpdateComponentmst(ComponentMst);

            if (ComponentMst.RoutingType == ComponentType.DM801001.ToString() ||
                ComponentMst.RoutingType == ComponentType.DM801002.ToString())
            {
                PMSLayoutVM.UpdateResourceSensor(ComponentMst.ResourceM);
            }
        }

        /// <summary>
        /// 비디오 파일
        /// </summary>
        private void VideoFileCommandExecute()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a video",
                Filter = "Video files (avi,mkv,wmv,mp4,mov)|*.avi;*.mkv;*.wmv;*mp4;*mov|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    ComponentMst.PropertyM.FilePath = openFileDialog.FileName;
                    ComponentMst.PropertyM.FileName = openFileDialog.SafeFileName;
                }
                catch (Exception e)
                {
                    MessageBox.Show(Res.MsgCantLoadImage);
                    LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, e.ToString());
                }
            }
        }

        #endregion





        #region 맵 속성
        #region Properties

        private DmComponentMst _componentMst;
        public DmComponentMst ComponentMst
        {
            get
            {
                if (_componentMst == null)
                {
                    _componentMst = new DmComponentMst();
                }
                return _componentMst;
            }
            set
            {
                _componentMst = value;
                RaisePropertyChanged("ComponentMst");
            }
        }

        #region 분류/항목

        private DmBaseInfoTypeModel _selectedbaseInfoType;
        public DmBaseInfoTypeModel SelectedBaseInfoType
        {
            get
            {
                if (_selectedbaseInfoType == null)
                {
                    _selectedbaseInfoType = new DmBaseInfoTypeModel();
                }
                return _selectedbaseInfoType;
            }
            set
            {
                _selectedbaseInfoType = value;

                if (_selectedbaseInfoType != null)
                {
                    ComponentMst.PropertyM.CategoryCode = _selectedbaseInfoType.Type;
                    ComponentMst.PropertyM.CategoryName = _selectedbaseInfoType.Name;

                    SetItemList();
                }

                RaisePropertyChanged("SelectedBaseInfoType");
            }
        }

        private ObservableCollection<DmBaseInfoModel> _baseInfoList;
        public ObservableCollection<DmBaseInfoModel> BaseInfoList
        {
            get
            {
                if (_baseInfoList == null)
                {
                    _baseInfoList = new ObservableCollection<DmBaseInfoModel>();
                }
                return _baseInfoList;
            }
            set
            {
                _baseInfoList = value;
                RaisePropertyChanged("BaseInfoList");
            }
        }

        private DmBaseInfoModel _selectedbaseInfo;
        public DmBaseInfoModel SelectedBaseInfo
        {
            get
            {
                if (_selectedbaseInfo == null)
                {
                    _selectedbaseInfo = new DmBaseInfoModel();
                }
                return _selectedbaseInfo;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                _selectedbaseInfo = value;

                if (_selectedbaseInfo != null)
                {
                    SetComponentProperty();
                }

                RaisePropertyChanged("SelectedBaseInfo");
            }
        }

        private void SetComponentProperty()
        {
            //타이틀 없으면 선택된 센서 명칭으로 저장
            if (string.IsNullOrEmpty(ComponentMst.PropertyM.ItemName))
            {
                ComponentMst.PropertyM.ItemName = SelectedBaseInfo.Name;
            }

            if (ComponentMst.PropertyM.ItemCode == SelectedBaseInfo.BaseInfoCode)
            {
                return;
            }

            ComponentMst.PropertyM.ItemType = SelectedBaseInfo.Type;
            ComponentMst.PropertyM.ItemCode = SelectedBaseInfo.BaseInfoCode;
            ComponentMst.PropertyM.ItemName = SelectedBaseInfo.Name;
            ComponentMst.BaseInfoCode = SelectedBaseInfo.BaseInfoCode;

            //선택한 항목 Topic 코드 등록
            PMSLayoutVM.ChangeTopicCode(ComponentMst.RoutingCode);

            //선택한 항목 ResourceSensor 정보 등록
            BaseSingletonManager.Instance.RegisterResourceSensor(ComponentMst.BaseInfoCode);

            //센서 변경 시 item code 수정, 타이틀 수정
            switch (ComponentMst.RoutingType)
            {
                case nameof(ComponentType.DM801001):
                case nameof(ComponentType.DM801002):
                    {
                        //변경된 리소스 할당
                        ComponentMst.ResourceM = LinqManager.FilterFirstResourceSensor(ComponentMst.PropertyM.ItemCode);
                        //차트 이력 데이터 조회
                        LoadResourceSensorData(ComponentMst);
                        break;
                    }
                case nameof(ComponentType.DM801003):
                case nameof(ComponentType.Animation):
                    {
                        SearchAlarmStateInfo();
                        break;
                    }
                case nameof(ComponentType.Layout):
                    {
                        var resultLinq = PMSLayoutVM.VMList.FirstOrDefault(p => p.DmComponentM.RoutingCode == ComponentMst.RoutingCode);

                        (resultLinq as CP_LayoutViewModel).LoadComponentList();
                        break;
                    }
                case nameof(ComponentType.DetailInfo):
                    {
                        var resultLinq = PMSLayoutVM.VMList.FirstOrDefault(p => p.DmComponentM.RoutingCode == ComponentMst.RoutingCode);
                        (resultLinq as CP_DetailInformationViewModel).SetMasterData();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void SetCPView(ComponentType cpType, DmComponentMst dmComponentMst)
        {
            ComponentMst = dmComponentMst;
        }

        /// <summary>
        /// 기존, 신규 컴포넌트 속성창 제공
        /// </summary>
        /// <param name="cpType"></param>
        public void SetComponentPropertyView(ComponentType cpType)
        {
            DmComponentMst componentMst = new DmComponentMst();

            switch (cpType)
            {
                case ComponentType.DM601001:
                    {
                        break;
                    }
                case ComponentType.DM601002:
                    {
                        componentMst.RoutingName = Res.StrWorkcenterComponent;
                        componentMst.RoutingType = ComponentType.DM601002.ToString();
                        break;
                    }
                case ComponentType.DM601003:
                    {
                        componentMst.RoutingName = Res.StrEquipmentComponent;
                        componentMst.RoutingType = ComponentType.DM601003.ToString();
                        break;
                    }
                case ComponentType.DM601004:
                    {
                        componentMst.RoutingName = Res.StrTerminalComponent;
                        componentMst.RoutingType = ComponentType.DM601003.ToString();
                        break;
                    }
                case ComponentType.DM601005:
                    {
                        componentMst.RoutingType = ComponentType.DM601005.ToString();
                        break;
                    }
                case ComponentType.DM701000:
                    {
                        componentMst.RoutingName = Res.StrVirtualComponent;
                        componentMst.RoutingType = ComponentType.DM701000.ToString();
                        break;
                    }
                case ComponentType.DM701001:
                    {
                        componentMst.RoutingName = Res.StrImageComponent;
                        componentMst.RoutingType = ComponentType.DM701001.ToString();
                        break;
                    }
                case ComponentType.DM701002:
                    {
                        componentMst.RoutingName = Res.StrVideoComponent;
                        componentMst.RoutingType = ComponentType.DM701002.ToString();
                        break;
                    }
                case ComponentType.DM701003:
                    {
                        componentMst.RoutingName = Res.StrIPCameraComponent;
                        componentMst.RoutingType = ComponentType.DM701003.ToString();
                        break;
                    }
                case ComponentType.DM701005:
                    {
                        componentMst.RoutingName = Res.StrTextBoxComponent;
                        componentMst.RoutingType = ComponentType.DM701005.ToString();
                        break;
                    }
                case ComponentType.DM801001:
                    {
                        componentMst.RoutingName = Res.StrLineGraphComponent;
                        componentMst.RoutingType = ComponentType.DM801001.ToString();
                        break;
                    }
                case ComponentType.DM801002:
                    {
                        componentMst.RoutingName = Res.StrVerticalBarGraphComponent;
                        componentMst.RoutingType = ComponentType.DM801002.ToString();
                        break;
                    }
                case ComponentType.DM801003:
                    {
                        componentMst.RoutingName = Res.StrDataBoxComponent;
                        componentMst.RoutingType = ComponentType.DM801003.ToString();
                        break;
                    }
                case ComponentType.Gauge:
                    {
                        componentMst.RoutingName = Res.StrGaugeComponent;
                        componentMst.RoutingType = ComponentType.Gauge.ToString();
                        break;
                    }
                case ComponentType.Indicator:
                    {
                        componentMst.RoutingName = Res.StrIndicatorComponent;
                        componentMst.RoutingType = ComponentType.Indicator.ToString();
                        break;
                    }
                case ComponentType.ParetoChart:
                    {
                        componentMst.RoutingName = Res.StrParetoChartComponent;
                        componentMst.RoutingType = ComponentType.ParetoChart.ToString();
                        break;
                    }
                case ComponentType.PieChart:
                    {
                        componentMst.RoutingName = Res.StrPieChartComponent;
                        componentMst.RoutingType = ComponentType.PieChart.ToString();
                        break;
                    }
                case ComponentType.ScatterChart:
                    {
                        componentMst.RoutingName = Res.StrScatterChartComponent;
                        componentMst.RoutingType = ComponentType.ScatterChart.ToString();
                        break;
                    }
                case ComponentType.XbarChart:
                    {
                        componentMst.RoutingName = Res.StrXbarChartComponent;
                        componentMst.RoutingType = ComponentType.XbarChart.ToString();
                        break;
                    }
                case ComponentType.LineSeries:
                    {
                        componentMst.RoutingName = Res.StrLineSeriesComponent;
                        componentMst.RoutingType = ComponentType.LineSeries.ToString();
                        break;
                    }
                case ComponentType.BarSeries:
                    {
                        componentMst.RoutingName = Res.StrBarSeriesComponent;
                        componentMst.RoutingType = ComponentType.BarSeries.ToString();
                        break;
                    }
                case ComponentType.DataGrid:
                    {
                        componentMst.RoutingName = Res.StrDataGridComponent;
                        componentMst.RoutingType = ComponentType.DataGrid.ToString();
                        break;
                    }
                case ComponentType.RadialProgressBar:
                    {
                        componentMst.RoutingName = Res.StrRadialProgressBarComponent;
                        componentMst.RoutingType = ComponentType.RadialProgressBar.ToString();
                        break;
                    }
                case ComponentType.NestedDonutSeries:
                    {
                        componentMst.RoutingName = Res.StrNestedDonutSeriesComponent;
                        componentMst.RoutingType = ComponentType.NestedDonutSeries.ToString();
                        break;
                    }
                case ComponentType.ProgressBar:
                    {
                        componentMst.RoutingName = Res.StrProgressBar;
                        componentMst.RoutingType = ComponentType.ProgressBar.ToString();
                        break;
                    }
                case ComponentType.Move:
                    {
                        componentMst.RoutingName = "이동";
                        componentMst.RoutingType = ComponentType.Move.ToString();
                        break;
                    }
                case ComponentType.Layout:
                    {
                        componentMst.RoutingName = Res.StrLayoutComponent;
                        componentMst.RoutingType = ComponentType.Layout.ToString();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            ComponentMst = componentMst;
        }

        #endregion

        private readonly List<Coever.Lib.CoPlatform.Core.Models.DmIpcamMst> _IPCamList = new List<Coever.Lib.CoPlatform.Core.Models.DmIpcamMst>();
        private ListCollectionView _IPCamListView;
        public ListCollectionView IPCamListView
        {
            get
            {
                if (_IPCamListView == null)
                {
                    _IPCamListView = new ListCollectionView(_IPCamList);
                }

                return _IPCamListView;
            }
        }

        private uint? _SelectedCamCode;
        public uint? SelectedCamCode
        {
            get => _SelectedCamCode;
            set
            {
                _SelectedCamCode = value;
                ComponentMst.PropertyM.TargetCode = _SelectedCamCode.ToString();

                var linqResult = _IPCamList.FirstOrDefault(p => p.Code.ToString() == ComponentMst.PropertyM.TargetCode);

                if (linqResult == null)
                {
                    return;
                }

                ComponentMst.PropertyM.ItemName = linqResult.Name;

                SelectedIPCamera();

                RaisePropertyChanged("SelectedCamCode");
            }
        }

        private void SelectedIPCamera()
        {
            var resultLinq = PMSLayoutVM.VMList.FirstOrDefault(p =>
            {
                return p.DmComponentM.RoutingCode == ComponentMst.RoutingCode;
            });

            CP_IPCameraViewModel ipcamVM = resultLinq as CP_IPCameraViewModel;

            ipcamVM.DmComponentM.PropertyM.TargetCode = ComponentMst.PropertyM.TargetCode;

            if (ComponentMst != null && ComponentMst.PropertyM.TargetCode != null && uint.TryParse(ComponentMst.PropertyM.TargetCode, out uint camCode))
            {
                ipcamVM.IPCamPopupViewModel = new IPCamPopupWindowViewModel(camCode, ComponentMst.RoutingCode);
            }
        }

        private void SearchAlarmStateInfo()
        {
            try
            {
                List<DmAlarmStatus> dmAlarmStatuses = CoPlatformWebClient.Instance.GetAlarmStatus("resourceCode=" + ComponentMst.PropertyM.ItemCode);

                if (dmAlarmStatuses == null)
                {
                    return;
                }

                if (dmAlarmStatuses.Count == 0)
                {
                    return;
                }

                if (dmAlarmStatuses[0] == null)
                {
                    return;
                }

                ComponentMst.PropertyM.TextContent = dmAlarmStatuses[0].CollectionValue.ToString();
                ComponentMst.PropertyM.AlarmState = (AlarmState)dmAlarmStatuses[0].AlarmLevel;
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, e.ToString());
            }
        }

        public List<ResourceIcon> ResourceIconList => Enum.GetValues(typeof(ResourceIcon)).Cast<ResourceIcon>().ToList<ResourceIcon>();

        private ResourceIcon _SelectedResourceIcon;
        public ResourceIcon SelectedResourceIcon
        {
            get => _SelectedResourceIcon;
            set
            {
                _SelectedResourceIcon = value;
                ComponentMst.PropertyM.IconCode = _SelectedResourceIcon.ToString();
                RaisePropertyChanged("SelectedResourceIcon");
            }
        }

        #endregion

        #region Methods

        #region 분류/항목

        /// <summary>
        /// 분류 - 선택 항목
        /// </summary>
        public void SelectedCategoryData()
        {
            SelectedBaseInfoType = LinqManager.FilterFirstBaseInfoType(ComponentMst.PropertyM.CategoryCode);
        }

        private void SetItemList()
        {
            try
            {
                if(BaseInfoList != null)
                {
                    BaseInfoList = null;
                }

                if (SelectedBaseInfoType.Type == Res.StrScreen)
                {
                    foreach (DmComponentMst item in LinqManager.FilterWhereRoutingConfiguration())
                    {
                        //자기 자신 레이아웃은 배제: 무한 루프 발생
                        if(ComponentMst.PRoutingCode == item.RoutingCode)
                        {
                            continue;
                        }

                        BaseInfoList.Add(new DmBaseInfoModel()
                        {
                            Name = item.RoutingName,
                            BaseInfoCode = item.RoutingCode
                        });
                    }
                }
                else
                {
                    BaseInfoList = WebRequests.GetBaseInfo(CoPlatformWebClient.Instance, SelectedBaseInfoType.Type).ToObservableCollection();
                }

                if (BaseInfoList == null)
                {
                    return;
                }

                var result = BaseInfoList.FirstOrDefault(p =>
                {
                    return p.BaseInfoCode == ComponentMst.PropertyM.ItemCode;
                });

                if (result == null)
                {
                    return;
                }

                SelectedBaseInfo = result;
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        #endregion

        #region 컴포넌트 기본 속성 설정

        private void SetBasicResourceSensorComponentData()
        {
            //Null 체크
            if (ComponentMst.PropertyM.IconCode == null)
            {
                ComponentMst.PropertyM.IconCode = ResourceIcon.A000.ToString();
            }

            //Icon 명칭 체크
            if (!Enum.IsDefined(typeof(ResourceIcon), ComponentMst.PropertyM.IconCode))
            {
                SelectedResourceIcon = ResourceIcon.A000;
            }

            SelectedResourceIcon = (ResourceIcon)Enum.Parse(typeof(ResourceIcon), ComponentMst.PropertyM.IconCode);
        }

        private void SetBasicIPCameraComponentData()
        {
            LoadCamList();
        }

        #endregion

        /// <summary>
        /// 컴포넌트 기본 데이터 설정
        /// </summary>
        private void SetInitComponentInfo()
        {
            if (ComponentMst.RoutingType == null)
            {
                return;
            }

            switch ((ComponentType)Enum.Parse(typeof(ComponentType), ComponentMst.RoutingType))
            {
                case ComponentType.DM601005:
                    {
                        SetBasicResourceSensorComponentData();
                        break;
                    }
                case ComponentType.DM701003:
                    {
                        SetBasicIPCameraComponentData();
                        break;
                    }
            }
        }

        private async void LoadCamList()
        {
            IList<Coever.Lib.CoPlatform.Core.Models.DmIpcamMst> ipcamList = null;

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    ipcamList = CoPlatformWebClient.Instance.GetIPCams();
                }
                catch { }
            });

            _IPCamList.Clear();
            if (ipcamList != null)
            {
                _IPCamList.AddRange(ipcamList);
            }
            IPCamListView.Refresh();

            SetSelectedIPCamera();
        }

        private void SetSelectedIPCamera()
        {
            try
            {
                if (ComponentMst != null &&
                    ComponentMst.PropertyM.TargetCode != null &&
                    uint.TryParse(ComponentMst.PropertyM.TargetCode, out uint camCode))
                {
                    SelectedCamCode = camCode;
                    ComponentMst.PropertyM.ItemName = _IPCamList.FirstOrDefault(p => p.Code.ToString() == ComponentMst.PropertyM.TargetCode).Name;
                }
                else
                {
                    SelectedCamCode = null;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        #endregion

        #region Commands

        #endregion
        #endregion

        #region BroadCast

        private async void OnAlarmCode(string actionName, object sender, object e)
        {
            try
            {
                var routignCode = sender as string;
                var alarmCode = e as string;

                var resultLinq = BaseSingletonManager.Instance.AssetList.FirstOrDefault(p => p.RoutingCode == routignCode);

                resultLinq.PropertyM.AlarmCode = alarmCode;

                await Task.Run(() => WebRequests.PutComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst> { resultLinq }));

                MqttReceiveManager.RegisterMQTTtopic();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        private async void OnAlarmLayout(string actionName, object sender, object e)
        {
            try
            {
                var routignCode = sender as string;
                bool alarmLayout = Convert.ToBoolean(e);

                var resultLinq = BaseSingletonManager.Instance.AssetList.FirstOrDefault(p => p.RoutingCode == routignCode);

                resultLinq.PropertyM.IsAlarmLayout = alarmLayout;

                await Task.Run(() => WebRequests.PutComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst> { resultLinq }));

                MqttReceiveManager.RegisterMQTTtopic();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        #endregion
    }
}
