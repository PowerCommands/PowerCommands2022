using $safeprojectname$.Attributes;
using $safeprojectname$.DomainObjects.Core;

namespace $safeprojectname$.Contracts;

public interface IConsoleCommand
{
    string Identifier { get; }
    bool InitializeAndValidateInput(ICommandLineInput input, PowerCommandDesignAttribute designAttribute);
    void RunCompleted();
    RunResult Run();
    Task<RunResult> RunAsync();
}