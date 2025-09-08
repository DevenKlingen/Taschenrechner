using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class RechnerManager
{
    private Dictionary<RechnerTyp, BaseRechner> aktiveRechner;
    private BaseRechner aktuellerRechner;
    private Hilfsfunktionen _help;

    public BaseRechner AktuellerRechner
    {
        get { return aktuellerRechner; }
        private set { aktuellerRechner = value; }
    }

    public RechnerManager(Hilfsfunktionen help)
    {
        aktiveRechner = new Dictionary<RechnerTyp, BaseRechner>();
        _help = help;
    }

    public BaseRechner WechsleZuRechner(RechnerTyp typ)
    {
        // Lazy Loading: Rechner nur bei Bedarf erstellen
        if (!aktiveRechner.ContainsKey(typ))
        {
            aktiveRechner[typ] = RechnerFactory.ErstelleRechner(typ);
            _help.Write($"{typ}-Rechner wurde erstellt.");
        }

        AktuellerRechner = aktiveRechner[typ];
        _help.Write($"Gewechselt zu: {AktuellerRechner.RechnerTyp}");

        return AktuellerRechner;
    }

    public void ZeigeAktiveRechner()
    {
        _help.Write("\n=== AKTIVE RECHNER ===");

        if (aktiveRechner.Count == 0)
        {
            _help.Write("Keine Rechner aktiv.");
            return;
        }

        foreach (var kvp in aktiveRechner)
        {
            string marker = (kvp.Value == AktuellerRechner) ? " [AKTIV]" : "";
            _help.Write($"- {kvp.Key}: {kvp.Value.AnzahlBerechnungen} Berechnungen{marker}");
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
            _help.Write($"{typ}-Rechner wurde geschlossen.");
        }
    }

    public void SchliesseAlleRechner()
    {
        aktiveRechner.Clear();
        AktuellerRechner = null;
        _help.Write("Alle Rechner wurden geschlossen.");
    }
}