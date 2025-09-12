using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Services;

namespace TaschenrechnerCore.Utils
{
    public class FlexibleLogger
    {
        private readonly ILogTarget _logTarget;
        BenutzerManagement _benutzerManagement;

        public FlexibleLogger(ILogTarget logTarget, BenutzerManagement benutzerManagement)
        {
            _logTarget = logTarget;
            _benutzerManagement = benutzerManagement;
        }

        public void Log(string message, string level, bool noNewLine, bool ignoreTimeStamp)
        {
            bool zeigeZeitstempel = false;

            using var context = new TaschenrechnerContext();
            if (_benutzerManagement.getBenutzer() != null)
            {
                var benutzerId = _benutzerManagement.getBenutzer().Id;
                string einstellungsObj = context.Einstellungen.FirstOrDefault(e => e.BenutzerId == benutzerId && e.Schluessel.ToLower() == "zeigezeitstempel").Wert.ToString().ToLower();

                zeigeZeitstempel = einstellungsObj == "j" || einstellungsObj == "true" || einstellungsObj == "ja";
            }

            switch(level){
                case "WARNING":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "ERROR":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            if (zeigeZeitstempel && !noNewLine)
            {
                if (!ignoreTimeStamp)
                {
                    _logTarget.WriteLog($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}", noNewLine);
                    return;
                }
            }
            _logTarget.WriteLog(message, noNewLine);
        }

        public void Info(string message, bool noNewLine = false, bool ignoreTimeStamp = false) => Log(message, "INFO", noNewLine, ignoreTimeStamp);
        public void Warn(string message, bool noNewLine = false, bool ignoreTimeStamp = false) => Log(message, "WARN", noNewLine, ignoreTimeStamp);
        public void Error(string message, bool noNewLine = false, bool ignoreTimeStamp = false) => Log(message, "ERROR", noNewLine, ignoreTimeStamp);
        public void Debug(string message, bool noNewLine = false, bool ignoreTimeStamp = false) => Log(message, "DEBUG", noNewLine, ignoreTimeStamp);
    }
}
