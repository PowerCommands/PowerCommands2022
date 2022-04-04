using Microsoft.Extensions.Logging;

namespace PainKiller.PowerCommands.Shared.Contracts;

public interface IPowerCommandsService<out TConfiguration> where TConfiguration : ICommandsConfiguration
{
    IPowerCommandsRuntime Runtime { get; }
    TConfiguration Configuration { get; }
    ILogger Logger { get; }
    IDiagnosticManager Diagnostic { get; }
}