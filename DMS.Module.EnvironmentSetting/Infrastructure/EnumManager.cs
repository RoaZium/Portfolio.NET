namespace DMS.Module.EnvironmentSetting.Infrastructure
{
    public enum AlarmStateLevel
    {
        Normal = 0, // 정상
        Abnormal = 1, // 이상
        Danger = 2, // 위험
        Stop = 900, // 정지
    }

    public enum EnvSettingViewType
    {
        Basic,      // 기본 설정
        AlarmNoti   // 알람통보 설정
    }
}
