using Coever.Lib.CoPlatform.Scenario.Core;
using PrismWPF.Common.MVVM;

namespace DMS.Module.Map.ViewModel
{
    public class ActionPackerModel : DMViewModelBase
    {
        #region Properties
        public readonly ScenarioTypes TargetType;

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

        public ActionTypes ActionType => ActionModel != null ? ActionModel.ActionType : ActionTypes.invalid;

        public int? ActionDelaySecond => ActionModel?.DelaySecond;
        #endregion

        #region Constructor
        public ActionPackerModel()
        {
        }

        public ActionPackerModel(IActionModel actionModel)
        {
            ActionModel = actionModel;
        }

        public ActionPackerModel(ScenarioTypes targetType)
        {
            TargetType = targetType;
        }

        public ActionPackerModel(ScenarioTypes targetType, IActionModel actionModel)
        {
            TargetType = targetType;
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
