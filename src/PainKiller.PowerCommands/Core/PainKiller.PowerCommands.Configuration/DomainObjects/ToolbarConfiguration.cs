using PainKiller.PowerCommands.Configuration.Enums;

namespace PainKiller.PowerCommands.Configuration.DomainObjects;

public class ToolbarConfiguration
{
    public HideToollbarOption HideToollbarOption { get; set; } = HideToollbarOption.Never;
    public List<ToolbarItemConfiguration> ToolbarItems { get; set; } = new();
}