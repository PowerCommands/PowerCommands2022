﻿using PainKiller.PowerCommands.Configuration;
using PainKiller.PowerCommands.Configuration.DomainObjects;
using PainKiller.PowerCommands.Configuration.Enums;
using PainKiller.PowerCommands.Security.Contracts;
using PainKiller.PowerCommands.Security.Managers;

namespace PainKiller.PowerCommands.Core.BaseClasses;

public class EncryptionCommandBase : CommandBase
{
    public EncryptionCommandBase(string identifier) : base(identifier)
    {
        SecurityConfiguration1 = ConfigurationManager.GetAppDataConfiguration(new SecurityConfiguration { Encryption = new EncryptionConfiguration { ShareSecretEnvironmentKey = nameof(IEncryptionManager), ShareSecretSalt = "-- salt --" } }, $"{ConfigurationFiles.Security}.yaml").Configuration;
        Salt = SecurityConfiguration1.Encryption.ShareSecretSalt;
        SharedSecret = Environment.GetEnvironmentVariable(SecurityConfiguration1.Encryption.ShareSecretEnvironmentKey, EnvironmentVariableTarget.User) ?? string.Empty;
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
    protected string GetEnvironmentVariable(string variableName, bool decrypt = false)
    {
        var retVal = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User) ?? "";
        if (decrypt) retVal = DecryptString(retVal);
        return retVal;
    }
}