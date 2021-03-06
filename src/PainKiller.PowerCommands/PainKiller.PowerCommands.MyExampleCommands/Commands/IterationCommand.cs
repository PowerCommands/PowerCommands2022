using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("example|iteration|async")]
[PowerCommand("A dummie iteration is running in async mode", example:"iteration", useAsync: true)]
public class IterationCommand : CommandBase<CommandsConfiguration>
{
    public IterationCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override async Task<RunResult> RunAsync(CommandLineInput input)
    {
        await RunIterations(PowerCommandServices.Service.Runtime.Commands);
        return CreateRunResult(input);
    }
    private async Task RunIterations(List<IConsoleCommand> runtimeCommands)
    {
        await Task.Yield();
        foreach (var command in runtimeCommands)
        {
            Console.WriteLine(command.Identifier);
            Thread.Sleep(100);
        }
        Console.Write($"\nDone!\n{{ConfigurationConstants.Prompt}}");
    }
}