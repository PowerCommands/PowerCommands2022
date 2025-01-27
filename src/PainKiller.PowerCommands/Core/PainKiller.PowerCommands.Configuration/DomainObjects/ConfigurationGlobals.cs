﻿namespace PainKiller.PowerCommands.Configuration.DomainObjects
{
    public static class ConfigurationGlobals
    {
        public const string ApplicationName = "PC";
        public const string MainConfigurationFile = "PowerCommandsConfiguration.yaml";
        public const string SecurityFileName = "security.yaml";
        public const string WhatsNewFileName = "whats_new.md";
        public const char ArraySplitter = '|';
        public const string SetupConfigurationFile = "setup.yaml";
        public const string EncryptionEnvironmentVariableName = "_encryptionManager";

        public static readonly string ApplicationDataFolder = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\{nameof(PowerCommands)}", ApplicationName);
        public static readonly string EnvironmentVariableName = $"{nameof(PowerCommands)}_{ApplicationName}";
    }
}