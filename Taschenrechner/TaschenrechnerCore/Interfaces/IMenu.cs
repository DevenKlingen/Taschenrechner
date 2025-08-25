namespace TaschenrechnerCore.Interfaces;

public interface IMenu
{
    /// <summary>
    /// Zeigt das Menü an und wertet die Eingabe aus
    /// </summary>
    void Show();

    string GetMenuTitle(int optionTitle);
}