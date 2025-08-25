using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;

namespace TaschenrechnerCore.Services;

public class KonfigBearbeiten
{
    static Hilfsfunktionen help = new Hilfsfunktionen();
    static KonfigVerwaltung konfigV = new KonfigVerwaltung();
    static BenutzerManagement benutzerManagement = new();
    static BenutzerEinstellungen benutzerEinstellungen = new();

    /// <summary>
    /// Erlaubt dem Nutzer, die Konfiguration zu ändern
    /// </summary>
    public void KonfigurationAendern()
    {
        help.Mischen();
        Benutzer akt = benutzerManagement.getBenutzer();

        using var context = new TaschenrechnerContext();
        var userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Thema");

        help.Write("=== KONFIGURATION ÄNDERN ===");

        Console.Write($"Thema ({benutzerEinstellungen.config.Thema}): ");
        string eingabe = Console.ReadLine().ToLower();

        switch (eingabe)
        {
            case "hell":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                benutzerEinstellungen.config.Thema = "Hell";
                userSetting.Wert = "Hell";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "dunkel":
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                benutzerEinstellungen.config.Thema = "Dunkel";
                userSetting.Wert = "Dunkel";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "grün":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Green;
                benutzerEinstellungen.config.Thema = "Grün";
                userSetting.Wert = "Grün";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "gelb":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Yellow;
                benutzerEinstellungen.config.Thema = "Gelb";
                userSetting.Wert = "Gelb";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "blau":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Blue;
                benutzerEinstellungen.config.Thema = "Blau";
                userSetting.Wert = "Blau";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "rot":
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                benutzerEinstellungen.config.Thema = "Rot";
                userSetting.Wert = "Rot";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "lila":
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                benutzerEinstellungen.config.Thema = "Lila";
                userSetting.Wert = "Lila";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "bunt":
                benutzerEinstellungen.config.Thema = "Bunt";
                userSetting.Wert = "Bunt";
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            case "matrix":
                benutzerEinstellungen.config.Thema = "Matrix";
                userSetting.Wert = "Matrix";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                context.SaveChanges();
                Console.Clear();
                Console.WriteLine("Farbe wurde geändert!");
                break;
            default:
                help.Write("Ungültige Wahl!");
                break;
        }

        help.Write($"Nachkommastellen ({benutzerEinstellungen.config.Nachkommastellen}): ");
        eingabe = Console.ReadLine();

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Nachkommastellen");

        if (int.TryParse(eingabe, out int stellen) && stellen >= 0 && stellen <= 10)
        {
            benutzerEinstellungen.config.Nachkommastellen = stellen;
            userSetting.Wert = stellen.ToString();
            context.SaveChanges();
        }

        help.Write($"Standardrechner ({benutzerEinstellungen.config.Standardrechner}): ");
        eingabe = Console.ReadLine();

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Standardrechner");

        if (eingabe == "Basis" || eingabe == "Wissenschaftlich")
        {
            benutzerEinstellungen.config.Standardrechner = eingabe;
            userSetting.Wert = eingabe;
            context.SaveChanges();
        }

        help.Write($"Auto-Speichern (j/n, aktuell: {(benutzerEinstellungen.config.AutoSpeichern ? "j" : "n")}): ");
        eingabe = Console.ReadLine().ToLower();

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "AutoSpeichern");

        if (eingabe == "j" || eingabe == "n")
        {
            benutzerEinstellungen.config.AutoSpeichern = eingabe == "j";
            userSetting.Wert = benutzerEinstellungen.config.AutoSpeichern ? "j" : "n";
            context.SaveChanges();
        }

        help.Write($"Sprache ({benutzerEinstellungen.config.Sprache}): ");
        eingabe = Console.ReadLine();

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "Sprache");

        if (eingabe == "Deutsch" || eingabe == "Englisch" || eingabe == "Spanisch" || eingabe == "Italienisch" || eingabe == "Französisch")
        {
            benutzerEinstellungen.config.Sprache = eingabe;
            userSetting.Wert = eingabe;
            context.SaveChanges();
        }

        help.Write($"Zeige Zeitstempel (j/n, aktuell: {(benutzerEinstellungen.config.ZeigeZeitstempel ? "j" : "n")}): ");
        eingabe = Console.ReadLine().ToLower();

        userSetting = context.Einstellungen
            .FirstOrDefault(us => us.BenutzerId == akt.Id && us.Schluessel == "ZeigeZeitstempel");

        if (eingabe == "j" || eingabe == "n")
        {
            benutzerEinstellungen.config.ZeigeZeitstempel = eingabe == "j";
            userSetting.Wert = benutzerEinstellungen.config.ZeigeZeitstempel ? "j" : "n";
            context.SaveChanges();
        }

        var aktuellerBenutzer = benutzerManagement.getBenutzer();
        string konfig = "Konfig";
        string KonfigOrdner = Path.Combine(aktuellerBenutzer.Name, konfig);
        if (!Directory.Exists(KonfigOrdner))
        {
            Directory.CreateDirectory(KonfigOrdner);
        }

        konfigV.KonfigurationSpeichern(); // Erstellt die Datei, falls sie nicht existiert

        help.Write("Konfiguration aktualisiert!");
    }

    /// <summary>
    /// Setzt die Pfade für die Konfigurationsdateien
    /// </summary>
    public void SetzeKonfigDateiPfad()
    {
        var akt = benutzerManagement.getBenutzer();
        string konfigOrdner = $"Benutzer/{akt.Name}/Konfig";

        if (!Directory.Exists(Path.Combine(konfigOrdner)))
        {
            Directory.CreateDirectory(Path.Combine(konfigOrdner));
        }

        konfigV.konfigJson = Path.Combine(konfigOrdner, "config.json");
        konfigV.konfigToml = Path.Combine(konfigOrdner, "config.toml");
    }
}