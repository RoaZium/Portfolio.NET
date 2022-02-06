using Coever.Lib.CoPlatform.Core.Models;
using System.Collections.Generic;

namespace DMS.Module.Management.Events
{
    public class AlarmScenarioTargetListOpenEvent
    {
        public readonly object Token;
        public readonly int? ScenarioCode;
        public readonly List<DmAlarmScenarioMapping> CheckedTargetList;

        public AlarmScenarioTargetListOpenEvent(object token, int? scenarioCode, List<DmAlarmScenarioMapping> checkedTargetList)
        {
            Token = token;
            ScenarioCode = scenarioCode;
            CheckedTargetList = checkedTargetList;
        }
    }
}
