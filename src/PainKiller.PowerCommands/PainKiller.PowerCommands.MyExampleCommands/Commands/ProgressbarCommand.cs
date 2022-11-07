namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(tests: " ")]
[PowerCommandDesign(description: "Sample command just to show how ProgressBar looks", example:"progressbar")]
public class ProgressbarCommand : CommandBase<CommandsConfiguration>
{
    public ProgressbarCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var itemCount = 100;
        var progressbar = new ProgressBar(itemCount);
        for (int i = 0; i < itemCount; i++)
        {
            progressbar.Update(i);
            Thread.Sleep(1);
            progressbar.Show();
        }
        return Ok();
    }
}