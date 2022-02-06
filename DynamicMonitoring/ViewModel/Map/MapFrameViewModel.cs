using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using DevExpress.Xpf.Docking;
using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Resources;
using DynamicMonitoring.View;
using Prism.Commands;

namespace DynamicMonitoring.ViewModel
{
    public class MapFrameViewModel : DMViewModelBase
    {
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

        private AlarmNotiHistoryView _AlarmNotiView;
        public AlarmNotiHistoryView AlarmNotiView
        {
            get
            {
                if (_AlarmNotiView == null)
                {
                    _AlarmNotiView = new AlarmNotiHistoryView();
                }
                return _AlarmNotiView;
            }
            set
            {
                _AlarmNotiView = value;
                RaisePropertyChanged("AlarmNotiView");
            }
        }

        private MapListView _MapListView;
        public MapListView MapListView
        {
            get
            {
                if (_MapListView == null)
                {
                    _MapListView = new MapListView();
                }
                return _MapListView;
            }
            set
            {
                _MapListView = value;
                RaisePropertyChanged("MapListView");
            }
        }

        private MapToolBarView _MapToolBarView;
        public MapToolBarView MapToolBarView
        {
            get
            {
                if (_MapToolBarView == null)
                {
                    _MapToolBarView = new MapToolBarView();
                }
                return _MapToolBarView;
            }
            set
            {
                _MapToolBarView = value;
                RaisePropertyChanged("MapToolBarView");
            }
        }

        private ComponentPropertyFrameView _ComponentPropertyFrameV;
        public ComponentPropertyFrameView ComponentPropertyFrameV
        {
            get
            {
                if (_ComponentPropertyFrameV == null)
                {
                    _ComponentPropertyFrameV = new ComponentPropertyFrameView();
                }
                return _ComponentPropertyFrameV;
            }
            set
            {
                _ComponentPropertyFrameV = value;
                RaisePropertyChanged("ComponentPropertyFrameV");
            }
        }

        private Dictionary<string, MapMainView> _OpenMapMainList;
        public Dictionary<string, MapMainView> OpenMapMainList
        {
            get
            {
                if (_OpenMapMainList == null)
                {
                    _OpenMapMainList = new Dictionary<string, MapMainView>();
                }
                return _OpenMapMainList;
            }
            set
            {
                _OpenMapMainList = new Dictionary<string, MapMainView>();
                RaisePropertyChanged("OpenMapMainList");
            }
        }

        /// <summary>
        /// MapToolbar Visibility 처리 속성
        /// </summary>
        private Visibility _MapToolbarVisibility = Visibility.Collapsed;
        public Visibility MapToolbarVisibility
        {
            get { return _MapToolbarVisibility; }
            set
            {
                _MapToolbarVisibility = value;
                RaisePropertyChanged("MapToolbarVisibility");
            }
        }

        private bool _IsMapEdit;
        private bool IsMapEdit
        {
            get { return _IsMapEdit; }
            set
            {
                if (_IsMapEdit != value)
                {
                    _IsMapEdit = value;
                    ChangedMapEditMode(value);
                }
            }
        }

        public MapFrameViewModel()
        {
            LocalizationManager.Instance.CultureChanged += OnCultureChanged;
        }

        public override void Load()
        {
            Workspace.Add(MapListView);
            Workspace.Add(AlarmNotiView);
            Workspace.Add(MapToolBarView);

            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_SELECTED_MAPLIST, OnBroadcastReceived);
            BroadcastReceiver.RegisterReceiver(Constants.BROADCAST_EDIT_MAP, OnMapEditMode);
            BroadcastReceiver.RegisterReceiver(Constants.BRODCAST_ADD_COMPONENT, OnBroadcastReceived);
        }

        public override void Unload()
        {
            if (Workspace != null)
                Workspace = null;

            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_SELECTED_MAPLIST, OnBroadcastReceived);
            BroadcastReceiver.UnregisterReceiver(Constants.BROADCAST_EDIT_MAP, OnMapEditMode);
            BroadcastReceiver.UnregisterReceiver(Constants.BRODCAST_ADD_COMPONENT, OnBroadcastReceived);
        }

        #region Methods

        /// <summary>
        /// MapLayout 추가
        /// </summary>
        private void AddMapLayout(string id, MapMainView mainview)
        {
            if (string.IsNullOrEmpty(id) || mainview == null)
                return;

            OpenMapMainList.Add(id, mainview);

            // 추가된 MapLayout 항목 선택되도록 적용
            (mainview.DataContext as MapMainViewModel).IsActive = true;
        }

        /// <summary>
        /// MapLayout 삭제
        /// </summary>
        private void RemoveMapLayout(string id)
        {
            if (id == null)
                return;

            OpenMapMainList.Remove(id);
        }

        /// <summary>
        /// Workspace 추가 메서드
        /// 현재 추가 되는건 MapMainView 밖에 없음 -> MapList, MapProperty, MapToolbar는 닫기 버튼 삭제 됨
        /// </summary>
        /// <param name="workspace"></param>
        private void AddWorkspace(UserControl workspace)
        {
            if (workspace == null)
                return;

            Workspace.Add(workspace);
        }

        /// <summary>
        /// Workspace 삭제
        /// 현재 삭제 되는건 MapMainView 밖에 없음 -> MapList, MapProperty, MapToolbar는 닫기 버튼 삭제 됨
        /// </summary>
        /// <param name="workspace"></param>
        private void RemoveWorkspace(UserControl workspace)
        {
            if (workspace == null)
                return;

            if (Workspace.Contains(workspace))
                Workspace.Remove(workspace);
        }

        /// <summary>
        /// MapList 선택
        /// </summary>
        /// <param name="pageInfo"></param>
        private void SelectedMapListInfo(MapModel mapmodel)
        {
            if (mapmodel == null || string.IsNullOrEmpty(mapmodel.MapID))
                return;

            if (OpenMapMainList.ContainsKey(mapmodel.MapID))
            {
                (OpenMapMainList[mapmodel.MapID].DataContext as MapMainViewModel).IsActive = true;
            }
            else
            {
                if (mapmodel.MapType == "layout")
                {
                    MapMainView view = new MapMainView();
                    (view.DataContext as MapMainViewModel).PageInfo = mapmodel;

                    AddWorkspace(view);
                    AddMapLayout(mapmodel.MapID, view);
                }
            }
        }

        /// <summary>
        /// MapToolbar Visibility
        /// </summary>
        private void SetVisibilityMapToolbar()
        {
            if (OpenMapMainList.Count > 0)
                MapToolbarVisibility = Visibility.Visible;
            else
                MapToolbarVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Map Edit Mode일 때 MapPropertyView <-> MapComponentView 전환
        /// </summary>
        /// <param name="isMapEdit"></param>
        private void ChangedMapEditMode(bool isMapEdit)
        {
            BroadcastReceiver.SendBroadcast(Constants.MAP1, null, isMapEdit);

            if (isMapEdit)
            {
                Workspace.Remove(AlarmNotiView);
                Workspace.Add(ComponentPropertyFrameV);
            }
            else
            {
                Workspace.Remove(ComponentPropertyFrameV);
                Workspace.Add(AlarmNotiView);
            }
        }

        /// <summary>
        /// MapEdit Mode 체크
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMapEditMode(string actionName, object sender, object e)
        {
            IsMapEdit = bool.Parse(sender.ToString());
        }

        private void OnBroadcastReceived(string actionName, object sender, object data)
        {
            switch (actionName)
            {
                case Constants.BROADCAST_SELECTED_MAPLIST:
                    {
                        MapModel mapInfo = data as MapModel;

                        if (mapInfo != null)
                        {
                            SelectedMapListInfo(mapInfo);
                            SetVisibilityMapToolbar();
                        }
                        break;
                    }
                case Constants.BRODCAST_SELECTED_COMPONENT:
                    {
                        if (Workspace.Contains(ComponentPropertyFrameV))
                        {
                            //(ComponentPropertyFrameV.DataContext as ComponentPropertyFrameViewModel).TargetComponent = data as PropertyBaseViewModel;
                        }
                        break;
                    }
                case Constants.BRODCAST_ADD_COMPONENT:
                    {
                        //(ComponentPropertyFrameV.DataContext as ComponentPropertyFrameViewModel).TargetComponent = data as PropertyBaseViewModel;
                        break;
                    }
                case Constants.BROADCAST_MAP_REMOVED:
                    {
                        MapModel map = data as MapModel;

                        if (map != null && OpenMapMainList.ContainsKey(map.MapID))
                        {
                            MapMainView mapView = OpenMapMainList[map.MapID];
                            (mapView.DataContext as MapMainViewModel).Close();
                        }
                        break;
                    }
            }
        }

        public void OnCultureChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged("Name");
        }

        #endregion

        #region Commands

        public DelegateCommand<DocumentGroup> SelectedMapChangedCommand
        {
            get { return new DelegateCommand<DocumentGroup>(SelectedMapChangedCommandExecute); }
        }

        public DelegateCommand<object> CloseCommand
        {
            get { return new DelegateCommand<object>(CloseCommandExecute); }
        }

        private void SelectedMapChangedCommandExecute(DocumentGroup docGroup)
        {
            if (docGroup.SelectedItem == null)
            {
                IsMapEdit = false;
                return;
            }

            if (docGroup.SelectedItem.DataContext is MapMainViewModel)
            {
                if ((docGroup.SelectedItem.DataContext as MapMainViewModel).IsEditMode)
                {
                    (docGroup.SelectedItem.DataContext as MapMainViewModel).IsEditMode = false;
                    IsMapEdit = false;
                }
                else
                {
                    IsMapEdit = false;
                }
            }
            docGroup.SelectedTabIndex = docGroup.SelectedTabIndex;
        }

        private void CloseCommandExecute(object sender)
        {
            if (sender == null)
                return;

            var layoutID = ((sender as DocumentPanel).DataContext as MapMainViewModel).PageInfo.MapID;
            var workspace = (sender as DocumentPanel).Content as MapMainView;

            RemoveMapLayout(layoutID);
            RemoveWorkspace(workspace);
            SetVisibilityMapToolbar();
        }

        #endregion



        #region 리팩토링 대상

        public string Name
        {
            get { return AppSettings.GeneralSettings.Title; }
        }

        public ImageSource Glyph
        {
            get { return ObjectConverter.ToImageSource(Res.Icon); }
        }

        #endregion
    }
}
