using DMS.Module.Management;

namespace DynamicMonitoring.Modules
{
    internal class ManagementDataBroker : IDataBroker
    {
        #region 싱글톤
        private static ManagementDataBroker instance;

        public static ManagementDataBroker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ManagementDataBroker();
                }
                return instance;
            }
        }

        private ManagementDataBroker()
        {
        }
        #endregion
    }
}
