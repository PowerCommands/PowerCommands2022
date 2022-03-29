using System.Reflection;
using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public abstract class CommandBase : IConsoleCommand
{
    
    protected CommandBase(string identifier)
    {
        Identifier = identifier;
    }
    public string Identifier { get; }

    

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