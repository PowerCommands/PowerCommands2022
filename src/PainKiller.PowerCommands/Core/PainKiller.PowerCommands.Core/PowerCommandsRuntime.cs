﻿namespace PainKiller.PowerCommands.Core;

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
        foreach (var component in _configuration.Components)
        {
            Commands.AddRange(ReflectionService.Service.GetCommands(component, _configuration));
            if (!_configuration.ShowDiagnosticInformation) continue;
            _diagnostic.Header($"\nFound commands in component: {component.Name}");
            foreach (var consoleCommand in Commands) _diagnostic.Message(consoleCommand.Identifier);
        }

        foreach (var proxyCommand in _configuration.ProxyCommands)
        {
            foreach (var command in proxyCommand.Commands)
            {
                ConsoleService.WriteLine("PowerCommandsRuntime", $"Proxy command [{command}] added", null);
                var powerCommand = new ProxyCommando(command, _configuration, proxyCommand.Name, proxyCommand.WorkingDirctory);
                if(Commands.All(c => c.Identifier != powerCommand.Identifier)) Commands.Add(powerCommand);
                else ConsoleService.WriteWarning("PowerCommandsRuntime", $"A command with the same identifier [{command}] already exist, proxy command not added.");
            }
        }
        IPowerCommandsRuntime.DefaultInstance = this;
    }
    public string[] CommandIDs => Commands.Select(c => c.Identifier).ToArray();
    public RunResult ExecuteCommand(string rawInput)
    {
        var input = rawInput.Interpret();
        var command = Commands.FirstOrDefault(c => c.Identifier.ToLower() == input.Identifier);
        if (command == null && !string.IsNullOrEmpty(_configuration.DefaultCommand))
        {
            input = $"{_configuration.DefaultCommand} {rawInput}".Interpret();
            command = Commands.FirstOrDefault(c => c.Identifier.ToLower() == input.Identifier); //Retry with default command if no command found on the first try
        }
        if (command == null) throw new ArgumentOutOfRangeException($"Could not identify any Commmand with identy {input.Identifier} and there is no defaultCommand defined in configuration file either.");
        
        if (input.Flags.Any(f => f == "--help"))
        {
            if (!command.GetPowerCommandAttribute().OverrideHelpFlag)
            {
                HelpService.Service.ShowHelp(command, clearConsole: false);
                return new RunResult(command, input, "User prompted for help with --help flag", RunResultStatus.Ok);
            }
        }
        if (command.InitializeAndValidateInput(input))
        {
            Latest = new RunResult(command, input, "Validation error", RunResultStatus.InputValidationError);
            return Latest;
        }
        if (command.GetPowerCommandAttribute().UseAsync) return ExecuteAsyncCommand(command, input);
        try { Latest = command.Run(); }
        catch (Exception e) { Latest = new RunResult(command, input, e.Message, RunResultStatus.ExceptionThrown); }
        return Latest;
    }
    public RunResult ExecuteAsyncCommand(IConsoleCommand command, CommandLineInput input)
    {
        try
        {
            command.RunAsync().ConfigureAwait(continueOnCapturedContext:false);
        }
        catch (Exception e)
        {
            Latest = new RunResult(command, input, e.Message, RunResultStatus.ExceptionThrown);
            return Latest;
        }
        Latest = new RunResult(command, input, "Command running async operation", RunResultStatus.Async);
        return Latest;
    }
    public RunResult? Latest { get; private set; }
}