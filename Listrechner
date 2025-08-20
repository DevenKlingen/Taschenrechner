namespace MeinTaschenrechner
{
    public class Listrechner
    {
        static Hilfsfunktionen help = new Hilfsfunktionen();

        /// <summary>
        /// Zeigt das List Menü, wertet die Eingabe aus und führt die dazugehörige Funktion aus
        /// </summary>
        public void ListRechnerMenu()
        {
            bool listRechnerAktiv = true;

            List<int> liste1 = ZahlenListeEinlesen("Liste 1");
            List<int> liste2 = ZahlenListeEinlesen("Liste 2");

            while (listRechnerAktiv)
            {
                help.Mischen();

                help.Write("\n=== ZAHLEN-LISTEN OPERATIONEN ===");
                help.Write("1. Listen sortieren");
                help.Write("2. Duplikate entfernen");
                help.Write("3. Schnittmenge finden");
                help.Write("4. Vereinigung erstellen");
                help.Write("5. Zurück zum Rechenmenü");
                help.Write("Deine Wahl (1-5): ");
                int wahl = help.MenuWahlEinlesen();

                switch (wahl)
                {
                    case 1:
                        ListenSortieren(liste1, liste2);
                        break;
                    case 2:
                        DuplikateEntfernen(liste1, liste2);
                        break;
                    case 3:
                        Schnittmenge(liste1, liste2);
                        break;
                    case 4:
                        Vereinigung(liste1, liste2);
                        break;
                    case 5:
                        listRechnerAktiv = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }

                if (listRechnerAktiv)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Sortiert zwei Listen
        /// </summary>
        /// <param name="liste1"></param>
        /// <param name="liste2"></param>
        static void ListenSortieren(List<int> liste1, List<int> liste2)
        {
            List<int> sortiert1 = new List<int>(liste1);
            List<int> sortiert2 = new List<int>(liste2);

            sortiert1.Sort();
            sortiert2.Sort();

            help.Write($"Liste 1 sortiert: [{string.Join(", ", sortiert1)}]");
            help.Write($"Liste 2 sortiert: [{string.Join(", ", sortiert2)}]");
        }

        /// <summary>
        /// Entfernt Duplikate aus zwei Listen
        /// </summary>
        /// <param name="liste1"></param>
        /// <param name="liste2"></param>
        static void DuplikateEntfernen(List<int> liste1, List<int> liste2)
        {
            HashSet<int> unique1 = new HashSet<int>(liste1);
            HashSet<int> unique2 = new HashSet<int>(liste2);

            help.Write($"Liste 1 ohne Duplikate: [{string.Join(", ", unique1)}]");
            help.Write($"Liste 2 ohne Duplikate: [{string.Join(", ", unique2)}]");
        }

        /// <summary>
        /// Erstellt die Schnittmenge von zwei Listen
        /// </summary>
        /// <param name="liste1"></param>
        /// <param name="liste2"></param>
        static void Schnittmenge(List<int> liste1, List<int> liste2)
        {
            HashSet<int> unique1 = new HashSet<int>(liste1);
            HashSet<int> unique2 = new HashSet<int>(liste2);

            HashSet<int> schnittmenge = new HashSet<int>(unique1);
            schnittmenge.IntersectWith(unique2);

            help.Write($"Schnittmenge: [{string.Join(", ", schnittmenge)}]");
        }

        /// <summary>
        /// Vereinigt zwei Listen miteinander
        /// </summary>
        /// <param name="liste1"></param>
        /// <param name="liste2"></param>
        static void Vereinigung(List<int> liste1, List<int> liste2)
        {
            HashSet<int> unique1 = new HashSet<int>(liste1);
            HashSet<int> unique2 = new HashSet<int>(liste2);

            HashSet<int> vereinigung = new HashSet<int>(unique1);
            vereinigung.UnionWith(unique2);

            help.Write($"Vereinigung: [{string.Join(", ", vereinigung)}]");
        }

        /// <summary>
        /// Liest eine Liste ein und prüft, ob die Eingabe valide ist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static List<int> ZahlenListeEinlesen(string name)
        {
            help.Write($"\n{name} eingeben (beende mit 'fertig'):");
            List<int> zahlen = new List<int>();

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

            return zahlen;
        }
    }
}