namespace PainKiller.PowerCommands.WebClientCommands.Commands;

[PowerCommandDesign( description: "Demo command just to se that your solution is setup properly", example: "demo")]
public class DemoCommand : WebCommandBase<PowerCommandsConfiguration>
{
    public DemoCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteLine("Hello World!");
        WriteHeadLine("Congratulations! You have setup your PowerCommands solution correctly!");
        WriteLine("You may set up some things in your PowerCommandsConfiguration.yaml file, like the path to your favorite code editor");
        WriteLine("Just type dir and hit enter to open up this applications bin directory");
        WriteLine("");
        WriteLine("This command could now be removed from your solution.");
        WriteLine("The commands [config] and [dir] could also be deleted (practical help commands only)");
        return Ok();
    }
}