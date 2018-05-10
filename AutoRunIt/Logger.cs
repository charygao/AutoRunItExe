using NLog;

namespace AutoRunIt
{
    static class Logger
    {
        private static ILogger _log;

        public static ILogger Log => _log ?? (_log = NLog.LogManager.GetCurrentClassLogger());
    }
}