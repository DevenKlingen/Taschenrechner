using TaschenrechnerCore.Interfaces;

namespace TaschenrechnerCore.Services
{
    public class ConsoleLogTarget : ILogTarget
    {
        public void WriteLog(string message, bool noNewLine = false)
        {
            if (noNewLine)
            {
                Console.Write(message);
                return;
            }
            Console.WriteLine(message);
        }
    }
}