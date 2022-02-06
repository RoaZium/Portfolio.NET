using DMS.Module.Map.Model;
using Prism.Commands;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Map.ViewModel
{
    public class MapListItemViewModel : DMViewModelBase
    {
        #region Properties
        private bool _IsExpanded;
        public bool IsExpanded
        {
            get
            {
                if (Model == null || Model.MapType == 1)
                {
                    return false;
                }
                return _IsExpanded;
            }
            set
            {
                if (Model != null && Model.MapType == 0)
                {
                    _IsExpanded = value;
                    RaisePropertyChanged("IsExpanded");
                }
            }
        }

        private bool _IsEditMode;
        public bool IsEditMode
        {
            get => _IsEditMode;
            set
            {
                _IsEditMode = value;
                RaisePropertyChanged("IsEditMode");
            }
        }

        public string ID
        {
            get => Model == null ? null : Model.MapID;
            set
            {
                if (Model != null)
                {
                    Model.MapID = value;
                    RaisePropertyChanged("ID");
                }
            }
        }

        public string ParentID
        {
            get => Model == null ? null : Model.ParentID;
            set
            {
                if (Model != null)
                {
                    Model.ParentID = value;
                    RaisePropertyChanged("ParentID");
                }
            }
        }

        private MapModel _Model;
        public MapModel Model
        {
            get => _Model;
            set
            {
                _Model = value;
                RaisePropertyChanged("Model");
            }
        }
        #endregion

        #region Commands
        public DelegateCommand ModifiedMapCommand => new DelegateCommand(ModifiedMapCommandExecute);

        private void ModifiedMapCommandExecute()
        {
        }
        #endregion
    }
}