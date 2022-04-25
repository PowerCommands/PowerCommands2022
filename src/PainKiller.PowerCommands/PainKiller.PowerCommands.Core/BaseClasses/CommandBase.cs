using System.Text;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.BaseClasses;
public abstract class CommandBase<TConfig> : IConsoleCommand where TConfig : new()
{
    private readonly StringBuilder _ouput = new();
    protected CommandBase(string identifier, TConfig configuration)
    {
        Identifier = identifier;
        Configuration = configuration;
    }
    public string Identifier { get; }
    protected TConfig Configuration { get; set; }
    public virtual RunResult Run(CommandLineInput input) => throw new NotImplementedException();
    public virtual async Task<RunResult> RunAsync(CommandLineInput input) => await Task.FromResult(new RunResult(this, input,"",RunResultStatus.Initializing));
    protected RunResult CreateRunResult(CommandLineInput input) => new(this, input, _ouput.ToString(), RunResultStatus.Ok);
    protected RunResult CreateQuitResult(CommandLineInput input) => new(this, input, _ouput.ToString(), RunResultStatus.Quit);
    protected RunResult CreateBadParameterRunResult(CommandLineInput input, string output) => new(this, input, output, RunResultStatus.ArgumentError);
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
}