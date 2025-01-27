using $safeprojectname$.Contracts;
using $safeprojectname$.Extensions;

namespace $safeprojectname$.DomainObjects.Core;

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