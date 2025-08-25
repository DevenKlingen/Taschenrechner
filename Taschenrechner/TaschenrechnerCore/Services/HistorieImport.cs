using TaschenrechnerConsole;
using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Xml;

namespace TaschenrechnerCore.Services;

public class HistorieImport
{
    static Program program = new Program();
    static Hilfsfunktionen help = new Hilfsfunktionen();

    /// <summary>
    /// Importiert die Historie aus einer XML-Datei
    /// </summary>
    public void HistorieVonXMLImportieren()
    {
        try
        {
            var akt = program.getAktBenutzer();
            string xmlDatei = $"Benutzer/{akt.Name}/Backups/berechnungen.xml";

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

                    program.detaillierteBerechnungen.Add(berechnung);
                    program.berechnungsHistorie.Add(berechnung.ToString());
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
}