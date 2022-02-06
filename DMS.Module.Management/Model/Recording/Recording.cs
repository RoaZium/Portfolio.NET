using Coever.Lib.Axis.Core;
using Coever.Lib.Axis.Core.Model;
using Coever.Lib.CoPlatform.Core.Models;
using System;

namespace DMS.Module.Management.Model.Recording
{
    public class Recording
    {
        public DmIpcamMst IPCam { get; set; }

        public string DiskId { get; set; }

        public string RecordingId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? StopTime { get; set; }

        public RecordingType RecordingType { get; set; }

        public string EventId { get; set; }

        public string EventTrigger { get; set; }

        public RecordingStatus RecordingStatus { get; set; }

        public int Source { get; set; }

        public bool Locked { get; set; }

        public Video Video { get; set; }
    }
}
