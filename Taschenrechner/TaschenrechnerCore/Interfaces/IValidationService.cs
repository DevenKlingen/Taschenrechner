namespace TaschenrechnerCore.Interfaces
{
    public interface IValidationService
    {
        static abstract bool IstGanzzahl(double zahl);
        static abstract bool IstGueltigeZahl(string eingabe, out double zahl);
        static abstract bool IstImBereich(double zahl, double min, double max);
        static abstract bool IstPositiv(double zahl);
        bool isKonstante(string eingabe);
    }
}