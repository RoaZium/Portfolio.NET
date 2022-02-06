using Newtonsoft.Json;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.Model
{
    public class TargetModel : DMViewModelBase
    {
        private string _TargetCode;
        public string TargetCode
        {
            get => _TargetCode;
            set
            {
                _TargetCode = value;
                RaisePropertyChanged("TargetCode");
            }
        }

        private string _TargetType;
        public string TargetType
        {
            get => _TargetType;
            set
            {
                _TargetType = value;
                RaisePropertyChanged("TargetType");
            }
        }

        private string _TargetName;
        [JsonIgnore]
        public string TargetName
        {
            get => _TargetName;
            set
            {
                _TargetName = value;
                RaisePropertyChanged("TargetName");
            }
        }
    }
}
