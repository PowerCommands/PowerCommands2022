using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.Extensions;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class CurrentDirectoryInfoPanel : IInfoPanelContent
{
    public string GetText()
    {
        var retVal = $"dir: {Environment.CurrentDirectory}";
        ShortText = $"dir: {Environment.CurrentDirectory}".GetCompressedPath(Console.WindowWidth-2);
        return retVal;
    }
    public string? ShortText { get; private set; }
}