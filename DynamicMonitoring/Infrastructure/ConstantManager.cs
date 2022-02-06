using System;
using System.IO;

namespace DynamicMonitoring.Infrastructure
{
    public class ConstantManager
    {
        public const string BROADCAST_MODULE = "Module";
        public const string BROADCAST_MAPEDIT = "MapEdit";
        public const string BROADCAST_FULLSCREEN = "FullScreen";

        public const string MQTTTOPIC_IPCAMERA = "/event/+/ipcamfull/#";
        public const string MQTTTOPIC_SENSOR = "/event/+/sensor/#";
        public const string MQTTTOPIC_ALARMSTATE = "/event/c/alarm_state/#";
        public const string MQTTTOPIC_DATACOLLECTIONDIGIT = "/event/c/data_collection_digit/#";
        public const string MQTTTOPIC_KPI = "/event/c/KPI/#";
        public const string MQTTTOPIC_DATAGRID = "/event/c/DataGrid/#";
        public const string MQTTTOPIC_IMAGE = "/event/c/image/#";

        public static readonly string DIRECTORY_PROGRAM = AppDomain.CurrentDomain.BaseDirectory;
        public const string Logo = @"Logo/Logo.png";
    }
}
