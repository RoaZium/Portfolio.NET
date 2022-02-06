using DMS.Module.Management.Events;
using DMS.Module.Management.Infrastructure;
using Prism.Commands;
using Prism.Events;
using PrismWPF.Common.MVVM;
using System;
using System.Windows;

namespace DMS.Module.Management.ViewModel
{
    public class ManagementListViewModel : DMViewModelBase
    {
        private bool _IsShowMenu = true;
        public bool IsShowMenu
        {
            get => _IsShowMenu;
            set
            {
                _IsShowMenu = value;

                if (_IsShowMenu == true)
                {
                    VisibilityLayoutList = Visibility.Visible;
                }
                else
                {
                    VisibilityLayoutList = Visibility.Collapsed;
                }
            }
        }

        private Visibility _VisibilityLayoutList;
        public Visibility VisibilityLayoutList
        {
            get => _VisibilityLayoutList;
            set
            {
                _VisibilityLayoutList = value;
                RaisePropertyChanged("VisibilityLayoutList");
            }
        }

        #region Commands
        public DelegateCommand<Nullable<ManagementType>> ManagementClickedCommand => new DelegateCommand<Nullable<ManagementType>>(ManagementClickedCommandExecute);

        private void ManagementClickedCommandExecute(Nullable<ManagementType> managementType)
        {
            if (managementType != null)
            {
                EventAggregator?.GetEvent<PubSubEvent<ManagementSelectedEvent>>().Publish(
                    new ManagementSelectedEvent(managementType.Value));
            }
        }

        #endregion
    }
}
