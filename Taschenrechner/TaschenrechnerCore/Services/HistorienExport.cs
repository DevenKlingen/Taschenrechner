using System.Xml;
using TaschenrechnerCore.Utils;
using System.IO.Compression;

namespace TaschenrechnerCore.Services;

public class HistorienExport
{
    private readonly Hilfsfunktionen _help;
    private readonly BenutzerManagement _benutzerManagement;
    private readonly HistorieVerwaltung _historieVerwaltung;

    public HistorienExport(
        Hilfsfunktionen help, 
        BenutzerManagement benutzerManagement, 
        HistorieVerwaltung historieVerwaltung)
    {
        _help = help;
        _benutzerManagement = benutzerManagement;
        _historieVerwaltung = historieVerwaltung;
    }



    /// <summary>
    /// Exportiert die Historie als Zip datei
    /// </summary>
    public void HistorieAlsZipExportieren(string pfad)
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();
            string zipDatei = Path.Join("Benutzer", akt.Name, "Backups", "berechnungen.zip");
            using (FileStream fs = new FileStream(zipDatei, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(pfad, Path.GetFileName(pfad));
                _help.Write($"Historie als ZIP exportiert: {zipDatei}");
            }
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim ZIP-Export: {ex.Message}");
        }
    }

    /// <summary>
    /// Exportiert die Historie als XML-Datei
    /// </summary>
    public void HistorieAlsXMLExportieren()
    {
        try
        {
            var akt = _benutzerManagement.getBenutzer();
            string xmlDatei = $"Benutzer/{akt.Name}/Backups/berechnungen.xml";

            XmlDocument doc = new XmlDocument();

            // XML-Deklaration
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declaration);

            // Root-Element
            XmlElement root = doc.CreateElement("Berechnungen");
            doc.AppendChild(root);

            foreach (var berechnung in _historieVerwaltung._detaillierteBerechnungen)
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
            _help.Write($"Historie als XML exportiert: {xmlDatei}");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim XML-Export: {ex.Message}");
        }
    }

    /// <summary>
    /// Exportiert die Historie als CSV-Datei
    /// </summary>
    public void HistorieAlsCSVExportieren()
    {
        try
        {

            var akt = _benutzerManagement.getBenutzer();
            string csvDatei = $"Benutzer/{akt.Name}/Backups/berechnungen.csv";

            List<string> csvZeilen = new List<string>();

            // Header
            csvZeilen.Add("Zeitstempel;Operation;Eingaben;Ergebnis;Kommentar");

            foreach (var berechnung in _historieVerwaltung._detaillierteBerechnungen)
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
            _help.Write($"Historie als CSV exportiert: {csvDatei}");
            _help.Write("Die Datei kann in Excel geöffnet werden.");
        }
        catch (Exception ex)
        {
            _help.Write($"Fehler beim CSV-Export: {ex.Message}");
        }
    }
}