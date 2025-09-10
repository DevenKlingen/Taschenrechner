namespace TaschenrechnerCore.Interfaces
{
    public interface IKonfigurationsService
    {
        void EinstellungAendern(string einstellung, string input);
        void EinstellungenBearbeiten();
        void ZeigeEinstellungen();
    }
}