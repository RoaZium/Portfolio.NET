using DMS.Module.Management.Model;
using System.Collections.Generic;

namespace DMS.Module.Management
{
    public static class ModuleManager
    {
        private static IDataBroker _DataManager;
        public static IDataBroker DataManager
        {
            internal get => _DataManager;
            set => _DataManager = value;
        }
    }

    public interface IDataBroker
    {
    }
}
