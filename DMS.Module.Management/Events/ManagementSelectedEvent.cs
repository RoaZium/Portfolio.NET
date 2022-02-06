using DMS.Module.Management.Infrastructure;

namespace DMS.Module.Management.Events
{
    public class ManagementSelectedEvent
    {
        public readonly ManagementType ManagementType;

        public ManagementSelectedEvent(ManagementType managementType)
        {
            ManagementType = managementType;
        }
    }
}
