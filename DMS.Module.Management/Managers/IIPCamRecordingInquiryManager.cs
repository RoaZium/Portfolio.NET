using Prism.Events;

namespace DMS.Module.Management.Managers
{
    public interface IIPCamRecordingInquiryManager
    {
        IEventAggregator EventAggregator { get; }
    }
}
