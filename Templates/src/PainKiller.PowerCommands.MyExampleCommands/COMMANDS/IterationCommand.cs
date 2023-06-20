namespace $safeprojectname$.Commands;

[PowerCommandDesign(description:"A dummie iteration is running in async mode",
                       useAsync: true,
                        example: "iteration")]
public class IterationCommand : CommandBase<CommandsConfiguration>
{
    public IterationCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override async Task<RunResult> RunAsync()
    {
        await RunIterations(PowerCommandServices.Service.Runtime.Commands);
        return Ok();
    }
    public override void RunCompleted()
    {
        base.RunCompleted();
        WriteSuccessLine($"\nDone!\n");
    }
    private async Task RunIterations(List<IConsoleCommand> runtimeCommands)
    {
        await Task.Yield();
        foreach (var command in runtimeCommands)
        {
            Console.WriteLine(command.Identifier);
            Thread.Sleep(100);
        }
    }
}