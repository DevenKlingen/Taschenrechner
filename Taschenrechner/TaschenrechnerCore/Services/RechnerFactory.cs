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

    private static readonly Dictionary<RechnerTyp, Func<BaseRechner>> rechnerRegistry
        = new Dictionary<RechnerTyp, Func<BaseRechner>>();

    // Static Constructor für Initialisierung
    public static void Initialisiere(
         Hilfsfunktionen help,
         RechnerManager rechnerManager,
         BenutzerManagement benutzerManagement,
         DatenbankBerechnungen datenbankBerechnungen)
    {
        _help = help;
        _rechnerManager = rechnerManager;
        _benutzerManagement = benutzerManagement;
        _datenbankBerechnungen = datenbankBerechnungen;
        RegistriereRechner();
    }

    private static void RegistriereRechner()
    {
        rechnerRegistry.Clear();
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
        _help.WriteInfo("\n=== VERFÜGBARE RECHNER ===");
        var typen = GetVerfuegbareRechnerTypen();

        for (int i = 0; i < typen.Count; i++)
        {
            _help.WriteInfo($"{i + 1}. {typen[i]}");
        }
    }
}