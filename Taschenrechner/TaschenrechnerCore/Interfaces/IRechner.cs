namespace TaschenrechnerCore.Interfaces
{
    public interface IRechner
    {
        int AnzahlBerechnungen { get; }
        string RechnerTyp { get; }

        double Berechnen(string operation, params double[] werte);
        void BerechnungSpeichern(string operation, List<double> eingaben, double ergebnis);
        void HistorieAnzeigen();
    }
}