using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.Model
{
    public class SelectionModel : DMViewModelBase
    {
        private object _Key;
        public object Key
        {
            get => _Key;
            set
            {
                _Key = value;
                RaisePropertyChanged("Key");
            }
        }

        private object _Value;
        public object Value
        {
            get => _Value;
            set
            {
                _Value = value;
                RaisePropertyChanged("Value");
            }
        }

        private string _DisplayName;
        public new string DisplayName
        {
            get => _DisplayName;
            set
            {
                _DisplayName = value;
                RaisePropertyChanged("DisplayName");
            }
        }
    }
}
