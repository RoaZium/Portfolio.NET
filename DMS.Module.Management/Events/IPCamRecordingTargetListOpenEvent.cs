using DMS.Module.Management.Model;
using System;
using System.Collections.Generic;

namespace DMS.Module.Management.Events
{
    public class IPCamRecordingTargetListOpenEvent
    {
        public readonly int? IPCamCode;
        public readonly List<TargetModel> CheckedTargetList;

        public IPCamRecordingTargetListOpenEvent(int? ipCamCode, List<TargetModel> checkedTargetList)
        {
            if (ipCamCode == null)
            {
                throw new ArgumentNullException("ipCamCode");
            }
            IPCamCode = ipCamCode;
            CheckedTargetList = checkedTargetList;
        }
    }
}
