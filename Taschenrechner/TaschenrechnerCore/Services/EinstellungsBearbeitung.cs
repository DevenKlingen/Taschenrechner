using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services
{
    public class EinstellungsBearbeitung : IKonfigurationsService
    {
        Hilfsfunktionen _help;
        BenutzerManagement _benutzerManagement;

        public EinstellungsBearbeitung(Hilfsfunktionen help, BenutzerManagement benutzerManagement)
        {
            _help = help;
            _benutzerManagement = benutzerManagement;
        }

        public void EinstellungenBearbeiten()
        {
            _help.WriteInfo("Welche Einstellung möchtest du bearbeiten?");
            using var context = new TaschenrechnerContext();

            var einstellungen = context.Einstellungen
                .Where(e => e.BenutzerId == _benutzerManagement.getBenutzer().Id)
                .ToList();

            for (int i = 1; i <= einstellungen.Count; i++)
            {
                _help.WriteInfo($"{i}. {einstellungen[i - 1].Schluessel}");
            }

            int wahl = (int)_help.ZahlEinlesen("Deine Wahl: ");

            if (wahl < 1 || wahl > einstellungen.Count)
            {
                _help.WriteWarning("Ungültige Auswahl.");
                return;
            }

            var einstellung = einstellungen[wahl - 1];

            _help.WriteInfo($"{einstellung.Schluessel}: Aktuell: {einstellung.Wert}");

            string neu = _help.Einlesen("Wozu möchtest du es ändern? ");
            EinstellungAendern(einstellung.Schluessel, neu);

            // Wert in DB speichern
            einstellung.Wert = neu;
            context.SaveChanges();
            _help.WriteInfo("Einstellung wurde gespeichert.");
        }

        public void EinstellungAendern(string einstellung, string input)
        {
            input = input.ToLower();

            using var context = new TaschenrechnerContext();
            var benutzerId = _benutzerManagement.getBenutzer().Id;
            var einstellungsObj = context.Einstellungen.FirstOrDefault(e => e.BenutzerId == benutzerId && e.Schluessel.ToLower() == einstellung.ToLower());

            switch (einstellung.ToLower())
            {
                case "standardrechner":
                    if (input == "wissenschaftlich" || input == "basis" || input == "finanz" || input == "matrix" || input == "statistik")
                    {
                        if (einstellungsObj != null)
                        {
                            einstellungsObj.Wert = input;
                            context.SaveChanges();
                        }
                        Console.WriteLine($"Standardrechner wurde auf '{input}' gesetzt.");
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Eingabe für Standardrechner. Erlaubt: 'wissenschaftlich', 'basis', 'finanz', 'matrix', 'statistik'.");
                    }
                    break;
                case "nachkommastellen":
                    if (int.TryParse(input, out int nachkommastellen) && nachkommastellen >= 0)
                    {
                        if (einstellungsObj != null)
                        {
                            einstellungsObj.Wert = nachkommastellen.ToString();
                            context.SaveChanges();
                        }
                        Console.WriteLine($"Nachkommastellen wurden auf {nachkommastellen} gesetzt.");
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Eingabe für Nachkommastellen. Bitte eine positive Zahl eingeben.");
                    }
                    break;
                case "thema":
                    var erlaubteThemen = new[] { "hell", "dunkel", "grün", "gelb", "blau", "rot", "lila", "matrix", "bunt" };
                    if (Array.Exists(erlaubteThemen, thema => thema == input))
                    {
                        if (einstellungsObj != null)
                        {
                            einstellungsObj.Wert = input;
                            context.SaveChanges();
                        }
                        Console.WriteLine($"Thema wurde auf '{input}' gesetzt.");
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Eingabe für Thema. Erlaubte Werte: 'hell', 'dunkel', 'grün', 'gelb', 'blau', 'rot', 'lila', 'matrix', 'bunt'.");
                    }
                    break;

                case "autospeichern":
                    if (input == "j" || input == "n")
                    {
                        bool autospeichern = input == "j";
                        if (einstellungsObj != null)
                        {
                            einstellungsObj.Wert = autospeichern ? "j" : "n";
                            context.SaveChanges();
                        }
                        Console.WriteLine($"Autospeichern wurde auf '{(autospeichern ? "Ja" : "Nein")}' gesetzt.");
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Eingabe für Autospeichern. Erlaubt: 'j' (Ja), 'n' (Nein).");
                    }
                    break;

                case "sprache":
                    var erlaubteSprachen = new[] { "deutsch", "englisch", "französisch" };
                    if (Array.Exists(erlaubteSprachen, sprache => sprache == input))
                    {
                        if (einstellungsObj != null)
                        {
                            einstellungsObj.Wert = input;
                            context.SaveChanges();
                        }
                        Console.WriteLine($"Sprache wurde auf '{input}' gesetzt.");
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Eingabe für Sprache. Erlaubte Werte: 'deutsch', 'englisch', 'französisch'.");
                    }
                    break;

                case "zeigezeitstempel":
                    if (input == "j" || input == "n")
                    {
                        bool zeigeZeitstempel = input == "j";
                        if (einstellungsObj != null)
                        {
                            einstellungsObj.Wert = zeigeZeitstempel ? "j" : "n";
                            context.SaveChanges();
                        }
                        Console.WriteLine($"ZeigeZeitstempel wurde auf '{(zeigeZeitstempel ? "Ja" : "Nein")}' gesetzt.");
                    }
                    else
                    {
                        Console.WriteLine("Ungültige Eingabe für ZeigeZeitstempel. Erlaubt: 'j' (Ja), 'n' (Nein).");
                    }
                    break;

                default:
                    Console.WriteLine("Ungültige Einstellung. Bitte eine gültige Einstellung angeben.");
                    break;
            }
        }

        public void ZeigeEinstellungen()
        {
            using var context = new TaschenrechnerContext();

            var einstellungen = context.Einstellungen
                .Where(e => e.BenutzerId == _benutzerManagement.getBenutzer().Id)
                .ToList();

            foreach (var e in einstellungen)
            {
                _help.WriteInfo($"{e.Schluessel}: {e.Wert}");
            }

            _help.Einlesen("Drücke Enter um zum Menu zurückzukehren... ");
        }
    }
}
