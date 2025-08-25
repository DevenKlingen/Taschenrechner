using TaschenrechnerCore.Enums;
using TaschenrechnerCore.Interfaces;

namespace TaschenrechnerCore.Services;

// Factory-Klasse für Rechner-Erstellung
public static class RechnerFactory
{
    // Static Dictionary für Rechner-Registry
    private static readonly Dictionary<RechnerTyp, Func<BaseRechner>> rechnerRegistry
        = new Dictionary<RechnerTyp, Func<BaseRechner>>();

    // Static Constructor für Initialisierung
    static RechnerFactory()
    {
        RegistriereRechner();
    }

    private static void RegistriereRechner()
    {
        rechnerRegistry[RechnerTyp.Basis] = () => new BasisRechner();
        rechnerRegistry[RechnerTyp.Wissenschaftlich] = () => new WissenschaftlicherRechner();
        rechnerRegistry[RechnerTyp.Finanz] = () => new FinanzRechner();
        rechnerRegistry[RechnerTyp.Matrix] = () => new MatrixRechner();
        rechnerRegistry[RechnerTyp.Statistik] = () => new StatistikRechner();
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
        Console.WriteLine("=== VERFÜGBARE RECHNER ===");
        var typen = GetVerfuegbareRechnerTypen();

        for (int i = 0; i < typen.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {typen[i]}");
        }
    }
}