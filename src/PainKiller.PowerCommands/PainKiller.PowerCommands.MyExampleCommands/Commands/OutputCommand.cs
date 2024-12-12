using PainKiller.PowerCommands.Core.Commands;

namespace PainKiller.PowerCommands.MyExampleCommands.Commands;

[PowerCommandTest(tests: " ")]
[PowerCommandDesign(description: "Run this command and the file command separated with the [PIPE] character to write output from this command to a file that you provide as the first parameter or as a value to the option flag --path.",
                        options: "text",
                        example: "//Run this command and the file command separated with the [PIPE] character to write output from option --text from this command to a file that you provide as the first parameter.|output test.txt --text \"hello world\" [PIPE] file")]

public class OutputCommand(string identifier, PowerCommandsConfiguration configuration) : CdCommand(identifier, configuration)
{
    public override RunResult Run()
    {
        WriteSuccessLine($"Content: {GetOptionValue("text")}");
        return Ok();
    }
}