using PainKiller.PowerCommands.Shared.Contracts;
using PainKiller.PowerCommands.Shared.DomainObjects.Core;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public class PocCommand<TConfig> : IConsoleCommand where TConfig : new()
{
    public PocCommand(string identifier, TConfig configuration)
    {
        Identifier = identifier;
        Configuration = configuration;
    }

    protected TConfig Configuration { get; }
    public string Identifier { get; }
    public virtual RunResult Run(string input)
    {
        throw new NotImplementedException();
    }

    public Task<RunResult> RunAsync(string input)
    {
        throw new NotImplementedException();
    }
}