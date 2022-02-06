using Coever.Lib.CoPlatform.Core.Models;
using Coever.Lib.CoPlatform.Core.Network;
using DMS.Module.Management.Events;
using DMS.Module.Management.Model;
using DMS.Module.Management.Network;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace DMS.Module.Management.ViewModel
{
    public class AlarmScenarioTargetListViewModel : DMViewModelBase
    {
        #region Properties
        private readonly object _Token;
        internal object Token => _Token;

        private readonly List<DmAlarmScenarioMapping> _OriginalCheckedTargetList;

        private readonly List<AlarmScenarioMappingCheckableItemViewModel> _TargetList;
        private readonly ListCollectionView _TargetListView;
        public ListCollectionView TargetListView => _TargetListView;

        private bool _IsCheckedAll = false;
        public bool IsCheckedAll
        {
            get => _IsCheckedAll;
            set
            {
                _IsCheckedAll = value;
                RaisePropertyChanged("IsCheckedAll");

            }
        }
        #endregion

        #region Constructor
        public AlarmScenarioTargetListViewModel(object token, int? scenarioCode, List<DmAlarmScenarioMapping> checkedTargetList)
        {
            _Token = token;

            _OriginalCheckedTargetList = checkedTargetList ?? new List<DmAlarmScenarioMapping>();

            _TargetList = new List<AlarmScenarioMappingCheckableItemViewModel>();
            _TargetListView = new ListCollectionView(_TargetList);

            IList<TargetModel> targetList = CoPlatformWebClient.Instance.GetAlarmTargetList();
            _TargetList.Clear();
            _TargetList.AddRange(targetList.ToList().ConvertAll<AlarmScenarioMappingCheckableItemViewModel>(
                target => new AlarmScenarioMappingCheckableItemViewModel()
                {
                    Mapping = new DmAlarmScenarioMapping
                    {
                        AlarmScenarioCode = scenarioCode,
                        TargetCode = target.TargetCode,
                    },
                    Name = target.TargetName,
                    IsChecked = _OriginalCheckedTargetList.Exists(checkedTarget => checkedTarget.TargetCode == target.TargetCode)
                }
            ));
            _TargetListView.Refresh();
            IsCheckedAll = !_TargetList.Exists(checkableTarget => !checkableTarget.IsChecked);
        }
        #endregion

        #region Commands / Events
        public DelegateCommand CloseCommand => new DelegateCommand(CloseCommandExecute);

        private void CloseCommandExecute()
        {
            EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListCloseEvent>>().Publish(
                new AlarmScenarioTargetListCloseEvent(_Token));
        }

        public DelegateCommand SaveCommand => new DelegateCommand(SaveCommandExecute);

        private void SaveCommandExecute()
        {
            List<AlarmScenarioTargetItemViewModel> newCheckedTargets = _TargetList.Where(target => target.IsChecked)
                .ToList().ConvertAll(target => new AlarmScenarioTargetItemViewModel(target.Mapping) { Name = target.Name });

            EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListSaveEvent>>().Publish(
                            new AlarmScenarioTargetListSaveEvent(_Token, newCheckedTargets));
        }

        public DelegateCommand<TextChangedEventArgs> FilterTextChangedCommand => new DelegateCommand<TextChangedEventArgs>(FilterTextChangedCommandExecute);

        private void FilterTextChangedCommandExecute(TextChangedEventArgs e)
        {
            string filter = (e.Source as TextBox).Text;

            if (string.IsNullOrEmpty(filter))
            {
                TargetListView.Filter = null;
            }
            else
            {
                TargetListView.Filter = new Predicate<object>((selection) =>
                {
                    return (selection as AlarmScenarioMappingCheckableItemViewModel).Name.Contains(filter);
                });
            }
        }

        public DelegateCommand AllCheckChangedCommand => new DelegateCommand(AllCheckChangedCommandExecute);

        private void AllCheckChangedCommandExecute()
        {
            _TargetList.ForEach(checkableTarget => checkableTarget.IsChecked = IsCheckedAll);
        }

        public DelegateCommand TargetCheckChangedCommand => new DelegateCommand(TargetCheckChangedCommandExecute);

        private void TargetCheckChangedCommandExecute()
        {
            IsCheckedAll = !_TargetList.Exists(checkableTarget => !checkableTarget.IsChecked);
        }
        #endregion
    }
}
