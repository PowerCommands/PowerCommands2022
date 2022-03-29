using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IConsoleCommand
{
    string Identifier { get; }
    RunResult Run(string input);
    Task<RunResult> RunAsync(string input);
}