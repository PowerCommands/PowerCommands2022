using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Core;
public class RunResult
{
    public RunResult(IConsoleCommand executingCommand, CommandLineInput input, string output, RunResultStatus status)
    {
        ExecutingCommand = executingCommand;
        Input = input;
        Output = output;
        Status = status;
    }
    public IConsoleCommand? ExecutingCommand { get;}
    public CommandLineInput Input { get; }
    public string Output { get; }
    public RunResultStatus Status { get;}
}