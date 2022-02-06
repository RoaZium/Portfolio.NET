using Prism.Events;

namespace DMS.Module.Management.Managers
{
    public interface IManagementFrameManager
    {
        IEventAggregator EventAggregator { get; }
    }
}
