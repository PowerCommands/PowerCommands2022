using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class EnvironmentCommand : CommandBase<CommandsConfiguration>
{
    public EnvironmentCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        WriteLine(Configuration.Environment.GetValue("KEY_VAULT_NAME"));
        WriteLine(Configuration.Environment.GetValue("AZURE_CLIENT_ID"));
        return CreateRunResult(input);
    }
}