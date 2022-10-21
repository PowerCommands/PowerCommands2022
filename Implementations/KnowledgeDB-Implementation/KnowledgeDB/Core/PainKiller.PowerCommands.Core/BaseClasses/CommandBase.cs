using System.Text;
using Microsoft.Extensions.Logging;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.BaseClasses;
public abstract class CommandBase<TConfig> : IConsoleCommand where TConfig : new()
{
    protected ICommandLineInput Input = new CommandLineInput();
    private readonly StringBuilder _ouput = new();
    protected CommandBase(string identifier, TConfig configuration)
    {
        Identifier = identifier;
        Configuration = configuration;
    }
    public string Identifier { get; }
    public void InitializeRun(ICommandLineInput input) => Input = input;

    protected TConfig Configuration { get; set; }

    public virtual RunResult Run() => throw new NotImplementedException();
    public virtual async Task<RunResult> RunAsync() => await Task.FromResult(new RunResult(this, Input,"",RunResultStatus.Initializing));

    #region helpers
    protected RunResult CreateRunResult() => new(this, Input, _ouput.ToString(), RunResultStatus.Ok);
    protected RunResult CreateQuitResult() => new(this, Input, _ouput.ToString(), RunResultStatus.Quit);
    protected RunResult CreateBadParameterRunResult(string output) => new(this, Input, output, RunResultStatus.ArgumentError);
    protected void WriteLine(string output, bool addToOutput = true)
    {
        if(addToOutput && !string.IsNullOrEmpty(output.Trim())) _ouput.AppendLine(output);
        Console.WriteLine(output);
    }
    protected void WriteHeadLine(string output, bool addToOutput = true)
    {
        if (addToOutput && string.IsNullOrEmpty(output.Trim())) _ouput.AppendLine(output);
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(output);
        Console.ForegroundColor = color;
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