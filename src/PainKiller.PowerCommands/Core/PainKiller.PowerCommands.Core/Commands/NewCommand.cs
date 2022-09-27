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

        cli.RenameDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands", $"PainKiller.PowerCommands.{name}Commands");
        
        cli.WriteNewSolutionFile();

        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap\\PainKiller.PowerCommands.Bootstrap.csproj", "MyExampleCommands",$"{name}Commands");
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.MyExampleCommands.csproj", $"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.{name}Commands.csproj");

        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\Core", $"Core");
        cli.MoveDirectory($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands", $"PainKiller.PowerCommands.{name}Commands");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap", $"PainKiller.PowerCommands.Bootstrap");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.PowerCommandsConsole", $"PainKiller.PowerCommands.PowerCommandsConsole");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\Third party components", $"Third party components");
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PowerCommands.{name}.sln", $"PowerCommands.{name}.sln");

        cli.DeleteDir("PowerCommands2022");

        return CreateRunResult(input);
    }

    
}