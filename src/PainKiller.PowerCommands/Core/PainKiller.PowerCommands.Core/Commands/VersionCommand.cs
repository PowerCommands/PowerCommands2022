namespace PainKiller.PowerCommands.Core.Commands;

public class VersionCommand : CommandBase<CommandsConfiguration>
{
    public VersionCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        WriteHeadLine($"Core 1.0.0");
        return CreateRunResult(input);
    }
}