using Coever.Lib.CoPlatform.Scenario.Core;
using PrismWPF.Common.MVVM;
using System.ComponentModel;

namespace DMS.Module.Management.ViewModel
{
    public class ScenarioItemViewModel : DMViewModelBase, IEditableObject
    {
        private IScenarioModel _Scenario;
        public IScenarioModel Scenario
        {
            get => _Scenario;
            set
            {
                _Scenario = value;
                RaisePropertyChanged("Scenario");
            }
        }

        public ScenarioItemViewModel()
        {
        }

        public ScenarioItemViewModel(IScenarioModel scenario)
        {
            _Scenario = scenario;
        }

        #region IEditableObject
        private IScenarioModel __Scenario;

        public void BeginEdit()
        {
            __Scenario = Scenario;
        }

        public void EndEdit()
        {
            __Scenario = null;
        }

        public void CancelEdit()
        {
            Scenario = __Scenario;

            __Scenario = null;
        }
        #endregion
    }
}
