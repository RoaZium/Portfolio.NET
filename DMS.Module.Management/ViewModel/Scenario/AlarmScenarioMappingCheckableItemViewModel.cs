using Coever.Lib.CoPlatform.Core.Models;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.Model
{
    public class AlarmScenarioMappingCheckableItemViewModel : DMViewModelBase
    {
        private DmAlarmScenarioMapping _Mapping;
        public DmAlarmScenarioMapping Mapping
        {
            get => _Mapping;
            set
            {
                _Mapping = value;
                RaisePropertyChanged("Mapping");
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

        private bool _IsChecked;
        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }
    }
}
