using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.Model
{
    public class DmBaseInfoModel : DMViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _baseInfoCode;
        public string BaseInfoCode
        {
            get => _baseInfoCode;
            set
            {
                _baseInfoCode = value;
                RaisePropertyChanged("BaseInfoCode");
            }
        }

        private string _type;
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }

        private string _useYn;
        public string UseYn
        {
            get => _useYn;
            set
            {
                _useYn = value;
                RaisePropertyChanged("UseYn");
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
