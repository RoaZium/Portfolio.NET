
namespace DMS.Module.Management.Events
{
    public class ActionPropertiesCloseEvent
    {
        public readonly object Token;

        public ActionPropertiesCloseEvent(object token)
        {
            Token = token;
        }
    }
}
