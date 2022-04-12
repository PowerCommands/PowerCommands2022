using System.Reflection;
using System.Text;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;
using PainKiller.PowerCommands.Shared.Enums;

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
    protected string RootPath()
    {
        var retVal = Assembly.GetEntryAssembly()?.Location ?? "";
        retVal = retVal.Replace($"{Assembly.GetEntryAssembly()?.GetName(false).Name}.dll", "");
        return retVal;
    }
    protected RunResult CreateRunResult(IConsoleCommand executingCommand, CommandLineInput input, RunResultStatus status) => new(executingCommand, input, _ouput.ToString(), status);
    protected void WriteLine(string output, bool addToOutput = true)
    {
        if(addToOutput) _ouput.AppendLine(output);
        Console.WriteLine(output);
    }
}