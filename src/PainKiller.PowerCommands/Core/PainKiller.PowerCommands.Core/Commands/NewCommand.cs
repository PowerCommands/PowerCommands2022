using PainKiller.PowerCommands.Core.Managers;

namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core|cli|project")]
[PowerCommand(  description: "Create a new Visual Studio Solution with all depended projects",
                    example: "new testproject \"C:\\Temp\\\"",
                    arguments:"Solution name:<name>",
                    argumentMandatory: true,
                    qutes: "Path: <path>")]
public class NewCommand : CommandBase<CommandsConfiguration>
{
    private string _path = "";

    public NewCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run(CommandLineInput input)
    {
        var name = input.SingleArgument;
        _path = string.IsNullOrEmpty(input.SingleQuote) ? Path.Combine(AppContext.BaseDirectory, "output",name) : Path.Combine(input.SingleQuote, name);

        var cli = new CliManager(name, _path, WriteLine);
        cli.CreateRootDirectory();
        
        cli.CloneRepo("https://github.com/PowerCommands/PowerCommands2022");
        
        cli.DeleteDir("PowerCommands2022\\.vscode");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Implementations");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Custom Components");

        cli.MoveDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands", $"PainKiller.PowerCommands.{name}Commands");
        


        cli.WriteNewSolutionFile();

        return CreateRunResult(input);
    }

    
}