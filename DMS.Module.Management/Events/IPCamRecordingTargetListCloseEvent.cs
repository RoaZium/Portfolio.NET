using System;

namespace DMS.Module.Management.Events
{
    public class IPCamRecordingTargetListCloseEvent
    {
        public readonly int? IPCamCode;

        public IPCamRecordingTargetListCloseEvent(int? ipCamCode)
        {
            if (ipCamCode == null)
            {
                throw new ArgumentNullException("ipCamCode");
            }
            IPCamCode = ipCamCode;
        }
    }
}
