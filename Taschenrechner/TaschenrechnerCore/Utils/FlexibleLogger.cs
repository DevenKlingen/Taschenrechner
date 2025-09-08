using TaschenrechnerCore.Interfaces;

namespace TaschenrechnerCore.Utils
{
    public class FlexibleLogger
    {
        private readonly ILogTarget _logTarget;

        public FlexibleLogger(ILogTarget logTarget)
        {
            _logTarget = logTarget;
        }

        public void Log(string message, string level = "INFO")
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";
            _logTarget.WriteLog(logMessage);
        }

        public void Info(string message) => Log(message, "INFO");
        public void Warn(string message) => Log(message, "WARN");
        public void Error(string message) => Log(message, "ERROR");
    }
}
