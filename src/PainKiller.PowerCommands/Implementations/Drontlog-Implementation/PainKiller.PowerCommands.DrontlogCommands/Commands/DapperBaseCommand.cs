using PainKiller.PowerCommands.Core.Extensions;

namespace PainKiller.PowerCommands.DrontlogCommands.Commands;

public abstract class DapperCommandBase : CommandBase<PowerCommandsConfiguration>
{
    protected readonly string ConnectionString;
    protected readonly string Schema = "timeline";
    protected DapperCommandBase(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration)
    {
        ConnectionString = Configuration.Secret.DecryptSecret(Configuration.ConnectionString);
    }
}