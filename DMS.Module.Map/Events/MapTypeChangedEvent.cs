using Coever.Lib.CoPlatform.Scenario.Core;
using System;

namespace DMS.Module.Map.Events
{
    public class MapTypeChangedEvent
    {
        public readonly ScenarioTypes Value;

        public MapTypeChangedEvent(ScenarioTypes value)
        {
            Value = value;
        }
    }
}
