using Coever.Lib.CoPlatform.Scenario.Core;
using System;

namespace DMS.Module.Map.Requests
{
    public class ExecuteActionRequest
    {
        public readonly ScenarioModel Scenario;
        public readonly IActionModel Action;
        public readonly string TargetCode;

        public bool IsRunning => !IsCanceled && !IsFinished;

        public bool IsCanceled { get; set; }

        public bool IsFinished { get; set; }

        public ExecuteActionRequest(ScenarioModel scenario, IActionModel action, string targetCode = null)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException("scenario");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            Scenario = scenario;
            Action = action;
            TargetCode = targetCode;
        }
    }
}
