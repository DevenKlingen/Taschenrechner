using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class RechnerManager
{
    private Dictionary<RechnerTyp, BaseRechner> aktiveRechner;
    private BaseRechner aktuellerRechner;
    private Hilfsfunktionen _help;
    private BenutzerManagement _benutzerManagement;
    private DatenbankBerechnungen _datenbankBerechnungen;

    public BaseRechner AktuellerRechner
    {
        get { return aktuellerRechner; }
        private set { aktuellerRechner = value; }
    }

    public RechnerManager(Hilfsfunktionen help, BenutzerManagement benutzerManagement, DatenbankBerechnungen datenbankBerechnungen)
    {
        aktiveRechner = new Dictionary<RechnerTyp, BaseRechner>();
        _help = help;
        _benutzerManagement = benutzerManagement;
        _datenbankBerechnungen = datenbankBerechnungen;
        RechnerFactory.Initialisiere(_help, this, _benutzerManagement, _datenbankBerechnungen);
    }

    public BaseRechner WechsleZuRechner(RechnerTyp typ)
    {
        // Lazy Loading: Rechner nur bei Bedarf erstellen
        if (!aktiveRechner.ContainsKey(typ))
        {
            aktiveRechner[typ] = RechnerFactory.ErstelleRechner(typ);
            _help.WriteInfo($"{typ}-Rechner wurde erstellt.");
        }

        AktuellerRechner = aktiveRechner[typ];
        _help.WriteInfo($"Gewechselt zu: {AktuellerRechner.RechnerTyp}");

        return AktuellerRechner;
    }

    public void ZeigeAktiveRechner()
    {
        _help.WriteInfo("\n=== AKTIVE RECHNER ===");

        if (aktiveRechner.Count == 0)
        {
            _help.WriteWarning("Keine Rechner aktiv.");
            return;
        }

        foreach (var kvp in aktiveRechner)
        {
            string marker = (kvp.Value == AktuellerRechner) ? " [AKTIV]" : "";
            _help.WriteInfo($"- {kvp.Key}: {kvp.Value.AnzahlBerechnungen} Berechnungen{marker}");
        }
    }

    public void SchliesseRechner(RechnerTyp typ)
    {
        if (aktiveRechner.ContainsKey(typ))
        {
            if (aktiveRechner[typ] == AktuellerRechner)
            {
                AktuellerRechner = null;
            }

            aktiveRechner.Remove(typ);
            _help.WriteInfo($"{typ}-Rechner wurde geschlossen.");
        }
    }

    public void SchliesseAlleRechner()
    {
        aktiveRechner.Clear();
        AktuellerRechner = null;
        _help.WriteInfo("Alle Rechner wurden geschlossen.");
    }

    public void RechnerWechseln()
    {
        _help.WriteInfo("=== RECHNER WECHSELN ===");
        RechnerFactory.ZeigeVerfuegbareRechner();

        int wahl = (int)_help.ZahlEinlesen("Rechner wählen (Nummer): ");

        var verfuegbareTypen = RechnerFactory.GetVerfuegbareRechnerTypen();

        if (wahl >= 1 && wahl <= verfuegbareTypen.Count)
        {
            string typName = verfuegbareTypen[wahl - 1];
            RechnerTyp typ = (RechnerTyp)Enum.Parse(typeof(RechnerTyp), typName);
            WechsleZuRechner(typ);
        }
        else
        {
            _help.WriteWarning("Ungültige Wahl!");
        }
    }
}