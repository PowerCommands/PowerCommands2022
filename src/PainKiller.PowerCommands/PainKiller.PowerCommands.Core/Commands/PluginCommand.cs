using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

public class PluginCommand : CommandBase<BasicCommandsConfiguration>
{
    public PluginCommand(string identifier, BasicCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(string input)
    {
        Console.WriteLine(Configuration.Metadata.Name);
        return new RunResult {Status = RunResultStatus.Ok, Output = ""};
    }
}