using TaschenrechnerCore.Models;
using TaschenrechnerCore.Services;
using TaschenrechnerCore.Utils;

namespace TaschenrechnerCore.Interfaces
{
    public interface IBenutzerService
    {
        void BenutzerAnmelden();
        Benutzer BenutzerErstellen(string name, TaschenrechnerContext context);
        void BenutzerLöschen();
        void BenutzerWechseln();
        Benutzer getBenutzer();
        Hilfsfunktionen getHelp();
        void setBenutzerEinstellungen(BenutzerEinstellungen benutzerEinstellungen);
        void setHelp(Hilfsfunktionen help);
    }
}