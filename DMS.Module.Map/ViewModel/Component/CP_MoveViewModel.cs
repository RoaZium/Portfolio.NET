using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model.RestAPI;
using Prism.Commands;
using System.Linq;

namespace DMS.Module.Map.ViewModel.Component
{
    public class CP_MoveViewModel : CP_CommonBaseViewModel
    {
        public override void Load()
        {
        }

        public override void Unload()
        {
        }

        protected override void OnComponentData()
        {
        }

        public DelegateCommand MoveCommand => new DelegateCommand(MoveCommandExecute);

        private void MoveCommandExecute()
        {
            DmComponentMst resultLinq = LinqManager.FilterFirstRoutingCode(DmComponentM.PropertyM.ItemCode);

            if (resultLinq == null)
            {
                return;
            }

            MapFrameViewModel.Instance.SelectedComponentConfig(resultLinq);
        }
    }
}
