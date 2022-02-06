using DMS.Module.Map.Model.RestAPI;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Map.ViewModel.Popup
{
    public class CP_CommonPopupViewModel : DMViewModelBase
    {
        public CP_CommonPopupViewModel()
        {
        }

        public override void Load()
        {
        }

        public override void Unload()
        {
            ResourceSensorM = null;
        }

        private DmComponentMst _dmComponentM;
        public DmComponentMst DmComponentM
        {
            get
            {
                if (_dmComponentM == null)
                {
                    _dmComponentM = new DmComponentMst();
                }
                return _dmComponentM;
            }
            set
            {
                _dmComponentM = value;
                RaisePropertyChanged("DmComponentM");
            }
        }

        private DmResourceSensorModel _ResourceSensorM;
        public DmResourceSensorModel ResourceSensorM
        {
            get
            {
                if (_ResourceSensorM == null)
                {
                    _ResourceSensorM = new DmResourceSensorModel();
                }
                return _ResourceSensorM;
            }
            set
            {
                _ResourceSensorM = value;
                RaisePropertyChanged("ResourceSensorM");
            }
        }
    }
}
