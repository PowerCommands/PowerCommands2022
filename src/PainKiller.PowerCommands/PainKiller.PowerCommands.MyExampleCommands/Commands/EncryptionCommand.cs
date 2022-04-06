using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class EncryptCommand : CommandBase<CommandsConfiguration>
{
    public EncryptCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        var encrypt = EncryptionService.Service.EncryptString(input.SingleArgument);
        WriteLine($"Input encrypted: {encrypt}",false);

        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}