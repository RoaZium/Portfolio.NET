using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Management.Events;
using Prism.Events;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.ViewModel
{
    public class AlarmScenarioManagementViewModel : DMViewModelBase
    {
        #region Properties
        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesOpenEvent>>().Unsubscribe(OnScenarioPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesSaveEvent>>().Unsubscribe(OnScenarioPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesCloseEvent>>().Unsubscribe(OnScenarioPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesOpenEvent>>().Unsubscribe(OnActionPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesSaveEvent>>().Unsubscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesCloseEvent>>().Unsubscribe(OnActionPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListOpenEvent>>().Unsubscribe(OnRecordingTargetListOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListSaveEvent>>().Unsubscribe(OnRecordingTargetListSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListCloseEvent>>().Unsubscribe(OnRecordingTargetListClose);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesOpenEvent>>().Subscribe(OnScenarioPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesSaveEvent>>().Subscribe(OnScenarioPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioPropertiesCloseEvent>>().Subscribe(OnScenarioPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesOpenEvent>>().Subscribe(OnActionPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesSaveEvent>>().Subscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmActionPropertiesCloseEvent>>().Subscribe(OnActionPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListOpenEvent>>().Subscribe(OnRecordingTargetListOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListSaveEvent>>().Subscribe(OnRecordingTargetListSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<AlarmScenarioTargetListCloseEvent>>().Subscribe(OnRecordingTargetListClose);
                }
            }
        }

        private AlarmScenarioListViewModel _ScenarioListVM = new AlarmScenarioListViewModel();
        public AlarmScenarioListViewModel ScenarioListVM
        {
            get => _ScenarioListVM;
            set
            {
                _ScenarioListVM = value;
                RaisePropertyChanged("ScenarioListVM");
            }
        }

        private AlarmScenarioPropertiesViewModel _ScenarioPropVM;
        public AlarmScenarioPropertiesViewModel ScenarioPropVM
        {
            get => _ScenarioPropVM;
            set
            {
                _ScenarioPropVM = value;
                RaisePropertyChanged("ScenarioPropVM");
            }
        }

        private AlarmActionPropertiesViewModel _ActionPropVM;
        public AlarmActionPropertiesViewModel ActionPropVM
        {
            get => _ActionPropVM;
            set
            {
                _ActionPropVM = value;
                RaisePropertyChanged("ActionPropVM");
            }
        }

        private AlarmScenarioTargetListViewModel _TargetListVM;
        public AlarmScenarioTargetListViewModel TargetListVM
        {
            get => _TargetListVM;
            set
            {
                _TargetListVM = value;
                RaisePropertyChanged("TargetListVM");
            }
        }
        #endregion

        #region Commands / Events
        private void OnScenarioPropertiesOpen(AlarmScenarioPropertiesOpenEvent e)
        {
            if (ScenarioPropVM != null && e?.Scenario?.ScenarioCode != null
                && e.Token == ScenarioPropVM.Token)
            {
                return;
            }

            ActionPropVM = null;
            if (e.Token != null)
            {
                ScenarioPropVM = new AlarmScenarioPropertiesViewModel(e.Token, e.Scenario as AlarmScenarioModel);
            }
            else
            {
                ScenarioPropVM = null;
            }
            TargetListVM = null;
        }

        private void OnScenarioPropertiesSave(AlarmScenarioPropertiesSaveEvent e)
        {
            ActionPropVM = null;
            ScenarioPropVM = null;
            TargetListVM = null;
        }

        private void OnScenarioPropertiesClose(AlarmScenarioPropertiesCloseEvent e)
        {
            ActionPropVM = null;
            ScenarioPropVM = null;
            TargetListVM = null;
        }

        private void OnActionPropertiesOpen(AlarmActionPropertiesOpenEvent e)
        {
            if (e != null && ActionPropVM != null
                && e.Token == ActionPropVM.Token)
            {
                return;
            }

            ActionPropVM = null;
            if (e == null)
            {
                ActionPropVM = null;
            }
            else
            {
                ActionPropVM = new AlarmActionPropertiesViewModel(e.Token, e.Action);
            }
        }

        private void OnActionPropertiesSave(AlarmActionPropertiesSaveEvent e)
        {
            ActionPropVM = null;
        }

        private void OnActionPropertiesClose(AlarmActionPropertiesCloseEvent e)
        {
            ActionPropVM = null;
        }

        private void OnRecordingTargetListOpen(AlarmScenarioTargetListOpenEvent e)
        {
            if (e != null && TargetListVM != null
                && e.Token == TargetListVM.Token)
            {
                return;
            }

            TargetListVM = null;
            if (e == null)
            {
                TargetListVM = null;
            }
            else
            {
                TargetListVM = new AlarmScenarioTargetListViewModel(e.Token, e.ScenarioCode, e.CheckedTargetList);
            }
        }

        private void OnRecordingTargetListSave(AlarmScenarioTargetListSaveEvent e)
        {
            TargetListVM = null;
        }

        private void OnRecordingTargetListClose(AlarmScenarioTargetListCloseEvent e)
        {
            TargetListVM = null;
        }
        #endregion
    }
}
