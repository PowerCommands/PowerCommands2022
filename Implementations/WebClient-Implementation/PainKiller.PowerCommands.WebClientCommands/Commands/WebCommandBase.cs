using System.Text;
using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Shared.Enums;

namespace PainKiller.PowerCommands.WebClientCommands.Commands;

public abstract class WebCommandBase<TConfig> : IConsoleCommand, IConsoleWriter where TConfig : new()
{
    protected ICommandLineInput Input = new CommandLineInput();
    protected List<PowerFlag> Flags = new();
    private readonly StringBuilder _ouput = new();

    protected WebCommandBase(string identifier, TConfig configuration)
    {
        Identifier = identifier;
        Configuration = configuration;
    }
    public string Identifier { get; }
    protected TConfig Configuration { get; set; }
    public bool InitializeAndValidateInput(ICommandLineInput input)
    {
        Input = input;
        var validationManager = new InputValidationManager(this, input, WriteError);
        var result = validationManager.ValidateAndInitialize();
        if (result.Flags.Count > 0) Flags.AddRange(result.Flags);
        return result.HasValidationError;
    }
    public virtual RunResult Run() => throw new NotImplementedException();
    public virtual Task<RunResult> RunAsync() => throw new NotImplementedException();

    #region helpers
    protected RunResult Ok() => new(this, Input, _ouput.ToString(), RunResultStatus.Ok);
    protected RunResult Quit() => new(this, Input, _ouput.ToString(), RunResultStatus.Quit);
    protected RunResult Continue() => new(this, Input, _ouput.ToString(), RunResultStatus.Continue);
    protected RunResult BadParameterError(string output) => new(this, Input, output, RunResultStatus.ArgumentError);
    protected RunResult ExceptionError(string output) => new(this, Input, output, RunResultStatus.ExceptionThrown);

    public void Write(string output, bool addToOutput = true, ConsoleColor? color = null) => _ouput.Append($"<span>{output}</span>");
    public void WriteSuccess(string output, bool addToOutput = true) => _ouput.Append($"<span style=\"color: chartreuse\">{output}</span>");
    public void WriteFailure(string output, bool addToOutput = true) => _ouput.Append($"<span style=\"color: crimson\">{output}</span>");
    public void WriteLine(string output, bool addToOutput = true) => _ouput.Append($"<p>{output}</p>");
    public void WriteHeadLine(string output, bool addToOutput = true) => _ouput.AppendLine($"<h2 style=\"color: cornflowerblue\">{output}</h2>");
    public void WriteSuccessLine(string output, bool addToOutput = true) => _ouput.AppendLine($"<p style=\"color: chartreuse\">{output}</p>");
    public void WriteWarning(string output, bool addToOutput = true) => _ouput.AppendLine($"<p style=\"color: gold\">{output}</p>");
    public void WriteError(string output, bool addToOutput = true) => _ouput.AppendLine($"<p style=\"color: red\">{output}</p>");
    public void WriteCritical(string output, bool addToOutput = true) => _ouput.AppendLine($"<p style=\"color: red\">{output}</p>");
    protected void WriteProcessLog(string processTag, string processDescription) => IPowerCommandServices.DefaultInstance?.Logger.LogInformation($"#{processTag}# {processDescription}");
    #endregion

}