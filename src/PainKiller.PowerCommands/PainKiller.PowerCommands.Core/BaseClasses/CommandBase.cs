using System.Reflection;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public abstract class CommandBase<TConfig> : IConsoleCommand where TConfig : new()
{
    
    protected CommandBase(string identifier, TConfig configuration)
    {
        Identifier = identifier;
        Configuration = configuration;
    }
    public string Identifier { get; }
    protected TConfig Configuration { get; set; }
    public virtual RunResult Run(string input)
    {
        throw new NotImplementedException();
    }
    public virtual Task<RunResult> RunAsync(string input)
    {
        throw new NotImplementedException();
    }
    protected string RootPath()
    {
        var retVal = Assembly.GetEntryAssembly()?.Location ?? "";
        retVal = retVal.Replace($"{Assembly.GetEntryAssembly()?.GetName(false).Name}.dll", "");
        return retVal;
    }
}