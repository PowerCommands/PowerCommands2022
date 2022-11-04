using System.Text;
using Microsoft.Extensions.Logging;

namespace PainKiller.PowerCommands.Core.BaseClasses;
public abstract class CommandBase<TConfig> : IConsoleCommand where TConfig : new()
{
    protected ICommandLineInput Input = new CommandLineInput();
    protected List<PowerFlag> Flags = new();
    private readonly StringBuilder _ouput = new();
    protected CommandBase(string identifier, TConfig configuration)
    {
        Identifier = identifier;
        Configuration = configuration;
    }
    public string Identifier { get; }
    public bool InitializeAndValidateInput(ICommandLineInput input)
    {
        Input = input;
        var validationManager = new InputValidationManager(this, input, WriteError);
        var result = validationManager.ValidateAndInitialize();
        if(result.Flags.Count > 0) Flags.AddRange(result.Flags);
        return result.HasValidationError;
    }

    protected TConfig Configuration { get; set; }

    public virtual RunResult Run() => throw new NotImplementedException();
    public virtual async Task<RunResult> RunAsync() => await Task.FromResult(new RunResult(this, Input,"",RunResultStatus.Initializing));

    #region helpers
    protected RunResult Ok() => new(this, Input, _ouput.ToString(), RunResultStatus.Ok);
    protected RunResult Quit() => new(this, Input, _ouput.ToString(), RunResultStatus.Quit);
    protected RunResult BadParameterError(string output) => new(this, Input, output, RunResultStatus.ArgumentError);
    protected RunResult ExceptionError(string output) => new(this, Input, output, RunResultStatus.ExceptionThrown);
    protected void WriteLine(string output, bool addToOutput = true)
    {
        if(addToOutput && !string.IsNullOrEmpty(output.Trim())) _ouput.AppendLine(output);
        ConsoleService.WriteLine(GetType().Name, output, null);
    }
    protected void WriteHeadLine(string output, bool addToOutput = true)
    {
        if (addToOutput && string.IsNullOrEmpty(output.Trim())) _ouput.AppendLine(output);
        ConsoleService.WriteHeaderLine(GetType().Name, output);
    }

    protected void WriteWarning(string output, bool addToOutput = true)
    {
        if (addToOutput && string.IsNullOrEmpty(output.Trim())) _ouput.AppendLine(output);
        ConsoleService.WriteWarning(GetType().Name, output);
    }

    protected void WriteError(string output, bool addToOutput = true)
    {
        if (addToOutput && string.IsNullOrEmpty(output.Trim())) _ouput.AppendLine(output);
        ConsoleService.WriteError(GetType().Name, output);
    }

    protected void WriteCritical(string output, bool addToOutput = true)
    {
        if (addToOutput && string.IsNullOrEmpty(output.Trim())) _ouput.AppendLine(output);
        ConsoleService.WriteCritical(GetType().Name, output);
    }

    protected void OverwritePreviousLine(string output)
    {
        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
        var padRight = Console.BufferWidth - output.Length;
        WriteLine(output.PadRight(padRight > 0 ? padRight : 0), false);
    }
    protected void WriteProcessLog(string processTag, string processDescription) => IPowerCommandServices.DefaultInstance?.Logger.LogInformation($"#{processTag}# {processDescription}");
    #endregion
}