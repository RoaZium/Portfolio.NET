
namespace DMS.Module.Management.Events
{
    public class ScenarioPropertiesCloseEvent
    {
        public readonly object Token;

        public ScenarioPropertiesCloseEvent(object token)
        {
            Token = token;
        }
    }
}
