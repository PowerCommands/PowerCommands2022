using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.Core.Managers;

public class PowerCommandsManager<TConfig> : IPowerCommandsManager where TConfig : BasicCommandsConfiguration
{
    private readonly TConfig _configuration;
    private readonly List<IConsoleCommand> _commands = new();

    public PowerCommandsManager(TConfig configuration)
    {
        _configuration = configuration;
        Initialize();
    }
    private void Initialize()
    {
        var reflectionManager = new ReflectionManager();
        foreach (var component in _configuration.Components) _commands.AddRange(reflectionManager.GetCommands(component, _configuration));
        if(_configuration.ShowDiagnosticInformation) foreach (var consoleCommand in _commands) Console.WriteLine(consoleCommand.Identifier);
    }
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