using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;

public class UserNameInfoPanelContent : IInfoPanelContent
{
    public string GetText()
    {
        ShortText = $"{Environment.UserName}";
        return ShortText;
    }

    public string? ShortText { get; private set; }
}