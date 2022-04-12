using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core;

public class PowerCommandsRuntime<TConfig> : IPowerCommandsRuntime where TConfig : CommandsConfiguration
{
    private readonly TConfig _configuration;
    private readonly IDiagnosticManager _diagnostic;
    public List<IConsoleCommand> Commands { get; } = new();
    public PowerCommandsRuntime(TConfig configuration, IDiagnosticManager diagnosticManager)
    {
        _configuration = configuration;
        _diagnostic = diagnosticManager;
        Initialize();
    }
    private void Initialize()
    {
        foreach (var component in _configuration.Components) Commands.AddRange(ReflectionService.Service.GetCommands(component, _configuration));
        if(_configuration.ShowDiagnosticInformation) foreach (var consoleCommand in Commands) _diagnostic.Message(consoleCommand.Identifier);
        IPowerCommandsRuntime.DefaultInstance = this;
    }
    public string[] CommandIDs => Commands.Select(c => c.Identifier).ToArray();
    public RunResult ExecuteCommand(string rawInput)
    {
        var input = rawInput.Interpret();
        var command = Commands.FirstOrDefault(c => c.Identifier.ToLower() == input.Identifier);
        if (command == null) throw new ArgumentOutOfRangeException($"Could not identify any Commmand with identy {input.Identifier}");
        if (command.GetPowerCommandAttribute().UseAsync) return ExecuteAsyncCommand(command, input);
        try { Latest = command.Run(input); }
        catch (Exception e) { Latest = new RunResult(command, input, e.Message, RunResultStatus.ExceptionThrown); }
        return Latest;
    }

    public RunResult ExecuteAsyncCommand(IConsoleCommand command, CommandLineInput input)
    {
        try
        {
            command.RunAsync(input);
        }
        catch (Exception e)
        {
            Latest = new RunResult(command, input, e.Message, RunResultStatus.ExceptionThrown);
        }
        Latest = new RunResult(command, input, "Command running async operation", RunResultStatus.Ok);
        return Latest;
    }
    public RunResult? Latest { get; private set; }
}