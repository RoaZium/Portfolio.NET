using log4net;

namespace DynamicMonitoring
{
    public class LogManager
    {
        private static readonly object locker = new object();

        public Logger ClientLog = null;
        public Logger SocketLog = null;
        public Logger DatabaseLog = null;

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

        public LogManager()
        {
            ClientLog = new Logger("Client");
            SocketLog = new Logger("Socket");
            DatabaseLog = new Logger("Database");
        }
    }

    public class Logger
    {
        private static readonly object locker = new object();

        private ILog _Log = null;
        public ILog Log
        {
            get { return _Log; }
            set { _Log = value; }
        }

        public Logger(string log)
        {
            Log = log4net.LogManager.GetLogger(log);
        }

        public void WriteLine(LOG_LEVEL lvl, string msg)
        {
            lock (locker)
            {
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
    }

    public enum LOG_LEVEL
    {
        DebugLevel,
        InfoLevel,
        WarningLevel,
        ErrorLevel,
        FatalLevel
    }
}