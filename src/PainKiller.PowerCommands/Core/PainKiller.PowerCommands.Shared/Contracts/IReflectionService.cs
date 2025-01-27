using System.Reflection;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.Events;

namespace PainKiller.PowerCommands.Shared.Contracts
{
    public interface IReflectionService
    {
        event EventHandler<DesignAttributeReflectedArgs>? DesignAttributeReflected;
        List<IConsoleCommand> GetCommands<TConfiguration>(BaseComponentConfiguration pluginInfo, TConfiguration configuration) where TConfiguration : CommandsConfiguration;
        string GetVersion(Assembly assembly);
    }
}