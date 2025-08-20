namespace MeinTaschenrechner
{
    public class Mehrfachrechner
    {
        static Hilfsfunktionen help = new Hilfsfunktionen();
        static Historiemenu historiemenu = new Historiemenu();
        static Datenbankmenu datenbankmenu = new Datenbankmenu();

        /// <summary>
        /// Zeigt das Mehrfachberechnungsmenü an, wertet die Eingabe aus und führt die dazugehörige Funktion aus
        /// </summary>
        public void MehrfachBerechnungenMenu()
        {
            bool mehrfachBerechnungenAktiv = true;
            while (mehrfachBerechnungenAktiv)
            {
                help.Mischen();
                help.Write("\n=== MEHRFACH-BERECHNUNGEN ===");
                help.Write("1. Addition");
                help.Write("2. Subtraktion");
                help.Write("3. Multiplikation");
                help.Write("4. Division");
                help.Write("5. Zurück zum Rechenmenü");
                help.Write("Deine Wahl (1-5): ");
                int wahl = help.MenuWahlEinlesen();
                switch (wahl)
                {
                    case 1:
                        MehrfachAddition();
                        break;
                    case 2:
                        MehrfachSubtraktion();
                        break;
                    case 3:
                        MehrfachMultiplikation();
                        break;
                    case 4:
                        MehrfachDividieren();
                        break;
                    case 5:
                        mehrfachBerechnungenAktiv = false;
                        help.Write("Zurückzum Hauptmenü.");
                        break;
                    default:
                        help.Write("UngültigeWahl!");
                        break;
                }
                if (mehrfachBerechnungenAktiv)
                {
                    help.Write("\nDrücke Enter fürMenü...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Addiert mindestens zwei Zahlen miteinander
        /// </summary>
        static void MehrfachAddition()
        {
            help.Write("\n===MEHRFACH-ADDITION ===");
            help.Write("Gibmehrere Zahlenein (beendemit 'fertig'):");

            List<double> zahlen = new List<double>();

            while (true)
            {
                help.Write($"Zahl {zahlen.Count + 1}: ");
                string eingabe = Console.ReadLine();

                if (eingabe.ToLower() == "fertig")
                    break;

                if (int.TryParse(eingabe, out int zahl))
                {
                    zahlen.Add(zahl);
                }
            }

            if (zahlen.Count < 2)
            {
                help.Write("Mindestens 2 Zahlen erforderlich!");
                return;
            }

            double summe = zahlen.Sum();

            string berechnung = $"{string.Join(" + ", zahlen)} = {summe}";
            help.Write($"Ergebnis: {berechnung}");

            historiemenu.BerechnungHinzufuegen("+", zahlen.ToArray(), summe);

            datenbankmenu.BerechnungInDatenbankSpeichern("+", zahlen.ToArray(), summe);

            historiemenu.HistorieHinzufuegen(berechnung);
        }

        /// <summary>
        /// Subtrahiert mindestens zwei Zahlen voneinander
        /// </summary>
        static void MehrfachSubtraktion()
        {
            help.Write("\n=== MEHRFACH-SUBTRAKTION ===");
            help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");
            List<double> zahlen = new List<double>();

            while (true)
            {
                help.Write($"Zahl {zahlen.Count + 1}: ");
                string eingabe = Console.ReadLine();
                if (eingabe.ToLower() == "fertig")
                    break;
                if (double.TryParse(eingabe, out double zahl))
                {
                    zahlen.Add(zahl);
                }
            }

            if (zahlen.Count < 2)
            {
                help.Write("Mindestens zwei Zahlen erforderlich!");
                return;
            }

            double ergebnis = zahlen[0];

            for (int i = 1; i < zahlen.Count; i++)
            {
                ergebnis -= zahlen[i];
            }

            string berechnung = $"{string.Join(" - ", zahlen)} = {ergebnis}";
            help.Write($"Ergebnis: {berechnung}");

            historiemenu.BerechnungHinzufuegen("-", zahlen.ToArray(), ergebnis);

            datenbankmenu.BerechnungInDatenbankSpeichern("-", zahlen.ToArray(), ergebnis);

            historiemenu.HistorieHinzufuegen(berechnung);
        }

        /// <summary>
        /// Multipliziert mindestens zwei Zahlen miteinander
        /// </summary>
        static void MehrfachMultiplikation()
        {
            help.Write("\n=== MEHRFACH-MULTIPLIKATION ===");
            help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

            List<double> zahlen = new List<double>();

            while (true)
            {
                help.Write($"Zahl {zahlen.Count + 1}: ");
                string eingabe = Console.ReadLine();
                if (eingabe.ToLower() == "fertig")
                    break;
                if (double.TryParse(eingabe, out double zahl))
                {
                    zahlen.Add(zahl);
                }
            }

            if (zahlen.Count < 2)
            {
                help.Write("Mindestens zwei Zahlen erforderlich!");
                return;
            }

            double ergebnis = 1;

            foreach (var zahl in zahlen)
            {
                ergebnis *= zahl;
            }

            string berechnung = $"{string.Join(" * ", zahlen)} = {ergebnis}";
            help.Write($"Ergebnis: {berechnung}");

            historiemenu.BerechnungHinzufuegen("*", zahlen.ToArray(), ergebnis);

            datenbankmenu.BerechnungInDatenbankSpeichern("*", zahlen.ToArray(), ergebnis);

            historiemenu.HistorieHinzufuegen(berechnung);
        }

        /// <summary>
        /// Dividiert mindestens zwei Zahlen durcheinander
        /// </summary>
        static void MehrfachDividieren()
        {
            help.Write("\n=== MEHRFACH-DIVISION ===");
            help.Write("Gib mehrere Zahlen ein (beende mit 'fertig'):");

            List<double> zahlen = new List<double>();

            while (true)
            {
                help.Write($"Zahl {zahlen.Count + 1}: ");
                string eingabe = Console.ReadLine();
                if (eingabe.ToLower() == "fertig")
                    break;
                if (double.TryParse(eingabe, out double zahl))
                {
                    zahlen.Add(zahl);
                }
            }

            if (zahlen.Count < 2)
            {
                help.Write("Mindestens zwei Zahlen erforderlich!");
                return;
            }

            double ergebnis = zahlen[0];

            for (int i = 1; i < zahlen.Count; i++)
            {
                if (zahlen[i] == 0)
                {
                    help.Write("Fehler: Division durch Null ist nicht möglich!");
                    return;
                }
                ergebnis /= zahlen[i];
            }

            string berechnung = $"{string.Join(" / ", zahlen)} = {ergebnis}";
            help.Write($"Ergebnis: {berechnung}");

            historiemenu.BerechnungHinzufuegen("/", zahlen.ToArray(), ergebnis);

            datenbankmenu.BerechnungInDatenbankSpeichern("/", zahlen.ToArray(), ergebnis);

            historiemenu.HistorieHinzufuegen(berechnung);
        }
    }
}