using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Commands;

public class ClsCommand : CommandBase<CommandsConfiguration>
{
    public ClsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        Console.Clear();
        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}