using Coever.Lib.CoPlatform.Scenario.Core;
using System;

namespace DMS.Module.Management.Events
{
    public class ActionPropertiesSaveEvent
    {
        public readonly object Token;
        public readonly IActionModel Action;

        public ActionPropertiesSaveEvent(object token, IActionModel action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            Token = token;
            Action = action;
        }
    }
}
