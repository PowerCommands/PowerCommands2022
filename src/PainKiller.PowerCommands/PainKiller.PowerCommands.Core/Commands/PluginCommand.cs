using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

public class PluginCommand : CommandBase
{
    public PluginCommand(string identifier) : base(identifier) { }

    public override RunResult Run(string input)
    {
        //Hur kommer man åt PowerCommandsConfiguration här???
        return new RunResult {Status = RunResultStatus.Ok, Output = ""};
    }
}