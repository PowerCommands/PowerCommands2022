using PainKiller.PowerCommands.Core.Commands;

namespace $safeprojectname$.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Run kubectl commands using the alias k",
    overrideHelpOption: true,
    example: "k")]
public class KCommand : MasterCommando
{
    public KCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration, alias: "kubectl") { }
}