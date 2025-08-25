namespace TaschenrechnerCore.Utils;

// Static Klasse für Formatierungen
public static class FormatUtils
{
    public static string FormatiereZahl(double zahl, int nachkommastellen = 2)
    {
        return zahl.ToString($"F{nachkommastellen}");
    }

    public static string FormatiereZeitspanne(TimeSpan zeitspanne)
    {
        if (zeitspanne.TotalMinutes < 1)
            return $"{zeitspanne.Seconds} Sekunden";
        else if (zeitspanne.TotalHours < 1)
            return $"{zeitspanne.Minutes}:{zeitspanne.Seconds:00} Minuten";
        else
            return $"{zeitspanne.Hours}:{zeitspanne.Minutes:00}:{zeitspanne.Seconds:00}";
    }

    public static string FormatiereBytes(long bytes)
    {
        string[] einheiten = { "B", "KB", "MB", "GB" };
        double groesse = bytes;
        int einheitIndex = 0;

        while (groesse >= 1024 && einheitIndex < einheiten.Length - 1)
        {
            groesse /= 1024;
            einheitIndex++;
        }

        return $"{groesse:F1} {einheiten[einheitIndex]}";
    }
}

    
