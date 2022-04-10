using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core;

public class PowerCommandsRuntime<TConfig> : IPowerCommandsRuntime where TConfig : CommandsConfiguration
{
    private readonly TConfig _configuration;
    private readonly IDiagnosticManager _diagnostic;
    private readonly List<IConsoleCommand> _commands = new();

    public PowerCommandsRuntime(TConfig configuration, IDiagnosticManager diagnosticManager)
    {
        _configuration = configuration;
        _diagnostic = diagnosticManager;
        Initialize();
    }
    private void Initialize()
    {
        var reflectionManager = new ReflectionManager();
        foreach (var component in _configuration.Components) _commands.AddRange(reflectionManager.GetCommands(component, _configuration));
        if(_configuration.ShowDiagnosticInformation) foreach (var consoleCommand in _commands) _diagnostic.Message(consoleCommand.Identifier);
    }
    public string[] CommandIDs => _commands.Select(c => c.Identifier).ToArray();
    public RunResult ExecuteCommand(string rawInput)
    {
        var input = rawInput.Interpret();
        var command = _commands.FirstOrDefault(c => c.Identifier.ToLower() == input.Identifier);
        if (command == null) throw new ArgumentOutOfRangeException($"Could not identify any Commmand with identy {input.Identifier}");
        try { Latest = command.Run(input); }
        catch (Exception e) { Latest = new RunResult(command, input, e.Message, RunResultStatus.ExceptionThrown); }
        return Latest;
    }
    public RunResult? Latest { get; private set; }
}