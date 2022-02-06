using Coever.Lib.CoPlatform.Scenario.Core;
using PrismWPF.Common.MVVM;
using System.ComponentModel;

namespace DMS.Module.Management.Model
{
    public class ActionItemViewModel : DMViewModelBase, IEditableObject
    {
        #region Properties
        private IActionModel _ActionModel;
        public IActionModel ActionModel
        {
            get => _ActionModel;
            set
            {
                _ActionModel = value;
                RaisePropertyChanged("ActionModel");
                RaisePropertyChanged("ID");
                RaisePropertyChanged("ActionType");
                RaisePropertyChanged("ActionName");
            }
        }
        #endregion

        #region Constructor
        public ActionItemViewModel()
        {
        }

        public ActionItemViewModel(IActionModel actionModel)
        {
            ActionModel = actionModel;
        }
        #endregion

        #region IEditableObject
        private IActionModel __ActionModel;

        public void BeginEdit()
        {
            __ActionModel = ActionModel;
        }

        public void EndEdit()
        {
            __ActionModel = null;
        }

        public void CancelEdit()
        {
            ActionModel = __ActionModel;

            __ActionModel = null;
        }
        #endregion
    }
}
