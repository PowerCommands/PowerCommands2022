using PainKiller.PowerCommands.Core.Extensions;
using PainKiller.PowerCommands.Core.Managers;
using PainKiller.PowerCommands.Core.Services;
using PainKiller.PowerCommands.Shared.Contracts;
using System.Reflection;

namespace PainKiller.PowerCommands.Core.Commands;

[Tags("core|cli|project")]
[PowerCommand(      description: "Create or update the Visual Studio Solution with all depended projects, create new Commands from template",
                    example: "/*create new VS solution*/|powercommand new --name testproject --output \"C:\\Temp\\\"|/*Update powercommands core, this will first delete current Core projects and than apply the new Core projects*/|powercommand update|/*Only update template(s)*/|powercommand update --templates|/*Update templates with backup*/|powercommand update --templates --backup",
                    arguments:"Solution name:<name>",
                    argumentMandatory: true,
                    flags:"name|output|template|backup",
                    suggestion:"new",
                    qutes: "Path: <path>")]
public class PowerCommandCommand : CommandBase<CommandsConfiguration>
{
    private string _path = "";

    public PowerCommandCommand(string identifier, CommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        if (Input.Arguments.Length == 0) return CreateBadParameterRunResult("Missing arguments ");
        if (Input.SingleArgument == "new" && !Input.HasFlag("output")) return CreateBadParameterRunResult("You must provide a output path using the flag --output");

        var action = Input.Arguments.First().ToLower();
        var name = Input.GetFlagValue("name");
        var output = Input.GetFlagValue("output");
        var template = Input.HasFlag("template");
        var backup = Input.HasFlag("backup");

        switch (action)
        {
            case "version":
                Console.WriteLine($"{nameof(Core)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Core"))}");
                Console.WriteLine($"{nameof(PowerCommands.Configuration)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Configuration"))}");
                Console.WriteLine($"{nameof(ReadLine)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.ReadLine"))}");
                Console.WriteLine($"{nameof(Security)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Security"))}");
                Console.WriteLine($"{nameof(Shared)}: {ReflectionService.Service.GetVersion(Assembly.Load($"PainKiller.PowerCommands.Shared"))}");
                return CreateRunResult();
            case "new":
                return RunNewPowerCommandsProject(output, name);
            case "update":
                if (!template) return RunUpdatePowerCommandsProject(backup);
                _path = string.IsNullOrEmpty(output) ? Path.Combine(AppContext.BaseDirectory, "output", name) : Path.Combine(output, name);
                UpdateTemplates(new CliManager(name,_path, WriteLine), cloneRepo: true);
                return CreateRunResult();
            default:
                return CreateBadParameterRunResult("Missing arguments");
        }
    }

    private RunResult RunNewPowerCommandsProject(string output, string name)
    {
        _path = string.IsNullOrEmpty(output) ? Path.Combine(AppContext.BaseDirectory, "output", name) : Path.Combine(output, name);

        var cli = new CliManager(name, _path, WriteLine);
        
        cli.DeleteDownloadsDirectory();
        cli.CreateRootDirectory();

        cli.CloneRepo(Configuration.Repository);
        WriteLine("Fetching repo from Github...");

        UpdateTemplates(cli, newProjectName: name);

        cli.DeleteDir("PowerCommands2022\\.vscode");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Implementations");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Custom Components");

        cli.RenameDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.MyExampleCommands", $"PainKiller.PowerCommands.{name}Commands");

        cli.WriteNewSolutionFile();

        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap\\PainKiller.PowerCommands.Bootstrap.csproj", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.PowerCommandsConsole\\PainKiller.PowerCommands.PowerCommandsConsole.csproj", "<AssemblyName>pc</AssemblyName>", $"<AssemblyName>{name}</AssemblyName>");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap\\PowerCommandsManager.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap\\Startup.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.PowerCommandsConsole\\Program.cs", "Power Commands 1.0", $"{name} Commands 1.0");

        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PowerCommandsConfiguration.yaml", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Configuration\\PowerCommandsConfiguration.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Configuration\\FavoriteConfiguration.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PowerCommandServices.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DemoCommand.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\ConfigCommand.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DirCommand.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DocCommand.cs", "MyExampleCommands", $"{name}Commands");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.MyExampleCommands.csproj", @"<ItemGroup>
    <Compile Remove=""Commands\Templates\DefaultCommand.cs"" />
  </ItemGroup>", $"");
        cli.ReplaceContentInFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.MyExampleCommands.csproj", @"<ItemGroup>
    <None Include=""Commands\Templates\DefaultCommand.cs"" />
  </ItemGroup>", $"");

        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.MyExampleCommands.csproj", $"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\PainKiller.PowerCommands.{name}Commands.csproj");

        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\Core", "Core");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.Bootstrap", "PainKiller.PowerCommands.Bootstrap");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.PowerCommandsConsole", "PainKiller.PowerCommands.PowerCommandsConsole");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\Third party components", "Third party components");
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PowerCommands.{name}.sln", $"PowerCommands.{name}.sln");

        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DemoCommand.cs", "DemoCommand.cs");
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\ConfigCommand.cs", "ConfigCommand.cs");
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DirCommand.cs", "DirCommand.cs");
        cli.MoveFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands\\Commands\\DocCommand.cs", "DocCommand.cs");
        cli.MoveDirectory($"PowerCommands2022\\src\\PainKiller.PowerCommands\\PainKiller.PowerCommands.{name}Commands", $"PainKiller.PowerCommands.{name}Commands");
        cli.DeleteDir($"PainKiller.PowerCommands.{name}Commands\\Commands");
        cli.CreateDirectory($"PainKiller.PowerCommands.{name}Commands\\Commands");
        cli.MoveFile($"DemoCommand.cs", $"PainKiller.PowerCommands.{name}Commands\\Commands\\DemoCommand.cs");
        cli.MoveFile($"ConfigCommand.cs", $"PainKiller.PowerCommands.{name}Commands\\Commands\\ConfigCommand.cs");
        cli.MoveFile($"DirCommand.cs", $"PainKiller.PowerCommands.{name}Commands\\Commands\\DirCommand.cs");
        cli.MoveFile($"DocCommand.cs", $"PainKiller.PowerCommands.{name}Commands\\Commands\\DocCommand.cs");

        cli.DeleteDownloadsDirectory();

        WriteHeadLine("\nAll work is done, now do the following steps");
        WriteHeadLine("\n1. Set the PowerCommandsConsole project as startup project");
        WriteHeadLine("\n2. Build and then run the solution");
        WriteHeadLine("\n3. type demo and then hit enter to see that it is all working");
        WriteHeadLine("\nNow you are ready to add your commands, read more about that on github:\nhttps://github.com/PowerCommands/PowerCommands2022/blob/main/PowerCommands%20Design%20Principles%20And%20Guidlines.md");

        ShellService.Service.OpenDirectory(_path);
        return CreateRunResult();
    }

    private RunResult RunUpdatePowerCommandsProject(bool backup)
    {
        _path = CliManager.GetLocalSolutionRoot();
        var solutionFile = Directory.GetFileSystemEntries(_path, "*.sln").FirstOrDefault();
        var name = CliManager.GetName();
        var cli = new CliManager(name, _path, WriteLine);

        if (solutionFile == null)
        {
            WriteLine("When running in application scope (outside VS Env) only the Documentation file will be updated...");
            cli.MergeDocsDB();
            return CreateRunResult();
        }

        Console.WriteLine("Update will delete and replace everything in the [Core] and [Third party components] folder");
        if(backup) Console.WriteLine($"A backup will be saved in folder [{Path.Combine(_path)}]","Backup. /nEarlier backups needs to be removed");
        Console.WriteLine("");
        Console.WriteLine("Do you want to continue with the update? y/n");
        var response = Console.ReadLine();
        if ($"{response?.Trim()}" != "y") return CreateRunResult();
        
        

        cli.DeleteDownloadsDirectory();
        cli.CreateRootDirectory(onlyRepoSrcCodeRootPath: true);

        var backupDirectory = "";
        if(backup) backupDirectory = cli.BackupDirectory("Core");

        cli.CloneRepo(Configuration.Repository);
        WriteLine("Fetching repo from Github...");

        UpdateTemplates(cli);

        cli.DeleteDir("PowerCommands2022\\.vscode");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Custom Components");
        cli.DeleteDir("PowerCommands2022\\src\\PainKiller.PowerCommands\\Implementations");

        cli.DeleteDir("Core");
        cli.MoveDirectory("PowerCommands2022\\src\\PainKiller.PowerCommands\\Core", "Core");

        cli.DeleteDownloadsDirectory();

        WriteLine("Your PowerCommands Core component is now up to date with latest code from github!");
        WriteLine("if you started this from Visual Studio you probably need to restart Visual Studio to reload all dependencies");
        if(backup) WriteLine($"A backup of the Core projects has been stored here [{backupDirectory}]");

        ShellService.Service.OpenDirectory(backupDirectory);

        return CreateRunResult();
    }
    private void UpdateTemplates(ICliManager cliManager, bool cloneRepo = false, string newProjectName = "")
    {
        if (cloneRepo)
        {
            cliManager.DeleteDownloadsDirectory();
            cliManager.CreateDownloadsDirectory();
            cliManager.CloneRepo(Configuration.Repository);
        }

        var name = string.IsNullOrEmpty(newProjectName) ? CliManager.GetName() : newProjectName;
        var templateManager = new TemplateManager(name, WriteLine);
        templateManager.InitializeTemplatesDirectory();
        templateManager.CopyTemplates();
        
        cliManager.MergeDocsDB();
        cliManager.DeleteFile($"PowerCommands2022\\src\\PainKiller.PowerCommands\\Core\\PainKiller.PowerCommands.Core\\DocsDB.data", repoFile:true);
    }
}