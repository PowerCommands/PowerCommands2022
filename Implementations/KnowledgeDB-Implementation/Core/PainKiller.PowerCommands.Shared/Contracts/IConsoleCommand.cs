using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IConsoleCommand
{
    string Identifier { get; }
    bool InitializeAndValidateInput(ICommandLineInput input);   
    RunResult Run();
    Task<RunResult> RunAsync();
}