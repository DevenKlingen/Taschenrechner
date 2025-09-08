using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class Fibonacci
{
    private readonly Hilfsfunktionen _help;
    
    public Fibonacci(Hilfsfunktionen help)
    {
        _help = help;
    }

    /// <summary>
    /// Erstellt die Fibonacci-Sequenz der ersten n Zahlen, wobei n vom Nutzer festgelegt wird
    /// </summary>
    public void FibonacciErstellen()
    {
        _help.Write("Gib die Anzahl der Fibonacci-Zahlen ein: ");
        string eingabe = Console.ReadLine();

        int.TryParse(eingabe, out int n);

        List<long> fibonacciZahlen = MathUtils.FibonacciSequenz((int)n);

        _help.Write("Fibonacci-Zahlen: " + string.Join(", ", fibonacciZahlen));
    }
}
