using TaschenrechnerCore.Utils;
using TaschenrechnerCore.Models;
using System.Xml;

namespace TaschenrechnerCore.Services;

public class HistorieImport
{
    private readonly Hilfsfunktionen _help;
    private readonly BenutzerManagement _benutzerManagement;
    private readonly HistorieVerwaltung _historieVerwaltung;
    private readonly BenutzerEinstellungen _benutzerEinstellungen;

    public HistorieImport(
        Hilfsfunktionen help, 
        BenutzerManagement benutzerManagement,
        HistorieVerwaltung historieVerwaltung,
        BenutzerEinstellungen benutzerEinstellungen)
    {
        _help = help;
        _benutzerManagement = benutzerManagement;
        _historieVerwaltung = historieVerwaltung;
        _benutzerEinstellungen = benutzerEinstellungen;
    }



    /// <summary>
    /// Importiert die Historie aus einer XML-Datei
    /// </summary>
    public void HistorieVonXMLImportieren()
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();
            string xmlDatei = $"Benutzer/{akt.Name}/Backups/berechnungen.xml";

            if (!File.Exists(xmlDatei))
            {
                _help.Write("Keine XML-Dateizum Importieren gefunden.");
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
                    berechnung.setBenutzerEinstellungen(_benutzerEinstellungen);

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

                    _historieVerwaltung._detaillierteBerechnungen.Add(berechnung);
                    _historieVerwaltung._berechnungsHistorie.Add(berechnung.ToString());
                    importiert++;
                }
                catch (Exception ex)
                {
                    _help.Write($"Fehler beimImportieren einerBerechnung: {ex.Message}");
                }
            }

            _help.Write($"{importiert} Berechnungen ausXML importiert.");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim XML-Import:{ex.Message}");
        }
    }
}