using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class TimeInfoPanelContent : IInfoPanelContent
{
    public string GetText()
    {
        var retVal = $"{DateTime.Now.ToString("dddd d MMMM yyyy HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)}";
        ShortText = $"{DateTime.Now.ToString("dddd d MMMM", System.Globalization.CultureInfo.CurrentCulture)}";
        return retVal;
    }
    public string ShortText { get; private set; } = "";
}