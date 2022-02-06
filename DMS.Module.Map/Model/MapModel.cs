using PrismWPF.Common.MVVM;

namespace DMS.Module.Map.Model
{
    public class MapModel : DMViewModelBase
    {
        private string _MapID;
        public string MapID
        {
            get => _MapID;
            set
            {
                _MapID = value;
                RaisePropertyChanged("MapID");
            }
        }

        private string _ParentID;
        public string ParentID
        {
            get => _ParentID;
            set
            {
                _ParentID = value;
                RaisePropertyChanged("ParentID");
            }
        }

        private string _MapName;
        public string MapName
        {
            get => _MapName;
            set
            {
                _MapName = value;
                RaisePropertyChanged("MapName");
            }
        }

        private int _MapType;
        public int MapType
        {
            get => _MapType;
            set
            {
                _MapType = value;
                RaisePropertyChanged("MapType");
            }
        }
    }
}
