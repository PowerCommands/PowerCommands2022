using System.Reflection;
using $safeprojectname$.DomainObjects.Configuration;
using $safeprojectname$.Events;

namespace $safeprojectname$.Contracts
{
    public interface IReflectionService
    {
        event EventHandler<DesignAttributeReflectedArgs>? DesignAttributeReflected;
        List<IConsoleCommand> GetCommands<TConfiguration>(BaseComponentConfiguration pluginInfo, TConfiguration configuration) where TConfiguration : CommandsConfiguration;
        string GetVersion(Assembly assembly);
    }
}