namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core|console|clear")]
[PowerCommand(description: "Clears the console",
                  example: "cls")]
public class ClsCommand : CommandBase<CommandsConfiguration>
{
    public ClsCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        Console.Clear();
        return CreateRunResult(input);
    }
}