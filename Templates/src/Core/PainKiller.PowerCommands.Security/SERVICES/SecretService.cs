﻿using $safeprojectname$.Contracts;

namespace $safeprojectname$.Services;

public class SecretService : ISecretService
{
    private SecretService() { }
    private static readonly Lazy<ISecretService> Lazy = new(() => new SecretService());
    public static ISecretService Service => Lazy.Value;
    public string GetSecret(string name, Dictionary<string, string> options, Func<string,string> decryptFunction)
    {
        Enum.TryParse<EnvironmentVariableTarget>( options["target"], out var target);
        var val = Environment.GetEnvironmentVariable(name, target) ?? "";
        var decryptedVal = decryptFunction(val);
        return decryptedVal;
    }
    public string SetSecret(string name, string secret, Dictionary<string, string> options, Func<string, string> encryptFunction)
    {
        Enum.TryParse<EnvironmentVariableTarget>(options["target"], out var target);
        var encryptedVal = encryptFunction(secret);
        Environment.SetEnvironmentVariable(name, encryptedVal, target);
        return encryptedVal;
    }
    public string ReplaceSecret(string content, string name, Dictionary<string, string> options, Func<string, string> decryptFunction)
    {
        if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(name)) return content;
        var hasPlaceHolder = content.ToLower().Contains($"##{name.ToLower()}##");
        if (!hasPlaceHolder) return content;

        Enum.TryParse<EnvironmentVariableTarget>(options["target"], out var target);
        var val = Environment.GetEnvironmentVariable(name, target) ?? "";
        var decryptedVal = decryptFunction(val);
        return content.Replace($"##{name}##", decryptedVal);
    }
}