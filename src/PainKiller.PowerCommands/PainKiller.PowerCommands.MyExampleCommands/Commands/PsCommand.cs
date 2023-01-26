using PainKiller.PowerCommands.Core.Commands;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Run powershell commands, with possibility to filter the output. You don´t have to use the options, you could just write your powershell commands as is.",
                         options: "Get-Help|Get-Command|Get-Process|Get-Service|filter",
                         example: "//Get all available powershell commands on your machine.|ps Get-Command")]

public class PsCommand : MasterCommando
{
    public PsCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration, alias: "powershell") { }
}