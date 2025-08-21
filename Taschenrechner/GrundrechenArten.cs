namespace MeinTaschenrechner
{
    internal class GrundrechenArten
    {
        static Dictionary<string, double> konstanten = new Dictionary<string, double>
        {
            ["pi"] = Math.PI,
            ["e"] = Math.E,
            ["phi"] = 1.618033988749,
            ["wurzel2"] = Math.Sqrt(2)
        };
        static Hilfsfunktionen help = new Hilfsfunktionen();
        static Historiemenu historiemenu = new Historiemenu();
        static Datenbankmenu datenbankmenu = new Datenbankmenu();

        /// <summary>
        /// Addition zweier Zahlen
        /// </summary>
        public void Addieren()
        {
            double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
            double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

            double ergebnis = zahl1 + zahl2;
            help.Write($"Ergebnis: {zahl1} + {zahl2} = {ergebnis}");

            string berechnung = $"{zahl1} + {zahl2} = {ergebnis}";
            historiemenu.HistorieHinzufuegen(berechnung);

            double[] eingaben = { zahl1, zahl2 };
            historiemenu.BerechnungHinzufuegen("+", eingaben, ergebnis);

            datenbankmenu.BerechnungInDatenbankSpeichern("+", eingaben, ergebnis);
        }

        /// <summary>
        /// Subtraktion zweier Zahlen
        /// </summary>
        public void Subtraktion()
        {
            double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
            double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

            double ergebnis = zahl1 - zahl2;
            help.Write($"Ergebnis: {zahl1} - {zahl2} = {ergebnis}");

            string berechnung = $"{zahl1} - {zahl2} = {ergebnis}";
            historiemenu.HistorieHinzufuegen(berechnung);

            double[] eingaben = { zahl1, zahl2 };
            historiemenu.BerechnungHinzufuegen("-", eingaben, ergebnis);

            datenbankmenu.BerechnungInDatenbankSpeichern("-", eingaben, ergebnis);
        }

        /// <summary>
        /// Multiplikation zweier Zahlen
        /// </summary>
        public void Multiplikation()
        {
            double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
            double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

            double ergebnis = zahl1 * zahl2;
            help.Write($"Ergebnis: {zahl1} * {zahl2} = {ergebnis}");

            string berechnung = $"{zahl1} * {zahl2} = {ergebnis}";
            historiemenu.HistorieHinzufuegen(berechnung);

            double[] eingaben = { zahl1, zahl2 };
            historiemenu.BerechnungHinzufuegen("*", eingaben, ergebnis);

            datenbankmenu.BerechnungInDatenbankSpeichern("*", eingaben, ergebnis);
        }

        /// <summary>
        /// Division zweier Zahlen
        /// </summary>
        public void Dividieren()
        {
            double zahl1 = help.ZahlEinlesen("Gib die erste Zahl ein: ");
            double zahl2 = help.ZahlEinlesen("Gib die zweite Zahl ein: ");

            if (zahl2 == 0)
            {
                help.Write("Fehler: Division durch Null ist nicht möglich!");
            }
            else
            {
                double ergebnis = zahl1 / zahl2;
                help.Write($"Ergebnis: {zahl1} / {zahl2} = {ergebnis}");

                string berechnung = $"{zahl1} / {zahl2} = {ergebnis}";
                historiemenu.HistorieHinzufuegen(berechnung);

                double[] eingaben = { zahl1, zahl2 };
                historiemenu.BerechnungHinzufuegen("/", eingaben, ergebnis);

                datenbankmenu.BerechnungInDatenbankSpeichern("/", eingaben, ergebnis);
            }
        }

        /// <summary>
        /// Durchsucht das Dictionary nach einer Konstanten
        /// </summary>
        public void Suche()
        {
            help.Write("Wonach willst du suchen? ");
            string input = Console.ReadLine();

            if (konstanten.ContainsKey(input) && input != null)
            {
                help.Write(konstanten[input].ToString());
            }
        }
    }
}