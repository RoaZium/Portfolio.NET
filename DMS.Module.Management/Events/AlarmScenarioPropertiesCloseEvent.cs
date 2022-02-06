
namespace DMS.Module.Management.Events
{
    public class AlarmScenarioPropertiesCloseEvent
    {
        public readonly object Token;

        public AlarmScenarioPropertiesCloseEvent(object token)
        {
            Token = token;
        }
    }
}
