using PainKiller.PowerCommands.Core.Commands;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandDesign(description: "Run commands that supports pipe functionality.",
                        example: "//First run this command and then the version command|version [PIPE] pipe")]
public class PipeCommand(string identifier, PowerCommandsConfiguration configuration) : CdCommand(identifier, configuration)
{
    public override RunResult Run()
    {
        PauseService.Pause(3);
        Console.Clear();
        var result = IPowerCommandsRuntime.DefaultInstance?.Latest;
        if (result == null) return Ok();
        WriteLine($"Previous command: {result!.ExecutingCommand.Identifier}");
        WriteLine($"Previous output length: {result!.Output.Length}");
        WriteLine($"Previous command status: {result!.Status}");
        WriteLine($"Previous command input: {result!.Input.Raw}");
        var retVal = Ok();
        return retVal;
    }
}