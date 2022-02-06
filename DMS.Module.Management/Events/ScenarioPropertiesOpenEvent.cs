using Coever.Lib.CoPlatform.Scenario.Core;

namespace DMS.Module.Management.Events
{
    public class ScenarioPropertiesOpenEvent
    {
        public readonly object Token;
        public readonly IScenarioModel Scenario;
        public readonly ScenarioTypes ScenarioType;

        public ScenarioPropertiesOpenEvent(object token, IScenarioModel scenario, ScenarioTypes scenarioType)
        {
            Token = token;
            Scenario = scenario;
            ScenarioType = scenarioType;
        }
    }
}
