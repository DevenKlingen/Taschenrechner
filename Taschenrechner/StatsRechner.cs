namespace MeinTaschenrechner
{
    internal class StatsRechner
    {
        static Hilfsfunktionen help = new Hilfsfunktionen();
        /// <summary>
        /// Erstellt eine Statistik für eingegebene Zahlen (Liste)
        /// </summary>
        public void StatsRechnerMenu()
        {
            help.Write("\n=== STATISTIK-RECHNER ===");
            help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

            List<double> zahlen = new List<double>();

            while (true)
            {
                help.Write($"Zahl {zahlen.Count + 1} (oder 'fertig'): ");
                string eingabe = Console.ReadLine();

                if (eingabe.ToLower() == "fertig")
                    break;

                if (double.TryParse(eingabe, out double zahl))
                {
                    zahlen.Add(zahl);
                }
                else
                {
                    help.Write("Ungültige Eingabe!");
                }
            }

            if (zahlen.Count == 0)
            {
                help.Write("Keine Zahlen eingegeben!");
                return;
            }

            // Liste in Array umwandeln für die Statistik-Methoden
            double[] zahlenArray = zahlen.ToArray();

            help.Write($"\nAnzahl Werte: {zahlenArray.Length}");
            help.Write($"Minimum: {FindeMinimum(zahlenArray)}");
            help.Write($"Maximum: {FindeMaximum(zahlenArray)}");
            help.Write($"Summe: {BerechneSumme(zahlenArray)}");
            help.Write($"Durchschnitt: {BerechneDurchschnitt(zahlenArray):F2}");
        }

        /// <summary>
        /// Sucht das Minimum aus einem Array von Zahlen
        /// </summary>
        /// <param name="zahlen"></param>
        /// <returns></returns>
        static double FindeMinimum(double[] zahlen)
        {
            double min = 0;
            if (zahlen.Length > 0)
            {
                min = zahlen[0];
                int n = 0;
                while (n < zahlen.Length)
                {
                    if (zahlen[n] < min)
                    {
                        min = zahlen[n];
                    }
                    n++;
                }
            }

            return min;
        }

        /// <summary>
        /// Sucht das Maximum aus einem Array von Zahlen
        /// </summary>
        /// <param name="zahlen"></param>
        /// <returns></returns>
        static double FindeMaximum(double[] zahlen)
        {
            double max = 0;
            if (zahlen.Length > 0)
            {
                max = zahlen[0];
                int n = 0;
                while (n < zahlen.Length)
                {
                    if (zahlen[n] > max)
                    {
                        max = zahlen[n];
                    }
                    n++;
                }
            }

            return max;
        }

        /// <summary>
        /// Berechnet die Summe eines Arrays von Zahlen
        /// </summary>
        /// <param name="zahlen"></param>
        /// <returns></returns>
        static double BerechneSumme(double[] zahlen)
        {
            double summe = 0;

            foreach (double zahl in zahlen)
            {
                summe += zahl;
            }

            return summe;
        }

        /// <summary>
        /// Berechnet den Durchschnitt eines Arrays von Zahlen
        /// </summary>
        /// <param name="zahlen"></param>
        /// <returns></returns>
        static double BerechneDurchschnitt(double[] zahlen)
        {
            return BerechneSumme(zahlen) / zahlen.Length;
        }

    }
}
