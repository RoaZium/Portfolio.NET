using System;

/// <summary>
/// 알람 상태 레벨 조회
/// </summary>
namespace DMS.Module.Map.Model.RestAPI
{
    public class DmRoutingConfigurationAlarmStatusModel
    {
        public string RoutingCode { get; set; }
        public string BaseInfoCode { get; set; }
        public string RoutingType { get; set; }
        public string CollectionValue { get; set; }
        public int? AlarmLevel { get; set; }
        public DateTime? AlarmDate { get; set; }
        public int? BlinkingLevel { get; set; }
    }
}
