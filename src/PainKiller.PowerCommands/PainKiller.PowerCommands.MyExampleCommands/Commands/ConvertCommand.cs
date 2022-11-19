using PainKiller.PowerCommands.Shared.DomainObjects.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(   description: "Converting Yaml format to XML or JSON format.",
                           options: "!path|format",
                           example: "//Convert to json|convert --path \"C:\\temp\\test.yaml\" --json")]
public class ConvertCommand : CommandBase<CommandsConfiguration>
{
    public ConvertCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        //insert your code here
        return Ok();
    }
}