using $safeprojectname$.Contracts;

namespace $safeprojectname$.DomainObjects.Core;

public class UserNameInfoPanelContent : IInfoPanelContent
{
    public string GetText()
    {
        ShortText = $"{Environment.UserName}";
        return ShortText;
    }

    public string? ShortText { get; private set; }
}