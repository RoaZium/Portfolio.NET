using Coever.Lib.CoPlatform.Scenario.Core;
using System;

namespace DMS.Module.Management.Events
{
    public class AlarmScenarioPropertiesSaveEvent
    {
        public readonly object Token;
        public readonly IScenarioModel Scenario;

        public AlarmScenarioPropertiesSaveEvent(object token, IScenarioModel scenario)
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
