using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

public class DemoCommand : CommandBase<PowerCommandsConfiguration>
{
    public DemoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        WriteHeadLine("Congratulatins, you have setup your PowerCommands solution correctly, this command could now be removed from your solution.");
        return CreateRunResult(input);
    }
}