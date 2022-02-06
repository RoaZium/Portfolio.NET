using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Management.Events;
using DMS.Module.Management.Model;
using DMS.Module.Management.Network;
using DMS.Module.Management.Properties;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class AlarmActionPropertiesViewModel : DMViewModelBase
    {
        #region Properties
        private readonly object _Token;
        internal object Token => _Token;

        private readonly IActionModel _Action;
        internal IActionModel Action => _Action;

        private readonly List<KeyValuePair<ActionTypes, string>> _ActionTypeList;
        private readonly ListCollectionView _ActionTypeListView;
        public ListCollectionView ActionTypeListView => _ActionTypeListView;

        #region layout action
        private readonly List<SelectionModel> _layout_LayoutList;
        private readonly ListCollectionView _layout_LayoutListView;
        public ListCollectionView layout_LayoutListView => _layout_LayoutListView;
        #endregion

        #region ipcam_compo action
        private readonly List<SelectionModel> _ipcam_compo_CompoList;
        private readonly ListCollectionView _ipcam_compo_CompoListView;
        public ListCollectionView ipcam_compo_CompoListView => _ipcam_compo_CompoListView;

        private readonly List<SelectionModel> _ipcam_compo_AreaList;
        private readonly ListCollectionView _ipcam_compo_AreaListView;
        public ListCollectionView ipcam_compo_AreaListView => _ipcam_compo_AreaListView;
        #endregion

        private int _DelaySecond;
        [Range(3, 3600, ErrorMessage = "Please enter a number between 3 and 3600.")]
        public int DelaySecond
        {
            get => _DelaySecond;
            set
            {
                _DelaySecond = value;
                RaisePropertyChanged("DelaySecond");
            }
        }
        #endregion

        #region Constructor
        public AlarmActionPropertiesViewModel(object token, IActionModel action)
        {
            _Token = token;
            _Action = action ?? new AlarmLayoutActionModel();

            _ActionTypeList = new List<KeyValuePair<ActionTypes, string>>()
            {
                new KeyValuePair<ActionTypes, string>(ActionTypes.alarm_layout, Res.StrActionTypeAlarmLayout),
                new KeyValuePair<ActionTypes, string>(ActionTypes.layout, Res.StrActionTypeLayout),
                new KeyValuePair<ActionTypes, string>(ActionTypes.ipcam_compo, Res.StrActionTypeIPCamComponent),
            };
            _ActionTypeListView = new ListCollectionView(_ActionTypeList);

            _layout_LayoutList = new List<SelectionModel>();
            _layout_LayoutList.AddRange(
                    CoPlatformWebClient.Instance.GetMESLayoutSelectionList());
            _layout_LayoutListView = new ListCollectionView(_layout_LayoutList);

            _ipcam_compo_CompoList = new List<SelectionModel>();
            _ipcam_compo_CompoList.AddRange(
                    CoPlatformWebClient.Instance.GetIPCamCompoSelectionList("PMS"));
            _ipcam_compo_CompoListView = new ListCollectionView(_ipcam_compo_CompoList);
            _ipcam_compo_CompoListView.CurrentChanged += OnIPCamCompoChanged;

            _ipcam_compo_AreaList = new List<SelectionModel>();
            _ipcam_compo_AreaListView = new ListCollectionView(_ipcam_compo_AreaList);

            InitMembers();
        }
        #endregion

        #region Methods
        private void InitMembers()
        {
            try
            {
                DelaySecond = Action.DelaySecond ?? 3;

                switch (Action.ActionType)
                {
                    case ActionTypes.layout:
                        {
                            ActionTypeListView.MoveCurrentTo(_ActionTypeList.Find(item => item.Key == Action.ActionType));

                            var action = Action as LayoutActionModel;
                            int idx = _layout_LayoutList.FindIndex(item => item.Key.ToString() == action.LayoutID);
                            if (idx >= 0)
                            {
                                layout_LayoutListView.MoveCurrentToPosition(idx);
                            }
                            break;
                        }
                    case ActionTypes.alarm_layout:
                        {
                            ActionTypeListView.MoveCurrentTo(_ActionTypeList.Find(item => item.Key == Action.ActionType));
                            break;
                        }
                    case ActionTypes.ipcam_compo:
                        {
                            ActionTypeListView.MoveCurrentTo(_ActionTypeList.Find(item => item.Key == Action.ActionType));

                            var action = Action as IPCamCompoActionModel;
                            int idx = _ipcam_compo_CompoList.FindIndex(item => item.Key.ToString() == action.ComponentID);
                            if (idx >= 0)
                            {
                                ipcam_compo_CompoListView.MoveCurrentToPosition(idx);
                            }
                            break;
                        }
                    default:
                        {
                            ActionTypeListView.MoveCurrentToFirst();
                            break;
                        }
                }
            }
            catch { }
        }
        #endregion

        #region Commands / Events
        public DelegateCommand CloseCommand => new DelegateCommand(CloseCommandExecute);

        private void CloseCommandExecute()
        {
            EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesCloseEvent>>().Publish(
                new AlarmActionPropertiesCloseEvent(Token));
        }

        public DelegateCommand SaveCommand => new DelegateCommand(SaveCommandExecute);

        private void SaveCommandExecute()
        {
            if (DelaySecond < 3 || DelaySecond > 3600)
            {
                MessageBox.Show(Res.MsgRangeNumError, Res.StrCaution, MessageBoxButton.OK);
                return;
            }

            if (HasErrors)
            {
                MessageBox.Show(GetErrors().First().Value.First());
                return;
            }

            KeyValuePair<ActionTypes, string>? selectedType
                    = ActionTypeListView.CurrentItem as KeyValuePair<ActionTypes, string>?;
            if (selectedType == null)
            {
                MessageBox.Show(Res.MsgPleaseSelectActionsType);
                return;
            }

            IActionModel newAction = null;
            switch (selectedType.Value.Key)
            {
                case ActionTypes.alarm_layout:
                    {
                        newAction = new AlarmLayoutActionModel()
                        {
                            DelaySecond = DelaySecond,
                            DisplayName = Res.StrActionNameAlarmLayout
                        };
                        break;
                    }
                case ActionTypes.layout:
                    {
                        if (layout_LayoutListView.CurrentItem == null)
                        {
                            MessageBox.Show(Res.MsgPleaseSelectLayout);
                            return;
                        }

                        SelectionModel currentItem = (SelectionModel)layout_LayoutListView.CurrentItem;
                        newAction = new LayoutActionModel()
                        {
                            LayoutID = currentItem.Key.ToString(),
                            DelaySecond = DelaySecond,
                            DisplayName = currentItem.DisplayName
                        };
                        break;
                    }
                case ActionTypes.ipcam_compo:
                    {
                        if (ipcam_compo_CompoListView.CurrentItem == null)
                        {
                            MessageBox.Show(Res.MsgPleaseSelectIPCamComponent);
                            return;
                        }
                        if (ipcam_compo_AreaListView.CurrentItem == null)
                        {
                            MessageBox.Show(Res.MsgPleaseSelectArea);
                            return;
                        }

                        CompSelectionModel compoSelection = (CompSelectionModel)ipcam_compo_CompoListView.CurrentItem;
                        SelectionModel areaSelection = (SelectionModel)ipcam_compo_AreaListView.CurrentItem;

                        newAction = new IPCamCompoActionModel()
                        {
                            ComponentID = compoSelection.Key.ToString(),
                            AreaName = areaSelection.Key?.ToString(),
                            DelaySecond = DelaySecond,
                            DisplayName = compoSelection.DisplayName + "(" + (areaSelection.Key == null ? areaSelection.DisplayName : areaSelection.Key?.ToString()) + ")"
                        };
                        break;
                    }
                default:
                    {
                        MessageBox.Show(Res.MsgPleaseSelectActionsType);
                        return;
                    }
            }
            if (newAction == null)
            {
                MessageBox.Show(Res.MsgIncorrectInput);
            }

            EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesSaveEvent>>().Publish(
                new AlarmActionPropertiesSaveEvent(Token, newAction));
        }

        public DelegateCommand<TextChangedEventArgs> layout_FilterTextChangedCommand => new DelegateCommand<TextChangedEventArgs>(layout_FilterTextChangedCommandExecute);

        private void layout_FilterTextChangedCommandExecute(TextChangedEventArgs e)
        {
            string filter = (e.Source as TextBox).Text;

            if (string.IsNullOrEmpty(filter))
            {
                layout_LayoutListView.Filter = null;
            }
            else
            {
                layout_LayoutListView.Filter = new Predicate<object>((selection) =>
                {
                    return ((SelectionModel)selection).DisplayName.Contains(filter);
                });
            }
        }

        public DelegateCommand<TextChangedEventArgs> ipcam_compo_FilterTextChangedCommand => new DelegateCommand<TextChangedEventArgs>(ipcam_compo_FilterTextChangedCommandExecute);

        private void ipcam_compo_FilterTextChangedCommandExecute(TextChangedEventArgs e)
        {
            string filter = (e.Source as TextBox).Text;

            if (string.IsNullOrEmpty(filter))
            {
                ipcam_compo_CompoListView.Filter = null;
            }
            else
            {
                ipcam_compo_CompoListView.Filter = new Predicate<object>((selection) =>
                {
                    return ((SelectionModel)selection).DisplayName.Contains(filter);
                });
            }
        }

        public void OnIPCamCompoChanged(object sender, EventArgs e)
        {

            if (ipcam_compo_CompoListView.CurrentItem is SelectionModel currentIPCam)
            {
                string oldSelected = null;
                if (ipcam_compo_AreaListView.CurrentItem != null)
                {
                    oldSelected = (ipcam_compo_AreaListView.CurrentItem as SelectionModel).Key?.ToString();
                }
                else if (Action != null && Action is IPCamCompoActionModel)
                {
                    IPCamCompoActionModel action = Action as IPCamCompoActionModel;

                    oldSelected = action.AreaName;
                }

                if(currentIPCam.Value == null)
                {
                    return;
                }

                var ipcams = CoPlatformWebClient.Instance.GetIPCamFull(((uint?)currentIPCam.Value).Value);
                _ipcam_compo_AreaList.Clear();
                _ipcam_compo_AreaList.Add(new SelectionModel()
                {
                    Key = null,
                    Value = "",
                    DisplayName = Res.StrAreaNameOfNull,
                });

                if (ipcams != null)
                {
                    _ipcam_compo_AreaList.AddRange(ipcams.Areas.ConvertAll(
                        item => new SelectionModel
                        {
                            Key = item.Name,
                            DisplayName = item.Name,
                            Value = item.Name
                        }
                    ));
                }

                ipcam_compo_AreaListView.Refresh();

                if (oldSelected != null)
                {
                    int idx = _ipcam_compo_AreaList.FindIndex(
                        item => item.Key != null && item.Key.Equals(oldSelected));
                    ipcam_compo_AreaListView.MoveCurrentToPosition(idx);
                    ipcam_compo_AreaListView.Refresh();
                }
                else
                {
                    ipcam_compo_AreaListView.MoveCurrentToFirst();
                    ipcam_compo_AreaListView.Refresh();
                }
            }
        }
        #endregion
    }
}
