using Coever.Lib.CoPlatform.Core.Models;
using Coever.Lib.CoPlatform.Core.Network;
using DMS.Module.Management.Events;
using DMS.Module.Management.Model.Recording;
using DMS.Module.Management.Network;
using DMS.Module.Management.Properties;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.Async;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class IPCamRecordInquiryViewModel : DMViewModelBase
    {
        #region private variables
        private readonly AsyncTask<string, IList<DmIpcamMst>> _loadListTask;
        private readonly AsyncTask<object, IList<Recording>> _inquiryRecordListTask;
        #endregion

        #region Properties
        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<IPCamRecordPlayerCloseEvent>>().Unsubscribe(OnPlayerClosed);
                    base.EventAggregator = value;
                    base.EventAggregator?.GetEvent<PubSubEvent<IPCamRecordPlayerCloseEvent>>().Subscribe(OnPlayerClosed);
                }
            }
        }

        private bool _IsCheckedStartTime = false;
        public bool IsCheckedStartTime
        {
            get => _IsCheckedStartTime;
            set
            {
                _IsCheckedStartTime = value;
                RaisePropertyChanged("IsCheckedStartTime");
            }
        }

        private bool _IsCheckedEndTime = false;
        public bool IsCheckedEndTime
        {
            get => _IsCheckedEndTime;
            set
            {
                _IsCheckedEndTime = value;
                RaisePropertyChanged("IsCheckedEndTime");
            }
        }

        private DateTime _StartTime = DateTime.Now.AddDays(-1);
        public DateTime StartTime
        {
            get => _StartTime;
            set
            {
                _StartTime = value;
                RaisePropertyChanged("StartTime");
            }
        }

        private DateTime _EndTime = DateTime.Now;
        public DateTime EndTime
        {
            get => _EndTime;
            set
            {
                _EndTime = value;
                RaisePropertyChanged("EndTime");
            }
        }

        private readonly List<DmIpcamMst> _IPCamList;
        private readonly ListCollectionView _IPCamListView;
        public ListCollectionView IPCamListView => _IPCamListView;

        private readonly List<Recording> _IPCamRecordList;
        private readonly ListCollectionView _IPCamRecordListView;
        public ListCollectionView IPCamRecordListView => _IPCamRecordListView;

        private IPCamRecordPalyerViewModel _IPCamRecordPlayerVM;
        public IPCamRecordPalyerViewModel IPCamRecordPlayerVM
        {
            get => _IPCamRecordPlayerVM;
            set
            {
                if (_IPCamRecordPlayerVM != value)
                {
                    IPCamRecordPalyerViewModel old = _IPCamRecordPlayerVM;

                    _IPCamRecordPlayerVM = value;
                    RaisePropertyChanged("IPCamRecordPlayerVM");

                    old?.Dispose();
                }
            }
        }
        #endregion

        #region Constructor
        public IPCamRecordInquiryViewModel()
        {
            _IPCamList = new List<DmIpcamMst>();
            _IPCamListView = new ListCollectionView(_IPCamList);

            _IPCamRecordList = new List<Recording>();
            _IPCamRecordListView = new ListCollectionView(_IPCamRecordList);

            _loadListTask = new AsyncTask<string, IList<DmIpcamMst>>(
                null,
                (task, inputs) =>
                {
                    List<DmIpcamMst> result = CoPlatformWebClient.Instance.GetIPCams();
                    return task.Status == AsyncTaskStatus.RUNNING ? result : null;
                },
                (task, result, inputs) =>
                {
                    if (result == null)
                    {
                        return;
                    }

                    try
                    {
                        _IPCamList.Clear();
                        _IPCamList.AddRange(result);
                        IPCamListView.Refresh();
                        if (_IPCamList.Count > 0)
                        {
                            IPCamListView.MoveCurrentToFirst();
                        }
                    }
                    catch { }
                });

            _inquiryRecordListTask = new AsyncTask<object, IList<Recording>>(
                (task, inputs) =>
                {
                    _IPCamRecordList.Clear();
                    IPCamRecordListView.Refresh();
                },
                (task, inputs) =>
                {
                    DmIpcamMst ipcam = null;
                    DateTime? startTime = null;
                    DateTime? endTime = null;

                    if (inputs.Length == 3)
                    {
                        ipcam = inputs[0] as DmIpcamMst;
                        startTime = (DateTime?)inputs[1];
                        endTime = (DateTime?)inputs[2];
                    }

                    IList<Recording> result = Coever.Lib.Axis.Core.Utils.GetRecordingList(ipcam.Address, ipcam.Account, ipcam.Pwd, startTime, endTime)
                    .ConvertAll(item => new Recording
                    {
                        DiskId = item.DiskId,
                        EventId = item.EventId,
                        EventTrigger = item.EventTrigger,
                        IPCam = ipcam,
                        Locked = item.Locked,
                        RecordingId = item.RecordingId,
                        RecordingStatus = item.RecordingStatus,
                        RecordingType = item.RecordingType,
                        Source = item.Source,
                        StartTime = item.StartTime,
                        StopTime = item.StopTime,
                        Video = item.Video,
                    });

                    return task.Status == AsyncTaskStatus.RUNNING ? result : null;
                },
                (task, result, inputs) =>
                {
                    if (result == null)
                    {
                        return;
                    }

                    try
                    {
                        _IPCamRecordList.Clear();
                        _IPCamRecordList.AddRange(result);
                        IPCamRecordListView.Refresh();
                    }
                    catch { }
                },
                null,
                (task, ex, inputs) =>
                {
                    MessageBox.Show(Res.MsgFailedLoadData + "\r\n" + (ex.InnerException ?? ex).Message);
                });
        }
        #endregion

        #region Commands / Events
        public DelegateCommand<RoutedEventArgs> LoadedCommand => new DelegateCommand<RoutedEventArgs>(LoadedCommandExecute);

        private void LoadedCommandExecute(RoutedEventArgs e)
        {
            try
            {
                UIElement control = e.Source as UIElement;

                if (control == null || !control.IsVisible)
                {
                    return;
                }

                if (_loadListTask.IsPending)
                {
                    _loadListTask.execute();
                }
            }
            catch { }
        }

        public DelegateCommand CommitCommand => new DelegateCommand(CommitCommandExecute);

        private void CommitCommandExecute()
        {
            try
            {
                if(IPCamListView.CurrentItem == null)
                {
                    return;
                }

                if (_inquiryRecordListTask.IsPending)
                {
                    DateTime? startTime = null;
                    DateTime? endTime = null;

                    if (IsCheckedStartTime)
                    {
                        startTime = StartTime;
                    }
                    if (IsCheckedEndTime)
                    {
                        endTime = EndTime;
                    }

                    _inquiryRecordListTask.execute(IPCamListView.CurrentItem, startTime, endTime);
                }
            }
            catch
            {
                MessageBox.Show(Res.MsgFailedLoadData);
            }

        }

        public DelegateCommand<DevExpress.Xpf.Grid.RowDoubleClickEventArgs> RowDoubleClickCommand => new DelegateCommand<DevExpress.Xpf.Grid.RowDoubleClickEventArgs>(RowDoubleClickCommandExecute);

        private void RowDoubleClickCommandExecute(DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {
            try
            {
                Recording selectedItem = e.Source.DataControl.CurrentItem as Recording;

                if (IPCamRecordPlayerVM == null
                    || IPCamRecordPlayerVM.RecordModel?.IPCam != selectedItem?.IPCam
                    || IPCamRecordPlayerVM.RecordModel?.RecordingId != selectedItem?.RecordingId)
                {
                    IPCamRecordPlayerVM = new IPCamRecordPalyerViewModel(selectedItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public DelegateCommand<Recording> RemoveCommand => new DelegateCommand<Recording>(RemoveCommandExecute);

        private void RemoveCommandExecute(Recording selectedItem)
        {
            if (MessageBox.Show(Res.MsgIsOkRemoveItem, Res.StrWarning, MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {
                    if (Coever.Lib.Axis.Core.Utils.RemoveRecording(
                        selectedItem?.IPCam?.Address, selectedItem?.IPCam?.Account,
                        selectedItem?.IPCam?.Pwd, selectedItem?.RecordingId))
                    {
                        _IPCamRecordList.Remove(selectedItem);
                        IPCamRecordListView.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(Res.MsgFailedDeleteReq);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(Res.MsgFailedDeleteReq + Environment.NewLine + (e.InnerException ?? e).Message);
                }
            }
        }

        public DelegateCommand<Recording> RemovePreviousCommand => new DelegateCommand<Recording>(RemovePreviousCommandExecute);

        private void RemovePreviousCommandExecute(Recording selectedItem)
        {
            if (MessageBox.Show(Res.MsgIsOkRemoveItem, Res.StrWarning, MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                try
                {
                    if (selectedItem != null && Coever.Lib.Axis.Core.Utils.RemoveRecordings(
                        selectedItem?.IPCam?.Address, selectedItem?.IPCam?.Account,
                        selectedItem?.IPCam?.Pwd, selectedItem.StartTime))
                    {
                        _IPCamRecordList.RemoveAll(item =>
                        {
                            return item.IPCam == selectedItem.IPCam && item.StartTime < selectedItem.StartTime;
                        });
                        IPCamRecordListView.Refresh();
                    }
                    else
                    {
                        MessageBox.Show(Res.MsgFailedDeleteReq);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(Res.MsgFailedDeleteReq + Environment.NewLine + (e.InnerException ?? e).Message);
                }
            }
        }

        private void OnPlayerClosed(IPCamRecordPlayerCloseEvent e)
        {
            IPCamRecordPlayerVM = null;
        }
        #endregion
    }
}
