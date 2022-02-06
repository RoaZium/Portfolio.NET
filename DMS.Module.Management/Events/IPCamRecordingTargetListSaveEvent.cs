using DMS.Module.Management.Model;
using System;
using System.Collections.Generic;

namespace DMS.Module.Management.Events
{
    public class IPCamRecordingTargetListSaveEvent
    {
        public readonly int? IPCamCode;
        public readonly List<TargetModel> TargetList;

        public IPCamRecordingTargetListSaveEvent(int? ipCamCode, List<TargetModel> targetList)
        {
            if (ipCamCode == null)
            {
                throw new ArgumentNullException("ipCamCode");
            }
            IPCamCode = ipCamCode;
            TargetList = targetList;
        }
    }
}
