namespace PainKiller.PowerCommands.Core.Commands;

public class CommandNewSolution : PowerCommandCommand
{
    private readonly ArtifactPathsConfiguration _artifact;
    private string _path = "";
    public CommandNewSolution(string identifier, CommandsConfiguration configuration, ArtifactPathsConfiguration artifact, ICommandLineInput input) : base(identifier, configuration)
    {
        _artifact = artifact;
        Input = input;
    }
    public override RunResult Run()
    {
        var name = Input.GetFlagValue("solution");
        var output = Input.GetFlagValue("output");

        _path = string.IsNullOrEmpty(output) ? Path.Combine(AppContext.BaseDirectory, "output", name) : Path.Combine(output, name);
        var cli = new CliManager(name, _path, WriteLine);
        cli.DeleteDownloadsDirectory();
        cli.CreateRootDirectory();

        cli.CloneRepo(Configuration.Repository);
        WriteLine("Fetching repo from Github...");

        UpdateTemplates(cli, newProjectName: name);

        cli.DeleteDir(_artifact.VsCode);
        cli.DeleteDir(_artifact.CustomComponents);

        cli.RenameDirectory(_artifact.Source.CommandsProject, _artifact.GetPath(_artifact.Target.CommandsProject));

        cli.WriteNewSolutionFile();

        cli.ReplaceContentInFile($"{_artifact.Source.BootstrapProject}\\PainKiller.PowerCommands.Bootstrap.csproj", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"{_artifact.Source.ConsoleProject}\\PainKiller.PowerCommands.PowerCommandsConsole.csproj", "<AssemblyName>pc</AssemblyName>", $"<AssemblyName>{name}</AssemblyName>");
        cli.ReplaceContentInFile($"{_artifact.Source.BootstrapProject}\\PowerCommandsManager.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"{_artifact.Source.BootstrapProject}\\Startup.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"{_artifact.Source.ConsoleProject}\\Program.cs", "Power Commands 1.0", $"{name} Commands 1.0");
        cli.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PowerCommandsConfiguration.yaml", "My Example Command", $"{name} Commands");
        cli.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PowerCommandsConfiguration.yaml", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\Configuration\\PowerCommandsConfiguration.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PowerCommandServices.cs", "MyExampleCommands", $"{name}Commands");
        foreach (var cmdName in _artifact.Commands) cli.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\Commands\\{cmdName}Command.cs", "MyExampleCommands", $"{name}Commands");
        foreach (var cmdName in _artifact.TemplateCommands)
        {
            cli.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.MyExampleCommands.csproj", $@"<ItemGroup>
    <Compile Remove=""Commands\Templates\{cmdName}Command.cs"" />
  </ItemGroup>", $"");
            cli.ReplaceContentInFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.MyExampleCommands.csproj", $@"<ItemGroup>
    <None Include=""Commands\Templates\{cmdName}Command.cs"" />
  </ItemGroup>", $"");
        }

        cli.MoveFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.MyExampleCommands.csproj", $"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\PainKiller.PowerCommands.{name}Commands.csproj");

        cli.MoveDirectory(_artifact.Source.Core, _artifact.Target.Core);
        cli.MoveDirectory(_artifact.Source.BootstrapProject, _artifact.Target.BootstrapProject);
        cli.MoveDirectory(_artifact.Source.ConsoleProject, _artifact.Target.ConsoleProject);
        cli.MoveDirectory(_artifact.Source.ThirdParty, _artifact.Target.ThirdParty);
        cli.MoveFile($"{_artifact.GetPath(_artifact.Source.SolutionFileName)}", $"{_artifact.GetPath(_artifact.Target.SolutionFileName)}");
        foreach (var cmdName in _artifact.Commands) cli.MoveFile($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}\\Commands\\{cmdName}Command.cs", $"{cmdName}Command.cs");
        cli.MoveDirectory($"{_artifact.GetPath(_artifact.Source.RenamedCommandsProject)}", $"{_artifact.GetPath(_artifact.Target.CommandsProject)}");
        cli.DeleteDir($"{_artifact.GetPath(_artifact.Target.CommandsProject)}\\Commands");
        cli.CreateDirectory($"{_artifact.GetPath(_artifact.Target.CommandsProject)}\\Commands");
        foreach (var cmdName in _artifact.Commands) cli.MoveFile($"{cmdName}Command.cs", $"{_artifact.GetPath(_artifact.Target.CommandsProject)}\\Commands\\{cmdName}Command.cs");

        cli.DeleteDownloadsDirectory();

        WriteHeadLine("\nAll work is done, now do the following steps");
        WriteHeadLine("\n1. Set the PowerCommandsConsole project as startup project");
        WriteHeadLine("\n2. Build and then run the solution");
        WriteHeadLine("\n3. type demo and then hit enter to see that it is all working");
        WriteHeadLine("\nNow you are ready to add your commands, read more about that on github:\nhttps://github.com/PowerCommands/PowerCommands2022/blob/main/PowerCommands%20Design%20Principles%20And%20Guidlines.md");

        ShellService.Service.OpenDirectory(_path);
        return Ok();
    }
}