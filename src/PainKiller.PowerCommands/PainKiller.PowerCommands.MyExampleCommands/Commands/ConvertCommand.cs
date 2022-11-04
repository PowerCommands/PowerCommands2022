using PainKiller.PowerCommands.MyExampleCommands.Configuration;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Converting yaml format to json or xml format",
                          flags: "!path|format",
                        example: "//Convert to json format|convert --path \"c:\\temp\\test.yaml\" --format json|//Convert to xml format|convert --path \"c:\\temp\\test.yaml\" --format xml")]
public class ConvertCommand : CommandBase<PowerCommandsConfiguration>
{
    public ConvertCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        return Ok();
    }
}