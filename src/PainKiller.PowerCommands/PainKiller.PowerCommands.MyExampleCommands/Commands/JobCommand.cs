using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[Tags("example|iteration|job|start|quit")]
[PowerCommand("Start up the application with commandline argument job to run once and then quit the program", example: "job")]
public class JobCommand : CommandBase<CommandsConfiguration>
{
    public JobCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run(CommandLineInput input)
    {
        RunIterations(PowerCommandServices.Service.Runtime.Commands);
        return CreateQuitResult(input);
    }
    private void RunIterations(List<IConsoleCommand> runtimeCommands)
    {
        foreach (var command in runtimeCommands)
        {
            Console.WriteLine(command.Identifier);
            Thread.Sleep(100);
        }
        Console.Write("\nDone!\npcm>");
    }
}