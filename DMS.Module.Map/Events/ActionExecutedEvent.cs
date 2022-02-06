using Coever.Lib.CoPlatform.Scenario.Core;
using System;

namespace DMS.Module.Map.Events
{
    public class ActionExecutedEvent
    {
        public readonly IActionModel Value;

        public ActionExecutedEvent(IActionModel value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Value = value;
        }
    }
}
