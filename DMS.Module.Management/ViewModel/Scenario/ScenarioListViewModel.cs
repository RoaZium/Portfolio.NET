using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Management.Events;
using DMS.Module.Management.Network;
using DMS.Module.Management.Properties;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.Async;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class ScenarioListViewModel : DMViewModelBase
    {
        #region private variables
        private object lockItemListView = new object();

        private readonly AsyncTask<string, List<ScenarioModel>> _loadListTask;
        private readonly AsyncTask<ScenarioItemViewModel, bool> _swapScenarioTask;
        private readonly AsyncTask<ScenarioItemViewModel, bool> _deleteScenarioTask;

        private ScenarioItemViewModel _beingDeletingScenario;
        private AsyncTask<object, ScenarioModel> _currentScenarioInfoLoadTask;

        private bool IsAddItem = false;
        #endregion

        #region Properties

        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesSaveEvent>>().Unsubscribe(OnScenarioPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesCloseEvent>>().Unsubscribe(OnScenarioPropertiesClose);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesSaveEvent>>().Subscribe(OnScenarioPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesCloseEvent>>().Subscribe(OnScenarioPropertiesClose);
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
            }
        }

        private readonly List<KeyValuePair<ScenarioTypes, string>> _ScenarioTypeList;
        private readonly ListCollectionView _ScenarioTypeListView;
        public ListCollectionView ScenarioTypeListView => _ScenarioTypeListView;

        private bool _IsEnableChangeScenarioType = true;
        public bool IsEnableChangeScenarioType
        {
            get => _IsEnableChangeScenarioType;
            set
            {
                _IsEnableChangeScenarioType = value;
                RaisePropertyChanged("IsEnableChangeScenarioType");
            }
        }

        private string _FilterText;
        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                RaisePropertyChanged("FilterText");

                try
                {
                    lock (lockItemListView)
                    {
                        object edittingItem = ScenarioListView.CurrentEditItem;

                        if (edittingItem != null)
                        {
                            ScenarioListView.CancelEdit();
                        }
                        ScenarioListView.Refresh();

                        if (edittingItem != null)
                        {
                            ScenarioListView.EditItem(edittingItem);
                        }
                    }
                }
                catch { }
            }
        }

        private bool _IsRefresh = false;

        private int _selectedIndexScenario;
        public int SelectedIndexScenario
        {
            get => _selectedIndexScenario;
            set
            {
                _selectedIndexScenario = value;
                RaisePropertyChanged("SelectedIndexScenario");
            }
        }

        private readonly List<ScenarioItemViewModel> _ScenarioList;
        private readonly ListCollectionView _ScenarioListView;
        public ListCollectionView ScenarioListView => _ScenarioListView;

        private ScenarioItemViewModel _selectedScenario;
        public ScenarioItemViewModel SelectedScenario
        {
            get
            {
                if (_selectedScenario == null)
                {
                    _selectedScenario = new ScenarioItemViewModel();
                }
                return _selectedScenario;
            }
            set => _selectedScenario = value;
        }

        #endregion

        #region Constructor
        public ScenarioListViewModel()
        {
            _ScenarioList = new List<ScenarioItemViewModel>();
            _ScenarioListView = new ListCollectionView(_ScenarioList);

            ScenarioListView.Filter = new Predicate<object>(
                item => string.IsNullOrEmpty(FilterText)
                    || ((item as ScenarioItemViewModel)?.Scenario?.ScenarioName?.Contains(FilterText) ?? false));

            _ScenarioTypeList = new List<KeyValuePair<ScenarioTypes, string>>()
            {
                new KeyValuePair<ScenarioTypes, string>(ScenarioTypes.PMS, "PMS - Map list"),
                new KeyValuePair<ScenarioTypes, string>(ScenarioTypes.USER, "USER - Map list"),
            };
            _ScenarioTypeListView = new ListCollectionView(_ScenarioTypeList);
            _ScenarioTypeListView.CurrentChanging += OnScenarioTypeChanging;
            _ScenarioTypeListView.CurrentChanged += OnScenarioTypeChanged;

            _loadListTask = new AsyncTask<string, List<ScenarioModel>>(
                (task, inputs) =>
                {
                    _currentScenarioInfoLoadTask?.cancel();
                },
                (task, inputs) =>
                {
                    if (inputs.Length == 1)
                    {
                        List<ScenarioModel> result = CoPlatformWebClient.Instance.GetScenarioList(inputs[0]);

                        return task.Status == AsyncTaskStatus.RUNNING ? result : null;
                    }
                    return null;
                },
                (task, result, inputs) =>
                {
                    if (result != null)
                    {
                        try
                        {
                            lock (lockItemListView)
                            {
                                ScenarioItemViewModel addingItem = null;
                                ScenarioItemViewModel edittingItem = null;
                                if (ScenarioListView.IsAddingNew)
                                {
                                    addingItem = ScenarioListView.CurrentAddItem as ScenarioItemViewModel;
                                    ScenarioListView.CancelNew();
                                }
                                else if (ScenarioListView.IsEditingItem)
                                {
                                    edittingItem = ScenarioListView.CurrentEditItem as ScenarioItemViewModel;
                                    ScenarioListView.CancelEdit();

                                    EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesOpenEvent>>().Publish(
                                        new ScenarioPropertiesOpenEvent(null, null, ScenarioType));
                                }
                                _ScenarioList.Clear();
                                _ScenarioList.AddRange(result.ConvertAll<ScenarioItemViewModel>(model => new ScenarioItemViewModel(model)));
                                ScenarioListView.Refresh();
                            }

                            if(_IsRefresh == true)
                            {
                                SelectedIndexScenario = _ScenarioList.Count - 1;
                                _IsRefresh = false;
                            }
                        }
                        catch
                        {
                            MessageBox.Show(Res.MsgFailedLoadData);
                        }
                    }
                });
            _swapScenarioTask = new AsyncTask<ScenarioItemViewModel, bool>(
                null,
                (task, inputs) =>
                {
                    if (inputs.Length == 2)
                    {
                        return CoPlatformWebClient.Instance.SwapScenarioPosition(inputs[0].Scenario.ScenarioCode, inputs[1].Scenario.ScenarioCode);
                    }
                    return false;
                },
                (task, result, inputs) =>
                {
                    if (result)
                    {
                        lock (lockItemListView)
                        {
                            object edittingItem = ScenarioListView.CurrentEditItem;
                            int firstOneIndex = _ScenarioList.IndexOf(inputs[0]);
                            int secondOneIndex = _ScenarioList.IndexOf(inputs[1]);

                            if (firstOneIndex >= 0 && secondOneIndex >= 0)
                            {
                                ScenarioListView.CancelEdit();

                                _ScenarioList[firstOneIndex] = inputs[1];
                                _ScenarioList[secondOneIndex] = inputs[0];
                                ScenarioListView.Refresh();

                                if (edittingItem != null)
                                {
                                    ScenarioListView.EditItem(edittingItem);
                                }
                            }
                        }
                    }
                });
            _deleteScenarioTask = new AsyncTask<ScenarioItemViewModel, bool>(
                null,
                (task, inputs) =>
                {
                    if (inputs.Length == 1)
                    {
                        return CoPlatformWebClient.Instance.RemoveScenario(inputs[0].Scenario.ScenarioCode);
                    }
                    return false;
                },
                (task, result, inputs) =>
                {
                    if (result)
                    {
                        lock (lockItemListView)
                        {
                            object edittingItem = ScenarioListView.CurrentEditItem;

                            ScenarioListView.CancelEdit();
                            ScenarioListView.Remove(inputs[0]);
                            if (edittingItem != null)
                            {
                                ScenarioListView.EditItem(edittingItem);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(Res.MsgFailedDeleteReq);
                    }
                    _beingDeletingScenario = null;
                }, null,
                (task, ex, inputs) =>
                {
                    _beingDeletingScenario = null;
                });

            _loadListTask.execute();
        }
        #endregion

        #region EventHandlers
        public void OnScenarioTypeChanging(object sender, CurrentChangingEventArgs e)
        {
            if (ScenarioListView.IsEditingItem)
            {
                e.Cancel = true;

                MessageBox.Show(Res.MsgCantChangeTypeWhileEdittingScenario);
                return;
            }

            _ScenarioList.Clear();
            ScenarioListView.Refresh();
            FilterText = "";
        }

        public void OnScenarioTypeChanged(object sender, EventArgs e)
        {
            try
            {
                _loadListTask.execute(ScenarioType.ToString());
            }
            catch { }
        }
        #endregion

        #region Commands
        public DelegateCommand<System.Windows.Input.MouseButtonEventArgs> ScenarioDoubleClickCommand => new DelegateCommand<System.Windows.Input.MouseButtonEventArgs>(ScenarioDoubleClickCommandExecute);

        private void ScenarioDoubleClickCommandExecute(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsAddItem == true)
            {
                object editItem = ScenarioListView.CurrentEditItem;

                ScenarioListView.CancelEdit();
                ScenarioListView.Remove(editItem);

                IsAddItem = false;
            }

            object dataContext = (e.Source as FrameworkElement)?.DataContext;

            if (dataContext is ScenarioItemViewModel && !dataContext.Equals(_beingDeletingScenario))
            {
                _currentScenarioInfoLoadTask?.cancel();
                ScenarioListView.CancelEdit();

                var selectedItem = dataContext as ScenarioItemViewModel;
                lock (lockItemListView)
                {
                    var edittingItem = ScenarioListView.CurrentEditItem as ScenarioItemViewModel;

                    if (edittingItem == dataContext || selectedItem?.Scenario?.ScenarioCode == null)
                    {
                        return;
                    }
                }

                _currentScenarioInfoLoadTask = new AsyncTask<object, ScenarioModel>(
                    null,
                    (task, inputs) =>
                    {
                        ScenarioModel result = CoPlatformWebClient.Instance.GetScenarioInfo(selectedItem.Scenario.ScenarioCode);
                        return task.Status == AsyncTaskStatus.RUNNING ? result : null;
                    },
                    (task, result, inputs) =>
                    {
                        if (result != null)
                        {
                            selectedItem.Scenario = result;
                            lock (lockItemListView)
                            {
                                var edittingItem = ScenarioListView.CurrentEditItem as ScenarioItemViewModel;
                                ScenarioListView.CancelEdit();
                                if (edittingItem?.Scenario == null)
                                {
                                    ScenarioListView.Remove(edittingItem);
                                }
                                ScenarioListView.EditItem(selectedItem);
                            }
                            IsEnableChangeScenarioType = false;

                            EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesOpenEvent>>().Publish(
                                new ScenarioPropertiesOpenEvent(selectedItem, selectedItem.Scenario, ScenarioType));
                        }
                        else
                        {
                            MessageBox.Show(Res.MsgFailedLoadData);
                            return;
                        }
                    }, null,
                    (task, ex, inputs) =>
                    {
                        MessageBox.Show(Res.MsgFailedLoadData);
                    });
                _currentScenarioInfoLoadTask.execute();
            }
        }

        public DelegateCommand AddScenarioCommand => new DelegateCommand(AddScenarioCommandExecute);

        private void AddScenarioCommandExecute()
        {
            lock (lockItemListView)
            {
                var currentEditItem = ScenarioListView.CurrentEditItem as ScenarioItemViewModel;

                if (currentEditItem != null && currentEditItem.Scenario?.ScenarioCode == null)
                {
                    return;
                }

                ScenarioListView.CancelEdit();

                var newItem = new ScenarioItemViewModel(null);

                ScenarioListView.AddNewItem(newItem);
                ScenarioListView.CommitNew();
                ScenarioListView.EditItem(newItem);
                IsEnableChangeScenarioType = false;

                EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesOpenEvent>>().Publish(
                    new ScenarioPropertiesOpenEvent(newItem, null, ScenarioType));

                IsAddItem = true;
            }
        }

        public DelegateCommand RefreshCommand => new DelegateCommand(RefreshCommandExecute);

        private void RefreshCommandExecute()
        {
            if (!ScenarioListView.IsEditingItem || MessageBox.Show(Res.MsgWarningRefresh, Res.StrRefresh,
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    _loadListTask.execute(ScenarioType.ToString());
                }
                catch { }
            }
        }

        public DelegateCommand MoveUpScenarioCommand => new DelegateCommand(MoveUpScenarioCommandExecute);

        private void MoveUpScenarioCommandExecute()
        {
            var currentItem = ScenarioListView.CurrentItem as ScenarioItemViewModel;

            if (currentItem != null)
            {
                int currentIndex = _ScenarioList.IndexOf(currentItem);

                if (currentIndex > 0)
                {
                    var prevItem = _ScenarioList[currentIndex - 1];

                    try
                    {
                        _swapScenarioTask.execute(currentItem, prevItem);
                    }
                    catch { }
                }
            }
        }

        public DelegateCommand MoveDownScenarioCommand => new DelegateCommand(MoveDownScenarioCommandExecute);

        private void MoveDownScenarioCommandExecute()
        {
            var currentItem = ScenarioListView.CurrentItem as ScenarioItemViewModel;

            if (currentItem != null)
            {
                int currentIndex = _ScenarioList.IndexOf(currentItem);

                if (currentIndex < _ScenarioList.Count - 1)
                {
                    var nextItem = _ScenarioList[currentIndex + 1];

                    try
                    {
                        _swapScenarioTask.execute(currentItem, nextItem);
                    }
                    catch { }
                }
            }
        }

        public DelegateCommand DeleteScenarioCommand => new DelegateCommand(DeleteScenarioCommandExecute);

        private void DeleteScenarioCommandExecute()
        {
            if (ScenarioListView.CurrentItem == null)
            {
                return;
            }

            if (MessageBox.Show(Res.MsgIsOkRemoveItem, Res.StrRemoveItem,
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                lock (lockItemListView)
                {
                    object edittingItem = ScenarioListView.CurrentEditItem;

                    if (edittingItem != null)
                    {
                        if (edittingItem == ScenarioListView.CurrentItem)
                        {
                            MessageBox.Show(Res.MsgCantDeleteEditingItem);
                            return;
                        }
                    }

                    _beingDeletingScenario = (ScenarioListView.CurrentItem as ScenarioItemViewModel);
                    try
                    {
                        _deleteScenarioTask.execute(_beingDeletingScenario);
                    }
                    catch
                    {
                        _beingDeletingScenario = null;
                    }
                }
            }
        }

        public DelegateCommand CopyScenarioCommand => new DelegateCommand(CopyScenarioCommandExecute);

        private void CopyScenarioCommandExecute()
        {
            if (SelectedScenario.Scenario == null)
            {
                MessageBox.Show(Res.StrSelectedItem, Res.StrCaution, MessageBoxButton.OK);
                return;
            }

            //DB 열에 대한 데이터 길이가 50이여서 맞춰야 된다.
            if ((SelectedScenario.Scenario.ScenarioName + Res.StrCopy).Length > 50)
            {
                MessageBox.Show(Res.MsgLimitCharacters, Res.StrCaution, MessageBoxButton.OK);
                return;
            }

            var newScenarioModel = SelectedScenario.Scenario as ScenarioModel;

            if (newScenarioModel == null)
            {
                return;
            }

            newScenarioModel.ScenarioName = newScenarioModel.ScenarioName + Res.StrCopy;
            newScenarioModel.ScenarioCode = 0;
            newScenarioModel.ScenarioSort = ScenarioListView.Count + 1;

            bool isSuccess = false;

            var result = CoPlatformWebClient.Instance.AddScenario(newScenarioModel);
            if ((isSuccess = result != null))
            {
                newScenarioModel = result;
            }

            if (isSuccess)
            {
                ScenarioListView.CancelEdit();

                var newItem = new ScenarioItemViewModel(null);
                newItem.Scenario = newScenarioModel;

                ScenarioListView.AddNewItem(newItem);
                ScenarioListView.CommitNew();
                RefreshCommandExecute();

                SelectedScenario = newItem;

                _IsRefresh = true;
                return;
            }
            else
            {
                MessageBox.Show(Res.MsgFailedSaveReq);
                return;
            }
        }

        private void OnScenarioPropertiesSave(ScenarioPropertiesSaveEvent e)
        {
            lock (lockItemListView)
            {
                if (e.Token is ScenarioItemViewModel && e.Token == ScenarioListView.CurrentEditItem)
                {
                    (e.Token as ScenarioItemViewModel).Scenario = e.Scenario;

                    ScenarioListView.CommitEdit();

                    if (_ScenarioList.FindAll(item => item?.Scenario?.ScenarioCode == e.Scenario?.ScenarioCode).Count > 1)
                    {
                        ScenarioListView.Remove(e.Token);
                    }
                }
            }
            IsEnableChangeScenarioType = true;
        }

        private void OnScenarioPropertiesClose(ScenarioPropertiesCloseEvent e)
        {
            lock (lockItemListView)
            {
                var currentEditItem = ScenarioListView.CurrentEditItem as ScenarioItemViewModel;

                if (currentEditItem != null)
                {
                    ScenarioListView.CancelEdit();

                    if (currentEditItem.Scenario?.ScenarioCode == null)
                    {
                        ScenarioListView.Remove(currentEditItem);
                        ScenarioListView.Refresh();
                    }
                }
            }
            IsEnableChangeScenarioType = true;
        }
        #endregion
    }
}
