namespace DynamicMonitoring.Infrastructure
{
    public enum ClientModuleType
    {
        Map,
        Management,
        EnvironmentSetting
    }

    /// <summary>
    /// Module Type
    /// </summary>
    public enum ModuleType
    {
        DMLog      // DanamicMonitoring Log
    }

    /// <summary>
    /// SensorType
    /// </summary>
    public enum SensorTypeInfo
    {
        Numerical,      // 수치형(0, 0.333 같은 수치형 데이터 수집)
        Categorical     // 카테고리형(0, 1, 2 같은 카테고리형 데이터 수집)
    }

    public enum ReportType
    {
        R1, // 전체 공정.센서 정보 조회
        R2, // 센서 상태 이력 조회
        R3  // 센서 과거 정보 조회
    }
}
