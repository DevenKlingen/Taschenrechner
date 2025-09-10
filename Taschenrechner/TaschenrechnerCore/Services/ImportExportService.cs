using System.Text.Json;
using System.Xml;
using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Models;
using TaschenrechnerCore.Utils;

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

        public void DatenbankExportieren()
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

                string exportDatei = Path.Join(directory, $"export_{akt.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.json");

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
                    Directory.CreateDirectory(directory);
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
    }
}
