using System.Reflection;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Core.Managers;

public class ReflectionManager
{
    public static List<IConsoleCommand> GetCommands<TConfiguration>(BaseComponentConfiguration pluginInfo, TConfiguration configuration) where TConfiguration : new()
    {
        var currentAssembly = Assembly.Load($"{pluginInfo.Component}".Replace(".dll",""));
        return GetCommands(currentAssembly, configuration);
    }

    public static List<IConsoleCommand> GetCommands<TConfiguration>(Assembly assembly, TConfiguration configuration) where TConfiguration : new()
    {
        var retVal = new List<IConsoleCommand>();

        var types = assembly.GetTypes().Where(t => t.IsClass && t.Name.EndsWith("Command") && !t.IsAbstract).ToList();

        foreach (var commandType in types)
        {
            Object[] args = { commandType.Name, configuration };
            var command = (IConsoleCommand)Activator.CreateInstance(commandType, args)!;
            retVal.Add(command);
        }
        return retVal.OrderBy(c => c.Identifier).ToList();
    }
}