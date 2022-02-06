using PrismWPF.Common.MVVM;

namespace DMS.Module.Map.Model.RestAPI
{
    public class DmBaseInfoTypeModel : DMViewModelBase
    {
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
    }
}
