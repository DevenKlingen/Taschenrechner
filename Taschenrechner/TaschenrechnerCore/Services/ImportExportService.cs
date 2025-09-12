using System.Text.Json;
using System.Xml;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;
using System.Data;

namespace TaschenrechnerCore.Services
{
    public class ImportExportService
    {
        Hilfsfunktionen _help;
        BenutzerManagement _benutzerManagement;
        DatenbankBerechnungen _datenbankBerechnungen;

        public ImportExportService(Hilfsfunktionen help, BenutzerManagement benutzerManagement, DatenbankBerechnungen datenbankBerechnungen)
        {
            _help = help;
            _benutzerManagement = benutzerManagement;
            _datenbankBerechnungen = datenbankBerechnungen;
        }

        public void DatenbankZuJSON()
        {
            Benutzer akt = _benutzerManagement.getBenutzer();

            if (akt == null)
            {
                _help.WriteInfo("Kein Benutzer angemeldet!");
                return;
            }

            string directory = Path.Join("Benutzer", akt.Name, "Datenbank", "JSONexports");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            try
            {
                using var context = new TaschenrechnerContext();

                var berechnungen = context.Berechnungen
                    .Where(b => b.BenutzerId == akt.Id)
                    .OrderBy(b => b.Zeitstempel)
                    .ToList();

                string exportDatei = Path.Join(directory, $"export_{akt.Name}.json");

                var exportData = new
                {
                    Benutzer = akt.Name,
                    ExportiertAm = DateTime.Now,
                    AnzahlBerechnungen = berechnungen.Count,
                    Berechnungen = berechnungen.Select(b => new
                    {
                        b.Zeitstempel,
                        b.Operation,
                        Eingaben = JsonSerializer.Deserialize<double[]>(b.Eingaben),
                        b.Ergebnis,
                        b.Kommentar,
                        b.Rechnertyp
                    })
                };

                string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(exportDatei, json);
                _help.WriteInfo($"Datenbank exportiert nach: {exportDatei}");
            }
            catch (Exception ex)
            {
                _help.WriteError($"Fehler beim Export: {ex.Message}");
            }
        }

        public void DatenbankVonJSON()
        {
            try
            {
                Benutzer akt = _benutzerManagement.getBenutzer();

                if (akt == null)
                {
                    _help.WriteInfo("Kein Benutzer angemeldet!");
                    return;
                }

                string directory = Path.Join("Benutzer", akt.Name, "Datenbank", "JSONexports");

                if (!Directory.Exists(directory))
                {
                    _help.WriteWarning("Es gibt kein Directory!");
                    return;
                }

                string importDatei = Path.Join(directory, $"export_{akt.Name}.json");

                if (!File.Exists(importDatei))
                {
                    _help.WriteWarning("Keine JSON-Datei zum Importieren gefunden.");
                    return;
                }

                string jsonContent = File.ReadAllText(importDatei);

                // JSON in ein Export-Objekt deserialisieren
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var exportData = JsonSerializer.Deserialize<Export>(jsonContent, options);

                if (exportData == null || exportData.Berechnungen == null)
                {
                    _help.WriteWarning("Die JSON-Datei konnte nicht konvertiert werden.");
                    return;
                }

                using var context = new TaschenrechnerContext();

                // Vorhandene Berechnungen des Benutzers löschen
                var zuLoeschendeEintraege = context.Berechnungen
                    .Where(b => b.BenutzerId == akt.Id)
                    .ToList();

                context.Berechnungen.RemoveRange(zuLoeschendeEintraege);
                context.SaveChanges();
                _help.WriteInfo($"Datenbank von {akt.Name} gelöscht. Bereit für den Import.");

                // Neue Berechnungen hinzufügen
                foreach (var berechnung in exportData.Berechnungen)
                {
                    context.Berechnungen.Add(new BerechnungDB
                    {
                        BenutzerId = akt.Id,
                        Zeitstempel = berechnung.Zeitstempel,
                        Operation = berechnung.Operation,
                        Eingaben = JsonSerializer.Serialize(berechnung.Eingaben),
                        Ergebnis = berechnung.Ergebnis,
                        Kommentar = berechnung.Kommentar,
                        Rechnertyp = berechnung.RechnerTyp
                    });
                }

                context.SaveChanges();
                _help.WriteInfo("Die Berechnungen wurden erfolgreich in die Datenbank importiert.");
            }
            catch (Exception ex)
            {
                _help.WriteError($"Fehler beim Importieren: {ex.Message}");
            }
        }

        public void DatenbankZuXML()
        {
            try
            {
                Benutzer akt = _benutzerManagement.getBenutzer();

                if (akt == null)
                {
                    _help.WriteInfo("Kein Benutzer angemeldet!");
                    return;
                }

                string directory = Path.Join("Benutzer", akt.Name, "Datenbank", "XMLexports");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string xmlDatei = Path.Join(directory, "berechnungen.xml");

                XmlDocument doc = new XmlDocument();

                // XML-Deklaration
                XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                doc.AppendChild(declaration);

                // Root-Element
                XmlElement root = doc.CreateElement("Berechnungen");
                doc.AppendChild(root);

                using var context = new TaschenrechnerContext();

                var berechnungen = context.Berechnungen
                    .Where(b => b.BenutzerId == akt.Id)
                    .OrderBy(b => b.Zeitstempel)
                    .ToList();

                var Berechnungen = berechnungen.Select(b => new
                {
                    b.Zeitstempel,
                    b.Operation,
                    Eingaben = JsonSerializer.Deserialize<double[]>(b.Eingaben),
                    b.Ergebnis,
                    b.Kommentar,
                    b.Rechnertyp
                });

                foreach (var berechnung in Berechnungen)
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

                    if (!string.IsNullOrEmpty(berechnung.Rechnertyp))
                    {
                        XmlElement rechnerTyp = doc.CreateElement("RechnerTyp");
                        rechnerTyp.InnerText = berechnung.Rechnertyp;
                        berechnungElement.AppendChild(rechnerTyp);
                    }

                    root.AppendChild(berechnungElement);
                }

                doc.Save(xmlDatei);
                Console.WriteLine($"Historie als XML exportiert: {xmlDatei}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim XML-Export: {ex.Message}");
            }
        }

        public void DatenbankVonXML()
        {
            try
            {
                Benutzer akt = _benutzerManagement.getBenutzer();

                if (akt == null)
                {
                    _help.WriteInfo("Kein Benutzer angemeldet!");
                    return;
                }

                string directory = Path.Join("Benutzer", akt.Name, "Datenbank", "XMLexports");

                if (!Directory.Exists(directory))
                {
                    _help.WriteWarning("Es gibt kein Directory!");
                }

                string xmlDatei = Path.Join(directory, "berechnungen.xml");

                if (!File.Exists(xmlDatei))
                {
                    Console.WriteLine("Keine XML-Datei zum Importieren gefunden.");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlDatei);

                XmlNodeList berechnungNodes = doc.SelectNodes("//Berechnung");
                int importiert = 0;

                using var context = new TaschenrechnerContext();

                var zuLoeschendeEintraege = context.Berechnungen
                    .Where(b => b.BenutzerId == akt.Id)
                    .ToList();

                try
                {
                    context.Berechnungen.RemoveRange(zuLoeschendeEintraege);
                    context.SaveChanges();
                    _help.WriteInfo($"Datenbank von {akt.Name} gelöscht. Bereit für den Import.");
                }
                catch (Exception ex)
                {
                    _help.WriteError($"Fehler beim Löschen: {ex.Message}");
                }

                foreach (XmlNode node in berechnungNodes)
                {
                    try
                    {
                        var berechnung = new Berechnung();

                        // Zeitstempel parsen
                        string zeitString = node.SelectSingleNode("Zeitstempel")?.InnerText;
                        if (DateTime.TryParse(zeitString, out DateTime zeit))
                        {
                            berechnung.Zeitstempel = zeit;
                        }

                        // Operation
                        berechnung.Operation = node.SelectSingleNode("Operation")?.InnerText ?? "";

                        // Eingaben
                        XmlNodeList eingabeNodes = node.SelectNodes("Eingaben/Wert");
                        foreach (XmlNode eingabeNode in eingabeNodes)
                        {
                            if (double.TryParse(eingabeNode.InnerText, out double wert))
                            {
                                berechnung.Eingaben.Add(wert);
                            }
                        }

                        // Ergebnis
                        string ergebnisString = node.SelectSingleNode("Ergebnis")?.InnerText;
                        if (double.TryParse(ergebnisString, out double ergebnis))
                        {
                            berechnung.Ergebnis = ergebnis;
                        }

                        // Kommentar
                        berechnung.Kommentar = node.SelectSingleNode("Kommentar")?.InnerText ?? "";

                        berechnung.RechnerTyp = node.SelectSingleNode("RechnerTyp")?.InnerText;
                        var berechnungErgebnis = new BerechnungErgebnis
                        {
                            Zeitstempel = DateTime.Parse(zeitString),
                            Operation = berechnung.Operation,
                            Eingaben = berechnung.Eingaben,
                            Ergebnis = berechnung.Ergebnis,
                            RechnerTyp = berechnung.RechnerTyp
                        };

                        // In Datenbank speichern (falls Benutzer angemeldet)
                        if (akt != null)
                        {
                            _datenbankBerechnungen.BerechnungInDatenbankSpeichern(berechnungErgebnis.Operation, berechnungErgebnis.Eingaben, berechnungErgebnis.Ergebnis, "", berechnungErgebnis.RechnerTyp);
                        }

                        importiert++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Fehler beim Importieren einer Berechnung: {ex.Message}");
                    }
                }

                Console.WriteLine($"{importiert} Berechnungen aus XML importiert.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim XML-Import: {ex.Message}");
            }
        }

        public void DatenbankzuCSV()
        {
            try
            {
                Benutzer akt = _benutzerManagement.getBenutzer();

                if (akt == null)
                {
                    _help.WriteInfo("Kein Benutzer angemeldet!");
                    return;
                }

                string directory = Path.Join("Benutzer", akt.Name, "Datenbank", "CSVexports");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string csvDatei = Path.Join(directory, "berechnungen.csv");

                List<string> csvZeilen = new List<string>();

                using var context = new TaschenrechnerContext();

                var berechnungen = context.Berechnungen
                    .Where(b => b.BenutzerId == akt.Id)
                    .OrderBy(b => b.Zeitstempel)
                    .ToList();

                var Berechnungen = berechnungen.Select(b => new
                {
                    b.Zeitstempel,
                    b.Operation,
                    Eingaben = JsonSerializer.Deserialize<double[]>(b.Eingaben),
                    b.Ergebnis,
                    b.Kommentar,
                    b.Rechnertyp
                });

                // Header
                csvZeilen.Add("Zeitstempel;Operation;Eingaben;Ergebnis;Kommentar");

                foreach (var berechnung in Berechnungen)
                {
                    string eingabenString = string.Join(" ", berechnung.Eingaben);
                    string zeile = $"{berechnung.Zeitstempel:yyyy-MM-dd HH:mm:ss};" +
                                  $"{berechnung.Operation};" +
                                  $"{eingabenString};" +
                                  $"{berechnung.Ergebnis};" +
                                  $"{berechnung.Kommentar}" +
                                  $"{berechnung.Rechnertyp}";
                    csvZeilen.Add(zeile);
                }

                File.WriteAllLines(csvDatei, csvZeilen);
                Console.WriteLine($"Historie als CSV exportiert: {csvDatei}");
                Console.WriteLine("Die Datei kann in Excel geöffnet werden.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim CSV-Export: {ex.Message}");
            }
        }

        public void DatenbankVonCSV()
        {
            try
            {
                // Benutzer abrufen
                Benutzer akt = _benutzerManagement.getBenutzer();

                if (akt == null)
                {
                    _help.WriteInfo("Kein Benutzer angemeldet!");
                    return;
                }

                // Verzeichnis und Datei prüfen
                string directory = Path.Join("Benutzer", akt.Name, "Datenbank", "CSVexports");

                if (!Directory.Exists(directory))
                {
                    _help.WriteWarning("Das Verzeichnis existiert nicht!");
                    return;
                }

                string csvDatei = Path.Combine(directory, "berechnungen.csv");

                if (!File.Exists(csvDatei))
                {
                    _help.WriteWarning("Die CSV-Datei existiert nicht!");
                    return;
                }

                // CSV-Daten lesen
                var einträge = new List<BerechnungDB>();

                using (var reader = new StreamReader(csvDatei))
                {
                    string headerLine = reader.ReadLine();
                    if (headerLine == null)
                    {
                        throw new Exception("Die CSV-Datei ist leer!");
                    }

                    string[] headers = headerLine.Split(';'); 

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        // Sicherstellen, dass die Anzahl der Werte mit den Headern übereinstimmt
                        if (values.Length != headers.Length)
                        {
                            Console.WriteLine("Ungültige Zeile in der CSV-Datei übersprungen.");
                            continue;
                        }

                        try
                        {
                            // Neue Berechnung erstellen
                            var berechnung = new BerechnungDB
                            {
                                BenutzerId = akt.Id,
                                Zeitstempel = DateTime.Parse(values[0]), 
                                Operation = values[1],
                                Eingaben = SerializeEingaben(values[2]), 
                                Ergebnis = double.Parse(values[3]),
                                Kommentar = values[4],
                                Rechnertyp = values.Length > 5 && !string.IsNullOrEmpty(values[5]) ? values[5] : "Standard"
                            };

                            einträge.Add(berechnung);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Fehler beim Verarbeiten einer Zeile: {ex.Message}");
                        }
                    }
                }

                // Daten in die SQLite-Datenbank einfügen
                using (var context = new TaschenrechnerContext())
                {
                    // Vorhandene Berechnungen des Benutzers löschen
                    var zuLoeschendeEintraege = context.Berechnungen
                        .Where(b => b.BenutzerId == akt.Id)
                        .ToList();

                    if (zuLoeschendeEintraege.Any())
                    {
                        context.Berechnungen.RemoveRange(zuLoeschendeEintraege);
                        context.SaveChanges();
                        _help.WriteInfo($"Vorherige Berechnungen von {akt.Name} gelöscht.");
                    }
                    else
                    {
                        _help.WriteInfo($"Keine vorhandenen Berechnungen von {akt.Name} gefunden.");
                    }

                    // Neue Berechnungen hinzufügen
                    context.Berechnungen.AddRange(einträge);
                    context.SaveChanges();
                }

                _help.WriteInfo("Daten erfolgreich in die Datenbank importiert!");
            }
            catch (Exception ex)
            {
                _help.WriteError($"Fehler beim CSV-Import: {ex.Message}");
            }
        }

        // Hilfsmethode zur sicheren Serialisierung der Eingaben
        private string SerializeEingaben(string eingabenString)
        {
            try
            {
                var eingaben = eingabenString.Split(' ').Select(double.Parse).ToArray();
                return JsonSerializer.Serialize(eingaben);
            }
            catch
            {
                Console.WriteLine("Fehler beim Serialisieren der Eingaben.");
                return "[]"; // Leeres JSON-Array zurückgeben
            }
        }
    }
    
    internal class Export
    {
        public string Benutzer { get; set; }
        public DateTime ExportiertAm { get; set; }
        public int AnzahlBerechnungen { get; set; }
        public List<Berechnung> Berechnungen { get; set; }
    }
}
