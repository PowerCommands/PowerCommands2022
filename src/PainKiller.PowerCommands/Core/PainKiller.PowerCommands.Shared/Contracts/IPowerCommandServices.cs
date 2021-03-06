using Microsoft.Extensions.Logging;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IPowerCommandServices
{
    IPowerCommandsRuntime Runtime { get; }
    ICommandsConfiguration Configuration { get; }
    ILogger Logger { get; }
    IDiagnosticManager Diagnostic { get; }

    public static IPowerCommandServices? DefaultInstance { get; protected set; }
}