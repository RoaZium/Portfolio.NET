using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using Coever.Lib.Core.Log;
using Coever.Lib.Mqtt.Core;
using DMS.Module.Map.Events;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using DMS.Module.Map.Network;
using DMS.Module.Map.Properties;
using DMS.Module.Map.Requests;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.Async;
using PrismWPF.Common.Infrastructure;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace DMS.Module.Map.ViewModel
{
    public class ScenarioViewModel : DMViewModelBase
    {
        #region AsyncTask
        private readonly AsyncTask<ScenarioModel, object> _ScenarioRunningTask;
        private readonly AsyncTask<object, object> _AlarmCheckTask;

        private AsyncTask<ScenarioTypes, IList<ScenarioModel>> _currentLoadScenarioListTask;
        private AsyncTask<ScenarioModel, ScenarioModel> _currentLoadScenarioInfoTask;
        #endregion

        #region Private variables
        public volatile bool _skipAlarmCheckDelay = false;
        private volatile bool _IsInHandlingAlarmTargetListView = false;

        private volatile object _alarmScenarioListToken = null;
        #endregion

        #region Properties

        public string Name { get => Res.StrScenario; set { } }

        private bool _IsScenarioStarted = false;
        public bool IsScenarioStarted
        {
            get => _IsScenarioStarted;
            set
            {
                if (_IsScenarioStarted != value)
                {
                    _IsScenarioStarted = value;
                    RaisePropertyChanged("IsScenarioStarted");
                    RaisePropertyChanged("IsScenarioPaused");
                }
            }
        }

        private bool _IsAlarmScenarioActivated = false;
        public bool IsAlarmScenarioActivated
        {
            get => _IsAlarmScenarioActivated;
            set
            {
                if (_IsAlarmScenarioActivated != value)
                {
                    _IsAlarmScenarioActivated = value;
                    _skipAlarmCheckDelay = true;
                    RaisePropertyChanged("IsAlarmScenarioActivated");
                }
            }
        }

        private bool _CanExecuteAlarmScenario = false;
        public bool CanExecuteAlarmScenario
        {
            get => _CanExecuteAlarmScenario;
            set
            {
                if (_CanExecuteAlarmScenario != value)
                {
                    _CanExecuteAlarmScenario = value;
                    RaisePropertyChanged("CanExecuteAlarmScenario");
                }
            }
        }

        public bool IsScenarioPaused => !IsScenarioStarted;

        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<MapTypeChangedEvent>>().Unsubscribe(OnMapTypeChanged);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<MapTypeChangedEvent>>().Subscribe(OnMapTypeChanged);
                }
            }
        }

        private ScenarioTypes _ScenarioType = ScenarioTypes.PMS;
        public ScenarioTypes ScenarioType
        {
            get => _ScenarioType;
            set
            {
                _ScenarioType = value;
                RaisePropertyChanged("ScenarioType");
                CanExecuteAlarmScenario = _ScenarioType == ScenarioTypes.PMS;
            }
        }

        private readonly List<ScenarioModel> _ScenarioList;
        private readonly ListCollectionView _ScenarioListView;
        public ListCollectionView ScenarioListView => _ScenarioListView;

        private readonly List<ActionPackerModel> _ActionList;
        private readonly ListCollectionView _ActionListView;
        public ListCollectionView ActionListView => _ActionListView;

        private readonly List<KeyValuePair<string, object>?> _RunningScenarioList;
        private readonly ListCollectionView _RunningScenarioListView;
        public ListCollectionView RunningScenarioListView => _RunningScenarioListView;

        private readonly List<ActionPackerModel> _RunningActionList;
        private readonly ListCollectionView _RunningActionListView;
        public ListCollectionView RunningActionListView => _RunningActionListView;

        private readonly List<AlarmMonitoringTargetModel> _AlarmMonitoringDangerTargetList;
        private readonly ListCollectionView _AlarmMonitoringDangerTargetListView;
        public ListCollectionView AlarmMonitoringDangerTargetListView => _AlarmMonitoringDangerTargetListView;

        private readonly List<AlarmMonitoringTargetModel> _AlarmMonitoringAbnormalTargetList;
        private readonly ListCollectionView _AlarmMonitoringAbnormalTargetListView;
        public ListCollectionView AlarmMonitoringAbnormalTargetListView => _AlarmMonitoringAbnormalTargetListView;

        private readonly List<AlarmMonitoringTargetModel> _AlarmMonitoringStopTargetList;
        private readonly ListCollectionView _AlarmMonitoringStopTargetListView;
        public ListCollectionView AlarmMonitoringStopTargetListView => _AlarmMonitoringStopTargetListView;

        #endregion

        #region Constructor
        public ScenarioViewModel()
        {
            _ScenarioList = new List<ScenarioModel>();
            _ScenarioListView = new ListCollectionView(_ScenarioList);

            _ActionList = new List<ActionPackerModel>();
            _ActionListView = new ListCollectionView(_ActionList);

            _RunningScenarioList = new List<KeyValuePair<string, object>?>();
            _RunningScenarioListView = new ListCollectionView(_RunningScenarioList);

            _RunningActionList = new List<ActionPackerModel>();
            _RunningActionListView = new ListCollectionView(_RunningActionList);

            _AlarmMonitoringDangerTargetList = new List<AlarmMonitoringTargetModel>();
            _AlarmMonitoringDangerTargetListView = new ListCollectionView(_AlarmMonitoringDangerTargetList);

            _AlarmMonitoringAbnormalTargetList = new List<AlarmMonitoringTargetModel>();
            _AlarmMonitoringAbnormalTargetListView = new ListCollectionView(_AlarmMonitoringAbnormalTargetList);

            _AlarmMonitoringStopTargetList = new List<AlarmMonitoringTargetModel>();
            _AlarmMonitoringStopTargetListView = new ListCollectionView(_AlarmMonitoringStopTargetList);

            _ScenarioRunningTask = new AsyncTask<ScenarioModel, object>(
                (task, inputs) =>
                {
                    _AlarmMonitoringDangerTargetList.Clear();
                    _AlarmMonitoringAbnormalTargetList.Clear();
                    _AlarmMonitoringStopTargetList.Clear();
                    _RunningScenarioList.Clear();
                    _RunningActionList.Clear();
                    AlarmMonitoringDangerTargetListView.Refresh();
                    AlarmMonitoringAbnormalTargetListView.Refresh();
                    AlarmMonitoringStopTargetListView.Refresh();
                    RunningScenarioListView.Refresh();
                    RunningActionListView.Refresh();
                    _alarmScenarioListToken = new object();
                    _AlarmCheckTask.execute();
                },
                (task, inputs) =>
                {
                    if (inputs.Length == 1)
                    {
                        // 사용자가 선택한 시나리오
                        ScenarioModel selectedScenario = inputs[0];

                        selectedScenario.ScenarioType = ScenarioTypes.PMS;

                        // 사용자가 선택한 시나리오(Packed)
                        KeyValuePair<string, object>? packedSelectedScenario = null;
                        if (selectedScenario != null)
                        {
                            packedSelectedScenario = new KeyValuePair<string, object>(
                                selectedScenario.ScenarioName ?? "Nonamed",
                                selectedScenario);
                        }

                        // 알람 타겟 리스트가 변경됐는지 체크하기 위한 토큰
                        object alarmScenarioListToken = null;

                        // 자동 모니터링 로직 시작
                        while (task.IsRunning)
                        {
                            try
                            {
                                object target = null;

                                // 알람 타겟 리스트가 업데이트중이라면 끝나길 기다린다.
                                while (_IsInHandlingAlarmTargetListView && task.IsRunning) { }
                                if (!task.IsRunning)
                                {
                                    return null;
                                }

                                // 이번에 돌릴 시나리오(target)를 찾는다.
                                try
                                {
                                    _IsInHandlingAlarmTargetListView = true;

                                    if (AlarmMonitoringDangerTargetListView.Count > 0)
                                    {
                                        AlarmMonitoringDangerTargetListView.MoveCurrentToNext();
                                        if (AlarmMonitoringDangerTargetListView.IsCurrentAfterLast)
                                        {
                                            AlarmMonitoringDangerTargetListView.MoveCurrentToFirst();
                                        }
                                        target = AlarmMonitoringDangerTargetListView.CurrentItem;
                                    }
                                    else if (AlarmMonitoringAbnormalTargetListView.Count > 0)
                                    {
                                        AlarmMonitoringAbnormalTargetListView.MoveCurrentToNext();
                                        if (AlarmMonitoringAbnormalTargetListView.IsCurrentAfterLast)
                                        {
                                            AlarmMonitoringAbnormalTargetListView.MoveCurrentToFirst();
                                        }
                                        target = AlarmMonitoringAbnormalTargetListView.CurrentItem;
                                    }
                                    else if (AlarmMonitoringStopTargetListView.Count > 0)
                                    {
                                        AlarmMonitoringStopTargetListView.MoveCurrentToNext();
                                        if (AlarmMonitoringStopTargetListView.IsCurrentAfterLast)
                                        {
                                            AlarmMonitoringStopTargetListView.MoveCurrentToFirst();
                                        }
                                        target = AlarmMonitoringStopTargetListView.CurrentItem;
                                    }
                                    else
                                    {
                                        target = selectedScenario;
                                    }
                                }
                                catch { }
                                finally
                                {
                                    _IsInHandlingAlarmTargetListView = false;
                                }

                                // target이 null이면 돌릴 시나리오가 없다는 의미
                                if (target == null)
                                {
                                    alarmScenarioListToken = _alarmScenarioListToken;

                                    // 기존 리스트와 다른점이 있는지 확인
                                    if (RunningScenarioListView.Count > 0)
                                    {
                                        // 리스트를 비워줌
                                        _RunningScenarioList.Clear();
                                        _RunningActionList.Clear();
                                        task.Dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            RunningScenarioListView.Refresh();
                                            RunningActionListView.Refresh();
                                        })).Wait();
                                    }
                                }
                                // target이 selectedScenario이면 돌릴 리스트에 selectedScenario만 있다는 의미
                                else if (target == selectedScenario)
                                {
                                    alarmScenarioListToken = _alarmScenarioListToken;

                                    // 기존 리스트와 다른점이 있는지 확인
                                    if (RunningScenarioListView.CurrentItem == null
                                        || !RunningScenarioListView.CurrentItem.Equals(packedSelectedScenario))
                                    {
                                        // 리스트에 selectedScenario만 넣어줌
                                        _RunningScenarioList.Clear();
                                        _RunningActionList.Clear();
                                        _RunningScenarioList.Add(packedSelectedScenario);
                                        if (selectedScenario.ScenarioActions != null)
                                        {
                                            _RunningActionList.AddRange(
                                                selectedScenario.ScenarioActions.ConvertAll(item
                                                    => new ActionPackerModel(selectedScenario.ScenarioType, item)));
                                        }
                                        task.Dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            RunningScenarioListView.Refresh();
                                            RunningActionListView.Refresh();
                                            RunningScenarioListView.MoveCurrentToFirst();
                                        })).Wait();
                                    }
                                }
                                // target이 알람 시나리오일 경우
                                else
                                {
                                    // 알람 타겟 리스트가 변경되었으면 시나리오 리스트를 재구성한다
                                    if (alarmScenarioListToken != _alarmScenarioListToken
                                        || !_RunningScenarioList.Exists(
                                            item => item != null && item.Value.Value == target))
                                    {
                                        alarmScenarioListToken = _alarmScenarioListToken;

                                        List<KeyValuePair<string, object>?> runningScenarioList
                                            = new List<KeyValuePair<string, object>?>();
                                        KeyValuePair<string, object>? currentItem = null;
                                        List<IActionModel> currentItemsActions = null;
                                        ScenarioTypes mapType = ScenarioTypes.PMS;

                                        foreach (AlarmMonitoringTargetModel item in _AlarmMonitoringDangerTargetList)
                                        {
                                            if (item != null)
                                            {
                                                KeyValuePair<string, object> packedItem
                                                    = new KeyValuePair<string, object>(item.ResourceName, item);
                                                if (target == item)
                                                {
                                                    currentItem = packedItem;
                                                    currentItemsActions = item.Actions;
                                                }
                                                runningScenarioList.Add(packedItem);
                                            }
                                        }
                                        foreach (AlarmMonitoringTargetModel item in _AlarmMonitoringAbnormalTargetList)
                                        {
                                            if (item != null)
                                            {
                                                KeyValuePair<string, object> packedItem
                                                    = new KeyValuePair<string, object>(item.ResourceName, item);
                                                if (target == item)
                                                {
                                                    currentItem = packedItem;
                                                    currentItemsActions = item.Actions;
                                                }
                                                runningScenarioList.Add(packedItem);
                                            }
                                        }
                                        foreach (AlarmMonitoringTargetModel item in _AlarmMonitoringStopTargetList)
                                        {
                                            if (item != null)
                                            {
                                                KeyValuePair<string, object> packedItem
                                                    = new KeyValuePair<string, object>(item.ResourceName, item);
                                                if (target == item)
                                                {
                                                    currentItem = packedItem;
                                                    currentItemsActions = item.Actions;
                                                }
                                                runningScenarioList.Add(packedItem);
                                            }
                                        }

                                        if (packedSelectedScenario != null)
                                        {
                                            if (currentItem == null)
                                            {
                                                currentItem = packedSelectedScenario;
                                                currentItemsActions = selectedScenario.ScenarioActions;
                                                mapType = selectedScenario.ScenarioType;
                                            }
                                            runningScenarioList.Add(packedSelectedScenario);
                                        }

                                        _RunningScenarioList.Clear();
                                        _RunningActionList.Clear();
                                        _RunningScenarioList.AddRange(runningScenarioList);
                                        if (currentItemsActions != null)
                                        {
                                            _RunningActionList.AddRange(
                                            currentItemsActions.ConvertAll(item => new ActionPackerModel(mapType, item)));
                                        }
                                        task.Dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            RunningScenarioListView.Refresh();
                                            RunningScenarioListView.MoveCurrentTo(currentItem);
                                            RunningActionListView.Refresh();
                                        })).Wait();
                                    }
                                    else
                                    {
                                        KeyValuePair<string, object>? currentItem = _RunningScenarioList.Find(
                                            item => item != null && item.Value.Value == target);

                                        if (RunningScenarioListView.CurrentItem == null
                                            || !RunningScenarioListView.CurrentItem.Equals(currentItem))
                                        {
                                            _RunningActionList.Clear();
                                            if (target is AlarmMonitoringTargetModel
                                                && (target as AlarmMonitoringTargetModel).Actions != null)
                                            {
                                                AlarmMonitoringTargetModel scenario = target as AlarmMonitoringTargetModel;
                                                _RunningActionList.AddRange(scenario.Actions.ConvertAll(
                                                    item => new ActionPackerModel(ScenarioTypes.PMS, item)));
                                            }
                                            else if (target is ScenarioModel
                                                && (target as ScenarioModel).ScenarioActions != null)
                                            {
                                                ScenarioModel scenario = target as ScenarioModel;
                                                _RunningActionList.AddRange(scenario.ScenarioActions.ConvertAll(
                                                    item => new ActionPackerModel(scenario.ScenarioType, item)));
                                            }

                                            task.Dispatcher.BeginInvoke((Action)(() =>
                                            {
                                                RunningScenarioListView.MoveCurrentTo(currentItem);
                                                RunningActionListView.Refresh();
                                            })).Wait();
                                        }
                                    }
                                }

                                if (_RunningActionList.Count > 0)
                                {
                                    string targetResourceCode = null;
                                    if (target is AlarmMonitoringTargetModel)
                                    {
                                        targetResourceCode = (target as AlarmMonitoringTargetModel).ResourceCode;
                                    }

                                    foreach (ActionPackerModel actionPacker in _RunningActionList)
                                    {
                                        task.Dispatcher.BeginInvoke((Action)(() =>
                                        {
                                            RunningActionListView.MoveCurrentTo(actionPacker);
                                        })).Wait();

                                        if (actionPacker != null && actionPacker.ActionModel != null)
                                        {
                                            ScenarioModel reqScenario = target.CopyObject<ScenarioModel>();
                                            if (reqScenario.ScenarioType == null)
                                            {
                                                reqScenario.ScenarioType = actionPacker.TargetType;
                                            }
                                            ExecuteActionRequest req
                                                = new ExecuteActionRequest(
                                                    reqScenario, actionPacker.ActionModel, targetResourceCode);

                                            task.Dispatcher.BeginInvoke((Action)(() =>
                                            {
                                                EventAggregator?
                                                    .GetEvent<PubSubEvent<ExecuteActionRequest>>().Publish(req);
                                            }));

                                            while (req.IsRunning && task.IsRunning)
                                            {
                                                Thread.Sleep(100);
                                            }
                                            if (req.IsFinished)
                                            {
                                                int limitCount = (actionPacker.ActionDelaySecond ?? 0) * 1000 / 100;
                                                for (int count = 0;
                                                    count < limitCount && task.IsRunning;
                                                    count++)
                                                {
                                                    Thread.Sleep(100);
                                                }
                                            }
                                            else
                                            {
                                                req.IsCanceled = true;
                                            }

                                            if (!task.IsRunning)
                                            {
                                                return null;
                                            }

                                            if (target == selectedScenario && 
                                            (AlarmMonitoringAbnormalTargetListView.Count > 0 || 
                                            AlarmMonitoringDangerTargetListView.Count > 0 || 
                                            AlarmMonitoringStopTargetListView.Count > 0))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            catch(Exception ex)
                            { }
                        }
                    }
                    return null;
                },
                (task, result, inputs) =>
                {
                    _AlarmCheckTask.cancel();
                },
                (task, inputs) =>
                {
                    _AlarmCheckTask.cancel();
                },
                (task, ex, inputs) =>
                {
                    _AlarmCheckTask.cancel();
                });

            _AlarmCheckTask = new AsyncTask<object, object>(
                null,
                (task, inputs) =>
                {
                    while (task.IsRunning)
                    {
                        while (_IsInHandlingAlarmTargetListView && task.IsRunning)
                        {
                            Thread.Sleep(10);
                        }
                        if (!task.IsRunning)
                        {
                            return null;
                        }

                        int oldListCount = _AlarmMonitoringDangerTargetList.Count()
                            + _AlarmMonitoringAbnormalTargetList.Count()
                            + _AlarmMonitoringStopTargetList.Count();
                        try
                        {
                            _IsInHandlingAlarmTargetListView = true;
                            if (IsAlarmScenarioActivated && ScenarioType == ScenarioTypes.PMS)
                            {
                                LoadAlarmMonitoringInfo();
                            }
                            else
                            {
                                _AlarmMonitoringDangerTargetList.Clear();
                                _AlarmMonitoringAbnormalTargetList.Clear();
                                _AlarmMonitoringStopTargetList.Clear();
                                AlarmMonitoringDangerTargetListView.Refresh();
                                AlarmMonitoringAbnormalTargetListView.Refresh();
                                AlarmMonitoringStopTargetListView.Refresh();
                            }
                        }
                        catch { }
                        finally
                        {
                            if (oldListCount > 0
                                || (_AlarmMonitoringDangerTargetList.Count()
                                    + _AlarmMonitoringAbnormalTargetList.Count() 
                                    + _AlarmMonitoringStopTargetList.Count() > 0))
                            {
                                _alarmScenarioListToken = new object();
                            }
                            _IsInHandlingAlarmTargetListView = false;
                        }

                        for (int count = 0; count < 0 && task.IsRunning && !_skipAlarmCheckDelay; count++)
                        {
                            Thread.Sleep(100);
                        }
                        _skipAlarmCheckDelay = false;
                    }

                    return null;
                },
                (task, result, inputs) =>
                {
                    IsScenarioStarted = false;
                    _alarmScenarioListToken = null;
                },
                (task, inputs) =>
                {
                    IsScenarioStarted = false;
                    _alarmScenarioListToken = null;
                },
                (task, ex, inputs) =>
                {
                    _ScenarioRunningTask.cancel();

                    IsScenarioStarted = false;
                    _alarmScenarioListToken = null;
                });
        }
        #endregion

        #region Methods
        private void LoadScenarioList()
        {
            _currentLoadScenarioListTask?.cancel();
            _currentLoadScenarioInfoTask?.cancel();

            _currentLoadScenarioListTask = new AsyncTask<ScenarioTypes, IList<ScenarioModel>>(
                null,
                (task, inputs) =>
                {
                    if (inputs.Length == 1)
                    {
                        return CoPlatformWebClient.Instance.GetScenarioList(inputs[0].ToString());
                    }
                    return null;
                },
                (task, result, inputs) =>
                {
                    if (task.Status == AsyncTaskStatus.CANCELING)
                    {
                        return;
                    }

                    if (result != null)
                    {
                        result.ToList().ForEach(item => item.ScenarioType = inputs[0]);
                        _ScenarioList.Clear();
                        _ScenarioList.AddRange(result);
                        ScenarioListView.Refresh();
                        ScenarioListView.MoveCurrentToFirst();
                    }
                });
            _currentLoadScenarioListTask.execute(ScenarioType);
        }

        /// <summary>
        /// 알람 모니터링 타겟 리스트를 업데이트한다.
        /// </summary>
        private void LoadAlarmMonitoringInfo()
        {
            try
            {
                List<AlarmMonitoringTargetModel> targets = null;
                targets = CoPlatformWebClient.Instance.GetAlarmMonitoringTargets();

                object currentDangerTarget = AlarmMonitoringDangerTargetListView.CurrentItem;
                object currentAbnormalTarget = AlarmMonitoringAbnormalTargetListView.CurrentItem;
                object currentStopTarget = AlarmMonitoringStopTargetListView.CurrentItem;

                List<AlarmMonitoringTargetModel> oldDanger
                    = new List<AlarmMonitoringTargetModel>(_AlarmMonitoringDangerTargetList);
                List<AlarmMonitoringTargetModel> oldAbnormal
                    = new List<AlarmMonitoringTargetModel>(_AlarmMonitoringAbnormalTargetList);
                List<AlarmMonitoringTargetModel> oldStop
                    = new List<AlarmMonitoringTargetModel>(_AlarmMonitoringStopTargetList);

                foreach (AlarmMonitoringTargetModel target in targets)
                {
                    if (target == null)
                    {
                        continue;
                    }

                    List<AlarmMonitoringTargetModel> oldList = null;
                    List<AlarmMonitoringTargetModel> targetList = null;
                    if (target.AlarmLevel == 1)
                    {
                        oldList = oldAbnormal;
                        targetList = _AlarmMonitoringAbnormalTargetList;
                    }
                    else if (target.AlarmLevel == 2)
                    {
                        oldList = oldDanger;
                        targetList = _AlarmMonitoringDangerTargetList;
                    }
                    else if (target.AlarmLevel == 900)
                    {
                        oldList = oldStop;
                        targetList = _AlarmMonitoringStopTargetList;
                    }
                    else
                    {
                        continue;
                    }

                    AlarmMonitoringTargetModel origin
                        = targetList?.Find(
                            item => item != null
                            && item.CollectionID == target.CollectionID
                            && item.ResourceCode == target.ResourceCode);

                    if (origin != null)
                    {
                        oldList?.Remove(origin);
                    }
                    else
                    {
                        targetList?.Add(target);
                    }
                }
                AlarmMonitoringDangerTargetListView.Refresh();
                AlarmMonitoringAbnormalTargetListView.Refresh();
                AlarmMonitoringStopTargetListView.Refresh();

                foreach (AlarmMonitoringTargetModel target in _AlarmMonitoringDangerTargetList.ToArray())
                {
                    if (oldDanger.Exists(item => item == target))
                    {
                        if (target.Equals(currentDangerTarget))
                        {
                            AlarmMonitoringDangerTargetListView.MoveCurrentToPrevious();
                            currentDangerTarget = AlarmMonitoringDangerTargetListView.CurrentItem;
                        }
                        AlarmMonitoringDangerTargetListView.Remove(target);
                    }
                }
                foreach (AlarmMonitoringTargetModel target in _AlarmMonitoringAbnormalTargetList.ToArray())
                {
                    if (oldAbnormal.Exists(item => item == target))
                    {
                        if (target.Equals(currentAbnormalTarget))
                        {
                            AlarmMonitoringAbnormalTargetListView.MoveCurrentToPrevious();
                            currentAbnormalTarget = AlarmMonitoringAbnormalTargetListView.CurrentItem;
                        }
                        AlarmMonitoringAbnormalTargetListView.Remove(target);
                    }
                }
                foreach (AlarmMonitoringTargetModel target in _AlarmMonitoringStopTargetList.ToArray())
                {
                    if (oldStop.Exists(item => item == target))
                    {
                        if (target.Equals(currentStopTarget))
                        {
                            AlarmMonitoringStopTargetListView.MoveCurrentToPrevious();
                            currentStopTarget = AlarmMonitoringStopTargetListView.CurrentItem;
                        }
                        AlarmMonitoringStopTargetListView.Remove(target);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region Commands
        public DelegateCommand StartScenarioCommand => new DelegateCommand(StartScenarioCommandExecute);

        private void StartScenarioCommandExecute()
        {
            ScenarioModel currentScenario = ScenarioListView.CurrentItem as ScenarioModel;

            IsScenarioStarted = true;

            ScenarioModel SelectedScenario = currentScenario.CopyObject();
            if (SelectedScenario != null)
            {
                SelectedScenario.ScenarioActions = _ActionList.ConvertAll<IActionModel>(item => item.ActionModel);
            }

            try
            {
                _ScenarioRunningTask.execute(SelectedScenario);
            }
            catch
            {
            }
        }

        public DelegateCommand PauseScenarioCommand => new DelegateCommand(PauseScenarioCommandExecute);

        private void PauseScenarioCommandExecute()
        {
            _ScenarioRunningTask.cancel();
        }
        #endregion

        #region Event Handlers
        public override void Load()
        {
            ScenarioListView.CurrentChanged += OnCurrentScenarioChanged;

            LoadScenarioList();
        }

        public override void Unload()
        {
            ScenarioListView.CurrentChanged -= OnCurrentScenarioChanged;

            _ScenarioRunningTask.cancel();
        }

        private void OnCurrentScenarioChanged(object sender, EventArgs e)
        {
            _currentLoadScenarioInfoTask?.cancel();

            _ActionList.Clear();
            ActionListView.Refresh();

            if (ScenarioListView.CurrentItem is ScenarioModel currentItem)
            {
                _currentLoadScenarioInfoTask = new AsyncTask<ScenarioModel, ScenarioModel>(
                    null,
                    (task, inputs) =>
                    {
                        if (inputs.Length == 1)
                        {
                            return CoPlatformWebClient.Instance.GetScenarioInfo(inputs[0].ScenarioCode);
                        }
                        return null;
                    },
                    (task, result, inputs) =>
                    {
                        if (result != null)
                        {
                            _ActionList.Clear();
                            _ActionList.AddRange(result.ScenarioActions.ConvertAll<ActionPackerModel>(
                                item => new ActionPackerModel(result.ScenarioType, item)));
                            ActionListView.Refresh();
                            ActionListView.MoveCurrentToPosition(-1);
                        }
                        else
                        {
                            MessageBox.Show(Res.MsgFailedLoadScenarioAction);
                        }
                    });
                _currentLoadScenarioInfoTask.execute(currentItem);
            }
        }

        private void OnMapTypeChanged(MapTypeChangedEvent e)
        {
            ScenarioType = e.Value;

            LoadScenarioList();
        }
        #endregion
    }
}
