using Prism.Events;

namespace DMS.Module.Management.Managers
{
    public interface IScenarioManagementManager
    {
        IEventAggregator EventAggregator { get; }
    }
}
