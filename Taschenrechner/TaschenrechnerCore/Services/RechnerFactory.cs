using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Interfaces;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

// Factory-Klasse für Rechner-Erstellung
public static class RechnerFactory
{
    static Hilfsfunktionen _help;
    static RechnerManager _rechnerManager;
    static BenutzerManagement _benutzerManagement;
    static DatenbankBerechnungen _datenbankBerechnungen;

    // Static Dictionary für Rechner-Registry
    private static readonly Dictionary<RechnerTyp, Func<BaseRechner>> rechnerRegistry
        = new Dictionary<RechnerTyp, Func<BaseRechner>>();

    // Static Constructor für Initialisierung
    static RechnerFactory()
    {

        RegistriereRechner();
    }

    public static void setHelp(Hilfsfunktionen hilfsfunktionen)
    {
        _help = hilfsfunktionen;
    }
    public static void setDatenbankBerechnungen(DatenbankBerechnungen datenbankBerechnungen)
    {
        _datenbankBerechnungen = datenbankBerechnungen;
    }

    public static void setRechnerManager(RechnerManager rechnerManager)
    {
        _rechnerManager = rechnerManager;
    }

    public static void setBenutzerManagement(BenutzerManagement benutzerManager)
    {
        _benutzerManagement = benutzerManager;
    }

    private static void RegistriereRechner()
    {
        rechnerRegistry[RechnerTyp.Basis] = () => new BasisRechner(_benutzerManagement, _datenbankBerechnungen);
        rechnerRegistry[RechnerTyp.Wissenschaftlich] = () => new WissenschaftlicherRechner(_benutzerManagement, _datenbankBerechnungen);
        rechnerRegistry[RechnerTyp.Finanz] = () => new FinanzRechner(_benutzerManagement, _datenbankBerechnungen);
        rechnerRegistry[RechnerTyp.Matrix] = () => new MatrixRechner(_help, _rechnerManager, _benutzerManagement, _datenbankBerechnungen);
        rechnerRegistry[RechnerTyp.Statistik] = () => new StatistikRechner(_benutzerManagement, _datenbankBerechnungen, _help);
    }

    public static BaseRechner ErstelleRechner(RechnerTyp typ)
    {
        if (rechnerRegistry.ContainsKey(typ))
        {
            return rechnerRegistry[typ]();
        }

        throw new NotSupportedException($"Rechner-Typ '{typ}' wird nicht unterstützt.");
    }

    public static BaseRechner ErstelleRechner(string typName)
    {
        if (Enum.TryParse<RechnerTyp>(typName, true, out RechnerTyp typ))
        {
            return ErstelleRechner(typ);
        }

        throw new ArgumentException($"Unbekannter Rechner-Typ: '{typName}'");
    }

    public static List<string> GetVerfuegbareRechnerTypen()
    {
        return Enum.GetNames(typeof(RechnerTyp)).ToList();
    }

    public static void ZeigeVerfuegbareRechner()
    {
        _help.Write("\n=== VERFÜGBARE RECHNER ===");
        var typen = GetVerfuegbareRechnerTypen();

        for (int i = 0; i < typen.Count; i++)
        {
            _help.Write($"{i + 1}. {typen[i]}");
        }
    }
}