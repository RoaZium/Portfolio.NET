using System;
using System.IO;

namespace DMS.Module.EnvironmentSetting.Infrastructure
{
    public class ConstantManager
    {
        public const string BROADCAST_TITLE_CHANGED = "TitleChanged";

        public const string KEY_CONTAINER_NAME = "670A1B79";

        public static readonly string DIRECTORY_PROGRAM = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string DIRECTORY_SETTINGS = Path.Combine(DIRECTORY_PROGRAM, "Settings");
        public static readonly string FILE_GENERAL_SETTINGS = Path.Combine(DIRECTORY_SETTINGS, "GeneralSettings");
    }
}
