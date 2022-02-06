using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using Coever.Lib.Core.Tasks;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Network;
using DMS.Module.Map.Properties;
using DMS.Module.Map.View.Component;
using DMS.Module.Map.ViewModel.Component;
using Prism.Commands;
using PrismWPF.Common;
using PrismWPF.Common.Broadcast;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DMS.Module.Map.ViewModel
{
    public class PMSLayoutViewModel : DMViewModelBase, ILayoutViewModel
    {
        public readonly AsyncTask<object, IEnumerable<DmComponentMst>> _LoadTask;

        public PMSLayoutViewModel()
        {
            IsLoaded = true;
        }

        public override void Load()
        {
            IsLoaded = true;

            LoadComponentList();
        }

        public override void Unload()
        {
            if (VMList != null)
            {
                VMList = null;
            }

            IsLoaded = false;

            //관리 모듈 이동 시 맵 편집 모드 해제 안되는 문제
            WebRequests.DeleteSystemUseCheck(CoPlatformWebClient.Instance, SystemUseCheckM);
        }

        public bool IsLoaded { get; set; }

        private ObservableCollection<CP_CommonBaseViewModel> _vMList;
        public ObservableCollection<CP_CommonBaseViewModel> VMList
        {
            get
            {
                if (_vMList == null)
                {
                    _vMList = new ObservableCollection<CP_CommonBaseViewModel>();
                }
                return _vMList;
            }
            set
            {
                _vMList = value;
                RaisePropertyChanged("VMList");
            }
        }

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

        private SystemUseCheckModel _systemUseCheckM;
        public SystemUseCheckModel SystemUseCheckM
        {
            get
            {
                if (_systemUseCheckM == null)
                {
                    _systemUseCheckM = new SystemUseCheckModel();
                }
                return _systemUseCheckM;
            }
            set => _systemUseCheckM = value;
        }

        /// <summary>
        /// 뒤로 되돌리기
        /// </summary>
        private ObservableCollection<DmComponentMst> _revertBackList;
        public ObservableCollection<DmComponentMst> RevertBackList
        {
            get
            {
                if (_revertBackList == null)
                {
                    _revertBackList = new ObservableCollection<DmComponentMst>();
                }
                return _revertBackList;
            }
            set => _revertBackList = value;
        }

        /// <summary>
        /// 앞으로 되돌리기
        /// </summary>
        private ObservableCollection<DmComponentMst> _revertForwardList;
        public ObservableCollection<DmComponentMst> RevertForwardList
        {
            get
            {
                if (_revertForwardList == null)
                {
                    _revertForwardList = new ObservableCollection<DmComponentMst>();
                }
                return _revertForwardList;
            }
            set => _revertForwardList = value;
        }

        public DmComponentMst OriginComponentData;

        private bool _IsActive;
        public bool IsActive
        {
            get => _IsActive;
            set
            {
                _IsActive = value;
                RaisePropertyChanged("IsActive");
            }
        }

        public async void LoadComponentList()
        {
            await Task.Run(() =>
            {
                var resultLinq = LinqManager.FilterWhereLayout(DmComponentM.RoutingCode);

                if (resultLinq == null)
                {
                    return;
                }

                CP_CommonBaseViewModel dmComponentMst = null;

                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    foreach (var item in resultLinq)
                    {
                        BaseSingletonManager.Instance.AssetVMList.TryGetValue(item.RoutingCode, out dmComponentMst);

                        if(dmComponentMst != null)
                        {
                            dmComponentMst.Load();
                            VMList.Add(dmComponentMst);
                        }
                    }
                }));
            });
        }

        public async Task ExecuteComponentAction(IActionModel action)
        {
            if (action == null)
            {
                return;
            }

            switch (action.ActionType)
            {
                case ActionTypes.ipcam_compo:
                    {
                        IPCamCompoActionModel parsedAction = action as IPCamCompoActionModel;

                        await Task.Factory.StartNew(() =>
                        {
                            int limitCount = 3000 / 100;
                            for (int count = 0; count < limitCount && !IsLoaded; count++)
                            {
                                Thread.Sleep(100);
                            }
                        });

                        if (IsLoaded)
                        {
                            if (VMList.Where(p =>
                            {
                                if (p is CP_IPCameraViewModel)
                                {
                                    CP_IPCameraViewModel vm = p as CP_IPCameraViewModel;

                                    if (vm?.DmComponentM != null)
                                    {
                                        return parsedAction.ComponentID == vm.DmComponentM.RoutingCode;
                                    }
                                }
                                return false;
                            }).FirstOrDefault() is CP_IPCameraViewModel compoVM)
                            {
                                await compoVM.ExecuteAction(parsedAction);
                            }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// 사용자별 동일 레이아웃 편집 체크
        /// </summary>
        private bool SetSystemUseCheck()
        {
            try
            {
                SystemUseCheckM.Account = string.IsNullOrEmpty(Settings.Default.Account) ? "DMS" : Settings.Default.Account;
                SystemUseCheckM.WindowName = DmComponentM.RoutingCode;

                var result = WebRequests.GetSystemUseCheck(CoPlatformWebClient.Instance, SystemUseCheckM);

                if (result.Count == 0 || result[0] == null || result[0].WindowName != DmComponentM.RoutingCode) //미사용
                {
                    SystemUseCheckM.Account = string.IsNullOrEmpty(Settings.Default.Account) ? "DMS" : Settings.Default.Account;
                    SystemUseCheckM.WindowName = DmComponentM.RoutingCode;
                    SystemUseCheckM.UseYn = "Y";

                    WebRequests.PostSystemUseCheck(CoPlatformWebClient.Instance, SystemUseCheckM);
                    return true;
                }
                else
                {
                    MessageBox.Show(Res.MsgSystemUseCheck, string.Empty, MessageBoxButton.OK);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
                return false;
            }
        }

        #region Component Master(CREATE/UPDATE/DELETE)

        public async void DeleteComponentmst(DmComponentMst dmComponentMst)
        {
            int statusCode = await Task.Run(() => WebRequests.DeleteComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst> { dmComponentMst }));

            //이미지
            if (dmComponentMst.RoutingType == ComponentType.DM701001.ToString())
            {
                DeleteImageDataInfo(dmComponentMst);
            }

            DmComponentMst resultLinq = LinqManager.FilterFirstRoutingCode(dmComponentMst.RoutingCode);

            BaseSingletonManager.Instance.AssetList.Remove(resultLinq);
            BaseSingletonManager.Instance.AssetVMList.Remove(resultLinq.RoutingCode);
        }

        public async void SaveComponentmst(ComponentType cpType, DmComponentMst dmComponentMst)
        {
            dmComponentMst.MapType = MapFrameViewModel.Instance.RoutingConfigType == ScenarioTypes.PMS ? 1 : 2;
            dmComponentMst.Zindex = MapFrameViewModel.Instance.PMSLayoutVM.VMList.Count;
            dmComponentMst.PRoutingCode = MapFrameViewModel.Instance.PMSLayoutVM.DmComponentM.RoutingCode;

            int statusCode = await Task.Run(() => WebRequests.PostComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst> { dmComponentMst }));

            AddComponent(cpType, dmComponentMst);
        }

        public async void UpdateComponentmst(DmComponentMst dmComponentMst)
        {
            int statusCode = await Task.Run(() => WebRequests.PutComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst> { dmComponentMst }));
        }

        #endregion

        public async void DeleteImageDataInfo(DmComponentMst dmComponentMst)
        {
            int statusCode = await Task.Run(() => WebRequests.DeleteFileInfo(CoPlatformWebClient.Instance, new List<DmComponentMst> { dmComponentMst }));
        }

        private void AddComponent(ComponentType componentType, DmComponentMst dmComponentMst)
        {
            InitComponentProperty();


            var vm = BaseSingletonManager.Instance.ConvertDMSMasterDataViewModel(componentType);

            if (vm != null)
            {
                vm.DmComponentM = dmComponentMst;
                vm.Load();
                VMList.Add(vm);
            }

            BaseSingletonManager.Instance.AssetList.Add(dmComponentMst);
            BaseSingletonManager.Instance.AssetVMList.Add(vm.DmComponentM.RoutingCode, vm);
        }

        public void UpdateResourceSensor(DmResourceSensorModel resourceSensor)
        {
            if (resourceSensor == null)
            {
                return;
            }

            WebRequests.PutResourceSensors(CoPlatformWebClient.Instance, new List<DmResourceSensorModel>() { resourceSensor });
        }

        private async void UpdateComponentZIndex()
        {
            List<DmComponentMst> dmComponentMsts = new List<DmComponentMst>();

            foreach (CP_CommonBaseViewModel item in VMList)
            {
                DmComponentMst linqResult = item.DmComponentM;
                dmComponentMsts.Add(linqResult);
            }

            int statusCode = await Task.Run(() => WebRequests.PutComponentMst(CoPlatformWebClient.Instance, dmComponentMsts));
        }

        public void RevertBackComponent()
        {
            if (RevertBackList.Count == 0)
            {
                return;
            }

            if (OriginComponentData.Action != ActionType.N)
            {
                RevertForwardList.Add(OriginComponentData.CopyObject());
                OriginComponentData.Action = ActionType.N;
            }

            var resultLinq = RevertBackList.Last();

            switch (resultLinq.Action)
            {
                case ActionType.C:
                    {
                        CP_CommonBaseViewModel itemData = null;

                        if (resultLinq.RoutingType == ComponentType.DM701001.ToString())
                        {
                            itemData = VMList.FirstOrDefault(p => p.DmComponentM.PropertyM.ItemCode == resultLinq.PropertyM.ItemCode);
                        }
                        else
                        {
                            itemData = VMList.FirstOrDefault(p => p.DmComponentM.RoutingCode == resultLinq.RoutingCode);
                        }

                        int itemIndex = VMList.IndexOf(itemData);

                        if (itemIndex == -1)
                        {
                            LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "Revert ItemIndex Error");
                            return;
                        }

                        VMList.RemoveAt(itemIndex);

                        DeleteComponentmst(resultLinq);

                        resultLinq.Action = ActionType.D;

                        //속성창 삭제
                        //(MapFrameViewModel.Instance.LayoutPropertyV.DataContext as LayoutPropertyViewModel).InitPropertyView();

                        break;
                    }
                case ActionType.D:
                    {
                        SaveComponentmst((ComponentType)Enum.Parse(typeof(ComponentType), resultLinq.RoutingType), resultLinq);

                        resultLinq.Action = ActionType.C;

                        //속성창 추가
                        MapFrameViewModel.Instance.SetCPView((ComponentType)Enum.Parse(typeof(ComponentType), resultLinq.RoutingType), resultLinq);

                        break;
                    }
                case ActionType.U:
                    {
                        UpdateComponentmst(resultLinq);

                        if (resultLinq.RoutingType == ComponentType.DM801001.ToString() ||
                            resultLinq.RoutingType == ComponentType.DM801002.ToString())
                        {
                            (MapFrameViewModel.Instance.PMSLayoutVM as PMSLayoutViewModel).UpdateResourceSensor(resultLinq.ResourceM);
                        }

                        DmComponentMst linq = LinqManager.FilterFirstRoutingCode(resultLinq.RoutingCode);

                        if (linq == null)
                        {
                            return;
                        }

                        linq.Action = ActionType.U;
                        resultLinq.PropertyM.ImageSource = linq.PropertyM.ImageSource;

                        linq.PropertyM = resultLinq.PropertyM;
                        linq.ResourceM = resultLinq.ResourceM;

                        linq.CoordinateX = resultLinq.CoordinateX;
                        linq.CoordinateY = resultLinq.CoordinateY;
                        linq.Height = resultLinq.Height;
                        linq.Width = resultLinq.Width;

                        // 업데이트 할 때 다른 항목이면 속성창 활성화(임시)
                        //if (!(MapFrameViewModel.Instance.ComponentPropertyV.DataContext is ComponentPropertyViewModel))
                        //{
                        //    MapFrameViewModel.Instance.ComponentPropertyVM.SetCPView((ComponentType)Enum.Parse(typeof(ComponentType), resultLinq.RoutingType), resultLinq);
                        //}

                        break;
                    }
            }

            RevertBackList.Remove(RevertBackList.Last());
            RevertForwardList.Add(resultLinq.CopyObject());
        }

        public void RevertForwardComponent()
        {
            try
            {
                if (RevertForwardList.Count == 0)
                {
                    return;
                }

                var resultLinq = RevertForwardList.Last();

                switch (resultLinq.Action)
                {
                    case ActionType.C:
                        {
                            CP_CommonBaseViewModel itemData = null;

                            if (resultLinq.RoutingType == ComponentType.DM701001.ToString())
                            {
                                itemData = VMList.FirstOrDefault(p => p.DmComponentM.PropertyM.ItemCode == resultLinq.PropertyM.ItemCode);
                            }
                            else
                            {
                                itemData = VMList.FirstOrDefault(p => p.DmComponentM.RoutingCode == resultLinq.RoutingCode);
                            }

                            int itemIndex = VMList.IndexOf(itemData);

                            if (itemIndex == -1)
                            {
                                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "Revert ItemIndex Error");
                                return;
                            }

                            VMList.RemoveAt(itemIndex);

                            DeleteComponentmst(resultLinq);

                            resultLinq.Action = ActionType.D;

                            //속성창 삭제
                            //(MapFrameViewModel.Instance.LayoutPropertyV.DataContext as LayoutPropertyViewModel).InitPropertyView();
                            break;
                        }
                    case ActionType.D:
                        {
                            SaveComponentmst((ComponentType)Enum.Parse(typeof(ComponentType), resultLinq.RoutingType), resultLinq);

                            resultLinq.Action = ActionType.C;

                            //속성창 추가
                            MapFrameViewModel.Instance.SetCPView((ComponentType)Enum.Parse(typeof(ComponentType), resultLinq.RoutingType), resultLinq);

                            break;
                        }
                    case ActionType.U:
                        {
                            UpdateComponentmst(resultLinq);

                            if (resultLinq.RoutingType == ComponentType.DM801001.ToString() ||
                                resultLinq.RoutingType == ComponentType.DM801002.ToString())
                            {
                                (MapFrameViewModel.Instance.PMSLayoutVM as PMSLayoutViewModel).UpdateResourceSensor(resultLinq.ResourceM);
                            }

                            DmComponentMst linq = LinqManager.FilterFirstRoutingCode(resultLinq.RoutingCode);

                            if (linq == null)
                            {
                                return;
                            }

                            linq.Action = ActionType.U;
                            resultLinq.PropertyM.ImageSource = linq.PropertyM.ImageSource;

                            linq.PropertyM = resultLinq.PropertyM;
                            linq.ResourceM = resultLinq.ResourceM;

                            linq.CoordinateX = resultLinq.CoordinateX;
                            linq.CoordinateY = resultLinq.CoordinateY;
                            linq.Height = resultLinq.Height;
                            linq.Width = resultLinq.Width;

                            // 업데이트 할 때 다른 항목이면 속성창 활성화(임시)
                            //if (!(MapFrameViewModel.Instance.ComponentPropertyV.DataContext is ComponentPropertyViewModel))
                            //{
                            //    MapFrameViewModel.Instance.ComponentPropertyVM.SetCPView((ComponentType)Enum.Parse(typeof(ComponentType), resultLinq.RoutingType), resultLinq);
                            //}

                            OriginComponentData.Action = ActionType.U;
                            break;
                        }
                }

                RevertForwardList.Remove(RevertForwardList.Last());
                RevertBackList.Add(resultLinq.CopyObject());
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, e.ToString());
            }
        }

        public void ChangeTopicCode(string routingCode)
        {
            var linqResult = VMList.FirstOrDefault(p => p.DmComponentM.RoutingCode == routingCode);

            if (linqResult == null)
            {
                return;
            }

            linqResult.SetMqttSubscribe();
        }

        private void InitComponentProperty()
        {
            MapFrameViewModel.Instance.ComponentMst = null;
            MapFrameViewModel.Instance.SelectedBaseInfoType = null;
            MapFrameViewModel.Instance.BaseInfoList = null;
            MapFrameViewModel.Instance.SelectedBaseInfo = null;

            BaseSingletonManager.Instance.SetBaseInfoType();
        }

        #region Commands

        public DelegateCommand<MenuItem> EditMenuClickedCommand => new DelegateCommand<MenuItem>(EditMenuClickedCommandExecute);
        public DelegateCommand MapModificationCommand => new DelegateCommand(MapModificationCommandExecute);
        public DelegateCommand MapObservationCommand => new DelegateCommand(MapObservationCommandExecute);
        public DelegateCommand MouseLeftButtonUpCommand => new DelegateCommand(MouseLeftButtonUpCommandExecute);
        public DelegateCommand ZoomValueChagedCommand => new DelegateCommand(ZoomValueChagedCommandExecute);

        private void EditMenuClickedCommandExecute(MenuItem menu)
        {
            try
            {
                if (menu == null)
                {
                    return;
                }

                CP_CommonBaseViewModel itemData = VMList.FirstOrDefault(p => p == menu.DataContext);
                int itemIndex = VMList.IndexOf(itemData);
                //UserControl itemData = ComponentList.FirstOrDefault(p => p.DataContext == menu.DataContext);
                //int itemIndex = ComponentList.IndexOf(itemData);

                switch (menu.Tag)
                {
                    case Arrange.Remove:
                        {
                            DmComponentMst dmComponentMst = itemData.DmComponentM;

                            if (dmComponentMst.RoutingType == ComponentType.DM601001.ToString() ||
                                dmComponentMst.RoutingType == ComponentType.DM601002.ToString() ||
                                dmComponentMst.RoutingType == ComponentType.DM601003.ToString() ||
                                dmComponentMst.RoutingType == ComponentType.DM601004.ToString() ||
                                dmComponentMst.RoutingType == ComponentType.DM601005.ToString())
                            {
                                MessageBox.Show(Res.MsgNotRemoveRoutingConfig);
                                return;
                            }

                            DeleteComponentmst(dmComponentMst);
                            //ComponentList.RemoveAt(itemIndex);
                            VMList.RemoveAt(itemIndex);

                            //Revert 항목 추가
                            dmComponentMst.Action = ActionType.D;
                            RevertBackList.Add(dmComponentMst.CopyObject());
                            OriginComponentData = dmComponentMst;

                            //(MapFrameViewModel.Instance.LayoutPropertyV.DataContext as LayoutPropertyViewModel).InitPropertyView();
                            break;
                        }
                    case Arrange.BringToFront:
                        {
                            if (itemIndex < VMList.Count - 1)
                            {
                                VMList.Move(itemIndex, VMList.Count - 1);
                            }
                            break;
                        }
                    case Arrange.BringForward:
                        {
                            if (itemIndex < VMList.Count - 1)
                            {
                                VMList.Move(itemIndex, itemIndex + 1);
                            }
                            break;
                        }
                    case Arrange.SendToBack:
                        {
                            if (itemIndex > 0)
                            {
                                VMList.Move(itemIndex, 0);
                            }
                            break;
                        }
                    case Arrange.SendBackward:
                        {
                            if (itemIndex > 0)
                            {
                                VMList.Move(itemIndex, itemIndex - 1);
                            }
                            break;
                        }
                }

                foreach (var item in VMList)
                {
                    item.DmComponentM.Zindex = VMList.IndexOf(item);
                }

                UpdateComponentZIndex();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        private void MapModificationCommandExecute()
        {
            bool resultFlag = SetSystemUseCheck();

            if (resultFlag == false)
            {
                return;
            }

            MapFrameViewModel.Instance.IsMapEdit = true;
            BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_MAPEDIT, MapFrameViewModel.Instance.IsMapEdit, null);

            OriginComponentData = new DmComponentMst();
        }

        private void MapObservationCommandExecute()
        {
            WebRequests.DeleteSystemUseCheck(CoPlatformWebClient.Instance, SystemUseCheckM);

            MapFrameViewModel.Instance.IsMapEdit = false;

            if (MapFrameViewModel.Instance.RoutingConfigType == ScenarioTypes.USER)
            {
                (MapFrameViewModel.Instance.AssetListV.DataContext as Asset_ListViewModel).SetLayoutList();
            }

            RevertBackList = null;
            RevertForwardList = null;

            SetComponentVisibility();

            BroadcastReceiver.SendBroadcast(ConstantManager.BROADCAST_MAPEDIT, MapFrameViewModel.Instance.IsMapEdit, null);
        }

        private void ZoomValueChagedCommandExecute()
        {
            UpdateComponentmst(DmComponentM);
        }

        /// <summary>
        /// 설명: 빈 레이아웃 클릭 이벤트
        /// 목적: 컴포넌트 선택 포커스 해제
        /// </summary>
        private void MouseLeftButtonUpCommandExecute()
        {
            try
            {
                if (MapFrameViewModel.Instance.IsMapEdit == false)
                {
                    return;
                }

                SetComponentVisibility();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, "[Layout Property]: " + ex.ToString());
            }
        }

        private void SetComponentVisibility()
        {
            foreach (var item in VMList)
            {
                item.DmComponentM.IsSelectMode = Visibility.Collapsed;
            }

            //var resultLinq = ComponentList.Where(p => (p.DataContext as CP_CommonBaseViewModel).DmComponentM.IsSelectMode == Visibility.Visible);

            //if (resultLinq == null)
            //{
            //    return;
            //}

            //foreach (var item in resultLinq)
            //{
            //    (item.DataContext as CP_CommonBaseViewModel).DmComponentM.IsSelectMode = Visibility.Collapsed;
            //}

            MapFrameViewModel.Instance.ComponentMst = null;
        }

        #endregion Commands
    }
}
