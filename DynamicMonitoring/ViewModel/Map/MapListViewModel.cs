using DevExpress.Data.TreeList;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DynamicMonitoring.Common;
using DynamicMonitoring.Common.Broadcast;
using DynamicMonitoring.Common.Enums;
using DynamicMonitoring.Common.MVVM;
using DynamicMonitoring.Model;
using DynamicMonitoring.Resources;
using DynamicMonitoring.Utils;
using Newtonsoft.Json;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DynamicMonitoring.ViewModel
{
    public class MapListViewModel : DMViewModelBase
    {
        public ImageSource Glyph { get { return DynamicMonitoring.Common.ObjectConverter.ToImageSource(Res.ImgExplorer); } set { } }

        public string Name { get { return Res.StrExplorer; } set { } }

        private ObservableCollection<MapListItemViewModel> _MapList;
        public ObservableCollection<MapListItemViewModel> MapList
        {
            get
            {
                if (_MapList == null)
                {
                    _MapList = new ObservableCollection<MapListItemViewModel>();
                }
                return _MapList;
            }
        }

        public MapListViewModel()
        {
            LocalizationManager.Instance.CultureChanged += OnCultureChanged;

            LoadTree();
        }

        private async void LoadTree()
        {
            DataTable dt = await DBHelper.GetMapList();

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    MapModel map = row.ToObject<MapModel>();

                    MapList.Add(new MapListItemViewModel()
                    {
                        Model = map
                    });
                }
            }
        }

        private async void NewFolder(string parentID)
        {
            try
            {
                DataRow dr = await DBHelper.AddMap(parentID, "New Folder", "folder", MapList.Count);

                if (dr != null)
                {
                    MapModel map = dr.ToObject<MapModel>();

                    MapList.Add(new MapListItemViewModel()
                    {
                        Model = map
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void NewLayout(string parentID)
        {
            try
            {
                DataRow dr = await DBHelper.AddMap(parentID, "New Layout", "layout", MapList.Count);

                if (dr != null)
                {
                    MapModel map = dr.ToObject<MapModel>();

                    MapList.Add(new MapListItemViewModel()
                    {
                        Model = map
                    });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void Remove(MapListItemViewModel item)
        {
            try
            {
                await DBHelper.RemoveMap(item.Model.MapID);

                RemoveItemOnList(item);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void RemoveItemOnList(MapListItemViewModel item)
        {
            if (item != null)
            {
                if (item.Model.MapType == "folder")
                {
                    var subitems = MapList.Where(_item => _item.Model.ParentID == item.Model.MapID).ToList();

                    foreach (MapListItemViewModel subItem in subitems)
                    {
                        Remove(subItem);
                    }
                }

                MapList.Remove(item);
                BroadcastReceiver.SendBroadcast(Constants.BROADCAST_MAP_REMOVED, this, item.Model);
            }
        }

        private void OnMapListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_Dragitem != null && e.NewItems != null && e.NewItems.Count != 0)
            {
                MapListItemViewModel dragedItem = _Dragitem;
                _Dragitem = null;
                foreach (MapListItemViewModel changedItem in e.NewItems)
                {
                    if (changedItem == dragedItem)
                    {
                        DBHelper.ModifyMap(changedItem.ID, changedItem.ParentID,
                            changedItem.Model.MapName, MapList.IndexOf(changedItem));

                        changedItem.Model.ParentID = changedItem.ParentID;
                    }
                }
            }
        }

        public void OnTreeStartRecordDrag(object sender, DevExpress.Xpf.Core.StartRecordDragEventArgs e)
        {
            var typeName = Mouse.DirectlyOver.GetType().FullName;
            Console.WriteLine(typeName);
            if (typeName == "System.Windows.Controls.TextBoxView"
                || typeName == "System.Windows.Controls.TextBox")
            {
                e.AllowDrag = false;
                e.Handled = true;
                return;
            }
        }

        private MapListItemViewModel _Dragitem = null;
        public void OnTreeDropRecord(object sender, DevExpress.Xpf.Core.DropRecordEventArgs e)
        {
            try
            {
                TreeListView treeListView = sender as TreeListView;
                RecordDragDropData dragData = e.Data.GetData(typeof(RecordDragDropData)) as RecordDragDropData;
                MapListItemViewModel dragRecord = dragData.Records[0] as MapListItemViewModel;
                MapListItemViewModel targetRecord = e.TargetRecord as MapListItemViewModel;

                _Dragitem = dragRecord;

                if (e.DropPosition != DropPosition.Inside)
                {
                    TreeListNode targetParentNode = treeListView.GetNodeByKeyValue(targetRecord.ParentID);

                    if (targetParentNode == null)
                    {
                        return;
                    }

                    targetRecord = targetParentNode.Content as MapListItemViewModel;
                }

                if (targetRecord != null && targetRecord.Model.MapType == "layout")
                {
                    e.Effects = DragDropEffects.None;
                    MessageBox.Show(Res.MsgCanOnlyDropInFolder);
                    _Dragitem = null;

                    return;
                }
            }
            catch (Exception ex)
            {
                e.Effects = DragDropEffects.None;
                MessageBox.Show(ex.Message);
            }
        }

        public void OnTreeNodeChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeChangedEventArgs e)
        {
            try
            {
                if (e.ChangeType == NodeChangeType.Remove || e.ChangeType == NodeChangeType.Add)
                {
                    TreeListNode parentNode = e.Node.ParentNode;
                    if (parentNode != null)
                    {
                        MapListItemViewModel parentContent = parentNode.Content as MapListItemViewModel;

                        if (parentContent != null && parentContent.Model.MapType == "folder")
                        {
                            (parentContent as MapListItemViewModel).IsExpanded = parentNode.HasChildren && parentNode.IsExpanded;
                        }
                    }
                }
            }
            catch { }
        }

        public void OnTreeRowDoubleClicked(object sender, RowDoubleClickEventArgs e)
        {
            try
            {
                var typeName = Mouse.DirectlyOver.GetType().FullName;
                if (typeName == "System.Windows.Controls.TextBoxView" || e.ChangedButton != MouseButton.Left)
                {
                    return;
                }

                TreeListView treeListView = sender as TreeListView;
                TreeListNode targetNode = treeListView.GetNodeByRowHandle(e.HitInfo.RowHandle);

                if (targetNode != null)
                {
                    if (targetNode.HasChildren)
                    {
                        targetNode.IsExpanded = !targetNode.IsExpanded;
                    }
                    else
                    {
                        MapListItemViewModel targetRecord = targetNode.Content as MapListItemViewModel;

                        if (targetRecord != null && targetRecord.Model.MapType == "layout")
                        {
                            BroadcastReceiver.SendBroadcast(Constants.BROADCAST_SELECTED_MAPLIST, this, targetRecord.Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        public void OnTreeViewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TreeListView s = sender as TreeListView;
                switch (e.Key)
                {
                    case Key.F2:
                        {
                            if (s.DataControl.SelectedItem != null)
                            {
                                MapListItemViewModel target = s.DataControl.SelectedItem as MapListItemViewModel;

                                target.IsEditMode = true;
                            }
                            return;
                        }
                }
            }
            catch { }
        }

        #region Commands
        public DelegateCommand<MapListItemViewModel> NewFolderCommand
        {
            get { return new DelegateCommand<MapListItemViewModel>(NewFolderCommandExecute); }
        }

        public DelegateCommand<MapListItemViewModel> NewMapCommand
        {
            get { return new DelegateCommand<MapListItemViewModel>(NewMapCommandExecute); }
        }

        public DelegateCommand<MapListItemViewModel> ModifyMapCommand
        {
            get { return new DelegateCommand<MapListItemViewModel>(ModifyMapCommandExecute); }
        }

        public DelegateCommand<MapListItemViewModel> RemoveMapCommand
        {
            get { return new DelegateCommand<MapListItemViewModel>(RemoveMapCommandExecute); }
        }

        private void NewFolderCommandExecute(MapListItemViewModel target)
        {
            if (target == null)
            {
                NewFolder(null);
            }
            else
            {
                NewFolder(target.Model.MapID);
            }
        }

        private void NewMapCommandExecute(MapListItemViewModel target)
        {
            if (target == null)
            {
                NewLayout(null);
            }
            else
            {
                NewLayout(target.Model.MapID);
            }
        }

        private void ModifyMapCommandExecute(MapListItemViewModel target)
        {
            target.IsEditMode = true;
        }

        private void RemoveMapCommandExecute(MapListItemViewModel target)
        {
            MessageBoxResult result = MessageBox.Show(
                                    target.Model.MapType == "folder" ? Res.MsgFolderRemoveWarning : Res.MsgItemRemoveWarning,
                                    Res.StrWarning, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Remove(target);
            }
        }
        #endregion
    }
}
