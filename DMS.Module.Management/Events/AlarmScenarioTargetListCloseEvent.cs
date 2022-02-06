using System;

namespace DMS.Module.Management.Events
{
    public class AlarmScenarioTargetListCloseEvent
    {
        public readonly object Token;

        public AlarmScenarioTargetListCloseEvent(object token)
        {
            Token = token;
        }
    }
}
