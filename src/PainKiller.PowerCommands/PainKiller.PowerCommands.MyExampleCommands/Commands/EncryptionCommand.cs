using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class EncryptionCommand : CommandBase<CommandsConfiguration>
{
    public EncryptionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        WriteLine(EncryptionService.Service.EncryptString(input.SingleQuote));
        return CreateRunResult(input);
    }
}