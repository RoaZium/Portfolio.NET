using DevExpress.Mvvm.POCO;
using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Model.Component;
using DynamicMonitoring.Resources;
using DynamicMonitoring.Utils;
using DynamicMonitoring.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DynamicMonitoring.ViewModel
{
    public class MapMainViewModel : DMViewModelBase
    {
        private ObservableCollection<UserControl> _ComponentItemList;
        public ObservableCollection<UserControl> ComponentItemList
        {
            get
            {
                if (_ComponentItemList == null)
                {
                    _ComponentItemList = new ObservableCollection<UserControl>();
                }
                return _ComponentItemList;
            }
            set
            {
                _ComponentItemList = value;
                RaisePropertyChanged("ComponentItemList");
            }
        }

        private ObservableCollection<UserControl> _ComponentItemBackupList;
        public ObservableCollection<UserControl> ComponentItemBackupList
        {
            get
            {
                if (_ComponentItemBackupList == null)
                {
                    _ComponentItemBackupList = new ObservableCollection<UserControl>();
                }
                return _ComponentItemBackupList;
            }
            set{ _ComponentItemBackupList = value; }
        }

        private Dictionary<UserControl, SaveComponentInfo> _SaveComponentItemList;
        public Dictionary<UserControl, SaveComponentInfo> SaveComponentItemList
        {
            get
            {
                if (_SaveComponentItemList == null)
                {
                    _SaveComponentItemList = new Dictionary<UserControl, SaveComponentInfo>();
                }
                return _SaveComponentItemList;
            }
        }

        public MapMainViewModel()
        {
        }

        public override void Load()
        {
            // MapLayout Save
            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_SAVE_MAPLAYOUT, OnBroadcastSaveMapLayout);
            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_ADD_MAPCOMPONENT, OnBroadcastToolbarReceived);
            BroadcastReceiver.RegisterReceiver(Constants.MAP2, OnMapLayoutEditMode);
        }

        public override void Unload()
        {
            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_SAVE_MAPLAYOUT, OnBroadcastSaveMapLayout);
            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_ADD_MAPCOMPONENT, OnBroadcastToolbarReceived);
            BroadcastReceiver.UnregisterReceiver(Constants.MAP2, OnMapLayoutEditMode);
        }

        /// <summary>
        /// 데이터를 로드중인지 여부
        /// </summary>
        private bool _IsLoadingTaskRunning = false;
        private bool _IsSaved = false;

        /// <summary>
        /// MapLayout 저장
        /// 리팩토링 대상: 매개변수가 필요없는 항목 -> 매개변수 동적 할당 로직 필요
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBroadcastSaveMapLayout(string actionName, object sender, object e)
        {
            SaveTimer.Change(1000, Timeout.Infinite);
        }

        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                _IsActive = value;
                RaisePropertyChanged("IsActive");
            }
        }

        private bool _IsEditMode;
        public bool IsEditMode
        {
            get { return _IsEditMode; }
            set
            {
                bool old = _IsEditMode;

                _IsEditMode = value;
                RaisePropertyChanged("IsEditMode");

                if (old != value)
                {
                    OnEditModeChanged(value);
                }
            }
        }

        private MapModel _pageInfo;
        public MapModel PageInfo
        {
            get { return _pageInfo; }
            set
            {
                _pageInfo = value;
                RaisePropertyChanged("PageInfo");

                LoadLayout();
            }
        }

        private Timer _saveTimer;
        public Timer SaveTimer
        {
            get
            {
                if (_saveTimer == null)
                {
                    _saveTimer = new Timer(new TimerCallback((obj) =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            SaveLayout();
                        });
                    }));
                }
                return _saveTimer;
            }
        }

        private async void LoadLayout()
        {
            if (_IsLoadingTaskRunning)
                return;

            _IsLoadingTaskRunning = true;

            try
            {
                ComponentItemList.Clear();

                DataTable dt = await DBHelper.GetMapComponentList(PageInfo.MapID);

                if (dt != null)
                {
                    List<SaveComponentInfo> loadedComponenetList = dt.ToObjectList<SaveComponentInfo>();

                    foreach (SaveComponentInfo info in loadedComponenetList)
                    {
                        try
                        {
                            UserControl uc = null;

                            if (info.ComponentType == "Alarm")
                            {
                                uc = new AlarmComponentView();
                                (uc.DataContext as AlarmComponentViewModel).Model = info.CopyObject<AlarmComponentModel>();
                            }
                            else if (info.ComponentType == "Image")
                            {
                                uc = new ImageComponentView();
                                (uc.DataContext as ImageComponentViewModel).Model = info.CopyObject<ImageComponentModel>();
                            }
                            else if (info.ComponentType == "KPI")
                            {
                                uc = new KPIComponentView();
                                (uc.DataContext as KPIComponentViewModel).Model = info.CopyObject<KPIComponentModel>();
                            }
                            else if (info.ComponentType == "Graph")
                            {
                                uc = new LineChartComponentView();
                                (uc.DataContext as LineChartComponentViewModel).Model = info.CopyObject<LineChartComponentModel>();
                            }
                            else if (info.ComponentType == "Panel")
                            {
                                uc = new DisplayPanelComponentView();
                                (uc.DataContext as DisplayPanelComponentViewModel).Model = info.CopyObject<DisplayPanelComponentModel>();
                            }

                            if (uc != null)
                            {
                                ComponentItemList.Add(uc);
                                ComponentItemBackupList.Add(uc);
                            }
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            _IsLoadingTaskRunning = false;
        }

        private void OnEditModeChanged(bool eidtMode)
        {
            if (eidtMode)
            {
                foreach (UserControl item in ComponentItemList)
                {
                    Type originType = item.DataContext.GetType();
                    PropertyInfo modelProperty = originType.GetProperty("Model");

                    SaveComponentInfo saveInfo = new SaveComponentInfo();
                    saveInfo.CopyPropertiesFrom(modelProperty.GetValue(item.DataContext));

                    SaveComponentItemList.Add(item, saveInfo);
                }
            }
            else
            {
                if (SaveComponentItemList != null)
                    SaveComponentItemList.Clear();

                if (_IsSaved)
                {
                    _IsSaved = false;
                }
                else
                {
                    ComponentItemList.Clear();
                    LoadLayout();
                }

            }
        }

        public void OnEditMenuClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem menu = sender as MenuItem;

                DMViewModelBase item = menu.DataContext as DMViewModelBase;

                var itemData = ComponentItemList.FirstOrDefault(p => p.DataContext == item);

                string tag = menu.Tag.ToString();

                switch (tag)
                {
                    case "MD":
                        {
                            if (item is AlarmComponentViewModel)
                            {
                                var result = GetAlarmSelectionAssistantList();
                                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MAP_ALARMANDDNP, result[0], result[1]);

                                string action = SaveComponentItemList[itemData].Action;
                                var vmModel = (itemData.DataContext as AlarmComponentViewModel).Model;

                                if (action == "N")
                                {
                                    SaveComponentItemList[itemData].Action = "U";
                                }

                                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MODIFY_MAPCOMPONENTPROPERTY, vmModel, ComponentType.ALM);
                            }
                            else if (item is ImageComponentViewModel)
                            {
                                string action = SaveComponentItemList[itemData].Action;
                                var vmModel = (itemData.DataContext as ImageComponentViewModel).Model;

                                if (action == "N")
                                {
                                    SaveComponentItemList[itemData].Action = "U";
                                }

                                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MODIFY_MAPCOMPONENTPROPERTY, vmModel, ComponentType.IMG);
                            }
                            else if (item is LineChartComponentViewModel)
                            {
                                string action = SaveComponentItemList[itemData].Action;
                                var vmModel = (itemData.DataContext as LineChartComponentViewModel).Model;

                                if (action == "N")
                                {
                                    SaveComponentItemList[itemData].Action = "U";
                                }

                                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MODIFY_MAPCOMPONENTPROPERTY, vmModel, ComponentType.LNC);
                            }
                            else if (item is DisplayPanelComponentViewModel)
                            {
                                var result = GetPanelSelectionAssistantList();
                                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MAP_ALARMANDDNP, result[0], result[1]);

                                string action = SaveComponentItemList[itemData].Action;
                                var vmModel = (itemData.DataContext as DisplayPanelComponentViewModel).Model;

                                if (action == "N")
                                {
                                    SaveComponentItemList[itemData].Action = "U";
                                }

                                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MODIFY_MAPCOMPONENTPROPERTY, vmModel, ComponentType.DPN);
                            }
                            else if (item is KPIComponentViewModel)
                            {
                                string action = SaveComponentItemList[itemData].Action;
                                var vmModel = (itemData.DataContext as KPIComponentViewModel).Model;

                                if (action == "N")
                                {
                                    SaveComponentItemList[itemData].Action = "U";
                                }

                                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MODIFY_MAPCOMPONENTPROPERTY, vmModel, ComponentType.KPI);
                            }
                            break;
                        }
                    case "RM":
                        {
                            if (item != null)
                            {
                                ComponentItemList.Remove(itemData);

                                if (SaveComponentItemList[itemData].Action == "C")
                                {
                                    SaveComponentItemList.Remove(itemData);
                                }
                                else
                                {
                                    object model = itemData.DataContext.GetType().GetProperty("Model").GetValue(item);
                                    SaveComponentItemList[itemData].CopyPropertiesFrom(model);
                                    SaveComponentItemList[itemData].Action = "D";
                                }
                            }
                            break;
                        }
                    case "FT":
                        {
                            int itemIndex = ComponentItemList.IndexOf(itemData);
                            if (itemIndex < ComponentItemList.Count - 1)
                            {
                                ComponentItemList.Move(itemIndex, ComponentItemList.Count - 1);
                            }
                            break;
                        }
                    case "FW":
                        {
                            int itemIndex = ComponentItemList.IndexOf(itemData);
                            if (itemIndex < ComponentItemList.Count - 1)
                            {
                                ComponentItemList.Move(itemIndex, itemIndex + 1);
                            }
                            break;
                        }
                    case "BK":
                        {
                            int itemIndex = ComponentItemList.IndexOf(itemData);
                            if (itemIndex > 0)
                            {
                                ComponentItemList.Move(itemIndex, 0);
                            }
                            break;
                        }
                    case "BW":
                        {
                            int itemIndex = ComponentItemList.IndexOf(itemData);
                            if (itemIndex > 0)
                            {
                                ComponentItemList.Move(itemIndex, itemIndex - 1);
                            }
                            break;
                        }
                }
            }
            catch(Exception ex)
            { }
        }

        private void OnMapLayoutEditMode(string actionName, object sender, object e)
        {
            IsEditMode = bool.Parse(sender.ToString());
        }

        private void OnBroadcastToolbarReceived(string actionName, object sender, object e)
        {
            var type = e as Nullable<ComponentType>;

            // ItemListBackup 및 SaveComponentList 정보를 Alarm 및 Display에 전달하기 위한 용도
            if (sender == null && type == ComponentType.ALM)
            {
                var result = GetAlarmSelectionAssistantList();
                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MAP_ALARMANDDNP, result[0], result[1]);
                return;
            }

            // ItemListBackup 및 SaveComponentList 정보를 Alarm 및 Display에 전달하기 위한 용도
            if (sender == null && type == ComponentType.DPN)
            {
                var result = GetPanelSelectionAssistantList();
                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MAP_ALARMANDDNP, result[0], result[1]);
                return;
            }

            AddMapComponent(sender, type);
        }

        private List<Dictionary<string, List<string>>> GetAlarmSelectionAssistantList()
        {
            List<Dictionary<string, List<string>>> result = new List<Dictionary<string, List<string>>>();

            Dictionary<string, List<string>> inclusionList = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> exclusionList = new Dictionary<string, List<string>>();

            foreach (var originalItem in ComponentItemBackupList)
            {
                PropertyInfo modelProperty = originalItem.DataContext.GetType().GetProperty("Model");
                object model = modelProperty.GetValue(originalItem.DataContext);

                SaveComponentInfo originalModelInfo = model.CopyObject<SaveComponentInfo>();

                if (originalModelInfo.ComponentType == "Alarm")
                {
                    if (!inclusionList.ContainsKey(originalModelInfo.TargetType))
                    {
                        inclusionList.Add(originalModelInfo.TargetType, new List<string>());
                    }
                    inclusionList[originalModelInfo.TargetType].Add(originalModelInfo.TargetCode);
                }
            }

            foreach (SaveComponentInfo info in SaveComponentItemList.Values)
            {
                if (info.ComponentType == "Alarm")
                {
                    if (info.Action == "D")
                    {
                        if (!inclusionList.ContainsKey(info.TargetType))
                        {
                            inclusionList.Add(info.TargetType, new List<string>());
                        }
                        inclusionList[info.TargetType].Add(info.TargetCode);
                    }
                    else
                    {
                        if (inclusionList.ContainsKey(info.TargetType))
                        {
                            inclusionList[info.TargetType].RemoveAll(item => item == info.TargetCode);
                        }

                        if (!exclusionList.ContainsKey(info.TargetType))
                        {
                            exclusionList.Add(info.TargetType, new List<string>());
                        }
                        exclusionList[info.TargetType].Add(info.TargetCode);
                    }
                }
            }

            result.Add(inclusionList);
            result.Add(exclusionList);

            return result;
        }

        private List<List<string>> GetPanelSelectionAssistantList()
        {
            List<List<string>> result = new List<List<string>>();

            List<string> inclusionList = new List<string>();
            List<string> exclusionList = new List<string>();

            foreach (var originalItem in ComponentItemBackupList)
            {
                PropertyInfo modelProperty = originalItem.DataContext.GetType().GetProperty("Model");
                object model = modelProperty.GetValue(originalItem.DataContext);

                SaveComponentInfo originalModelInfo = model.CopyObject<SaveComponentInfo>();

                if (originalModelInfo.ComponentType == "Panel")
                {
                    inclusionList.Add(originalModelInfo.TargetCode);
                }
            }

            foreach (SaveComponentInfo info in SaveComponentItemList.Values)
            {
                if (info.ComponentType == "Panel")
                {
                    if (info.Action == "D")
                    {
                        inclusionList.Add(info.TargetCode);
                    }
                    else
                    {
                        inclusionList.RemoveAll(item => item == info.TargetCode);
                        exclusionList.Add(info.TargetCode);
                    }
                }
            }
            
            result.Add(inclusionList);
            result.Add(exclusionList);

            return result;
        }

        private async void SaveLayout()
        {
            try
            {
                DataTable components = DBHelper.GetComponentsUpdateFormat();

                foreach (KeyValuePair<UserControl, SaveComponentInfo> saveInfo in SaveComponentItemList)
                {
                    PropertyInfo modelProperty = saveInfo.Key.DataContext.GetType().GetProperty("Model");

                    if (saveInfo.Value.Action == "N")
                    {
                        saveInfo.Value.Action = "U";
                    }

                    if (saveInfo.Value.ComponentType == "Image")
                    {
                        ImageComponentView imageV = saveInfo.Key as ImageComponentView;

                        if (!string.IsNullOrEmpty((imageV.DataContext as ImageComponentViewModel).Model.ImagePath))
                        {
                            DataRow dr = await DBHelper.AddImage(FileUtils.GetBytesFromFile((imageV.DataContext as ImageComponentViewModel).Model.ImagePath));
                            ImageModel image = dr.ToObject<ImageModel>();

                            (imageV.DataContext as ImageComponentViewModel).Model.ImagePath = null;
                            (imageV.DataContext as ImageComponentViewModel).Model.ImageID = image.ImageID;
                        }
                    }

                    saveInfo.Value.CopyPropertiesFrom(modelProperty.GetValue(saveInfo.Key.DataContext));
                    saveInfo.Value.Z = ComponentItemList.IndexOf(saveInfo.Key);

                    components.AddDataRowFromObject(saveInfo.Value);
                }

                DataTable dt = await DBHelper.SaveMapComponentList(PageInfo.MapID, components);

                _IsSaved = true;

                LoadLayout();

                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_EDIT_MAP, false, null);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    BroadcastReceiver.SendBroadcast(Constants.MAP5, null, false);
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(Res.MsgFailedSaveMapLayout + " : " + e.Message);
            }
        }

        /// <summary>
        /// Component 추가
        /// </summary>
        /// <param name="obj"></param>
        private void AddMapComponent(object componentModel, ComponentType? obj)
        {
            try
            {
                UserControl uc = null;

                switch (obj)
                {
                    case ComponentType.ALM:
                        {
                            uc = new AlarmComponentView();
                            AlarmComponentModel alarmCPModel = componentModel as AlarmComponentModel;
                            (uc.DataContext as AlarmComponentViewModel).Model = alarmCPModel;

                            ComponentItemList.Add(uc);

                            SaveComponentInfo saveInfo = alarmCPModel.CopyObject<SaveComponentInfo>();
                            saveInfo.Action = "C";
                            SaveComponentItemList.Add(uc, saveInfo);

                            BroadcastReceiver.SendBroadcast(Constants.BRODCAST_ADD_COMPONENT, null, alarmCPModel);
                            break;
                        }
                    case ComponentType.IMG:
                        {
                            uc = new ImageComponentView();
                            ImageComponentModel imgCPModel = componentModel as ImageComponentModel;
                            (uc.DataContext as ImageComponentViewModel).Model = imgCPModel;

                            ComponentItemList.Add(uc);

                            SaveComponentInfo saveInfo = imgCPModel.CopyObject<SaveComponentInfo>();
                            saveInfo.Action = "C";
                            SaveComponentItemList.Add(uc, saveInfo);

                            BroadcastReceiver.SendBroadcast(Constants.BRODCAST_ADD_COMPONENT, null, imgCPModel);
                            break;
                        }
                    case ComponentType.KPI:
                        {
                            uc = new KPIComponentView();
                            KPIComponentModel kpiCPModel = componentModel as KPIComponentModel;
                            (uc.DataContext as KPIComponentViewModel).Model = kpiCPModel;

                            ComponentItemList.Add(uc);

                            SaveComponentInfo saveInfo = kpiCPModel.CopyObject<SaveComponentInfo>();
                            saveInfo.Action = "C";
                            SaveComponentItemList.Add(uc, saveInfo);

                            BroadcastReceiver.SendBroadcast(Constants.BRODCAST_ADD_COMPONENT, null, kpiCPModel);
                            break;
                        }
                    case ComponentType.LNC:
                        {
                            uc = new LineChartComponentView();
                            LineChartComponentModel lineChartCPModel = componentModel as LineChartComponentModel;
                            (uc.DataContext as LineChartComponentViewModel).Model = lineChartCPModel;

                            ComponentItemList.Add(uc);

                            SaveComponentInfo saveInfo = lineChartCPModel.CopyObject<SaveComponentInfo>();
                            saveInfo.Action = "C";
                            SaveComponentItemList.Add(uc, saveInfo);

                            BroadcastReceiver.SendBroadcast(Constants.BRODCAST_ADD_COMPONENT, null, lineChartCPModel);
                            break;
                        }
                    case ComponentType.DPN:
                        {
                            uc = new DisplayPanelComponentView();
                            DisplayPanelComponentModel disCPModel = componentModel as DisplayPanelComponentModel;
                            (uc.DataContext as DisplayPanelComponentViewModel).Model = disCPModel;

                            ComponentItemList.Add(uc);

                            SaveComponentInfo saveInfo = disCPModel.CopyObject<SaveComponentInfo>();
                            saveInfo.Action = "C";
                            SaveComponentItemList.Add(uc, saveInfo);

                            BroadcastReceiver.SendBroadcast(Constants.BRODCAST_ADD_COMPONENT, null, disCPModel);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        #region 리팩토링 항목

        public ImageSource Glyph { get; set; }

        public string Name
        {
            get { return string.IsNullOrEmpty(PageInfo.MapName) ? "Unnamed" : PageInfo.MapName; }
            set { }
        }

        public override string DisplayName { get { return "M"; } }

        private void OnNameChanged(IPageInfo sender)
        {
            RaisePropertyChanged("Name");
        }

        #endregion
    }
}
