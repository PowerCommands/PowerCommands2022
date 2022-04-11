using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IConsoleCommand
{
    string Identifier { get; }
    RunResult Run(CommandLineInput input);
    Task RunAsync(CommandLineInput input);
}