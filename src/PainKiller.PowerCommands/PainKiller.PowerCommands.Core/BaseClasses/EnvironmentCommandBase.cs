using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public abstract class EnvironmentCommandBase : EncryptionCommandBase
{
    public EnvironmentCommandBase(string identifier, BasicCommandsConfiguration configuration) : base(identifier, configuration) { }
    protected string GetEnvironmentVariable(string variableName, bool decrypt = false)
    {
        var retVal = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User) ?? "";
        if (decrypt) retVal = DecryptString(retVal);
        return retVal;
    }
}