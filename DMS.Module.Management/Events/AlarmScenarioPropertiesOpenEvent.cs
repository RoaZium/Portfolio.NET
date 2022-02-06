using Coever.Lib.CoPlatform.Scenario.Core;

namespace DMS.Module.Management.Events
{
    public class AlarmScenarioPropertiesOpenEvent
    {
        public readonly object Token;
        public readonly IScenarioModel Scenario;

        public AlarmScenarioPropertiesOpenEvent(object token, IScenarioModel scenario)
        {
            Token = token;
            Scenario = scenario;
        }
    }
}
