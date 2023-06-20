namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(         tests: " orange")]
[PowerCommandsToolbar(    labels: "[ Pick a valid fruit ->]|[Apple]|[Orange]|[Banana]",
                          colors: new [] { ConsoleColor.DarkBlue,ConsoleColor.DarkGreen ,ConsoleColor.Red,ConsoleColor.DarkYellow})]
[PowerCommandDesign( description: "Demonstration of the usage of a command with a toolbar",
                     suggestions: "Apple|Orange|Banana",
                         example: "toolbar")]
public class ToolbarCommand : CommandWithToolbarBase<PowerCommandsConfiguration>
{
    public ToolbarCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        base.Run();
        var fruits = new[] { "Apple", "Orange", "Banana" };
        var fruit = Input.SingleArgument;
        if(fruits.Any(f => f == fruit))WriteSuccessLine($"You picked {fruit}\nWell done");
        else
        {
            WriteFailureLine($"{fruit} is not a valid fruit!");
            DrawToolbar();
        }
        return Ok();
    }
}