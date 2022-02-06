using Prism.Events;

namespace DMS.Module.Map.Managers
{
    public interface IMonitoringManager
    {
        IEventAggregator EventAggregator { get; }
    }
}
