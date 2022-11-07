﻿using PainKiller.PowerCommands.Core.Services;

namespace PainKiller.PowerCommands.Core.Commands;

[PowerCommand(description: "Proxy command, this command is executing a command outside this application, the functionallity is therefore unknown")]
public class ProxyCommando : CommandBase<CommandsConfiguration>
{
    private readonly string _name;
    private readonly string _workingDirctory;
    public ProxyCommando(string identifier, CommandsConfiguration configuration, string name, string workingDirectory) : base(identifier, configuration)
    {
        _name = name;
        _workingDirctory = workingDirectory;
    }

    public override RunResult Run()
    {
        WriteProcessLog("Proxy", $"{Input.Raw}");
        ShellService.Service.Execute(_name, Input.Raw, _workingDirctory, WriteLine, useShellExecute: true);
        return CreateRunResult();
    }
}