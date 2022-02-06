using DynamicMonitoring.Properties;
using System.Globalization;
using System.Threading;

namespace DynamicMonitoring.Infrastructure
{
    public class DMConfigManager
    {
        public string userAuth = string.Empty;

        public string Title = Res.StrDefaultAppTitle;

        public CultureInfo cultureInfo;

        private static readonly object locker = new object();

        private static DMConfigManager _Instance;
        public static DMConfigManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new DMConfigManager();
                        }
                    }
                }
                return _Instance;
            }
        }

        public void ChangeCultureInfo(string localeName)
        {
            try
            {
                cultureInfo = CultureInfo.CreateSpecificCulture(localeName);

                // 현재 쓰레드의 컬쳐를 변경함
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

                // 다른 쓰레드가 생성될 때의 기본 컬쳐를 설정
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
            // If an exception occurs, we'll just fall back to the system default.
            catch
            {
                return;
            }
        }
    }
}
