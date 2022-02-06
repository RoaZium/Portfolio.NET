using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace PrismWPF.Common
{
    public class LogManager
    {
        private static readonly object locker = new object();

        private static LogManager _Instance;
        public static LogManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (locker)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new LogManager();
                        }
                    }
                }
                return _Instance;
            }
        }

        private ILog _Log = null;
        public ILog Log
        {
            get => _Log;
            set => _Log = value;
        }

        public void WriteLine(object moduleType, LOG_LEVEL lvl, string msg)
        {
            lock (locker)
            {
                Log = log4net.LogManager.GetLogger(moduleType.ToString());

                switch (lvl)
                {
                    case LOG_LEVEL.DebugLevel:
                        {
                            Log.Debug(msg);
                            break;
                        }
                    case LOG_LEVEL.ErrorLevel:
                        {
                            Log.Error(msg);
                            break;
                        }
                    case LOG_LEVEL.FatalLevel:
                        {
                            Log.Fatal(msg);
                            break;
                        }
                    case LOG_LEVEL.InfoLevel:
                        {
                            Log.Info(msg);
                            break;
                        }
                    case LOG_LEVEL.WarningLevel:
                        {
                            Log.Warn(msg);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        public void WriteLine(object moduleType, LOG_LEVEL lvl, string methodName, string msg)
        {
            lock (locker)
            {
                string mName = ConverterMethodName(methodName);
                Log = log4net.LogManager.GetLogger(moduleType.ToString());

                switch (lvl)
                {
                    case LOG_LEVEL.DebugLevel:
                        {
                            Log.Debug(mName + msg);
                            break;
                        }
                    case LOG_LEVEL.ErrorLevel:
                        {
                            Log.Error(mName + msg);
                            break;
                        }
                    case LOG_LEVEL.FatalLevel:
                        {
                            Log.Fatal(mName + msg);
                            break;
                        }
                    case LOG_LEVEL.InfoLevel:
                        {
                            Log.Info(mName + msg);
                            break;
                        }
                    case LOG_LEVEL.WarningLevel:
                        {
                            Log.Warn(mName + msg);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private string ConverterMethodName(string methodName)
        {
            string result = "[" + methodName + "]";
            return result;
        }
    }

    public enum LOG_LEVEL
    {
        DebugLevel,
        InfoLevel,
        WarningLevel,
        ErrorLevel,
        FatalLevel
    }

    public enum INOUT
    {
        IN,
        OUT
    }
}
