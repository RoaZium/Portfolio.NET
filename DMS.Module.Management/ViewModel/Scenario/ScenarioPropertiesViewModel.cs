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
using System.Windows;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class ScenarioPropertiesViewModel : DMViewModelBase
    {
        #region Properties
        private readonly object _Token;
        internal object Token => _Token;

        private int? _ScenarioId;

        private int _ScenarioSort;

        private readonly ScenarioTypes _ScenarioType;

        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesSaveEvent>>().Unsubscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesCloseEvent>>().Unsubscribe(OnActionPropertiesClose);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesSaveEvent>>().Subscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesCloseEvent>>().Subscribe(OnActionPropertiesClose);
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
        #endregion

        #region Constructor
        public ScenarioPropertiesViewModel(object token, ScenarioModel scenario, ScenarioTypes scenarioType)
        {
            _Token = token;

            _ScenarioType = scenarioType;
            _ScenarioId = scenario?.ScenarioCode;
            _ScenarioSort = scenario?.ScenarioSort ?? 0;
            _ActionList = scenario?.ScenarioActions == null ? new List<ActionItemViewModel>() :
                new List<ActionItemViewModel>(
                    scenario.ScenarioActions.ConvertAll<ActionItemViewModel>(
                        model => new ActionItemViewModel(model)
                    )
                );
            _ActionListView = new ListCollectionView(_ActionList);

            Name = scenario?.ScenarioName;
        }
        #endregion

        #region Methods
        private ScenarioModel GenScenarioModelFromCurrentSetup()
        {
            if (ActionListView.IsEditingItem)
            {
                MessageBox.Show(Res.MsgCantSaveScenarioWhileEdittingAction);
                return null;
            }

            List<IActionModel> newActions = _ActionList.ConvertAll<IActionModel>(packer => packer.ActionModel);
            newActions.RemoveAll(item => item == null);

            if(string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
            {
                Name = Res.StrNoname;
            }

            return new ScenarioModel
            {
                ScenarioCode = _ScenarioId ?? 0,
                ScenarioSort = _ScenarioSort,
                ScenarioName = Name,
                ScenarioType = _ScenarioType,
                ScenarioActions = newActions,
            };
        }
        #endregion

        #region Commands
        public DelegateCommand CloseCommand => new DelegateCommand(CloseCommandExecute);

        private void CloseCommandExecute()
        {
            EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesCloseEvent>>().Publish(
                new ScenarioPropertiesCloseEvent(_Token));
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
            if(_ScenarioId == null)
            {
                var result = CoPlatformWebClient.Instance.AddScenario(newScenarioModel);
                if ((isSuccess = result != null))
                {
                    newScenarioModel = result;
                }
            }
            else
            {
                isSuccess = CoPlatformWebClient.Instance.SaveScenarioProperties(newScenarioModel);
            }

            if(isSuccess)
            {
                EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesSaveEvent>>().Publish(
                    new ScenarioPropertiesSaveEvent(_Token, newScenarioModel));
                return;
            }
            else
            {
                MessageBox.Show(Res.MsgFailedSaveReq);
                return;
            }
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

            EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesOpenEvent>>().Publish(
                new ActionPropertiesOpenEvent(newItem, newItem.ActionModel, _ScenarioType));
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

                EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesOpenEvent>>().Publish(
                    new ActionPropertiesOpenEvent(editItem, editItem.ActionModel, _ScenarioType));
            }
        }

        private void OnActionPropertiesClose(ActionPropertiesCloseEvent e)
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

        private void OnActionPropertiesSave(ActionPropertiesSaveEvent e)
        {
            var currentEditItem = ActionListView.CurrentEditItem as ActionItemViewModel;
            if (currentEditItem != null && currentEditItem == e.Token)
            {
                currentEditItem.ActionModel = e.Action;
                ActionListView.CommitEdit();
                ActionListView.Refresh();
            }
        }
        #endregion
    }
}
