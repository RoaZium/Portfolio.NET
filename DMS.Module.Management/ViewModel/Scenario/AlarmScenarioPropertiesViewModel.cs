using Coever.Lib.CoPlatform.Core.Models;
using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Management.Events;
using DMS.Module.Management.Model;
using DMS.Module.Management.Network;
using DMS.Module.Management.Properties;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.MVVM;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class AlarmScenarioPropertiesViewModel : DMViewModelBase
    {
        #region Properties
        private readonly object _Token;
        internal object Token => _Token;

        private int? _ScenarioId;

        private int _ScenarioSort;

        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesSaveEvent>>().Unsubscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesCloseEvent>>().Unsubscribe(OnActionPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListSaveEvent>>().Unsubscribe(OnTargetListSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListCloseEvent>>().Unsubscribe(OnTargetListClose);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesSaveEvent>>().Subscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesCloseEvent>>().Subscribe(OnActionPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListSaveEvent>>().Subscribe(OnTargetListSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListCloseEvent>>().Subscribe(OnTargetListClose);
                }
            }
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private readonly List<ActionItemViewModel> _ActionList;
        private readonly ListCollectionView _ActionListView;
        public ListCollectionView ActionListView => _ActionListView;

        private readonly List<AlarmScenarioTargetItemViewModel> _TargetList;
        private readonly ListCollectionView _TargetListView;
        public ListCollectionView TargetListView => _TargetListView;

        private bool _IsSettingUpTargetList = false;
        #endregion

        #region Constructor
        public AlarmScenarioPropertiesViewModel(object token, AlarmScenarioModel scenario)
        {
            _Token = token;

            _ScenarioId = scenario?.ScenarioCode;
            _ScenarioSort = scenario?.ScenarioSort ?? 0;
            _ActionList = scenario?.ScenarioActions == null ? new List<ActionItemViewModel>() :
                new List<ActionItemViewModel>(
                    scenario.ScenarioActions.ConvertAll<ActionItemViewModel>(
                        model => new ActionItemViewModel(model)
                    )
                );
            _ActionListView = new ListCollectionView(_ActionList);
            List<TargetModel> targetList = CoPlatformWebClient.Instance.GetAlarmTargetList().ToList();
            _TargetList = scenario?.DmAlarmScenarioMapping?.ToList().ConvertAll(item => new AlarmScenarioTargetItemViewModel(item)
            {
                Name = targetList.Find(x => x.TargetCode == item.TargetCode)?.TargetName
            }) ?? new List<AlarmScenarioTargetItemViewModel>();
            _TargetListView = new ListCollectionView(_TargetList);

            Name = scenario?.ScenarioName;
        }
        #endregion

        #region Methods
        private AlarmScenarioModel GenScenarioModelFromCurrentSetup()
        {
            if (ActionListView.IsEditingItem)
            {
                MessageBox.Show(Res.MsgCantSaveScenarioWhileEdittingAction);
                return null;
            }
            if (_IsSettingUpTargetList)
            {
                MessageBox.Show(Res.MsgCantSaveScenarioWhileEdittingSensorList);
                return null;
            }

            List<IActionModel> newActions = _ActionList.ConvertAll<IActionModel>(packer => packer.ActionModel);
            newActions.RemoveAll(item => item == null);

            if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                Name = Res.StrNoname;
            }

            return new AlarmScenarioModel
            {
                ScenarioCode = _ScenarioId ?? 0,
                ScenarioSort = _ScenarioSort,
                ScenarioName = Name,
                ScenarioActions = newActions,
                DmAlarmScenarioMapping = _TargetList.ConvertAll(item => item.Target)
            };
        }
        #endregion

        #region Commands
        public DelegateCommand CloseCommand => new DelegateCommand(CloseCommandExecute);

        private void CloseCommandExecute()
        {
            EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesCloseEvent>>().Publish(
                new AlarmScenarioPropertiesCloseEvent(_Token));
        }

        public DelegateCommand SaveCommand => new DelegateCommand(SaveCommandExecute);

        private void SaveCommandExecute()
        {
            var newScenarioModel = GenScenarioModelFromCurrentSetup();

            if (newScenarioModel == null)
            {
                return;
            }

            bool isSuccess = false;
            if (_ScenarioId == null)
            {
                var result = CoPlatformWebClient.Instance.AddAlarmScenario(newScenarioModel);
                if ((isSuccess = result != null))
                {
                    newScenarioModel = result;
                }
            }
            else
            {
                CoPlatformWebClient.Instance.RemoveAlarmScenario(newScenarioModel.ScenarioCode);
                var result = CoPlatformWebClient.Instance.AddAlarmScenario(newScenarioModel);
                if ((isSuccess = result != null))
                {
                    newScenarioModel = result;
                }
            }

            if (isSuccess)
            {
                EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesSaveEvent>>().Publish(
                    new AlarmScenarioPropertiesSaveEvent(_Token, newScenarioModel));
                return;
            }
            else
            {
                MessageBox.Show(Res.MsgFailedSaveReq);
                return;
            }
        }

        public DelegateCommand SetTargetListCommand => new DelegateCommand(SetTargetListCommandExecute);

        private void SetTargetListCommandExecute()
        {
            _IsSettingUpTargetList = true;
            EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListOpenEvent>>().Publish(
                new AlarmScenarioTargetListOpenEvent(Token, _ScenarioId, _TargetList.ConvertAll(item => item.Target)));
        }

        public DelegateCommand AddActionCommand => new DelegateCommand(AddActionCommandExecute);

        private void AddActionCommandExecute()
        {
            var currentEditItem = ActionListView.CurrentEditItem as ActionItemViewModel;

            if (currentEditItem != null && currentEditItem.ActionModel == null)
            {
                return;
            }

            var newItem = new ActionItemViewModel();

            ActionListView.AddNewItem(newItem);
            ActionListView.CommitNew();
            ActionListView.EditItem(newItem);

            EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesOpenEvent>>().Publish(
                new AlarmActionPropertiesOpenEvent(newItem, newItem.ActionModel));
        }

        public DelegateCommand MoveUpScenarioCommand => new DelegateCommand(MoveUpScenarioCommandExecute);

        private void MoveUpScenarioCommandExecute()
        {
            var currentItem = ActionListView.CurrentItem as ActionItemViewModel;

            if (currentItem != null)
            {
                int currentIndex = _ActionList.IndexOf(currentItem);

                if (currentIndex > 0)
                {
                    object edittingItem = ActionListView.CurrentEditItem;
                    if (edittingItem != null)
                    {
                        ActionListView.CancelEdit();
                        _ActionList.Remove(currentItem);
                        _ActionList.Insert(currentIndex - 1, currentItem);
                        ActionListView.Refresh();
                        ActionListView.EditItem(edittingItem);
                    }
                    else
                    {
                        _ActionList.Remove(currentItem);
                        _ActionList.Insert(currentIndex - 1, currentItem);
                        ActionListView.Refresh();
                    }
                }
            }
        }

        public DelegateCommand MoveDownScenarioCommand => new DelegateCommand(MoveDownScenarioCommandExecute);

        private void MoveDownScenarioCommandExecute()
        {
            var currentItem = ActionListView.CurrentItem as ActionItemViewModel;

            if (currentItem != null)
            {
                int currentIndex = _ActionList.IndexOf(currentItem);

                if (currentIndex < _ActionList.Count - 1)
                {
                    object edittingItem = ActionListView.CurrentEditItem;
                    if (edittingItem != null)
                    {
                        ActionListView.CancelEdit();
                        _ActionList.Remove(currentItem);
                        _ActionList.Insert(currentIndex + 1, currentItem);
                        ActionListView.Refresh();
                        ActionListView.EditItem(edittingItem);
                    }
                    else
                    {
                        _ActionList.Remove(currentItem);
                        _ActionList.Insert(currentIndex + 1, currentItem);
                        ActionListView.Refresh();
                    }
                }
            }
        }

        public DelegateCommand DeleteActionCommand => new DelegateCommand(DeleteActionCommandExecute);

        private void DeleteActionCommandExecute()
        {
            if (ActionListView.CurrentItem == null)
            {
                return;
            }

            object edittingItem = ActionListView.CurrentEditItem;

            if (edittingItem != null)
            {
                if (edittingItem == ActionListView.CurrentItem)
                {
                    MessageBox.Show(Res.MsgCantDeleteEditingItem);
                    return;
                }

                ActionListView.CancelEdit();
                ActionListView.Remove(ActionListView.CurrentItem);
                ActionListView.EditItem(edittingItem);
            }
            else
            {
                ActionListView.Remove(ActionListView.CurrentItem);
            }
        }

        public DelegateCommand<System.Windows.Input.MouseButtonEventArgs> ActionDoubleClickCommand => new DelegateCommand<System.Windows.Input.MouseButtonEventArgs>(ActionDoubleClickCommandExecute);

        private void ActionDoubleClickCommandExecute(System.Windows.Input.MouseButtonEventArgs e)
        {
            var currentEditItem = ActionListView.CurrentEditItem as ActionItemViewModel;

            if (currentEditItem != null && currentEditItem.ActionModel == null)
            {
                MessageBox.Show(Res.MsgPleaseCompleateNewAction);
                return;
            }

            object dataContext = (e.Source as FrameworkElement)?.DataContext;

            if (dataContext != null && dataContext is ActionItemViewModel)
            {
                if (currentEditItem != null)
                {
                    ActionListView.CancelEdit();
                }

                var editItem = dataContext as ActionItemViewModel;

                ActionListView.EditItem(editItem);

                EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesOpenEvent>>().Publish(
                    new AlarmActionPropertiesOpenEvent(editItem, editItem.ActionModel));
            }
        }

        private void OnActionPropertiesClose(AlarmActionPropertiesCloseEvent e)
        {
            var currentEditItem = ActionListView.CurrentEditItem as ActionItemViewModel;

            if (currentEditItem != null)
            {
                ActionListView.CancelEdit();

                if (currentEditItem.ActionModel == null)
                {
                    ActionListView.Remove(currentEditItem);
                }
            }
        }

        private void OnActionPropertiesSave(AlarmActionPropertiesSaveEvent e)
        {
            var currentEditItem = ActionListView.CurrentEditItem as ActionItemViewModel;
            if (currentEditItem != null && currentEditItem == e.Token)
            {
                currentEditItem.ActionModel = e.Action;
                ActionListView.CommitEdit();
                ActionListView.Refresh();
            }
        }

        private void OnTargetListSave(AlarmScenarioTargetListSaveEvent e)
        {
            if (Token == e.Token)
            {
                _TargetList.Clear();
                _TargetList.AddRange(e.TargetList);
                TargetListView.Refresh();

                _IsSettingUpTargetList = false;
            }
        }

        private void OnTargetListClose(AlarmScenarioTargetListCloseEvent e)
        {
            if (Token == e.Token)
            {
                _IsSettingUpTargetList = false;
            }
        }
        #endregion
    }
}
