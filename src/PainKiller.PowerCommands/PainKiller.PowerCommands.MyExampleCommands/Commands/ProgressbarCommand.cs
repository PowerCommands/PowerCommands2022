using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Sample command just to show how ProgressBar looks", example:"progressbar")]
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