using TaschenrechnerCore.Interfaces;

namespace TaschenrechnerCore.Services
{
    public class DateiLogTarget : ILogTarget
    {

        public void Clear()
        {
            File.WriteAllText("log.txt", string.Empty);
        }

        public void WriteLog(string message, bool noNewLine = false)
        {
            var directory = Path.Join("log.txt");

            if (!File.Exists(directory))
            {
                File.Create(directory);
            }
            try
            {
                using (StreamWriter writer = new StreamWriter(directory, append: true))
                {
                    if (noNewLine)
                    {
                        writer.Write(message);
                        return;
                    }
                    writer.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Schreiben in die Log-Datei: {ex.Message}");
            }
        }
    }
}
