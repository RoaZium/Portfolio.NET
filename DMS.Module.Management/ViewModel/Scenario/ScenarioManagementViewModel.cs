using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Management.Events;
using Prism.Events;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.ViewModel
{
    public class ScenarioManagementViewModel : DMViewModelBase
    {
        #region Properties
        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesOpenEvent>>().Unsubscribe(OnScenarioPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesSaveEvent>>().Unsubscribe(OnScenarioPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesCloseEvent>>().Unsubscribe(OnScenarioPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesOpenEvent>>().Unsubscribe(OnActionPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesSaveEvent>>().Unsubscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesCloseEvent>>().Unsubscribe(OnActionPropertiesClose);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesOpenEvent>>().Subscribe(OnScenarioPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesSaveEvent>>().Subscribe(OnScenarioPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ScenarioPropertiesCloseEvent>>().Subscribe(OnScenarioPropertiesClose);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesOpenEvent>>().Subscribe(OnActionPropertiesOpen);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesSaveEvent>>().Subscribe(OnActionPropertiesSave);
                    base.EventAggregator?.GetEvent<PubSubEvent<ActionPropertiesCloseEvent>>().Subscribe(OnActionPropertiesClose);
                }
            }
        }

        private ScenarioListViewModel _ScenarioListVM = new ScenarioListViewModel();
        public ScenarioListViewModel ScenarioListVM
        {
            get => _ScenarioListVM;
            set
            {
                _ScenarioListVM = value;
                RaisePropertyChanged("ScenarioListVM");
            }
        }

        private ScenarioPropertiesViewModel _ScenarioPropVM;
        public ScenarioPropertiesViewModel ScenarioPropVM
        {
            get => _ScenarioPropVM;
            set
            {
                _ScenarioPropVM = value;
                RaisePropertyChanged("ScenarioPropVM");
            }
        }

        private ActionPropertiesViewModel _ActionPropVM;
        public ActionPropertiesViewModel ActionPropVM
        {
            get => _ActionPropVM;
            set
            {
                _ActionPropVM = value;
                RaisePropertyChanged("ActionPropVM");
            }
        }
        #endregion

        #region Commands / Events
        private void OnScenarioPropertiesOpen(ScenarioPropertiesOpenEvent e)
        {
            if (ScenarioPropVM != null && e?.Scenario?.ScenarioCode != null
                && e.Token == ScenarioPropVM.Token)
            {
                return;
            }

            ActionPropVM = null;
            if (e.Token != null)
            {
                ScenarioPropVM = new ScenarioPropertiesViewModel(e.Token, e.Scenario as ScenarioModel, e.ScenarioType);
            }
            else
            {
                ScenarioPropVM = null;
            }
        }

        private void OnActionPropertiesOpen(ActionPropertiesOpenEvent e)
        {
            if (e != null && ActionPropVM != null
                && e.Token == ActionPropVM.Token)
            {
                return;
            }

            ActionPropVM = null;
            if (e != null)
            {
                ActionPropVM = new ActionPropertiesViewModel(e.Token, e.Action, e.ScenarioType);
            }
        }

        private void OnScenarioPropertiesSave(ScenarioPropertiesSaveEvent e)
        {
            ActionPropVM = null;
            ScenarioPropVM = null;
        }

        private void OnActionPropertiesSave(ActionPropertiesSaveEvent e)
        {
            ActionPropVM = null;
        }

        private void OnScenarioPropertiesClose(ScenarioPropertiesCloseEvent e)
        {
            ActionPropVM = null;
            ScenarioPropVM = null;
        }

        private void OnActionPropertiesClose(ActionPropertiesCloseEvent e)
        {
            ActionPropVM = null;
        }
        #endregion
    }
}
