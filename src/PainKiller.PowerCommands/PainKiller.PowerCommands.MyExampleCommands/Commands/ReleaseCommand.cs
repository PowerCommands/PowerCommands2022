namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Demo command just to se that your solution is setup properly", example: "demo")]
public class ReleaseCommand : CommandBase<PowerCommandsConfiguration>
{
    public ReleaseCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        return base.Run();
    }
}