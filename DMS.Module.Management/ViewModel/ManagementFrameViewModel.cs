using DMS.Module.Management.Events;
using DMS.Module.Management.Infrastructure;
using DMS.Module.Management.View;
using Prism.Events;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Management.ViewModel
{
    public class ManagementFrameViewModel : DMViewModelBase
    {
        #region Properties
        private ManagementType _SelectedManagementType = ManagementType.RoutingConfigStatus;
        public ManagementType SelectedManagementType
        {
            get => _SelectedManagementType;
            set
            {
                _SelectedManagementType = value;
                RaisePropertyChanged("SelectedManagementType");
            }
        }

        public override IEventAggregator EventAggregator
        {
            get => base.EventAggregator;
            set
            {
                if (base.EventAggregator != value)
                {
                    base.EventAggregator?.GetEvent<PubSubEvent<ManagementSelectedEvent>>().Unsubscribe(OnManagementSelected);
                    base.EventAggregator = value;

                    base.EventAggregator?.GetEvent<PubSubEvent<ManagementSelectedEvent>>().Subscribe(OnManagementSelected);
                }
            }
        }

        private ManagementListView _ManagementListV;
        public ManagementListView ManagementListV
        {
            get
            {
                if (_ManagementListV == null)
                {
                    _ManagementListV = new ManagementListView();
                }
                return _ManagementListV;
            }
            set
            {
                _ManagementListV = value;
                RaisePropertyChanged("ManagementListV");
            }
        }

        #endregion

        #region Constructor
        public ManagementFrameViewModel()
        {
        }
        #endregion

        #region Commands / Events
        private void OnManagementSelected(ManagementSelectedEvent e)
        {
            SelectedManagementType = e.ManagementType;
        }
        #endregion
    }
}
