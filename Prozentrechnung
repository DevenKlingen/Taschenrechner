namespace MeinTaschenrechner
{
    public class Prozentrechnung
    {
        static Hilfsfunktionen help = new Hilfsfunktionen();
        static Historiemenu historiemenu = new Historiemenu();
        static Datenbankmenu datenbankmenu = new Datenbankmenu();

        /// <summary>
        /// Zeigt das Menü für Prozentrechnung an, wertet die Eingabe aus und führt die dazugehörige Funktion aus
        /// </summary>
        public void ProzentMenu()
        {
            bool prozentRechnerAktiv = true;
            while (prozentRechnerAktiv)
            {
                help.Mischen();

                help.Write("\n=== Prozentrechner ===");
                help.Write("1. Prozentrechnung");
                help.Write("2. Prozentualer Anteil");
                help.Write("3. Grundwert");
                help.Write("4. Zurück zum Rechenmenü");
                help.Write("Deine Wahl (1-4): ");
                int wahl = help.MenuWahlEinlesen();
                switch (wahl)
                {
                    case 1:
                        Prozentwert();
                        break;
                    case 2:
                        ProzentualerAnteil();
                        break;
                    case 3:
                        Grundwert();
                        break;
                    case 4:
                        prozentRechnerAktiv = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }

                if (prozentRechnerAktiv)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Berechnet den Prozentwert anhand des gegebenen Grundwertes und Prozentsatzes
        /// </summary>
        static void Prozentwert()
        {
            double grundwert = help.ZahlEinlesen("Gib den Grundwert ein: ");
            double prozentsatz = help.ZahlEinlesen("Gib den Prozentsatz ein: ");
            double prozentwert = (prozentsatz / 100) * grundwert;

            help.Write($"Der Prozentwert von {grundwert} bei {prozentsatz}% ist {prozentwert}");

            string berechnung = $"({prozentsatz} / {100}) * {grundwert} = {prozentwert}";
            historiemenu.HistorieHinzufuegen(berechnung);

            double[] eingaben = { prozentsatz, 100, grundwert };
            historiemenu.BerechnungHinzufuegen("/, *", eingaben, prozentwert);

            datenbankmenu.BerechnungInDatenbankSpeichern("/, *", eingaben, prozentwert);
        }

        /// <summary>
        /// Berechnet den Prozentsatz anhand des gegebenen Grundwertes und Prozentwertes
        /// </summary>
        static void ProzentualerAnteil()
        {
            double grundwert = help.ZahlEinlesen("Gib den Grundwert ein: ");
            double prozentwert = help.ZahlEinlesen("Gib den Prozentwert ein: ");
            double prozentsatz = (prozentwert / grundwert) * 100;

            help.Write($"Der Prozentsatz von {prozentwert} bei {grundwert} ist {prozentsatz}%");

            string berechnung = $"({prozentwert} / {grundwert}) * {100} = {prozentsatz}";
            historiemenu.HistorieHinzufuegen(berechnung);

            double[] eingaben = { prozentwert, grundwert, 100 };
            historiemenu.BerechnungHinzufuegen("/, *", eingaben, prozentsatz);

            datenbankmenu.BerechnungInDatenbankSpeichern("/, *", eingaben, prozentsatz);
        }

        /// <summary>
        /// Berechnet den Grundwert anhand des gegebenen Prozentwertes und Prozentsatzes
        /// </summary>
        static void Grundwert()
        {
            double prozentwert = help.ZahlEinlesen("Gib den Prozentwert ein: ");
            double prozentsatz = help.ZahlEinlesen("Gib den Prozentsatz ein: ");
            double grundwert = (prozentwert * 100) / prozentsatz;

            help.Write($"Der Grundwert von {prozentwert} bei {prozentsatz}% ist {grundwert}");

            string berechnung = $"({prozentwert} * {100}) / {prozentsatz} = {grundwert} ";
            historiemenu.HistorieHinzufuegen(berechnung);

            double[] eingaben = { prozentwert, 100, prozentsatz };
            historiemenu.BerechnungHinzufuegen("*, /", eingaben, grundwert);

            datenbankmenu.BerechnungInDatenbankSpeichern("*, /", eingaben, grundwert);
        }
    }
}
