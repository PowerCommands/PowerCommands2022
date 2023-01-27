using PainKiller.PowerCommands.Core.Commands;

namespace $safeprojectname$.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Run powershell commands",
                         options: "Get-Help|Get-Command|Get-Process|Get-Service|filter",
                         example: "//Get all available powershell commands on your machine.|ps Get-Command")]

public class PsCommand : MasterCommando
{
    public PsCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration, alias: "powershell") { }
}