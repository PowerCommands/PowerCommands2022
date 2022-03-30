using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.MyExampleCommands.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class BabarCommand : PocCommand<PowerCommandsConfiguration>
{
    public BabarCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(string input)
    {
        Console.WriteLine($"{Configuration.MyExampleCommand.Component} {Configuration.MyExampleCommand.Checksum}");
        return new RunResult {Status = RunResultStatus.Ok};
    }
}