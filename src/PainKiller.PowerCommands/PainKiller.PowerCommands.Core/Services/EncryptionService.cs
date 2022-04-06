﻿using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Security.Contracts;
using PainKiller.PowerCommands.Security.Managers;
using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Core.Services;

public class EncryptionService : IEncryptionService
{
    public EncryptionService() 
    {
        var securityConfiguration = ConfigurationManager.GetAppDataConfiguration(new SecurityConfiguration { Encryption = new EncryptionConfiguration { SharedSecretEnvironmentKey = nameof(_encryptionManager), SharedSecretSalt = "-- salt --" } }, ConfigurationConstants.SecurityFileName).Configuration;
        _salt = securityConfiguration.Encryption.SharedSecretSalt;
        _sharedSecret = Environment.GetEnvironmentVariable(securityConfiguration.Encryption.SharedSecretEnvironmentKey, EnvironmentVariableTarget.User) ?? string.Empty;
        _encryptionManager = new AESEncryptionManager(_salt);
    }

    private static readonly Lazy<EncryptionService> Lazy = new(() => new EncryptionService());
    public static EncryptionService Service => Lazy.Value;

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
}