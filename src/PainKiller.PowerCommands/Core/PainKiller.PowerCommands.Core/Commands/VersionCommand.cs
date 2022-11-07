using System.Reflection;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommandTest(tests: " ")]
[PowerCommandDesign(description:"Shows current version for the Core components.")]
public class VersionCommand : CommandBase<CommandsConfiguration>
{
    public VersionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        Console.WriteLine($"{nameof(Core)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Core"))}");
        Console.WriteLine($"{nameof(PowerCommands.Configuration)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Configuration"))}");
        Console.WriteLine($"{nameof(ReadLine)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.ReadLine"))}");
        Console.WriteLine($"{nameof(Security)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Security"))}");
        Console.WriteLine($"{nameof(Shared)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Shared"))}");
        return Ok();
    }
}