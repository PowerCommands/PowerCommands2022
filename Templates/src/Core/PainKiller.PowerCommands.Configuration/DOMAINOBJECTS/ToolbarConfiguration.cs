using $safeprojectname$.Enums;

namespace $safeprojectname$.DomainObjects;

public class ToolbarConfiguration
{
    public HideToollbarOption HideToollbarOption { get; set; } = HideToollbarOption.Never;
    public List<ToolbarItemConfiguration> ToolbarItems { get; set; } = new();
}