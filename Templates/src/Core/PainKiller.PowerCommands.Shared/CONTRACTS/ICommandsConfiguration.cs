﻿using $safeprojectname$.DomainObjects.Configuration;

namespace $safeprojectname$.Contracts
{
    public interface ICommandsConfiguration
    {
        bool ShowDiagnosticInformation { get; set; }
        public string Prompt { get; set; }
        string DefaultCommand { get; set; }
        public string CodeEditor { get; set; }
        string Repository { get; set; }
        string BackupPath { get; set; }
        Metadata Metadata { get; set; }
        LogComponentConfiguration Log { get; set; }
        List<BaseComponentConfiguration> Components { get; set; }
        SecretConfiguration Secret { get; set; }
        EnvironmentConfiguration Environment { get; set; }
        InfoPanelConfiguration InfoPanel { get; set; }
    }
}