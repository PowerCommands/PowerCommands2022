using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Security.Contracts;
using PainKiller.PowerCommands.Security.Managers;
using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public abstract class EncryptionCommandBase : CommandBase<CommandsConfiguration>
{
    public EncryptionCommandBase(string identifier, CommandsConfiguration configuration) : base(identifier, configuration)
    {
        SecurityConfiguration1 = ConfigurationManager.GetAppDataConfiguration(new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = nameof(IEncryptionManager), SharedSecretSalt = "-- salt --" } }, ConfigurationConstants.SecurityFileName).Configuration;
        Salt = SecurityConfiguration1.Encryption.SharedSecretSalt;
        SharedSecret = Environment.GetEnvironmentVariable(SecurityConfiguration1.Encryption.SharedSecretEnvironmentKey, EnvironmentVariableTarget.User) ?? string.Empty;
    }
    protected SecurityConfiguration SecurityConfiguration1 { get; }
    protected string SharedSecret { get; }
    protected string Salt { get; }
    protected string EncryptString(string plainText)
    {
        var encryptionManager = new AESEncryptionManager(Salt);
        var retVal = encryptionManager.EncryptString(plainText, SharedSecret);
        return retVal;
    }
    protected string DecryptString(string plainText)
    {
        var encryptionManager = new AESEncryptionManager(Salt);
        var retVal = encryptionManager.DecryptString(plainText, SharedSecret);
        return retVal;
    }
}