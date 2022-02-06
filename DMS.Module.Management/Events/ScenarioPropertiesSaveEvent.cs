using Coever.Lib.CoPlatform.Scenario.Core;
using System;

namespace DMS.Module.Management.Events
{
    public class ScenarioPropertiesSaveEvent
    {
        public readonly object Token;
        public readonly IScenarioModel Scenario;

        public ScenarioPropertiesSaveEvent(object token, IScenarioModel scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException("scenario");
            }
            Token = token;
            Scenario = scenario;
        }
    }
}
