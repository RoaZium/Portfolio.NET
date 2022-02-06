using Coever.Lib.CoPlatform.Core.Models;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.ViewModel
{
    public class AlarmScenarioTargetItemViewModel : DMViewModelBase
    {
        private DmAlarmScenarioMapping _Target;
        public DmAlarmScenarioMapping Target
        {
            get => _Target;
            set
            {
                _Target = value;
                RaisePropertyChanged("Target");
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

        public AlarmScenarioTargetItemViewModel()
        {
        }

        public AlarmScenarioTargetItemViewModel(DmAlarmScenarioMapping target)
        {
            _Target = target;
        }
    }
}
