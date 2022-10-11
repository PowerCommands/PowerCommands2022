namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core")]
[PowerCommand(       description: "Exit command exits the program",
                       arguments: "answer:y",
                      suggestion: "y",
                         example: "exit|exit y|exit Yes")]
public class ExitCommand : CommandBase<CommandsConfiguration>
{
    public ExitCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (Input.Arguments.Length > 0 && Input.Arguments.First().ToLower().StartsWith("y")) return new RunResult(this, Input, output: "exit program", RunResultStatus.Quit);
        Console.WriteLine("Do you wanna quit the program? y/?");
        var response = $"{Console.ReadLine()}";
        if (response.ToLower().StartsWith("y")) return new RunResult(this, Input, output: "exit program", RunResultStatus.Quit);
        return new RunResult(this, Input, output: "No, dont exit the program", RunResultStatus.Ok);
    }
}