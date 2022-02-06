using DMS.Module.Management.ViewModel;
using System;
using System.Collections.Generic;

namespace DMS.Module.Management.Events
{
    public class AlarmScenarioTargetListSaveEvent
    {
        public readonly object Token;
        public readonly List<AlarmScenarioTargetItemViewModel> TargetList;

        public AlarmScenarioTargetListSaveEvent(object token, List<AlarmScenarioTargetItemViewModel> targetList)
        {
            if (targetList == null)
            {
                throw new ArgumentNullException("targetList");
            }
            Token = token;
            TargetList = targetList;
        }
    }
}
