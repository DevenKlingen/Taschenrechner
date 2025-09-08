using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Services;

public class ListManipulation
{
    private readonly Hilfsfunktionen _help;

    public ListManipulation(Hilfsfunktionen help)
    {  
        _help = help; 
    }

    /// <summary>
    /// Sortiert zwei Listen
    /// </summary>
    /// <param name="liste1"></param>
    /// <param name="liste2"></param>
    public void ListenSortieren(List<int> liste1, List<int> liste2)
    {
        List<int> sortiert1 = new List<int>(liste1);
        List<int> sortiert2 = new List<int>(liste2);

        sortiert1.Sort();
        sortiert2.Sort();

        _help.Write($"Liste 1 sortiert: [{string.Join(", ", sortiert1)}]");
        _help.Write($"Liste 2 sortiert: [{string.Join(", ", sortiert2)}]");
    }

    /// <summary>
    /// Entfernt Duplikate aus zwei Listen
    /// </summary>
    /// <param name="liste1"></param>
    /// <param name="liste2"></param>
    public void DuplikateEntfernen(List<int> liste1, List<int> liste2)
    {
        HashSet<int> unique1 = new HashSet<int>(liste1);
        HashSet<int> unique2 = new HashSet<int>(liste2);

        _help.Write($"Liste 1 ohne Duplikate: [{string.Join(", ", unique1)}]");
        _help.Write($"Liste 2 ohne Duplikate: [{string.Join(", ", unique2)}]");
    }

    /// <summary>
    /// Erstellt die Schnittmenge von zwei Listen
    /// </summary>
    /// <param name="liste1"></param>
    /// <param name="liste2"></param>
    public void Schnittmenge(List<int> liste1, List<int> liste2)
    {
        HashSet<int> unique1 = new HashSet<int>(liste1);
        HashSet<int> unique2 = new HashSet<int>(liste2);

        HashSet<int> schnittmenge = new HashSet<int>(unique1);
        schnittmenge.IntersectWith(unique2);

        _help.Write($"Schnittmenge: [{string.Join(", ", schnittmenge)}]");
    }

    /// <summary>
    /// Vereinigt zwei Listen miteinander
    /// </summary>
    /// <param name="liste1"></param>
    /// <param name="liste2"></param>
    public void Vereinigung(List<int> liste1, List<int> liste2)
    {
        HashSet<int> unique1 = new HashSet<int>(liste1);
        HashSet<int> unique2 = new HashSet<int>(liste2);

        HashSet<int> vereinigung = new HashSet<int>(unique1);
        vereinigung.UnionWith(unique2);

        _help.Write($"Vereinigung: [{string.Join(", ", vereinigung)}]");
    }
}