namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign("A dummie iteration is running in async mode", example:"iteration", useAsync: true)]
public class IterationCommand : CommandBase<CommandsConfiguration>
{
    public IterationCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override async Task<RunResult> RunAsync()
    {
        await RunIterations(PowerCommandServices.Service.Runtime.Commands);
        return Ok();
    }
    private async Task RunIterations(List<IConsoleCommand> runtimeCommands)
    {
        await Task.Yield();
        foreach (var command in runtimeCommands)
        {
            Console.WriteLine(command.Identifier);
            Thread.Sleep(100);
        }
        Console.Write($"\nDone!\n{ConfigurationGlobals.Prompt}");
    }
}