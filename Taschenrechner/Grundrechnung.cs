using System.Text;

namespace MeinTaschenrechner
{
    public class Grundrechnung
    {
        static Hilfsfunktionen help = new Hilfsfunktionen();
        static Prozentrechnung prozentrechnung = new Prozentrechnung();
        static MatrixOperationen matrixrechner = new MatrixOperationen();
        static Listrechner listrechner = new Listrechner();
        static Mehrfachrechner mehrfachrechner = new Mehrfachrechner();
        static Historiemenu historiemenu = new Historiemenu();
        static Datenbankmenu datenbankmenu = new Datenbankmenu();
        static GrundrechenArten grundrechenArten = new GrundrechenArten();
        static StatsRechner statistikRechner = new StatsRechner();

        /// <summary>
        /// Zeigt das Rechenmenü, wertet die Eingabe aus und führt die dazugehörige Funktion aus
        /// </summary>
        public void RechenMenu()
        {
            bool programmLaeuft = true;

            while (programmLaeuft)
            {
                help.Mischen();

                help.Write("\n=== RECHENMENÜ ===");
                help.Write("Wähle eine Operation:");
                help.Write("1. Addition");
                help.Write("2. Subtraktion");
                help.Write("3. Multiplikation");
                help.Write("4. Division");
                help.Write("5. Währungsrechner");
                help.Write("6. Potenz");
                help.Write("7. Prozentrechner");
                help.Write("8. Zahl in Binär umwandeln");
                help.Write("9. Zahl in Hexadezimal umwandeln");
                help.Write("10. Statistik-Rechner");
                help.Write("11. Matrix-Rechner");
                help.Write("12. Listen-Rechner");
                help.Write("13. Mehrfach-Berechnungen");
                help.Write("14. Fibonacci-Zahlen");
                help.Write("15. Primzahlen-Rechner");
                help.Write("16. Suche im Dictionary");
                help.Write("17. Zurück zum Hauptmenü");
                help.Write("Deine Wahl (1-17): ");
                int wahl = help.MenuWahlEinlesen();

                switch (wahl)
                {
                    case 1:
                        grundrechenArten.Addieren();
                        break;
                    case 2:
                        grundrechenArten.Subtraktion();
                        break;
                    case 3:
                        grundrechenArten.Multiplikation();
                        break;
                    case 4:
                        grundrechenArten.Dividieren();
                        break;
                    case 5:
                        WaehrungsRechner();
                        break;
                    case 6:
                        Potenz();
                        break;
                    case 7:
                        prozentrechnung.ProzentMenu();
                        break;
                    case 8:
                        ToBinary();
                        break;
                    case 9:
                        ToHexadecimal();
                        break;
                    case 10:
                        statistikRechner.StatsRechnerMenu();
                        break;
                    case 11:
                        matrixrechner.MatrixRechnerMenu();
                        break;
                    case 12:
                        listrechner.ListRechnerMenu();
                        break;
                    case 13:
                        mehrfachrechner.MehrfachBerechnungenMenu();
                        break;
                    case 14:
                        Fibonacci();
                        break;
                    case 15:
                        PrimzahlenRechner();
                        break;
                    case 16:
                        grundrechenArten.Suche();
                        break;
                    case 17:
                        programmLaeuft = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }

                if (programmLaeuft)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Rechnet Euro in Dollar mit einem Wechselkurs von 1.1 um
        /// </summary>
        static void WaehrungsRechner()
        {
            help.Write("\n=== WÄHRUNGSRECHNER ===");
            help.Write("Betrag in Euro: ");
            bool umgerechnet = false;
            while (!umgerechnet)
            {
                if (decimal.TryParse(Console.ReadLine(), out decimal euro))
                {
                    Console.OutputEncoding = Encoding.UTF8;
                    decimal dollar = euro * 1.1m; // Beispiel-Wechselkurs
                    help.Write($"{euro}€ = ${dollar}");

                    string berechnung = $"{euro} + {1.1m} = {dollar}";
                    historiemenu.HistorieHinzufuegen(berechnung);

                    double euroDouble = (double)euro;
                    double dollarDouble = (double)dollar;
                    double[] eingaben = { euroDouble };
                    historiemenu.BerechnungHinzufuegen("$", eingaben, dollarDouble);
                    datenbankmenu.BerechnungInDatenbankSpeichern("$", eingaben, dollarDouble);
                    umgerechnet = true;
                }
                else
                {
                    help.Write("Ungültige Eingabe!");
                }
            }
        }

        /// <summary>
        /// Rechnet eine Potenz einer Zahl aus
        /// </summary>
        static void Potenz()
        {
            double zahl = help.ZahlEinlesen("Gib die erste Zahl ein: ");
            double potenz = help.ZahlEinlesen("Gib die Potenz ein: ");

            double ergebnis = Math.Pow(zahl, potenz);
            help.Write($"Ergebnis: {zahl} ^ {potenz} = {ergebnis}");

            string berechnung = $"{zahl} ^ {potenz} = {ergebnis}";
            historiemenu.HistorieHinzufuegen(berechnung);

            double[] eingaben = { zahl, potenz };
            historiemenu.BerechnungHinzufuegen("^", eingaben, ergebnis);

            datenbankmenu.BerechnungInDatenbankSpeichern("^", eingaben, ergebnis);
        }

        /// <summary>
        /// Wandelt eine Dezimalzahl in die dazugehörige Binärzahl um
        /// </summary>
        static void ToBinary()
        {
            help.Write("Gib eine Zahl ein: ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int zahl))
                {
                    string binär = Convert.ToString(zahl, 2);
                    help.Write($"Die Binärdarstellung von {zahl} ist {binär}");

                    string berechnung = $"{zahl} = {binär}";
                    historiemenu.HistorieHinzufuegen(berechnung);

                    double.TryParse(binär, out double bin);
                    double[] eingaben = { zahl };
                    historiemenu.BerechnungHinzufuegen("binary", eingaben, bin);
                    datenbankmenu.BerechnungInDatenbankSpeichern("binary", eingaben, bin);
                    break;
                }
                else
                {
                    help.Write("Ungültige Eingabe!");
                }
            }
        }

        /// <summary>
        /// Wandelt eine Dezimalzahl in die dazugehörige Hexadezimalzahl um
        /// </summary>
        static void ToHexadecimal()
        {
            help.Write("Gib eine Zahl ein: ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int zahl))
                {
                    string hexadezimal = Convert.ToString(zahl, 16).ToUpper();
                    help.Write($"Die Hexadezimaldarstellung von {zahl} ist {hexadezimal}");

                    string berechnung = $"{zahl} = {hexadezimal}";
                    historiemenu.HistorieHinzufuegen(berechnung);

                    double.TryParse(hexadezimal, out double hex);
                    double[] eingaben = { zahl };
                    historiemenu.BerechnungHinzufuegen("hexadecimal", eingaben, hex);
                    datenbankmenu.BerechnungInDatenbankSpeichern("hexadecimal", eingaben, hex);
                    break;
                }
                else
                {
                    help.Write("Ungültige Eingabe!");
                }
            }
        }

        /// <summary>
        /// Erstellt die Fibonacci-Sequenz der ersten n Zahlen, wobei n vom Nutzer festgelegt wird
        /// </summary>
        static void Fibonacci()
        {
            help.Write("Gib die Anzahl der Fibonacci-Zahlen ein: ");
            string eingabe = Console.ReadLine();

            long.TryParse(eingabe, out long n);

            List<long> fibonacciZahlen = new List<long>();

            for (int i = 0; i < n; i++)
            {
                if (i == 0)
                {
                    fibonacciZahlen.Add(0);
                }
                else if (i == 1)
                {
                    fibonacciZahlen.Add(1);
                }
                else
                {
                    long neueZahl = fibonacciZahlen[i - 1] + fibonacciZahlen[i - 2];
                    fibonacciZahlen.Add(neueZahl);
                }
            }

            help.Write("Fibonacci-Zahlen: " + string.Join(", ", fibonacciZahlen));
        }

        /// <summary>
        /// Berechnet alle Primzahlen bis n, wobei n vom Nutzer festgelegt wird
        /// </summary>
        static void PrimzahlenRechner()
        {
            help.Write("Gib eine Zahl ein: ");
            string eingabe = Console.ReadLine();

            long.TryParse(eingabe, out long zahl);

            List<long> zahlen = new List<long>();

            for (int i = 0; i <= zahl; i++)
            {
                zahlen.Add(i);
            }

            List<long> primzahlen = new List<long>();

            foreach (var num in zahlen)
            {
                if (num < 2) continue; // 0 und 1 sind keine Primzahlen

                bool istPrim = true;

                for (int i = 2; i <= Math.Sqrt(num); i++)
                {
                    if (num % i == 0)
                    {
                        istPrim = false;
                        break;
                    }
                }
                if (istPrim)
                {
                    primzahlen.Add(num);
                }
            }

            help.Write("Primzahlen bis " + zahl + ": " + string.Join(", ", primzahlen));
        }
    }
}