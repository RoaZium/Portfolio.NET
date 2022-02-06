using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using DevExpress.Data.TreeList;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.Network;
using DMS.Module.Map.Properties;
using DMS.Module.Map.View;
using Prism.Commands;
using PrismWPF.Common;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DMS.Module.Map.ViewModel
{
    public class Asset_ListViewModel : DMViewModelBase
    {
        public Asset_ListViewModel()
        { }

        public override void Load()
        {
            SetLayoutList();
        }

        public override void Unload()
        {
        }

        private ObservableCollection<DmComponentMst> _LayoutList;
        public ObservableCollection<DmComponentMst> LayoutList
        {
            get
            {
                if (_LayoutList == null)
                {
                    _LayoutList = new ObservableCollection<DmComponentMst>();
                    _LayoutList.CollectionChanged += OnMapListChanged;
                }
                return _LayoutList;
            }
            set
            {
                _LayoutList = value;
                RaisePropertyChanged("LayoutList");
            }
        }

        private DmComponentMst _SelectedItemLayoutList;
        public DmComponentMst SelectedItemLayoutList
        {
            get
            {
                if (_SelectedItemLayoutList == null)
                {
                    _SelectedItemLayoutList = new DmComponentMst();
                }
                return _SelectedItemLayoutList;
            }
            set
            {
                _SelectedItemLayoutList = value;
                RaisePropertyChanged("SelectedItemLayoutList");
            }
        }

        private DmComponentMst _dmComponentMstM;
        public DmComponentMst DmComponentMstM
        {
            get
            {
                if (_dmComponentMstM == null)
                {
                    _dmComponentMstM = new DmComponentMst();
                }
                return _dmComponentMstM;
            }
            set => _dmComponentMstM = value;
        }

        private DmComponentMst _Dragitem = null;

        private UserControl _scenarioListV;
        public UserControl ScenarioListV
        {
            get
            {
                if (_scenarioListV == null)
                {
                    _scenarioListV = new ScenarioView();
                }
                return _scenarioListV;
            }
            set
            {
                _scenarioListV = value;
                RaisePropertyChanged("ScenarioListV");
            }
        }

        private bool _IsScenarioMode = false;
        public bool IsScenarioMode
        {
            get => _IsScenarioMode;
            set
            {
                _IsScenarioMode = value;

                if (_IsScenarioMode == true)
                {
                    ScenarioListV = new ScenarioView();
                }

                RaisePropertyChanged("IsScenarioMode");
            }
        }

        public void SetLayoutList()
        {
            LayoutList = LinqManager.FilterWhereRoutingConfiguration(1).ToObservableCollection();
            SetLastLayout();
        }

        public async void CreateUserRoutingConfiguration(DmComponentMst dmComponentMst)
        {
            if (dmComponentMst == null)
            {
                return;
            }

            int statusCode = await Task.Run(() =>
            WebRequests.PostComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst>()
            {
                dmComponentMst
            }));

            BaseSingletonManager.Instance.AssetList.Add(dmComponentMst.CopyObject<DmComponentMst>());
            (MapFrameViewModel.Instance.AssetListV.DataContext as Asset_ListViewModel).SetLayoutList();
        }

        public async void UpdateRoutingConfiguration(DmComponentMst dmComponentMst)
        {
            if (dmComponentMst == null)
            {
                return;
            }

            int statusCode = await Task.Run(() => WebRequests.PutComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst>() { dmComponentMst }));
        }

        public async void DeleteUserRoutingConfiguration(DmComponentMst dmComponentMst)
        {
            if (dmComponentMst == null)
            {
                return;
            }

            int statusCode = await Task.Run(() => WebRequests.DeleteComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst>() { dmComponentMst }));

            DmComponentMst linqResult = LinqManager.FilterFirstRoutingCode(dmComponentMst.RoutingCode);
            BaseSingletonManager.Instance.AssetList.Remove(linqResult);

            ObservableCollection<UserControl> linqResult2 = MapFrameViewModel.Instance.Workspace.Where(p =>
            {
                return (p.DataContext as PMSLayoutViewModel).DmComponentM.RoutingCode == dmComponentMst.RoutingCode || (p.DataContext as PMSLayoutViewModel).DmComponentM.PRoutingCode == dmComponentMst.RoutingCode;
            }).ToObservableCollection();

            foreach (UserControl data in linqResult2)
            {
                MapFrameViewModel.Instance.Workspace.Remove(data);
            }

            MapFrameViewModel.Instance.PMSLayoutVM.LoadComponentList();
            (MapFrameViewModel.Instance.AssetListV.DataContext as Asset_ListViewModel).SetLayoutList();

            SelectedItemLayoutList = null;
        }

        public async void OnMapListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_Dragitem != null && e.NewItems != null && e.NewItems.Count != 0)
            {
                DmComponentMst dragedItem = _Dragitem;
                _Dragitem = null;

                foreach (DmComponentMst changedItem in e.NewItems)
                {
                    if (changedItem == dragedItem)
                    {
                        int statusCode = await Task.Run(() => WebRequests.PutComponentMst(CoPlatformWebClient.Instance, new List<DmComponentMst>() { changedItem }));
                    }
                }
            }
        }

        private void SetLastLayout()
        {
            if (string.IsNullOrEmpty(Settings.Default.LastLayoutCode))
            {
                return;
            }

            SelectedItemLayoutList = LinqManager.FilterFirstRoutingCode(Settings.Default.LastLayoutCode);
            SetLayout();
        }

        public void SetLayout()
        {
            if (SelectedItemLayoutList == null || SelectedItemLayoutList.RoutingCode == null)
            {
                return;
            }

            MapFrameViewModel.Instance.SelectedComponentConfig(SelectedItemLayoutList);

            Settings.Default.LastLayoutCode = SelectedItemLayoutList.RoutingCode;
            Settings.Default.Save();
        }

        #region Commands

        public DelegateCommand CreateMainLayoutCommand => new DelegateCommand(CreateMainLayoutCommandExecute);
        public DelegateCommand CreateLayoutCommand => new DelegateCommand(CreateLayoutCommandExecute);
        public DelegateCommand<DmComponentMst> ModifyLayoutCommand => new DelegateCommand<DmComponentMst>(ModifyLayoutCommandExecute);
        public DelegateCommand<DmComponentMst> RemoveLayoutCommand => new DelegateCommand<DmComponentMst>(RemoveLayoutCommandExecute);
        public DelegateCommand<DevExpress.Xpf.Core.StartRecordDragEventArgs> TreeStartRecordDragCommand => new DelegateCommand<DevExpress.Xpf.Core.StartRecordDragEventArgs>(TreeStartRecordDragCommandExecute);
        public DelegateCommand<DevExpress.Xpf.Core.DropRecordEventArgs> TreeDropRecordCommand => new DelegateCommand<DevExpress.Xpf.Core.DropRecordEventArgs>(TreeDropRecordCommandExecute);
        public DelegateCommand<DevExpress.Xpf.Grid.TreeList.TreeListNodeChangedEventArgs> TreeNodeChangedCommand => new DelegateCommand<DevExpress.Xpf.Grid.TreeList.TreeListNodeChangedEventArgs>(TreeNodeChangedCommandExecute);
        public DelegateCommand<RowDoubleClickEventArgs> TreeRowDoubleClickedCommand => new DelegateCommand<RowDoubleClickEventArgs>(TreeRowDoubleClickedCommandExecute);
        public DelegateCommand<KeyEventArgs> TreeViewKeyDownCommand => new DelegateCommand<KeyEventArgs>(TreeViewKeyDownCommandExecute);
        public DelegateCommand RowDoubleClickCommand => new DelegateCommand(RowDoubleClickCommandExecute);
        public DelegateCommand ModifiedMapCommand => new DelegateCommand(ModifiedMapCommandExecute);

        private void CreateMainLayoutCommandExecute()
        {
            DmComponentMst model = new DmComponentMst
            {
                MapType = 1,
                RoutingCode = Guid.NewGuid().ToString(),
                RoutingType = ComponentType.Screen.ToString(),
                RoutingName = Res.StrScreen
            };

            model.PRoutingCode = null;

            CreateUserRoutingConfiguration(model);
        }

        private void CreateLayoutCommandExecute()
        {
            DmComponentMst model = new DmComponentMst
            {
                MapType = 2,
                RoutingCode = Guid.NewGuid().ToString()
            };

            model.PropertyM.ItemCode = model.RoutingCode;

            //선택된 항목이 없는 경우
            if (SelectedItemLayoutList.PRoutingCode == null)
            {
                model.PRoutingCode = model.RoutingCode;
            }
            else// 선택된 항목이 있는 경우
            {
                model.PRoutingCode = SelectedItemLayoutList.RoutingCode;
            }

            model.RoutingType = ComponentType.DM701000.ToString();
            model.RoutingName = Res.StrVirtual;

            CreateUserRoutingConfiguration(model);
        }

        private void ModifyLayoutCommandExecute(DmComponentMst model)
        {
            if (model == null)
            {
                return;
            }

            model.IsEditMode = true;
            DmComponentMstM = model;
        }

        private void RemoveLayoutCommandExecute(DmComponentMst model)
        {
            if (model == null)
            {
                return;
            }

            DeleteUserRoutingConfiguration(model);
        }

        private void TreeStartRecordDragCommandExecute(DevExpress.Xpf.Core.StartRecordDragEventArgs e)
        {
            if (MapFrameViewModel.Instance.RoutingConfigType == ScenarioTypes.PMS)
            {
                e.AllowDrag = false;
                e.Handled = true;
                return;
            }

            string typeName = Mouse.DirectlyOver.GetType().FullName;
            if (typeName == "System.Windows.Controls.TextBoxView" || typeName == "System.Windows.Controls.TextBox")
            {
                e.AllowDrag = false;
                e.Handled = true;
                return;
            }
        }

        private void TreeDropRecordCommandExecute(DevExpress.Xpf.Core.DropRecordEventArgs e)
        {
            try
            {
                TreeListView treeListView = e.Source as TreeListView;
                RecordDragDropData dragData = e.Data.GetData(typeof(RecordDragDropData)) as RecordDragDropData;
                DmComponentMst dragRecord = dragData.Records[0] as DmComponentMst;
                DmComponentMst targetRecord = e.TargetRecord as DmComponentMst;

                _Dragitem = dragRecord;

                if (e.DropPosition != DropPosition.Inside)
                {
                    TreeListNode targetParentNode = treeListView.GetNodeByKeyValue(targetRecord.PRoutingCode);

                    if (targetParentNode == null)
                    {
                        return;
                    }

                    targetRecord = targetParentNode.Content as DmComponentMst;
                }
            }
            catch (Exception ex)
            {
                e.Effects = DragDropEffects.None;
                MessageBox.Show(ex.Message);
            }
        }

        private void TreeNodeChangedCommandExecute(DevExpress.Xpf.Grid.TreeList.TreeListNodeChangedEventArgs e)
        {
            if (MapFrameViewModel.Instance.RoutingConfigType == ScenarioTypes.PMS)
            {
                return;
            }

            try
            {
                if (e.ChangeType == NodeChangeType.Remove || e.ChangeType == NodeChangeType.Add)
                {
                    TreeListNode parentNode = e.Node.ParentNode;
                    if (parentNode != null)
                    {
                        if (parentNode.Content is MapListItemViewModel parentContent &&
                            parentContent.Model.MapType == 0)
                        {
                            (parentContent as MapListItemViewModel).IsExpanded = parentNode.HasChildren && parentNode.IsExpanded;
                        }
                    }
                }
            }
            catch { }
        }

        private void TreeRowDoubleClickedCommandExecute(RowDoubleClickEventArgs e)
        {
            try
            {
                string typeName = Mouse.DirectlyOver.GetType().FullName;
                if (typeName == "System.Windows.Controls.TextBoxView" || e.ChangedButton != MouseButton.Left)
                {
                    return;
                }

                TreeListView treeListView = e.Source as TreeListView;
                TreeListNode targetNode = treeListView.GetNodeByRowHandle(e.HitInfo.RowHandle);

                if (targetNode != null)
                {
                    if (targetNode.HasChildren)
                    {
                        targetNode.IsExpanded = true;
                    }
                    else
                    {
                        if (targetNode.Content is MapListItemViewModel targetRecord && targetRecord.Model.MapType == 1)
                        {
                            // [수정]
                            //MapFrameViewModel.Instance.SelectedMapListInfo(targetRecord.Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void TreeViewKeyDownCommandExecute(KeyEventArgs e)
        {
            try
            {
                TreeListView s = e.Source as TreeListView;
                switch (e.Key)
                {
                    case Key.F2:
                        {
                            if (s.DataControl.SelectedItem != null)
                            {
                                DmComponentMst target = s.DataControl.SelectedItem as DmComponentMst;

                                target.IsEditMode = true;
                            }
                            return;
                        }
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteLine(LOG.LOG, LOG_LEVEL.ErrorLevel, ex.ToString());
            }
        }

        private void RowDoubleClickCommandExecute()
        {
            SetLayout();
        }

        private void ModifiedMapCommandExecute()
        {
            if (DmComponentMstM == null)
            {
                return;
            }

            DmComponentMstM.PropertyM.ItemCode = DmComponentMstM.RoutingCode;
            UpdateRoutingConfiguration(DmComponentMstM);

            UserControl resultLinq = MapFrameViewModel.Instance.Workspace.FirstOrDefault(p => (p.DataContext as PMSLayoutViewModel).DmComponentM.RoutingCode == DmComponentMstM.RoutingCode);

            if (resultLinq == null)
            {
                return;
            }

            (resultLinq.DataContext as PMSLayoutViewModel).DmComponentM.RoutingName = DmComponentMstM.RoutingName;
        }

        #endregion
    }
}
