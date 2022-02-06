using Coever.Lib.CoPlatform.Scenario.Core;

namespace DMS.Module.Management.Events
{
    public class ActionPropertiesOpenEvent
    {
        public readonly object Token;
        public readonly IActionModel Action;
        public readonly ScenarioTypes ScenarioType;

        public ActionPropertiesOpenEvent(object token, IActionModel action, ScenarioTypes scenarioType)
        {
            Token = token;
            Action = action;
            ScenarioType = scenarioType;
        }
    }
}
