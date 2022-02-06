
namespace DMS.Module.Management.Events
{
    public class AlarmActionPropertiesCloseEvent
    {
        public readonly object Token;

        public AlarmActionPropertiesCloseEvent(object token)
        {
            Token = token;
        }
    }
}
