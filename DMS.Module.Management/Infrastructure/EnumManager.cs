
namespace DMS.Module.Management.Infrastructure
{
    public enum ActionType
    {
        C,  //Create
        R,  //Read
        U,  //Update
        D,  //Delete
        N,  //None
        F   //Refair
    }

    public enum ManagementType
    {
        RoutingConfigStatus,
        Scenario,
        AlarmScenario,
        RecordingList,
        Alarm
    }

    /// <summary>
    /// MQTT Topic
    /// </summary>
    public enum MQTTTopic
    {
        data_collection_digit,
        sensor,
        alarm_state
    }

    /// <summary>
    /// SensorType
    /// </summary>
    public enum SensorType
    {
        Numerical,      // 수치형(0, 0.333 같은 수치형 데이터 수집)
        Categorical     // 카테고리형(0, 1, 2 같은 카테고리형 데이터 수집)
    }
}
