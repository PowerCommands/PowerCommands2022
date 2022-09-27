using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;

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
        
        cli.CloneRepo(Configuration.Repository);
        WriteLine("Fetching repo from Github...");

        cli.DeleteDir("PowerCommands2022\\.vscode");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Implementations");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Custom Components");

        cli.RenameDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands", $"PainKiller.PowerCommands.{name}Commands");
        
        cli.WriteNewSolutionFile();

        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap\\PainKiller.PowerCommands.Bootstrap.csproj", "MyExampleCommands",$"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap\\PowerCommandsManager.cs", "MyExampleCommands",$"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap\\Startup.cs", "MyExampleCommands",$"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.PowerCommandsConsole\\Program.cs", "Power Commands 1.0", $"{name} Commands 1.0");

        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PowerCommandsConfiguration.yaml", "MyExampleCommands",$"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Configuration\\PowerCommandsConfiguration.cs", "MyExampleCommands",$"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Configuration\\FavoriteConfiguration.cs", "MyExampleCommands",$"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PowerCommandServices.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DemoCommand.cs", "MyExampleCommands",$"{name}Commands");
        
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.MyExampleCommands.csproj", $"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.{name}Commands.csproj");

        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\Core", $"Core");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap", $"PainKiller.PowerCommands.Bootstrap");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.PowerCommandsConsole", $"PainKiller.PowerCommands.PowerCommandsConsole");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\Third party components", $"Third party components");
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PowerCommands.{name}.sln", $"PowerCommands.{name}.sln");

        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DemoCommand.cs", "DemoCommand.cs");
        cli.MoveDirectory($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands", $"PainKiller.PowerCommands.{name}Commands");
        cli.DeleteDir($"PainKiller.PowerCommands.{name}Commands\\Commands");
        cli.CreateDirectory($"PainKiller.PowerCommands.{name}Commands\\Commands");
        cli.MoveFile($"DemoCommand.cs", $"PainKiller.PowerCommands.{name}Commands\\Commands\\DemoCommand.cs");

        WriteHeadLine("\nAll work is done, now do the following steps");
        WriteHeadLine("\n1. Delete the directory PowerCommands2022 and it´s content");
        WriteHeadLine("\n2. Set the PowerCommandsConsole project as startup project");
        WriteHeadLine("\n3. Build the solution");
        WriteHeadLine("\n4. Run the solution, to test your DemoCommand just type demo and hit enter");
        
        
        ShellService.Service.OpenDirectory(_path);
        return CreateRunResult(input);
    }

    
}