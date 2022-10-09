using System.Security.Cryptography;
using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Security.Contracts;
using PainKiller.PowerCommands.Security.Managers;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class EncryptionService : IEncryptionService
{
    private EncryptionService() 
    {
        var securityConfiguration = ConfigurationService.Service.GetAppDataConfiguration(new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = nameof(_encryptionManager), SharedSecretSalt = GetRandomSalt() } }, ConfigurationGlobals.SecurityFileName).Configuration;
        _salt = securityConfiguration.Encryption.SharedSecretSalt;
        _sharedSecret = Environment.GetEnvironmentVariable(securityConfiguration.Encryption.SharedSecretEnvironmentKey, EnvironmentVariableTarget.User) ?? string.Empty;
        _encryptionManager = new AESEncryptionManager(_salt);
    }

    private static readonly Lazy<IEncryptionService> Lazy = new(() => new EncryptionService());
    public static IEncryptionService Service => Lazy.Value;

    private readonly string _sharedSecret;
    private readonly string _salt;
    private readonly IEncryptionManager _encryptionManager;
    public string EncryptString(string plainText)
    {
        var encryptionManager = new AESEncryptionManager(_salt);
        var retVal = encryptionManager.EncryptString(plainText, _sharedSecret);
        return retVal;
    }
    public string DecryptString(string plainText)
    {
        var encryptionManager = new AESEncryptionManager(_salt);
        var retVal = encryptionManager.DecryptString(plainText, _sharedSecret);
        return retVal;
    }
    private string GetRandomSalt()
    {
        var data = new byte[32];
        for (var i = 0; i < 10; i++) RandomNumberGenerator.Fill(data);
        var retVal = Convert.ToBase64String(data);
        return retVal;
    }
}