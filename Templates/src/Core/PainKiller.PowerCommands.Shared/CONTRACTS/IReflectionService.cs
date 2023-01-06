using System.Reflection;
using $safeprojectname$.DomainObjects.Configuration;

namespace $safeprojectname$.Contracts;

public interface IReflectionService
{
    List<IConsoleCommand> GetCommands<TConfiguration>(BaseComponentConfiguration pluginInfo, TConfiguration configuration) where TConfiguration : CommandsConfiguration;
    List<IConsoleCommand> GetCommands<TConfiguration>(Assembly assembly, TConfiguration configuration) where TConfiguration : CommandsConfiguration;
    string GetVersion(Assembly assembly);
}