﻿using PainKiller.PowerCommands.Shared.Contracts;

namespace PainKiller.PowerCommands.Shared.DomainObjects.Configuration
{
    public class CommandsConfiguration : ICommandsConfiguration
    {
        public bool ShowDiagnosticInformation { get; set; } = true;
        public string DefaultCommand { get; set; } = "";
        public string CodeEditor { get; set; } = "code";
        public string Repository { get; set; } = "https://github.com/PowerCommands/PowerCommands2022";
        public string BackupPath { get; set; } = "C:\\Temp";
        public Metadata Metadata { get; set; } = new();
        public LogComponentConfiguration Log { get; set; } = new();
        public List<BaseComponentConfiguration> Components { get; set; } = new() {new BaseComponentConfiguration {Name = "PainKiller Core", Component = "PainKiller.PowerCommands.Core.dll", Checksum = "e6d2d6cb64863e9dc68a9602f83bcfde"}};
        public SecretConfiguration Secret { get; set; } = new();
        public EnvironmentConfiguration Environment { get; set; } = new();
    }
}