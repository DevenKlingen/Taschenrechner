using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Interfaces;

namespace TaschenrechnerCore.Services;

public class RechnerManager
{
    private Dictionary<RechnerTyp, BaseRechner> aktiveRechner;
    private BaseRechner aktuellerRechner;

    public BaseRechner AktuellerRechner
    {
        get { return aktuellerRechner; }
        private set { aktuellerRechner = value; }
    }

    public RechnerManager()
    {
        aktiveRechner = new Dictionary<RechnerTyp, BaseRechner>();
    }

    public BaseRechner WechsleZuRechner(RechnerTyp typ)
    {
        // Lazy Loading: Rechner nur bei Bedarf erstellen
        if (!aktiveRechner.ContainsKey(typ))
        {
            aktiveRechner[typ] = RechnerFactory.ErstelleRechner(typ);
            Console.WriteLine($"{typ}-Rechner wurde erstellt.");
        }

        AktuellerRechner = aktiveRechner[typ];
        Console.WriteLine($"Gewechselt zu: {AktuellerRechner.RechnerTyp}");

        return AktuellerRechner;
    }

    public void ZeigeAktiveRechner()
    {
        Console.WriteLine("=== AKTIVE RECHNER ===");

        if (aktiveRechner.Count == 0)
        {
            Console.WriteLine("Keine Rechner aktiv.");
            return;
        }

        foreach (var kvp in aktiveRechner)
        {
            string marker = (kvp.Value == AktuellerRechner) ? " [AKTIV]" : "";
            Console.WriteLine($"- {kvp.Key}: {kvp.Value.AnzahlBerechnungen} Berechnungen{marker}");
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
            Console.WriteLine($"{typ}-Rechner wurde geschlossen.");
        }
    }

    public void SchliesseAlleRechner()
    {
        aktiveRechner.Clear();
        AktuellerRechner = null;
        Console.WriteLine("Alle Rechner wurden geschlossen.");
    }
}