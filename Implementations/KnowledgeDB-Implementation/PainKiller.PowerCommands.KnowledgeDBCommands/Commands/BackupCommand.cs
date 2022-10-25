﻿using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.KnowledgeDBCommands.Configuration;
using PainKiller.PowerCommands.KnowledgeDBCommands.DomainObjects;

namespace PainKiller.PowerCommands.KnowledgeDBCommands.Commands;

[PowerCommand(description: "Backup your knowledge DB file to the configured path in PowerCommandsConfiguration.yaml file",
    example: "/*Backup file to the configured path in PowerCommandsConfiguration.yaml file*/|backup")]
public class BackupCommand : CommandBase<PowerCommandsConfiguration>
{
    
    public BackupCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        var fileName = StorageService<KnowledgeDatabase>.Service.Backup();
        WriteLine($"File is backed up to {fileName}");
        return CreateRunResult();
    }
}