using Coever.Lib.CoPlatform.Scenario.Core;
using DMS.Module.Map.Model.RestAPI;
using System.Threading.Tasks;

namespace DMS.Module.Map.ViewModel
{
    public interface ILayoutViewModel
    {
        DmComponentMst DmComponentM { get; set; }

        Task ExecuteComponentAction(IActionModel action);
    }
}
