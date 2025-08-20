using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace MeinTaschenrechner
{
    public class Historiemenu
    {
        static Program programm = new Program();
        static Hilfsfunktionen help = new Hilfsfunktionen();

        /// <summary>
        /// Zeigt das Historiemenü an, wertet die Eingabe aus und führt die dazugehörige Funktion aus
        /// </summary>
        public void HistorieMenu()
        {
            bool historieMenuAktiv = true;
            while (historieMenuAktiv)
            {
                help.Mischen();

                help.Write("\n=== HISTORIE ===");
                help.Write("1. Historie ansehen");
                help.Write("2. Historie speichern");
                help.Write("3. Historie löschen");
                help.Write("4. Als XML Exportieren");
                help.Write("5. Von XML Importieren");
                help.Write("6. Als CSV Exportieren");
                help.Write("7. Zurück zum Hauptmenü");
                help.Write("Deine Wahl (1-7): ");
                int wahl = help.MenuWahlEinlesen();
                switch (wahl)
                {
                    case 1:
                        HistorieAnzeigen();
                        break;
                    case 2:
                        HistorieSpeichern();
                        break;
                    case 3:
                        HistorieLöschen();
                        break;
                    case 4:
                        HistorieAlsXMLExportieren();
                        break;
                    case 5:
                        HistorieVonXMLImportieren();
                        break;
                    case 6:
                        HistorieAlsCSVExportieren();
                        break;
                    case 7:
                        historieMenuAktiv = false;
                        help.Write("Zurück zum Hauptmenü.");
                        break;
                    default:
                        help.Write("Ungültige Wahl!");
                        break;
                }

                if (historieMenuAktiv)
                {
                    help.Write("\nDrücke Enter für Menü...");
                    Console.ReadLine();
                }
            }
        }

        /// <summary>
        /// Zeigt die aktuelle Historie an
        /// </summary>
        static void HistorieAnzeigen()
        {
            help.Write("\nMöchtest du die Historie aus der Datei (1) oder aus der DB (2)?");
            double.TryParse(Console.ReadLine(), out double wahl);
            if (wahl == 1)
            {
                help.Write("\n=== BERECHNUNGSHISTORIE ===");
                if (programm.berechnungsHistorie.Count == 0)
                {
                    help.Write("Keine Berechnungen durchgeführt.");
                    return;
                }
                else
                {
                    foreach (var eintrag in programm.berechnungsHistorie)
                    {
                        help.Write(eintrag);
                    }
                }
            }
            else if (wahl == 2)
            {
                bool dbHistorie = true;
                int seite = 1;
                int eintraege = 10;

                while (dbHistorie)
                {
                    help.Write("\n=== BERECHNUNGSHISTORIE AUS DER DB ===");
                    help.Write("Vorige Seite: <");
                    help.Write("Nächste Seite: >");
                    help.Write("Beenden: 0");
                    help.Write("Seite " + seite + ":");

                    using var context = new TaschenrechnerContext();

                    var berechnungen = context.Berechnungen
                        .OrderBy(b => b.Zeitstempel)
                        .Skip((seite - 1) * eintraege)
                        .Take(eintraege)
                        .ToList();

                    if (berechnungen.Count == 0)
                    {
                        help.Write("Keine Berechnungen in der Datenbank gefunden.");
                        dbHistorie = false;
                        continue;
                    }

                    foreach (var berechnung in berechnungen)
                    {
                        try
                        {
                            double[] eingaben = JsonSerializer.Deserialize<double[]>(berechnung.Eingaben);
                            if (berechnung.Operation == "$")
                            {
                                Console.OutputEncoding = Encoding.UTF8;
                                help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingaben[0]}€ = ${berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                            }
                            else if (berechnung.Operation == "/, *")
                            {
                                help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} / {eingaben[1]}) * {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                            }
                            else if (berechnung.Operation == "*, /")
                            {
                                help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"({eingaben[0]} * {eingaben[1]}) / {eingaben[2]} = {berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                            }
                            else
                            {
                                string eingabenStr = string.Join($" {berechnung.Operation} ", eingaben);

                                help.Write($"[{berechnung.Zeitstempel:HH:mm:ss}] " + $"{eingabenStr} = {berechnung.Ergebnis.ToString($"F{programm.config.Nachkommastellen}")} " + $"({berechnung.Rechnertyp})");
                            }

                            if (!string.IsNullOrEmpty(berechnung.Kommentar))
                            {
                                help.Write($"    Kommentar: {berechnung.Kommentar}");
                            }
                        }
                        catch (Exception ex)
                        {
                            help.Write($"Fehler beim Anzeigen einer Berechnung: {ex.Message}");
                        }
                    }

                    help.Write("");
                    string input = Console.ReadLine();
                    if (input == "<")
                    {
                        if (seite > 1)
                        {
                            seite--;
                        }
                    }
                    else if (input == ">")
                    {
                        seite++;
                    }
                    else if (input == "0")
                    {
                        dbHistorie = false;
                        help.Write("Zurück zum Hauptmenü.");
                    }
                    else
                    {
                        help.Write("Ungültige Eingabe!");
                    }
                }
            }
            else
            {
                help.Write("Ungültige Wahl!");
            }
        }

        /// <summary>
        /// Speichert die Historie in einer Textdatei
        /// </summary>
        public void HistorieSpeichern()
        {
            try
            {
                var akt = programm.getAktBenutzer();
                string pfad = Path.Join("Benutzer", akt.Name, "Backups", programm.historieDatei);

                // Prüfe die Größe der Datei im Backup-Ordner
                if (File.Exists(pfad) && new FileInfo(pfad).Length > 1_000_000)
                {
                    HistorieAlsZipExportieren(pfad);
                    return;
                }

                File.WriteAllLines(pfad, programm.berechnungsHistorie);
                help.Write($"Historie gespeichert in {pfad}");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Speichern: {ex.Message}");
            }
        }

        /// <summary>
        /// Exportiert die Historie als Zip datei
        /// </summary>
        static void HistorieAlsZipExportieren(string pfad)
        {
            try
            {
                var akt = programm.getAktBenutzer();
                string zipDatei = Path.Join("Benutzer", akt.Name, "Backups", "berechnungen.zip");
                using (FileStream fs = new FileStream(zipDatei, FileMode.Create))
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(pfad, Path.GetFileName(pfad));
                    help.Write($"Historie als ZIP exportiert: {zipDatei}");
                }
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim ZIP-Export: {ex.Message}");
            }
        }

        /// <summary>
        /// Löscht die Historie und alle dazugehörigen dateien
        /// </summary>
        static void HistorieLöschen()
        {
            try
            {
                // Benutzerverzeichnis ermitteln
                var akt = programm.getAktBenutzer();
                string benutzerVerzeichnis = Path.Join("Benutzer", akt.Name, "Backups");

                // Allerelevanten Dateinamen/Pattern
                string[] muster = new[]
                {
                    "berechnungen*.txt",
                    "berechnungen*.xml",
                    "berechnungen*.csv",
                    "berechnungen*.zip"
                };

                int geloescht = 0;
                foreach (var pattern in muster)
                {
                    string[] dateien = Directory.GetFiles(benutzerVerzeichnis, pattern);
                    foreach (var datei in dateien)
                    {
                        try
                        {
                            File.Delete(datei);
                            help.Write($"Datei gelöscht:{Path.GetFileName(datei)}");
                            geloescht++;
                        }
                        catch (Exception ex)
                        {
                            help.Write($"Fehler beim Löschen von{datei}: {ex.Message}");
                        }
                    }
                }

                programm.berechnungsHistorie.Clear();
                if (geloescht == 0)
                    help.Write("Keine Historie-Dateien gefunden.");
                else
                    help.Write("Alle Historie-Dateien wurden gelöscht!");

                help.Write("Möchtest du auch die Datenbankhistorie löschen? (j/n)");
                string eingabe = Console.ReadLine()?.ToLower();

                if (eingabe == "j")
                {
                    using var context = new TaschenrechnerContext();
                    var berechnungen = context.Berechnungen.ToList();
                    context.Berechnungen.RemoveRange(berechnungen);
                    context.SaveChanges();
                    help.Write("Datenbankhistorie gelöscht.");
                }
                else if (eingabe != "n")
                {
                    help.Write("Ungültige Eingabe! Es wird keine Datenbankhistorie gelöscht.");
                }
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Löschen:{ex.Message}");
            }
        }

        /// <summary>
        /// Exportiert die Historie als XML-Datei
        /// </summary>
        public void HistorieAlsXMLExportieren()
        {
            try
            {
                var akt = programm.getAktBenutzer();
                string xmlDatei = $"Benutzer/{ akt.Name}/Backups/berechnungen.xml";

                XmlDocument doc = new XmlDocument();

                // XML-Deklaration
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(declaration);

                // Root-Element
                XmlElement root = doc.CreateElement("Berechnungen");
                doc.AppendChild(root);

                foreach (var berechnung in programm.detaillierteBerechnungen)
                {
                    XmlElement berechnungElement = doc.CreateElement("Berechnung");

                    // Zeitstempel
                    XmlElement zeitstempel = doc.CreateElement("Zeitstempel");
                    zeitstempel.InnerText = berechnung.Zeitstempel.ToString("yyyy-MM-dd HH:mm:ss");
                    berechnungElement.AppendChild(zeitstempel);

                    // Operation
                    XmlElement operation = doc.CreateElement("Operation");
                    operation.InnerText = berechnung.Operation;
                    berechnungElement.AppendChild(operation);

                    // Eingaben
                    XmlElement eingaben = doc.CreateElement("Eingaben");
                    foreach (double eingabe in berechnung.Eingaben)
                    {
                        XmlElement eingabeElement = doc.CreateElement("Wert");
                        eingabeElement.InnerText = eingabe.ToString();
                        eingaben.AppendChild(eingabeElement);
                    }
                    berechnungElement.AppendChild(eingaben);

                    // Ergebnis
                    XmlElement ergebnis = doc.CreateElement("Ergebnis");
                    ergebnis.InnerText = berechnung.Ergebnis.ToString();
                    berechnungElement.AppendChild(ergebnis);

                    // Kommentar (falls vorhanden)
                    if (!string.IsNullOrEmpty(berechnung.Kommentar))
                    {
                        XmlElement kommentar = doc.CreateElement("Kommentar");
                        kommentar.InnerText = berechnung.Kommentar;
                        berechnungElement.AppendChild(kommentar);
                    }

                    root.AppendChild(berechnungElement);
                }

                doc.Save(xmlDatei);
                help.Write($"Historie als XML exportiert: {xmlDatei}");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim XML-Export: {ex.Message}");
            }
        }

        /// <summary>
        /// Importiert die Historie aus einer XML-Datei
        /// </summary>
        static void HistorieVonXMLImportieren()
        {
            try
            {
                var akt = programm.getAktBenutzer();
                string xmlDatei = $"Benutzer/{ akt.Name}/Backups/berechnungen.xml";

                if (!File.Exists(xmlDatei))
                {
                    help.Write("Keine XML-Dateizum Importieren gefunden.");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDatei);

                XmlNodeList berechnungNodes = doc.SelectNodes("//Berechnung");
                int importiert = 0;

                foreach (XmlNode node in berechnungNodes)
                {
                    try
                    {
                        var berechnung = new Berechnung();

                        // Zeitstempelparsen
                        string zeitString = node.SelectSingleNode("Zeitstempel")?.InnerText;
                        if (DateTime.TryParse(zeitString, out DateTime zeit))
                        {
                            berechnung.Zeitstempel = zeit;
                        }

                        // Operation
                        berechnung.Operation = node.SelectSingleNode("Operation")?.InnerText ?? "";

                        //Eingaben
                        XmlNodeList eingabeNodes = node.SelectNodes("Eingaben/Wert");
                        foreach (XmlNode eingabeNode in eingabeNodes)
                        {
                            if (double.TryParse(eingabeNode.InnerText, out double wert))
                            {
                                berechnung.Eingaben.Add(wert);
                            }
                        }

                        //Ergebnis
                        string ergebnisString = node.SelectSingleNode("Ergebnis")?.InnerText;
                        if (double.TryParse(ergebnisString, out double ergebnis))
                        {
                            berechnung.Ergebnis = ergebnis;
                        }

                        //Kommentar
                        berechnung.Kommentar = node.SelectSingleNode("Kommentar")?.InnerText ?? "";

                        programm.detaillierteBerechnungen.Add(berechnung);
                        programm.berechnungsHistorie.Add(berechnung.ToString());
                        importiert++;
                    }
                    catch (Exception ex)
                    {
                        help.Write($"Fehler beimImportieren einerBerechnung: {ex.Message}");
                    }
                }

                help.Write($"{importiert} Berechnungen ausXML importiert.");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim XML-Import:{ex.Message}");
            }
        }

        /// <summary>
        /// Exportiert die Historie als CSV-Datei
        /// </summary>
        static void HistorieAlsCSVExportieren()
        {
            try
            {

                var akt = programm.getAktBenutzer();
                string csvDatei = $"Benutzer/{akt.Name}/Backups/berechnungen.csv";

                List<string> csvZeilen = new List<string>();

                // Header
                csvZeilen.Add("Zeitstempel;Operation;Eingaben;Ergebnis;Kommentar");

                foreach (var berechnung in programm.detaillierteBerechnungen)
                {
                    string eingabenString = string.Join(" ", berechnung.Eingaben);
                    string zeile = $"{berechnung.Zeitstempel:yyyy-MM-dd HH:mm:ss};" +
                                  $"{berechnung.Operation};" +
                                  $"{eingabenString};" +
                                  $"{berechnung.Ergebnis};" +
                                  $"{berechnung.Kommentar}";
                    csvZeilen.Add(zeile);
                }

                File.WriteAllLines(csvDatei, csvZeilen);
                help.Write($"Historie als CSV exportiert: {csvDatei}");
                help.Write("Die Datei kann in Excel geöffnet werden.");
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim CSV-Export: {ex.Message}");
            }
        }

        /// <summary>
        /// Lädt die Historie aus einer Datei
        /// </summary>
        public void HistorieLaden()
        {
            try
            {
                if (File.Exists(programm.historieDatei))
                {
                    string[] zeilen = File.ReadAllLines(programm.historieDatei);
                    programm.berechnungsHistorie.AddRange(zeilen);
                    help.Write($"{zeilen.Length} Einträge aus Historie geladen.");
                }
                else
                {
                    help.Write("Keine gespeicherteHistorie gefunden.");
                }
            }
            catch (Exception ex)
            {
                help.Write($"Fehler beim Laden: {ex.Message}");
            }
        }

        /// <summary>
        /// Fügt eine Berechnung zur Historie hinzu
        /// </summary>
        /// <param name="berechnung"></param>
        public void HistorieHinzufuegen(string berechnung)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            programm.berechnungsHistorie.Add($"[{timestamp}] {berechnung}");
        }

        /// <summary>
        /// Fügt eine neue Berechnung zur Historie hinzu und aktualisiert die detaillierte Liste
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="eingaben"></param>
        /// <param name="ergebnis"></param>
        /// <param name="kommentar"></param>
        public void BerechnungHinzufuegen(string operation, double[] eingaben, double ergebnis, string kommentar = "")
        {
            var berechnung = new Berechnung
            {
                Zeitstempel = DateTime.Now,
                Operation = operation,
                Eingaben = eingaben.ToList(),
                Ergebnis = ergebnis,
                Kommentar = kommentar
            };

            programm.detaillierteBerechnungen.Add(berechnung);

            // Alte string-basierte Historie auch aktualisieren
            HistorieHinzufuegen(berechnung.ToString());
        }
    }
}