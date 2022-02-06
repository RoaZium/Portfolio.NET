using PrismWPF.Common.MVVM;

namespace DMS.Module.Map.Model
{
    public class MasterDetailInfoModel : DMViewModelBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string _value;
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }
    }
}
