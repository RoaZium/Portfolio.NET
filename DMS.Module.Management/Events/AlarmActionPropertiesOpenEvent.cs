using Coever.Lib.CoPlatform.Scenario.Core;

namespace DMS.Module.Management.Events
{
    public class AlarmActionPropertiesOpenEvent
    {
        public readonly object Token;
        public readonly IActionModel Action;

        public AlarmActionPropertiesOpenEvent(object token, IActionModel action)
        {
            Token = token;
            Action = action;
        }
    }
}
