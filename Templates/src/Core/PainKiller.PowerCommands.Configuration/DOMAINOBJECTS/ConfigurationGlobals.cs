﻿namespace $safeprojectname$.DomainObjects;

public static class ConfigurationGlobals
{
    public const string Prompt = "pcm>";
    public const string MainConfigurationFile = "PowerCommandsConfiguration.yaml";
    public const string SecurityFileName = "security.yaml";
    public const string WhatsNewFileName = "whats_new.md";
    public const char ArraySplitter = '|';
    public const string SetupConfigurationFile = "setup.yaml";
    public const string EncryptionEnvironmentVariableName = "_encryptionManager";

    public static readonly string ApplicationDataFolder = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{nameof(PowerCommands)}";
    public static readonly string MainConfigurationFileFullPath = Path.Combine(AppContext.BaseDirectory, MainConfigurationFile);
}