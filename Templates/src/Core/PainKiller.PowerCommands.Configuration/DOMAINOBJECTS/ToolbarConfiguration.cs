using $safeprojectname$.Enums;

namespace $safeprojectname$.DomainObjects
{
    public class ToolbarConfiguration
    {
        public HideToolbarOption HideToolbarOption { get; set; } = HideToolbarOption.Never;
        public List<ToolbarItemConfiguration> ToolbarItems { get; set; } = new();
    }
}