using PainKiller.PowerCommands.Core.BaseClasses;
using PainKiller.PowerCommands.Shared.Attributes;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommand("A dummie iteration is running in async mode", example:"iteration", useAsync: true)]
[Tags("example|iteration|async")]
public class IterationCommand : CommandBase<CommandsConfiguration>
{
    public IterationCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override async Task<RunResult> RunAsync(CommandLineInput input)
    {
        await RunIterations(PowerCommandServices.Service.Runtime.Commands);
        return CreateRunResult(this, input,RunResultStatus.Ok);
    }
    private async Task RunIterations(List<IConsoleCommand> runtimeCommands)
    {
        await Task.Yield();
        foreach (var command in runtimeCommands)
        {
            Console.WriteLine(command.Identifier);
            Thread.Sleep(100);
        }
        Console.Write("\nDone!\npcm>");
    }
}