using Prism.Events;

namespace DMS.Module.Management.Managers
{
    public interface IAlarmScenarioManagementManager
    {
        IEventAggregator EventAggregator { get; }
    }
}
