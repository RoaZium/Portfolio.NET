namespace DMS.Module.Management.Infrastructure
{
    public class ConstantManager
    {
        #region API Keys

        /// <summary>
        /// 공정 구성 현황 조회
        /// </summary>
        public static readonly string APIKey_RoutingConfigurationStatus = $"RoutingConfigurationStatus?userId=0";

        /// <summary>
        /// 항목
        /// </summary>
        public static readonly string APIKey_BaseInfo = $"BaseInfo?type=";

        #endregion

        public const string BROADCAST_ALARMCODE = "AlarmCode";
        public const string BROADCAST_ALARMLAYOUT = "AlarmLayout";
    }
}
