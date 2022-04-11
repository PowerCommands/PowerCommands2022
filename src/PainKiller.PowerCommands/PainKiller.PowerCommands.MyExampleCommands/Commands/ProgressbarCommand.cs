using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand(description: "Sample command just to show how ProgressBar looks")]
public class ProgressbarCommand : CommandBase<CommandsConfiguration>
{
    public ProgressbarCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var itemCount = 100;
        var progressbar = new ProgressBar(itemCount);
        for (int i = 0; i < itemCount; i++)
        {
            progressbar.Update(i);
            Thread.Sleep(1);
            progressbar.Show();
        }

        return CreateRunResult(this, input, RunResultStatus.Ok);
    }
}